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
        Me.SuspendLayout()
        '
        'lblModel
        '
        Me.lblModel.AutoSize = True
        Me.lblModel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.lblModel.Location = New System.Drawing.Point(157, 92)
        Me.lblModel.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.lblModel.Name = "lblModel"
        Me.lblModel.Size = New System.Drawing.Size(86, 15)
        Me.lblModel.TabIndex = 0
        Me.lblModel.Text = "Tên sản phẩm"
        '
        'txtModel
        '
        Me.txtModel.Location = New System.Drawing.Point(251, 82)
        Me.txtModel.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.txtModel.Multiline = True
        Me.txtModel.Name = "txtModel"
        Me.txtModel.Size = New System.Drawing.Size(180, 25)
        Me.txtModel.TabIndex = 1
        '
        'lblPrice
        '
        Me.lblPrice.AutoSize = True
        Me.lblPrice.Location = New System.Drawing.Point(157, 141)
        Me.lblPrice.Name = "lblPrice"
        Me.lblPrice.Size = New System.Drawing.Size(26, 15)
        Me.lblPrice.TabIndex = 2
        Me.lblPrice.Text = "Giá"
        '
        'txtPrice
        '
        Me.txtPrice.Location = New System.Drawing.Point(251, 130)
        Me.txtPrice.Multiline = True
        Me.txtPrice.Name = "txtPrice"
        Me.txtPrice.Size = New System.Drawing.Size(180, 26)
        Me.txtPrice.TabIndex = 3
        '
        'txtStock
        '
        Me.txtStock.Location = New System.Drawing.Point(251, 178)
        Me.txtStock.Multiline = True
        Me.txtStock.Name = "txtStock"
        Me.txtStock.Size = New System.Drawing.Size(180, 27)
        Me.txtStock.TabIndex = 4
        '
        'lblStock
        '
        Me.lblStock.AutoSize = True
        Me.lblStock.Location = New System.Drawing.Point(157, 190)
        Me.lblStock.Name = "lblStock"
        Me.lblStock.Size = New System.Drawing.Size(51, 15)
        Me.lblStock.TabIndex = 5
        Me.lblStock.Text = "Tồn kho"
        '
        'cboBrand
        '
        Me.cboBrand.FormattingEnabled = True
        Me.cboBrand.Location = New System.Drawing.Point(251, 229)
        Me.cboBrand.Name = "cboBrand"
        Me.cboBrand.Size = New System.Drawing.Size(180, 23)
        Me.cboBrand.TabIndex = 6
        '
        'lblBrand
        '
        Me.lblBrand.AutoSize = True
        Me.lblBrand.Location = New System.Drawing.Point(157, 237)
        Me.lblBrand.Name = "lblBrand"
        Me.lblBrand.Size = New System.Drawing.Size(76, 15)
        Me.lblBrand.TabIndex = 8
        Me.lblBrand.Text = "Thương hiệu"
        '
        'btnSave
        '
        Me.btnSave.Location = New System.Drawing.Point(251, 306)
        Me.btnSave.Name = "btnSave"
        Me.btnSave.Size = New System.Drawing.Size(75, 28)
        Me.btnSave.TabIndex = 9
        Me.btnSave.Text = "Lưu"
        Me.btnSave.UseVisualStyleBackColor = True
        '
        'btnCancel
        '
        Me.btnCancel.Location = New System.Drawing.Point(358, 306)
        Me.btnCancel.Name = "btnCancel"
        Me.btnCancel.Size = New System.Drawing.Size(73, 28)
        Me.btnCancel.TabIndex = 10
        Me.btnCancel.Text = "Hủy"
        Me.btnCancel.UseVisualStyleBackColor = True
        '
        'PhoneInputForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(653, 401)
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
        Me.Margin = New System.Windows.Forms.Padding(4, 4, 4, 4)
        Me.Name = "PhoneInputForm"
        Me.Text = "PhoneInputForm"
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
End Class
