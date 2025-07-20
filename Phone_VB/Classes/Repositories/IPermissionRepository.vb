Public Interface IPermissionRepository
    Function GetPagedByRole(roleId As Integer, query As BaseQuery) As PagedResult(Of Permission)
    Function GetAllByRole(roleId As Integer) As List(Of Permission)
    Function AddPermissionsToRole(roleId As Integer, permissionIds As List(Of Integer)) As Boolean
    Function RemovePermissionFromRole(roleId As Integer, permissionId As Integer) As Boolean
    Function GetUnassignedPermissions(roleId As Integer) As List(Of Permission)
End Interface