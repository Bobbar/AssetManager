Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class Form1
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'DBConnect()
        BuildLocationIndex()
        BuildChangeTypeIndex()
        BuildEquipTypeIndex()
        BuildOSTypeIndex()
        Clear_All()
    End Sub
    Private Sub Clear_All()
        txtAssetTag.Clear()
        txtSerial.Clear()
        cmbEquipType.Items.Clear()
        cmbLocation.Items.Clear()
        txtCurUser.Clear()
        FillLocationCombo()
        FillEquipTypeCombo()
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        cn_global.Close()
    End Sub
    Private strSerial As String, strDescription As String, strAssetTag As String, strPurchaseDate As String, strReplacementYear As String,
        strPO As String, strOSVersion As String, strLocation As String, strCurUser As String, strNotes As String, strEQType As String
    Private Sub cmbEquipType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEquipType.SelectedIndexChanged
    End Sub
    Private Sub DataGridHistory_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridHistory.CellContentClick
    End Sub
    Private Sub GetSearchDBValues() 'cleanup user input for db
        strSerial = Trim(txtSerial.Text)
        'strDescription = Trim(txtDescription.Text)
        strAssetTag = Trim(txtAssetTag.Text)
        'strPurchaseDate = Format(dtPurchaseDate.Text, strDBDateFormat)
        'strPurchaseDate = dtPurchaseDate.Text
        strEQType = GetCMBShortValue(ComboType.EquipType, cmbEquipType.SelectedIndex)
        'strReplacementYear = Trim(txtReplaceYear.Text)
        strLocation = GetCMBShortValue(ComboType.Location, cmbLocation.SelectedIndex)
        strCurUser = Trim(txtCurUser.Text)
        'strNotes = Trim(txtNotes.Text)
        'strPO =
        'strOSVersion =
    End Sub
    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        'Dim cmd As New MySqlCommand
        'Dim ds As New DataSet
        'Dim adapter As New MySqlDataAdapter(strGetDevices, cn_global)
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        cn_global.Open()
        GetSearchDBValues()
        Dim strQry = "SELECT * FROM devices WHERE " & (IIf(strSerial <> "", " dev_serial Like '" & strSerial & "%' AND", "")) & (IIf(strAssetTag <> "", " dev_asset_tag LIKE '%" & strAssetTag & "%' AND", "")) & (IIf(strEQType <> "", " dev_eq_type LIKE '%" & strEQType & "%' AND", "")) & (IIf(strCurUser <> "", " dev_cur_user LIKE '%" & strCurUser & "%' AND", "")) & (IIf(strLocation <> "", " dev_location LIKE '%" & strLocation & "%' AND", ""))
        If Strings.Right(strQry, 3) = "AND" Then 'remove trailing AND from dynamic query
            strQry = Strings.Left(strQry, Strings.Len(strQry) - 3)
        End If
        Debug.Print(strQry)
        Dim cmd As New MySqlCommand(strQry, cn_global)
        reader = cmd.ExecuteReader
        table.Columns.Add("User", GetType(String))
        table.Columns.Add("Asset ID", GetType(String))
        table.Columns.Add("Serial", GetType(String))
        table.Columns.Add("Description", GetType(String))
        table.Columns.Add("Location", GetType(String))
        table.Columns.Add("Purchase Date", GetType(String))
        With reader
            Do While .Read()
                Debug.Print(!dev_description)
                table.Rows.Add(!dev_cur_user,!dev_asset_tag,!dev_serial,!dev_description,!dev_location,!dev_purchase_date)
            Loop
        End With
        ResultGrid.DataSource = table
        cn_global.Close()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        Clear_All()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        AddNew.Show()
    End Sub
    Private Sub ResultGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellContentClick
    End Sub
    Private Sub ResultGrid_DoubleClick(sender As Object, e As EventArgs) Handles ResultGrid.DoubleClick
        Debug.Print("Double CLICK!")
        Debug.Print(ResultGrid.Item(1, ResultGrid.CurrentRow.Index).Value)
        ViewDevice(ResultGrid.Item(1, ResultGrid.CurrentRow.Index).Value)
    End Sub
    Private Sub ViewDevice(ByVal strAssetTag As String)
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        cn_global.Open()
        Dim strQry = "SELECT * FROM devices, historical WHERE dev_UID = hist_dev_UID AND dev_asset_tag = '" & strAssetTag & "'"
        Debug.Print(strQry)
        Dim cmd As New MySqlCommand(strQry, cn_global)
        reader = cmd.ExecuteReader
        table.Columns.Add("Action Type", GetType(String))
        table.Columns.Add("User", GetType(String))
        table.Columns.Add("Asset ID", GetType(String))
        table.Columns.Add("Serial", GetType(String))
        table.Columns.Add("Description", GetType(String))
        table.Columns.Add("Location", GetType(String))
        table.Columns.Add("Purchase Date", GetType(String))
        With reader
            Do While .Read()
                Debug.Print(!dev_description)
                txtAssetTag_View.Text = !dev_asset_tag
                txtDescription_View.Text = !dev_description
                cmbEquipType_View.SelectedIndex = GetComboIndexFromShort(ComboType.EquipType,!dev_eq_type)
                txtSerial_View.Text = !dev_serial
                cmbLocation_View.SelectedIndex = GetComboIndexFromShort(ComboType.Location,!dev_location)
                txtCurUser_View.Text = !dev_cur_user
                dtPurchaseDate_View.Value = !dev_purchase_date
                txtReplacementYear_View.Text = !dev_replacement_year
                table.Rows.Add(GetHumanValue(ComboType.ChangeType,!hist_change_type),!dev_cur_user,!dev_asset_tag,!dev_serial,!dev_description,!dev_location,!dev_purchase_date)
            Loop
        End With
        DataGridHistory.DataSource = table
        cn_global.Close()
    End Sub
    Private Sub FillEquipTypeCombo()
        Dim i As Integer
        cmbEquipType_View.Items.Clear()
        cmbEquipType.Items.Clear()
        cmbEquipType_View.Text = ""
        cmbEquipType.Text = ""
        For i = 0 To UBound(EquipType)
            cmbEquipType_View.Items.Insert(i, EquipType(i).strLong)
            cmbEquipType.Items.Insert(i, EquipType(i).strLong)
        Next
    End Sub
    Private Sub FillLocationCombo()
        Dim i As Integer
        cmbLocation.Items.Clear()
        cmbLocation_View.Items.Clear()
        cmbLocation.Text = ""
        cmbLocation_View.Text = ""
        For i = 0 To UBound(Locations)
            cmbLocation.Items.Insert(i, Locations(i).strLong)
            cmbLocation_View.Items.Insert(i, Locations(i).strLong)
        Next
    End Sub
End Class
