Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Imports System.DirectoryServices.AccountManagement
Imports System.Threading
Public Class AssetManager
    Private strSearchString As String, strPrevSearchString As String
    'Private SearchResults() As String
    Private CurrentControl As Control
    Private strWorkerQry As String
    Private Const strShowAllQry As String = "SELECT * FROM devices ORDER BY dev_input_datetime DESC"
    Private ClickedButton As Control, ClickedButtonPrevText As String
    Dim dtResults As New DataTable
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
        GetGridStylez()
        CopyDefaultCellStyles()
        ViewFormIndex = 0
        Status("Loading devices...")
        StartBigQuery(strShowAllQry)
        'ShowAll()
        Status("Ready!")
        Thread.Sleep(1000)
        SplashScreen.Hide()
        Me.Show()
        'Tracking.Show()
    End Sub
    Public Sub GetGridStylez()
        Dim tmpStyle As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        tmpStyle.Alignment = ResultGrid.DefaultCellStyle.Alignment
        tmpStyle.BackColor = ResultGrid.DefaultCellStyle.BackColor
        tmpStyle.Font = ResultGrid.DefaultCellStyle.Font
        tmpStyle.ForeColor = ResultGrid.DefaultCellStyle.ForeColor
        tmpStyle.SelectionBackColor = ResultGrid.DefaultCellStyle.SelectionBackColor
        tmpStyle.SelectionForeColor = ResultGrid.DefaultCellStyle.SelectionForeColor
        tmpStyle.WrapMode = ResultGrid.DefaultCellStyle.WrapMode
        'Me.ResultGrid.DefaultCellStyle = DataGridViewCellStyle2
        GridStylez = tmpStyle
    End Sub
    Public Sub Status(Text As String)
        SplashScreen.lblStatus.Text = Text
        SplashScreen.Refresh()
    End Sub
    Public Sub CopyDefaultCellStyles()
        'View.DataGridHistory.DefaultCellStyle = ResultGrid.DefaultCellStyle
        View.DataGridHistory.BackgroundColor = ResultGrid.BackgroundColor
        'View.TrackingGrid.DefaultCellStyle = ResultGrid.DefaultCellStyle
        View.TrackingGrid.BackgroundColor = ResultGrid.BackgroundColor
        View.DataGridHistory.DefaultCellStyle = GridStylez
        View.DataGridHistory.DefaultCellStyle.Font = GridFont
        View.TrackingGrid.DefaultCellStyle = GridStylez
        View.TrackingGrid.DefaultCellStyle.Font = GridFont
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
        LiveBox.Items.Clear()
        LiveBox.Visible = False
        txtAssetTag.Clear()
        txtAssetTagSearch.Clear()
        txtSerial.Clear()
        txtSerialSearch.Clear()
        cmbEquipType.Items.Clear()
        cmbLocation.Items.Clear()
        txtCurUser.Clear()
        txtDescription.Clear()
        RefreshCombos()
        ReDim SearchResults(0)
        '  ResultGrid.DataSource = Nothing
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        EndProgram()
    End Sub
    Private SearchValues As Device_Info
    Private Sub BlahToolStripMenuItem_Click(sender As Object, e As EventArgs)
    End Sub
    Private Sub cmdShowAll_Click(sender As Object, e As EventArgs) Handles cmdShowAll.Click
        If Not BigQueryWorker.IsBusy Then
            ClickedButton = cmdShowAll
            'ShowAll()
            StartBigQuery(strShowAllQry)
        End If
    End Sub
    Private Sub StartBigQuery(strQry As String)
        If Not BigQueryWorker.IsBusy Then
            strWorkerQry = strQry
            If ClickedButton IsNot Nothing Then
                ClickedButtonPrevText = ClickedButton.Text
                ClickedButton.Enabled = False
                ClickedButton.Text = "Working..."
            End If
            StatusBar("Request sent to background...")
            'picRunning.Visible = True
            StripSpinner.Visible = True
            BigQueryWorker.RunWorkerAsync()
        End If
    End Sub
    Private Sub BigQueryDone()
        SendToGrid(ResultGrid, SearchResults)
        StripSpinner.Visible = False
        If ClickedButton IsNot Nothing Then
            ClickedButton.Enabled = True
            ClickedButton.Text = ClickedButtonPrevText
            ClickedButton = Nothing
        End If
        StatusBar("Idle...")
        'picRunning.Visible = False
    End Sub
    Private Sub ShowAll()
        On Error GoTo errs
        Waiting()
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
        DoneWaiting()
        Exit Sub
