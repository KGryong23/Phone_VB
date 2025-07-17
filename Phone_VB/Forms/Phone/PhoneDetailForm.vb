Public Class PhoneDetailForm
    Private _phone As PhoneDto

    Public Sub New(phone As PhoneDto)
        InitializeComponent()
        _phone = phone
    End Sub

    Private Sub PhoneDetailForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _phone IsNot Nothing Then
            txtModel.Text = _phone.Model
            txtPrice.Text = _phone.Price.ToString("N0") & " VNĐ"
            txtStock.Text = _phone.Stock.ToString()
            txtBrand.Text = _phone.BrandName
            txtCreatedAt.Text = If(_phone.CreatedAt = DateTime.MinValue, "", _phone.CreatedAt.ToString("dd/MM/yyyy HH:mm:ss"))
            txtLastModified.Text = If(_phone.LastModified = DateTime.MinValue, "", _phone.LastModified.ToString("dd/MM/yyyy HH:mm:ss"))
        End If
    End Sub
End Class