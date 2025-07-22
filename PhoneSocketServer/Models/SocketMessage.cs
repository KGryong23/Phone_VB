namespace PhoneSocketServer.Models;

public class SocketMessage
{
    public string Type { get; set; } = string.Empty;
    public object Data { get; set; } = new();
}

public class RolePermissionData
{
    public int RoleId { get; set; }
}

public class ForceLogoutData
{
    public int UserId { get; set; }
    public string Reason { get; set; } = string.Empty;
}

public class UserConnectionData
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string ConnectedAt { get; set; } = string.Empty;
}

public class OnlineUsersData
{
    public List<OnlineUser> Users { get; set; } = new();
}

public class OnlineUser
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int RoleId { get; set; }
    public string ConnectedAt { get; set; } = string.Empty;
}
