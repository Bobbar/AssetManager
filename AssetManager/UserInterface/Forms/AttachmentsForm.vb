Option Compare Binary

Imports System.ComponentModel
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Threading
Imports System.Net


Public Class AttachmentsForm

#Region "Fields"

    Private AttachFolderUID As String
    Private Const FileSizeMBLimit As Short = 150
    Private _attachTable As AttachmentsBaseCols
    Private AttachDevice As DeviceStruct
    Private AttachRequest As RequestStruct
    Private bolAllowDrag As Boolean = False
    Private bolDragging As Boolean = False
    Private bolGridFilling As Boolean
    Private taskCancelTokenSource As CancellationTokenSource
    Private TransferTaskRunning As Boolean = False

    ''' <summary>
    ''' "ftp://  strServerIP  /attachments/  CurrentDB  /"
    ''' </summary>
    Private FTPUri As String = "ftp://" & ServerInfo.MySQLServerIP & "/attachments/" & ServerInfo.CurrentDataBase & "/"

    Private MouseStartPos As Point
    Private Progress As New ProgressCounter
    Private strSelectedFolder As String

#End Region

#Region "Constructors"

    Sub New(ParentForm As ThemedForm, AttachTable As AttachmentsBaseCols, Optional AttachInfo As Object = Nothing)
        InitializeComponent()
        Tag = ParentForm
        Icon = ParentForm.Icon
        GridTheme = ParentForm.GridTheme
        AttachGrid.DefaultCellStyle.SelectionBackColor = GridTheme.CellSelectColor
        ExtendedMethods.DoubleBufferedDataGrid(AttachGrid, True)
        SetStatusBar("Idle...")
        _attachTable = AttachTable
        If Not IsNothing(AttachInfo) Then
            If TypeOf AttachInfo Is RequestStruct Then
                AttachRequest = DirectCast(AttachInfo, RequestStruct)
                AttachFolderUID = AttachRequest.GUID
                FormUID = AttachFolderUID
                strSelectedFolder = GetHumanValueFromIndex(SibiIndex.AttachFolder, 0)
                Me.Text = "Sibi Attachements"
                DeviceGroup.Visible = False
                SibiGroup.Dock = DockStyle.Top
                FillFolderCombos()
                FillSibiInfo()
            ElseIf TypeOf AttachInfo Is DeviceStruct Then
                AttachDevice = DirectCast(AttachInfo, DeviceStruct)
                AttachFolderUID = AttachDevice.GUID
                FormUID = AttachFolderUID
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
        Me.Show()
    End Sub

#End Region

#Region "Methods"

    Private Sub FillSibiInfo()
        txtUID.Text = AttachRequest.GUID
        txtRequestNum.Text = AttachRequest.RequestNumber
        txtDescription.Text = AttachRequest.Description
        cmbFolder.SelectedIndex = 0
    End Sub

    Public Function ActiveTransfer() As Boolean
        Return TransferTaskRunning
    End Function

    Public Sub CancelTransfers()
        taskCancelTokenSource.Cancel()
    End Sub

    Private Sub ListAttachments()
        Waiting()
        Try
            Dim strQry As String
            strQry = GetQry()
            Using results As DataTable = DBFunc.GetDatabase.DataTableFromQueryString(strQry),
                table As DataTable = GetTable()
                Dim strFullFilename As String
                Dim strFileSizeHuman As String
                For Each r As DataRow In results.Rows
                    strFileSizeHuman = Math.Round((CInt(r.Item(_attachTable.FileSize)) / 1024), 1) & " KB"
                    strFullFilename = r.Item(_attachTable.FileName).ToString & r.Item(_attachTable.FileType).ToString
                    If TypeOf _attachTable Is SibiAttachmentsCols Then
                        table.Rows.Add(FileIcon.GetFileIcon(r.Item(_attachTable.FileType).ToString), strFullFilename, strFileSizeHuman, r.Item(_attachTable.Timestamp), GetHumanValue(SibiIndex.AttachFolder, r.Item(_attachTable.Folder).ToString), r.Item(_attachTable.FileUID), r.Item(_attachTable.FileHash))
                    Else
                        table.Rows.Add(FileIcon.GetFileIcon(r.Item(_attachTable.FileType).ToString), strFullFilename, strFileSizeHuman, r.Item(_attachTable.Timestamp), r.Item(_attachTable.FileUID), r.Item(_attachTable.FileHash))
                    End If
                Next
                bolGridFilling = True
                AttachGrid.DataSource = table
            End Using
            AttachGrid.Columns("Filename").DefaultCellStyle.Font = New Font("Consolas", 9.75, FontStyle.Bold)
            RefreshAttachCount()
            AttachGrid.ClearSelection()
            If Me.Visible Then bolGridFilling = False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            DoneWaiting()
        End Try
    End Sub

    Private Sub SetStatusBar(Text As String)
        StatusLabel.Text = Text
        StatusStrip1.Update()
    End Sub

    Private Sub Attachments_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If ActiveTransfer() Then
            e.Cancel = True
            Dim blah = Message("There are active uploads/downloads. Do you wish to cancel the current operation?", MessageBoxIcon.Warning + vbYesNo, "Worker Busy", Me)
            If blah = vbYes Then
                CancelTransfers()
            End If
        End If
        PurgeTempDir()
    End Sub

    Private Sub MoveAttachmentFolder()
        If Not CheckForAccess(AccessGroup.ManageAttachment) Then Exit Sub
        If cmbMoveFolder.SelectedIndex > -1 Then MoveAttachFolder(SelectedAttachment, GetDBValue(SibiIndex.AttachFolder, cmbMoveFolder.SelectedIndex))
    End Sub

    Private Sub UploadFileDialog()
        Try
            If Not CheckForAccess(AccessGroup.ManageAttachment) Then Exit Sub
            Using fd As OpenFileDialog = New OpenFileDialog()
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
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Function CopyAttachement(AttachObject As IDataObject, DataFormat As String) As String()
        Try
            Dim FileName As String = GetAttachFileName(AttachObject, DataFormat)
            Dim strFullPath(0) As String
            strFullPath(0) = DownloadPath & FileName
            Directory.CreateDirectory(DownloadPath)
            Using streamFileData = DirectCast(AttachObject.GetData("FileContents"), MemoryStream),
                     outputStream = IO.File.Create(strFullPath(0))
                streamFileData.CopyTo(outputStream)
                Return strFullPath
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

    Private Sub DoneWaiting()
        SetWaitCursor(False, Me)
        SetStatusBar("Idle...")
    End Sub

    Private Async Function DownloadAttachment(AttachUID As String) As Task(Of Attachment)
        If TransferTaskRunning Then Return Nothing
        TransferTaskRunning = True
        Dim dAttachment As New Attachment
        Try
            taskCancelTokenSource = New CancellationTokenSource
            Dim cancelToken As CancellationToken = taskCancelTokenSource.Token
            SetStatusBar("Connecting...")
            Dim LocalFTPComm As New FtpComms
            dAttachment = GetSQLAttachment(AttachUID)
            Dim FtpRequestString As String = FTPUri & dAttachment.FolderGUID & "/" & AttachUID
            'get file size
            Progress = New ProgressCounter
            Progress.BytesToTransfer = CInt(LocalFTPComm.ReturnFtpResponse(FtpRequestString, Net.WebRequestMethods.Ftp.GetFileSize).ContentLength)
            'setup download
            SetStatusBar("Downloading...")
            WorkerFeedback(True)
            dAttachment.DataStream = Await Task.Run(Function()
                                                        Using respStream = LocalFTPComm.ReturnFtpResponse(FtpRequestString, Net.WebRequestMethods.Ftp.DownloadFile).GetResponseStream
                                                            Dim memStream As New IO.MemoryStream
                                                            Dim buffer(1023) As Byte
                                                            Dim bytesIn As Integer
                                                            'ftp download
                                                            bytesIn = 1
                                                            Do Until bytesIn < 1 Or cancelToken.IsCancellationRequested
                                                                bytesIn = respStream.Read(buffer, 0, 1024)
                                                                If bytesIn > 0 Then
                                                                    memStream.Write(buffer, 0, bytesIn) 'download data to memory before saving to disk
                                                                    Progress.BytesMoved = bytesIn
                                                                End If
                                                            Loop
                                                            Return memStream
                                                        End Using
                                                    End Function)

            If Not cancelToken.IsCancellationRequested Then
                If VerifyAttachment(dAttachment) Then
                    Return dAttachment
                End If
            End If
            dAttachment.Dispose()
            Return Nothing
        Catch ex As Exception
            If dAttachment IsNot Nothing Then dAttachment.Dispose()
            Throw ex
        Finally
            If Not GlobalSwitches.ProgramEnding Then
                TransferTaskRunning = False
                WorkerFeedback(False)
            End If
        End Try
    End Function

    Private Sub FillDeviceInfo()
        txtAssetTag.Text = AttachDevice.AssetTag
        txtSerial.Text = AttachDevice.Serial
        txtDeviceDescription.Text = AttachDevice.Description
    End Sub

    Private Sub FillFolderCombos()
        FillComboBox(SibiIndex.AttachFolder, cmbFolder)
        FillToolComboBox(SibiIndex.AttachFolder, cmbMoveFolder)
    End Sub

    Private Function GetAttachFileName(AttachObject As IDataObject, DataFormat As String) As String
        Try
            Select Case DataFormat
                Case "RenPrivateItem"
                    Using streamFileName As MemoryStream = DirectCast(AttachObject.GetData("FileGroupDescriptor"), MemoryStream)
                        streamFileName.Position = 0
                        Dim sr As New StreamReader(streamFileName)
                        Dim fullString As String = sr.ReadToEnd
                        fullString = Replace(fullString, vbNullChar, "")
                        fullString = Replace(fullString, ChrW(1), "")
                        Return fullString
                    End Using
            End Select
            Return Nothing
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

    Private Function GetQry() As String
        Dim strQry As String = ""
        If TypeOf _attachTable Is SibiAttachmentsCols Then
            Select Case GetDBValue(SibiIndex.AttachFolder, cmbFolder.SelectedIndex)
                Case "ALL"
                    strQry = "Select * FROM " & _attachTable.TableName & " WHERE " & _attachTable.FKey & "='" & AttachRequest.GUID & "' ORDER BY " & _attachTable.Timestamp & " DESC"
                Case Else
                    strQry = "Select * FROM " & _attachTable.TableName & " WHERE " & _attachTable.Folder & "='" & GetDBValue(SibiIndex.AttachFolder, cmbFolder.SelectedIndex) & "' AND " & _attachTable.FKey & " ='" & AttachRequest.GUID & "' ORDER BY " & _attachTable.Timestamp & " DESC"
            End Select
        Else
            strQry = "Select * FROM " & _attachTable.TableName & " WHERE " & _attachTable.FKey & "='" & AttachDevice.GUID & "' ORDER BY " & _attachTable.Timestamp & " DESC"
        End If
        Return strQry
    End Function

    Private Function GetSQLAttachment(AttachUID As String) As Attachment
        Dim strQry As String = "SELECT * FROM " & _attachTable.TableName & " WHERE " & _attachTable.FileUID & "='" & AttachUID & "' LIMIT 1"
        Return New Attachment(DBFunc.GetDatabase.DataTableFromQueryString(strQry), _attachTable)
    End Function

    Private Function GetTable() As DataTable
        Dim table As New DataTable
        If TypeOf _attachTable Is SibiAttachmentsCols Then
            table.Columns.Add(" ", GetType(Image))
            table.Columns.Add("Filename", GetType(String))
            table.Columns.Add("Size", GetType(String))
            table.Columns.Add("Date", GetType(String))
            table.Columns.Add("Folder", GetType(String))
            table.Columns.Add("AttachUID", GetType(String))
            table.Columns.Add("MD5", GetType(String))
        Else
            table.Columns.Add(" ", GetType(Image))
            table.Columns.Add("Filename", GetType(String))
            table.Columns.Add("Size", GetType(String))
            table.Columns.Add("Date", GetType(String))
            table.Columns.Add("AttachUID", GetType(String))
            table.Columns.Add("MD5", GetType(String))
        End If
        Return table
    End Function

    Private Sub InsertSQLAttachment(Attachment As Attachment)
        Dim InsertAttachmentParams As New List(Of DBParameter)
        InsertAttachmentParams.Add(New DBParameter(Attachment.AttachTable.FKey, Attachment.FolderGUID))
        InsertAttachmentParams.Add(New DBParameter(Attachment.AttachTable.FileName, Attachment.FileName))
        InsertAttachmentParams.Add(New DBParameter(Attachment.AttachTable.FileType, Attachment.Extension))
        InsertAttachmentParams.Add(New DBParameter(Attachment.AttachTable.FileSize, Attachment.Filesize))
        InsertAttachmentParams.Add(New DBParameter(Attachment.AttachTable.FileUID, Attachment.FileUID))
        InsertAttachmentParams.Add(New DBParameter(Attachment.AttachTable.FileHash, Attachment.MD5))
        If TypeOf Attachment Is SibiAttachment Then
            Dim SibiAttach = DirectCast(Attachment, SibiAttachment)
            InsertAttachmentParams.Add(New DBParameter(Attachment.AttachTable.Folder, SibiAttach.SelectedFolder))
        End If
        DBFunc.GetDatabase.InsertFromParameters(Attachment.AttachTable.TableName, InsertAttachmentParams)
    End Sub

    Private Async Function MakeDirectory(FolderGUID As String) As Task(Of Boolean)
        Return Await Task.Run(Function()
                                  Try
                                      Dim LocalFTPComm As New FtpComms
                                      Using MkDirResp = DirectCast(LocalFTPComm.ReturnFtpResponse(FTPUri & FolderGUID, Net.WebRequestMethods.Ftp.MakeDirectory), FtpWebResponse)
                                          If MkDirResp.StatusCode = FtpStatusCode.PathnameCreated Then
                                              Return True
                                          End If
                                          Return False
                                      End Using
                                  Catch ex As WebException
                                      If ex.Response IsNot Nothing Then
                                          Dim resp = DirectCast(ex.Response, FtpWebResponse)
                                          If resp.StatusCode = FtpStatusCode.ActionNotTakenFileUnavailable Then
                                              'directory already exists
                                              Return True
                                          End If
                                      End If
                                      Return False
                                  End Try
                              End Function)
    End Function

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

    Private Sub MoveAttachFolder(AttachUID As String, Folder As String)
        Try
            AssetFunc.UpdateSqlValue(_attachTable.TableName, _attachTable.Folder, Folder, _attachTable.FileUID, AttachUID)
            ListAttachments()
            cmbMoveFolder.SelectedIndex = -1
            cmbMoveFolder.Text = "Select a folder"
            RightClickMenu.Close()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Function OKFileSize(File As Attachment) As Boolean
        Dim FileSizeMB = CInt(File.Filesize / (1024 * 1024))
        If FileSizeMB > FileSizeMBLimit Then
            Return False
        End If
        Return True
    End Function

    Private Async Sub DownloadAndOpenAttachment(AttachUID As String)
        Try
            If AttachUID = "" Then Exit Sub
            Dim saveAttachment = Await DownloadAttachment(AttachUID)
            If saveAttachment Is Nothing Then Exit Sub
            Dim strFullPath As String = TempPathFilename(saveAttachment)
            SaveAttachmentToDisk(saveAttachment, strFullPath)
            Process.Start(strFullPath)
            SetStatusBar("Idle...")
            saveAttachment.Dispose()
        Catch ex As Exception
            SetStatusBar("Idle...")
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Function TempPathFilename(attachment As Attachment) As String
        Dim strTimeStamp As String = Now.ToString("_hhmmss")
        Return DownloadPath & attachment.FileName & strTimeStamp & attachment.Extension
    End Function

    Private Sub ProcessDrop(AttachObject As IDataObject)
        Try
            Dim File As Object
            Select Case True
                Case AttachObject.GetDataPresent("RenPrivateItem")
                    File = CopyAttachement(AttachObject, "RenPrivateItem")
                    If Not IsNothing(File) Then
                        UploadAttachments(DirectCast(File, String()))
                    End If
                Case AttachObject.GetDataPresent(DataFormats.FileDrop)
                    Dim Files() = CType(AttachObject.GetData(DataFormats.FileDrop), String())
                    If Not IsNothing(Files) Then
                        UploadAttachments(Files)
                    End If
            End Select
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
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

    Private Sub RenameAttachement(AttachUID As String, NewFileName As String)
        Try
            AssetFunc.UpdateSqlValue(_attachTable.TableName, _attachTable.FileName, NewFileName, _attachTable.FileUID, AttachUID)
            ListAttachments()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub RenameAttachmentDialog()
        Try
            If Not CheckForAccess(AccessGroup.ManageAttachment) Then Exit Sub
            Dim strCurrentFileName As String = AssetFunc.GetSqlValue(_attachTable.TableName, _attachTable.FileUID, SelectedAttachment, _attachTable.FileName)
            Dim strAttachUID As String = SelectedAttachment()
            Dim blah As String = InputBox("Enter new filename.", "Rename", strCurrentFileName)
            If blah = "" Then
                blah = strCurrentFileName
            Else
                RenameAttachement(strAttachUID, Trim(blah))
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub DeleteAttachment(AttachUID As String)
        Try
            If Not CheckForAccess(AccessGroup.ManageAttachment) Then Exit Sub
            Dim strFilename As String = AttachGrid.Item(GetColIndex(AttachGrid, "Filename"), AttachGrid.CurrentRow.Index).Value.ToString
            Dim blah = Message("Are you sure you want to delete '" & strFilename & "'?", vbYesNo + vbQuestion, "Confirm Delete", Me)
            If blah = vbYes Then
                Waiting()
                If AssetFunc.DeleteSqlAttachment(GetSQLAttachment(AttachUID)) > 0 Then
                    ListAttachments()
                Else
                    blah = Message("Deletion failed!", vbOKOnly + vbExclamation, "Unexpected Results", Me)
                End If
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            DoneWaiting()
        End Try
    End Sub

    Private Sub UploadFile(Files() As String)
        SetStatusBar("Starting Upload...")
        UploadAttachments(Files)
    End Sub

    Private Async Sub UploadAttachments(files As String())
        If TransferTaskRunning Then Exit Sub
        TransferTaskRunning = True
        Dim CurrentAttachment As New Attachment
        Try
            Dim LocalFTPComm As New FtpComms
            Dim FileNumber As Integer = 1
            taskCancelTokenSource = New CancellationTokenSource
            Dim cancelToken As CancellationToken = taskCancelTokenSource.Token
            WorkerFeedback(True)
            For Each file As String In files
                If TypeOf _attachTable Is SibiAttachmentsCols Then
                    CurrentAttachment = New SibiAttachment(file, AttachFolderUID, strSelectedFolder, _attachTable)
                Else
                    CurrentAttachment = New Attachment(file, AttachFolderUID, _attachTable)
                End If
                If Not OKFileSize(CurrentAttachment) Then
                    CurrentAttachment.Dispose()
                    Message("The file is too large.   Please select a file less than " & FileSizeMBLimit & "MB.", vbOKOnly + vbExclamation, "Size Limit Exceeded", Me)
                    Continue For
                End If
                SetStatusBar("Creating Directory...")
                If Not Await MakeDirectory(CurrentAttachment.FolderGUID) Then
                    CurrentAttachment.Dispose()
                    Message("Error creating FTP directory.", vbOKOnly + vbExclamation, "FTP Upload Error", Me)
                    Exit Sub
                End If
                SetStatusBar("Uploading... " & FileNumber & " of " & files.Count)
                Progress = New ProgressCounter
                Await Task.Run(Sub()
                                   Using FileStream As FileStream = DirectCast(CurrentAttachment.DataStream(), FileStream),
           FTPStream As System.IO.Stream = LocalFTPComm.ReturnFtpRequestStream(FTPUri & CurrentAttachment.FolderGUID & "/" & CurrentAttachment.FileUID, Net.WebRequestMethods.Ftp.UploadFile)
                                       Dim buffer(1023) As Byte
                                       Dim bytesIn As Integer = 1
                                       Progress.BytesToTransfer = CInt(FileStream.Length)
                                       Do Until bytesIn < 1 Or cancelToken.IsCancellationRequested
                                           bytesIn = FileStream.Read(buffer, 0, 1024)
                                           If bytesIn > 0 Then
                                               FTPStream.Write(buffer, 0, bytesIn)
                                               Progress.BytesMoved = bytesIn
                                           End If
                                       Loop
                                   End Using
                               End Sub)
                If cancelToken.IsCancellationRequested Then
                    FTPFunc.DeleteFtpAttachment(CurrentAttachment.FileUID, CurrentAttachment.FolderGUID)
                Else
                    InsertSQLAttachment(CurrentAttachment)
                    FileNumber += 1
                End If
                CurrentAttachment.Dispose()
            Next
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            If Not GlobalSwitches.ProgramEnding And Not Me.IsDisposed Then
                TransferTaskRunning = False
                If CurrentAttachment IsNot Nothing Then CurrentAttachment.Dispose()
                SetStatusBar("Idle...")
                WorkerFeedback(False)
                ListAttachments()
            End If
        End Try
    End Sub

    Private Async Sub DownloadAndSaveAttachment(AttachUID As String)
        Try
            If AttachUID = "" Then Exit Sub
            Dim saveAttachment = Await DownloadAttachment(AttachUID)
            If saveAttachment Is Nothing Then Exit Sub
            SetStatusBar("Idle...")
            If bolDragging Then
                Dim strFullPath As String = TempPathFilename(saveAttachment)
                SaveAttachmentToDisk(saveAttachment, strFullPath)
                SetStatusBar("Drag/Drop...")
                Dim fileList As New Collections.Specialized.StringCollection
                fileList.Add(strFullPath)
                Dim dataObj As New DataObject()
                dataObj.SetFileDropList(fileList)
                AttachGrid.DoDragDrop(dataObj, DragDropEffects.All)
                bolDragging = False
            Else
                Using saveDialog As New SaveFileDialog()
                    saveDialog.Filter = "All files (*.*)|*.*"
                    saveDialog.FileName = saveAttachment.FullFileName
                    If saveDialog.ShowDialog() = DialogResult.OK Then
                        SaveAttachmentToDisk(saveAttachment, saveDialog.FileName)
                    End If
                End Using
            End If
            SetStatusBar("Idle...")
            saveAttachment.Dispose()
        Catch ex As Exception
            SetStatusBar("Idle...")
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Function SaveAttachmentToDisk(attachment As Attachment, savePath As String) As Boolean
        Try
            SetStatusBar("Saving to disk...")
            Directory.CreateDirectory(DownloadPath)
            Using outputStream = IO.File.Create(savePath),
            memStream = DirectCast(attachment.DataStream, MemoryStream)
                memStream.CopyTo(outputStream) 'once data is verified we go ahead and copy it to disk
            End Using
            SetStatusBar("Idle...")
            Return True
        Catch ex As Exception
            SetStatusBar("Idle...")
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

    Private Function VerifyAttachment(attachment As Attachment) As Boolean
        SetStatusBar("Verifying data...")
        Dim FileResultHash = GetHashOfIOStream(DirectCast(attachment.DataStream, MemoryStream))
        If FileResultHash = attachment.MD5 Then
            Return True
        Else
            'something is very wrong
            Logger("FILE VERIFICATION FAILURE: Device:" & attachment.FolderGUID & "  FileUID: " & attachment.FileUID & " | Expected hash:" & attachment.MD5 & " Result hash:" & FileResultHash)
            Message("File verification failed! The file on the database is corrupt or there was a problem reading the data.    Please contact IT about this.", vbOKOnly + MessageBoxIcon.Stop, "Hash Value Mismatch", Me)
            attachment.Dispose()
            PurgeTempDir()
            Return False
        End If
    End Function

    Private Sub Waiting()
        SetWaitCursor(True, Me)
        SetStatusBar("Processing...")
    End Sub

    Private Sub WorkerFeedback(WorkerRunning As Boolean)
        If Not GlobalSwitches.ProgramEnding Then
            If WorkerRunning Then
                ProgressBar1.Value = 0
                ProgressBar1.Visible = True
                cmdCancel.Visible = True
                Spinner.Visible = True
                ProgTimer.Enabled = True
            Else
                Progress = New ProgressCounter
                ProgressBar1.Value = 0
                ProgressBar1.Visible = False
                cmdCancel.Visible = False
                Spinner.Visible = False
                ProgTimer.Enabled = False
                statMBPS.Text = Nothing
                SetStatusBar("Idle...")
                DoneWaiting()
            End If
        End If
    End Sub

    Private Function SelectedAttachment() As String
        Dim AttachUID As String = AttachGrid.Item(GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString
        If AttachUID <> "" Then
            Return AttachUID
        Else
            Throw New Exception("No Attachment Selected.")
        End If
    End Function

    Private Sub AttachmentsForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        AttachGrid.ClearSelection()
        bolGridFilling = False
    End Sub

#Region "Control Event Methods"

    Private Sub AttachGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles AttachGrid.CellDoubleClick
        DownloadAndOpenAttachment(SelectedAttachment)
    End Sub

    Private Sub AttachGrid_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles AttachGrid.CellEnter
        If Not bolGridFilling Then
            HighlightRow(AttachGrid, GridTheme, e.RowIndex)
        End If
    End Sub

    Private Sub AttachGrid_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles AttachGrid.CellLeave
        LeaveRow(AttachGrid, GridTheme, e.RowIndex)
    End Sub

    Private Sub AttachGrid_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles AttachGrid.CellMouseDown
        If e.ColumnIndex >= 0 And e.RowIndex >= 0 Then
            If e.Button = MouseButtons.Right And Not AttachGrid.Item(e.ColumnIndex, e.RowIndex).Selected Then
                AttachGrid.Rows(e.RowIndex).Selected = True
                AttachGrid.CurrentCell = AttachGrid(e.ColumnIndex, e.RowIndex)
            End If
        End If
    End Sub

    Private Sub AttachGrid_CellMouseUp(sender As Object, e As DataGridViewCellMouseEventArgs) Handles AttachGrid.CellMouseUp
        bolDragging = False
    End Sub

    Private Sub AttachGrid_DragDrop(sender As Object, e As DragEventArgs) Handles AttachGrid.DragDrop
        If Not bolAllowDrag Then ProcessDrop(e.Data)
    End Sub

    Private Sub AttachGrid_DragLeave(sender As Object, e As EventArgs) Handles AttachGrid.DragLeave
        bolDragging = False
    End Sub

    Private Sub AttachGrid_DragOver(sender As Object, e As DragEventArgs) Handles AttachGrid.DragOver
        e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub AttachGrid_MouseDown(sender As Object, e As MouseEventArgs) Handles AttachGrid.MouseDown
        If bolAllowDrag Then
            MouseIsDragging(e.Location)
        End If
    End Sub

    Private Sub AttachGrid_MouseMove(sender As Object, e As MouseEventArgs) Handles AttachGrid.MouseMove
        If bolAllowDrag And Not bolDragging Then
            If e.Button = MouseButtons.Left Then
                If MouseIsDragging(, e.Location) AndAlso AttachGrid.CurrentRow IsNot Nothing Then
                    bolDragging = True
                    DownloadAndSaveAttachment(SelectedAttachment)
                End If
            End If
        End If
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

    Private Sub cmbFolder_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbFolder.SelectedIndexChanged
        If Visible Then
            ListAttachments()
            strSelectedFolder = GetDBValue(SibiIndex.AttachFolder, cmbFolder.SelectedIndex)
        End If
    End Sub

    Private Sub cmbMoveFolder_DropDownClosed(sender As Object, e As EventArgs) Handles cmbMoveFolder.DropDownClosed
        MoveAttachmentFolder()
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        DeleteAttachment(SelectedAttachment)
    End Sub

    Private Sub cmdOpen_Click(sender As Object, e As EventArgs) Handles cmdOpen.Click
        DownloadAndOpenAttachment(SelectedAttachment)
    End Sub

    Private Sub cmdUpload_Click(sender As Object, e As EventArgs) Handles cmdUpload.Click
        UploadFileDialog()
    End Sub

    Private Sub CopyTextTool_Click(sender As Object, e As EventArgs) Handles CopyTextTool.Click
        Clipboard.SetDataObject(Me.AttachGrid.GetClipboardContent())
    End Sub

    Private Sub DeleteAttachmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteAttachmentToolStripMenuItem.Click
        DeleteAttachment(SelectedAttachment)
    End Sub

    Private Sub OpenTool_Click(sender As Object, e As EventArgs) Handles OpenTool.Click
        DownloadAndOpenAttachment(SelectedAttachment)
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

    Private Sub RenameStripMenuItem_Click(sender As Object, e As EventArgs) Handles RenameStripMenuItem.Click
        RenameAttachmentDialog()
    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        CancelTransfers()
    End Sub

    Private Sub SaveToMenuItem_Click(sender As Object, e As EventArgs) Handles SaveToMenuItem.Click
        DownloadAndSaveAttachment(SelectedAttachment)
    End Sub

#End Region

#End Region

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

        <DllImport("user32.dll", SetLastError:=True)>
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