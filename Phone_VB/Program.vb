Imports System.Configuration
Imports System.Data.Odbc

Module Program
    ' Đọc chuỗi kết nối từ App.config
    Private ReadOnly connStr As String = ConfigurationManager.ConnectionStrings("PhoneDbConnection").ConnectionString

    ' Hàm kiểm tra kết nối cơ sở dữ liệu
    Private Function TestConnection() As Boolean
        Try
            Using conn As New OdbcConnection(connStr)
                conn.Open()
                Return True
            End Using
        Catch ex As Exception
            MessageBox.Show("Database connection failed: " & ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return False
        End Try
    End Function

    ' Hàm Main - Điểm bắt đầu của ứng dụng
    <STAThread>
    Sub Main()
        Application.EnableVisualStyles()
        Application.SetCompatibleTextRenderingDefault(False)

        ' Kiểm tra kết nối cơ sở dữ liệu trước khi chạy ứng dụng
        If TestConnection() Then
            ' Nếu kết nối thành công, chạy form chính
            Application.Run(New PhoneForm())
        Else
            ' Nếu kết nối thất bại, thoát ứng dụng
            Application.Exit()
        End If
    End Sub
End Module