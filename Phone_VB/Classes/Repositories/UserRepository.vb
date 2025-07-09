Imports System.Configuration
Imports System.Data.Odbc

Public Class UserRepository
    Implements IUserRepository

    Private ReadOnly connStr As String = ConfigurationManager.ConnectionStrings("PhoneDbConnection").ConnectionString

    Public Function CheckLogin(username As String, password As String) As Boolean Implements IUserRepository.CheckLogin
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT COUNT(*) FROM users WHERE username = ? AND password = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("username", username)
                cmd.Parameters.AddWithValue("password", password)
                Dim count As Integer = Convert.ToInt32(cmd.ExecuteScalar())
                Return count > 0
            End Using
        End Using
    End Function
End Class
