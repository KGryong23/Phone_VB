Public Class SocketMessage
    Public Property Type As String = String.Empty
    Public Property Data As Object = New Object()
End Class

Public Class RolePermissionData
    Public Property RoleId As Integer
End Class

Public Class ForceLogoutData
    Public Property UserId As Integer
    Public Property Reason As String = String.Empty
End Class

Public Class UserRoleChangeData
    Public Property UserId As Integer
    Public Property NewRoleId As Integer
End Class

Public Class UserConnectionData
    Public Property UserId As Integer
    Public Property Username As String = String.Empty
    Public Property RoleId As Integer
    Public Property ConnectedAt As String = String.Empty
End Class

Public Class OnlineUsersData
    Public Property Users As List(Of OnlineUser) = New List(Of OnlineUser)()
End Class

Public Class OnlineUser
    Public Property UserId As Integer
    Public Property Username As String = String.Empty
    Public Property RoleId As Integer
    Public Property ConnectedAt As String = String.Empty
End Class
