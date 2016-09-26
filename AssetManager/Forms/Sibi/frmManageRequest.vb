Option Explicit On
Imports System.Collections
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class frmManageRequest
    Public bolUpdating As Boolean = False
    Private bolGridFilling As Boolean = False
    Public CurrentRequest As Request_Info
    Private MyText As String
    Private Sub frmNewRequest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ExtendedMethods.DoubleBuffered(RequestItemsGrid, True)
        Dim MyMunisTools As New MunisToolsMenu
        ToolStrip.Items.Insert(7, MyMunisTools.MunisTools)
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
        ToolStrip.BackColor = colToolBarColor
        cmdUpdate.Font = New Font(cmdUpdate.Font, FontStyle.Regular)
        cmdUpdate.Text = "Update"
        bolUpdating = False
        fieldErrorIcon.Clear()
    End Sub
    Private Sub ClearTextBoxes(ByVal control As Control)
        If TypeOf control Is TextBox Then
            Dim txt As TextBox = control
            txt.Clear()
        End If
    End Sub
    Private Sub ClearCombos(ByVal control As Control)
        If TypeOf control Is ComboBox Then
            Dim cmb As ComboBox = control
            cmb.SelectedIndex = -1
            cmb.Text = Nothing
        End If
    End Sub
    Private Sub ClearDTPicker(ByVal control As Control)
        If TypeOf control Is DateTimePicker Then
            Dim dtp As DateTimePicker = control
            dtp.Value = Now
        End If
    End Sub
    Private Sub ClearCheckBox(ByVal control As Control)
        If TypeOf control Is CheckBox Then
            Dim chk As CheckBox = control
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
                    Dim txt As TextBox = c
                    txt.ReadOnly = True
                Case TypeOf c Is ComboBox
                    Dim cmb As ComboBox = c
                    cmb.Enabled = False
                Case TypeOf c Is DateTimePicker
                    Dim dtp As DateTimePicker = c
                    dtp.Enabled = False
                Case TypeOf c Is CheckBox
                    c.Enabled = False
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
                    Dim txt As TextBox = c
                    If txt.Name <> "txtRequestNum" Then
                        txt.ReadOnly = False
                    End If
                Case TypeOf c Is ComboBox
                    Dim cmb As ComboBox = c
                    cmb.Enabled = True
                Case TypeOf c Is DateTimePicker
                    Dim dtp As DateTimePicker = c
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
    End Sub
    Private Sub SetupGrid()
        RequestItemsGrid.DataSource = Nothing
        RequestItemsGrid.Rows.Clear()
        RequestItemsGrid.Columns.Clear()
        Dim intQty As New DataGridViewColumn
        intQty.Name = "Qty"
        intQty.HeaderText = "Qty"
        intQty.ValueType = GetType(Integer)
        intQty.CellTemplate = New DataGridViewTextBoxCell
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
        SetFieldTags()
        bolFieldsValid = True
        Dim ValidateResults As Boolean = CheckFields(Me, bolFieldsValid)
        If Not ValidateResults Then
            Dim blah = Message("Some required fields are missing. Please enter data into all require fields.", vbOKOnly + vbExclamation, "Missing Data")
        End If
        Return ValidateResults
    End Function
    Private bolFieldsValid As Boolean
    Private Function CheckFields(parent As Control, FieldsValid As Boolean) As Boolean
        Dim c As Control
        For Each c In parent.Controls
            If Not IsNothing(c.Tag) And c.Tag = True Then
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
                        Dim cmb As ComboBox = c
                        If cmb.SelectedIndex = -1 Then
                            bolFieldsValid = False
                            cmb.BackColor = colMissingField
                            AddErrorIcon(cmb)
                        Else
                            cmb.BackColor = Color.Empty
                            ClearErrorIcon(cmb)
                        End If
                    Case TypeOf c Is DataGridView
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
                End Select
            End If
            If c.HasChildren Then CheckFields(c, bolFieldsValid)
        Next
        Return bolFieldsValid 'if fields are missing return false to trigger a message if needed
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
    Private Function DataGridCombo(IndexType() As Combo_Data, HeaderText As String, Name As String) As DataGridViewComboBoxColumn
        Dim tmpCombo As New DataGridViewComboBoxColumn
        tmpCombo.Items.Clear()
        tmpCombo.HeaderText = HeaderText
        tmpCombo.Name = Name
        tmpCombo.Width = 200
        Dim myList As New List(Of String)
        Dim i As Integer = 0
        For Each ComboItem As Combo_Data In IndexType
            myList.Add(ComboItem.strLong)
            'tmpCombo.Items.Insert(i, ComboItem.strLong)
            'i += 1
        Next
        tmpCombo.DataSource = myList
        ' tmpCombo.ValueMember = ""
        Return tmpCombo
    End Function
    Private Function CollectData() As Request_Info
        Try
            Dim info As Request_Info
            ' Dim dt As DataTable = RequestItemsGrid.DataSource
            RequestItemsGrid.EndEdit()
            ' Dim GridTable = TryCast(RequestItemsGrid.DataSource, DataTable)
            With info
                .strDescription = Trim(txtDescription.Text)
                .strUser = Trim(txtUser.Text)
                .strType = GetDBValue(SibiIndex.RequestType, cmbType.SelectedIndex)
                .dtNeedBy = dtNeedBy.Value.ToString(strDBDateFormat)
                .strStatus = GetDBValue(SibiIndex.StatusType, cmbStatus.SelectedIndex)
                .strPO = Trim(txtPO.Text)
                .strRequisitionNumber = Trim(txtReqNumber.Text)
                .strRTNumber = Trim(txtRTNumber.Text)
                '   .RequstItems = dt 'GridTable
            End With
            Dim DBTable As New DataTable
            For Each col As DataGridViewColumn In RequestItemsGrid.Columns
                DBTable.Columns.Add(col.Name)
            Next
            For Each row As DataGridViewRow In RequestItemsGrid.Rows
                If Not row.IsNewRow Then
                    Dim NewRow As DataRow = DBTable.NewRow()
                    For Each dcell As DataGridViewCell In row.Cells
                        If dcell.OwningColumn.CellType.Name = "DataGridViewComboBoxCell" Then


                            Select Case dcell.OwningColumn.Name
                                Case Attrib_Type.Location
                                    NewRow(dcell.ColumnIndex) = GetDBValueFromHuman(DeviceIndex.Locations, dcell.Value)
                                Case Attrib_Type.SibiItemStatusType
                                    NewRow(dcell.ColumnIndex) = GetDBValueFromHuman(SibiIndex.ItemStatusType, dcell.Value)

                            End Select

                            '**************************NewRow(dcell.ColumnIndex) = GetDBValueFromHuman(dcell.OwningColumn.Name, dcell.Value)
                        Else
                            NewRow(dcell.ColumnIndex) = Trim(dcell.Value)
                        End If
                        ' Debug.Print(dcell.OwningColumn.Name & " - " & dcell.OwningColumn.CellType.ToString & " - " & dcell.FormattedValue)
                    Next
                    DBTable.Rows.Add(NewRow)
                End If
            Next
            info.RequstItems = DBTable
            Return info
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            EndProgram()
        End Try
    End Function
    Private Sub cmdAddNew_Click(sender As Object, e As EventArgs) Handles cmdAddNew.Click
        If Not CheckForAccess(AccessGroup.Sibi_Add) Then Exit Sub
        pnlCreate.Visible = False
        AddNewRequest()
    End Sub
    Private Sub AddNewRequest()
        If Not ValidateFields() Then Exit Sub
        Dim RequestData As Request_Info = CollectData()
        Dim strRequestUID As String = Guid.NewGuid.ToString
        Try
            Dim rows As Integer
            'If Not CheckFields() Then
            '    Dim blah = Message("Some required fields are missing.  Please fill in all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data")
            '    bolCheckFields = True
            '    Exit Sub
            'End If
            Dim strSqlQry1 = "INSERT INTO `asset_manager`.`sibi_requests`
