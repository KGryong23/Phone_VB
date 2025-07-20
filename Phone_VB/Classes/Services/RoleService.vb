Public Class RoleService
    Implements IRoleService

    Private ReadOnly roleRepo As IRoleRepository

    Public Sub New(roleRepo As IRoleRepository)
        Me.roleRepo = roleRepo
    End Sub

    Public Function GetPaged(query As BaseQuery) As PagedResult(Of RoleDto) Implements IRoleService.GetPaged
        Dim pagedRoles = roleRepo.GetPaged(query)
        Dim dtos As New List(Of RoleDto)        ' Fix: Use pagedRoles.Data instead of pagedRoles.Items if the property is named Data
        For Each r In pagedRoles.Data
            dtos.Add(MapToDto(r))
        Next
        Return New PagedResult(Of RoleDto)(dtos, pagedRoles.TotalRecords)
    End Function

    Public Function Add(request As RoleCreateRequest) As Boolean Implements IRoleService.Add
        Dim role = New Role With {
            .Name = request.Name,
            .Description = request.Description
        }
        Return roleRepo.Add(role)
    End Function

    Public Function Update(request As RoleUpdateRequest) As Boolean Implements IRoleService.Update
        Dim role = New Role With {
            .Id = request.Id,
            .Name = request.Name,
            .Description = request.Description
        }
        Return roleRepo.Update(role)
    End Function

    Public Function Delete(id As Integer) As Boolean Implements IRoleService.Delete
        Return roleRepo.Delete(id)
    End Function

    Public Function GetById(id As Integer) As RoleDto Implements IRoleService.GetById
        Dim role = roleRepo.GetById(id)
        If role Is Nothing Then Return Nothing
        Return MapToDto(role)
    End Function

    Private Function MapToDto(role As Role) As RoleDto
        Return New RoleDto With {
            .Id = role.Id,
            .Name = role.Name,
            .Description = role.Description
        }
    End Function
End Class
