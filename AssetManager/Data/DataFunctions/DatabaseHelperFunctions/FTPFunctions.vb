Public Class FtpFunctions

#Region "Fields"

    Private FTPComms As New FtpComms

#End Region

#Region "Methods"

    Public Function DeleteFtpAttachment(fileUID As String, fKey As String) As Boolean
        Dim resp As Net.FtpWebResponse = Nothing
        Try
            resp = DirectCast(FTPComms.ReturnFtpResponse("ftp://" & ServerInfo.MySQLServerIP & "/attachments/" & ServerInfo.CurrentDataBase.ToString & "/" & fKey & "/" & fileUID, Net.WebRequestMethods.Ftp.DeleteFile), Net.FtpWebResponse)
            If resp.StatusCode = Net.FtpStatusCode.FileActionOK Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Return ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function

    Public Function DeleteFtpFolder(folderUID As String) As Boolean
        Try
            Dim files = ListDirectory("ftp://" & ServerInfo.MySQLServerIP & "/attachments/" & ServerInfo.CurrentDataBase.ToString & "/" & folderUID & "/")
            Dim i As Integer = 0
            For Each file As String In files   'delete each file counting for successes
                If DeleteFtpAttachment(file, folderUID) Then i += 1
            Next
            If files.Count = i Then ' if successful deletetions = total # of files, delete the directory
                Using deleteResp = DirectCast(FTPComms.ReturnFtpResponse("ftp://" & ServerInfo.MySQLServerIP & "/attachments/" & ServerInfo.CurrentDataBase.ToString & "/" & folderUID, Net.WebRequestMethods.Ftp.RemoveDirectory), Net.FtpWebResponse)
                    If deleteResp.StatusCode = Net.FtpStatusCode.FileActionOK Then
                        Return True
                    End If
                End Using
            End If
            Return False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

    Public Function HasFtpFolder(itemGUID As String) As Boolean
        Try
            Using resp As Net.FtpWebResponse = DirectCast(FTPComms.ReturnFtpResponse("ftp://" & ServerInfo.MySQLServerIP & "/attachments/" & ServerInfo.CurrentDataBase.ToString & "/" & itemGUID & "/", Net.WebRequestMethods.Ftp.ListDirectory), Net.FtpWebResponse)
                If resp.StatusCode = Net.FtpStatusCode.OpeningData Then Return True
            End Using
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub ScanAttachements()
        Try
            Logger("***********************************")
            Logger("******Attachment Scan Results******")

            Dim FTPDirs = ListDirectory("ftp://" & ServerInfo.MySQLServerIP & "/attachments/" & ServerInfo.CurrentDataBase.ToString & "/")
            Dim FTPFiles = ListFTPFiles(FTPDirs)
            Dim SQLFiles = ListSQLFiles()

            Dim MissingFTPDirs = ListMissingFTPDirs(FTPDirs)
            Dim MissingSQLDirs = ListMissingSQLDirs(SQLFiles, FTPDirs)

            Dim MissingFTPFiles = ListMissingFTPFiles(SQLFiles, FTPFiles)
            Dim MissingSQLFiles = ListMissingSQLFiles(SQLFiles, FTPFiles)

            If MissingFTPDirs.Count > 0 Or MissingSQLDirs.Count > 0 Or MissingFTPFiles.Count > 0 Or MissingSQLFiles.Count > 0 Then
                Dim StatsText As String = ""
                Logger("Orphan Files/Directories found!")
                StatsText = "Orphan Files/Directories found!  Do you want to delete the corrupt SQL/FTP entries?

FTP: 
Missing Dirs: " & MissingFTPDirs.Count & "
Missing Files: " & MissingFTPFiles.Count & "

