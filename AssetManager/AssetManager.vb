Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Imports System.DirectoryServices.AccountManagement
Public Class AssetManager
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'DBConnect()
        Dim userFullName As String = UserPrincipal.Current.DisplayName
        Debug.Print(userFullName)
        BuildLocationIndex()
        BuildChangeTypeIndex()
        BuildEquipTypeIndex()
        BuildOSTypeIndex()
        Clear_All()
        ViewFormIndex = 0

        'View.Show()
    End Sub
    Private Sub Clear_All()
        txtAssetTag.Clear()
        txtSerial.Clear()
        cmbEquipType.Items.Clear()
        cmbLocation.Items.Clear()
        txtCurUser.Clear()
        FillLocationCombo()
        FillEquipTypeCombo()
        FillOSTypeCombo()
        FillChangeTypeCombo()
        ResultGrid.DataSource = Nothing
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        cn_global.Close()
    End Sub
    Private strSerial As String, strDescription As String, strAssetTag As String, strPurchaseDate As String, strReplacementYear As String,
        strPO As String, strOSVersion As String, strLocation As String, strCurUser As String, strNotes As String, strEQType As String
    Private Sub BlahToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BlahToolStripMenuItem.Click
        AddNew.Show()
    End Sub
    Private Sub cmbShowAll_Click(sender As Object, e As EventArgs) Handles cmbShowAll.Click
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        cn_global.Open()
        'GetSearchDBValues()
        Dim strQry = "SELECT * FROM devices"
        Dim cmd As New MySqlCommand(strQry, cn_global)
        Dim Results As Device_Info

        reader = cmd.ExecuteReader
        'table.Columns.Add("User", GetType(String))
        'table.Columns.Add("Asset ID", GetType(String))
        'table.Columns.Add("Serial", GetType(String))
        'table.Columns.Add("Description", GetType(String))
        'table.Columns.Add("Location", GetType(String))
        'table.Columns.Add("Purchase Date", GetType(String))
        'table.Columns.Add("Device UID", GetType(String))
        With reader
            Do While .Read()

                'table.Rows.Add(!dev_cur_user,!dev_asset_tag,!dev_serial,!dev_description,!dev_location,!dev_purchase_date,!dev_UID)
                Results.strCurrentUser = !dev_



            Loop
        End With
        'ResultGrid.DataSource = table
        cn_global.Close()
    End Sub
    Private Sub SendToGrid(ByRef Grid As DataGridView, Data() As Device_Info)
        Dim table As New DataTable
        Dim i As Integer

        table.Columns.Add("User", GetType(String))
        table.Columns.Add("Asset ID", GetType(String))
        table.Columns.Add("Serial", GetType(String))
        table.Columns.Add("Description", GetType(String))
        table.Columns.Add("Location", GetType(String))
        table.Columns.Add("Purchase Date", GetType(String))
        table.Columns.Add("Device UID", GetType(String))

        For i = 0 To UBound(Data)

            table.Rows.Add(Data(i).strCurrentUser, Data(i).strAssetTag, Data(i).strSerial, Data(i).strDescription, Data(i).strLocation, Data(i).dtPurchaseDate, Data(i).strGUID)



        Next
        Grid.DataSource = table



    End Sub
    Private Sub GetSearchDBValues() 'cleanup user input for db
        strSerial = Trim(txtSerial.Text)
        'strDescription = Trim(txtDescription.Text)
        strAssetTag = Trim(txtAssetTag.Text)
        'strPurchaseDate = Format(dtPurchaseDate.Text, strDBDateFormat)
        'strPurchaseDate = dtPurchaseDate.Text
        strEQType = GetDBValue(ComboType.EquipType, cmbEquipType.SelectedIndex)
        'strReplacementYear = Trim(txtReplaceYear.Text)
        strLocation = GetDBValue(ComboType.Location, cmbLocation.SelectedIndex)
        strCurUser = Trim(txtCurUser.Text)
        'strNotes = Trim(txtNotes.Text)
        'strPO =
        'strOSVersion =
    End Sub
    Private Sub ResultGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellContentClick
    End Sub

    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        StartImport()

    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        Clear_All()
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
        table.Columns.Add("Device UID", GetType(String))
        With reader
            Do While .Read()
                Debug.Print(!dev_description)
                table.Rows.Add(!dev_cur_user,!dev_asset_tag,!dev_serial,!dev_description,!dev_location,!dev_purchase_date,!dev_UID)
            Loop
        End With
        ResultGrid.DataSource = table
        cn_global.Close()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Clear_All()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        AddNew.Show()
    End Sub
    Private Sub ResultGrid_DoubleClick(sender As Object, e As EventArgs) Handles ResultGrid.CellDoubleClick
        Debug.Print("Double CLICK!")
        'Debug.Print(ResultGrid.Item(6, ResultGrid.CurrentRow.Index).Value)
        View.ViewDevice(ResultGrid.Item(6, ResultGrid.CurrentRow.Index).Value)

        View.Show()
    End Sub



    Public Sub RefreshCombos()
        FillEquipTypeCombo()
        FillLocationCombo()
        FillChangeTypeCombo()
        FillOSTypeCombo()
    End Sub
    Private Sub FillEquipTypeCombo()
        Dim i As Integer
        View.cmbEquipType_View.Items.Clear()
        cmbEquipType.Items.Clear()
        View.cmbEquipType_View.Text = ""
        cmbEquipType.Text = ""
        For i = 0 To UBound(EquipType)
            View.cmbEquipType_View.Items.Insert(i, EquipType(i).strLong)
            cmbEquipType.Items.Insert(i, EquipType(i).strLong)
        Next
    End Sub
    Private Sub FillLocationCombo()
        Dim i As Integer
        cmbLocation.Items.Clear()
        View.cmbLocation_View.Items.Clear()
        cmbLocation.Text = ""
        View.cmbLocation_View.Text = ""
        For i = 0 To UBound(Locations)
            cmbLocation.Items.Insert(i, Locations(i).strLong)
            View.cmbLocation_View.Items.Insert(i, Locations(i).strLong)
        Next
    End Sub
    Private Sub FillChangeTypeCombo()
        Dim i As Integer
        UpdateDev.cmbUpdate_ChangeType.Items.Clear()
        UpdateDev.cmbUpdate_ChangeType.Text = ""
        For i = 0 To UBound(ChangeType)
            UpdateDev.cmbUpdate_ChangeType.Items.Insert(i, ChangeType(i).strLong)
        Next
    End Sub
    Private Sub FillOSTypeCombo()
        Dim i As Integer
        View.cmbOSVersion.Items.Clear()
        View.cmbOSVersion.Text = ""
        For i = 0 To UBound(OSType)
            View.cmbOSVersion.Items.Insert(i, OSType(i).strLong)
        Next
    End Sub
End Class
