Public Class CurrentUser
    Private Shared _userId As Integer?
    Private Shared _email As String
    Private Shared _roleId As Integer?
    Private Shared _permissionNames As HashSet(Of String)

    Public Shared Sub SetUser(id As Integer, email As String, roleId As Integer, Optional permissionNames As HashSet(Of String) = Nothing)
        _userId = id
        _email = email
        _roleId = roleId
        _permissionNames = permissionNames
    End Sub

    Public Shared Sub ClearUser()
        _userId = Nothing
        _email = Nothing
        _roleId = Nothing
        _permissionNames = Nothing
    End Sub

    Public Shared ReadOnly Property UserId As Integer?
        Get
            Return _userId
        End Get
    End Property

    Public Shared ReadOnly Property Email As String
        Get
            Return _email
        End Get
    End Property

    Public Shared ReadOnly Property RoleId As Integer?
        Get
            Return _roleId
        End Get
    End Property

    Public Shared ReadOnly Property PermissionNames As HashSet(Of String)
        Get
            Return _permissionNames
        End Get
    End Property

    ' Kiểm tra user có quyền nào đó không
    Public Shared Function HasPermission(permissionName As String) As Boolean
        Return _permissionNames IsNot Nothing AndAlso _permissionNames.Contains(permissionName)
    End Function
End Class