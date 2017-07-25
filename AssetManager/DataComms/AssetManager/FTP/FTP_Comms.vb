Imports System.Net
Public Class FTP_Comms

#Region "Fields"

    Private Const EncFTPUserPass As String = "BzPOHPXLdGu9CxaHTAEUCXY4Oa5EVM2B/G7O9En28LQ="
    Private Const strFTPUser As String = "asset_manager"
    Private FTPcreds As NetworkCredential = New NetworkCredential(strFTPUser, DecodePassword(EncFTPUserPass))
    Private intSocketTimeout As Integer = 5000

#End Region

#Region "Methods"

    Public Function Return_FTPRequestStream(strUri As String, Method As String) As IO.Stream
        Dim request As FtpWebRequest = DirectCast(FtpWebRequest.Create(strUri), FtpWebRequest)
        With request
            .Proxy = New WebProxy() 'set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
            .Credentials = FTPcreds
            .Method = Method
            .ReadWriteTimeout = intSocketTimeout
            Return .GetRequestStream
        End With
    End Function

    Public Function Return_FTPResponse(strUri As String, Method As String) As WebResponse
        Dim request As FtpWebRequest = DirectCast(FtpWebRequest.Create(strUri), FtpWebRequest)
        With request
            .Proxy = New WebProxy() 'set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
            .Credentials = FTPcreds
            .Method = Method
            .ReadWriteTimeout = intSocketTimeout
            Return .GetResponse
        End With
    End Function

#End Region

End Class