Public Interface IStockTransactionRepository
    Function GetPaged(query As BaseQuery) As PagedResult(Of StockTransaction)
    Function ImportStock(request As StockTransaction) As Boolean
    Function ExportStock(request As StockTransaction) As Boolean
    Function ApproveRequest(id As Integer, approvedBy As Integer) As Boolean
    Function DeleteRequest(id As Integer) As Boolean
    Function GetById(id As Integer) As StockTransaction
End Interface