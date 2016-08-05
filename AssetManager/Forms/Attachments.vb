Option Explicit On
Option Compare Binary
Imports System.ComponentModel
Imports System.IO
Imports MySql.Data.MySqlClient
Class Attachments
    Public bolAdminMode As Boolean = False
    Private AttachQry As String
    Private Const FileSizeMBLimit As Short = 150
    Private intProgress As Short
    Private lngBytesMoved As Integer
    Private stpSpeed As New Stopwatch
    Private bolGridFilling As Boolean
    Private progIts As Integer = 0
    Private Structure Attach_Struct
        Public strFilename As String
        Public strFileType As String
        Public FileSize As Int32
        Public strFileUID As String
    End Structure
    Private AttachIndex() As Attach_Struct
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles cmdUpload.Click
        Dim fd As OpenFileDialog = New OpenFileDialog()
        Dim strFileName As String
        fd.ShowHelp = True
        fd.Title = "Select File To Upload - " & FileSizeMBLimit & "MB Limit"
        fd.InitialDirectory = "C:\"
        fd.Filter = "All files (*.*)|*.*|All files (*.*)|*.*"
        fd.FilterIndex = 2
        fd.RestoreDirectory = True
        If fd.ShowDialog() = DialogResult.OK Then
            strFileName = fd.FileName
        Else
            Exit Sub
        End If
        UploadFile(strFileName)
    End Sub
    Private Sub UploadFile(FilePath As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not UploadWorker.IsBusy Then
            StatusBar("Starting Upload...")
            WorkerFeedback(True)
            UploadWorker.RunWorkerAsync(FilePath)
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
    Public Sub ListAttachments(Optional DeviceUID As String = Nothing)
        If Not ConnectionReady() Then
            Exit Sub
        End If
        Waiting()
        Try
            Dim reader As MySqlDataReader
            Dim table As New DataTable
            Dim strQry As String
            If bolAdminMode Then
                strQry = "Select UID,attach_file_name,attach_file_type,attach_file_size,attach_upload_date,attach_file_UID,attach_file_hash,dev_UID,dev_asset_tag FROM dev_attachments,devices WHERE dev_UID = attach_dev_UID ORDER BY attach_upload_date DESC"
                table.Columns.Add("Filename", GetType(String))
                table.Columns.Add("Size", GetType(String))
                table.Columns.Add("Date", GetType(String))
                table.Columns.Add("Device", GetType(String))
                table.Columns.Add("AttachUID", GetType(String))
                table.Columns.Add("MD5", GetType(String))
            ElseIf Not bolAdminMode Then
                strQry = "Select UID,attach_file_name,attach_file_type,attach_file_size,attach_upload_date,attach_file_UID,attach_file_hash FROM dev_attachments WHERE attach_dev_UID='" & DeviceUID & "' ORDER BY attach_upload_date DESC"
                table.Columns.Add("Filename", GetType(String))
                table.Columns.Add("Size", GetType(String))
                table.Columns.Add("Date", GetType(String))
                table.Columns.Add("AttachUID", GetType(String))
                table.Columns.Add("MD5", GetType(String))
            End If
            reader = ReturnSQLReader(strQry)
            Dim strFullFilename As String
            Dim row As Integer
            ReDim AttachIndex(0)
            With reader
                Do While .Read()
                    Dim strFileSizeHuman As String = Math.Round((!attach_file_size / 1024), 1) & " KB"
                    strFullFilename = !attach_file_name & !attach_file_type
                    If bolAdminMode Then
                        table.Rows.Add(strFullFilename, strFileSizeHuman, !attach_upload_date, !dev_asset_tag, !attach_file_UID, !attach_file_hash)
                    Else
                        table.Rows.Add(strFullFilename, strFileSizeHuman, !attach_upload_date, !attach_file_UID, !attach_file_hash)
                    End If
                    ReDim Preserve AttachIndex(row)
                    AttachIndex(row).strFilename = !attach_file_name
                    AttachIndex(row).strFileType = !attach_file_type
                    AttachIndex(row).FileSize = !attach_file_size
                    AttachIndex(row).strFileUID = IIf(IsDBNull(!attach_file_UID), "", !attach_file_UID) '!UID
                    row += 1
                Loop
            End With
            reader.Close()
            bolGridFilling = True
            AttachGrid.DataSource = table
            AttachGrid.Columns("Filename").DefaultCellStyle.Font = New Font("Consolas", 9.75, FontStyle.Bold)
            table.Dispose()
            DoneWaiting()
            Me.Show()
            bolGridFilling = False
            Exit Sub
        Catch ex As Exception
            DoneWaiting()
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Exit Sub
        End Try
    End Sub
    Private Function GetUIDFromIndex(Index As Integer) As String
        Return AttachIndex(Index).strFileUID
    End Function
    Private Function GetIndexFromUID(UID As String) As Integer
        For i As Integer = 0 To AttachIndex.Count
            If AttachIndex(i).strFileUID = UID Then Return i
        Next
        Return -1
    End Function
    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs)
    End Sub
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
        FillDeviceInfo()
        DoneWaiting()
    End Sub
    Public Sub FillDeviceInfo()
        txtAssetTag.Text = CurrentDevice.strAssetTag
        txtSerial.Text = CurrentDevice.strSerial
        txtDescription.Text = CurrentDevice.strDescription
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
        Dim strFilename As String
        Dim i As Integer = GetIndexFromUID(AttachUID)
        strFilename = AttachIndex(i).strFilename & AttachIndex(i).strFileType
        Dim blah
        blah = MsgBox("Are you sure you want to delete '" & strFilename & "'?", vbYesNo + vbQuestion, "Confirm Delete")
        If blah = vbYes Then
            Waiting()
            If DeleteAttachment(AttachIndex(i).strFileUID, AttachmentType.Device) > 0 Then
                ListAttachments(CurrentDevice.strGUID)
                DoneWaiting()
                blah = MsgBox("'" & strFilename & "' has been deleted.", vbOKOnly + vbInformation, "Deleted")
            Else
                DoneWaiting()
                blah = MsgBox("Deletion failed!", vbOKOnly + vbExclamation, "Unexpected Results")
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
        Dim Foldername As String = CurrentDevice.strGUID
        Dim strFileGuid As String = Guid.NewGuid.ToString
        Dim FilePath As String = DirectCast(e.Argument, String)
        Dim strFilename As String = Path.GetFileNameWithoutExtension(FilePath)
        Dim strFileType As String = Path.GetExtension(FilePath)
        Dim strFullFilename As String = Path.GetFileName(FilePath)
        Dim myFileInfo As New FileInfo(FilePath)
        Dim FileSize As Long
        Dim FileSizeMB As Integer
        FileSize = myFileInfo.Length
        FileSizeMB = FileSize / (1024 * 1024)
        If FileSizeMB > FileSizeMBLimit Then
            e.Result = False
            UploadWorker.ReportProgress(2, "Error!")
            Dim blah = MsgBox("The file is too large.   Please select a file less than " & FileSizeMBLimit & "MB.", vbOKOnly + vbExclamation, "Size Limit Exceeded")
            Exit Sub
        End If
        UploadWorker.ReportProgress(1, "Connecting...")
        'sql stuff
        Dim conn As New MySqlConnection(MySQLConnectString)
        Dim cmd As New MySqlCommand
        Dim SQL As String
        Try
            Dim resp As Net.FtpWebResponse = Nothing
            Using resp 'check if device folder exists. create directory if not.
                resp = ReturnFTPResponse("ftp://" & strServerIP & "/attachments", Net.WebRequestMethods.Ftp.ListDirectoryDetails)
                Dim sr As StreamReader = New StreamReader(resp.GetResponseStream(), System.Text.Encoding.ASCII)
                Dim s As String = sr.ReadToEnd()
                If Not s.Contains(Foldername) Then
                    resp = ReturnFTPResponse("ftp://" & strServerIP & "/attachments/" & Foldername, Net.WebRequestMethods.Ftp.MakeDirectory)
                End If
            End Using
            'ftp upload
            Dim buffer(1023) As Byte
            Dim bytesIn As Integer = 1
            Dim totalBytesIn As Integer
            Dim ftpStream As System.IO.FileStream = myFileInfo.OpenRead()
            Dim FileHash As String = GetHashOfStream(ftpStream)
            Dim flLength As Integer = ftpstream.Length
            Dim reqfile As System.IO.Stream = ReturnFTPRequestStream("ftp://" & strServerIP & "/attachments/" & Foldername & "/" & strFileGuid, Net.WebRequestMethods.Ftp.UploadFile) 'request.GetRequestStream
            Dim perc As Short
            stpSpeed.Start()
            UploadWorker.ReportProgress(1, "Uploading...")
            Do Until bytesIn < 1 Or UploadWorker.CancellationPending
                bytesIn = ftpstream.Read(buffer, 0, 1024)
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
            ftpstream.Close()
            ftpstream.Dispose()
            If UploadWorker.CancellationPending Then
                e.Cancel = True
                DeleteFTPAttachment(strFileGuid, Foldername)
            End If
            'update sql table
            If Not UploadWorker.CancellationPending Then
                SQL = "INSERT INTO dev_attachments (`attach_dev_UID`, `attach_file_name`, `attach_file_type`, `attach_file_size`, `attach_file_UID`, `attach_file_hash`) VALUES(@attach_dev_UID, @attach_file_name, @attach_file_type, @attach_file_size, @attach_file_UID, @attach_file_hash)"
                conn.Open()
                cmd.Connection = conn
                cmd.CommandText = SQL
                cmd.Parameters.AddWithValue("@attach_dev_UID", CurrentDevice.strGUID)
                cmd.Parameters.AddWithValue("@attach_file_name", strFilename)
                cmd.Parameters.AddWithValue("@attach_file_type", strFileType)
                cmd.Parameters.AddWithValue("@attach_file_size", FileSize)
                cmd.Parameters.AddWithValue("@attach_file_UID", strFileGuid)
                cmd.Parameters.AddWithValue("@attach_file_hash", FileHash)
                cmd.ExecuteNonQuery()
                conn.Close()
                conn.Dispose()
                cmd.Dispose()
                e.Result = True
            Else
                e.Result = False
            End If
            UploadWorker.ReportProgress(1, "Idle...")
        Catch ex As Exception
            e.Result = False
            conn.Close()
            conn.Dispose()
            UploadWorker.ReportProgress(1, "Idle...")
            If Not ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then EndProgram()
        End Try
    End Sub
    Private Sub UploadWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles UploadWorker.RunWorkerCompleted
        Try
            WorkerFeedback(False)
            ListAttachments(CurrentDevice.strGUID)
            If Not e.Cancelled Then
                If e.Result Then
                    MessageBox.Show("File uploaded successfully!",
              "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                Else
                    MessageBox.Show("File upload failed.",
         "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
                End If
            Else
                MessageBox.Show("The upload was cancelled.",
     "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub DownloadWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles DownloadWorker.DoWork
        Dim Foldername As String
        Dim FileExpectedHash As String
        Dim FileUID As String
        Dim Success As Boolean = False
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim AttachUID As String = DirectCast(e.Argument, String)
        Dim strQry = "Select attach_file_name,attach_file_type,attach_file_size,attach_file_UID,attach_dev_UID,attach_file_hash FROM dev_attachments WHERE attach_file_UID='" & AttachUID & "'"
        DownloadWorker.ReportProgress(1, "Connecting...")
        Dim conn As New MySqlConnection(MySQLConnectString)
        Dim cmd As New MySqlCommand(strQry, conn)
        'Dim FileSize As UInt32
        Dim strFilename As String, strFiletype As String, strFullPath As String
        Dim di As DirectoryInfo = Directory.CreateDirectory(strTempPath)
        Try
            conn.Open()
            reader = cmd.ExecuteReader
            With reader
                While .Read()
                    strFilename = !attach_file_name
                    strFiletype = !attach_file_type
                    strFullPath = strTempPath & strFilename & strFiletype
                    'FileSize = !attach_file_size
                    Foldername = !attach_dev_UID
                    FileExpectedHash = !attach_file_hash
                    FileUID = !attach_file_UID
                End While
            End With
            reader.Close()
            reader.Dispose()
            conn.Close()
            conn.Dispose()
            'FTP STUFF
            Dim buffer(1023) As Byte
            Dim bytesIn As Integer
            Dim totalBytesIn As Integer
            Dim output As IO.Stream
            Dim FtpRequestString As String = "ftp://" & strServerIP & "/attachments/" & Foldername & "/" & AttachUID
            Dim resp As Net.FtpWebResponse = Nothing
            'get file size
            Dim flLength As Int64 = CInt(ReturnFTPResponse(FtpRequestString, Net.WebRequestMethods.Ftp.GetFileSize).ContentLength)
            'setup download
            resp = ReturnFTPResponse(FtpRequestString, Net.WebRequestMethods.Ftp.DownloadFile)
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
                    Process.Start(strFullPath)
                    e.Result = True
                Else
                    'something is very wrong
                    Logger("FILE VERIFICATION FAILURE: Device:" & Foldername & "  Filepath: " & strFullPath & "  FileUID: " & FileUID & " | Expected hash:" & FileExpectedHash & " Result hash:" & FileResultHash)
                    Dim blah = MsgBox("File varification failed! The file on the database is corrupt or there was a problem writing the data do the disk.   The local copy of the attachment will now be deleted for saftey.   Please contact IT about this.", vbOKOnly + MessageBoxIcon.Stop, "Hash Value Mismatch")
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
            If Not ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
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
                End If
            Else
                MessageBox.Show("The download was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Stop)
            End If
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
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
    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        DeleteFTPDeviceFolder(CurrentDevice.strGUID, AttachmentType.Device)
        ListAttachments(CurrentDevice.strGUID)
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
    Private Sub AttachGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles AttachGrid.CellContentClick
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
    Private Sub AttachGrid_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles AttachGrid.CellMouseDown
        On Error Resume Next
        If e.Button = MouseButtons.Right And Not AttachGrid.Item(e.ColumnIndex, e.RowIndex).Selected Then
            AttachGrid.Rows(e.RowIndex).Selected = True
            AttachGrid.CurrentCell = AttachGrid(e.ColumnIndex, e.RowIndex)
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
            Dim blah = MsgBox("There are active uploads/downloads. Do you wish to cancel the current operation?", MessageBoxIcon.Warning + vbYesNo, "Worker Busy")
            If blah = vbYes Then
                If UploadWorker.IsBusy Then UploadWorker.CancelAsync()
                If DownloadWorker.IsBusy Then DownloadWorker.CancelAsync()
            End If
        End If
    End Sub
    Private Sub ToolStripDropDownButton1_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        If UploadWorker.IsBusy Then UploadWorker.CancelAsync()
        If DownloadWorker.IsBusy Then DownloadWorker.CancelAsync()
    End Sub
End Class