Public Interface IPermissionService
    Function GetPagedByRole(roleId As Integer, query As BaseQuery) As PagedResult(Of PermissionDto)
    Function GetAllByRole(roleId As Integer) As List(Of PermissionDto)
    Function AddPermissionsToRole(roleId As Integer, permissionIds As List(Of Integer)) As Boolean
    Function RemovePermissionFromRole(roleId As Integer, permissionId As Integer) As Boolean
    Function GetUnassignedPermissions(roleId As Integer) As List(Of PermissionDto)
End Interface