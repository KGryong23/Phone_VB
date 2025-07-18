Imports System.Configuration
Imports System.Data.Odbc

Public Class StockTransactionRepository
    Implements IStockTransactionRepository

    Private ReadOnly connStr As String = ConfigurationManager.ConnectionStrings("PhoneDbConnection").ConnectionString

    Public Function GetPaged(query As BaseQuery) As PagedResult(Of StockTransaction) Implements IStockTransactionRepository.GetPaged
        Dim result As New List(Of StockTransaction)()
        Dim totalRecords As Integer = 0
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            ' Đếm tổng số bản ghi
            Dim countQuery As String = "SELECT COUNT(*) FROM stock_transactions"
            If Not String.IsNullOrEmpty(query.Keyword) Then
                countQuery &= " WHERE note LIKE ?"
            End If
            Using cmd As New OdbcCommand(countQuery, conn)
                If Not String.IsNullOrEmpty(query.Keyword) Then
                    cmd.Parameters.AddWithValue("keyword", "%" & query.Keyword & "%")
                End If
                totalRecords = Convert.ToInt32(cmd.ExecuteScalar())
            End Using

            ' Lấy dữ liệu phân trang
            Dim dataQuery As String = "SELECT * FROM stock_transactions"
            If Not String.IsNullOrEmpty(query.Keyword) Then
                dataQuery &= " WHERE note LIKE ?"
            End If
            dataQuery &= " ORDER BY created_at DESC LIMIT ? OFFSET ?"
            Using cmd As New OdbcCommand(dataQuery, conn)
                If Not String.IsNullOrEmpty(query.Keyword) Then
                    cmd.Parameters.AddWithValue("keyword", "%" & query.Keyword & "%")
                End If
                cmd.Parameters.AddWithValue("take", query.Take)
                cmd.Parameters.AddWithValue("skip", query.Skip)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    While reader.Read()
                        result.Add(MapToStockTransaction(reader))
                    End While
                End Using
            End Using
        End Using
        Return New PagedResult(Of StockTransaction)(result, totalRecords)
    End Function

    Public Function ImportStock(request As StockTransaction) As Boolean Implements IStockTransactionRepository.ImportStock
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "INSERT INTO stock_transactions (phone_id, user_id, quantity, transaction_type, status, note, created_at) VALUES (?, ?, ?, 'import', 'pending', ?, NOW())"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("phone_id", request.PhoneId)
                cmd.Parameters.AddWithValue("user_id", request.UserId)
                cmd.Parameters.AddWithValue("quantity", request.Quantity)
                cmd.Parameters.AddWithValue("note", request.Note)
                Return cmd.ExecuteNonQuery() > 0
            End Using
        End Using
    End Function

    Public Function ExportStock(request As StockTransaction) As Boolean Implements IStockTransactionRepository.ExportStock
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "INSERT INTO stock_transactions (phone_id, user_id, quantity, transaction_type, status, note, created_at) VALUES (?, ?, ?, 'export', 'pending', ?, NOW())"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("phone_id", request.PhoneId)
                cmd.Parameters.AddWithValue("user_id", request.UserId)
                cmd.Parameters.AddWithValue("quantity", request.Quantity)
                cmd.Parameters.AddWithValue("note", request.Note)
                Return cmd.ExecuteNonQuery() > 0
            End Using
        End Using
    End Function

    Private Function GetPhoneById(conn As OdbcConnection, trans As OdbcTransaction, phoneId As Integer) As Integer
        Dim query As String = "SELECT stock FROM phones WHERE id = ? FOR UPDATE"
        Using cmd As New OdbcCommand(query, conn, trans)
            cmd.Parameters.AddWithValue("id", phoneId)
            Using reader As OdbcDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    Return reader.GetInt32(0)
                Else
                    Throw New Exception("Không tìm thấy sản phẩm.")
                End If
            End Using
        End Using
    End Function

    Public Function ApproveRequest(id As Integer, approvedBy As Integer) As Boolean Implements IStockTransactionRepository.ApproveRequest
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim trans As OdbcTransaction = conn.BeginTransaction()
            Try
                ' Lấy thông tin giao dịch
                Dim transaction As StockTransaction = GetByIdInternal(conn, trans, id)
                If transaction Is Nothing Then
                    Throw New Exception("Không tìm thấy giao dịch kho.")
                End If

                ' Lấy tồn kho hiện tại
                Dim currentStock As Integer = GetPhoneById(conn, trans, transaction.PhoneId)

                ' Kiểm tra và cập nhật tồn kho
                Dim newStock As Integer
                If transaction.TransactionType = "import" Then
                    newStock = currentStock + transaction.Quantity
                ElseIf transaction.TransactionType = "export" Then
                    If currentStock < transaction.Quantity Then
                        Throw New Exception("Tồn kho không đủ để xuất.")
                    End If
                    newStock = currentStock - transaction.Quantity
                Else
                    Throw New Exception("Loại giao dịch không hợp lệ.")
                End If

                ' Cập nhật tồn kho sản phẩm
                Dim updateStockQuery As String = "UPDATE phones SET stock = ? WHERE id = ?"
                Using cmd As New OdbcCommand(updateStockQuery, conn, trans)
                    cmd.Parameters.AddWithValue("stock", newStock)
                    cmd.Parameters.AddWithValue("id", transaction.PhoneId)
                    cmd.ExecuteNonQuery()
                End Using

                ' Cập nhật trạng thái giao dịch
                Dim updateTxQuery As String = "UPDATE stock_transactions SET status = 'approved', approved_by = ?, approved_at = NOW() WHERE id = ?"
                Using cmd As New OdbcCommand(updateTxQuery, conn, trans)
                    cmd.Parameters.AddWithValue("approved_by", approvedBy)
                    cmd.Parameters.AddWithValue("id", id)
                    cmd.ExecuteNonQuery()
                End Using

                trans.Commit()
                Return True
            Catch ex As Exception
                trans.Rollback()
                Throw New Exception("Lỗi khi phê duyệt giao dịch kho: " & ex.Message)
            End Try
        End Using
    End Function

    ' Hàm lấy giao dịch sử dụng kết nối và transaction (nội bộ)
    Private Function GetByIdInternal(conn As OdbcConnection, trans As OdbcTransaction, id As Integer) As StockTransaction
        Dim query As String = "SELECT * FROM stock_transactions WHERE id = ? FOR UPDATE"
        Using cmd As New OdbcCommand(query, conn, trans)
            cmd.Parameters.AddWithValue("id", id)
            Using reader As OdbcDataReader = cmd.ExecuteReader()
                If reader.Read() Then
                    Return MapToStockTransaction(reader)
                End If
            End Using
        End Using
        Return Nothing
    End Function
    Public Function GetById(id As Integer) As StockTransaction Implements IStockTransactionRepository.GetById
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "SELECT * FROM stock_transactions WHERE id = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("id", id)
                Using reader As OdbcDataReader = cmd.ExecuteReader()
                    If reader.Read() Then
                        Return MapToStockTransaction(reader)
                    End If
                End Using
            End Using
        End Using
        Return Nothing
    End Function
    Public Function DeleteRequest(id As Integer) As Boolean Implements IStockTransactionRepository.DeleteRequest
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            Dim query As String = "DELETE FROM stock_transactions WHERE id = ?"
            Using cmd As New OdbcCommand(query, conn)
                cmd.Parameters.AddWithValue("id", id)
                Return cmd.ExecuteNonQuery() > 0
            End Using
        End Using
    End Function
    Private Function MapToStockTransaction(reader As OdbcDataReader) As StockTransaction
        Dim tx As New StockTransaction()
        tx.Id = reader.GetInt32(reader.GetOrdinal("id"))
        tx.PhoneId = reader.GetInt32(reader.GetOrdinal("phone_id"))
        tx.UserId = reader.GetInt32(reader.GetOrdinal("user_id"))
        If Not reader.IsDBNull(reader.GetOrdinal("approved_by")) Then
            tx.ApprovedBy = reader.GetInt32(reader.GetOrdinal("approved_by"))
        End If
        tx.Quantity = reader.GetInt32(reader.GetOrdinal("quantity"))
        tx.TransactionType = reader.GetString(reader.GetOrdinal("transaction_type"))
        tx.Status = reader.GetString(reader.GetOrdinal("status"))
        tx.Note = If(reader.IsDBNull(reader.GetOrdinal("note")), "", reader.GetString(reader.GetOrdinal("note")))
        tx.CreatedAt = reader.GetDateTime(reader.GetOrdinal("created_at"))
        If Not reader.IsDBNull(reader.GetOrdinal("approved_at")) Then
            tx.ApprovedAt = reader.GetDateTime(reader.GetOrdinal("approved_at"))
        End If
        Return tx
    End Function
End Class
