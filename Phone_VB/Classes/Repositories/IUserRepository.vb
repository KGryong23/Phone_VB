Public Interface IUserRepository
    Function CheckLogin(username As String, password As String) As User
End Interface
