Public Interface IPhoneService
    Function GetById(id As Integer) As PhoneDto
    Function GetAll() As List(Of PhoneDto)
    Function Add(request As CreatePhoneRequest) As PhoneDto
    Function Update(request As UpdatePhoneRequest) As PhoneDto
    Sub Delete(id As Integer)
    Function GetPaged(query As BaseQuery) As PagedResult(Of PhoneDto)
End Interface
