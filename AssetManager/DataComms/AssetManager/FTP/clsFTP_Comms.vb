Public Class clsFTP_Comms
    Private Const strFTPUser As String = "asset_manager"
    Private Const EncFTPUserPass As String = "BzPOHPXLdGu9CxaHTAEUCXY4Oa5EVM2B/G7O9En28LQ="
    Private strFTPPass As String = DecodePassword(EncFTPUserPass)
    Private FTPcreds As Net.NetworkCredential = New Net.NetworkCredential(strFTPUser, strFTPPass)
    Private intSocketTimeout As Integer = 30000 'timeout for FTP comms in MS
    Public Function Return_FTPResponse(strUri As String, Method As String) As Net.WebResponse
        Dim request As Net.FtpWebRequest = Net.FtpWebRequest.Create(strUri)
        Try
            With request
                '.KeepAlive = True
                .Proxy = New Net.WebProxy() 'set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
                .Credentials = FTPcreds
                .Method = Method
                .ReadWriteTimeout = intSocketTimeout
                Return .GetResponse
            End With
        Catch ex As Exception
            Return request.GetResponse
        End Try
    End Function
    Public Function Return_FTPRequestStream(strUri As String, Method As String) As IO.Stream
        Try
            Dim request As Net.FtpWebRequest = Net.FtpWebRequest.Create(strUri)
            With request
                '.KeepAlive = True
                .Proxy = New Net.WebProxy() 'set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
                .Credentials = FTPcreds
                .Method = Method
                .ReadWriteTimeout = intSocketTimeout
                Return .GetRequestStream
            End With
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
End Class
