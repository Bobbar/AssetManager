Option Explicit On
Option Compare Binary
Imports System.ComponentModel
Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Text
Imports System.Threading
Class frmAttachments
    Public bolAdminMode As Boolean = False
    Private AttachQry As String
    Private Const FileSizeMBLimit As Short = 150
    Private intProgress As Short
    Private lngBytesMoved As Integer
    Private stpSpeed As New Stopwatch
    Private bolGridFilling As Boolean
    Private progIts As Integer = 0
    Private strSelectedFolder As String
    Private strMultiFileCount As String
    Private bolDragging As Boolean = False
    Private bolAllowDrag As Boolean = False
    Private strDragFilePath As String
    Private AttachRequest As Request_Info
    Private AttachDevice As Device_Info
    Private AttachType As Entry_Type
    Public AttachFolderID As String
    Private AttachTable As String
    Sub New(ParentForm As MyForm, Optional AttachInfo As Object = Nothing)
        InitializeComponent()
        Tag = ParentForm
        Icon = ParentForm.Icon
        GridTheme = ParentForm.GridTheme
        ExtendedMethods.DoubleBuffered(AttachGrid, True)
        StatusBar("Idle...")
        If Not IsNothing(AttachInfo) Then
            If TypeOf AttachInfo Is Request_Info Then
                AttachType = Entry_Type.Sibi
                AttachRequest = AttachInfo
                AttachFolderID = AttachRequest.strUID
                AttachTable = sibi_attachments.TableName
                strSelectedFolder = GetHumanValueFromIndex(SibiIndex.AttachFolder, 0)
                Me.Text = "Sibi Attachements"
                DeviceGroup.Visible = False
                SibiGroup.Dock = DockStyle.Top
                FillFolderCombos()
                FillSibiInfo()
            ElseIf TypeOf AttachInfo Is Device_Info Then
                AttachType = Entry_Type.Device
                AttachDevice = AttachInfo
                AttachFolderID = AttachDevice.strGUID
                AttachTable = dev_attachments.TableName
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
            AttachType = Entry_Type.Device
            AttachTable = dev_attachments.TableName
        End If

        If CanAccess(AccessGroup.ManageAttachment, UserAccess.intAccessLevel) Then
            cmdUpload.Enabled = True
            cmdDelete.Enabled = True
        Else
            cmdUpload.Enabled = False
            cmdDelete.Enabled = False
        End If
        ListAttachments()
    End Sub
    Private AttachIndex As New List(Of Attach_Info)
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles cmdUpload.Click
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
            SetCursor(Cursors.WaitCursor)
            intProgress = 0
            ProgressBar1.Value = 0
            ProgressBar1.Visible = True
            cmdCancel.Visible = True
            Spinner.Visible = True
            ProgTimer.Enabled = True
        Else
            lngBytesMoved = 0
            SetCursor(Cursors.Default)
            stpSpeed.Stop()
            stpSpeed.Reset()
            intProgress = 0
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
        If AttachType = Entry_Type.Sibi Then
            Select Case GetDBValue(SibiIndex.AttachFolder, cmbFolder.SelectedIndex)
                Case "ALL"
                    strQry = "Select * FROM " & AttachTable & " WHERE " & sibi_attachments.FKey & "='" & AttachRequest.strUID & "' ORDER BY " & sibi_attachments.TimeStamp & " DESC"
                Case Else
                    strQry = "Select * FROM " & AttachTable & " WHERE " & sibi_attachments.Folder & "='" & GetDBValue(SibiIndex.AttachFolder, cmbFolder.SelectedIndex) & "' AND " & sibi_attachments.FKey & " ='" & AttachRequest.strUID & "' ORDER BY " & sibi_attachments.TimeStamp & " DESC"
            End Select
        ElseIf AttachType = Entry_Type.Device Then
            If bolAdminMode Then
                strQry = "Select * FROM " & AttachTable & "," & devices.TableName & " WHERE " & devices.DeviceUID & " = " & dev_attachments.FKey & " ORDER BY " & dev_attachments.TimeStamp & " DESC"
            ElseIf Not bolAdminMode Then
                strQry = "Select * FROM " & AttachTable & " WHERE " & dev_attachments.FKey & "='" & AttachDevice.strGUID & "' ORDER BY " & dev_attachments.TimeStamp & " DESC"
            End If
        End If
        Return strQry
    End Function
    Private Function GetTable() As DataTable
        Dim table As New DataTable
        If AttachType = Entry_Type.Sibi Then
            table.Columns.Add(" ", GetType(Image))
            table.Columns.Add("Filename", GetType(String))
            table.Columns.Add("Size", GetType(String))
            table.Columns.Add("Date", GetType(String))
            table.Columns.Add("Folder", GetType(String))
            table.Columns.Add("AttachUID", GetType(String))
            table.Columns.Add("MD5", GetType(String))
        ElseIf AttachType = Entry_Type.Device Then
            If bolAdminMode Then
                table.Columns.Add("Filename", GetType(String))
                table.Columns.Add("Size", GetType(String))
                table.Columns.Add("Date", GetType(String))
                table.Columns.Add("Device", GetType(String))
                table.Columns.Add("AttachUID", GetType(String))
                table.Columns.Add("MD5", GetType(String))
            ElseIf Not bolAdminMode Then
                table.Columns.Add(" ", GetType(Image))
                table.Columns.Add("Filename", GetType(String))
                table.Columns.Add("Size", GetType(String))
                table.Columns.Add("Date", GetType(String))
                table.Columns.Add("AttachUID", GetType(String))
                table.Columns.Add("MD5", GetType(String))
            End If
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
            Using SQLComms As New clsMySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strQry)
                Dim strFullFilename As String
                Dim strFileSizeHuman As String
                Dim NewAttachment As New Attach_Info
                For Each r As DataRow In results.Rows
                    strFileSizeHuman = Math.Round((r.Item(main_attachments.FileSize) / 1024), 1) & " KB"
                    strFullFilename = r.Item(main_attachments.FileName) & r.Item(main_attachments.FileType)
                    If AttachType = Entry_Type.Sibi Then
                        table.Rows.Add(GetFileIcon(r.Item(sibi_attachments.FileType)), strFullFilename, strFileSizeHuman, r.Item(sibi_attachments.TimeStamp), GetHumanValue(SibiIndex.AttachFolder, r.Item(sibi_attachments.Folder)), r.Item(sibi_attachments.FileUID), r.Item(sibi_attachments.FileHash))
                    ElseIf AttachType = Entry_Type.Device Then
                        If bolAdminMode Then
                            table.Rows.Add(strFullFilename, strFileSizeHuman, r.Item(dev_attachments.TimeStamp), r.Item(devices.AssetTag), r.Item(dev_attachments.FileUID), r.Item(dev_attachments.FileHash))
                        Else
                            table.Rows.Add(GetFileIcon(r.Item(dev_attachments.FileType)), strFullFilename, strFileSizeHuman, r.Item(dev_attachments.TimeStamp), r.Item(dev_attachments.FileUID), r.Item(dev_attachments.FileHash))
                        End If
                    End If
                    NewAttachment.Filename = r.Item(main_attachments.FileName)
                    NewAttachment.Extention = r.Item(main_attachments.FileType)
                    NewAttachment.FileSize = r.Item(main_attachments.FileSize)
                    NewAttachment.FileUID = IIf(IsDBNull(r.Item(main_attachments.FileUID)), "", r.Item(main_attachments.FileUID)) '!UID
                    AttachIndex.Add(NewAttachment)
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
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Exit Sub
        End Try
    End Sub
    Private Sub RefreshAttachCount()
        If TypeOf Tag Is frmView Then
            Dim vw As frmView = Tag
            vw.SetAttachCount()
        ElseIf TypeOf Tag Is frmManageRequest Then
            Dim req As frmManageRequest = Tag
            req.SetAttachCount()
        End If
    End Sub
    Private Function GetIndexFromUID(UID As String) As Integer
        For Each Attach As Attach_Info In AttachIndex
            If Attach.FileUID = UID Then Return AttachIndex.IndexOf(Attach)
        Next
        Return -1
    End Function
    Private Sub OpenAttachment(AttachUID As String)
        If Not DownloadWorker.IsBusy Then
            StatusBar("Starting Download...")
            WorkerFeedback(True)
            DownloadWorker.RunWorkerAsync(AttachUID)
        End If
    End Sub
    Private Sub Waiting()
        SetCursor(Cursors.WaitCursor)
        StatusBar("Processing...")
    End Sub
    Private Sub DoneWaiting()
        SetCursor(Cursors.Default)
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
        Dim strFilename As String
        Dim i As Integer = GetIndexFromUID(AttachUID)
        strFilename = AttachIndex(i).Filename & AttachIndex(i).Extention
        Dim blah
        blah = Message("Are you sure you want to delete '" & strFilename & "'?", vbYesNo + vbQuestion, "Confirm Delete", Me)
        If blah = vbYes Then
            Waiting()
            If Asset.DeleteSQLAttachment(AttachIndex(i).FileUID, AttachType) > 0 Then
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
        Dim LocalFTPComm As New clsFTP_Comms
        Dim Foldername As String = AttachFolderID
        Dim Files() As String = DirectCast(e.Argument, String())
        Dim FileSizeMB As Integer
        Dim FileNumber As Integer = 1
        For Each file As String In Files
            Dim CurrentFile As New Attachment(file)
            FileSizeMB = CurrentFile.Filesize / (1024 * 1024)
            If FileSizeMB > FileSizeMBLimit Then
                e.Result = False
                UploadWorker.ReportProgress(2, "Error!")
                Dim blah = Message("The file is too large.   Please select a file less than " & FileSizeMBLimit & "MB.", vbOKOnly + vbExclamation, "Size Limit Exceeded", Me)
                Exit Sub
            End If
            UploadWorker.ReportProgress(1, "Connecting...")
            Dim resp As Net.FtpWebResponse = Nothing
            Using resp 'check if device folder exists. create directory if not.
                resp = LocalFTPComm.Return_FTPResponse("ftp://" & strServerIP & "/attachments", Net.WebRequestMethods.Ftp.ListDirectoryDetails)
                Dim sr As StreamReader = New StreamReader(resp.GetResponseStream(), System.Text.Encoding.ASCII)
                Dim s As String = sr.ReadToEnd()
                If Not s.Contains(Foldername) Then
                    resp = LocalFTPComm.Return_FTPResponse("ftp://" & strServerIP & "/attachments/" & Foldername, Net.WebRequestMethods.Ftp.MakeDirectory)
                End If
            End Using
            'ftp upload
            Dim buffer(1023) As Byte
            Dim bytesIn As Integer = 1
            Dim totalBytesIn As Integer
            Dim ftpStream As System.IO.FileStream = CurrentFile.FileInfo.OpenRead()
            Dim flLength As Integer = ftpStream.Length
            Dim reqfile As System.IO.Stream = LocalFTPComm.Return_FTPRequestStream("ftp://" & strServerIP & "/attachments/" & Foldername & "/" & CurrentFile.FileUID, Net.WebRequestMethods.Ftp.UploadFile) 'request.GetRequestStream
            Dim perc As Short = 0
            stpSpeed.Start()
            UploadWorker.ReportProgress(1, "Uploading... " & FileNumber & " of " & Files.Count)
            lngBytesMoved = 0
            intProgress = 0
            totalBytesIn = 0
            Do Until bytesIn < 1 Or UploadWorker.CancellationPending
                bytesIn = ftpStream.Read(buffer, 0, 1024)
                If bytesIn > 0 Then
                    reqfile.Write(buffer, 0, bytesIn)
                    totalBytesIn += bytesIn
                    lngBytesMoved += bytesIn
                    If flLength > 0 Then
                        perc = (totalBytesIn / flLength) * 100
                        intProgress = perc
                    End If
                End If
            Loop
            reqfile.Close()
            reqfile.Dispose()
            ftpStream.Close()
            ftpStream.Dispose()
            If UploadWorker.CancellationPending Then
                e.Cancel = True
                FTP.DeleteFTPAttachment(CurrentFile.FileUID, Foldername)
                Throw New BackgroundWorkerCancelledException("The upload was cancelled.")
            End If
            'update sql table
            If Not UploadWorker.CancellationPending Then
                Dim SQL As String
                If AttachType = Entry_Type.Sibi Then
                    SQL = "INSERT INTO " & AttachTable & " (" & sibi_attachments.FKey & ", 
