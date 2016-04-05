Option Explicit On
Imports MySql.Data.MySqlClient
Public Class AddNew
    Private strSerial As String, strDescription As String, strAssetTag As String, strPurchaseDate As String, strReplacementYear As String,
        strPO As String, strOSVersion As String, strLocation As String, strCurUser As String, strNotes As String, strOSType As String, strEQType As String
    Private Sub cmdAdd_Click(sender As Object, e As EventArgs)
        GetDBValues()
        cn_global.Open()
        Dim strSqlQry1 = "INSERT INTO devices (dev_description,dev_location,dev_cur_user,dev_serial,dev_asset_tag,dev_purchase_date,dev_replacement_year,dev_eq_type) VALUES ('" & strDescription & "','" & strLocation & "','" & strCurUser & "','" & strSerial & "','" & strAssetTag & "','" & strPurchaseDate & "','" & strReplacementYear & "','" & strEQType & "')"
        Debug.Print(strSqlQry1)
        Dim cmd As New MySqlCommand
        cmd.Connection = cn_global
        cmd.CommandText = strSqlQry1
        cmd.ExecuteNonQuery()
        Dim strSqlQry2 = "INSERT INTO historical (hist_change_type,hist_notes,hist_serial,hist_description,hist_location,hist_cur_user,hist_asset_tag,hist_purchase_date,hist_replacement_year,hist_po,hist_osversion,hist_dev_UID) VALUES ('NEWD','" & strNotes & "','" & strSerial & "','" & strDescription & "','" & strLocation & "','" & strCurUser & "','" & strAssetTag & "','" & strPurchaseDate & "','" & strReplacementYear & "','" & strPO & "','" & strOSVersion & "','" & GetDeviceUID(strAssetTag) & "')"
        cmd.CommandText = strSqlQry2
        cmd.ExecuteNonQuery()
        cn_global.Close()
    End Sub
    Private Sub GetDBValues() 'cleanup user input for db
        strSerial = Trim(txtSerial.Text)
        strDescription = Trim(txtDescription.Text)
        strAssetTag = Trim(txtAssetTag.Text)
        'strPurchaseDate = Format(dtPurchaseDate.Text, strDBDateFormat)
        strPurchaseDate = dtPurchaseDate.Text
        Debug.Print(strPurchaseDate)
        strReplacementYear = Trim(txtReplaceYear.Text)
        strLocation = GetCMBShortValue(ComboType.Location, cmbLocation.SelectedIndex)
        strCurUser = Trim(txtCurUser.Text)
        strNotes = Trim(txtNotes.Text)
        strOSType = GetCMBShortValue(ComboType.OSType, cmbOSType.SelectedIndex)
        strEQType = GetCMBShortValue(ComboType.EquipType, cmbEquipType.SelectedIndex)
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
        txtSerial.Text = ""
        txtCurUser.Text = ""
        txtReplaceYear.Text = ""
        txtAssetTag.Text = ""
        txtDescription.Text = ""
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
    Private Sub AddNew_Activated(sender As Object, e As EventArgs) Handles Me.Activated
    End Sub
    Private Sub AddNew_HelpRequested(sender As Object, hlpevent As HelpEventArgs) Handles Me.HelpRequested
    End Sub
End Class
