Public Class CurrentUser
    Private Shared _userId As Integer?
    Private Shared _email As String

    Public Shared Sub SetUser(id As Integer, email As String)
        _userId = id
        _email = email
    End Sub

    Public Shared Sub ClearUser()
        _userId = Nothing
        _email = Nothing
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

End Class