# Phone Socket Server

Real-time WebSocket server cho ứng dụng Phone_VB để handle permission updates và user management.

## 🚀 Quick Start

### Chạy Local
```bash
cd PhoneSocketServer
dotnet run
```

Server sẽ chạy trên: http://localhost:8080

### Chạy với Docker
```bash
# Build và chạy với docker-compose
docker-compose up phone-socket-server

# Hoặc build manual
cd PhoneSocketServer
docker build -t phone-socket-server .
docker run -p 8080:8080 phone-socket-server
```

## 📡 Endpoints

- **WebSocket**: `ws://localhost:8080/socket`
- **Health Check**: `http://localhost:8080/health`
- **Online Users**: `http://localhost:8080/users`
- **Stats**: `http://localhost:8080/stats`

## 📨 Message Types

### 1. User Connected
```json
{
  "type": "USER_CONNECTED",
  "data": {
    "userId": 123,
    "username": "elite",
    "roleId": 1,
    "connectedAt": "2025-07-21 10:30:00"
  }
}
```

### 2. Role Permissions Changed
```json
{
  "type": "ROLE_PERMISSIONS_CHANGED",
  "data": {
    "roleId": 1
  }
}
```

### 3. Force Logout User
```json
{
  "type": "USER_FORCE_LOGOUT",
  "data": {
    "userId": 123,
    "reason": "admin_action"
  }
}
```

### 4. Get Online Users
```json
{
  "type": "GET_ONLINE_USERS",
  "data": {}
}
```

### 5. Online Users Response
```json
{
  "type": "ONLINE_USERS_LIST",
  "data": {
    "users": [
      {
        "userId": 123,
        "username": "elite",
        "roleId": 1,
        "connectedAt": "2025-07-21 10:30:00"
      }
    ]
  }
}
```

## 🏗️ Architecture

```
Phone_VB (VB.NET Client)
    ↕ WebSocket
PhoneSocketServer (C# .NET 8)
    ├── WebSocketServer.cs      # Handle WebSocket connections
    ├── ClientManager.cs        # Track connected users
    ├── MessageHandler.cs       # Process messages
    └── Models/
        ├── SocketMessage.cs    # Message structure
        ├── ConnectedClient.cs  # Client info
        └── MessageTypes.cs     # Message constants
```

## 🔧 Configuration

### appsettings.json
```json
{
  "SocketServer": {
    "Host": "localhost",
    "Port": 8080,
    "Path": "/socket",
    "AllowedOrigins": ["*"],
    "MaxConnections": 1000
  }
}
```

## 📋 Features

- ✅ Real-time permission updates
- ✅ Force logout users
- ✅ Online user tracking
- ✅ Role-based message broadcasting
- ✅ Health monitoring
- ✅ Docker support
- ✅ Logging với Serilog
- ✅ Auto cleanup disconnected clients

## 🔗 Integration với VB.NET

1. **Connect**: VB.NET client connects khi login
2. **Send Messages**: Admin actions trigger socket messages
3. **Receive Updates**: Users receive real-time updates
4. **Handle Responses**: Update UI và redirect users

## 📊 Monitoring

- Check health: `curl http://localhost:8080/health`
- View online users: `curl http://localhost:8080/users`
- Get stats: `curl http://localhost:8080/stats`

## 🐛 Troubleshooting

### Port đã được sử dụng
```bash
netstat -ano | findstr :8080
taskkill /PID <PID> /F
```

### Docker build issues
```bash
docker system prune -f
docker-compose build --no-cache
```

### WebSocket connection failed
- Kiểm tra firewall settings
- Verify port 8080 không bị block
- Check server logs trong console hoặc logs/ folder
