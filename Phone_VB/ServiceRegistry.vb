Public NotInheritable Class ServiceRegistry
    Public Shared UserService As IUserService
    Public Shared PhoneService As IPhoneService
    Public Shared BrandService As IBrandService

    Public Shared Sub InitializeServices()
        UserService = New UserService(New UserRepository())
        PhoneService = New PhoneService(New PhoneRepository(), New BrandRepository())
        BrandService = New BrandService(New BrandRepository())
    End Sub
End Class
