Public Class StockTransaction
    Public Property Id As Integer
    Public Property PhoneId As Integer
    Public Property UserId As Integer
    Public Property ApprovedBy As Integer?
    Public Property Quantity As Integer
    Public Property TransactionType As String ' "import" hoặc "export"
    Public Property Status As String ' "pending", "approved", "rejected"
    Public Property Note As String
    Public Property CreatedAt As DateTime
    Public Property ApprovedAt As DateTime?
End Class