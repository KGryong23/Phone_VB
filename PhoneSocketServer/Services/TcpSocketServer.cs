using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using PhoneSocketServer.Models;

namespace PhoneSocketServer.Services;

public class TcpSocketServer
{
    private readonly ClientManager _clientManager;
    private readonly MessageHandler _messageHandler;
    private readonly ILogger<TcpSocketServer> _logger;
    private TcpListener? _tcpListener;
    private bool _isRunning = false;
    private readonly List<TcpClientHandler> _clientHandlers = new();

    public TcpSocketServer(ClientManager clientManager, MessageHandler messageHandler, ILogger<TcpSocketServer> logger)
    {
        _clientManager = clientManager;
        _messageHandler = messageHandler;
        _logger = logger;
    }

    public async Task StartAsync(int port = 8081)
    {
        try
        {
            _tcpListener = new TcpListener(IPAddress.Any, port);
            _tcpListener.Start();
            _isRunning = true;
            
            _logger.LogInformation("TCP Socket Server started on port {Port}", port);

            // Accept clients in background
            _ = Task.Run(AcceptClientsAsync);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Failed to start TCP Socket Server");
        }
    }

    private async Task AcceptClientsAsync()
    {
        while (_isRunning && _tcpListener != null)
        {
            try
            {
                var tcpClient = await _tcpListener.AcceptTcpClientAsync();
                var clientHandler = new TcpClientHandler(tcpClient, _clientManager, _messageHandler, _logger);
                
                lock (_clientHandlers)
                {
                    _clientHandlers.Add(clientHandler);
                }

                // Handle client in background
                _ = Task.Run(() => clientHandler.HandleClientAsync());
                
                _logger.LogInformation("New TCP client connected from {RemoteEndPoint}", tcpClient.Client.RemoteEndPoint);
            }
            catch (Exception ex)
            {
                if (_isRunning)
                {
                    _logger.LogError(ex, "Error accepting TCP client");
                }
            }
        }
    }

    public async Task SendMessageToAllClientsAsync(SocketMessage message)
    {
        var json = JsonConvert.SerializeObject(message);
        var tasks = new List<Task>();

        lock (_clientHandlers)
        {
            foreach (var handler in _clientHandlers.ToList())
            {
                if (handler.IsConnected)
                {
                    tasks.Add(handler.SendMessageAsync(json));
                }
                else
                {
                    _clientHandlers.Remove(handler);
                }
            }
        }

        await Task.WhenAll(tasks);
    }

    public async Task SendMessageToClientAsync(ConnectedClient client, SocketMessage message)
    {
        var json = JsonConvert.SerializeObject(message);
        
        lock (_clientHandlers)
        {
            var handler = _clientHandlers.FirstOrDefault(h => h.Client?.ConnectionId == client.ConnectionId);
            if (handler != null && handler.IsConnected)
            {
                _ = Task.Run(() => handler.SendMessageAsync(json));
            }
        }
    }

    public void Stop()
    {
        _isRunning = false;
        _tcpListener?.Stop();
        
        lock (_clientHandlers)
        {
            foreach (var handler in _clientHandlers)
            {
                handler.Disconnect();
            }
            _clientHandlers.Clear();
        }
        
        _logger.LogInformation("TCP Socket Server stopped");
    }
}

public class TcpClientHandler
{
    private readonly TcpClient _tcpClient;
    private readonly NetworkStream _networkStream;
    private readonly ClientManager _clientManager;
    private readonly MessageHandler _messageHandler;
    private readonly ILogger _logger;
    private bool _isConnected = true;

    public ConnectedClient? Client { get; private set; }
    public bool IsConnected => _isConnected && _tcpClient.Connected;

    public TcpClientHandler(TcpClient tcpClient, ClientManager clientManager, MessageHandler messageHandler, ILogger logger)
    {
        _tcpClient = tcpClient;
        _networkStream = tcpClient.GetStream();
        _clientManager = clientManager;
        _messageHandler = messageHandler;
        _logger = logger;
    }

    public async Task HandleClientAsync()
    {
        try
        {
            while (_isConnected && _tcpClient.Connected)
            {
                // Read message length (4 bytes)
                var lengthBytes = new byte[4];
                int bytesRead = await _networkStream.ReadAsync(lengthBytes, 0, 4);
                if (bytesRead == 0) break;

                int messageLength = BitConverter.ToInt32(lengthBytes, 0);
                
                // Read message data
                var messageBytes = new byte[messageLength];
                int totalRead = 0;
                while (totalRead < messageLength)
                {
                    bytesRead = await _networkStream.ReadAsync(messageBytes, totalRead, messageLength - totalRead);
                    if (bytesRead == 0) break;
                    totalRead += bytesRead;
                }

                if (totalRead == messageLength)
                {
                    var json = Encoding.UTF8.GetString(messageBytes);
                    await ProcessMessageAsync(json);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error handling TCP client");
        }
        finally
        {
            Disconnect();
        }
    }

    private async Task ProcessMessageAsync(string messageJson)
    {
        try
        {
            var response = await _messageHandler.HandleMessageAsync(messageJson, Client);
            if (response != null)
            {
                var responseJson = JsonConvert.SerializeObject(response);
                await SendMessageAsync(responseJson);
            }

            // Handle user connection registration
            var message = JsonConvert.DeserializeObject<SocketMessage>(messageJson);
            if (message?.Type == MessageTypes.USER_CONNECTED && Client == null)
            {
                var userData = JsonConvert.DeserializeObject<UserConnectionData>(message.Data?.ToString() ?? "");
                if (userData != null)
                {
                    Client = new ConnectedClient
                    {
                        ConnectionId = Guid.NewGuid().ToString(),
                        UserId = userData.UserId,
                        Username = userData.Username,
                        RoleId = userData.RoleId,
                        ConnectedAt = DateTime.UtcNow
                    };
                    
                    _clientManager.AddClient(Client);
                    _logger.LogInformation("TCP Client registered: {Username} (ID: {UserId})", userData.Username, userData.UserId);
                }
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error processing message: {Message}", messageJson);
        }
    }

    public async Task SendMessageAsync(string json)
    {
        if (!IsConnected) return;

        try
        {
            var data = Encoding.UTF8.GetBytes(json);
            var length = BitConverter.GetBytes(data.Length);

            // Send length first, then data
            await _networkStream.WriteAsync(length, 0, 4);
            await _networkStream.WriteAsync(data, 0, data.Length);
            await _networkStream.FlushAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error sending message to TCP client");
            Disconnect();
        }
    }

    public void Disconnect()
    {
        if (!_isConnected) return;
        
        _isConnected = false;
        
        if (Client != null)
        {
            _clientManager.RemoveClientByConnectionId(Client.ConnectionId);
            _logger.LogInformation("TCP Client disconnected: {Username} (ID: {UserId})", Client.Username, Client.UserId);
        }

        try
        {
            _networkStream?.Close();
            _tcpClient?.Close();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error closing TCP client connection");
        }
    }
}
