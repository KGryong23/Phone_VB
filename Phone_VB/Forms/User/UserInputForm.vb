Public Class UserInputForm
    Inherits Form

    Private userId As Integer? ' Null cho Create, có giá trị cho Update

    Public Sub New(Optional userDto As UserDto = Nothing)
        InitializeComponent()
        userId = If(userDto IsNot Nothing, userDto.Id, Nothing)
        Text = If(userId.HasValue AndAlso userId.Value <> 0, "Cập nhật người dùng", "Thêm người dùng mới")
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        StartPosition = FormStartPosition.CenterParent
        LoadRoles()
        If userDto IsNot Nothing Then
            PopulateFields(userDto)
        End If
    End Sub

    Private Sub LoadRoles()
        cboRole.DataSource = ServiceRegistry.RoleService.GetAll()
        cboRole.DisplayMember = "Name"
        cboRole.ValueMember = "Id"
        If cboRole.Items.Count > 0 Then
            cboRole.SelectedIndex = 0
        End If
    End Sub

    Private Sub PopulateFields(userDto As UserDto)
        txtUsername.Text = userDto.Username
        txtUsername.ReadOnly = False ' Cho phép sửa username khi cập nhật
        txtPassword.Text = userDto.Password ' Cho phép sửa password khi cập nhật
        Dim role = ServiceRegistry.RoleService.GetAll().FirstOrDefault(Function(r) r.Name = userDto.RoleName)
        If role IsNot Nothing Then
            cboRole.SelectedValue = role.Id
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        lblUsernameError.Text = ""
        lblPasswordError.Text = ""
        lblRoleError.Text = ""

        Dim hasError As Boolean = False

        ' Kiểm tra Username
        If String.IsNullOrEmpty(txtUsername.Text.Trim()) Then
            lblUsernameError.Text = "Vui lòng nhập tên đăng nhập"
            hasError = True
        ElseIf txtUsername.Text.Trim().Length < 3 Then
            lblUsernameError.Text = "Tên đăng nhập phải có ít nhất 3 ký tự"
            hasError = True
        End If

        ' Kiểm tra Password
        If String.IsNullOrEmpty(txtPassword.Text) Then
            lblPasswordError.Text = "Vui lòng nhập mật khẩu"
            hasError = True
        ElseIf txtPassword.Text.Length < 6 Then
            lblPasswordError.Text = "Mật khẩu phải có ít nhất 6 ký tự"
            hasError = True
        End If

        ' Kiểm tra Role
        If cboRole.SelectedValue Is Nothing Then
            lblRoleError.Text = "Vui lòng chọn vai trò"
            hasError = True
        End If

        If hasError Then
            Return
        End If

        Try
            If userId.HasValue AndAlso userId.Value <> 0 Then
                ' Update
                Dim request As New UserUpdateRequest With {
                    .Id = userId.Value,
                    .Username = txtUsername.Text.Trim(),
                    .Password = txtPassword.Text,
                    .RoleId = Convert.ToInt32(cboRole.SelectedValue)
                }
                ServiceRegistry.UserService.Update(request)
                MessageBox.Show("Cập nhật người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Else
                ' Create
                Dim request As New UserCreateRequest With {
                    .Username = txtUsername.Text.Trim(),
                    .Password = txtPassword.Text,
                    .RoleId = Convert.ToInt32(cboRole.SelectedValue)
                }
                ServiceRegistry.UserService.Add(request)
                MessageBox.Show("Thêm người dùng thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            End If
            DialogResult = DialogResult.OK
            Close()
        Catch ex As Exception
            MessageBox.Show("Lỗi khi lưu người dùng: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

End Class