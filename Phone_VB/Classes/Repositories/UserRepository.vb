Imports System.Configuration
Imports System.Data.Odbc

Public Class UserRepository
    Implements IUserRepository

    Private ReadOnly connStr As String = ConfigurationManager.ConnectionStrings("PhoneDbConnection").ConnectionString

    Public Function CheckLogin(username As String, password As String) As User Implements IUserRepository.CheckLogin
        Dim result As New User
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT * FROM users WHERE username = ? AND password = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("username", username)
                cmd.Parameters.AddWithValue("password", password)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        result = MapToUser(reader)
                    End If
                End Using
            End Using
        End Using
        Return result
    End Function

    Public Function GetPaged(query As BaseQuery) As PagedResult(Of User) Implements IUserRepository.GetPaged
        Dim result As New List(Of User)()
        Dim totalRecords As Integer = 0
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim countQuery As String = "SELECT COUNT(*) FROM users"
            If Not String.IsNullOrEmpty(query.Keyword) Then
                countQuery &= " WHERE LOWER(username) LIKE ?"
            End If
            Using cmd As New OdbcCommand(countQuery, conn)
                If Not String.IsNullOrEmpty(query.Keyword) Then
                    cmd.Parameters.AddWithValue("keyword", "%" & query.Keyword.ToLower() & "%")
                End If
                totalRecords = Convert.ToInt32(cmd.ExecuteScalar())
            End Using

            Dim dataQuery As String = "SELECT * FROM users"
            If Not String.IsNullOrEmpty(query.Keyword) Then
                dataQuery &= " WHERE LOWER(username) LIKE ?"
            End If
            dataQuery &= " ORDER BY created_at DESC LIMIT ? OFFSET ?"
            Using cmd As New OdbcCommand(dataQuery, conn)
                If Not String.IsNullOrEmpty(query.Keyword) Then
                    cmd.Parameters.AddWithValue("keyword", "%" & query.Keyword.ToLower() & "%")
                End If
                cmd.Parameters.AddWithValue("take", query.Take)
                cmd.Parameters.AddWithValue("skip", query.Skip)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        result.Add(MapToUser(reader))
                    End While
                End Using
            End Using
        End Using
        Return New PagedResult(Of User)(result, totalRecords)
    End Function

    Public Function Add(user As User) As Boolean Implements IUserRepository.Add
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "INSERT INTO users (username, password, role_id, created_at, last_modified) VALUES (?, ?, ?, NOW(), NOW())"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("username", user.Username)
                cmd.Parameters.AddWithValue("password", user.Password)
                cmd.Parameters.AddWithValue("role_id", user.RoleId)
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                Return rowsAffected > 0
            End Using
        End Using
    End Function

    Public Function Update(user As User) As Boolean Implements IUserRepository.Update
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "UPDATE users SET username = ?, password = ?, role_id = ?, last_modified = NOW() WHERE id = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("username", user.Username)
                cmd.Parameters.AddWithValue("password", user.Password)
                cmd.Parameters.AddWithValue("role_id", user.RoleId)
                cmd.Parameters.AddWithValue("id", user.Id)
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                Return rowsAffected > 0
            End Using
        End Using
    End Function

    Public Function Delete(id As Integer) As Boolean Implements IUserRepository.Delete
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "DELETE FROM users WHERE id = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("id", id)
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                Return rowsAffected > 0
            End Using
        End Using
    End Function

    Public Function GetById(id As Integer) As User Implements IUserRepository.GetById
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT * FROM users WHERE id = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("id", id)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Return MapToUser(reader)
                    End If
                End Using
            End Using
        End Using
        Return Nothing
    End Function

    Private Function MapToUser(reader As OdbcDataReader) As User
        Dim user As New User()
        user.Id = reader.GetInt32(reader.GetOrdinal("id"))
        user.Username = reader.GetString(reader.GetOrdinal("username"))
        user.Password = reader.GetString(reader.GetOrdinal("password"))
        user.RoleId = reader.GetInt32(reader.GetOrdinal("role_id"))
        user.CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
        user.LastModified = reader.GetDateTime(reader.GetOrdinal("last_modified"))
        Return user
    End Function
End Class
