Public Class UserForm
    Private userService As IUserService
    Private roleService As IRoleService
    Private currentPage As Integer = 1
    Private pageSize As Integer = 10
    Private totalPages As Integer = 0
    Private searchKeyword As String = ""
    Private WithEvents bgwLoad As New System.ComponentModel.BackgroundWorker()

    Public Sub New()
        InitializeComponent()
        userService = ServiceRegistry.UserService
        roleService = ServiceRegistry.RoleService
    End Sub

    Private Sub UserForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitializeControls()
        LoadUsers()
    End Sub

    Private Sub InitializeControls()
        cboQuantity.Items.Clear()
        cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(5, "5 bản ghi"))
        cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(10, "10 bản ghi"))
        cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(20, "20 bản ghi"))
        cboQuantity.Items.Add(New KeyValuePair(Of Integer, String)(50, "50 bản ghi"))
        cboQuantity.DisplayMember = "Value"
        cboQuantity.ValueMember = "Key"
        cboQuantity.SelectedIndex = 1

        SetupDataGridView()
    End Sub

    Private Sub SetupDataGridView()
        dgvUsers.AutoGenerateColumns = False
        dgvUsers.Columns.Clear()

        dgvUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Username",
            .HeaderText = "Tên đăng nhập",
            .Width = 150
        })

        dgvUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "Password",
            .HeaderText = "Mật khẩu",
            .Width = 120
        })

        dgvUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "RoleName",
            .HeaderText = "Vai trò",
            .Width = 120
        })

        dgvUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "CreatedAt",
            .HeaderText = "Ngày tạo",
            .Width = 120,
            .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy HH:mm"}
        })

        dgvUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .DataPropertyName = "LastModified",
            .HeaderText = "Lần cập nhật cuối",
            .Width = 120,
            .DefaultCellStyle = New DataGridViewCellStyle With {.Format = "dd/MM/yyyy HH:mm"}
        })
    End Sub

    Private Sub LoadUsers()
        If Not bgwLoad.IsBusy Then
            btnSearch.Enabled = False
            btnPrevPage.Enabled = False
            btnNextPage.Enabled = False
            dgvUsers.DataSource = Nothing
            Dim skip = (currentPage - 1) * pageSize
            bgwLoad.RunWorkerAsync(New BaseQuery(searchKeyword, skip, pageSize))
        End If
    End Sub

    Private Sub bgwLoad_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwLoad.DoWork
        Dim query As BaseQuery = CType(e.Argument, BaseQuery)
        Dim result As PagedResult(Of UserDto) = userService.GetPaged(query)
        e.Result = result
    End Sub

    Private Sub bgwLoad_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwLoad.RunWorkerCompleted
        btnSearch.Enabled = True
        btnPrevPage.Enabled = True
        btnNextPage.Enabled = True

        If e.Error IsNot Nothing Then
            MessageBox.Show("Lỗi khi tải danh sách người dùng: " & e.Error.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End If

        Dim result As PagedResult(Of UserDto) = CType(e.Result, PagedResult(Of UserDto))
        totalPages = Math.Ceiling(result.TotalRecords / pageSize)

        If Not dgvUsers.Columns.Contains("STT") Then
            Dim sttColumn As New DataGridViewTextBoxColumn()
            sttColumn.Name = "STT"
            sttColumn.HeaderText = "STT"
            sttColumn.Width = 30
            sttColumn.ReadOnly = True
            dgvUsers.Columns.Insert(0, sttColumn)
        End If

        dgvUsers.DataSource = result.Data

        For i As Integer = 0 To dgvUsers.Rows.Count - 1
            dgvUsers.Rows(i).Cells("STT").Value = (currentPage - 1) * pageSize + i + 1
        Next

        UpdatePageInfo()
    End Sub

    Private Sub UpdatePageInfo()
        lblPageInfo.Text = String.Format("Trang {0} trên {1} ({2} tổng số bản ghi)", currentPage, If(totalPages = 0, 1, totalPages), If(dgvUsers.DataSource IsNot Nothing, CType(dgvUsers.DataSource, IList).Count, 0))
        btnPrevPage.Enabled = currentPage > 1
        btnNextPage.Enabled = currentPage < totalPages
    End Sub

    Private Sub btnCreate_Click(sender As Object, e As EventArgs) Handles btnCreate.Click
        Dim inputForm As New UserInputForm()
        If inputForm.ShowDialog() = DialogResult.OK Then
            LoadUsers()
        End If
    End Sub

    Private Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        If dgvUsers.SelectedRows.Count = 0 Then
            MessageBox.Show("Vui lòng chọn người dùng cần cập nhật.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim selectedUser = CType(dgvUsers.SelectedRows(0).DataBoundItem, UserDto)
        Dim inputForm As New UserInputForm(selectedUser)
        If inputForm.ShowDialog() = DialogResult.OK Then
            LoadUsers()
        End If
    End Sub

    Private Sub btnDelete_Click(sender As Object, e As EventArgs) Handles btnDelete.Click
        If dgvUsers.SelectedRows.Count = 0 Then
            MessageBox.Show("Vui lòng chọn người dùng cần xóa.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        Dim selectedUser = CType(dgvUsers.SelectedRows(0).DataBoundItem, UserDto)

        If MessageBox.Show("Bạn có chắc chắn muốn xóa người dùng '" + selectedUser.Username + "'?", "Xác nhận", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
            Try
                userService.Delete(selectedUser.Id)
                LoadUsers()
                MessageBox.Show("Xóa người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Catch ex As Exception
                MessageBox.Show("Lỗi khi xóa người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End If
    End Sub

    Private Sub btnSearch_Click(sender As Object, e As EventArgs) Handles btnSearch.Click
        searchKeyword = txtKeyword.Text.Trim()
        currentPage = 1
        LoadUsers()
    End Sub

    Private Sub btnPrevPage_Click(sender As Object, e As EventArgs) Handles btnPrevPage.Click
        If currentPage > 1 Then
            currentPage -= 1
            LoadUsers()
        End If
    End Sub

    Private Sub btnNextPage_Click(sender As Object, e As EventArgs) Handles btnNextPage.Click
        If currentPage < totalPages Then
            currentPage += 1
            LoadUsers()
        End If
    End Sub

    Private Sub cboQuantity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboQuantity.SelectedIndexChanged
        If cboQuantity.SelectedItem IsNot Nothing Then
            pageSize = CType(cboQuantity.SelectedItem, KeyValuePair(Of Integer, String)).Key
            currentPage = 1
            LoadUsers()
        End If
    End Sub

    Private Sub txtKeyword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtKeyword.KeyPress
        If e.KeyChar = Convert.ToChar(Keys.Enter) Then
            btnSearch_Click(sender, e)
        End If
    End Sub
End Class