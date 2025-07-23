Public Class OnlineUsersForm
    Private socketClient As PermissionSocketClient
    Private onlineUsers As List(Of OnlineUser)
    Private filteredUsers As List(Of OnlineUser)

    Public Sub New()
        InitializeComponent()
        onlineUsers = New List(Of OnlineUser)()
        filteredUsers = New List(Of OnlineUser)()
        socketClient = PermissionSocketClient.Instance
    End Sub

    Private Sub OnlineUsersForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Try
            ' Kết nối tới socket nếu chưa kết nối
            If Not socketClient.IsConnected Then
                socketClient.ConnectAsync()
            End If

            ' Đăng ký event để nhận cập nhật online users
            AddHandler socketClient.OnlineUsersUpdated, AddressOf OnOnlineUsersUpdated

            ' Request danh sách online users
            socketClient.RequestOnlineUsersAsync()

            ' Setup DataGridView
            SetupDataGridView()

        Catch ex As Exception
            MessageBox.Show("Không thể tải danh sách người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SetupDataGridView()
        ' Clear existing columns
        dgvOnlineUsers.Columns.Clear()
        dgvOnlineUsers.AutoGenerateColumns = False

        ' Add columns with Vietnamese headers
        dgvOnlineUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "UserId",
            .HeaderText = "ID",
            .DataPropertyName = "UserId",
            .Width = 60
        })

        dgvOnlineUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Username",
            .HeaderText = "Tên đăng nhập",
            .DataPropertyName = "Username",
            .Width = 200
        })

        dgvOnlineUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "ConnectedAt",
            .HeaderText = "Thời gian kết nối",
            .DataPropertyName = "ConnectedAt",
            .Width = 150
        })

        ' Set font for DataGridView
        dgvOnlineUsers.DefaultCellStyle.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        dgvOnlineUsers.ColumnHeadersDefaultCellStyle.Font = New System.Drawing.Font("Segoe UI", 10.0!, FontStyle.Bold)
    End Sub

    Private Sub OnOnlineUsersUpdated(users As List(Of OnlineUser))
        Try
            onlineUsers = users
            FilterAndRefreshGrid()
        Catch ex As Exception
            Debug.WriteLine("Error updating online users: " + ex.Message)
        End Try
    End Sub

    Private Sub FilterAndRefreshGrid()
        Dim searchText = txtSearch.Text.Trim().ToLower()

        If String.IsNullOrEmpty(searchText) Then
            filteredUsers = onlineUsers
        Else
            filteredUsers = onlineUsers.Where(Function(u) u.Username.ToLower().Contains(searchText)).ToList()
        End If

        dgvOnlineUsers.DataSource = Nothing
        dgvOnlineUsers.DataSource = filteredUsers
        dgvOnlineUsers.Refresh()

        ' Update status label
        lblStatus.Text = "Tổng số người dùng online: " + onlineUsers.Count.ToString() + " | Hiển thị: " + filteredUsers.Count.ToString()
    End Sub

    Private Sub txtSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSearch.TextChanged
        FilterAndRefreshGrid()
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        RefreshOnlineUsersList()
    End Sub

    Private Sub RefreshOnlineUsersList()
        Try
            socketClient.RequestOnlineUsersAsync()
            Debug.WriteLine("Refreshing online users list...")
        Catch ex As Exception
            MessageBox.Show("Không thể làm mới danh sách: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnForceLogout_Click(sender As Object, e As EventArgs) Handles btnForceLogout.Click
        Try
            If dgvOnlineUsers.SelectedRows.Count = 0 Then
                MessageBox.Show("Vui lòng chọn một người dùng để đăng xuất.", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim selectedUser = CType(dgvOnlineUsers.SelectedRows(0).DataBoundItem, OnlineUser)

            ' Kiểm tra xem có phải admin đang đăng xuất chính mình không
            If CurrentUser.UserId.HasValue AndAlso CurrentUser.UserId.Value = selectedUser.UserId Then
                Dim selfLogoutResult = MessageBox.Show("Bạn có chắc muốn đăng xuất chính mình không?",
                                                     "Xác nhận đăng xuất",
                                                     MessageBoxButtons.YesNo,
                                                     MessageBoxIcon.Question)
                If selfLogoutResult = DialogResult.Yes Then
                    ForceLogoutUser(selectedUser.UserId, "Tự đăng xuất bởi quản trị viên")
                End If
            Else
                ' Confirm force logout user khác
                Dim result = MessageBox.Show("Bạn có chắc muốn đăng xuất người dùng '" + selectedUser.Username + "' không?",
                                           "Xác nhận đăng xuất",
                                           MessageBoxButtons.YesNo,
                                           MessageBoxIcon.Question)

                If result = DialogResult.Yes Then
                    ForceLogoutUser(selectedUser.UserId, "Đăng xuất bởi quản trị viên")
                End If
            End If

        Catch ex As Exception
            MessageBox.Show("Không thể đăng xuất người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub ForceLogoutUser(userId As Integer, reason As String)
        Try
            socketClient.ForceLogoutUserAsync(userId, reason)

            ' Delay nhỏ để server xử lý logout trước khi refresh
            System.Threading.Thread.Sleep(500)

            ' Refresh online users list
            RefreshOnlineUsersList()

            ' Optional: Hiển thị thông báo thành công
            Debug.WriteLine("Force logout user successful, refreshing online users list")

        Catch ex As Exception
            MessageBox.Show("Không thể đăng xuất người dùng: " + ex.Message, "Lỗi", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub
    Private Sub OnlineUsersForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Remove event handler
        If socketClient IsNot Nothing Then
            RemoveHandler socketClient.OnlineUsersUpdated, AddressOf OnOnlineUsersUpdated
        End If
    End Sub
End Class
