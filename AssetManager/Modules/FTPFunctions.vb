Module FTPFunctions
    Public Const strFTPUser As String = "asset_manager"
    Public strFTPPass As String = DecodePassword(EncFTPUserPass)
    Public FTPcreds As Net.NetworkCredential = New Net.NetworkCredential(strFTPUser, strFTPPass)
    Private intSocketTimeout As Integer = 30000 'timeout for FTP comms in MS
    Public Structure FTPScan_Parms
        Public IsOrphan As Boolean
        Public strTable As String
    End Structure
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
    Private Function FTPFolderIsOrphan(FolderUID As String) As FTPScan_Parms
        Dim ScanResults As New FTPScan_Parms
        Dim intHits As Integer = 0
        Dim results As String
        results = MySQLDB.Get_SQLValue("devices", "dev_UID", FolderUID, "dev_UID")
        If results <> "" Then
            intHits += 1
        End If
        results = MySQLDB.Get_SQLValue("sibi_requests", "sibi_uid", FolderUID, "sibi_uid")
        If results <> "" Then
            intHits += 1
        End If
        If intHits > 0 Then
            ScanResults.IsOrphan = False
            Return ScanResults
        Else
            ScanResults.IsOrphan = True
            Return ScanResults
        End If
    End Function
    Private Function FTPFileIsOrphan(FolderUID As String, FileUID As String) As FTPScan_Parms
        Dim ScanResults As New FTPScan_Parms
        Dim intHits As Integer = 0
        Dim strQRYDev As String = "SELECT * FROM dev_attachments WHERE attach_dev_UID ='" & FolderUID & "' AND attach_file_UID = '" & FileUID & "'"
        Dim strQRYSibi As String = "SELECT * FROM sibi_attachments WHERE sibi_attach_uid ='" & FolderUID & "' AND sibi_attach_file_UID='" & FileUID & "'"
        Dim results As DataTable
        results = MySQLDB.Return_SQLTable(strQRYDev)
        If results.Rows.Count > 0 Then
            intHits += 1
        Else
            ScanResults.strTable = "Device"
        End If
        results = MySQLDB.Return_SQLTable(strQRYSibi)
        If results.Rows.Count > 0 Then
            intHits += 1
        Else
            ScanResults.strTable = "Sibi"
        End If
        If intHits > 0 Then
            ScanResults.IsOrphan = False
            Return ScanResults
        Else
            ScanResults.IsOrphan = True
            Return ScanResults
        End If
    End Function
    Private Function ListDirectory(Uri As String) As List(Of String)
        Dim resp As Net.FtpWebResponse
        Dim files As List(Of String)
        Try
            resp = Return_FTPResponse(Uri, Net.WebRequestMethods.Ftp.ListDirectory) '"ftp://" & strServerIP & "/attachments/"
            Dim responseStream As System.IO.Stream = resp.GetResponseStream
            files = New List(Of String)
            Dim reader As IO.StreamReader = New IO.StreamReader(responseStream)
            While Not reader.EndOfStream 'collect list of files in directory
                files.Add(reader.ReadLine)
            End While
            Return files
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Function
    Public Sub ScanAttachements()
        Dim BadFiles As New List(Of String)
        Dim files As List(Of String)
        Dim intOrphanFolders, intOrphanFiles As Integer
        Try
            Logger("***********************************")
            Logger("******Attachment Scan Results******")
            files = ListDirectory("ftp://" & strServerIP & "/attachments/")
            For Each file In files
                Dim FolderScan As FTPScan_Parms = FTPFolderIsOrphan(file)
                If FolderScan.IsOrphan Then
                    intOrphanFolders += 1
                    BadFiles.Add(file)
                    Logger("Orphan FOLDER Found: " & file)
                Else
                    Dim subfiles As List(Of String) = ListDirectory("ftp://" & strServerIP & "/attachments/" & file & "/")
                    For Each sfile In subfiles
                        Dim FileScan As FTPScan_Parms = FTPFileIsOrphan(file, sfile)
                        If FileScan.IsOrphan Then
                            intOrphanFiles += 1
                            BadFiles.Add(file & "/" & sfile)
                            Logger("Orphan FILE Found. Table:" & FileScan.strTable & "  Path: " & file & "/" & sfile)
                        End If
                    Next
                End If
            Next
            If intOrphanFiles > 0 Or intOrphanFolders > 0 Then
                Dim blah = MyDialog.Message("Orphans found!  Folders:" & intOrphanFolders & "  Files:" & intOrphanFiles & vbCrLf & "See log for details:" & strLogPath & vbCrLf & vbCrLf & "Press OK now to delete orphans.", vbOKCancel + vbCritical, "Corruption Detected")
                'Dim blah = MsgBox("Orphans found!  Folders:" & intOrphanFolders & "  Files:" & intOrphanFiles & vbCrLf & "See log for details:" & strLogPath & vbCrLf & vbCrLf & "Press OK now to delete orphans.", vbOKCancel + vbCritical, "Corruption Detected")
                If blah = vbOK Then
                    CleanFiles(BadFiles)
                    ScanAttachements()
                End If
            Else
                Logger("No Orphans Found.")
                Dim blah = MyDialog.Message("No issues found.", vbOKOnly + vbInformation, "Scan OK")
                'Dim blah = MsgBox("No issues found.", vbOKOnly + vbInformation, "Scan OK")
            End If
            Logger("**********End Scan Results*********")
            Logger("***********************************")
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub CleanFiles(DirList As List(Of String))
        Dim intSuccesses As Integer = 0
        For Each item In DirList
            If InStr(item, "/") Then ' if file and folder
                Dim strBreakPath() As String = Split(item, "/")
                If DeleteFTPAttachment(strBreakPath(1), strBreakPath(0)) Then intSuccesses += 1
            Else  'if folder only
                If DeleteDirectory(item) Then
                    intSuccesses += 1
                End If
            End If
        Next
        Dim blah = MsgBox("Cleaned " & intSuccesses & " orphans.")
    End Sub
    Private Function DeleteDirectory(Directory As String) As Boolean
        Dim FileList As List(Of String) = ListDirectory("ftp://" & strServerIP & "/attachments/" & Directory & "/")
        Dim i As Integer
        For Each file In FileList
            If DeleteFTPAttachment(file, Directory) Then i += 1
        Next
        If FileList.Count = i Then
            Dim resp As Net.FtpWebResponse = Nothing
            resp = Return_FTPResponse("ftp://" & strServerIP & "/attachments/" & Directory, Net.WebRequestMethods.Ftp.RemoveDirectory)
            If resp.StatusCode = Net.FtpStatusCode.FileActionOK Then
                Return True
            Else
                Return False
            End If
        End If
    End Function
End Module
