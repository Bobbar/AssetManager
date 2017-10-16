Imports System.Net

Public Class FtpComms

#Region "Fields"

    Private Const EncFTPUserPass As String = "BzPOHPXLdGu9CxaHTAEUCXY4Oa5EVM2B/G7O9En28LQ="
    Private Const strFTPUser As String = "asset_manager"
    Private FTPcreds As NetworkCredential = New NetworkCredential(strFTPUser, SecurityTools.DecodePassword(EncFTPUserPass))
    Private intSocketTimeout As Integer = 5000

#End Region

#Region "Methods"

    Public Function ReturnFtpRequestStream(uri As String, method As String) As IO.Stream
        Dim request As FtpWebRequest = DirectCast(FtpWebRequest.Create(uri), FtpWebRequest)
        With request
            .Proxy = New WebProxy() 'set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
            .Credentials = FTPcreds
            .Method = method
            .ReadWriteTimeout = intSocketTimeout
            Return .GetRequestStream
        End With
    End Function

    Public Function ReturnFtpResponse(uri As String, method As String) As WebResponse
        Dim request As FtpWebRequest = DirectCast(FtpWebRequest.Create(uri), FtpWebRequest)
        With request
            .Proxy = New WebProxy() 'set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
            .Credentials = FTPcreds
            .Method = method
            .ReadWriteTimeout = intSocketTimeout
            Return .GetResponse
        End With
    End Function

#End Region

End Class