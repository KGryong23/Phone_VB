Imports System.Configuration
Imports System.Data.Odbc

Public Class PhoneRepository
    Implements IPhoneRepository

    Private ReadOnly connStr As String = ConfigurationManager.ConnectionStrings("PhoneDbConnection").ConnectionString

    Public Function GetById(id As Integer) As Phone Implements IPhoneRepository.GetById
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT * FROM phones WHERE id = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("id", id)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Return MapToPhone(reader)
                    End If
                End Using
            End Using
        End Using
        Throw New Exception("Phone not found")
    End Function

    Public Function GetAll() As List(Of Phone) Implements IPhoneRepository.GetAll
        Dim result As New List(Of Phone)()
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT * FROM phones"
            Using cmd As New OdbcCommand(query, conn)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        result.Add(MapToPhone(reader))
                    End While
                End Using
            End Using
        End Using
        Return result
    End Function

    Public Function Add(phone As Phone) As Phone Implements IPhoneRepository.Add
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "INSERT INTO phones (model, price, stock, brand_id, created_at, last_modified) VALUES (?, ?, ?, ?, ?, ?)"
            Using cmd As New OdbcCommand(query, conn)
                Dim now As DateTime = DateTime.Now
                ' Explicitly specify parameter types to avoid binding issues
                cmd.Parameters.AddWithValue("", phone.Model)
                cmd.Parameters.AddWithValue("", phone.Price)
                cmd.Parameters.AddWithValue("", phone.Stock)
                cmd.Parameters.AddWithValue("", phone.BrandId)
                cmd.Parameters.AddWithValue("", now)
                cmd.Parameters.AddWithValue("", now)
                ' Thực thi câu lệnh INSERT
                cmd.ExecuteNonQuery()

                ' Retrieve the last inserted ID
                Dim idQuery As String = "SELECT LAST_INSERT_ID()"
                Using idCmd As New OdbcCommand(idQuery, conn)
                    phone.Id = Convert.ToInt32(idCmd.ExecuteScalar())
                End Using
            End Using
        End Using
        Return phone
    End Function

    Public Function Update(phone As Phone) As Phone Implements IPhoneRepository.Update
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "UPDATE phones SET model = ?, price = ?, stock = ?, brand_id = ?, last_modified = ? WHERE id = ?"
            Using cmd As New OdbcCommand(query, conn)
                Dim now As DateTime = DateTime.Now
                cmd.Parameters.AddWithValue("", phone.Model)
                cmd.Parameters.AddWithValue("", phone.Price)
                cmd.Parameters.AddWithValue("", phone.Stock)
                cmd.Parameters.AddWithValue("", phone.BrandId)
                cmd.Parameters.AddWithValue("", now)
                cmd.Parameters.AddWithValue("", phone.Id)
                cmd.ExecuteNonQuery()
            End Using
        End Using
        Return phone
    End Function

    Public Sub Delete(id As Integer) Implements IPhoneRepository.Delete
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "DELETE FROM phones WHERE id = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("id", id)
                cmd.ExecuteNonQuery()
            End Using
        End Using
    End Sub

    Public Function GetPaged(query As BaseQuery) As PagedResult(Of Phone) Implements IPhoneRepository.GetPaged
        Dim result As New List(Of Phone)()
        Dim totalRecords As Integer = 0
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            ' Đếm tổng số bản ghi
            Dim countQuery As String = "SELECT COUNT(*) FROM phones"
            If Not String.IsNullOrEmpty(query.Keyword) Then
                countQuery &= " WHERE LOWER(model) LIKE ?"
            End If
            Using cmd As New OdbcCommand(countQuery, conn)
                If Not String.IsNullOrEmpty(query.Keyword) Then
                    cmd.Parameters.AddWithValue("keyword", "%" & query.Keyword.ToLower() & "%")
                End If
                totalRecords = Convert.ToInt32(cmd.ExecuteScalar())
            End Using

            ' Lấy dữ liệu phân trang
            Dim dataQuery As String = "SELECT * FROM phones"
            If Not String.IsNullOrEmpty(query.Keyword) Then
                dataQuery &= " WHERE LOWER(model) LIKE ?"
            End If
            dataQuery &= " ORDER BY price DESC LIMIT ? OFFSET ?"
            Using cmd As New OdbcCommand(dataQuery, conn)
                If Not String.IsNullOrEmpty(query.Keyword) Then
                    cmd.Parameters.AddWithValue("keyword", "%" & query.Keyword.ToLower() & "%")
                End If
                cmd.Parameters.AddWithValue("take", query.Take)
                cmd.Parameters.AddWithValue("skip", query.Skip)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        result.Add(MapToPhone(reader))
                    End While
                End Using
            End Using
        End Using
        Return New PagedResult(Of Phone)(result, totalRecords)
    End Function

    Private Function MapToPhone(reader As OdbcDataReader) As Phone
        Dim phone As New Phone()
        phone.Id = reader.GetInt32(reader.GetOrdinal("id"))
        phone.Model = reader.GetString(reader.GetOrdinal("model"))
        phone.Price = reader.GetDecimal(reader.GetOrdinal("price"))
        phone.Stock = reader.GetInt32(reader.GetOrdinal("stock"))
        phone.BrandId = reader.GetInt32(reader.GetOrdinal("brand_id"))
        phone.CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
        phone.LastModified = reader.GetDateTime(reader.GetOrdinal("last_modified"))
        Return phone
    End Function
End Class