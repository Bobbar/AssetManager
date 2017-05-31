Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.IO
Imports PingVisLib
Public Class frmView
    Private bolCheckFields As Boolean
    Public CurrentViewDevice As New Device_Info
    Public MunisUser As Emp_Info = Nothing
    Private OldData As New Device_Info
    Private bolGridFilling As Boolean = False
    Private MyPingHostname As String = Nothing
    Private DeviceHostname As String = Nothing
    Private MyPingVis As PingVis
    Private intFailedPings As Integer = 0
    Private DataParser As New DBControlParser
    Private Domain As String = Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties.DomainName
    Private MyMunisToolBar As New MunisToolBar(Me)
    Private MyLiveBox As New clsLiveBox(Me)
    Private MyWindowList As New WindowList(Me)
    Private Structure Ping_Results
        Public CanPing As Boolean
        Public Address As String
    End Structure
    Sub New(ParentForm As MyForm, DeviceGUID As String)
        InitializeComponent()
        InitDBControls()
        MyLiveBox.AttachToControl(txtCurUser_View_REQ, LiveBoxType.UserSelect, devices.CurrentUser, devices.Munis_Emp_Num)
        MyLiveBox.AttachToControl(txtDescription_View_REQ, LiveBoxType.SelectValue, devices.Description)
        MyMunisToolBar.InsertMunisDropDown(ToolStrip1, 6)
        MyWindowList.InsertWindowList(ToolStrip1)
        Tag = ParentForm
        Icon = ParentForm.Icon
        GridTheme = ParentForm.GridTheme
        RefreshCombos()
        grpNetTools.Visible = False
        '    ToolStrip1.BackColor = colAssetToolBarColor
        lblGUID.BackColor = SetBarColor(DeviceGUID)
        lblGUID.ForeColor = GetFontColor(lblGUID.BackColor)
        ExtendedMethods.DoubleBufferedDataGrid(DataGridHistory, True)
        ExtendedMethods.DoubleBufferedDataGrid(TrackingGrid, True)
        ViewDevice(DeviceGUID)
    End Sub
    Private Sub InitDBControls()
        'Required Fields
        txtAssetTag_View_REQ.Tag = New DBControlInfo(devices_main.AssetTag, True)
        txtSerial_View_REQ.Tag = New DBControlInfo(devices_main.Serial, True)
        txtCurUser_View_REQ.Tag = New DBControlInfo(devices_main.CurrentUser, True)
        txtDescription_View_REQ.Tag = New DBControlInfo(devices_main.Description, True)
        dtPurchaseDate_View_REQ.Tag = New DBControlInfo(devices_main.PurchaseDate, True)
        cmbEquipType_View_REQ.Tag = New DBControlInfo(devices_main.EQType, DeviceIndex.EquipType, True)
        cmbLocation_View_REQ.Tag = New DBControlInfo(devices_main.Location, DeviceIndex.Locations, True)
        cmbOSVersion_REQ.Tag = New DBControlInfo(devices_main.OSVersion, DeviceIndex.OSType, True)
        cmbStatus_REQ.Tag = New DBControlInfo(devices_main.Status, DeviceIndex.StatusType, True)

        'Non-required and Misc Fields
        txtPONumber.Tag = New DBControlInfo(devices_main.PO, False)
        txtReplacementYear_View.Tag = New DBControlInfo(devices_main.ReplacementYear, False)
        txtPhoneNumber.Tag = New DBControlInfo(devices_main.PhoneNumber, False)
        lblGUID.Tag = New DBControlInfo(devices_main.DeviceUID, False)
        chkTrackable.Tag = New DBControlInfo(devices_main.Trackable, False)
    End Sub
    Public Sub SetAttachCount()
        AttachmentTool.Text = "(" + Asset.GetAttachmentCount(CurrentViewDevice).ToString + ")"
        AttachmentTool.ToolTipText = "Attachments " + AttachmentTool.Text
    End Sub
    Private Sub GetCurrentValues()
        Using SQLComms As New clsMySQL_Comms
            OldData = Asset.CollectDeviceInfo(SQLComms.Return_SQLTable("SELECT * FROM " & devices.TableName & " WHERE " & devices.DeviceUID & " = '" & CurrentViewDevice.strGUID & "'"))
        End Using
    End Sub
    Private Function GetUpdateTable(Adapter As MySqlDataAdapter) As DataTable
        Dim tmpTable = DataParser.ReturnUpdateTable(Me, Adapter.SelectCommand.CommandText)
        Dim DBRow = tmpTable.Rows(0)
        'Add Add'l info
        If Not IsNothing(MunisUser.Number) Then
            DBRow(devices.CurrentUser) = MunisUser.Name
            DBRow(devices.Munis_Emp_Num) = MunisUser.Number
        Else
            If OldData.strCurrentUser <> Trim(txtCurUser_View_REQ.Text) Then
                DBRow(devices.CurrentUser) = Trim(txtCurUser_View_REQ.Text)
                DBRow(devices.Munis_Emp_Num) = DBNull.Value
            Else
                DBRow(devices.CurrentUser) = OldData.strCurrentUser
                DBRow(devices.Munis_Emp_Num) = CleanDBValue(OldData.strCurrentUserEmpNum)
            End If
        End If
        DBRow(devices.Sibi_Link_UID) = CleanDBValue(CurrentViewDevice.strSibiLink)
        DBRow(devices.LastMod_User) = strLocalUser
        DBRow(devices.LastMod_Date) = Now
        DBRow(devices.CheckSum) = GetHashOfTable(tmpTable)
        MunisUser = Nothing
        Return tmpTable
    End Function
    Private Function GetInsertTable(Adapter As MySqlDataAdapter, UpdateInfo As Update_Info) As DataTable
        Dim tmpTable = DataParser.ReturnInsertTable(Me, Adapter.SelectCommand.CommandText)
        Dim DBRow = tmpTable.Rows(0)
        'Add Add'l info
        DBRow(historical_dev.ChangeType) = UpdateInfo.strChangeType
        DBRow(historical_dev.Notes) = UpdateInfo.strNote
        DBRow(historical_dev.ActionUser) = strLocalUser
        DBRow(historical_dev.DeviceUID) = CurrentViewDevice.strGUID
        Return tmpTable
    End Function

    Private Sub EnableControls()
        Dim c As Control
        For Each c In DeviceInfoBox.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    Dim txt As TextBox = c
                    If txt.Name <> "txtGUID" Then
                        txt.ReadOnly = False
                    End If
                Case TypeOf c Is MaskedTextBox
                    Dim txt As MaskedTextBox = c
                    txt.ReadOnly = False
                Case TypeOf c Is ComboBox
                    Dim cmb As ComboBox = c
                    cmb.Enabled = True
                Case TypeOf c Is DateTimePicker
                    Dim dtp As DateTimePicker = c
                    dtp.Enabled = True
                Case TypeOf c Is CheckBox
                    c.Enabled = True
                Case TypeOf c Is Label
                    'do nut-zing
            End Select
        Next
        For Each c In pnlOtherFunctions.Controls
            c.Visible = False
        Next
        cmdSetSibi.Visible = True
        cmdMunisSearch.Visible = True
        Me.Text = "*View - MODIFYING*"
        tsSaveModify.Visible = True

    End Sub
    Private Sub DisableControls()
        Dim c As Control
        For Each c In DeviceInfoBox.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    Dim txt As TextBox = c
                    txt.ReadOnly = True
                Case TypeOf c Is MaskedTextBox
                    Dim txt As MaskedTextBox = c
                    txt.ReadOnly = True
                Case TypeOf c Is ComboBox
                    Dim cmb As ComboBox = c
                    cmb.Enabled = False
                Case TypeOf c Is DateTimePicker
                    Dim dtp As DateTimePicker = c
                    dtp.Enabled = False
                Case TypeOf c Is CheckBox
                    c.Enabled = False
                Case TypeOf c Is Label
                    'do nut-zing
            End Select
        Next
        For Each c In pnlOtherFunctions.Controls
            If c.Name IsNot "cmdRDP" Then c.Visible = True
        Next
        cmdSetSibi.Visible = False
        cmdMunisSearch.Visible = False
        Me.Text = "View"
        tsSaveModify.Visible = False
        tsTracking.Visible = False
    End Sub
    Public Sub UpdateDevice(UpdateInfo As Update_Info)
        Try
            Dim rows As Integer = 0
            Dim SelectQry As String = "SELECT * FROM " & devices.TableName & " WHERE " & devices.DeviceUID & "='" & CurrentViewDevice.strGUID & "'"
            Dim InsertQry As String = "SELECT * FROM " & historical_dev.TableName & " LIMIT 0"
            Using SQLComms As New clsMySQL_Comms,
                    UpdateAdapter = SQLComms.Return_Adapter(SelectQry),
                    InsertAdapter = SQLComms.Return_Adapter(InsertQry)
                rows += UpdateAdapter.Update(GetUpdateTable(UpdateAdapter))
                rows += InsertAdapter.Update(GetInsertTable(InsertAdapter, UpdateInfo))
            End Using
            If rows = 2 Then
                ViewDevice(CurrentViewDevice.strGUID)
                Dim blah = Message("Update Added.", vbOKOnly + vbInformation, "Success", Me)
            Else
                ViewDevice(CurrentViewDevice.strGUID)
                Dim blah = Message("Unsuccessful! The number of affected rows was not what was expected.", vbOKOnly + vbExclamation, "Unexpected Result", Me)
            End If
            Exit Sub
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
                ViewDevice(CurrentViewDevice.strGUID)
                Exit Sub
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Public Sub ViewDevice(ByVal DeviceUID As String)
        Try
            Waiting()
            bolGridFilling = True
            If ViewHistory(DeviceUID) Then
                ViewTracking(CurrentViewDevice.strGUID)
                Me.Text = Me.Text + FormTitle(CurrentViewDevice)
                Me.Show()
                DataGridHistory.ClearSelection()
                bolGridFilling = False
                DeviceHostname = "D" & CurrentViewDevice.strSerial & "." '& Domain
                MyPingHostname = DeviceHostname
                CheckRDP()
                tmr_RDPRefresher.Enabled = True
            Else
                Me.Dispose()
            End If
            DoneWaiting()
        Catch ex As Exception
            DoneWaiting()
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Function ViewHistory(ByVal DeviceUID As String) As Boolean
        Try
            Using SQLComms As New clsMySQL_Comms,
                    DeviceResults = SQLComms.Return_SQLTable("Select * FROM " & devices.TableName & " WHERE " & devices.DeviceUID & " = '" & DeviceUID & "'"),
                    HistoricalResults = SQLComms.Return_SQLTable("Select * FROM " & historical_dev.TableName & " WHERE " & historical_dev.DeviceUID & " = '" & DeviceUID & "' ORDER BY " & historical_dev.ActionDateTime & " DESC")
                If DeviceResults.Rows.Count < 1 Then
                    CloseChildren(Me)
                    CurrentViewDevice = Nothing
                    Dim blah = Message("That device was not found!  It may have been deleted.  Re-execute your search.", vbOKOnly + vbExclamation, "Not Found", Me)
                    Return False
                End If
                CurrentViewDevice = Asset.CollectDeviceInfo(DeviceResults)
                DataParser.FillDBFields(Me, DeviceResults)
                SetMunisEmpStatus()
                SendToHistGrid(DataGridHistory, HistoricalResults)
                DisableControls()
                SetAttachCount()
                Return True
            End Using
        Catch ex As Exception
            DoneWaiting()
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

    Private Sub SetMunisEmpStatus()
        ToolTip1.SetToolTip(txtCurUser_View_REQ, "")
        If CurrentViewDevice.strCurrentUserEmpNum <> "" Then
            txtCurUser_View_REQ.BackColor = colEditColor
            ToolTip1.SetToolTip(txtCurUser_View_REQ, "Munis Linked Employee")
        End If
    End Sub

    Private Sub SendToHistGrid(Grid As DataGridView, tblResults As DataTable)
        Dim table As New DataTable
        Try
            If tblResults.Rows.Count > 0 Then
                table.Columns.Add("Time Stamp", GetType(Date))
                table.Columns.Add("Change Type", GetType(String))
                table.Columns.Add("Action User", GetType(String))
                table.Columns.Add("Note Peek", GetType(String))
                table.Columns.Add("User", GetType(String))
                table.Columns.Add("Asset ID", GetType(String))
                table.Columns.Add("Serial", GetType(String))
                table.Columns.Add("Description", GetType(String))
                table.Columns.Add("Location", GetType(String))
                table.Columns.Add("Purchase Date", GetType(Date))
                table.Columns.Add("GUID", GetType(String))
                For Each r As DataRow In tblResults.Rows
                    table.Rows.Add(NoNull(r.Item(historical_dev.ActionDateTime)),
                           GetHumanValue(DeviceIndex.ChangeType, NoNull(r.Item(historical_dev.ChangeType))),
                           NoNull(r.Item(historical_dev.ActionUser)),
                           NotePreview(NoNull(r.Item(historical_dev.Notes)), 25),
                           NoNull(r.Item(historical_dev.CurrentUser)),
                           NoNull(r.Item(historical_dev.AssetTag)),
                           NoNull(r.Item(historical_dev.Serial)),
                           NoNull(r.Item(historical_dev.Description)),
                           GetHumanValue(DeviceIndex.Locations, NoNull(r.Item(historical_dev.Location))),
                           NoNull(r.Item(historical_dev.PurchaseDate)),
                           NoNull(r.Item(historical_dev.History_Entry_UID)))
                Next
                Grid.DataSource = table
                table.Dispose()
            Else
                table.Dispose()
                Grid.DataSource = Nothing
            End If
        Catch ex As Exception
            table.Dispose()
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub SendToTrackGrid(Grid As DataGridView, tblResults As DataTable)
        Dim table As New DataTable
        Try
            If tblResults.Rows.Count > 0 Then
                table.Columns.Add("Date", GetType(String))
                table.Columns.Add("Check Type", GetType(String))
                table.Columns.Add("Check Out User", GetType(String))
                table.Columns.Add("Check In User", GetType(String))
                table.Columns.Add("Check Out", GetType(String))
                table.Columns.Add("Check In", GetType(String))
                table.Columns.Add("Due Back", GetType(String))
                table.Columns.Add("Location", GetType(String))
                table.Columns.Add("GUID", GetType(String))
                For Each r As DataRow In tblResults.Rows
                    table.Rows.Add(NoNull(r.Item(trackable.DateStamp)),
                           NoNull(r.Item(trackable.CheckType)),
                           NoNull(r.Item(trackable.CheckOut_User)),
                           NoNull(r.Item(trackable.CheckIn_User)),
                           NoNull(r.Item(trackable.CheckOut_Time)),
                           NoNull(r.Item(trackable.CheckIn_Time)),
                           NoNull(r.Item(trackable.DueBackDate)),
                           NoNull(r.Item(trackable.UseLocation)),
                           NoNull(r.Item(trackable.UID)))
                Next
                Grid.DataSource = table
                table.Dispose()
            Else
                table.Dispose()
                Grid.DataSource = Nothing
            End If
        Catch ex As Exception
            table.Dispose()
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub Waiting()
        SetCursor(Cursors.WaitCursor)
        StatusBar("Processing...")
    End Sub
    Private Sub DoneWaiting()
        SetCursor(Cursors.Default)
        StatusBar("Idle...")
    End Sub
    Public Sub StatusBar(Text As String)
        StatusLabel.Text = Text
        StatusLabel.Invalidate()
    End Sub
    Public Sub ViewTracking(strGUID As String)
        Dim strQry = "Select * FROM " & trackable.TableName & ", " & devices.TableName & " WHERE " & trackable.DeviceUID & " = " & devices.DeviceUID & " And " & trackable.DeviceUID & " = '" & strGUID & "' ORDER BY " & trackable.DateStamp & " DESC"
        Try
            Using SQLComms As New clsMySQL_Comms, Results As DataTable = SQLComms.Return_SQLTable(strQry)
                If Results.Rows.Count > 0 Then
                    CollectCurrentTracking(Results)
                    SendToTrackGrid(TrackingGrid, Results)
                    DisableSorting(TrackingGrid)
                Else
                    Results.Dispose()
                    TrackingGrid.DataSource = Nothing
                End If
                FillTrackingBox()
                SetTracking(CurrentViewDevice.bolTrackable, CurrentViewDevice.Tracking.bolCheckedOut)
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            DoneWaiting()
        End Try
    End Sub
    Private Sub CollectCurrentTracking(Results As DataTable)
        With CurrentViewDevice
            .Tracking.strCheckOutTime = NoNull(Results.Rows(0).Item(trackable.CheckOut_Time))
            .Tracking.strCheckInTime = NoNull(Results.Rows(0).Item(trackable.CheckIn_Time))
            .Tracking.strUseLocation = NoNull(Results.Rows(0).Item(trackable.UseLocation))
            .Tracking.strCheckOutUser = NoNull(Results.Rows(0).Item(trackable.CheckOut_User))
            .Tracking.strCheckInUser = NoNull(Results.Rows(0).Item(trackable.CheckIn_User))
            .Tracking.strDueBackTime = NoNull(Results.Rows(0).Item(trackable.DueBackDate))
            .Tracking.strUseReason = NoNull(Results.Rows(0).Item(trackable.Notes))
        End With
    End Sub
    Private Sub DisableSorting(Grid As DataGridView)
        Dim c As DataGridViewColumn
        For Each c In Grid.Columns
            c.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub
    Private Sub FillTrackingBox()
        If CBool(CurrentViewDevice.Tracking.bolCheckedOut) Then
            txtCheckOut.BackColor = colCheckOut
            txtCheckLocation.Text = CurrentViewDevice.Tracking.strUseLocation
            lblCheckTime.Text = "CheckOut Time:"
            txtCheckTime.Text = CurrentViewDevice.Tracking.strCheckOutTime
            lblCheckUser.Text = "CheckOut User:"
            txtCheckUser.Text = CurrentViewDevice.Tracking.strCheckOutUser
            lblDueBack.Visible = True
            txtDueBack.Visible = True
            txtDueBack.Text = CurrentViewDevice.Tracking.strDueBackTime
        Else
            txtCheckOut.BackColor = colCheckIn
            txtCheckLocation.Text = GetHumanValue(DeviceIndex.Locations, CurrentViewDevice.strLocation)
            lblCheckTime.Text = "CheckIn Time:"
            txtCheckTime.Text = CurrentViewDevice.Tracking.strCheckInTime
            lblCheckUser.Text = "CheckIn User:"
            txtCheckUser.Text = CurrentViewDevice.Tracking.strCheckInUser
            lblDueBack.Visible = False
            txtDueBack.Visible = False
        End If
        txtCheckOut.Text = IIf(CurrentViewDevice.Tracking.bolCheckedOut, "Checked Out", "Checked In").ToString
    End Sub
    Public Sub SetTracking(bolEnabled As Boolean, bolCheckedOut As Boolean)
        If bolEnabled Then
            If Not TabControl1.TabPages.Contains(TrackingTab) Then TabControl1.TabPages.Insert(1, TrackingTab)
            SetGridStyle(DataGridHistory)
            SetGridStyle(TrackingGrid)
            DataGridHistory.DefaultCellStyle.SelectionBackColor = GridTheme.CellSelectColor
            TrackingBox.Visible = True
            tsTracking.Visible = bolEnabled
            CheckOutTool.Visible = Not bolCheckedOut
            CheckInTool.Visible = bolCheckedOut
        Else
            tsTracking.Visible = bolEnabled
            TabControl1.TabPages.Remove(TrackingTab)
            SetGridStyle(DataGridHistory)
            SetGridStyle(TrackingGrid)
            DataGridHistory.DefaultCellStyle.SelectionBackColor = GridTheme.CellSelectColor
            TrackingBox.Visible = False
        End If
    End Sub
    Private Sub ModifyDevice()
        GetCurrentValues()
        EnableControls()
    End Sub
    Private Sub ClearFields()
        Dim c As Control
        For Each c In DeviceInfoBox.Controls
            If TypeOf c Is TextBox Then
                Dim txt As TextBox = c
                txt.Text = ""
            End If
            If TypeOf c Is ComboBox Then
                Dim cmb As ComboBox = c
                cmb.SelectedIndex = -1
            End If
        Next
        fieldErrorIcon.Clear()
    End Sub
    Private Function CheckFields() As Boolean
        Dim bolMissingField As Boolean
        bolMissingField = False
        fieldErrorIcon.Clear()
        Dim c As Control
        For Each c In DeviceInfoBox.Controls
            Dim DBInfo As DBControlInfo = DirectCast(c.Tag, DBControlInfo)
            Select Case True
                Case TypeOf c Is TextBox
                    If DBInfo.Required Then
                        If Trim(c.Text) = "" Then
                            bolMissingField = True
                            c.BackColor = colMissingField
                            AddErrorIcon(c)
                        Else
                            c.BackColor = Color.Empty
                            ClearErrorIcon(c)
                        End If
                    End If
                Case TypeOf c Is ComboBox
                    Dim cmb As ComboBox = c
                    If DBInfo.Required Then
                        If cmb.SelectedIndex = -1 Then
                            bolMissingField = True
                            cmb.BackColor = colMissingField
                            AddErrorIcon(cmb)
                        Else
                            cmb.BackColor = Color.Empty
                            ClearErrorIcon(cmb)
                        End If
                    End If
            End Select
        Next
        Return Not bolMissingField 'if fields are missing return false to trigger a message if needed
    End Function
    Private Sub AddErrorIcon(ctl As Control)
        If fieldErrorIcon.GetError(ctl) Is String.Empty Then
            fieldErrorIcon.SetIconAlignment(ctl, ErrorIconAlignment.MiddleRight)
            fieldErrorIcon.SetIconPadding(ctl, 4)
            fieldErrorIcon.SetError(ctl, "Required Field")
        End If
    End Sub
    Private Sub ClearErrorIcon(ctl As Control)
        fieldErrorIcon.SetError(ctl, String.Empty)
    End Sub
    Private Sub ResetBackColors()
        Dim c As Control
        For Each c In DeviceInfoBox.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    c.BackColor = Color.Empty
                Case TypeOf c Is ComboBox
                    c.BackColor = Color.Empty
            End Select
        Next
    End Sub
    Private Sub cmdUpdate_Click(sender As Object, e As EventArgs)
        If Not CheckFields() Then
            Dim blah = Message("Some required fields are missing or invalid.  Please fill and or validate all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data", Me)
            bolCheckFields = True
            Exit Sub
        End If
        Dim UpdateDia As New UpdateDev(Me)
    End Sub
    Private Sub View_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.Dispose()
    End Sub
    Private Sub DataGridHistory_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridHistory.CellDoubleClick
        Dim EntryUID As String = DataGridHistory.Item(GetColIndex(DataGridHistory, "GUID"), DataGridHistory.CurrentRow.Index).Value.ToString
        If Not EntryIsOpen(EntryUID) Then
            NewEntryView(EntryUID)
        Else
            ActivateFormByUID(EntryUID)
        End If
    End Sub
    Private Sub NewEntryView(GUID As String)
        Waiting()
        Dim NewEntry As New View_Entry(Me, GUID)
        DoneWaiting()
    End Sub
    Private Sub NewMunisView(Device As Device_Info)
        Munis.NewMunisView_Device(Device, Me)
    End Sub
    Private Sub NewTrackingView(GUID As String)
        Waiting()
        Dim NewTracking As New View_Tracking(Me, GUID, CurrentViewDevice)
        DoneWaiting()
    End Sub
    Private Sub RefreshCombos()
        FillComboBox(DeviceIndex.EquipType, cmbEquipType_View_REQ)
        FillComboBox(DeviceIndex.Locations, cmbLocation_View_REQ)
        FillComboBox(DeviceIndex.OSType, cmbOSVersion_REQ)
        FillComboBox(DeviceIndex.StatusType, cmbStatus_REQ)
    End Sub
    Private Sub DeleteDevice()
        If Not CheckForAccess(AccessGroup.Delete) Then Exit Sub
        Dim blah = Message("Are you absolutely sure?  This cannot be undone and will delete all historical data, tracking and attachments.", vbYesNo + vbExclamation, "WARNING", Me)
        If blah = vbYes Then
            If Asset.DeleteMaster(CurrentViewDevice.strGUID, Entry_Type.Device) Then
                Dim blah2 = Message("Device deleted successfully.", vbOKOnly + vbInformation, "Device Deleted", Me)
                CurrentViewDevice = Nothing
                Me.Dispose()
                MainForm.RefreshCurrent()
            Else
                Logger("*****DELETION ERROR******: " & CurrentViewDevice.strGUID)
                Dim blah2 = Message("Failed to delete device succesfully!  Please let Bobby Lovell know about this.", vbOKOnly + vbCritical, "Delete Failed", Me)
                CurrentViewDevice = Nothing
                Me.Dispose()
            End If
        Else
            Exit Sub
        End If
    End Sub
    Private Sub txtAssetTag_View_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag_View_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub txtDescription_View_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtDescription_View_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmbEquipType_View_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEquipType_View_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub txtSerial_View_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtSerial_View_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmbLocation_View_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLocation_View_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmbOSVersion_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbOSVersion_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmbStatus_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatus_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub txtCurUser_View_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtCurUser_View_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub dtPurchaseDate_View_REQ_ValueChanged(sender As Object, e As EventArgs) Handles dtPurchaseDate_View_REQ.ValueChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub DeleteEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteEntryToolStripMenuItem.Click
        If Not CheckForAccess(AccessGroup.Modify) Then Exit Sub
        Dim strGUID As String = DataGridHistory.Item(GetColIndex(DataGridHistory, "GUID"), DataGridHistory.CurrentRow.Index).Value.ToString
        Dim Info As Device_Info = Asset.Get_EntryInfo(strGUID)
        Dim blah = Message("Are you absolutely sure?  This cannot be undone!" & vbCrLf & vbCrLf & "Entry info: " & Info.Historical.dtActionDateTime & " - " & Info.Historical.strChangeType & " - " & strGUID, vbYesNo + vbExclamation, "WARNING", Me)
        If blah = vbYes Then
            Dim blah2 = Message(DeleteHistoryEntry(strGUID) & " rows affected.", vbOKOnly + vbInformation, "Deletion Results", Me)
            ViewDevice(CurrentViewDevice.strGUID)
        Else
            Exit Sub
        End If
    End Sub
    Private Function DeleteHistoryEntry(ByVal strGUID As String) As Integer
        Try
            Dim rows As Integer
            Dim strSQLQry As String = "DELETE FROM " & historical_dev.TableName & " WHERE " & historical_dev.History_Entry_UID & "='" & strGUID & "'"
            Using SQLComms As New clsMySQL_Comms
                rows = SQLComms.Return_SQLCommand(strSQLQry).ExecuteNonQuery
                Return rows
            End Using
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
            Else
                EndProgram()
            End If
        End Try
    End Function
    Private Sub DataGridHistory_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridHistory.CellMouseDown
        If e.Button = MouseButtons.Right And e.ColumnIndex > -1 And e.RowIndex > -1 Then
            DataGridHistory.CurrentCell = DataGridHistory(e.ColumnIndex, e.RowIndex)
        End If
    End Sub
    Private Sub TrackingGrid_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles TrackingGrid.RowPrePaint
        Dim c1 As Color = ColorTranslator.FromHtml("#8BCEE8") 'highlight color
        TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.ForeColor = Color.Black
        TrackingGrid.Rows(e.RowIndex).Cells(GetColIndex(TrackingGrid, "Check Type")).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        If TrackingGrid.Rows(e.RowIndex).Cells(GetColIndex(TrackingGrid, "Check Type")).Value.ToString = strCheckIn Then
            TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.BackColor = colCheckIn
            Dim c2 As Color = Color.FromArgb(colCheckIn.R, colCheckIn.G, colCheckIn.B)
            Dim BlendColor As Color
            BlendColor = Color.FromArgb((CInt(c1.A) + CInt(c2.A)) / 2,
                                                (CInt(c1.R) + CInt(c2.R)) / 2,
                                                (CInt(c1.G) + CInt(c2.G)) / 2,
                                                (CInt(c1.B) + CInt(c2.B)) / 2)
            TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.SelectionBackColor = BlendColor
        ElseIf TrackingGrid.Rows(e.RowIndex).Cells(GetColIndex(TrackingGrid, "Check Type")).Value.ToString = strCheckOut Then
            TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.BackColor = colCheckOut
            Dim c2 As Color = Color.FromArgb(colCheckOut.R, colCheckOut.G, colCheckOut.B)
            Dim BlendColor As Color
            BlendColor = Color.FromArgb((CInt(c1.A) + CInt(c2.A)) / 2,
                                                (CInt(c1.R) + CInt(c2.R)) / 2,
                                                (CInt(c1.G) + CInt(c2.G)) / 2,
                                                (CInt(c1.B) + CInt(c2.B)) / 2)
            TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.SelectionBackColor = BlendColor
        End If
    End Sub
    Private Sub TrackingGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles TrackingGrid.CellDoubleClick
        NewTrackingView(TrackingGrid.Item(GetColIndex(TrackingGrid, "GUID"), TrackingGrid.CurrentRow.Index).Value.ToString)
    End Sub
    Private Sub tsbModify_Click(sender As Object, e As EventArgs) Handles tsbModify.Click
        If Not CheckForAccess(AccessGroup.Modify) Then Exit Sub
        ModifyDevice()
    End Sub
    Private Sub tsbNewNote_Click(sender As Object, e As EventArgs) Handles tsbNewNote.Click
        If Not CheckForAccess(AccessGroup.Modify) Then Exit Sub
        Dim UpdateDia As New UpdateDev(Me, True)
        If UpdateDia.DialogResult = DialogResult.OK Then
            If Not ConcurrencyCheck() Then
                CancelModify()
                Exit Sub
            End If
            GetCurrentValues()
            UpdateDevice(UpdateDia.UpdateInfo)
        Else
            CancelModify()
        End If
    End Sub
    Private Sub tsbDeleteDevice_Click(sender As Object, e As EventArgs) Handles tsbDeleteDevice.Click
        DeleteDevice()
    End Sub
    Private Sub CheckInTool_Click(sender As Object, e As EventArgs) Handles CheckInTool.Click
        If Not CheckForAccess(AccessGroup.Tracking) Then Exit Sub
        Waiting()
        Dim NewTracking As New Tracking(CurrentViewDevice, Me)
        DoneWaiting()
    End Sub
    Private Sub CheckOutTool_Click(sender As Object, e As EventArgs) Handles CheckOutTool.Click
        If Not CheckForAccess(AccessGroup.Tracking) Then Exit Sub
        Waiting()
        Dim NewTracking As New Tracking(CurrentViewDevice, Me)
        DoneWaiting()
    End Sub
    Private Sub AttachmentTool_Click(sender As Object, e As EventArgs) Handles AttachmentTool.Click
        If Not CheckForAccess(AccessGroup.ViewAttachment) Then Exit Sub
        If Not AttachmentsIsOpen() Then
            Dim NewAttachments As New frmAttachments(Me, CurrentViewDevice)
        Else
            ActivateFormByUID(CurrentViewDevice.strGUID)
        End If
    End Sub
    Public Function AttachmentsIsOpen() As Boolean
        For Each frm As Form In My.Application.OpenForms
            If TypeOf frm Is frmAttachments And frm.Tag Is Me Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Sub CancelModify()
        bolCheckFields = False
        DisableControls()
        ResetBackColors()
        Me.Refresh()
        ViewDevice(CurrentViewDevice.strGUID)
    End Sub
    Private Sub cmbEquipType_View_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbEquipType_View_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub
    Private Sub cmbLocation_View_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbLocation_View_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub
    Private Sub cmbOSVersion_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbOSVersion_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub
    Private Sub cmbStatus_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbStatus_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub
    Private Sub cmdAccept_Tool_Click(sender As Object, e As EventArgs) Handles cmdAccept_Tool.Click
        If Not CheckFields() Then
            Dim blah = Message("Some required fields are missing.  Please fill in all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data", Me)
            bolCheckFields = True
            Exit Sub
        End If
        DisableControls()
        Dim UpdateDia As New UpdateDev(Me)
        If UpdateDia.DialogResult = DialogResult.OK Then
            If Not ConcurrencyCheck() Then
                CancelModify()
                Exit Sub
            Else
                UpdateDevice(UpdateDia.UpdateInfo)
            End If
        Else
            CancelModify()
        End If
    End Sub
    Private Function ConcurrencyCheck() As Boolean
        Dim InDBCheckSum As String = Asset.Get_SQLValue(devices.TableName, devices.DeviceUID, CurrentViewDevice.strGUID, devices.CheckSum)
        If InDBCheckSum = CurrentViewDevice.CheckSum Then
            Return True
        Else
            Message("This record appears to have been modified by someone else since the start of this modification.", vbOKOnly + vbExclamation, "Concurrency Error", Me)
            Return False
        End If
    End Function
    Private Sub cmdCancel_Tool_Click(sender As Object, e As EventArgs) Handles cmdCancel_Tool.Click
        CancelModify()
    End Sub
    Private Sub TabControl1_MouseDown(sender As Object, e As MouseEventArgs) Handles TabControl1.MouseDown
        TrackingGrid.Refresh()
    End Sub
    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles cmdMunisInfo.Click
        NewMunisView(CurrentViewDevice)
    End Sub
    Private Sub View_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        MyWindowList = Nothing
        MyLiveBox.Dispose()
        CloseChildren(Me)
        If MyPingVis IsNot Nothing Then MyPingVis.Dispose()
    End Sub
    Private Sub TrackingGrid_Paint(sender As Object, e As PaintEventArgs) Handles TrackingGrid.Paint
        Try
            TrackingGrid.Columns("Check Type").DefaultCellStyle.Font = New Font(TrackingGrid.Font, FontStyle.Bold)
        Catch
        End Try
    End Sub
    Private Sub SetupNetTools(PingResults As PingReply)
        If PingResults.Status <> IPStatus.Success Then
            intFailedPings += 1
        Else
            intFailedPings = 0
        End If
        If Not grpNetTools.Visible And PingResults.Status = IPStatus.Success Then
            intFailedPings = 0
            cmdShowIP.Tag = PingResults.Address
            grpNetTools.Visible = True
        End If
        If intFailedPings > 10 And grpNetTools.Visible Then
            grpNetTools.Visible = False
        End If
    End Sub
    Private Sub tmr_RDPRefresher_Tick(sender As Object, e As EventArgs) Handles tmr_RDPRefresher.Tick
        CheckRDP()
    End Sub
    Private Sub CheckRDP()
        Try
            If CurrentViewDevice.strOSVersion.Contains("WIN") Then 'CurrentDevice.strEqType = "DESK" Or CurrentDevice.strEqType = "LAPT" Then
                If MyPingVis Is Nothing Then MyPingVis = New PingVis(cmdShowIP, MyPingHostname)
                If MyPingVis.CurrentResult IsNot Nothing Then
                    If MyPingVis.CurrentResult.Status = NetworkInformation.IPStatus.Success Then
                        SetupNetTools(MyPingVis.CurrentResult)
                    End If
                End If
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub cmdSibiLink_Click(sender As Object, e As EventArgs) Handles cmdSibiLink.Click
        If Not CheckForAccess(AccessGroup.Sibi_View) Then Exit Sub
        OpenSibiLink(CurrentViewDevice)
    End Sub
    Private Sub LinkSibi()
        Dim f As New frmSibiSelector(Me)
        If f.DialogResult = DialogResult.OK Then
            CurrentViewDevice.strSibiLink = f.SibiUID
            Message("Sibi Link Set.", vbOKOnly + vbInformation, "Success", Me)
        End If
    End Sub
    Private Sub OpenSibiLink(LinkDevice As Device_Info)
        Dim SibiUID As String
        If LinkDevice.strSibiLink = "" Then
            If LinkDevice.strPO = "" Then
                Message("A valid PO Number or Sibi Link is required.", vbOKOnly + vbInformation, "Missing Info", Me)
                Exit Sub
            Else
                SibiUID = Asset.Get_SQLValue(sibi_requests.TableName, sibi_requests.PO, LinkDevice.strPO, sibi_requests.UID)
            End If
        Else
            SibiUID = LinkDevice.strSibiLink
        End If
        If SibiUID = "" Then
            Message("No Sibi request found with matching PO number.", vbOKOnly + vbInformation, "Not Found", Me)
        Else
            If Not RequestIsOpen(SibiUID) Then
                Dim ManRequest As New frmManageRequest(Me, SibiUID)
            Else
                ActivateFormByUID(SibiUID)
            End If
        End If
    End Sub
    Private Sub cmdSetSibi_Click(sender As Object, e As EventArgs) Handles cmdSetSibi.Click
        LinkSibi()
    End Sub
    Private Sub cmdBrowseFiles_Click(sender As Object, e As EventArgs) Handles cmdBrowseFiles.Click
        Try
            Process.Start("\\D" & CurrentViewDevice.strSerial & "\c$")
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub tsmAssetInputForm_Click(sender As Object, e As EventArgs) Handles tsmAssetInputForm.Click
        If CurrentViewDevice.strPO <> "" Then
            Dim PDFForm As New PDFFormFilling(Me, CurrentViewDevice, PDFFormType.InputForm)
        Else
            Message("Please add a valid PO number to this device.", vbOKOnly + vbExclamation, "Missing Info", Me)
        End If
    End Sub
    Private Sub tsmAssetTransferForm_Click(sender As Object, e As EventArgs) Handles tsmAssetTransferForm.Click
        Dim PDFForm As New PDFFormFilling(Me, CurrentViewDevice, PDFFormType.TransferForm)
    End Sub
    Private Sub cmdMunisSearch_Click(sender As Object, e As EventArgs) Handles cmdMunisSearch.Click
        Dim NewMunisSearch As New frmMunisUser(Me)
        MunisUser = NewMunisSearch.EmployeeInfo
        If MunisUser.Name <> "" Then
            txtCurUser_View_REQ.Text = MunisUser.Name
            txtCurUser_View_REQ.ReadOnly = True
        End If
    End Sub
    Private Sub cmdRDP_Click(sender As Object, e As EventArgs) Handles cmdRDP.Click
        LaunchRDP()
    End Sub
    Private Sub LaunchRDP()
        Dim StartInfo As New ProcessStartInfo
        StartInfo.FileName = "mstsc.exe"
        StartInfo.Arguments = "/v:D" & CurrentViewDevice.strSerial
        Process.Start(StartInfo)
    End Sub
    Private Sub View_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            MinimizeChildren(Me)
        End If
    End Sub
    Private Sub lblGUID_Click(sender As Object, e As EventArgs) Handles lblGUID.Click
        Clipboard.SetText(lblGUID.Text)
        Message("GUID Copied to clipboard.", vbInformation + vbOKOnly,, Me)
    End Sub
    Private Sub cmdShowIP_Click(sender As Object, e As EventArgs) Handles cmdShowIP.Click
        If Not IsNothing(cmdShowIP.Tag) Then
            Dim blah = Message(cmdShowIP.Tag.ToString & vbCrLf & vbCrLf & "Press 'Yes' to copy to clipboard.", vbInformation + vbYesNo, "IP Address", Me)
            If blah = vbYes Then
                Clipboard.SetText(cmdShowIP.Tag.ToString)
            End If
        End If
    End Sub
    Private Sub AssetDisposalFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AssetDisposalForm.Click
        Dim PDFForm As New PDFFormFilling(Me, CurrentViewDevice, PDFFormType.DisposeForm)
    End Sub
    Private Sub DataGridHistory_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridHistory.CellEnter
        If Not bolGridFilling Then
            HighlightRow(DataGridHistory, GridTheme, e.RowIndex)
        End If
    End Sub
    Private Sub DataGridHistory_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridHistory.CellLeave
        LeaveRow(DataGridHistory, GridTheme, e.RowIndex)
    End Sub
    Private Sub cmdRestart_Click(sender As Object, e As EventArgs) Handles cmdRestart.Click
        Dim blah = Message("Click 'Yes' to reboot this device.", vbYesNo + vbQuestion, "Are you sure?")
        If blah = vbYes Then
            Dim IP As String = MyPingVis.CurrentResult.Address.ToString
            Dim DeviceName As String = "D" & CurrentViewDevice.strSerial
            If SendRestart(IP, DeviceName) Then
                Message("Success", vbOKOnly + vbInformation, "Restart Device", Me)
            Else
                Message("Failed", vbOKOnly + vbInformation, "Restart Device", Me)
            End If
        End If
    End Sub
    Private Function SendRestart(IP As String, DeviceName As String) As Boolean
        Try
            If VerifyAdminCreds() Then
                Dim FullPath As String = "\\" & IP & "\IPC$"
                Using NetCon As New NetworkConnection(FullPath, AdminCreds)
                    Dim p As Process = New Process
                    p.StartInfo.UseShellExecute = False
                    p.StartInfo.RedirectStandardOutput = True
                    p.StartInfo.RedirectStandardError = True
                    p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                    p.StartInfo.FileName = "shutdown.exe"
                    p.StartInfo.Arguments = "/m \\" & IP & " /f /r /t 0"
                    p.Start()
                    Dim output As String
                    output = p.StandardError.ReadToEnd
                    p.WaitForExit()
                    If Trim(output) = "" Then
                        Return True
                    Else
                        Return False
                    End If
                End Using
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
        Return False
    End Function
    Private Sub cmdGKUpdate_Click(sender As Object, e As EventArgs) Handles cmdGKUpdate.Click
        If VerifyAdminCreds() Then
            GKUpdater_Form.AddUpdate(CurrentViewDevice)
            If Not GKUpdater_Form.Visible Then GKUpdater_Form.Show()
        End If
    End Sub
    Private Sub txtPhoneNumber_Leave(sender As Object, e As EventArgs) Handles txtPhoneNumber.Leave
        If Trim(txtPhoneNumber.Text) <> "" And Not ValidPhoneNumber(txtPhoneNumber.Text) Then
            Message("Invalid phone number.", vbOKOnly + vbExclamation, "Error", Me)
            txtPhoneNumber.Focus()
        End If
    End Sub
End Class