<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PhoneInputForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.lblModel = New System.Windows.Forms.Label()
        Me.txtModel = New System.Windows.Forms.TextBox()
        Me.lblPrice = New System.Windows.Forms.Label()
        Me.txtPrice = New System.Windows.Forms.TextBox()
        Me.txtStock = New System.Windows.Forms.TextBox()
        Me.lblStock = New System.Windows.Forms.Label()
        Me.cboBrand = New System.Windows.Forms.ComboBox()
        Me.lblBrand = New System.Windows.Forms.Label()
        Me.btnSave = New System.Windows.Forms.Button()
        Me.btnCancel = New System.Windows.Forms.Button()
        Me.ErrorProvider1 = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.lblModelError = New System.Windows.Forms.Label()
        Me.lblPriceError = New System.Windows.Forms.Label()
        Me.lblStockError = New System.Windows.Forms.Label()
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'lblModel
        '
        Me.lblModel.AutoSize = True
        Me.lblModel.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblModel.Location = New System.Drawing.Point(132, 77)
        Me.lblModel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblModel.Name = "lblModel"
        Me.lblModel.Size = New System.Drawing.Size(94, 19)
        Me.lblModel.TabIndex = 0
        Me.lblModel.Text = "Tên sản phẩm"
        '
        'txtModel
        '
        Me.txtModel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtModel.Location = New System.Drawing.Point(239, 66)
        Me.txtModel.Margin = New System.Windows.Forms.Padding(4)
        Me.txtModel.Multiline = True
        Me.txtModel.Name = "txtModel"
        Me.txtModel.Size = New System.Drawing.Size(180, 30)
        Me.txtModel.TabIndex = 1
        '
        'lblPrice
        '
        Me.lblPrice.AutoSize = True
        Me.lblPrice.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPrice.Location = New System.Drawing.Point(199, 139)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(27, 17)
        Me.lblPrice.TabIndex = 2
        Me.lblPrice.Text = "Giá"
        '
        'txtPrice
        '
        Me.txtPrice.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPrice.Location = New System.Drawing.Point(239, 127)
        Me.txtPrice.Multiline = True
        Me.txtPrice.Name = "txtPrice"
        Me.txtPrice.Size = New System.Drawing.Size(180, 29)
        Me.txtPrice.TabIndex = 3
        '
        'txtStock
        '
        Me.txtStock.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtStock.Location = New System.Drawing.Point(239, 183)
        Me.txtStock.Multiline = True
        Me.txtStock.Name = "txtStock"
        Me.txtStock.Size = New System.Drawing.Size(180, 31)
        Me.txtStock.TabIndex = 4
        '
        'lblStock
        '
        Me.lblStock.AutoSize = True
        Me.lblStock.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblStock.Location = New System.Drawing.Point(167, 195)
        Me.lblStock.Name = "lblStock"
        Me.lblStock.Size = New System.Drawing.Size(59, 19)
        Me.lblStock.TabIndex = 5
        Me.lblStock.Text = "Tồn kho"
        '
        'cboBrand
        '
        Me.cboBrand.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.cboBrand.FormattingEnabled = True
        Me.cboBrand.Location = New System.Drawing.Point(239, 241)
        Me.cboBrand.Name = "cboBrand"
        Me.cboBrand.Size = New System.Drawing.Size(180, 25)
        Me.cboBrand.TabIndex = 6
        '
        'lblBrand
        '
        Me.lblBrand.AutoSize = True
        Me.lblBrand.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblBrand.Location = New System.Drawing.Point(140, 247)
        Me.lblBrand.Name = "lblBrand"
        Me.lblBrand.Size = New System.Drawing.Size(86, 19)
        Me.lblBrand.TabIndex = 8
        Me.lblBrand.Text = "Thương hiệu"
        '
        'btnSave
        '
        Me.btnSave.BackColor = System.Drawing.Color.RoyalBlue
        Me.btnSave.Location = New System.Drawing.Point(239, 298)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(83, 36)
        Me.btnSave.TabIndex = 9
        Me.btnSave.Text = "Lưu"
        Me.btnSave.UseVisualStyleBackColor = False
        '
        'btnCancel
        '
        Me.btnCancel.BackColor = System.Drawing.Color.Crimson
        Me.btnCancel.Location = New System.Drawing.Point(340, 298)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(79, 36)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "Hủy"
        Me.btnCancel.UseVisualStyleBackColor = False
        '
        'ErrorProvider1
        '
        Me.ErrorProvider1.ContainerControl = Me
        '
        'lblModelError
        '
        Me.lblModelError.AutoSize = True
        Me.lblModelError.ForeColor = System.Drawing.Color.Red
        Me.lblModelError.Location = New System.Drawing.Point(236, 100)
        Me.lblModelError.Name = "lblModelError"
        Me.lblModelError.Size = New System.Drawing.Size(0, 15)
        Me.lblModelError.TabIndex = 11
        '
        'lblPriceError
        '
        Me.lblPriceError.AutoSize = True
        Me.lblPriceError.Location = New System.Drawing.Point(236, 159)
        Me.lblPriceError.Name = "lblPriceError"
        Me.lblPriceError.Size = New System.Drawing.Size(0, 15)
        Me.lblPriceError.ForeColor = System.Drawing.Color.Red
        Me.lblPriceError.TabIndex = 12
        '
        'lblStockError
        '
        Me.lblStockError.AutoSize = True
        Me.lblStockError.Location = New System.Drawing.Point(236, 217)
        Me.lblStockError.Name = "lblStockError"
        Me.lblStockError.Size = New System.Drawing.Size(0, 15)
        Me.lblStockError.ForeColor = System.Drawing.Color.Red
        Me.lblStockError.TabIndex = 13
        '
        'PhoneInputForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.DarkKhaki
        Me.ClientSize = New System.Drawing.Size(607, 401)
        Me.Controls.Add(Me.lblStockError)
        Me.Controls.Add(Me.lblPriceError)
        Me.Controls.Add(Me.lblModelError)
        Me.Controls.Add(Me.btnCancel)
        Me.Controls.Add(Me.btnSave)
        Me.Controls.Add(Me.lblBrand)
        Me.Controls.Add(Me.cboBrand)
        Me.Controls.Add(Me.lblStock)
        Me.Controls.Add(Me.txtStock)
        Me.Controls.Add(Me.txtPrice)
        Me.Controls.Add(Me.lblPrice)
        Me.Controls.Add(Me.txtModel)
        Me.Controls.Add(Me.lblModel)
        Me.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "PhoneInputForm"
        Me.Text = "Thêm mới điện thoại"
        CType(Me.ErrorProvider1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents lblModel As Label
    Friend WithEvents txtModel As TextBox
    Friend WithEvents lblPrice As Label
    Friend WithEvents txtPrice As TextBox
    Friend WithEvents txtStock As TextBox
    Friend WithEvents lblStock As Label
    Friend WithEvents cboBrand As ComboBox
    Friend WithEvents lblBrand As Label
    Friend WithEvents btnSave As Button
    Friend WithEvents btnCancel As Button
    Friend WithEvents ErrorProvider1 As ErrorProvider
    Friend WithEvents lblStockError As Label
    Friend WithEvents lblPriceError As Label
    Friend WithEvents lblModelError As Label
End Class
