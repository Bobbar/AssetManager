Module FTPFunctions
    Public FTPcreds As Net.NetworkCredential = New Net.NetworkCredential(strFTPUser, strFTPPass)
    Public Function DeleteFTPAttachment(AttachUID As String, DeviceUID As String) As Object


        Dim resp As Net.FtpWebResponse = Nothing
        Dim request As Net.FtpWebRequest = Net.FtpWebRequest.Create("ftp://" & strServerIP & "/attachments/" & DeviceUID & "/" & AttachUID)
        request.Proxy = New Net.WebProxy() 'set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
        request.Credentials = FTPcreds
        request.Method = Net.WebRequestMethods.Ftp.DeleteFile


        resp = request.GetResponse

        Return resp.StatusCode
    End Function
    Public Function ReturnFTPResponse(strUri As String, Method As String) As Object
        Dim request As Net.FtpWebRequest = Net.FtpWebRequest.Create(strUri)
        request.Proxy = New Net.WebProxy() 'set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.

        request.Credentials = FTPcreds
        request.Method = Method

        Return request.GetResponse
    End Function
End Module
