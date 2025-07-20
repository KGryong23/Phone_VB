Imports System.ComponentModel

Public Class PermissionForm
    Private ReadOnly _roleId As Integer
    Private currentPage As Integer = 1
    Private pageSize As Integer = 5
    Private totalRecords As Integer = 0
    Private WithEvents bgwLoad As New BackgroundWorker()

    ' Constructor nhận roleId từ form gọi
    Public Sub New(roleId As Integer)
        InitializeComponent()
        _roleId = roleId
        bgwLoad.WorkerReportsProgress = False
        bgwLoad.WorkerSupportsCancellation = False
    End Sub

    Private Sub PermissionForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(5, "5 bản ghi"))
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(10, "10 bản ghi"))
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(15, "15 bản ghi"))
            cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(25, "25 bản ghi"))
            cboQuantity.DisplayMember = "Value"
            cboQuantity.ValueMember = "Key"
            cboQuantity.SelectedIndex = 0
            LoadPermissions()
        Catch ex As Exception
            MessageBox.Show("Không thể tải dữ liệu: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        Try
            ' Mở form chọn quyền chưa có trong role (dùng worker trong PermissionSelectForm)
            Dim selectForm As New PermissionSelectForm(_roleId)
            If selectForm.ShowDialog() = DialogResult.OK Then
                Dim selectedIds = selectForm.SelectedPermissionIds
                If selectedIds IsNot Nothing AndAlso selectedIds.Count > 0 Then
                    If ServiceRegistry.PermissionService.AddPermissionsToRole(_roleId, selectedIds) Then
                        MessageBox.Show("Thêm quyền thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        LoadPermissions()
                    Else
                        MessageBox.Show("Thêm quyền thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End If
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Không thể thêm quyền: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        Try
            If dgvPermissions.SelectedRows.Count = 0 Then
                MessageBox.Show("Vui lòng chọn quyền để xóa.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            Dim selectedRow As DataGridViewRow = dgvPermissions.SelectedRows(0)
            Dim permDto As PermissionDto = TryCast(selectedRow.DataBoundItem, PermissionDto)
            If permDto Is Nothing Then
                Throw New Exception("Quyền không hợp lệ")
            End If
            If MessageBox.Show("Bạn có chắc muốn xóa quyền này khỏi vai trò?", "Xác nhận xóa", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                ServiceRegistry.PermissionService.RemovePermissionFromRole(_roleId, permDto.Id)
                MessageBox.Show("Xóa quyền thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                LoadPermissions()
            End If
        Catch ex As Exception
            MessageBox.Show("Không thể xóa quyền: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        Try
            currentPage = 1
            LoadPermissions()
        Catch ex As Exception
            MessageBox.Show("Không thể tìm kiếm: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnPrevPage_Click(sender As Object, e As EventArgs) Handles btnPrevPage.Click
        Try
            If currentPage > 1 Then
                currentPage -= 1
                LoadPermissions()
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
                LoadPermissions()
            End If
        Catch ex As Exception
            MessageBox.Show("Không thể chuyển trang sau: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
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
                LoadPermissions()
            End If
        Catch ex As Exception
            MessageBox.Show("Không thể thay đổi số lượng trang: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub LoadPermissions()
        If Not bgwLoad.IsBusy Then
            btnSearch.Enabled = False
            btnPrevPage.Enabled = False
            btnNextPage.Enabled = False
            dgvPermissions.DataSource = Nothing
            bgwLoad.RunWorkerAsync(New BaseQuery(txtKeyword.Text, (currentPage - 1) * pageSize, pageSize))
        End If
    End Sub

    Private Sub bgwLoad_DoWork(sender As Object, e As DoWorkEventArgs) Handles bgwLoad.DoWork
        Dim query As BaseQuery = CType(e.Argument, BaseQuery)
        Dim result As PagedResult(Of PermissionDto) = ServiceRegistry.PermissionService.GetPagedByRole(_roleId, query)
        e.Result = result
    End Sub

    Private Sub bgwLoad_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwLoad.RunWorkerCompleted
        btnSearch.Enabled = True
        btnPrevPage.Enabled = True
        btnNextPage.Enabled = True

        If e.Error IsNot Nothing Then
            MessageBox.Show("Không thể tải danh sách quyền: " & e.Error.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim result As PagedResult(Of PermissionDto) = CType(e.Result, PagedResult(Of PermissionDto))
        totalRecords = result.TotalRecords
        dgvPermissions.DataSource = result.Data

        ' Thêm cột STT nếu chưa có
        If Not dgvPermissions.Columns.Contains("STT") Then
            Dim sttColumn As New DataGridViewTextBoxColumn()
            sttColumn.Name = "STT"
            sttColumn.HeaderText = "STT"
            sttColumn.Width = 40
            sttColumn.ReadOnly = True
            dgvPermissions.Columns.Insert(0, sttColumn)
        End If

        ' Gán giá trị STT cho từng dòng
        For i As Integer = 0 To dgvPermissions.Rows.Count - 1
            dgvPermissions.Rows(i).Cells("STT").Value = (currentPage - 1) * pageSize + i + 1
        Next

        If dgvPermissions.Columns.Contains("Id") Then
            dgvPermissions.Columns("Id").Visible = False
        End If

        dgvPermissions.DefaultCellStyle.Font = New Font("Segoe UI", 9)
        dgvPermissions.ColumnHeadersDefaultCellStyle.Font = New Font("Segoe UI", 10)
        Dim totalPages As Integer = CInt(Math.Ceiling(totalRecords / pageSize))
        lblPageInfo.Text = String.Format("Trang {0} trên {1} ({2} tổng số quyền)", currentPage, If(totalPages = 0, 1, totalPages), totalRecords)
        btnPrevPage.Enabled = currentPage > 1
        btnNextPage.Enabled = currentPage < totalPages
    End Sub
End Class