Option Explicit On
Imports System.Collections
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class frmManageRequest
    Public bolUpdating As Boolean = False
    Private bolGridFilling As Boolean = False
    Public CurrentRequest As Request_Info
    Private MyText As String
    Private bolNewRequest As Boolean = False
    Sub New(ParentForm As Form, RequestUID As String)
        InitializeComponent()
        Tag = ParentForm
        OpenRequest(RequestUID)
    End Sub
    Sub New(ParentForm As Form)
        InitializeComponent()
        Tag = ParentForm
        Text += " - *New Request*"
        NewRequest()
        Show()
        Activate()
    End Sub
    Private Sub frmNewRequest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ExtendedMethods.DoubleBuffered(RequestItemsGrid, True)
        Dim MyMunisTools As New MunisToolsMenu
        MyMunisTools.Tag = Me
        ToolStrip.Items.Insert(7, MyMunisTools.MunisTools)
    End Sub
    Public Sub SetAttachCount()
        cmdAttachments.Text = "Attachments (" + Asset.GetAttachmentCount(CurrentRequest).ToString + ")"
    End Sub
    Private Sub SetTitle()
        If MyText = "" Then
            MyText = Me.Text
        End If
        Me.Text = MyText + " - " + CurrentRequest.strDescription
    End Sub
    Public Sub ClearAll()
        bolNewRequest = False
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
        For Each ComboItem As Combo_Data In IndexType
            myList.Add(ComboItem.strLong)
        Next
        tmpCombo.DataSource = myList
        Return tmpCombo
    End Function
    Private Function CollectData() As Request_Info
        Try
            Dim info As Request_Info
            RequestItemsGrid.EndEdit()
            With info
                .strDescription = Trim(txtDescription.Text)
                .strUser = Trim(txtUser.Text)
                .strType = GetDBValue(SibiIndex.RequestType, cmbType.SelectedIndex)
                .dtNeedBy = dtNeedBy.Value.ToString(strDBDateFormat)
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
                        If dcell.OwningColumn.CellType.Name = "DataGridViewComboBoxCell" Then
                            Select Case dcell.OwningColumn.Name
                                Case Attrib_Type.Location
                                    NewRow(dcell.ColumnIndex) = GetDBValueFromHuman(DeviceIndex.Locations, dcell.Value)
                                Case Attrib_Type.SibiItemStatusType
                                    NewRow(dcell.ColumnIndex) = GetDBValueFromHuman(SibiIndex.ItemStatusType, dcell.Value)
                            End Select
                        Else
                            NewRow(dcell.ColumnIndex) = Trim(dcell.Value)
                        End If
                    Next
                    DBTable.Rows.Add(NewRow)
                End If
            Next
            info.RequstItems = DBTable
            Return info
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
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
        Dim strRequestUID As String = Guid.NewGuid.ToString
        Try
            Dim rows As Integer
            Dim strSqlQry1 = "INSERT INTO " & sibi_requests.TableName & "
