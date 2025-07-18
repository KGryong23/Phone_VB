Public Interface IStockTransactionService
    Function GetPaged(query As BaseQuery) As PagedResult(Of StockTransactionDto)
    Function GetById(id As Integer) As StockTransactionDto
    Function ImportStock(request As ImportRequestDto) As Boolean
    Function ExportStock(request As ExportRequestDto) As Boolean
    Function ApproveRequest(id As Integer, approvedBy As Integer) As Boolean
    Function DeleteRequest(id As Integer) As Boolean
End Interface
