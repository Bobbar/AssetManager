Option Explicit On
Imports MySql.Data.MySqlClient
Public Class AddNew
    Private Device As Device_Info
    Private Sub cmdAdd_Click(sender As Object, e As EventArgs) Handles cmdAdd.Click
        GetDBValues()
        cn_global.Open()
        Dim strSqlQry1 = "INSERT INTO devices (dev_description,dev_location,dev_cur_user,dev_serial,dev_asset_tag,dev_purchase_date,dev_replacement_year,dev_eq_type,dev_osversion,dev_status,dev_lastmod_user) VALUES ('" & Device.strDescription & "','" & Device.strLocation & "','" & Device.strCurrentUser & "','" & Device.strSerial & "','" & Device.strAssetTag & "','" & Device.dtPurchaseDate & "','" & Device.strReplaceYear & "','" & Device.strEqType & "','" & Device.strOSVersion & "','" & Device.strStatus & "','" & strLocalUser & "')"
        Debug.Print(strSqlQry1)
        Dim cmd As New MySqlCommand
        cmd.Connection = cn_global
        cmd.CommandText = strSqlQry1
        cmd.ExecuteNonQuery()
        Dim strSqlQry2 = "INSERT INTO historical (hist_change_type, hist_notes, hist_serial, hist_description, hist_location, hist_cur_user, hist_asset_tag, hist_purchase_date, hist_replacement_year, hist_po, hist_osversion, hist_dev_UID, hist_action_user, hist_eq_type, hist_status) VALUES ('NEWD','" & Device.strNote & "','" & Device.strSerial & "','" & Device.strDescription & "','" & Device.strLocation & "','" & Device.strCurrentUser & "','" & Device.strAssetTag & "','" & Device.dtPurchaseDate & "','" & Device.strReplaceYear & "','" & Device.strPO & "','" & Device.strOSVersion & "','" & GetDeviceUID(Device.strAssetTag) & "','" & strLocalUser & "','" & Device.strEqType & "','" & Device.strStatus & "')"
        cmd.CommandText = strSqlQry2
        cmd.ExecuteNonQuery()
        cn_global.Close()
        Dim blah = MsgBox("New Device Added", vbOKOnly, "Complete")
        ClearAll()
    End Sub
    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        ClearAll()
    End Sub
    Private Sub GetDBValues() 'cleanup user input for db
        Device.strSerial = Trim(txtSerial.Text)
        Device.strDescription = Trim(txtDescription.Text)
        Device.strAssetTag = Trim(txtAssetTag.Text)
        'strPurchaseDate = Format(dtPurchaseDate.Text, strDBDateFormat)
        Device.dtPurchaseDate = dtPurchaseDate.Text
        Device.strReplaceYear = Trim(txtReplaceYear.Text)
        Device.strLocation = GetDBValue(ComboType.Location, cmbLocation.SelectedIndex)
        Device.strCurrentUser = Trim(txtCurUser.Text)
        Device.strNote = Trim(txtNotes.Text)
        Device.strOSVersion = GetDBValue(ComboType.OSType, cmbOSType.SelectedIndex)
        Device.strEqType = GetDBValue(ComboType.EquipType, cmbEquipType.SelectedIndex)
        Device.strStatus = GetDBValue(ComboType.StatusType, cmbStatus.SelectedIndex)
        'strPO =
        'strOSVersion =
    End Sub
    Private Sub cmbLocation_SelectedIndexChanged(sender As Object, e As EventArgs)
    End Sub
    Private Sub AddNew_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClearAll()
    End Sub
    Private Sub ClearAll()
        LoadCombos()
        FillEquipTypeCombo()
        FillOSTypeCombo()
        FillStatusTypeCombo()
        ClearFields()
    End Sub
    Private Sub ClearFields()
        Dim c As Control
        For Each c In GroupBox1.Controls
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
    Private Sub LoadCombos()
        Dim i As Integer
        cmbLocation.Items.Clear()
        cmbLocation.Text = ""
        For i = 0 To UBound(Locations)
            cmbLocation.Items.Insert(i, Locations(i).strLong)
        Next
    End Sub
    Private Sub FillEquipTypeCombo()
        Dim i As Integer
        cmbEquipType.Items.Clear()
        cmbEquipType.Text = ""
        For i = 0 To UBound(EquipType)
            cmbEquipType.Items.Insert(i, EquipType(i).strLong)
        Next
    End Sub
    Private Sub FillOSTypeCombo()
        Dim i As Integer
        cmbOSType.Items.Clear()
        cmbOSType.Text = ""
        For i = 0 To UBound(EquipType)
            cmbOSType.Items.Insert(i, OSType(i).strLong)
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
    Private Sub AddNew_Activated(sender As Object, e As EventArgs) Handles Me.Activated
    End Sub
    Private Sub AddNew_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
    End Sub
End Class
