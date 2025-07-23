Public Class PermissionService
    Implements IPermissionService

    Private ReadOnly permissionRepo As IPermissionRepository

    Public Sub New(permissionRepo As IPermissionRepository)
        Me.permissionRepo = permissionRepo
    End Sub

    Public Function GetPagedByRole(roleId As Integer, query As BaseQuery) As PagedResult(Of PermissionDto) Implements IPermissionService.GetPagedByRole
        Dim paged = permissionRepo.GetPagedByRole(roleId, query)
        Dim dtos = paged.Data.Select(Function(p) MapToDto(p)).ToList()
        Return New PagedResult(Of PermissionDto)(dtos, paged.TotalRecords)
    End Function

    Public Function AddPermissionsToRole(roleId As Integer, permissionIds As List(Of Integer)) As Boolean Implements IPermissionService.AddPermissionsToRole
        Dim result = permissionRepo.AddPermissionsToRole(roleId, permissionIds)

        ' Gửi thông báo socket nếu thành công
        If result Then
            Try
                Debug.WriteLine("About to send socket notification for role: " + roleId.ToString())

                Dim socketClient = PermissionSocketClient.Instance
                If socketClient IsNot Nothing Then
                    socketClient.SendRolePermissionChangedAsync(roleId)
                End If
            Catch ex As Exception
                Debug.WriteLine("Failed to send socket notification: " + ex.Message)
            End Try
        End If

        Return result
    End Function

    Public Function RemovePermissionFromRole(roleId As Integer, permissionId As Integer) As Boolean Implements IPermissionService.RemovePermissionFromRole
        Dim result = permissionRepo.RemovePermissionFromRole(roleId, permissionId)

        ' Gửi thông báo socket nếu thành công
        If result Then
            Try
                Dim socketClient = PermissionSocketClient.Instance
                If socketClient IsNot Nothing Then
                    socketClient.SendRolePermissionChangedAsync(roleId)
                End If
            Catch ex As Exception
                Debug.WriteLine("Failed to send socket notification: " + ex.Message)
            End Try
        End If

        Return result
    End Function

    Public Function GetUnassignedPermissions(roleId As Integer) As List(Of PermissionDto) Implements IPermissionService.GetUnassignedPermissions
        Dim perms = permissionRepo.GetUnassignedPermissions(roleId)
        Return perms.Select(Function(p) MapToDto(p)).ToList()
    End Function

    Public Function GetAllByRole(roleId As Integer) As List(Of PermissionDto) Implements IPermissionService.GetAllByRole
        Dim perms = permissionRepo.GetAllByRole(roleId)
        Return perms.Select(Function(p) MapToDto(p)).ToList()
    End Function

    Private Function MapToDto(p As Permission) As PermissionDto
        Return New PermissionDto With {
            .Id = p.Id,
            .Name = p.Name,
            .Description = p.Description
        }
    End Function
End Class