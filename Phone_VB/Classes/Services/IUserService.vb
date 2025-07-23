Public Interface IUserService
    Function CheckLogin(request As LoginRequest) As UserDto
    Function GetPaged(query As BaseQuery) As PagedResult(Of UserDto)
    Function GetById(id As Integer) As UserDto
    Function Add(request As UserCreateRequest) As UserDto
    Function Update(request As UserUpdateRequest) As UserDto
    Sub Delete(id As Integer)
End Interface
