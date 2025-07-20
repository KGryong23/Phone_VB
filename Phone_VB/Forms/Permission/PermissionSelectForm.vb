Public Class PermissionSelectForm
    Private ReadOnly _roleId As Integer
    Private WithEvents bgwLoad As New System.ComponentModel.BackgroundWorker()
    Private permissions As List(Of PermissionDto)
    Public ReadOnly Property SelectedPermissionIds As List(Of Integer)

    Public Sub New(roleId As Integer)
        InitializeComponent()
        _roleId = roleId
        SelectedPermissionIds = New List(Of Integer)()
        bgwLoad.WorkerReportsProgress = False
        bgwLoad.WorkerSupportsCancellation = False
    End Sub

    Private Sub PermissionSelectForm_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        btnOK.Enabled = False
        btnCancel.Enabled = False
        dgvPermissions.DataSource = Nothing
        dgvPermissions.Enabled = False
        bgwLoad.RunWorkerAsync()
    End Sub

    Private Sub bgwLoad_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles bgwLoad.DoWork
        ' Lấy danh sách quyền chưa gán cho role
        permissions = ServiceRegistry.PermissionService.GetUnassignedPermissions(_roleId)
    End Sub

    Private Sub bgwLoad_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgwLoad.RunWorkerCompleted
        dgvPermissions.Enabled = True
        btnOK.Enabled = True
        btnCancel.Enabled = True

        If permissions Is Nothing OrElse permissions.Count = 0 Then
            MessageBox.Show("Không có quyền nào để chọn!", "Thông báo", MessageBoxButtons.OK, MessageBoxIcon.Information)
            Return
        End If

        dgvPermissions.AutoGenerateColumns = False
        dgvPermissions.DataSource = permissions

        If dgvPermissions.Columns.Count = 1 Then ' Chỉ có cột checkbox
            Dim colName As New DataGridViewTextBoxColumn()
            colName.DataPropertyName = "Name"
            colName.HeaderText = "Tên quyền"
            colName.Name = "Name"
            colName.Width = 200
            dgvPermissions.Columns.Add(colName)

            Dim colDesc As New DataGridViewTextBoxColumn()
            colDesc.DataPropertyName = "Description"
            colDesc.HeaderText = "Mô tả"
            colDesc.Name = "Description"
            colDesc.Width = 300
            dgvPermissions.Columns.Add(colDesc)
        End If
    End Sub

    Private Sub btnOK_Click(sender As Object, e As EventArgs) Handles btnOK.Click
        SelectedPermissionIds.Clear()
        For Each row As DataGridViewRow In dgvPermissions.Rows
            Dim isChecked As Boolean = False
            If row.Cells("colSelect").Value IsNot Nothing Then
                isChecked = Convert.ToBoolean(row.Cells("colSelect").Value)
            End If
            If isChecked Then
                Dim perm As PermissionDto = TryCast(row.DataBoundItem, PermissionDto)
                If perm IsNot Nothing Then
                    SelectedPermissionIds.Add(perm.Id)
                End If
            End If
        Next
        Me.DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Me.DialogResult = DialogResult.Cancel
        Me.Close()
    End Sub
End Class