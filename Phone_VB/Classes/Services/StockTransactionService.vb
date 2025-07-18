Public Class StockTransactionService
    Implements IStockTransactionService

    Private ReadOnly _stockRepo As IStockTransactionRepository
    Private ReadOnly _phoneRepo As IPhoneRepository
    Private ReadOnly _userRepo As IUserRepository

    Public Sub New(stockRepo As IStockTransactionRepository, phoneRepo As IPhoneRepository, userRepo As IUserRepository)
        _stockRepo = stockRepo
        _phoneRepo = phoneRepo
        _userRepo = userRepo
    End Sub

    Public Function GetPaged(query As BaseQuery) As PagedResult(Of StockTransactionDto) Implements IStockTransactionService.GetPaged
        Dim paged = _stockRepo.GetPaged(query)
        Dim dtos As New List(Of StockTransactionDto)
        For Each tx In paged.Data
            dtos.Add(MapToDto(tx))
        Next
        Return New PagedResult(Of StockTransactionDto)(dtos, paged.TotalRecords)
    End Function

    Public Function GetById(id As Integer) As StockTransactionDto Implements IStockTransactionService.GetById
        Dim tx = _stockRepo.GetById(id)
        If tx Is Nothing Then Return Nothing
        Return MapToDto(tx)
    End Function

    Public Function ImportStock(request As ImportRequestDto) As Boolean Implements IStockTransactionService.ImportStock
        Dim model As New StockTransaction With {
            .PhoneId = request.PhoneId,
            .UserId = request.UserId,
            .Quantity = request.Quantity,
            .Note = request.Note
        }
        Return _stockRepo.ImportStock(model)
    End Function

    Public Function ExportStock(request As ExportRequestDto) As Boolean Implements IStockTransactionService.ExportStock
        Dim model As New StockTransaction With {
            .PhoneId = request.PhoneId,
            .UserId = request.UserId,
            .Quantity = request.Quantity,
            .Note = request.Note
        }
        Return _stockRepo.ExportStock(model)
    End Function

    Public Function ApproveRequest(id As Integer, approvedBy As Integer) As Boolean Implements IStockTransactionService.ApproveRequest
        Return _stockRepo.ApproveRequest(id, approvedBy)
    End Function

    Public Function DeleteRequest(id As Integer) As Boolean Implements IStockTransactionService.DeleteRequest
        Return _stockRepo.DeleteRequest(id)
    End Function

    Private Function MapToDto(tx As StockTransaction) As StockTransactionDto
        Dim dto As New StockTransactionDto()
        dto.Id = tx.Id
        dto.PhoneName = If(_phoneRepo.GetById(tx.PhoneId)?.Model, "")
        dto.UserName = If(_userRepo.GetById(tx.UserId)?.Username, "")
        dto.ApprovedByName = If(tx.ApprovedBy.HasValue, If(_userRepo.GetById(tx.ApprovedBy.Value)?.Username, ""), "")
        dto.Quantity = tx.Quantity
        dto.TransactionType = If(tx.TransactionType = "import", "Nhập kho", If(tx.TransactionType = "export", "Xuất kho", tx.TransactionType))
        dto.Status = If(tx.Status = "pending", "Chờ duyệt", If(tx.Status = "approved", "Đã duyệt", If(tx.Status = "rejected", "Từ chối", tx.Status)))
        dto.Note = tx.Note
        Dim vnTimeZone As TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time")
        dto.CreatedAt = TimeZoneInfo.ConvertTime(tx.CreatedAt, vnTimeZone).ToString("dd/MM/yyyy HH:mm:ss")
        dto.ApprovedAt = If(tx.ApprovedAt.HasValue, TimeZoneInfo.ConvertTime(tx.ApprovedAt.Value, vnTimeZone).ToString("dd/MM/yyyy HH:mm:ss"), "")
        Return dto
    End Function
End Class
