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
    Private DefGridBC As Color, DefGridSelCol As Color
    Private intPrevRow As Integer
    Private bolGridFilling As Boolean = False
    Private ConnectAttempts As Integer = 0
    Private SearchValues As Device_Info
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ' ResultGrid.RowHeadersDefaultCellStyle.BackColor = Color.Green
        DateTimeLabel.ToolTipText = My.Application.Info.Version.ToString
        ResultGrid.DefaultCellStyle.SelectionBackColor = colHighlightOrange
        ToolStrip1.BackColor = colToolBarColor
        View.ToolStrip1.BackColor = colToolBarColor
        'View.StatusStrip1.BackColor = colToolBarColor
        Logger("Starting AssetManager...")
        Status("Loading...")
        SplashScreen.Show()
        Status("Checking Server Connection...")
        If OpenConnections() Then 'CheckConnection() Then
            ConnectionReady()
            'Liveconn.Open()
            'do nut-zing
        Else
            Dim blah = MsgBox("Error connecting to server!", vbOKOnly + vbCritical, "Could not connect")
            EndProgram()
        End If
        Dim userFullName As String = UserPrincipal.Current.DisplayName
        Logger("Enabling Double-Buffered Controls...")
        ExtendedMethods.DoubleBuffered(ResultGrid, True)
        ExtendedMethods.DoubleBufferedListBox(LiveBox, True)
        Status("Loading Indexes...")
        BuildIndexes()
        Status("Checking Access Level...")
        GetAccessLevels()
        GetUserAccess()
        If Not CanAccess("can_run") Then
            MsgBox("You do not have permission to run this software.", vbOKOnly + vbCritical, "Access Denied")
            EndProgram()
        End If
        If IsAdmin() Then
            AdminDropDown.Visible = True
        Else
            AdminDropDown.Visible = False
            'GetDBs()
        End If
        Clear_All()
        GetGridStylez()
        CopyDefaultCellStyles()
        'Status("Loading devices...")
        ConnectionWatchDog.RunWorkerAsync()
        StartBigQuery(strShowAllQry)
        Status("Ready!")
        Thread.Sleep(1000)
        SplashScreen.Hide()
        Me.Show()
        'Tracking.Show()
    End Sub
    Public Sub GetGridStylez()
        'set colors
        ResultGrid.DefaultCellStyle.SelectionBackColor = colSelectColor
        DefGridBC = ResultGrid.DefaultCellStyle.BackColor
        DefGridSelCol = ResultGrid.DefaultCellStyle.SelectionBackColor
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
        chkTrackables.Checked = False
        RefreshCombos()
        ReDim SearchResults(0)
        '  ResultGrid.DataSource = Nothing
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        EndProgram()
    End Sub
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
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
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
        bolGridFilling = True
        Grid.DataSource = table
        bolGridFilling = False
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
        SearchValues.bolTrackable = chkTrackables.Checked
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
        Dim table As New DataTable
        GetSearchDBValues()
        Dim strStartQry = "SELECT * FROM devices WHERE "
        Dim strDynaQry = (IIf(SearchValues.strSerial <> "", " dev_serial Like '" & SearchValues.strSerial & "%' AND", "")) & (IIf(SearchValues.strAssetTag <> "", " dev_asset_tag LIKE '%" & SearchValues.strAssetTag & "%' AND", "")) & (IIf(SearchValues.strEqType <> "", " dev_eq_type LIKE '%" & SearchValues.strEqType & "%' AND", "")) & (IIf(SearchValues.strCurrentUser <> "", " dev_cur_user LIKE '%" & SearchValues.strCurrentUser & "%' AND", "")) & (IIf(SearchValues.strLocation <> "", " dev_location LIKE '%" & SearchValues.strLocation & "%' AND", "")) & (IIf(SearchValues.bolTrackable, " dev_trackable = '" & Convert.ToInt32(SearchValues.bolTrackable) & "' AND", "")) & (IIf(SearchValues.strStatus <> "", " dev_status LIKE '%" & SearchValues.strStatus & "%' AND", "")) & (IIf(SearchValues.strDescription <> "", " dev_description LIKE '%" & SearchValues.strDescription & "%' AND", ""))
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
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Clear_All()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        AddNew.Show()
    End Sub
    Private Sub ResultGrid_DoubleClick(sender As Object, e As EventArgs) Handles ResultGrid.CellDoubleClick
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
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        Waiting()
        View.CloseChildren()
        View.ViewDevice(strGUID)
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
    Private Sub ViewSelectedToolStripMenuItem_Click(sender As Object, e As EventArgs)
        LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value)
    End Sub
    Private Sub ViewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewToolStripMenuItem.Click
        LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value)
    End Sub
    Private Sub Waiting()
        Me.Cursor = Cursors.WaitCursor
        StatusBar("Processing...")
    End Sub
    Private Sub DoneWaiting()
        Me.Cursor = Cursors.Default
        If ConnectionReady() Then StatusBar("Idle...")
    End Sub
    Public Sub StatusBar(Text As String)
        On Error Resume Next
        StatusLabel.Text = Text
        Me.Refresh()
    End Sub
    Private Sub YearsSincePurchaseToolStripMenuItem_Click(sender As Object, e As EventArgs)
        ReportView.Show()
    End Sub
    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If Not CheckForAdmin() Then Exit Sub
        AddNew.Show()
    End Sub
    Private Sub BackgroundWorker1_DoWork(sender As Object, e As DoWorkEventArgs) Handles LiveQueryWorker.DoWork
        On Error GoTo errs
        strPrevSearchString = strSearchString
        Dim ds As New DataSet
        Dim da As New MySqlDataAdapter
        Dim RowLimit As Integer = 15
        Dim strQryRow As String
        Dim strQry As String
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
        strQry = "SELECT dev_UID," & strQryRow & " FROM devices WHERE " & strQryRow & " LIKE '%" & strSearchString & "%' GROUP BY " & strQryRow & " ORDER BY " & strQryRow & " LIMIT " & RowLimit
        da.SelectCommand = New MySqlCommand(strQry)
        'Debug.Print(strQry)
        da.SelectCommand.Connection = LiveConn
        da.Fill(ds)
        'conn.Close()
        dtResults = Nothing
        dtResults = ds.Tables(0)
        Exit Sub
