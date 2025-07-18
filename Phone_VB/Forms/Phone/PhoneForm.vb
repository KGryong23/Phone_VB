Public Class PhoneForm
    Private currentPage As Integer = 1
    Private pageSize As Integer = 5
    Private totalRecords As Integer = 0

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Khởi tạo cboQuantity
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(5, "5 bản ghi"))
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(10, "10 bản ghi"))
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(15, "15 bản ghi"))
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(25, "25 bản ghi"))

            cboQuantity.DisplayMember = "Value"
            cboQuantity.ValueMember = "Key"
            cboQuantity.SelectedIndex = 0

            'If Not CurrentUser.IsAdmin Then
            '    btnCreate.Visible = False
            '    btnUpdate.Visible = False
            '    btnDelete.Visible = False
            'End If

            ' Tải danh sách điện thoại
            LoadPhones()
        Catch ex As Exception
            MessageBox.Show("Failed to load data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If dgvPhones.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a phone to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            Dim selectedRow As DataGridViewRow = dgvPhones.SelectedRows(0)
            Dim phoneDto As PhoneDto = TryCast(selectedRow.DataBoundItem, PhoneDto)
            If phoneDto Is Nothing Then
                Throw New Exception("Invalid phone selected")
            End If
            If MessageBox.Show("Are you sure you want to delete this phone?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                ServiceRegistry.PhoneService.Delete(phoneDto.Id)
                MessageBox.Show("Phone deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadPhones()
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to delete phone: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDetail_Click(sender As Object, e As EventArgs) Handles btnDetail.Click
        Try
            If dgvPhones.SelectedRows.Count = 0 Then
                MessageBox.Show("Vui lòng chọn điện thoại để xem chi tiết.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            Dim selectedRow As DataGridViewRow = dgvPhones.SelectedRows(0)
            Dim phoneDto As PhoneDto = TryCast(selectedRow.DataBoundItem, PhoneDto)
            If phoneDto Is Nothing Then
                Throw New Exception("Không hợp lệ")
            End If

            Dim detailForm As New PhoneDetailForm(phoneDto)
            detailForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Không thể xem chi tiết: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            currentPage = 1 ' Reset về trang đầu khi tìm kiếm
            LoadPhones()
        Catch ex As Exception
            MessageBox.Show("Failed to search phones: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvPhones_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPhones.SelectionChanged
        Try
            btnUpdate.Enabled = dgvPhones.SelectedRows.Count > 0
            btnDelete.Enabled = dgvPhones.SelectedRows.Count > 0
            btnDetail.Enabled = dgvPhones.SelectedRows.Count > 0
        Catch ex As Exception
            MessageBox.Show("Failed to handle selection: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadPhones()
        Try
            Dim query As New BaseQuery(txtKeyword.Text, (currentPage - 1) * pageSize, pageSize)
            Dim result As PagedResult(Of PhoneDto) = ServiceRegistry.PhoneService.GetPaged(query)
            totalRecords = result.TotalRecords

            ' Thêm cột STT nếu chưa có
            If Not dgvPhones.Columns.Contains("STT") Then
                Dim sttColumn As New DataGridViewTextBoxColumn()
                sttColumn.Name = "STT"
                sttColumn.HeaderText = "STT"
                sttColumn.Width = 30
                sttColumn.ReadOnly = True
                dgvPhones.Columns.Insert(0, sttColumn)
            End If

            dgvPhones.DataSource = result.Data

            ' Cập nhật giá trị STT cho từng dòng
            For i As Integer = 0 To dgvPhones.Rows.Count - 1
                dgvPhones.Rows(i).Cells("STT").Value = (currentPage - 1) * pageSize + i + 1
            Next

            If dgvPhones.Columns.Contains("Id") Then
                dgvPhones.Columns("Id").Visible = False
            End If
            If dgvPhones.Columns.Contains("CreatedAt") Then
                dgvPhones.Columns("CreatedAt").Visible = False
            End If
            If dgvPhones.Columns.Contains("LastModified") Then
                dgvPhones.Columns("LastModified").Visible = False
            End If

            If dgvPhones.Columns.Contains("Model") Then
                dgvPhones.Columns("Model").HeaderText = "Mẫu điện thoại"
            End If
            If dgvPhones.Columns.Contains("Price") Then
                dgvPhones.Columns("Price").HeaderText = "Giá"
            End If
            If dgvPhones.Columns.Contains("Stock") Then
                dgvPhones.Columns("Stock").HeaderText = "Tồn kho"
            End If
            If dgvPhones.Columns.Contains("BrandName") Then
                dgvPhones.Columns("BrandName").HeaderText = "Hãng"
            End If
            dgvPhones.DefaultCellStyle.Font = New Font("Segoe UI", 9)
            dgvPhones.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10)
            Dim totalPages As Integer = CInt(Math.Ceiling(totalRecords / pageSize))
            lblPageInfo.Text = String.Format("Trang {0} trên {1} ({2} tổng số bản ghi)", currentPage, If(totalPages = 0, 1, totalPages), totalRecords)
            btnPrevPage.Enabled = currentPage > 1
            btnNextPage.Enabled = currentPage < totalPages
        Catch ex As Exception
            MessageBox.Show("Failed to load phones: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub cboQuantity_SelectedChanged(sender As Object, e As EventArgs) Handles cboQuantity.SelectedValueChanged
        Try
            Dim quantity As Integer = 5 ' Mặc định

            Dim selectedItem = DirectCast(cboQuantity.SelectedItem, KeyValuePair(Of Integer, String)?)
            If selectedItem.HasValue Then
                quantity = selectedItem.Value.Key
            End If
            If quantity >= 0 Then
                pageSize = Convert.ToInt32(quantity)
                currentPage = 1 ' Reset về trang đầu khi thay đổi pageSize
                LoadPhones()
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to change page size: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub


    Private Sub btnPrevPage_Click(sender As Object, e As EventArgs) Handles btnPrevPage.Click
        Try
            If currentPage > 1 Then
                currentPage -= 1
                LoadPhones()
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to load previous page: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
        Try
            Dim totalPages As Integer = CInt(Math.Ceiling(totalRecords / pageSize))
            If currentPage < totalPages Then
                currentPage += 1
                LoadPhones()
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to load next page: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        Try
            Dim inputForm As New PhoneInputForm()
            If inputForm.ShowDialog() = DialogResult.OK Then
                LoadPhones()
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to open create form: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            If dgvPhones.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a phone to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            Dim selectedRow As DataGridViewRow = dgvPhones.SelectedRows(0)
            Dim phoneDto As PhoneDto = TryCast(selectedRow.DataBoundItem, PhoneDto)
            If phoneDto Is Nothing Then
                Throw New Exception("Invalid phone selected")
            End If
            Dim inputForm As New PhoneInputForm(phoneDto)
            If inputForm.ShowDialog() = DialogResult.OK Then
                LoadPhones()
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to update phone: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnImport_Click(sender As Object, e As EventArgs) Handles btnImport.Click
        If dgvPhones.SelectedRows.Count = 0 Then
            MessageBox.Show("Vui lòng chọn điện thoại để nhập kho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        Dim selectedRow As DataGridViewRow = dgvPhones.SelectedRows(0)
        Dim phoneDto As PhoneDto = TryCast(selectedRow.DataBoundItem, PhoneDto)
        If phoneDto Is Nothing Then
            MessageBox.Show("Không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim currentUserId As Integer = CurrentUser.UserId
        Dim stockForm As New StockInOutForm("import", phoneDto, currentUserId)
        stockForm.ShowDialog()
    End Sub

    Private Sub btnExport_Click(sender As Object, e As EventArgs) Handles btnExport.Click
        If dgvPhones.SelectedRows.Count = 0 Then
            MessageBox.Show("Vui lòng chọn điện thoại để xuất kho.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If
        Dim selectedRow As DataGridViewRow = dgvPhones.SelectedRows(0)
        Dim phoneDto As PhoneDto = TryCast(selectedRow.DataBoundItem, PhoneDto)
        If phoneDto Is Nothing Then
            MessageBox.Show("Không hợp lệ", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim currentUserId As Integer = CurrentUser.UserId ' Giả sử có biến CurrentUser toàn cục
        Dim stockForm As New StockInOutForm("export", phoneDto, currentUserId)
        stockForm.ShowDialog()
    End Sub
End Class