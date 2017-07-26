Option Explicit On

Imports System.ComponentModel
Imports System.Data.Common
Imports System.Deployment.Application
Imports System.Threading

Public Class MainForm

#Region "Fields"

    Private Const strShowAllQry As String = "SELECT * FROM " & devices.TableName & " ORDER BY " & devices.Input_DateTime & " DESC"
    Private bolGridFilling As Boolean = False
    Private LastCommand As DbCommand
    Private MyLiveBox As New LiveBox(Me)
    Private MyMunisToolBar As New MunisToolBar(Me)
    Private MyWindowList As New WindowList(Me)
    Private strServerTime As String = Now.ToString

#End Region

#Region "Methods"

    Public Sub DynamicSearch() 'dynamically creates sql query using any combination of search filters the users wants
        Try
            Dim cmd = DBFunc.GetCommand()
            Dim strStartQry As String
            If chkHistorical.Checked Then
                strStartQry = "SELECT * FROM " & historical_dev.TableName & " WHERE "
            Else
                strStartQry = "SELECT * FROM " & devices.TableName & " WHERE "
            End If
            Dim strDynaQry As String = ""
            Dim SearchValCol As List(Of SearchVal) = BuildSearchList()
            For Each fld As SearchVal In SearchValCol
                If Not IsNothing(fld.Value) Then
                    If fld.Value.ToString <> "" Then
                        If TypeOf fld.Value Is Boolean Then  'trackable boolean. if false, dont add it.
                            Dim bolTrackable As Boolean = CType(fld.Value, Boolean)
                            If bolTrackable Then
                                strDynaQry = strDynaQry + " " + fld.FieldName + " LIKE @" + fld.FieldName + " AND"
                                cmd.AddParameterWithValue("@" & fld.FieldName, Convert.ToInt32(fld.Value))
                            End If
                        Else
                            Select Case fld.FieldName 'use the fixed fields with EQUALS operator instead of LIKE
                                Case devices.OSVersion
                                    strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                    cmd.AddParameterWithValue("@" & fld.FieldName, fld.Value)
                                Case devices.EQType
                                    strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                    cmd.AddParameterWithValue("@" & fld.FieldName, fld.Value)
                                Case devices.Location
                                    strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                    cmd.AddParameterWithValue("@" & fld.FieldName, fld.Value)
                                Case devices.Status
                                    strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                    cmd.AddParameterWithValue("@" & fld.FieldName, fld.Value)
                                Case Else
                                    strDynaQry = strDynaQry + " " + fld.FieldName + " LIKE @" + fld.FieldName + " AND"
                                    Dim Value As String = "%" & fld.Value.ToString & "%"
                                    cmd.AddParameterWithValue("@" & fld.FieldName, Value)
                            End Select
                        End If
                    End If
                End If
            Next
            If strDynaQry = "" Then
                Message("Please add some filter data.", vbOKOnly + vbInformation, "Fields Missing", Me)
                Exit Sub
            End If
            Dim strQry = strStartQry & strDynaQry
            If Strings.Right(strQry, 3) = "AND" Then 'remove trailing AND from dynamic query
                strQry = Strings.Left(strQry, Strings.Len(strQry) - 3)
            End If
            cmd.CommandText = strQry
            StartBigQuery(cmd)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Public Sub LoadDevice(ByVal strGUID As String)
        If Not FormIsOpenByUID(GetType(ViewDeviceForm), strGUID) Then
            Waiting()
            ResultGrid.Enabled = False
            Dim NewView As New ViewDeviceForm(Me, strGUID)
            ResultGrid.Enabled = True
            DoneWaiting()
        End If
    End Sub

    Public Sub RefreshCurrent()
        StartBigQuery(LastCommand)
    End Sub

    Private Sub StatusBar(Text As String)
        StatusLabel.Text = Text
        StatusLabel.Invalidate()
    End Sub

    Private Sub AddDeviceTool_Click(sender As Object, e As EventArgs) Handles AddDeviceTool.Click
        If Not CheckForAccess(AccessGroup.Add) Then Exit Sub
        NewDeviceForm.Show()
        NewDeviceForm.Activate()
        NewDeviceForm.WindowState = FormWindowState.Normal
    End Sub

    Private Sub AdvancedSearchMenuItem_Click(sender As Object, e As EventArgs) Handles AdvancedSearchMenuItem.Click
        If Not CheckForAccess(AccessGroup.AdvancedSearch) Then Exit Sub
        Dim NewAdvancedSearch As New AdvancedSearchForm(Me)
    End Sub

    Private Sub BigQueryDone(ByRef Results As DataTable)
        SendToGrid(Results)
        DoneWaiting()
    End Sub

    Private Sub BigQueryWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles BigQueryWorker.DoWork
        Using QryComm = DirectCast(e.Argument, DbCommand)
            BigQueryWorker.ReportProgress(1)
            LastCommand = QryComm
            e.Result = DBFunc.DataTableFromCommand(QryComm)
        End Using
    End Sub

    Private Sub BigQueryWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs) Handles BigQueryWorker.ProgressChanged
        StatusBar("Background query running...")
    End Sub

    Private Sub BigQueryWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles BigQueryWorker.RunWorkerCompleted
        If e.Error Is Nothing Then
            If e.Result IsNot Nothing Then
                BigQueryDone(DirectCast(e.Result, DataTable))
            End If
        Else
            ErrHandle(e.Error, System.Reflection.MethodInfo.GetCurrentMethod())
        End If
        DoneWaiting()
    End Sub

    Private Function BuildSearchList() As List(Of SearchVal)
        Dim tmpList As New List(Of SearchVal)
        Dim CtlList As New List(Of Control)
        Dim DataParser As New DBControlParser(Me)
        DataParser.GetDBControls(Me, CtlList)
        For Each ctl In CtlList
            Dim DBInfo = DirectCast(ctl.Tag, DBControlInfo)
            tmpList.Add(New SearchVal(DBInfo.DataColumn, DataParser.GetDBControlValue(ctl)))
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
        If Not BigQueryWorker.IsBusy Then
            DynamicSearch()
        End If
    End Sub

    Private Sub cmdShowAll_Click(sender As Object, e As EventArgs) Handles cmdShowAll.Click
        If Not BigQueryWorker.IsBusy Then
            ShowAll()
        End If
    End Sub

    Private Sub cmdSibi_Click(sender As Object, e As EventArgs) Handles cmdSibi.Click
        If Not CheckForAccess(AccessGroup.Sibi_View) Then Exit Sub
        Waiting()
        If Not SibiIsOpen() Then
            SibiMainForm.Tag = Me
            SibiMainForm.Show()
            SibiMainForm.Activate()
        Else
            SibiMainForm.Show()
            SibiMainForm.Activate()
            SibiMainForm.WindowState = FormWindowState.Normal
        End If
        DoneWaiting()
    End Sub

    Private Sub cmdSupDevSearch_Click(sender As Object, e As EventArgs) Handles cmdSupDevSearch.Click
        Dim results As DataTable = AssetFunc.DevicesBySup(Me)
        If results IsNot Nothing Then
            SendToGrid(results)
        Else
            'do nutzing
        End If
    End Sub

    Private Async Sub ConnectionWatchDog()
        If OfflineMode Then
            StatusStrip1.BackColor = colFormBackColor
            ConnectStatus("Cached Mode", Color.Black, "Server Offline. Using Local DB Cache.")
        End If
        Try
            Dim FailedPings As Integer = 0
            Const MaxFailedPings As Integer = 2
            Const ItsTillHashCheck As Integer = 60
            Dim Its As Integer = 0
            Dim NoCacheMessageSent As Boolean = False
            Do Until ProgramEnding
                ServerPinging = Await Task.Run(Async Function()
                                                   Await Task.Delay(5000)
                                                   Try
                                                       Dim CanPing As Boolean = My.Computer.Network.Ping(strServerIP)
                                                       If CanPing Then
                                                           Using LocalSQLComm As New MySQL_Comms(True), cmd = LocalSQLComm.Return_SQLCommand("SELECT NOW()")
                                                               Dim strTime As String = cmd.ExecuteScalar.ToString
                                                               strServerTime = strTime
                                                           End Using
                                                       End If
                                                       Return CanPing
                                                   Catch
                                                       Return False
                                                   End Try
                                               End Function)
                CacheAvailable = Await VerifyLocalCacheHashOnly(OfflineMode)
                If DateTimeLabel.Text <> strServerTime Then DateTimeLabel.Text = strServerTime

                'Cache and connection handling.
                Select Case True
                    Case ServerPinging And Not OfflineMode
                        'Everything is normal

