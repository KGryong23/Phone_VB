Public NotInheritable Class ServiceRegistry
    Public Shared UserService As IUserService
    Public Shared PhoneService As IPhoneService
    Public Shared BrandService As IBrandService
    Public Shared StockTransactionService As IStockTransactionService
    Public Shared RoleService As IRoleService
    Public Shared PermissionService As IPermissionService

    Public Shared Sub InitializeServices()
        UserService = New UserService(New UserRepository())
        PhoneService = New PhoneService(New PhoneRepository(), New BrandRepository())
        BrandService = New BrandService(New BrandRepository())
        StockTransactionService = New StockTransactionService(New StockTransactionRepository(), New PhoneRepository(), New UserRepository())
        RoleService = New RoleService(New RoleRepository())
        PermissionService = New PermissionService(New PermissionRepository())
    End Sub
End Class
