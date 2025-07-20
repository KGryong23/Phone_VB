<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class StockTransDetailForm
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
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtPhoneName = New System.Windows.Forms.Label()
        Me.txtUserName = New System.Windows.Forms.Label()
        Me.txtApprovedByName = New System.Windows.Forms.Label()
        Me.txtQuantity = New System.Windows.Forms.Label()
        Me.txtTransactionType = New System.Windows.Forms.Label()
        Me.txtStatus = New System.Windows.Forms.Label()
        Me.txtNote = New System.Windows.Forms.Label()
        Me.txtCreatedAt = New System.Windows.Forms.Label()
        Me.txtApprovedAt = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(100, 44)
        Me.Label1.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 19)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Tên sản phẩm:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(131, 145)
        Me.Label4.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(66, 19)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Số lượng:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(97, 79)
        Me.Label2.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(100, 19)
        Me.Label2.TabIndex = 5
        Me.Label2.Text = "Người yêu cầu:"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(109, 112)
        Me.Label3.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(88, 19)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Người duyệt:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(100, 176)
        Me.Label5.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(97, 19)
        Me.Label5.TabIndex = 7
        Me.Label5.Text = "Kiểu giao dịch:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(124, 207)
        Me.Label6.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(73, 19)
        Me.Label6.TabIndex = 8
        Me.Label6.Text = "Trạng thái:"
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(138, 241)
        Me.Label7.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(59, 19)
        Me.Label7.TabIndex = 9
        Me.Label7.Text = "Ghi chú:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(105, 274)
        Me.Label8.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(92, 19)
        Me.Label8.TabIndex = 10
        Me.Label8.Text = "Thời gian tạo:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(90, 309)
        Me.Label9.Margin = New System.Windows.Forms.Padding(4, 0, 4, 0)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(107, 19)
        Me.Label9.TabIndex = 11
        Me.Label9.Text = "Thời gian duyệt:"
        '
        'txtPhoneName
        '
        Me.txtPhoneName.AutoSize = True
        Me.txtPhoneName.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtPhoneName.Location = New System.Drawing.Point(210, 42)
        Me.txtPhoneName.Name = "txtPhoneName"
        Me.txtPhoneName.Size = New System.Drawing.Size(61, 20)
        Me.txtPhoneName.TabIndex = 12
        Me.txtPhoneName.Text = "Label10"
        '
        'txtUserName
        '
        Me.txtUserName.AutoSize = True
        Me.txtUserName.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtUserName.Location = New System.Drawing.Point(210, 79)
        Me.txtUserName.Name = "txtUserName"
        Me.txtUserName.Size = New System.Drawing.Size(61, 20)
        Me.txtUserName.TabIndex = 13
        Me.txtUserName.Text = "Label11"
        '
        'txtApprovedByName
        '
        Me.txtApprovedByName.AutoSize = True
        Me.txtApprovedByName.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtApprovedByName.Location = New System.Drawing.Point(210, 112)
        Me.txtApprovedByName.Name = "txtApprovedByName"
        Me.txtApprovedByName.Size = New System.Drawing.Size(61, 20)
        Me.txtApprovedByName.TabIndex = 14
        Me.txtApprovedByName.Text = "Label12"
        '
        'txtQuantity
        '
        Me.txtQuantity.AutoSize = True
        Me.txtQuantity.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtQuantity.Location = New System.Drawing.Point(210, 145)
        Me.txtQuantity.Name = "txtQuantity"
        Me.txtQuantity.Size = New System.Drawing.Size(61, 20)
        Me.txtQuantity.TabIndex = 15
        Me.txtQuantity.Text = "Label13"
        '
        'txtTransactionType
        '
        Me.txtTransactionType.AutoSize = True
        Me.txtTransactionType.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtTransactionType.Location = New System.Drawing.Point(210, 176)
        Me.txtTransactionType.Name = "txtTransactionType"
        Me.txtTransactionType.Size = New System.Drawing.Size(61, 20)
        Me.txtTransactionType.TabIndex = 16
        Me.txtTransactionType.Text = "Label14"
        '
        'txtStatus
        '
        Me.txtStatus.AutoSize = True
        Me.txtStatus.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtStatus.Location = New System.Drawing.Point(210, 207)
        Me.txtStatus.Name = "txtStatus"
        Me.txtStatus.Size = New System.Drawing.Size(61, 20)
        Me.txtStatus.TabIndex = 17
        Me.txtStatus.Text = "Label15"
        '
        'txtNote
        '
        Me.txtNote.AutoSize = True
        Me.txtNote.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtNote.Location = New System.Drawing.Point(210, 241)
        Me.txtNote.Name = "txtNote"
        Me.txtNote.Size = New System.Drawing.Size(61, 20)
        Me.txtNote.TabIndex = 18
        Me.txtNote.Text = "Label16"
        '
        'txtCreatedAt
        '
        Me.txtCreatedAt.AutoSize = True
        Me.txtCreatedAt.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtCreatedAt.Location = New System.Drawing.Point(210, 274)
        Me.txtCreatedAt.Name = "txtCreatedAt"
        Me.txtCreatedAt.Size = New System.Drawing.Size(61, 20)
        Me.txtCreatedAt.TabIndex = 19
        Me.txtCreatedAt.Text = "Label17"
        '
        'txtApprovedAt
        '
        Me.txtApprovedAt.AutoSize = True
        Me.txtApprovedAt.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtApprovedAt.Location = New System.Drawing.Point(210, 309)
        Me.txtApprovedAt.Name = "txtApprovedAt"
        Me.txtApprovedAt.Size = New System.Drawing.Size(61, 20)
        Me.txtApprovedAt.TabIndex = 20
        Me.txtApprovedAt.Text = "Label18"
        '
        'StockTransDetailForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(475, 377)
        Me.Controls.Add(Me.txtApprovedAt)
        Me.Controls.Add(Me.txtCreatedAt)
        Me.Controls.Add(Me.txtNote)
        Me.Controls.Add(Me.txtStatus)
        Me.Controls.Add(Me.txtTransactionType)
        Me.Controls.Add(Me.txtQuantity)
        Me.Controls.Add(Me.txtApprovedByName)
        Me.Controls.Add(Me.txtUserName)
        Me.Controls.Add(Me.txtPhoneName)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label1)
        Me.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "StockTransDetailForm"
        Me.Text = "Xem chi tiết giao dịch"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label1 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents Label7 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label9 As Label
    Friend WithEvents txtPhoneName As Label
    Friend WithEvents txtUserName As Label
    Friend WithEvents txtApprovedByName As Label
    Friend WithEvents txtQuantity As Label
    Friend WithEvents txtTransactionType As Label
    Friend WithEvents txtStatus As Label
    Friend WithEvents txtNote As Label
    Friend WithEvents txtCreatedAt As Label
    Friend WithEvents txtApprovedAt As Label
End Class
