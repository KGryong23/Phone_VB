Public Class UserService
    Implements IUserService

    Private ReadOnly userRepo As IUserRepository
    Private ReadOnly roleRepo As IRoleRepository

    Public Sub New(userRepo As IUserRepository, roleRepo As IRoleRepository)
        Me.userRepo = userRepo
        Me.roleRepo = roleRepo
    End Sub

    Public Function CheckLogin(request As LoginRequest) As UserDto Implements IUserService.CheckLogin
        If String.IsNullOrEmpty(request.Username) Then
            Throw New ArgumentException("Username cannot be empty")
        End If
        If String.IsNullOrEmpty(request.Password) Then
            Throw New ArgumentException("Password cannot be empty")
        End If
        Return MapToUserDto(userRepo.CheckLogin(request.Username, request.Password))
    End Function

    Public Function GetPaged(query As BaseQuery) As PagedResult(Of UserDto) Implements IUserService.GetPaged
        Dim result = userRepo.GetPaged(query)
        Dim userDtos As New List(Of UserDto)()

        ' Get all roles using GetPaged with large take value
        Dim roleQuery As New BaseQuery("", 0, 1000) ' Get up to 1000 roles
        Dim rolesResult = roleRepo.GetPaged(roleQuery)

        For Each user As User In result.Data
            Dim role = rolesResult.Data.FirstOrDefault(Function(r) r.Id = user.RoleId)
            Dim userDto = MapToUserDto(user)
            userDto.RoleName = If(role IsNot Nothing, role.Name, "Unknown")
            userDtos.Add(userDto)
        Next

        Return New PagedResult(Of UserDto)(userDtos, result.TotalRecords)
    End Function

    Public Function GetById(id As Integer) As UserDto Implements IUserService.GetById
        Dim user = userRepo.GetById(id)
        If user Is Nothing Then
            Throw New ArgumentException("User not found")
        End If
        Return MapToUserDto(user)
    End Function

    Public Function Add(request As UserCreateRequest) As UserDto Implements IUserService.Add
        If String.IsNullOrEmpty(request.Username) Then
            Throw New ArgumentException("Username cannot be empty")
        End If
        If String.IsNullOrEmpty(request.Password) Then
            Throw New ArgumentException("Password cannot be empty")
        End If
        If request.RoleId <= 0 Then
            Throw New ArgumentException("Invalid RoleId")
        End If

        Dim user As New User With {
            .Username = request.Username,
            .Password = request.Password,
            .RoleId = request.RoleId,
            .CreatedAt = DateTime.Now,
            .LastModified = DateTime.Now
        }

        Dim success = userRepo.Add(user)
        If Not success Then
            Throw New Exception("Failed to add user")
        End If

        ' Return the created user (note: we don't get the ID back from Add method)
        Return MapToUserDto(user)
    End Function

    Public Function Update(request As UserUpdateRequest) As UserDto Implements IUserService.Update
        If request.Id <= 0 Then
            Throw New ArgumentException("Invalid user ID")
        End If
        If request.RoleId <= 0 Then
            Throw New ArgumentException("Invalid RoleId")
        End If

        Dim existingUser = userRepo.GetById(request.Id)
        If existingUser Is Nothing Then
            Throw New ArgumentException("User not found")
        End If

        ' Lưu role cũ để so sánh
        Dim oldRoleId As Integer = existingUser.RoleId

        existingUser.Username = request.Username
        If String.IsNullOrEmpty(request.Password) Then
            Throw New ArgumentException("Password cannot be empty")
        End If
        existingUser.RoleId = request.RoleId
        existingUser.LastModified = DateTime.Now

        ' Only update password if provided
        If Not String.IsNullOrEmpty(request.Password) Then
            existingUser.Password = request.Password
        End If

        Dim success = userRepo.Update(existingUser)
        If Not success Then
            Throw New Exception("Failed to update user")
        End If

        ' Kiểm tra nếu role thay đổi thì gửi thông báo cập nhật quyền
        If oldRoleId <> request.RoleId Then
            Try
                Dim socketClient = PermissionSocketClient.Instance
                If socketClient IsNot Nothing Then
                    socketClient.SendUserRoleChangedAsync(request.Id, request.RoleId)
                    Debug.WriteLine("Sent user role change notification: UserId=" + request.Id.ToString() + ", OldRoleId=" + oldRoleId.ToString() + ", NewRoleId=" + request.RoleId.ToString())
                End If
            Catch ex As Exception
                Debug.WriteLine("Failed to send user role change notification: " + ex.Message)
                ' Không throw exception vì việc cập nhật user đã thành công
            End Try
        End If

        Return MapToUserDto(existingUser)
    End Function

    Public Sub Delete(id As Integer) Implements IUserService.Delete
        If id <= 0 Then
            Throw New ArgumentException("Invalid user ID")
        End If

        Dim user = userRepo.GetById(id)
        If user Is Nothing Then
            Throw New ArgumentException("User not found")
        End If

        Dim success = userRepo.Delete(id)
        If Not success Then
            Throw New Exception("Failed to delete user")
        End If
    End Sub

    Private Function MapToUserDto(user As User) As UserDto
        If user Is Nothing Then
            Return Nothing
        End If

        Dim role = roleRepo.GetById(user.RoleId)
        Dim roleName = If(role IsNot Nothing, role.Name, "Unknown")

        Return New UserDto With {
            .Id = user.Id,
            .RoleId = user.RoleId,
            .RoleName = roleName,
            .Username = user.Username,
            .Password = user.Password,
            .CreatedAt = user.CreatedAt,
            .LastModified = user.LastModified
        }
    End Function
End Class
