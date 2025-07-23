Public Interface IRoleRepository
    Function GetPaged(query As BaseQuery) As PagedResult(Of Role)
    Function Add(role As Role) As Boolean
    Function Update(role As Role) As Boolean
    Function Delete(id As Integer) As Boolean
    Function GetById(id As Integer) As Role
    Function GetAll() As List(Of Role) ' Thêm dòng này
End Interface
