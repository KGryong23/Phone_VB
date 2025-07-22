using System.Net.WebSockets;

namespace PhoneSocketServer.Models;

public class ConnectedClient
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public WebSocket WebSocket { get; set; } = null!;
    public string ConnectionId { get; set; } = string.Empty;
    public DateTime ConnectedAt { get; set; }
}