(`sibi_uid`,
`sibi_request_user`,
`sibi_description`,
`sibi_need_by`,
`sibi_status`,
`sibi_type`,
`sibi_PO`,
`sibi_requisition_number`,
`sibi_replace_asset`,
`sibi_replace_serial`,
`sibi_RT_number`)
VALUES
(@sibi_uid,
@sibi_request_user,
@sibi_description,
@sibi_need_by,
@sibi_status,
@sibi_type,
@sibi_PO,
@sibi_requisition_number,
@sibi_replace_asset,
@sibi_replace_serial,
@sibi_RT_number)"
            Dim cmd As MySqlCommand = MySQLDB.Return_SQLCommand(strSqlQry1)
            cmd.Parameters.AddWithValue("@sibi_uid", strRequestUID)
            cmd.Parameters.AddWithValue("@sibi_request_user", RequestData.strUser)
            cmd.Parameters.AddWithValue("@sibi_description", RequestData.strDescription)
            cmd.Parameters.AddWithValue("@sibi_need_by", RequestData.dtNeedBy)
            cmd.Parameters.AddWithValue("@sibi_status", RequestData.strStatus)
            cmd.Parameters.AddWithValue("@sibi_type", RequestData.strType)
            cmd.Parameters.AddWithValue("@sibi_PO", RequestData.strPO)
            cmd.Parameters.AddWithValue("@sibi_requisition_number", RequestData.strRequisitionNumber)
            cmd.Parameters.AddWithValue("@sibi_replace_asset", RequestData.strReplaceAsset)
            cmd.Parameters.AddWithValue("@sibi_replace_serial", RequestData.strReplaceSerial)
            cmd.Parameters.AddWithValue("@sibi_RT_number", RequestData.strRTNumber)
            rows = rows + cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            For Each row As DataRow In RequestData.RequstItems.Rows
                Dim strItemUID As String = Guid.NewGuid.ToString
                Dim strSqlQry2 = "INSERT INTO `asset_manager`.`sibi_request_items`
