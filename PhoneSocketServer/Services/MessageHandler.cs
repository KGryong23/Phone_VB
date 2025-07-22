using Newtonsoft.Json;
using PhoneSocketServer.Models;

namespace PhoneSocketServer.Services;

public class MessageHandler
{
    private readonly ClientManager _clientManager;
    private WebSocketServer? _webSocketServer;
    private TcpSocketServer? _tcpSocketServer;
    private readonly ILogger<MessageHandler> _logger;
    private readonly Dictionary<int, List<SocketMessage>> _pendingMessages = new();
    private readonly object _pendingMessagesLock = new object();

    public MessageHandler(ClientManager clientManager, ILogger<MessageHandler> logger)
    {
        _clientManager = clientManager;
        _logger = logger;
    }

    public void SetWebSocketServer(WebSocketServer webSocketServer)
    {
        _webSocketServer = webSocketServer;
    }

    public void SetTcpServer(TcpSocketServer tcpSocketServer)
    {
        _tcpSocketServer = tcpSocketServer;
    }

    public async Task<SocketMessage?> HandleMessageAsync(string messageJson, ConnectedClient? sender)
    {
        try
        {
            var message = JsonConvert.DeserializeObject<SocketMessage>(messageJson);
            if (message == null) return null;

            _logger.LogDebug("Handling message type: {MessageType}", message.Type);

            return message.Type switch
            {
                MessageTypes.USER_CONNECTED => await HandleUserConnected(message, sender),
                MessageTypes.USER_DISCONNECTED => await HandleUserDisconnected(message, sender),
                MessageTypes.ROLE_PERMISSIONS_CHANGED => await HandleRolePermissionsChanged(message, sender),
                MessageTypes.USER_FORCE_LOGOUT => await HandleUserForceLogout(message, sender),
                MessageTypes.GET_ONLINE_USERS => await HandleGetOnlineUsers(message, sender),
                _ => null
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling message: {Message}", messageJson);
            return null;
        }
    }

    private async Task<SocketMessage?> HandleUserConnected(SocketMessage message, ConnectedClient? sender)
    {
        var userData = JsonConvert.DeserializeObject<UserConnectionData>(message.Data.ToString()!);
        if (userData == null) return null;

        _logger.LogInformation("User connected: UserId={UserId}, Username={Username}, RoleId={RoleId}",
            userData.UserId, userData.Username, userData.RoleId);

        // Broadcast to other clients that a new user connected
        var broadcastMessage = new SocketMessage
        {
            Type = MessageTypes.USER_CONNECTED,
            Data = userData
        };

        await BroadcastToOthersAsync(broadcastMessage, userData.UserId);

        return message; // Return for client addition
    }

    private async Task<SocketMessage?> HandleUserDisconnected(SocketMessage message, ConnectedClient? sender)
    {
        var userData = JsonConvert.DeserializeObject<UserConnectionData>(message.Data.ToString()!);
        if (userData == null) return null;

        _logger.LogInformation("User disconnected: UserId={UserId}", userData.UserId);

        // Broadcast to other clients that user disconnected
        await BroadcastToOthersAsync(message, userData.UserId);

        return null;
    }

    private async Task<SocketMessage?> HandleRolePermissionsChanged(SocketMessage message, ConnectedClient? sender)
    {
        var roleData = JsonConvert.DeserializeObject<RolePermissionData>(message.Data.ToString()!);
        if (roleData == null) return null;

        _logger.LogInformation("Role permissions changed: RoleId={RoleId}", roleData.RoleId);

        // Broadcast to all clients with this role via WebSocket
        if (_webSocketServer != null)
        {
            await _webSocketServer.BroadcastToRoleAsync(roleData.RoleId, message);
        }

        // Broadcast to all TCP clients with this role
        if (_tcpSocketServer != null)
        {
            await BroadcastToTcpRoleAsync(roleData.RoleId, message);
        }

        return null;
    }

    private async Task<SocketMessage?> HandleUserForceLogout(SocketMessage message, ConnectedClient? sender)
    {
        var logoutData = JsonConvert.DeserializeObject<ForceLogoutData>(message.Data.ToString()!);
        if (logoutData == null) return null;

        _logger.LogInformation("Force logout user: UserId={UserId}, Reason={Reason}", 
            logoutData.UserId, logoutData.Reason);

        // Send logout message to specific user
        if (_webSocketServer != null)
        {
            await _webSocketServer.SendToUserAsync(logoutData.UserId, message);
        }

        return null;
    }

    private async Task<SocketMessage?> HandleGetOnlineUsers(SocketMessage message, ConnectedClient? sender)
    {
        var onlineClients = _clientManager.GetAllClients();
        var onlineUsers = onlineClients.Select(c => new OnlineUser
        {
            UserId = c.UserId,
            Username = c.Username,
            RoleId = c.RoleId,
            ConnectedAt = c.ConnectedAt.ToString("yyyy-MM-dd HH:mm:ss")
        }).ToList();

        var responseMessage = new SocketMessage
        {
            Type = MessageTypes.ONLINE_USERS_LIST,
            Data = new OnlineUsersData { Users = onlineUsers }
        };

        _logger.LogInformation("Sending online users list: {Count} users", onlineUsers.Count);

        return responseMessage;
    }

    private async Task BroadcastToOthersAsync(SocketMessage message, int excludeUserId)
    {
        var allClients = _clientManager.GetAllClients()
            .Where(c => c.UserId != excludeUserId)
            .ToList();

        // Broadcast via WebSocket
        if (_webSocketServer != null)
        {
            var messageJson = JsonConvert.SerializeObject(message);
            var tasks = allClients.Select(client => _webSocketServer.SendMessageToClientAsync(client, messageJson));
            await Task.WhenAll(tasks);
        }

        // Broadcast via TCP Socket
        if (_tcpSocketServer != null)
        {
            await _tcpSocketServer.SendMessageToAllClientsAsync(message);
        }
    }

    // HTTP Polling support methods
    public async Task BroadcastRolePermissionChanged(int roleId)
    {
        var message = new SocketMessage
        {
            Type = MessageTypes.ROLE_PERMISSIONS_CHANGED,
            Data = new { RoleId = roleId }
        };

        // Find all clients with this role and add to their pending messages
        var affectedClients = _clientManager.GetAllClients()
            .Where(c => c.RoleId == roleId)
            .ToList();

        // Send via TCP Socket immediately
        if (_tcpSocketServer != null)
        {
            foreach (var client in affectedClients)
            {
                await _tcpSocketServer.SendMessageToClientAsync(client, message);
            }
        }

        // Also store in pending messages for HTTP polling fallback
        lock (_pendingMessagesLock)
        {
            foreach (var client in affectedClients)
            {
                if (!_pendingMessages.ContainsKey(client.UserId))
                {
                    _pendingMessages[client.UserId] = new List<SocketMessage>();
                }
                _pendingMessages[client.UserId].Add(message);
            }
        }

        _logger.LogInformation("Role permission changed broadcast sent for role {RoleId}, affecting {Count} users", 
            roleId, affectedClients.Count);
    }

    public async Task ForceLogoutUser(int userId, string reason)
    {
        var message = new SocketMessage
        {
            Type = MessageTypes.USER_FORCE_LOGOUT,
            Data = new { UserId = userId, Reason = reason }
        };

        // Find the specific client
        var targetClient = _clientManager.GetAllClients()
            .FirstOrDefault(c => c.UserId == userId);

        // Send via TCP Socket immediately
        if (_tcpSocketServer != null && targetClient != null)
        {
            await _tcpSocketServer.SendMessageToClientAsync(targetClient, message);
        }

        // Also store in pending messages for HTTP polling fallback
        lock (_pendingMessagesLock)
        {
            if (!_pendingMessages.ContainsKey(userId))
            {
                _pendingMessages[userId] = new List<SocketMessage>();
            }
            _pendingMessages[userId].Add(message);
        }

        _logger.LogInformation("Force logout sent for user {UserId}: {Reason}", userId, reason);
    }

    private async Task BroadcastToTcpRoleAsync(int roleId, SocketMessage message)
    {
        try
        {
            var clients = _clientManager.GetClientsByRole(roleId);
            _logger.LogInformation("Broadcasting to {Count} TCP clients with role {RoleId}", clients.Count, roleId);

            var tasks = new List<Task>();
            foreach (var client in clients)
            {
                if (_tcpSocketServer != null)
                {
                    tasks.Add(_tcpSocketServer.SendMessageToClientAsync(client, message));
                }
            }

            await Task.WhenAll(tasks);
            _logger.LogInformation("TCP broadcast completed for role {RoleId}", roleId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error broadcasting to TCP clients for role {RoleId}", roleId);
        }
    }

    public List<SocketMessage> GetPendingMessages(int userId)
    {
        lock (_pendingMessagesLock)
        {
            if (_pendingMessages.ContainsKey(userId))
            {
                var messages = _pendingMessages[userId].ToList();
                _pendingMessages[userId].Clear(); // Clear after retrieving
                return messages;
            }
            return new List<SocketMessage>();
        }
    }
}
