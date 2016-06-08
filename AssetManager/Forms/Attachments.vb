﻿Imports System.ComponentModel
Imports System.IO
Imports MySql.Data.MySqlClient
Class Attachments
    Public bolAdminMode As Boolean = False
    Private AttachQry As String
    Private Const FileSizeMBLimit As Long = 30
    Private lngProgress As Long
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
            ProgressBar1.Visible = True
            Spinner.Visible = True
            Dim File() As Byte = IO.File.ReadAllBytes(FilePath)
            ProgressBar1.Maximum = File.Length
            Me.Refresh()
            ProgTimer.Enabled = True
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
                strQry = "Select UID,attach_file_name,attach_file_type,attach_file_size,attach_upload_date,attach_file_UID,dev_UID,dev_asset_tag FROM attachments,devices WHERE dev_UID = attach_dev_UID ORDER BY attach_upload_date DESC"
                ListView1.Columns.Clear()
                ListView1.Columns.Add("Filename")
                ListView1.Columns.Add("Size")
                ListView1.Columns.Add("Date")
                ListView1.Columns.Add("Device")
            ElseIf Not bolAdminMode Then
                strQry = "Select UID,attach_file_name,attach_file_type,attach_file_size,attach_upload_date,attach_file_UID FROM attachments WHERE attach_dev_UID='" & DeviceUID & "' ORDER BY attach_upload_date DESC"
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
                    AttachIndex(row).strFileUID = IIf(IsDBNull(!attach_file_UID), "",!attach_file_UID) '!UID
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
            ProgTimer.Enabled = True
            Spinner.Visible = True
            ProgressBar1.Visible = True
            ProgressBar1.Maximum = 100
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
            Dim reader As MySqlDataReader
            Dim strDeviceID As String
            Dim strSQLDevIDQry As String = "SELECT attach_dev_UID FROM attachments WHERE attach_file_UID='" & AttachUID & "'"
            cmd.Connection = GlobalConn
            cmd.CommandText = strSQLDevIDQry
            reader = cmd.ExecuteReader
            With reader
                Do While .Read()
                    strDeviceID = !attach_dev_UID
                Loop
            End With
            reader.Close()
            'Delete FTP Attachment
            If DeleteFTPAttachment(AttachUID, strDeviceID) = Net.FtpStatusCode.FileActionOK Then
                'delete SQL entry
                Dim strSQLDelQry As String = "DELETE FROM attachments WHERE attach_file_UID='" & AttachUID & "'"
                cmd.Connection = GlobalConn
                cmd.CommandText = strSQLDelQry
                rows = cmd.ExecuteNonQuery()
                DoneWaiting()
                Return rows
            End If
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
        If CanAccess("manage_attach") Then
            cmdUpload.Enabled = True
            cmdDelete.Enabled = True
        Else
            cmdUpload.Enabled = False
            cmdDelete.Enabled = False
        End If
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
                ListAttachments(CurrentDevice.strGUID)
                blah = MsgBox("'" & strFilename & "' has been deleted.", vbOKOnly + vbInformation, "Deleted")
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
        'file stuff
        Dim Foldername As String = CurrentDevice.strGUID
        Dim strFileGuid As String = Guid.NewGuid.ToString
        Dim FilePath As String = DirectCast(e.Argument, String)
        Dim strFilename As String = Path.GetFileNameWithoutExtension(FilePath)
        Dim strFileType As String = Path.GetExtension(FilePath)
        Dim strFullFilename As String = Path.GetFileName(FilePath)
        Dim File() As Byte = IO.File.ReadAllBytes(FilePath)
        Dim FileSize As Long
        Dim FileSizeMB As Long
        FileSize = File.Length
        FileSizeMB = FileSize / (1024 * 1024)
        If FileSizeMB > FileSizeMBLimit Then
            e.Result = False
            UploadWorker.ReportProgress(1, "Error!")
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
            Dim request As Net.FtpWebRequest = Net.FtpWebRequest.Create("ftp://" & strServerIP & "/attachments")
            request.Proxy = New Net.WebProxy() 'set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
            request.Credentials = FTPcreds
            request.Method = Net.WebRequestMethods.Ftp.ListDirectoryDetails
            request.KeepAlive = True
            Using resp
                resp = request.GetResponse
                Dim sr As StreamReader = New StreamReader(resp.GetResponseStream(), System.Text.Encoding.ASCII)
                Dim s As String = sr.ReadToEnd()
                If Not s.Contains(Foldername) Then
                    request = Net.FtpWebRequest.Create("ftp://" & strServerIP & "/attachments/" & Foldername)
                    request.Proxy = New Net.WebProxy() 'set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
                    request.Credentials = FTPcreds
                    request.Method = Net.WebRequestMethods.Ftp.MakeDirectory
                    resp = request.GetResponse()
                End If
            End Using
            request = Net.FtpWebRequest.Create("ftp://" & strServerIP & "/attachments/" & Foldername & "/" & strFileGuid)
            request.Proxy = New Net.WebProxy() 'set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
            request.Credentials = FTPcreds
            request.Method = Net.WebRequestMethods.Ftp.UploadFile
            Dim clsStream As IO.Stream = request.GetRequestStream()
            clsStream.Write(File, 0, File.Length)
            'ftp upload
            UploadWorker.ReportProgress(1, "Uploading...")
            For offset As Integer = 0 To File.Length Step 1024
                lngProgress = CType(offset + ProgressBar1.Maximum / File.Length, Integer)
                Dim chunkSize As Integer = File.Length - offset - 1
                If chunkSize > 1024 Then chunkSize = 1024
                clsStream.Write(File, offset, chunkSize)
            Next
            clsStream.Close()
            clsStream.Dispose()
            'update sql table
            SQL = "INSERT INTO attachments (`attach_dev_UID`, `attach_file_name`, `attach_file_type`, `attach_file_size`, `attach_file_UID`) VALUES(@attach_dev_UID, @attach_file_name, @attach_file_type, @attach_file_size, @attach_file_UID)"
            conn.Open()
            cmd.Connection = conn
            cmd.CommandText = SQL
            cmd.Parameters.AddWithValue("@attach_dev_UID", CurrentDevice.strGUID)
            cmd.Parameters.AddWithValue("@attach_file_name", strFilename)
            cmd.Parameters.AddWithValue("@attach_file_type", strFileType)
            cmd.Parameters.AddWithValue("@attach_file_size", FileSize)
            cmd.Parameters.AddWithValue("@attach_file_UID", strFileGuid)
            cmd.ExecuteNonQuery()
            conn.Close()
            conn.Dispose()
            cmd.Dispose()
            UploadWorker.ReportProgress(1, "Idle...")
            e.Result = True
        Catch ex As Exception
            e.Result = False
            conn.Close()
            conn.Dispose()
            'clsStream.Close()
            'clsStream.Dispose()
            UploadWorker.ReportProgress(1, "Idle...")
            If Not ErrHandle(ex.HResult, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then EndProgram()
        End Try
    End Sub
    Private Sub UploadWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles UploadWorker.RunWorkerCompleted
        ProgressBar1.Visible = False
        ProgressBar1.Value = 0
        ProgTimer.Enabled = False
        DoneWaiting()
        StatusBar("Idle...")
        Spinner.Visible = False
        ListAttachments(CurrentDevice.strGUID)
        If e.Result Then
            MessageBox.Show("File uploaded successfully!",
          "Success!", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        Else
            MessageBox.Show("File upload failed.",
         "Failed", MessageBoxButtons.OK, MessageBoxIcon.Asterisk)
        End If
    End Sub
    Private Sub DownloadWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles DownloadWorker.DoWork
        Dim Foldername As String
        Dim Success As Boolean = False
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim AttachUID As String = DirectCast(e.Argument, String)
        Dim strQry = "Select attach_file_name,attach_file_type,attach_file_size,attach_file_UID,attach_dev_UID FROM attachments WHERE attach_file_UID='" & AttachUID & "'"
        DownloadWorker.ReportProgress(1, "Connecting...")
        Dim conn As New MySqlConnection(MySQLConnectString)
        Dim cmd As New MySqlCommand(strQry, conn)
        Dim FileSize As UInt32
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
                    FileSize = !attach_file_size
                    Foldername = !attach_dev_UID
                End While
            End With
            reader.Close()
            reader.Dispose()
            conn.Close()
            conn.Dispose()
        Catch
        End Try
        Try 'FTP STUFF
            Dim buffer(1023) As Byte
            Dim bytesIn As Integer
            Dim totalBytesIn As Integer
            Dim output As IO.Stream
            Dim FtpRequestString As String = "ftp://" & strServerIP & "/attachments/" & Foldername & "/" & AttachUID
            Dim request As Net.FtpWebRequest = Net.FtpWebRequest.Create(FtpRequestString)
            request.Proxy = New Net.WebProxy()
            'Dim creds As Net.NetworkCredential = New Net.NetworkCredential(strFTPUser, strFTPPass)
            Dim resp As Net.FtpWebResponse = Nothing
            'get file size


            'request = Net.FtpWebRequest.Create(FtpRequestString)
            'request.Proxy = New Net.WebProxy() 'set proxy to nothing to bypass .NET auto-detect process. This speeds up the initial connection greatly.
            Dim flLength As Integer
            'request.Credentials = FTPcreds
            'request.Method = Net.WebRequestMethods.Ftp.GetFileSize
            '



            flLength = CInt(ReturnFTPResponse(FtpRequestString, Net.WebRequestMethods.Ftp.GetFileSize)) 'CInt(request.GetResponse.ContentLength)




            'setup download
            request = Net.FtpWebRequest.Create(FtpRequestString)
            request.Proxy = New Net.WebProxy() 'blank proxy again
            request.Credentials = FTPcreds
            request.Method = Net.WebRequestMethods.Ftp.DownloadFile
            resp = request.GetResponse
            Dim respStream As IO.Stream = resp.GetResponseStream
            'ftp download
            ProgTimer.Enabled = True
            DownloadWorker.ReportProgress(1, "Downloading...")
            output = IO.File.Create(strFullPath)
            bytesIn = 1
            Do Until bytesIn < 1
                bytesIn = respStream.Read(buffer, 0, 1024)
                If bytesIn > 0 Then
                    output.Write(buffer, 0, bytesIn)
                    totalBytesIn += bytesIn 'downloaded bytes
                    If flLength > 0 Then
                        Dim perc As Integer = (totalBytesIn / flLength) * 100
                        'report progress
                        lngProgress = perc
                    End If
                End If
            Loop
            output.Close()
            output.Dispose()
            respStream.Close()
            respStream.Dispose()
            resp.Close()
            resp.Dispose()
            DownloadWorker.ReportProgress(1, "Idle...")
            Process.Start(strFullPath)
            e.Result = True
        Catch ex As Exception
            e.Result = False
            DownloadWorker.ReportProgress(1, "Idle...")
            If Not ErrHandle(ex.HResult, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                EndProgram()
            Else
                e.Result = Success
            End If
        End Try
    End Sub
    Private Sub DownloadWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles DownloadWorker.RunWorkerCompleted
        ProgressBar1.Visible = False
        ProgressBar1.Value = 0
        ProgTimer.Enabled = False
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
        ' ProgressBar1.Value = e.ProgressPercentage
        StatusBar(e.UserState)
    End Sub
    Private Sub UploadWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles UploadWorker.ProgressChanged
        'Debug.Print(e.ProgressPercentage)
        'ProgressBar1.Value = e.ProgressPercentage
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
    Private Sub ProgTimer_Tick(sender As Object, e As EventArgs) Handles ProgTimer.Tick
        ' Debug.Print(lngProgress)
        ProgressBar1.Value = lngProgress
    End Sub
End Class