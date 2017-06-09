﻿Option Explicit On
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
        cmdAttachments.Text = "(" + AssetFunc.GetAttachmentCount(CurrentRequest).ToString + ")"
        cmdAttachments.ToolTipText = "Attachments " + cmdAttachments.Text
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
            RequestItemsGrid.DataSource = Nothing
            RequestItemsGrid.Rows.Clear()
            RequestItemsGrid.Columns.Clear()
            Dim intQty As New DataGridViewColumn
            intQty.Name = "Qty"
            intQty.HeaderText = "Qty"
            intQty.ValueType = GetType(Integer)
            intQty.CellTemplate = New DataGridViewTextBoxCell
            intQty.SortMode = DataGridViewColumnSortMode.Automatic
            With RequestItemsGrid.Columns
                .Add("User", "User")
                .Add("Description", "Description")
                .Add(intQty) '"Qty", "Qty")
                .Add(DataGridCombo(DeviceIndex.Locations, "Location", Attrib_Type.Location)) '.Add("Location")
                .Add(DataGridCombo(SibiIndex.ItemStatusType, "Status", Attrib_Type.SibiItemStatusType))
                .Add("Replace Asset", "Replace Asset")
                .Add("Replace Serial", "Replace Serial")
                .Add("New Asset", "New Asset")
                .Add("New Serial", "New Serial")
                .Add("Org Code", "Org Code")
                .Add("Object Code", "Object Code")
                .Add("Item UID", "Item UID")
            End With
            SetColumnWidths()
            RequestItemsGrid.Columns.Item("Item UID").ReadOnly = True
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub FillCombos()
        FillComboBox(SibiIndex.StatusType, cmbStatus)
        FillComboBox(SibiIndex.RequestType, cmbType)
    End Sub
    Private Sub SetFieldTags()
        txtDescription.Tag = True
        txtUser.Tag = True
        cmbType.Tag = True
        dtNeedBy.Tag = True
        cmbStatus.Tag = True
        RequestItemsGrid.Tag = True
    End Sub
    Private Function ValidateFields() As Boolean
        '    SetFieldTags()
        bolFieldsValid = True
        Dim ValidateResults As Boolean = CheckFields(Me, bolFieldsValid)
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
            If DBInfo.Required Then 'Not IsNothing(c.Tag) And c.Tag = True Then
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
        ValidateRequestItems()
        Return bolFieldsValid 'if fields are missing return false to trigger a message if needed
    End Function
    Private Sub ValidateRequestItems()
        For Each row As DataGridViewRow In RequestItemsGrid.Rows
            If Not row.IsNewRow Then
                For Each dcell As DataGridViewCell In row.Cells
                    If dcell.OwningColumn.CellType.Name = "DataGridViewComboBoxCell" Then
                        If dcell.Value Is Nothing Then
                            bolFieldsValid = False
                            dcell.ErrorText = "Required Field!"
                        Else
                            dcell.ErrorText = Nothing
                        End If
                    End If
                    If dcell.OwningColumn.Name = "Qty" Then
                        If IsDBNull(dcell.Value) Then
                            bolFieldsValid = False
                            dcell.ErrorText = "Required Field!"
                        Else
                            dcell.ErrorText = Nothing
                        End If
                    End If
                Next
            End If
        Next
    End Sub
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
    Private Function DataGridCombo(IndexType() As Combo_Data, HeaderText As String, Name As String) As DataGridViewComboBoxColumn
        Dim tmpCombo As New DataGridViewComboBoxColumn
        tmpCombo.Items.Clear()
        tmpCombo.HeaderText = HeaderText
        tmpCombo.Name = Name
        tmpCombo.Width = 200
        tmpCombo.SortMode = DataGridViewColumnSortMode.Automatic
        Dim myList As New List(Of String)
        For Each ComboItem As Combo_Data In IndexType
            myList.Add(ComboItem.strLong)
        Next
        tmpCombo.DataSource = myList
        Return tmpCombo
    End Function
    Private Function CollectData() As Request_Info
        Try
            Dim info As Request_Info
            ' RequestItemsGrid.EndEdit()
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
            Dim DBTable As New DataTable
            For Each col As DataGridViewColumn In RequestItemsGrid.Columns
                DBTable.Columns.Add(col.Name)
            Next
            For Each row As DataGridViewRow In RequestItemsGrid.Rows
                If Not row.IsNewRow Then
                    Dim NewRow As DataRow = DBTable.NewRow()
                    For Each dcell As DataGridViewCell In row.Cells
                        If dcell.OwningColumn.CellType Is GetType(DataGridViewComboBoxCell) Then
                            Dim cmb = DirectCast(dcell, DataGridViewComboBoxCell)
                            Dim SelectedIndex = cmb.Items.IndexOf(dcell.Value)
                            Select Case dcell.OwningColumn.Name
                                Case Attrib_Type.Location
                                    NewRow(dcell.ColumnIndex) = GetDBValue(DeviceIndex.Locations, SelectedIndex)
                                Case Attrib_Type.SibiItemStatusType
                                    NewRow(dcell.ColumnIndex) = GetDBValue(SibiIndex.ItemStatusType, SelectedIndex)
                            End Select
                        Else
                            If dcell.Value IsNot Nothing Then
                                NewRow(dcell.ColumnIndex) = Trim(dcell.Value.ToString)
                            Else
                                NewRow(dcell.ColumnIndex) = Nothing
                            End If
                        End If
                    Next
                    DBTable.Rows.Add(NewRow)
                End If
            Next
            info.RequstItems = DBTable
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
        RequestData.strUID = Guid.NewGuid.ToString
        Try
            Dim rows As Integer
            Dim InsertRequestQry As String = "SELECT * FROM " & sibi_requests.TableName & " LIMIT 0"
            Using SQLComms As New MySQL_Comms,
                cmd As MySqlCommand = SQLComms.Return_SQLCommand(),
                InsertAdapter = SQLComms.Return_Adapter(InsertRequestQry)
                rows += InsertAdapter.Update(GetInsertTable(InsertAdapter, RequestData.strUID))
                For Each row As DataRow In RequestData.RequstItems.Rows
                    InsertRequestItem(row, cmd, RequestData)
                    rows = rows + cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                Next
            End Using
            pnlCreate.Visible = False
            Dim blah = Message("New Request Added.", vbOKOnly + vbInformation, "Complete", Me)
            If TypeOf Me.Tag Is SibiMainForm Then
                Dim ParentForm As SibiMainForm = DirectCast(Me.Tag, SibiMainForm)
                ParentForm.RefreshResults()
            End If
            OpenRequest(RequestData.strUID)
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
            If RequestData.RequstItems Is Nothing Then Exit Sub
            Dim rows As Integer = 0
            Dim RequestUpdateQry As String = "SELECT * FROM " & sibi_requests.TableName & " WHERE " & sibi_requests.UID & " = '" & CurrentRequest.strUID & "'"
            Using SQLComms As New MySQL_Comms,
                cmd As MySqlCommand = SQLComms.Return_SQLCommand(),
                RequestUpdateAdapter = SQLComms.Return_Adapter(RequestUpdateQry)
                rows += RequestUpdateAdapter.Update(GetUpdateTable(RequestUpdateAdapter))
                For Each row As DataRow In RequestData.RequstItems.Rows
                    If row.Item("Item UID").ToString <> "" Then
                        SetRequestItemParameters(row, cmd, RequestData, False)
                        cmd.CommandText = RequestItemUpdateQry(row)
                        cmd.ExecuteNonQuery()
                        cmd.Parameters.Clear()
                    Else
                        InsertRequestItem(row, cmd, RequestData)
                        rows += cmd.ExecuteNonQuery()
                        cmd.Parameters.Clear()
                    End If
                Next
            End Using
            If TypeOf Me.Tag Is SibiMainForm Then
                Dim ParentForm As SibiMainForm = DirectCast(Me.Tag, SibiMainForm)
                ParentForm.RefreshResults()
            End If
            ' Message("Success!")
            OpenRequest(CurrentRequest.strUID)
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Private Function RequestItemUpdateQry(row As DataRow) As String
        Return "UPDATE " & sibi_request_items.TableName & "
                    SET
                    " & sibi_request_items.User & " = @" & sibi_request_items.User & " ,
                    " & sibi_request_items.Description & " = @" & sibi_request_items.Description & " ,
                    " & sibi_request_items.Location & " = @" & sibi_request_items.Location & " ,
                    " & sibi_request_items.Status & " = @" & sibi_request_items.Status & " ,
                    " & sibi_request_items.Replace_Asset & " = @" & sibi_request_items.Replace_Asset & " ,
                    " & sibi_request_items.Replace_Serial & " = @" & sibi_request_items.Replace_Serial & ",
                    " & sibi_request_items.New_Asset & " = @" & sibi_request_items.New_Asset & " ,
                    " & sibi_request_items.New_Serial & " = @" & sibi_request_items.New_Serial & ",
                    " & sibi_request_items.Org_Code & " = @" & sibi_request_items.Org_Code & ",
                    " & sibi_request_items.Object_Code & " = @" & sibi_request_items.Object_Code & ",
                    " & sibi_request_items.Qty & " = @" & sibi_request_items.Qty & "
                    WHERE " & sibi_request_items.Item_UID & " ='" & row.Item("Item UID").ToString & "'"
    End Function
    Private Function RequestItemInsertQry() As String
        Return "INSERT INTO " & sibi_request_items.TableName & "