(`sibi_items_uid`,
`sibi_items_request_uid`,
`sibi_items_user`,
`sibi_items_description`,
`sibi_items_location`,
`sibi_items_status`,
`sibi_items_replace_asset`,
`sibi_items_replace_serial`,
`sibi_items_new_asset`,
`sibi_items_new_serial`,
`sibi_items_org_code`,
`sibi_items_object_code`,
`sibi_items_qty`
)
VALUES
(@sibi_items_uid,
@sibi_items_request_uid,
@sibi_items_user,
@sibi_items_description,
@sibi_items_location,
@sibi_items_status,
@sibi_items_replace_asset,
@sibi_items_replace_serial,
@sibi_items_new_asset,
@sibi_items_new_serial,
@sibi_items_org_code,
@sibi_items_object_code,
@sibi_items_qty
)"
                cmd.Parameters.AddWithValue("@sibi_items_uid", strItemUID)
                cmd.Parameters.AddWithValue("@sibi_items_request_uid", strRequestUID)
                cmd.Parameters.AddWithValue("@sibi_items_user", row.Item("User"))
                cmd.Parameters.AddWithValue("@sibi_items_description", row.Item("Description"))
                cmd.Parameters.AddWithValue("@sibi_items_location", row.Item(Attrib_Type.Location))
                cmd.Parameters.AddWithValue("@sibi_items_status", row.Item(Attrib_Type.SibiItemStatusType))
                cmd.Parameters.AddWithValue("@sibi_items_replace_asset", row.Item("Replace Asset"))
                cmd.Parameters.AddWithValue("@sibi_items_replace_serial", row.Item("Replace Serial"))
                cmd.Parameters.AddWithValue("@sibi_items_new_asset", row.Item("Replace Asset"))
                cmd.Parameters.AddWithValue("@sibi_items_new_serial", row.Item("Replace Serial"))
                cmd.Parameters.AddWithValue("@sibi_items_org_code", row.Item("Org Code"))
                cmd.Parameters.AddWithValue("@sibi_items_object_code", row.Item("Object Code"))
                cmd.Parameters.AddWithValue("@sibi_items_qty", row.Item("Qty"))
                cmd.CommandText = strSqlQry2
                rows = rows + cmd.ExecuteNonQuery()
                cmd.Parameters.Clear()
            Next
            cmd.Dispose()
            Dim blah = Message("New Request Added.", vbOKOnly + vbInformation, "Complete")
            frmSibiMain.ShowAll()
            OpenRequest(strRequestUID)
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Exit Sub
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Private Sub UpdateRequest()
        Try
            Dim RequestData As Request_Info = CollectData()
            If RequestData.RequstItems Is Nothing Then Exit Sub
            Dim rows As Integer
            Dim strRequestQRY As String = "UPDATE sibi_requests
