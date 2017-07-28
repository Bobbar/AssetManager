Public Class FTP_Functions

#Region "Fields"

    Private FTPComms As New FTP_Comms

#End Region

#Region "Methods"

    Public Function DeleteFTPAttachment(FileUID As String, FKey As String) As Boolean
        Dim resp As Net.FtpWebResponse = Nothing
        Try
            resp = DirectCast(FTPComms.Return_FTPResponse("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/" & FKey & "/" & FileUID, Net.WebRequestMethods.Ftp.DeleteFile), Net.FtpWebResponse)
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
        Try
            Dim files = ListDirectory("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/" & FolderUID & "/")
            Dim i As Integer = 0
            For Each file As String In files   'delete each file counting for successes
                If DeleteFTPAttachment(file, FolderUID) Then i += 1
            Next
            If files.Count = i Then ' if successful deletetions = total # of files, delete the directory
                    Using deleteResp = DirectCast(FTPComms.Return_FTPResponse("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/" & FolderUID, Net.WebRequestMethods.Ftp.RemoveDirectory), Net.FtpWebResponse)
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

    Public Function Has_FTPFolder(ItemUID As String) As Boolean
        Try
            Using resp As Net.FtpWebResponse = DirectCast(FTPComms.Return_FTPResponse("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/" & ItemUID & "/", Net.WebRequestMethods.Ftp.ListDirectory), Net.FtpWebResponse)
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

            Dim FTPDirs = ListDirectory("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/")
            Dim FTPFiles = ListFTPFiles(FTPDirs)
            Dim SQLFiles = ListSQLFiles()

            Dim MissingFTPDirs = ListMissingFTPDirs(SQLFiles, FTPDirs)
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
    Private Function CheckForPrimaryItem(ItemUID As String) As Boolean
        Dim exists As Boolean = False
        If AssetFunc.Get_SQLValue(devices.TableName, devices.DeviceUID, ItemUID, devices.DeviceUID) <> "" Then exists = True
        If AssetFunc.Get_SQLValue(sibi_requests.TableName, sibi_requests.UID, ItemUID, sibi_requests.UID) <> "" Then exists = True
        Return exists
    End Function

    Private Function CleanSQLFiles(MissingSQLFiles As List(Of AttachScanInfo)) As Integer
        If MissingSQLFiles.Count > 0 Then
            Dim DeviceTable As New dev_attachments
            Dim SibiTable As New sibi_attachments
            Using SQLComms As New MySQL_Comms

                Dim deletions As Integer = 0
                For Each sqlItem In MissingSQLFiles
                    Using cmd = SQLComms.Return_SQLCommand("DELETE FROM " & DeviceTable.TableName & " WHERE " & DeviceTable.FileUID & "='" & sqlItem.FileUID & "'")
                        Dim rows = cmd.ExecuteNonQuery()
                        If rows > 0 Then
                            deletions += rows
                            Logger("Deleted Device SQL File: " & sqlItem.FKey & "/" & sqlItem.FileUID)
                        End If
                    End Using

                    Using cmd = SQLComms.Return_SQLCommand("DELETE FROM " & SibiTable.TableName & " WHERE " & SibiTable.FileUID & "='" & sqlItem.FileUID & "'")
                        Dim rows = cmd.ExecuteNonQuery()
                        If rows > 0 Then
                            deletions += rows
                            Logger("Deleted Sibi SQL File: " & sqlItem.FKey & "/" & sqlItem.FileUID)
                        End If
                    End Using
                Next
                Return deletions
            End Using
        End If
        Return 0
    End Function

    Private Function CleanSQLEntries(MissingSQLDirs As List(Of AttachScanInfo)) As Integer
        If MissingSQLDirs.Count > 0 Then
            Dim DeviceTable As New dev_attachments
            Dim SibiTable As New sibi_attachments
            Using SQLComms As New MySQL_Comms
                Dim deletions As Integer = 0
                For Each sqlItem In MissingSQLDirs
                    If Not CheckForPrimaryItem(sqlItem.FKey) Then
                        Using cmd = SQLComms.Return_SQLCommand("DELETE FROM " & DeviceTable.TableName & " WHERE " & DeviceTable.FKey & "='" & sqlItem.FKey & "'")
                            Dim rows = cmd.ExecuteNonQuery()
                            If rows > 0 Then
                                deletions += rows
                                Logger("Deleted " & rows & " Device SQL Entries For: " & sqlItem.FKey)
                            End If
                        End Using

                        Using cmd = SQLComms.Return_SQLCommand("DELETE FROM " & SibiTable.TableName & " WHERE " & SibiTable.FKey & "='" & sqlItem.FKey & "'")
                            Dim rows = cmd.ExecuteNonQuery()
                            If rows > 0 Then
                                deletions += rows
                                Logger("Deleted " & rows & " Sibi SQL Entries For: " & sqlItem.FKey)
                            End If
                        End Using
                    End If
                Next
                Return deletions
            End Using
        End If
        Return 0
    End Function

    Private Function CleanFTPFiles(MissingFTPFiles As List(Of AttachScanInfo)) As Integer
        Dim deletions As Integer = 0
        For Each file In MissingFTPFiles
            If DeleteFTPAttachment(file.FileUID, file.FKey) Then
                deletions += 1
                Logger("Deleted SQL File: " & file.FKey & "/" & file.FileUID)
            End If
        Next
        Return deletions
    End Function

    Private Function CleanFTPDirs(MissingFTPDirs As List(Of String)) As Integer
        Dim deletions As Integer = 0
        For Each fDir In MissingFTPDirs
            If DeleteFTPFolder(fDir) Then
                deletions += 1
                Logger("Deleted SQL Directory: " & fDir)
            End If
        Next
        Return deletions
    End Function

    ''' <summary>
    ''' Returns list of SQL entries not found in FTP directory list.
    ''' </summary>
    ''' <param name="SQLFiles"></param>
    ''' <param name="FTPDirs"></param>
    ''' <returns></returns>
    Private Function ListMissingSQLDirs(SQLFiles As List(Of AttachScanInfo), FTPDirs As List(Of String)) As List(Of AttachScanInfo)
        Dim MissingDirs As New List(Of AttachScanInfo)
        For Each SQLfile In SQLFiles
            Dim match As Boolean = False
            FTPDirs.ForEach(Sub(f) If SQLfile.FKey = f Then match = True)
            If Not match Then
                If Not MissingDirs.Exists(Function(d) SQLfile.FKey = d.FKey) Then MissingDirs.Add(SQLfile)
            End If
        Next
        MissingDirs.ForEach(Sub(d) Logger("Orphan SQL Dir Found: " & d.FKey))
        Return MissingDirs
    End Function

    ''' <summary>
    ''' Returns list of FTP dirs not found in SQL file list.
    ''' </summary>
    ''' <param name="SQLFiles"></param>
    ''' <param name="FTPDirs"></param>
    ''' <returns></returns>
    Private Function ListMissingFTPDirs(SQLFiles As List(Of AttachScanInfo), FTPDirs As List(Of String)) As List(Of String)
        Dim MissingDirs = FTPDirs.FindAll(Function(f) Not CheckForPrimaryItem(f))
        MissingDirs.ForEach(Sub(f) Logger("Orphan FTP Dir Found: " & f))
        Return MissingDirs
    End Function

    ''' <summary>
    ''' Returns list of SQL files not found in FTP file list.
    ''' </summary>
    ''' <param name="SQLFiles"></param>
    ''' <param name="FTPFiles"></param>
    ''' <returns></returns>
    Private Function ListMissingSQLFiles(SQLFiles As List(Of AttachScanInfo), FTPFiles As List(Of AttachScanInfo)) As List(Of AttachScanInfo)
        Dim MissingFiles = SQLFiles.Except(FTPFiles).ToList
        MissingFiles.ForEach(Sub(f) Logger("Orphan SQL File Found: " & f.FKey & "/" & f.FileUID))
        Return SQLFiles.Except(FTPFiles).ToList
    End Function

    ''' <summary>
    ''' Returns list of FTP files not found in SQL file list.
    ''' </summary>
    ''' <param name="SQLFiles"></param>
    ''' <param name="FTPFiles"></param>
    ''' <returns></returns>
    Private Function ListMissingFTPFiles(SQLFiles As List(Of AttachScanInfo), FTPFiles As List(Of AttachScanInfo)) As List(Of AttachScanInfo)
        Dim MissingFiles = FTPFiles.Except(SQLFiles).ToList
        MissingFiles.ForEach(Sub(f) Logger("Orphan FTP File Found: " & f.FKey & "/" & f.FileUID))
        Return MissingFiles
    End Function

    Private Function ListFTPFiles(FTPDirs As List(Of String)) As List(Of AttachScanInfo)
        Dim FTPFileList As New List(Of AttachScanInfo)
        For Each fDir In FTPDirs
            For Each file In ListDirectory("ftp://" & strServerIP & "/attachments/" & CurrentDB & "/" & fDir & "/")
                FTPFileList.Add(New AttachScanInfo(fDir, file))
            Next
        Next
        Return FTPFileList
    End Function

    Private Function ListSQLFiles() As List(Of AttachScanInfo)
        Dim DeviceTable As New dev_attachments
        Dim SibiTable As New sibi_attachments
        Dim SQLFileList As New List(Of AttachScanInfo)
        Using SQLComms As New MySQL_Comms
            Dim devFiles = SQLComms.Return_SQLTable("SELECT * FROM " & DeviceTable.TableName)
            For Each file As DataRow In devFiles.Rows
                SQLFileList.Add(New AttachScanInfo(file.Item(DeviceTable.FKey).ToString, file.Item(DeviceTable.FileUID).ToString))
            Next

            Dim sibiFiles = SQLComms.Return_SQLTable("SELECT * FROM " & SibiTable.TableName)
            For Each file As DataRow In sibiFiles.Rows
                SQLFileList.Add(New AttachScanInfo(file.Item(SibiTable.FKey).ToString, file.Item(SibiTable.FileUID).ToString))
            Next

        End Using
        Return SQLFileList
    End Function

    Private Function ListDirectory(Uri As String) As List(Of String)
        Try
            Using resp = DirectCast(FTPComms.Return_FTPResponse(Uri, Net.WebRequestMethods.Ftp.ListDirectory), Net.FtpWebResponse)
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