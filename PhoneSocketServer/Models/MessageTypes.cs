namespace PhoneSocketServer.Models;

public static class MessageTypes
{
    public const string USER_CONNECTED = "USER_CONNECTED";
    public const string USER_DISCONNECTED = "USER_DISCONNECTED";
    public const string ROLE_PERMISSIONS_CHANGED = "ROLE_PERMISSIONS_CHANGED";
    public const string USER_ROLE_CHANGED = "USER_ROLE_CHANGED";
    public const string USER_FORCE_LOGOUT = "USER_FORCE_LOGOUT";
    public const string GET_ONLINE_USERS = "GET_ONLINE_USERS";
    public const string ONLINE_USERS_LIST = "ONLINE_USERS_LIST";
}