SET
sibi_request_user = @sibi_request_user ,
sibi_description = @sibi_description ,
sibi_need_by = @sibi_need_by ,
sibi_status = @sibi_status ,
sibi_type = @sibi_type ,
sibi_PO = @sibi_PO ,
sibi_requisition_number = @sibi_requisition_number ,
sibi_replace_asset = @sibi_replace_asset ,
sibi_replace_serial = @sibi_replace_serial ,
sibi_RT_number = @sibi_RT_number 
WHERE sibi_uid ='" & CurrentRequest.strUID & "'"
            Dim cmd As MySqlCommand = MySQLDB.Return_SQLCommand(strRequestQRY)
            cmd.Parameters.AddWithValue("@sibi_request_user", RequestData.strUser)
            cmd.Parameters.AddWithValue("@sibi_description", RequestData.strDescription)
            cmd.Parameters.AddWithValue("@sibi_need_by", RequestData.dtNeedBy)
            cmd.Parameters.AddWithValue("@sibi_status", RequestData.strStatus)
            cmd.Parameters.AddWithValue("@sibi_type", RequestData.strType)
            cmd.Parameters.AddWithValue("@sibi_PO", RequestData.strPO)
            cmd.Parameters.AddWithValue("@sibi_requisition_number", RequestData.strRequisitionNumber)
            cmd.Parameters.AddWithValue("@sibi_replace_asset", RequestData.strReplaceAsset)
            cmd.Parameters.AddWithValue("@sibi_replace_serial", RequestData.strReplaceSerial)
            cmd.Parameters.AddWithValue("@sibi_RT_number", RequestData.strRTNumber)
            rows += cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            Dim strRequestItemsQry As String
            For Each row As DataRow In RequestData.RequstItems.Rows
                If row.Item("Item UID") <> "" Then
                    strRequestItemsQry = "UPDATE sibi_request_items
SET
sibi_items_user = @sibi_items_user ,
sibi_items_description = @sibi_items_description ,
sibi_items_location = @sibi_items_location ,
sibi_items_status = @sibi_items_status ,
sibi_items_replace_asset = @sibi_items_replace_asset ,
sibi_items_replace_serial = @sibi_items_replace_serial,
sibi_items_new_asset = @sibi_items_new_asset ,
sibi_items_new_serial = @sibi_items_new_serial,
sibi_items_org_code = @sibi_items_org_code,
sibi_items_object_code = @sibi_items_object_code,
sibi_items_qty = @sibi_items_qty
WHERE sibi_items_uid ='" & row.Item("Item UID") & "'"
                    cmd.Parameters.AddWithValue("@sibi_items_user", row.Item("User"))
                    cmd.Parameters.AddWithValue("@sibi_items_description", row.Item("Description"))
                    cmd.Parameters.AddWithValue("@sibi_items_location", row.Item(Attrib_Type.Location))
                    cmd.Parameters.AddWithValue("@sibi_items_status", row.Item(Attrib_Type.SibiItemStatusType))
                    cmd.Parameters.AddWithValue("@sibi_items_replace_asset", row.Item("Replace Asset"))
                    cmd.Parameters.AddWithValue("@sibi_items_replace_serial", row.Item("Replace Serial"))
                    cmd.Parameters.AddWithValue("@sibi_items_new_asset", row.Item("New Asset"))
                    cmd.Parameters.AddWithValue("@sibi_items_new_serial", row.Item("New Serial"))
                    cmd.Parameters.AddWithValue("@sibi_items_org_code", row.Item("Org Code"))
                    cmd.Parameters.AddWithValue("@sibi_items_object_code", row.Item("Object Code"))
                    cmd.Parameters.AddWithValue("@sibi_items_qty", row.Item("Qty"))
                    cmd.CommandText = strRequestItemsQry
                    cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                Else
                    Dim strItemUID As String = Guid.NewGuid.ToString
                    strRequestItemsQry = "INSERT INTO `asset_manager`.`sibi_request_items`