errs:
        ConnectionReady()
    End Sub
    Private Sub txtAssetTag_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag.TextChanged
        CurrentControl = txtAssetTag
        strSearchString = txtAssetTag.Text
        StartLiveSearch()
    End Sub
    Private Sub DrawLiveBox()
        On Error GoTo errs
        If dtResults.Rows.Count < 1 Then
            LiveBox.Visible = False
            Exit Sub
        End If
        Dim dr As DataRow
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
        LiveBox.Items.Clear()
        With dr
            For Each dr In dtResults.Rows
                LiveBox.Items.Add(dr.Item(strQryRow))
            Next
        End With
        Dim ScreenPos As Point = Me.PointToClient(CurrentControl.Parent.PointToScreen(CurrentControl.Location))
        ScreenPos.Y = ScreenPos.Y + CurrentControl.Height
        LiveBox.Location = ScreenPos
        LiveBox.Width = CurrentControl.Width
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
            If Not LiveQueryWorker.IsBusy And ConnectionReady() Then LiveQueryWorker.RunWorkerAsync()
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
        On Error GoTo errs
        Dim i As Integer
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        'Dim ConnID As String = Guid.NewGuid.ToString
        Dim strQry = strWorkerQry '"SELECT * FROM devices ORDER BY dev_input_datetime DESC"
        strLastQry = strQry
        Dim conn As New MySqlConnection(MySQLConnectString)
        Dim cmd As New MySqlCommand(strQry, conn)
        conn.Open()
        reader = cmd.ExecuteReader
        With reader
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
        reader.Close()
        conn.Close()
        conn = Nothing
        reader = Nothing
        Exit Sub