errs:
        DoneWaiting()
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
        SearchValues.strSerial = Trim(txtSerialSearch.Text)
        'strDescription = Trim(txtDescription.Text)
        SearchValues.strAssetTag = Trim(txtAssetTagSearch.Text)
        'strPurchaseDate = Format(dtPurchaseDate.Text, strDBDateFormat)
        'strPurchaseDate = dtPurchaseDate.Text
        SearchValues.strDescription = Trim(txtDescription.Text)
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
        If Not BigQueryWorker.IsBusy Then
            ClickedButton = cmdSearch
            HideLiveBox()
            DynamicSearch()
        End If
    End Sub
    Private Sub DynamicSearch() 'dynamically creates sql query using any combination of search filters the users wants
        ' On Error GoTo errs
        ' Waiting()
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim ConnID As String = Guid.NewGuid.ToString
        GetSearchDBValues()
        Dim strStartQry = "SELECT * FROM devices WHERE "
        Dim strDynaQry = (IIf(SearchValues.strSerial <> "", " dev_serial Like '" & SearchValues.strSerial & "%' AND", "")) & (IIf(SearchValues.strAssetTag <> "", " dev_asset_tag LIKE '%" & SearchValues.strAssetTag & "%' AND", "")) & (IIf(SearchValues.strEqType <> "", " dev_eq_type LIKE '%" & SearchValues.strEqType & "%' AND", "")) & (IIf(SearchValues.strCurrentUser <> "", " dev_cur_user LIKE '%" & SearchValues.strCurrentUser & "%' AND", "")) & (IIf(SearchValues.strLocation <> "", " dev_location LIKE '%" & SearchValues.strLocation & "%' AND", "")) & (IIf(SearchValues.strStatus <> "", " dev_status LIKE '%" & SearchValues.strStatus & "%' AND", "")) & (IIf(SearchValues.strDescription <> "", " dev_description LIKE '%" & SearchValues.strDescription & "%' AND", ""))
        If strDynaQry = "" Then
            Dim blah = MsgBox("Please add some filter data.", vbOKOnly + vbInformation, "Fields Missing")
            Exit Sub
        End If
        Dim strQry = strStartQry & strDynaQry
        If Strings.Right(strQry, 3) = "AND" Then 'remove trailing AND from dynamic query
            strQry = Strings.Left(strQry, Strings.Len(strQry) - 3)
        End If
        strLastQry = strQry
        StartBigQuery(strQry)
        '        Dim cmd As New MySqlCommand(strQry, GetConnection(ConnID).DBConnection)
        '        reader = cmd.ExecuteReader
        '        With reader
        '            StatusBar(strCommMessage)
        '            Do While .Read()
        '                Dim Results As Device_Info
        '                Results.strCurrentUser = !dev_cur_user
        '                Results.strAssetTag = !dev_asset_tag
        '                Results.strSerial = !dev_serial
        '                Results.strDescription = !dev_description
        '                Results.strLocation = !dev_location
        '                Results.dtPurchaseDate = !dev_purchase_date
        '                Results.strGUID = !dev_UID
        '                Results.strEqType = !dev_eq_type
        '                AddToResults(Results)
        '            Loop
        '        End With
        '        SendToGrid(ResultGrid, SearchResults)
        '        CloseConnection(ConnID)
        '        DoneWaiting()
        '        Exit Sub
        'errs:
        '        DoneWaiting()
        '        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
        '            Exit Sub
        '        Else
        '            EndProgram()
        '        End If
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
    Private Sub HideLiveBox()
        On Error GoTo errs
        LiveBox.Visible = False
        LiveBox.Items.Clear()
        If CurrentControl.Parent.Name = "InstantGroup" Then
            CurrentControl.Text = ""
        End If
