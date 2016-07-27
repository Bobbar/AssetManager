Option Explicit On
Imports System.Collections
Imports MySql.Data.MySqlClient
Public Class frmManageRequest
    ' Public table As New DataTable
    Private Sub frmNewRequest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ExtendedMethods.DoubleBuffered(RequestItemsGrid, True)
        'FillCombos()
        '   SetupGrid()
    End Sub
    Private Sub ClearAll()
        ClearControls(Me)


        SetupGrid()
        FillCombos()
        EnableControls(Me)
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

    Private Sub DisableGrid()
        RequestItemsGrid.EditMode = DataGridViewEditMode.EditProgrammatically
        RequestItemsGrid.AllowUserToAddRows = False
    End Sub
    Private Sub EnableGrid()
        RequestItemsGrid.EditMode = DataGridViewEditMode.EditOnEnter
        RequestItemsGrid.AllowUserToAddRows = True
    End Sub
    Private Sub SetupGrid()

        RequestItemsGrid.DataSource = Nothing
        RequestItemsGrid.Rows.Clear()
        RequestItemsGrid.Columns.Clear()

        With RequestItemsGrid.Columns
            .Add("User", "User")
            .Add("Description", "Description")
            .Add(DataGridCombo(Locations, "Location", ComboType.Location)) '.Add("Location")
            .Add(DataGridCombo(Sibi_ItemStatusType, "Status", ComboType.SibiItemStatusType))
            .Add("Replace Asset", "Replace Asset")
            .Add("Replace Serial", "Replace Serial")
        End With
        'table.Dispose()
    End Sub
    Private Sub FillCombos()
        FillComboBox(Sibi_StatusType, cmbStatus)
        FillComboBox(Sibi_RequestType, cmbType)
    End Sub
    Private Function DataGridCombo(IndexType() As Combo_Data, HeaderText As String, Name As String) As DataGridViewComboBoxColumn
        Dim tmpCombo As New DataGridViewComboBoxColumn
        tmpCombo.Items.Clear()
        tmpCombo.HeaderText = HeaderText
        tmpCombo.Name = Name
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
        Dim info As Request_Info
        ' Dim dt As DataTable = RequestItemsGrid.DataSource
        RequestItemsGrid.EndEdit()
        ' Dim GridTable = TryCast(RequestItemsGrid.DataSource, DataTable)
        With info
            .strDescription = Trim(txtDescription.Text)
            .strUser = Trim(txtUser.Text)
            .strType = GetDBValue(Sibi_RequestType, cmbType.SelectedIndex)
            .dtNeedBy = dtNeedBy.Value.ToString(strDBDateFormat)
            .strStatus = GetDBValue(Sibi_StatusType, cmbStatus.SelectedIndex)
            .strPO = Trim(txtPO.Text)
            .strRequisitionNumber = Trim(txtReqNumber.Text)
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
                        'Dim cmb As DataGridViewComboBoxCell = dcell
                        'Debug.Print(cmb.Value)
                        NewRow(dcell.ColumnIndex) = GetDBValueFromHuman(dcell.OwningColumn.Name, dcell.Value)
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
    End Function
    Private Sub cmdAddNew_Click(sender As Object, e As EventArgs) Handles cmdAddNew.Click
        'CollectData()
        AddNewRequest()
    End Sub
    Private Sub AddNewRequest()
        Dim RequestData As Request_Info = CollectData()
        Dim strRequestUID As String = Guid.NewGuid.ToString
        Try
            Dim rows As Integer
            'If Not CheckFields() Then
            '    Dim blah = MsgBox("Some required fields are missing.  Please fill in all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data")
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
`sibi_replace_serial`)
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
@sibi_replace_serial)"
            Dim cmd As MySqlCommand = ReturnSQLCommand(strSqlQry1)
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
            rows = rows + cmd.ExecuteNonQuery()
            cmd.Parameters.Clear()
            For Each row As DataRow In RequestData.RequstItems.Rows
                Dim strItemUID As String = Guid.NewGuid.ToString
                Debug.Print(strItemUID)
                Dim strSqlQry2 = "INSERT INTO `asset_manager`.`sibi_request_items`
