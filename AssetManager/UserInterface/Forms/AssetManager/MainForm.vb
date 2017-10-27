Option Explicit On

Imports System.ComponentModel
Imports System.Data.Common
Imports System.Deployment.Application
Imports MyDialogLib

Public Class MainForm

#Region "Fields"

    Private Const strShowAllQry As String = "SELECT * FROM " & DevicesCols.TableName & " ORDER BY " & DevicesCols.InputDateTime & " DESC"
    Private bolGridFilling As Boolean = False
    Private LastCommand As DbCommand
    Private MyLiveBox As New LiveBox(Me)
    Private MyMunisToolBar As New MunisToolBar(Me)
    Private MyWindowList As New WindowList(Me)
    Private QueryRunning As Boolean = False
    Private WatchDog As ConnectionMonitoring.ConnectionWatchdog
    Private CurrentTransaction As DbTransaction = Nothing

#End Region

#Region "Delegates"

    Delegate Sub ConnectStatusVoidDelegate(text As String, foreColor As Color, backColor As Color, toolTipText As String)

    Delegate Sub StatusVoidDelegate(text As String)

    Delegate Sub ServerTimeVoidDelegate(serverTime As String)

#End Region

#Region "Methods"
    Private Sub StartTransaction()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.CanStartTransaction) Then Exit Sub
        If Message("This will allow unchecked changes to the database. Incorrect inputs WILL BREAK THINGS! " & vbCrLf & vbCrLf & "Changes must be 'applied' and 'committed' before they will be permanently stored in the database.", vbOKCancel + vbExclamation, "WARNING", Me) = MsgBoxResult.Ok Then
            CurrentTransaction = DBFactory.GetDatabase.StartTransaction
            RefreshData()
            GridEditMode(True)
            DoneWaiting()
        End If
    End Sub

    Private Sub CommitTransaction()
        If CurrentTransaction IsNot Nothing Then
            If Message("Are you sure? This will permanently apply the changes to the database.", vbYesNo + vbExclamation, "Commit Transaction", Me) = MsgBoxResult.Yes Then
                CurrentTransaction.Commit()
                CurrentTransaction.Dispose()
                CurrentTransaction = Nothing
                RefreshData()
                GridEditMode(False)
                DoneWaiting()
            End If
        End If
    End Sub

    Private Sub RollbackTransaction()
        If CurrentTransaction IsNot Nothing Then
            If Message("Restore database to original state?", vbYesNo + vbQuestion, "Rollback Transaction", Me) = MsgBoxResult.Yes Then
                CurrentTransaction.Rollback()
                CurrentTransaction.Dispose()
                CurrentTransaction = Nothing
                RefreshData()
                GridEditMode(False)
                DoneWaiting()
            End If
        End If
    End Sub

    Private Sub UpdateRecords()
        If CurrentTransaction IsNot Nothing Then
            DBFactory.GetDatabase.UpdateTable(strShowAllQry, DirectCast(ResultGrid.DataSource, DataTable), CurrentTransaction)
            RefreshData()
            DoneWaiting()
        End If
    End Sub

    Private Sub GridEditMode(canEdit As Boolean)
        If canEdit Then
            ResultGrid.ReadOnly = False
            ResultGrid.EditMode = DataGridViewEditMode.EditOnEnter
            TransactionBox.Visible = True
        Else
            ResultGrid.ReadOnly = True
            TransactionBox.Visible = False
            ResultGrid.EditMode = DataGridViewEditMode.EditProgrammatically
        End If
    End Sub

    Public Sub DynamicSearch() 'dynamically creates sql query using any combination of search filters the users wants
        Try
            Dim strStartQry As String
            If chkHistorical.Checked Then
                strStartQry = "SELECT * FROM " & HistoricalDevicesCols.TableName & " WHERE "
            Else
                strStartQry = "SELECT * FROM " & DevicesCols.TableName & " WHERE "
            End If
            Dim strDynaQry As String = ""
            Dim SearchValCol As List(Of DBQueryParameter) = BuildSearchList()
            StartBigQuery(DBFactory.GetDatabase.GetCommandFromParams(strStartQry, SearchValCol))
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Public Sub LoadDevice(deviceGUID As String)
        If Not FormIsOpenByUID(GetType(ViewDeviceForm), deviceGUID) Then
            Waiting()
            Dim NewView As New ViewDeviceForm(Me, deviceGUID)
            DoneWaiting()
        End If
    End Sub

    Public Overrides Sub RefreshData()
        StartBigQuery(LastCommand)
    End Sub

    Private Sub AddNewDevice()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.AddDevice) Then Exit Sub
        Dim NewDevForm = GetChildOfType(Me, GetType(NewDeviceForm))
        If NewDevForm Is Nothing Then
            Dim NewDev As New NewDeviceForm(Me)
        Else
            ActivateForm(NewDevForm)
        End If
    End Sub

    Private Function BuildSearchList() As List(Of DBQueryParameter)
        Dim tmpList As New List(Of DBQueryParameter)
        Dim DataParser As New DBControlParser(Me)
        For Each ctl In DataParser.GetDBControls(Me)
            Dim CtlValue = DataParser.GetDBControlValue(ctl)
            If TypeOf CtlValue IsNot DBNull AndAlso CtlValue.ToString <> "" Then
                Dim DBInfo = DirectCast(ctl.Tag, DBControlInfo)
                Dim IsExact As Boolean = False
                Select Case DBInfo.DataColumn
                    Case DevicesCols.OSVersion
                        IsExact = True
                    Case DevicesCols.EQType
                        IsExact = True
                    Case DevicesCols.Location
                        IsExact = True
                    Case DevicesCols.Status
                        IsExact = True
                    Case Else
                        IsExact = False
                End Select
                tmpList.Add(New DBQueryParameter(DBInfo.DataColumn, CtlValue, IsExact))
            End If
        Next
        Return tmpList
    End Function

    Private Sub Clear_All()
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
        chkHistorical.Checked = False
        RefreshCombos()
    End Sub

    Private Async Sub WatchDogRebuildCache(sender As Object, e As EventArgs)
        If GlobalSwitches.BuildingCache Then Exit Sub
        GlobalSwitches.BuildingCache = True
        Try
            SetStatusBar("Rebuilding DB Cache...")
            Await Task.Run(Sub()
                               If Not DBCache.VerifyCacheHashes() Then
                                   DBCache.RefreshLocalDBCache()
                               End If
                           End Sub)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            GlobalSwitches.BuildingCache = False
            SetStatusBar("Idle...")
        End Try
    End Sub

    Private Sub WatchDogTick(sender As Object, e As EventArgs)
        Dim TickEvent = DirectCast(e, ConnectionMonitoring.WatchDogTickEventArgs)
        If DateTimeLabel.Text <> TickEvent.ServerTime Then
            SetServerTime(TickEvent.ServerTime)
        End If
    End Sub

    Private Sub WatchDogStatusChanged(sender As Object, e As EventArgs)
        Dim ConnectionEventArgs = DirectCast(e, ConnectionMonitoring.WatchDogStatusEventArgs)
        Logger("Connection Status changed to: " & ConnectionEventArgs.ConnectionStatus.ToString)
        Select Case ConnectionEventArgs.ConnectionStatus

            Case ConnectionMonitoring.WatchDogConnectionStatus.Online
                ConnectStatus("Connected", Color.Green, Colors.DefaultFormBackColor, "Connection OK")
                GlobalSwitches.CachedMode = False
                ServerInfo.ServerPinging = True

            Case ConnectionMonitoring.WatchDogConnectionStatus.Offline
                ConnectStatus("Offline", Color.Red, Colors.StatusBarProblem, "No connection. Cache unavailable.")
                GlobalSwitches.CachedMode = False
                ServerInfo.ServerPinging = False

            Case ConnectionMonitoring.WatchDogConnectionStatus.CachedMode
                ConnectStatus("Cached Mode", Color.Black, Colors.StatusBarProblem, "Server Offline. Using Local DB Cache.")
                GlobalSwitches.CachedMode = True
                ServerInfo.ServerPinging = False

        End Select
    End Sub

    Private Sub ConnectStatus(text As String, foreColor As Color, backColor As Color, toolTipText As String)
        If StatusStrip1.InvokeRequired Then
            Dim d As New ConnectStatusVoidDelegate(AddressOf ConnectStatus)
            StatusStrip1.Invoke(d, New Object() {text, foreColor, backColor, toolTipText})
        Else
            ConnStatusLabel.Text = text
            ConnStatusLabel.ToolTipText = toolTipText
            ConnStatusLabel.ForeColor = foreColor
            StatusStrip1.BackColor = backColor
            StatusStrip1.Update()
        End If
    End Sub

    Private Sub DisplayRecords(NumberOf As Integer)
        lblRecords.Text = "Records: " & NumberOf
    End Sub

    Private Sub DoneWaiting()
        SetWaitCursor(False, Me)
        StripSpinner.Visible = False
        SetStatusBar("Idle...")
    End Sub

    Private Sub EnqueueGKUpdate()
        If SecurityTools.VerifyAdminCreds() Then
            Dim SelectedDevices As New List(Of DeviceObject)
            Dim Rows As New HashSet(Of Integer)
            'Iterate selected cells and collect unique row indexes via a HashSet.(HashSet only allows unique values to be added to collection).
            For Each cell As DataGridViewCell In ResultGrid.SelectedCells
                Rows.Add(cell.RowIndex)
            Next

            For Each row In Rows
                Dim DevUID As String = GridFunctions.GetCurrentCellValue(ResultGrid, DevicesCols.DeviceUID)
                SelectedDevices.Add(AssetFunc.GetDeviceInfoFromGUID(DevUID))
            Next

            GKUpdaterForm.AddMultipleUpdates(SelectedDevices)
        End If
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If CurrentTransaction IsNot Nothing Then
            Message("There is currently an active transaction. Please commit or rollback before closing.", vbOKOnly + vbExclamation, "Cannot Close")
        End If

        If Not OKToEnd() Or Not OKToCloseChildren(Me) Or CurrentTransaction IsNot Nothing Then
            e.Cancel = True
        Else
            LastCommand.Dispose()
            MyLiveBox.Dispose()
            MyWindowList.Dispose()
            MyMunisToolBar.Dispose()
            EndProgram()
        End If
    End Sub

    Private Sub GetGridStyles()
        'set colors

        Colors.DefaultGridBackColor = ResultGrid.DefaultCellStyle.BackColor
        Colors.DefaultGridSelectColor = ResultGrid.DefaultCellStyle.SelectionBackColor

        ResultGrid.DefaultCellStyle.SelectionBackColor = Colors.OrangeSelectColor

        Me.GridTheme = New GridTheme(Colors.OrangeHighlightColor, ResultGrid.DefaultCellStyle.SelectionBackColor, ResultGrid.DefaultCellStyle.BackColor)

        Dim tmpStyle As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        tmpStyle.Alignment = ResultGrid.DefaultCellStyle.Alignment
        tmpStyle.BackColor = ResultGrid.DefaultCellStyle.BackColor
        tmpStyle.Font = ResultGrid.DefaultCellStyle.Font
        tmpStyle.ForeColor = ResultGrid.DefaultCellStyle.ForeColor
        tmpStyle.SelectionBackColor = ResultGrid.DefaultCellStyle.SelectionBackColor
        tmpStyle.SelectionForeColor = ResultGrid.DefaultCellStyle.SelectionForeColor
        tmpStyle.WrapMode = ResultGrid.DefaultCellStyle.WrapMode
        GridStyles = tmpStyle
    End Sub

    Private Sub InitDBControls()
        txtSerialSearch.Tag = New DBControlInfo(DevicesCols.Serial)
        txtAssetTagSearch.Tag = New DBControlInfo(DevicesCols.AssetTag)
        txtDescription.Tag = New DBControlInfo(DevicesCols.Description)
        cmbEquipType.Tag = New DBControlInfo(DevicesCols.EQType, DeviceAttribute.EquipType)
        txtReplaceYear.Tag = New DBControlInfo(DevicesCols.ReplacementYear)
        cmbOSType.Tag = New DBControlInfo(DevicesCols.OSVersion, DeviceAttribute.OSType)
        cmbLocation.Tag = New DBControlInfo(DevicesCols.Location, DeviceAttribute.Locations)
        txtCurUser.Tag = New DBControlInfo(DevicesCols.CurrentUser)
        cmbStatus.Tag = New DBControlInfo(DevicesCols.Status, DeviceAttribute.StatusType)
        chkTrackables.Tag = New DBControlInfo(DevicesCols.Trackable)
    End Sub

    Private Sub InitLiveBox()
        MyLiveBox.AttachToControl(txtDescription, DevicesCols.Description, LiveBoxType.DynamicSearch)
        MyLiveBox.AttachToControl(txtCurUser, DevicesCols.CurrentUser, LiveBoxType.DynamicSearch)
        MyLiveBox.AttachToControl(txtSerial, DevicesCols.Serial, LiveBoxType.InstaLoad, DevicesCols.DeviceUID)
        MyLiveBox.AttachToControl(txtAssetTag, DevicesCols.AssetTag, LiveBoxType.InstaLoad, DevicesCols.DeviceUID)
    End Sub

    Private Sub InitDBCombo()
        For Each item In [Enum].GetValues(GetType(Databases))
            DatabaseToolCombo.Items.Add(item.ToString)
        Next
        DatabaseToolCombo.SelectedIndex = ServerInfo.CurrentDataBase
    End Sub

    Private Sub LoadProgram()
        Try
            ShowAll()
            DateTimeLabel.Text = Now.ToString
            ToolStrip1.BackColor = Colors.AssetToolBarColor
            ExtendedMethods.DoubleBufferedDataGrid(ResultGrid, True)
            If SecurityTools.CanAccess(SecurityTools.AccessGroup.IsAdmin) Then
                AdminDropDown.Visible = True
            Else
                AdminDropDown.Visible = False
            End If
            GetGridStyles()
            SetGridStyle(ResultGrid)

            WatchDog = New ConnectionMonitoring.ConnectionWatchdog(GlobalSwitches.CachedMode)
            AddHandler WatchDog.StatusChanged, AddressOf WatchDogStatusChanged
            AddHandler WatchDog.RebuildCache, AddressOf WatchDogRebuildCache
            AddHandler WatchDog.WatcherTick, AddressOf WatchDogTick
            WatchDog.StartWatcher()

            MyMunisToolBar.InsertMunisDropDown(ToolStrip1, 2)
            MyWindowList.InsertWindowList(ToolStrip1)
            ImageCaching.CacheControlImages(Me)
            InitLiveBox()
            InitDBControls()
            Clear_All()
            ShowTestDBWarning()
            InitDBCombo()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            EndProgram()
        End Try
    End Sub

    Private Sub MainForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        SplashScreenForm.Dispose()
    End Sub

    Private Sub NewTextCrypterForm()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.IsAdmin) Then Exit Sub
        Dim NewEncryp As New CrypterForm(Me)
    End Sub

    Private Sub OpenSibiMainForm()
        Try
            If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ViewSibi) Then Exit Sub
            Waiting()
            Dim SibiForm = GetChildOfType(Me, GetType(SibiMainForm))
            If SibiForm Is Nothing Then
                Dim NewSibi As New SibiMainForm(Me)
            Else
                ActivateForm(SibiForm)
            End If
        Finally
            DoneWaiting()
        End Try
    End Sub

    Private Sub RefreshCombos()
        FillComboBox(DeviceAttribute.EquipType, cmbEquipType)
        FillComboBox(DeviceAttribute.Locations, cmbLocation)
        FillComboBox(DeviceAttribute.StatusType, cmbStatus)
        FillComboBox(DeviceAttribute.OSType, cmbOSType)
    End Sub

    Private Function ResultGridColumns() As List(Of DataGridColumn)
        Dim AttribColumnType As ColumnFormatTypes
        If CurrentTransaction IsNot Nothing Then
            AttribColumnType = ColumnFormatTypes.AttributeCombo
        Else
            AttribColumnType = ColumnFormatTypes.AttributeDisplayMemberOnly
        End If
        Dim ColList As New List(Of DataGridColumn)
        ColList.Add(New DataGridColumn(DevicesCols.CurrentUser, "User", GetType(String)))
        ColList.Add(New DataGridColumn(DevicesCols.AssetTag, "Asset ID", GetType(String)))
        ColList.Add(New DataGridColumn(DevicesCols.Serial, "Serial", GetType(String)))
        ColList.Add(New DataGridColumn(DevicesCols.EQType, "Device Type", DeviceAttribute.EquipType, AttribColumnType))
        ColList.Add(New DataGridColumn(DevicesCols.Description, "Description", GetType(String)))
        ColList.Add(New DataGridColumn(DevicesCols.OSVersion, "OS Version", DeviceAttribute.OSType, AttribColumnType))
        ColList.Add(New DataGridColumn(DevicesCols.Location, "Location", DeviceAttribute.Locations, AttribColumnType))
        ColList.Add(New DataGridColumn(DevicesCols.PO, "PO Number", GetType(String)))
        ColList.Add(New DataGridColumn(DevicesCols.PurchaseDate, "Purchase Date", GetType(Date)))
        ColList.Add(New DataGridColumn(DevicesCols.ReplacementYear, "Replace Year", GetType(String)))
        ColList.Add(New DataGridColumn(DevicesCols.LastModDate, "Modified", GetType(Date)))
        ColList.Add(New DataGridColumn(DevicesCols.DeviceUID, "GUID", GetType(String)))
        Return ColList
    End Function

    Private Sub SendToGrid(ByRef results As DataTable)
        If results Is Nothing Then Exit Sub
        Using results
            SetStatusBar("Building Grid...")
            bolGridFilling = True
            If CurrentTransaction IsNot Nothing Then
                GridFunctions.PopulateGrid(ResultGrid, results, ResultGridColumns, True)
            Else
                GridFunctions.PopulateGrid(ResultGrid, results, ResultGridColumns)
            End If
            ResultGrid.ClearSelection()
            ResultGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells)
            bolGridFilling = False
            DisplayRecords(ResultGrid.Rows.Count)
        End Using
    End Sub

    Private Sub ShowAll()
        Dim cmd = DBFactory.GetDatabase.GetCommand(strShowAllQry)
        StartBigQuery(cmd)
    End Sub

    Private Sub StartAdvancedSearch()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.AdvancedSearch) Then Exit Sub
        Dim NewAdvancedSearch As New AdvancedSearchForm(Me)
    End Sub

    Private Sub StartAttachScan()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ManageAttachment) Then Exit Sub
        FTPFunc.ScanAttachements()
    End Sub

    Private Async Sub StartBigQuery(QryCommand As DbCommand)
        Try
            If Not QueryRunning Then
                SetWaitCursor(True, Me)
                StripSpinner.Visible = True
                SetStatusBar("Background query running...")
                QueryRunning = True
                Dim Results = Await Task.Run(Function()
                                                 LastCommand = QryCommand
                                                 ' If CurrentTransaction IsNot Nothing Then
                                                 Return DBFactory.GetDatabase.DataTableFromCommand(QryCommand, CurrentTransaction)
                                                 'Else
                                                 '    Return DBFactory.GetDatabase.DataTableFromCommand(QryCommand)
                                                 'End If
                                             End Function)
                QryCommand.Dispose()
                SendToGrid(Results)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            QueryRunning = False
            DoneWaiting()
        End Try
    End Sub

    Private Sub StartUserManager()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.IsAdmin) Then Exit Sub
        Dim NewUserMan As New UserManagerForm(Me)
    End Sub

    Private Sub SetStatusBar(text As String)
        If StatusStrip1.InvokeRequired Then
            Dim d As New StatusVoidDelegate(AddressOf SetStatusBar)
            StatusStrip1.Invoke(d, New Object() {text})
        Else
            StatusLabel.Text = text
            StatusStrip1.Update()
        End If
    End Sub

    Private Sub SetServerTime(serverTime As String)
        If StatusStrip1.InvokeRequired Then
            Dim d As New ServerTimeVoidDelegate(AddressOf SetServerTime)
            StatusStrip1.Invoke(d, New Object() {serverTime})
        Else
            DateTimeLabel.Text = serverTime
            StatusStrip1.Update()
        End If
    End Sub

    Private Sub ChangeDatabase(database As Databases)
        Try
            If CurrentTransaction Is Nothing Then
                If Not GlobalSwitches.CachedMode And ServerInfo.ServerPinging Then
                    If database <> ServerInfo.CurrentDataBase Then
                        Dim blah = Message("Are you sure? This will close all open forms.", vbYesNo + vbQuestion, "Change Database", Me)
                        If blah = MsgBoxResult.Yes Then
                            If OKToCloseChildren(Me) Then
                                CloseChildren(Me)
                                ServerInfo.CurrentDataBase = database
                                PopulateAttributeIndexes()
                                RefreshCombos()
                                SecurityTools.GetUserAccess()
                                InitDBControls()
                                GlobalSwitches.BuildingCache = True
                                Task.Run(Sub() DBCache.RefreshLocalDBCache())
                                ShowTestDBWarning()
                                SetDatabaseTitleText()
                                ShowAll()
                            End If
                        End If
                    End If
                Else
                    Message("Cannot switch database while Offline or in Cached Mode.", vbOK + vbInformation, "Unavailable", Me)
                End If
            Else
                Message("There is currently an active transaction. Please commit or rollback before switching databases.", vbOKOnly + vbExclamation, "Stop")
            End If

        Finally
            DatabaseToolCombo.SelectedIndex = ServerInfo.CurrentDataBase
        End Try
    End Sub

    Private Sub SetDatabaseTitleText()
        Select Case ServerInfo.CurrentDataBase
            Case Databases.asset_manager
                Me.Text = "Asset Manager - Main"
            Case Databases.test_db
                Me.Text = "Asset Manager - Main - ****TEST DATABASE****"
            Case Databases.vintondd
                Me.Text = "Asset Manager - Main - Vinton DD"
        End Select

    End Sub

    Private Sub ShowTestDBWarning()
        If ServerInfo.CurrentDataBase = Databases.test_db Then
            Message("TEST DATABASE IN USE", vbOKOnly + vbExclamation, "WARNING", Me)
            Me.BackColor = Color.DarkRed
            'Me.Text += " - ****TEST DATABASE****"
        Else
            Me.BackColor = Color.FromArgb(232, 232, 232)
            '  Me.Text = "Asset Manager - Main"
        End If
    End Sub

    Private Sub Waiting()
        SetWaitCursor(True, Me)
        SetStatusBar("Processing...")
    End Sub

    Private Async Sub StartPowerShellScript(scriptByte() As Byte)
        Try
            Dim Hostname As String
            Using GetHostnameDialog As New AdvancedDialog(Me)
                With GetHostnameDialog
                    .Text = "Remote Computer Hostname"
                    .AddTextBox("HostnameText", "Hostname:")
                    .ShowDialog()
                    If .DialogResult = DialogResult.OK Then
                        Hostname = Trim(GetHostnameDialog.GetControlValue("HostnameText").ToString)
                    Else
                        Exit Sub
                    End If
                End With
            End Using

            If Hostname <> "" Then
                If SecurityTools.VerifyAdminCreds() Then
                    Waiting()
                    If Await ExecutePowerShellScript(Hostname, scriptByte) Then
                        Message("Command successful.", vbOKOnly + vbInformation, "Done", Me)
                    End If
                End If
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            DoneWaiting()
        End Try
    End Sub

    Private Async Function ExecutePowerShellScript(hostname As String, scriptByte() As Byte) As Task(Of Boolean)
        Dim UpdateResult = Await Task.Run(Function()
                                              Dim PSWrapper As New PowerShellWrapper
                                              Return PSWrapper.ExecuteRemotePSScript(hostname, scriptByte, SecurityTools.AdminCreds)
                                          End Function)
        If UpdateResult <> "" Then
            Message(UpdateResult, vbOKOnly + vbExclamation, "Error Running Script")
            Return False
        Else
            Return True
        End If
    End Function

