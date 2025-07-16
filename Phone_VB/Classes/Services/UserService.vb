Public Class UserService
    Implements IUserService

    Private ReadOnly userRepo As IUserRepository

    Public Sub New(userRepo As IUserRepository)
        Me.userRepo = userRepo
    End Sub

    Public Function CheckLogin(request As LoginRequest) As UserWithPermissionDto Implements IUserService.CheckLogin
        If String.IsNullOrEmpty(request.Username) Then
            Throw New ArgumentException("Username cannot be empty")
        End If
        If String.IsNullOrEmpty(request.Password) Then
            Throw New ArgumentException("Password cannot be empty")
        End If
        Return MapToUerDto(userRepo.CheckLogin(request.Username, request.Password))
    End Function
    Private Function MapToUerDto(user As User) As UserWithPermissionDto
        Return New UserWithPermissionDto With {
            .Id = user.Id,
            .Username = user.Username
        }
    End Function
End Class
