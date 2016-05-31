Imports System.ComponentModel
Imports System.IO
Imports MySql.Data.MySqlClient
Class Attachments
    Public bolAdminMode As Boolean = False
    Private AttachQry As String
    Private Const FileSizeMBLimit As Long = 30
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
            'Waiting()
            Me.Cursor = Cursors.AppStarting
            StatusBar("Starting Upload...")
            Spinner.Visible = True
            Me.Refresh()
            UploadWorker.RunWorkerAsync(FilePath)
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
                strQry = "Select UID,attach_file_name,attach_file_type,attach_file_size,attach_upload_date,dev_UID,dev_asset_tag FROM attachments,devices WHERE dev_UID = attach_dev_UID ORDER BY attach_upload_date DESC"
                ListView1.Columns.Clear()
                ListView1.Columns.Add("Filename")
                ListView1.Columns.Add("Size")
                ListView1.Columns.Add("Date")
                ListView1.Columns.Add("Device")
            ElseIf Not bolAdminMode Then
                strQry = "Select UID,attach_file_name,attach_file_type,attach_file_size,attach_upload_date FROM attachments WHERE attach_dev_UID='" & DeviceUID & "' ORDER BY attach_upload_date DESC"
                ListView1.Columns.Clear()
                ListView1.Columns.Add("Filename")
                ListView1.Columns.Add("Size")
                ListView1.Columns.Add("Date")
            End If
            Dim cmd As New MySqlCommand(strQry, GlobalConn)
            reader = cmd.ExecuteReader
            Dim strFullFilename As String
            Dim row As Integer
            ListView1.Items.Clear()
            ReDim AttachIndex(0)
            With reader
                Do While .Read()
                    Dim strFileSizeHuman As String = Math.Round((!attach_file_size / 1024), 1) & " KB"
                    strFullFilename = !attach_file_name &!attach_file_type
                    ListView1.Items.Add(strFullFilename)
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(strFileSizeHuman)
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(!attach_upload_date)
                    If bolAdminMode Then ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(!dev_asset_tag)
                    ReDim Preserve AttachIndex(row)
                    AttachIndex(row).strFilename = !attach_file_name
                    AttachIndex(row).strFileType = !attach_file_type
                    AttachIndex(row).FileSize = !attach_file_size
                    AttachIndex(row).strFileUID = !UID
                    row += 1
                Loop
            End With
            reader.Close()
            If ListView1.Items.Count > 0 Then
                ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            End If
            DoneWaiting()
            Me.Show()
            Exit Sub
        Catch ex As MySqlException
            DoneWaiting()
            ErrHandle(ex.ErrorCode, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Exit Sub
        End Try
    End Sub
    Private Function GetUIDFromIndex(Index As Integer) As String
        Return AttachIndex(Index).strFileUID
    End Function
    Private Sub ListBox1_SelectedIndexChanged(sender As Object, e As EventArgs)
    End Sub
    Private Sub OpenAttachment(AttachUID As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not DownloadWorker.IsBusy Then
            'Waiting()
            Me.Cursor = Cursors.AppStarting
            StatusBar("Starting Download...")
            Spinner.Visible = True
            DownloadWorker.RunWorkerAsync(AttachUID)
        End If
    End Sub
    Private Function DeleteAttachment(AttachUID As String) As Integer
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Function
        End If
        Try
            Waiting()
            Dim cmd As New MySqlCommand
            Dim rows
            Dim strSQLQry As String = "DELETE FROM attachments WHERE UID='" & AttachUID & "'"
            cmd.Connection = GlobalConn
            cmd.CommandText = strSQLQry
            rows = cmd.ExecuteNonQuery()
            DoneWaiting()
            Return rows
            Exit Function
        Catch ex As Exception
            If ErrHandle(ex.HResult, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                DoneWaiting()
                Exit Try
            Else
                EndProgram()
            End If
        End Try
        Return -1
    End Function
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
        'Attachments.StatusLabel.Text = Text
        'Attachments.Refresh()
        Me.Refresh()
    End Sub
    Private Sub Attachments_Load(sender As Object, e As EventArgs) Handles Me.Load
        DoubleBufferedListView(ListView1, True)
        StatusBar("Idle...")
        Waiting()
        FillDeviceInfo()
        ' ListAttachments(CurrentDevice.strGUID)
        DoneWaiting()
    End Sub
    Public Sub FillDeviceInfo()
        txtAssetTag.Text = CurrentDevice.strAssetTag
        txtSerial.Text = CurrentDevice.strSerial
        txtDescription.Text = CurrentDevice.strDescription
    End Sub
    Private Sub ListView1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ListView1.SelectedIndexChanged
    End Sub
    Private Sub ListView1_DoubleClick(sender As Object, e As EventArgs) Handles ListView1.DoubleClick
        OpenAttachment(GetUIDFromIndex(ListView1.FocusedItem.Index))
    End Sub
    Private Sub DeleteAttachmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteAttachmentToolStripMenuItem.Click
        If ListView1.FocusedItem IsNot Nothing Then
            StartAttachDelete()
        End If
    End Sub
    Private Sub StartAttachDelete()
        Dim strFilename As String
        strFilename = AttachIndex(ListView1.FocusedItem.Index).strFilename & AttachIndex(ListView1.FocusedItem.Index).strFileType
        Dim blah
        blah = MsgBox("Are you sure you want to delete '" & strFilename & "'?", vbYesNo + vbQuestion, "Confirm Delete")
        If blah = vbYes Then
            If DeleteAttachment(GetUIDFromIndex(ListView1.FocusedItem.Index)) > 0 Then
                blah = MsgBox("'" & strFilename & "' has been deleted.", vbOKOnly + vbInformation, "Deleted")
                ListAttachments(CurrentDevice.strGUID)
            Else
                blah = MsgBox("Deletion failed!", vbOKOnly + vbExclamation, "Unexpected Results")
            End If
        End If
    End Sub
    Private Sub RightClickMenu_Opening(sender As Object, e As System.ComponentModel.CancelEventArgs) Handles RightClickMenu.Opening
    End Sub
    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        If ListView1.FocusedItem IsNot Nothing Then
            StartAttachDelete()
        End If
    End Sub
    Private Sub cmdOpen_Click(sender As Object, e As EventArgs) Handles cmdOpen.Click
        If ListView1.FocusedItem IsNot Nothing Then
            OpenAttachment(GetUIDFromIndex(ListView1.FocusedItem.Index))
        End If
    End Sub
    Private Sub UploadWorker_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles UploadWorker.DoWork
        Dim ConnID As String = Guid.NewGuid.ToString
        'Dim table As New DataTable
        Dim conn As New MySqlConnection(MySQLConnectString)
        Dim cmd As New MySqlCommand '(strQry, conn)
        UploadWorker.ReportProgress(1, "Connecting...")
        Dim SQL As String
        Dim FileSize As Long
        Dim FileSizeMB As Long
        Dim rawData() As Byte
        Dim fs As FileStream
        Dim FilePath As String = DirectCast(e.Argument, String)
        Try
            fs = New FileStream(FilePath, FileMode.Open, FileAccess.Read)
            FileSize = fs.Length
            FileSizeMB = FileSize / (1024 * 1024)
            If FileSizeMB > FileSizeMBLimit Then
                Dim blah = MsgBox("The file Is too large.   Please select a file less than " & FileSizeMBLimit & "MB.", vbOKOnly + vbExclamation, "Too Large")
                fs.Close()
                fs = Nothing
                Exit Sub
            End If
            Dim strFilename As String = Path.GetFileNameWithoutExtension(FilePath)
            Dim strFileType As String = Path.GetExtension(FilePath)
            rawData = New Byte(FileSize) {}
            fs.Read(rawData, 0, FileSize)
            fs.Close()
            SQL = "INSERT INTO attachments (`attach_dev_UID`, `attach_file_name`, `attach_file_type`, `attach_file_binary`, `attach_file_size`) VALUES(@attach_dev_UID, @attach_file_name, @attach_file_type, @attach_file_binary, @attach_file_size)"
            conn.Open()
            cmd.Connection = conn
            cmd.CommandText = SQL
            cmd.Parameters.AddWithValue("@attach_dev_UID", CurrentDevice.strGUID)
            cmd.Parameters.AddWithValue("@attach_file_name", strFilename)
            cmd.Parameters.AddWithValue("@attach_file_type", strFileType)
            cmd.Parameters.AddWithValue("@attach_file_binary", rawData)
            cmd.Parameters.AddWithValue("@attach_file_size", FileSize)
            UploadWorker.ReportProgress(1, "Uploading...")
            cmd.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
            fs.Dispose()
            rawData = Nothing
            cmd = Nothing
            Spinner.Visible = False
            UploadWorker.ReportProgress(1, "Idle...")
            MessageBox.Show("File uploaded successfully!",
            "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        Catch ex As Exception
            rawData = Nothing
            conn.Close()
            fs.Close()
            fs = Nothing
            Spinner.Visible = False
            UploadWorker.ReportProgress(1, "Idle...")
            If Not ErrHandle(ex.HResult, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then EndProgram()
        End Try
    End Sub
    Private Sub UploadWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles UploadWorker.RunWorkerCompleted
        DoneWaiting()
        StatusBar("Idle...")
        Spinner.Visible = False
        ListAttachments(CurrentDevice.strGUID)
    End Sub
    Private Sub DownloadWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles DownloadWorker.DoWork
        Dim Success As Boolean = False
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim AttachUID As String = DirectCast(e.Argument, String)
        Dim strQry = "Select * FROM attachments WHERE UID='" & AttachUID & "'"
        DownloadWorker.ReportProgress(1, "Connecting...")
        Dim conn As New MySqlConnection(MySQLConnectString)
        Dim cmd As New MySqlCommand(strQry, conn)
        Dim FileSize As UInt32
        Dim strFilename As String, strFiletype As String, strFullPath As String
        Dim di As DirectoryInfo = Directory.CreateDirectory(strTempPath)
        Dim rawData() As Byte
        Try
            conn.Open()
            DownloadWorker.ReportProgress(1, "Downloading...")
            reader = cmd.ExecuteReader
            With reader
                While .Read()
                    strFilename = !attach_file_name
                    strFiletype = !attach_file_type
                    strFullPath = strTempPath & strFilename & strFiletype
                    FileSize = !attach_file_size
                    rawData = New Byte(FileSize) {}
                    .GetBytes(.GetOrdinal("attach_file_binary"), 0, rawData, 0, FileSize)
                End While
            End With
            reader.Close()
            reader.Dispose()
            conn.Close()
            conn.Dispose()
            Dim fs As FileStream = New FileStream(strFullPath, FileMode.Create)
            DownloadWorker.ReportProgress(1, "Creating File...")
            fs.Write(rawData, 0, FileSize)
            fs.Close()
            fs.Dispose()
            rawData = Nothing
            fs.Dispose()
            DownloadWorker.ReportProgress(1, "Idle...")
            Spinner.Visible = False
            Process.Start(strFullPath)
            Success = True
            e.Result = Success
        Catch ex As Exception
            Success = False
            conn.Close()
            conn.Dispose()
            DownloadWorker.ReportProgress(1, "Idle...")
            Spinner.Visible = False
            If Not ErrHandle(ex.HResult, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                EndProgram()
            Else
                e.Result = Success
            End If
        End Try
    End Sub
    Private Sub DownloadWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles DownloadWorker.RunWorkerCompleted
        StatusBar("Idle...")
        Spinner.Visible = False
        DoneWaiting()
        If Not e.Result Then 'if did not complete with success, kill the form.
            Me.Dispose()
        End If
    End Sub
    Private Sub OpenTool_Click(sender As Object, e As EventArgs) Handles OpenTool.Click
        If ListView1.FocusedItem IsNot Nothing Then
            OpenAttachment(GetUIDFromIndex(ListView1.FocusedItem.Index))
        End If
    End Sub
    Private Sub DownloadWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles DownloadWorker.ProgressChanged
        StatusBar(e.UserState)
    End Sub
    Private Sub UploadWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles UploadWorker.ProgressChanged
        StatusBar(e.UserState)
    End Sub
    Private Sub cmdListAll_Click(sender As Object, e As EventArgs)
        If Not ConnectionReady() Then
            Exit Sub
        End If
        Waiting()
        Try
            Dim reader As MySqlDataReader
            Dim table As New DataTable
            Dim strQry = "Select UID,attach_file_name,attach_file_type,attach_file_size,attach_upload_date FROM attachments ORDER BY attach_upload_date DESC"
            Dim cmd As New MySqlCommand(strQry, GlobalConn)
            reader = cmd.ExecuteReader
            Dim strFullFilename As String
            Dim row As Integer
            ListView1.Items.Clear()
            ReDim AttachIndex(0)
            With reader
                Do While .Read()
                    Dim strFileSizeHuman As String = Math.Round((!attach_file_size / 1024), 1) & " KB"
                    strFullFilename = !attach_file_name &!attach_file_type
                    ListView1.Items.Add(strFullFilename)
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(strFileSizeHuman)
                    ListView1.Items(ListView1.Items.Count - 1).SubItems.Add(!attach_upload_date)
                    ReDim Preserve AttachIndex(row)
                    AttachIndex(row).strFilename = !attach_file_name
                    AttachIndex(row).strFileType = !attach_file_type
                    AttachIndex(row).FileSize = !attach_file_size
                    AttachIndex(row).strFileUID = !UID
                    row += 1
                Loop
            End With
            reader.Close()
            If ListView1.Items.Count > 0 Then
                ListView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.ColumnContent)
            End If
            DoneWaiting()
            Exit Sub
        Catch ex As MySqlException
            DoneWaiting()
            ErrHandle(ex.ErrorCode, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Exit Sub
        End Try
    End Sub
    Private Sub Attachments_Shown(sender As Object, e As EventArgs) Handles Me.Shown
    End Sub
End Class