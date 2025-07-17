<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
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
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnUserTransfer = New System.Windows.Forms.Button()
        Me.btnRoleTransfer = New System.Windows.Forms.Button()
        Me.btnStockTransfer = New System.Windows.Forms.Button()
        Me.btnPhoneTransfer = New System.Windows.Forms.Button()
        Me.btnHomeTransfer = New System.Windows.Forms.Button()
        Me.pnlContent = New System.Windows.Forms.Panel()
        Me.pnlHeader.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pnlHeader
        '
        Me.pnlHeader.BackColor = System.Drawing.SystemColors.GradientActiveCaption
        Me.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlHeader.Controls.Add(Me.Label1)
        Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
        Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
        Me.pnlHeader.Margin = New System.Windows.Forms.Padding(2)
        Me.pnlHeader.Name = "pnlHeader"
        Me.pnlHeader.Size = New System.Drawing.Size(956, 60)
        Me.pnlHeader.TabIndex = 0
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Segoe UI", 14.0!)
        Me.Label1.ForeColor = System.Drawing.SystemColors.ActiveCaptionText
        Me.Label1.Location = New System.Drawing.Point(9, 14)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 0, 2, 0)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(163, 25)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Ứng dụng quản lý"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.Silver
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.btnUserTransfer)
        Me.Panel1.Controls.Add(Me.btnRoleTransfer)
        Me.Panel1.Controls.Add(Me.btnStockTransfer)
        Me.Panel1.Controls.Add(Me.btnPhoneTransfer)
        Me.Panel1.Controls.Add(Me.btnHomeTransfer)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Left
        Me.Panel1.Location = New System.Drawing.Point(0, 60)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(141, 370)
        Me.Panel1.TabIndex = 1
        '
        'btnUserTransfer
        '
        Me.btnUserTransfer.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnUserTransfer.Location = New System.Drawing.Point(0, 188)
        Me.btnUserTransfer.Margin = New System.Windows.Forms.Padding(2)
        Me.btnUserTransfer.Name = "btnUserTransfer"
        Me.btnUserTransfer.Size = New System.Drawing.Size(139, 51)
        Me.btnUserTransfer.TabIndex = 3
        Me.btnUserTransfer.Text = "Quản lý người dùng"
        Me.btnUserTransfer.UseVisualStyleBackColor = True
        '
        'btnRoleTransfer
        '
        Me.btnRoleTransfer.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnRoleTransfer.Location = New System.Drawing.Point(0, 140)
        Me.btnRoleTransfer.Margin = New System.Windows.Forms.Padding(2)
        Me.btnRoleTransfer.Name = "btnRoleTransfer"
        Me.btnRoleTransfer.Size = New System.Drawing.Size(139, 48)
        Me.btnRoleTransfer.TabIndex = 4
        Me.btnRoleTransfer.Text = "Quản lý vai trò"
        Me.btnRoleTransfer.UseVisualStyleBackColor = True
        '
        'btnStockTransfer
        '
        Me.btnStockTransfer.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnStockTransfer.Location = New System.Drawing.Point(0, 94)
        Me.btnStockTransfer.Margin = New System.Windows.Forms.Padding(2)
        Me.btnStockTransfer.Name = "btnStockTransfer"
        Me.btnStockTransfer.Size = New System.Drawing.Size(139, 46)
        Me.btnStockTransfer.TabIndex = 2
        Me.btnStockTransfer.Text = "Quản lý xuất nhập"
        Me.btnStockTransfer.UseVisualStyleBackColor = True
        '
        'btnPhoneTransfer
        '
        Me.btnPhoneTransfer.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnPhoneTransfer.Location = New System.Drawing.Point(0, 48)
        Me.btnPhoneTransfer.Margin = New System.Windows.Forms.Padding(2)
        Me.btnPhoneTransfer.Name = "btnPhoneTransfer"
        Me.btnPhoneTransfer.Size = New System.Drawing.Size(139, 46)
        Me.btnPhoneTransfer.TabIndex = 3
        Me.btnPhoneTransfer.Text = "Quản lý điện thoại"
        Me.btnPhoneTransfer.UseVisualStyleBackColor = True
        '
        'btnHomeTransfer
        '
        Me.btnHomeTransfer.BackColor = System.Drawing.SystemColors.ButtonHighlight
        Me.btnHomeTransfer.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnHomeTransfer.Location = New System.Drawing.Point(0, 0)
        Me.btnHomeTransfer.Margin = New System.Windows.Forms.Padding(2)
        Me.btnHomeTransfer.Name = "btnHomeTransfer"
        Me.btnHomeTransfer.Size = New System.Drawing.Size(139, 48)
        Me.btnHomeTransfer.TabIndex = 0
        Me.btnHomeTransfer.Text = "Trang chủ"
        Me.btnHomeTransfer.UseVisualStyleBackColor = False
        '
        'pnlContent
        '
        Me.pnlContent.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.pnlContent.Dock = System.Windows.Forms.DockStyle.Fill
        Me.pnlContent.Location = New System.Drawing.Point(141, 60)
        Me.pnlContent.Name = "pnlContent"
        Me.pnlContent.Size = New System.Drawing.Size(815, 370)
        Me.pnlContent.TabIndex = 2
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(956, 430)
        Me.Controls.Add(Me.pnlContent)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.pnlHeader)
        Me.Font = New System.Drawing.Font("Segoe UI", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "MainForm"
        Me.Text = "MainForm"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlHeader As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnUserTransfer As Button
    Friend WithEvents btnRoleTransfer As Button
    Friend WithEvents btnStockTransfer As Button
    Friend WithEvents btnPhoneTransfer As Button
    Friend WithEvents btnHomeTransfer As Button
    Friend WithEvents pnlContent As Panel
End Class