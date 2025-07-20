Imports System.Configuration
Imports System.Data.Odbc

Public Module PermissionSyncService
    Public Sub SyncPermissions()
        Dim connStr As String = ConfigurationManager.ConnectionStrings("PhoneDbConnection").ConnectionString
        Using conn As New OdbcConnection(connStr)
            conn.Open()
            For Each perm In [Enum].GetValues(GetType(PermissionEnum))
                Dim name As String = perm.ToString()
                Dim desc As String = PermissionDesc(CType(perm, PermissionEnum))
                Dim checkQuery As String = "SELECT COUNT(*) FROM permissions WHERE name = ?"
                Using checkCmd As New OdbcCommand(checkQuery, conn)
                    checkCmd.Parameters.AddWithValue("name", name)
                    Dim exists As Integer = Convert.ToInt32(checkCmd.ExecuteScalar())
                    If exists = 0 Then
                        Dim insertQuery As String = "INSERT INTO permissions (name, description) VALUES (?, ?)"
                        Using insertCmd As New OdbcCommand(insertQuery, conn)
                            insertCmd.Parameters.AddWithValue("name", name)
                            insertCmd.Parameters.AddWithValue("description", desc)
                            insertCmd.ExecuteNonQuery()
                        End Using
                    End If
                End Using
            Next
        End Using
    End Sub
End Module