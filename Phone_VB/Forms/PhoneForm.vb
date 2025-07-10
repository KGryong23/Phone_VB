Public Class PhoneForm
    Private phoneService As IPhoneService = New PhoneService(New PhoneRepository(), New BrandRepository())
    Private brandService As IBrandService = New BrandService(New BrandRepository())
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

            If Not CurrentUser.IsAdmin Then
                btnCreate.Visible = False
                btnUpdate.Visible = False
                btnDelete.Visible = False
            End If

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
                phoneService.Delete(phoneDto.Id)
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
                MessageBox.Show("Please select a phone to view details.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            Dim selectedRow As DataGridViewRow = dgvPhones.SelectedRows(0)
            Dim phoneDto As PhoneDto = TryCast(selectedRow.DataBoundItem, PhoneDto)
            If phoneDto Is Nothing Then
                Throw New Exception("Invalid phone selected")
            End If
            Dim details As String = String.Format("ID: {0}{1}Model: {2}{1}Price: {3}{1}Stock: {4}{1}Brand: {5}{1}Created At: {6}{1}Last Modified: {7}",
                                                 phoneDto.Id, vbCrLf, phoneDto.Model, phoneDto.Price, phoneDto.Stock, phoneDto.BrandName,
                                                 phoneDto.CreatedAt, phoneDto.LastModified)
            MessageBox.Show(details, "Phone Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
        Catch ex As Exception
            MessageBox.Show("Failed to view phone details: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
        Dim worker As New ComponentModel.BackgroundWorker()
        AddHandler worker.DoWork, Sub(sender, e)
                                      Dim query As New BaseQuery(txtKeyword.Text, (currentPage - 1) * pageSize, pageSize)
                                      e.Result = phoneService.GetPaged(query)
                                  End Sub
        AddHandler worker.RunWorkerCompleted, Sub(sender, e)
                                                  If e.Error IsNot Nothing Then
                                                      MessageBox.Show("Failed to load phones: " & e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                                                  Else
                                                      Dim result As PagedResult(Of PhoneDto) = CType(e.Result, PagedResult(Of PhoneDto))
                                                      totalRecords = result.TotalRecords
                                                      dgvPhones.DataSource = result.Data
                                                      Dim totalPages As Integer = CInt(Math.Ceiling(totalRecords / pageSize))
                                                      lblPageInfo.Text = String.Format("Trang {0} trên {1} ({2} tổng số bản ghi)", currentPage, If(totalPages = 0, 1, totalPages), totalRecords)
                                                      btnPrevPage.Enabled = currentPage > 1
                                                      btnNextPage.Enabled = currentPage < totalPages
                                                  End If
                                              End Sub
        worker.RunWorkerAsync()
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
            Dim inputForm As New PhoneInputForm(phoneService, brandService)
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
            Dim inputForm As New PhoneInputForm(phoneService, brandService, phoneDto)
            If inputForm.ShowDialog() = DialogResult.OK Then
                LoadPhones()
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to update phone: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class