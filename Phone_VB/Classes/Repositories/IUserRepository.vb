Public Interface IUserRepository
    Function CheckLogin(username As String, password As String) As User
    Function GetPaged(query As BaseQuery) As PagedResult(Of User)
    Function Add(user As User) As Boolean
    Function Update(user As User) As Boolean
    Function Delete(id As Integer) As Boolean
    Function GetById(id As Integer) As User
End Interface
