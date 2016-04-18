Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Imports System.DirectoryServices.AccountManagement
Public Class AssetManager
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim userFullName As String = UserPrincipal.Current.DisplayName
        Debug.Print(userFullName)
        BuildIndexes()
        Clear_All()
        ViewFormIndex = 0
    End Sub
    Private Sub BuildIndexes()
        BuildLocationIndex()
        BuildChangeTypeIndex()
        BuildEquipTypeIndex()
        BuildOSTypeIndex()
        BuildStatusTypeIndex()
    End Sub
    Private Sub Clear_All()
        txtAssetTag.Clear()
        txtSerial.Clear()
        cmbEquipType.Items.Clear()
        cmbLocation.Items.Clear()
        txtCurUser.Clear()
        RefreshCombos()
        ReDim SearchResults(0)
        ResultGrid.DataSource = Nothing
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        cn_global.Close()
    End Sub
    Private SearchValues As Device_Info
    Private Sub BlahToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BlahToolStripMenuItem.Click
        AddNew.Show()
    End Sub
    Private Sub cmbShowAll_Click(sender As Object, e As EventArgs) Handles cmbShowAll.Click
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        cn_global.Open()
        Dim strQry = "SELECT * FROM devices"
        Dim cmd As New MySqlCommand(strQry, cn_global)
        reader = cmd.ExecuteReader
        With reader
            Do While .Read()
                Dim Results As Device_Info
                Results.strCurrentUser = !dev_cur_user
                Results.strAssetTag = !dev_asset_tag
                Results.strSerial = !dev_serial
                Results.strDescription = !dev_description
                Results.strLocation = !dev_location
                Results.dtPurchaseDate = !dev_purchase_date
                Results.strGUID = !dev_UID
                AddToResults(Results)
            Loop
        End With
        SendToGrid(ResultGrid, SearchResults)
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
        For i = 1 To UBound(Data)
            table.Rows.Add(Data(i).strCurrentUser, Data(i).strAssetTag, Data(i).strSerial, Data(i).strDescription, GetHumanValue(ComboType.Location, Data(i).strLocation), Data(i).dtPurchaseDate, Data(i).strGUID)
        Next
        Grid.DataSource = table
        Grid.Columns("User").DefaultCellStyle.Font = New Font(Grid.Font, FontStyle.Bold)
        Grid.AutoResizeColumns()

        ReDim SearchResults(0)
    End Sub
    Private Sub GetSearchDBValues() 'cleanup user input for db
        SearchValues.strSerial = Trim(txtSerial.Text)
        'strDescription = Trim(txtDescription.Text)
        SearchValues.strAssetTag = Trim(txtAssetTag.Text)
        'strPurchaseDate = Format(dtPurchaseDate.Text, strDBDateFormat)
        'strPurchaseDate = dtPurchaseDate.Text
        SearchValues.strEqType = GetDBValue(ComboType.EquipType, cmbEquipType.SelectedIndex)
        'strReplacementYear = Trim(txtReplaceYear.Text)
        SearchValues.strLocation = GetDBValue(ComboType.Location, cmbLocation.SelectedIndex)
        SearchValues.strCurrentUser = Trim(txtCurUser.Text)
        SearchValues.strStatus = GetDBValue(ComboType.StatusType, cmbStatus.SelectedIndex)
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
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim ds As DataSet

        GetSearchDBValues()

        Dim strStartQry = "SELECT * FROM devices WHERE " '& (IIf(SearchValues.strSerial <> "", " dev_serial Like '" & SearchValues.strSerial & "%' AND", "")) & (IIf(SearchValues.strAssetTag <> "", " dev_asset_tag LIKE '%" & SearchValues.strAssetTag & "%' AND", "")) & (IIf(SearchValues.strEqType <> "", " dev_eq_type LIKE '%" & SearchValues.strEqType & "%' AND", "")) & (IIf(SearchValues.strCurrentUser <> "", " dev_cur_user LIKE '%" & SearchValues.strCurrentUser & "%' AND", "")) & (IIf(SearchValues.strLocation <> "", " dev_location LIKE '%" & SearchValues.strLocation & "%' AND", "")) & (IIf(SearchValues.strStatus <> "", " dev_status LIKE '%" & SearchValues.strStatus & "%' AND", ""))
        Dim strDynaQry = (IIf(SearchValues.strSerial <> "", " dev_serial Like '" & SearchValues.strSerial & "%' AND", "")) & (IIf(SearchValues.strAssetTag <> "", " dev_asset_tag LIKE '%" & SearchValues.strAssetTag & "%' AND", "")) & (IIf(SearchValues.strEqType <> "", " dev_eq_type LIKE '%" & SearchValues.strEqType & "%' AND", "")) & (IIf(SearchValues.strCurrentUser <> "", " dev_cur_user LIKE '%" & SearchValues.strCurrentUser & "%' AND", "")) & (IIf(SearchValues.strLocation <> "", " dev_location LIKE '%" & SearchValues.strLocation & "%' AND", "")) & (IIf(SearchValues.strStatus <> "", " dev_status LIKE '%" & SearchValues.strStatus & "%' AND", ""))

        If strDynaQry = "" Then
            Dim blah = MsgBox("Please add some filter data.", vbOKOnly + vbInformation, "Fields Missing")
            Exit Sub
        End If
        Dim strQry = strStartQry & strDynaQry

        If Strings.Right(strQry, 3) = "AND" Then 'remove trailing AND from dynamic query
            strQry = Strings.Left(strQry, Strings.Len(strQry) - 3)
        End If
        Debug.Print(strQry)
        cn_global.Open()
        Dim cmd As New MySqlCommand(strQry, cn_global)
        reader = cmd.ExecuteReader
        With reader

            Do While .Read()
                Dim Results As Device_Info
                Results.strCurrentUser = !dev_cur_user
                Results.strAssetTag = !dev_asset_tag
                Results.strSerial = !dev_serial
                Results.strDescription = !dev_description
                Results.strLocation = !dev_location
                Results.dtPurchaseDate = !dev_purchase_date
                Results.strGUID = !dev_UID
                AddToResults(Results)
            Loop

        End With
        SendToGrid(ResultGrid, SearchResults)
        cn_global.Close()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Clear_All()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        AddNew.Show()
    End Sub
    Private Sub ResultGrid_DoubleClick(sender As Object, e As EventArgs) Handles ResultGrid.CellDoubleClick
        View.ViewDevice(ResultGrid.Item(6, ResultGrid.CurrentRow.Index).Value)
        View.Show()
    End Sub
    Public Sub RefreshCombos()
        FillEquipTypeCombo()
        FillLocationCombo()
        FillChangeTypeCombo()
        FillOSTypeCombo()
        FillStatusTypeCombo()
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
    Private Sub FillStatusTypeCombo()
        Dim i As Integer
        View.cmbStatus.Items.Clear()
        View.cmbStatus.Text = ""
        cmbStatus.Items.Clear()
        cmbStatus.Text = ""
        For i = 0 To UBound(StatusType)
            View.cmbStatus.Items.Insert(i, StatusType(i).strLong)
            cmbStatus.Items.Insert(i, StatusType(i).strLong)
        Next
    End Sub
End Class
