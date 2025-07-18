<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StockInOutForm
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.lblModel = New System.Windows.Forms.Label()
        Me.lblStock = New System.Windows.Forms.Label()
        Me.txtQuantity = New System.Windows.Forms.TextBox()
        Me.lblQuantityError = New System.Windows.Forms.Label()
        Me.btnSubmit = New System.Windows.Forms.Button()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtNote = New System.Windows.Forms.TextBox()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Label1.Location = New System.Drawing.Point(87, 46)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(101, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Tên sản phẩm :"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Label2.Location = New System.Drawing.Point(122, 79)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(66, 19)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Tồn kho :"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Label3.Location = New System.Drawing.Point(82, 116)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(106, 19)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Nhập số lượng :"
        '
        'lblModel
        '
        Me.lblModel.AutoSize = True
        Me.lblModel.Location = New System.Drawing.Point(194, 52)
        Me.lblModel.Name = "lblModel"
        Me.lblModel.Size = New System.Drawing.Size(40, 13)
        Me.lblModel.TabIndex = 3
        Me.lblModel.Text = "Label4"
        '
        'lblStock
        '
        Me.lblStock.AutoSize = True
        Me.lblStock.Location = New System.Drawing.Point(194, 85)
        Me.lblStock.Name = "lblStock"
        Me.lblStock.Size = New System.Drawing.Size(40, 13)
        Me.lblStock.TabIndex = 4
        Me.lblStock.Text = "Label5"
        '
        'txtQuantity
        '
        Me.txtQuantity.Font = New System.Drawing.Font("Segoe UI", 10.2!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtQuantity.Location = New System.Drawing.Point(194, 109)
        Me.txtQuantity.Name = "txtQuantity"
        Me.txtQuantity.Size = New System.Drawing.Size(164, 26)
        Me.txtQuantity.TabIndex = 5
        '
        'lblQuantityError
        '
        Me.lblQuantityError.AutoSize = True
        Me.lblQuantityError.ForeColor = System.Drawing.Color.Red
        Me.lblQuantityError.Location = New System.Drawing.Point(194, 138)
        Me.lblQuantityError.Name = "lblQuantityError"
        Me.lblQuantityError.Size = New System.Drawing.Size(40, 13)
        Me.lblQuantityError.TabIndex = 6
        Me.lblQuantityError.Text = "Label4"
        '
        'btnSubmit
        '
        Me.btnSubmit.Location = New System.Drawing.Point(144, 221)
        Me.btnSubmit.Name = "btnSubmit"
        Me.btnSubmit.Size = New System.Drawing.Size(106, 35)
        Me.btnSubmit.TabIndex = 7
        Me.btnSubmit.Text = "Button1"
        Me.btnSubmit.UseVisualStyleBackColor = True
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(130, 157)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(58, 17)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Ghi chú :"
        '
        'txtNote
        '
        Me.txtNote.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtNote.Location = New System.Drawing.Point(194, 154)
        Me.txtNote.Multiline = True
        Me.txtNote.Name = "txtNote"
        Me.txtNote.Size = New System.Drawing.Size(164, 50)
        Me.txtNote.TabIndex = 9
        '
        'StockInOutForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 12.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(415, 289)
        Me.Controls.Add(Me.txtNote)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.btnSubmit)
        Me.Controls.Add(Me.lblQuantityError)
        Me.Controls.Add(Me.txtQuantity)
        Me.Controls.Add(Me.lblStock)
        Me.Controls.Add(Me.lblModel)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Segoe UI", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "StockInOutForm"
        Me.Text = "StockInOutForm"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents lblModel As Label
    Friend WithEvents lblStock As Label
    Friend WithEvents txtQuantity As TextBox
    Friend WithEvents lblQuantityError As Label
    Friend WithEvents btnSubmit As Button
    Friend WithEvents Label4 As Label
    Friend WithEvents txtNote As TextBox
End Class
