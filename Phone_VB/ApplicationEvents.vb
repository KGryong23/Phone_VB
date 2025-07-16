Imports System.Configuration
Imports System.Data.Odbc

Namespace My
    ' The following events are available for MyApplication:
    ' Startup: Raised when the application starts, before the startup form is created.
    ' Shutdown: Raised after all application forms are closed. This event is not raised if the application terminates abnormally.
    ' UnhandledException: Raised if the application encounters an unhandled exception.
    ' StartupNextInstance: Raised when launching a single-instance application and the application is already active. 
    ' NetworkAvailabilityChanged: Raised when the network connection is connected or disconnected.
    Partial Friend Class MyApplication
        Private Sub MyApplication_Startup(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup

            ' Kiểm tra kết nối cơ sở dữ liệu
            If TestConnection() Then
                ServiceRegistry.InitializeServices()
                ' Khởi tạo các service
            Else
                ' Hủy khởi động nếu kết nối thất bại
                e.Cancel = True
            End If
        End Sub

        ' Hàm kiểm tra kết nối cơ sở dữ liệu
        Private Function TestConnection() As Boolean
            Try
                Dim connStr As String = ConfigurationManager.ConnectionStrings("PhoneDbConnection")?.ConnectionString
                If connStr Is Nothing Then
                    MessageBox.Show("Connection string 'PhoneDbConnection' not found in App.config", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Return False
                End If
                Using conn As New OdbcConnection(connStr)
                    conn.Open()
                    Debug.WriteLine("Database connection successful at " & DateTime.Now.ToString())
                    Return True
                End Using
            Catch ex As Exception
                MessageBox.Show("Database connection failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
        End Function
    End Class
End Namespace