<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class HomeForm
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
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.btnStockTransfer = New System.Windows.Forms.Button()
        Me.btnRoleTransfer = New System.Windows.Forms.Button()
        Me.btnUserTransfer = New System.Windows.Forms.Button()
        Me.TextBox1 = New System.Windows.Forms.TextBox()
        Me.ColorDialog1 = New System.Windows.Forms.ColorDialog()
        Me.btnPhoneTransfer = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(61, 4)
        '
        'btnStockTransfer
        '
        Me.btnStockTransfer.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.btnStockTransfer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnStockTransfer.Location = New System.Drawing.Point(31, 196)
        Me.btnStockTransfer.Name = "btnStockTransfer"
        Me.btnStockTransfer.Size = New System.Drawing.Size(167, 47)
        Me.btnStockTransfer.TabIndex = 3
        Me.btnStockTransfer.Text = "Quản lý xuất nhập"
        Me.btnStockTransfer.UseVisualStyleBackColor = False
        '
        'btnRoleTransfer
        '
        Me.btnRoleTransfer.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.btnRoleTransfer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnRoleTransfer.Location = New System.Drawing.Point(31, 263)
        Me.btnRoleTransfer.Name = "btnRoleTransfer"
        Me.btnRoleTransfer.Size = New System.Drawing.Size(167, 47)
        Me.btnRoleTransfer.TabIndex = 4
        Me.btnRoleTransfer.Text = "Quản lý vai trò"
        Me.btnRoleTransfer.UseVisualStyleBackColor = False
        '
        'btnUserTransfer
        '
        Me.btnUserTransfer.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.btnUserTransfer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnUserTransfer.Location = New System.Drawing.Point(31, 329)
        Me.btnUserTransfer.Name = "btnUserTransfer"
        Me.btnUserTransfer.Size = New System.Drawing.Size(167, 47)
        Me.btnUserTransfer.TabIndex = 5
        Me.btnUserTransfer.Text = "Quản lý người dùng"
        Me.btnUserTransfer.UseVisualStyleBackColor = False
        '
        'TextBox1
        '
        Me.TextBox1.BackColor = System.Drawing.SystemColors.Info
        Me.TextBox1.Enabled = False
        Me.TextBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 25.0!)
        Me.TextBox1.Location = New System.Drawing.Point(31, 32)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(742, 60)
        Me.TextBox1.TabIndex = 6
        Me.TextBox1.Text = "Welcome"
        Me.TextBox1.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'btnPhoneTransfer
        '
        Me.btnPhoneTransfer.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.btnPhoneTransfer.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnPhoneTransfer.Location = New System.Drawing.Point(31, 130)
        Me.btnPhoneTransfer.Name = "btnPhoneTransfer"
        Me.btnPhoneTransfer.Size = New System.Drawing.Size(167, 47)
        Me.btnPhoneTransfer.TabIndex = 7
        Me.btnPhoneTransfer.Text = "Quản lý điện thoại"
        Me.btnPhoneTransfer.UseVisualStyleBackColor = False
        '
        'HomeForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.btnPhoneTransfer)
        Me.Controls.Add(Me.TextBox1)
        Me.Controls.Add(Me.btnUserTransfer)
        Me.Controls.Add(Me.btnRoleTransfer)
        Me.Controls.Add(Me.btnStockTransfer)
        Me.Name = "HomeForm"
        Me.Text = "Trang chủ"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents btnStockTransfer As Button
    Friend WithEvents btnRoleTransfer As Button
    Friend WithEvents btnUserTransfer As Button
    Friend WithEvents TextBox1 As TextBox
    Friend WithEvents ColorDialog1 As ColorDialog
    Friend WithEvents btnPhoneTransfer As Button
End Class
