﻿Public Interface IUserService
    Function CheckLogin(request As LoginRequest) As UserWithPermissionDto
End Interface
