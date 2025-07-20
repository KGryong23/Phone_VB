Public Class StockTransDetailForm
    Private ReadOnly _transaction As StockTransactionDto

    Public Sub New(transaction As StockTransactionDto)
        InitializeComponent()
        _transaction = transaction
    End Sub

    Private Sub StockTransDetailForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If _transaction IsNot Nothing Then
            LoadDetail(_transaction)
        End If
    End Sub

    Public Sub LoadDetail(txDto As StockTransactionDto)
        txtPhoneName.Text = txDto.PhoneName
        txtUserName.Text = txDto.UserName
        txtApprovedByName.Text = If(String.IsNullOrEmpty(txDto.ApprovedByName), "Chưa duyệt", txDto.ApprovedByName)
        txtQuantity.Text = txDto.Quantity.ToString()
        txtTransactionType.Text = txDto.TransactionType
        txtStatus.Text = txDto.Status
        txtNote.Text = txDto.Note
        txtCreatedAt.Text = txDto.CreatedAt
        txtApprovedAt.Text = If(String.IsNullOrEmpty(txDto.ApprovedAt), "Chưa duyệt", txDto.ApprovedAt)
    End Sub
End Class