Option Explicit On
Imports System.Collections
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class SibiManageRequestForm
    Public bolUpdating As Boolean = False
    Private bolGridFilling As Boolean = False
    Public CurrentRequest As Request_Info
    Private MyText As String
    Private MyWindowList As New WindowList(Me)
    Private bolDragging As Boolean = False
    Private DataParser As New DBControlParser(Me)
    Private MyMunisToolBar As New MunisToolBar(Me)
    Sub New(ParentForm As MyForm, RequestUID As String)
        Waiting()
        InitializeComponent()
        InitForm(ParentForm)
        OpenRequest(RequestUID)
        DoneWaiting()
    End Sub
    Sub New(ParentForm As MyForm)
        InitializeComponent()
        InitForm(ParentForm)
        Text += " - *New Request*"
        NewRequest()
        Show()
    End Sub
    Private Sub InitForm(ParentForm As MyForm)
        InitDBControls()
        ExtendedMethods.DoubleBufferedDataGrid(RequestItemsGrid, True)
        MyMunisToolBar.InsertMunisDropDown(ToolStrip)
        MyWindowList.InsertWindowList(ToolStrip) ' = New WindowList(Me, ToolStrip)
        Tag = ParentForm
        Icon = ParentForm.Icon
        GridTheme = ParentForm.GridTheme
        dgvNotes.DefaultCellStyle.SelectionBackColor = GridTheme.CellSelectColor
        ToolStrip.BackColor = colSibiToolBarColor
    End Sub
    Private Sub InitDBControls()
        txtDescription.Tag = New DBControlInfo(sibi_requests.Description, True)
        txtUser.Tag = New DBControlInfo(sibi_requests.RequestUser, True)
        cmbType.Tag = New DBControlInfo(sibi_requests.Type, SibiIndex.RequestType, True)
        dtNeedBy.Tag = New DBControlInfo(sibi_requests.NeedBy, True)
        cmbStatus.Tag = New DBControlInfo(sibi_requests.Status, SibiIndex.StatusType, True)
        txtPO.Tag = New DBControlInfo(sibi_requests.PO, False)
        txtReqNumber.Tag = New DBControlInfo(sibi_requests.RequisitionNumber, False)
        txtRequestNum.Tag = New DBControlInfo(sibi_requests.RequestNumber, ParseType.DisplayOnly, False)
        txtRTNumber.Tag = New DBControlInfo(sibi_requests.RT_Number, False)
        txtCreateDate.Tag = New DBControlInfo(sibi_requests.DateStamp, ParseType.DisplayOnly, False)

    End Sub
    Public Sub SetAttachCount()
        If Not OfflineMode Then
            cmdAttachments.Text = "(" + AssetFunc.GetAttachmentCount(CurrentRequest.strUID, New sibi_attachments).ToString + ")"
            cmdAttachments.ToolTipText = "Attachments " + cmdAttachments.Text
        End If
    End Sub
    Private Sub SetTitle()
        If MyText = "" Then
            MyText = Me.Text
        End If
        Me.Text = MyText + " - " + CurrentRequest.strDescription
    End Sub
    Public Sub ClearAll()
        ClearControls(Me)
        ResetBackColors(Me)
        HideEditControls()
        dgvNotes.DataSource = Nothing
        SetupGrid()
        FillCombos()
        EnableControls(Me)
        pnlCreate.Visible = False
        CurrentRequest = Nothing
        DisableControls(Me)
        ToolStrip.BackColor = colSibiToolBarColor
        bolUpdating = False
        fieldErrorIcon.Clear()
    End Sub
    Private Sub ClearTextBoxes(ByVal control As Control)
        If TypeOf control Is TextBox Then
            Dim txt = DirectCast(control, TextBox)
            txt.Clear()
        End If
    End Sub
    Private Sub ClearCombos(ByVal control As Control)
        If TypeOf control Is ComboBox Then
            Dim cmb = DirectCast(control, ComboBox)
            cmb.SelectedIndex = -1
            cmb.Text = Nothing
        End If
    End Sub
    Private Sub ClearDTPicker(ByVal control As Control)
        If TypeOf control Is DateTimePicker Then
            Dim dtp = DirectCast(control, DateTimePicker)
            dtp.Value = Now
        End If
    End Sub
    Private Sub ClearCheckBox(ByVal control As Control)
        If TypeOf control Is CheckBox Then
            Dim chk = DirectCast(control, CheckBox)
            chk.Checked = False
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
    Private Sub DisableControls(ByVal control As Control)
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
                DisableControls(c)
            End If
        Next
        DisableGrid()
    End Sub
    Private Sub EnableControls(ByVal control As Control)
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
                EnableControls(c)
            End If
        Next
        EnableGrid()
    End Sub
    Private Sub ShowEditControls()
        pnlEditButtons.Visible = True
    End Sub
    Private Sub HideEditControls()
        pnlEditButtons.Visible = False
    End Sub
    Private Sub DisableGrid()
        RequestItemsGrid.EditMode = DataGridViewEditMode.EditProgrammatically
        RequestItemsGrid.AllowUserToAddRows = False
        RequestItemsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells
    End Sub
    Private Sub EnableGrid()
        RequestItemsGrid.EditMode = DataGridViewEditMode.EditOnEnter
        RequestItemsGrid.AllowUserToAddRows = True
        SetColumnWidths()
    End Sub
    Private Sub SetColumnWidths()
        RequestItemsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None
        RequestItemsGrid.Columns(1).Width = 200
        RequestItemsGrid.Columns(2).Width = 50
        RequestItemsGrid.Columns(3).Width = 200
        RequestItemsGrid.Columns(4).Width = 200
        RequestItemsGrid.RowHeadersWidth = 57
    End Sub
    Private Sub SetupGrid()
        Try
            bolGridFilling = True
            RequestItemsGrid.DataSource = Nothing
            RequestItemsGrid.Rows.Clear()
            RequestItemsGrid.Columns.Clear()
            RequestItemsGrid.AutoGenerateColumns = False

            For Each col In RequestItemsColumns()
                RequestItemsGrid.Columns.Add(GetColumn(col))
            Next
            bolGridFilling = False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Function GetColumn(Column As ColumnStruct) As DataGridViewColumn
        Select Case Column.ColumnType
            Case GetType(String), GetType(Integer)
                Return GenericColumn(Column)
            Case GetType(Combo_Data)
                Select Case Column.ColumnName
                    Case sibi_request_items.Location
                        Return DataGridCombo(DeviceIndex.Locations, Column.ColumnCaption, Column.ColumnName)
                    Case sibi_request_items.Status
                        Return DataGridCombo(SibiIndex.ItemStatusType, Column.ColumnCaption, Column.ColumnName)
                End Select
        End Select
    End Function
    Private Function GenericColumn(Column As ColumnStruct) As DataGridViewColumn
        Dim NewCol As New DataGridViewColumn
        NewCol.Name = Column.ColumnName
        NewCol.DataPropertyName = Column.ColumnName
        NewCol.HeaderText = Column.ColumnCaption
        NewCol.ValueType = Column.ColumnType
        NewCol.CellTemplate = New DataGridViewTextBoxCell
        NewCol.SortMode = DataGridViewColumnSortMode.Automatic
        NewCol.ReadOnly = Column.ColumnReadOnly
        NewCol.Visible = Column.ColumnVisible
        Return NewCol
    End Function
    Private Function DataGridCombo(IndexType() As Combo_Data, HeaderText As String, Name As String) As DataGridViewComboBoxColumn
        Dim NewCombo As New DataGridViewComboBoxColumn
        NewCombo.Items.Clear()
        NewCombo.HeaderText = HeaderText
        NewCombo.DataPropertyName = Name
        NewCombo.Name = Name
        NewCombo.Width = 200
        NewCombo.SortMode = DataGridViewColumnSortMode.Automatic
        NewCombo.DisplayMember = "strLong"
        NewCombo.ValueMember = "strShort"
        NewCombo.DataSource = IndexType
        Return NewCombo
    End Function
    Private Sub FillCombos()
        FillComboBox(SibiIndex.StatusType, cmbStatus)
        FillComboBox(SibiIndex.RequestType, cmbType)
    End Sub
    Private Function ValidateFields() As Boolean
        bolFieldsValid = True
        Dim ValidateResults As Boolean = CheckFields(Me, bolFieldsValid)
        If ValidateResults Then ValidateResults = ValidateRequestItems()
        If Not ValidateResults Then
            Dim blah = Message("Some required fields are missing. Please enter data into all require fields.", vbOKOnly + vbExclamation, "Missing Data", Me)
        End If
        Return ValidateResults
    End Function
    Private bolFieldsValid As Boolean
    Private Function CheckFields(parent As Control, FieldsValid As Boolean) As Boolean
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
            If c.HasChildren Then CheckFields(c, bolFieldsValid)
        Next
        Return bolFieldsValid 'if fields are missing return false to trigger a message if needed
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
                    If dcell.OwningColumn.Name = sibi_request_items.Qty Then
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
    Private Sub AddErrorIcon(ctl As Control)
        If fieldErrorIcon.GetError(ctl) Is String.Empty Then
            fieldErrorIcon.SetIconAlignment(ctl, ErrorIconAlignment.MiddleRight)
            fieldErrorIcon.SetIconPadding(ctl, 4)
            fieldErrorIcon.SetError(ctl, "Required Field")
        End If
    End Sub
    Private Sub ClearErrorIcon(ctl As Control)
        fieldErrorIcon.SetError(ctl, String.Empty)
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

    Private Function CollectData() As Request_Info
        Try
            Dim info As New Request_Info
            With info
                .strDescription = Trim(txtDescription.Text)
                .strUser = Trim(txtUser.Text)
                .strType = GetDBValue(SibiIndex.RequestType, cmbType.SelectedIndex)
                .dtNeedBy = dtNeedBy.Value
                .strStatus = GetDBValue(SibiIndex.StatusType, cmbStatus.SelectedIndex)
                .strPO = Trim(txtPO.Text)
                .strRequisitionNumber = Trim(txtReqNumber.Text)
                .strRTNumber = Trim(txtRTNumber.Text)
            End With
            RequestItemsGrid.EndEdit()
            For Each row As DataGridViewRow In RequestItemsGrid.Rows
                For Each dcell As DataGridViewCell In row.Cells
                    If dcell.Value IsNot Nothing Then
                        dcell.Value = Trim(dcell.Value.ToString)
                    Else
                        Select Case dcell.OwningColumn.Name
                            Case sibi_request_items.Item_UID
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
            EndProgram()
        End Try
    End Function
    Private Sub cmdAddNew_Click(sender As Object, e As EventArgs) Handles cmdAddNew.Click
        If Not CheckForAccess(AccessGroup.Sibi_Add) Then Exit Sub
        AddNewRequest()
    End Sub
    Private Sub AddNewRequest()
        If Not ValidateFields() Then Exit Sub
        Dim RequestData As Request_Info = CollectData()
        Try
            Dim InsertRequestQry As String = "SELECT * FROM " & sibi_requests.TableName & " LIMIT 0"
            Dim InsertRequestItemsQry As String = "SELECT " & ColumnsString() & " FROM " & sibi_request_items.TableName & " LIMIT 0"
            Using SQLComms As New MySQL_Comms,
                cmd As MySqlCommand = SQLComms.Return_SQLCommand(),
                InsertAdapter = SQLComms.Return_Adapter(InsertRequestQry),
                InsertItemsAdapter = SQLComms.Return_Adapter(InsertRequestItemsQry)

                InsertAdapter.Update(GetInsertTable(InsertAdapter, CurrentRequest.strUID))
                InsertItemsAdapter.Update(RequestData.RequestItems)
            End Using
            pnlCreate.Visible = False
            Dim blah = Message("New Request Added.", vbOKOnly + vbInformation, "Complete", Me)
            If TypeOf Me.Tag Is SibiMainForm Then
                Dim ParentForm As SibiMainForm = DirectCast(Me.Tag, SibiMainForm)
                ParentForm.RefreshResults()
            End If
            bolUpdating = False
            OpenRequest(CurrentRequest.strUID)
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
                Exit Sub
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Private Function GetUpdateTable(Adapter As MySqlDataAdapter) As DataTable
        Try
            Dim tmpTable = DataParser.ReturnUpdateTable(Adapter.SelectCommand.CommandText)
            'Add Add'l info
            Return tmpTable
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Private Function GetInsertTable(Adapter As MySqlDataAdapter, UID As String) As DataTable
        Try
            Dim tmpTable = DataParser.ReturnInsertTable(Adapter.SelectCommand.CommandText)
            Dim DBRow = tmpTable.Rows(0)
            'Add Add'l info
            DBRow(sibi_requests.UID) = UID
            Return tmpTable
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Private Sub UpdateRequest()
        Try
            Dim RequestData As Request_Info = CollectData()
            RequestData.strUID = CurrentRequest.strUID
            If RequestData.RequestItems Is Nothing Then Exit Sub
            Dim RequestUpdateQry As String = "SELECT * FROM " & sibi_requests.TableName & " WHERE " & sibi_requests.UID & " = '" & CurrentRequest.strUID & "'"
            Dim RequestItemsUpdateQry As String = "SELECT " & ColumnsString() & " FROM " & sibi_request_items.TableName & " WHERE " & sibi_request_items.Request_UID & " = '" & CurrentRequest.strUID & "'"
            Using SQLComms As New MySQL_Comms,
               RequestUpdateAdapter = SQLComms.Return_Adapter(RequestUpdateQry),
               RequestItemsUpdateAdapter = SQLComms.Return_Adapter(RequestItemsUpdateQry)
                RequestUpdateAdapter.Update(GetUpdateTable(RequestUpdateAdapter))
                RequestItemsUpdateAdapter.Update(RequestData.RequestItems)
            End Using
            If TypeOf Me.Tag Is SibiMainForm Then
                Dim ParentForm As SibiMainForm = DirectCast(Me.Tag, SibiMainForm)
                ParentForm.RefreshResults()
            End If
            OpenRequest(CurrentRequest.strUID)
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Private Function AddNewNote(RequestUID As String, Note As String) As Boolean
        Dim strNoteUID As String = Guid.NewGuid.ToString
        Try
            Dim strAddNoteQry As String = "INSERT INTO " & sibi_notes.TableName & "
