Public Class BrandService
    Implements IBrandService

    Private ReadOnly brandRepo As IBrandRepository

    Public Sub New(brandRepo As IBrandRepository)
        Me.brandRepo = brandRepo
    End Sub

    Public Function GetAll() As List(Of BrandDto) Implements IBrandService.GetAll
        Dim brands As List(Of Brand) = brandRepo.GetAll()
        Dim result As New List(Of BrandDto)()
        For Each brand As Brand In brands
            result.Add(New BrandDto With {
                .Id = brand.Id,
                .Name = brand.Name
            })
        Next
        Return result
    End Function
End Class
