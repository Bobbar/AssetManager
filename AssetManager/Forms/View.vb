Option Explicit On
Option Strict Off
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class View
    Private bolCheckFields As Boolean
    Private Structure UserInput
        Public strAssetTag As String
        Public strDescription As String
        Public strEqType As String
        Public strSerial As String
        Public strLocation As String
        Public strCurrentUser As String
        Public dtPurchaseDate As String
        Public strReplaceYear As String
        Public strOSVersion As String
    End Structure
    Private OldData As Device_Info
    Public NewData As Device_Info
    Private Sub View_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ExtendedMethods.DoubleBuffered(DataGridHistory, True)
        AssetManager.CopyDefaultCellStyles()
        'ClearFields()
    End Sub
    Private Sub GetCurrentValues()
        With OldData
            .strAssetTag = Trim(txtAssetTag_View_REQ.Text)
            .strDescription = Trim(txtDescription_View_REQ.Text)
            .strEqType = GetDBValue(ComboType.EquipType, cmbEquipType_View_REQ.SelectedIndex)
            .strSerial = Trim(txtSerial_View_REQ.Text)
            .strLocation = GetDBValue(ComboType.Location, cmbLocation_View_REQ.SelectedIndex)
            .strCurrentUser = Trim(txtCurUser_View_REQ.Text)
            .dtPurchaseDate = dtPurchaseDate_View_REQ.Value.ToString(strDBDateFormat)
            .strReplaceYear = Trim(txtReplacementYear_View.Text)
            .strOSVersion = GetDBValue(ComboType.OSType, cmbOSVersion_REQ.SelectedIndex)
            .strStatus = GetDBValue(ComboType.StatusType, cmbStatus_REQ.SelectedIndex)
            .bolTrackable = chkTrackable.Checked
        End With
    End Sub
    Public Sub GetNewValues()
        With NewData
            .strAssetTag = Trim(txtAssetTag_View_REQ.Text)
            .strDescription = Trim(txtDescription_View_REQ.Text)
            .strEqType = GetDBValue(ComboType.EquipType, cmbEquipType_View_REQ.SelectedIndex)
            .strSerial = Trim(txtSerial_View_REQ.Text)
            .strLocation = GetDBValue(ComboType.Location, cmbLocation_View_REQ.SelectedIndex)
            .strCurrentUser = Trim(txtCurUser_View_REQ.Text)
            .dtPurchaseDate = dtPurchaseDate_View_REQ.Value.ToString(strDBDateFormat)
            .strReplaceYear = Trim(txtReplacementYear_View.Text)
            .strOSVersion = GetDBValue(ComboType.OSType, cmbOSVersion_REQ.SelectedIndex)
            .strStatus = GetDBValue(ComboType.StatusType, cmbStatus_REQ.SelectedIndex)
            .strNote = UpdateDev.strNewNote
            .bolTrackable = chkTrackable.Checked
        End With
    End Sub
    Private Sub EnableControls()
        Dim c As Control
        For Each c In pnlViewControls.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    Dim txt As TextBox = c
                    If txt.Name <> "txtGUID" Then
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
        Next
        cmdUpdate.Visible = True
        cmdCancel.Visible = True
    End Sub
    Private Sub DisableControls()
        Dim c As Control
        For Each c In pnlViewControls.Controls
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
        Next
        cmdUpdate.Visible = False
        cmdCancel.Visible = False
    End Sub
    Public Sub ViewDevice(ByVal DeviceUID As String)
        On Error GoTo errs
        ClearFields()
        RefreshCombos()
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim strQry = "Select * FROM devices, historical WHERE dev_UID = hist_dev_UID And dev_UID = '" & DeviceUID & "' ORDER BY hist_action_datetime DESC"
        Dim cmd As New MySqlCommand(strQry, GetConnection(ConnID).DBConnection)
        reader = cmd.ExecuteReader
        table.Columns.Add("Date", GetType(String))
        table.Columns.Add("Action Type", GetType(String))
        table.Columns.Add("Action User", GetType(String))
        table.Columns.Add("User", GetType(String))
        table.Columns.Add("Asset ID", GetType(String))
        table.Columns.Add("Serial", GetType(String))
        table.Columns.Add("Description", GetType(String))
        table.Columns.Add("Location", GetType(String))
        table.Columns.Add("Purchase Date", GetType(String))
        table.Columns.Add("GUID", GetType(String))
        With reader
            Do While .Read()
                CollectDeviceInfo(!dev_UID,!dev_description,!dev_location,!dev_cur_user,!dev_serial,!dev_asset_tag,!dev_purchase_date,!dev_replacement_year,!dev_po,!dev_osversion,!dev_eq_type,!dev_status, CBool(!dev_trackable))
                txtAssetTag_View_REQ.Text = !dev_asset_tag
                txtDescription_View_REQ.Text = !dev_description
                cmbEquipType_View_REQ.SelectedIndex = GetComboIndexFromShort(ComboType.EquipType,!dev_eq_type)
                txtSerial_View_REQ.Text = !dev_serial
                cmbLocation_View_REQ.SelectedIndex = GetComboIndexFromShort(ComboType.Location,!dev_location)
                txtCurUser_View_REQ.Text = !dev_cur_user
                dtPurchaseDate_View_REQ.Value = !dev_purchase_date
                txtReplacementYear_View.Text = !dev_replacement_year
                cmbOSVersion_REQ.SelectedIndex = GetComboIndexFromShort(ComboType.OSType,!dev_osversion)
                cmbStatus_REQ.SelectedIndex = GetComboIndexFromShort(ComboType.StatusType,!dev_status)
                txtGUID.Text = !dev_UID
                chkTrackable.Checked = CBool(!dev_trackable)
                SetTracking(CBool(!dev_trackable))
                table.Rows.Add(!hist_action_datetime, GetHumanValue(ComboType.ChangeType,!hist_change_type),!hist_action_user,!hist_cur_user,!hist_asset_tag,!hist_serial,!hist_description, GetHumanValue(ComboType.Location,!hist_location),!hist_purchase_date,!hist_uid)
            Loop
        End With
        CloseConnection(ConnID)
        DataGridHistory.DataSource = table
        DataGridHistory.Columns("Action Type").DefaultCellStyle.Font = New Font(DataGridHistory.Font, FontStyle.Bold)
        DisableControls()
        DataGridHistory.AutoResizeColumns()
        Exit Sub
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Resume Next
        Else
            EndProgram()
        End If
    End Sub
    Public Sub SetTracking(bolEnabled As Boolean)
        If bolEnabled Then
            'TrackingTab.Visible = True
            'TrackingTab.Enabled = True
            TrackingToolStripMenuItem.Visible = True
            TabControl1.TabPages.Insert(1, TrackingTab)
        Else
            'TrackingTab.Enabled = False
            'TrackingTab.Visible = False
            TrackingToolStripMenuItem.Visible = False
            TabControl1.TabPages.Remove(TrackingTab)
        End If
    End Sub
    Private Sub ModifyDevice()
        GetCurrentValues()
        EnableControls()
    End Sub
    Private Sub ClearFields()
        Dim c As Control
        For Each c In pnlViewControls.Controls
            If TypeOf c Is TextBox Then
                Dim txt As TextBox = c
                txt.Text = ""
            End If
            If TypeOf c Is ComboBox Then
                Dim cmb As ComboBox = c
                cmb.SelectedIndex = -1
            End If
        Next
    End Sub
    Private Function CheckFields() As Boolean
        Dim bolMissingField As Boolean
        bolMissingField = False
        Dim c As Control
        For Each c In pnlViewControls.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    If c.Name.Contains("REQ") Then
                        If Trim(c.Text) = "" Then
                            bolMissingField = True
                            c.BackColor = colMissingField
                        Else
                            c.BackColor = Color.Empty
                        End If
                    End If
                Case TypeOf c Is ComboBox
                    Dim cmb As ComboBox = c
                    If cmb.Name.Contains("REQ") Then
                        If cmb.SelectedIndex = -1 Then
                            bolMissingField = True
                            cmb.BackColor = colMissingField
                        Else
                            cmb.BackColor = Color.Empty
                        End If
                    End If
            End Select
        Next
        Return Not bolMissingField 'if fields are missing return false to trigger a message if needed
    End Function
    Private Sub ResetBackColors()
        Dim c As Control
        For Each c In pnlViewControls.Controls
            ' c.BackColor = Color.Empty
            Select Case True
                Case TypeOf c Is TextBox
                    c.BackColor = Color.Empty
                Case TypeOf c Is ComboBox
                    c.BackColor = Color.Empty
            End Select
        Next
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        EnableControls()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        DisableControls()
    End Sub
    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        If Not CheckForAdmin() Then Exit Sub
        ModifyDevice()
    End Sub
    Private Sub cmdUpdate_Click(sender As Object, e As EventArgs) Handles cmdUpdate.Click
        If Not CheckFields() Then
            Dim blah = MsgBox("Some required fields are missing.  Please fill in all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data")
            bolCheckFields = True
            Exit Sub
        End If
        UpdateDev.cmbUpdate_ChangeType.SelectedIndex = -1
        UpdateDev.cmbUpdate_ChangeType.Enabled = True
        UpdateDev.Show()
    End Sub
    Private Sub View_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.Hide()
    End Sub
    Private Sub DataGridHistory_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
    End Sub
    Private Sub DataGridHistory_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs)
        NewEntryView(DataGridHistory.Item(GetColIndex(DataGridHistory, "GUID"), DataGridHistory.CurrentRow.Index).Value)
    End Sub
    Private Sub NewEntryView(GUID As String)
        Dim NewEntry As New View_Entry
        NewEntry.ViewEntry(GUID)
        NewEntry.Show()
    End Sub
    Private Sub AddNoteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddNoteToolStripMenuItem.Click
        If Not CheckForAdmin() Then Exit Sub
        UpdateDev.cmbUpdate_ChangeType.SelectedIndex = GetComboIndexFromShort(ComboType.ChangeType, "NOTE")
        UpdateDev.cmbUpdate_ChangeType.Enabled = False
        UpdateDev.Show()
    End Sub
    Private Sub RefreshCombos()
        FillEquipTypeCombo()
        FillLocationCombo()
        FillOSTypeCombo()
        FillStatusTypeCombo()
    End Sub
    Private Sub FillEquipTypeCombo()
        Dim i As Integer
        cmbEquipType_View_REQ.Items.Clear()
        cmbEquipType_View_REQ.Text = ""
        For i = 0 To UBound(EquipType)
            cmbEquipType_View_REQ.Items.Insert(i, EquipType(i).strLong)
        Next
    End Sub
    Private Sub FillLocationCombo()
        Dim i As Integer
        cmbLocation_View_REQ.Items.Clear()
        cmbLocation_View_REQ.Text = ""
        For i = 0 To UBound(Locations)
            cmbLocation_View_REQ.Items.Insert(i, Locations(i).strLong)
        Next
    End Sub
    Private Sub FillOSTypeCombo()
        Dim i As Integer
        cmbOSVersion_REQ.Items.Clear()
        cmbOSVersion_REQ.Text = ""
        For i = 0 To UBound(OSType)
            cmbOSVersion_REQ.Items.Insert(i, OSType(i).strLong)
        Next
    End Sub
    Private Sub FillStatusTypeCombo()
        Dim i As Integer
        cmbStatus_REQ.Items.Clear()
        cmbStatus_REQ.Text = ""
        For i = 0 To UBound(StatusType)
            cmbStatus_REQ.Items.Insert(i, StatusType(i).strLong)
        Next
    End Sub
    Private Sub DeleteDeviceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteDeviceToolStripMenuItem.Click
        If Not CheckForAdmin() Then Exit Sub
        Dim blah = MsgBox("Are you absolutely sure?  This cannot be undone and will delete all histrical data.", vbYesNo + vbCritical, "WARNING")
        If blah = vbYes Then
            Dim blah2 = MsgBox(DeleteDevice(CurrentDevice.strGUID) & " rows affected.", vbOKOnly + vbInformation, "Deletion Results")
            Me.Hide()
        Else
            Exit Sub
        End If
    End Sub
    Private Sub txtAssetTag_View_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag_View_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub txtDescription_View_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtDescription_View_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmbEquipType_View_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEquipType_View_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub txtSerial_View_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtSerial_View_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmbLocation_View_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLocation_View_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmbOSVersion_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbOSVersion_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmbStatus_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatus_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub txtCurUser_View_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtCurUser_View_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub dtPurchaseDate_View_REQ_ValueChanged(sender As Object, e As EventArgs) Handles dtPurchaseDate_View_REQ.ValueChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        bolCheckFields = False
        ClearFields()
        DisableControls()
        ViewDevice(CurrentDevice.strGUID)
    End Sub
    Private Sub DataGridHistory_CellContentClick_1(sender As Object, e As DataGridViewCellEventArgs)
    End Sub
    Private Sub DeleteEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteEntryToolStripMenuItem.Click
        If Not CheckForAdmin() Then Exit Sub
        Dim strGUID As String = DataGridHistory.Item(GetColIndex(DataGridHistory, "GUID"), DataGridHistory.CurrentRow.Index).Value
        Dim Info As Device_Info = GetEntryInfo(strGUID)
        Dim blah = MsgBox("Are you absolutely sure?  This cannot be undone!" & vbCrLf & vbCrLf & "Entry info: " & Info.Historical.dtActionDateTime & " - " & Info.Historical.strChangeType & " - " & strGUID, vbYesNo + vbCritical, "WARNING")
        If blah = vbYes Then
            Dim blah2 = MsgBox(DeleteEntry(strGUID) & " rows affected.", vbOKOnly + vbInformation, "Deletion Results")
            ViewDevice(CurrentDevice.strGUID)
            'Me.Hide()
        Else
            Exit Sub
        End If
        'DeleteEntry(DataGridHistory.Item(GetColIndex(DataGridHistory, "GUID"), DataGridHistory.CurrentRow.Index).Value)
    End Sub
    Private Sub DataGridHistory_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs)
        If e.Button = MouseButtons.Right Then
            DataGridHistory.CurrentCell = DataGridHistory(e.ColumnIndex, e.RowIndex) 'DataGridHistory.RowIndex
        End If
    End Sub
    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs)
    End Sub
End Class