(`sibi_items_uid`,
`sibi_items_request_uid`,
`sibi_items_user`,
`sibi_items_description`,
`sibi_items_location`,
`sibi_items_status`,
`sibi_items_replace_asset`,
`sibi_items_replace_serial`)
VALUES
(@sibi_items_uid,
@sibi_items_request_uid,
@sibi_items_user,
@sibi_items_description,
@sibi_items_location,
@sibi_items_status,
@sibi_items_replace_asset,
@sibi_items_replace_serial)"
                cmd.Parameters.AddWithValue("@sibi_items_uid", strItemUID)
                cmd.Parameters.AddWithValue("@sibi_items_request_uid", strRequestUID)
                cmd.Parameters.AddWithValue("@sibi_items_user", row.Item("User"))
                cmd.Parameters.AddWithValue("@sibi_items_description", row.Item("Description"))
                cmd.Parameters.AddWithValue("@sibi_items_location", row.Item(ComboType.Location))
                cmd.Parameters.AddWithValue("@sibi_items_status", row.Item(ComboType.SibiItemStatusType))
                cmd.Parameters.AddWithValue("@sibi_items_replace_asset", row.Item("Replace Asset"))
                cmd.Parameters.AddWithValue("@sibi_items_replace_serial", row.Item("Replace Serial"))
                cmd.CommandText = strSqlQry2
                rows = rows + cmd.ExecuteNonQuery()
                cmd.Parameters.Clear()
            Next
            Debug.Print("Rows: " & rows)
            cmd.Dispose()
            'If rows = 2 Then 'ExecuteQuery returns the number of rows affected. We can check this to make sure the qry completed successfully.
            '    Dim blah = MsgBox("New Device Added.   Add another?", vbYesNo + vbInformation, "Complete")
            '    If Not chkNoClear.Checked Then ClearAll()
            '    If blah = vbNo Then Me.Hide()
            'Else
            '    Dim blah = MsgBox("Unsuccessful! The number of affected rows was not what was expected.", vbOKOnly + vbExclamation, "Unexpected Result")
            'End If
            'cmd.Dispose()
            'Exit Sub
            OpenRequest(strRequestUID)
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Exit Sub
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Public Sub OpenRequest(RequestUID As String)
        Dim strRequestQRY As String = "SELECT * FROM sibi_requests WHERE sibi_uid='" & RequestUID & "'"
        Dim strRequestItemsQRY As String = "SELECT * FROM sibi_request_items WHERE sibi_items_request_uid='" & RequestUID & "'"
        Dim Results As DataTable = ReturnSQLTable(strRequestQRY)
        ClearAll()
        With Results.Rows(0)
            txtDescription.Text = .Item("sibi_description")
            txtUser.Text = .Item("sibi_request_user")
            cmbType.SelectedIndex = GetComboIndexFromShort(ComboType.SibiRequestType, .Item("sibi_type"))
            dtNeedBy.Value = .Item("sibi_need_by")
            cmbStatus.SelectedIndex = GetComboIndexFromShort(ComboType.SibiStatusType, .Item("sibi_status"))
            txtPO.Text = .Item("sibi_PO")
            txtReqNumber.Text = .Item("sibi_requisition_number")
            txtRequestNum.Text = .Item("sibi_request_number")
        End With

        SendToGrid(ReturnSQLTable(strRequestItemsQRY))
        'RequestItemsGrid.ReadOnly = True
        DisableControls(Me)

        Me.Show()
    End Sub

    Private Sub SendToGrid(Results As DataTable) ' Data() As Device_Info)
        'Try
        'StatusBar(strLoadingGridMessage)
        '   Dim table As New DataTable
        'table.Columns.Add("Request #", GetType(String))
        'table.Columns.Add("Status", GetType(String))
        'table.Columns.Add("Description", GetType(String))
        'table.Columns.Add("Request User", GetType(String))
        'table.Columns.Add("Request Type", GetType(String))
        'table.Columns.Add("Need By", GetType(String))
        'table.Columns.Add("PO Number", GetType(String))
        'table.Columns.Add("Req. Number", GetType(String))
        'table.Columns.Add("UID", GetType(String))
        'table.Columns.Add("Location", GetType(String))
        'table.Columns.Add("Purchase Date", GetType(String))
        'table.Columns.Add("Replace Year", GetType(String))
        'table.Columns.Add("GUID", GetType(String))
        'RequestItemsGrid.Rows.Add(,)
        SetupGrid()

        For Each r As DataRow In Results.Rows
            With RequestItemsGrid.Rows
                .Add(r.Item("sibi_items_user"),
                    r.Item("sibi_items_description"),
                    GetHumanValue(ComboType.Location, r.Item("sibi_items_location")),
                         GetHumanValue(ComboType.SibiItemStatusType, r.Item("sibi_items_status")),
                         r.Item("sibi_items_replace_asset"),
                         r.Item("sibi_items_replace_serial"))



            End With

        Next
        'bolGridFilling = True
        ' RequestItemsGrid.DataSource = table
        RequestItemsGrid.ClearSelection()
        'bolGridFilling = False
        'table.Dispose()
        ' Catch ex As Exception
        ' ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        '  End Try
    End Sub

    Private Sub cmdClearAll_Click(sender As Object, e As EventArgs) Handles cmdClearAll.Click
        ClearAll()
    End Sub

    Private Sub cmdUpdate_Click(sender As Object, e As EventArgs) Handles cmdUpdate.Click
        EnableControls(Me)
    End Sub
End Class

