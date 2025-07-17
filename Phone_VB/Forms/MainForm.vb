Public Class MainForm
    Private pnlContent As Panel
    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        pnlContent = New Panel()
        pnlContent.Dock = DockStyle.Fill
        SplitContainer1.Panel2.Controls.Add(pnlContent)

        Dim homeForm As New HomeForm()
        ShowFormInContentPanel(homeForm)
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

    End Sub

    Private Sub btnRoleTransfer_Click(sender As Object, e As EventArgs) Handles btnRoleTransfer.Click

    End Sub

    Private Sub btnUserTransfer_Click(sender As Object, e As EventArgs) Handles btnUserTransfer.Click

    End Sub
End Class