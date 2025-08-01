﻿Public Class MainForm
    Private socketClient As PermissionSocketClient

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' Khởi tạo và kết nối socket client
        InitializeSocketClient()

        Dim homeForm As New HomeForm()
        ShowFormInContentPanel(homeForm)
        ' Ẩn nút nếu không có quyền
        UpdateUIBasedOnPermissions()
    End Sub

    Private Sub InitializeSocketClient()
        Try
            socketClient = PermissionSocketClient.Instance

            ' Đăng ký event handlers
            AddHandler socketClient.PermissionsChanged, AddressOf OnPermissionsChanged
            AddHandler socketClient.UserForceLoggedOut, AddressOf OnUserForceLoggedOut
            AddHandler socketClient.OnlineUsersUpdated, AddressOf OnOnlineUsersUpdated

            ' Kết nối tới socket server
            socketClient.ConnectAsync()

            ' Đăng ký user với server
            If CurrentUser.UserId.HasValue AndAlso CurrentUser.RoleId.HasValue Then
                socketClient.RegisterUserAsync(CurrentUser.UserId.Value, CurrentUser.RoleId.Value)
            End If

        Catch ex As Exception
            Debug.WriteLine("Failed to initialize socket client: " + ex.Message)
        End Try
    End Sub

    Private Sub OnPermissionsChanged(roleId As Integer)
        Try
            Debug.WriteLine("=== OnPermissionsChanged called with roleId: " + roleId.ToString() + " ===")

            ' Đóng tất cả form con đang mở
            CloseAllChildForms()

            ' Cập nhật UI dựa trên quyền mới
            UpdateUIBasedOnPermissions()

            ' Chuyển về HomeForm
            Dim homeForm As New HomeForm()
            ShowFormInContentPanel(homeForm)

            Debug.WriteLine("=== OnPermissionsChanged completed ===")
        Catch ex As Exception
            Debug.WriteLine("Error handling permissions changed: " + ex.Message)
        End Try
    End Sub

    Private Sub OnUserForceLoggedOut(reason As String)
        Try
            ' Đưa form lên foreground để user biết là đang bị logout
            Me.WindowState = FormWindowState.Normal
            Me.BringToFront()
            Me.TopMost = True
            Me.Focus()
            Me.Activate()
            Me.TopMost = False

            ' Log lý do logout cho debugging
            Debug.WriteLine("User force logged out: " + reason)

            ' Đóng tất cả form con đang mở
            CloseAllChildForms()

            ' Clear current user
            CurrentUser.ClearUser()

            ' Đóng socket connection
            If socketClient IsNot Nothing Then
                socketClient.Disconnect()
            End If

            ' Ẩn MainForm thay vì đóng
            Me.Hide()

            ' Tìm LoginForm gốc (main form của application)
            Dim loginForm As LoginForm = Nothing
            For Each form As Form In Application.OpenForms
                If TypeOf form Is LoginForm Then
                    loginForm = DirectCast(form, LoginForm)
                    Exit For
                End If
            Next

            ' Nếu tìm thấy LoginForm gốc, hiển thị lại
            If loginForm IsNot Nothing Then
                loginForm.Show()
                loginForm.WindowState = FormWindowState.Normal
                loginForm.BringToFront()
                loginForm.ResetForm() ' Reset form về trạng thái ban đầu
            Else
                ' Nếu không tìm thấy LoginForm gốc, có thể là lỗi nghiêm trọng
                ' Tắt toàn bộ application để tránh process chạy ngầm
                Debug.WriteLine("ERROR: LoginForm gốc không tìm thấy! Đang tắt application...")
                Application.Exit()
            End If

        Catch ex As Exception
            Debug.WriteLine("Error handling force logout: " + ex.Message)
        End Try
    End Sub

    Private Sub OnOnlineUsersUpdated(onlineUsers As List(Of OnlineUser))
        Try
            Debug.WriteLine("Online users updated: " + onlineUsers.Count.ToString() + " users online")
            ' TODO: Update online users display if needed
        Catch ex As Exception
            Debug.WriteLine("Error handling online users update: " + ex.Message)
        End Try
    End Sub

    Private Sub UpdateUIBasedOnPermissions()
        ' Ẩn/hiện nút dựa trên quyền
        btnPhoneTransfer.Visible = CurrentUser.HasPermission("view_phones")
        btnStockTransfer.Visible = CurrentUser.HasPermission("view_stocktrans")
        btnRoleTransfer.Visible = CurrentUser.HasPermission("view_roles")
        btnUserTransfer.Visible = CurrentUser.HasPermission("view_users")
        btnSessionTransfer.Visible = CurrentUser.HasPermission("view_sessions")
    End Sub

    Private Sub MainForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Đóng socket connection khi đóng form
        If socketClient IsNot Nothing Then
            socketClient.Disconnect()
        End If

        ' Clear current user khi đóng MainForm
        CurrentUser.ClearUser()

        ' Tìm LoginForm gốc và hiển thị lại thay vì để application chạy ngầm
        Dim loginForm As LoginForm = Nothing
        For Each form As Form In Application.OpenForms
            If TypeOf form Is LoginForm Then
                loginForm = DirectCast(form, LoginForm)
                Exit For
            End If
        Next

        ' Nếu tìm thấy LoginForm gốc, hiển thị lại
        If loginForm IsNot Nothing Then
            loginForm.Show()
            loginForm.WindowState = FormWindowState.Normal
            loginForm.BringToFront()
            loginForm.ResetForm()
        Else
            ' Nếu không tìm thấy LoginForm gốc, tắt application
            Application.Exit()
        End If
    End Sub

    Private Sub ShowFormInContentPanel(form As Form)
        pnlContent.Controls.Clear()
        form.TopLevel = False
        form.FormBorderStyle = FormBorderStyle.None
        form.Dock = DockStyle.Fill
        form.Visible = True
        pnlContent.Controls.Add(form)
    End Sub

    Private Sub CloseAllChildForms()
        Try
            ' Tạo list để tránh modification exception
            Dim formsToClose As New List(Of Form)()

            ' Tìm tất cả form con của MainForm
            For Each form As Form In Application.OpenForms
                If form IsNot Me AndAlso form.Owner Is Me Then
                    formsToClose.Add(form)
                End If
            Next

            ' Tìm các modal form khác
            For Each form As Form In Application.OpenForms
                If form IsNot Me AndAlso Not TypeOf form Is LoginForm Then
                    If form.Modal OrElse form.FormBorderStyle = FormBorderStyle.FixedDialog Then
                        formsToClose.Add(form)
                    End If
                End If
            Next

            ' Đóng tất cả form đã tìm được
            For Each form In formsToClose
                Debug.WriteLine("Closing form: " + form.GetType().Name)
                form.Close()
            Next

            Debug.WriteLine("Closed " + formsToClose.Count.ToString() + " child forms")
        Catch ex As Exception
            Debug.WriteLine("Error closing child forms: " + ex.Message)
        End Try
    End Sub

    Private Sub btnHomeTransfer_Click(sender As Object, e As EventArgs) Handles btnHomeTransfer.Click
        Dim homeForm As New HomeForm()
        ShowFormInContentPanel(homeForm)
    End Sub

    Private Sub btnPhoneTransfer_Click(sender As Object, e As EventArgs) Handles btnPhoneTransfer.Click
        Dim phoneForm As New PhoneForm()
        ShowFormInContentPanel(phoneForm)
    End Sub

    Private Sub btnStockTransfer_Click(sender As Object, e As EventArgs) Handles btnStockTransfer.Click
        Dim stockTransForm As New StockTransForm()
        ShowFormInContentPanel(stockTransForm)
    End Sub

    Private Sub btnRoleTransfer_Click(sender As Object, e As EventArgs) Handles btnRoleTransfer.Click
        Dim roleForm As New RoleForm()
        ShowFormInContentPanel(roleForm)
    End Sub

    Private Sub btnUserTransfer_Click(sender As Object, e As EventArgs) Handles btnUserTransfer.Click
        Dim userForm As New UserForm()
        ShowFormInContentPanel(userForm)
    End Sub

    Private Sub btnSessionTransfer_Click(sender As Object, e As EventArgs) Handles btnSessionTransfer.Click
        Dim onlineUsersForm As New OnlineUsersForm()
        ShowFormInContentPanel(onlineUsersForm)
    End Sub

    Private Sub btnLogOut_Click(sender As Object, e As EventArgs) Handles btnLogOut.Click
        Try
            ' Đóng tất cả form con đang mở
            CloseAllChildForms()

            ' Clear current user
            CurrentUser.ClearUser()

            ' Đóng socket connection
            If socketClient IsNot Nothing Then
                socketClient.Disconnect()
            End If

            ' Ẩn MainForm thay vì đóng
            Me.Hide()

            ' Tìm LoginForm gốc (main form của application)
            Dim loginForm As LoginForm = Nothing
            For Each form As Form In Application.OpenForms
                If TypeOf form Is LoginForm Then
                    loginForm = DirectCast(form, LoginForm)
                    Exit For
                End If
            Next

            ' Nếu tìm thấy LoginForm gốc, hiển thị lại
            If loginForm IsNot Nothing Then
                loginForm.Show()
                loginForm.WindowState = FormWindowState.Normal
                loginForm.BringToFront()
                loginForm.ResetForm() ' Reset form về trạng thái ban đầu
            Else
                ' Nếu không tìm thấy LoginForm gốc, có thể là lỗi nghiêm trọng
                ' Tắt toàn bộ application để tránh process chạy ngầm
                Debug.WriteLine("ERROR: LoginForm gốc không tìm thấy! Đang tắt application...")
                Application.Exit()
            End If

        Catch ex As Exception
            Debug.WriteLine("Error during logout: " + ex.Message)
        End Try
    End Sub
End Class