(" & sibi_requests.UID & ",
" & sibi_requests.RequestUser & ",
" & sibi_requests.Description & ",
" & sibi_requests.NeedBy & ",
" & sibi_requests.Status & ",
" & sibi_requests.Type & ",
" & sibi_requests.PO & ",
" & sibi_requests.RequisitionNumber & ",
" & sibi_requests.Replace_Asset & ",
" & sibi_requests.Replace_Serial & ",
" & sibi_requests.RT_Number & ")
VALUES
(@" & sibi_requests.UID & ",
@" & sibi_requests.RequestUser & ",
@" & sibi_requests.Description & ",
@" & sibi_requests.NeedBy & ",
@" & sibi_requests.Status & ",
@" & sibi_requests.Type & ",
@" & sibi_requests.PO & ",
@" & sibi_requests.RequisitionNumber & ",
@" & sibi_requests.Replace_Asset & ",
@" & sibi_requests.Replace_Serial & ",
@" & sibi_requests.RT_Number & ")"
            Dim cmd As MySqlCommand = SQLComms.Return_SQLCommand(strSqlQry1)
            cmd.Parameters.AddWithValue("@" & sibi_requests.UID, strRequestUID)
            cmd.Parameters.AddWithValue("@" & sibi_requests.RequestUser, RequestData.strUser)
            cmd.Parameters.AddWithValue("@" & sibi_requests.Description, RequestData.strDescription)
            cmd.Parameters.AddWithValue("@" & sibi_requests.NeedBy, RequestData.dtNeedBy)
            cmd.Parameters.AddWithValue("@" & sibi_requests.Status, RequestData.strStatus)
            cmd.Parameters.AddWithValue("@" & sibi_requests.Type, RequestData.strType)
            cmd.Parameters.AddWithValue("@" & sibi_requests.PO, RequestData.strPO)
            cmd.Parameters.AddWithValue("@" & sibi_requests.RequisitionNumber, RequestData.strRequisitionNumber)
            cmd.Parameters.AddWithValue("@" & sibi_requests.Replace_Asset, RequestData.strReplaceAsset)
            cmd.Parameters.AddWithValue("@" & sibi_requests.Replace_Serial, RequestData.strReplaceSerial)
            cmd.Parameters.AddWithValue("@" & sibi_requests.RT_Number, RequestData.strRTNumber)
            rows = rows + cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            For Each row As DataRow In RequestData.RequstItems.Rows
                Dim strItemUID As String = Guid.NewGuid.ToString
                Dim strSqlQry2 = "INSERT INTO " & sibi_request_items.TableName & "
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
" & sibi_request_items.Qty & "
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
@" & sibi_request_items.Qty & "
)"
                cmd.Parameters.AddWithValue("@" & sibi_request_items.Item_UID, strItemUID)
                cmd.Parameters.AddWithValue("@" & sibi_request_items.Request_UID, strRequestUID)
                cmd.Parameters.AddWithValue("@" & sibi_request_items.User, row.Item("User"))
                cmd.Parameters.AddWithValue("@" & sibi_request_items.Description, row.Item("Description"))
                cmd.Parameters.AddWithValue("@" & sibi_request_items.Location, row.Item(Attrib_Type.Location))
                cmd.Parameters.AddWithValue("@" & sibi_request_items.Status, row.Item(Attrib_Type.SibiItemStatusType))
                cmd.Parameters.AddWithValue("@" & sibi_request_items.Replace_Asset, row.Item("Replace Asset"))
                cmd.Parameters.AddWithValue("@" & sibi_request_items.Replace_Serial, row.Item("Replace Serial"))
                cmd.Parameters.AddWithValue("@" & sibi_request_items.New_Asset, row.Item("Replace Asset"))
                cmd.Parameters.AddWithValue("@" & sibi_request_items.New_Serial, row.Item("Replace Serial"))
                cmd.Parameters.AddWithValue("@" & sibi_request_items.Org_Code, row.Item("Org Code"))
                cmd.Parameters.AddWithValue("@" & sibi_request_items.Object_Code, row.Item("Object Code"))
                cmd.Parameters.AddWithValue("@" & sibi_request_items.Qty, row.Item("Qty"))
                cmd.CommandText = strSqlQry2
                rows = rows + cmd.ExecuteNonQuery()
                cmd.Parameters.Clear()
            Next
            cmd.Dispose()
            bolNewRequest = False
            pnlCreate.Visible = False
            Dim blah = Message("New Request Added.", vbOKOnly + vbInformation, "Complete")
            frmSibiMain.ShowAll()
            OpenRequest(strRequestUID)
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                bolNewRequest = False
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
            Dim strRequestQRY As String = "UPDATE " & sibi_requests.TableName & "
