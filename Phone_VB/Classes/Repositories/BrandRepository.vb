Imports System.Configuration
Imports System.Data.Odbc

Public Class BrandRepository
    Implements IBrandRepository

    Private ReadOnly connStr As String = ConfigurationManager.ConnectionStrings("PhoneDbConnection").ConnectionString

    Public Function GetAll() As List(Of Brand) Implements IBrandRepository.GetAll
        Dim result As New List(Of Brand)()
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT * FROM brands"
            Using cmd As New OdbcCommand(query, conn)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        Dim brand As New Brand()
                        brand.Id = reader.GetInt32(reader.GetOrdinal("id"))
                        brand.Name = reader.GetString(reader.GetOrdinal("name"))
                        brand.CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
                        brand.LastModified = reader.GetDateTime(reader.GetOrdinal("last_modified"))
                        result.Add(brand)
                    End While
                End Using
            End Using
        End Using
        Return result
    End Function
End Class
