Public Class PhoneInputForm
    Inherits Form

    Private phoneId As Integer? ' Null cho Create, có giá trị cho Update

    Public Sub New(Optional phoneDto As PhoneDto = Nothing)
        InitializeComponent()
        phoneId = If(phoneDto IsNot Nothing, phoneDto.Id, Nothing)
        Text = If(phoneId.HasValue AndAlso phoneId.Value <> 0, "Cập nhật điện thoại", "Thêm điện thoại mới")
        FormBorderStyle = FormBorderStyle.FixedDialog
        MaximizeBox = False
        MinimizeBox = False
        StartPosition = FormStartPosition.CenterParent
        LoadBrands()
        If phoneDto IsNot Nothing Then
            PopulateFields(phoneDto)
        End If
    End Sub

    Private Sub LoadBrands()
        cboBrand.DataSource = ServiceRegistry.BrandService.GetAll()
        cboBrand.DisplayMember = "Name"
        cboBrand.ValueMember = "Id"
        If cboBrand.Items.Count > 0 Then
            cboBrand.SelectedIndex = 0
        End If
    End Sub

    Private Sub PopulateFields(phoneDto As PhoneDto)
        txtModel.Text = phoneDto.Model
        txtPrice.Text = phoneDto.Price.ToString()
        txtStock.Text = phoneDto.Stock.ToString()
        Dim brand As BrandDto = ServiceRegistry.BrandService.GetAll().FirstOrDefault(Function(b) b.Name = phoneDto.BrandName)
        If brand IsNot Nothing Then
            cboBrand.SelectedValue = brand.Id
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        ' Xóa thông báo lỗi cũ
        lblModelError.Text = ""
        lblPriceError.Text = ""
        lblStockError.Text = ""

        Dim hasError As Boolean = False

        ' Kiểm tra Model
        If String.IsNullOrEmpty(txtModel.Text) Then
            lblModelError.Text = "Vui lòng nhập mẫu điện thoại"
            hasError = True
        End If

        ' Kiểm tra Price
        Dim priceValue As Decimal
        If Not Decimal.TryParse(txtPrice.Text, priceValue) Then
            lblPriceError.Text = "Giá phải là số hợp lệ"
            hasError = True
        ElseIf priceValue < 0 Then
            lblPriceError.Text = "Giá phải >= 0"
            hasError = True
        End If

        ' Kiểm tra Stock
        Dim stockValue As Integer
        If Not Integer.TryParse(txtStock.Text, stockValue) Then
            lblStockError.Text = "Tồn kho phải là số nguyên"
            hasError = True
        ElseIf stockValue < 0 Then
            lblStockError.Text = "Tồn kho phải >= 0"
            hasError = True
        End If

        If hasError Then
            Return
        End If

        If phoneId.HasValue AndAlso phoneId.Value <> 0 Then
            ' Update
            Dim request As New UpdatePhoneRequest With {
                .Id = phoneId.Value,
                .Model = txtModel.Text,
                .Price = priceValue,
                .Stock = stockValue,
                .BrandId = Convert.ToInt32(cboBrand.SelectedValue)
            }
            ServiceRegistry.PhoneService.Update(request)
        Else
            ' Create
            Dim request As New CreatePhoneRequest With {
                .Model = txtModel.Text,
                .Price = priceValue,
                .Stock = stockValue,
                .BrandId = Convert.ToInt32(cboBrand.SelectedValue)
            }
            ServiceRegistry.PhoneService.Add(request)
        End If
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub

End Class