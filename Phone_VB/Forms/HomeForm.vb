Public Class HomeForm
    Private Sub HomeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub btnPhoneTransfer_Click(sender As Object, e As EventArgs) Handles btnPhoneTransfer.Click
        Using phoneForm As New PhoneForm()
            Hide()
            phoneForm.ShowDialog()
            Show()
        End Using
    End Sub

    Private Sub btnStockTransfer_Click(sender As Object, e As EventArgs) Handles btnStockTransfer.Click
        'Using stockForm As New StockForm()
        '    Me.Hide()
        '    stockForm.ShowDialog()
        '    Me.Show()
        'End Using
    End Sub

    Private Sub btnRoleTransfer_Click(sender As Object, e As EventArgs) Handles btnRoleTransfer.Click
        'Using roleForm As New RoleForm()
        '    Me.Hide()
        '    roleForm.ShowDialog()
        '    Me.Show()
        'End Using
    End Sub

    Private Sub btnUserTransfer_Click(sender As Object, e As EventArgs) Handles btnUserTransfer.Click
        'Using userForm As New UserForm()
        '    Me.Hide()
        '    userForm.ShowDialog()
        '    Me.Show()
        'End Using
    End Sub
End Class