(`sibi_items_uid`,
`sibi_items_request_uid`,
`sibi_items_user`,
`sibi_items_description`,
`sibi_items_location`,
`sibi_items_status`,
`sibi_items_replace_asset`,
`sibi_items_replace_serial`,
`sibi_items_new_asset`,
`sibi_items_new_serial`,
`sibi_items_org_code`,
`sibi_items_object_code`,
`sibi_items_qty`
)
VALUES
(@sibi_items_uid,
@sibi_items_request_uid,
@sibi_items_user,
@sibi_items_description,
@sibi_items_location,
@sibi_items_status,
@sibi_items_replace_asset,
@sibi_items_replace_serial,
@sibi_items_new_asset,
@sibi_items_new_serial,
@sibi_items_org_code,
@sibi_items_object_code,
@sibi_items_qty
)"
                    cmd.Parameters.AddWithValue("@sibi_items_uid", strItemUID)
                    cmd.Parameters.AddWithValue("@sibi_items_request_uid", CurrentRequest.strUID)
                    cmd.Parameters.AddWithValue("@sibi_items_user", row.Item("User"))
                    cmd.Parameters.AddWithValue("@sibi_items_description", row.Item("Description"))
                    cmd.Parameters.AddWithValue("@sibi_items_location", row.Item(Attrib_Type.Location))
                    cmd.Parameters.AddWithValue("@sibi_items_status", row.Item(Attrib_Type.SibiItemStatusType))
                    cmd.Parameters.AddWithValue("@sibi_items_replace_asset", row.Item("Replace Asset"))
                    cmd.Parameters.AddWithValue("@sibi_items_replace_serial", row.Item("Replace Serial"))
                    cmd.Parameters.AddWithValue("@sibi_items_new_asset", row.Item("New Asset"))
                    cmd.Parameters.AddWithValue("@sibi_items_new_serial", row.Item("New Serial"))
                    cmd.Parameters.AddWithValue("@sibi_items_org_code", row.Item("Org Code"))
                    cmd.Parameters.AddWithValue("@sibi_items_object_code", row.Item("Object Code"))
                    cmd.Parameters.AddWithValue("@sibi_items_qty", row.Item("Qty"))
                    cmd.CommandText = strRequestItemsQry
                    rows += cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                End If
            Next
            cmd.Dispose()
            'If rows = RequestData.RequstItems.Rows.Count + 1 Then
            frmSibiMain.ShowAll()
            Message("Success!")
            'End If
            OpenRequest(CurrentRequest.strUID)
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Public Sub OpenRequest(RequestUID As String)
        Try
            Dim strRequestQRY As String = "SELECT * FROM sibi_requests WHERE sibi_uid='" & RequestUID & "'"
            Dim strRequestItemsQRY As String = "SELECT * FROM sibi_request_items WHERE sibi_items_request_uid='" & RequestUID & "'"
            Dim RequestResults As DataTable = MySQLDB.Return_SQLTable(strRequestQRY)
            Dim RequestItemsResults As DataTable = MySQLDB.Return_SQLTable(strRequestItemsQRY)
            ClearAll()
            CollectRequestInfo(RequestResults, RequestItemsResults)
            With RequestResults.Rows(0)
                txtDescription.Text = NoNull(.Item("sibi_description"))
                txtUser.Text = NoNull(.Item("sibi_request_user"))
                cmbType.SelectedIndex = GetComboIndexFromShort(SibiIndex.RequestType, NoNull(.Item("sibi_type")))
                dtNeedBy.Value = NoNull(.Item("sibi_need_by"))
                cmbStatus.SelectedIndex = GetComboIndexFromShort(SibiIndex.StatusType, NoNull(.Item("sibi_status")))
                txtPO.Text = NoNull(.Item("sibi_PO"))
                txtReqNumber.Text = NoNull(.Item("sibi_requisition_number"))
                txtRequestNum.Text = NoNull(.Item("sibi_request_number"))
                txtRTNumber.Text = NoNull(.Item("sibi_RT_number"))
            End With
            SendToGrid(RequestItemsResults)
            LoadNotes(CurrentRequest.strUID)
            'RequestItemsGrid.ReadOnly = True
            DisableControls(Me)
            SetTitle()
            Me.Show()
            Me.Activate()
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Private Sub LoadNotes(RequestUID As String)
        Dim strPullNotesQry As String = "SELECT * FROM sibi_notes WHERE sibi_request_uid='" & RequestUID & "' ORDER BY sibi_datestamp DESC"
        Dim Results As DataTable = MySQLDB.Return_SQLTable(strPullNotesQry)
        Dim table As New DataTable
        Dim intPreviewChars As Integer = 50
        table.Columns.Add("Date Stamp")
        table.Columns.Add("Preview")
        table.Columns.Add("UID")
        For Each r As DataRow In Results.Rows
            table.Rows.Add(r.Item("sibi_datestamp"),
                           IIf(Len(r.Item("sibi_note")) > intPreviewChars, NotePreview(r.Item("sibi_note")), r.Item("sibi_note")),
                           r.Item("sibi_note_uid"))
        Next
        dgvNotes.DataSource = table
        dgvNotes.ClearSelection()
        table.Dispose()
        Results.Dispose()
    End Sub
    Private Function DeleteItem(ItemUID As String, ItemColumnName As String, Table As String) As Integer
        Try
            Dim rows
            Dim strSQLQry As String = "DELETE FROM " & Table & " WHERE " & ItemColumnName & "='" & ItemUID & "'"
            rows = MySQLDB.Return_SQLCommand(strSQLQry).ExecuteNonQuery
            Return rows
            Exit Function
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                EndProgram()
            End If
        End Try
    End Function
    Private Sub SendToGrid(Results As DataTable) ' Data() As Device_Info)
        Try
            bolGridFilling = True
            SetupGrid()
            For Each r As DataRow In Results.Rows
                With RequestItemsGrid.Rows
                    .Add(NoNull(r.Item("sibi_items_user")),
                        NoNull(r.Item("sibi_items_description")),
                         NoNull(r.Item("sibi_items_qty")),
                        GetHumanValue(DeviceIndex.Locations, NoNull(r.Item("sibi_items_location"))),
                             GetHumanValue(SibiIndex.ItemStatusType, NoNull(r.Item("sibi_items_status"))),
                             NoNull(r.Item("sibi_items_replace_asset")),
                             NoNull(r.Item("sibi_items_replace_serial")),
                         NoNull(r.Item("sibi_items_new_asset")),
                         NoNull(r.Item("sibi_items_new_serial")),
                         NoNull(r.Item("sibi_items_org_code")),
                         NoNull(r.Item("sibi_items_object_code")),
                         NoNull(r.Item("sibi_items_uid")))
                End With
            Next
            RequestItemsGrid.ClearSelection()
            bolGridFilling = False
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
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
            cmdUpdate.Font = New Font(cmdUpdate.Font, FontStyle.Bold)
            cmdUpdate.Text = "*Accept Changes*"
            ShowEditControls()
            ' cmdUpdate.Image = My.Resources.checked_checkbox
            bolUpdating = True
        Else
            DisableControls(Me)
            ToolStrip.BackColor = colToolBarColor
            cmdUpdate.Font = New Font(cmdUpdate.Font, FontStyle.Regular)
            cmdUpdate.Text = "Update"
            HideEditControls()
            ' cmdUpdate.Image = My.Resources.Edit
            UpdateRequest()
            bolUpdating = False
        End If
    End Sub
    Private Sub cmdAttachments_Click(sender As Object, e As EventArgs) Handles cmdAttachments.Click
        If Not CheckForAccess(AccessGroup.Sibi_View) Then Exit Sub
        If Not AttachmentsIsOpen(CurrentRequest.strUID) Then
            If CurrentRequest.strUID <> "" Then
                Dim NewAttach As New frmSibiAttachments
                NewAttach.Tag = Me
                NewAttach.ListAttachments(CurrentRequest)
                NewAttach.Activate()
                NewAttach.Show()
            End If
        Else
            ActivateForm(CurrentRequest.strUID)
        End If
    End Sub
    Public Function AttachmentsIsOpen(strGUID As String) As Boolean
        For Each frm As Form In My.Application.OpenForms
            If frm.Name = "frmSibiAttachments" Then
                Dim attach As frmSibiAttachments = frm
                If attach.AttachRequest.strUID = strGUID Then Return True
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
        CloseChildren()
        Me.Dispose()
    End Sub
    Private Sub CloseChildren()
        Dim children(0) As frmSibiAttachments
        For Each frms As Form In My.Application.OpenForms
            If TypeOf frms Is frmSibiAttachments Then
                Dim AttachForm As frmSibiAttachments = frms
                If AttachForm.AttachRequest.strUID = CurrentRequest.strUID Then
                    children(UBound(children)) = frms
                    ReDim Preserve children(UBound(children) + 1)
                End If
            End If
        Next
        For Each child As frmSibiAttachments In children
            If Not IsNothing(child) Then child.Dispose()
        Next
    End Sub
    Private Sub tsmDeleteItem_Click(sender As Object, e As EventArgs) Handles tsmDeleteItem.Click
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        Dim blah
        blah = Message("Delete selected item?", vbYesNo + vbQuestion, "Delete Item Row")
        If blah = vbYes Then
            blah = Message(DeleteItem(RequestItemsGrid.Item(GetColIndex(RequestItemsGrid, "Item UID"), RequestItemsGrid.CurrentRow.Index).Value, "sibi_items_uid", "sibi_request_items") & " Rows affected.", vbOKOnly + vbInformation, "Delete Item")
            OpenRequest(CurrentRequest.strUID)
        Else
        End If
    End Sub
    Private Sub txtRTNumber_Click(sender As Object, e As EventArgs) Handles txtRTNumber.Click
        Dim RTNum As String = Trim(txtRTNumber.Text)
        If Not bolUpdating And RTNum <> "" Then
            Process.Start("http://rt.co.fairfield.oh.us/rt/Ticket/Display.html?id=" & RTNum)
        End If
    End Sub
    Private Sub NewMunisViewReq(ReqNum As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        Dim NewMunis As New View_Munis
        NewMunis.HideFixedAssetGrid()
        NewMunis.LoadMunisRequisitionGridByReqNo(ReqNum, YearFromDate(CurrentRequest.dtDateStamp))
        NewMunis.Show()
    End Sub
    Private Sub NewMunisViewPO(PO As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        Dim NewMunis As New View_Munis
        NewMunis.HideFixedAssetGrid()
        NewMunis.LoadMunisRequisitionGridByReqNo(Munis.Get_ReqNumber_From_PO(PO), Munis.Get_FY_From_PO(PO)) 'YearFromDate(CurrentRequest.dtDateStamp))
        NewMunis.Show()
    End Sub
    Private Sub txtReqNumber_Click(sender As Object, e As EventArgs) Handles txtReqNumber.Click
        Dim ReqNum As String = Trim(txtReqNumber.Text)
        If Not bolUpdating And ReqNum <> "" Then
            NewMunisViewReq(ReqNum)
        End If
    End Sub
    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        If Not CheckForAccess(AccessGroup.Sibi_Delete) Then Exit Sub
        If IsNothing(CurrentRequest.RequstItems) Then Exit Sub
        Dim blah = Message("Are you absolutely sure?  This cannot be undone and will delete all data including attachments.", vbYesNo + vbExclamation, "WARNING")
        If blah = vbYes Then
            If DeleteMaster(CurrentRequest.strUID, Entry_Type.Sibi) Then
                Dim blah2 = Message("Sibi Request deleted successfully.", vbOKOnly + vbInformation, "Device Deleted")
                CurrentRequest = Nothing
                frmSibiMain.ShowAll()
                Me.Dispose()
            Else
                Logger("*****DELETION ERROR******: " & CurrentRequest.strUID)
                Dim blah2 = Message("Failed to delete request succesfully!  Please let Bobby Lovell know about this.", vbOKOnly + vbCritical, "Delete Failed")
                CurrentRequest = Nothing
                Me.Dispose()
            End If
        Else
            Exit Sub
        End If
    End Sub
    Private Sub cmdAddNote_Click(sender As Object, e As EventArgs) Handles cmdAddNote.Click
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        If CurrentRequest.strUID <> "" Then
            Dim NewNote As New frmNotes
            NewNote.LoadNote(CurrentRequest, Me)
        End If
    End Sub
    Private Sub dgvNotes_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvNotes.CellDoubleClick
        frmNotes.ViewNote(dgvNotes.Item(GetColIndex(dgvNotes, "UID"), dgvNotes.CurrentRow.Index).Value)
    End Sub
    Private Sub cmdDeleteNote_Click(sender As Object, e As EventArgs) Handles cmdDeleteNote.Click
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        Dim blah = Message(DeleteItem(dgvNotes.Item(GetColIndex(dgvNotes, "UID"), dgvNotes.CurrentRow.Index).Value, "sibi_note_uid", "sibi_notes") & " Rows affected.", vbOKOnly + vbInformation, "Delete Item")
        OpenRequest(CurrentRequest.strUID)
    End Sub
    Private Sub cmdClearForm_Click(sender As Object, e As EventArgs) Handles cmdClearForm.Click
        ClearAll()
        Me.Text = MyText
        CurrentRequest = Nothing
    End Sub
    Private Sub RequestItemsGrid_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles RequestItemsGrid.CellMouseDown
        On Error Resume Next
        If e.Button = MouseButtons.Right And Not RequestItemsGrid.Item(e.ColumnIndex, e.RowIndex).Selected Then
            RequestItemsGrid.Rows(e.RowIndex).Selected = True
            RequestItemsGrid.CurrentCell = RequestItemsGrid(e.ColumnIndex, e.RowIndex)
        End If
        If RequestItemsGrid.Item(GetColIndex(RequestItemsGrid, "Replace Asset"), RequestItemsGrid.CurrentRow.Index).Value IsNot "" Or RequestItemsGrid.Item(GetColIndex(RequestItemsGrid, "Replace Serial"), RequestItemsGrid.CurrentRow.Index).Value IsNot "" Then
            tsmLookupDevice.Visible = True
        Else
            tsmLookupDevice.Visible = False
        End If
    End Sub
    Private Sub HighlightCurrentRow(Row As Integer)
        On Error Resume Next
        If Not bolGridFilling Then
            Dim BackColor As Color = DefGridBC
            Dim SelectColor As Color = DefGridSelCol
            Dim c1 As Color = colHighlightColor 'highlight color
            If Row > -1 Then
                For Each cell As DataGridViewCell In RequestItemsGrid.Rows(Row).Cells
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
    Private Sub RequestItemsGrid_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles RequestItemsGrid.CellEnter
        HighlightCurrentRow(e.RowIndex)
    End Sub
    Private Sub RequestItemsGrid_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles RequestItemsGrid.CellLeave
        Dim BackColor As Color = DefGridBC
        Dim SelectColor As Color = DefGridSelCol
        If e.RowIndex > -1 Then
            For Each cell As DataGridViewCell In RequestItemsGrid.Rows(e.RowIndex).Cells
                cell.Style.SelectionBackColor = SelectColor
                cell.Style.BackColor = BackColor
            Next
        End If
    End Sub
    Private Sub LookupDevice(Device As Device_Info)
        View.ViewDevice(Device.strGUID)
    End Sub
    Private Sub tsmLookupDevice_Click(sender As Object, e As EventArgs) Handles tsmLookupDevice.Click
        If RequestItemsGrid.Item(GetColIndex(RequestItemsGrid, "Replace Asset"), RequestItemsGrid.CurrentRow.Index).Value IsNot "" Then
            LookupDevice(MySQLDB.FindDevice(RequestItemsGrid.Item(GetColIndex(RequestItemsGrid, "Replace Asset"), RequestItemsGrid.CurrentRow.Index).Value))
        ElseIf RequestItemsGrid.Item(GetColIndex(RequestItemsGrid, "Replace Serial"), RequestItemsGrid.CurrentRow.Index).Value IsNot "" Then
            LookupDevice(MySQLDB.FindDevice(RequestItemsGrid.Item(GetColIndex(RequestItemsGrid, "Replace Serial"), RequestItemsGrid.CurrentRow.Index).Value))
        End If
    End Sub
    Private Sub cmdAccept_Click(sender As Object, e As EventArgs) Handles cmdAccept.Click
        If Not ValidateFields() Then Exit Sub
        DisableControls(Me)
        ToolStrip.BackColor = colToolBarColor
        cmdUpdate.Font = New Font(cmdUpdate.Font, FontStyle.Regular)
        cmdUpdate.Text = "Update"
        HideEditControls()
        ' cmdUpdate.Image = My.Resources.Edit
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
            NewMunisViewPO(PO)
        End If
    End Sub
    Private Sub RequestItemsGrid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles RequestItemsGrid.DataError
        Dim blah = Message("DataGrid Error: " & Chr(34) & e.Exception.Message & Chr(34) & "   Col/Row:" & e.ColumnIndex & "/" & e.RowIndex, vbOKOnly + vbExclamation, "DataGrid Error")
        ' ErrHandleNew(e.Exception, System.Reflection.MethodInfo.GetCurrentMethod().Name)
    End Sub
    Private Sub RequestItemsGrid_DefaultValuesNeeded(sender As Object, e As DataGridViewRowEventArgs) Handles RequestItemsGrid.DefaultValuesNeeded
        e.Row.Cells("Qty").Value = 1
    End Sub
    Private Sub CollectRequestInfo(RequestResults As DataTable, RequestItemsResults As DataTable)
        Try
            With CurrentRequest
                .strUID = NoNull(RequestResults.Rows(0).Item("sibi_UID"))
                .strUser = NoNull(RequestResults.Rows(0).Item("sibi_request_user"))
                .strDescription = NoNull(RequestResults.Rows(0).Item("sibi_description"))
                .dtDateStamp = NoNull(RequestResults.Rows(0).Item("sibi_datestamp"))
                .dtNeedBy = NoNull(RequestResults.Rows(0).Item("sibi_need_by"))
                .strStatus = NoNull(RequestResults.Rows(0).Item("sibi_status"))
                .strType = NoNull(RequestResults.Rows(0).Item("sibi_type"))
                .strPO = NoNull(RequestResults.Rows(0).Item("sibi_PO")) '
                .strRequisitionNumber = NoNull(RequestResults.Rows(0).Item("sibi_requisition_number"))
                .strReplaceAsset = NoNull(RequestResults.Rows(0).Item("sibi_replace_asset"))
                .strReplaceSerial = NoNull(RequestResults.Rows(0).Item("sibi_replace_serial"))
                .strRequestNumber = NoNull(RequestResults.Rows(0).Item("sibi_request_number"))
                .RequstItems = RequestItemsResults
            End With
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub

End Class
