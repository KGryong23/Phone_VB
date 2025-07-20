Imports System.Configuration
Imports System.Data.Odbc

Public Class RoleRepository
    Implements IRoleRepository

    Private ReadOnly connStr As String = ConfigurationManager.ConnectionStrings("PhoneDbConnection").ConnectionString

    Public Function GetPaged(query As BaseQuery) As PagedResult(Of Role) Implements IRoleRepository.GetPaged
        Dim result As New List(Of Role)()
        Dim totalRecords As Integer = 0
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim countQuery As String = "SELECT COUNT(*) FROM roles"
            If Not String.IsNullOrEmpty(query.Keyword) Then
                countQuery &= " WHERE LOWER(name) LIKE ?"
            End If
            Using cmd As New OdbcCommand(countQuery, conn)
                If Not String.IsNullOrEmpty(query.Keyword) Then
                    cmd.Parameters.AddWithValue("keyword", "%" & query.Keyword.ToLower() & "%")
                End If
                totalRecords = Convert.ToInt32(cmd.ExecuteScalar())
            End Using

            Dim dataQuery As String = "SELECT * FROM roles"
            If Not String.IsNullOrEmpty(query.Keyword) Then
                dataQuery &= " WHERE LOWER(name) LIKE ?"
            End If
            dataQuery &= " ORDER BY id DESC LIMIT ? OFFSET ?"
            Using cmd As New OdbcCommand(dataQuery, conn)
                If Not String.IsNullOrEmpty(query.Keyword) Then
                    cmd.Parameters.AddWithValue("keyword", "%" & query.Keyword.ToLower() & "%")
                End If
                cmd.Parameters.AddWithValue("take", query.Take)
                cmd.Parameters.AddWithValue("skip", query.Skip)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        result.Add(MapToRole(reader))
                    End While
                End Using
            End Using
        End Using
        Return New PagedResult(Of Role)(result, totalRecords)
    End Function

    Public Function Add(role As Role) As Boolean Implements IRoleRepository.Add
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "INSERT INTO roles (name, description) VALUES (?, ?)"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("name", role.Name)
                cmd.Parameters.AddWithValue("description", role.Description)
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                Return rowsAffected > 0
            End Using
        End Using
    End Function

    Public Function Update(role As Role) As Boolean Implements IRoleRepository.Update
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "UPDATE roles SET name = ?, description = ? WHERE id = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("name", role.Name)
                cmd.Parameters.AddWithValue("description", role.Description)
                cmd.Parameters.AddWithValue("id", role.Id)
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                Return rowsAffected > 0
            End Using
        End Using
    End Function

    Public Function Delete(id As Integer) As Boolean Implements IRoleRepository.Delete
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "DELETE FROM roles WHERE id = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("id", id)
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                Return rowsAffected > 0
            End Using
        End Using
    End Function

    Public Function GetById(id As Integer) As Role Implements IRoleRepository.GetById
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT * FROM roles WHERE id = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("id", id)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Return MapToRole(reader)
                    End If
                End Using
            End Using
        End Using
        Return Nothing
    End Function

    Private Function MapToRole(reader As OdbcDataReader) As Role
        Dim role As New Role()
        role.Id = reader.GetInt32(reader.GetOrdinal("id"))
        role.Name = reader.GetString(reader.GetOrdinal("name"))
        role.Description = If(Not reader.IsDBNull(reader.GetOrdinal("description")), reader.GetString(reader.GetOrdinal("description")), "")
        Return role
    End Function
End Class
