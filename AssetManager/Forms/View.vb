Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class View
    Private Children(0) As Form
    Private bolCheckFields As Boolean

    Private Structure UserInput
        Public strAssetTag As String
        Public strDescription As String
        Public strEqType As String
        Public strSerial As String
        Public strLocation As String
        Public strCurrentUser As String
        Public dtPurchaseDate As String
        Public strReplaceYear As String
        Public strOSVersion As String
    End Structure
    Private OldData As Device_Info
    Public NewData As Device_Info
    'Private fieldErrorIcon As ErrorProvider = New ErrorProvider
    Private Sub View_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        grpNetTools.Visible = False
        ToolStrip1.BackColor = colToolBarColor
        ExtendedMethods.DoubleBuffered(DataGridHistory, True)
        ExtendedMethods.DoubleBuffered(TrackingGrid, True)
        CheckRDP()
    End Sub
    Private Sub GetCurrentValues()
        With OldData
            .strAssetTag = Trim(txtAssetTag_View_REQ.Text)
            .strDescription = Trim(txtDescription_View_REQ.Text)
            .strEqType = GetDBValue(EquipType, cmbEquipType_View_REQ.SelectedIndex)
            .strSerial = Trim(txtSerial_View_REQ.Text)
            .strLocation = GetDBValue(Locations, cmbLocation_View_REQ.SelectedIndex)
            .strCurrentUser = Trim(txtCurUser_View_REQ.Text)
            .dtPurchaseDate = dtPurchaseDate_View_REQ.Value.ToString(strDBDateFormat)
            .strReplaceYear = Trim(txtReplacementYear_View.Text)
            .strOSVersion = GetDBValue(OSType, cmbOSVersion_REQ.SelectedIndex)
            .strStatus = GetDBValue(StatusType, cmbStatus_REQ.SelectedIndex)
            .bolTrackable = chkTrackable.Checked
            .strPO = Trim(txtPONumber.Text)
        End With
    End Sub
    Public Sub GetNewValues()
        With NewData
            .strAssetTag = Trim(txtAssetTag_View_REQ.Text)
            .strDescription = Trim(txtDescription_View_REQ.Text)
            .strEqType = GetDBValue(EquipType, cmbEquipType_View_REQ.SelectedIndex)
            .strSerial = Trim(txtSerial_View_REQ.Text)
            .strLocation = GetDBValue(Locations, cmbLocation_View_REQ.SelectedIndex)
            .strCurrentUser = Trim(txtCurUser_View_REQ.Text)
            .dtPurchaseDate = dtPurchaseDate_View_REQ.Value.ToString(strDBDateFormat)
            .strReplaceYear = Trim(txtReplacementYear_View.Text)
            .strOSVersion = GetDBValue(OSType, cmbOSVersion_REQ.SelectedIndex)
            .strStatus = GetDBValue(StatusType, cmbStatus_REQ.SelectedIndex)
            .strNote = UpdateDev.strNewNote
            .bolTrackable = chkTrackable.Checked
            .strPO = Trim(txtPONumber.Text)
        End With
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
    Public Sub UpdateDevice()
        Try
            Dim rows As Integer
            Dim strSQLQry1 = "UPDATE devices SET dev_description=@dev_description, dev_location=@dev_location, dev_cur_user=@dev_cur_user, dev_serial=@dev_serial, dev_asset_tag=@dev_asset_tag, dev_purchase_date=@dev_purchase_date, dev_replacement_year=@dev_replacement_year, dev_osversion=@dev_osversion, dev_eq_type=@dev_eq_type, dev_status=@dev_status, dev_trackable=@dev_trackable, dev_po=@dev_po WHERE dev_UID='" & CurrentDevice.strGUID & "'"
            Dim cmd As MySqlCommand = Return_SQLCommand(strSQLQry1)
            cmd.Parameters.AddWithValue("@dev_description", NewData.strDescription)
            cmd.Parameters.AddWithValue("@dev_location", NewData.strLocation)
            cmd.Parameters.AddWithValue("@dev_cur_user", NewData.strCurrentUser)
            cmd.Parameters.AddWithValue("@dev_serial", NewData.strSerial)
            cmd.Parameters.AddWithValue("@dev_asset_tag", NewData.strAssetTag)
            cmd.Parameters.AddWithValue("@dev_purchase_date", NewData.dtPurchaseDate)
            cmd.Parameters.AddWithValue("@dev_replacement_year", NewData.strReplaceYear)
            cmd.Parameters.AddWithValue("@dev_osversion", NewData.strOSVersion)
            cmd.Parameters.AddWithValue("@dev_eq_type", NewData.strEqType)
            cmd.Parameters.AddWithValue("@dev_status", NewData.strStatus)
            cmd.Parameters.AddWithValue("@dev_trackable", Convert.ToInt32(NewData.bolTrackable))
            cmd.Parameters.AddWithValue("@dev_po", NewData.strPO)
            rows = rows + cmd.ExecuteNonQuery()
            Dim strSqlQry2 = "INSERT INTO dev_historical (hist_change_type,hist_notes,hist_serial,hist_description,hist_location,hist_cur_user,hist_asset_tag,hist_purchase_date,hist_replacement_year,hist_osversion,hist_dev_UID,hist_action_user,hist_eq_type,hist_status,hist_trackable,hist_po) VALUES (@hist_change_type,@hist_notes,@hist_serial,@hist_description,@hist_location,@hist_cur_user,@hist_asset_tag,@hist_purchase_date,@hist_replacement_year,@hist_osversion,@hist_dev_UID,@hist_action_user,@hist_eq_type,@hist_status,@hist_trackable,@hist_po)"
            cmd.CommandText = strSqlQry2
            cmd.Parameters.AddWithValue("@hist_change_type", GetDBValue(ChangeType, UpdateDev.cmbUpdate_ChangeType.SelectedIndex))
            cmd.Parameters.AddWithValue("@hist_notes", NewData.strNote)
            cmd.Parameters.AddWithValue("@hist_serial", NewData.strSerial)
            cmd.Parameters.AddWithValue("@hist_description", NewData.strDescription)
            cmd.Parameters.AddWithValue("@hist_location", NewData.strLocation)
            cmd.Parameters.AddWithValue("@hist_cur_user", NewData.strCurrentUser)
            cmd.Parameters.AddWithValue("@hist_asset_tag", NewData.strAssetTag)
            cmd.Parameters.AddWithValue("@hist_purchase_date", NewData.dtPurchaseDate)
            cmd.Parameters.AddWithValue("@hist_replacement_year", NewData.strReplaceYear)
            cmd.Parameters.AddWithValue("@hist_osversion", NewData.strOSVersion)
            cmd.Parameters.AddWithValue("@hist_dev_UID", CurrentDevice.strGUID)
            cmd.Parameters.AddWithValue("@hist_action_user", strLocalUser)
            cmd.Parameters.AddWithValue("@hist_eq_type", NewData.strEqType)
            cmd.Parameters.AddWithValue("@hist_status", NewData.strStatus)
            cmd.Parameters.AddWithValue("@hist_trackable", Convert.ToInt32(NewData.bolTrackable))
            cmd.Parameters.AddWithValue("@hist_po", NewData.strPO)
            rows = rows + cmd.ExecuteNonQuery()
            UpdateDev.strNewNote = Nothing
            cmd.Dispose()
            If rows = 2 Then
                ViewDevice(CurrentDevice.strGUID)
                Dim blah = MsgBox("Update Added.", vbOKOnly + vbInformation, "Success")
            Else
                ViewDevice(CurrentDevice.strGUID)
                Dim blah = MsgBox("Unsuccessful! The number of affected rows was not what was expected.", vbOKOnly + vbAbort, "Unexpected Result")
            End If
            Exit Sub
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                ViewDevice(CurrentDevice.strGUID)
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
            ClearFields()
            RefreshCombos()
            If ViewHistory(DeviceUID) Then
                ViewTracking(CurrentDevice.strGUID)
                DoneWaiting()
                Me.Show()
                Me.Activate()
            End If
        Catch ex As Exception
            DoneWaiting()
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Function ViewHistory(ByVal DeviceUID As String) As Boolean
        Dim table, Results As New DataTable
        Try
            Results = Return_SQLTable("Select * FROM devices, dev_historical WHERE dev_UID = hist_dev_UID And dev_UID = '" & DeviceUID & "' ORDER BY hist_action_datetime DESC")
            If Results.Rows.Count < 1 Then
                CloseChildren()
                Results.Dispose()
                CurrentDevice = Nothing
                Me.Dispose()
                Dim blah = MsgBox("That device was not found!  It may have been deleted.  Re-execute your search.", vbOKOnly + vbExclamation, "Not Found")
                Return False
            End If
            CurrentDevice = CollectDeviceInfo(Results)
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
        With CurrentDevice
            txtAssetTag_View_REQ.Text = .strAssetTag
            txtDescription_View_REQ.Text = .strDescription
            cmbEquipType_View_REQ.SelectedIndex = GetComboIndexFromShort(ComboType.EquipType, .strEqType)
            txtSerial_View_REQ.Text = .strSerial
            cmbLocation_View_REQ.SelectedIndex = GetComboIndexFromShort(ComboType.Location, .strLocation)
            txtCurUser_View_REQ.Text = .strCurrentUser
            dtPurchaseDate_View_REQ.Value = .dtPurchaseDate
            txtReplacementYear_View.Text = .strReplaceYear
            cmbOSVersion_REQ.SelectedIndex = GetComboIndexFromShort(ComboType.OSType, .strOSVersion)
            cmbStatus_REQ.SelectedIndex = GetComboIndexFromShort(ComboType.StatusType, .strStatus)
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
                table.Columns.Add("User", GetType(String))
                table.Columns.Add("Asset ID", GetType(String))
                table.Columns.Add("Serial", GetType(String))
                table.Columns.Add("Description", GetType(String))
                table.Columns.Add("Location", GetType(String))
                table.Columns.Add("Purchase Date", GetType(String))
                table.Columns.Add("GUID", GetType(String))
                For Each r As DataRow In tblResults.Rows
                    table.Rows.Add(r.Item("hist_action_datetime"),
                           GetHumanValue(ComboType.ChangeType, r.Item("hist_change_type")),
                           r.Item("hist_action_user"),
                           r.Item("hist_cur_user"),
                           r.Item("hist_asset_tag"),
                           r.Item("hist_serial"),
                           r.Item("hist_description"),
                           GetHumanValue(ComboType.Location, r.Item("hist_location")),
                           r.Item("hist_purchase_date"),
                           r.Item("hist_uid"))
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
                    table.Rows.Add(r.Item("track_datestamp"),
                           r.Item("track_check_type"),
                           r.Item("track_checkout_user"),
                           r.Item("track_checkin_user"),
                           r.Item("track_checkout_time"),
                           r.Item("track_checkin_time"),
                           r.Item("track_dueback_date"),
                           r.Item("track_use_location"),
                           r.Item("track_uid"))
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
            Results = Return_SQLTable(strQry)
            If Results.Rows.Count > 0 Then
                CollectCurrentTracking(Results)
                SendToTrackGrid(TrackingGrid, Results)
                DisableSorting(TrackingGrid)
            Else
                Results.Dispose()
                TrackingGrid.DataSource = Nothing
            End If
            FillTrackingBox()
            SetTracking(CurrentDevice.bolTrackable, CurrentDevice.Tracking.bolCheckedOut)
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
        CurrentDevice.Tracking.strCheckOutTime = NoNull(Results.Rows(0).Item("track_checkout_time"))
        CurrentDevice.Tracking.strCheckInTime = NoNull(Results.Rows(0).Item("track_checkin_time"))
        CurrentDevice.Tracking.strUseLocation = NoNull(Results.Rows(0).Item("track_use_location"))
        CurrentDevice.Tracking.strCheckOutUser = NoNull(Results.Rows(0).Item("track_checkout_user"))
        CurrentDevice.Tracking.strCheckInUser = NoNull(Results.Rows(0).Item("track_checkin_user"))
        CurrentDevice.Tracking.strDueBackTime = NoNull(Results.Rows(0).Item("track_dueback_date"))
        CurrentDevice.Tracking.strUseReason = NoNull(Results.Rows(0).Item("track_notes"))
    End Sub
    Private Sub DisableSorting(Grid As DataGridView)
        Dim c As DataGridViewColumn
        For Each c In Grid.Columns
            c.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub
    Private Sub FillTrackingBox()
        If CBool(CurrentDevice.Tracking.bolCheckedOut) Then
            txtCheckOut.BackColor = colCheckOut
            txtCheckLocation.Text = CurrentDevice.Tracking.strUseLocation
            lblCheckTime.Text = "CheckOut Time:"
            txtCheckTime.Text = CurrentDevice.Tracking.strCheckOutTime
            lblCheckUser.Text = "CheckOut User:"
            txtCheckUser.Text = CurrentDevice.Tracking.strCheckOutUser
            lblDueBack.Visible = True
            txtDueBack.Visible = True
            txtDueBack.Text = CurrentDevice.Tracking.strDueBackTime
        Else
            txtCheckOut.BackColor = colCheckIn
            txtCheckLocation.Text = GetHumanValue(ComboType.Location, CurrentDevice.strLocation)
            lblCheckTime.Text = "CheckIn Time:"
            txtCheckTime.Text = CurrentDevice.Tracking.strCheckInTime
            lblCheckUser.Text = "CheckIn User:"
            txtCheckUser.Text = CurrentDevice.Tracking.strCheckInUser
            lblDueBack.Visible = False
            txtDueBack.Visible = False
        End If
        txtCheckOut.Text = IIf(CurrentDevice.Tracking.bolCheckedOut, "Checked Out", "Checked In")
    End Sub
    Public Sub SetTracking(bolEnabled As Boolean, bolCheckedOut As Boolean)
        If bolEnabled Then
            TrackingToolStripMenuItem.Visible = True
            If Not TabControl1.TabPages.Contains(TrackingTab) Then TabControl1.TabPages.Insert(1, TrackingTab)
            MainForm.CopyDefaultCellStyles()
            TrackingBox.Visible = True
            CheckOutMenu.Visible = Not bolCheckedOut
            CheckInMenu.Visible = bolCheckedOut
            TrackingTool.Visible = bolEnabled
            CheckOutTool.Visible = Not bolCheckedOut
            CheckInTool.Visible = bolCheckedOut
        Else
            TrackingTool.Visible = bolEnabled
            TrackingToolStripMenuItem.Visible = False
            TabControl1.TabPages.Remove(TrackingTab)
            MainForm.CopyDefaultCellStyles()
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
        HideLiveBox()
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
    Private Sub EditToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles EditToolStripMenuItem.Click
        If Not CheckForAccess(AccessGroup.Modify) Then Exit Sub
        ModifyDevice()
    End Sub
    Private Sub cmdUpdate_Click(sender As Object, e As EventArgs)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not CheckFields() Then
            Dim blah = MsgBox("Some required fields are missing.  Please fill in all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data")
            bolCheckFields = True
            Exit Sub
        End If
        UpdateDev.cmbUpdate_ChangeType.SelectedIndex = -1
        UpdateDev.cmbUpdate_ChangeType.Enabled = True
        UpdateDev.Show()
    End Sub
    Private Sub View_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.Dispose()
        Attachments.Dispose()
        Tracking.Dispose()
        CloseChildren()
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
        AddChild(NewEntry)
        NewEntry.ViewEntry(GUID)
        NewEntry.Show()
        DoneWaiting()
    End Sub
    Private Sub NewMunisView(Device As Device_Info)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        Dim NewMunis As New View_Munis
        Waiting()
        AddChild(NewMunis)
        NewMunis.Show()
        NewMunis.LoadMunisInfoByDevice(Device)
        ' NewMunis.ViewEntry(GUID)
        DoneWaiting()
    End Sub
    Private Sub NewMunisViewEmp(Name As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        Dim SplitName() As String = Split(Name, " ")
        Dim LastName As String = SplitName(SplitName.Count - 1)
        Dim NewMunis As New View_Munis
        Waiting()
        AddChild(NewMunis)
        NewMunis.HideFixedAssetGrid()
        NewMunis.Show()
        NewMunis.LoadMunisEmployeeByLastName(LastName)
        ' NewMunis.ViewEntry(GUID)
        DoneWaiting()
    End Sub
    Private Sub NewTrackingView(GUID As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        Dim NewTracking As New View_Tracking
        Waiting()
        AddChild(NewTracking)
        NewTracking.ViewTrackingEntry(GUID)
        NewTracking.Show()
        DoneWaiting()
    End Sub
    Private Sub AddChild(form As Form)
        ReDim Preserve Children(UBound(Children) + 1)
        Children(UBound(Children)) = form
    End Sub
    Public Sub CloseChildren()
        If UBound(Children) > 0 Then
            For i As Integer = 1 To UBound(Children)
                Children(i).Dispose()
            Next
            ReDim Children(0)
        End If
    End Sub
    Private Sub AddNoteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AddNoteToolStripMenuItem.Click
        If Not CheckForAccess(AccessGroup.Modify) Then Exit Sub
        UpdateDev.cmbUpdate_ChangeType.SelectedIndex = GetComboIndexFromShort(ComboType.ChangeType, "NOTE")
        UpdateDev.cmbUpdate_ChangeType.Enabled = False
        UpdateDev.Show()
    End Sub
    Private Sub RefreshCombos()
        FillComboBox(EquipType, cmbEquipType_View_REQ)
        FillComboBox(Locations, cmbLocation_View_REQ)
        FillComboBox(OSType, cmbOSVersion_REQ)
        FillComboBox(StatusType, cmbStatus_REQ)
    End Sub
    Private Sub DeleteDeviceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteDeviceToolStripMenuItem.Click
        If Not CheckForAccess(AccessGroup.Delete) Then Exit Sub
        Dim blah = MsgBox("Are you absolutely sure?  This cannot be undone and will delete all histrical data.", vbYesNo + vbCritical, "WARNING")
        If blah = vbYes Then
            Dim rows As Integer
            rows = DeleteMaster(CurrentDevice.strGUID, Entry_Type.Device)
            If rows > 0 Then
                Dim blah2 = MsgBox("Device deleted successfully.", vbOKOnly + vbInformation, "Device Deleted")
                CurrentDevice = Nothing
                Me.Dispose()
            Else
                Logger("*****DELETION ERROR******: " & CurrentDevice.strGUID)
                Dim blah2 = MsgBox("Failed to delete device succesfully!  Please let Bobby Lovell know about this.", vbOKOnly + vbCritical, "Delete Failed")
                CurrentDevice = Nothing
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
    Private Sub txtCurUser_View_REQ_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCurUser_View_REQ.KeyUp
        StartLiveSearch(sender, LiveBoxType.SelectValue, "dev_cur_user")
    End Sub
    Private Sub dtPurchaseDate_View_REQ_ValueChanged(sender As Object, e As EventArgs) Handles dtPurchaseDate_View_REQ.ValueChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmdCancel_Click(sender As Object, e As EventArgs)
        bolCheckFields = False
        DisableControls()
        ResetBackColors()
        Me.Refresh()
        ViewDevice(CurrentDevice.strGUID)
    End Sub
    Private Sub DeleteEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteEntryToolStripMenuItem.Click
        If Not CheckForAccess(AccessGroup.Modify) Then Exit Sub
        Dim strGUID As String = DataGridHistory.Item(GetColIndex(DataGridHistory, "GUID"), DataGridHistory.CurrentRow.Index).Value
        Dim Info As Device_Info = Get_EntryInfo(strGUID)
        Dim blah = MsgBox("Are you absolutely sure?  This cannot be undone!" & vbCrLf & vbCrLf & "Entry info: " & Info.Historical.dtActionDateTime & " - " & Info.Historical.strChangeType & " - " & strGUID, vbYesNo + vbCritical, "WARNING")
        If blah = vbYes Then
            Dim blah2 = MsgBox(DeleteHistoryEntry(strGUID) & " rows affected.", vbOKOnly + vbInformation, "Deletion Results")
            ViewDevice(CurrentDevice.strGUID)
        Else
            Exit Sub
        End If
    End Sub
    Private Function DeleteHistoryEntry(ByVal strGUID As String) As Integer
        Try
            Dim rows
            Dim strSQLQry As String = "DELETE FROM dev_historical WHERE hist_uid='" & strGUID & "'"
            rows = Return_SQLCommand(strSQLQry).ExecuteNonQuery
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
    Private Sub CheckInMenu_Click(sender As Object, e As EventArgs) Handles CheckInMenu.Click
        Waiting()
        Tracking.SetupTracking()
        Tracking.Show()
        DoneWaiting()
    End Sub
    Private Sub CheckOutMenu_Click(sender As Object, e As EventArgs) Handles CheckOutMenu.Click
        Waiting()
        Tracking.SetupTracking()
        Tracking.Show()
        DoneWaiting()
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
        UpdateDev.cmbUpdate_ChangeType.SelectedIndex = GetComboIndexFromShort(ComboType.ChangeType, "NOTE")
        UpdateDev.cmbUpdate_ChangeType.Enabled = False
        UpdateDev.Show()
    End Sub
    Private Sub ToolStripButton3_Click(sender As Object, e As EventArgs) Handles ToolStripButton3.Click
        If Not CheckForAccess(AccessGroup.Delete) Then Exit Sub
        Dim blah = MsgBox("Are you absolutely sure?  This cannot be undone and will delete all histrical data, tracking and attachments.", vbYesNo + vbCritical, "WARNING")
        If blah = vbYes Then
            If DeleteMaster(CurrentDevice.strGUID, Entry_Type.Device) Then
                Dim blah2 = MsgBox("Device deleted successfully.", vbOKOnly + vbInformation, "Device Deleted")
                CurrentDevice = Nothing
                Me.Dispose()
            Else
                Logger("*****DELETION ERROR******: " & CurrentDevice.strGUID)
                Dim blah2 = MsgBox("Failed to delete device succesfully!  Please let Bobby Lovell know about this.", vbOKOnly + vbCritical, "Delete Failed")
                CurrentDevice = Nothing
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
        Tracking.SetupTracking()
        AddChild(Tracking)
        Tracking.Show()
        DoneWaiting()
    End Sub
    Private Sub CheckOutTool_Click(sender As Object, e As EventArgs) Handles CheckOutTool.Click
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not CheckForAccess(AccessGroup.Tracking) Then Exit Sub
        Waiting()
        Tracking.SetupTracking()
        AddChild(Tracking)
        Tracking.Show()
        DoneWaiting()
    End Sub
    Private Sub AttachmentTool_Click(sender As Object, e As EventArgs) Handles AttachmentTool.Click
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        If Not CheckForAccess(AccessGroup.ViewAttachment) Then Exit Sub
        Attachments.FillDeviceInfo()
        AddChild(Attachments)
        Attachments.ListAttachments(CurrentDevice.strGUID)
        Attachments.Activate()
    End Sub
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
        ViewDevice(CurrentDevice.strGUID)
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
            Dim blah = MsgBox("Some required fields are missing.  Please fill in all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data")
            bolCheckFields = True
            Exit Sub
        End If
        DisableControls()
        UpdateDev.cmbUpdate_ChangeType.SelectedIndex = -1
        UpdateDev.cmbUpdate_ChangeType.Enabled = True
        UpdateDev.txtUpdate_Note.Clear()
        UpdateDev.Show()
    End Sub
    Private Sub cmdCancel_Tool_Click(sender As Object, e As EventArgs) Handles cmdCancel_Tool.Click
        CancelModify()
    End Sub
    Private Sub TabControl1_MouseDown(sender As Object, e As MouseEventArgs) Handles TabControl1.MouseDown
        TrackingGrid.Refresh()
    End Sub
    Private Sub Button1_Click_2(sender As Object, e As EventArgs) Handles cmdMunisInfo.Click
        NewMunisView(CurrentDevice)
    End Sub
    Private Sub PingWorker_DoWork(sender As Object, e As DoWorkEventArgs) Handles PingWorker.DoWork
        Try
            e.Result = My.Computer.Network.Ping("D" & CurrentDevice.strSerial)
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
        CloseChildren()
    End Sub
    Private Sub TrackingGrid_Paint(sender As Object, e As PaintEventArgs) Handles TrackingGrid.Paint
        On Error Resume Next
        TrackingGrid.Columns("Check Type").DefaultCellStyle.Font = New Font(TrackingGrid.Font, FontStyle.Bold)
    End Sub
    Private Sub SetupNetTools()
        grpNetTools.Visible = True
    End Sub
    Private Sub CheckRDP()
        If InStr(CurrentDevice.strOSVersion, "WIN") Then 'CurrentDevice.strEqType = "DESK" Or CurrentDevice.strEqType = "LAPT" Then
            If Not PingWorker.IsBusy Then PingWorker.RunWorkerAsync()
        End If
    End Sub
    Private Sub tmr_RDPRefresher_Tick(sender As Object, e As EventArgs) Handles tmr_RDPRefresher.Tick
        CheckRDP()
    End Sub
    Private Sub cmdSibiLink_Click(sender As Object, e As EventArgs) Handles cmdSibiLink.Click
        If Not CheckForAccess(AccessGroup.Sibi_View) Then Exit Sub
        If CurrentDevice.strSibiLink Is "" Then
            Dim blah = MsgBox("Sibi Link not set.  Set one now?", vbYesNo + vbQuestion, "Sibi Link")
            If blah = vbYes Then
                LinkSibi()
            End If
        Else
            OpenSibiLink(CurrentDevice.strSibiLink)
        End If
    End Sub
    Private Sub LinkSibi()
        Dim f As New frmSibiSelector
        f.ShowDialog(Me)
        If f.DialogResult = DialogResult.OK Then
            Update_SQLValue("devices", "dev_sibi_link", f.SibiUID, "dev_UID", CurrentDevice.strGUID)
            ViewDevice(CurrentDevice.strGUID)
        End If
    End Sub
    Private Sub OpenSibiLink(SibiUID As String)
        Dim sibiForm As New frmManageRequest
        AddChild(sibiForm)
        sibiForm.OpenRequest(SibiUID)
    End Sub
    Private Sub Button1_Click_3(sender As Object, e As EventArgs) Handles cmdSetSibi.Click
        LinkSibi()
    End Sub

    Private Sub cmdBrowseFiles_Click(sender As Object, e As EventArgs) Handles cmdBrowseFiles.Click
        Try
            Process.Start("\\D" & CurrentDevice.strSerial & "\c$")
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub

    Private Sub cmdRDP_Click(sender As Object, e As EventArgs) Handles cmdRDP.Click
        LaunchRDP()
    End Sub
    Private Sub LaunchRDP()
        Dim StartInfo As New ProcessStartInfo
        StartInfo.FileName = "mstsc.exe"
        StartInfo.Arguments = "/v:D" & CurrentDevice.strSerial
        Process.Start(StartInfo)
    End Sub
End Class