#Region "Server Online. Not In Cache Mode."

                        FailedPings = 0
                        ConnectStatus("Connected", Color.Green, "Connection OK")
                        StatusStrip1.BackColor = colFormBackColor
                        If CacheAvailable Then
                            Its += 1
                            If Its >= ItsTillHashCheck Then
                                Its = 0
                                RebuildCache()
                            End If
                        Else
                            SQLiteTableHashes = Nothing
                            RemoteTableHashes = Nothing
                            NoCacheMessageSent = False
                            RebuildCache()
                        End If

#End Region

                    Case Not ServerPinging And Not OfflineMode
                        'Server offline. Verify cache and enable if possible.

#Region "Server Offline. Not In Cache Mode."

                        FailedPings += 1
                        If FailedPings >= MaxFailedPings Then
                            FailedPings = 0
                            StatusStrip1.BackColor = colStatusBarProblem
                            ConnectStatus("Offline", Color.Red, "No connection. Cache unavailable.")

                            If CacheAvailable Then
                                OfflineMode = True
                                StatusStrip1.BackColor = colStatusBarProblem
                                ConnectStatus("Cached Mode", Color.Black, "Server Offline. Using Local DB Cache.")
                                '   Message("Connection Lost, cached mode enabled. Some functionality will be disabled.", vbOKOnly + vbInformation, "Cache Mode Enabled", Me)
                            Else
                                OfflineMode = False
                                If Not NoCacheMessageSent Then
                                    Message("Connection to the server was lost, and the local DB cache could not be verified. All functionality disabled.", vbOKOnly + vbExclamation, "Cache Mode Failed", Me)
                                    NoCacheMessageSent = True
                                End If
                            End If
                        End If

