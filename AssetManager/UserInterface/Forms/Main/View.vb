Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class View
    Private bolCheckFields As Boolean
    Public CurrentViewDevice As Device_Info
    Public MunisUser As Emp_Info = Nothing
    Private OldData As Device_Info
    Public NewData As Device_Info
    Private MyLiveBox As New clsLiveBox
    Private SQLComms As New clsMySQL_Comms
    Private Sub View_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim MyMunisMenu As New MunisToolsMenu
        MyMunisMenu.Tag = Me
        ToolStrip1.Items.Insert(6, MyMunisMenu.MunisTools)
        MyLiveBox.InitializeLiveBox()
        grpNetTools.Visible = False
        ToolStrip1.BackColor = colToolBarColor
        ExtendedMethods.DoubleBuffered(DataGridHistory, True)
        ExtendedMethods.DoubleBuffered(TrackingGrid, True)
        CheckRDP()
    End Sub
    Private Sub GetCurrentValues()
        OldData = Asset.CollectDeviceInfo(SQLComms.Return_SQLTable("SELECT * FROM devices WHERE dev_UID = '" & CurrentViewDevice.strGUID & "'"))
    End Sub
    Public Sub GetNewValues(UpdateInfo As Update_Info)
        With NewData
            .strAssetTag = Trim(txtAssetTag_View_REQ.Text)
            .strDescription = Trim(txtDescription_View_REQ.Text)
            .strEqType = GetDBValue(DeviceIndex.EquipType, cmbEquipType_View_REQ.SelectedIndex)
            .strSerial = Trim(txtSerial_View_REQ.Text)
            .strLocation = GetDBValue(DeviceIndex.Locations, cmbLocation_View_REQ.SelectedIndex)
            If Not IsNothing(MunisUser.Number) Then
                .strCurrentUser = MunisUser.Name
                .strCurrentUserEmpNum = MunisUser.Number
            Else
                If OldData.strCurrentUser <> Trim(txtCurUser_View_REQ.Text) Then
                    .strCurrentUser = Trim(txtCurUser_View_REQ.Text)
                    .strCurrentUserEmpNum = ""
                Else
                    .strCurrentUser = OldData.strCurrentUser
                    .strCurrentUserEmpNum = OldData.strCurrentUserEmpNum
                End If
            End If
            .dtPurchaseDate = dtPurchaseDate_View_REQ.Value.ToString(strDBDateFormat)
            .strReplaceYear = Trim(txtReplacementYear_View.Text)
            .strOSVersion = GetDBValue(DeviceIndex.OSType, cmbOSVersion_REQ.SelectedIndex)
            .strStatus = GetDBValue(DeviceIndex.StatusType, cmbStatus_REQ.SelectedIndex)
            .strNote = UpdateInfo.strNote
            .bolTrackable = chkTrackable.Checked
            .strPO = Trim(txtPONumber.Text)
        End With
        MunisUser = Nothing
    End Sub
    Private Sub EnableControls()
        Dim c As Control
        For Each c In DeviceInfoBox.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    Dim txt As TextBox = c
                    If txt.Name <> "txtGUID" Then
                        txt.ReadOnly = False
                    End If
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
        ToolStrip1.BackColor = colEditColor
        For Each t As ToolStripItem In ToolStrip1.Items
            If TypeOf t IsNot ToolStripSeparator Then
                t.Visible = False
            Else
                t.Visible = True
            End If
        Next
        cmdAccept_Tool.Visible = True
        cmdCancel_Tool.Visible = True
    End Sub
    Private Sub DisableControls()
        Dim c As Control
        For Each c In DeviceInfoBox.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    Dim txt As TextBox = c
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
        ToolStrip1.BackColor = colToolBarColor
        For Each t As ToolStripItem In ToolStrip1.Items
            If TypeOf t IsNot ToolStripSeparator Then
                t.Visible = True
            Else
                t.Visible = False
            End If
        Next
        cmdAccept_Tool.Visible = False
        cmdCancel_Tool.Visible = False
    End Sub
    Public Sub UpdateDevice(UpdateInfo As Update_Info)
        Try
            Dim rows As Integer
            Dim strSQLQry1 = "UPDATE devices SET dev_description=@dev_description, dev_location=@dev_location, dev_cur_user=@dev_cur_user, dev_serial=@dev_serial, dev_asset_tag=@dev_asset_tag, dev_purchase_date=@dev_purchase_date, dev_replacement_year=@dev_replacement_year, dev_osversion=@dev_osversion, dev_eq_type=@dev_eq_type, dev_status=@dev_status, dev_trackable=@dev_trackable, dev_po=@dev_po, dev_lastmod_user=@dev_lastmod_user, dev_lastmod_date=@dev_lastmod_date, dev_cur_user_emp_num=@dev_cur_user_emp_num WHERE dev_UID='" & CurrentViewDevice.strGUID & "'"
            Dim cmd As MySqlCommand = SQLComms.Return_SQLCommand(strSQLQry1)
            cmd.Parameters.AddWithValue("@dev_description", NewData.strDescription)
            cmd.Parameters.AddWithValue("@dev_location", NewData.strLocation)
            cmd.Parameters.AddWithValue("@dev_cur_user", NewData.strCurrentUser)
            cmd.Parameters.AddWithValue("@dev_cur_user_emp_num", NewData.strCurrentUserEmpNum)
            cmd.Parameters.AddWithValue("@dev_serial", NewData.strSerial)
            cmd.Parameters.AddWithValue("@dev_asset_tag", NewData.strAssetTag)
            cmd.Parameters.AddWithValue("@dev_purchase_date", NewData.dtPurchaseDate)
            cmd.Parameters.AddWithValue("@dev_replacement_year", NewData.strReplaceYear)
            cmd.Parameters.AddWithValue("@dev_osversion", NewData.strOSVersion)
            cmd.Parameters.AddWithValue("@dev_eq_type", NewData.strEqType)
            cmd.Parameters.AddWithValue("@dev_status", NewData.strStatus)
            cmd.Parameters.AddWithValue("@dev_trackable", Convert.ToInt32(NewData.bolTrackable))
            cmd.Parameters.AddWithValue("@dev_po", NewData.strPO)
            cmd.Parameters.AddWithValue("@dev_lastmod_user", strLocalUser)
            cmd.Parameters.AddWithValue("@dev_lastmod_date", Now)
            rows = rows + cmd.ExecuteNonQuery()
            Dim strSqlQry2 = "INSERT INTO dev_historical (hist_change_type,hist_notes,hist_serial,hist_description,hist_location,hist_cur_user,hist_asset_tag,hist_purchase_date,hist_replacement_year,hist_osversion,hist_dev_UID,hist_action_user,hist_eq_type,hist_status,hist_trackable,hist_po) VALUES (@hist_change_type,@hist_notes,@hist_serial,@hist_description,@hist_location,@hist_cur_user,@hist_asset_tag,@hist_purchase_date,@hist_replacement_year,@hist_osversion,@hist_dev_UID,@hist_action_user,@hist_eq_type,@hist_status,@hist_trackable,@hist_po)"
            cmd.CommandText = strSqlQry2
            cmd.Parameters.AddWithValue("@hist_change_type", UpdateInfo.strChangeType) 'GetDBValue(ChangeType, UpdateDev.cmbUpdate_ChangeType.SelectedIndex))
            cmd.Parameters.AddWithValue("@hist_notes", NewData.strNote)
            cmd.Parameters.AddWithValue("@hist_serial", NewData.strSerial)
            cmd.Parameters.AddWithValue("@hist_description", NewData.strDescription)
            cmd.Parameters.AddWithValue("@hist_location", NewData.strLocation)
            cmd.Parameters.AddWithValue("@hist_cur_user", NewData.strCurrentUser)
            cmd.Parameters.AddWithValue("@hist_asset_tag", NewData.strAssetTag)
            cmd.Parameters.AddWithValue("@hist_purchase_date", NewData.dtPurchaseDate)
            cmd.Parameters.AddWithValue("@hist_replacement_year", NewData.strReplaceYear)
            cmd.Parameters.AddWithValue("@hist_osversion", NewData.strOSVersion)
            cmd.Parameters.AddWithValue("@hist_dev_UID", CurrentViewDevice.strGUID)
            cmd.Parameters.AddWithValue("@hist_action_user", strLocalUser)
            cmd.Parameters.AddWithValue("@hist_eq_type", NewData.strEqType)
            cmd.Parameters.AddWithValue("@hist_status", NewData.strStatus)
            cmd.Parameters.AddWithValue("@hist_trackable", Convert.ToInt32(NewData.bolTrackable))
            cmd.Parameters.AddWithValue("@hist_po", NewData.strPO)
            rows = rows + cmd.ExecuteNonQuery()
            UpdateDev.strNewNote = Nothing
            cmd.Dispose()
            If rows = 2 Then
                ViewDevice(CurrentViewDevice.strGUID)
                Dim blah = Message("Update Added.", vbOKOnly + vbInformation, "Success")
            Else
                ViewDevice(CurrentViewDevice.strGUID)
                Dim blah = Message("Unsuccessful! The number of affected rows was not what was expected.", vbOKOnly + vbAbort, "Unexpected Result")
            End If
            Exit Sub
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                ViewDevice(CurrentViewDevice.strGUID)
                Exit Sub
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Public Sub ViewDevice(ByVal DeviceUID As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        Try
            Waiting()
            RefreshCombos()
            If ViewHistory(DeviceUID) Then
                ViewTracking(CurrentViewDevice.strGUID)
                Me.Text = Me.Text + FormTitle(CurrentViewDevice)
                Me.Show()
                Me.Activate()
            Else
                Me.Dispose()
            End If
            DoneWaiting()
        Catch ex As Exception
            DoneWaiting()
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Function ViewHistory(ByVal DeviceUID As String) As Boolean
        Dim table, Results As New DataTable
        Try
            Results = SQLComms.Return_SQLTable("Select * FROM devices, dev_historical WHERE dev_UID = hist_dev_UID And dev_UID = '" & DeviceUID & "' ORDER BY hist_action_datetime DESC")
            If Results.Rows.Count < 1 Then
                CloseChildren(Me)
                Results.Dispose()
                CurrentViewDevice = Nothing
                Dim blah = Message("That device was not found!  It may have been deleted.  Re-execute your search.", vbOKOnly + vbExclamation, "Not Found")
                Return False
            End If
            CurrentViewDevice = Asset.CollectDeviceInfo(Results)
            FillDeviceInfo()
            SendToHistGrid(DataGridHistory, Results)
            Results.Dispose()
            DisableControls()
            Return True
        Catch ex As Exception
            DoneWaiting()
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Results.Dispose()
            Return False
        End Try
    End Function
    Private Sub FillDeviceInfo()
        With CurrentViewDevice
            txtAssetTag_View_REQ.Text = .strAssetTag
            txtDescription_View_REQ.Text = .strDescription
            cmbEquipType_View_REQ.SelectedIndex = GetComboIndexFromShort(DeviceIndex.EquipType, .strEqType)
            txtSerial_View_REQ.Text = .strSerial
            cmbLocation_View_REQ.SelectedIndex = GetComboIndexFromShort(DeviceIndex.Locations, .strLocation)
            txtCurUser_View_REQ.Text = .strCurrentUser
            ToolTip1.SetToolTip(txtCurUser_View_REQ, "")
            If .strCurrentUserEmpNum <> "" Then
                txtCurUser_View_REQ.BackColor = colEditColor
                ToolTip1.SetToolTip(txtCurUser_View_REQ, "Munis Linked Employee")
            End If
            txtCurUser_View_REQ.Text = .strCurrentUser
            dtPurchaseDate_View_REQ.Value = .dtPurchaseDate
            txtReplacementYear_View.Text = .strReplaceYear
            cmbOSVersion_REQ.SelectedIndex = GetComboIndexFromShort(DeviceIndex.OSType, .strOSVersion)
            cmbStatus_REQ.SelectedIndex = GetComboIndexFromShort(DeviceIndex.StatusType, .strStatus)
            txtGUID.Text = .strGUID
            chkTrackable.Checked = CBool(.bolTrackable)
            txtPONumber.Text = .strPO
        End With
    End Sub
    Private Sub SendToHistGrid(Grid As DataGridView, tblResults As DataTable)
        Dim table As New DataTable
        Try
            If tblResults.Rows.Count > 0 Then
                table.Columns.Add("Date", GetType(String))
                table.Columns.Add("Action Type", GetType(String))
                table.Columns.Add("Action User", GetType(String))
                table.Columns.Add("Note Peek", GetType(String))
                table.Columns.Add("User", GetType(String))
                table.Columns.Add("Asset ID", GetType(String))
                table.Columns.Add("Serial", GetType(String))
                table.Columns.Add("Description", GetType(String))
                table.Columns.Add("Location", GetType(String))
                table.Columns.Add("Purchase Date", GetType(String))
                table.Columns.Add("GUID", GetType(String))
                For Each r As DataRow In tblResults.Rows
                    table.Rows.Add(NoNull(r.Item("hist_action_datetime")),
                           GetHumanValue(DeviceIndex.ChangeType, NoNull(r.Item("hist_change_type"))),
                           NoNull(r.Item("hist_action_user")),
                           NotePreview(NoNull(r.Item("hist_notes")), 25),
                           NoNull(r.Item("hist_cur_user")),
                           NoNull(r.Item("hist_asset_tag")),
                           NoNull(r.Item("hist_serial")),
                           NoNull(r.Item("hist_description")),
                           GetHumanValue(DeviceIndex.Locations, NoNull(r.Item("hist_location"))),
                           NoNull(r.Item("hist_purchase_date")),
                           NoNull(r.Item("hist_uid")))
                Next
                Grid.DataSource = table
                table.Dispose()
            Else
                table.Dispose()
                Grid.DataSource = Nothing
            End If
        Catch ex As Exception
            table.Dispose()
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
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
                    table.Rows.Add(NoNull(r.Item("track_datestamp")),
                           NoNull(r.Item("track_check_type")),
                           NoNull(r.Item("track_checkout_user")),
                           NoNull(r.Item("track_checkin_user")),
                           NoNull(r.Item("track_checkout_time")),
                           NoNull(r.Item("track_checkin_time")),
                           NoNull(r.Item("track_dueback_date")),
                           NoNull(r.Item("track_use_location")),
                           NoNull(r.Item("track_uid")))
                Next
                Grid.DataSource = table
                table.Dispose()
            Else
                table.Dispose()
                Grid.DataSource = Nothing
            End If
        Catch ex As Exception
            table.Dispose()
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub Waiting()
        Me.Cursor = Cursors.WaitCursor
        StatusBar("Processing...")
    End Sub
    Private Sub DoneWaiting()
        Me.Cursor = Cursors.Default
        StatusBar("Idle...")
    End Sub
    Public Sub StatusBar(Text As String)
        StatusLabel.Text = Text
        Me.Refresh()
    End Sub
    Public Sub ViewTracking(strGUID As String)
        Dim Results As New DataTable
        Dim strQry = "Select * FROM dev_trackable, devices WHERE track_device_uid = dev_UID And track_device_uid = '" & strGUID & "' ORDER BY track_datestamp DESC"
        Try
            If Not ConnectionReady() Then
                ConnectionNotReady()
                Exit Sub
            End If
            Waiting()
            Results = SQLComms.Return_SQLTable(strQry)
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
            Results.Dispose()
            DoneWaiting()
            Exit Sub
        Catch ex As Exception
            Results.Dispose()
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            DoneWaiting()
        End Try
    End Sub
    Private Sub CollectCurrentTracking(Results As DataTable)
        With CurrentViewDevice
            .Tracking.strCheckOutTime = NoNull(Results.Rows(0).Item("track_checkout_time"))
            .Tracking.strCheckInTime = NoNull(Results.Rows(0).Item("track_checkin_time"))
            .Tracking.strUseLocation = NoNull(Results.Rows(0).Item("track_use_location"))
            .Tracking.strCheckOutUser = NoNull(Results.Rows(0).Item("track_checkout_user"))
            .Tracking.strCheckInUser = NoNull(Results.Rows(0).Item("track_checkin_user"))
            .Tracking.strDueBackTime = NoNull(Results.Rows(0).Item("track_dueback_date"))
            .Tracking.strUseReason = NoNull(Results.Rows(0).Item("track_notes"))
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
        txtCheckOut.Text = IIf(CurrentViewDevice.Tracking.bolCheckedOut, "Checked Out", "Checked In")
    End Sub
    Public Sub SetTracking(bolEnabled As Boolean, bolCheckedOut As Boolean)
        If bolEnabled Then
            If Not TabControl1.TabPages.Contains(TrackingTab) Then TabControl1.TabPages.Insert(1, TrackingTab)
            SetGridStyle(DataGridHistory)
            SetGridStyle(TrackingGrid)
            TrackingBox.Visible = True
            TrackingTool.Visible = bolEnabled
            CheckOutTool.Visible = Not bolCheckedOut
            CheckInTool.Visible = bolCheckedOut
        Else
            TrackingTool.Visible = bolEnabled
            TabControl1.TabPages.Remove(TrackingTab)
            SetGridStyle(DataGridHistory)
            SetGridStyle(TrackingGrid)
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
        MyLiveBox.HideLiveBox()
    End Sub
    Private Function CheckFields() As Boolean
        Dim bolMissingField As Boolean
        bolMissingField = False
        Dim c As Control
        For Each c In DeviceInfoBox.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    If c.Name.Contains("REQ") Then
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
                    If cmb.Name.Contains("REQ") Then
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
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        EnableControls()
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs)
        DisableControls()
    End Sub
    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If Not CheckForAccess(AccessGroup.Modify) Then Exit Sub
        ModifyDevice()
    End Sub
    Private Sub cmdUpdate_Click(sender As Object, e As EventArgs)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not CheckFields() Then
            Dim blah = Message("Some required fields are missing.  Please fill in all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data")
            bolCheckFields = True
            Exit Sub
        End If
        UpdateDev.cmbUpdate_ChangeType.SelectedIndex = -1
        UpdateDev.cmbUpdate_ChangeType.Enabled = True
        UpdateDev.Show()
    End Sub
    Private Sub View_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.Dispose()
    End Sub
    Private Sub DataGridHistory_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridHistory.CellDoubleClick
        NewEntryView(DataGridHistory.Item(GetColIndex(DataGridHistory, "GUID"), DataGridHistory.CurrentRow.Index).Value)
    End Sub
    Private Sub NewEntryView(GUID As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        Dim NewEntry As New View_Entry
        Waiting()
        NewEntry.Tag = Me
        NewEntry.ViewEntry(GUID)
        NewEntry.Show()
        DoneWaiting()
    End Sub
    Private Sub NewMunisView(Device As Device_Info)
        Waiting()
        Munis.NewMunisView_Device(Device, Me)
        DoneWaiting()
    End Sub
    Private Sub NewTrackingView(GUID As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        Dim NewTracking As New View_Tracking
        Waiting()
        NewTracking.Tag = Me
        NewTracking.ViewTrackingEntry(GUID, CurrentViewDevice)
        NewTracking.Show()
        DoneWaiting()
    End Sub
    Private Sub AddNoteToolStripMenuItem_Click(sender As Object, e As EventArgs)
        If Not CheckForAccess(AccessGroup.Modify) Then Exit Sub
        UpdateDev.cmbUpdate_ChangeType.SelectedIndex = GetComboIndexFromShort(DeviceIndex.ChangeType, "NOTE")
        UpdateDev.cmbUpdate_ChangeType.Enabled = False
        UpdateDev.Show()
    End Sub
    Private Sub RefreshCombos()
        FillComboBox(DeviceIndex.EquipType, cmbEquipType_View_REQ)
        FillComboBox(DeviceIndex.Locations, cmbLocation_View_REQ)
        FillComboBox(DeviceIndex.OSType, cmbOSVersion_REQ)
        FillComboBox(DeviceIndex.StatusType, cmbStatus_REQ)
    End Sub
    Private Sub DeleteDevice()
        If Not CheckForAccess(AccessGroup.Delete) Then Exit Sub
        Dim blah = Message("Are you absolutely sure?" & vbCrLf & vbCrLf & "This cannot be undone and will delete all historical data.", vbYesNo + vbExclamation, "WARNING")
        If blah = vbYes Then
            Dim rows As Integer
            rows = Asset.DeleteMaster(CurrentViewDevice.strGUID, Entry_Type.Device)
            If rows > 0 Then
                Dim blah2 = Message("Device deleted successfully.", vbOKOnly + vbInformation, "Device Deleted")
                CurrentViewDevice = Nothing
                Me.Dispose()
            Else
                Logger("*****DELETION ERROR******: " & CurrentViewDevice.strGUID)
                Dim blah2 = Message("Failed to delete device succesfully!  Please let Bobby Lovell know about this.", vbOKOnly + vbCritical, "Delete Failed")
                CurrentViewDevice = Nothing
                Me.Dispose()
            End If
        Else
            Exit Sub
        End If
    End Sub
    Private Sub DeleteDeviceToolStripMenuItem_Click(sender As Object, e As EventArgs)
        DeleteDevice()
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
    Private Sub txtCurUser_View_REQ_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCurUser_View_REQ.KeyUp
        MyLiveBox.StartLiveSearch(sender, MyLiveBox.LiveBoxType.UserSelect, "dev_cur_user", "dev_cur_user_emp_num")
    End Sub
    Private Sub txtDescription_View_REQ_KeyUp(sender As Object, e As KeyEventArgs) Handles txtDescription_View_REQ.KeyUp
        MyLiveBox.StartLiveSearch(sender, MyLiveBox.LiveBoxType.SelectValue, "dev_description")
    End Sub
    Private Sub dtPurchaseDate_View_REQ_ValueChanged(sender As Object, e As EventArgs) Handles dtPurchaseDate_View_REQ.ValueChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmdCancel_Click(sender As Object, e As EventArgs)
        bolCheckFields = False
        DisableControls()
        ResetBackColors()
        Me.Refresh()
        ViewDevice(CurrentViewDevice.strGUID)
    End Sub
    Private Sub DeleteEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteEntryToolStripMenuItem.Click
        If Not CheckForAccess(AccessGroup.Modify) Then Exit Sub
        Dim strGUID As String = DataGridHistory.Item(GetColIndex(DataGridHistory, "GUID"), DataGridHistory.CurrentRow.Index).Value
        Dim Info As Device_Info = Asset.Get_EntryInfo(strGUID)
        Dim blah = Message("Are you absolutely sure?  This cannot be undone!" & vbCrLf & vbCrLf & "Entry info: " & Info.Historical.dtActionDateTime & " - " & Info.Historical.strChangeType & " - " & strGUID, vbYesNo + vbExclamation, "WARNING")
        If blah = vbYes Then
            Dim blah2 = Message(DeleteHistoryEntry(strGUID) & " rows affected.", vbOKOnly + vbInformation, "Deletion Results")
            ViewDevice(CurrentViewDevice.strGUID)
        Else
            Exit Sub
        End If
    End Sub
    Private Function DeleteHistoryEntry(ByVal strGUID As String) As Integer
        Try
            Dim rows
            Dim strSQLQry As String = "DELETE FROM dev_historical WHERE hist_uid='" & strGUID & "'"
            rows = SQLComms.Return_SQLCommand(strSQLQry).ExecuteNonQuery
            Return rows
            Exit Function
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                EndProgram()
            End If
        End Try
    End Function
    Private Sub DataGridHistory_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridHistory.CellMouseDown
        If e.Button = MouseButtons.Right Then
            DataGridHistory.CurrentCell = DataGridHistory(e.ColumnIndex, e.RowIndex)
        End If
    End Sub
    Private Sub TrackingGrid_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles TrackingGrid.RowPrePaint
        Dim c1 As Color = colHighlightBlue 'highlight color
        TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.ForeColor = Color.Black
        TrackingGrid.Rows(e.RowIndex).Cells(GetColIndex(TrackingGrid, "Check Type")).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        If TrackingGrid.Rows(e.RowIndex).Cells(GetColIndex(TrackingGrid, "Check Type")).Value = strCheckIn Then
            TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.BackColor = colCheckIn
            Dim c2 As Color = Color.FromArgb(colCheckIn.R, colCheckIn.G, colCheckIn.B)
            Dim BlendColor As Color
            BlendColor = Color.FromArgb((CInt(c1.A) + CInt(c2.A)) / 2,
                                                (CInt(c1.R) + CInt(c2.R)) / 2,
                                                (CInt(c1.G) + CInt(c2.G)) / 2,
                                                (CInt(c1.B) + CInt(c2.B)) / 2)
            TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.SelectionBackColor = BlendColor
        ElseIf TrackingGrid.Rows(e.RowIndex).Cells(GetColIndex(TrackingGrid, "Check Type")).Value = strCheckOut Then
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
    Private Sub Button1_Click_1(sender As Object, e As EventArgs)
        TrackingGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnMode.ColumnHeader
        TrackingGrid.AutoResizeColumns()
    End Sub
    Private Sub TrackingGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles TrackingGrid.CellDoubleClick
        NewTrackingView(TrackingGrid.Item(GetColIndex(TrackingGrid, "GUID"), TrackingGrid.CurrentRow.Index).Value)
    End Sub
    Private Sub ToolStripButton1_Click(sender As Object, e As EventArgs) Handles ToolStripButton1.Click
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not CheckForAccess(AccessGroup.Modify) Then Exit Sub
        ModifyDevice()
    End Sub
    Private Sub ToolStripButton2_Click(sender As Object, e As EventArgs) Handles ToolStripButton2.Click
        If Not CheckForAccess(AccessGroup.Modify) Then Exit Sub
        GetCurrentValues()
        Dim UpdateDia As New UpdateDev
        FillComboBox(DeviceIndex.ChangeType, UpdateDia.cmbUpdate_ChangeType)
        UpdateDia.cmbUpdate_ChangeType.SelectedIndex = GetComboIndexFromShort(DeviceIndex.ChangeType, "NOTE")
        UpdateDia.cmbUpdate_ChangeType.Enabled = False
        UpdateDia.txtUpdate_Note.Clear()
        UpdateDia.ShowDialog(Me)
        If UpdateDia.DialogResult = DialogResult.OK Then
            GetNewValues(UpdateDia.UpdateInfo)
            UpdateDevice(UpdateDia.UpdateInfo)
        Else
            CancelModify()
        End If
    End Sub
    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        If Not CheckForAccess(AccessGroup.Delete) Then Exit Sub
        Dim blah = Message("Are you absolutely sure?  This cannot be undone and will delete all histrical data, tracking and attachments.", vbYesNo + vbExclamation, "WARNING")
        If blah = vbYes Then
            If Asset.DeleteMaster(CurrentViewDevice.strGUID, Entry_Type.Device) Then
                Dim blah2 = Message("Device deleted successfully.", vbOKOnly + vbInformation, "Device Deleted")
                CurrentViewDevice = Nothing
                Me.Dispose()
            Else
                Logger("*****DELETION ERROR******: " & CurrentViewDevice.strGUID)
                Dim blah2 = Message("Failed to delete device succesfully!  Please let Bobby Lovell know about this.", vbOKOnly + vbCritical, "Delete Failed")
                CurrentViewDevice = Nothing
                Me.Dispose()
            End If
        Else
            Exit Sub
        End If
    End Sub
    Private Sub CheckInTool_Click(sender As Object, e As EventArgs) Handles CheckInTool.Click
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not CheckForAccess(AccessGroup.Tracking) Then Exit Sub
        Waiting()
        Dim NewTracking As New Tracking
        NewTracking.SetupTracking(CurrentViewDevice, Me)
        NewTracking.Tag = Me
        NewTracking.Show()
        DoneWaiting()
    End Sub
    Private Sub CheckOutTool_Click(sender As Object, e As EventArgs) Handles CheckOutTool.Click
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not CheckForAccess(AccessGroup.Tracking) Then Exit Sub
        Waiting()
        Dim NewTracking As New Tracking
        NewTracking.SetupTracking(CurrentViewDevice, Me)
        NewTracking.Tag = Me
        NewTracking.Show()
        DoneWaiting()
    End Sub
    Private Sub AttachmentTool_Click(sender As Object, e As EventArgs) Handles AttachmentTool.Click
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not CheckForAccess(AccessGroup.ViewAttachment) Then Exit Sub
        If Not AttachmentsIsOpen() Then
            Dim NewAttachments As New Attachments
            NewAttachments.Tag = Me
            NewAttachments.LoadAttachments(CurrentViewDevice)
            NewAttachments.Activate()
            NewAttachments.Show()
        Else
            ActivateForm(CurrentViewDevice.strGUID)
        End If
    End Sub
    Public Function AttachmentsIsOpen() As Boolean
        For Each frm As Form In My.Application.OpenForms
            If frm.Name = "Attachments" And frm.Tag Is Me Then
                Return True
            End If
        Next
        Return False
    End Function
    Private DefGridBC As Color, DefGridSelCol As Color, bolGridFilling As Boolean = False
    Private Sub HighlightCurrentRow(Row As Integer)
        On Error Resume Next
        If Not bolGridFilling Then
            DefGridBC = TrackingGrid.Rows(Row).DefaultCellStyle.BackColor
            DefGridSelCol = TrackingGrid.Rows(Row).DefaultCellStyle.SelectionBackColor
            Dim BackColor As Color = DefGridBC
            Dim SelectColor As Color = DefGridSelCol
            Dim c1 As Color = colHighlightColor 'highlight color
            If Row > -1 Then
                For Each cell As DataGridViewCell In TrackingGrid.Rows(Row).Cells
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
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not CheckFields() Then
            Dim blah = Message("Some required fields are missing.  Please fill in all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data")
            bolCheckFields = True
            Exit Sub
        End If
        DisableControls()
        Dim UpdateDia As New UpdateDev
        FillComboBox(DeviceIndex.ChangeType, UpdateDia.cmbUpdate_ChangeType)
        UpdateDia.cmbUpdate_ChangeType.SelectedIndex = -1
        UpdateDia.cmbUpdate_ChangeType.Enabled = True
        UpdateDia.txtUpdate_Note.Clear()
        UpdateDia.ShowDialog(Me)
        If UpdateDia.DialogResult = DialogResult.OK Then
            GetNewValues(UpdateDia.UpdateInfo)
            UpdateDevice(UpdateDia.UpdateInfo)
        Else
            CancelModify()
        End If
    End Sub
    Private Sub cmdCancel_Tool_Click(sender As Object, e As EventArgs) Handles cmdCancel_Tool.Click
        CancelModify()
    End Sub
    Private Sub TabControl1_MouseDown(sender As Object, e As MouseEventArgs) Handles TabControl1.MouseDown
        TrackingGrid.Refresh()
    End Sub
    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles cmdMunisInfo.Click
        NewMunisView(CurrentViewDevice)
    End Sub
    Private Sub PingWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles PingWorker.DoWork
        Try
            e.Result = My.Computer.Network.Ping("D" & CurrentViewDevice.strSerial)
        Catch ex As Exception
            e.Result = ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub PingWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs) Handles PingWorker.RunWorkerCompleted
        If e.Result Then
            SetupNetTools()
        Else
            grpNetTools.Visible = False
        End If
    End Sub
    Private Sub View_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        MyLiveBox.Unload()
        Attachments.Dispose()
        Tracking.Dispose()
        CloseChildren(Me)
    End Sub
    Private Sub TrackingGrid_Paint(sender As Object, e As PaintEventArgs) Handles TrackingGrid.Paint
        On Error Resume Next
        TrackingGrid.Columns("Check Type").DefaultCellStyle.Font = New Font(TrackingGrid.Font, FontStyle.Bold)
    End Sub
    Private Sub SetupNetTools()
        grpNetTools.Visible = True
    End Sub
    Private Sub CheckRDP()
        If InStr(CurrentViewDevice.strOSVersion, "WIN") Then 'CurrentDevice.strEqType = "DESK" Or CurrentDevice.strEqType = "LAPT" Then
            If Not PingWorker.IsBusy Then PingWorker.RunWorkerAsync()
        End If
    End Sub
    Private Sub tmr_RDPRefresher_Tick(sender As Object, e As EventArgs) Handles tmr_RDPRefresher.Tick
        CheckRDP()
    End Sub
    Private Sub cmdSibiLink_Click(sender As Object, e As EventArgs) Handles cmdSibiLink.Click
        If Not CheckForAccess(AccessGroup.Sibi_View) Then Exit Sub
        If CurrentViewDevice.strSibiLink Is "" Then
            Dim blah = Message("Sibi Link not set.  Set one now?", vbYesNo + vbQuestion, "Sibi Link")
            If blah = vbYes Then
                LinkSibi()
            End If
        Else
            OpenSibiLink(CurrentViewDevice.strSibiLink)
        End If
    End Sub
    Private Sub LinkSibi()
        Dim f As New frmSibiSelector
        f.ShowDialog(Me)
        If f.DialogResult = DialogResult.OK Then
            Asset.Update_SQLValue("devices", "dev_sibi_link", f.SibiUID, "dev_UID", CurrentViewDevice.strGUID)
            ViewDevice(CurrentViewDevice.strGUID)
        End If
    End Sub
    Private Sub OpenSibiLink(SibiUID As String)
        Dim sibiForm As New frmManageRequest
        sibiForm.Tag = Me
        sibiForm.OpenRequest(SibiUID)
    End Sub
    Private Sub Button1_Click_3(sender As Object, e As EventArgs) Handles cmdSetSibi.Click
        LinkSibi()
    End Sub
    Private Sub cmdBrowseFiles_Click(sender As Object, e As EventArgs) Handles cmdBrowseFiles.Click
        Try
            Process.Start("\\D" & CurrentViewDevice.strSerial & "\c$")
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub tsmAssetInputForm_Click(sender As Object, e As EventArgs) Handles tsmAssetInputForm.Click
        FillForm(CurrentViewDevice, FormType.InputForm)
    End Sub
    Private Sub tsmAssetTransferForm_Click(sender As Object, e As EventArgs) Handles tsmAssetTransferForm.Click
        'Dim newDialog As New MyDialog
        'With newDialog
        '    .Text = "Error Dialog"
        '    .AddLabel("Blah blah blah blah blah blah Blah blah blah blah blah blahBlah blah blah blah blah blahBlah blah blah blah blah blahBlah blah blah blah blah blah")
        '    .ShowDialog()
        'End With
        'ListFieldNames()
        FillForm(CurrentViewDevice, FormType.TransferForm)
    End Sub

    Private Sub cmdMunisSearch_Click(sender As Object, e As EventArgs) Handles cmdMunisSearch.Click
        Dim NewMunisSearch As New frmMunisUser
        NewMunisSearch.ShowDialog()
        If NewMunisSearch.DialogResult = DialogResult.Yes Then
            MunisUser = NewMunisSearch.EmployeeInfo
            NewMunisSearch.Dispose()
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
        Dim f As Form = sender
        If f.WindowState = FormWindowState.Minimized Then
            MinimizeChildren(Me)
            PrevWindowState = f.WindowState
        ElseIf f.WindowState <> PrevWindowState And f.WindowState = FormWindowState.Normal Then
            If PrevWindowState <> FormWindowState.Maximized Then RestoreChildren(Me)
        End If
    End Sub
    Private PrevWindowState As Integer
    Private Sub View_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin
        Dim f As Form = sender
        PrevWindowState = f.WindowState
    End Sub
End Class