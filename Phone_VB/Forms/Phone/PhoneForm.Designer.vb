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
        Me.Button1 = New System.Windows.Forms.Button()
        Me.Button2 = New System.Windows.Forms.Button()
        CType(Me.dgvPhones, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'btnCreate
        '
        Me.btnCreate.BackColor = System.Drawing.Color.Olive
        Me.btnCreate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnCreate.Location = New System.Drawing.Point(12, 24)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(97, 37)
        Me.btnCreate.TabIndex = 0
        Me.btnCreate.Text = "Thêm mới"
        Me.btnCreate.UseVisualStyleBackColor = False
        '
        'btnSearch
        '
        Me.btnSearch.BackColor = System.Drawing.Color.DodgerBlue
        Me.btnSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnSearch.Location = New System.Drawing.Point(466, 26)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(94, 36)
        Me.btnSearch.TabIndex = 1
        Me.btnSearch.Text = "Tìm kiếm"
        Me.btnSearch.UseVisualStyleBackColor = False
        '
        'dgvPhones
        '
        Me.dgvPhones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPhones.AutoSizeRowsMode = System.Windows.Forms.DataGridViewAutoSizeRowsMode.AllHeaders
        Me.dgvPhones.BackgroundColor = System.Drawing.Color.Gainsboro
        Me.dgvPhones.ColumnHeadersHeight = 29
        Me.dgvPhones.Location = New System.Drawing.Point(14, 77)
        Me.dgvPhones.MultiSelect = False
        Me.dgvPhones.Name = "dgvPhones"
        Me.dgvPhones.ReadOnly = True
        Me.dgvPhones.RowHeadersWidth = 51
        Me.dgvPhones.RowTemplate.Height = 32
        Me.dgvPhones.RowTemplate.ReadOnly = True
        Me.dgvPhones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPhones.Size = New System.Drawing.Size(760, 229)
        Me.dgvPhones.TabIndex = 33
        '
        'btnUpdate
        '
        Me.btnUpdate.BackColor = System.Drawing.Color.Orange
        Me.btnUpdate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnUpdate.Location = New System.Drawing.Point(211, 24)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(94, 37)
        Me.btnUpdate.TabIndex = 4
        Me.btnUpdate.Text = "Cập nhật"
        Me.btnUpdate.UseVisualStyleBackColor = False
        '
        'btnDelete
        '
        Me.btnDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnDelete.Location = New System.Drawing.Point(311, 26)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(95, 35)
        Me.btnDelete.TabIndex = 5
        Me.btnDelete.Text = "Xóa"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnPrevPage
        '
        Me.btnPrevPage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnPrevPage.Location = New System.Drawing.Point(584, 349)
        Me.btnPrevPage.Name = "btnPrevPage"
        Me.btnPrevPage.Size = New System.Drawing.Size(94, 38)
        Me.btnPrevPage.TabIndex = 6
        Me.btnPrevPage.Text = "Trước"
        Me.btnPrevPage.UseVisualStyleBackColor = True
        '
        'btnNextPage
        '
        Me.btnNextPage.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnNextPage.Location = New System.Drawing.Point(684, 349)
        Me.btnNextPage.Name = "btnNextPage"
        Me.btnNextPage.Size = New System.Drawing.Size(90, 37)
        Me.btnNextPage.TabIndex = 7
        Me.btnNextPage.Text = "Sau"
        Me.btnNextPage.UseVisualStyleBackColor = True
        '
        'cboQuantity
        '
        Me.cboQuantity.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.cboQuantity.ForeColor = System.Drawing.SystemColors.Highlight
        Me.cboQuantity.FormattingEnabled = True
        Me.cboQuantity.Location = New System.Drawing.Point(632, 318)
        Me.cboQuantity.Name = "cboQuantity"
        Me.cboQuantity.Size = New System.Drawing.Size(85, 25)
        Me.cboQuantity.TabIndex = 10
        '
        'btnDetail
        '
        Me.btnDetail.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.btnDetail.Location = New System.Drawing.Point(115, 24)
        Me.btnDetail.Name = "btnDetail"
        Me.btnDetail.Size = New System.Drawing.Size(90, 37)
        Me.btnDetail.TabIndex = 11
        Me.btnDetail.Text = "Xem chi tiết"
        Me.btnDetail.UseVisualStyleBackColor = True
        '
        'txtKeyword
        '
        Me.txtKeyword.Font = New System.Drawing.Font("Segoe UI", 10.0!)
        Me.txtKeyword.Location = New System.Drawing.Point(594, 27)
        Me.txtKeyword.Multiline = True
        Me.txtKeyword.Name = "txtKeyword"
        Me.txtKeyword.Size = New System.Drawing.Size(180, 35)
        Me.txtKeyword.TabIndex = 12
        '
        'lblPageInfo
        '
        Me.lblPageInfo.Enabled = False
        Me.lblPageInfo.Font = New System.Drawing.Font("Microsoft Sans Serif", 10.0!)
        Me.lblPageInfo.Location = New System.Drawing.Point(12, 362)
        Me.lblPageInfo.Multiline = True
        Me.lblPageInfo.Name = "lblPageInfo"
        Me.lblPageInfo.RightToLeft = System.Windows.Forms.RightToLeft.No
        Me.lblPageInfo.Size = New System.Drawing.Size(220, 26)
        Me.lblPageInfo.TabIndex = 13
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.Button1.Location = New System.Drawing.Point(115, 319)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(97, 37)
        Me.Button1.TabIndex = 34
        Me.Button1.Text = "Xuất kho"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'Button2
        '
        Me.Button2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!)
        Me.Button2.Location = New System.Drawing.Point(12, 318)
        Me.Button2.Name = "Button2"
        Me.Button2.Size = New System.Drawing.Size(94, 38)
        Me.Button2.TabIndex = 35
        Me.Button2.Text = "Nhập kho"
        Me.Button2.UseVisualStyleBackColor = True
        '
        'PhoneForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.ClientSize = New System.Drawing.Size(786, 422)
        Me.Controls.Add(Me.Button2)
        Me.Controls.Add(Me.Button1)
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
    Friend WithEvents Button1 As Button
    Friend WithEvents Button2 As Button
End Class
