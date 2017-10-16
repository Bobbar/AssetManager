Imports System.ComponentModel

Public Class SibiManageRequestForm

#Region "Fields"

    Private CurrentRequest As New RequestObject
    Private CurrentHash As String
    Private IsModifying As Boolean = False
    Private IsNewRequest As Boolean = False
    Private bolDragging As Boolean = False
    Private bolFieldsValid As Boolean
    Private bolGridFilling As Boolean = False
    Private DataParser As New DBControlParser(Me)
    Private MouseStartPos As Point
    Private MyMunisToolBar As New MunisToolBar(Me)
    Private TitleText As String = "Manage Request"
    Private MyWindowList As New WindowList(Me)
    Private PrevWindowState As Integer

#End Region

#Region "Constructors"

    Sub New(parentForm As ExtendedForm, requestUID As String)
        InitializeComponent()
        InitForm(parentForm, requestUID)
        OpenRequest(requestUID)
    End Sub

    Sub New(parentForm As ExtendedForm)
        InitializeComponent()
        InitForm(parentForm)
        Text += " - *New Request*"
        NewRequest()
    End Sub

#End Region

#Region "Methods"

    Private Function CancelModify() As Boolean
        If IsModifying Then
            Me.WindowState = FormWindowState.Normal
            Me.Activate()
            Dim blah = Message("Are you sure you want to discard all changes?", vbOKCancel + vbQuestion, "Discard Changes", Me)
            If blah = vbOK Then
                If IsNewRequest Then
                    Return True
                Else
                    HideEditControls()
                    OpenRequest(CurrentRequest.GUID)
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Private Sub ClearAll()
        ClearControls(Me)
        ResetBackColors(Me)
        HideEditControls()
        dgvNotes.DataSource = Nothing
        FillCombos()
        pnlCreate.Visible = False
        CurrentRequest = Nothing
        DisableControls()
        ToolStrip.BackColor = colSibiToolBarColor
        IsModifying = False
        IsNewRequest = False
        fieldErrorIcon.Clear()
        SetAttachCount()
        SetMunisStatus()

    End Sub

    Private Sub NewRequest()
        Try
            SetWaitCursor(True, Me)
            If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.AddSibi) Then
                If Not Me.Visible Then
                    Me.Dispose()
                End If
                Exit Sub
            End If
            If IsModifying Then
                Dim blah = Message("All current changes will be lost. Are you sure you want to create a new request?", vbOKCancel + vbQuestion, "Create New Request", Me)
                If blah <> vbOK Then
                    Exit Sub
                End If
            End If
            ClearAll()
            IsNewRequest = True
            SetTitle(True)
            CurrentRequest = New RequestObject
            Me.FormUID = CurrentRequest.GUID
            IsModifying = True
            'Set the datasource to a new empty DB table.
            Dim EmptyTable = DBFactory.GetDatabase.DataTableFromQueryString("SELECT " & GridFunctions.ColumnsString(RequestItemsColumns) & " FROM " & SibiRequestItemsCols.TableName & " LIMIT 0")
            GridFunctions.PopulateGrid(RequestItemsGrid, EmptyTable, RequestItemsColumns)
            EnableControls()
            pnlCreate.Visible = True
            Me.Show()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Me.Dispose()
        Finally
            SetWaitCursor(False, Me)
        End Try
    End Sub

    Private Sub OpenRequest(RequestUID As String)
        SetWaitCursor(True, Me)
        Try
            Dim strRequestQRY As String = "SELECT * FROM " & SibiRequestCols.TableName & " WHERE " & SibiRequestCols.UID & "='" & RequestUID & "'"
            Dim strRequestItemsQRY As String = "SELECT " & GridFunctions.ColumnsString(RequestItemsColumns) & " FROM " & SibiRequestItemsCols.TableName & " WHERE " & SibiRequestItemsCols.RequestUID & "='" & RequestUID & "' ORDER BY " & SibiRequestItemsCols.Timestamp
            Using RequestResults As DataTable = DBFactory.GetDatabase.DataTableFromQueryString(strRequestQRY),
                RequestItemsResults As DataTable = DBFactory.GetDatabase.DataTableFromQueryString(strRequestItemsQRY)
                RequestResults.TableName = SibiRequestCols.TableName
                RequestItemsResults.TableName = SibiRequestItemsCols.TableName
                CurrentHash = GetHash(RequestResults, RequestItemsResults)
                ClearAll()
                CollectRequestInfo(RequestResults, RequestItemsResults)
                DataParser.FillDBFields(RequestResults)
                SendToGrid(RequestItemsResults)
                LoadNotes(CurrentRequest.GUID)
                DisableControls()
                SetTitle(False)
                SetAttachCount()
                Me.Show()
                Me.Activate()
                SetMunisStatus()
                bolGridFilling = False
            End Using
        Catch ex As Exception
            Message("An error occured while opening the request. It may have been deleted.", vbOKOnly + vbExclamation, "Error", Me)
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Me.Dispose()
        Finally
            SetWaitCursor(False, Me)
        End Try
    End Sub
    Private Function GetHash(RequestTable As DataTable, ItemsTable As DataTable) As String
        Dim RequestHash As String = SecurityTools.GetSHAOfTable(RequestTable)
        Dim ItemHash As String = SecurityTools.GetSHAOfTable(ItemsTable)
        Return RequestHash & ItemHash
    End Function
    Private Function ConcurrencyCheck() As Boolean
        Try
            Using RequestTable = DBFactory.GetDatabase.DataTableFromQueryString("SELECT * FROM " & SibiRequestCols.TableName & " WHERE " & SibiRequestCols.UID & "='" & CurrentRequest.GUID & "'"),
            ItemTable = DBFactory.GetDatabase.DataTableFromQueryString("SELECT " & GridFunctions.ColumnsString(RequestItemsColumns) & " FROM " & SibiRequestItemsCols.TableName & " WHERE " & SibiRequestItemsCols.RequestUID & "='" & CurrentRequest.GUID & "' ORDER BY " & SibiRequestItemsCols.Timestamp)
                RequestTable.TableName = SibiRequestCols.TableName
                ItemTable.TableName = SibiRequestItemsCols.TableName
                Dim DBHash As String = GetHash(RequestTable, ItemTable)
                If DBHash <> CurrentHash Then
                    Return False
                End If
                Return True
            End Using
        Catch ex As Exception
            Throw
        End Try
    End Function
    Public Sub SetAttachCount()
        If Not GlobalSwitches.CachedMode Then

            If CurrentRequest IsNot Nothing Then
                cmdAttachments.Text = "(" + AssetFunc.GetAttachmentCount(CurrentRequest.GUID, New SibiAttachmentsCols).ToString + ")"
                cmdAttachments.ToolTipText = "Attachments " + cmdAttachments.Text
            Else
                cmdAttachments.Text = "(0)"
                cmdAttachments.ToolTipText = "Attachments " + cmdAttachments.Text
            End If

        End If
    End Sub

    Private Sub AddErrorIcon(ctl As Control)
        If fieldErrorIcon.GetError(ctl) Is String.Empty Then
            fieldErrorIcon.SetIconAlignment(ctl, ErrorIconAlignment.MiddleRight)
            fieldErrorIcon.SetIconPadding(ctl, 4)
            fieldErrorIcon.SetError(ctl, "Required Field")
        End If
    End Sub

    Private Function AddNewNote(RequestUID As String, Note As String) As Boolean
        Dim NoteUID As String = Guid.NewGuid.ToString
        Try
            Dim NewNoteParams As New List(Of DBParameter)
            NewNoteParams.Add(New DBParameter(SibiNotesCols.RequestUID, RequestUID))
            NewNoteParams.Add(New DBParameter(SibiNotesCols.NoteUID, NoteUID))
            NewNoteParams.Add(New DBParameter(SibiNotesCols.Note, Note))
            If DBFactory.GetDatabase.InsertFromParameters(SibiNotesCols.TableName, NewNoteParams) > 0 Then
                Return True
            End If
            Return False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

    Private Sub AddNewRequest()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.AddSibi) Then Exit Sub
        If Not ValidateFields() Then Exit Sub
        Dim RequestData As RequestObject = CollectData()
        Using trans = DBFactory.GetDatabase.StartTransaction, conn = trans.Connection
            Try
                Dim InsertRequestQry As String = "SELECT * FROM " & SibiRequestCols.TableName & " LIMIT 0"
                Dim InsertRequestItemsQry As String = "SELECT " & GridFunctions.ColumnsString(RequestItemsColumns) & " FROM " & SibiRequestItemsCols.TableName & " LIMIT 0"
                DBFactory.GetDatabase.UpdateTable(InsertRequestQry, GetInsertTable(InsertRequestQry, CurrentRequest.GUID), trans)
                DBFactory.GetDatabase.UpdateTable(InsertRequestItemsQry, RequestData.RequestItems, trans)
                pnlCreate.Visible = False
                trans.Commit()
                IsModifying = False
                IsNewRequest = False
                OpenRequest(CurrentRequest.GUID)
                ParentForm.RefreshData()
                Message("New Request Added.", vbOKOnly + vbInformation, "Complete", Me)
            Catch ex As Exception
                trans.Rollback()
                ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            End Try
        End Using
    End Sub

    Private Sub AddNote()
        Try
            If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifySibi) Then Exit Sub
            If CurrentRequest.GUID <> "" And Not IsNewRequest Then
                Dim NewNote As New SibiNotesForm(Me, CurrentRequest)
                If NewNote.DialogResult = DialogResult.OK Then
                    AddNewNote(NewNote.Request.GUID, Trim(NewNote.rtbNotes.Rtf))
                    LoadNotes(CurrentRequest.GUID)
                End If
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Function CheckFields(parent As Control) As Boolean
        Dim c As Control
        For Each c In parent.Controls
            Dim DBInfo As New DBControlInfo
            If c.Tag IsNot Nothing Then DBInfo = DirectCast(c.Tag, DBControlInfo)
            If DBInfo.Required Then
                Select Case True
                    Case TypeOf c Is TextBox
                        If Trim(c.Text) = "" Then
                            bolFieldsValid = False
                            c.BackColor = colMissingField
                            AddErrorIcon(c)
                        Else
                            c.BackColor = Color.Empty
                            ClearErrorIcon(c)
                        End If
                    Case TypeOf c Is ComboBox
                        Dim cmb = DirectCast(c, ComboBox)
                        If cmb.SelectedIndex = -1 Then
                            bolFieldsValid = False
                            cmb.BackColor = colMissingField
                            AddErrorIcon(cmb)
                        Else
                            cmb.BackColor = Color.Empty
                            ClearErrorIcon(cmb)
                        End If
                End Select
            End If
            If c.HasChildren Then CheckFields(c)
        Next
        Return bolFieldsValid 'if fields are missing return false to trigger a message if needed
    End Function

    Private Async Sub CheckForPO()
        If CurrentRequest.RequisitionNumber <> "" And CurrentRequest.PO = "" Then
            Dim GetPO As String = Await MunisFunc.GetPOFromReqNumberAsync(CurrentRequest.RequisitionNumber, CurrentRequest.DateStamp.Year.ToString)
            If GetPO IsNot Nothing AndAlso GetPO.Length > 1 Then
                Dim blah = Message("PO Number " & GetPO & " was detected in the Requisition. Do you wish to add it to this request?", vbQuestion + vbYesNo, "New PO Detected", Me)
                If blah = MsgBoxResult.Yes Then
                    InsertPONumber(GetPO)
                    OpenRequest(CurrentRequest.GUID)
                Else
                    Exit Sub
                End If
            End If
        End If
    End Sub

    Private Sub chkAllowDrag_CheckedChanged(sender As Object, e As EventArgs) Handles chkAllowDrag.CheckedChanged
        If chkAllowDrag.Checked Then
            RequestItemsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect
            RequestItemsGrid.MultiSelect = False
        Else
            RequestItemsGrid.SelectionMode = DataGridViewSelectionMode.CellSelect
            RequestItemsGrid.MultiSelect = True
        End If
    End Sub

    Private Sub ClearCheckBox(ByVal control As Control)
        If TypeOf control Is CheckBox Then
            Dim chk = DirectCast(control, CheckBox)
            chk.Checked = False
        End If
    End Sub

    Private Sub ClearCombos(ByVal control As Control)
        If TypeOf control Is ComboBox Then
            Dim cmb = DirectCast(control, ComboBox)
            cmb.SelectedIndex = -1
            cmb.Text = Nothing
        End If
    End Sub

    Private Sub ClearControls(ByVal control As Control)
        For Each c As Control In control.Controls
            ClearTextBoxes(c)
            ClearCombos(c)
            ClearDTPicker(c)
            ClearCheckBox(c)
            If c.HasChildren Then
                ClearControls(c)
            End If
        Next
    End Sub

    Private Sub ClearDTPicker(ByVal control As Control)
        If TypeOf control Is DateTimePicker Then
            Dim dtp = DirectCast(control, DateTimePicker)
            dtp.Value = Now
        End If
    End Sub

    Private Sub ClearErrorIcon(ctl As Control)
        fieldErrorIcon.SetError(ctl, String.Empty)
    End Sub

    Private Sub ClearTextBoxes(ByVal control As Control)
        If TypeOf control Is TextBox Then
            Dim txt = DirectCast(control, TextBox)
            txt.Clear()
        End If
    End Sub

    Private Sub cmdAccept_Click(sender As Object, e As EventArgs) Handles cmdAccept.Click
        RequestItemsGrid.EndEdit()
        If Not ValidateFields() Then Exit Sub
        DisableControls()
        ToolStrip.BackColor = colSibiToolBarColor
        HideEditControls()
        UpdateRequest()
        IsModifying = False
    End Sub

    Private Sub cmdAddNew_Click(sender As Object, e As EventArgs) Handles cmdAddNew.Click
        AddNewRequest()
    End Sub

    Private Sub cmdAddNote_Click(sender As Object, e As EventArgs) Handles cmdAddNote.Click
        AddNote()
    End Sub

    Private Sub ViewAttachments()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ViewAttachment) Then Exit Sub
        If Not AttachmentsIsOpen(Me) Then
            If CurrentRequest.GUID <> "" And Not IsNewRequest Then
                Dim NewAttach As New AttachmentsForm(Me, New SibiAttachmentsCols, CurrentRequest)
            End If
        End If
    End Sub

    Private Sub cmdAttachments_Click(sender As Object, e As EventArgs) Handles cmdAttachments.Click
        ViewAttachments()
    End Sub

    Private Sub cmdClearAll_Click(sender As Object, e As EventArgs)
        ClearAll()
        CurrentRequest = Nothing
    End Sub

    Private Sub cmdCreate_Click(sender As Object, e As EventArgs) Handles cmdCreate.Click
        NewRequest()
    End Sub

    Private Sub DeleteCurrentSibiReqest()
        Try
            If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.DeleteSibi) Then Exit Sub
            If IsNothing(CurrentRequest.RequestItems) Then Exit Sub
            Dim blah = Message("Are you absolutely sure?  This cannot be undone and will delete all data including attachments.", vbYesNo + vbExclamation, "WARNING", Me)
            If blah = vbYes Then
                SetWaitCursor(True, Me)
                If AssetFunc.DeleteFtpAndSql(CurrentRequest.GUID, EntryType.Sibi) Then
                    Message("Sibi Request deleted successfully.", vbOKOnly + vbInformation, "Device Deleted", Me)
                    CurrentRequest = Nothing
                    ParentForm.RefreshData()
                    Me.Dispose()
                Else
                    Logger("*****DELETION ERROR******: " & CurrentRequest.GUID)
                    Message("Failed to delete request succesfully!  Please let Bobby Lovell know about this.", vbOKOnly + vbCritical, "Delete Failed", Me)
                    CurrentRequest = Nothing
                    Me.Dispose()
                End If
            Else
                Exit Sub
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            SetWaitCursor(False, Me)
        End Try
    End Sub

    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        DeleteCurrentSibiReqest()
    End Sub

    Private Sub DeleteCurrentNote()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifySibi) Then Exit Sub
        If dgvNotes.CurrentRow IsNot Nothing AndAlso dgvNotes.CurrentRow.Index > -1 Then
            Dim blah = Message("Are you sure?", vbYesNo + vbQuestion, "Delete Note", Me)
            If blah = vbYes Then
                Dim NoteUID As String = GridFunctions.GetCurrentCellValue(dgvNotes, SibiNotesCols.NoteUID)
                If NoteUID <> "" Then
                    Message(DeleteItem_FromSQL(NoteUID, SibiNotesCols.NoteUID, SibiNotesCols.TableName) & " Rows affected.", vbOKOnly + vbInformation, "Delete Item", Me)
                    OpenRequest(CurrentRequest.GUID)
                End If
            End If
        End If
    End Sub

    Private Sub cmdDeleteNote_Click(sender As Object, e As EventArgs) Handles cmdDeleteNote.Click
        DeleteCurrentNote()
    End Sub

    Private Sub cmdDiscard_Click(sender As Object, e As EventArgs) Handles cmdDiscard.Click
        CancelModify()
    End Sub

    Private Sub cmdNewNote_Click(sender As Object, e As EventArgs) Handles cmdNewNote.Click
        AddNote()
    End Sub

    Private Sub ModifyRequest()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifySibi) Then Exit Sub
        If CurrentRequest.GUID <> "" And Not IsModifying Then SetModifyMode(IsModifying)
    End Sub

    Private Sub ModifyButton_Click(sender As Object, e As EventArgs) Handles ModifyButton.Click
        ModifyRequest()
    End Sub

    Private Function CollectData() As RequestObject
        Try
            Dim info As New RequestObject
            With info
                .Description = Trim(txtDescription.Text)
                .RequestUser = Trim(txtUser.Text)
                .RequestType = GetDBValue(SibiAttribute.RequestType, cmbType.SelectedIndex)
                .NeedByDate = dtNeedBy.Value
                .Status = GetDBValue(SibiAttribute.StatusType, cmbStatus.SelectedIndex)
                .PO = Trim(txtPO.Text)
                .RequisitionNumber = Trim(txtReqNumber.Text)
                .RTNumber = Trim(txtRTNumber.Text)
            End With
            RequestItemsGrid.EndEdit()
            For Each row As DataGridViewRow In RequestItemsGrid.Rows
                For Each dcell As DataGridViewCell In row.Cells
                    If dcell.Value IsNot Nothing Then
                        dcell.Value = Trim(dcell.Value.ToString)
                    Else
                        Select Case dcell.OwningColumn.Name
                            Case SibiRequestItemsCols.ItemUID
                                dcell.Value = Guid.NewGuid.ToString
                            Case Else
                                dcell.Value = DBNull.Value
                        End Select
                    End If
                Next
            Next
            info.RequestItems = CType(RequestItemsGrid.DataSource, DataTable)
            Return info
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

    Private Sub CollectRequestInfo(RequestResults As DataTable, RequestItemsResults As DataTable)
        Try
            CurrentRequest = New RequestObject(RequestResults)
            CurrentRequest.RequestItems = RequestItemsResults
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Function DeleteItem_FromLocal(RowIndex As Integer) As Boolean
        Try
            If Not RequestItemsGrid.Rows(RowIndex).IsNewRow Then
                RequestItemsGrid.Rows.Remove(RequestItemsGrid.Rows(RowIndex))
                Return True
            End If
            Return False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

    Private Function DeleteItem_FromSQL(ItemUID As String, ItemColumnName As String, Table As String) As Integer
        Try
            Dim rows As Integer
            Dim DeleteItemQuery As String = "DELETE FROM " & Table & " WHERE " & ItemColumnName & "='" & ItemUID & "'"
            rows = DBFactory.GetDatabase.ExecuteQuery(DeleteItemQuery)
            Return rows
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return -1
        End Try
    End Function

    Private Sub dgvNotes_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvNotes.CellDoubleClick
        ViewNote()
    End Sub
    Private Sub ViewNote()
        Try
            Dim NoteUID = GridFunctions.GetCurrentCellValue(dgvNotes, SibiNotesCols.NoteUID)
            If Not FormIsOpenByUID(GetType(SibiNotesForm), NoteUID) Then
                Dim ViewNote As New SibiNotesForm(Me, NoteUID)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub DisableControlsRecursive(control As Control)
        For Each c As Control In control.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    Dim txt = DirectCast(c, TextBox)
                    txt.ReadOnly = True
                Case TypeOf c Is ComboBox
                    Dim cmb = DirectCast(c, ComboBox)
                    cmb.Enabled = False
                Case TypeOf c Is DateTimePicker
                    Dim dtp = DirectCast(c, DateTimePicker)
                    dtp.Enabled = False
                Case TypeOf c Is CheckBox
                    If c IsNot chkAllowDrag Then
                        c.Enabled = False
                    End If
                Case TypeOf c Is Label
                    'do nut-zing
            End Select
            If c.HasChildren Then
                DisableControlsRecursive(c)
            End If
        Next
    End Sub

    Private Sub DisableControls()
        DisableControlsRecursive(Me)
        DisableGrid()
    End Sub

    Private Sub DisableGrid()
        RequestItemsGrid.EditMode = DataGridViewEditMode.EditProgrammatically
        RequestItemsGrid.AllowUserToAddRows = False
        RequestItemsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
    End Sub

    Private Sub EnableControlsRecursive(control As Control)
        For Each c As Control In control.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    Dim txt = DirectCast(c, TextBox)
                    If txt IsNot txtRequestNum And txt IsNot txtCreateDate Then
                        txt.ReadOnly = False
                    End If
                Case TypeOf c Is ComboBox
                    Dim cmb = DirectCast(c, ComboBox)
                    cmb.Enabled = True
                Case TypeOf c Is DateTimePicker
                    Dim dtp = DirectCast(c, DateTimePicker)
                    dtp.Enabled = True
                Case TypeOf c Is CheckBox
                    c.Enabled = True
                Case TypeOf c Is Label
                    'do nut-zing
            End Select
            If c.HasChildren Then
                EnableControlsRecursive(c)
            End If
        Next
    End Sub

    Private Sub EnableControls()
        EnableControlsRecursive(Me)
        EnableGrid()
    End Sub

    Private Sub EnableGrid()
        RequestItemsGrid.EditMode = DataGridViewEditMode.EditOnEnter
        RequestItemsGrid.AllowUserToAddRows = True
        SetColumnWidths()
    End Sub

    Private Sub FillCombos()
        FillComboBox(SibiAttribute.StatusType, cmbStatus)
        FillComboBox(SibiAttribute.RequestType, cmbType)
    End Sub

    Public Overrides Function OKToClose() As Boolean
        Dim CanClose As Boolean = True
        If Not OKToCloseChildren(Me) Then CanClose = False
        If IsModifying AndAlso Not CancelModify() Then CanClose = False
        Return CanClose
    End Function

    Private Sub frmManageRequest_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Not OKToClose() Then
            e.Cancel = True
        End If
    End Sub

    Private Sub frmManageRequest_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        MyMunisToolBar.Dispose()
        MyWindowList.Dispose()
        CloseChildren(Me)
    End Sub

    Private Sub frmManageRequest_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Dim f As Form = DirectCast(sender, Form)
        If f.WindowState = FormWindowState.Minimized Then
            MinimizeChildren(Me)
            PrevWindowState = f.WindowState
        ElseIf f.WindowState <> PrevWindowState And f.WindowState = FormWindowState.Normal Then
            If PrevWindowState <> FormWindowState.Maximized Then RestoreChildren(Me)
        End If
    End Sub

    Private Sub frmManageRequest_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin
        Dim f As Form = DirectCast(sender, Form)
        PrevWindowState = f.WindowState
    End Sub

    Private Function GetInsertTable(selectQuery As String, UID As String) As DataTable
        Try
            Dim tmpTable = DataParser.ReturnInsertTable(selectQuery)
            Dim DBRow = tmpTable.Rows(0)
            'Add Add'l info
            DBRow(SibiRequestCols.UID) = UID
            Return tmpTable
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

    Private Function GetUpdateTable(selectQuery As String) As DataTable
        Try
            Dim tmpTable = DataParser.ReturnUpdateTable(selectQuery)
            'Add Add'l info
            Return tmpTable
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

    Private Sub HideEditControls()
        pnlEditButtons.Visible = False
    End Sub

    Private Sub HighlightCurrentRow(Row As Integer)
        Try
            If Not bolGridFilling Then
                HighlightRow(RequestItemsGrid, GridTheme, Row)
            End If
        Catch
        End Try
    End Sub

    Private Sub InitDBControls()
        txtDescription.Tag = New DBControlInfo(SibiRequestCols.Description, True)
        txtUser.Tag = New DBControlInfo(SibiRequestCols.RequestUser, True)
        cmbType.Tag = New DBControlInfo(SibiRequestCols.Type, SibiAttribute.RequestType, True)
        dtNeedBy.Tag = New DBControlInfo(SibiRequestCols.NeedBy, True)
        cmbStatus.Tag = New DBControlInfo(SibiRequestCols.Status, SibiAttribute.StatusType, True)
        txtPO.Tag = New DBControlInfo(SibiRequestCols.PO, False)
        txtReqNumber.Tag = New DBControlInfo(SibiRequestCols.RequisitionNumber, False)
        txtRequestNum.Tag = New DBControlInfo(SibiRequestCols.RequestNumber, ParseType.DisplayOnly, False)
        txtRTNumber.Tag = New DBControlInfo(SibiRequestCols.RTNumber, False)
        txtCreateDate.Tag = New DBControlInfo(SibiRequestCols.DateStamp, ParseType.DisplayOnly, False)

    End Sub

    Private Sub InitForm(ParentForm As ExtendedForm, Optional UID As String = "")
        InitDBControls()
        ExtendedMethods.DoubleBufferedDataGrid(RequestItemsGrid, True)
        MyMunisToolBar.InsertMunisDropDown(ToolStrip)
        Me.ParentForm = ParentForm
        Me.FormUID = UID
        ImageCaching.CacheControlImages(Me)
        MyWindowList.InsertWindowList(ToolStrip)
        dgvNotes.DefaultCellStyle.SelectionBackColor = GridTheme.CellSelectColor
        RequestItemsGrid.DefaultCellStyle.SelectionBackColor = GridTheme.CellSelectColor
        ToolStrip.BackColor = colSibiToolBarColor

    End Sub

    Private Sub InsertPONumber(PO As String)
        Try
            AssetFunc.UpdateSqlValue(SibiRequestCols.TableName, SibiRequestCols.PO, PO, SibiRequestCols.UID, CurrentRequest.GUID)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub LoadNotes(RequestUID As String)
        Try
            Dim NotesQry As String = "SELECT * FROM " & SibiNotesCols.TableName & " WHERE " & SibiNotesCols.RequestUID & "='" & RequestUID & "' ORDER BY " & SibiNotesCols.DateStamp & " DESC"
            Using Results As DataTable = DBFactory.GetDatabase.DataTableFromQueryString(NotesQry)
                GridFunctions.PopulateGrid(dgvNotes, Results, NotesGridColumns)
            End Using

            dgvNotes.ClearSelection()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

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

    Private Async Sub PopulateFromFA(ColumnName As String)
        Try
            SetWaitCursor(True, Me)
            Select Case ColumnName
                Case SibiRequestItemsCols.NewSerial
                    Dim ItemUID = GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ItemUID)
                    Dim Serial = Await Task.Run(Function()
                                                    Return MunisFunc.GetSerialFromAsset(GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.NewAsset))
                                                End Function)
                    If Serial <> "" Then
                        AssetFunc.UpdateSqlValue(SibiRequestItemsCols.TableName, SibiRequestItemsCols.NewSerial, Serial, SibiRequestItemsCols.ItemUID, ItemUID)
                        RefreshRequest()
                    End If
                Case SibiRequestItemsCols.NewAsset
                    Dim ItemUID = GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ItemUID)
                    Dim Asset = Await Task.Run(Function()
                                                   Return MunisFunc.GetAssetFromSerial(GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.NewSerial))
                                               End Function)
                    If Asset <> "" Then
                        AssetFunc.UpdateSqlValue(SibiRequestItemsCols.TableName, SibiRequestItemsCols.NewAsset, Asset, SibiRequestItemsCols.ItemUID, ItemUID)
                        RefreshRequest()
                    End If
                Case SibiRequestItemsCols.ReplaceSerial
                    Dim ItemUID = GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ItemUID)
                    Dim Serial = Await Task.Run(Function()
                                                    Return MunisFunc.GetSerialFromAsset(GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ReplaceAsset))
                                                End Function)
                    If Serial <> "" Then
                        AssetFunc.UpdateSqlValue(SibiRequestItemsCols.TableName, SibiRequestItemsCols.ReplaceSerial, Serial, SibiRequestItemsCols.ItemUID, ItemUID)
                        RefreshRequest()
                    End If
                Case SibiRequestItemsCols.ReplaceAsset
                    Dim ItemUID = GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ItemUID)
                    Dim Asset = Await Task.Run(Function()
                                                   Return MunisFunc.GetAssetFromSerial(GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ReplaceSerial))
                                               End Function)
                    If Asset <> "" Then
                        AssetFunc.UpdateSqlValue(SibiRequestItemsCols.TableName, SibiRequestItemsCols.ReplaceAsset, Asset, SibiRequestItemsCols.ItemUID, ItemUID)
                        RefreshRequest()
                    End If
            End Select
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            SetWaitCursor(False, Me)
        End Try
    End Sub

    Private Sub RefreshRequest()
        OpenRequest(CurrentRequest.GUID)
    End Sub

    Private Function NotesGridColumns() As List(Of DataGridColumn)
        Dim ColList As New List(Of DataGridColumn)
        ColList.Add(New DataGridColumn(SibiNotesCols.DateStamp, "Date Stamp", GetType(Date)))
        ColList.Add(New DataGridColumn(SibiNotesCols.Note, "Note", GetType(String), ColumnDisplayTypes.NotePreview))
        ColList.Add(New DataGridColumn(SibiNotesCols.NoteUID, "UID", GetType(String), True, False))
        Return ColList
    End Function

    Private Function RequestItemsColumns() As List(Of DataGridColumn)
        Dim ColList As New List(Of DataGridColumn)
        ColList.Add(New DataGridColumn(SibiRequestItemsCols.User, "User", GetType(String)))
        ColList.Add(New DataGridColumn(SibiRequestItemsCols.Description, "Description", GetType(String)))
        ColList.Add(New DataGridColumn(SibiRequestItemsCols.Qty, "Qty", GetType(Integer)))
        ColList.Add(New DataGridColumn(SibiRequestItemsCols.Location, "Location", DeviceAttribute.Locations))
        ColList.Add(New DataGridColumn(SibiRequestItemsCols.Status, "Status", SibiAttribute.ItemStatusType))
        ColList.Add(New DataGridColumn(SibiRequestItemsCols.ReplaceAsset, "Replace Asset", GetType(String)))
        ColList.Add(New DataGridColumn(SibiRequestItemsCols.ReplaceSerial, "Replace Serial", GetType(String)))
        ColList.Add(New DataGridColumn(SibiRequestItemsCols.NewAsset, "New Asset", GetType(String)))
        ColList.Add(New DataGridColumn(SibiRequestItemsCols.NewSerial, "New Serial", GetType(String)))
        ColList.Add(New DataGridColumn(SibiRequestItemsCols.OrgCode, "Org Code", GetType(String)))
        ColList.Add(New DataGridColumn(SibiRequestItemsCols.ObjectCode, "Object Code", GetType(String)))
        ColList.Add(New DataGridColumn(SibiRequestItemsCols.ItemUID, "Item UID", GetType(String), True, True))
        ColList.Add(New DataGridColumn(SibiRequestItemsCols.RequestUID, "Request UID", GetType(String), True, False))
        Return ColList
    End Function

    Private Sub RequestItemsGrid_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles RequestItemsGrid.CellEnter
        HighlightCurrentRow(e.RowIndex)
    End Sub

    Private Sub RequestItemsGrid_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles RequestItemsGrid.CellLeave
        LeaveRow(RequestItemsGrid, GridTheme, e.RowIndex)
    End Sub

    Private Sub RequestItemsGrid_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles RequestItemsGrid.CellMouseDown
        Try
            If e.ColumnIndex >= -1 And e.RowIndex >= 0 Then
                Dim ColIndex As Integer = CInt(IIf(e.ColumnIndex = -1, 0, e.ColumnIndex))
                If Not RequestItemsGrid.Item(ColIndex, e.RowIndex).Selected Then
                    RequestItemsGrid.Rows(e.RowIndex).Selected = True
                    RequestItemsGrid.CurrentCell = RequestItemsGrid(ColIndex, e.RowIndex)
                End If
                SetToolStripItems()
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub RequestItemsGrid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles RequestItemsGrid.DataError
        Message("DataGrid Error: " & Chr(34) & e.Exception.Message & Chr(34) & "   Col/Row:" & e.ColumnIndex & "/" & e.RowIndex, vbOKOnly + vbExclamation, "DataGrid Error", Me)
    End Sub

    Private Sub RequestItemsGrid_DefaultValuesNeeded(sender As Object, e As DataGridViewRowEventArgs) Handles RequestItemsGrid.DefaultValuesNeeded
        e.Row.Cells(SibiRequestItemsCols.Qty).Value = 1
    End Sub

    Private Sub RequestItemsGrid_DragDrop(sender As Object, e As DragEventArgs) Handles RequestItemsGrid.DragDrop
        'Drag-drop rows from other data grids, adding new UIDs and the current FKey (RequestUID).
        'This was a tough nut to crack. In the end I just ended up building an item array and adding it to the receiving grids datasource.
        Try
            If IsModifying Then
                Dim R = DirectCast(e.Data.GetData(GetType(DataGridViewRow)), DataGridViewRow) 'Cast the DGVRow
                If R.DataBoundItem IsNot Nothing Then
                    Dim NewDataRow As DataRow = DirectCast(R.DataBoundItem, DataRowView).Row 'Get the databound row
                    Dim ItemArr As New List(Of Object)
                    For Each col As DataColumn In NewDataRow.Table.Columns 'Iterate through columns and build a new item list
                        Select Case col.ColumnName
                            Case SibiRequestItemsCols.ItemUID
                                ItemArr.Add(Guid.NewGuid.ToString)
                            Case SibiRequestItemsCols.RequestUID
                                ItemArr.Add(CurrentRequest.GUID)
                            Case Else
                                ItemArr.Add(NewDataRow.Item(col))
                        End Select
                    Next
                    DirectCast(RequestItemsGrid.DataSource, DataTable).Rows.Add(ItemArr.ToArray) 'Add the item list as an array
                End If
                bolDragging = False
            Else
                If Not bolDragging Then
                    Message("You must be modifying this request before you can drag-drop rows from another request.", vbOKOnly + MsgBoxStyle.Exclamation, "Not Allowed", Me)
                End If
            End If
        Catch ex As Exception
            bolDragging = False
        End Try
    End Sub

    Private Sub RequestItemsGrid_DragEnter(sender As Object, e As DragEventArgs) Handles RequestItemsGrid.DragEnter
        e.Effect = DragDropEffects.Copy
    End Sub

    Private Sub RequestItemsGrid_DragLeave(sender As Object, e As EventArgs) Handles RequestItemsGrid.DragLeave
        bolDragging = False
    End Sub

    Private Sub RequestItemsGrid_MouseDown(sender As Object, e As MouseEventArgs) Handles RequestItemsGrid.MouseDown
        MouseIsDragging(e.Location)
    End Sub

    Private Sub RequestItemsGrid_MouseMove(sender As Object, e As MouseEventArgs) Handles RequestItemsGrid.MouseMove
        If RequestItemsGrid.SelectedRows.Count > 0 Then
            If chkAllowDrag.Checked And Not bolDragging Then
                If e.Button = MouseButtons.Left Then
                    If MouseIsDragging(, e.Location) Then
                        bolDragging = True
                        RequestItemsGrid.DoDragDrop(RequestItemsGrid.SelectedRows(0), DragDropEffects.All)
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub RequestItemsGrid_MouseUp(sender As Object, e As MouseEventArgs) Handles RequestItemsGrid.MouseUp
        bolDragging = False
    End Sub

    Private Sub RequestItemsGrid_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles RequestItemsGrid.RowEnter
        If Not bolGridFilling Then
            RequestItemsGrid.Rows(e.RowIndex).Cells(SibiRequestItemsCols.RequestUID).Value = CurrentRequest.GUID
            If RequestItemsGrid.Rows(e.RowIndex).Cells(SibiRequestItemsCols.ItemUID).Value Is Nothing Then RequestItemsGrid.Rows(e.RowIndex).Cells(SibiRequestItemsCols.ItemUID).Value = Guid.NewGuid.ToString
        End If
    End Sub

    Private Sub RequestItemsGrid_RowLeave(sender As Object, e As DataGridViewCellEventArgs) Handles RequestItemsGrid.RowLeave
        If IsModifying Then ValidateRequestItems()
    End Sub

    Private Sub RequestItemsGrid_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles RequestItemsGrid.RowPostPaint
        Using b As SolidBrush = New SolidBrush(Color.Black)
            e.Graphics.DrawString((e.RowIndex + 1).ToString(),
                                   RequestItemsGrid.DefaultCellStyle.Font,
                                   b,
                                   e.RowBounds.Location.X + 20,
                                   e.RowBounds.Location.Y + 4)
        End Using
    End Sub

    Private Sub ResetBackColors(parent As Control)
        Dim c As Control
        For Each c In parent.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    c.BackColor = Color.Empty
                Case TypeOf c Is ComboBox
                    c.BackColor = Color.Empty
            End Select
            If c.HasChildren Then ResetBackColors(c)
        Next
    End Sub

    Private Sub SendToGrid(Results As DataTable)
        Try
            bolGridFilling = True
            GridFunctions.PopulateGrid(RequestItemsGrid, Results, RequestItemsColumns)
            RequestItemsGrid.ClearSelection()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub SetColumnWidths()
        RequestItemsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        RequestItemsGrid.Columns(1).Width = 200
        RequestItemsGrid.Columns(2).Width = 50
        RequestItemsGrid.Columns(3).Width = 200
        RequestItemsGrid.Columns(4).Width = 200
        RequestItemsGrid.RowHeadersWidth = 57
    End Sub

    Private Sub SetGLBudgetContextMenu()
        If RequestItemsGrid.CurrentCell IsNot Nothing Then
            Dim ColIndex As Integer = RequestItemsGrid.CurrentCell.ColumnIndex
            Select Case True
                Case ColIndex = GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.ObjectCode), ColIndex = GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.OrgCode)
                    If GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ObjectCode) <> "" And GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.OrgCode) <> "" Then
                        tsmGLBudget.Visible = True
                    Else
                        tsmGLBudget.Visible = False
                    End If
                Case Else
                    tsmGLBudget.Visible = False
            End Select
        End If
    End Sub

    Private Sub SetMunisStatus()
        If Not GlobalSwitches.CachedMode Then
            If CurrentRequest IsNot Nothing Then
                SetReqStatus(CurrentRequest.RequisitionNumber, CurrentRequest.DateStamp.Year)
                CheckForPO()
                SetPOStatus(CurrentRequest.PO)
            Else
                SetReqStatus(String.Empty, -1)
                SetPOStatus(String.Empty)
            End If
        End If
    End Sub

    Private Async Sub SetPOStatus(PO As String)
        Dim intPO As Integer = 0
        lblPOStatus.Text = "Status: NA"
        If PO <> "" And Int32.TryParse(PO, intPO) Then
            Dim GetStatusString As String = Await MunisFunc.GetPOStatusFromPO(intPO)
            If GetStatusString <> "" Then
                lblPOStatus.Text = "Status: " & GetStatusString
            End If
        End If
    End Sub

    Private Async Sub SetReqStatus(ReqNum As String, FY As Integer)
        Dim intReq As Integer = 0
        lblReqStatus.Text = "Status: NA"
        If FY > 0 Then
            If ReqNum <> "" And Int32.TryParse(ReqNum, intReq) Then
                Dim GetStatusString As String = Await MunisFunc.GetReqStatusFromReqNum(ReqNum, FY)
                If GetStatusString <> "" Then
                    lblReqStatus.Text = "Status: " & GetStatusString
                End If
            End If
        End If
    End Sub

    Private Sub SetTitle(Optional NewRequest As Boolean = False)
        If Not NewRequest Then
            Me.Text = TitleText + " - " + CurrentRequest.Description
        Else
            Me.Text = TitleText + " - *New Request*"
        End If
    End Sub

    Private Sub SetToolStripItems()
        If RequestItemsGrid.CurrentCell IsNot Nothing Then
            If ValidColumn() Then
                tsmPopFA.Visible = True
                tsmSeparator.Visible = True
                If RequestItemsGrid.CurrentCell.Value IsNot Nothing AndAlso RequestItemsGrid.CurrentCell.Value.ToString <> "" Then
                    tsmLookupDevice.Visible = True
                Else
                    tsmLookupDevice.Visible = False
                End If
            Else
                tsmPopFA.Visible = False
                tsmSeparator.Visible = False
                tsmLookupDevice.Visible = False
            End If
            If IsModifying Then
                tsmDeleteItem.Visible = True
            Else
                tsmDeleteItem.Visible = False
            End If
        End If
        SetGLBudgetContextMenu()
    End Sub

    Private Sub ShowEditControls()
        pnlEditButtons.Visible = True
    End Sub

    Private Sub tsbRefresh_Click(sender As Object, e As EventArgs) Handles tsbRefresh.Click
        If Not IsNewRequest Then OpenRequest(CurrentRequest.GUID)
    End Sub

    Private Sub tsmCopyText_Click(sender As Object, e As EventArgs) Handles tsmCopyText.Click
        RequestItemsGrid.RowHeadersVisible = False
        GridFunctions.CopySelectedGridData(RequestItemsGrid)
        RequestItemsGrid.RowHeadersVisible = True
    End Sub

    Private Sub DeleteSelectedRequestItem()
        Try
            If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifySibi) Then Exit Sub
            Dim blah = Message("Delete selected row?", vbYesNo + vbQuestion, "Delete Item Row", Me)
            If blah = vbYes Then
                If Not DeleteItem_FromLocal(RequestItemsGrid.CurrentRow.Index) Then
                    blah = Message("Failed to delete row.", vbExclamation + vbOKOnly, "Error", Me)
                End If
            Else
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub tsmDeleteItem_Click(sender As Object, e As EventArgs) Handles tsmDeleteItem.Click
        DeleteSelectedRequestItem()
    End Sub

    Private Sub tsmGLBudget_Click(sender As Object, e As EventArgs) Handles tsmGLBudget.Click
        Try
            Dim Org = GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.OrgCode)
            Dim Obj = GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ObjectCode)
            Dim FY = CurrentRequest.DateStamp.Year.ToString
            MunisFunc.NewOrgObView(Org, Obj, FY, Me)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub tsmLookupDevice_Click(sender As Object, e As EventArgs) Handles tsmLookupDevice.Click
        Try
            Dim ColIndex As Integer = RequestItemsGrid.CurrentCell.ColumnIndex
            Select Case True
                Case ColIndex = GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.ReplaceAsset)
                    LookupDevice(Me, AssetFunc.FindDeviceFromAssetOrSerial(RequestItemsGrid.Item(ColIndex, RequestItemsGrid.CurrentRow.Index).Value.ToString, FindDevType.AssetTag))
                Case ColIndex = GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.ReplaceSerial)
                    LookupDevice(Me, AssetFunc.FindDeviceFromAssetOrSerial(RequestItemsGrid.Item(ColIndex, RequestItemsGrid.CurrentRow.Index).Value.ToString, FindDevType.Serial))
                Case ColIndex = GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.NewAsset)
                    LookupDevice(Me, AssetFunc.FindDeviceFromAssetOrSerial(RequestItemsGrid.Item(ColIndex, RequestItemsGrid.CurrentRow.Index).Value.ToString, FindDevType.AssetTag))
                Case ColIndex = GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.NewSerial)
                    LookupDevice(Me, AssetFunc.FindDeviceFromAssetOrSerial(RequestItemsGrid.Item(ColIndex, RequestItemsGrid.CurrentRow.Index).Value.ToString, FindDevType.Serial))
            End Select
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub PopulateCurrentFAItem()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifySibi) Then Exit Sub
        Try
            If ValidColumn() Then
                PopulateFromFA(RequestItemsGrid.CurrentCell.OwningColumn.Name)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub tsmPopFA_Click(sender As Object, e As EventArgs) Handles tsmPopFA.Click
        PopulateCurrentFAItem()
    End Sub

    Private Sub txtPO_Click(sender As Object, e As EventArgs) Handles txtPO.Click
        Dim PO As String = Trim(txtPO.Text)
        If Not IsModifying And PO <> "" Then
            MunisFunc.NewMunisPOSearch(PO, Me)
        End If
    End Sub

    Private Sub txtReqNumber_Click(sender As Object, e As EventArgs) Handles txtReqNumber.Click
        Try
            SetWaitCursor(True, Me)
            Dim ReqNum As String = Trim(txtReqNumber.Text)
            If Not IsModifying And ReqNum <> "" Then
                MunisFunc.NewMunisReqSearch(ReqNum, YearFromDate(CurrentRequest.DateStamp), Me)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            SetWaitCursor(False, Me)
        End Try
    End Sub

    Private Sub txtRTNumber_Click(sender As Object, e As EventArgs) Handles txtRTNumber.Click
        Try
            Dim RTNum As String = Trim(txtRTNumber.Text)
            If Not IsModifying And RTNum <> "" Then
                Process.Start("http://rt.co.fairfield.oh.us/rt/Ticket/Display.html?id=" & RTNum)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub SetModifyMode(Enable As Boolean)
        If Not Enable Then
            If Not ConcurrencyCheck() Then
                RefreshRequest()
                Message("This request has been modified since it's been open and has been refreshed with the current data.", vbOKOnly + vbInformation, "Concurrency Check", Me)
            End If
            EnableControls()
            ToolStrip.BackColor = colEditColor
            ShowEditControls()
            IsModifying = True
        Else
            DisableControls()
            ToolStrip.BackColor = colSibiToolBarColor
            HideEditControls()
            UpdateRequest()
            IsModifying = False
        End If
    End Sub

    Private Sub UpdateRequest()
        Using trans = DBFactory.GetDatabase.StartTransaction, conn = trans.Connection
            Try
                If Not ConcurrencyCheck() Then
                    Message("It appears that someone else has modified this request. Please refresh and try again.", vbOKOnly + vbExclamation, "Concurrency Failure", Me)
                    Exit Sub
                End If
                Dim RequestData As RequestObject = CollectData()
                RequestData.GUID = CurrentRequest.GUID
                If RequestData.RequestItems Is Nothing Then Exit Sub
                Dim RequestUpdateQry As String = "SELECT * FROM " & SibiRequestCols.TableName & " WHERE " & SibiRequestCols.UID & " = '" & CurrentRequest.GUID & "'"
                Dim RequestItemsUpdateQry As String = "SELECT " & GridFunctions.ColumnsString(RequestItemsColumns) & " FROM " & SibiRequestItemsCols.TableName & " WHERE " & SibiRequestItemsCols.RequestUID & " = '" & CurrentRequest.GUID & "'"

                DBFactory.GetDatabase.UpdateTable(RequestUpdateQry, GetUpdateTable(RequestUpdateQry), trans)
                DBFactory.GetDatabase.UpdateTable(RequestItemsUpdateQry, RequestData.RequestItems, trans)

                trans.Commit()

                ParentForm.RefreshData()
                OpenRequest(CurrentRequest.GUID)
            Catch ex As Exception
                trans.Rollback()
                ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            End Try
        End Using
    End Sub

    Private Function ValidateFields() As Boolean
        bolFieldsValid = True
        Dim ValidateResults As Boolean = CheckFields(Me)
        If ValidateResults Then ValidateResults = ValidateRequestItems()
        If Not ValidateResults Then
            Message("Some required fields are missing. Please enter data into all require fields.", vbOKOnly + vbExclamation, "Missing Data", Me)
        End If
        Return ValidateResults
    End Function

    Private Function ValidateRequestItems() As Boolean
        Dim RowsValid As Boolean = True
        For Each row As DataGridViewRow In RequestItemsGrid.Rows
            If Not row.IsNewRow Then
                For Each dcell As DataGridViewCell In row.Cells
                    Dim CellString As String = ""
                    If dcell.Value IsNot Nothing Then
                        CellString = dcell.Value.ToString
                    Else
                        CellString = ""
                    End If
                    If dcell.OwningColumn.CellType Is GetType(DataGridViewComboBoxCell) Then
                        If dcell.Value Is Nothing Or CellString = "" Then
                            RowsValid = False
                            dcell.ErrorText = "Required Field!"
                        Else
                            dcell.ErrorText = Nothing
                        End If
                    End If
                    If dcell.OwningColumn.Name = SibiRequestItemsCols.Qty Then
                        If dcell.Value Is Nothing Or CellString = "" Then
                            RowsValid = False
                            dcell.ErrorText = "Required Field!"
                        Else
                            dcell.ErrorText = Nothing
                        End If
                    End If
                Next
            End If
        Next
        Return RowsValid
    End Function

    Private Function ValidColumn() As Boolean
        Try
            Select Case True
                Case RequestItemsGrid.CurrentCell.ColumnIndex = GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.ReplaceAsset)
                    Return True
                Case RequestItemsGrid.CurrentCell.ColumnIndex = GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.ReplaceSerial)
                    Return True
                Case RequestItemsGrid.CurrentCell.ColumnIndex = GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.NewAsset)
                    Return True
                Case RequestItemsGrid.CurrentCell.ColumnIndex = GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.NewSerial)
                    Return True
                Case Else
                    Return False
            End Select
            Return False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

#End Region

End Class