(" & sibi_request_items.Item_UID & ",
" & sibi_request_items.Request_UID & ",
" & sibi_request_items.User & ",
" & sibi_request_items.Description & ",
" & sibi_request_items.Location & ",
" & sibi_request_items.Status & ",
" & sibi_request_items.Replace_Asset & ",
" & sibi_request_items.Replace_Serial & ",
" & sibi_request_items.New_Asset & ",
" & sibi_request_items.New_Serial & ",
" & sibi_request_items.Org_Code & ",
" & sibi_request_items.Object_Code & ",
" & sibi_request_items.Qty & ",
" & sibi_request_items.Sequence & "
)
VALUES
(@" & sibi_request_items.Item_UID & ",
@" & sibi_request_items.Request_UID & ",
@" & sibi_request_items.User & ",
@" & sibi_request_items.Description & ",
@" & sibi_request_items.Location & ",
@" & sibi_request_items.Status & ",
@" & sibi_request_items.Replace_Asset & ",
@" & sibi_request_items.Replace_Serial & ",
@" & sibi_request_items.New_Asset & ",
@" & sibi_request_items.New_Serial & ",
@" & sibi_request_items.Org_Code & ",
@" & sibi_request_items.Object_Code & ",
@" & sibi_request_items.Qty & ",
@" & sibi_request_items.Sequence & "
)"
    End Function
    Private Sub SetRequestItemParameters(row As DataRow, ByRef cmd As MySqlCommand, RequestData As Request_Info, IsInsert As Boolean)
        If IsInsert Then
            Dim strItemUID As String = Guid.NewGuid.ToString
            cmd.Parameters.AddWithValue("@" & sibi_request_items.Item_UID, strItemUID)
            cmd.Parameters.AddWithValue("@" & sibi_request_items.Request_UID, RequestData.strUID)
        End If
        cmd.Parameters.AddWithValue("@" & sibi_request_items.User, row.Item("User"))
        cmd.Parameters.AddWithValue("@" & sibi_request_items.Description, row.Item("Description"))
        cmd.Parameters.AddWithValue("@" & sibi_request_items.Location, row.Item(Attrib_Type.Location))
        cmd.Parameters.AddWithValue("@" & sibi_request_items.Status, row.Item(Attrib_Type.SibiItemStatusType))
        cmd.Parameters.AddWithValue("@" & sibi_request_items.Replace_Asset, row.Item("Replace Asset"))
        cmd.Parameters.AddWithValue("@" & sibi_request_items.Replace_Serial, row.Item("Replace Serial"))
        cmd.Parameters.AddWithValue("@" & sibi_request_items.New_Asset, row.Item("New Asset"))
        cmd.Parameters.AddWithValue("@" & sibi_request_items.New_Serial, row.Item("New Serial"))
        cmd.Parameters.AddWithValue("@" & sibi_request_items.Org_Code, row.Item("Org Code"))
        cmd.Parameters.AddWithValue("@" & sibi_request_items.Object_Code, row.Item("Object Code"))
        cmd.Parameters.AddWithValue("@" & sibi_request_items.Qty, row.Item("Qty"))
        cmd.Parameters.AddWithValue("@" & sibi_request_items.Sequence, RequestData.RequstItems.Rows.IndexOf(row) + 1)
    End Sub
    Private Sub InsertRequestItem(row As DataRow, ByRef cmd As MySqlCommand, RequestData As Request_Info)
        SetRequestItemParameters(row, cmd, RequestData, True)
        cmd.CommandText = RequestItemInsertQry()
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
            Dim strRequestItemsQRY As String = "SELECT * FROM " & sibi_request_items.TableName & " WHERE " & sibi_request_items.Request_UID & "='" & RequestUID & "' ORDER BY " & sibi_request_items.Sequence
            Using SQLComms As New MySQL_Comms
                Dim RequestResults As DataTable = SQLComms.Return_SQLTable(strRequestQRY)
                Dim RequestItemsResults As DataTable = SQLComms.Return_SQLTable(strRequestItemsQRY)
                ClearAll()
                CollectRequestInfo(RequestResults, RequestItemsResults)
                DataParser.FillDBFields(RequestResults)
                SendToGrid(RequestItemsResults)
                LoadNotes(CurrentRequest.strUID)
                'RequestItemsGrid.ReadOnly = True
                DisableControls(Me)
                SetTitle()
                SetAttachCount()
                Me.Show()
                Me.Activate()
                SetReqStatus(CurrentRequest.strRequisitionNumber, CurrentRequest.dtDateStamp.Year)
                CheckForPO()
                SetPOStatus(CurrentRequest.strPO)
            End Using
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
            Using SQLComms As New MySQL_Comms, table As New DataTable, Results As DataTable = SQLComms.Return_SQLTable(strPullNotesQry)
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
            RequestItemsGrid.Rows.Remove(RequestItemsGrid.Rows(RowIndex))
            Return True
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
            Else
                Return False
                EndProgram()
            End If
        End Try
        Return False
    End Function
    Private Sub SendToGrid(Results As DataTable) ' Data() As Device_Info)
        Try
            bolGridFilling = True
            SetupGrid()
            For Each r As DataRow In Results.Rows
                With RequestItemsGrid.Rows
                    .Add(NoNull(r.Item(sibi_request_items.User)),
                        NoNull(r.Item(sibi_request_items.Description)),
                         NoNull(r.Item(sibi_request_items.Qty)),
                        GetHumanValue(DeviceIndex.Locations, NoNull(r.Item(sibi_request_items.Location))),
                             GetHumanValue(SibiIndex.ItemStatusType, NoNull(r.Item(sibi_request_items.Status))),
                             NoNull(r.Item(sibi_request_items.Replace_Asset)),
                             NoNull(r.Item(sibi_request_items.Replace_Serial)),
                         NoNull(r.Item(sibi_request_items.New_Asset)),
                         NoNull(r.Item(sibi_request_items.New_Serial)),
                         NoNull(r.Item(sibi_request_items.Org_Code)),
                         NoNull(r.Item(sibi_request_items.Object_Code)),
                         NoNull(r.Item(sibi_request_items.Item_UID)))
                    RequestItemsGrid.Rows(RequestItemsGrid.Rows.Count - 1).HeaderCell.Value = r.Item(sibi_request_items.Sequence).ToString
                End With
            Next
            RequestItemsGrid.ClearSelection()
            bolGridFilling = False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
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
        If Not CheckForAccess(AccessGroup.Sibi_View) Then Exit Sub
        If Not AttachmentsIsOpen() Then
            If CurrentRequest.strUID <> "" Then
                Dim NewAttach As New AttachmentsForm(Me, CurrentRequest)
            End If
        Else
            ActivateFormByUID(CurrentRequest.strUID)
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
                If IsNewRow(RequestItemsGrid.CurrentRow.Index) Then
                    If DeleteItem_FromLocal(RequestItemsGrid.CurrentRow.Index) Then
                    Else
                        blah = Message("Failed to delete row.", vbExclamation + vbOKOnly, "Error", Me)
                    End If
                Else
                    blah = Message(DeleteItem_FromSQL(RequestItemsGrid.Item(GetColIndex(RequestItemsGrid, "Item UID"), RequestItemsGrid.CurrentRow.Index).Value.ToString, "sibi_items_uid", "sibi_request_items") & " Rows affected.", vbOKOnly + vbInformation, "Delete Item", Me)
                    OpenRequest(CurrentRequest.strUID)
                End If
            Else
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Function IsNewRow(RowIndex As Integer) As Boolean
        Try
            Dim GUID As String = RequestItemsGrid.Item(GetColIndex(RequestItemsGrid, "Item UID"), RowIndex).Value.ToString
            If GUID <> "" Then
                Return False
            End If
            Return True
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
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
            If IsNothing(CurrentRequest.RequstItems) Then Exit Sub
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
            If e.ColumnIndex >= 0 And e.RowIndex >= 0 Then
                If e.Button = MouseButtons.Right And Not RequestItemsGrid.Item(e.ColumnIndex, e.RowIndex).Selected Then
                    RequestItemsGrid.Rows(e.RowIndex).Selected = True
                    RequestItemsGrid.CurrentCell = RequestItemsGrid(e.ColumnIndex, e.RowIndex)
                End If
                If ValidColumn() Then
                    tsmLookupDevice.Visible = True
                    tsmSeparator.Visible = True
                Else
                    tsmLookupDevice.Visible = False
                    tsmSeparator.Visible = False
                End If
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Function ValidColumn() As Boolean
        Try
            If RequestItemsGrid.CurrentCell IsNot Nothing Then
                If RequestItemsGrid.CurrentCell.Value IsNot Nothing AndAlso RequestItemsGrid.CurrentCell.Value.ToString <> "" Then
                    Select Case True
                        Case RequestItemsGrid.CurrentCell.ColumnIndex = GetColIndex(RequestItemsGrid, "Replace Asset")
                            Return True
                        Case RequestItemsGrid.CurrentCell.ColumnIndex = GetColIndex(RequestItemsGrid, "Replace Serial")
                            Return True
                        Case RequestItemsGrid.CurrentCell.ColumnIndex = GetColIndex(RequestItemsGrid, "New Asset")
                            Return True
                        Case RequestItemsGrid.CurrentCell.ColumnIndex = GetColIndex(RequestItemsGrid, "New Serial")
                            Return True
                        Case Else
                            Return False
                    End Select
                End If
            End If
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
                Case ColIndex = GetColIndex(RequestItemsGrid, "Replace Asset")
                    LookupDevice(Me, AssetFunc.FindDevice(RequestItemsGrid.Item(ColIndex, RequestItemsGrid.CurrentRow.Index).Value.ToString, FindDevType.AssetTag))
                Case ColIndex = GetColIndex(RequestItemsGrid, "Replace Serial")
                    LookupDevice(Me, AssetFunc.FindDevice(RequestItemsGrid.Item(ColIndex, RequestItemsGrid.CurrentRow.Index).Value.ToString, FindDevType.Serial))
                Case ColIndex = GetColIndex(RequestItemsGrid, "New Asset")
                    LookupDevice(Me, AssetFunc.FindDevice(RequestItemsGrid.Item(ColIndex, RequestItemsGrid.CurrentRow.Index).Value.ToString, FindDevType.AssetTag))
                Case ColIndex = GetColIndex(RequestItemsGrid, "New Serial")
                    LookupDevice(Me, AssetFunc.FindDevice(RequestItemsGrid.Item(ColIndex, RequestItemsGrid.CurrentRow.Index).Value.ToString, FindDevType.Serial))
            End Select
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub cmdAccept_Click(sender As Object, e As EventArgs) Handles cmdAccept.Click
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
        e.Row.Cells("Qty").Value = 1
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
                .RequstItems = RequestItemsResults
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
        Try
            If bolUpdating Then
                Dim R As DataGridViewRow = DirectCast(e.Data.GetData(GetType(DataGridViewRow)), DataGridViewRow) 'get Row data then selectively add rows to grid.
                RequestItemsGrid.Rows.Add(R.Cells(0).Value, R.Cells(1).Value, R.Cells(2).Value, R.Cells(3).Value, R.Cells(4).Value, R.Cells(5).Value, R.Cells(6).Value, R.Cells(7).Value, R.Cells(8).Value, R.Cells(9).Value, R.Cells(10).Value)
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
End Class