'Imports System.Threading

'Public Class PermissionSocketClient
'    Private Shared hubConnection As HubConnection
'    Private Shared stockHubProxy As IHubProxy
'    Private Shared isConnected As Boolean = False
'    Private Shared syncContext As SynchronizationContext = SynchronizationContext.Current

'    ' Event when role is updated
'    Public Shared Event RoleUpdated(roleId As Integer)

'    Public Shared ReadOnly Property Connected As Boolean
'        Get
'            Return isConnected
'        End Get
'    End Property

'    ' Connect to SignalR hub (call once in MainForm)
'    Public Shared Function Connect(Optional serverUrl As String = "http://localhost:5000/", Optional hubName As String = "StockHub") As Boolean
'        If isConnected Then Return True
'        Try
'            hubConnection = New HubConnection(serverUrl)
'            stockHubProxy = hubConnection.CreateHubProxy(hubName)

'            ' Register callback for RoleUpdated event from server
'            stockHubProxy.On(Of Integer)("RoleUpdated", AddressOf OnRoleUpdated)

'            ' Start the connection synchronously (SignalR 1.x uses Start().Wait() or callbacks)
'            hubConnection.Start().Wait()
'            isConnected = True
'            Return True
'        Catch ex As Exception
'            MessageBox.Show("Không thể kết nối socket: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
'            isConnected = False
'            Return False
'        End Try
'    End Function

'    ' Callback for RoleUpdated event
'    Private Shared Sub OnRoleUpdated(updatedRoleId As Integer)
'        ' Ensure event is raised on the UI thread
'        If syncContext IsNot Nothing Then
'            syncContext.Post(New SendOrPostCallback(Sub(state) RaiseEvent RoleUpdated(updatedRoleId)), Nothing)
'        Else
'            RaiseEvent RoleUpdated(updatedRoleId)
'        End If
'    End Sub

'    Public Shared Sub Disconnect()
'        If hubConnection IsNot Nothing AndAlso isConnected Then
'            hubConnection.Stop()
'            isConnected = False
'        End If
'    End Sub
'End Class