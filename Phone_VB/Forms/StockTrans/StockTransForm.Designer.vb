<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class StockTransForm
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
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.btnDetail = New System.Windows.Forms.Button()
        Me.txtKeyword = New System.Windows.Forms.TextBox()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnApprove = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.cboQuantity = New System.Windows.Forms.ComboBox()
        Me.btnNextPage = New System.Windows.Forms.Button()
        Me.btnPrevPage = New System.Windows.Forms.Button()
        Me.lblPageInfo = New System.Windows.Forms.TextBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.dgvStockTrans = New System.Windows.Forms.DataGridView()
        Me.Panel1.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.Panel3.SuspendLayout()
        CType(Me.dgvStockTrans, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Controls.Add(Me.btnDetail)
        Me.Panel1.Controls.Add(Me.txtKeyword)
        Me.Panel1.Controls.Add(Me.btnSearch)
        Me.Panel1.Controls.Add(Me.btnDelete)
        Me.Panel1.Controls.Add(Me.btnApprove)
        Me.Panel1.Dock = System.Windows.Forms.DockStyle.Top
        Me.Panel1.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(765, 62)
        Me.Panel1.TabIndex = 0
        '
        'btnDetail
        '
        Me.btnDetail.Location = New System.Drawing.Point(243, 15)
        Me.btnDetail.Name = "btnDetail"
        Me.btnDetail.Size = New System.Drawing.Size(111, 38)
        Me.btnDetail.TabIndex = 4
        Me.btnDetail.Text = "Xem chi tiết"
        Me.btnDetail.UseVisualStyleBackColor = True
        '
        'txtKeyword
        '
        Me.txtKeyword.Font = New System.Drawing.Font("Segoe UI", 11.0!)
        Me.txtKeyword.Location = New System.Drawing.Point(576, 15)
        Me.txtKeyword.Multiline = True
        Me.txtKeyword.Name = "txtKeyword"
        Me.txtKeyword.Size = New System.Drawing.Size(164, 38)
        Me.txtKeyword.TabIndex = 3
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(451, 15)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(113, 38)
        Me.btnSearch.TabIndex = 2
        Me.btnSearch.Text = "Tìm kiếm"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(126, 15)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(111, 38)
        Me.btnDelete.TabIndex = 1
        Me.btnDelete.Text = "Xóa yêu cầu"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnApprove
        '
        Me.btnApprove.Location = New System.Drawing.Point(12, 15)
        Me.btnApprove.Name = "btnApprove"
        Me.btnApprove.Size = New System.Drawing.Size(108, 38)
        Me.btnApprove.TabIndex = 0
        Me.btnApprove.Text = "Duyệt yêu cầu"
        Me.btnApprove.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.cboQuantity)
        Me.Panel2.Controls.Add(Me.btnNextPage)
        Me.Panel2.Controls.Add(Me.btnPrevPage)
        Me.Panel2.Controls.Add(Me.lblPageInfo)
        Me.Panel2.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.Panel2.Location = New System.Drawing.Point(0, 272)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(765, 78)
        Me.Panel2.TabIndex = 1
        '
        'cboQuantity
        '
        Me.cboQuantity.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.cboQuantity.FormattingEnabled = True
        Me.cboQuantity.Location = New System.Drawing.Point(12, 33)
        Me.cboQuantity.Name = "cboQuantity"
        Me.cboQuantity.Size = New System.Drawing.Size(107, 25)
        Me.cboQuantity.TabIndex = 3
        '
        'btnNextPage
        '
        Me.btnNextPage.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnNextPage.Location = New System.Drawing.Point(647, 21)
        Me.btnNextPage.Name = "btnNextPage"
        Me.btnNextPage.Size = New System.Drawing.Size(102, 37)
        Me.btnNextPage.TabIndex = 2
        Me.btnNextPage.Text = "Sau"
        Me.btnNextPage.UseVisualStyleBackColor = True
        '
        'btnPrevPage
        '
        Me.btnPrevPage.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.btnPrevPage.Location = New System.Drawing.Point(539, 21)
        Me.btnPrevPage.Name = "btnPrevPage"
        Me.btnPrevPage.Size = New System.Drawing.Size(102, 37)
        Me.btnPrevPage.TabIndex = 1
        Me.btnPrevPage.Text = "Trước"
        Me.btnPrevPage.UseVisualStyleBackColor = True
        '
        'lblPageInfo
        '
        Me.lblPageInfo.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.lblPageInfo.Location = New System.Drawing.Point(243, 33)
        Me.lblPageInfo.Name = "lblPageInfo"
        Me.lblPageInfo.Size = New System.Drawing.Size(220, 25)
        Me.lblPageInfo.TabIndex = 0
        '
        'Panel3
        '
        Me.Panel3.Controls.Add(Me.dgvStockTrans)
        Me.Panel3.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel3.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.Panel3.Location = New System.Drawing.Point(0, 62)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(765, 210)
        Me.Panel3.TabIndex = 2
        '
        'dgvStockTrans
        '
        Me.dgvStockTrans.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvStockTrans.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders
        Me.dgvStockTrans.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.dgvStockTrans.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvStockTrans.Dock = System.Windows.Forms.DockStyle.Fill
        Me.dgvStockTrans.Location = New System.Drawing.Point(0, 0)
        Me.dgvStockTrans.Name = "dgvStockTrans"
        Me.dgvStockTrans.ReadOnly = True
        Me.dgvStockTrans.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvStockTrans.Size = New System.Drawing.Size(765, 210)
        Me.dgvStockTrans.TabIndex = 0
        '
        'StockTransForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(765, 350)
        Me.Controls.Add(Me.Panel3)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.Panel1)
        Me.Name = "StockTransForm"
        Me.Text = "StockForm"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        CType(Me.dgvStockTrans, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents Panel1 As Panel
    Friend WithEvents txtKeyword As TextBox
    Friend WithEvents btnSearch As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnApprove As Button
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Panel3 As Panel
    Friend WithEvents lblPageInfo As TextBox
    Friend WithEvents dgvStockTrans As DataGridView
    Friend WithEvents cboQuantity As ComboBox
    Friend WithEvents btnNextPage As Button
    Friend WithEvents btnPrevPage As Button
    Friend WithEvents btnDetail As Button
End Class
