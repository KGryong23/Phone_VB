using System.Net.WebSockets;
using System.Text;
using Newtonsoft.Json;
using PhoneSocketServer.Models;

namespace PhoneSocketServer.Services;

public class WebSocketServer
{
    private readonly ClientManager _clientManager;
    private readonly MessageHandler _messageHandler;
    private readonly ILogger<WebSocketServer> _logger;

    public WebSocketServer(ClientManager clientManager, MessageHandler messageHandler, ILogger<WebSocketServer> logger)
    {
        _clientManager = clientManager;
        _messageHandler = messageHandler;
        _logger = logger;
        
        // Set circular reference
        _messageHandler.SetWebSocketServer(this);
    }

    public async Task HandleWebSocketAsync(HttpContext context)
    {
        var webSocket = await context.WebSockets.AcceptWebSocketAsync();
        var connectionId = Guid.NewGuid().ToString();
        
        _logger.LogInformation("New WebSocket connection: {ConnectionId}", connectionId);

        try
        {
            await HandleConnectionAsync(webSocket, connectionId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling WebSocket connection {ConnectionId}", connectionId);
        }
        finally
        {
            await CleanupConnectionAsync(webSocket, connectionId);
        }
    }

    private async Task HandleConnectionAsync(WebSocket webSocket, string connectionId)
    {
        var buffer = new byte[1024 * 4];
        ConnectedClient? client = null;

        while (webSocket.State == WebSocketState.Open)
        {
            var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);

            if (result.MessageType == WebSocketMessageType.Text)
            {
                var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                _logger.LogDebug("Received message: {Message}", message);

                var response = await _messageHandler.HandleMessageAsync(message, client);
                
                // Update client info if this was a connection message
                if (response?.Type == MessageTypes.USER_CONNECTED && client == null)
                {
                    var userData = JsonConvert.DeserializeObject<UserConnectionData>(response.Data.ToString()!);
                    if (userData != null)
                    {
                        client = new ConnectedClient
                        {
                            UserId = userData.UserId,
                            Username = userData.Username,
                            RoleId = userData.RoleId,
                            WebSocket = webSocket,
                            ConnectionId = connectionId,
                            ConnectedAt = DateTime.UtcNow
                        };
                        _clientManager.AddClient(client);
                    }
                }

                // Send response back to client if needed
                if (response != null && response.Type == MessageTypes.ONLINE_USERS_LIST)
                {
                    await SendMessageToClientAsync(client!, JsonConvert.SerializeObject(response));
                }
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                break;
            }
        }
    }

    public async Task BroadcastToRoleAsync(int roleId, SocketMessage message)
    {
        var clients = _clientManager.GetClientsByRole(roleId);
        var messageJson = JsonConvert.SerializeObject(message);

        var tasks = clients.Select(client => SendMessageToClientAsync(client, messageJson));
        await Task.WhenAll(tasks);

        _logger.LogInformation("Broadcasted message to {Count} clients with role {RoleId}", clients.Count, roleId);
    }

    public async Task SendToUserAsync(int userId, SocketMessage message)
    {
        var client = _clientManager.GetClient(userId);
        if (client != null)
        {
            var messageJson = JsonConvert.SerializeObject(message);
            await SendMessageToClientAsync(client, messageJson);
            _logger.LogInformation("Sent message to user {UserId}", userId);
        }
        else
        {
            _logger.LogWarning("User {UserId} not found for message sending", userId);
        }
    }

    public async Task SendMessageToClientAsync(ConnectedClient client, string message)
    {
        if (client.WebSocket.State == WebSocketState.Open)
        {
            try
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await client.WebSocket.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message to client {UserId}", client.UserId);
                // Remove client if send fails
                _clientManager.RemoveClient(client.UserId);
            }
        }
    }

    private async Task CleanupConnectionAsync(WebSocket webSocket, string connectionId)
    {
        _clientManager.RemoveClientByConnectionId(connectionId);
        
        if (webSocket.State == WebSocketState.Open)
        {
            try
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error closing WebSocket connection {ConnectionId}", connectionId);
            }
        }
        
        _logger.LogInformation("Cleaned up WebSocket connection: {ConnectionId}", connectionId);
    }
}