errs:
    End Sub
    Private Sub LoadDevice(ByVal strGUID As String)
        Waiting()
        View.ViewDevice(strGUID)
        View.Show()
        ' View.SetTracking(CurrentDevice.bolTrackable, CurrentDevice.Tracking.bolCheckedOut)
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
    Private Sub Waiting()
        Me.Cursor = Cursors.WaitCursor
    End Sub
    Private Sub DoneWaiting()
        Me.Cursor = Cursors.Default
    End Sub
    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs)
    End Sub
    Private Sub Button2_Click_1(sender As Object, e As EventArgs)
        Debug.Print(vbCrLf)
        Dim i As Integer
        For i = 0 To UBound(CurrentConnections)
            Debug.Print(i & " - " & CurrentConnections(i).ConnectionID & " = " & CurrentConnections(i).DBConnection.State)
        Next
    End Sub
    Private Sub YearsSincePurchaseToolStripMenuItem_Click(sender As Object, e As EventArgs)
        ReportView.Show()
    End Sub
    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If Not CheckForAdmin() Then Exit Sub
        AddNew.Show()
    End Sub
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As DoWorkEventArgs) Handles LiveQueryWorker.DoWork
        strPrevSearchString = strSearchString
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim ds As New DataSet
        Dim da As New MySqlDataAdapter
        Dim RowLimit As Integer = 15
        Dim strQryRow As String
        Select Case CurrentControl.Name
            Case "txtAssetTag"
                strQryRow = "dev_asset_tag"
            Case "txtSerial"
                strQryRow = "dev_serial"
            Case "txtCurUser"
                strQryRow = "dev_cur_user"
            Case "txtDescription"
                strQryRow = "dev_description"
        End Select
        da.SelectCommand = New MySqlCommand("SELECT dev_UID," & strQryRow & " FROM devices WHERE " & strQryRow & " LIKE '%" & strSearchString & "%' GROUP BY " & strQryRow & " ORDER BY " & strQryRow & " LIMIT " & RowLimit)
        da.SelectCommand.Connection = GetConnection(ConnID).DBConnection
        da.Fill(ds)
        CloseConnection(ConnID)
        dtResults = ds.Tables(0)
    End Sub
    Private Sub ResultGrid_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles ResultGrid.CellMouseDown
        If e.Button = MouseButtons.Right Then
            ResultGrid.CurrentCell = ResultGrid(e.ColumnIndex, e.RowIndex)
        End If
    End Sub
    Private Sub ContextMenuStrip1_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip1.Opening
    End Sub
    Private Sub txtAssetTag_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag.TextChanged
        CurrentControl = txtAssetTag
        strSearchString = txtAssetTag.Text
        StartLiveSearch()
    End Sub
    Private Sub DrawLiveBox()
        On Error GoTo errs
        Dim dr As DataRow
        LiveBox.Items.Clear()
        Dim strQryRow As String
        Dim CntGroup As GroupBox
        CntGroup = CurrentControl.Parent
        Select Case CurrentControl.Name
            Case "txtAssetTag"
                strQryRow = "dev_asset_tag"
            Case "txtSerial"
                strQryRow = "dev_serial"
            Case "txtCurUser"
                strQryRow = "dev_cur_user"
            Case "txtDescription"
                strQryRow = "dev_description"
        End Select
        With dr
            For Each dr In dtResults.Rows
                LiveBox.Items.Add(dr.Item(strQryRow))
            Next
        End With
        LiveBox.Left = CurrentControl.Left + CntGroup.Left ' + SearchGroup.Left
        LiveBox.Top = CurrentControl.Top + CurrentControl.Height + CntGroup.Top ' + SearchGroup.Top
        LiveBox.Width = CurrentControl.Width
        LiveBox.AutoSize = True
        LiveBox.Height = LiveBox.PreferredHeight
        If dtResults.Rows.Count > 0 Then
            LiveBox.Visible = True
        Else
            LiveBox.Visible = False
        End If
        If strPrevSearchString <> CurrentControl.Text Then
            strSearchString = CurrentControl.Text
            StartLiveSearch() 'if search string has changed since last completetion, run again.
        End If
        Exit Sub
