Public Class MainForm
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim homeForm As New HomeForm()
        ShowFormInContentPanel(homeForm)

        ' Ẩn nút nếu không có quyền
        btnPhoneTransfer.Visible = CurrentUser.HasPermission("view_phones")
        btnStockTransfer.Visible = CurrentUser.HasPermission("view_stocktrans")
        btnRoleTransfer.Visible = CurrentUser.HasPermission("view_roles")
        btnUserTransfer.Visible = CurrentUser.HasPermission("view_users") ' Đổi tên quyền nếu cần
    End Sub

    Private Sub ShowFormInContentPanel(form As Form)
        pnlContent.Controls.Clear()
        form.TopLevel = False
        form.FormBorderStyle = FormBorderStyle.None
        form.Dock = DockStyle.Fill
        form.Visible = True
        pnlContent.Controls.Add(form)
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

    End Sub
End Class