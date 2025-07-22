Imports System.Net
Imports System.Text
Imports System.IO
Imports Newtonsoft.Json

Public Class PermissionSocketClient
    Private Shared _instance As PermissionSocketClient
    Private _timer As Timer
    Private _isConnected As Boolean = False
    Private _currentUserId As Integer?
    Private _currentRoleId As Integer?
    Private Const BASE_URL As String = "http://localhost:8080"

    ' Events
    Public Event PermissionsChanged(roleId As Integer)
    Public Event UserForceLoggedOut(reason As String)
    Public Event OnlineUsersUpdated(onlineUsers As List(Of OnlineUser))

    Public Sub New()
        ' Timer để polling messages mỗi 2 giây
        _timer = New Timer(AddressOf PollMessages, Nothing, Timeout.Infinite, 2000)
    End Sub

    Public ReadOnly Property IsConnected As Boolean
        Get
            Return _isConnected
        End Get
    End Property

    Public Async Function ConnectAsync() As Task
        Try
            ' Thử kết nối tới health endpoint để kiểm tra server
            Dim request As HttpWebRequest = CType(WebRequest.Create($"{BASE_URL}/health"), HttpWebRequest)
            request.Method = "GET"
            request.Timeout = 5000

            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                If response.StatusCode = HttpStatusCode.OK Then
                    _isConnected = True
                    ' Bắt đầu polling
                    _timer.Change(0, 2000)
                    Debug.WriteLine("Connected to socket server via HTTP polling")
                End If
            End Using
        Catch ex As Exception
            Debug.WriteLine($"Failed to connect to server: {ex.Message}")
            _isConnected = False
        End Try
    End Function

    Public Async Function RegisterUserAsync(userId As Integer, roleId As Integer) As Task
        _currentUserId = userId
        _currentRoleId = roleId

        Try
            Dim data = New With {
                .UserId = userId,
                .RoleId = roleId,
                .Username = CurrentUser.Email
            }

            Await SendPostRequestAsync("/api/register", data)
            Debug.WriteLine($"User registered: UserId={userId}, RoleId={roleId}")
        Catch ex As Exception
            Debug.WriteLine($"Failed to register user: {ex.Message}")
        End Try
    End Function

    Public Async Function SendRolePermissionChangedAsync(roleId As Integer) As Task
        Try
            Dim data = New With {.RoleId = roleId}
            Await SendPostRequestAsync("/api/role-permission-changed", data)
            Debug.WriteLine($"Role permission changed notification sent: RoleId={roleId}")
        Catch ex As Exception
            Debug.WriteLine($"Failed to send role permission changed: {ex.Message}")
        End Try
    End Function

    Public Async Function ForceLogoutUserAsync(userId As Integer, reason As String) As Task
        Try
            Dim data = New With {
                .UserId = userId,
                .Reason = reason
            }
            Await SendPostRequestAsync("/api/force-logout", data)
            Debug.WriteLine($"Force logout sent: UserId={userId}, Reason={reason}")
        Catch ex As Exception
            Debug.WriteLine($"Failed to send force logout: {ex.Message}")
        End Try
    End Function

    Public Async Function RequestOnlineUsersAsync() As Task
        Try
            Dim response = Await SendGetRequestAsync("/api/online-users")
            Dim onlineUsers = JsonConvert.DeserializeObject(Of List(Of OnlineUser))(response)
            
            ' Trigger event on UI thread
            Dim mainForm = GetMainForm()
            If mainForm IsNot Nothing Then
                mainForm.Invoke(Sub() RaiseEvent OnlineUsersUpdated(onlineUsers))
            End If
        Catch ex As Exception
            Debug.WriteLine($"Failed to get online users: {ex.Message}")
        End Try
    End Function

    Private Sub PollMessages(state As Object)
        If Not _isConnected OrElse Not _currentUserId.HasValue Then Return

        Try
            ' Poll for messages
            Dim response = SendGetRequestSync($"/api/messages/{_currentUserId.Value}")
            If Not String.IsNullOrEmpty(response) Then
                Dim messages = JsonConvert.DeserializeObject(Of List(Of SocketMessage))(response)
                For Each message In messages
                    ProcessMessage(message)
                Next
            End If
        Catch ex As Exception
            Debug.WriteLine($"Error polling messages: {ex.Message}")
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
            Debug.WriteLine($"Error processing message: {ex.Message}")
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
                    mainForm.Invoke(Sub() RaiseEvent PermissionsChanged(roleData.RoleId))
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine($"Error handling role permissions changed: {ex.Message}")
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
                    mainForm.Invoke(Sub() RaiseEvent UserForceLoggedOut(logoutData.Reason))
                End If
            End If
        Catch ex As Exception
            Debug.WriteLine($"Error handling force logout: {ex.Message}")
        End Try
    End Sub

    Private Sub HandleOnlineUsersList(message As SocketMessage)
        Try
            Dim onlineData = JsonConvert.DeserializeObject(Of OnlineUsersData)(message.Data.ToString())
            If onlineData Is Nothing Then Return

            ' Raise event on UI thread
            Dim mainForm = GetMainForm()
            If mainForm IsNot Nothing Then
                mainForm.Invoke(Sub() RaiseEvent OnlineUsersUpdated(onlineData.Users))
            End If
        Catch ex As Exception
            Debug.WriteLine($"Error handling online users list: {ex.Message}")
        End Try
    End Sub

    Private Sub RefreshCurrentUserPermissions()
        Try
            If Not _currentRoleId.HasValue Then Return

            ' Query fresh permissions from database
            Dim freshPermissions = ServiceRegistry.PermissionService.GetAllByRole(_currentRoleId.Value)
            Dim permissionNames = New HashSet(Of String)(freshPermissions.Select(Function(p) p.Name))

            ' Update CurrentUser on UI thread
            Dim mainForm = GetMainForm()
            If mainForm IsNot Nothing Then
                mainForm.Invoke(Sub()
                    CurrentUser.RefreshPermissions(permissionNames)
                    Debug.WriteLine($"User permissions refreshed. Count: {permissionNames.Count}")
                End Sub)
            End If
        Catch ex As Exception
            Debug.WriteLine($"Error refreshing user permissions: {ex.Message}")
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

    Private Async Function SendPostRequestAsync(endpoint As String, data As Object) As Task
        Dim json = JsonConvert.SerializeObject(data)
        Dim bytes = Encoding.UTF8.GetBytes(json)

        Dim request As HttpWebRequest = CType(WebRequest.Create($"{BASE_URL}{endpoint}"), HttpWebRequest)
        request.Method = "POST"
        request.ContentType = "application/json"
        request.ContentLength = bytes.Length

        Using stream = request.GetRequestStream()
            stream.Write(bytes, 0, bytes.Length)
        End Using

        Using response = request.GetResponse()
            ' Request completed
        End Using
    End Function

    Private Async Function SendGetRequestAsync(endpoint As String) As Task(Of String)
        Dim request As HttpWebRequest = CType(WebRequest.Create($"{BASE_URL}{endpoint}"), HttpWebRequest)
        request.Method = "GET"

        Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
            Using reader As New StreamReader(response.GetResponseStream())
                Return reader.ReadToEnd()
            End Using
        End Using
    End Function

    Private Function SendGetRequestSync(endpoint As String) As String
        Try
            Dim request As HttpWebRequest = CType(WebRequest.Create($"{BASE_URL}{endpoint}"), HttpWebRequest)
            request.Method = "GET"
            request.Timeout = 5000

            Using response As HttpWebResponse = CType(request.GetResponse(), HttpWebResponse)
                Using reader As New StreamReader(response.GetResponseStream())
                    Return reader.ReadToEnd()
                End Using
            End Using
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Sub Disconnect()
        Try
            _isConnected = False
            _timer?.Change(Timeout.Infinite, Timeout.Infinite)
            Debug.WriteLine("Socket client disconnected")
        Catch ex As Exception
            Debug.WriteLine($"Error disconnecting socket: {ex.Message}")
        End Try
    End Sub
End Class
