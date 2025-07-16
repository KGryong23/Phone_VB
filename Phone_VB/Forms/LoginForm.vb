Public Class LoginForm
    Inherits Form

    Public Sub New()
        InitializeComponent()
    End Sub

    Private Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Try
            Dim username As String = txtUsername.Text.Trim()
            Dim password As String = txtPassword.Text.Trim()

            If String.IsNullOrEmpty(username) Then
                MessageBox.Show("Vui lòng nhập tên đăng nhập.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If
            If String.IsNullOrEmpty(password) Then
                MessageBox.Show("Vui lòng nhập mật khẩu.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Dim user As UserDto = ServiceRegistry.UserService.CheckLogin(New LoginRequest With {.Username = username, .Password = password})
            If user.Username Is Nothing Then
                MessageBox.Show("Tên đăng nhập hoặc mật khẩu không đúng.", "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            ' Lưu thông tin người dùng hiện tại
            'CurrentUser.SetUser(user.Id, user.Username, user.Role)

            ' Mở PhoneForm trực tiếp
            Using homeForm As New HomeForm()
                Hide()
                PhoneForm.ShowDialog()
                Close()
            End Using
        Catch ex As Exception
            Debug.WriteLine("Login error: " & ex.Message)
            MessageBox.Show("Đăng nhập thất bại: " & ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub lblPassword_Click(sender As Object, e As EventArgs) Handles lblPassword.Click

    End Sub
End Class