errs:
        LiveBox.Visible = False
        LiveBox.Items.Clear()
    End Sub
    Private Sub QueryWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles LiveQueryWorker.RunWorkerCompleted
        DrawLiveBox()
    End Sub
    Private Sub txtSerial_TextChanged(sender As Object, e As EventArgs) Handles txtSerial.TextChanged
        CurrentControl = txtSerial
        strSearchString = txtSerial.Text
        StartLiveSearch()
    End Sub
    Private Sub StartLiveSearch()
        If Trim(strSearchString) <> "" Then
            If Not LiveQueryWorker.IsBusy Then LiveQueryWorker.RunWorkerAsync()
        Else
            HideLiveBox()
        End If
    End Sub
    Private Sub LiveBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LiveBox.SelectedIndexChanged
    End Sub
    Private Sub txtDescription_KeyUp(sender As Object, e As KeyEventArgs) Handles txtDescription.KeyUp
        CurrentControl = txtDescription
        strSearchString = txtDescription.Text
        StartLiveSearch()
    End Sub
    Private Sub BigQueryWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles BigQueryWorker.DoWork
        Dim i As Integer
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim strQry = strWorkerQry '"SELECT * FROM devices ORDER BY dev_input_datetime DESC"
        strLastQry = strQry
        Dim cmd As New MySqlCommand(strQry, GetConnection(ConnID).DBConnection)
        reader = cmd.ExecuteReader
        With reader
            StatusBar(strCommMessage)
            i += 1
            BigQueryWorker.ReportProgress(i)
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
        CloseConnection(ConnID)
    End Sub
    Private Sub txtCurUser_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCurUser.KeyUp
        CurrentControl = txtCurUser
        strSearchString = txtCurUser.Text
        StartLiveSearch()
    End Sub
    Private Sub Button2_Click_2(sender As Object, e As EventArgs)
    End Sub
    Private Sub BigQueryWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BigQueryWorker.RunWorkerCompleted
        BigQueryDone()
    End Sub
    Private Sub BigQueryWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BigQueryWorker.ProgressChanged
        StatusBar("Background query running...")
    End Sub
    Private Sub txtAssetTag_KeyDown(sender As Object, e As KeyEventArgs) Handles txtAssetTag.KeyDown
        If e.KeyCode = Keys.Down Then
            LiveBox.Focus()
            LiveBox.SelectedIndex = 0
        End If
    End Sub
    Private Sub LiveBox_MouseClick(sender As Object, e As MouseEventArgs) Handles LiveBox.MouseClick
        LiveBoxSelect()
    End Sub
    Private Sub LiveBoxSelect()
        Select Case CurrentControl.Name
            Case "txtDescription"
                CurrentControl.Text = LiveBox.Text
                DynamicSearch()
            Case "txtCurUser"
                CurrentControl.Text = LiveBox.Text
                DynamicSearch()
            Case Else
                LoadDevice(dtResults.Rows(LiveBox.SelectedIndex).Item("dev_UID"))
        End Select
        'If CurrentControl.Name <> "txtDescription" Or CurrentControl.Name <> "txtCurUser" Then
        '    LoadDevice(dtResults.Rows(LiveBox.SelectedIndex).Item("dev_UID"))
        'Else
        '    CurrentControl.Text = LiveBox.Text
        'End If
        HideLiveBox()
    End Sub
    Private Sub LiveBox_KeyPress(sender As Object, e As KeyPressEventArgs) Handles LiveBox.KeyPress
    End Sub
    Private Sub txtCurUser_TextChanged(sender As Object, e As EventArgs) Handles txtCurUser.TextChanged
    End Sub
    Private Sub LiveBox_KeyDown(sender As Object, e As KeyEventArgs) Handles LiveBox.KeyDown
        If e.KeyCode = Keys.Enter Then LiveBoxSelect()
    End Sub
    Private Sub txtDescription_TextChanged(sender As Object, e As EventArgs) Handles txtDescription.TextChanged
    End Sub
    Private Sub txtSerial_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSerial.KeyDown
        If e.KeyCode = Keys.Down Then
            LiveBox.Focus()
            LiveBox.SelectedIndex = 0
        End If
    End Sub
    Private Sub txtAssetTagSearch_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTagSearch.TextChanged
    End Sub
    Private Sub txtCurUser_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCurUser.KeyDown
        If e.KeyCode = Keys.Down Then
            LiveBox.Focus()
            LiveBox.SelectedIndex = 0
        End If
    End Sub
    Private Sub AddDeviceTool_Click(sender As Object, e As EventArgs) Handles AddDeviceTool.Click
        If Not CheckForAdmin() Then Exit Sub
        AddNew.Show()
    End Sub
    Private Sub YearsSincePurchaseToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles YearsSincePurchaseToolStripMenuItem1.Click
        ReportView.Show()
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        StripConns.Visible = True
        StripConns.Text = UBound(CurrentConnections)
    End Sub
    Private Sub txtSerialSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSerialSearch.TextChanged
    End Sub
    Private Sub txtDescription_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDescription.KeyDown
        If e.KeyCode = Keys.Down Then
            LiveBox.Focus()
            LiveBox.SelectedIndex = 0
        End If
    End Sub
End Class
