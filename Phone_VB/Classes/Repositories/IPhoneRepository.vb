Public Interface IPhoneRepository
    Function GetById(id As Integer) As Phone
    Function GetAll() As List(Of Phone)
    Function Add(phone As Phone) As Phone
    Function Update(phone As Phone) As Phone
    Sub Delete(id As Integer)
    Function GetPaged(query As BaseQuery) As PagedResult(Of Phone)
End Interface
