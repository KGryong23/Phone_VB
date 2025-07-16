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
        cboBrand.DataSource = ServiceContainer.BrandService.GetAll()
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
        Dim brand As BrandDto = ServiceContainer.BrandService.GetAll().FirstOrDefault(Function(b) b.Name = phoneDto.BrandName)
        If brand IsNot Nothing Then
            cboBrand.SelectedValue = brand.Id
        End If
    End Sub

    Private Sub btnSave_Click(sender As Object, e As EventArgs) Handles btnSave.Click
        If Not Decimal.TryParse(txtPrice.Text, Nothing) Then
            Throw New ArgumentException("Price must be a valid number")
        End If
        If Not Integer.TryParse(txtStock.Text, Nothing) Then
            Throw New ArgumentException("Stock must be a valid number")
        End If
        If cboBrand.SelectedValue Is Nothing Then
            Throw New ArgumentException("Please select a brand")
        End If

        If phoneId.HasValue AndAlso phoneId.Value <> 0 Then
            ' Update
            Dim request As New UpdatePhoneRequest With {
                .Id = phoneId.Value,
                .Model = txtModel.Text,
                .Price = Convert.ToDecimal(txtPrice.Text),
                .Stock = Convert.ToInt32(txtStock.Text),
                .BrandId = Convert.ToInt32(cboBrand.SelectedValue)
            }
            ServiceContainer.PhoneService.Update(request)
        Else
            ' Create
            Dim request As New CreatePhoneRequest With {
                .Model = txtModel.Text,
                .Price = Convert.ToDecimal(txtPrice.Text),
                .Stock = Convert.ToInt32(txtStock.Text),
                .BrandId = Convert.ToInt32(cboBrand.SelectedValue)
            }
            ServiceContainer.PhoneService.Add(request)
        End If
        DialogResult = DialogResult.OK
        Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        DialogResult = DialogResult.Cancel
        Close()
    End Sub
End Class