errs:
        ConnectionReady()
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
            GiveLiveBoxFocus()
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
            GiveLiveBoxFocus()
        End If
    End Sub
    Private Sub txtAssetTagSearch_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTagSearch.TextChanged
    End Sub
    Private Sub txtCurUser_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCurUser.KeyDown
        If e.KeyCode = Keys.Down Then
            GiveLiveBoxFocus()
        End If
    End Sub
    Private Sub AddDeviceTool_Click(sender As Object, e As EventArgs) Handles AddDeviceTool.Click
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not CheckForAccess("add") Then Exit Sub
        'If Not CheckForAdmin() Then Exit Sub
        AddNew.Show()
    End Sub
    Private Sub YearsSincePurchaseToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles YearsSincePurchaseToolStripMenuItem1.Click
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        ReportView.Show()
    End Sub
    Private Sub txtSerialSearch_TextChanged(sender As Object, e As EventArgs) Handles txtSerialSearch.TextChanged
    End Sub
    Private Sub txtDescription_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDescription.KeyDown
        If e.KeyCode = Keys.Down Then
            GiveLiveBoxFocus()
        End If
    End Sub
    Private Sub GiveLiveBoxFocus()
        LiveBox.Focus()
        If LiveBox.SelectedIndex = -1 Then
            LiveBox.SelectedIndex = 0
        End If
    End Sub
    Private Sub CopyTool_Click(sender As Object, e As EventArgs) Handles CopyTool.Click
        Clipboard.SetDataObject(Me.ResultGrid.GetClipboardContent())
    End Sub
    Private Sub AssetManager_Leave(sender As Object, e As EventArgs) Handles Me.Leave
    End Sub
    Private Sub LiveBox_MouseMove(sender As Object, e As MouseEventArgs) Handles LiveBox.MouseMove
        LiveBox.SelectedIndex = LiveBox.IndexFromPoint(e.Location)
    End Sub
    Private Sub GetDBs()
        '        On Error GoTo errs
        '        Dim ds As New DataSet
        '        Dim da As New MySqlDataAdapter
        '        Dim row As DataRow
        '        Dim conn As New MySqlConnection(MySQLConnectString)
        '        da.SelectCommand = New MySqlCommand("SHOW DATABASES")
        '        da.SelectCommand.Connection = GlobalConn
        '        da.Fill(ds)
        '        'rows = ds.Tables(0).Rows.Count
        '        cmbDBs.Items.Clear()
        '        Dim item As Object
        '        For Each row In ds.Tables(0).Rows
        '            For Each col As DataColumn In ds.Tables(0).Columns
        '                Debug.Print(row(col.ColumnName).ToString)
        '                cmbDBs.Items.Add(row(col.ColumnName).ToString)
        '            Next
        '        Next
        '        da.Dispose()
        '        ds.Dispose()
        '        Exit Sub
        'errs:
        '        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
        '            Resume Next
        '        Else
        '            EndProgram()
        '        End If
    End Sub
    Private Sub ResultGrid_SelectionChanged(sender As Object, e As EventArgs) Handles ResultGrid.SelectionChanged
        'HighlightCurrentRow()
    End Sub
    Private Sub HighlightCurrentRow(Row As Integer)
        On Error Resume Next
        If Not bolGridFilling Then
            Dim BackColor As Color = DefGridBC
            Dim SelectColor As Color = DefGridSelCol
            Dim Mod1 As Integer = 3
            Dim Mod2 As Integer = 4
            Dim Mod3 As Single = 0.6 '0.75
            Dim c1 As Color = colHighlightColor 'highlight color
            If Row > -1 Then
                For Each cell As DataGridViewCell In ResultGrid.Rows(Row).Cells
                    Dim c2 As Color = Color.FromArgb(SelectColor.R, SelectColor.G, SelectColor.B)
                    Dim BlendColor As Color
                    BlendColor = Color.FromArgb((CInt(c1.A) + CInt(c2.A)) / 2,
                                                (CInt(c1.R) + CInt(c2.R)) / 2,
                                                (CInt(c1.G) + CInt(c2.G)) / 2,
                                                (CInt(c1.B) + CInt(c2.B)) / 2)
                    cell.Style.SelectionBackColor = BlendColor
                    'cell.Style.SelectionBackColor = Color.FromArgb(SelectColor.R * Mod3, SelectColor.G * Mod3, SelectColor.B * Mod3)
                    c2 = Color.FromArgb(BackColor.R, BackColor.G, BackColor.B)
                    BlendColor = Color.FromArgb((CInt(c1.A) + CInt(c2.A)) / 2,
                                                (CInt(c1.R) + CInt(c2.R)) / 2,
                                                (CInt(c1.G) + CInt(c2.G)) / 2,
                                                (CInt(c1.B) + CInt(c2.B)) / 2)
                    cell.Style.BackColor = BlendColor
                    'cell.Style.BackColor = Color.FromArgb(BackColor.R * Mod3, BackColor.G * Mod3, BackColor.B * Mod3)
                Next
            End If
        End If
    End Sub
    Private Sub ConnectionWatchDog_Tick(sender As Object, e As EventArgs) Handles ConnectionWatcher.Tick
        If DateTimeLabel.Text <> strServerTime Then DateTimeLabel.Text = strServerTime
        Select Case GlobalConn.State
            Case ConnectionState.Connecting
                ConnectStatus("Connecting", Color.Black)
        End Select
    End Sub
    Private Sub ConnectionWatchDog_DoWork(sender As Object, e As DoWorkEventArgs) Handles ConnectionWatchDog.DoWork
        Do Until ProgramEnding
            If GlobalConn.State = ConnectionState.Open Then 'test connection
                Try
                    Dim ds As New DataSet
                    Dim da As New MySqlDataAdapter
                    Dim rows As Integer
                    Dim conn As New MySqlConnection(MySQLConnectString)
                    da.SelectCommand = New MySqlCommand("SELECT NOW()")
                    da.SelectCommand.Connection = conn
                    da.Fill(ds)
                    rows = ds.Tables(0).Rows.Count
                    strServerTime = ds.Tables(0).Rows(0).Item(0).ToString
                    conn.Close()
                    conn.Dispose()
                    da.Dispose()
                    ds.Dispose()
                Catch ex As MySqlException
                    If ex.HResult = -2147467259 Then
                        ConnectionWatchDog.ReportProgress(1, "Connection Problem! Checking...")
                        ConnectionWatchDog.ReportProgress(2, "Disconnected")
                        CheckConnection()
                    End If
                End Try
            ElseIf GlobalConn.State <> ConnectionState.Open Then 'connection recovery
                ConnectAttempts = 0
                Do Until GlobalConn.State = ConnectionState.Open
                    ConnectAttempts += 1
                    ConnectionWatchDog.ReportProgress(1, "Trying to reconnect... " & ConnectAttempts)
                    ConnectionWatchDog.ReportProgress(5, GlobalConn.State)
                    If OpenConnections() Then
                    Else
                        Thread.Sleep(5000)
                    End If
                Loop
                ConnectionWatchDog.ReportProgress(1, "Reconnected!")
            End If
            ConnectionWatchDog.ReportProgress(5, GlobalConn.State)
            Thread.Sleep(5000)
        Loop
    End Sub
    Private Sub ConnectionWatchDog_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles ConnectionWatchDog.ProgressChanged
        Select Case e.ProgressPercentage 'hack alert!
            Case 1 'status message
                StatusBar(e.UserState)
            Case 2 'custom connnect state red
                ConnectStatus(e.UserState, Color.Red)
                StatusStrip1.BackColor = colStatusBarProblem
            Case 5 'pass connect state
                Dim State As ConnectionState = e.UserState
                Select Case State
                    Case ConnectionState.Closed
                        ConnectStatus("Disconnected", Color.Red)
                    Case ConnectionState.Open
                        ConnectStatus("Connected", Color.Green)
                        StatusStrip1.BackColor = colFormBackColor
                    Case ConnectionState.Connecting
                        ConnectStatus("Connecting", Color.Black)
                    Case ConnectionState.Executing
                        ConnectStatus("Executing", Color.Green)
                    Case Else
                        ConnectStatus("Disconnected", Color.Red)
                End Select
        End Select
    End Sub
    Private Sub ResultGrid_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellLeave
        Dim BackColor As Color = DefGridBC
        Dim SelectColor As Color = DefGridSelCol
        If e.RowIndex > -1 Then
            For Each cell As DataGridViewCell In ResultGrid.Rows(e.RowIndex).Cells
                cell.Style.SelectionBackColor = SelectColor
                cell.Style.BackColor = BackColor
            Next
        End If
    End Sub
    Private Sub ResultGrid_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles ResultGrid.CellMouseDown
        On Error Resume Next
        If e.Button = MouseButtons.Right And Not ResultGrid.Item(e.ColumnIndex, e.RowIndex).Selected Then
            ResultGrid.Rows(e.RowIndex).Selected = True
            ResultGrid.CurrentCell = ResultGrid(e.ColumnIndex, e.RowIndex)
        End If
    End Sub
    Private Sub ResultGrid_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellEnter
        HighlightCurrentRow(e.RowIndex)
    End Sub
    Private Sub ReconnectThread_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles ReconnectThread.ProgressChanged
        StatusBar("Trying to reconnect... " & ConnectAttempts)
    End Sub
    Private Sub ReconnectThread_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles ReconnectThread.RunWorkerCompleted
        ConnectionReady()
        StatusBar("Connected!")
    End Sub
    Private Sub cmdChangeDB_Click(sender As Object, e As EventArgs)
        If cmbDBs.Text <> "" And cmbDBs.Text <> strDatabase Then
            strDatabase = cmbDBs.Text
            MySQLConnectString = "server=" & strServerIP & ";uid=asset_mgr_usr;pwd=A553tP455;database=" & strDatabase
            CloseConnections()
            GlobalConn = New MySqlConnection(MySQLConnectString)
            LiveConn = New MySqlConnection(MySQLConnectString)
            OpenConnections()
        End If
    End Sub
    Private Sub ToolStripComboBox1_Click(sender As Object, e As EventArgs) Handles cmbDBs.Click
    End Sub
    Private Sub ToolStripDropDownButton2_Click(sender As Object, e As EventArgs) Handles AdminDropDown.Click
    End Sub
    Private Sub cmbDBs_SelectedIndexChanged(sender As Object, e As EventArgs)
    End Sub
    Private Sub ManageAttachmentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageAttachmentsToolStripMenuItem.Click
        Dim ViewAttachments As New Attachments
        ViewAttachments.bolAdminMode = IsAdmin()
        ViewAttachments.ListAttachments()
        ViewAttachments.Text = ViewAttachments.Text & " - MANAGE ALL ATTACHMENTS"
        ViewAttachments.GroupBox2.Visible = False
        ViewAttachments.cmdUpload.Enabled = False
    End Sub
    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter
    End Sub
    Private Sub Button2_Click_1(sender As Object, e As EventArgs)
        Debug.Print(CanAccess("can_run"))
    End Sub
    Private Sub DateTimeLabel_Click(sender As Object, e As EventArgs) Handles DateTimeLabel.Click
        MsgBox(My.Application.Info.Version.ToString)
    End Sub
    Private Sub txtGUID_Click(sender As Object, e As EventArgs) Handles txtGUID.Click
    End Sub
    Private Sub cmbDBs_TextChanged(sender As Object, e As EventArgs) Handles cmbDBs.TextChanged
        If cmbDBs.Text <> "" And cmbDBs.Text <> strDatabase Then
            strDatabase = cmbDBs.Text
            MySQLConnectString = "server=" & strServerIP & ";uid=asset_mgr_usr;pwd=A553tP455;database=" & strDatabase
            CloseConnections()
            GlobalConn = New MySqlConnection(MySQLConnectString)
            LiveConn = New MySqlConnection(MySQLConnectString)
            OpenConnections()
        End If
    End Sub
    Private Sub cmbEquipType_DropDown(sender As Object, e As EventArgs) Handles cmbEquipType.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub
    Private Sub cmbLocation_DropDown(sender As Object, e As EventArgs) Handles cmbLocation.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub
    Private Sub txtGUID_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtGUID.KeyPress
    End Sub
    Private Sub txtGUID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtGUID.KeyDown
        If e.KeyCode = Keys.Return Then
            LoadDevice(Trim(txtGUID.Text))
            txtGUID.Clear()
        End If
    End Sub
    Private Sub txtGUID_RightToLeftChanged(sender As Object, e As EventArgs) Handles txtGUID.RightToLeftChanged
    End Sub
End Class
