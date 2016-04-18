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
    Private NewData As Device_Info
    Private Sub View_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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
    Private Sub GetNewValues()
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
        End With
    End Sub
    Private Sub EnableControls()
        Dim c As Control
        For Each c In pnlViewControls.Controls
            If TypeOf c IsNot Label Then c.Enabled = True
        Next
        cmdUpdate.Visible = True
    End Sub
    Private Sub DisableControls()
        Dim c As Control
        For Each c In pnlViewControls.Controls
            If TypeOf c IsNot Label Then c.Enabled = False
        Next
        cmdUpdate.Visible = False
    End Sub
    Public Sub ViewDevice(ByVal DeviceUID As String)
        AssetManager.RefreshCombos()
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
                table.Rows.Add(!hist_action_datetime, GetHumanValue(ComboType.ChangeType,!hist_change_type),!hist_cur_user,!hist_asset_tag,!hist_serial,!hist_description,!hist_location,!hist_purchase_date,!hist_uid)
            Loop
        End With
        cn_global.Close()
        DataGridHistory.DataSource = table
        DataGridHistory.Columns("Action Type").DefaultCellStyle.Font = New Font(DataGridHistory.Font, FontStyle.Bold)
        DisableControls()
    End Sub
    Private Sub ModifyDevice()
        GetCurrentValues()
        EnableControls()
    End Sub
    Public Sub UpdateDevice()
        Dim strNote As String = Trim(UpdateDev.txtUpdate_Note.Text)
        cn_global.Open()
        Dim strSQLQry1 = "UPDATE devices set dev_description='" & NewData.strDescription & "', dev_location='" & NewData.strLocation & "', dev_cur_user='" & NewData.strCurrentUser & "', dev_serial='" & NewData.strSerial & "', dev_asset_tag='" & NewData.strAssetTag & "', dev_purchase_date='" & NewData.dtPurchaseDate & "', dev_replacement_year='" & NewData.strReplaceYear & "', dev_osversion='" & NewData.strOSVersion & "', dev_eq_type='" & NewData.strEqType & "', dev_status='" & NewData.strStatus & "' WHERE dev_UID='" & CurrentDevice.strGUID & "'"
        Dim cmd As New MySqlCommand
        cmd.Connection = cn_global
        cmd.CommandText = strSQLQry1
        cmd.ExecuteNonQuery()
        Dim strSqlQry2 = "INSERT INTO historical (hist_change_type,hist_notes,hist_serial,hist_description,hist_location,hist_cur_user,hist_asset_tag,hist_purchase_date,hist_replacement_year,hist_osversion,hist_dev_UID,hist_action_user,hist_eq_type,hist_status) VALUES ('" & GetDBValue(ComboType.ChangeType, UpdateDev.cmbUpdate_ChangeType.SelectedIndex) & "','" & strNote & "','" & NewData.strSerial & "','" & NewData.strDescription & "','" & NewData.strLocation & "','" & NewData.strCurrentUser & "','" & NewData.strAssetTag & "','" & NewData.dtPurchaseDate & "','" & NewData.strReplaceYear & "','" & NewData.strOSVersion & "','" & CurrentDevice.strGUID & "','" & strLocalUser & "','" & NewData.strEqType & "','" & NewData.strStatus & "')"
        cmd.CommandText = strSqlQry2
        cmd.ExecuteNonQuery()
        cn_global.Close()
        Dim blah
        blah = MsgBox("Update Added", vbOKOnly, "Complete")
        ViewDevice(CurrentDevice.strGUID)
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
        GetNewValues()
        UpdateDev.Show()
    End Sub
    Private Sub View_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.Hide()
    End Sub
    Private Sub DataGridHistory_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridHistory.CellContentClick
    End Sub
    Private Sub DataGridHistory_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridHistory.CellDoubleClick
        NewEntryView(DataGridHistory.Item(8, DataGridHistory.CurrentRow.Index).Value)
    End Sub
    Private Sub NewEntryView(GUID As String)
        Dim NewEntry As New View_Entry
        NewEntry.ViewEntry(GUID)
        NewEntry.Text = NewEntry.Text + " - " & CurrentDevice.strAssetTag
        NewEntry.Show()
    End Sub
End Class