" & sibi_attachments.FileName & ",
" & sibi_attachments.FileType & ",
" & sibi_attachments.FileSize & ",
" & sibi_attachments.FileUID & ", 
" & sibi_attachments.FileHash & ", 
" & sibi_attachments.Folder & ") 
VALUES(@" & sibi_attachments.FKey & ", 
@" & sibi_attachments.FileName & ", 
@" & sibi_attachments.FileType & ",
@" & sibi_attachments.FileSize & ", 
@" & sibi_attachments.FileUID & ",
@" & sibi_attachments.FileHash & ",
@" & sibi_attachments.Folder & ")"
                ElseIf AttachType = Entry_Type.Device Then
                    SQL = "INSERT INTO " & AttachTable & " (" & dev_attachments.FKey & ", 
" & dev_attachments.FileName & ",
" & dev_attachments.FileType & ", 
" & dev_attachments.FileSize & ", 
" & dev_attachments.FileUID & ", 
" & dev_attachments.FileHash & ") 
VALUES(@" & dev_attachments.FKey & ",
@" & dev_attachments.FileName & ", 
@" & dev_attachments.FileType & ", 
@" & dev_attachments.FileSize & ",
@" & dev_attachments.FileUID & ",
@" & dev_attachments.FileHash & ")"
                End If
                Using LocalSQLComm As New clsMySQL_Comms, cmd As MySqlCommand = LocalSQLComm.Return_SQLCommand(SQL)
                    cmd.Parameters.AddWithValue("@" & main_attachments.FKey, AttachFolderID)
                    cmd.Parameters.AddWithValue("@" & main_attachments.FileName, CurrentFile.Filename)
                    cmd.Parameters.AddWithValue("@" & main_attachments.FileType, CurrentFile.Extention)
                    cmd.Parameters.AddWithValue("@" & main_attachments.FileSize, CurrentFile.Filesize)
                    cmd.Parameters.AddWithValue("@" & main_attachments.FileUID, CurrentFile.FileUID)
                    cmd.Parameters.AddWithValue("@" & main_attachments.FileHash, CurrentFile.MD5)
                    If AttachType = Entry_Type.Sibi Then cmd.Parameters.AddWithValue("@" & sibi_attachments.Folder, strSelectedFolder)
                    cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                    e.Result = True

                End Using
            Else
                e.Result = False
            End If
            FileNumber += 1
            UploadWorker.ReportProgress(3, "Idle...")
        Next
        UploadWorker.ReportProgress(3, "Idle...")
    End Sub
    Private Sub UploadWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles UploadWorker.ProgressChanged
        Select Case e.ProgressPercentage
            Case 1
                StatusBar(e.UserState)
            Case 2
                stpSpeed.Stop()
                stpSpeed.Reset()
                statMBPS.Text = Nothing
                ProgressBar1.Visible = False
                ProgressBar1.Value = 0
                ProgTimer.Enabled = False
                Spinner.Visible = False
                StatusBar(e.UserState)
                Me.Refresh()
            Case 3
                StatusBar(e.UserState)
                ListAttachments()
        End Select
    End Sub
    Private Sub UploadWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles UploadWorker.RunWorkerCompleted
        Try
            If Not Me.IsDisposed Then
                WorkerFeedback(False)
                If e.Error Is Nothing Then
                    strMultiFileCount = ""
                    If Not e.Cancelled Then
                        If e.Result Then
                            ListAttachments()
                            ' MessageBox.Show("File uploaded successfully!",
                            '"Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                        Else
                            Message("File upload failed.", MessageBoxButtons.OK + MessageBoxIcon.Exclamation, "Failed", Me)
                        End If
                    Else
                        '           MessageBox.Show("The upload was cancelled.",
                        '"Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    End If
                Else
                    DoneWaiting()
                    If Not ErrHandle(e.Error, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then EndProgram()
                    Message("File upload failed.", MessageBoxButtons.OK + MessageBoxIcon.Exclamation, "Failed", Me)
                End If
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub MoveAttachFolder(AttachUID As String, Folder As String)
        Dim Filename As String = AttachGrid.Item(GetColIndex(AttachGrid, "Filename"), AttachGrid.CurrentRow.Index).Value.ToString
        Asset.Update_SQLValue(sibi_attachments.TableName, sibi_attachments.Folder, Folder, sibi_attachments.FileUID, AttachUID)
        ListAttachments()
        cmbMoveFolder.SelectedIndex = -1
        cmbMoveFolder.Text = "Select a folder"
        RightClickMenu.Close()
    End Sub
    Private Sub RenameAttachement(AttachUID As String, NewFileName As String)
        Try
            Asset.Update_SQLValue(AttachTable, main_attachments.FileName, NewFileName, main_attachments.FileUID, AttachUID)
            ListAttachments()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub DownloadWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles DownloadWorker.DoWork
        Dim LocalSQLComm As New clsMySQL_Comms
        Dim LocalFTPComm As New clsFTP_Comms
        Dim strTimeStamp As String = Now.ToString("_hhmmss")
        Dim Foldername As String = Nothing
        Dim FileExpectedHash As String = Nothing
        Dim FileUID As String
        Dim Success As Boolean = False
        Dim results As New DataTable
        Dim table As New DataTable
        Dim AttachUID As String = DirectCast(e.Argument, String)
        Dim strQry As String
        strQry = "Select * FROM " & AttachTable & " WHERE " & main_attachments.FileUID & "='" & AttachUID & "'"
        DownloadWorker.ReportProgress(1, "Connecting...")
        Dim strFilename As String, strFiletype As String, strFullPath As String = Nothing
        Dim di As DirectoryInfo = Directory.CreateDirectory(strTempPath)
        results = LocalSQLComm.Return_SQLTable(strQry)
        For Each r As DataRow In results.Rows
                strFilename = r.Item(main_attachments.FileName) & strTimeStamp
                strFiletype = r.Item(main_attachments.FileType)
                strFullPath = strTempPath & strFilename & strFiletype
                Foldername = r.Item(main_attachments.FKey)
                FileExpectedHash = r.Item(main_attachments.FileHash)
                FileUID = r.Item(main_attachments.FileUID)
            Next
            'FTP STUFF
            Dim buffer(1023) As Byte
            Dim bytesIn As Integer
            Dim totalBytesIn As Integer
            Dim FtpRequestString As String = "ftp://" & strServerIP & "/attachments/" & Foldername & "/" & AttachUID
            'get file size
            Dim flLength As Int64 = CInt(LocalFTPComm.Return_FTPResponse(FtpRequestString, Net.WebRequestMethods.Ftp.GetFileSize).ContentLength)
            'setup download
            Using resp = LocalFTPComm.Return_FTPResponse(FtpRequestString, Net.WebRequestMethods.Ftp.DownloadFile),
                respStream = resp.GetResponseStream,
                outputStream = IO.File.Create(strFullPath),
                memStream As New IO.MemoryStream
                'ftp download
                ProgTimer.Enabled = True
                DownloadWorker.ReportProgress(1, "Downloading...")
                bytesIn = 1
                Dim perc As Integer = 0
                stpSpeed.Start()
                Do Until bytesIn < 1 Or DownloadWorker.CancellationPending
                    bytesIn = respStream.Read(buffer, 0, 1024)
                    If bytesIn > 0 Then
                        memStream.Write(buffer, 0, bytesIn) 'download data to memory before saving to disk
                        totalBytesIn += bytesIn 'downloaded bytes
                        lngBytesMoved += bytesIn
                        If flLength > 0 Then
                            perc = (totalBytesIn / flLength) * 100
                            'report progress
                            intProgress = perc
                        End If
                    End If
                Loop
                e.Cancel = DownloadWorker.CancellationPending
                If Not e.Cancel Then
                    DownloadWorker.ReportProgress(2, "Verifying file...")
                    Dim FileResultHash As String = GetHashOfIOStream(memStream)
                    If FileResultHash = FileExpectedHash Then
                        memStream.CopyTo(outputStream) 'once data is verified we go ahead and copy it to disk
                        outputStream.Close()
                        If bolDragging Then
                            strDragFilePath = strFullPath
                            e.Result = True
                        Else
                            Process.Start(strFullPath)
                            e.Result = True
                        End If
                    Else
                        'something is very wrong
                        Logger("FILE VERIFICATION FAILURE: Device:" & Foldername & "  Filepath: " & strFullPath & "  FileUID: " & FileUID & " | Expected hash:" & FileExpectedHash & " Result hash:" & FileResultHash)
                        Dim blah = Message("File verification failed! The file on the database is corrupt or there was a problem reading the data.    Please contact IT about this.", vbOKOnly + MessageBoxIcon.Stop, "Hash Value Mismatch", Me)
                        PurgeTempDir()
                        e.Result = False
                    End If
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
                    Dim Success As Boolean = CType(e.Result, Boolean)
                    If Not Success Then 'if did not complete with success, kill the form.
                        Me.Dispose()
                    Else
                    End If
                Else
                    'Message("The download was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Stop)
                End If
            Else
                If Not ErrHandle(e.Error, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then EndProgram()
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub OpenTool_Click(sender As Object, e As EventArgs) Handles OpenTool.Click
        If AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString <> "" Then
            OpenAttachment(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString)
        End If
    End Sub
    Private Sub DownloadWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles DownloadWorker.ProgressChanged
        Select Case e.ProgressPercentage
            Case 1
                StatusBar(e.UserState)
            Case 2
                stpSpeed.Stop()
                stpSpeed.Reset()
                statMBPS.Text = Nothing
                ProgressBar1.Visible = False
                ProgressBar1.Value = 0
                ProgTimer.Enabled = False
                Spinner.Visible = False
                StatusBar(e.UserState)
                Me.Refresh()
        End Select
    End Sub
    Private Sub ProgTimer_Tick(sender As Object, e As EventArgs) Handles ProgTimer.Tick
        Dim BytesPerSecond As Single
        Dim ResetCounter As Integer = 40
        If lngBytesMoved > 0 Then
            progIts += 1
            BytesPerSecond = Math.Round((lngBytesMoved / stpSpeed.ElapsedMilliseconds) / 1000, 2)
            statMBPS.Text = BytesPerSecond.ToString("0.00") & " MB/s"
            If progIts > ResetCounter Then
                progIts = 0
                stpSpeed.Restart()
                lngBytesMoved = 0
            End If
        Else
            statMBPS.Text = Nothing
        End If
        ProgressBar1.Value = intProgress
        If intProgress > 1 Then ProgressBar1.Value = ProgressBar1.Value - 1 'doing this bypasses the progressbar control animation. This way it doesn't lag behind and fills completely
        ProgressBar1.Value = intProgress
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
        Dim strCurrentFileName As String = Asset.Get_SQLValue(AttachTable, main_attachments.FileUID, AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString, main_attachments.FileName) 'AttachGrid.Item(GetColIndex(AttachGrid, "Filename"), AttachGrid.CurrentRow.Index).Value.ToString
        Dim strAttachUID As String = AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString
        Dim blah As String = InputBox("Enter new filename.", "Rename", strCurrentFileName)
        If blah Is "" Then
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
        On Error Resume Next
        If e.Button = MouseButtons.Right And Not AttachGrid.Item(e.ColumnIndex, e.RowIndex).Selected Then
            AttachGrid.Rows(e.RowIndex).Selected = True
            AttachGrid.CurrentCell = AttachGrid(e.ColumnIndex, e.RowIndex)
        End If
    End Sub
    Private Sub AttachGrid_MouseDown(sender As Object, e As MouseEventArgs) Handles AttachGrid.MouseDown
        If bolAllowDrag Then
            MouseIsDragging(e.Location)
        End If
    End Sub
    Private MouseStartPos As Point
    Private Function MouseIsDragging(Optional NewStartPos As Point = Nothing, Optional CurrentPos As Point = Nothing) As Boolean
        Dim intMouseMoveThreshold As Integer = 100
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
                If MouseIsDragging(, e.Location) And Not DownloadWorker.IsBusy Then
                    bolDragging = True
                    DownloadWorker.RunWorkerAsync(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString)
                    WaitForDownload()
                    Dim fileList As New Collections.Specialized.StringCollection
                    fileList.Add(strDragFilePath)
                    Dim dataObj As New DataObject
                    dataObj.SetFileDropList(fileList)
                    AttachGrid.DoDragDrop(dataObj, DragDropEffects.All)
                End If
            End If
        End If
    End Sub
    Private Sub WaitForDownload()
        Do While DownloadWorker.IsBusy
            Thread.Sleep(10)
            Application.DoEvents()
        Loop
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
            Dim strTimeStamp As String = Now.ToString("_hhmmss")
            Dim streamFileData As New MemoryStream
            Dim FileName As String
            FileName = GetAttachFileName(AttachObject, DataFormat)
            streamFileData = AttachObject.GetData("FileContents")
            streamFileData.Position = 0
            Dim di As DirectoryInfo = Directory.CreateDirectory(strTempPath)
            Dim output As IO.Stream
            Dim strFullPath(0) As String
            strFullPath(0) = strTempPath & FileName ' & strTimeStamp
            output = IO.File.Create(strFullPath(0))
            Dim buffer(1023) As Byte
            Dim bytesIn As Integer = 1
            Dim totalBytesIn As Integer
            Dim flLength As Int64 = streamFileData.Length
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
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Function
    Private Function GetAttachFileName(AttachObject As IDataObject, DataFormat As String) As String
        Try
            Dim streamFileName As New MemoryStream
            Select Case DataFormat
                Case "RenPrivateItem" '"FileGroupDescriptor"
                    streamFileName = AttachObject.GetData("FileGroupDescriptor")
                    streamFileName.Position = 0
                    Dim sr As New StreamReader(streamFileName)
                    Dim fullString As String = sr.ReadToEnd
                    fullString = Replace(fullString, vbNullChar, "")
                    fullString = Replace(fullString, ChrW(1), "")
                    Return fullString
            End Select
        Catch ex As Exception
            Return Nothing
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Function

    Private Sub AttachGrid_DragLeave(sender As Object, e As EventArgs) Handles AttachGrid.DragLeave
        bolDragging = False
    End Sub
End Class