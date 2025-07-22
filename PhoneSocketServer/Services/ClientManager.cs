using PhoneSocketServer.Models;
using System.Collections.Concurrent;

namespace PhoneSocketServer.Services;

public class ClientManager
{
    private readonly ConcurrentDictionary<int, ConnectedClient> _clientsByUserId = new();
    private readonly ConcurrentDictionary<string, ConnectedClient> _clientsByConnectionId = new();
    private readonly ILogger<ClientManager> _logger;

    public ClientManager(ILogger<ClientManager> logger)
    {
        _logger = logger;
    }

    public void AddClient(ConnectedClient client)
    {
        _clientsByUserId.TryAdd(client.UserId, client);
        _clientsByConnectionId.TryAdd(client.ConnectionId, client);
        
        _logger.LogInformation("Added client: UserId={UserId}, Username={Username}, ConnectionId={ConnectionId}", 
            client.UserId, client.Username, client.ConnectionId);
    }

    public void RemoveClient(int userId)
    {
        if (_clientsByUserId.TryRemove(userId, out var client))
        {
            _clientsByConnectionId.TryRemove(client.ConnectionId, out _);
            _logger.LogInformation("Removed client: UserId={UserId}, Username={Username}", 
                client.UserId, client.Username);
        }
    }

    public void RemoveClientByConnectionId(string connectionId)
    {
        if (_clientsByConnectionId.TryRemove(connectionId, out var client))
        {
            _clientsByUserId.TryRemove(client.UserId, out _);
            _logger.LogInformation("Removed client by connection: UserId={UserId}, Username={Username}, ConnectionId={ConnectionId}", 
                client.UserId, client.Username, connectionId);
        }
    }

    public ConnectedClient? GetClient(int userId)
    {
        _clientsByUserId.TryGetValue(userId, out var client);
        return client;
    }

    public List<ConnectedClient> GetClientsByRole(int roleId)
    {
        return _clientsByUserId.Values
            .Where(c => c.RoleId == roleId)
            .ToList();
    }

    public List<ConnectedClient> GetAllClients()
    {
        return _clientsByUserId.Values.ToList();
    }

    public int GetOnlineCount()
    {
        return _clientsByUserId.Count;
    }

    public bool IsUserOnline(int userId)
    {
        return _clientsByUserId.ContainsKey(userId);
    }
}
