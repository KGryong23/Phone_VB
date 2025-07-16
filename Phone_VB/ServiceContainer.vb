Module ServiceContainer
    ' Khai báo các service dưới dạng Public ReadOnly
    Public UserService As IUserService
    Public PhoneService As IPhoneService
    Public BrandService As IBrandService

    ' Hàm để khởi tạo các service
    Public Sub InitializeServices()
        ' Khởi tạo các repository và service
        UserService = New UserService(New UserRepository())
        PhoneService = New PhoneService(New PhoneRepository(), New BrandRepository())
        BrandService = New BrandService(New BrandRepository())
    End Sub
End Module
