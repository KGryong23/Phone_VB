using PhoneSocketServer.Services;
using PhoneSocketServer.Models;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .CreateLogger();

builder.Host.UseSerilog();

// Add services
builder.Services.AddSingleton<ClientManager>();
builder.Services.AddSingleton<MessageHandler>();
builder.Services.AddSingleton<WebSocketServer>();
builder.Services.AddSingleton<TcpSocketServer>();

var app = builder.Build();

// Configure middleware
app.UseWebSockets();

// Start TCP Socket Server
var tcpServer = app.Services.GetRequiredService<TcpSocketServer>();
await tcpServer.StartAsync(8081);

// Update MessageHandler with TCP server reference
var messageHandler = app.Services.GetRequiredService<MessageHandler>();
messageHandler.SetTcpServer(tcpServer);

// Map WebSocket endpoint
app.Map("/socket", async (HttpContext context, WebSocketServer socketServer) =>
{
    if (context.WebSockets.IsWebSocketRequest)
    {
        await socketServer.HandleWebSocketAsync(context);
    }
    else
    {
        context.Response.StatusCode = 400;
        await context.Response.WriteAsync("WebSocket connections only");
    }
});

// Health check endpoint
app.MapGet("/health", () => "Phone Socket Server is running");

// HTTP Polling endpoints for VB.NET compatibility
app.MapPost("/api/register", async (RegisterRequest request, ClientManager clientManager) =>
{
    var connectionId = Guid.NewGuid().ToString();
    var client = new ConnectedClient
    {
        ConnectionId = connectionId,
        UserId = request.UserId,
        Username = request.Username,
        RoleId = request.RoleId,
        ConnectedAt = DateTime.UtcNow
    };
    
    clientManager.AddClient(client);
    Log.Information("User {Username} (ID: {UserId}) registered via HTTP", request.Username, request.UserId);
    return Results.Ok(new { ConnectionId = connectionId });
});

app.MapPost("/api/role-permission-changed", async (RolePermissionRequest request, MessageHandler messageHandler) =>
{
    await messageHandler.BroadcastRolePermissionChanged(request.RoleId);
    return Results.Ok();
});

app.MapPost("/api/force-logout", async (ForceLogoutRequest request, MessageHandler messageHandler) =>
{
    await messageHandler.ForceLogoutUser(request.UserId, request.Reason);
    return Results.Ok();
});

app.MapGet("/api/online-users", (ClientManager clientManager) =>
{
    var users = clientManager.GetAllClients().Select(c => new
    {
        c.UserId,
        c.Username,
        c.RoleId,
        ConnectedAt = c.ConnectedAt.ToString("yyyy-MM-dd HH:mm:ss")
    });
    return Results.Ok(users);
});

app.MapGet("/api/messages/{userId:int}", (int userId, MessageHandler messageHandler) =>
{
    var messages = messageHandler.GetPendingMessages(userId);
    return Results.Ok(messages);
});

// Get online users endpoint (for monitoring)
app.MapGet("/users", (ClientManager clientManager) => 
{
    return clientManager.GetAllClients().Select(c => new 
    {
        c.UserId,
        c.Username,
        c.RoleId,
        ConnectedAt = c.ConnectedAt.ToString("yyyy-MM-dd HH:mm:ss"),
        ConnectionId = c.ConnectionId
    });
});

// Get online count
app.MapGet("/stats", (ClientManager clientManager) => new
{
    OnlineUsers = clientManager.GetOnlineCount(),
    Timestamp = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
});

var socketServerConfig = builder.Configuration.GetSection("SocketServer");
var host = socketServerConfig["Host"] ?? "localhost";
var port = socketServerConfig.GetValue<int>("Port", 8080);

Log.Information("Starting Phone Socket Server on {Host}:{Port}", host, port);
Console.WriteLine($"🚀 Phone Socket Server starting on {host}:{port}");
Console.WriteLine($"📡 WebSocket endpoint: ws://{host}:{port}/socket");
Console.WriteLine($"🔍 Health check: http://{host}:{port}/health");
Console.WriteLine($"👥 Online users: http://{host}:{port}/users");
Console.WriteLine($"📊 Stats: http://{host}:{port}/stats");
Console.WriteLine("Press Ctrl+C to stop the server");

app.Run($"http://{host}:{port}");
