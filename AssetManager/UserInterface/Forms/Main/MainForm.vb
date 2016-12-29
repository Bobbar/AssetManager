Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Imports System.DirectoryServices.AccountManagement
Imports System.Threading
Imports System.Deployment.Application

Public Class MainForm
    Private strSearchString As String, strPrevSearchString As String
    Private StartingControl As Control
    Private strWorkerQry As String
    Private Const strShowAllQry As String = "SELECT * FROM " & devices.TableName & " ORDER BY " & devices.Input_DateTime & " DESC"
    Private ClickedButton As Control
    Dim dtResults As New DataTable
    Private intPrevRow As Integer
    Private bolGridFilling As Boolean = False
    Private ConnectAttempts As Integer = 0
    Private MyLiveBox As New clsLiveBox
    Private strLastQry As String
    Private cmdLastCommand As MySqlCommand
    Private MyWindowList As WindowList
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadProgram()
    End Sub
    Private Sub LoadProgram()
        Try
            DateTimeLabel.ToolTipText = My.Application.Info.Version.ToString
            ResultGrid.DefaultCellStyle.SelectionBackColor = colHighlightOrange
            ToolStrip1.BackColor = colAssetToolBarColor
            Logger("Starting AssetManager...")
            Status("Loading...")
            SplashScreen.Show()
            Status("Checking Server Connection...")
            If OpenConnections() Then
                ConnectionReady()
            Else
                Dim blah = Message("Error connecting to server!", vbOKOnly + vbExclamation, "Could not connect")
                EndProgram()
            End If
            ExtendedMethods.DoubleBuffered(ResultGrid, True)
            Status("Loading Indexes...")
            BuildIndexes()
            Status("Checking Access Level...")
            Asset.GetAccessLevels()
            Asset.GetUserAccess()
            If Not CanAccess(AccessGroup.CanRun, UserAccess.intAccessLevel) Then
                Message("You do not have permission to run this software.", vbOKOnly + vbExclamation, "Access Denied")
                EndProgram()
            End If
            If CanAccess(AccessGroup.IsAdmin, UserAccess.intAccessLevel) Then
                AdminDropDown.Visible = True
            Else
                AdminDropDown.Visible = False
            End If
            Clear_All()
            GetGridStyles()
            SetGridStyle(ResultGrid)
            ConnectionWatchDog.RunWorkerAsync()
            Status("Ready!")
            ShowAll()
            Thread.Sleep(1000)
            SplashScreen.Hide()
            Dim MyMunisTools As New MunisToolsMenu(Me, ToolStrip1, 2)
            MyWindowList = New WindowList(Me, tsdSelectWindow)
            Me.Show()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Sub GetGridStyles()
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
        GridStyles = tmpStyle
    End Sub
    Public Sub Status(Text As String)
        SplashScreen.lblStatus.Text = Text
        SplashScreen.Refresh()
    End Sub
    Private Sub Clear_All()
        MyLiveBox.HideLiveBox()
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
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Dim CancelClose As Boolean = CheckForActiveTransfers()
        If CancelClose Then
            e.Cancel = True
        Else
            EndProgram()
        End If
    End Sub
    Private Sub cmdShowAll_Click(sender As Object, e As EventArgs) Handles cmdShowAll.Click
        If Not BigQueryWorker.IsBusy Then
            ShowAll()
        End If
    End Sub
    Private Sub ShowAll()
        Dim cmd As New MySqlCommand
        cmd.CommandText = strShowAllQry
        strLastQry = strShowAllQry
        StartBigQuery(cmd)
    End Sub
    Public Sub RefreshCurrent()
        StartBigQuery(cmdLastCommand)
    End Sub
    Private Sub StartBigQuery(QryCommand As Object)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not BigQueryWorker.IsBusy Then
            If ClickedButton IsNot Nothing Then
                ClickedButton.Enabled = False
            End If
            StatusBar("Request sent to background...")
            StripSpinner.Visible = True
            BigQueryWorker.RunWorkerAsync(QryCommand)
        End If
    End Sub
    Private Sub BigQueryDone(Results As DataTable)
        SendToGrid(Results)
        StripSpinner.Visible = False
        If ClickedButton IsNot Nothing Then
            ClickedButton.Enabled = True
            ClickedButton = Nothing
        End If
    End Sub
    Private Sub DisplayRecords(NumberOf As Integer)
        lblRecords.Text = "Records: " & NumberOf
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
            table.Columns.Add("PO Number", GetType(String))
            table.Columns.Add("Purchase Date", GetType(String))
            table.Columns.Add("Replace Year", GetType(String))
            table.Columns.Add("Modified", GetType(String))
            table.Columns.Add("GUID", GetType(String))
            For Each r As DataRow In Results.Rows
                table.Rows.Add(NoNull(r.Item(devices.CurrentUser)),
                               NoNull(r.Item(devices.AssetTag)),
                               NoNull(r.Item(devices.Serial)),
                               GetHumanValue(DeviceIndex.EquipType, NoNull(r.Item(devices.EQType))),
                               NoNull(r.Item(devices.Description)),
                               GetHumanValue(DeviceIndex.OSType, NoNull(r.Item(devices.OSVersion))),
                               GetHumanValue(DeviceIndex.Locations, NoNull(r.Item(devices.Location))),
                               NoNull(r.Item(devices.PO)),
                               NoNull(r.Item(devices.PurchaseDate)),
                               NoNull(r.Item(devices.ReplacementYear)),
                               NoNull(r.Item(devices.LastMod_Date)),
                               NoNull(r.Item(devices.DeviceUID)))
            Next
            bolGridFilling = True
            ResultGrid.DataSource = table
            ResultGrid.ClearSelection()
            bolGridFilling = False
            DisplayRecords(table.Rows.Count)
            table.Dispose()
            StatusBar("Idle...")
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Function BuildSearchListNew() As List(Of SearchVal)
        Dim tmpList As New List(Of SearchVal)
        tmpList.Add(New SearchVal(devices.Serial, Trim(txtSerialSearch.Text)))
        tmpList.Add(New SearchVal(devices.AssetTag, Trim(txtAssetTagSearch.Text)))
        tmpList.Add(New SearchVal(devices.Description, Trim(txtDescription.Text)))
        tmpList.Add(New SearchVal(devices.EQType, GetDBValue(DeviceIndex.EquipType, cmbEquipType.SelectedIndex)))
        tmpList.Add(New SearchVal(devices.ReplacementYear, Trim(txtReplaceYear.Text)))
        tmpList.Add(New SearchVal(devices.OSVersion, GetDBValue(DeviceIndex.OSType, cmbOSType.SelectedIndex)))
        tmpList.Add(New SearchVal(devices.Location, GetDBValue(DeviceIndex.Locations, cmbLocation.SelectedIndex)))
        tmpList.Add(New SearchVal(devices.CurrentUser, Trim(txtCurUser.Text)))
        tmpList.Add(New SearchVal(devices.Status, GetDBValue(DeviceIndex.StatusType, cmbStatus.SelectedIndex)))
        tmpList.Add(New SearchVal(devices.Trackable, chkTrackables.Checked))
        Return tmpList
    End Function
    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        Clear_All()
    End Sub
    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        If Not BigQueryWorker.IsBusy Then
            ClickedButton = cmdSearch
            MyLiveBox.HideLiveBox()
            DynamicSearch()
        End If
    End Sub

    Public Sub DynamicSearch() 'dynamically creates sql query using any combination of search filters the users wants
        Dim table As New DataTable
        Dim cmd As New MySqlCommand
        Dim strStartQry As String
        If chkHistorical.Checked Then
            strStartQry = "SELECT * FROM " & historical_dev.TableName & " WHERE "
        Else
            strStartQry = "SELECT * FROM " & devices.TableName & " WHERE "
        End If
        Dim strDynaQry As String
        Dim SearchValCol As List(Of SearchVal) = BuildSearchListNew()
        For Each fld As SearchVal In SearchValCol
            If Not IsNothing(fld.Value) Then
                If fld.Value.ToString <> "" Then
                    If TypeOf fld.Value Is Boolean Then  'trackable boolean. if false, dont add it.
                        Dim bolTrackable As Boolean = CType(fld.Value, Boolean)
                        If Not bolTrackable Then
                            strDynaQry = strDynaQry + " " + fld.FieldName + " LIKE CONCAT('%', @" + fld.FieldName + ", '%') AND"
                            cmd.Parameters.AddWithValue("@" & fld.FieldName, Convert.ToInt32(fld.Value))
                        End If
                    Else
                        Select Case fld.FieldName 'use the fixed fields with EQUALS operator instead of LIKE
                            Case devices.OSVersion
                                strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                            Case devices.EQType
                                strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                            Case devices.Location
                                strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                            Case devices.Status
                                strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                            Case Else
                                strDynaQry = strDynaQry + " " + fld.FieldName + " LIKE CONCAT('%', @" + fld.FieldName + ", '%') AND"
                                cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                        End Select
                    End If
                End If
            End If
        Next
        If strDynaQry = "" Then
            Dim blah = Message("Please add some filter data.", vbOKOnly + vbInformation, "Fields Missing")
            Exit Sub
        End If
        Dim strQry = strStartQry & strDynaQry
        If Strings.Right(strQry, 3) = "AND" Then 'remove trailing AND from dynamic query
            strQry = Strings.Left(strQry, Strings.Len(strQry) - 3)
        End If
        strLastQry = strQry
        Debug.Print(strQry)
        cmd.CommandText = strQry
        StartBigQuery(cmd)
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        Clear_All()
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        AddNew.Show()
    End Sub
    Private Sub ResultGrid_DoubleClick(sender As Object, e As EventArgs) Handles ResultGrid.CellDoubleClick
        LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value.ToString)
    End Sub
    Public Sub LoadDevice(ByVal strGUID As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not DeviceIsOpen(strGUID) Then
            Waiting()
            Dim NewView As New frmView(Me, strGUID)
            DoneWaiting()
        Else
            ' Dim blah = Message("That device is already open.", vbOKOnly + vbExclamation, "Duplicate Window")
            ActivateFormByUID(strGUID)
        End If
    End Sub
    Private Sub RefreshCombos()
        FillComboBox(DeviceIndex.EquipType, cmbEquipType)
        FillComboBox(DeviceIndex.Locations, cmbLocation)
        FillComboBox(DeviceIndex.StatusType, cmbStatus)
        FillComboBox(DeviceIndex.OSType, cmbOSType)
    End Sub
    Private Sub ViewSelectedToolStripMenuItem_Click(sender As Object, e As EventArgs)
        LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value.ToString)
    End Sub
    Private Sub ViewToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ViewToolStripMenuItem.Click
        LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value.ToString)
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
        Application.DoEvents()
    End Sub
    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If Not CheckForAccess(AccessGroup.Add) Then Exit Sub
        AddNew.Show()
    End Sub
    Private Sub txtSerial_TextChanged(sender As Object, e As EventArgs) Handles txtSerial.TextChanged
        MyLiveBox.StartLiveSearch(sender, LiveBoxType.InstaLoad, devices.Serial)
    End Sub
    Private Sub txtAssetTag_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag.TextChanged
        MyLiveBox.StartLiveSearch(sender, LiveBoxType.InstaLoad, devices.AssetTag)
    End Sub
    Private Sub txtDescription_KeyUp(sender As Object, e As KeyEventArgs) Handles txtDescription.KeyUp
        MyLiveBox.StartLiveSearch(sender, LiveBoxType.DynamicSearch, devices.Description)
    End Sub
    Private Sub txtCurUser_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCurUser.KeyUp
        MyLiveBox.StartLiveSearch(sender, LiveBoxType.DynamicSearch, devices.CurrentUser)
    End Sub

    Private Sub BigQueryWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles BigQueryWorker.DoWork
        Try
            Dim LocalSQLComm As New clsMySQL_Comms
            Dim QryComm As New MySqlCommand
            QryComm = DirectCast(e.Argument, MySqlCommand)
            cmdLastCommand = QryComm
            Dim ds As New DataSet
            Dim da As New MySqlDataAdapter
            Dim conn As MySqlConnection = LocalSQLComm.NewConnection '(MySQLDB.MySQLConnectString)
            QryComm.Connection = conn
            BigQueryWorker.ReportProgress(1)
            da.SelectCommand = QryComm
            da.Fill(ds)
            da.Dispose()
            e.Result = ds.Tables(0)
            ds.Dispose()
            Asset.CloseConnection(conn) 'conn.Close()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
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
            MyLiveBox.GiveLiveBoxFocus()
        End If
    End Sub
    Private Sub txtSerial_KeyDown(sender As Object, e As KeyEventArgs) Handles txtSerial.KeyDown
        If e.KeyCode = Keys.Down Then
            MyLiveBox.GiveLiveBoxFocus()
        End If
    End Sub
    Private Sub txtCurUser_KeyDown(sender As Object, e As KeyEventArgs) Handles txtCurUser.KeyDown
        If e.KeyCode = Keys.Down Then
            MyLiveBox.GiveLiveBoxFocus()
        End If
    End Sub
    Private Sub AddDeviceTool_Click(sender As Object, e As EventArgs) Handles AddDeviceTool.Click
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not CheckForAccess(AccessGroup.Add) Then Exit Sub
        AddNew.Show()
    End Sub
    Private Sub txtDescription_KeyDown(sender As Object, e As KeyEventArgs) Handles txtDescription.KeyDown
        If e.KeyCode = Keys.Down Then
            MyLiveBox.GiveLiveBoxFocus()
        End If
    End Sub
    Private Sub CopyTool_Click(sender As Object, e As EventArgs) Handles CopyTool.Click
        Clipboard.SetDataObject(Me.ResultGrid.GetClipboardContent())
    End Sub
    Private Sub HighlightCurrentRow(Row As Integer)
        On Error Resume Next
        If Not bolGridFilling Then
            Dim BackColor As Color = DefGridBC
            Dim SelectColor As Color = DefGridSelCol
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
                    c2 = Color.FromArgb(BackColor.R, BackColor.G, BackColor.B)
                    BlendColor = Color.FromArgb((CInt(c1.A) + CInt(c2.A)) / 2,
                                                (CInt(c1.R) + CInt(c2.R)) / 2,
                                                (CInt(c1.G) + CInt(c2.G)) / 2,
                                                (CInt(c1.B) + CInt(c2.B)) / 2)
                    cell.Style.BackColor = BlendColor
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
        If Not ConnectionWatchDog.IsBusy Then ConnectionWatchDog.RunWorkerAsync()
    End Sub
    Private Sub ConnectionWatchDog_DoWork(sender As Object, e As DoWorkEventArgs) Handles ConnectionWatchDog.DoWork
        Do Until ProgramEnding
            If GlobalConn.State = ConnectionState.Open Then 'test connection
                Try
                    Dim LocalSQLComm As New clsMySQL_Comms
                    Dim ds As New DataSet
                    Dim da As New MySqlDataAdapter
                    Dim rows As Integer
                    Dim conn As MySqlConnection = LocalSQLComm.NewConnection
                    da.SelectCommand = New MySqlCommand("SELECT NOW()")
                    da.SelectCommand.Connection = conn
                    da.Fill(ds)
                    rows = ds.Tables(0).Rows.Count
                    strServerTime = ds.Tables(0).Rows(0).Item(0).ToString
                    Asset.CloseConnection(conn)
                    da.Dispose()
                    ds.Dispose()
                Catch ex As MySqlException
                    If ex.HResult = -2147467259 Then
                        ConnectionWatchDog.ReportProgress(1, "Connection Problem! Checking...")
                        ConnectionWatchDog.ReportProgress(2, "Disconnected")
                        'MySQLDB.CheckConnection()
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
    Private Sub ManageAttachmentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageAttachmentsToolStripMenuItem.Click
        Dim ViewAttachments As New frmAttachments(Me)
        ViewAttachments.bolAdminMode = CanAccess(AccessGroup.IsAdmin, UserAccess.intAccessLevel)
        ViewAttachments.ListAttachments()
        ViewAttachments.Text = ViewAttachments.Text & " - MANAGE ALL ATTACHMENTS"
        ViewAttachments.DeviceGroup.Visible = False
        ViewAttachments.cmdUpload.Enabled = False
    End Sub
    Private Sub DateTimeLabel_Click(sender As Object, e As EventArgs) Handles DateTimeLabel.Click
        If ApplicationDeployment.IsNetworkDeployed Then
            Message(ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString)
        Else
            Message("Debug")
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
        MyLiveBox.HideLiveBox()
    End Sub
    Private Sub Panel1_MouseWheel(sender As Object, e As MouseEventArgs)
        MyLiveBox.HideLiveBox()
    End Sub
    Private Sub cmbOSType_DropDown(sender As Object, e As EventArgs) Handles cmbOSType.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub
    Private Sub PanelNoScrollOnFocus1_Scroll(sender As Object, e As ScrollEventArgs) Handles PanelNoScrollOnFocus1.Scroll
        MyLiveBox.HideLiveBox()
    End Sub
    Private Sub PanelNoScrollOnFocus1_MouseWheel(sender As Object, e As MouseEventArgs) Handles PanelNoScrollOnFocus1.MouseWheel
        MyLiveBox.HideLiveBox()
    End Sub
    Private Sub cmdSibi_Click(sender As Object, e As EventArgs) Handles cmdSibi.Click
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not CheckForAccess(AccessGroup.Sibi_View) Then Exit Sub
        If Not SibiIsOpen() Then
            frmSibiMain.Tag = Me
            frmSibiMain.Show()
            frmSibiMain.Activate()
            ' MyWindowList.RefreshWindowList()
        Else
            frmSibiMain.Show()
            frmSibiMain.Activate()
            frmSibiMain.WindowState = FormWindowState.Normal
        End If
    End Sub
    Private Sub tsmUserManager_Click(sender As Object, e As EventArgs) Handles tsmUserManager.Click
        Dim NewUserMan As New frmUserManager(Me)
    End Sub
    Private Sub TextEnCrypterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TextEnCrypterToolStripMenuItem.Click
        Dim NewEncryp As New frmEncrypter(Me)
    End Sub
    Private Sub txtReplaceYear_KeyDown(sender As Object, e As KeyEventArgs) Handles txtReplaceYear.KeyDown
        If e.KeyCode = Keys.Down Then
            MyLiveBox.GiveLiveBoxFocus()
        End If
    End Sub
    Private Sub ResultGrid_KeyDown(sender As Object, e As KeyEventArgs) Handles ResultGrid.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value.ToString)
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub ScanAttachmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScanAttachmentToolStripMenuItem.Click
        FTP.ScanAttachements()
    End Sub
    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles cmdSupDevSearch.Click
        SendToGrid(Asset.DevicesBySup(Me))
    End Sub
End Class
