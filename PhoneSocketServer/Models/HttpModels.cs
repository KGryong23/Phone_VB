namespace PhoneSocketServer.Models;

public class RegisterRequest
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int RoleId { get; set; }
}

public class RolePermissionRequest
{
    public int RoleId { get; set; }
}

public class ForceLogoutRequest
{
    public int UserId { get; set; }
    public string Reason { get; set; } = string.Empty;
}
