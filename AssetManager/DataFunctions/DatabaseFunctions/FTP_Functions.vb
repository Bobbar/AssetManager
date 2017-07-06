Public Class FTP_Functions

#Region "Fields"

    Private FTPComms As New FTP_Comms

#End Region

#Region "Methods"

    Public Function DeleteFTPAttachment(AttachUID As String, DeviceUID As String) As Boolean
        Dim resp As Net.FtpWebResponse = Nothing
        Try
            resp = DirectCast(FTPComms.Return_FTPResponse("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/" & DeviceUID & "/" & AttachUID, Net.WebRequestMethods.Ftp.DeleteFile), Net.FtpWebResponse)
            If resp.StatusCode = Net.FtpStatusCode.FileActionOK Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function

    Public Function DeleteFTPFolder(FolderUID As String) As Boolean
        Dim resp As Net.FtpWebResponse = Nothing
        Dim files As List(Of String)
        Try
            resp = DirectCast(FTPComms.Return_FTPResponse("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/" & FolderUID & "/", Net.WebRequestMethods.Ftp.ListDirectory), Net.FtpWebResponse)
            Dim responseStream As System.IO.Stream = resp.GetResponseStream
            files = New List(Of String)
            Dim reader As IO.StreamReader = New IO.StreamReader(responseStream)
            While Not reader.EndOfStream 'collect list of files in directory
                files.Add(reader.ReadLine)
            End While
            reader.Close()
            responseStream.Dispose()
            Dim i As Integer = 0
            For Each file As String In files  'delete each file counting for successes
                If DeleteFTPAttachment(file, FolderUID) Then i += 1
            Next
            If files.Count = i Then ' if successful deletetions = total # of files, delete the directory
                resp = DirectCast(FTPComms.Return_FTPResponse("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/" & FolderUID, Net.WebRequestMethods.Ftp.RemoveDirectory), Net.FtpWebResponse)
            End If
            If resp.StatusCode = Net.FtpStatusCode.FileActionOK Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

    Public Function Has_FTPFolder(ItemUID As String) As Boolean
        Dim files As New List(Of String)
        Try
            Using resp As Net.FtpWebResponse = DirectCast(FTPComms.Return_FTPResponse("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/" & ItemUID & "/", Net.WebRequestMethods.Ftp.ListDirectory), Net.FtpWebResponse)
                Return True
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Sub ScanAttachements()
        Dim BadFiles As New List(Of String)
        Dim files As List(Of String)
        Dim intOrphanFolders, intOrphanFiles As Integer
        Try
            Logger("***********************************")
            Logger("******Attachment Scan Results******")
            files = ListDirectory("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/")
            intOrphanFolders = CheckForMissingDir(files)
            For Each file In files
                Dim FolderScan As FTPScan_Parms = FTPFolderIsOrphan(file)
                If FolderScan.IsOrphan Then
                    intOrphanFolders += 1
                    BadFiles.Add(file)
                    Logger("Orphan FOLDER Found: " & file)
                Else
                    Dim subfiles As List(Of String) = ListDirectory("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/" & file & "/")
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
                Dim blah = Message("Orphans found!  Folders:" & intOrphanFolders & "  Files:" & intOrphanFiles & vbCrLf & vbCrLf & "See log for details:" & Chr(34) & strLogPath & Chr(34) & vbCrLf & vbCrLf & "Press OK now to delete orphans.", vbOKCancel + vbExclamation, "Corruption Detected")
                'Dim blah = Message("Orphans found!  Folders:" & intOrphanFolders & "  Files:" & intOrphanFiles & vbCrLf & "See log for details:" & strLogPath & vbCrLf & vbCrLf & "Press OK now to delete orphans.", vbOKCancel + vbCritical, "Corruption Detected")
                If blah = vbOK Then
                    CleanFiles(BadFiles)
                    ScanAttachements()
                End If
            Else
                Logger("No Orphans Found.")
                Dim blah = Message("No issues found.", vbOKOnly + vbInformation, "Scan OK")
                'Dim blah = Message("No issues found.", vbOKOnly + vbInformation)
            End If
            Logger("**********End Scan Results*********")
            Logger("***********************************")
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Function CheckForMissingDir(FTPFolderUIDs As List(Of String)) As Integer
        Dim DBFolders As New List(Of String)
        Dim DevQry As String = "SELECT DISTINCT attach_fkey_uid  FROM dev_attachments"
        Dim SibiQry As String = "SELECT DISTINCT attach_fkey_uid  FROM sibi_attachments"
        Using MyComms As New MySQL_Comms
            For Each row As DataRow In MyComms.Return_SQLTable(DevQry).Rows
                DBFolders.Add(row.Item("attach_fkey_UID").ToString)
            Next
            For Each row As DataRow In MyComms.Return_SQLTable(SibiQry).Rows
                DBFolders.Add(row.Item("attach_fkey_UID").ToString)
            Next

        End Using
        Dim ExpectedDirCount As Integer = DBFolders.Count
        Dim DirCount As Integer = 0
        Dim MissingDirs As New List(Of String)
        For Each DBFolder In DBFolders
            If DirInList(FTPFolderUIDs, DBFolder) Then
                DirCount += 1
            Else
                MissingDirs.Add(DBFolder)
            End If
        Next
        If ExpectedDirCount <> DirCount Then
            Logger("Orphan FOLDER(s) Found: ")
            For Each sDir In MissingDirs
                Logger(sDir)
            Next
            Return MissingDirs.Count
        End If
        Return 0
    End Function

    Private Sub CleanFiles(DirList As List(Of String))
        Dim intSuccesses As Integer = 0
        For Each item In DirList
            If item.Contains("/") Then ' if file and folder
                Dim strBreakPath() As String = Split(item, "/")
                If DeleteFTPAttachment(strBreakPath(1), strBreakPath(0)) Then intSuccesses += 1
            Else  'if folder only
                If DeleteDirectory(item) Then
                    intSuccesses += 1
                End If
            End If
        Next
        Dim blah = Message("Cleaned " & intSuccesses & " orphans.")
    End Sub

    Private Function DeleteDirectory(Directory As String) As Boolean
        Try
            Dim FileList As List(Of String) = ListDirectory("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/" & Directory & "/")
            Dim i As Integer
            For Each file In FileList
                If DeleteFTPAttachment(file, Directory) Then i += 1
            Next
            If FileList.Count = i Then
                Dim resp As Net.FtpWebResponse = Nothing
                resp = DirectCast(FTPComms.Return_FTPResponse("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/" & Directory, Net.WebRequestMethods.Ftp.RemoveDirectory), Net.FtpWebResponse)
                If resp.StatusCode = Net.FtpStatusCode.FileActionOK Then
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

    Private Function DirInList(DirList As List(Of String), CompDir As String) As Boolean
        For Each sDir In DirList
            If sDir = CompDir Then Return True
        Next
        Return False
    End Function

    Private Function FTPFileIsOrphan(FolderUID As String, FileUID As String) As FTPScan_Parms
        Dim ScanResults As New FTPScan_Parms
        Dim intHits As Integer = 0
        Dim DeviceTable As New dev_attachments
        Dim SibiTable As New sibi_attachments
        Dim strQRYDev As String = "SELECT * FROM " & DeviceTable.TableName & " WHERE " & DeviceTable.FKey & " ='" & FolderUID & "' AND " & DeviceTable.FileUID & " = '" & FileUID & "'"
        Dim strQRYSibi As String = "SELECT * FROM " & SibiTable.TableName & " WHERE " & SibiTable.FKey & " ='" & FolderUID & "' AND " & SibiTable.FileUID & "='" & FileUID & "'"
        Using SQLComms As New MySQL_Comms
            Dim results As DataTable
            results = SQLComms.Return_SQLTable(strQRYDev)
            If results.Rows.Count > 0 Then
                intHits += 1
            Else
                ScanResults.strTable = "Device"
            End If
            results = SQLComms.Return_SQLTable(strQRYSibi)
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
        End Using
    End Function

    Private Function FTPFolderIsOrphan(FolderUID As String) As FTPScan_Parms
        Dim ScanResults As New FTPScan_Parms
        Dim intHits As Integer = 0
        Dim results As String
        results = AssetFunc.Get_SQLValue(devices.TableName, devices.DeviceUID, FolderUID, devices.DeviceUID)
        If results <> "" Then
            intHits += 1
        End If
        results = AssetFunc.Get_SQLValue(sibi_requests.TableName, sibi_requests.UID, FolderUID, sibi_requests.UID)
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
    Private Function ListDirectory(Uri As String) As List(Of String)
        Dim resp As Net.FtpWebResponse
        Dim files As List(Of String)
        Try
            resp = DirectCast(FTPComms.Return_FTPResponse(Uri, Net.WebRequestMethods.Ftp.ListDirectory), Net.FtpWebResponse) '"ftp://" & strServerIP & "/attachments/"
            Dim responseStream As System.IO.Stream = resp.GetResponseStream
            files = New List(Of String)
            Dim reader As IO.StreamReader = New IO.StreamReader(responseStream)
            While Not reader.EndOfStream 'collect list of files in directory
                files.Add(reader.ReadLine)
            End While
            Return files
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

#End Region

End Class