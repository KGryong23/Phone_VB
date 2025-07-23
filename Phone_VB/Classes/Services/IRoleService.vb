Public Interface IRoleService
    Function GetPaged(query As BaseQuery) As PagedResult(Of RoleDto)
    Function Add(request As RoleCreateRequest) As Boolean
    Function Update(request As RoleUpdateRequest) As Boolean
    Function Delete(id As Integer) As Boolean
    Function GetById(id As Integer) As RoleDto
    Function GetAll() As List(Of RoleDto) ' Thêm dòng này
End Interface
