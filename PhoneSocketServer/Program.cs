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
Console.WriteLine($"� TCP Socket Server: {host}:8081");
Console.WriteLine($"�🔍 Health check: http://{host}:{port}/health");
Console.WriteLine($"👥 Online users: http://{host}:{port}/users");
Console.WriteLine($"📊 Stats: http://{host}:{port}/stats");
Console.WriteLine("Press Ctrl+C to stop the server");

app.Run($"http://{host}:{port}");
