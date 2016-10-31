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
    Private Const strShowAllQry As String = "SELECT * FROM devices ORDER BY " & devices.Input_DateTime & " DESC"
    Private ClickedButton As Control
    Dim dtResults As New DataTable
    Private intPrevRow As Integer
    Private bolGridFilling As Boolean = False
    Private ConnectAttempts As Integer = 0
    Private MyLiveBox As New clsLiveBox
    Private strLastQry As String
    Private cmdLastCommand As MySqlCommand
    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LoadProgram()
    End Sub
    Private Sub LoadProgram()
        Try
            DateTimeLabel.ToolTipText = My.Application.Info.Version.ToString
            ResultGrid.DefaultCellStyle.SelectionBackColor = colHighlightOrange
            ToolStrip1.BackColor = colToolBarColor
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
            Dim userFullName As String = UserPrincipal.Current.DisplayName
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
            Thread.Sleep(500)
            SplashScreen.Hide()
            Dim MyMunisTools As New MunisToolsMenu
            ToolStrip1.Items.Insert(2, MyMunisTools.MunisTools)
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
        RefreshCombos()
    End Sub
    Private Sub Form1_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        For Each frm As Form In My.Application.OpenForms
            If TypeOf frm Is frmAttachments Then
                Dim Attachments As frmAttachments = frm
                If Not Attachments.UploadWorker.IsBusy And Not Attachments.DownloadWorker.IsBusy Then
                    EndProgram()
                Else
                    e.Cancel = True
                    Attachments.Activate()
                    Dim blah = Message("There are active uploads/downloads. Do you wish to cancel the current operation?", MessageBoxIcon.Warning + vbYesNo, "Worker Busy")
                    If blah = vbYes Then
                        If Attachments.UploadWorker.IsBusy Then Attachments.UploadWorker.CancelAsync()
                        If Attachments.DownloadWorker.IsBusy Then Attachments.DownloadWorker.CancelAsync()
                    End If
                End If
            End If
        Next
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
        StatusBar("Idle...")
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
            Dim ModDateColumn As String
            If chkHistorical.Checked Then
                ModDateColumn = "hist_action_datetime"
            Else
                ModDateColumn = devices.LastMod_Date
            End If
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
                               NoNull(r.Item(ModDateColumn)),
                               NoNull(r.Item(devices.UID)))
            Next
            bolGridFilling = True
            ResultGrid.DataSource = table
            ResultGrid.ClearSelection()
            bolGridFilling = False
            DisplayRecords(table.Rows.Count)
            table.Dispose()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Function BuildSearchList() As IEnumerable(Of SearchVal)
        Return New List(Of SearchVal) From
            {
            New SearchVal("dev_serial", Trim(txtSerialSearch.Text)),
            New SearchVal("dev_asset_tag", Trim(txtAssetTagSearch.Text)),
            New SearchVal(devices.Description, Trim(txtDescription.Text)),
            New SearchVal("dev_eq_type", GetDBValue(DeviceIndex.EquipType, cmbEquipType.SelectedIndex)),
            New SearchVal("dev_replacement_year", Trim(txtReplaceYear.Text)),
            New SearchVal("dev_osversion", GetDBValue(DeviceIndex.OSType, cmbOSType.SelectedIndex)),
            New SearchVal("dev_location", GetDBValue(DeviceIndex.Locations, cmbLocation.SelectedIndex)),
            New SearchVal("dev_cur_user", Trim(txtCurUser.Text)),
            New SearchVal("dev_status", GetDBValue(DeviceIndex.StatusType, cmbStatus.SelectedIndex)),
            New SearchVal("dev_trackable", chkTrackables.Checked)
                           }
    End Function
    Function BuildSearchListNew() As List(Of SearchVal)
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

        'If chkHistorical.Checked Then

        '    tmpList.Add(New SearchVal("hist_serial", Trim(txtSerialSearch.Text)))
        '    tmpList.Add(New SearchVal("hist_asset_tag", Trim(txtAssetTagSearch.Text)))
        '    tmpList.Add(New SearchVal("hist_description", Trim(txtDescription.Text)))
        '    tmpList.Add(New SearchVal("hist_eq_type", GetDBValue(DeviceIndex.EquipType, cmbEquipType.SelectedIndex)))
        '    tmpList.Add(New SearchVal("hist_replacement_year", Trim(txtReplaceYear.Text)))
        '    tmpList.Add(New SearchVal("hist_osversion", GetDBValue(DeviceIndex.OSType, cmbOSType.SelectedIndex)))
        '    tmpList.Add(New SearchVal("hist_location", GetDBValue(DeviceIndex.Locations, cmbLocation.SelectedIndex)))
        '    tmpList.Add(New SearchVal("hist_cur_user", Trim(txtCurUser.Text)))
        '    tmpList.Add(New SearchVal("hist_status", GetDBValue(DeviceIndex.StatusType, cmbStatus.SelectedIndex)))
        '    'tmpList.Add(New SearchVal("dev_trackable", chkTrackables.Checked))

        'End If


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
    Public Class SearchVal
        Public Property FieldName As String
        Public Property Value As Object
        Public Sub New(ByVal strFieldName As String, ByVal obValue As Object)
            FieldName = strFieldName
            Value = obValue
        End Sub
    End Class
    Public Sub DynamicSearch() 'dynamically creates sql query using any combination of search filters the users wants
        Dim table As New DataTable
        Dim cmd As New MySqlCommand
        Dim strStartQry As String
        If chkHistorical.Checked Then
            strStartQry = "SELECT * FROM dev_historical WHERE "
        Else
            strStartQry = "SELECT * FROM devices WHERE "
        End If
        Dim strDynaQry As String
        ' Dim SearchValCol As IEnumerable(Of SearchVal) = BuildSearchList()
        Dim SearchValCol As List(Of SearchVal) = BuildSearchListNew()
        For Each fld As SearchVal In SearchValCol
            If Not IsNothing(fld.Value) Then
                If fld.Value.ToString <> "" Then
                    If TypeOf fld.Value Is Boolean Then  'trackable boolean. if false, dont add it.
                        If fld.Value <> False Then
                            strDynaQry = strDynaQry + " " + fld.FieldName + " LIKE CONCAT('%', @" + fld.FieldName + ", '%') AND"
                            cmd.Parameters.AddWithValue("@" & fld.FieldName, Convert.ToInt32(fld.Value))
                        End If
                    Else
                        Select Case fld.FieldName 'use the fixed fields with EQUALS operator instead of LIKE
                            Case devices.OSVersion
                                strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                            Case "hist_osversion"
                                strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                            Case devices.EQType
                                strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                            Case "hist_eq_type"
                                strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                            Case devices.Location
                                strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                            Case "hist_location"
                                strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                            Case devices.Status
                                strDynaQry = strDynaQry + " " + fld.FieldName + "=@" + fld.FieldName + " AND"
                                cmd.Parameters.AddWithValue("@" & fld.FieldName, fld.Value)
                            Case "hist_status"
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
        LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value)
    End Sub
    Public Sub LoadDevice(ByVal strGUID As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not DeviceIsOpen(strGUID) Then
            Waiting()
            Dim NewView As New View
            NewView.ViewDevice(strGUID)
            ' If Not NewView.IsDisposed Then
            'NewView.Show()
            'NewView.Activate()
            'End If
            DoneWaiting()
        Else
            ' Dim blah = Message("That device is already open.", vbOKOnly + vbExclamation, "Duplicate Window")
            ActivateForm(strGUID)
        End If
    End Sub
    Private Sub RefreshCombos()
        FillComboBox(DeviceIndex.EquipType, cmbEquipType)
        FillComboBox(DeviceIndex.Locations, cmbLocation)
        FillComboBox(DeviceIndex.ChangeType, UpdateDev.cmbUpdate_ChangeType)
        FillComboBox(DeviceIndex.StatusType, AddNew.cmbStatus_REQ)
        FillComboBox(DeviceIndex.StatusType, cmbStatus)
        FillComboBox(DeviceIndex.OSType, cmbOSType)
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
    Private Sub NewToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If Not CheckForAccess(AccessGroup.Add) Then Exit Sub
        AddNew.Show()
    End Sub
    Private Sub txtSerial_TextChanged(sender As Object, e As EventArgs) Handles txtSerial.TextChanged
        MyLiveBox.StartLiveSearch(sender, MyLiveBox.LiveBoxType.InstaLoad, devices.Serial)
    End Sub
    Private Sub txtAssetTag_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag.TextChanged
        MyLiveBox.StartLiveSearch(sender, MyLiveBox.LiveBoxType.InstaLoad, devices.AssetTag)
    End Sub
    Private Sub txtDescription_KeyUp(sender As Object, e As KeyEventArgs) Handles txtDescription.KeyUp
        MyLiveBox.StartLiveSearch(sender, MyLiveBox.LiveBoxType.DynamicSearch, devices.Description)
    End Sub
    Private Sub txtCurUser_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCurUser.KeyUp
        MyLiveBox.StartLiveSearch(sender, MyLiveBox.LiveBoxType.DynamicSearch, devices.CurrentUser)
    End Sub
    Private Sub BigQueryWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles BigQueryWorker.DoWork
        Try
            Dim LocalSQLComm As New clsMySQL_Comms
            Dim QryComm As New MySqlCommand
            QryComm = DirectCast(e.Argument, Object)
            cmdLastCommand = QryComm
            Dim ds As New DataSet
            Dim da As New MySqlDataAdapter
            Dim conn As MySqlConnection = LocalSQLComm.NewConnection '(MySQLDB.MySQLConnectString)
            QryComm.Connection = conn
            BigQueryWorker.ReportProgress(1)
            da.SelectCommand = QryComm
            da.Fill(ds)
            da.Dispose()
            Debug.Print(ds.Tables(0).Rows.Count)
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
    'Private Sub cmdChangeDB_Click(sender As Object, e As EventArgs)
    '    If cmbDBs.Text <> "" And cmbDBs.Text <> MySQLDB.strDatabase Then
    '        MySQLDB.strDatabase = cmbDBs.Text
    '        MySQLDB.MySQLConnectString = "server=" & strServerIP & ";uid=asset_mgr_usr;pwd=" & DecodePassword(EncMySqlPass) & ";database=" & MySQLDB.strDatabase
    '        CloseConnections()
    '        GlobalConn = MySQLDB.NewConnection '(MySQLDB.MySQLConnectString)
    '        OpenConnections()
    '    End If
    'End Sub
    Private Sub ManageAttachmentsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ManageAttachmentsToolStripMenuItem.Click
        Dim ViewAttachments As New frmAttachments()
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
    'Private Sub cmbDBs_TextChanged(sender As Object, e As EventArgs) Handles cmbDBs.TextChanged
    '    If cmbDBs.Text <> "" And cmbDBs.Text <> MySQLDB.strDatabase Then
    '        MySQLDB.strDatabase = cmbDBs.Text
    '        MySQLDB.MySQLConnectString = "server=" & strServerIP & ";uid=asset_mgr_usr;pwd=" & DecodePassword(EncMySqlPass) & ";database=" & MySQLDB.strDatabase
    '        CloseConnections()
    '        GlobalConn = MySQLDB.NewConnection 'New MySqlConnection(MySQLDB.MySQLConnectString)
    '        OpenConnections()
    '    End If
    'End Sub
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
        frmSibiMain.Show()
        frmSibiMain.Activate()
    End Sub
    Private Sub tsmUserManager_Click(sender As Object, e As EventArgs) Handles tsmUserManager.Click
        frmUserManager.Show()
    End Sub
    Private Sub TextEnCrypterToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TextEnCrypterToolStripMenuItem.Click
        frmEncrypter.Show()
        frmEncrypter.Activate()
    End Sub
    Private Sub txtReplaceYear_KeyDown(sender As Object, e As KeyEventArgs) Handles txtReplaceYear.KeyDown
        If e.KeyCode = Keys.Down Then
            MyLiveBox.GiveLiveBoxFocus()
        End If
    End Sub
    Private Sub ResultGrid_KeyDown(sender As Object, e As KeyEventArgs) Handles ResultGrid.KeyDown
        If e.KeyCode = Keys.Enter Then
            LoadDevice(ResultGrid.Item(GetColIndex(ResultGrid, "GUID"), ResultGrid.CurrentRow.Index).Value)
            e.SuppressKeyPress = True
        End If
    End Sub
    Private Sub ScanAttachmentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ScanAttachmentToolStripMenuItem.Click
        FTP.ScanAttachements()
    End Sub
    Private Sub Button2_Click_1(sender As Object, e As EventArgs) Handles cmdSupDevSearch.Click
        SendToGrid(Asset.DevicesBySup())
    End Sub
    Private Sub MainForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Dim f As Form = sender
        If f.WindowState = FormWindowState.Minimized Then
            MinimizeAll()
        ElseIf f.WindowState = FormWindowState.Normal Then
            RestoreAll()
        End If
    End Sub
End Class
