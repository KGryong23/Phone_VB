Public Class StockTransForm
    Private currentPage As Integer = 1
    Private pageSize As Integer = 5
    Private totalRecords As Integer = 0

    Private Sub StockTransForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(5, "5 bản ghi"))
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(10, "10 bản ghi"))
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(15, "15 bản ghi"))
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(25, "25 bản ghi"))

            cboQuantity.DisplayMember = "Value"
            cboQuantity.ValueMember = "Key"
            cboQuantity.SelectedIndex = 0

            LoadStockTransactions()
        Catch ex As Exception
            MessageBox.Show("Không thể tải dữ liệu: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadStockTransactions()
        Dim worker As New ComponentModel.BackgroundWorker()
        AddHandler worker.DoWork, Sub(sender, e)
                                      Dim query As New BaseQuery(txtKeyword.Text, (currentPage - 1) * pageSize, pageSize)
                                      e.Result = ServiceRegistry.StockTransactionService.GetPaged(query)
                                  End Sub
        AddHandler worker.RunWorkerCompleted, Sub(sender, e)
                                                  If e.Error IsNot Nothing Then
                                                      MessageBox.Show("Không thể tải giao dịch kho: " & e.Error.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                                  Else
                                                      Dim result As PagedResult(Of StockTransactionDto) = CType(e.Result, PagedResult(Of StockTransactionDto))
                                                      totalRecords = result.TotalRecords

                                                      If Not dgvStockTrans.Columns.Contains("STT") Then
                                                          Dim sttColumn As New DataGridViewTextBoxColumn()
                                                          sttColumn.Name = "STT"
                                                          sttColumn.HeaderText = "STT"
                                                          sttColumn.Width = 30
                                                          sttColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None
                                                          dgvStockTrans.Columns.Insert(0, sttColumn)
                                                      End If

                                                      dgvStockTrans.DataSource = result.Data

                                                      For i As Integer = 0 To dgvStockTrans.Rows.Count - 1
                                                          dgvStockTrans.Rows(i).Cells("STT").Value = (currentPage - 1) * pageSize + i + 1
                                                      Next

                                                      If dgvStockTrans.Columns.Contains("Id") Then
                                                          dgvStockTrans.Columns("Id").Visible = False
                                                      End If

                                                      Dim totalPages As Integer = CInt(Math.Ceiling(totalRecords / pageSize))
                                                      lblPageInfo.Text = String.Format("Trang {0} trên {1} ({2} tổng số bản ghi)", currentPage, If(totalPages = 0, 1, totalPages), totalRecords)
                                                      btnPrevPage.Enabled = currentPage > 1
                                                      btnNextPage.Enabled = currentPage < totalPages

                                                      For Each col As DataGridViewColumn In dgvStockTrans.Columns
                                                          Select Case col.Name
                                                              Case "STT", "PhoneName", "UserName", "Quantity", "TransactionType", "Status"
                                                                  col.Visible = True
                                                              Case Else
                                                                  col.Visible = False
                                                          End Select
                                                      Next

                                                      If dgvStockTrans.Columns.Contains("PhoneName") Then
                                                          dgvStockTrans.Columns("PhoneName").HeaderText = "Tên sản phẩm"
                                                      End If
                                                      If dgvStockTrans.Columns.Contains("UserName") Then
                                                          dgvStockTrans.Columns("UserName").HeaderText = "Người yêu cầu"
                                                      End If
                                                      If dgvStockTrans.Columns.Contains("Quantity") Then
                                                          dgvStockTrans.Columns("Quantity").HeaderText = "Số lượng"
                                                      End If
                                                      If dgvStockTrans.Columns.Contains("TransactionType") Then
                                                          dgvStockTrans.Columns("TransactionType").HeaderText = "Kiểu"
                                                      End If
                                                      If dgvStockTrans.Columns.Contains("Status") Then
                                                          dgvStockTrans.Columns("Status").HeaderText = "Trạng thái"
                                                      End If
                                                  End If
                                              End Sub
        worker.RunWorkerAsync()
    End Sub

    Private Sub cboQuantity_SelectedChanged(sender As Object, e As EventArgs) Handles cboQuantity.SelectedValueChanged
        Try
            Dim quantity As Integer = 5
            Dim selectedItem = DirectCast(cboQuantity.SelectedItem, KeyValuePair(Of Integer, String)?)
            If selectedItem.HasValue Then
                quantity = selectedItem.Value.Key
            End If
            If quantity >= 0 Then
                pageSize = Convert.ToInt32(quantity)
                currentPage = 1
                LoadStockTransactions()
            End If
        Catch ex As Exception
            MessageBox.Show("Không thể thay đổi số lượng trang: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPrevPage_Click(sender As Object, e As EventArgs) Handles btnPrevPage.Click
        Try
            If currentPage > 1 Then
                currentPage -= 1
                LoadStockTransactions()
            End If
        Catch ex As Exception
            MessageBox.Show("Không thể chuyển trang trước: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
        Try
            Dim totalPages As Integer = CInt(Math.Ceiling(totalRecords / pageSize))
            If currentPage < totalPages Then
                currentPage += 1
                LoadStockTransactions()
            End If
        Catch ex As Exception
            MessageBox.Show("Không thể chuyển trang sau: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If dgvStockTrans.SelectedRows.Count = 0 Then
                MessageBox.Show("Vui lòng chọn giao dịch để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            Dim selectedRow As DataGridViewRow = dgvStockTrans.SelectedRows(0)
            Dim txDto = TryCast(selectedRow.DataBoundItem, StockTransactionDto)
            If txDto Is Nothing Then
                Throw New Exception("Giao dịch không hợp lệ")
            End If
            If MessageBox.Show("Bạn có chắc muốn xóa giao dịch này?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                ServiceRegistry.StockTransactionService.DeleteRequest(txDto.Id)
                MessageBox.Show("Xóa thành công.", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadStockTransactions()
            End If
        Catch ex As Exception
            MessageBox.Show("Không thể xóa giao dịch: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDetail_Click(sender As Object, e As EventArgs) Handles btnDetail.Click
        Try
            If dgvStockTrans.SelectedRows.Count = 0 Then
                MessageBox.Show("Vui lòng chọn giao dịch để xem chi tiết.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            Dim selectedRow As DataGridViewRow = dgvStockTrans.SelectedRows(0)
            Dim txDto = TryCast(selectedRow.DataBoundItem, StockTransactionDto)
            If txDto Is Nothing Then
                Throw New Exception("Giao dịch không hợp lệ")
            End If

            Dim detailForm As New StockTransDetailForm(txDto)
            detailForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Không thể xem chi tiết: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            currentPage = 1
            LoadStockTransactions()
        Catch ex As Exception
            MessageBox.Show("Không thể tìm kiếm: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvStockTrans_SelectionChanged(sender As Object, e As EventArgs) Handles dgvStockTrans.SelectionChanged
        Try
            btnDelete.Enabled = dgvStockTrans.SelectedRows.Count > 0
            btnDetail.Enabled = dgvStockTrans.SelectedRows.Count > 0

            ' Disable btnApprove if the selected transaction is already approved
            If dgvStockTrans.SelectedRows.Count > 0 Then
                Dim selectedRow As DataGridViewRow = dgvStockTrans.SelectedRows(0)
                Dim txDto = TryCast(selectedRow.DataBoundItem, StockTransactionDto)
                If txDto IsNot Nothing AndAlso (txDto.Status = "Đã duyệt" OrElse txDto.Status.ToLower = "approved") Then
                    btnApprove.Enabled = False
                Else
                    btnApprove.Enabled = True
                End If
            Else
                btnApprove.Enabled = False
            End If
        Catch ex As Exception
            MessageBox.Show("Không thể xử lý chọn dòng: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnApprove_Click(sender As Object, e As EventArgs) Handles btnApprove.Click
        Try
            If dgvStockTrans.SelectedRows.Count = 0 Then
                MessageBox.Show("Vui lòng chọn giao dịch để phê duyệt.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim selectedRow As DataGridViewRow = dgvStockTrans.SelectedRows(0)
            Dim txDto = TryCast(selectedRow.DataBoundItem, StockTransactionDto)
            If txDto Is Nothing Then
                MessageBox.Show("Giao dịch không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim currentUserId As Integer = CurrentUser.UserId ' Thay bằng cách lấy user hiện tại của bạn

            If txDto.Status = "Đã duyệt" OrElse txDto.Status.ToLower = "approved" Then
                MessageBox.Show("Giao dịch đã được duyệt trước đó.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            If ServiceRegistry.StockTransactionService.ApproveRequest(txDto.Id, currentUserId) Then
                MessageBox.Show("Phê duyệt thành công!", "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadStockTransactions()
            Else
                MessageBox.Show("Phê duyệt thất bại.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        Catch ex As Exception
            MessageBox.Show("Lỗi khi phê duyệt: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class