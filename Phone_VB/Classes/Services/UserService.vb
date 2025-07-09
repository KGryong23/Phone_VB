Public Class UserService
    Implements IUserService

    Private ReadOnly userRepo As IUserRepository

    Public Sub New(userRepo As IUserRepository)
        Me.userRepo = userRepo
    End Sub

    Public Function CheckLogin(request As LoginRequest) As Boolean Implements IUserService.CheckLogin
        If String.IsNullOrEmpty(request.Username) Then
            Throw New ArgumentException("Username cannot be empty")
        End If
        If String.IsNullOrEmpty(request.Password) Then
            Throw New ArgumentException("Password cannot be empty")
        End If
        Return userRepo.CheckLogin(request.Username, request.Password)
    End Function
End Class
