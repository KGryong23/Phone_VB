Public Class UserDto
    Public Property Id As Integer
    Public Property Username As String
    Public Property Role As String
End Class

Public Class UserWithPermissionDto
    Public Property Id As Integer
    Public Property Username As String
    Public Property Permission As List(Of String) = New List(Of String)()
End Class