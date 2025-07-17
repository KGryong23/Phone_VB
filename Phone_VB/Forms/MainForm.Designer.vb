<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MainForm
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
        Me.pnlHeader = New System.Windows.Forms.Panel()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.SplitContainer1 = New System.Windows.Forms.SplitContainer()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnUserTransfer = New System.Windows.Forms.Button()
        Me.btnRoleTransfer = New System.Windows.Forms.Button()
        Me.btnStockTransfer = New System.Windows.Forms.Button()
        Me.btnPhoneTransfer = New System.Windows.Forms.Button()
        Me.btnHomeTransfer = New System.Windows.Forms.Button()
        Me.pnlHeader.SuspendLayout()
        Me.SplitContainer1.Panel1.SuspendLayout()
        Me.SplitContainer1.SuspendLayout()
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
        Me.pnlHeader.Size = New System.Drawing.Size(939, 60)
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
        'SplitContainer1
        '
        Me.SplitContainer1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.SplitContainer1.Location = New System.Drawing.Point(0, 60)
        Me.SplitContainer1.Name = "SplitContainer1"
        '
        'SplitContainer1.Panel1
        '
        Me.SplitContainer1.Panel1.Controls.Add(Me.Panel1)
        '
        'SplitContainer1.Panel2
        '
        Me.SplitContainer1.Panel2.Padding = New System.Windows.Forms.Padding(2)
        Me.SplitContainer1.Size = New System.Drawing.Size(939, 370)
        Me.SplitContainer1.SplitterDistance = 157
        Me.SplitContainer1.TabIndex = 2
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
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Margin = New System.Windows.Forms.Padding(2)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(155, 370)
        Me.Panel1.TabIndex = 2
        '
        'btnUserTransfer
        '
        Me.btnUserTransfer.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnUserTransfer.Location = New System.Drawing.Point(0, 212)
        Me.btnUserTransfer.Margin = New System.Windows.Forms.Padding(2)
        Me.btnUserTransfer.Name = "btnUserTransfer"
        Me.btnUserTransfer.Size = New System.Drawing.Size(153, 53)
        Me.btnUserTransfer.TabIndex = 3
        Me.btnUserTransfer.Text = "Quản lý người dùng"
        Me.btnUserTransfer.UseVisualStyleBackColor = True
        '
        'btnRoleTransfer
        '
        Me.btnRoleTransfer.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnRoleTransfer.Location = New System.Drawing.Point(0, 159)
        Me.btnRoleTransfer.Margin = New System.Windows.Forms.Padding(2)
        Me.btnRoleTransfer.Name = "btnRoleTransfer"
        Me.btnRoleTransfer.Size = New System.Drawing.Size(153, 53)
        Me.btnRoleTransfer.TabIndex = 4
        Me.btnRoleTransfer.Text = "Quản lý vai trò"
        Me.btnRoleTransfer.UseVisualStyleBackColor = True
        '
        'btnStockTransfer
        '
        Me.btnStockTransfer.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnStockTransfer.Location = New System.Drawing.Point(0, 106)
        Me.btnStockTransfer.Margin = New System.Windows.Forms.Padding(2)
        Me.btnStockTransfer.Name = "btnStockTransfer"
        Me.btnStockTransfer.Size = New System.Drawing.Size(153, 53)
        Me.btnStockTransfer.TabIndex = 2
        Me.btnStockTransfer.Text = "Quản lý xuất nhập"
        Me.btnStockTransfer.UseVisualStyleBackColor = True
        '
        'btnPhoneTransfer
        '
        Me.btnPhoneTransfer.Dock = System.Windows.Forms.DockStyle.Top
        Me.btnPhoneTransfer.Location = New System.Drawing.Point(0, 53)
        Me.btnPhoneTransfer.Margin = New System.Windows.Forms.Padding(2)
        Me.btnPhoneTransfer.Name = "btnPhoneTransfer"
        Me.btnPhoneTransfer.Size = New System.Drawing.Size(153, 53)
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
        Me.btnHomeTransfer.Size = New System.Drawing.Size(153, 53)
        Me.btnHomeTransfer.TabIndex = 0
        Me.btnHomeTransfer.Text = "Trang chủ"
        Me.btnHomeTransfer.UseVisualStyleBackColor = False
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(939, 430)
        Me.Controls.Add(Me.SplitContainer1)
        Me.Controls.Add(Me.pnlHeader)
        Me.Font = New System.Drawing.Font("Segoe UI", 7.8!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Margin = New System.Windows.Forms.Padding(2)
        Me.Name = "MainForm"
        Me.Text = "MainForm"
        Me.pnlHeader.ResumeLayout(False)
        Me.pnlHeader.PerformLayout()
        Me.SplitContainer1.Panel1.ResumeLayout(False)
        Me.SplitContainer1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pnlHeader As Panel
    Friend WithEvents Label1 As Label
    Friend WithEvents SplitContainer1 As SplitContainer
    Friend WithEvents Panel1 As Panel
    Friend WithEvents btnUserTransfer As Button
    Friend WithEvents btnRoleTransfer As Button
    Friend WithEvents btnStockTransfer As Button
    Friend WithEvents btnPhoneTransfer As Button
    Friend WithEvents btnHomeTransfer As Button
End Class
