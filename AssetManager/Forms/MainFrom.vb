Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Imports System.DirectoryServices.AccountManagement
Imports System.Threading
Public Class MainFrom
    Private strSearchString As String, strPrevSearchString As String
    Private StartingControl As Control
    Private strWorkerQry As String
    Private Const strShowAllQry As String = "SELECT * FROM devices ORDER BY dev_input_datetime DESC"
    Private ClickedButton As Control, ClickedButtonPrevText As String
    Dim dtResults As New DataTable
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
        If OpenConnections() Then
            ConnectionReady()
        Else
            Dim blah = MsgBox("Error connecting to server!", vbOKOnly + vbCritical, "Could not connect")
            EndProgram()
        End If
        Dim userFullName As String = UserPrincipal.Current.DisplayName
        'Logger("Enabling Double-Buffered Controls...")
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
        'Attachments.AttachGrid.DefaultCellStyle = GridStylez
        'Attachments.AttachGrid.DefaultCellStyle.Font = GridFont
        'Attachments.AttachGrid.ColumnHeadersDefaultCellStyle.Font = GridFont
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
        cmbOSType.Items.Clear()
        cmbLocation.Items.Clear()
        txtCurUser.Clear()
        txtDescription.Clear()
        txtReplaceYear.Clear()
        chkTrackables.Checked = False
        RefreshCombos()
        '  ResultGrid.DataSource = Nothing
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Not Attachments.UploadWorker.IsBusy And Not Attachments.DownloadWorker.IsBusy Then
            EndProgram()
        Else
            e.Cancel = True
            Attachments.Activate()
            Dim blah = MsgBox("There are active uploads/downloads. Do you wish to cancel the current operation?", MessageBoxIcon.Warning + vbYesNo, "Worker Busy")
            If blah = vbYes Then
                If Attachments.UploadWorker.IsBusy Then Attachments.UploadWorker.CancelAsync()
                If Attachments.DownloadWorker.IsBusy Then Attachments.DownloadWorker.CancelAsync()
            End If
        End If
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
    Private Sub BigQueryDone(Results As DataTable)
        SendToGrid(Results)
        StripSpinner.Visible = False
        If ClickedButton IsNot Nothing Then
            ClickedButton.Enabled = True
            ClickedButton.Text = ClickedButtonPrevText
            ClickedButton = Nothing
        End If
        StatusBar("Idle...")
        'picRunning.Visible = False
    End Sub
    Private Sub SendToGrid(Results As DataTable) ' Data() As Device_Info)
        Try
            StatusBar(strLoadingGridMessage)
            Dim table As New DataTable
            table.Columns.Add("User", GetType(String))
            table.Columns.Add("Asset ID", GetType(String))
            table.Columns.Add("Serial", GetType(String))
            table.Columns.Add("Device Type", GetType(String))
            table.Columns.Add("Description", GetType(String))
            table.Columns.Add("OS Version", GetType(String))
            table.Columns.Add("Location", GetType(String))
            table.Columns.Add("Purchase Date", GetType(String))
            table.Columns.Add("Replace Year", GetType(String))
            table.Columns.Add("GUID", GetType(String))
            For Each r As DataRow In Results.Rows
                table.Rows.Add(r.Item("dev_cur_user"), r.Item("dev_asset_tag"), r.Item("dev_serial"), GetHumanValue(ComboType.EquipType, r.Item("dev_eq_type")), r.Item("dev_description"), GetHumanValue(ComboType.OSType, r.Item("dev_osversion")), GetHumanValue(ComboType.Location, r.Item("dev_location")), r.Item("dev_purchase_date"), r.Item("dev_replacement_year"), r.Item("dev_UID"))
            Next
            bolGridFilling = True
            ResultGrid.DataSource = table
            ResultGrid.ClearSelection()
            bolGridFilling = False
            table.Dispose()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub GetSearchDBValues() 'cleanup user input for db
        SearchValues.strSerial = Trim(txtSerialSearch.Text)
        'strDescription = Trim(txtDescription.Text)
        SearchValues.strAssetTag = Trim(txtAssetTagSearch.Text)
        'strPurchaseDate = Format(dtPurchaseDate.Text, strDBDateFormat)
        'strPurchaseDate = dtPurchaseDate.Text
        SearchValues.strDescription = Trim(txtDescription.Text)
        SearchValues.strEqType = GetDBValue(ComboType.EquipType, cmbEquipType.SelectedIndex)
        SearchValues.strReplaceYear = Trim(txtReplaceYear.Text)
        SearchValues.strOSVersion = GetDBValue(ComboType.OSType, cmbOSType.SelectedIndex)
        SearchValues.strLocation = GetDBValue(ComboType.Location, cmbLocation.SelectedIndex)
        SearchValues.strCurrentUser = Trim(txtCurUser.Text)
        SearchValues.strStatus = GetDBValue(ComboType.StatusType, cmbStatus.SelectedIndex)
        SearchValues.bolTrackable = chkTrackables.Checked
        'strNotes = Trim(txtNotes.Text)
        'strPO =
        'strOSVersion =
    End Sub
    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
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
        Dim strDynaQry = (IIf(SearchValues.strSerial <> "", " dev_serial Like '" & SearchValues.strSerial & "%' AND", "")) & (IIf(SearchValues.strAssetTag <> "", " dev_asset_tag LIKE '%" & SearchValues.strAssetTag & "%' AND", "")) & (IIf(SearchValues.strEqType <> "", " dev_eq_type LIKE '%" & SearchValues.strEqType & "%' AND", "")) & (IIf(SearchValues.strReplaceYear <> "", " dev_replacement_year LIKE '%" & SearchValues.strReplaceYear & "%' AND", "")) & (IIf(SearchValues.strOSVersion <> "", " dev_osversion LIKE '%" & SearchValues.strOSVersion & "%' AND", "")) & (IIf(SearchValues.strCurrentUser <> "", " dev_cur_user LIKE '%" & SearchValues.strCurrentUser & "%' AND", "")) & (IIf(SearchValues.strLocation <> "", " dev_location LIKE '%" & SearchValues.strLocation & "%' AND", "")) & (IIf(SearchValues.bolTrackable, " dev_trackable = '" & Convert.ToInt32(SearchValues.bolTrackable) & "' AND", "")) & (IIf(SearchValues.strStatus <> "", " dev_status LIKE '%" & SearchValues.strStatus & "%' AND", "")) & (IIf(SearchValues.strDescription <> "", " dev_description LIKE '%" & SearchValues.strDescription & "%' AND", ""))
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
        FillOSTypeCombo()
    End Sub
    Private Sub FillOSTypeCombo()
        Dim i As Integer
        cmbOSType.Items.Clear()
        cmbOSType.Text = ""
        For i = 0 To UBound(OSType)
            cmbOSType.Items.Insert(i, OSType(i).strLong)
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
        Try
            strPrevSearchString = strSearchString
            Dim ds As New DataSet
            Dim da As New MySqlDataAdapter
            Dim RowLimit As Integer = 15
            Dim strQryRow As String
            Dim strQry As String
            Select Case ActiveControl.Name
                Case "txtAssetTag"
                    strQryRow = "dev_asset_tag"
                Case "txtSerial"
                    strQryRow = "dev_serial"
                Case "txtCurUser"
                    strQryRow = "dev_cur_user"
                Case "txtDescription"
                    strQryRow = "dev_description"
                Case "txtReplaceYear"
                    strQryRow = "dev_replacement_year"
            End Select
            strQry = "SELECT dev_UID," & strQryRow & " FROM devices WHERE " & strQryRow & " LIKE '%" & strSearchString & "%' GROUP BY " & strQryRow & " ORDER BY " & strQryRow & " LIMIT " & RowLimit
            da.SelectCommand = New MySqlCommand(strQry)
            da.SelectCommand.Connection = LiveConn
            da.Fill(ds)
            dtResults = ds.Tables(0)
            da.Dispose()
            ds.Dispose()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            ConnectionReady()
        End Try
    End Sub
    Private Sub DrawLiveBox(Optional PositionOnly As Boolean = False)
        Try
            Dim dr As DataRow
            Dim strQryRow As String
            Select Case TypeName(ActiveControl.Parent)
                Case "GroupBox"
                    Dim CntGroup As GroupBox
                    CntGroup = ActiveControl.Parent
                Case "Panel"
                    Dim CntGroup As Panel
                    CntGroup = ActiveControl.Parent
            End Select
            If Not PositionOnly Then
                If dtResults.Rows.Count < 1 Then
                    LiveBox.Visible = False
                    Exit Sub
                End If
                Select Case ActiveControl.Name
                    Case "txtAssetTag"
                        strQryRow = "dev_asset_tag"
                    Case "txtSerial"
                        strQryRow = "dev_serial"
                    Case "txtCurUser"
                        strQryRow = "dev_cur_user"
                    Case "txtDescription"
                        strQryRow = "dev_description"
                    Case "txtReplaceYear"
                        strQryRow = "dev_replacement_year"
                End Select
                LiveBox.Items.Clear()
                With dr
                    For Each dr In dtResults.Rows
                        LiveBox.Items.Add(dr.Item(strQryRow))
                    Next
                End With
            End If
            Dim ScreenPos As Point = Me.PointToClient(ActiveControl.Parent.PointToScreen(ActiveControl.Location))
            ScreenPos.Y = ScreenPos.Y + ActiveControl.Height
            LiveBox.Location = ScreenPos
            LiveBox.Width = ActiveControl.Width
            LiveBox.Height = LiveBox.PreferredHeight
            If dtResults.Rows.Count > 0 Then
                LiveBox.Visible = True
            Else
                LiveBox.Visible = False
            End If
            If strPrevSearchString <> ActiveControl.Text Then
                strSearchString = ActiveControl.Text
                StartLiveSearch() 'if search string has changed since last completetion, run again.
            End If
            Exit Sub
        Catch
            LiveBox.Visible = False
            LiveBox.Items.Clear()
        End Try
    End Sub
    Private Sub QueryWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles LiveQueryWorker.RunWorkerCompleted
        DrawLiveBox()
    End Sub
    Private Sub txtSerial_TextChanged(sender As Object, e As EventArgs) Handles txtSerial.TextChanged
        'CurrentControl = txtSerial
        strSearchString = txtSerial.Text
        StartLiveSearch()
    End Sub
    Private Sub txtAssetTag_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag.TextChanged
        ' CurrentControl = txtAssetTag
        strSearchString = txtAssetTag.Text
        StartLiveSearch()
    End Sub
    Private Sub txtDescription_KeyUp(sender As Object, e As KeyEventArgs) Handles txtDescription.KeyUp
        ' CurrentControl = txtDescription
        strSearchString = txtDescription.Text
        StartLiveSearch()
    End Sub
    Private Sub txtCurUser_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCurUser.KeyUp
        'CurrentControl = txtCurUser
        strSearchString = txtCurUser.Text
        StartLiveSearch()
    End Sub
    Private Sub StartLiveSearch()
        StartingControl = ActiveControl
        If Trim(strSearchString) <> "" Then
            If Not LiveQueryWorker.IsBusy And ConnectionReady() Then LiveQueryWorker.RunWorkerAsync()
        Else
            HideLiveBox()
        End If
    End Sub
    Private Sub LiveBox_SelectedIndexChanged(sender As Object, e As EventArgs) Handles LiveBox.SelectedIndexChanged
    End Sub
    Private Sub BigQueryWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles BigQueryWorker.DoWork
        Try
            Dim ds As New DataSet
            Dim da As New MySqlDataAdapter
            Dim strQry = strWorkerQry
            strLastQry = strQry
            Dim conn As New MySqlConnection(MySQLConnectString)
            conn.Open()
            BigQueryWorker.ReportProgress(1)
            da.SelectCommand = New MySqlCommand(strQry, conn)
            da.Fill(ds)
            da.Dispose()
            e.Result = ds.Tables(0)
            ds.Dispose()
            conn.Close()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            ConnectionReady()
        End Try
    End Sub
    Private Sub BigQueryWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BigQueryWorker.RunWorkerCompleted
        BigQueryDone(e.Result)
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
        Select Case StartingControl.Name
            Case "txtDescription"
                StartingControl.Text = LiveBox.Text
                DynamicSearch()
            Case "txtCurUser"
                StartingControl.Text = LiveBox.Text
                DynamicSearch()
            Case "txtReplaceYear"
                StartingControl.Text = LiveBox.Text
                DynamicSearch()
            Case Else
                LoadDevice(dtResults.Rows(LiveBox.SelectedIndex).Item("dev_UID"))
        End Select
        HideLiveBox()
    End Sub
    Private Sub HideLiveBox()
        Try
            LiveBox.Visible = False
            LiveBox.Items.Clear()
            If ActiveControl.Parent.Name = "InstantGroup" Then
                ActiveControl.Text = ""
            End If
        Catch
        End Try
    End Sub
    Private Sub LiveBox_KeyDown(sender As Object, e As KeyEventArgs) Handles LiveBox.KeyDown
        If e.KeyCode = Keys.Enter Then LiveBoxSelect()
    End Sub
    Private Sub txtSerial_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSerial.KeyDown
        If e.KeyCode = Keys.Down Then
            GiveLiveBoxFocus()
        End If
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
    Private Sub LiveBox_MouseMove(sender As Object, e As MouseEventArgs) Handles LiveBox.MouseMove
        LiveBox.SelectedIndex = LiveBox.IndexFromPoint(e.Location)
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
    Private Sub ReconnectThread_ProgressChanged(sender As Object, e As ProgressChangedEventArgs)
        StatusBar("Trying to reconnect... " & ConnectAttempts)
    End Sub
    Private Sub ReconnectThread_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
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
    Private Sub ManageAttachmentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageAttachmentsToolStripMenuItem.Click
        Dim ViewAttachments As New Attachments
        ViewAttachments.bolAdminMode = IsAdmin()
        ViewAttachments.ListAttachments()
        ViewAttachments.Text = ViewAttachments.Text & " - MANAGE ALL ATTACHMENTS"
        ViewAttachments.GroupBox2.Visible = False
        ViewAttachments.cmdUpload.Enabled = False
    End Sub
    Private Sub DateTimeLabel_Click(sender As Object, e As EventArgs) Handles DateTimeLabel.Click
        MsgBox(My.Application.Info.Version.ToString)
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
    Private Sub txtGUID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtGUID.KeyDown
        If e.KeyCode = Keys.Return Then
            LoadDevice(Trim(txtGUID.Text))
            txtGUID.Clear()
        End If
    End Sub
    Private Sub Panel1_Scroll(sender As Object, e As ScrollEventArgs)
        'If LiveBox.Visible Then DrawLiveBox(True)
        HideLiveBox()
    End Sub
    Private Sub Panel1_MouseWheel(sender As Object, e As MouseEventArgs)
        HideLiveBox()
    End Sub
    Private Sub txtDescription_TextChanged(sender As Object, e As EventArgs)
    End Sub
    Private Sub cmbOSType_DropDown(sender As Object, e As EventArgs) Handles cmbOSType.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub
    Private Sub txtGUIDSearch_KeyUp(sender As Object, e As KeyEventArgs) Handles txtReplaceYear.KeyUp
        strSearchString = txtReplaceYear.Text
        StartLiveSearch()
    End Sub
    Private Sub PanelNoScrollOnFocus1_Scroll(sender As Object, e As ScrollEventArgs) Handles PanelNoScrollOnFocus1.Scroll
        HideLiveBox()
    End Sub
    Private Sub PanelNoScrollOnFocus1_MouseWheel(sender As Object, e As MouseEventArgs) Handles PanelNoScrollOnFocus1.MouseWheel
        HideLiveBox()
    End Sub
    Private Sub txtReplaceYear_KeyDown(sender As Object, e As KeyEventArgs) Handles txtReplaceYear.KeyDown
        If e.KeyCode = Keys.Down Then
            GiveLiveBoxFocus()
        End If
    End Sub
End Class
