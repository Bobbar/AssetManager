Option Explicit On
Option Strict Off
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class View
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
    End Sub
    Private Sub GetCurrentValues()
        With OldData
            .strAssetTag = Trim(txtAssetTag_View.Text)
            .strDescription = Trim(txtDescription_View.Text)
            .strEqType = GetDBValue(ComboType.EquipType, cmbEquipType_View.SelectedIndex)
            .strSerial = Trim(txtSerial_View.Text)
            .strLocation = GetDBValue(ComboType.Location, cmbLocation_View.SelectedIndex)
            .strCurrentUser = Trim(txtCurUser_View.Text)
            .dtPurchaseDate = dtPurchaseDate_View.Value.ToString(strDBDateFormat)
            .strReplaceYear = Trim(txtReplacementYear_View.Text)
            .strOSVersion = GetDBValue(ComboType.OSType, cmbOSVersion.SelectedIndex)
            .strStatus = GetDBValue(ComboType.StatusType, cmbStatus.SelectedIndex)
        End With
    End Sub
    Public Sub GetNewValues()
        With NewData
            .strAssetTag = Trim(txtAssetTag_View.Text)
            .strDescription = Trim(txtDescription_View.Text)
            .strEqType = GetDBValue(ComboType.EquipType, cmbEquipType_View.SelectedIndex)
            .strSerial = Trim(txtSerial_View.Text)
            .strLocation = GetDBValue(ComboType.Location, cmbLocation_View.SelectedIndex)
            .strCurrentUser = Trim(txtCurUser_View.Text)
            .dtPurchaseDate = dtPurchaseDate_View.Value.ToString(strDBDateFormat)
            .strReplaceYear = Trim(txtReplacementYear_View.Text)
            .strOSVersion = GetDBValue(ComboType.OSType, cmbOSVersion.SelectedIndex)
            .strStatus = GetDBValue(ComboType.StatusType, cmbStatus.SelectedIndex)
            .strNote = UpdateDev.strNewNote
        End With
    End Sub
    Private Sub EnableControls()
        Dim c As Control
        For Each c In pnlViewControls.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    Dim txt As TextBox = c
                    txt.ReadOnly = False
                Case TypeOf c Is ComboBox
                    Dim cmb As ComboBox = c
                    cmb.Enabled = True
                Case TypeOf c Is DateTimePicker
                    Dim dtp As DateTimePicker = c
                    dtp.Enabled = True
                Case TypeOf c Is Label
                    'do nut-zing
            End Select
        Next
        cmdUpdate.Visible = True
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
                Case TypeOf c Is Label
                    'do nut-zing
            End Select
        Next
        cmdUpdate.Visible = False
    End Sub
    Public Sub ViewDevice(ByVal DeviceUID As String)
        RefreshCombos()
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        cn_global.Open()
        Dim strQry = "Select * FROM devices, historical WHERE dev_UID = hist_dev_UID And dev_UID = '" & DeviceUID & "' ORDER BY hist_action_datetime DESC"
        Dim cmd As New MySqlCommand(strQry, cn_global)
        reader = cmd.ExecuteReader
        table.Columns.Add("Date", GetType(String))
        table.Columns.Add("Action Type", GetType(String))
        table.Columns.Add("User", GetType(String))
        table.Columns.Add("Asset ID", GetType(String))
        table.Columns.Add("Serial", GetType(String))
        table.Columns.Add("Description", GetType(String))
        table.Columns.Add("Location", GetType(String))
        table.Columns.Add("Purchase Date", GetType(String))
        table.Columns.Add("GUID", GetType(String))
        With reader
            Do While .Read()
                CollectDeviceInfo(!dev_UID,!dev_description,!dev_location,!dev_cur_user,!dev_serial,!dev_asset_tag,!dev_purchase_date,!dev_replacement_year,!dev_po,!dev_osversion,!dev_eq_type,!dev_status)
                txtAssetTag_View.Text = !dev_asset_tag
                txtDescription_View.Text = !dev_description
                cmbEquipType_View.SelectedIndex = GetComboIndexFromShort(ComboType.EquipType,!dev_eq_type)
                txtSerial_View.Text = !dev_serial
                cmbLocation_View.SelectedIndex = GetComboIndexFromShort(ComboType.Location,!dev_location)
                txtCurUser_View.Text = !dev_cur_user
                dtPurchaseDate_View.Value = !dev_purchase_date
                txtReplacementYear_View.Text = !dev_replacement_year
                cmbOSVersion.SelectedIndex = GetComboIndexFromShort(ComboType.OSType,!dev_osversion)
                cmbStatus.SelectedIndex = GetComboIndexFromShort(ComboType.StatusType,!dev_status)
                txtGUID.Text = !dev_UID
                table.Rows.Add(!hist_action_datetime, GetHumanValue(ComboType.ChangeType,!hist_change_type),!hist_cur_user,!hist_asset_tag,!hist_serial,!hist_description, GetHumanValue(ComboType.Location,!hist_location),!hist_purchase_date,!hist_uid)
            Loop
        End With
        cn_global.Close()
        DataGridHistory.DataSource = table
        DataGridHistory.Columns("Action Type").DefaultCellStyle.Font = New Font(DataGridHistory.Font, FontStyle.Bold)
        DisableControls()
        DataGridHistory.AutoResizeColumns()
    End Sub
    Private Sub ModifyDevice()
        GetCurrentValues()
        EnableControls()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        EnableControls()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        DisableControls()
    End Sub
    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        ModifyDevice()
    End Sub
    Private Sub cmdUpdate_Click(sender As Object, e As EventArgs) Handles cmdUpdate.Click
        UpdateDev.cmbUpdate_ChangeType.SelectedIndex = -1
        UpdateDev.cmbUpdate_ChangeType.Enabled = True
        UpdateDev.Show()
    End Sub
    Private Sub View_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.Hide()
    End Sub
    Private Sub DataGridHistory_CellContentClick(sender As Object, e As DataGridViewCellEventArgs)
    End Sub
    Private Sub DataGridHistory_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridHistory.CellDoubleClick
        NewEntryView(DataGridHistory.Item(8, DataGridHistory.CurrentRow.Index).Value)
    End Sub
    Private Sub NewEntryView(GUID As String)
        Dim NewEntry As New View_Entry
        NewEntry.ViewEntry(GUID)
        NewEntry.Show()
    End Sub
    Private Sub AddNoteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddNoteToolStripMenuItem.Click
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
        cmbEquipType_View.Items.Clear()
        cmbEquipType_View.Text = ""
        For i = 0 To UBound(EquipType)
            cmbEquipType_View.Items.Insert(i, EquipType(i).strLong)
        Next
    End Sub
    Private Sub FillLocationCombo()
        Dim i As Integer
        cmbLocation_View.Items.Clear()
        cmbLocation_View.Text = ""
        For i = 0 To UBound(Locations)
            cmbLocation_View.Items.Insert(i, Locations(i).strLong)
        Next
    End Sub
    Private Sub FillOSTypeCombo()
        Dim i As Integer
        cmbOSVersion.Items.Clear()
        cmbOSVersion.Text = ""
        For i = 0 To UBound(OSType)
            cmbOSVersion.Items.Insert(i, OSType(i).strLong)
        Next
    End Sub
    Private Sub FillStatusTypeCombo()
        Dim i As Integer
        cmbStatus.Items.Clear()
        cmbStatus.Text = ""
        For i = 0 To UBound(StatusType)
            cmbStatus.Items.Insert(i, StatusType(i).strLong)
        Next
    End Sub
    Private Sub DeleteDeviceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteDeviceToolStripMenuItem.Click
        Dim blah = MsgBox("Are you absolutely sure?  This cannot be undone and will delete all histrical data.", vbYesNo + vbCritical, "WARNING")
        If blah = vbYes Then
            Dim blah2 = MsgBox(DeleteDevice(CurrentDevice.strGUID) & " rows affected.", vbOKOnly + vbInformation, "Deletion Results")
            Me.Hide()
        Else
            Exit Sub
        End If
    End Sub
End Class