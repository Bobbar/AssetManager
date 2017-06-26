Option Explicit On
Option Compare Binary
Imports System.ComponentModel
Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Text
Imports System.Threading
Imports System.Runtime.InteropServices
Class AttachmentsForm
    Public bolAdminMode As Boolean = False
    Private Const FileSizeMBLimit As Short = 150
    Private bolGridFilling As Boolean
    Private strSelectedFolder As String
    Private bolDragging As Boolean = False
    Private bolAllowDrag As Boolean = False
    Private AttachRequest As Request_Info
    Private AttachDevice As Device_Info
    Private _attachTable As main_attachments
    Public AttachFolderUID As String
    Private Progress As New ProgressCounter
    ''' <summary>
    ''' "ftp://  strServerIP  /attachments/  CurrentDB  /"
    ''' </summary>
    Private FTPUri As String = "ftp://" & strServerIP & "/attachments/" & CurrentDB & "/"
    Sub New(ParentForm As MyForm, AttachTable As main_attachments, Optional AttachInfo As Object = Nothing)
        InitializeComponent()
        Tag = ParentForm
        Icon = ParentForm.Icon
        GridTheme = ParentForm.GridTheme
        ExtendedMethods.DoubleBufferedDataGrid(AttachGrid, True)
        StatusBar("Idle...")
        _attachTable = AttachTable
        If Not IsNothing(AttachInfo) Then
            If TypeOf AttachInfo Is Request_Info Then
                AttachRequest = DirectCast(AttachInfo, Request_Info)
                AttachFolderUID = AttachRequest.strUID
                strSelectedFolder = GetHumanValueFromIndex(SibiIndex.AttachFolder, 0)
                Me.Text = "Sibi Attachements"
                DeviceGroup.Visible = False
                SibiGroup.Dock = DockStyle.Top
                FillFolderCombos()
                FillSibiInfo()
            ElseIf TypeOf AttachInfo Is Device_Info Then
                AttachDevice = DirectCast(AttachInfo, Device_Info)
                AttachFolderUID = AttachDevice.strGUID
                Me.Text = "Device Attachements"
                SibiGroup.Visible = False
                DeviceGroup.Dock = DockStyle.Top
                MoveStripMenuItem.Visible = False
                FolderPanel.Visible = False
                FillDeviceInfo()
            End If
        Else
            SibiGroup.Visible = False
            MoveStripMenuItem.Visible = False
            FolderPanel.Visible = False
        End If

        If CanAccess(AccessGroup.ManageAttachment) Then
            cmdUpload.Enabled = True
            cmdDelete.Enabled = True
        Else
            cmdUpload.Enabled = False
            cmdDelete.Enabled = False
        End If
        ListAttachments()
    End Sub

    Private Sub cmdUpload_Click(sender As Object, e As EventArgs) Handles cmdUpload.Click
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        Dim fd As OpenFileDialog = New OpenFileDialog()
        fd.ShowHelp = True
        fd.Title = "Select File To Upload - " & FileSizeMBLimit & "MB Limit"
        fd.InitialDirectory = "C:\"
        fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.Multiselect = True
        fd.RestoreDirectory = True
        If fd.ShowDialog() = DialogResult.OK Then
            UploadFile(fd.FileNames)
        Else
            Exit Sub
        End If
    End Sub
    Private Sub UploadFile(Files() As String)
        If Not UploadWorker.IsBusy Then
            StatusBar("Starting Upload...")
            WorkerFeedback(True)
            UploadWorker.RunWorkerAsync(Files)
        End If
    End Sub
    Private Sub WorkerFeedback(WorkerRunning As Boolean)
        If WorkerRunning Then
            ' SetWaitCursor(True)
            ProgressBar1.Value = 0
            ProgressBar1.Visible = True
            cmdCancel.Visible = True
            Spinner.Visible = True
            ProgTimer.Enabled = True
        Else
            Progress = New ProgressCounter
            SetWaitCursor(False)
            ProgressBar1.Value = 0
            ProgressBar1.Visible = False
            cmdCancel.Visible = False
            Spinner.Visible = False
            ProgTimer.Enabled = False
            statMBPS.Text = Nothing
            StatusBar("Idle...")
            DoneWaiting()
        End If
    End Sub
    Private Function GetQry() As String
        Dim strQry As String = ""
        If TypeOf _attachTable Is sibi_attachments Then
            Select Case GetDBValue(SibiIndex.AttachFolder, cmbFolder.SelectedIndex)
                Case "ALL"
                    strQry = "Select * FROM " & _attachTable.TableName & " WHERE " & _attachTable.FKey & "='" & AttachRequest.strUID & "' ORDER BY " & _attachTable.TimeStamp & " DESC"
                Case Else
                    strQry = "Select * FROM " & _attachTable.TableName & " WHERE " & _attachTable.Folder & "='" & GetDBValue(SibiIndex.AttachFolder, cmbFolder.SelectedIndex) & "' AND " & _attachTable.FKey & " ='" & AttachRequest.strUID & "' ORDER BY " & _attachTable.TimeStamp & " DESC"
            End Select
        Else
            'If bolAdminMode Then 'TODO: include all attachment type tables for admin mode

            '    strQry = "Select * FROM " & _attachTable.TableName & "," & _attachTable.TableName & " WHERE " & devices.DeviceUID & " = " & _attachTable.FKey & " ORDER BY " & _attachTable.TimeStamp & " DESC"
            'ElseIf Not bolAdminMode Then
            strQry = "Select * FROM " & _attachTable.TableName & " WHERE " & _attachTable.FKey & "='" & AttachDevice.strGUID & "' ORDER BY " & _attachTable.TimeStamp & " DESC"
            '  End If
        End If
        Return strQry
    End Function
    Private Function GetTable() As DataTable
        Dim table As New DataTable
        If TypeOf _attachTable Is sibi_attachments Then
            table.Columns.Add(" ", GetType(Image))
            table.Columns.Add("Filename", GetType(String))
            table.Columns.Add("Size", GetType(String))
            table.Columns.Add("Date", GetType(String))
            table.Columns.Add("Folder", GetType(String))
            table.Columns.Add("AttachUID", GetType(String))
            table.Columns.Add("MD5", GetType(String))
        Else
            'If bolAdminMode Then
            '    table.Columns.Add("Filename", GetType(String))
            '    table.Columns.Add("Size", GetType(String))
            '    table.Columns.Add("Date", GetType(String))
            '    table.Columns.Add("Device", GetType(String))
            '    table.Columns.Add("AttachUID", GetType(String))
            '    table.Columns.Add("MD5", GetType(String))
            'ElseIf Not bolAdminMode Then
            table.Columns.Add(" ", GetType(Image))
            table.Columns.Add("Filename", GetType(String))
            table.Columns.Add("Size", GetType(String))
            table.Columns.Add("Date", GetType(String))
            table.Columns.Add("AttachUID", GetType(String))
            table.Columns.Add("MD5", GetType(String))
            '  End If
        End If
        Return table
    End Function
    Public Sub ListAttachments()
        Waiting()
        Try
            Dim table As New DataTable
            Dim strQry As String
            strQry = GetQry()
            table = GetTable()
            Using SQLComms As New MySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strQry)
                Dim strFullFilename As String
                Dim strFileSizeHuman As String
                For Each r As DataRow In results.Rows
                    strFileSizeHuman = Math.Round((CInt(r.Item(_attachTable.FileSize)) / 1024), 1) & " KB"
                    strFullFilename = r.Item(_attachTable.FileName).ToString & r.Item(_attachTable.FileType).ToString
                    If TypeOf _attachTable Is sibi_attachments Then
                        table.Rows.Add(FileIcon.GetFileIcon(r.Item(_attachTable.FileType).ToString), strFullFilename, strFileSizeHuman, r.Item(_attachTable.TimeStamp), GetHumanValue(SibiIndex.AttachFolder, r.Item(_attachTable.Folder).ToString), r.Item(_attachTable.FileUID), r.Item(_attachTable.FileHash))
                    Else
                        'If bolAdminMode Then
                        '    table.Rows.Add(strFullFilename, strFileSizeHuman, r.Item(_attachTable.TimeStamp), r.Item(devices.AssetTag), r.Item(_attachTable.FileUID), r.Item(_attachTable.FileHash))
                        'Else
                        table.Rows.Add(FileIcon.GetFileIcon(r.Item(_attachTable.FileType).ToString), strFullFilename, strFileSizeHuman, r.Item(_attachTable.TimeStamp), r.Item(_attachTable.FileUID), r.Item(_attachTable.FileHash))
                        ' End If
                    End If
                Next
            End Using
            bolGridFilling = True
            AttachGrid.DataSource = table
            AttachGrid.Columns("Filename").DefaultCellStyle.Font = New Font("Consolas", 9.75, FontStyle.Bold)
            table.Dispose()
            RefreshAttachCount()
            DoneWaiting()
            Me.Show()
            AttachGrid.ClearSelection()
            bolGridFilling = False
        Catch ex As Exception
            DoneWaiting()
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Exit Sub
        End Try
    End Sub
    Private Sub RefreshAttachCount()
        If TypeOf Tag Is ViewDeviceForm Then
            Dim vw As ViewDeviceForm = DirectCast(Tag, ViewDeviceForm)
            vw.SetAttachCount()
        ElseIf TypeOf Tag Is SibiManageRequestForm Then
            Dim req As SibiManageRequestForm = DirectCast(Tag, SibiManageRequestForm)
            req.SetAttachCount()
        End If
    End Sub
    Private Sub OpenAttachment(AttachUID As String)
        If Not DownloadWorker.IsBusy Then
            StatusBar("Starting Download...")
            WorkerFeedback(True)
            DownloadWorker.RunWorkerAsync(AttachUID)
        End If
    End Sub
    Private Sub Waiting()
        SetWaitCursor(True)
        StatusBar("Processing...")
    End Sub
    Private Sub DoneWaiting()
        SetWaitCursor(False)
        StatusBar("Idle...")
    End Sub
    Public Sub StatusBar(Text As String)
        StatusLabel.Text = Text
        StatusLabel.Invalidate()
    End Sub
    Private Sub FillFolderCombos()
        FillComboBox(SibiIndex.AttachFolder, cmbFolder)
        FillToolComboBox(SibiIndex.AttachFolder, cmbMoveFolder)
    End Sub
    Public Sub FillSibiInfo()
        txtUID.Text = AttachRequest.strUID
        txtRequestNum.Text = AttachRequest.strRequestNumber
        txtDescription.Text = AttachRequest.strDescription
        cmbFolder.SelectedIndex = 0
    End Sub
    Private Sub FillDeviceInfo()
        txtAssetTag.Text = AttachDevice.strAssetTag
        txtSerial.Text = AttachDevice.strSerial
        txtDeviceDescription.Text = AttachDevice.strDescription
    End Sub
    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs)
        OpenAttachment(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString)
    End Sub
    Private Sub DeleteAttachmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteAttachmentToolStripMenuItem.Click
        If AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString <> "" Then
            StartAttachDelete(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString)
        End If
    End Sub
    Private Sub StartAttachDelete(AttachUID As String)
        If Not CheckForAccess(AccessGroup.ManageAttachment) Then Exit Sub
        Dim strFilename As String = AttachGrid.Item(GetColIndex(AttachGrid, "Filename"), AttachGrid.CurrentRow.Index).Value.ToString
        Dim blah = Message("Are you sure you want to delete '" & strFilename & "'?", vbYesNo + vbQuestion, "Confirm Delete", Me)
        If blah = vbYes Then
            Waiting()
            If AssetFunc.DeleteSQLAttachment(GetSQLAttachment(AttachUID)) > 0 Then
                ListAttachments()
                DoneWaiting()
                ' blah = Message("'" & strFilename & "' has been deleted.", vbOKOnly + vbInformation, "Deleted")
            Else
                DoneWaiting()
                blah = Message("Deletion failed!", vbOKOnly + vbExclamation, "Unexpected Results", Me)
            End If
        End If
    End Sub
    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        If AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString <> "" Then
            StartAttachDelete(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString)
        End If
    End Sub
    Private Sub cmdOpen_Click(sender As Object, e As EventArgs) Handles cmdOpen.Click
        If AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString <> "" Then
            OpenAttachment(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString)
        End If
    End Sub
    Private Sub UploadWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles UploadWorker.DoWork
        Dim LocalFTPComm As New FTP_Comms
        Dim Files() As String = DirectCast(e.Argument, String())
        Dim FileNumber As Integer = 1
        For Each file As String In Files
            Dim CurrentAttachment As Attachment
            If TypeOf _attachTable Is sibi_attachments Then
                CurrentAttachment = New Sibi_Attachment(file, AttachFolderUID, strSelectedFolder, _attachTable)
            Else
                CurrentAttachment = New Attachment(file, AttachFolderUID, _attachTable)
            End If
            If Not OKFileSize(CurrentAttachment) Then
                e.Result = TransferReturnType.FileTooLarge
                UploadWorker.ReportProgress(2, "Error!")
                Exit Sub
            End If
            UploadWorker.ReportProgress(1, "Connecting...")
            MakeDirectory(CurrentAttachment.FolderGUID)
            Using FileStream As FileStream = DirectCast(CurrentAttachment.DataStream(), FileStream), 'CurrentAttachment.FileInfo.OpenRead(),
                FTPStream As System.IO.Stream = LocalFTPComm.Return_FTPRequestStream(FTPUri & CurrentAttachment.FolderGUID & "/" & CurrentAttachment.FileUID, Net.WebRequestMethods.Ftp.UploadFile)
                Dim buffer(1023) As Byte
                Dim bytesIn As Integer = 1
                Progress = New ProgressCounter
                Progress.BytesToTransfer = CInt(FileStream.Length)
                UploadWorker.ReportProgress(1, "Uploading... " & FileNumber & " of " & Files.Count)
                Do Until bytesIn < 1 Or UploadWorker.CancellationPending
                    bytesIn = FileStream.Read(buffer, 0, 1024)
                    If bytesIn > 0 Then
                        FTPStream.Write(buffer, 0, bytesIn)
                        Progress.BytesMoved = bytesIn
                    End If
                Loop
            End Using
            If UploadWorker.CancellationPending Then
                e.Cancel = True
                FTPFunc.DeleteFTPAttachment(CurrentAttachment.FileUID, CurrentAttachment.FolderGUID)
                Throw New BackgroundWorkerCancelledException("The upload was cancelled.")
            End If
            'update sql table
            If Not UploadWorker.CancellationPending Then
                If InsertSQLAttachment(CurrentAttachment) Then
                    CurrentAttachment.Dispose()
                    e.Result = TransferReturnType.Success
                Else
                    CurrentAttachment.Dispose()
                    e.Result = TransferReturnType.Failure
                End If
            Else
                CurrentAttachment.Dispose()
                e.Result = TransferReturnType.Failure
            End If
            FileNumber += 1
            UploadWorker.ReportProgress(3, "Idle...")
        Next
        UploadWorker.ReportProgress(3, "Idle...")
    End Sub
    Private Function InsertSQLAttachment(Attachment As Attachment) As Boolean
        Try
            Dim SQL As String
            If TypeOf Attachment Is Sibi_Attachment Then
                SQL = "INSERT INTO " & Attachment.AttachTable.TableName & " (" & Attachment.AttachTable.FKey & ", 
" & Attachment.AttachTable.FileName & ",
" & Attachment.AttachTable.FileType & ",
" & Attachment.AttachTable.FileSize & ",
" & Attachment.AttachTable.FileUID & ", 
" & Attachment.AttachTable.FileHash & ", 
" & Attachment.AttachTable.Folder & ") 
VALUES(@" & Attachment.AttachTable.FKey & ", 
@" & Attachment.AttachTable.FileName & ", 
@" & Attachment.AttachTable.FileType & ",
@" & Attachment.AttachTable.FileSize & ", 
@" & Attachment.AttachTable.FileUID & ",
@" & Attachment.AttachTable.FileHash & ",
@" & Attachment.AttachTable.Folder & ")"
            Else
                SQL = "INSERT INTO " & Attachment.AttachTable.TableName & " (" & Attachment.AttachTable.FKey & ", 
" & Attachment.AttachTable.FileName & ",
" & Attachment.AttachTable.FileType & ", 
" & Attachment.AttachTable.FileSize & ", 
" & Attachment.AttachTable.FileUID & ", 
" & Attachment.AttachTable.FileHash & ") 
VALUES(@" & Attachment.AttachTable.FKey & ",
@" & Attachment.AttachTable.FileName & ", 
@" & Attachment.AttachTable.FileType & ", 
@" & Attachment.AttachTable.FileSize & ",
@" & Attachment.AttachTable.FileUID & ",
@" & Attachment.AttachTable.FileHash & ")"
            End If
            Using LocalSQLComm As New MySQL_Comms, cmd As MySqlCommand = LocalSQLComm.Return_SQLCommand(SQL)
                cmd.Parameters.AddWithValue("@" & Attachment.AttachTable.FKey, Attachment.FolderGUID)
                cmd.Parameters.AddWithValue("@" & Attachment.AttachTable.FileName, Attachment.Filename)
                cmd.Parameters.AddWithValue("@" & Attachment.AttachTable.FileType, Attachment.Extention)
                cmd.Parameters.AddWithValue("@" & Attachment.AttachTable.FileSize, Attachment.Filesize)
                cmd.Parameters.AddWithValue("@" & Attachment.AttachTable.FileUID, Attachment.FileUID)
                cmd.Parameters.AddWithValue("@" & Attachment.AttachTable.FileHash, Attachment.MD5)
                If TypeOf Attachment Is Sibi_Attachment Then
                    Dim SibiAttach = DirectCast(Attachment, Sibi_Attachment)
                    cmd.Parameters.AddWithValue("@" & Attachment.AttachTable.Folder, SibiAttach.SelectedFolder)
                End If
                cmd.ExecuteNonQuery()
                cmd.Parameters.Clear()
                Return True
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function
    Private Sub MakeDirectory(FolderGUID As String)
        Dim LocalFTPComm As New FTP_Comms
        Using resp = LocalFTPComm.Return_FTPResponse(FTPUri, Net.WebRequestMethods.Ftp.ListDirectoryDetails), 'check if device folder exists. create directory if not.
               sr As StreamReader = New StreamReader(resp.GetResponseStream(), System.Text.Encoding.ASCII)
            Dim s As String = sr.ReadToEnd()
            If Not s.Contains(FolderGUID) Then
                Using MkDirResp = LocalFTPComm.Return_FTPResponse(FTPUri & FolderGUID, Net.WebRequestMethods.Ftp.MakeDirectory)
                End Using
            End If
        End Using
    End Sub
    Private Function OKFileSize(File As Attachment) As Boolean
        Dim FileSizeMB = CInt(File.Filesize / (1024 * 1024))
        If FileSizeMB > FileSizeMBLimit Then
            Return False
        End If
        Return True
    End Function
    Private Sub UploadWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles UploadWorker.ProgressChanged
        Select Case e.ProgressPercentage
            Case 1
                StatusBar(DirectCast(e.UserState, String))
            Case 2
                statMBPS.Text = Nothing
                ProgressBar1.Visible = False
                ProgressBar1.Value = 0
                ProgTimer.Enabled = False
                Spinner.Visible = False
                StatusBar(DirectCast(e.UserState, String))
                Me.Refresh()
            Case 3
                StatusBar(DirectCast(e.UserState, String))
                ListAttachments()
        End Select
    End Sub
    Private Sub UploadWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles UploadWorker.RunWorkerCompleted
        Try
            If Not Me.IsDisposed Then
                WorkerFeedback(False)
                If e.Error Is Nothing Then
                    If Not e.Cancelled Then
                        Dim UploadResult As TransferReturnType = DirectCast(e.Result, TransferReturnType)

                        Select Case UploadResult

                            Case TransferReturnType.Success
                                ListAttachments()

                            Case TransferReturnType.Failure
                                Message("File upload failed.", MessageBoxButtons.OK + MessageBoxIcon.Exclamation, "Failed", Me)

                            Case TransferReturnType.FileTooLarge
                                Dim blah = Message("The file is too large.   Please select a file less than " & FileSizeMBLimit & "MB.", vbOKOnly + vbExclamation, "Size Limit Exceeded", Me)

                        End Select

                    Else
                        '           MessageBox.Show("The upload was cancelled.",
                        '"Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                Else
                    DoneWaiting()
                    If TypeOf e.Error Is BackgroundWorkerCancelledException Then
                        Message("File upload was cancelled.", vbOKOnly + vbInformation, "Cancelled", Me)
                    Else
                        If Not ErrHandle(e.Error, System.Reflection.MethodInfo.GetCurrentMethod()) Then EndProgram()
                        Message("File upload failed.", MessageBoxButtons.OK + MessageBoxIcon.Exclamation, "Failed", Me)
                    End If
                End If
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub MoveAttachFolder(AttachUID As String, Folder As String)
        AssetFunc.Update_SQLValue(_attachTable.TableName, _attachTable.Folder, Folder, _attachTable.FileUID, AttachUID)
        ListAttachments()
        cmbMoveFolder.SelectedIndex = -1
        cmbMoveFolder.Text = "Select a folder"
        RightClickMenu.Close()
    End Sub
    Private Sub RenameAttachement(AttachUID As String, NewFileName As String)
        Try
            AssetFunc.Update_SQLValue(_attachTable.TableName, _attachTable.FileName, NewFileName, _attachTable.FileUID, AttachUID)
            ListAttachments()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub DownloadWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles DownloadWorker.DoWork
        DownloadWorker.ReportProgress(1, "Connecting...")
        Dim LocalFTPComm As New FTP_Comms
        Dim AttachUID As String = DirectCast(e.Argument, String)
        Dim di As DirectoryInfo = Directory.CreateDirectory(DownloadPath)
        Dim dAttachment = GetSQLAttachment(AttachUID)
        Dim FtpRequestString As String = FTPUri & dAttachment.FolderGUID & "/" & AttachUID
        'get file size
        Progress = New ProgressCounter
        Progress.BytesToTransfer = CInt(LocalFTPComm.Return_FTPResponse(FtpRequestString, Net.WebRequestMethods.Ftp.GetFileSize).ContentLength)
        'setup download
        Using respStream = LocalFTPComm.Return_FTPResponse(FtpRequestString, Net.WebRequestMethods.Ftp.DownloadFile).GetResponseStream
            Dim memStream As New IO.MemoryStream
            Dim buffer(1023) As Byte
            Dim bytesIn As Integer
            'ftp download
            DownloadWorker.ReportProgress(1, "Downloading...")
            bytesIn = 1
            Do Until bytesIn < 1 Or DownloadWorker.CancellationPending
                bytesIn = respStream.Read(buffer, 0, 1024)
                If bytesIn > 0 Then
                    memStream.Write(buffer, 0, bytesIn) 'download data to memory before saving to disk
                    Progress.BytesMoved = bytesIn
                End If
            Loop
            e.Cancel = DownloadWorker.CancellationPending
            If Not e.Cancel Then
                dAttachment.DataStream = memStream
                e.Result = dAttachment 'memStream
            Else
                PurgeTempDir()
            End If
        End Using
    End Sub

    Private Sub DownloadWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles DownloadWorker.RunWorkerCompleted
        Try
            WorkerFeedback(False)
            If e.Error Is Nothing Then
                If Not e.Cancelled Then
                    StatusBar("Verifying file...")
                    VerifyAndOpenAttachment(DirectCast(e.Result, Attachment))
                    StatusBar("Idle...")
                Else
                    'Message("The download was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End If
            Else
                If Not ErrHandle(e.Error, System.Reflection.MethodInfo.GetCurrentMethod()) Then EndProgram()
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub VerifyAndOpenAttachment(ByRef Attachment As Attachment)
        Dim strTimeStamp As String = Now.ToString("_hhmmss")
        Dim strFullPath As String = DownloadPath & Attachment.Filename & strTimeStamp & Attachment.Extention
        Using memStream = DirectCast(Attachment.DataStream, MemoryStream)
            Dim FileResultHash As String = GetHashOfIOStream(memStream)
            If FileResultHash = Attachment.MD5 Then
                Using outputStream = IO.File.Create(strFullPath)
                    memStream.CopyTo(outputStream) 'once data is verified we go ahead and copy it to disk
                    outputStream.Close()
                End Using
                If bolDragging Then
                    StatusBar("Drag/Drop...")
                    Dim fileList As New Collections.Specialized.StringCollection
                    fileList.Add(strFullPath)
                    Dim dataObj As New DataObject()
                    dataObj.SetFileDropList(fileList)
                    AttachGrid.DoDragDrop(dataObj, DragDropEffects.All)
                    bolDragging = False
                Else
                    Process.Start(strFullPath)
                End If
                Attachment.Dispose()
            Else
                'something is very wrong
                Logger("FILE VERIFICATION FAILURE: Device:" & Attachment.FolderGUID & "  Filepath: " & strFullPath & "  FileUID: " & Attachment.FileUID & " | Expected hash:" & Attachment.MD5 & " Result hash:" & FileResultHash)
                Dim blah = Message("File verification failed! The file on the database is corrupt or there was a problem reading the data.    Please contact IT about this.", vbOKOnly + MessageBoxIcon.Stop, "Hash Value Mismatch", Me)
                Attachment.Dispose()
                PurgeTempDir()
            End If
        End Using
    End Sub
    Private Function GetSQLAttachment(AttachUID As String) As Attachment
        Dim strQry As String = "SELECT * FROM " & _attachTable.TableName & " WHERE " & _attachTable.FileUID & "='" & AttachUID & "' LIMIT 1"
        Using LocalSQLComm As New MySQL_Comms
            Return New Attachment(LocalSQLComm.Return_SQLTable(strQry), _attachTable)
        End Using
    End Function
    Private Sub OpenTool_Click(sender As Object, e As EventArgs) Handles OpenTool.Click
        If AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString <> "" Then
            OpenAttachment(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString)
        End If
    End Sub
    Private Sub DownloadWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles DownloadWorker.ProgressChanged
        Select Case e.ProgressPercentage
            Case 1
                StatusBar(DirectCast(e.UserState, String))
            Case 2
                statMBPS.Text = Nothing
                ProgressBar1.Visible = False
                ProgressBar1.Value = 0
                ProgTimer.Enabled = False
                Spinner.Visible = False
                StatusBar(DirectCast(e.UserState, String))
                Me.Refresh()
        End Select
    End Sub
    Private Sub ProgTimer_Tick(sender As Object, e As EventArgs) Handles ProgTimer.Tick
        Progress.Tick()
        If Progress.BytesMoved > 0 Then
            statMBPS.Text = Progress.Throughput.ToString("0.00") & " MB/s"

            ProgressBar1.Value = CInt(Progress.Percent)
            If Progress.Percent > 1 Then ProgressBar1.Value = ProgressBar1.Value - 1 'doing this bypasses the progressbar control animation. This way it doesn't lag behind and fills completely
            ProgressBar1.Value = CInt(Progress.Percent)
        Else
            statMBPS.Text = String.Empty
        End If
    End Sub
    Private Sub AttachGrid_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles AttachGrid.CellLeave
        LeaveRow(AttachGrid, GridTheme, e.RowIndex)
    End Sub
    Private Sub AttachGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles AttachGrid.CellDoubleClick
        OpenAttachment(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString)
    End Sub
    Private Sub CopyTextTool_Click(sender As Object, e As EventArgs) Handles CopyTextTool.Click
        Clipboard.SetDataObject(Me.AttachGrid.GetClipboardContent())
    End Sub
    Private Sub AttachGrid_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles AttachGrid.CellEnter
        If Not bolGridFilling Then
            HighlightRow(AttachGrid, GridTheme, e.RowIndex)
        End If
    End Sub
    Private Sub Attachments_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If UploadWorker.IsBusy Or DownloadWorker.IsBusy Then
            e.Cancel = True
            Dim blah = Message("There are active uploads/downloads. Do you wish to cancel the current operation?", MessageBoxIcon.Warning + vbYesNo, "Worker Busy", Me)
            If blah = vbYes Then
                If UploadWorker.IsBusy Then UploadWorker.CancelAsync()
                If DownloadWorker.IsBusy Then DownloadWorker.CancelAsync()
            End If
        End If
        PurgeTempDir()
    End Sub
    Private Sub ToolStripDropDownButton1_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        If UploadWorker.IsBusy Then UploadWorker.CancelAsync()
        If DownloadWorker.IsBusy Then DownloadWorker.CancelAsync()
    End Sub
    Private Sub cmbFolder_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFolder.SelectedIndexChanged
        If Visible Then
            ListAttachments()
            strSelectedFolder = GetDBValue(SibiIndex.AttachFolder, cmbFolder.SelectedIndex)
        End If
    End Sub
    Private Sub cmbMoveFolder_DropDownClosed(sender As Object, e As EventArgs) Handles cmbMoveFolder.DropDownClosed
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        If cmbMoveFolder.SelectedIndex > -1 Then MoveAttachFolder(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString, GetDBValue(SibiIndex.AttachFolder, cmbMoveFolder.SelectedIndex))
    End Sub
    Private Sub RenameStripMenuItem_Click(sender As Object, e As EventArgs) Handles RenameStripMenuItem.Click
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        Dim strCurrentFileName As String = AssetFunc.Get_SQLValue(_attachTable.TableName, _attachTable.FileUID, AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString, _attachTable.FileName)
        Dim strAttachUID As String = AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString
        Dim blah As String = InputBox("Enter new filename.", "Rename", strCurrentFileName)
        If blah = "" Then
            blah = strCurrentFileName
        Else
            RenameAttachement(strAttachUID, Trim(blah))
        End If
    End Sub
    Private Sub AttachGrid_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles AttachGrid.CellMouseUp
        bolDragging = False
    End Sub
    Private Sub chkAllowDrag_CheckedChanged(sender As Object, e As EventArgs) Handles chkAllowDrag.CheckedChanged
        If chkAllowDrag.CheckState = CheckState.Checked Then
            bolAllowDrag = True
            AttachGrid.MultiSelect = False
            AttachGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        Else
            bolAllowDrag = False
            AttachGrid.MultiSelect = True
            AttachGrid.SelectionMode = DataGridViewSelectionMode.CellSelect
        End If
    End Sub
    Private Sub AttachGrid_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles AttachGrid.CellMouseDown
        If e.ColumnIndex >= 0 And e.RowIndex >= 0 Then
            If e.Button = MouseButtons.Right And Not AttachGrid.Item(e.ColumnIndex, e.RowIndex).Selected Then
                AttachGrid.Rows(e.RowIndex).Selected = True
                AttachGrid.CurrentCell = AttachGrid(e.ColumnIndex, e.RowIndex)
            End If
        End If
    End Sub
    Private Sub AttachGrid_MouseDown(sender As Object, e As MouseEventArgs) Handles AttachGrid.MouseDown
        If bolAllowDrag Then
            MouseIsDragging(e.Location)
        End If
    End Sub
    Private MouseStartPos As Point
    Private Function MouseIsDragging(Optional NewStartPos As Point = Nothing, Optional CurrentPos As Point = Nothing) As Boolean
        Dim intMouseMoveThreshold As Integer = 50
        If NewStartPos <> Nothing Then
            MouseStartPos = NewStartPos
        Else
            Dim intDistanceMoved = Math.Sqrt((MouseStartPos.X - CurrentPos.X) ^ 2 + (MouseStartPos.Y - CurrentPos.Y) ^ 2)
            If intDistanceMoved > intMouseMoveThreshold Then
                Return True
            Else
                Return False
            End If
        End If
        Return False
    End Function
    Private Sub AttachGrid_MouseMove(sender As Object, e As MouseEventArgs) Handles AttachGrid.MouseMove
        If bolAllowDrag And Not bolDragging Then
            If e.Button = MouseButtons.Left Then
                If MouseIsDragging(, e.Location) And Not DownloadWorker.IsBusy AndAlso AttachGrid.CurrentRow IsNot Nothing Then
                    bolDragging = True
                    DownloadWorker.RunWorkerAsync(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString)
                End If
            End If
        End If
    End Sub
    Private Sub AttachGrid_DragOver(sender As Object, e As DragEventArgs) Handles AttachGrid.DragOver
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub AttachGrid_DragDrop(sender As Object, e As DragEventArgs) Handles AttachGrid.DragDrop
        If Not bolAllowDrag Then ProcessDrop(e.Data)
    End Sub
    Private Sub ProcessDrop(AttachObject As IDataObject) ' As String()
        Dim File As Object
        Select Case True
            Case AttachObject.GetDataPresent("RenPrivateItem")
                File = CopyAttachement(AttachObject, "RenPrivateItem")
                If Not IsNothing(File) Then
                    WorkerFeedback(True)
                    UploadWorker.RunWorkerAsync(File)
                End If
            Case AttachObject.GetDataPresent("FileDrop")
                File = AttachObject.GetData("FileNameW")
                If Not IsNothing(File) Then
                    WorkerFeedback(True)
                    UploadWorker.RunWorkerAsync(File)
                End If
        End Select
    End Sub
    Private Function CopyAttachement(AttachObject As IDataObject, DataFormat As String) As String()
        Try
            Dim streamFileData As New MemoryStream
            Dim FileName As String
            FileName = GetAttachFileName(AttachObject, DataFormat)
            streamFileData = DirectCast(AttachObject.GetData("FileContents"), MemoryStream)
            streamFileData.Position = 0
            Dim di As DirectoryInfo = Directory.CreateDirectory(DownloadPath)
            Dim output As IO.Stream
            Dim strFullPath(0) As String
            strFullPath(0) = DownloadPath & FileName ' & strTimeStamp
            output = IO.File.Create(strFullPath(0))
            Dim buffer(1023) As Byte
            Dim bytesIn As Integer = 1
            Dim totalBytesIn As Integer
            Do Until bytesIn < 1
                bytesIn = streamFileData.Read(buffer, 0, 1024)
                If bytesIn > 0 Then
                    output.Write(buffer, 0, bytesIn)
                    totalBytesIn += bytesIn 'downloaded bytes
                End If
            Loop
            output.Dispose()
            Return strFullPath
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Private Function GetAttachFileName(AttachObject As IDataObject, DataFormat As String) As String
        Try
            Dim streamFileName As New MemoryStream
            Select Case DataFormat
                Case "RenPrivateItem" '"FileGroupDescriptor"
                    streamFileName = DirectCast(AttachObject.GetData("FileGroupDescriptor"), MemoryStream)
                    streamFileName.Position = 0
                    Dim sr As New StreamReader(streamFileName)
                    Dim fullString As String = sr.ReadToEnd
                    fullString = Replace(fullString, vbNullChar, "")
                    fullString = Replace(fullString, ChrW(1), "")
                    Return fullString
            End Select
        Catch ex As Exception
            Return Nothing
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Private Sub AttachGrid_DragLeave(sender As Object, e As EventArgs) Handles AttachGrid.DragLeave
        bolDragging = False
    End Sub
    Private Enum TransferReturnType
        Success
        Failure
        FileTooLarge
    End Enum
    Private Class FileIcon
        Private Const MAX_PATH As Int32 = 260
        Private Const SHGFI_ICON As Int32 = &H100
        Private Const SHGFI_USEFILEATTRIBUTES As Int32 = &H10
        Private Const FILE_ATTRIBUTE_NORMAL As Int32 = &H80
        Private Structure SHFILEINFO
            Public hIcon As IntPtr
            Public iIcon As Int32
            Public dwAttributes As Int32
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=MAX_PATH)>
            Public szDisplayName As String
            <MarshalAs(UnmanagedType.ByValTStr, SizeConst:=80)>
            Public szTypeName As String
        End Structure
        Private Enum IconSize
            SHGFI_LARGEICON = 0
            SHGFI_SMALLICON = 1
        End Enum
        Private Declare Ansi Function SHGetFileInfo Lib "shell32.dll" (
                    ByVal pszPath As String,
                    ByVal dwFileAttributes As Int32,
                    ByRef psfi As SHFILEINFO,
                    ByVal cbFileInfo As Int32,
                    ByVal uFlags As Int32) As IntPtr
        '<DllImport("user32.dll", SetLastError:=True)>
        Private Shared Function DestroyIcon(ByVal hIcon As IntPtr) As Boolean
        End Function
        Public Shared Function GetFileIcon(ByVal fileExt As String) As Bitmap ', Optional ByVal ICOsize As IconSize = IconSize.SHGFI_SMALLICON
            Try
                Dim ICOSize As IconSize = IconSize.SHGFI_SMALLICON
                Dim shinfo As New SHFILEINFO
                shinfo.szDisplayName = New String(Chr(0), MAX_PATH)
                shinfo.szTypeName = New String(Chr(0), 80)
                SHGetFileInfo(fileExt, FILE_ATTRIBUTE_NORMAL, shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON Or ICOSize Or SHGFI_USEFILEATTRIBUTES)
                Dim bmp As Bitmap = System.Drawing.Icon.FromHandle(shinfo.hIcon).ToBitmap
                DestroyIcon(shinfo.hIcon) ' must destroy icon to avoid GDI leak!
                Return bmp ' return icon as a bitmap
            Catch ex As Exception
                ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            End Try
            Return Nothing
        End Function
    End Class
End Class