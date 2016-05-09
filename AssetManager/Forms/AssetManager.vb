Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Imports System.DirectoryServices.AccountManagement
Imports System.Threading
Public Class AssetManager
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Logger("Starting AssetManager...")
        Status("Loading...")
        SplashScreen.Show()
        Status("Checking Server Connection...")
        If CheckConnection() Then
            'do nut-zing
        Else
            Dim blah = MsgBox("Error connecting to server!", vbOKOnly + vbCritical, "Could not connect")
            EndProgram()
        End If
        Dim userFullName As String = UserPrincipal.Current.DisplayName
        Logger("Enabling Double-Buffered DataGrid...")
        ExtendedMethods.DoubleBuffered(ResultGrid, True)
        Status("Loading Indexes...")
        BuildIndexes()
        Status("Checking Access Level...")
        GetUserAccess()
        Clear_All()
        CopyDefaultCellStyles()
        ViewFormIndex = 0
        Status("Loading devices...")
        ShowAll()
        Status("Ready!")
        Thread.Sleep(2000)
        SplashScreen.Hide()
        Me.Show()
    End Sub
    Public Sub Status(Text As String)
        SplashScreen.lblStatus.Text = Text
        SplashScreen.Refresh()
    End Sub
    Public Sub CopyDefaultCellStyles()
        View.DataGridHistory.DefaultCellStyle = ResultGrid.DefaultCellStyle
        View.DataGridHistory.BackgroundColor = ResultGrid.BackgroundColor
    End Sub
    Private Sub BuildIndexes()
        Logger("Building Indexes...")
        BuildLocationIndex()
        BuildChangeTypeIndex()
        BuildEquipTypeIndex()
        BuildOSTypeIndex()
        BuildStatusTypeIndex()
        Logger("Building Indexes Done...")
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
        EndProgram()
    End Sub
    Private SearchValues As Device_Info
    Private Sub BlahToolStripMenuItem_Click(sender As Object, e As EventArgs)
    End Sub
    Private Sub cmbShowAll_Click(sender As Object, e As EventArgs) Handles cmbShowAll.Click
        Waiting()
        CheckForIllegalCrossThreadCalls = False
        'Dim Thread1 As New Thread(AddressOf ShowAll)
        'Thread1.Start()
        ShowAll()
        DoneWaiting()
    End Sub
    Private Sub ShowAll()
        On Error GoTo errs
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim strQry = "SELECT * FROM devices ORDER BY dev_input_datetime DESC"
        strLastQry = strQry
        Dim cmd As New MySqlCommand(strQry, GetConnection(ConnID).DBConnection)
        reader = cmd.ExecuteReader
        With reader
            StatusBar(strCommMessage)
            Do While .Read()
                Dim Results As Device_Info
                Results.strCurrentUser = !dev_cur_user
                Results.strAssetTag = !dev_asset_tag
                Results.strSerial = !dev_serial
                Results.strDescription = !dev_description
                Results.strLocation = !dev_location
                Results.dtPurchaseDate = !dev_purchase_date
                Results.strGUID = !dev_UID
                Results.strEqType = !dev_eq_type
                AddToResults(Results)
            Loop
        End With
        SendToGrid(ResultGrid, SearchResults)
        CloseConnection(ConnID)
        Exit Sub
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Resume Next
        Else
            EndProgram()
        End If
    End Sub
    Private Sub SendToGrid(ByRef Grid As DataGridView, Data() As Device_Info)
        StatusBar(strLoadingGridMessage)
        Dim table As New DataTable
        Dim i As Integer
        table.Columns.Add("User", GetType(String))
        table.Columns.Add("Asset ID", GetType(String))
        table.Columns.Add("Serial", GetType(String))
        table.Columns.Add("Device Type", GetType(String))
        table.Columns.Add("Description", GetType(String))
        table.Columns.Add("Location", GetType(String))
        table.Columns.Add("Purchase Date", GetType(String))
        table.Columns.Add("GUID", GetType(String))
        For i = 1 To UBound(Data)
            table.Rows.Add(Data(i).strCurrentUser, Data(i).strAssetTag, Data(i).strSerial, GetHumanValue(ComboType.EquipType, Data(i).strEqType), Data(i).strDescription, GetHumanValue(ComboType.Location, Data(i).strLocation), Data(i).dtPurchaseDate, Data(i).strGUID)
        Next
        Grid.DataSource = table
        'For i = 0 To Grid.Rows.Count - 1
        '    If Grid.Rows(i).Cells(GetColIndex(Grid, "Device Type")).Value = "Desktop" Then
        '        Grid.Rows(i).Cells(GetColIndex(Grid, "Device Type")).Style.BackColor = Color.Blue
        '    End If
        'Next
        'Grid.Columns("User").DefaultCellStyle.Font = New Font(Grid.Font, FontStyle.Bold)
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
    Private Sub Button1_Click_1(sender As Object, e As EventArgs) Handles Button1.Click
        StartImport()
    End Sub
    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        Clear_All()
    End Sub
    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        Waiting()
        DynamicSearch()
        DoneWaiting()
    End Sub
    Private Sub DynamicSearch() 'dynamically creates sql query using any combination of search filters the users wants
        On Error GoTo errs
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim ConnID As String = Guid.NewGuid.ToString
        GetSearchDBValues()
        Dim strStartQry = "SELECT * FROM devices WHERE "
        Dim strDynaQry = (IIf(SearchValues.strSerial <> "", " dev_serial Like '" & SearchValues.strSerial & "%' AND", "")) & (IIf(SearchValues.strAssetTag <> "", " dev_asset_tag LIKE '%" & SearchValues.strAssetTag & "%' AND", "")) & (IIf(SearchValues.strEqType <> "", " dev_eq_type LIKE '%" & SearchValues.strEqType & "%' AND", "")) & (IIf(SearchValues.strCurrentUser <> "", " dev_cur_user LIKE '%" & SearchValues.strCurrentUser & "%' AND", "")) & (IIf(SearchValues.strLocation <> "", " dev_location LIKE '%" & SearchValues.strLocation & "%' AND", "")) & (IIf(SearchValues.strStatus <> "", " dev_status LIKE '%" & SearchValues.strStatus & "%' AND", ""))
        If strDynaQry = "" Then
            Dim blah = MsgBox("Please add some filter data.", vbOKOnly + vbInformation, "Fields Missing")
            Exit Sub
        End If
        Dim strQry = strStartQry & strDynaQry
        If Strings.Right(strQry, 3) = "AND" Then 'remove trailing AND from dynamic query
            strQry = Strings.Left(strQry, Strings.Len(strQry) - 3)
        End If
        strLastQry = strQry
        Dim cmd As New MySqlCommand(strQry, GetConnection(ConnID).DBConnection)
        reader = cmd.ExecuteReader
        With reader
            StatusBar(strCommMessage)
            Do While .Read()
                Dim Results As Device_Info
                Results.strCurrentUser = !dev_cur_user
                Results.strAssetTag = !dev_asset_tag
                Results.strSerial = !dev_serial
                Results.strDescription = !dev_description
                Results.strLocation = !dev_location
                Results.dtPurchaseDate = !dev_purchase_date
                Results.strGUID = !dev_UID
                Results.strEqType = !dev_eq_type
                AddToResults(Results)
            Loop
        End With
        SendToGrid(ResultGrid, SearchResults)
        CloseConnection(ConnID)
        Exit Sub
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Exit Sub
        Else
            EndProgram()
        End If
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Clear_All()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        AddNew.Show()
    End Sub
    Private Sub ResultGrid_DoubleClick(sender As Object, e As EventArgs) Handles ResultGrid.CellDoubleClick
        'Waiting()
        'View.ViewDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value)
        'View.Show()
        'View.Activate()
        'DoneWaiting()
        LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value)
    End Sub
    Private Sub LoadDevice(ByVal strGUID As String)
        Waiting()
        View.ViewDevice(strGUID)
        View.Show()
        View.Activate()
        DoneWaiting()
    End Sub
    Private Sub RefreshCombos()
        FillEquipTypeCombo()
        FillLocationCombo()
        FillChangeTypeCombo()
        FillStatusTypeCombo()
    End Sub
    Private Sub FillEquipTypeCombo()
        Dim i As Integer
        cmbEquipType.Items.Clear()
        cmbEquipType.Text = ""
        For i = 0 To UBound(EquipType)
            cmbEquipType.Items.Insert(i, EquipType(i).strLong)
        Next
    End Sub
    Private Sub FillLocationCombo()
        Dim i As Integer
        cmbLocation.Items.Clear()
        cmbLocation.Text = ""
        For i = 0 To UBound(Locations)
            cmbLocation.Items.Insert(i, Locations(i).strLong)
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
    Private Sub FillStatusTypeCombo()
        Dim i As Integer
        AddNew.cmbStatus_REQ.Items.Clear()
        AddNew.cmbStatus_REQ.Text = ""
        cmbStatus.Items.Clear()
        cmbStatus.Text = ""
        For i = 0 To UBound(StatusType)
            cmbStatus.Items.Insert(i, StatusType(i).strLong)
            AddNew.cmbStatus_REQ.Items.Insert(i, StatusType(i).strLong)
        Next
    End Sub
    Private Sub ResultGrid_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellContentClick
    End Sub
    Private Sub ViewSelectedToolStripMenuItem_Click(sender As Object, e As EventArgs)
        LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value)
    End Sub
    Private Sub ViewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewToolStripMenuItem.Click
        LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value)
    End Sub
    Public Sub Waiting()
        Me.Cursor = Cursors.WaitCursor
    End Sub
    Public Sub DoneWaiting()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
    End Sub
    Private Sub Button2_Click_1(sender As Object, e As EventArgs)
        Debug.Print(vbCrLf)
        Dim i As Integer
        For i = 0 To UBound(CurrentConnections)
            Debug.Print(i & " - " & CurrentConnections(i).ConnectionID & " = " & CurrentConnections(i).DBConnection.State)
        Next
    End Sub
    Private Sub Button2_Click_2(sender As Object, e As EventArgs) Handles Button2.Click
        For i As Integer = 0 To UBound(CurrentConnections)
            Debug.Print(CurrentConnections(i).ConnectionID & " - " & CurrentConnections(i).DBConnection.State)
        Next
    End Sub
    Private Sub YearsSincePurchaseToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles YearsSincePurchaseToolStripMenuItem.Click
        ReportView.Show()
    End Sub
    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles NewToolStripMenuItem.Click
        If Not CheckForAdmin() Then Exit Sub
        AddNew.Show()
    End Sub
    Private Sub ResultGrid_RowPostPaint(sender As Object, e As DataGridViewRowPostPaintEventArgs) Handles ResultGrid.RowPostPaint
    End Sub
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As DoWorkEventArgs) Handles QueryWorker.DoWork
    End Sub
    Private Sub ResultGrid_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles ResultGrid.CellMouseDown
        If e.Button = MouseButtons.Right Then
            ResultGrid.CurrentCell = ResultGrid(e.ColumnIndex, e.RowIndex)
        End If
    End Sub
    Private Sub ContextMenuStrip1_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip1.Opening
    End Sub
End Class
