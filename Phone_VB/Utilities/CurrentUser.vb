Public Class CurrentUser
    Private Shared _userId As Integer?
    Private Shared _email As String
    Private Shared _roleId As String

    Public Shared Sub SetUser(id As Integer, email As String, roleId As String)
        _userId = id
        _email = email
        _roleId = roleId
    End Sub

    Public Shared Sub ClearUser()
        _userId = Nothing
        _email = Nothing
        _roleId = Nothing
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

    Public Shared ReadOnly Property RoleId As String
        Get
            Return _roleId
        End Get
    End Property

    Public Shared ReadOnly Property IsAdmin As Boolean
        Get
            Return _roleId = "admin"
        End Get
    End Property
End Class