SQL:
Missing Dirs: " & MissingSQLDirs.Count & "
Missing Files: " & MissingSQLFiles.Count

                Dim blah = Message(StatsText, vbYesNo + vbExclamation, "Orphans Found")
                If blah = MsgBoxResult.Yes Then
                    'clean it up
                    Logger("Cleaning attachments...")
                    Dim itemsCleaned As Integer = 0
                    itemsCleaned += CleanFTPFiles(MissingFTPFiles)
                    itemsCleaned += CleanFTPDirs(MissingFTPDirs)
                    itemsCleaned += CleanSQLFiles(MissingSQLFiles)
                    itemsCleaned += CleanSQLEntries(MissingSQLDirs)
                    Message("Cleaned " & itemsCleaned & " orphans.")
                    ScanAttachements()
                End If
            Else
                Logger("No Orphans Found.")
                Message("No issues found.", vbOKOnly + vbInformation, "Scan OK")
            End If
            Logger("**********End Scan Results*********")
            Logger("***********************************")
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    ''' <summary>
    ''' Checks if supplied UID exists in Devices or Sibi tables.
    ''' </summary>
    ''' <returns></returns>
    Private Function CheckForPrimaryItem(itemUID As String) As Boolean
        Dim exists As Boolean = False
        If AssetFunc.GetSqlValue(DevicesCols.TableName, DevicesCols.DeviceUID, itemUID, DevicesCols.DeviceUID) <> "" Then exists = True
        If AssetFunc.GetSqlValue(SibiRequestCols.TableName, SibiRequestCols.UID, itemUID, SibiRequestCols.UID) <> "" Then exists = True
        Return exists
    End Function

    Private Function CleanSQLFiles(missingSQLFiles As List(Of AttachScanInfo)) As Integer
        If missingSQLFiles.Count > 0 Then
            Dim DeviceTable As New DeviceAttachmentsCols
            Dim SibiTable As New SibiAttachmentsCols
            Dim deletions As Integer = 0
            For Each sqlItem In missingSQLFiles
                Dim DeviceRows = DBFunc.GetDatabase.ExecuteQuery("DELETE FROM " & DeviceTable.TableName & " WHERE " & DeviceTable.FileUID & "='" & sqlItem.FileUID & "'")
                If DeviceRows > 0 Then
                    deletions += DeviceRows
                    Logger("Deleted Device SQL File: " & sqlItem.FKey & "/" & sqlItem.FileUID)
                End If

                Dim SibiRows = DBFunc.GetDatabase.ExecuteQuery("DELETE FROM " & SibiTable.TableName & " WHERE " & SibiTable.FileUID & "='" & sqlItem.FileUID & "'")
                If SibiRows > 0 Then
                    deletions += SibiRows
                    Logger("Deleted Sibi SQL File: " & sqlItem.FKey & "/" & sqlItem.FileUID)
                End If
            Next
            Return deletions
        End If
        Return 0
    End Function

    Private Function CleanSQLEntries(missingSQLDirs As List(Of AttachScanInfo)) As Integer
        If missingSQLDirs.Count > 0 Then
            Dim DeviceTable As New DeviceAttachmentsCols
            Dim SibiTable As New SibiAttachmentsCols
            Dim deletions As Integer = 0
            For Each sqlItem In missingSQLDirs
                If Not CheckForPrimaryItem(sqlItem.FKey) Then

                    Dim DeviceRows = DBFunc.GetDatabase.ExecuteQuery("DELETE FROM " & DeviceTable.TableName & " WHERE " & DeviceTable.FKey & "='" & sqlItem.FKey & "'")
                    If DeviceRows > 0 Then
                        deletions += DeviceRows
                        Logger("Deleted " & DeviceRows & " Device SQL Entries For: " & sqlItem.FKey)
                    End If

                    Dim SibiRows = DBFunc.GetDatabase.ExecuteQuery("DELETE FROM " & SibiTable.TableName & " WHERE " & SibiTable.FKey & "='" & sqlItem.FKey & "'")
                    If SibiRows > 0 Then
                        deletions += SibiRows
                        Logger("Deleted " & SibiRows & " Sibi SQL Entries For: " & sqlItem.FKey)
                    End If

                End If
            Next
            Return deletions
        End If
        Return 0
    End Function

    Private Function CleanFTPFiles(missingFTPFiles As List(Of AttachScanInfo)) As Integer
        Dim deletions As Integer = 0
        For Each file In missingFTPFiles
            If DeleteFtpAttachment(file.FileUID, file.FKey) Then
                deletions += 1
                Logger("Deleted SQL File: " & file.FKey & "/" & file.FileUID)
            End If
        Next
        Return deletions
    End Function

    Private Function CleanFTPDirs(missingFTPDirs As List(Of String)) As Integer
        Dim deletions As Integer = 0
        For Each fDir In missingFTPDirs
            If DeleteFtpFolder(fDir) Then
                deletions += 1
                Logger("Deleted SQL Directory: " & fDir)
            End If
        Next
        Return deletions
    End Function

    ''' <summary>
    ''' Returns list of SQL entries not found in FTP directory list.
    ''' </summary>
    ''' <param name="sqlFiles"></param>
    ''' <param name="ftpDirs"></param>
    ''' <returns></returns>
    Private Function ListMissingSQLDirs(sqlFiles As List(Of AttachScanInfo), ftpDirs As List(Of String)) As List(Of AttachScanInfo)
        Dim MissingDirs As New List(Of AttachScanInfo)
        For Each SQLfile In sqlFiles
            Dim match As Boolean = False
            ftpDirs.ForEach(Sub(f) If SQLfile.FKey = f Then match = True)
            If Not match Then
                If Not MissingDirs.Exists(Function(d) SQLfile.FKey = d.FKey) Then MissingDirs.Add(SQLfile)
            End If
        Next
        MissingDirs.ForEach(Sub(d) Logger("Orphan SQL Dir Found: " & d.FKey))
        Return MissingDirs
    End Function

    ''' <summary>
    ''' Returns list of FTP dirs that do not have an associated Device or Sibi request.
    ''' </summary>
    ''' <param name="ftpDirs"></param>
    ''' <returns></returns>
    Private Function ListMissingFTPDirs(ftpDirs As List(Of String)) As List(Of String)
        Dim MissingDirs = ftpDirs.FindAll(Function(f) Not CheckForPrimaryItem(f))
        MissingDirs.ForEach(Sub(f) Logger("Orphan FTP Dir Found: " & f))
        Return MissingDirs
    End Function

    ''' <summary>
    ''' Returns list of SQL files not found in FTP file list.
    ''' </summary>
    ''' <param name="sqlFiles"></param>
    ''' <param name="ftpFiles"></param>
    ''' <returns></returns>
    Private Function ListMissingSQLFiles(sqlFiles As List(Of AttachScanInfo), ftpFiles As List(Of AttachScanInfo)) As List(Of AttachScanInfo)
        Dim MissingFiles = sqlFiles.Except(ftpFiles).ToList
        MissingFiles.ForEach(Sub(f) Logger("Orphan SQL File Found: " & f.FKey & "/" & f.FileUID))
        Return sqlFiles.Except(ftpFiles).ToList
    End Function

    ''' <summary>
    ''' Returns list of FTP files not found in SQL file list.
    ''' </summary>
    ''' <param name="sqlFiles"></param>
    ''' <param name="ftpFiles"></param>
    ''' <returns></returns>
    Private Function ListMissingFTPFiles(sqlFiles As List(Of AttachScanInfo), ftpFiles As List(Of AttachScanInfo)) As List(Of AttachScanInfo)
        Dim MissingFiles = ftpFiles.Except(sqlFiles).ToList
        MissingFiles.ForEach(Sub(f) Logger("Orphan FTP File Found: " & f.FKey & "/" & f.FileUID))
        Return MissingFiles
    End Function

    Private Function ListFTPFiles(ftpDirs As List(Of String)) As List(Of AttachScanInfo)
        Dim FTPFileList As New List(Of AttachScanInfo)
        For Each fDir In ftpDirs
            For Each file In ListDirectory("ftp://" & ServerInfo.MySQLServerIP & "/attachments/" & ServerInfo.CurrentDataBase.ToString & "/" & fDir & "/")
                FTPFileList.Add(New AttachScanInfo(fDir, file))
            Next
        Next
        Return FTPFileList
    End Function

    Private Function ListSQLFiles() As List(Of AttachScanInfo)
        Dim DeviceTable As New DeviceAttachmentsCols
        Dim SibiTable As New SibiAttachmentsCols
        Dim SQLFileList As New List(Of AttachScanInfo)

        Dim devFiles = DBFunc.GetDatabase.DataTableFromQueryString("SELECT * FROM " & DeviceTable.TableName)
        For Each file As DataRow In devFiles.Rows
            SQLFileList.Add(New AttachScanInfo(file.Item(DeviceTable.FKey).ToString, file.Item(DeviceTable.FileUID).ToString))
        Next

        Dim sibiFiles = DBFunc.GetDatabase.DataTableFromQueryString("SELECT * FROM " & SibiTable.TableName)
        For Each file As DataRow In sibiFiles.Rows
            SQLFileList.Add(New AttachScanInfo(file.Item(SibiTable.FKey).ToString, file.Item(SibiTable.FileUID).ToString))
        Next

        Return SQLFileList
    End Function

    Private Function ListDirectory(uri As String) As List(Of String)
        Try
            Using resp = DirectCast(FTPComms.ReturnFtpResponse(uri, Net.WebRequestMethods.Ftp.ListDirectory), Net.FtpWebResponse)
                Dim responseStream As System.IO.Stream = resp.GetResponseStream
                Using reader As IO.StreamReader = New IO.StreamReader(responseStream)
                    Dim files As New List(Of String)
                    While Not reader.EndOfStream 'collect list of files in directory
                        files.Add(reader.ReadLine)
                    End While
                    Return files
                End Using
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

    Private Structure AttachScanInfo
        Public FKey As String
        Public FileUID As String
        Sub New(FKey As String, FileUID As String)
            Me.FKey = FKey
            Me.FileUID = FileUID
        End Sub
    End Structure
#End Region

End Class