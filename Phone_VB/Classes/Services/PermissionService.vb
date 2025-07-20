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
        Return permissionRepo.AddPermissionsToRole(roleId, permissionIds)
    End Function

    Public Function RemovePermissionFromRole(roleId As Integer, permissionId As Integer) As Boolean Implements IPermissionService.RemovePermissionFromRole
        Return permissionRepo.RemovePermissionFromRole(roleId, permissionId)
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