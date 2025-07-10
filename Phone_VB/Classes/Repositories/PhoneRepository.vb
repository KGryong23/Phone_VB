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
            Dim escapedModel As String = phone.Model.Replace("'", "''")
            Dim query As String = String.Format("CALL upsert_phone(0, '{0}', {1}, {2}, {3})",
                    escapedModel, phone.Price.ToString("F2", Globalization.CultureInfo.InvariantCulture),
                    phone.Stock, phone.BrandId)
            Using cmd As New OdbcCommand(query, conn)
                Debug.WriteLine("Add: Query=" & query)
                cmd.ExecuteNonQuery()

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
            Dim escapedModel As String = phone.Model.Replace("'", "''")
            Dim query As String = String.Format("CALL upsert_phone({0}, '{1}', {2}, {3}, {4})",
                phone.Id, escapedModel,
                phone.Price.ToString("F2", Globalization.CultureInfo.InvariantCulture),
                phone.Stock, phone.BrandId)
            Using cmd As New OdbcCommand(query, conn)
                Debug.WriteLine("Update: Query=" & query)
                Dim rowsAffected As Integer = cmd.ExecuteNonQuery()
                If rowsAffected = 0 Then
                    Throw New Exception("No phone found with ID " & phone.Id)
                End If
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