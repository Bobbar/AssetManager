Option Compare Binary

Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Runtime.InteropServices
Imports System.Threading

Public Class AttachmentsForm

#Region "Fields"

    Private AttachFolderUID As String
    Private Const FileSizeMBLimit As Short = 150
    Private _attachTable As AttachmentsBaseCols
    Private AttachDevice As DeviceObject
    Private AttachRequest As RequestObject
    Private bolAllowDrag As Boolean = True
    Private bolDragging As Boolean = False
    Private bolGridFilling As Boolean
    Private taskCancelTokenSource As CancellationTokenSource
    Private TransferTaskRunning As Boolean = False
    Private dragDropDataObj As New DataObject

    ''' <summary>
    ''' "ftp://  strServerIP  /attachments/  CurrentDB  /"
    ''' </summary>
    Private FTPUri As String = "ftp://" & ServerInfo.MySQLServerIP & "/attachments/" & ServerInfo.CurrentDataBase.ToString & "/"

    Private MouseStartPos As Point
    Private Progress As New ProgressCounter
    Private PrevSelectedFolder As String
    Private _currentFolder As String = ""

    Private Property CurrentSelectedFolder As String
        Get
            If FolderListView.SelectedItems.Count > 0 Then
                If FolderListView.SelectedItems(0).Index = 0 Then
                    _currentFolder = ""
                Else
                    _currentFolder = FolderListView.SelectedItems(0).Text
                End If
                Return _currentFolder
            Else
                Return _currentFolder
            End If
        End Get
        Set(value As String)
            _currentFolder = value
            SetActiveFolderByName(value)
        End Set
    End Property

#End Region

#Region "Constructors"

    Sub New(ParentForm As ExtendedForm, AttachTable As AttachmentsBaseCols, Optional AttachInfo As Object = Nothing)
        InitializeComponent()
        ImageCaching.CacheControlImages(Me)
        Me.ParentForm = ParentForm
        AttachGrid.DefaultCellStyle.SelectionBackColor = GridTheme.CellSelectColor
        ExtendedMethods.DoubleBufferedDataGrid(AttachGrid, True)
        SetStatusBar("Idle...")
        _attachTable = AttachTable
        If Not IsNothing(AttachInfo) Then
            If TypeOf AttachInfo Is RequestObject Then
                AttachRequest = DirectCast(AttachInfo, RequestObject)
                AttachFolderUID = AttachRequest.GUID
                FormUID = AttachFolderUID
                Me.Text = "Sibi Attachments"
                DeviceGroup.Visible = False
                SibiGroup.Dock = DockStyle.Top
                FillSibiInfo()
            ElseIf TypeOf AttachInfo Is DeviceObject Then
                AttachDevice = DirectCast(AttachInfo, DeviceObject)
                AttachFolderUID = AttachDevice.GUID
                FormUID = AttachFolderUID
                Me.Text = "Device Attachments"
                SibiGroup.Visible = False
                DeviceGroup.Dock = DockStyle.Top
                FillDeviceInfo()
            End If
            PopulateFolderList()
        Else
            SibiGroup.Visible = False
        End If

        If SecurityTools.CanAccess(SecurityTools.AccessGroup.ManageAttachment) Then
            cmdUpload.Enabled = True
            cmdDelete.Enabled = True
        Else
            cmdUpload.Enabled = False
            cmdDelete.Enabled = False
        End If
        Me.Show()
    End Sub

#End Region

#Region "Methods"

    Private Sub SetActiveFolderByName(folderName As String)
        If folderName = "" Then
            FolderListView.Items(0).Selected = True
        Else
            For Each item As ListViewItem In FolderListView.Items
                If item.Text = folderName Then
                    item.Selected = True
                End If
            Next
        End If
        SetFolderListViewStates()
    End Sub

    Private Sub FillSibiInfo()
        ReqPO.Text = AttachRequest.PO
        ReqNumberTextBox.Text = AttachRequest.RequisitionNumber
        txtRequestNum.Text = AttachRequest.RequestNumber
        txtDescription.Text = AttachRequest.Description
        Me.Text += " - " & AttachRequest.Description
    End Sub

    Private Sub FillDeviceInfo()
        txtAssetTag.Text = AttachDevice.AssetTag
        txtSerial.Text = AttachDevice.Serial
        txtDeviceDescription.Text = AttachDevice.Description
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
            Using results As DataTable = DBFactory.GetDatabase.DataTableFromQueryString(strQry),
                table As DataTable = GetTable()
                Dim strFullFilename As String
                Dim strFileSizeHuman As String
                For Each r As DataRow In results.Rows
                    strFileSizeHuman = Math.Round((CInt(r.Item(_attachTable.FileSize)) / 1024), 1) & " KB"
                    strFullFilename = r.Item(_attachTable.FileName).ToString & r.Item(_attachTable.FileType).ToString
                    table.Rows.Add(FileIcon.GetFileIcon(r.Item(_attachTable.FileType).ToString), strFullFilename, strFileSizeHuman, r.Item(_attachTable.Timestamp), r.Item(_attachTable.Folder).ToString, r.Item(_attachTable.FileUID), r.Item(_attachTable.FileHash))
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

    Public Overrides Function OKToClose() As Boolean
        If ActiveTransfer() Then
            Me.WindowState = FormWindowState.Normal
            Me.Activate()
            Dim blah = Message("There are active uploads/downloads. Do you wish to cancel the current operation?", MessageBoxIcon.Warning + vbYesNo, "Worker Busy", Me)
            If blah = vbYes Then
                CancelTransfers()
                Return True
            Else
                Return False
            End If
        End If
        Return True
    End Function

    Private Sub Attachments_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Not OKToClose() Then
            e.Cancel = True
        End If
    End Sub

    Private Sub UploadFileDialog()
        Try
            If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ManageAttachment) Then Exit Sub
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
        Dim FileName As String = GetAttachFileName(AttachObject, DataFormat)
        Dim strFullPath(0) As String
        strFullPath(0) = Paths.DownloadPath & FileName
        Directory.CreateDirectory(Paths.DownloadPath)
        Using streamFileData = DirectCast(AttachObject.GetData("FileContents"), MemoryStream),
                     outputStream = IO.File.Create(strFullPath(0))
            streamFileData.CopyTo(outputStream)
            Return strFullPath
        End Using
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
            TransferTaskRunning = False
            If Not GlobalSwitches.ProgramEnding Then
                WorkerFeedback(False)
            End If
        End Try
    End Function

    Private Sub PopulateFolderList(Optional currentFolder As String = Nothing)
        FolderListView.Items.Clear()
        Dim folders = DBFactory.GetDatabase.DataTableFromQueryString("SELECT DISTINCT " & _attachTable.Folder & " FROM " & _attachTable.TableName & " WHERE " & _attachTable.FKey & "='" & AttachFolderUID & "' ORDER BY " & _attachTable.Folder)
        Dim allFolderItem = New ListViewItem("*All")
        allFolderItem.StateImageIndex = 1
        allFolderItem.Selected = True
        FolderListView.Items.Add(allFolderItem)
        For Each row As DataRow In folders.Rows
            If row.Item(_attachTable.Folder).ToString.Trim <> "" Then
                Dim newFolderItem = New ListViewItem(row.Item(_attachTable.Folder).ToString)
                newFolderItem.StateImageIndex = 0
                newFolderItem.Selected = False
                FolderListView.Items.Add(newFolderItem)
            End If
        Next
        If currentFolder IsNot Nothing Then
            CurrentSelectedFolder = currentFolder
        End If
    End Sub

    Private Function GetAttachFileName(AttachObject As IDataObject, DataFormat As String) As String
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
    End Function

    Private Function GetQry() As String
        Dim strQry As String = ""

        If FolderListView.SelectedIndices.Count > 0 AndAlso FolderListView.SelectedIndices(0) = 0 Then
            strQry = "Select * FROM " & _attachTable.TableName & " WHERE " & _attachTable.FKey & "='" & AttachFolderUID & "' ORDER BY " & _attachTable.Timestamp & " DESC"
        Else
            strQry = "Select * FROM " & _attachTable.TableName & " WHERE " & _attachTable.Folder & "='" & CurrentSelectedFolder & "' AND " & _attachTable.FKey & " ='" & AttachFolderUID & "' ORDER BY " & _attachTable.Timestamp & " DESC"
        End If

        Return strQry
    End Function

    Private Function GetSQLAttachment(AttachUID As String) As Attachment
        Dim strQry As String = "SELECT * FROM " & _attachTable.TableName & " WHERE " & _attachTable.FileUID & "='" & AttachUID & "' LIMIT 1"
        Return New Attachment(DBFactory.GetDatabase.DataTableFromQueryString(strQry), _attachTable)
    End Function

    Private Function GetTable() As DataTable
        Dim table As New DataTable
        table.Columns.Add(" ", GetType(Image))
        table.Columns.Add("Filename", GetType(String))
        table.Columns.Add("Size", GetType(String))
        table.Columns.Add("Date", GetType(String))
        table.Columns.Add("Folder", GetType(String))
        table.Columns.Add("AttachUID", GetType(String))
        table.Columns.Add("MD5", GetType(String))
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
        InsertAttachmentParams.Add(New DBParameter(Attachment.AttachTable.Folder, Attachment.FolderName))
        DBFactory.GetDatabase.InsertFromParameters(Attachment.AttachTable.TableName, InsertAttachmentParams)
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

    Private Sub MoveAttachToFolder(AttachUID As String, Folder As String, Optional isNew As Boolean = False)
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ManageAttachment) Then Exit Sub
        Try
            Waiting()
            RightClickMenu.Close()
            AssetFunc.UpdateSqlValue(_attachTable.TableName, _attachTable.Folder, Folder, _attachTable.FileUID, AttachUID)
            If isNew Then
                PopulateFolderList(Folder)
            Else
                PopulateFolderList(CurrentSelectedFolder)
            End If
            ListAttachments()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            DoneWaiting()
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
            Using saveAttachment = Await DownloadAttachment(AttachUID)
                If saveAttachment Is Nothing Then Exit Sub
                Dim strFullPath As String = TempPathFilename(saveAttachment)
                SaveAttachmentToDisk(saveAttachment, strFullPath)
                Process.Start(strFullPath)
            End Using
        Catch ex As Exception
            Logger("ERROR DOWNLOADING ATTACHMENT: " & Me.FormUID & "/" & AttachUID)
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            SetStatusBar("Idle...")
        End Try
    End Sub

    Private Function TempPathFilename(attachment As Attachment) As String
        Dim strTimeStamp As String = Now.ToString("_hhmmss")
        Return Paths.DownloadPath & attachment.FileName & strTimeStamp & attachment.Extension
    End Function

    Private Sub ProcessAttachGridDrop(dropObject As IDataObject)
        Try
            If DropIsFromOutside(dropObject) Then
                ProcessFileDrop(dropObject, CurrentSelectedFolder)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            bolDragging = False
        End Try
    End Sub

    Private Sub ProcessFolderListDrop(dropObject As IDataObject, folder As String)
        Try
            'If a datagridviewrow is present, and the drop not from outside our form.
            If dropObject.GetDataPresent(GetType(DataGridViewRow)) Then
                If Not DropIsFromOutside(dropObject) Then
                    'Cast out the datarow, get the attach UID, and move the attachment to the new folder.
                    Dim DragRow = DirectCast(dropObject.GetData(GetType(DataGridViewRow)), DataGridViewRow)
                    CurrentSelectedFolder = PrevSelectedFolder
                    MoveAttachToFolder(DragRow.Cells(GridFunctions.GetColIndex(AttachGrid, "AttachUID")).Value.ToString, folder)
                Else
                    'Drop from another form. Process as a normal file drop.
                    ProcessFileDrop(dropObject, folder)
                    CurrentSelectedFolder = folder
                End If
            Else
                'Otherwise the drag originated from windows explorer or Outlook. Process as a normal file drop.
                ProcessFileDrop(dropObject, folder)
                CurrentSelectedFolder = folder
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            bolDragging = False
        End Try
    End Sub

    Private Sub ProcessFileDrop(dropObject As IDataObject, folder As String)
        Dim File As Object
        Select Case True
                    'Outlook data object.
            Case dropObject.GetDataPresent("RenPrivateItem")
                File = CopyAttachement(dropObject, "RenPrivateItem")
                If Not IsNothing(File) Then
                    UploadAttachments(DirectCast(File, String()), folder)
                End If
                    'Explorer data object.
            Case dropObject.GetDataPresent(DataFormats.FileDrop)
                Dim Files() = CType(dropObject.GetData(DataFormats.FileDrop), String())
                If Not IsNothing(Files) Then
                    UploadAttachments(Files, folder)
                End If
        End Select
    End Sub

    Private Function DropIsFromOutside(dropObject As IDataObject) As Boolean
        If dropObject.GetDataPresent("FormID") Then
            Dim FormID As String = DirectCast(dropObject.GetData("FormID"), String)
            If FormID <> Me.FormUID Then
                Return True
            End If
        End If
        Return False
    End Function

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
            If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ManageAttachment) Then Exit Sub
            Dim strCurrentFileName As String = AssetFunc.GetSqlValue(_attachTable.TableName, _attachTable.FileUID, SelectedAttachmentUID, _attachTable.FileName)
            Dim strAttachUID As String = SelectedAttachmentUID()
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
            If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ManageAttachment) Then Exit Sub
            Dim strFilename As String = AttachGrid.Item(GridFunctions.GetColIndex(AttachGrid, "Filename"), AttachGrid.CurrentRow.Index).Value.ToString
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
        UploadAttachments(Files, CurrentSelectedFolder)
    End Sub

    Private Async Sub UploadAttachments(files As String(), folder As String)
        If TransferTaskRunning Then Exit Sub
        TransferTaskRunning = True
        Dim CurrentAttachment As New Attachment
        Try
            Dim LocalFTPComm As New FtpComms
            taskCancelTokenSource = New CancellationTokenSource
            Dim cancelToken As CancellationToken = taskCancelTokenSource.Token
            WorkerFeedback(True)
            For Each file As String In files
                CurrentAttachment = New Attachment(file, AttachFolderUID, folder, _attachTable)
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
                SetStatusBar("Uploading... " & files.ToList.IndexOf(file) + 1 & " of " & files.Count)
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
                End If
                CurrentAttachment.Dispose()
            Next
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            TransferTaskRunning = False
            If Not GlobalSwitches.ProgramEnding And Not Me.IsDisposed Then
                If CurrentAttachment IsNot Nothing Then CurrentAttachment.Dispose()
                SetStatusBar("Idle...")
                WorkerFeedback(False)
                ListAttachments()
            End If
        End Try
    End Sub

    Private Sub StartDragDropAttachment()
        dragDropDataObj = New DataObject()
        dragDropDataObj.SetData(GetType(DataGridViewRow), AttachGrid.CurrentRow)
        dragDropDataObj.SetData("FormID", Me.FormUID)
        AttachGrid.DoDragDrop(dragDropDataObj, DragDropEffects.All)
    End Sub

    Private Async Sub DownloadAndSaveAttachment(attachUID As String)
        Try
            If attachUID = "" Then Exit Sub
            Using saveAttachment = Await DownloadAttachment(attachUID)
                If saveAttachment Is Nothing Then Exit Sub
                Using saveDialog As New SaveFileDialog()
                    saveDialog.Filter = "All files (*.*)|*.*"
                    saveDialog.FileName = saveAttachment.FullFileName
                    If saveDialog.ShowDialog() = DialogResult.OK Then
                        SaveAttachmentToDisk(saveAttachment, saveDialog.FileName)
                    End If
                End Using
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Async Sub AddAttachmentFileToDragDropObject(attachUID As String)
        If Not dragDropDataObj.GetDataPresent(DataFormats.FileDrop) And Not TransferTaskRunning Then
            Waiting()
            Using saveAttachment = Await DownloadAttachment(attachUID)
                If saveAttachment Is Nothing Then Exit Sub
                Dim strFullPath As String = TempPathFilename(saveAttachment)
                Dim fileList As New Collections.Specialized.StringCollection
                fileList.Add(strFullPath)
                dragDropDataObj.SetFileDropList(fileList)
                SaveAttachmentToDisk(saveAttachment, strFullPath)
                SetStatusBar("Drag/Drop...")
                AttachGrid.DoDragDrop(dragDropDataObj, DragDropEffects.All)
            End Using
            bolDragging = False
            DoneWaiting()
        End If
    End Sub

    Private Function SaveAttachmentToDisk(attachment As Attachment, savePath As String) As Boolean
        Try
            SetStatusBar("Saving to disk...")
            Directory.CreateDirectory(Paths.DownloadPath)
            Using outputStream = IO.File.Create(savePath),
            memStream = DirectCast(attachment.DataStream, MemoryStream)
                memStream.CopyTo(outputStream) 'once data is verified we go ahead and copy it to disk
            End Using
            Return True
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        Finally
            SetStatusBar("Idle...")
        End Try
    End Function

    Private Function VerifyAttachment(attachment As Attachment) As Boolean
        SetStatusBar("Verifying data...")
        Dim FileResultHash = SecurityTools.GetMD5OfStream(DirectCast(attachment.DataStream, MemoryStream)) 'GetHashOfIOStream(DirectCast(attachment.DataStream, MemoryStream))
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

    Private Function SelectedAttachmentUID() As String
        Dim AttachUID As String = AttachGrid.Item(GridFunctions.GetColIndex(AttachGrid, "AttachUID"), AttachGrid.CurrentRow.Index).Value.ToString
        If AttachUID <> "" Then
            Return AttachUID
        Else
            Throw New Exception("No Attachment Selected.")
        End If
    End Function

    Private Sub ToggleDragMode()
        bolAllowDrag = Not bolAllowDrag
        AttachGrid.MultiSelect = Not bolAllowDrag
        If AttachGrid.MultiSelect Then
            AttachGrid.SelectionMode = DataGridViewSelectionMode.CellSelect
        Else
            AttachGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
        End If
        AllowDragCheckBox.Checked = bolAllowDrag
    End Sub

    Private Function FolderNameExists(folderName As String) As Boolean
        For Each item As ListViewItem In FolderListView.Items
            If item.Text.ToUpper = folderName.ToUpper Then Return True
        Next
        Return False
    End Function

#Region "Control Event Methods"

    Private Sub AttachmentsForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        AttachGrid.ClearSelection()
        bolGridFilling = False
    End Sub

    Private Sub AttachmentsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        ListAttachments()
    End Sub

    Private Sub AttachGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles AttachGrid.CellDoubleClick
        DownloadAndOpenAttachment(SelectedAttachmentUID)
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
        ProcessAttachGridDrop(e.Data)
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
                    PrevSelectedFolder = CurrentSelectedFolder
                    StartDragDropAttachment()
                End If
            End If
        End If
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        DeleteAttachment(SelectedAttachmentUID)
    End Sub

    Private Sub cmdOpen_Click(sender As Object, e As EventArgs) Handles cmdOpen.Click
        DownloadAndOpenAttachment(SelectedAttachmentUID)
    End Sub

    Private Sub cmdUpload_Click(sender As Object, e As EventArgs) Handles cmdUpload.Click
        UploadFileDialog()
    End Sub

    Private Sub CopyTextTool_Click(sender As Object, e As EventArgs) Handles CopyTextTool.Click
        Clipboard.SetDataObject(Me.AttachGrid.GetClipboardContent())
    End Sub

    Private Sub DeleteAttachmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteAttachmentToolStripMenuItem.Click
        DeleteAttachment(SelectedAttachmentUID)
    End Sub

    Private Sub OpenTool_Click(sender As Object, e As EventArgs) Handles OpenTool.Click
        DownloadAndOpenAttachment(SelectedAttachmentUID)
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
        DownloadAndSaveAttachment(SelectedAttachmentUID)
    End Sub

    Private Sub AttachmentsForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        PurgeTempDir()
    End Sub

    Private Sub FolderListView_ItemActivate(sender As Object, e As EventArgs) Handles FolderListView.ItemActivate
        For Each item As ListViewItem In FolderListView.Items
            item.StateImageIndex = Convert.ToInt32(item.Selected)
        Next
        If FolderListView.SelectedIndices.Count > 0 Then PrevSelectedFolder = CurrentSelectedFolder
        ListAttachments()

    End Sub

    Private Sub FolderListView_ItemSelectionChanged(sender As Object, e As ListViewItemSelectionChangedEventArgs) Handles FolderListView.ItemSelectionChanged
        If bolDragging Then
            SetFolderListViewStates()
        End If
    End Sub

    Private Sub SetFolderListViewStates()
        If FolderListView.Items.Count > 0 Then
            For Each item As ListViewItem In FolderListView.Items
                If item IsNot Nothing Then item.StateImageIndex = Convert.ToInt32(item.Selected)
            Next
        End If
    End Sub

    Private Sub FolderListView_DragOver(sender As Object, e As DragEventArgs) Handles FolderListView.DragOver
        e.Effect = DragDropEffects.Copy
        bolDragging = True
        Dim p As Point = FolderListView.PointToClient(New Point(e.X, e.Y))
        Dim dragToItem As ListViewItem = FolderListView.GetItemAt(p.X, p.Y)
        If dragToItem IsNot Nothing Then
            dragToItem.Selected = True
        End If
    End Sub

    Private Sub FolderListView_DragDrop(sender As Object, e As DragEventArgs) Handles FolderListView.DragDrop
        'Return if the items are not selected in the ListView control.
        If FolderListView.SelectedItems.Count = 0 Then Return
        'Returns the location of the mouse pointer in the ListView control.
        Dim p As Point = FolderListView.PointToClient(New Point(e.X, e.Y))
        'Obtain the item that is located at the specified location of the mouse pointer.
        Dim dragToItem As ListViewItem = FolderListView.GetItemAt(p.X, p.Y)
        If dragToItem Is Nothing Then Return

        If dragToItem.Index = 0 Then
            ProcessFolderListDrop(e.Data, "")
        Else
            ProcessFolderListDrop(e.Data, dragToItem.Text)
        End If
        bolDragging = False
    End Sub

    Private Sub AttachGrid_KeyDown(sender As Object, e As KeyEventArgs) Handles AttachGrid.KeyDown
        If e.KeyCode = Keys.Menu Then
            ToggleDragMode()
        End If
    End Sub

    Private Sub AllowDragCheckBox_Click(sender As Object, e As EventArgs) Handles AllowDragCheckBox.Click
        ToggleDragMode()
    End Sub

    Private Sub NewFolderMenuItem_Click(sender As Object, e As EventArgs) Handles NewFolderMenuItem.Click
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ManageAttachment) Then Exit Sub
        Dim NewFolderName As String = InputBox("Enter new folder name.", "New Folder").Trim
        If Not FolderNameExists(NewFolderName) Then
            MoveAttachToFolder(SelectedAttachmentUID, NewFolderName, True)
        Else
            Message("A folder with that name already exists.", vbOKOnly + vbInformation, "Duplicate Name", Me)
        End If
    End Sub

    Private Sub FolderListView_DragLeave(sender As Object, e As EventArgs) Handles FolderListView.DragLeave
        CurrentSelectedFolder = PrevSelectedFolder
    End Sub

    Private Sub AttachGrid_QueryContinueDrag(ByVal sender As Object, ByVal e As QueryContinueDragEventArgs) Handles AttachGrid.QueryContinueDrag
        Dim grid As DataGridView = CType(sender, DataGridView)
        If grid IsNot Nothing Then
            Dim f As Form = grid.FindForm
            If (((Control.MousePosition.X) < f.DesktopBounds.Left) Or
             ((Control.MousePosition.X) > f.DesktopBounds.Right) Or
             ((Control.MousePosition.Y) < f.DesktopBounds.Top) Or
             ((Control.MousePosition.Y) > f.DesktopBounds.Bottom)) Then

                AddAttachmentFileToDragDropObject(SelectedAttachmentUID)
            Else
                If bolDragging And TransferTaskRunning Then
                    taskCancelTokenSource.Cancel()
                End If

            End If

        End If
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