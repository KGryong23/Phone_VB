Public Class HomeForm
    Private Sub HomeForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LabelWelcome.Text = "Welcome " & CurrentUser.Email
    End Sub
End Class