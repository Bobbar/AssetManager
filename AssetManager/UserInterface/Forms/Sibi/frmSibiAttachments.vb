Option Explicit On
Option Compare Binary
Imports System.ComponentModel
Imports System.IO
Imports MySql.Data.MySqlClient
Imports System.Text
Imports System.Threading
Class frmSibiAttachments
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
    Public AttachRequest As Request_Info
    Private Structure Attach_Struct
        Public strFilename As String
        Public strFileType As String
        Public FileSize As Int32
        Public strFileUID As String
    End Structure
    Private AttachIndex() As Attach_Struct
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
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not UploadWorker.IsBusy Then
            StatusBar("Starting Upload...")
            WorkerFeedback(True)
            UploadWorker.RunWorkerAsync(Files)
        End If
    End Sub
    Private Sub WorkerFeedback(WorkerRunning As Boolean)
        If WorkerRunning Then
            Me.Cursor = Cursors.AppStarting
            intProgress = 0
            ProgressBar1.Value = 0
            ProgressBar1.Visible = True
            cmdCancel.Visible = True
            Spinner.Visible = True
            ProgTimer.Enabled = True
            Me.Refresh()
        Else
            lngBytesMoved = 0
            Me.Cursor = Cursors.Default
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
            Me.Refresh()
        End If
    End Sub
    Public Sub ListAttachments(Request As Request_Info)
        AttachRequest = Request
        If Not ConnectionReady() Then
            Exit Sub
        End If
        Waiting()
        Try
            Dim reader As MySqlDataReader
            Dim table As New DataTable
            Dim strQry As String
            Select Case GetDBValue(SibiIndex.AttachFolder, cmbFolder.SelectedIndex)
                Case "ALL"
                    strQry = "Select * FROM sibi_attachments WHERE sibi_attach_UID='" & AttachRequest.strUID & "' ORDER BY sibi_attach_timestamp DESC"
                Case Else
                    strQry = "Select * FROM sibi_attachments WHERE sibi_attach_folder='" & GetDBValue(SibiIndex.AttachFolder, cmbFolder.SelectedIndex) & "' AND sibi_attach_UID ='" & AttachRequest.strUID & "' ORDER BY sibi_attach_timestamp DESC"
            End Select
            table.Columns.Add(" ", GetType(Image))
            table.Columns.Add("Filename", GetType(String))
            table.Columns.Add("Size", GetType(String))
            table.Columns.Add("Date", GetType(String))
            table.Columns.Add("Folder", GetType(String))
            table.Columns.Add("AttachUID", GetType(String))
            table.Columns.Add("MD5", GetType(String))
            reader = SQLComms.Return_SQLReader(strQry)
            Dim strFullFilename As String
            Dim row As Integer
            ReDim AttachIndex(0)
            With reader
                Do While .Read()
                    Dim strFileSizeHuman As String = Math.Round((!sibi_attach_file_size / 1024), 1) & " KB"
                    strFullFilename = !sibi_attach_file_name & !sibi_attach_file_type
                    table.Rows.Add(FileIcon(!sibi_attach_file_type), strFullFilename, strFileSizeHuman, !sibi_attach_timestamp, GetHumanValue(SibiIndex.AttachFolder, !sibi_attach_folder), !sibi_attach_file_UID, !sibi_attach_file_hash)
                    ReDim Preserve AttachIndex(row)
                    AttachIndex(row).strFilename = !sibi_attach_file_name
                    AttachIndex(row).strFileType = !sibi_attach_file_type
                    AttachIndex(row).FileSize = !sibi_attach_file_size
                    AttachIndex(row).strFileUID = IIf(IsDBNull(!sibi_attach_file_UID), "", !sibi_attach_file_UID) '!UID
                    row += 1
                Loop
            End With
            reader.Close()
            bolGridFilling = True
            AttachGrid.DataSource = table
            AttachGrid.Columns("Filename").DefaultCellStyle.Font = New Font("Consolas", 9.75, FontStyle.Bold)
            AttachGrid.ClearSelection()
            table.Dispose()
            DoneWaiting()
            Me.Show()
            bolGridFilling = False
            Exit Sub
        Catch ex As Exception
            DoneWaiting()
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Exit Sub
        End Try
    End Sub
    Private Function FileIcon(strExtension As String) As Image
        Return GetFileIcon(strExtension)
    End Function
    Private Function GetUIDFromIndex(Index As Integer) As String
        Return AttachIndex(Index).strFileUID
    End Function
    Private Function GetIndexFromUID(UID As String) As Integer
        For i As Integer = 0 To AttachIndex.Count
            If AttachIndex(i).strFileUID = UID Then Return i
        Next
        Return -1
    End Function
    Private Sub OpenAttachment(AttachUID As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not DownloadWorker.IsBusy Then
            StatusBar("Starting Download...")
            WorkerFeedback(True)
            DownloadWorker.RunWorkerAsync(AttachUID)
        End If
    End Sub
    Private Sub Waiting()
        Me.Cursor = Cursors.WaitCursor
        StatusBar("Processing...")
    End Sub
    Private Sub DoneWaiting()
        Me.Cursor = Cursors.Default
        StatusBar("Idle...")
    End Sub
    Public Sub StatusBar(Text As String)
        StatusLabel.Text = Text
        Me.Refresh()
    End Sub
    Private Sub Attachments_Load(sender As Object, e As EventArgs) Handles Me.Load
        ExtendedMethods.DoubleBuffered(AttachGrid, True)
        StatusBar("Idle...")
        Waiting()
        If CanAccess(AccessGroup.ManageAttachment, UserAccess.intAccessLevel) Then
            cmdUpload.Enabled = True
            cmdDelete.Enabled = True
        Else
            cmdUpload.Enabled = False
            cmdDelete.Enabled = False
        End If
        FillFolderCombos()
        FillInfo()
        DoneWaiting()
    End Sub
    Private Sub FillFolderCombos()
        FillComboBox(SibiIndex.AttachFolder, cmbFolder)
        FillToolComboBox(SibiIndex.AttachFolder, cmbMoveFolder)
    End Sub
    Public Sub FillInfo()
        txtUID.Text = AttachRequest.strUID
        txtRequestNum.Text = AttachRequest.strRequestNumber
        txtDescription.Text = AttachRequest.strDescription
        cmbFolder.SelectedIndex = 0
    End Sub
    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs)
        OpenAttachment(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value)
    End Sub
    Private Sub DeleteAttachmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteAttachmentToolStripMenuItem.Click
        If AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value <> "" Then
            StartAttachDelete(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value)
        End If
    End Sub
    Private Sub StartAttachDelete(AttachUID As String)
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        Dim strFilename As String
        Dim i As Integer = GetIndexFromUID(AttachUID)
        strFilename = AttachIndex(i).strFilename & AttachIndex(i).strFileType
        Dim blah
        blah = Message("Are you sure you want to delete '" & strFilename & "'?", vbYesNo + vbQuestion, "Confirm Delete")
        If blah = vbYes Then
            Waiting()
            If Asset.DeleteSQLAttachment(AttachIndex(i).strFileUID, Entry_Type.Sibi) > 0 Then
                ListAttachments(AttachRequest)
                DoneWaiting()
                ' blah = Message("'" & strFilename & "' has been deleted.", vbOKOnly + vbInformation, "Deleted")
            Else
                DoneWaiting()
                blah = Message("Deletion failed!", vbOKOnly + vbExclamation, "Unexpected Results")
            End If
        End If
    End Sub
    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        If AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value <> "" Then
            StartAttachDelete(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value)
        End If
    End Sub
    Private Sub cmdOpen_Click(sender As Object, e As EventArgs) Handles cmdOpen.Click
        If AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value <> "" Then
            OpenAttachment(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value)
        End If
    End Sub
    Private Sub UploadWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles UploadWorker.DoWork
        'file stuff
        Dim LocalSQLComm As New clsMySQL_Comms
        Dim LocalFTPComm As New clsFTP_Comms
        Dim Foldername As String = AttachRequest.strUID
        Dim strFileGuid As String
        Dim Files() As String = DirectCast(e.Argument, String())
        Dim strFilename As String
        Dim strFileType As String
        Dim strFullFilename As String
        Dim myFileInfo As FileInfo
        Dim FileSize As Long
        Dim FileSizeMB As Integer
        Dim FileNumber As Integer = 1
        Dim conn As MySqlConnection = LocalSQLComm.NewConnection
        Dim cmd As New MySqlCommand
        Try
            For Each file As String In Files
                strFilename = Path.GetFileNameWithoutExtension(file)
                strFileType = Path.GetExtension(file)
                strFullFilename = Path.GetFileName(file)
                myFileInfo = New FileInfo(file)
                strFileGuid = Guid.NewGuid.ToString
                FileSize = myFileInfo.Length
                FileSizeMB = FileSize / (1024 * 1024)
                If FileSizeMB > FileSizeMBLimit Then
                    e.Result = False
                    UploadWorker.ReportProgress(2, "Error!")
                    Dim blah = Message("The file is too large.   Please select a file less than " & FileSizeMBLimit & "MB.", vbOKOnly + vbExclamation, "Size Limit Exceeded")
                    Exit Sub
                End If
                UploadWorker.ReportProgress(1, "Connecting...")
                'sql stuff
                Dim SQL As String
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
                Dim ftpStream As System.IO.FileStream = myFileInfo.OpenRead()
                Dim FileHash As String = GetHashOfStream(ftpStream)
                Dim flLength As Integer = ftpStream.Length
                Dim reqfile As System.IO.Stream = LocalFTPComm.Return_FTPRequestStream("ftp://" & strServerIP & "/attachments/" & Foldername & "/" & strFileGuid, Net.WebRequestMethods.Ftp.UploadFile) 'request.GetRequestStream
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
                    FTP.DeleteFTPAttachment(strFileGuid, Foldername)
                End If
                'update sql table
                If Not UploadWorker.CancellationPending Then
                    SQL = "INSERT INTO sibi_attachments (`sibi_attach_UID`, `sibi_attach_file_name`, `sibi_attach_file_type`, `sibi_attach_file_size`, `sibi_attach_file_UID`, `sibi_attach_file_hash`, `sibi_attach_folder`) VALUES(@attach_dev_UID, @attach_file_name, @attach_file_type, @attach_file_size, @attach_file_UID, @attach_file_hash, @sibi_attach_folder)"
                    conn.Open()
                    cmd.Connection = conn
                    cmd.CommandText = SQL
                    cmd.Parameters.AddWithValue("@attach_dev_UID", AttachRequest.strUID)
                    cmd.Parameters.AddWithValue("@attach_file_name", strFilename)
                    cmd.Parameters.AddWithValue("@attach_file_type", strFileType)
                    cmd.Parameters.AddWithValue("@attach_file_size", FileSize)
                    cmd.Parameters.AddWithValue("@attach_file_UID", strFileGuid)
                    cmd.Parameters.AddWithValue("@attach_file_hash", FileHash)
                    cmd.Parameters.AddWithValue("@sibi_attach_folder", strSelectedFolder)
                    cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                    e.Result = True
                Else
                    e.Result = False
                End If
                FileNumber += 1
                Asset.CloseConnection(conn)
                UploadWorker.ReportProgress(3, "Idle...")
            Next
            Asset.CloseConnection(conn)
            cmd.Dispose()
            UploadWorker.ReportProgress(3, "Idle...")
        Catch ex As Exception
            e.Result = False
            Asset.CloseConnection(conn)
            UploadWorker.ReportProgress(1, "Idle...")
            If Not ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then EndProgram()
        End Try
    End Sub
    Private Sub MoveAttachFolder(AttachUID As String, Folder As String)
        Dim Filename As String = AttachGrid.Item(GetColIndex(AttachGrid, "Filename"), AttachGrid.CurrentRow.Index).Value
        Asset.Update_SQLValue("sibi_attachments", "sibi_attach_folder", Folder, "sibi_attach_file_UID", AttachUID)
        ListAttachments(AttachRequest)
        cmbMoveFolder.SelectedIndex = -1
        cmbMoveFolder.Text = "Select a folder"
        RightClickMenu.Close()
    End Sub
    Private Sub RenameAttachement(AttachUID As String, NewFileName As String)
        Try
            Asset.Update_SQLValue("sibi_attachments", "sibi_attach_file_name", NewFileName, "sibi_attach_file_UID", AttachUID)
            ListAttachments(AttachRequest)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub UploadWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles UploadWorker.RunWorkerCompleted
        Try
            strMultiFileCount = ""
            WorkerFeedback(False)
            ListAttachments(AttachRequest)
            If Not e.Cancelled Then
                If e.Result Then
                    ' MessageBox.Show("File uploaded successfully!",
                    '"Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                Else
                    MessageBox.Show("File upload failed.",
         "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                End If
            Else
                MessageBox.Show("The upload was cancelled.",
     "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub DownloadWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles DownloadWorker.DoWork
        Dim LocalSQLComm As New clsMySQL_Comms
        Dim LocalFTPComm As New clsFTP_Comms
        Dim strTimeStamp As String = Now.ToString("_hhmmss")
        Dim Foldername As String
        Dim FileExpectedHash As String
        Dim FileUID As String
        Dim Success As Boolean = False
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim AttachUID As String = DirectCast(e.Argument, String)
        Dim strQry = "Select * FROM sibi_attachments WHERE sibi_attach_file_UID='" & AttachUID & "'"
        DownloadWorker.ReportProgress(1, "Connecting...")
        Dim conn As MySqlConnection = LocalSQLComm.NewConnection
        Dim cmd As New MySqlCommand(strQry, conn)
        Dim strFilename As String, strFiletype As String, strFullPath As String
        Dim di As DirectoryInfo = Directory.CreateDirectory(strTempPath)
        Try
            conn.Open()
            reader = cmd.ExecuteReader
            With reader
                While .Read()
                    strFilename = !sibi_attach_file_name & strTimeStamp
                    strFiletype = !sibi_attach_file_type
                    strFullPath = strTempPath & strFilename & strFiletype
                    Foldername = !sibi_attach_UID
                    FileExpectedHash = !sibi_attach_file_hash
                    FileUID = !sibi_attach_file_UID
                End While
            End With
            reader.Close()
            reader.Dispose()
            Asset.CloseConnection(conn)
            'FTP STUFF
            Dim buffer(1023) As Byte
            Dim bytesIn As Integer
            Dim totalBytesIn As Integer
            Dim output As IO.Stream
            Dim FtpRequestString As String = "ftp://" & strServerIP & "/attachments/" & Foldername & "/" & AttachUID
            Dim resp As Net.FtpWebResponse = Nothing
            'get file size
            Dim flLength As Int64 = CInt(LocalFTPComm.Return_FTPResponse(FtpRequestString, Net.WebRequestMethods.Ftp.GetFileSize).ContentLength)
            'setup download
            resp = LocalFTPComm.Return_FTPResponse(FtpRequestString, Net.WebRequestMethods.Ftp.DownloadFile)
            Dim respStream As IO.Stream = resp.GetResponseStream
            'ftp download
            ProgTimer.Enabled = True
            DownloadWorker.ReportProgress(1, "Downloading...")
            output = IO.File.Create(strFullPath)
            bytesIn = 1
            Dim perc As Integer = 0
            stpSpeed.Start()
            Do Until bytesIn < 1 Or DownloadWorker.CancellationPending
                bytesIn = respStream.Read(buffer, 0, 1024)
                If bytesIn > 0 Then
                    output.Write(buffer, 0, bytesIn)
                    totalBytesIn += bytesIn 'downloaded bytes
                    lngBytesMoved += bytesIn
                    If flLength > 0 Then
                        perc = (totalBytesIn / flLength) * 100
                        'report progress
                        intProgress = perc
                    End If
                End If
            Loop
            output.Close()
            output.Dispose()
            respStream.Close()
            respStream.Dispose()
            resp.Close()
            resp.Dispose()
            e.Cancel = DownloadWorker.CancellationPending
            If Not e.Cancel Then
                DownloadWorker.ReportProgress(2, "Verifying file...")
                Dim FileResultHash As String = GetHashOfFile(strFullPath)
                If FileResultHash = FileExpectedHash Then
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
                    Dim blah = Message("File varification failed! The file on the database is corrupt or there was a problem writing the data do the disk.   The local copy of the attachment will now be deleted for saftey.   Please contact IT about this.", vbOKOnly + MessageBoxIcon.Stop, "Hash Value Mismatch")
                    PurgeTempDir()
                    'DeleteAttachment(FileUID)
                    e.Result = False
                End If
            Else
                PurgeTempDir()
            End If
        Catch ex As Exception
            e.Result = False
            DownloadWorker.ReportProgress(2, "ERROR!")
            Logger("DOWNLOAD ERROR: " & "Device: " & Foldername & "  Filepath: " & strFullPath & "  FileUID: " & FileUID)
            If Not ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                EndProgram()
            Else
                e.Result = False
            End If
        End Try
    End Sub
    Private Sub DownloadWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles DownloadWorker.RunWorkerCompleted
        Try
            WorkerFeedback(False)
            If Not e.Cancelled Then
                If Not e.Result Then 'if did not complete with success, kill the form.
                    Me.Dispose()
                Else
                End If
            Else
                MessageBox.Show("The download was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub OpenTool_Click(sender As Object, e As EventArgs) Handles OpenTool.Click
        If AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value <> "" Then
            OpenAttachment(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value)
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
                ListAttachments(AttachRequest)
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
    Private Sub HighlightCurrentRow(Row As Integer)
        On Error Resume Next
        If Not bolGridFilling Then
            Dim BackColor As Color = DefGridBC
            Dim SelectColor As Color = DefGridSelCol
            Dim c1 As Color = colHighlightColor 'highlight color
            If Row > -1 Then
                For Each cell As DataGridViewCell In AttachGrid.Rows(Row).Cells
                    Dim c2 As Color = Color.FromArgb(SelectColor.R, SelectColor.G, SelectColor.B)
                    Dim BlendColor As Color
                    BlendColor = Color.FromArgb((CInt(c1.A) + CInt(c2.A)) / 2,
                                                (CInt(c1.R) + CInt(c2.R)) / 2,
                                                (CInt(c1.G) + CInt(c2.G)) / 2,
                                                (CInt(c1.B) + CInt(c2.B)) / 2)
                    cell.Style.SelectionBackColor = BlendColor
                    c2 = Color.FromArgb(BackColor.R, BackColor.G, BackColor.B)
                    BlendColor = Color.FromArgb((CInt(c1.A) + CInt(c2.A)) / 2,
                                                (CInt(c1.R) + CInt(c2.R)) / 2,
                                                (CInt(c1.G) + CInt(c2.G)) / 2,
                                                (CInt(c1.B) + CInt(c2.B)) / 2)
                    cell.Style.BackColor = BlendColor
                Next
            End If
        End If
    End Sub
    Private Sub AttachGrid_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles AttachGrid.CellLeave
        Dim BackColor As Color = DefGridBC
        Dim SelectColor As Color = DefGridSelCol
        If e.RowIndex > -1 Then
            For Each cell As DataGridViewCell In AttachGrid.Rows(e.RowIndex).Cells
                cell.Style.SelectionBackColor = SelectColor
                cell.Style.BackColor = BackColor
            Next
        End If
    End Sub
    Private Sub AttachGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles AttachGrid.CellDoubleClick
        OpenAttachment(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value)
    End Sub
    Private Sub CopyTextTool_Click(sender As Object, e As EventArgs) Handles CopyTextTool.Click
        Clipboard.SetDataObject(Me.AttachGrid.GetClipboardContent())
    End Sub
    Private Sub AttachGrid_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles AttachGrid.CellEnter
        HighlightCurrentRow(e.RowIndex)
    End Sub
    Private Sub Attachments_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If UploadWorker.IsBusy Or DownloadWorker.IsBusy Then
            e.Cancel = True
            Dim blah = Message("There are active uploads/downloads. Do you wish to cancel the current operation?", MessageBoxIcon.Warning + vbYesNo, "Worker Busy")
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
        ListAttachments(AttachRequest)
        strSelectedFolder = GetDBValue(SibiIndex.AttachFolder, cmbFolder.SelectedIndex)
    End Sub
    Private Sub cmbMoveFolder_DropDownClosed(sender As Object, e As EventArgs) Handles cmbMoveFolder.DropDownClosed
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        If cmbMoveFolder.SelectedIndex > -1 Then MoveAttachFolder(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value, GetDBValue(SibiIndex.AttachFolder, cmbMoveFolder.SelectedIndex))
    End Sub
    Private Sub RenameStripMenuItem_Click(sender As Object, e As EventArgs) Handles RenameStripMenuItem.Click
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        Dim strCurrentFileName As String = Asset.Get_SQLValue("sibi_attachments", "sibi_attach_file_UID", AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value, "sibi_attach_file_name") 'AttachGrid.Item(GetColIndex(AttachGrid, "Filename"), AttachGrid.CurrentRow.Index).Value
        Dim strAttachUID As String = AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value
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
                    DownloadWorker.RunWorkerAsync(AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value)
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
    Private Function ProcessDrop(AttachObject As IDataObject) ' As String()
        Dim File() As String
        Select Case True
            Case AttachObject.GetDataPresent("RenPrivateItem")
                File = CopyAttachement(AttachObject, "RenPrivateItem")
                If Not IsNothing(File) Then UploadWorker.RunWorkerAsync(File)
            Case AttachObject.GetDataPresent("FileDrop")
                File = AttachObject.GetData("FileNameW")
                If Not IsNothing(File) Then UploadWorker.RunWorkerAsync(File)
        End Select
    End Function
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
End Class