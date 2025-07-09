' Lớp lưu kết quả phân trang
Public Class PagedResult(Of T)
    Public Property Data As List(Of T)
    Public Property TotalRecords As Integer

    Public Sub New(data As List(Of T), totalRecords As Integer)
        Me.Data = data
        Me.TotalRecords = totalRecords
    End Sub
End Class

' Lớp chứa tham số truy vấn
Public Class BaseQuery
    Public Property Keyword As String
    Public Property Skip As Integer
    Public Property Take As Integer

    Public Sub New(keyword As String, skip As Integer, take As Integer)
        Me.Keyword = keyword
        Me.Skip = skip
        Me.Take = take
    End Sub
End Class
