Public NotInheritable Class ServiceRegistry
    Public Shared UserService As IUserService
    Public Shared PhoneService As IPhoneService
    Public Shared BrandService As IBrandService
    Public Shared StockTransactionService As IStockTransactionService

    Public Shared Sub InitializeServices()
        UserService = New UserService(New UserRepository())
        PhoneService = New PhoneService(New PhoneRepository(), New BrandRepository())
        BrandService = New BrandService(New BrandRepository())
        StockTransactionService = New StockTransactionService(New StockTransactionRepository(), New PhoneRepository(), New UserRepository())
    End Sub
End Class
