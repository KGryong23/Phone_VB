Public Class RoleInputForm
    Private _role As RoleDto
    Private _isReadOnly As Boolean

    ' Constructor for add/edit
    Public Sub New(Optional role As RoleDto = Nothing, Optional isReadOnly As Boolean = False)
        InitializeComponent()
        _role = role
        _isReadOnly = isReadOnly
    End Sub

    Private Sub RoleInputForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _role IsNot Nothing Then
            txtName.Text = _role.Name
            txtDescription.Text = _role.Description
        End If

        If _isReadOnly Then
            txtName.ReadOnly = True
            txtDescription.ReadOnly = True
            btnSave.Visible = False
            btnCancel.Text = "Đóng"
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        lblNameError.Text = ""
        lblDescriptionError.Text = ""
        ErrorProvider1.SetError(txtName, "")
        ErrorProvider1.SetError(txtDescription, "")

        Dim isValid As Boolean = True

        If String.IsNullOrEmpty(txtName.Text) Then
            lblNameError.Text = "Tên vai trò không được để trống."
            ErrorProvider1.SetError(txtName, "Tên vai trò không được để trống.")
            isValid = False
        End If

        If String.IsNullOrEmpty(txtDescription.Text) Then
            lblDescriptionError.Text = "Mô tả không được để trống."
            ErrorProvider1.SetError(txtDescription, "Mô tả không được để trống.")
            isValid = False
        End If

        If Not isValid Then Return

        Try
            If _role Is Nothing Then
                ' Create mode
                Dim request As New RoleCreateRequest With {
                    .Name = txtName.Text.Trim(),
                    .Description = txtDescription.Text.Trim()
                }
                If ServiceRegistry.RoleService.Add(request) Then
                    MessageBox.Show("Thêm vai trò thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                Else
                    MessageBox.Show("Thêm vai trò thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            Else
                ' Edit mode
                Dim request As New RoleUpdateRequest With {
                    .Id = _role.Id,
                    .Name = txtName.Text.Trim(),
                    .Description = txtDescription.Text.Trim()
                }
                If ServiceRegistry.RoleService.Update(request) Then
                    MessageBox.Show("Cập nhật vai trò thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                Else
                    MessageBox.Show("Cập nhật vai trò thất bại!", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Có lỗi xảy ra: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class