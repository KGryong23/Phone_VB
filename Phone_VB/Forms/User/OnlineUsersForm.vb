Public Class OnlineUsersForm
    Private socketClient As PermissionSocketClient
    Private onlineUsers As List(Of OnlineUser)

    Public Sub New()
        InitializeComponent()
        onlineUsers = New List(Of OnlineUser)()
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
            MessageBox.Show("Failed to load online users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub SetupDataGridView()
        ' Clear existing columns
        dgvOnlineUsers.Columns.Clear()
        dgvOnlineUsers.AutoGenerateColumns = False

        ' Add columns
        dgvOnlineUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "UserId",
            .HeaderText = "User ID",
            .DataPropertyName = "UserId",
            .Width = 80
        })

        dgvOnlineUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "Username",
            .HeaderText = "Username",
            .DataPropertyName = "Username",
            .Width = 150
        })

        dgvOnlineUsers.Columns.Add(New DataGridViewTextBoxColumn With {
            .Name = "ConnectedAt",
            .HeaderText = "Connected At",
            .DataPropertyName = "ConnectedAt",
            .Width = 140
        })

        ' Add Force Logout button column
        Dim btnColumn As New DataGridViewButtonColumn()
        btnColumn.Name = "ForceLogout"
        btnColumn.HeaderText = "Actions"
        btnColumn.Text = "Force Logout"
        btnColumn.UseColumnTextForButtonValue = True
        btnColumn.Width = 100
        dgvOnlineUsers.Columns.Add(btnColumn)
    End Sub

    Private Sub OnOnlineUsersUpdated(users As List(Of OnlineUser))
        Try
            onlineUsers = users
            RefreshGrid()
        Catch ex As Exception
            Debug.WriteLine("Error updating online users: " + ex.Message)
        End Try
    End Sub

    Private Sub RefreshGrid()
        dgvOnlineUsers.DataSource = Nothing
        dgvOnlineUsers.DataSource = onlineUsers
        dgvOnlineUsers.Refresh()

        ' Update status label
        lblStatus.Text = "Total Online Users: " + onlineUsers.Count.ToString()
    End Sub

    Private Sub dgvOnlineUsers_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvOnlineUsers.CellClick
        Try
            ' Check if clicked on Force Logout button
            If e.ColumnIndex = dgvOnlineUsers.Columns("ForceLogout").Index AndAlso e.RowIndex >= 0 Then
                Dim selectedUser = CType(dgvOnlineUsers.Rows(e.RowIndex).DataBoundItem, OnlineUser)
                
                ' Confirm force logout
                Dim result = MessageBox.Show("Are you sure you want to force logout user '" + selectedUser.Username + "'?", 
                                           "Confirm Force Logout", 
                                           MessageBoxButtons.YesNo, 
                                           MessageBoxIcon.Question)
                
                If result = DialogResult.Yes Then
                    ForceLogoutUser(selectedUser.UserId, "Forced logout by administrator")
                End If
            End If
        Catch ex As Exception
            MessageBox.Show("Error handling cell click: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ForceLogoutUser(userId As Integer, reason As String)
        Try
            socketClient.ForceLogoutUserAsync(userId, reason)
            MessageBox.Show("Force logout command sent successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information)
            
            ' Refresh online users list
            socketClient.RequestOnlineUsersAsync()
            
        Catch ex As Exception
            MessageBox.Show("Failed to force logout user: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
        Try
            socketClient.RequestOnlineUsersAsync()
        Catch ex As Exception
            MessageBox.Show("Failed to refresh online users: " + ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub OnlineUsersForm_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        ' Remove event handler
        If socketClient IsNot Nothing Then
            RemoveHandler socketClient.OnlineUsersUpdated, AddressOf OnOnlineUsersUpdated
        End If
    End Sub
End Class
