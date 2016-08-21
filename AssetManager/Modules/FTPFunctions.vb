﻿Module FTPFunctions
    Public Const strFTPUser As String = "asset_manager"
    Public strFTPPass As String = DecodePassword(EncFTPUserPass)
    Public FTPcreds As Net.NetworkCredential = New Net.NetworkCredential(strFTPUser, strFTPPass)
    Private intSocketTimeout As Integer = 30000 'timeout for FTP comms in MS
    Public Function DeleteFTPAttachment(AttachUID As String, DeviceUID As String) As Boolean
        Dim resp As Net.FtpWebResponse = Nothing
        Try
            resp = Return_FTPResponse("ftp://" & strServerIP & "/attachments/" & DeviceUID & "/" & AttachUID, Net.WebRequestMethods.Ftp.DeleteFile)
            If resp.StatusCode = Net.FtpStatusCode.FileActionOK Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Function
    Public Function DeleteFTPFolder(DeviceUID As String, Type As String) As Boolean
        Dim resp As Net.FtpWebResponse = Nothing
        Dim files As List(Of String)
        Try
            resp = Return_FTPResponse("ftp://" & strServerIP & "/attachments/" & DeviceUID & "/", Net.WebRequestMethods.Ftp.ListDirectory)
            Dim responseStream As System.IO.Stream = resp.GetResponseStream
            files = New List(Of String)
            Dim reader As IO.StreamReader = New IO.StreamReader(responseStream)
            While Not reader.EndOfStream 'collect list of files in directory
                files.Add(reader.ReadLine)
            End While
            reader.Close()
            responseStream.Dispose()
            Dim i As Integer = 0
            For Each file In files  'delete each file counting for successes
                i += MySQLDB.DeleteAttachment(file, Type)
            Next
            If files.Count = i Then ' if successful deletetions = total # of files, delete the directory
                resp = Return_FTPResponse("ftp://" & strServerIP & "/attachments/" & DeviceUID, Net.WebRequestMethods.Ftp.RemoveDirectory)
            End If
            If resp.StatusCode = Net.FtpStatusCode.FileActionOK Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
    End Function
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
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
End Module
