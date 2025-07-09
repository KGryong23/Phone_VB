Public Class PhoneService
    Implements IPhoneService

    Private ReadOnly phoneRepo As IPhoneRepository
    Private ReadOnly brandRepo As IBrandRepository

    Public Sub New(phoneRepo As IPhoneRepository, brandRepo As IBrandRepository)
        Me.phoneRepo = phoneRepo
        Me.brandRepo = brandRepo
    End Sub

    Public Function GetById(id As Integer) As PhoneDto Implements IPhoneService.GetById
        Dim phone As Phone = phoneRepo.GetById(id)
        Return MapToPhoneDto(phone)
    End Function

    Public Function GetAll() As List(Of PhoneDto) Implements IPhoneService.GetAll
        Dim phones As List(Of Phone) = phoneRepo.GetAll()
        Dim result As New List(Of PhoneDto)()
        For Each phone As Phone In phones
            result.Add(MapToPhoneDto(phone))
        Next
        Return result
    End Function

    Public Function Add(request As CreatePhoneRequest) As PhoneDto Implements IPhoneService.Add
        If String.IsNullOrEmpty(request.Model) Then
            Throw New ArgumentException("Model cannot be empty")
        End If
        If request.Price <= 0 Then
            Throw New ArgumentException("Price must be greater than zero")
        End If
        If request.Stock < 0 Then
            Throw New ArgumentException("Stock cannot be negative")
        End If
        If Not brandRepo.GetAll().Any(Function(b) b.Id = request.BrandId) Then
            Throw New ArgumentException("Invalid BrandId")
        End If

        Dim phone As New Phone With {
            .Model = request.Model,
            .Price = request.Price,
            .Stock = request.Stock,
            .BrandId = request.BrandId
        }
        Dim addedPhone As Phone = phoneRepo.Add(phone)
        Return MapToPhoneDto(addedPhone)
    End Function

    Public Function Update(request As UpdatePhoneRequest) As PhoneDto Implements IPhoneService.Update
        If request.Id <= 0 Then
            Throw New ArgumentException("Invalid Phone Id")
        End If
        If String.IsNullOrEmpty(request.Model) Then
            Throw New ArgumentException("Model cannot be empty")
        End If
        If request.Price <= 0 Then
            Throw New ArgumentException("Price must be greater than zero")
        End If
        If request.Stock < 0 Then
            Throw New ArgumentException("Stock cannot be negative")
        End If
        If Not brandRepo.GetAll().Any(Function(b) b.Id = request.BrandId) Then
            Throw New ArgumentException("Invalid BrandId")
        End If

        Dim phone As New Phone With {
            .Id = request.Id,
            .Model = request.Model,
            .Price = request.Price,
            .Stock = request.Stock,
            .BrandId = request.BrandId
        }
        Dim updatedPhone As Phone = phoneRepo.Update(phone)
        Return MapToPhoneDto(updatedPhone)
    End Function

    Public Sub Delete(id As Integer) Implements IPhoneService.Delete
        If id <= 0 Then
            Throw New ArgumentException("Invalid Phone Id")
        End If
        phoneRepo.Delete(id)
    End Sub

    Public Function GetPaged(query As BaseQuery) As PagedResult(Of PhoneDto) Implements IPhoneService.GetPaged
        Dim pagedResult As PagedResult(Of Phone) = phoneRepo.GetPaged(query)
        Dim result As New List(Of PhoneDto)()
        For Each phone As Phone In pagedResult.Data
            result.Add(MapToPhoneDto(phone))
        Next
        Return New PagedResult(Of PhoneDto)(result, pagedResult.TotalRecords)
    End Function

    Private Function MapToPhoneDto(phone As Phone) As PhoneDto
        Dim brand As Brand = brandRepo.GetAll().FirstOrDefault(Function(b) b.Id = phone.BrandId)
        Dim brandName As String = If(brand IsNot Nothing, brand.Name, "Unknown")
        Return New PhoneDto With {
            .Id = phone.Id,
            .Model = phone.Model,
            .Price = phone.Price,
            .Stock = phone.Stock,
            .BrandName = brandName,
            .CreatedAt = phone.CreatedAt,
            .LastModified = phone.LastModified
        }
    End Function
End Class