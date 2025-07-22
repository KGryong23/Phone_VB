Imports System.Net
Imports System.Text
Imports System.IO
Imports Newtonsoft.Json

Public Class PermissionSocketClient
    Private Shared _instance As PermissionSocketClient
    Private WithEvents _timer As Timer
    Private _isConnected As Boolean = False
    Private _currentUserId As Integer?
    Private _currentRoleId As Integer?
    Private Const BASE_URL As String = "http://localhost:8080"

    ' Events
    Public Event PermissionsChanged(roleId As Integer)
    Public Event UserForceLoggedOut(reason As String)
    Public Event OnlineUsersUpdated(onlineUsers As List(Of OnlineUser))

    Public Sub New()
        ' Timer để polling messages mỗi 3 giây
        _timer = New Timer(AddressOf PollMessages, Nothing, Timeout.Infinite, 3000)
    End Sub

    Public ReadOnly Property IsConnected As Boolean
        Get
            Return _isConnected
        End Get
    End Property

    Public Sub ConnectAsync()
        Try
            ' Thử kết nối tới health endpoint để kiểm tra server
            Dim request As HttpWebRequest = CType(WebRequest.Create(BASE_URL + "/health"), HttpWebRequest)
            request.Method = "GET"
            request.Timeout = 5000

            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                If response.StatusCode = HttpStatusCode.OK Then
                    _isConnected = True
                    ' Bắt đầu polling
                    _timer.Change(1000, 3000) ' Start after 1 second, repeat every 3 seconds
                    Debug.WriteLine("Connected to socket server via HTTP polling")
                End If
            End Using
        Catch ex As Exception
            Debug.WriteLine("Failed to connect to server: " + ex.Message)
            _isConnected = False
        End Try
    End Sub

    Public Sub RegisterUserAsync(userId As Integer, roleId As Integer)
        _currentUserId = userId
        _currentRoleId = roleId

        Try
            Dim data = New With {
                .UserId = userId,
                .RoleId = roleId,
                .Username = CurrentUser.Email
            }

            SendPostRequest("/api/register", data)
            Debug.WriteLine("User registered: UserId=" + userId.ToString() + ", RoleId=" + roleId.ToString())
        Catch ex As Exception
            Debug.WriteLine("Failed to register user: " + ex.Message)
        End Try
    End Sub

    Public Sub SendRolePermissionChangedAsync(roleId As Integer)
        Try
            Dim data = New With {.RoleId = roleId}
            SendPostRequest("/api/role-permission-changed", data)
            Debug.WriteLine("Role permission changed notification sent: RoleId=" + roleId.ToString())
        Catch ex As Exception
            Debug.WriteLine("Failed to send role permission changed: " + ex.Message)
        End Try
    End Sub

    Public Sub ForceLogoutUserAsync(userId As Integer, reason As String)
        Try
            Dim data = New With {
                .UserId = userId,
                .Reason = reason
            }
            SendPostRequest("/api/force-logout", data)
            Debug.WriteLine("Force logout sent: UserId=" + userId.ToString() + ", Reason=" + reason)
        Catch ex As Exception
            Debug.WriteLine("Failed to send force logout: " + ex.Message)
        End Try
    End Sub

    Public Sub RequestOnlineUsersAsync()
        Try
            Dim response = SendGetRequest("/api/online-users")
            If Not String.IsNullOrEmpty(response) Then
                Dim onlineUsers = JsonConvert.DeserializeObject(Of List(Of OnlineUser))(response)
                
                ' Trigger event on UI thread
                Dim mainForm = GetMainForm()
                If mainForm IsNot Nothing Then
                    If mainForm.InvokeRequired Then
                        Dim handler As New OnlineUsersEventHandler(Sub(users As List(Of OnlineUser)) RaiseEvent OnlineUsersUpdated(users))
                        mainForm.Invoke(handler, onlineUsers)
                    Else
                        RaiseEvent OnlineUsersUpdated(onlineUsers)
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine("Failed to get online users: " + ex.Message)
        End Try
    End Sub

    ' Delegate for event handling
    Private Delegate Sub OnlineUsersEventHandler(onlineUsers As List(Of OnlineUser))
    Private Delegate Sub PermissionEventHandler(roleId As Integer)
    Private Delegate Sub LogoutEventHandler(reason As String)

    Private Sub PollMessages(state As Object)
        If Not _isConnected OrElse Not _currentUserId.HasValue Then Return

        Try
            ' Poll for messages
            Dim response = SendGetRequest("/api/messages/" + _currentUserId.Value.ToString())
            If Not String.IsNullOrEmpty(response) Then
                Dim messages = JsonConvert.DeserializeObject(Of List(Of SocketMessage))(response)
                If messages IsNot Nothing Then
                    For Each message In messages
                        ProcessMessage(message)
                    Next
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine("Error polling messages: " + ex.Message)
        End Try
    End Sub

    Private Sub ProcessMessage(message As SocketMessage)
        Try
            Select Case message.Type
                Case SocketMessageTypes.RolePermissionChanged
                    HandleRolePermissionsChanged(message)
                Case SocketMessageTypes.ForceLogout
                    HandleUserForceLogout(message)
                Case SocketMessageTypes.OnlineUsersList
                    HandleOnlineUsersList(message)
            End Select
        Catch ex As Exception
            Debug.WriteLine("Error processing message: " + ex.Message)
        End Try
    End Sub

    Private Sub HandleRolePermissionsChanged(message As SocketMessage)
        Try
            Dim roleData = JsonConvert.DeserializeObject(Of RolePermissionData)(message.Data.ToString())
            If roleData Is Nothing Then Return

            ' Check if current user is affected
            If _currentRoleId.HasValue AndAlso _currentRoleId.Value = roleData.RoleId Then
                ' Refresh permissions from database
                RefreshCurrentUserPermissions()
                
                ' Raise event on UI thread
                Dim mainForm = GetMainForm()
                If mainForm IsNot Nothing Then
                    If mainForm.InvokeRequired Then
                        Dim handler As New PermissionEventHandler(Sub(id As Integer) RaiseEvent PermissionsChanged(id))
                        mainForm.Invoke(handler, roleData.RoleId)
                    Else
                        RaiseEvent PermissionsChanged(roleData.RoleId)
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine("Error handling role permissions changed: " + ex.Message)
        End Try
    End Sub

    Private Sub HandleUserForceLogout(message As SocketMessage)
        Try
            Dim logoutData = JsonConvert.DeserializeObject(Of ForceLogoutData)(message.Data.ToString())
            If logoutData Is Nothing Then Return

            ' Check if current user should be logged out
            If _currentUserId.HasValue AndAlso _currentUserId.Value = logoutData.UserId Then
                ' Raise event on UI thread
                Dim mainForm = GetMainForm()
                If mainForm IsNot Nothing Then
                    If mainForm.InvokeRequired Then
                        Dim handler As New LogoutEventHandler(Sub(r As String) RaiseEvent UserForceLoggedOut(r))
                        mainForm.Invoke(handler, logoutData.Reason)
                    Else
                        RaiseEvent UserForceLoggedOut(logoutData.Reason)
                    End If
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine("Error handling force logout: " + ex.Message)
        End Try
    End Sub

    Private Sub HandleOnlineUsersList(message As SocketMessage)
        Try
            Dim onlineData = JsonConvert.DeserializeObject(Of OnlineUsersData)(message.Data.ToString())
            If onlineData Is Nothing Then Return

            ' Raise event on UI thread
            Dim mainForm = GetMainForm()
            If mainForm IsNot Nothing Then
                If mainForm.InvokeRequired Then
                    Dim handler As New OnlineUsersEventHandler(Sub(users As List(Of OnlineUser)) RaiseEvent OnlineUsersUpdated(users))
                    mainForm.Invoke(handler, onlineData.Users)
                Else
                    RaiseEvent OnlineUsersUpdated(onlineData.Users)
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

    Private Sub SendPostRequest(endpoint As String, data As Object)
        Try
            Dim json = JsonConvert.SerializeObject(data)
            Dim bytes = Encoding.UTF8.GetBytes(json)

            Dim request As HttpWebRequest = CType(WebRequest.Create(BASE_URL + endpoint), HttpWebRequest)
            request.Method = "POST"
            request.ContentType = "application/json"
            request.ContentLength = bytes.Length
            request.Timeout = 5000

            Using stream = request.GetRequestStream()
                stream.Write(bytes, 0, bytes.Length)
            End Using

            Using response = request.GetResponse()
                ' Request completed
            End Using
        Catch ex As Exception
            Debug.WriteLine("Error in POST request to " + endpoint + ": " + ex.Message)
        End Try
    End Sub

    Private Function SendGetRequest(endpoint As String) As String
        Try
            Dim request As HttpWebRequest = CType(WebRequest.Create(BASE_URL + endpoint), HttpWebRequest)
            request.Method = "GET"
            request.Timeout = 5000

            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(response.GetResponseStream())
                    Return reader.ReadToEnd()
                End Using
            End Using
        Catch ex As Exception
            Debug.WriteLine("Error in GET request to " + endpoint + ": " + ex.Message)
            Return ""
        End Try
    End Function

    Public Sub Disconnect()
        Try
            _isConnected = False
            If _timer IsNot Nothing Then
                _timer.Change(Timeout.Infinite, Timeout.Infinite)
            End If
            Debug.WriteLine("Socket client disconnected")
        Catch ex As Exception
            Debug.WriteLine("Error disconnecting socket: " + ex.Message)
        End Try
    End Sub
End Class
