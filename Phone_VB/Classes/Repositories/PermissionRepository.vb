Imports System.Configuration
Imports System.Data.Odbc

Public Class PermissionRepository
    Implements IPermissionRepository

    Private ReadOnly connStr As String = ConfigurationManager.ConnectionStrings("PhoneDbConnection").ConnectionString

    Public Function GetPagedByRole(roleId As Integer, query As BaseQuery) As PagedResult(Of Permission) Implements IPermissionRepository.GetPagedByRole
        Dim result As New List(Of Permission)()
        Dim totalRecords As Integer = 0
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim countQuery As String = "SELECT COUNT(*) FROM permissions p INNER JOIN role_permissions rp ON p.id = rp.permission_id WHERE rp.role_id = ?"
            If Not String.IsNullOrEmpty(query.Keyword) Then
                countQuery &= " AND LOWER(p.name) LIKE ?"
            End If
            Using cmd As New OdbcCommand(countQuery, conn)
                cmd.Parameters.AddWithValue("role_id", roleId)
                If Not String.IsNullOrEmpty(query.Keyword) Then
                    cmd.Parameters.AddWithValue("keyword", "%" & query.Keyword.ToLower() & "%")
                End If
                totalRecords = Convert.ToInt32(cmd.ExecuteScalar())
            End Using

            Dim dataQuery As String = "SELECT p.* FROM permissions p INNER JOIN role_permissions rp ON p.id = rp.permission_id WHERE rp.role_id = ?"
            If Not String.IsNullOrEmpty(query.Keyword) Then
                dataQuery &= " AND LOWER(p.name) LIKE ?"
            End If
            dataQuery &= " ORDER BY p.id DESC LIMIT ? OFFSET ?"
            Using cmd As New OdbcCommand(dataQuery, conn)
                cmd.Parameters.AddWithValue("role_id", roleId)
                If Not String.IsNullOrEmpty(query.Keyword) Then
                    cmd.Parameters.AddWithValue("keyword", "%" & query.Keyword.ToLower() & "%")
                End If
                cmd.Parameters.AddWithValue("take", query.Take)
                cmd.Parameters.AddWithValue("skip", query.Skip)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        result.Add(MapToPermission(reader))
                    End While
                End Using
            End Using
        End Using
        Return New PagedResult(Of Permission)(result, totalRecords)
    End Function

    Public Function AddPermissionsToRole(roleId As Integer, permissionIds As List(Of Integer)) As Boolean Implements IPermissionRepository.AddPermissionsToRole
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            For Each permId In permissionIds
                Dim query As String = "INSERT IGNORE INTO role_permissions (role_id, permission_id) VALUES (?, ?)"
                Using cmd As New OdbcCommand(query, conn)
                    cmd.Parameters.AddWithValue("role_id", roleId)
                    cmd.Parameters.AddWithValue("permission_id", permId)
                    cmd.ExecuteNonQuery()
                End Using
            Next
        End Using
        Return True
    End Function

    Public Function RemovePermissionFromRole(roleId As Integer, permissionId As Integer) As Boolean Implements IPermissionRepository.RemovePermissionFromRole
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "DELETE FROM role_permissions WHERE role_id = ? AND permission_id = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("role_id", roleId)
                cmd.Parameters.AddWithValue("permission_id", permissionId)
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                Return rowsAffected > 0
            End Using
        End Using
    End Function

    Public Function GetUnassignedPermissions(roleId As Integer) As List(Of Permission) Implements IPermissionRepository.GetUnassignedPermissions
        Dim result As New List(Of Permission)()
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT * FROM permissions WHERE id NOT IN (SELECT permission_id FROM role_permissions WHERE role_id = ?)"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("role_id", roleId)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        result.Add(MapToPermission(reader))
                    End While
                End Using
            End Using
        End Using
        Return result
    End Function

    Public Function GetAllByRole(roleId As Integer) As List(Of Permission) Implements IPermissionRepository.GetAllByRole
        Dim result As New List(Of Permission)()
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT p.* FROM permissions p INNER JOIN role_permissions rp ON p.id = rp.permission_id WHERE rp.role_id = ? ORDER BY p.id DESC"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("role_id", roleId)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        result.Add(MapToPermission(reader))
                    End While
                End Using
            End Using
        End Using
        Return result
    End Function

    Private Function MapToPermission(reader As OdbcDataReader) As Permission
        Dim perm As New Permission()
        perm.Id = reader.GetInt32(reader.GetOrdinal("id"))
        perm.Name = reader.GetString(reader.GetOrdinal("name"))
        perm.Description = If(Not reader.IsDBNull(reader.GetOrdinal("description")), reader.GetString(reader.GetOrdinal("description")), "")
        Return perm
    End Function
End Class