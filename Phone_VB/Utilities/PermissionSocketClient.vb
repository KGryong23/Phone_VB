Imports System.Net.Sockets
Imports System.Text
Imports System.Threading
Imports System.Web.Script.Serialization

Public Class PermissionSocketClient
    Private Shared _instance As PermissionSocketClient
    Private _tcpClient As TcpClient
    Private _networkStream As NetworkStream
    Private _listenerThread As Thread
    Private _healthCheckTimer As System.Timers.Timer
    Private _isConnected As Boolean = False
    Private _currentUserId As Integer?
    Private _currentRoleId As Integer?
    Private _serializer As JavaScriptSerializer
    Private _skipNextPermissionNotification As Boolean = False ' Flag để skip notification
    Private Const SERVER_HOST As String = "localhost"
    Private Const SERVER_PORT As Integer = 8081 ' TCP port khác với HTTP port

    ' Events
    Public Event PermissionsChanged(roleId As Integer)
    Public Event UserForceLoggedOut(reason As String)
    Public Event OnlineUsersUpdated(onlineUsers As List(Of OnlineUser))

    Public Sub New()
        _tcpClient = New TcpClient()
        _serializer = New JavaScriptSerializer()
    End Sub

    ' Singleton instance
    Public Shared ReadOnly Property Instance As PermissionSocketClient
        Get
            If _instance Is Nothing Then
                _instance = New PermissionSocketClient()
            End If
            Return _instance
        End Get
    End Property

    ' Reset singleton instance để tạo connection mới
    Public Shared Sub ResetInstance()
        If _instance IsNot Nothing Then
            _instance.Disconnect()
            _instance = Nothing
        End If
    End Sub

    Public ReadOnly Property IsConnected As Boolean
        Get
            Dim result = _isConnected AndAlso _tcpClient IsNot Nothing AndAlso _tcpClient.Connected
            If Not result Then
                Debug.WriteLine("IsConnected = False. _isConnected: " + _isConnected.ToString() +
                              ", _tcpClient IsNot Nothing: " + (_tcpClient IsNot Nothing).ToString() +
                              ", _tcpClient.Connected: " + If(_tcpClient IsNot Nothing, _tcpClient.Connected.ToString(), "N/A"))
            End If
            Return result
        End Get
    End Property

    Public Function Connect() As Boolean
        Try
            ' Kết nối TCP tới server
            _tcpClient.Connect(SERVER_HOST, SERVER_PORT)
            _networkStream = _tcpClient.GetStream()
            _isConnected = True

            ' Bắt đầu thread để lắng nghe messages
            _listenerThread = New Thread(AddressOf ListenForMessages)
            _listenerThread.IsBackground = True
            _listenerThread.Start()

            ' Start health check timer (check every 5 seconds)
            _healthCheckTimer = New System.Timers.Timer(5000)
            AddHandler _healthCheckTimer.Elapsed, AddressOf HealthCheck
            _healthCheckTimer.AutoReset = True
            _healthCheckTimer.Enabled = True

            Debug.WriteLine("Connected to socket server via TCP")
            Return True
        Catch ex As Exception
            Debug.WriteLine("Failed to connect to server: " + ex.Message)
            _isConnected = False
            Return False
        End Try
    End Function

    Public Sub ConnectAsync()
        Connect()
    End Sub

    Public Sub RegisterUserAsync(userId As Integer, roleId As Integer)
        _currentUserId = userId
        _currentRoleId = roleId

        Try
            Dim message As New SocketMessage With {
                .Type = SocketMessageTypes.UserConnected,
                .Data = New UserConnectionData With {
                    .UserId = userId,
                    .RoleId = roleId,
                    .Username = CurrentUser.Email
                }
            }

            SendMessage(message)
            Debug.WriteLine("User registered: UserId=" + userId.ToString() + ", RoleId=" + roleId.ToString())
        Catch ex As Exception
            Debug.WriteLine("Failed to register user: " + ex.Message)
        End Try
    End Sub

    Public Sub SendRolePermissionChangedAsync(roleId As Integer)
        Try
            ' Set flag để skip notification cho user hiện tại
            If _currentRoleId.HasValue AndAlso _currentRoleId.Value = roleId Then
                _skipNextPermissionNotification = True
                Debug.WriteLine("Setting skip flag - current user initiated this change")
            End If

            Dim message As New SocketMessage With {
                .Type = SocketMessageTypes.RolePermissionChanged,
                .Data = New RolePermissionData With {
                    .RoleId = roleId
                }
            }

            SendMessage(message)
            Debug.WriteLine("Role permission changed notification sent: RoleId=" + roleId.ToString())
        Catch ex As Exception
            Debug.WriteLine("Failed to send role permission changed: " + ex.Message)
        End Try
    End Sub

    Public Sub SendUserRoleChangedAsync(userId As Integer, newRoleId As Integer)
        Try
            Dim message As New SocketMessage With {
                .Type = SocketMessageTypes.UserRoleChanged,
                .Data = New UserRoleChangeData With {
                    .UserId = userId,
                    .NewRoleId = newRoleId
                }
            }

            SendMessage(message)
            Debug.WriteLine("User role changed notification sent: UserId=" + userId.ToString() + ", NewRoleId=" + newRoleId.ToString())
        Catch ex As Exception
            Debug.WriteLine("Failed to send user role changed: " + ex.Message)
        End Try
    End Sub

    ' Test method to check if server broadcasts back
    Public Sub TestServerEcho()
        Try
            Dim message As New SocketMessage With {
                .Type = "TEST_ECHO",
                .Data = "Test message from VB.NET"
            }

            SendMessage(message)
            Debug.WriteLine("Test echo message sent")
        Catch ex As Exception
            Debug.WriteLine("Failed to send test echo: " + ex.Message)
        End Try
    End Sub

    Public Sub ForceLogoutUserAsync(userId As Integer, reason As String)
        Try
            Dim message As New SocketMessage With {
                .Type = SocketMessageTypes.ForceLogout,
                .Data = New ForceLogoutData With {
                    .UserId = userId,
                    .Reason = reason
                }
            }

            SendMessage(message)
            Debug.WriteLine("Force logout sent: UserId=" + userId.ToString() + ", Reason=" + reason)
        Catch ex As Exception
            Debug.WriteLine("Failed to send force logout: " + ex.Message)
        End Try
    End Sub

    Public Sub RequestOnlineUsersAsync()
        Try
            Dim message As New SocketMessage With {
                .Type = SocketMessageTypes.RequestOnlineUsers,
                .Data = Nothing
            }

            SendMessage(message)
            Debug.WriteLine("Requested online users list")
        Catch ex As Exception
            Debug.WriteLine("Failed to request online users: " + ex.Message)
        End Try
    End Sub

    ' Delegate for event handling
    Private Delegate Sub OnlineUsersEventHandler(onlineUsers As List(Of OnlineUser))
    Private Delegate Sub PermissionEventHandler(roleId As Integer)
    Private Delegate Sub LogoutEventHandler(reason As String)

    Private Sub SendMessage(message As SocketMessage)
        Debug.WriteLine("SendMessage called. IsConnected: " + IsConnected.ToString())

        ' Check if listener thread is still alive
        If _listenerThread IsNot Nothing AndAlso Not _listenerThread.IsAlive Then
            Debug.WriteLine("WARNING: Listener thread is dead! Attempting to restart...")
            RestartListener()
        End If

        If Not IsConnected Then
            Debug.WriteLine("Cannot send message - not connected")
            Return
        End If

        Try
            Dim json = _serializer.Serialize(message)
            Debug.WriteLine("Sending message: " + json)

            Dim data = Encoding.UTF8.GetBytes(json)
            Dim length = BitConverter.GetBytes(data.Length)

            ' Gửi length trước, rồi gửi data
            _networkStream.Write(length, 0, 4)
            _networkStream.Write(data, 0, data.Length)
            _networkStream.Flush()

            Debug.WriteLine("Message sent successfully")
        Catch ex As Exception
            Debug.WriteLine("Error sending message: " + ex.Message)
            Debug.WriteLine("Stack trace: " + ex.StackTrace)
        End Try
    End Sub

    Private Sub RestartListener()
        Try
            Debug.WriteLine("Restarting listener thread...")

            ' Tạo thread mới để lắng nghe
            _listenerThread = New Thread(AddressOf ListenForMessages)
            _listenerThread.IsBackground = True
            _listenerThread.Start()

            Debug.WriteLine("Listener thread restarted successfully")
        Catch ex As Exception
            Debug.WriteLine("Failed to restart listener thread: " + ex.Message)
        End Try
    End Sub

    Private Sub ListenForMessages()
        Dim buffer(4095) As Byte
        Debug.WriteLine("ListenForMessages started on thread: " + Thread.CurrentThread.ManagedThreadId.ToString())

        Try
            While _isConnected AndAlso _tcpClient.Connected
                Debug.WriteLine("Waiting for message...")

                ' Đọc message length (4 bytes)
                Dim lengthBytes(3) As Byte
                Dim bytesRead = _networkStream.Read(lengthBytes, 0, 4)
                Debug.WriteLine("Read " + bytesRead.ToString() + " bytes for length")

                If bytesRead = 0 Then
                    Debug.WriteLine("Connection closed by server (bytesRead = 0)")
                    Exit While
                End If

                Dim messageLength = BitConverter.ToInt32(lengthBytes, 0)
                Debug.WriteLine("Message length: " + messageLength.ToString())

                ' Đọc message data
                Dim messageBytes(messageLength - 1) As Byte
                Dim totalRead = 0
                While totalRead < messageLength
                    bytesRead = _networkStream.Read(messageBytes, totalRead, messageLength - totalRead)
                    If bytesRead = 0 Then Exit While
                    totalRead += bytesRead
                End While

                If totalRead = messageLength Then
                    Dim json = Encoding.UTF8.GetString(messageBytes)
                    Debug.WriteLine("Received message JSON: " + json)

                    Dim socketMessage = _serializer.Deserialize(Of SocketMessage)(json)

                    If socketMessage IsNot Nothing Then
                        Debug.WriteLine("Message deserialized successfully")
                        ProcessMessage(socketMessage)
                        Debug.WriteLine("Message processing completed, continuing listener loop...")
                    Else
                        Debug.WriteLine("Failed to deserialize message")
                    End If
                Else
                    Debug.WriteLine("Incomplete message received. Expected: " + messageLength.ToString() + ", Got: " + totalRead.ToString())
                End If

                Debug.WriteLine("End of message processing iteration, looping back...")
            End While
        Catch ex As Exception
            Debug.WriteLine("Error listening for messages: " + ex.Message)
            Debug.WriteLine("Stack trace: " + ex.StackTrace)

            ' Nếu connection vẫn tốt, thử restart listener
            If _tcpClient IsNot Nothing AndAlso _tcpClient.Connected Then
                Debug.WriteLine("Connection still good, attempting to restart listener...")
                Thread.Sleep(1000) ' Wait 1 second before restart
                RestartListener()
                Return ' Exit this thread instance
            End If
        Finally
            Debug.WriteLine("ListenForMessages ended. Thread: " + Thread.CurrentThread.ManagedThreadId.ToString())
        End Try
    End Sub

    Private Sub ProcessMessage(message As SocketMessage)
        Try
            Debug.WriteLine("Processing message type: " + message.Type)
            Debug.WriteLine("Processing message data: " + If(message.Data IsNot Nothing, message.Data.ToString(), "null"))

            Select Case message.Type
                Case SocketMessageTypes.UserConnected
                    Debug.WriteLine("Handling USER_CONNECTED message (no action needed)")
                Case SocketMessageTypes.RolePermissionChanged
                    Debug.WriteLine("Routing to HandleRolePermissionsChanged")
                    HandleRolePermissionsChanged(message)
                Case SocketMessageTypes.UserRoleChanged
                    Debug.WriteLine("Routing to HandleUserRoleChanged")
                    HandleUserRoleChanged(message)
                Case SocketMessageTypes.ForceLogout
                    Debug.WriteLine("Routing to HandleUserForceLogout")
                    HandleUserForceLogout(message)
                Case SocketMessageTypes.OnlineUsersList
                    Debug.WriteLine("Routing to HandleOnlineUsersList")
                    HandleOnlineUsersList(message)
                Case "TEST_ECHO"
                    Debug.WriteLine("Received TEST_ECHO response from server!")
                Case Else
                    Debug.WriteLine("Unknown message type: " + message.Type)
            End Select

        Catch ex As Exception
            Debug.WriteLine("Error processing message: " + ex.Message)
        End Try
    End Sub

    Private Sub HandleRolePermissionsChanged(message As SocketMessage)
        Try
            Debug.WriteLine("HandleRolePermissionsChanged called")
            Debug.WriteLine("Message data type: " + message.Data.GetType().ToString())

            Dim roleId As Integer = 0

            ' message.Data có thể là Dictionary hoặc RolePermissionData object
            If TypeOf message.Data Is Dictionary(Of String, Object) Then
                Dim dict = DirectCast(message.Data, Dictionary(Of String, Object))
                If dict.ContainsKey("RoleId") Then
                    roleId = Convert.ToInt32(dict("RoleId"))
                End If
            ElseIf TypeOf message.Data Is RolePermissionData Then
                Dim roleData = DirectCast(message.Data, RolePermissionData)
                roleId = roleData.RoleId
            Else
                ' Fallback: try to serialize back to JSON and deserialize
                Dim json = _serializer.Serialize(message.Data)
                Debug.WriteLine("Fallback JSON: " + json)
                Dim roleData = _serializer.Deserialize(Of RolePermissionData)(json)
                If roleData IsNot Nothing Then
                    roleId = roleData.RoleId
                End If
            End If

            Debug.WriteLine("Extracted RoleId: " + roleId.ToString())
            Debug.WriteLine("Current user RoleId: " + If(_currentRoleId.HasValue, _currentRoleId.Value.ToString(), "null"))

            ' Check if current user is affected
            If _currentRoleId.HasValue AndAlso _currentRoleId.Value = roleId Then
                Debug.WriteLine("Current user is affected, refreshing permissions")

                ' Refresh permissions from database
                RefreshCurrentUserPermissions()

                ' Always redirect to Home and update UI, but only show notification if from another user
                Debug.WriteLine("Skip flag status: " + _skipNextPermissionNotification.ToString())

                ' Always raise event for UI updates and redirect
                Debug.WriteLine("About to raise PermissionsChanged event...")
                Dim mainForm = GetMainForm()
                If mainForm IsNot Nothing Then
                    Debug.WriteLine("MainForm found, invoking event...")
                    If mainForm.InvokeRequired Then
                        Dim handler As New PermissionEventHandler(Sub(id As Integer)
                                                                      Debug.WriteLine("Event handler executing on UI thread...")
                                                                      RaiseEvent PermissionsChanged(id)
                                                                  End Sub)
                        mainForm.Invoke(handler, roleId)
                    Else
                        Debug.WriteLine("Event handler executing directly...")
                        RaiseEvent PermissionsChanged(roleId)
                    End If
                    Debug.WriteLine("Event raised successfully!")
                Else
                    Debug.WriteLine("ERROR: MainForm not found!")
                End If

                ' Reset skip flag after processing
                If _skipNextPermissionNotification Then
                    Debug.WriteLine("Resetting skip flag after processing")
                    _skipNextPermissionNotification = False
                End If
            Else
                Debug.WriteLine("Current user is NOT affected by this role change")
            End If
        Catch ex As Exception
            Debug.WriteLine("Error handling role permissions changed: " + ex.Message)
            Debug.WriteLine("Stack trace: " + ex.StackTrace)
        End Try
    End Sub

    Private Sub HandleUserRoleChanged(message As SocketMessage)
        Try
            Debug.WriteLine("HandleUserRoleChanged called")

            Dim userId As Integer = 0
            Dim newRoleId As Integer = 0

            ' Extract data
            If TypeOf message.Data Is Dictionary(Of String, Object) Then
                Dim dict = DirectCast(message.Data, Dictionary(Of String, Object))
                If dict.ContainsKey("UserId") Then userId = Convert.ToInt32(dict("UserId"))
                If dict.ContainsKey("NewRoleId") Then newRoleId = Convert.ToInt32(dict("NewRoleId"))
            Else
                Dim json = _serializer.Serialize(message.Data)
                Dim roleChangeData = _serializer.Deserialize(Of UserRoleChangeData)(json)
                userId = roleChangeData.UserId
                newRoleId = roleChangeData.NewRoleId
            End If

            Debug.WriteLine("UserRoleChanged: UserId=" + userId.ToString() + ", NewRoleId=" + newRoleId.ToString())

            ' Kiểm tra nếu user hiện tại bị thay đổi role
            If _currentUserId.HasValue AndAlso _currentUserId.Value = userId Then
                Debug.WriteLine("Current user role changed! Updating...")

                ' Cập nhật current role ID và CurrentUser
                _currentRoleId = newRoleId
                CurrentUser.UpdateRole(newRoleId)

                ' Refresh permissions từ database
                RefreshCurrentUserPermissions()

                ' Trigger UI update
                Dim mainForm = GetMainForm()
                If mainForm IsNot Nothing Then
                    If mainForm.InvokeRequired Then
                        Dim handler As New PermissionEventHandler(Sub(id As Integer) RaiseEvent PermissionsChanged(id))
                        mainForm.Invoke(handler, newRoleId)
                    Else
                        RaiseEvent PermissionsChanged(newRoleId)
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine("Error handling user role changed: " + ex.Message)
        End Try
    End Sub

    Private Sub HandleUserForceLogout(message As SocketMessage)
        Try
            Debug.WriteLine("HandleUserForceLogout called")
            Debug.WriteLine("Message data type: " + If(message.Data IsNot Nothing, message.Data.GetType().ToString(), "null"))
            Debug.WriteLine("Current user ID: " + If(_currentUserId.HasValue, _currentUserId.Value.ToString(), "null"))

            Dim userId As Integer = 0
            Dim reason As String = ""

            ' Handle different data types
            If TypeOf message.Data Is Dictionary(Of String, Object) Then
                Dim dict = DirectCast(message.Data, Dictionary(Of String, Object))
                Debug.WriteLine("Data is Dictionary with " + dict.Keys.Count.ToString() + " keys")
                If dict.ContainsKey("UserId") Then
                    userId = Convert.ToInt32(dict("UserId"))
                    Debug.WriteLine("Extracted UserId from dictionary: " + userId.ToString())
                End If
                If dict.ContainsKey("Reason") Then
                    reason = dict("Reason").ToString()
                    Debug.WriteLine("Extracted Reason from dictionary: " + reason)
                End If
            Else
                ' Fallback: serialize and deserialize
                Debug.WriteLine("Using fallback serialization method")
                Dim json = _serializer.Serialize(message.Data)
                Debug.WriteLine("Serialized data: " + json)
                Dim logoutData = _serializer.Deserialize(Of ForceLogoutData)(json)
                If logoutData IsNot Nothing Then
                    userId = logoutData.UserId
                    reason = logoutData.Reason
                    Debug.WriteLine("Extracted from deserialized object - UserId: " + userId.ToString() + ", Reason: " + reason)
                End If
            End If

            Debug.WriteLine("Final extracted - UserId: " + userId.ToString() + ", Reason: " + reason)

            ' Check if current user should be logged out
            If _currentUserId.HasValue AndAlso _currentUserId.Value = userId Then
                Debug.WriteLine("Current user matches logout target - raising UserForceLoggedOut event")

                ' Raise event on UI thread
                Dim mainForm = GetMainForm()
                If mainForm IsNot Nothing Then
                    Debug.WriteLine("MainForm found, invoking event on UI thread")
                    If mainForm.InvokeRequired Then
                        Dim handler As New LogoutEventHandler(Sub(r As String)
                                                                  Debug.WriteLine("Event handler executing on UI thread with reason: " + r)
                                                                  RaiseEvent UserForceLoggedOut(r)
                                                              End Sub)
                        mainForm.Invoke(handler, reason)
                    Else
                        Debug.WriteLine("Event handler executing directly with reason: " + reason)
                        RaiseEvent UserForceLoggedOut(reason)
                    End If
                    Debug.WriteLine("UserForceLoggedOut event raised successfully")
                Else
                    Debug.WriteLine("ERROR: MainForm not found!")
                End If
            Else
                Debug.WriteLine("Current user does NOT match logout target - ignoring")
            End If
        Catch ex As Exception
            Debug.WriteLine("Error handling force logout: " + ex.Message)
            Debug.WriteLine("Stack trace: " + ex.StackTrace)
        End Try
    End Sub

    Private Sub HandleOnlineUsersList(message As SocketMessage)
        Try
            Dim users As New List(Of OnlineUser)()

            ' Handle different data types
            If TypeOf message.Data Is Dictionary(Of String, Object) Then
                Dim dict = DirectCast(message.Data, Dictionary(Of String, Object))
                If dict.ContainsKey("Users") Then
                    ' Try to deserialize users list
                    Dim json = _serializer.Serialize(dict("Users"))
                    users = _serializer.Deserialize(Of List(Of OnlineUser))(json)
                End If
            Else
                ' Fallback: serialize and deserialize
                Dim json = _serializer.Serialize(message.Data)
                Dim onlineData = _serializer.Deserialize(Of OnlineUsersData)(json)
                If onlineData IsNot Nothing Then
                    users = onlineData.Users
                End If
            End If

            ' Raise event on UI thread
            Dim mainForm = GetMainForm()
            If mainForm IsNot Nothing Then
                If mainForm.InvokeRequired Then
                    Dim handler As New OnlineUsersEventHandler(Sub(userList As List(Of OnlineUser)) RaiseEvent OnlineUsersUpdated(userList))
                    mainForm.Invoke(handler, users)
                Else
                    RaiseEvent OnlineUsersUpdated(users)
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine("Error handling online users list: " + ex.Message)
        End Try
    End Sub

    Private Sub RefreshCurrentUserPermissions()
        Try
            If Not _currentRoleId.HasValue Then Return

            ' Query fresh permissions from database
            Dim freshPermissions = ServiceRegistry.PermissionService.GetAllByRole(_currentRoleId.Value)
            Dim permissionNames = New HashSet(Of String)()

            ' Convert to HashSet without LINQ
            For Each perm In freshPermissions
                permissionNames.Add(perm.Name)
            Next

            ' Update CurrentUser on UI thread
            Dim mainForm = GetMainForm()
            If mainForm IsNot Nothing Then
                If mainForm.InvokeRequired Then
                    Dim refreshHandler As New MethodInvoker(Sub()
                                                                CurrentUser.RefreshPermissions(permissionNames)
                                                                Debug.WriteLine("User permissions refreshed. Count: " + permissionNames.Count.ToString())
                                                            End Sub)
                    mainForm.Invoke(refreshHandler)
                Else
                    CurrentUser.RefreshPermissions(permissionNames)
                    Debug.WriteLine("User permissions refreshed. Count: " + permissionNames.Count.ToString())
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine("Error refreshing user permissions: " + ex.Message)
        End Try
    End Sub

    Private Function GetMainForm() As Form
        For Each form As Form In Application.OpenForms
            If TypeOf form Is MainForm Then
                Return form
            End If
        Next
        Return Nothing
    End Function

    Private Sub HealthCheck(sender As Object, e As System.Timers.ElapsedEventArgs)
        Try
            If _tcpClient IsNot Nothing AndAlso _tcpClient.Connected Then
                If _listenerThread Is Nothing OrElse Not _listenerThread.IsAlive Then
                    Debug.WriteLine("Health Check: Listener thread is dead, restarting...")
                    RestartListener()
                End If
            Else
                Debug.WriteLine("Health Check: Connection lost")
                _isConnected = False
            End If
        Catch ex As Exception
            Debug.WriteLine("Health Check error: " + ex.Message)
        End Try
    End Sub

    Public Sub CheckConnectionStatus()
        Debug.WriteLine("=== Connection Status Check ===")
        Debug.WriteLine("_isConnected: " + _isConnected.ToString())
        Debug.WriteLine("_tcpClient IsNot Nothing: " + (_tcpClient IsNot Nothing).ToString())
        If _tcpClient IsNot Nothing Then
            Debug.WriteLine("_tcpClient.Connected: " + _tcpClient.Connected.ToString())
        End If
        Debug.WriteLine("_listenerThread IsNot Nothing: " + (_listenerThread IsNot Nothing).ToString())
        If _listenerThread IsNot Nothing Then
            Debug.WriteLine("_listenerThread.IsAlive: " + _listenerThread.IsAlive.ToString())
            Debug.WriteLine("_listenerThread.ThreadState: " + _listenerThread.ThreadState.ToString())
        End If
        Debug.WriteLine("========================")
    End Sub

    Public Sub Disconnect()
        Try
            _isConnected = False

            If _healthCheckTimer IsNot Nothing Then
                _healthCheckTimer.Stop()
                _healthCheckTimer.Dispose()
                _healthCheckTimer = Nothing
            End If

            If _listenerThread IsNot Nothing AndAlso _listenerThread.IsAlive Then
                _listenerThread.Abort()
            End If

            If _networkStream IsNot Nothing Then
                _networkStream.Close()
            End If

            If _tcpClient IsNot Nothing Then
                _tcpClient.Close()
            End If

            Debug.WriteLine("Socket client disconnected")
        Catch ex As Exception
            Debug.WriteLine("Error disconnecting socket: " + ex.Message)
        End Try
    End Sub
End Class