(" & sibi_notes.Request_UID & ",
" & sibi_notes.Note_UID & ",
" & sibi_notes.Note & ")
VALUES
(@" & sibi_notes.Request_UID & ",
@" & sibi_notes.Note_UID & ",
@" & sibi_notes.Note & ")"
            Using SQLComms As New MySQL_Comms, cmd As MySqlCommand = SQLComms.Return_SQLCommand(strAddNoteQry)
                cmd.Parameters.AddWithValue("@" & sibi_notes.Request_UID, RequestUID)
                cmd.Parameters.AddWithValue("@" & sibi_notes.Note_UID, strNoteUID)
                cmd.Parameters.AddWithValue("@" & sibi_notes.Note, Note)
                If cmd.ExecuteNonQuery() > 0 Then
                    Return True
                End If
                Return False
            End Using
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
                Return False
            End If
        End Try
    End Function
    Public Sub OpenRequest(RequestUID As String)
        Waiting()
        Try
            Dim strRequestQRY As String = "SELECT * FROM " & sibi_requests.TableName & " WHERE " & sibi_requests.UID & "='" & RequestUID & "'"
            Dim strRequestItemsQRY As String = "SELECT " & ColumnsString() & " FROM " & sibi_request_items.TableName & " WHERE " & sibi_request_items.Request_UID & "='" & RequestUID & "' ORDER BY " & sibi_request_items.TimeStamp
            Dim RequestResults As DataTable = DBFunc.DataTableFromQueryString(strRequestQRY)
            Dim RequestItemsResults As DataTable = DBFunc.DataTableFromQueryString(strRequestItemsQRY)
            RequestItemsResults.TableName = sibi_request_items.TableName
            ClearAll()
            CollectRequestInfo(RequestResults, RequestItemsResults)
            DataParser.FillDBFields(RequestResults)
            SendToGrid(RequestItemsResults)
            LoadNotes(CurrentRequest.strUID)
            DisableControls(Me)
            SetTitle()
            SetAttachCount()
            Me.Show()
            Me.Activate()
            SetMunisStatus()
            bolGridFilling = False
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
                Dispose()
            Else
                EndProgram()
            End If
        Finally
            DoneWaiting()
        End Try
    End Sub
    Private Sub LoadNotes(RequestUID As String)
        Try
            Dim strPullNotesQry As String = "SELECT * FROM " & sibi_notes.TableName & " WHERE " & sibi_notes.Request_UID & "='" & RequestUID & "' ORDER BY " & sibi_notes.DateStamp & " DESC"
            Using table As New DataTable, Results As DataTable = DBFunc.DataTableFromQueryString(strPullNotesQry)
                Dim intPreviewChars As Integer = 50
                table.Columns.Add("Date Stamp")
                table.Columns.Add("Preview")
                table.Columns.Add("UID")
                For Each r As DataRow In Results.Rows
                    Dim NoteText As String = RTFToPlainText(r.Item(sibi_notes.Note).ToString)
                    table.Rows.Add(r.Item(sibi_notes.DateStamp),
                                   IIf(Len(NoteText) > intPreviewChars, NotePreview(NoteText), NoteText),
                                   r.Item(sibi_notes.Note_UID))
                Next
                dgvNotes.DataSource = table
                dgvNotes.ClearSelection()
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Function RTFToPlainText(strRTF As String) As String
        Try
            Using rtBox As New RichTextBox
                rtBox.Rtf = strRTF
                Return rtBox.Text
            End Using
        Catch ex As ArgumentException
            'If we get an argument error, that means the text is not RTF so we return the plain text.
            Return strRTF
        End Try
    End Function
    Private Function DeleteItem_FromSQL(ItemUID As String, ItemColumnName As String, Table As String) As Integer
        Try
            Dim rows As Integer
            Dim strSQLQry As String = "DELETE FROM " & Table & " WHERE " & ItemColumnName & "='" & ItemUID & "'"
            Using SQLComms As New MySQL_Comms
                rows = SQLComms.Return_SQLCommand(strSQLQry).ExecuteNonQuery
                Return rows
            End Using
            UpdateRequest()
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
            Else
                EndProgram()
            End If
        End Try
        Return -1
    End Function
    Private Function DeleteItem_FromLocal(RowIndex As Integer) As Boolean
        Try
            If Not RequestItemsGrid.Rows(RowIndex).IsNewRow Then
                RequestItemsGrid.Rows.Remove(RequestItemsGrid.Rows(RowIndex))
                Return True
            End If
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
            Else
                Return False
                EndProgram()
            End If
        End Try
        Return False
    End Function
    Private Sub SendToGrid(Results As DataTable)
        Try
            bolGridFilling = True
            RequestItemsGrid.DataSource = Results
            RequestItemsGrid.ClearSelection()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Function RequestItemsColumns() As List(Of ColumnStruct)
        Dim ColList As New List(Of ColumnStruct)
        ColList.Add(New ColumnStruct(sibi_request_items.User, "User", GetType(String)))
        ColList.Add(New ColumnStruct(sibi_request_items.Description, "Description", GetType(String)))
        ColList.Add(New ColumnStruct(sibi_request_items.Qty, "Qty", GetType(Integer)))
        ColList.Add(New ColumnStruct(sibi_request_items.Location, "Location", GetType(Combo_Data)))
        ColList.Add(New ColumnStruct(sibi_request_items.Status, "Status", GetType(Combo_Data)))
        ColList.Add(New ColumnStruct(sibi_request_items.Replace_Asset, "Replace Asset", GetType(String)))
        ColList.Add(New ColumnStruct(sibi_request_items.Replace_Serial, "Replace Serial", GetType(String)))
        ColList.Add(New ColumnStruct(sibi_request_items.New_Asset, "New Asset", GetType(String)))
        ColList.Add(New ColumnStruct(sibi_request_items.New_Serial, "New Serial", GetType(String)))
        ColList.Add(New ColumnStruct(sibi_request_items.Org_Code, "Org Code", GetType(String)))
        ColList.Add(New ColumnStruct(sibi_request_items.Object_Code, "Object Code", GetType(String)))
        ColList.Add(New ColumnStruct(sibi_request_items.Item_UID, "Item UID", GetType(String), True, True))
        ColList.Add(New ColumnStruct(sibi_request_items.Request_UID, "Request UID", GetType(String), True, False))
        Return ColList
    End Function
    Private Function ColumnsString() As String
        Dim ColString As String = ""
        For Each col In RequestItemsColumns()
            ColString += col.ColumnName
            If RequestItemsColumns.IndexOf(col) <> RequestItemsColumns.Count - 1 Then ColString += ","
        Next
        Return ColString
    End Function
    Private Sub cmdClearAll_Click(sender As Object, e As EventArgs)
        ClearAll()
        CurrentRequest = Nothing
    End Sub
    Private Sub cmdUpdate_Click(sender As Object, e As EventArgs) Handles cmdUpdate.Click
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        If CurrentRequest.strUID <> "" Then UpdateMode(bolUpdating)
    End Sub
    Private Sub UpdateMode(Enable As Boolean)
        If Not Enable Then
            EnableControls(Me)
            ToolStrip.BackColor = colEditColor
            ShowEditControls()
            bolUpdating = True
        Else
            DisableControls(Me)
            ToolStrip.BackColor = colSibiToolBarColor
            HideEditControls()
            UpdateRequest()
            bolUpdating = False
        End If
    End Sub
    Private Sub cmdAttachments_Click(sender As Object, e As EventArgs) Handles cmdAttachments.Click
        If Not CheckForAccess(AccessGroup.ViewAttachment) Then Exit Sub
        If Not AttachmentsIsOpen() Then
            If CurrentRequest.strUID <> "" Then
                Dim NewAttach As New AttachmentsForm(Me, New sibi_attachments, CurrentRequest)
            End If
        Else
            ActivateFormByUID(CurrentRequest.strUID, Me)
        End If
    End Sub
    Public Function AttachmentsIsOpen() As Boolean
        For Each frm As Form In My.Application.OpenForms
            If TypeOf frm Is AttachmentsForm And frm.Tag Is Me Then
                Return True
            End If
        Next
        Return False
    End Function
    Private Sub cmdCreate_Click(sender As Object, e As EventArgs) Handles cmdCreate.Click
        NewRequest()
    End Sub
    Public Sub NewRequest()
        If Not CheckForAccess(AccessGroup.Sibi_Add) Then Exit Sub
        ClearAll()
        CurrentRequest.strUID = Guid.NewGuid.ToString
        bolUpdating = True
        SetupGrid()

        Using Comms As New MySQL_Comms 'Set the datasource to a new empty DB table.
            RequestItemsGrid.DataSource = Comms.Return_SQLTable("SELECT " & ColumnsString() & " FROM " & sibi_request_items.TableName & " LIMIT 0")
        End Using

        EnableControls(Me)
        pnlCreate.Visible = True
    End Sub
    Private Sub frmManageRequest_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Dim CancelClose As Boolean = CheckForActiveTransfers()
        If CancelClose Then
            e.Cancel = True
        Else
            MyMunisToolBar.Dispose()
            MyWindowList.Dispose()
            CloseChildren(Me)
        End If
    End Sub
    Private Sub tsmDeleteItem_Click(sender As Object, e As EventArgs) Handles tsmDeleteItem.Click
        Try
            If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
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
    Private Sub txtRTNumber_Click(sender As Object, e As EventArgs) Handles txtRTNumber.Click
        Try
            Dim RTNum As String = Trim(txtRTNumber.Text)
            If Not bolUpdating And RTNum <> "" Then
                Process.Start("http://rt.co.fairfield.oh.us/rt/Ticket/Display.html?id=" & RTNum)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub txtReqNumber_Click(sender As Object, e As EventArgs) Handles txtReqNumber.Click
        Dim ReqNum As String = Trim(txtReqNumber.Text)
        If Not bolUpdating And ReqNum <> "" Then
            MunisFunc.NewMunisView_ReqSearch(ReqNum, YearFromDate(CurrentRequest.dtDateStamp), Me)
        End If
    End Sub
    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        Try
            If Not CheckForAccess(AccessGroup.Sibi_Delete) Then Exit Sub
            If IsNothing(CurrentRequest.RequestItems) Then Exit Sub
            Dim blah = Message("Are you absolutely sure?  This cannot be undone and will delete all data including attachments.", vbYesNo + vbExclamation, "WARNING", Me)
            If blah = vbYes Then
                Waiting()
                If AssetFunc.DeleteMaster(CurrentRequest.strUID, Entry_Type.Sibi) Then
                    Dim blah2 = Message("Sibi Request deleted successfully.", vbOKOnly + vbInformation, "Device Deleted", Me)
                    CurrentRequest = Nothing
                    If TypeOf Me.Tag Is SibiMainForm Then
                        Dim ParentForm As SibiMainForm = DirectCast(Me.Tag, SibiMainForm)
                        ParentForm.RefreshResults()
                    End If
                    DoneWaiting()
                    Me.Dispose()
                Else
                    Logger("*****DELETION ERROR******: " & CurrentRequest.strUID)
                    Dim blah2 = Message("Failed to delete request succesfully!  Please let Bobby Lovell know about this.", vbOKOnly + vbCritical, "Delete Failed", Me)
                    CurrentRequest = Nothing
                    Me.Dispose()
                End If
            Else
                Exit Sub
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub cmdAddNote_Click(sender As Object, e As EventArgs) Handles cmdAddNote.Click
        AddNote()
    End Sub
    Private Sub AddNote()
        Try
            If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
            If CurrentRequest.strUID <> "" Then
                Dim NewNote As New SibiNotesForm(Me, CurrentRequest)
                If NewNote.DialogResult = DialogResult.OK Then
                    AddNewNote(NewNote.Request.strUID, Trim(NewNote.rtbNotes.Rtf))
                    RefreshRequest()
                End If
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub RefreshRequest()
        OpenRequest(CurrentRequest.strUID)
    End Sub
    Private Sub dgvNotes_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvNotes.CellDoubleClick
        Try
            Dim ViewNote As New SibiNotesForm(Me, dgvNotes.Item(GetColIndex(dgvNotes, "UID"), dgvNotes.CurrentRow.Index).Value.ToString)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub cmdDeleteNote_Click(sender As Object, e As EventArgs) Handles cmdDeleteNote.Click
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        Dim blah = Message("Are you sure?", vbYesNo + vbQuestion, "Delete Note", Me)
        If blah = vbYes Then
            Dim NoteUID As String = dgvNotes.Item(GetColIndex(dgvNotes, "UID"), dgvNotes.CurrentRow.Index).Value.ToString
            If NoteUID <> "" Then
                Message(DeleteItem_FromSQL(NoteUID, sibi_notes.Note_UID, sibi_notes.TableName) & " Rows affected.", vbOKOnly + vbInformation, "Delete Item", Me)
                OpenRequest(CurrentRequest.strUID)
            End If
        End If
    End Sub
    Private Sub RequestItemsGrid_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles RequestItemsGrid.CellMouseDown
        Try
            If e.ColumnIndex >= -1 And e.RowIndex >= 0 Then
                Dim ColIndex As Integer = CInt(IIf(e.ColumnIndex = -1, 0, e.ColumnIndex))
                If Not RequestItemsGrid.Item(ColIndex, e.RowIndex).Selected Then
                    RequestItemsGrid.Rows(e.RowIndex).Selected = True
                    RequestItemsGrid.CurrentCell = RequestItemsGrid(ColIndex, e.RowIndex)
                End If
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
                    If bolUpdating Then
                        tsmDeleteItem.Visible = True
                    Else
                        tsmDeleteItem.Visible = False
                    End If
                End If
            End If

        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Function ValidColumn() As Boolean
        Try
            Select Case True
                Case RequestItemsGrid.CurrentCell.ColumnIndex = GetColIndex(RequestItemsGrid, sibi_request_items.Replace_Asset)
                    Return True
                Case RequestItemsGrid.CurrentCell.ColumnIndex = GetColIndex(RequestItemsGrid, sibi_request_items.Replace_Serial)
                    Return True
                Case RequestItemsGrid.CurrentCell.ColumnIndex = GetColIndex(RequestItemsGrid, sibi_request_items.New_Asset)
                    Return True
                Case RequestItemsGrid.CurrentCell.ColumnIndex = GetColIndex(RequestItemsGrid, sibi_request_items.New_Serial)
                    Return True
                Case Else
                    Return False
            End Select
            Return False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Private Sub HighlightCurrentRow(Row As Integer)
        Try
            If Not bolGridFilling Then
                HighlightRow(RequestItemsGrid, GridTheme, Row)
            End If
        Catch
        End Try
    End Sub
    Private Sub RequestItemsGrid_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles RequestItemsGrid.CellEnter
        HighlightCurrentRow(e.RowIndex)
    End Sub
    Private Sub RequestItemsGrid_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles RequestItemsGrid.CellLeave
        LeaveRow(RequestItemsGrid, GridTheme, e.RowIndex)
    End Sub
    Private Sub tsmLookupDevice_Click(sender As Object, e As EventArgs) Handles tsmLookupDevice.Click
        Try
            Dim ColIndex As Integer = RequestItemsGrid.CurrentCell.ColumnIndex
            Select Case True
                Case ColIndex = GetColIndex(RequestItemsGrid, sibi_request_items.Replace_Asset)
                    LookupDevice(Me, AssetFunc.FindDevice(RequestItemsGrid.Item(ColIndex, RequestItemsGrid.CurrentRow.Index).Value.ToString, FindDevType.AssetTag))
                Case ColIndex = GetColIndex(RequestItemsGrid, sibi_request_items.Replace_Serial)
                    LookupDevice(Me, AssetFunc.FindDevice(RequestItemsGrid.Item(ColIndex, RequestItemsGrid.CurrentRow.Index).Value.ToString, FindDevType.Serial))
                Case ColIndex = GetColIndex(RequestItemsGrid, sibi_request_items.New_Asset)
                    LookupDevice(Me, AssetFunc.FindDevice(RequestItemsGrid.Item(ColIndex, RequestItemsGrid.CurrentRow.Index).Value.ToString, FindDevType.AssetTag))
                Case ColIndex = GetColIndex(RequestItemsGrid, sibi_request_items.New_Serial)
                    LookupDevice(Me, AssetFunc.FindDevice(RequestItemsGrid.Item(ColIndex, RequestItemsGrid.CurrentRow.Index).Value.ToString, FindDevType.Serial))
            End Select
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub tsmPopFA_Click(sender As Object, e As EventArgs) Handles tsmPopFA.Click
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        Try
            If ValidColumn() Then
                PopulateFromFA(RequestItemsGrid.CurrentCell.OwningColumn.Name)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub PopulateFromFA(ColumnName As String)
        Select Case ColumnName
            Case sibi_request_items.New_Serial
                Dim ItemUID = GetCurrentCellValue(RequestItemsGrid, sibi_request_items.Item_UID)
                Dim Serial = MunisFunc.Get_SerialFromAsset(GetCurrentCellValue(RequestItemsGrid, sibi_request_items.New_Asset))
                If Serial <> "" Then
                    AssetFunc.Update_SQLValue(sibi_request_items.TableName, sibi_request_items.New_Serial, Serial, sibi_request_items.Item_UID, ItemUID)
                    RefreshRequest()
                End If
            Case sibi_request_items.New_Asset
                Dim ItemUID = GetCurrentCellValue(RequestItemsGrid, sibi_request_items.Item_UID)
                Dim Asset = MunisFunc.Get_AssetFromSerial(GetCurrentCellValue(RequestItemsGrid, sibi_request_items.New_Serial))
                If Asset <> "" Then
                    AssetFunc.Update_SQLValue(sibi_request_items.TableName, sibi_request_items.New_Asset, Asset, sibi_request_items.Item_UID, ItemUID)
                    RefreshRequest()
                End If
            Case sibi_request_items.Replace_Serial
                Dim ItemUID = GetCurrentCellValue(RequestItemsGrid, sibi_request_items.Item_UID)
                Dim Serial = MunisFunc.Get_SerialFromAsset(GetCurrentCellValue(RequestItemsGrid, sibi_request_items.Replace_Asset))
                If Serial <> "" Then
                    AssetFunc.Update_SQLValue(sibi_request_items.TableName, sibi_request_items.Replace_Serial, Serial, sibi_request_items.Item_UID, ItemUID)
                    RefreshRequest()
                End If
            Case sibi_request_items.Replace_Asset

                Dim ItemUID = GetCurrentCellValue(RequestItemsGrid, sibi_request_items.Item_UID)
                Dim Asset = MunisFunc.Get_AssetFromSerial(GetCurrentCellValue(RequestItemsGrid, sibi_request_items.Replace_Serial))
                If Asset <> "" Then
                    AssetFunc.Update_SQLValue(sibi_request_items.TableName, sibi_request_items.Replace_Asset, Asset, sibi_request_items.Item_UID, ItemUID)
                    RefreshRequest()
                End If
        End Select
    End Sub
    Private Sub cmdAccept_Click(sender As Object, e As EventArgs) Handles cmdAccept.Click
        RequestItemsGrid.EndEdit()
        If Not ValidateFields() Then Exit Sub
        DisableControls(Me)
        ToolStrip.BackColor = colSibiToolBarColor
        HideEditControls()
        UpdateRequest()
        bolUpdating = False
    End Sub
    Private Sub cmdDiscard_Click(sender As Object, e As EventArgs) Handles cmdDiscard.Click
        HideEditControls()
        OpenRequest(CurrentRequest.strUID)
    End Sub
    Private Sub txtPO_Click(sender As Object, e As EventArgs) Handles txtPO.Click
        Dim PO As String = Trim(txtPO.Text)
        If Not bolUpdating And PO <> "" Then
            MunisFunc.NewMunisView_POSearch(PO, Me)
        End If
    End Sub
    Private Sub RequestItemsGrid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles RequestItemsGrid.DataError
        Dim blah = Message("DataGrid Error: " & Chr(34) & e.Exception.Message & Chr(34) & "   Col/Row:" & e.ColumnIndex & "/" & e.RowIndex, vbOKOnly + vbExclamation, "DataGrid Error", Me)
    End Sub
    Private Sub RequestItemsGrid_DefaultValuesNeeded(sender As Object, e As DataGridViewRowEventArgs) Handles RequestItemsGrid.DefaultValuesNeeded
        e.Row.Cells(sibi_request_items.Qty).Value = 1
    End Sub
    Private Sub CollectRequestInfo(RequestResults As DataTable, RequestItemsResults As DataTable)
        Try
            With CurrentRequest
                .strUID = NoNull(RequestResults.Rows(0).Item(sibi_requests.UID))
                .strUser = NoNull(RequestResults.Rows(0).Item(sibi_requests.RequestUser))
                .strDescription = NoNull(RequestResults.Rows(0).Item(sibi_requests.Description))
                .dtDateStamp = DateTime.Parse(NoNull(RequestResults.Rows(0).Item(sibi_requests.DateStamp)))
                .dtNeedBy = DateTime.Parse(NoNull(RequestResults.Rows(0).Item(sibi_requests.NeedBy)))
                .strStatus = NoNull(RequestResults.Rows(0).Item(sibi_requests.Status))
                .strType = NoNull(RequestResults.Rows(0).Item(sibi_requests.Type))
                .strPO = NoNull(RequestResults.Rows(0).Item(sibi_requests.PO))
                .strRequisitionNumber = NoNull(RequestResults.Rows(0).Item(sibi_requests.RequisitionNumber))
                .strReplaceAsset = NoNull(RequestResults.Rows(0).Item(sibi_requests.Replace_Asset))
                .strReplaceSerial = NoNull(RequestResults.Rows(0).Item(sibi_requests.Replace_Serial))
                .strRequestNumber = NoNull(RequestResults.Rows(0).Item(sibi_requests.RequestNumber))
                .RequestItems = RequestItemsResults
            End With
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
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
    Private PrevWindowState As Integer
    Private Sub frmManageRequest_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin
        Dim f As Form = DirectCast(sender, Form)
        PrevWindowState = f.WindowState
    End Sub
    Private Sub frmManageRequest_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        CloseChildren(Me)
    End Sub
    Private Sub cmdNewNote_Click(sender As Object, e As EventArgs) Handles cmdNewNote.Click
        AddNote()
    End Sub
    Private Sub Waiting()
        SetWaitCursor(True)
    End Sub
    Private Sub DoneWaiting()
        SetWaitCursor(False)
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
    Private Sub RequestItemsGrid_DragLeave(sender As Object, e As EventArgs) Handles RequestItemsGrid.DragLeave
        bolDragging = False
    End Sub
    Private Sub RequestItemsGrid_DragEnter(sender As Object, e As DragEventArgs) Handles RequestItemsGrid.DragEnter
        e.Effect = DragDropEffects.Copy
    End Sub
    Private Sub RequestItemsGrid_DragDrop(sender As Object, e As DragEventArgs) Handles RequestItemsGrid.DragDrop
        'Drag-drop rows from other data grids, adding new UIDs and the currect FKey (RequestUID).
        'This was a tough nut to crack. In the end I just ended up building an item array and adding it to the receiving grids datasource.
        Try
            If bolUpdating Then
                Dim R = DirectCast(e.Data.GetData(GetType(DataGridViewRow)), DataGridViewRow) 'Cast the DGVRow
                Dim NewDataRow As DataRow = DirectCast(R.DataBoundItem, DataRowView).Row 'Get the databound row
                Dim ItemArr As New List(Of Object)
                For Each col As DataColumn In NewDataRow.Table.Columns 'Iterate through columns and build a new item list
                    Select Case col.ColumnName
                        Case sibi_request_items.Item_UID
                            ItemArr.Add(Guid.NewGuid.ToString)
                        Case sibi_request_items.Request_UID
                            ItemArr.Add(CurrentRequest.strUID)
                        Case Else
                            ItemArr.Add(NewDataRow.Item(col))
                    End Select
                Next
                DirectCast(RequestItemsGrid.DataSource, DataTable).Rows.Add(ItemArr.ToArray) 'Add the item list as an array
                bolDragging = False
            Else
                If Not bolDragging Then
                    Message("You must be modifying this request before you can drag-drop rows from another request.", vbOKOnly + MsgBoxStyle.Exclamation, "Not Allowed", Me)
                End If
            End If
        Catch ex As Exception
        End Try
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
    Private Sub SetMunisStatus()
        If Not OfflineMode Then
            SetReqStatus(CurrentRequest.strRequisitionNumber, CurrentRequest.dtDateStamp.Year)
            CheckForPO()
            SetPOStatus(CurrentRequest.strPO)
        End If
    End Sub
    Private Async Sub SetPOStatus(PO As String)
        Dim intPO As Integer = 0
        If PO <> "" And Int32.TryParse(PO, intPO) Then
            Dim GetStatusString As String = Await MunisFunc.Get_PO_Status(intPO)
            If GetStatusString <> "" Then
                lblPOStatus.Text = "Status: " & GetStatusString
            Else
                lblPOStatus.Text = "Status: NA"
            End If
        Else
            lblPOStatus.Text = "Status: NA"
        End If
    End Sub
    Private Async Sub SetReqStatus(ReqNum As String, FY As Integer)
        Dim intReq As Integer = 0
        If FY > 0 Then
            If ReqNum <> "" And Int32.TryParse(ReqNum, intReq) Then
                Dim GetStatusString As String = Await MunisFunc.Get_Req_Status(ReqNum, FY)
                If GetStatusString <> "" Then
                    lblReqStatus.Text = "Status: " & GetStatusString
                Else
                    lblReqStatus.Text = "Status: NA"
                End If
            Else
                lblReqStatus.Text = "Status: NA"
            End If
        End If
    End Sub
    Private Async Sub CheckForPO()
        If CurrentRequest.strRequisitionNumber <> "" And CurrentRequest.strPO = "" Then
            Dim GetPO As String = Await MunisFunc.Get_PO_From_ReqNumber_Async(CurrentRequest.strRequisitionNumber, CurrentRequest.dtDateStamp.Year.ToString)
            If GetPO.Length > 1 Then
                Dim blah = Message("PO Number " & GetPO & " was detected in the Requisition. Do you wish to add it to this request?", vbQuestion + vbYesNo, "New PO Detected", Me)
                If blah = MsgBoxResult.Yes Then
                    InsertPONumber(GetPO)
                    OpenRequest(CurrentRequest.strUID)
                Else
                    Exit Sub
                End If
            End If
        End If
    End Sub
    Private Sub InsertPONumber(PO As String)
        Try
            AssetFunc.Update_SQLValue(sibi_requests.TableName, sibi_requests.PO, PO, sibi_requests.UID, CurrentRequest.strUID)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub tsmCopyText_Click(sender As Object, e As EventArgs) Handles tsmCopyText.Click
        RequestItemsGrid.RowHeadersVisible = False
        RequestItemsGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
        Clipboard.SetDataObject(RequestItemsGrid.GetClipboardContent())
        RequestItemsGrid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText
        RequestItemsGrid.RowHeadersVisible = True
    End Sub
    Private Sub tsbRefresh_Click(sender As Object, e As EventArgs) Handles tsbRefresh.Click
        OpenRequest(CurrentRequest.strUID)
    End Sub
    Private Sub RequestItemsGrid_RowEnter(sender As Object, e As DataGridViewCellEventArgs) Handles RequestItemsGrid.RowEnter
        If Not bolGridFilling Then
            RequestItemsGrid.Rows(e.RowIndex).Cells(sibi_request_items.Request_UID).Value = CurrentRequest.strUID
            If RequestItemsGrid.Rows(e.RowIndex).Cells(sibi_request_items.Item_UID).Value Is Nothing Then RequestItemsGrid.Rows(e.RowIndex).Cells(sibi_request_items.Item_UID).Value = Guid.NewGuid.ToString
        End If
    End Sub
    Private Sub RequestItemsGrid_RowLeave(sender As Object, e As DataGridViewCellEventArgs) Handles RequestItemsGrid.RowLeave
        If bolUpdating Then ValidateRequestItems()
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

End Class
