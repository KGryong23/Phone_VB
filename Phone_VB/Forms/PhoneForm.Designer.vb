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
        Me.btnCreate.Location = New System.Drawing.Point(12, 77)
        Me.btnCreate.Name = "btnCreate"
        Me.btnCreate.Size = New System.Drawing.Size(97, 35)
        Me.btnCreate.TabIndex = 0
        Me.btnCreate.Text = "Thêm mới"
        Me.btnCreate.UseVisualStyleBackColor = True
        '
        'btnSearch
        '
        Me.btnSearch.Location = New System.Drawing.Point(497, 77)
        Me.btnSearch.Name = "btnSearch"
        Me.btnSearch.Size = New System.Drawing.Size(94, 35)
        Me.btnSearch.TabIndex = 1
        Me.btnSearch.Text = "Tìm kiếm"
        Me.btnSearch.UseVisualStyleBackColor = True
        '
        'dgvPhones
        '
        Me.dgvPhones.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
        Me.dgvPhones.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvPhones.Location = New System.Drawing.Point(12, 129)
        Me.dgvPhones.MultiSelect = False
        Me.dgvPhones.Name = "dgvPhones"
        Me.dgvPhones.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.dgvPhones.Size = New System.Drawing.Size(776, 229)
        Me.dgvPhones.TabIndex = 3
        '
        'btnUpdate
        '
        Me.btnUpdate.Location = New System.Drawing.Point(126, 77)
        Me.btnUpdate.Name = "btnUpdate"
        Me.btnUpdate.Size = New System.Drawing.Size(97, 35)
        Me.btnUpdate.TabIndex = 4
        Me.btnUpdate.Text = "Cập nhật"
        Me.btnUpdate.UseVisualStyleBackColor = True
        '
        'btnDelete
        '
        Me.btnDelete.Location = New System.Drawing.Point(242, 77)
        Me.btnDelete.Name = "btnDelete"
        Me.btnDelete.Size = New System.Drawing.Size(78, 35)
        Me.btnDelete.TabIndex = 5
        Me.btnDelete.Text = "Xóa"
        Me.btnDelete.UseVisualStyleBackColor = True
        '
        'btnPrevPage
        '
        Me.btnPrevPage.Location = New System.Drawing.Point(589, 390)
        Me.btnPrevPage.Name = "btnPrevPage"
        Me.btnPrevPage.Size = New System.Drawing.Size(94, 33)
        Me.btnPrevPage.TabIndex = 6
        Me.btnPrevPage.Text = "Trước"
        Me.btnPrevPage.UseVisualStyleBackColor = True
        '
        'btnNextPage
        '
        Me.btnNextPage.Location = New System.Drawing.Point(698, 390)
        Me.btnNextPage.Name = "btnNextPage"
        Me.btnNextPage.Size = New System.Drawing.Size(90, 33)
        Me.btnNextPage.TabIndex = 7
        Me.btnNextPage.Text = "Sau"
        Me.btnNextPage.UseVisualStyleBackColor = True
        '
        'cboQuantity
        '
        Me.cboQuantity.FormattingEnabled = True
        Me.cboQuantity.Location = New System.Drawing.Point(12, 397)
        Me.cboQuantity.Name = "cboQuantity"
        Me.cboQuantity.Size = New System.Drawing.Size(78, 21)
        Me.cboQuantity.TabIndex = 10
        '
        'btnDetail
        '
        Me.btnDetail.Location = New System.Drawing.Point(336, 77)
        Me.btnDetail.Name = "btnDetail"
        Me.btnDetail.Size = New System.Drawing.Size(85, 35)
        Me.btnDetail.TabIndex = 11
        Me.btnDetail.Text = "Xem chi tiết"
        Me.btnDetail.UseVisualStyleBackColor = True
        '
        'txtKeyword
        '
        Me.txtKeyword.Location = New System.Drawing.Point(608, 77)
        Me.txtKeyword.Multiline = True
        Me.txtKeyword.Name = "txtKeyword"
        Me.txtKeyword.Size = New System.Drawing.Size(180, 35)
        Me.txtKeyword.TabIndex = 12
        '
        'lblPageInfo
        '
        Me.lblPageInfo.Location = New System.Drawing.Point(230, 390)
        Me.lblPageInfo.Multiline = True
        Me.lblPageInfo.Name = "lblPageInfo"
        Me.lblPageInfo.Size = New System.Drawing.Size(271, 26)
        Me.lblPageInfo.TabIndex = 13
        '
        'PhoneForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
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
        Me.Text = "Form1"
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