SET
" & sibi_requests.RequestUser & " = @" & sibi_requests.RequestUser & " ,
" & sibi_requests.Description & " = @" & sibi_requests.Description & " ,
" & sibi_requests.NeedBy & " = @" & sibi_requests.NeedBy & " ,
" & sibi_requests.Status & " = @" & sibi_requests.Status & " ,
" & sibi_requests.Type & " = @" & sibi_requests.Type & " ,
" & sibi_requests.PO & " = @" & sibi_requests.PO & " ,
" & sibi_requests.RequisitionNumber & " = @" & sibi_requests.RequisitionNumber & " ,
" & sibi_requests.Replace_Asset & " = @" & sibi_requests.Replace_Asset & " ,
" & sibi_requests.Replace_Serial & " = @" & sibi_requests.Replace_Serial & " ,
" & sibi_requests.RT_Number & " = @" & sibi_requests.RT_Number & " 
WHERE " & sibi_requests.UID & " ='" & CurrentRequest.strUID & "'"
            Dim cmd As MySqlCommand = SQLComms.Return_SQLCommand(strRequestQRY)
            cmd.Parameters.AddWithValue("@" & sibi_requests.RequestUser, RequestData.strUser)
            cmd.Parameters.AddWithValue("@" & sibi_requests.Description, RequestData.strDescription)
            cmd.Parameters.AddWithValue("@" & sibi_requests.NeedBy, RequestData.dtNeedBy)
            cmd.Parameters.AddWithValue("@" & sibi_requests.Status, RequestData.strStatus)
            cmd.Parameters.AddWithValue("@" & sibi_requests.Type, RequestData.strType)
            cmd.Parameters.AddWithValue("@" & sibi_requests.PO, RequestData.strPO)
            cmd.Parameters.AddWithValue("@" & sibi_requests.RequisitionNumber, RequestData.strRequisitionNumber)
            cmd.Parameters.AddWithValue("@" & sibi_requests.Replace_Asset, RequestData.strReplaceAsset)
            cmd.Parameters.AddWithValue("@" & sibi_requests.Replace_Serial, RequestData.strReplaceSerial)
            cmd.Parameters.AddWithValue("@" & sibi_requests.RT_Number, RequestData.strRTNumber)
            rows += cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            Dim strRequestItemsQry As String
            For Each row As DataRow In RequestData.RequstItems.Rows
                If row.Item("Item UID") <> "" Then
                    strRequestItemsQry = "UPDATE " & sibi_request_items.TableName & "
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
WHERE " & sibi_request_items.Item_UID & " ='" & row.Item("Item UID") & "'"
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
                    cmd.CommandText = strRequestItemsQry
                    cmd.ExecuteNonQuery()
                    cmd.Parameters.Clear()
                Else
                    Dim strItemUID As String = Guid.NewGuid.ToString
                    strRequestItemsQry = "INSERT INTO " & sibi_request_items.TableName & "
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
" & sibi_request_items.Qty & "
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
@" & sibi_request_items.Qty & "
)"
                    cmd.Parameters.AddWithValue("@" & sibi_request_items.Item_UID, strItemUID)
                    cmd.Parameters.AddWithValue("@" & sibi_request_items.Request_UID, CurrentRequest.strUID)
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
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
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
            Dim cmd As MySqlCommand = SQLComms.Return_SQLCommand(strAddNoteQry)
            cmd.Parameters.AddWithValue("@" & sibi_notes.Request_UID, RequestUID)
            cmd.Parameters.AddWithValue("@" & sibi_notes.Note_UID, strNoteUID)
            cmd.Parameters.AddWithValue("@" & sibi_notes.Note, Note)
            If cmd.ExecuteNonQuery() > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Return False
            End If
        End Try
    End Function
    Public Sub OpenRequest(RequestUID As String)
        Try
            Dim strRequestQRY As String = "SELECT * FROM " & sibi_requests.TableName & " WHERE " & sibi_requests.UID & "='" & RequestUID & "'"
            Dim strRequestItemsQRY As String = "SELECT * FROM " & sibi_request_items.TableName & " WHERE " & sibi_request_items.Request_UID & "='" & RequestUID & "'"
            Dim RequestResults As DataTable = SQLComms.Return_SQLTable(strRequestQRY)
            Dim RequestItemsResults As DataTable = SQLComms.Return_SQLTable(strRequestItemsQRY)
            ClearAll()
            CollectRequestInfo(RequestResults, RequestItemsResults)
            With RequestResults.Rows(0)
                txtDescription.Text = NoNull(.Item(sibi_requests.Description))
                txtUser.Text = NoNull(.Item(sibi_requests.RequestUser))
                cmbType.SelectedIndex = GetComboIndexFromShort(SibiIndex.RequestType, NoNull(.Item(sibi_requests.Type)))
                dtNeedBy.Value = NoNull(.Item(sibi_requests.NeedBy))
                cmbStatus.SelectedIndex = GetComboIndexFromShort(SibiIndex.StatusType, NoNull(.Item(sibi_requests.Status)))
                txtPO.Text = NoNull(.Item(sibi_requests.PO))
                txtReqNumber.Text = NoNull(.Item(sibi_requests.RequisitionNumber))
                txtRequestNum.Text = NoNull(.Item(sibi_requests.RequestNumber))
                txtRTNumber.Text = NoNull(.Item(sibi_requests.RT_Number))
                txtCreateDate.Text = NoNull(.Item(sibi_requests.DateStamp))
            End With
            SendToGrid(RequestItemsResults)
            LoadNotes(CurrentRequest.strUID)
            'RequestItemsGrid.ReadOnly = True
            DisableControls(Me)
            SetTitle()
            SetAttachCount()
            Me.Show()
            Me.Activate()
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Private Sub LoadNotes(RequestUID As String)
        Dim strPullNotesQry As String = "SELECT * FROM " & sibi_notes.TableName & " WHERE " & sibi_notes.Request_UID & "='" & RequestUID & "' ORDER BY " & sibi_notes.DateStamp & " DESC"
        Dim Results As DataTable = SQLComms.Return_SQLTable(strPullNotesQry)
        Dim table As New DataTable
        Dim intPreviewChars As Integer = 50
        table.Columns.Add("Date Stamp")
        table.Columns.Add("Preview")
        table.Columns.Add("UID")
        For Each r As DataRow In Results.Rows
            table.Rows.Add(r.Item(sibi_notes.DateStamp),
                           IIf(Len(r.Item(sibi_notes.Note)) > intPreviewChars, NotePreview(r.Item(sibi_notes.Note)), r.Item(sibi_notes.Note)),
                           r.Item(sibi_notes.Note_UID))
        Next
        dgvNotes.DataSource = table
        dgvNotes.ClearSelection()
        table.Dispose()
        Results.Dispose()
    End Sub
    Private Function DeleteItem_FromSQL(ItemUID As String, ItemColumnName As String, Table As String) As Integer
        Try
            Dim rows
            Dim strSQLQry As String = "DELETE FROM " & Table & " WHERE " & ItemColumnName & "='" & ItemUID & "'"
            rows = SQLComms.Return_SQLCommand(strSQLQry).ExecuteNonQuery
            Return rows
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
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
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
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
                End With
            Next
            RequestItemsGrid.ClearSelection()
            bolGridFilling = False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
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
                Dim NewAttach As New frmAttachments(Me, CurrentRequest)
            End If
        Else
            ActivateForm(CurrentRequest.strUID)
        End If
    End Sub
    Public Function AttachmentsIsOpen(strGUID As String) As Boolean
        For Each frm As Form In My.Application.OpenForms
            If TypeOf frm Is frmAttachments And frm.Tag Is Me Then
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
        bolNewRequest = True
        EnableControls(Me)
        pnlCreate.Visible = True
    End Sub
    Private Sub frmManageRequest_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        CloseChildren(Me)
        SendWindowListRefresh()
        Me.Dispose()
    End Sub
    Private Sub SendWindowListRefresh()
        Dim ParentForm As Form = Me.Tag
        If TypeOf ParentForm Is frmSibiMain Then
            Dim sibi As frmSibiMain = ParentForm
            sibi.MyWindowList.RefreshWindowList()
        ElseIf TypeOf ParentForm Is View Then
            'do nut-zing
        End If
    End Sub
    Private Sub tsmDeleteItem_Click(sender As Object, e As EventArgs) Handles tsmDeleteItem.Click
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        Dim blah
        blah = Message("Delete selected item?", vbYesNo + vbQuestion, "Delete Item Row")
        If blah = vbYes Then
            If bolNewRequest Then
                If DeleteItem_FromLocal(RequestItemsGrid.CurrentRow.Index) Then
                Else
                    blah = Message("Failed to delete row.", vbExclamation + vbOKOnly, "Error", Me)
                End If
            Else
                blah = Message(DeleteItem_FromSQL(RequestItemsGrid.Item(GetColIndex(RequestItemsGrid, "Item UID"), RequestItemsGrid.CurrentRow.Index).Value, "sibi_items_uid", "sibi_request_items") & " Rows affected.", vbOKOnly + vbInformation, "Delete Item")
                OpenRequest(CurrentRequest.strUID)
            End If

        Else
        End If
    End Sub
    Private Sub txtRTNumber_Click(sender As Object, e As EventArgs) Handles txtRTNumber.Click
        Dim RTNum As String = Trim(txtRTNumber.Text)
        If Not bolUpdating And RTNum <> "" Then
            Process.Start("http://rt.co.fairfield.oh.us/rt/Ticket/Display.html?id=" & RTNum)
        End If
    End Sub
    Private Sub txtReqNumber_Click(sender As Object, e As EventArgs) Handles txtReqNumber.Click
        Dim ReqNum As String = Trim(txtReqNumber.Text)
        If Not bolUpdating And ReqNum <> "" Then
            Munis.NewMunisView_ReqSearch(ReqNum, YearFromDate(CurrentRequest.dtDateStamp), Me)

        End If
    End Sub
    Private Sub cmdDelete_Click(sender As Object, e As EventArgs) Handles cmdDelete.Click
        If Not CheckForAccess(AccessGroup.Sibi_Delete) Then Exit Sub
        If IsNothing(CurrentRequest.RequstItems) Then Exit Sub
        Dim blah = Message("Are you absolutely sure?  This cannot be undone and will delete all data including attachments.", vbYesNo + vbExclamation, "WARNING")
        If blah = vbYes Then
            If Asset.DeleteMaster(CurrentRequest.strUID, Entry_Type.Sibi) Then
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
        AddNote()
    End Sub
    Private Sub AddNote()
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        If CurrentRequest.strUID <> "" Then
            Dim NewNote As New frmNotes(Me, CurrentRequest)
            If NewNote.DialogResult = DialogResult.OK Then
                AddNewNote(NewNote.Request.strUID, Trim(NewNote.rtbNotes.Text))
                RefreshRequest()
            End If
        End If
    End Sub
    Private Sub RefreshRequest()
        OpenRequest(CurrentRequest.strUID)
    End Sub
    Private Sub dgvNotes_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles dgvNotes.CellDoubleClick
        Dim ViewNote As New frmNotes(dgvNotes.Item(GetColIndex(dgvNotes, "UID"), dgvNotes.CurrentRow.Index).Value)
    End Sub
    Private Sub cmdDeleteNote_Click(sender As Object, e As EventArgs) Handles cmdDeleteNote.Click
        If Not CheckForAccess(AccessGroup.Sibi_Modify) Then Exit Sub
        Dim blah = Message("Are you sure?", vbYesNo + vbQuestion, "Delete Note", Me)
        If blah = vbYes Then
            Dim NoteUID As String = dgvNotes.Item(GetColIndex(dgvNotes, "UID"), dgvNotes.CurrentRow.Index).Value
            If NoteUID <> "" Then
                Message(DeleteItem_FromSQL(NoteUID, sibi_notes.Note_UID, sibi_notes.TableName) & " Rows affected.", vbOKOnly + vbInformation, "Delete Item")
                OpenRequest(CurrentRequest.strUID)
            End If
        End If
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
            LookupDevice(Asset.FindDevice(RequestItemsGrid.Item(GetColIndex(RequestItemsGrid, "Replace Asset"), RequestItemsGrid.CurrentRow.Index).Value))
        ElseIf RequestItemsGrid.Item(GetColIndex(RequestItemsGrid, "Replace Serial"), RequestItemsGrid.CurrentRow.Index).Value IsNot "" Then
            LookupDevice(Asset.FindDevice(RequestItemsGrid.Item(GetColIndex(RequestItemsGrid, "Replace Serial"), RequestItemsGrid.CurrentRow.Index).Value))
        End If
    End Sub
    Private Sub cmdAccept_Click(sender As Object, e As EventArgs) Handles cmdAccept.Click
        If Not ValidateFields() Then Exit Sub
        DisableControls(Me)
        ToolStrip.BackColor = colToolBarColor
        cmdUpdate.Font = New Font(cmdUpdate.Font, FontStyle.Regular)
        cmdUpdate.Text = "Update"
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
            Munis.NewMunisView_POSearch(PO, Me)
        End If
    End Sub
    Private Sub RequestItemsGrid_DataError(sender As Object, e As DataGridViewDataErrorEventArgs) Handles RequestItemsGrid.DataError
        Dim blah = Message("DataGrid Error: " & Chr(34) & e.Exception.Message & Chr(34) & "   Col/Row:" & e.ColumnIndex & "/" & e.RowIndex, vbOKOnly + vbExclamation, "DataGrid Error")
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
                .dtDateStamp = NoNull(RequestResults.Rows(0).Item(sibi_requests.DateStamp))
                .dtNeedBy = NoNull(RequestResults.Rows(0).Item(sibi_requests.NeedBy))
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
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub frmManageRequest_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Dim f As Form = sender
        If f.WindowState = FormWindowState.Minimized Then
            MinimizeChildren(Me)
            PrevWindowState = f.WindowState
        ElseIf f.WindowState <> PrevWindowState And f.WindowState = FormWindowState.Normal Then
            If PrevWindowState <> FormWindowState.Maximized Then RestoreChildren(Me)
        End If
    End Sub
    Private PrevWindowState As Integer
    Private Sub frmManageRequest_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin
        Dim f As Form = sender
        PrevWindowState = f.WindowState
    End Sub
    Private Sub frmManageRequest_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        CloseChildren(Me)
    End Sub
    Private Sub cmdNewNote_Click(sender As Object, e As EventArgs) Handles cmdNewNote.Click
        AddNote()
    End Sub
End Class
