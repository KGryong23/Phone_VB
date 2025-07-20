Public Class RoleForm
    Private currentPage As Integer = 1
    Private pageSize As Integer = 5
    Private totalRecords As Integer = 0

    Private Sub RoleForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(5, "5 bản ghi"))
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(10, "10 bản ghi"))
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(15, "15 bản ghi"))
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(25, "25 bản ghi"))

            cboQuantity.DisplayMember = "Value"
            cboQuantity.ValueMember = "Key"
            cboQuantity.SelectedIndex = 0

            LoadRoles()
        Catch ex As Exception
            MessageBox.Show("Failed to load data: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        Try
            Dim inputForm As New RoleInputForm()
            If inputForm.ShowDialog() = DialogResult.OK Then
                LoadRoles()
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to open create form: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If dgvRoles.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a role to delete.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            Dim selectedRow As DataGridViewRow = dgvRoles.SelectedRows(0)
            Dim roleDto As RoleDto = TryCast(selectedRow.DataBoundItem, RoleDto)
            If roleDto Is Nothing Then
                Throw New Exception("Invalid role selected")
            End If
            If MessageBox.Show("Are you sure you want to delete this role?", "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                ServiceRegistry.RoleService.Delete(roleDto.Id)
                MessageBox.Show("Role deleted successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadRoles()
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to delete role: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            If dgvRoles.SelectedRows.Count = 0 Then
                MessageBox.Show("Please select a role to update.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            Dim selectedRow As DataGridViewRow = dgvRoles.SelectedRows(0)
            Dim roleDto As RoleDto = TryCast(selectedRow.DataBoundItem, RoleDto)
            If roleDto Is Nothing Then
                Throw New Exception("Invalid role selected")
            End If
            Dim inputForm As New RoleInputForm(roleDto)
            If inputForm.ShowDialog() = DialogResult.OK Then
                LoadRoles()
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to update role: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            currentPage = 1
            LoadRoles()
        Catch ex As Exception
            MessageBox.Show("Failed to search roles: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPrevPage_Click(sender As Object, e As EventArgs) Handles btnPrevPage.Click
        Try
            If currentPage > 1 Then
                currentPage -= 1
                LoadRoles()
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
                LoadRoles()
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to load next page: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
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
                LoadRoles()
            End If
        Catch ex As Exception
            MessageBox.Show("Failed to change page size: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub dgvRoles_SelectionChanged(sender As Object, e As EventArgs) Handles dgvRoles.SelectionChanged
        Try
            btnUpdate.Enabled = dgvRoles.SelectedRows.Count > 0
            btnDelete.Enabled = dgvRoles.SelectedRows.Count > 0
        Catch ex As Exception
            MessageBox.Show("Failed to handle selection: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadRoles()
        Try
            If Not BackgroundWorker1 Is Nothing AndAlso BackgroundWorker1.IsBusy Then
                ' Prevent multiple concurrent loads
                Return
            End If

            If BackgroundWorker1 Is Nothing Then
                BackgroundWorker1 = New ComponentModel.BackgroundWorker()
                AddHandler BackgroundWorker1.DoWork, AddressOf LoadRoles_DoWork
                AddHandler BackgroundWorker1.RunWorkerCompleted, AddressOf LoadRoles_RunWorkerCompleted
            End If

            btnPrevPage.Enabled = False
            btnNextPage.Enabled = False
            dgvRoles.Enabled = False
            lblPageInfo.Text = "Đang tải dữ liệu..."

            Dim query As New BaseQuery(txtKeyword.Text, (currentPage - 1) * pageSize, pageSize)
            BackgroundWorker1.RunWorkerAsync(query)
        Catch ex As Exception
            MessageBox.Show("Failed to load roles: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private BackgroundWorker1 As System.ComponentModel.BackgroundWorker

    Private Sub LoadRoles_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs)
        Dim query As BaseQuery = DirectCast(e.Argument, BaseQuery)
        Dim result As PagedResult(Of RoleDto) = Nothing
        Try
            result = ServiceRegistry.RoleService.GetPaged(query)
        Catch ex As Exception
            e.Result = ex
            Return
        End Try
        e.Result = result
    End Sub

    Private Sub LoadRoles_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs)
        dgvRoles.Enabled = True
        If TypeOf e.Result Is Exception Then
            MessageBox.Show("Failed to load roles: " & DirectCast(e.Result, Exception).Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            lblPageInfo.Text = ""
            btnPrevPage.Enabled = currentPage > 1
            btnNextPage.Enabled = True
            Return
        End If

        Dim result As PagedResult(Of RoleDto) = DirectCast(e.Result, PagedResult(Of RoleDto))
        totalRecords = result.TotalRecords

        If Not dgvRoles.Columns.Contains("STT") Then
            Dim sttColumn As New DataGridViewTextBoxColumn()
            sttColumn.Name = "STT"
            sttColumn.HeaderText = "STT"
            sttColumn.Width = 30
            sttColumn.ReadOnly = True
            dgvRoles.Columns.Insert(0, sttColumn)
        End If

        dgvRoles.DataSource = result.Data

        For i As Integer = 0 To dgvRoles.Rows.Count - 1
            dgvRoles.Rows(i).Cells("STT").Value = (currentPage - 1) * pageSize + i + 1
        Next

        If dgvRoles.Columns.Contains("Id") Then
            dgvRoles.Columns("Id").Visible = False
        End If

        dgvRoles.DefaultCellStyle.Font = New Font("Segoe UI", 9)
        dgvRoles.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10)
        Dim totalPages As Integer = CInt(Math.Ceiling(totalRecords / pageSize))
        lblPageInfo.Text = String.Format("Trang {0} trên {1} ({2} tổng số bản ghi)", currentPage, If(totalPages = 0, 1, totalPages), totalRecords)
        btnPrevPage.Enabled = currentPage > 1
        btnNextPage.Enabled = currentPage < totalPages
    End Sub

    Private Sub btnPermission_Click(sender As Object, e As EventArgs) Handles btnPermission.Click
        Try
            If dgvRoles.SelectedRows.Count = 0 Then
                MessageBox.Show("Vui lòng chọn vai trò để xem quyền.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            Dim selectedRow As DataGridViewRow = dgvRoles.SelectedRows(0)
            Dim roleDto As RoleDto = TryCast(selectedRow.DataBoundItem, RoleDto)
            If roleDto Is Nothing Then
                MessageBox.Show("Vai trò không hợp lệ.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim permForm As New PermissionForm(roleDto.Id)
            permForm.ShowDialog()
        Catch ex As Exception
            MessageBox.Show("Không thể mở form quyền: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
End Class