#Region "Control Event Methods"

    Private Sub AdvancedSearchMenuItem_Click(sender As Object, e As EventArgs) Handles AdvancedSearchMenuItem.Click
        StartAdvancedSearch()
    End Sub

    Private Sub AddDeviceTool_Click(sender As Object, e As EventArgs) Handles AddDeviceTool.Click
        AddNewDevice()
    End Sub

    Private Sub cmbEquipType_DropDown(sender As Object, e As EventArgs) Handles cmbEquipType.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub

    Private Sub cmbLocation_DropDown(sender As Object, e As EventArgs) Handles cmbLocation.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub

    Private Sub cmbOSType_DropDown(sender As Object, e As EventArgs) Handles cmbOSType.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        Clear_All()
    End Sub

    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        DynamicSearch()
    End Sub

    Private Sub cmdShowAll_Click(sender As Object, e As EventArgs) Handles cmdShowAll.Click
        ShowAll()
    End Sub

    Private Sub cmdSibi_Click(sender As Object, e As EventArgs) Handles cmdSibi.Click
        OpenSibiMainForm()
    End Sub

    Private Sub cmdSupDevSearch_Click(sender As Object, e As EventArgs) Handles cmdSupDevSearch.Click
        Try
            Dim results As DataTable = AssetFunc.DevicesBySupervisor(Me)
            If results IsNot Nothing Then
                SendToGrid(results)
            Else
                'do nutzing
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            DoneWaiting()
        End Try
    End Sub

    Private Sub CopyTool_Click(sender As Object, e As EventArgs) Handles CopyTool.Click
        GridFunctions.CopySelectedGridData(ResultGrid)
    End Sub

    Private Sub DateTimeLabel_Click(sender As Object, e As EventArgs) Handles DateTimeLabel.Click
        If ApplicationDeployment.IsNetworkDeployed Then
            Message(ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString,,, Me)
        Else
            Message("Debug",,, Me)
        End If
    End Sub

    Private Sub PanelNoScrollOnFocus1_MouseWheel(sender As Object, e As MouseEventArgs) Handles SearchPanel.MouseWheel
        MyLiveBox.HideLiveBox()
    End Sub

    Private Sub PanelNoScrollOnFocus1_Scroll(sender As Object, e As ScrollEventArgs) Handles SearchPanel.Scroll
        MyLiveBox.HideLiveBox()
    End Sub

    Private Sub ResultGrid_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellEnter
        If Not bolGridFilling Then
            HighlightRow(ResultGrid, GridTheme, e.RowIndex)
        End If
    End Sub

    Private Sub ResultGrid_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles ResultGrid.CellLeave
        LeaveRow(ResultGrid, GridTheme, e.RowIndex)
    End Sub

    Private Sub ResultGrid_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles ResultGrid.CellMouseDown
        If e.ColumnIndex >= 0 And e.RowIndex >= 0 Then
            If e.Button = MouseButtons.Right And Not ResultGrid.Item(e.ColumnIndex, e.RowIndex).Selected Then
                ResultGrid.Rows(e.RowIndex).Selected = True
                ResultGrid.CurrentCell = ResultGrid(e.ColumnIndex, e.RowIndex)
            End If
        End If
    End Sub

    Private Sub ResultGrid_DoubleClick(sender As Object, e As EventArgs) Handles ResultGrid.CellDoubleClick
        LoadDevice(GridFunctions.GetCurrentCellValue(ResultGrid, DevicesCols.DeviceUID))
    End Sub

    Private Sub ResultGrid_KeyDown(sender As Object, e As KeyEventArgs) Handles ResultGrid.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadDevice(GridFunctions.GetCurrentCellValue(ResultGrid, DevicesCols.DeviceUID))
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub ScanAttachmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScanAttachmentToolStripMenuItem.Click
        StartAttachScan()
    End Sub

    Private Sub TextEnCrypterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TextEnCrypterToolStripMenuItem.Click
        NewTextCrypterForm()
    End Sub

    Private Sub tsmAddGKUpdate_Click(sender As Object, e As EventArgs) Handles tsmAddGKUpdate.Click
        EnqueueGKUpdate()
    End Sub

    Private Sub tsmGKUpdater_Click(sender As Object, e As EventArgs) Handles tsmGKUpdater.Click
        If Not GKUpdaterForm.Visible Then
            GKUpdaterForm.Show()
        Else
            GKUpdaterForm.WindowState = FormWindowState.Normal
            GKUpdaterForm.Activate()
        End If
    End Sub

    Private Sub tsmSendToGridForm_Click(sender As Object, e As EventArgs) Handles tsmSendToGridForm.Click
        GridFunctions.CopyToGridForm(ResultGrid, Me)
    End Sub

    Private Sub tsmUserManager_Click(sender As Object, e As EventArgs) Handles tsmUserManager.Click
        StartUserManager()
    End Sub

    Private Sub txtGUID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtGUID.KeyDown
        If e.KeyCode = Keys.Return Then
            LoadDevice(Trim(txtGUID.Text))
            txtGUID.Clear()
        End If
    End Sub

    Private Sub ViewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewToolStripMenuItem.Click
        LoadDevice(GridFunctions.GetCurrentCellValue(ResultGrid, DevicesCols.DeviceUID))
    End Sub

    Private Sub InstallChromeMenuItem_Click(sender As Object, e As EventArgs) Handles InstallChromeMenuItem.Click
        StartPowerShellScript(My.Resources.UpdateChrome)
    End Sub

    Private Sub MainForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        LastCommand.Dispose()
        MyLiveBox.Dispose()
        MyMunisToolBar.Dispose()
        MyWindowList.Dispose()
        WatchDog.Dispose()
    End Sub

    Private Sub ReEnterLACredentialsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ReEnterLACredentialsToolStripMenuItem.Click
        SecurityTools.ClearAdminCreds()
        SecurityTools.VerifyAdminCreds()
    End Sub

    Private Sub ViewLogToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewLogToolStripMenuItem.Click
        Dim StartInfo As New ProcessStartInfo
        StartInfo.FileName = Paths.LogPath
        Process.Start(StartInfo)
    End Sub

    Private Sub DatabaseToolCombo_DropDownClosed(sender As Object, e As EventArgs) Handles DatabaseToolCombo.DropDownClosed
        ChangeDatabase(CType(DatabaseToolCombo.SelectedIndex, Databases))
    End Sub

    Private Sub MainForm_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadProgram()
        Application.DoEvents()
    End Sub
    Private Sub CommitButton_Click(sender As Object, e As EventArgs) Handles CommitButton.Click
        CommitTransaction()
    End Sub

    Private Sub RollbackButton_Click(sender As Object, e As EventArgs) Handles RollbackButton.Click
        RollbackTransaction()
    End Sub

    Private Sub StartTransactionToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles StartTransactionToolStripMenuItem.Click
        StartTransaction()
    End Sub

    Private Sub UpdateButton_Click(sender As Object, e As EventArgs) Handles UpdateButton.Click
        UpdateRecords()
    End Sub

#End Region

#End Region

End Class