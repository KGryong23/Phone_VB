Public Class StockInOutForm
    Private Mode As String
    Private Phone As PhoneDto
    Private UserId As Integer

    Public Sub New(mode As String, phone As PhoneDto, userId As Integer)
        InitializeComponent()
        Me.Mode = mode
        Me.Phone = phone
        Me.UserId = userId

        lblModel.Text = phone.Model
        lblStock.Text = phone.Stock.ToString()
        lblQuantityError.Text = ""

        If mode = "import" Then
            btnSubmit.Text = "Nhập kho"
            btnSubmit.BackColor = Color.Green
        Else
            btnSubmit.Text = "Xuất kho"
            btnSubmit.BackColor = Color.Orange
        End If
    End Sub

    Private Sub btnSubmit_Click(sender As Object, e As EventArgs) Handles btnSubmit.Click
        lblQuantityError.Text = ""
        Dim quantity As Integer

        If Not Integer.TryParse(txtQuantity.Text.Trim(), quantity) OrElse quantity <= 0 Then
            lblQuantityError.Text = "Số lượng phải là số nguyên dương."
            Return
        End If

        If Mode = "export" AndAlso quantity > Phone.Stock Then
            lblQuantityError.Text = "Số lượng xuất vượt quá tồn kho."
            Return
        End If

        Dim note As String = txtNote.Text.Trim()

        Try
            If Mode = "import" Then
                Dim request As New ImportRequestDto With {
                    .PhoneId = Phone.Id,
                    .Quantity = quantity,
                    .UserId = UserId,
                    .Note = note
                }
                If ServiceRegistry.StockTransactionService.ImportStock(request) Then
                    MessageBox.Show("Nhập kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                Else
                    lblQuantityError.Text = "Nhập kho thất bại."
                End If
            Else
                Dim request As New ExportRequestDto With {
                    .PhoneId = Phone.Id,
                    .Quantity = quantity,
                    .UserId = UserId,
                    .Note = note
                }
                If ServiceRegistry.StockTransactionService.ExportStock(request) Then
                    MessageBox.Show("Xuất kho thành công!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Me.DialogResult = DialogResult.OK
                    Me.Close()
                Else
                    lblQuantityError.Text = "Xuất kho thất bại."
                End If
            End If
        Catch ex As Exception
            lblQuantityError.Text = "Lỗi xử lý: " & ex.Message
        End Try
    End Sub
End Class