#End Region

                    Case Not ServerPinging And OfflineMode
                        'Notify user of cached mode.

                        If CacheAvailable Then
                            OfflineMode = True
                            StatusStrip1.BackColor = colStatusBarProblem
                            ConnectStatus("Cached Mode", Color.Black, "Server Offline. Using Local DB Cache.")
                            '   Message("Connection Lost, cached mode enabled. Some functionality will be disabled.", vbOKOnly + vbInformation, "Cache Mode Enabled", Me)
                        Else
                            OfflineMode = False
                            If Not NoCacheMessageSent Then
                                Message("Connection to the server was lost, and the local DB cache could not be verified. All functionality disabled.", vbOKOnly + vbExclamation, "Cache Mode Failed", Me)
                                NoCacheMessageSent = True
                            End If
                        End If

                    Case ServerPinging And OfflineMode
                        FailedPings = 0
                        'Connection Restored. Re-enable comms and rebuild cache.

#Region "Server Online. Running In Cache Mode."

                        OfflineMode = False
                        NoCacheMessageSent = False
                        ConnectStatus("Connection Restored!", Color.Green, "Connection OK")
                        StatusStrip1.BackColor = colFormBackColor
                        RebuildCache()

#End Region

                End Select
            Loop
        Catch
            ConnectionWatchDog()
        End Try
    End Sub

    Private Sub ConnectStatus(Message As String, FColor As Color, Optional ToolTipText As String = "")
        ConnStatusLabel.Text = Message
        ConnStatusLabel.ToolTipText = ToolTipText
        ConnStatusLabel.ForeColor = FColor
    End Sub

    Private Sub CopyTool_Click(sender As Object, e As EventArgs) Handles CopyTool.Click
        CopySelectedGridData(ResultGrid)
    End Sub

    Private Sub DateTimeLabel_Click(sender As Object, e As EventArgs) Handles DateTimeLabel.Click
        If ApplicationDeployment.IsNetworkDeployed Then
            Message(ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString,,, Me)
        Else
            Message("Debug",,, Me)
        End If
    End Sub

    Private Sub DisplayRecords(NumberOf As Integer)
        lblRecords.Text = "Records: " & NumberOf
    End Sub

    Private Sub DoneWaiting()
        SetWaitCursor(False)
        StripSpinner.Visible = False
        StatusBar("Idle...")
    End Sub

    Private Sub EnqueueGKUpdate()
        If VerifyAdminCreds() Then
            For Each cell As DataGridViewCell In ResultGrid.SelectedCells
                Dim DevUID As String = ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), cell.RowIndex).Value.ToString
                Dim SelectedDevice = AssetFunc.Get_DeviceInfo_From_UID(DevUID)
                GKUpdaterForm.AddUpdate(SelectedDevice)
            Next
            If Not GKUpdaterForm.Visible Then GKUpdaterForm.Show()
        End If
    End Sub

    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If Not OKToEnd() Then
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

        DefGridBC = ResultGrid.DefaultCellStyle.BackColor
        DefGridSelCol = ResultGrid.DefaultCellStyle.SelectionBackColor

        ResultGrid.DefaultCellStyle.SelectionBackColor = colSelectColor
        GridTheme.BackColor = ResultGrid.DefaultCellStyle.BackColor
        GridTheme.CellSelectColor = ResultGrid.DefaultCellStyle.SelectionBackColor
        GridTheme.RowHighlightColor = colHighlightColor

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
        txtSerialSearch.Tag = New DBControlInfo(devices.Serial)
        txtAssetTagSearch.Tag = New DBControlInfo(devices.AssetTag)
        txtDescription.Tag = New DBControlInfo(devices.Description)
        cmbEquipType.Tag = New DBControlInfo(devices.EQType, DeviceIndex.EquipType)
        txtReplaceYear.Tag = New DBControlInfo(devices.ReplacementYear)
        cmbOSType.Tag = New DBControlInfo(devices.OSVersion, DeviceIndex.OSType)
        cmbLocation.Tag = New DBControlInfo(devices.Location, DeviceIndex.Locations)
        txtCurUser.Tag = New DBControlInfo(devices.CurrentUser)
        cmbStatus.Tag = New DBControlInfo(devices.Status, DeviceIndex.StatusType)
        chkTrackables.Tag = New DBControlInfo(devices.Trackable)
    End Sub

    Private Sub InitLiveBox()
        MyLiveBox.AttachToControl(txtDescription, LiveBoxType.DynamicSearch, devices.Description)
        MyLiveBox.AttachToControl(txtCurUser, LiveBoxType.DynamicSearch, devices.CurrentUser)
        MyLiveBox.AttachToControl(txtSerial, LiveBoxType.InstaLoad, devices.Serial, devices.DeviceUID)
        MyLiveBox.AttachToControl(txtAssetTag, LiveBoxType.InstaLoad, devices.AssetTag, devices.DeviceUID)
    End Sub

    Private Sub LoadProgram()
        Try
            ShowAll()
            DateTimeLabel.Text = Now.ToString
            ToolStrip1.BackColor = colAssetToolBarColor
            ExtendedMethods.DoubleBufferedDataGrid(ResultGrid, True)
            If CanAccess(AccessGroup.IsAdmin) Then
                AdminDropDown.Visible = True
            Else
                AdminDropDown.Visible = False
            End If
            GetGridStyles()
            SetGridStyle(ResultGrid)
            ConnectionWatchDog()
            MyMunisToolBar.InsertMunisDropDown(ToolStrip1, 2)
            MyWindowList.InsertWindowList(ToolStrip1)
            InitLiveBox()
            InitDBControls()
            Clear_All()
            TestDBWarning()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            EndProgram()
        End Try
    End Sub

    Private Sub MainForm_HandleCreated(sender As Object, e As EventArgs) Handles Me.HandleCreated
        LoadProgram()
    End Sub

    Private Sub MainForm_Shown(sender As Object, e As EventArgs) Handles Me.Shown
        SplashScreenForm.Hide()
    End Sub

    Private Sub ManageAttachmentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageAttachmentsToolStripMenuItem.Click
        'Dim ViewAttachments As New AttachmentsForm(Me, New dev_attachments) 'TODO: Rework Attachments admin mode
        'ViewAttachments.bolAdminMode = CanAccess(AccessGroup.IsAdmin)
        'ViewAttachments.ListAttachments()
        'ViewAttachments.Text = ViewAttachments.Text & " - MANAGE ALL ATTACHMENTS"
        'ViewAttachments.DeviceGroup.Visible = False
        'ViewAttachments.cmdUpload.Enabled = False
    End Sub

    Private Sub PanelNoScrollOnFocus1_MouseWheel(sender As Object, e As MouseEventArgs) Handles SearchPanel.MouseWheel
        MyLiveBox.HideLiveBox()
    End Sub

    Private Sub PanelNoScrollOnFocus1_Scroll(sender As Object, e As ScrollEventArgs) Handles SearchPanel.Scroll
        MyLiveBox.HideLiveBox()
    End Sub

    Private Async Sub RebuildCache()
        Try
            StatusBar("Rebuilding DB Cache...")
            Await Task.Run(Sub()
                               If Not VerifyCacheHashes() Then RefreshLocalDBCache()
                           End Sub)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            StatusBar("Idle...")
        End Try
    End Sub

    Private Sub RefreshCombos()
        FillComboBox(DeviceIndex.EquipType, cmbEquipType)
        FillComboBox(DeviceIndex.Locations, cmbLocation)
        FillComboBox(DeviceIndex.StatusType, cmbStatus)
        FillComboBox(DeviceIndex.OSType, cmbOSType)
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
        LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value.ToString)
    End Sub

    Private Sub ResultGrid_KeyDown(sender As Object, e As KeyEventArgs) Handles ResultGrid.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value.ToString)
            e.SuppressKeyPress = True
        End If
    End Sub

    Private Sub ScanAttachmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScanAttachmentToolStripMenuItem.Click
        If Not CheckForAccess(AccessGroup.ManageAttachment) Then Exit Sub
        FTPFunc.ScanAttachements()
    End Sub

    Private Sub SendToGrid(ByRef Results As DataTable)
        Try
            If Results Is Nothing Then Exit Sub
            StatusBar("Building Grid...")
            Application.DoEvents()
            Dim table As New DataTable
            table.Columns.Add("User", GetType(String))
            table.Columns.Add("Asset ID", GetType(String))
            table.Columns.Add("Serial", GetType(String))
            table.Columns.Add("Device Type", GetType(String))
            table.Columns.Add("Description", GetType(String))
            table.Columns.Add("OS Version", GetType(String))
            table.Columns.Add("Location", GetType(String))
            table.Columns.Add("PO Number", GetType(String))
            table.Columns.Add("Purchase Date", GetType(Date))
            table.Columns.Add("Replace Year", GetType(String))
            table.Columns.Add("Modified", GetType(Date))
            table.Columns.Add("GUID", GetType(String))
            For Each r As DataRow In Results.Rows
                table.Rows.Add(r.Item(devices.CurrentUser),
                              r.Item(devices.AssetTag),
                              r.Item(devices.Serial),
                               GetHumanValue(DeviceIndex.EquipType, r.Item(devices.EQType).ToString),
                               r.Item(devices.Description),
                               GetHumanValue(DeviceIndex.OSType, r.Item(devices.OSVersion).ToString),
                               GetHumanValue(DeviceIndex.Locations, r.Item(devices.Location).ToString),
                               r.Item(devices.PO),
                               r.Item(devices.PurchaseDate),
                              r.Item(devices.ReplacementYear),
                              r.Item(devices.LastMod_Date),
                              r.Item(devices.DeviceUID))
            Next
            bolGridFilling = True
            ResultGrid.DataSource = table
            ResultGrid.ClearSelection()
            bolGridFilling = False
            DisplayRecords(table.Rows.Count)
            table.Dispose()
            Results.Dispose()
            DoneWaiting()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub ShowAll()
        Dim cmd = DBFunc.GetCommand(strShowAllQry)
        StartBigQuery(cmd)
    End Sub

    Private Sub StartBigQuery(QryCommand As DbCommand)
        If Not BigQueryWorker.IsBusy Then
            StatusBar("Request sent to background...")
            StripSpinner.Visible = True
            BigQueryWorker.RunWorkerAsync(QryCommand)
        End If
    End Sub

    Private Sub TestDBWarning()
        If bolUseTestDatabase Then
            Message("TEST DATABASE IN USE", vbOKOnly + vbExclamation, "WARNING", Me)
            Me.BackColor = Color.DarkRed
            Me.Text += " - ****TEST DATABASE****"
        End If
    End Sub

    Private Sub TextEnCrypterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TextEnCrypterToolStripMenuItem.Click
        If Not CheckForAccess(AccessGroup.IsAdmin) Then Exit Sub
        Dim NewEncryp As New CrypterForm(Me)
    End Sub

    Private Sub tsmAddGKUpdate_Click(sender As Object, e As EventArgs) Handles tsmAddGKUpdate.Click
        EnqueueGKUpdate()
    End Sub

    Private Sub tsmGKUpdater_Click(sender As Object, e As EventArgs) Handles tsmGKUpdater.Click
        If Not GKUpdaterForm.Visible Then
            GKUpdaterForm.Show()
        Else
            GKUpdaterForm.Activate()
        End If
    End Sub

    Private Sub tsmSendToGridForm_Click(sender As Object, e As EventArgs) Handles tsmSendToGridForm.Click
        CopyToGridForm(ResultGrid, Me)
    End Sub

    Private Sub tsmUserManager_Click(sender As Object, e As EventArgs) Handles tsmUserManager.Click
        If Not CheckForAccess(AccessGroup.IsAdmin) Then Exit Sub
        Dim NewUserMan As New UserManagerForm(Me)
    End Sub

    Private Sub txtGUID_KeyDown(sender As Object, e As KeyEventArgs) Handles txtGUID.KeyDown
        If e.KeyCode = Keys.Return Then
            LoadDevice(Trim(txtGUID.Text))
            txtGUID.Clear()
        End If
    End Sub

    Private Sub ViewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewToolStripMenuItem.Click
        LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value.ToString)
    End Sub

    Private Sub Waiting()
        SetWaitCursor(True)
        StatusBar("Processing...")
    End Sub

#End Region

End Class