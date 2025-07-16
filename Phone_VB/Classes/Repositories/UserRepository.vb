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
    Private Function MapToUser(reader As OdbcDataReader) As User
        Dim user As New User()
        user.Id = reader.GetInt32(reader.GetOrdinal("id"))
        user.Username = reader.GetString(reader.GetOrdinal("username"))
        user.Password = reader.GetString(reader.GetOrdinal("password"))
        user.RoleId = reader.GetString(reader.GetOrdinal("role_id"))
        user.CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
        user.LastModified = reader.GetDateTime(reader.GetOrdinal("last_modified"))
        Return user
    End Function
End Class
