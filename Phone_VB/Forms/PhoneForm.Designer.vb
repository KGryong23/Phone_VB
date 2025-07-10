<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class PhoneForm
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
        Me.btnCreate = New System.Windows.Forms.Button()
        Me.btnSearch = New System.Windows.Forms.Button()
        Me.dgvPhones = New System.Windows.Forms.DataGridView()
        Me.btnUpdate = New System.Windows.Forms.Button()
        Me.btnDelete = New System.Windows.Forms.Button()
        Me.btnPrevPage = New System.Windows.Forms.Button()
        Me.btnNextPage = New System.Windows.Forms.Button()
        Me.cboQuantity = New System.Windows.Forms.ComboBox()
        Me.btnDetail = New System.Windows.Forms.Button()
        Me.txtKeyword = New System.Windows.Forms.TextBox()
        Me.lblPageInfo = New System.Windows.Forms.TextBox()
        CType(Me.dgvPhones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCreate
        '
        Me.btnCreate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnCreate.Location = New System.Drawing.Point(414, 95)
        Me.btnCreate.Margin = New System.Windows.Forms.Padding(4)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(129, 42)
        Me.btnCreate.TabIndex = 0
        Me.btnCreate.Text = "Thêm mới"
        Me.btnCreate.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnSearch.Location = New System.Drawing.Point(663, 95)
        Me.btnSearch.Margin = New System.Windows.Forms.Padding(4)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(125, 43)
        Me.btnSearch.TabIndex = 1
        Me.btnSearch.Text = "Tìm kiếm"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'dgvPhones
        '
        Me.dgvPhones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPhones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPhones.Location = New System.Drawing.Point(16, 159)
        Me.dgvPhones.Margin = New System.Windows.Forms.Padding(4)
        Me.dgvPhones.MultiSelect = False
        Me.dgvPhones.Name = "dgvPhones"
        Me.dgvPhones.RowHeadersWidth = 51
        Me.dgvPhones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPhones.Size = New System.Drawing.Size(1035, 282)
        Me.dgvPhones.TabIndex = 3
        '
        'btnUpdate
        '
        Me.btnUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnUpdate.Location = New System.Drawing.Point(144, 95)
        Me.btnUpdate.Margin = New System.Windows.Forms.Padding(4)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(129, 43)
        Me.btnUpdate.TabIndex = 4
        Me.btnUpdate.Text = "Cập nhật"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnDelete.Location = New System.Drawing.Point(293, 95)
        Me.btnDelete.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(104, 43)
        Me.btnDelete.TabIndex = 5
        Me.btnDelete.Text = "Xóa"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnPrevPage
        '
        Me.btnPrevPage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnPrevPage.Location = New System.Drawing.Point(785, 480)
        Me.btnPrevPage.Margin = New System.Windows.Forms.Padding(4)
        Me.btnPrevPage.Name = "btnPrevPage"
        Me.btnPrevPage.Size = New System.Drawing.Size(125, 41)
        Me.btnPrevPage.TabIndex = 6
        Me.btnPrevPage.Text = "Trước"
        Me.btnPrevPage.UseVisualStyleBackColor = True
        '
        'btnNextPage
        '
        Me.btnNextPage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnNextPage.Location = New System.Drawing.Point(931, 480)
        Me.btnNextPage.Margin = New System.Windows.Forms.Padding(4)
        Me.btnNextPage.Name = "btnNextPage"
        Me.btnNextPage.Size = New System.Drawing.Size(120, 41)
        Me.btnNextPage.TabIndex = 7
        Me.btnNextPage.Text = "Sau"
        Me.btnNextPage.UseVisualStyleBackColor = True
        '
        'cboQuantity
        '
        Me.cboQuantity.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.5!)
        Me.cboQuantity.FormattingEnabled = True
        Me.cboQuantity.Location = New System.Drawing.Point(16, 489)
        Me.cboQuantity.Margin = New System.Windows.Forms.Padding(4)
        Me.cboQuantity.Name = "cboQuantity"
        Me.cboQuantity.Size = New System.Drawing.Size(103, 25)
        Me.cboQuantity.TabIndex = 10
        '
        'btnDetail
        '
        Me.btnDetail.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnDetail.Location = New System.Drawing.Point(13, 95)
        Me.btnDetail.Margin = New System.Windows.Forms.Padding(4)
        Me.btnDetail.Name = "btnDetail"
        Me.btnDetail.Size = New System.Drawing.Size(113, 43)
        Me.btnDetail.TabIndex = 11
        Me.btnDetail.Text = "Xem chi tiết"
        Me.btnDetail.UseVisualStyleBackColor = True
        '
        'txtKeyword
        '
        Me.txtKeyword.Location = New System.Drawing.Point(811, 95)
        Me.txtKeyword.Margin = New System.Windows.Forms.Padding(4)
        Me.txtKeyword.Multiline = True
        Me.txtKeyword.Name = "txtKeyword"
        Me.txtKeyword.Size = New System.Drawing.Size(239, 42)
        Me.txtKeyword.TabIndex = 12
        '
        'lblPageInfo
        '
        Me.lblPageInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.lblPageInfo.Location = New System.Drawing.Point(308, 482)
        Me.lblPageInfo.Margin = New System.Windows.Forms.Padding(4)
        Me.lblPageInfo.Multiline = True
        Me.lblPageInfo.Name = "lblPageInfo"
        Me.lblPageInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPageInfo.Size = New System.Drawing.Size(360, 31)
        Me.lblPageInfo.TabIndex = 13
        Me.lblPageInfo.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'PhoneForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(8.0!, 16.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1067, 554)
        Me.Controls.Add(Me.lblPageInfo)
        Me.Controls.Add(Me.txtKeyword)
        Me.Controls.Add(Me.btnDetail)
        Me.Controls.Add(Me.cboQuantity)
        Me.Controls.Add(Me.btnNextPage)
        Me.Controls.Add(Me.btnPrevPage)
        Me.Controls.Add(Me.btnDelete)
        Me.Controls.Add(Me.btnUpdate)
        Me.Controls.Add(Me.dgvPhones)
        Me.Controls.Add(Me.btnSearch)
        Me.Controls.Add(Me.btnCreate)
        Me.Margin = New System.Windows.Forms.Padding(4)
        Me.Name = "PhoneForm"
        Me.Text = "Quản lý điện thoại"
        CType(Me.dgvPhones, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents btnCreate As Button
    Friend WithEvents btnSearch As Button
    Friend WithEvents dgvPhones As DataGridView
    Friend WithEvents btnUpdate As Button
    Friend WithEvents btnDelete As Button
    Friend WithEvents btnPrevPage As Button
    Friend WithEvents btnNextPage As Button
    Friend WithEvents cboQuantity As ComboBox
    Friend WithEvents btnDetail As Button
    Friend WithEvents txtKeyword As TextBox
    Friend WithEvents lblPageInfo As TextBox
End Class
