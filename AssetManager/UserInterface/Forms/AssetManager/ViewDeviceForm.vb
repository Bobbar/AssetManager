Imports System.ComponentModel
Imports System.Net
Imports System.Net.NetworkInformation
Imports PingVisLib

Public Class ViewDeviceForm

#Region "Fields"

    Public MunisUser As MunisEmployeeStruct = Nothing
    Private CurrentViewDevice As New DeviceStruct
    Private bolCheckFields As Boolean
    Private bolGridFilling As Boolean = False
    Private DataParser As New DBControlParser(Me)
    Private DeviceHostname As String = Nothing
    Private Domain As String = Net.NetworkInformation.IPGlobalProperties.GetIPGlobalProperties.DomainName
    Private intFailedPings As Integer = 0
    Private MyLiveBox As New LiveBox(Me)
    Private MyMunisToolBar As New MunisToolBar(Me)
    Private MyPingVis As PingVis
    Private MyWindowList As New WindowList(Me)
    Private EditMode As Boolean = False

#End Region

#Region "Constructors"

    Sub New(parentForm As ThemedForm, deviceGUID As String)
        InitializeComponent()
        InitDBControls()
        MyLiveBox.AttachToControl(txtCurUser_View_REQ, LiveBoxType.UserSelect, DevicesCols.CurrentUser, DevicesCols.MunisEmpNum)
        MyLiveBox.AttachToControl(txtDescription_View_REQ, LiveBoxType.SelectValue, DevicesCols.Description)
        MyMunisToolBar.InsertMunisDropDown(ToolStrip1, 6)
        Tag = parentForm
        Icon = parentForm.Icon
        GridTheme = parentForm.GridTheme
        FormUID = deviceGUID
        MyWindowList.InsertWindowList(ToolStrip1)
        RefreshCombos()
        grpNetTools.Visible = False
        ExtendedMethods.DoubleBufferedDataGrid(DataGridHistory, True)
        ExtendedMethods.DoubleBufferedDataGrid(TrackingGrid, True)
        LoadDevice(deviceGUID)
    End Sub

#End Region

#Region "Methods"

    Private Function CancelModify() As Boolean
        If EditMode Then
            Dim blah = Message("All changes will be lost.  Are you sure you want to cancel?", vbYesNo + vbQuestion, "Cancel Edit", Me)
            If blah = vbYes Then
                bolCheckFields = False
                fieldErrorIcon.Clear()
                DisableControls()
                ResetBackColors()
                Me.Refresh()
                RefreshDevice()
                Return True
            End If
        End If
        Return False
    End Function

    Public Sub SetAttachCount()
        If Not GlobalSwitches.CachedMode Then
            AttachmentTool.Text = "(" + AssetFunc.GetAttachmentCount(CurrentViewDevice.GUID, New DeviceAttachmentsCols).ToString + ")"
            AttachmentTool.ToolTipText = "Attachments " + AttachmentTool.Text
        End If

    End Sub

    Private Sub SetTracking(bolEnabled As Boolean, bolCheckedOut As Boolean)
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

    Private Sub UpdateDevice(UpdateInfo As DeviceUpdateInfoStruct)
        Dim rows As Integer = 0
        Dim SelectQry As String = "SELECT * FROM " & DevicesCols.TableName & " WHERE " & DevicesCols.DeviceUID & "='" & CurrentViewDevice.GUID & "'"
        Dim InsertQry As String = "SELECT * FROM " & HistoricalDevicesCols.TableName & " LIMIT 0"
        Using trans = DBFunc.GetDatabase.StartTransaction, conn = trans.Connection
            Try
                rows += DBFunc.GetDatabase.UpdateTable(SelectQry, GetUpdateTable(SelectQry), trans)
                rows += DBFunc.GetDatabase.UpdateTable(InsertQry, GetInsertTable(InsertQry, UpdateInfo), trans)

                If rows = 2 Then
                    trans.Commit()
                    LoadDevice(CurrentViewDevice.GUID)
                    Message("Update Added.", vbOKOnly + vbInformation, "Success", Me)
                Else
                    trans.Rollback()
                    LoadDevice(CurrentViewDevice.GUID)
                    Message("Unsuccessful! The number of affected rows was not what was expected.", vbOKOnly + vbExclamation, "Unexpected Result", Me)
                End If

            Catch ex As Exception
                trans.Rollback()
                If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
                    LoadDevice(CurrentViewDevice.GUID)
                End If
            End Try
        End Using
    End Sub

    Public Sub LoadDevice(deviceGUID As String)
        Try
            Waiting()
            bolGridFilling = True
            If LoadHistoryAndFields(deviceGUID) Then
                If CurrentViewDevice.IsTrackable Then LoadTracking(CurrentViewDevice.GUID)
                SetTracking(CurrentViewDevice.IsTrackable, CurrentViewDevice.Tracking.IsCheckedOut)
                Me.Text = Me.Text + FormTitle(CurrentViewDevice)
                DeviceHostname = CurrentViewDevice.HostName & "." & Domain
                CheckRDP()
                tmr_RDPRefresher.Enabled = True
                Me.Show()
                DataGridHistory.ClearSelection()
                bolGridFilling = False
            Else
                Me.Dispose()
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            DoneWaiting()
        End Try
    End Sub
    Private Sub RefreshDevice()
        If EditMode Then
            CancelModify()
        Else
            ADPanel.Visible = False
            grpNetTools.Visible = False
            If MyPingVis IsNot Nothing Then
                MyPingVis.Dispose()
                MyPingVis = Nothing
            End If
            LoadDevice(CurrentViewDevice.GUID)
        End If
    End Sub

    Private Sub LoadTracking(strGUID As String)
        Dim strQry = "Select * FROM " & TrackablesCols.TableName & ", " & DevicesCols.TableName & " WHERE " & TrackablesCols.DeviceUID & " = " & DevicesCols.DeviceUID & " And " & TrackablesCols.DeviceUID & " = '" & strGUID & "' ORDER BY " & TrackablesCols.DateStamp & " DESC"
        Using Results As DataTable = DBFunc.GetDatabase.DataTableFromQueryString(strQry)
            If Results.Rows.Count > 0 Then
                CollectCurrentTracking(Results)
                SendToTrackGrid(TrackingGrid, Results)
                DisableSorting(TrackingGrid)
            Else
                TrackingGrid.DataSource = Nothing
            End If
            FillTrackingBox()
        End Using
    End Sub

    Private Sub AddErrorIcon(ctl As Control)
        If fieldErrorIcon.GetError(ctl) Is String.Empty Then
            fieldErrorIcon.SetIconAlignment(ctl, ErrorIconAlignment.MiddleRight)
            fieldErrorIcon.SetIconPadding(ctl, 4)
            fieldErrorIcon.SetError(ctl, "Required or Invalid Field")
        End If
    End Sub

    Private Sub AssetDisposalFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AssetDisposalForm.Click
        Dim PDFForm As New PdfFormFilling(Me, CurrentViewDevice, PdfFormType.DisposeForm)
    End Sub

    Private Sub ViewAttachments()
        If Not CheckForAccess(AccessGroup.ViewAttachment) Then Exit Sub
        If Not AttachmentsIsOpen(Me) Then
            Dim NewAttachments As New AttachmentsForm(Me, New DeviceAttachmentsCols, CurrentViewDevice)
        End If
    End Sub

    Private Sub AttachmentTool_Click(sender As Object, e As EventArgs) Handles AttachmentTool.Click
        ViewAttachments()
    End Sub

    Private Sub cmdMunisInfo_Click(sender As Object, e As EventArgs) Handles cmdMunisInfo.Click
        Try
            MunisFunc.LoadMunisInfoByDevice(CurrentViewDevice, Me)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Function CheckFields() As Boolean
        Dim bolMissingField As Boolean
        bolMissingField = False
        fieldErrorIcon.Clear()
        For Each c As Control In DataParser.GetDBControls(Me)
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
                    Dim cmb As ComboBox = DirectCast(c, ComboBox)
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
        If Not ValidPhoneNumber(txtPhoneNumber.Text) Then
            bolMissingField = True
            txtPhoneNumber.BackColor = colMissingField
            AddErrorIcon(txtPhoneNumber)
        Else
            txtPhoneNumber.BackColor = Color.Empty
            ClearErrorIcon(txtPhoneNumber)
        End If
        Return Not bolMissingField 'if fields are missing return false to trigger a message if needed
    End Function

    Private Sub StartTrackDeviceForm()
        If Not CheckForAccess(AccessGroup.Tracking) Then Exit Sub
        Waiting()
        Dim NewTracking As New TrackDeviceForm(CurrentViewDevice, Me)
        DoneWaiting()
    End Sub

    Private Sub CheckInTool_Click(sender As Object, e As EventArgs) Handles CheckInTool.Click
        StartTrackDeviceForm()
    End Sub

    Private Sub CheckOutTool_Click(sender As Object, e As EventArgs) Handles CheckOutTool.Click
        StartTrackDeviceForm()
    End Sub

    Private Sub CheckRDP()
        Try
            If CurrentViewDevice.OSVersion.Contains("WIN") Then
                If MyPingVis Is Nothing Then MyPingVis = New PingVis(DirectCast(cmdShowIP, Control), DeviceHostname)
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

    Private Sub ClearErrorIcon(ctl As Control)
        fieldErrorIcon.SetError(ctl, String.Empty)
    End Sub

    Private Sub cmbEquipType_View_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbEquipType_View_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub

    Private Sub cmbEquipType_View_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEquipType_View_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub

    Private Sub cmbLocation_View_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbLocation_View_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub

    Private Sub cmbLocation_View_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLocation_View_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub

    Private Sub cmbOSVersion_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbOSVersion_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub

    Private Sub cmbOSVersion_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbOSVersion_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub

    Private Sub cmbStatus_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbStatus_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub

    Private Sub cmbStatus_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatus_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub

    Private Sub cmdAccept_Tool_Click(sender As Object, e As EventArgs) Handles cmdAccept_Tool.Click
        AcceptChanges()
    End Sub

    Private Sub AcceptChanges()
        Try
            If Not CheckFields() Then
                Message("Some required fields are missing or invalid.  Please check and fill all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data", Me)
                bolCheckFields = True
                Exit Sub
            End If
            Using UpdateDia As New UpdateDev(Me)
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
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Async Sub BrowseFiles()
        Try
            If VerifyAdminCreds() Then
                Dim FullPath As String = "\\" & CurrentViewDevice.HostName & "\c$"
                Await Task.Run(Sub()
                                   Using NetCon As New NetworkConnection(FullPath, AdminCreds), p As Process = New Process
                                       p.StartInfo.UseShellExecute = False
                                       p.StartInfo.RedirectStandardOutput = True
                                       p.StartInfo.RedirectStandardError = True
                                       p.StartInfo.FileName = "explorer.exe"
                                       p.StartInfo.Arguments = FullPath
                                       p.Start()
                                       p.WaitForExit()
                                   End Using
                               End Sub)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub cmdBrowseFiles_Click(sender As Object, e As EventArgs) Handles cmdBrowseFiles.Click
        BrowseFiles()
    End Sub

    Private Sub cmdCancel_Tool_Click(sender As Object, e As EventArgs) Handles cmdCancel_Tool.Click
        CancelModify()
    End Sub

    Private Sub cmdGKUpdate_Click(sender As Object, e As EventArgs) Handles cmdGKUpdate.Click
        If VerifyAdminCreds() Then
            GKUpdaterForm.AddUpdate(CurrentViewDevice)
            If Not GKUpdaterForm.Visible Then GKUpdaterForm.Show()
        End If
    End Sub

    Private Sub cmdMunisSearch_Click(sender As Object, e As EventArgs) Handles cmdMunisSearch.Click
        Using NewMunisSearch As New MunisUserForm(Me)
            MunisUser = NewMunisSearch.EmployeeInfo
            If MunisUser.Name <> "" Then
                txtCurUser_View_REQ.Text = MunisUser.Name
                txtCurUser_View_REQ.ReadOnly = True
            End If
        End Using
    End Sub

    Private Sub cmdRDP_Click(sender As Object, e As EventArgs) Handles cmdRDP.Click
        LaunchRDP()
    End Sub

    Private Async Sub cmdRestart_Click(sender As Object, e As EventArgs) Handles cmdRestart.Click
        Dim blah = Message("Click 'Yes' to reboot this device.", vbYesNo + vbQuestion, "Are you sure?")
        If blah = vbYes Then
            Dim IP As String = MyPingVis.CurrentResult.Address.ToString
            Dim DeviceName As String = CurrentViewDevice.HostName
            Dim RestartOutput = Await SendRestart(IP, DeviceName)
            If RestartOutput = "" Then
                Message("Success", vbOKOnly + vbInformation, "Restart Device", Me)
            Else
                Message("Failed" & vbCrLf & vbCrLf & "Output: " & RestartOutput, vbOKOnly + vbInformation, "Restart Device", Me)
            End If
        End If
    End Sub

    Private Sub cmdSetSibi_Click(sender As Object, e As EventArgs)
        LinkSibi()
    End Sub

    Private Sub cmdShowIP_Click(sender As Object, e As EventArgs) Handles cmdShowIP.Click
        If Not IsNothing(cmdShowIP.Tag) Then
            Dim blah = Message(cmdShowIP.Tag.ToString & vbCrLf & vbCrLf & "Press 'Yes' to copy to clipboard.", vbInformation + vbYesNo, "IP Address", Me)
            If blah = vbYes Then
                Clipboard.SetText(cmdShowIP.Tag.ToString)
            End If
        End If
    End Sub

    Private Sub cmdSibiLink_Click(sender As Object, e As EventArgs) Handles cmdSibiLink.Click
        OpenSibiLink(CurrentViewDevice)
    End Sub

    Private Sub CollectCurrentTracking(Results As DataTable)
        With CurrentViewDevice
            DateTime.TryParse(NoNull(Results.Rows(0).Item(TrackablesCols.CheckoutTime)), .Tracking.CheckoutTime)
            DateTime.TryParse(NoNull(Results.Rows(0).Item(TrackablesCols.CheckinTime)), .Tracking.CheckinTime)
            DateTime.TryParse(NoNull(Results.Rows(0).Item(TrackablesCols.DueBackDate)), .Tracking.DueBackTime)
            .Tracking.UseLocation = NoNull(Results.Rows(0).Item(TrackablesCols.UseLocation))
            .Tracking.CheckoutUser = NoNull(Results.Rows(0).Item(TrackablesCols.CheckoutUser))
            .Tracking.CheckinUser = NoNull(Results.Rows(0).Item(TrackablesCols.CheckinUser))
            .Tracking.UseReason = NoNull(Results.Rows(0).Item(TrackablesCols.Notes))
        End With
    End Sub

    Private Function ConcurrencyCheck() As Boolean
        Dim InDBCheckSum As String = AssetFunc.GetSqlValue(DevicesCols.TableName, DevicesCols.DeviceUID, CurrentViewDevice.GUID, DevicesCols.Checksum)
        If InDBCheckSum = CurrentViewDevice.Checksum Then
            Return True
        Else
            Message("This record appears to have been modified by someone else since the start of this modification.", vbOKOnly + vbExclamation, "Concurrency Error", Me)
            Return False
        End If
    End Function

    Private Sub DataGridHistory_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridHistory.CellDoubleClick
        Dim EntryUID As String = DataGridHistory.Item(GetColIndex(DataGridHistory, "GUID"), DataGridHistory.CurrentRow.Index).Value.ToString
        If Not FormIsOpenByUID(GetType(ViewHistoryForm), EntryUID) Then
            NewEntryView(EntryUID)
        End If
    End Sub

    Private Sub DataGridHistory_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridHistory.CellEnter
        If Not bolGridFilling Then
            HighlightRow(DataGridHistory, GridTheme, e.RowIndex)
        End If
    End Sub

    Private Sub DataGridHistory_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridHistory.CellLeave
        LeaveRow(DataGridHistory, GridTheme, e.RowIndex)
    End Sub

    Private Sub DataGridHistory_CellMouseDown(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridHistory.CellMouseDown
        If e.Button = MouseButtons.Right And e.ColumnIndex > -1 And e.RowIndex > -1 Then
            DataGridHistory.CurrentCell = DataGridHistory(e.ColumnIndex, e.RowIndex)
        End If
    End Sub

    Private Sub DeleteDevice()
        If Not CheckForAccess(AccessGroup.DeleteDevice) Then Exit Sub
        Dim blah = Message("Are you absolutely sure?  This cannot be undone and will delete all historical data, tracking and attachments.", vbYesNo + vbExclamation, "WARNING", Me)
        If blah = vbYes Then
            If AssetFunc.DeleteFtpAndSql(CurrentViewDevice.GUID, EntryType.Device) Then
                Message("Device deleted successfully.", vbOKOnly + vbInformation, "Device Deleted", Me)
                CurrentViewDevice = Nothing
                MainForm.RefreshCurrent()
            Else
                Logger("*****DELETION ERROR******: " & CurrentViewDevice.GUID)
                Message("Failed to delete device succesfully!  Please let Bobby Lovell know about this.", vbOKOnly + vbCritical, "Delete Failed", Me)
                CurrentViewDevice = Nothing
            End If
            Me.Dispose()
        Else
            Exit Sub
        End If
    End Sub

    Private Sub DeleteSelectedHistoricalEntry()
        If Not CheckForAccess(AccessGroup.ModifyDevice) Then Exit Sub
        Dim strGUID As String = DataGridHistory.Item(GetColIndex(DataGridHistory, "GUID"), DataGridHistory.CurrentRow.Index).Value.ToString
        Dim Info As DeviceStruct = AssetFunc.GetHistoricalEntryInfo(strGUID)
        Dim blah = Message("Are you absolutely sure?  This cannot be undone!" & vbCrLf & vbCrLf & "Entry info: " & Info.Historical.ActionDateTime & " - " & Info.Historical.ChangeType & " - " & strGUID, vbYesNo + vbExclamation, "WARNING", Me)
        If blah = vbYes Then
            Message(DeleteHistoryEntry(strGUID) & " rows affected.", vbOKOnly + vbInformation, "Deletion Results", Me)
            LoadDevice(CurrentViewDevice.GUID)
        Else
            Exit Sub
        End If
    End Sub

    Private Sub DeleteEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteEntryToolStripMenuItem.Click
        DeleteSelectedHistoricalEntry()
    End Sub

    Private Function DeleteHistoryEntry(ByVal strGUID As String) As Integer
        Try
            Dim rows As Integer
            Dim DeleteEntryQuery As String = "DELETE FROM " & HistoricalDevicesCols.TableName & " WHERE " & HistoricalDevicesCols.HistoryEntryUID & "='" & strGUID & "'"
            rows = DBFunc.GetDatabase.ExecuteQuery(DeleteEntryQuery)
            Return rows
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return 0
        End Try
    End Function
    Private Sub DisableControlsRecursive(control As Control)
        For Each c As Control In control.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    Dim txt As TextBox = DirectCast(c, TextBox)
                    txt.ReadOnly = True
                Case TypeOf c Is MaskedTextBox
                    Dim txt As MaskedTextBox = DirectCast(c, MaskedTextBox)
                    txt.ReadOnly = True
                Case TypeOf c Is ComboBox
                    Dim cmb As ComboBox = DirectCast(c, ComboBox)
                    cmb.Enabled = False
                Case TypeOf c Is DateTimePicker
                    Dim dtp As DateTimePicker = DirectCast(c, DateTimePicker)
                    dtp.Enabled = False
                Case TypeOf c Is CheckBox
                    c.Enabled = False
                Case TypeOf c Is Label
                    'do nut-zing
            End Select
            If c.HasChildren Then
                DisableControlsRecursive(c)
            End If
        Next
    End Sub

    Private Sub DisableControls()
        EditMode = False
        DisableControlsRecursive(Me)
        pnlOtherFunctions.Visible = True
        cmdMunisSearch.Visible = False
        Me.Text = "View"
        tsSaveModify.Visible = False
        tsTracking.Visible = False
    End Sub

    Private Sub DisableSorting(Grid As DataGridView)
        Dim c As DataGridViewColumn
        For Each c In Grid.Columns
            c.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub DoneWaiting()
        SetWaitCursor(False, Me)
    End Sub

    Private Sub dtPurchaseDate_View_REQ_ValueChanged(sender As Object, e As EventArgs) Handles dtPurchaseDate_View_REQ.ValueChanged
        If bolCheckFields Then CheckFields()
    End Sub

    Private Sub EnableControlsRecursive(control As Control)
        For Each c As Control In control.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    Dim txt As TextBox = DirectCast(c, TextBox)
                    txt.ReadOnly = False
                Case TypeOf c Is MaskedTextBox
                    Dim txt As MaskedTextBox = DirectCast(c, MaskedTextBox)
                    txt.ReadOnly = False
                Case TypeOf c Is ComboBox
                    Dim cmb As ComboBox = DirectCast(c, ComboBox)
                    cmb.Enabled = True
                Case TypeOf c Is DateTimePicker
                    Dim dtp As DateTimePicker = DirectCast(c, DateTimePicker)
                    dtp.Enabled = True
                Case TypeOf c Is CheckBox
                    c.Enabled = True
                Case TypeOf c Is Label
                    'do nut-zing
            End Select
            If c.HasChildren Then
                EnableControlsRecursive(c)
            End If
        Next

    End Sub

    Private Sub EnableControls()
        EditMode = True
        EnableControlsRecursive(Me)
        ADPanel.Visible = False
        pnlOtherFunctions.Visible = False
        cmdMunisSearch.Visible = True
        Me.Text = "View" & FormTitle(CurrentViewDevice) & "  *MODIFYING**"
        tsSaveModify.Visible = True
    End Sub

    Private Sub FillTrackingBox()
        If CBool(CurrentViewDevice.Tracking.IsCheckedOut) Then
            txtCheckOut.BackColor = colCheckOut
            txtCheckLocation.Text = CurrentViewDevice.Tracking.UseLocation
            lblCheckTime.Text = "CheckOut Time:"
            txtCheckTime.Text = CurrentViewDevice.Tracking.CheckoutTime.ToString
            lblCheckUser.Text = "CheckOut User:"
            txtCheckUser.Text = CurrentViewDevice.Tracking.CheckoutUser
            lblDueBack.Visible = True
            txtDueBack.Visible = True
            txtDueBack.Text = CurrentViewDevice.Tracking.DueBackTime.ToString
        Else
            txtCheckOut.BackColor = colCheckIn
            txtCheckLocation.Text = GetHumanValue(DeviceIndex.Locations, CurrentViewDevice.Location)
            lblCheckTime.Text = "CheckIn Time:"
            txtCheckTime.Text = CurrentViewDevice.Tracking.CheckinTime.ToString
            lblCheckUser.Text = "CheckIn User:"
            txtCheckUser.Text = CurrentViewDevice.Tracking.CheckinUser
            lblDueBack.Visible = False
            txtDueBack.Visible = False
        End If
        txtCheckOut.Text = IIf(CurrentViewDevice.Tracking.IsCheckedOut, "Checked Out", "Checked In").ToString
    End Sub

    Private Function FormTitle(Device As DeviceStruct) As String
        Return " - " + Device.CurrentUser + " - " + Device.AssetTag + " - " + Device.Description
    End Function

    Private Function GetInsertTable(selectQuery As String, UpdateInfo As DeviceUpdateInfoStruct) As DataTable
        Dim tmpTable = DataParser.ReturnInsertTable(selectQuery)
        Dim DBRow = tmpTable.Rows(0)
        'Add Add'l info
        DBRow(HistoricalDevicesCols.ChangeType) = UpdateInfo.ChangeType
        DBRow(HistoricalDevicesCols.Notes) = UpdateInfo.Note
        DBRow(HistoricalDevicesCols.ActionUser) = strLocalUser
        DBRow(HistoricalDevicesCols.DeviceUID) = CurrentViewDevice.GUID
        Return tmpTable
    End Function

    Private Function GetUpdateTable(selectQuery As String) As DataTable
        Dim tmpTable = DataParser.ReturnUpdateTable(selectQuery)
        Dim DBRow = tmpTable.Rows(0)
        'Add Add'l info
        If MunisUser.Number IsNot Nothing Then
            DBRow(DevicesCols.CurrentUser) = MunisUser.Name
            DBRow(DevicesCols.MunisEmpNum) = MunisUser.Number
        Else
            If CurrentViewDevice.CurrentUser <> Trim(txtCurUser_View_REQ.Text) Then
                DBRow(DevicesCols.CurrentUser) = Trim(txtCurUser_View_REQ.Text)
                DBRow(DevicesCols.MunisEmpNum) = DBNull.Value
            Else
                DBRow(DevicesCols.CurrentUser) = CurrentViewDevice.CurrentUser
                DBRow(DevicesCols.MunisEmpNum) = CurrentViewDevice.CurrentUserEmpNum
            End If
        End If
        DBRow(DevicesCols.SibiLinkUID) = CleanDBValue(CurrentViewDevice.SibiLink)
        DBRow(DevicesCols.LastModUser) = strLocalUser
        DBRow(DevicesCols.LastModDate) = Now
        DBRow(DevicesCols.Checksum) = GetHashOfTable(tmpTable)
        MunisUser = Nothing
        Return tmpTable
    End Function

    Private Sub InitDBControls()
        'Required Fields
        txtAssetTag_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.AssetTag, True)
        txtSerial_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.Serial, True)
        txtCurUser_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.CurrentUser, True)
        txtDescription_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.Description, True)
        dtPurchaseDate_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.PurchaseDate, True)
        cmbEquipType_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.EQType, DeviceIndex.EquipType, True)
        cmbLocation_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.Location, DeviceIndex.Locations, True)
        cmbOSVersion_REQ.Tag = New DBControlInfo(DevicesBaseCols.OSVersion, DeviceIndex.OSType, True)
        cmbStatus_REQ.Tag = New DBControlInfo(DevicesBaseCols.Status, DeviceIndex.StatusType, True)

        'Non-required and Misc Fields
        txtPONumber.Tag = New DBControlInfo(DevicesBaseCols.PO, False)
        txtReplacementYear_View.Tag = New DBControlInfo(DevicesBaseCols.ReplacementYear, False)
        txtPhoneNumber.Tag = New DBControlInfo(DevicesBaseCols.PhoneNumber, False)
        lblGUID.Tag = New DBControlInfo(DevicesBaseCols.DeviceUID, False)
        chkTrackable.Tag = New DBControlInfo(DevicesBaseCols.Trackable, False)
        txtHostname.Tag = New DBControlInfo(DevicesBaseCols.HostName, False)
        iCloudTextBox.Tag = New DBControlInfo(DevicesBaseCols.iCloudAccount, False)
    End Sub

    Private Sub LaunchRDP()
        Dim StartInfo As New ProcessStartInfo
        StartInfo.FileName = "mstsc.exe"
        StartInfo.Arguments = "/v:" & CurrentViewDevice.HostName
        Process.Start(StartInfo)
    End Sub

    Private Sub lblGUID_Click(sender As Object, e As EventArgs) Handles lblGUID.Click
        Clipboard.SetText(lblGUID.Text)
        Message("GUID Copied to clipboard.", vbInformation + vbOKOnly,, Me)
    End Sub

    Private Sub LinkSibi()
        Using f As New SibiSelectorForm(Me)
            If f.DialogResult = DialogResult.OK Then
                CurrentViewDevice.SibiLink = f.SibiUID
                Message("Sibi Link Set.", vbOKOnly + vbInformation, "Success", Me)
            End If
        End Using
    End Sub

    Private Sub ModifyDevice()
        If Not CheckForAccess(AccessGroup.ModifyDevice) Then Exit Sub
        EnableControls()
    End Sub

    Private Sub NewEntryView(entryGUID As String)
        Waiting()
        Dim NewEntry As New ViewHistoryForm(Me, entryGUID, CurrentViewDevice.GUID)
        DoneWaiting()
    End Sub

    Private Sub NewTrackingView(GUID As String)
        Waiting()
        Dim NewTracking As New ViewTrackingForm(Me, GUID, CurrentViewDevice)
        DoneWaiting()
    End Sub

    Private Sub OpenSibiLink(LinkDevice As DeviceStruct)
        Try
            If Not CheckForAccess(AccessGroup.ViewSibi) Then Exit Sub
            Dim SibiUID As String
            If LinkDevice.SibiLink = "" Then
                If LinkDevice.PO = "" Then
                    Message("A valid PO Number or Sibi Link is required.", vbOKOnly + vbInformation, "Missing Info", Me)
                    Exit Sub
                Else
                    SibiUID = AssetFunc.GetSqlValue(SibiRequestCols.TableName, SibiRequestCols.PO, LinkDevice.PO, SibiRequestCols.UID)
                End If
            Else
                SibiUID = LinkDevice.SibiLink
            End If
            If SibiUID = "" Then
                Message("No Sibi request found with matching PO number.", vbOKOnly + vbInformation, "Not Found", Me)
            Else
                If Not FormIsOpenByUID(GetType(SibiManageRequestForm), SibiUID) Then
                    Dim NewRequest As New SibiManageRequestForm(Me, SibiUID)
                End If
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub RefreshCombos()
        FillComboBox(DeviceIndex.EquipType, cmbEquipType_View_REQ)
        FillComboBox(DeviceIndex.Locations, cmbLocation_View_REQ)
        FillComboBox(DeviceIndex.OSType, cmbOSVersion_REQ)
        FillComboBox(DeviceIndex.StatusType, cmbStatus_REQ)
    End Sub

    Private Sub ResetBackColors()
        For Each c As Control In DataParser.GetDBControls(Me)
            Select Case True
                Case TypeOf c Is TextBox
                    c.BackColor = Color.Empty
                Case TypeOf c Is ComboBox
                    c.BackColor = Color.Empty
            End Select
        Next
    End Sub

    Private Async Function SendRestart(IP As String, DeviceName As String) As Task(Of String)
        Dim OrigButtonImage = cmdRestart.Image
        Try
            If VerifyAdminCreds() Then
                cmdRestart.Image = My.Resources.LoadingAni
                Dim FullPath As String = "\\" & IP
                Dim output As String = Await Task.Run(Function()
                                                          Using NetCon As New NetworkConnection(FullPath, AdminCreds), p As Process = New Process
                                                              p.StartInfo.UseShellExecute = False
                                                              p.StartInfo.RedirectStandardOutput = True
                                                              p.StartInfo.RedirectStandardError = True
                                                              p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden
                                                              p.StartInfo.FileName = "shutdown.exe"
                                                              p.StartInfo.Arguments = "/m " & FullPath & " /f /r /t 0"
                                                              p.Start()
                                                              output = p.StandardError.ReadToEnd
                                                              p.WaitForExit()
                                                              output = Trim(output)
                                                              Return output
                                                          End Using
                                                      End Function)
                Return output
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            cmdRestart.Image = OrigButtonImage
        End Try
        Return Nothing
    End Function

    Private Sub SendToHistGrid(Grid As DataGridView, tblResults As DataTable)
        Try
            Using table As New DataTable
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
                        table.Rows.Add(NoNull(r.Item(HistoricalDevicesCols.ActionDateTime)),
                           GetHumanValue(DeviceIndex.ChangeType, NoNull(r.Item(HistoricalDevicesCols.ChangeType))),
                           NoNull(r.Item(HistoricalDevicesCols.ActionUser)),
                           NotePreview(NoNull(r.Item(HistoricalDevicesCols.Notes)), 25),
                           NoNull(r.Item(HistoricalDevicesCols.CurrentUser)),
                           NoNull(r.Item(HistoricalDevicesCols.AssetTag)),
                           NoNull(r.Item(HistoricalDevicesCols.Serial)),
                           NoNull(r.Item(HistoricalDevicesCols.Description)),
                           GetHumanValue(DeviceIndex.Locations, NoNull(r.Item(HistoricalDevicesCols.Location))),
                           NoNull(r.Item(HistoricalDevicesCols.PurchaseDate)),
                           NoNull(r.Item(HistoricalDevicesCols.HistoryEntryUID)))
                    Next
                    Grid.DataSource = table
                Else
                    Grid.DataSource = Nothing
                End If
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub SendToTrackGrid(Grid As DataGridView, tblResults As DataTable)
        Try
            Using table As New DataTable
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
                        table.Rows.Add(NoNull(r.Item(TrackablesCols.DateStamp)),
                           NoNull(r.Item(TrackablesCols.CheckType)),
                           NoNull(r.Item(TrackablesCols.CheckoutUser)),
                           NoNull(r.Item(TrackablesCols.CheckinUser)),
                           NoNull(r.Item(TrackablesCols.CheckoutTime)),
                           NoNull(r.Item(TrackablesCols.CheckinTime)),
                           NoNull(r.Item(TrackablesCols.DueBackDate)),
                           NoNull(r.Item(TrackablesCols.UseLocation)),
                           NoNull(r.Item(TrackablesCols.UID)))
                    Next
                    Grid.DataSource = table
                Else
                    Grid.DataSource = Nothing
                End If
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub SetMunisEmpStatus()
        ToolTip1.SetToolTip(txtCurUser_View_REQ, "")
        If CurrentViewDevice.CurrentUserEmpNum <> "" Then
            txtCurUser_View_REQ.BackColor = colEditColor
            ToolTip1.SetToolTip(txtCurUser_View_REQ, "Munis Linked Employee")
        End If
    End Sub

    Private Sub SetupNetTools(PingResults As PingVis.PingInfo)
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

    Private Sub TabControl1_MouseDown(sender As Object, e As MouseEventArgs) Handles TabControl1.MouseDown
        TrackingGrid.Refresh()
    End Sub

    Private Sub tmr_RDPRefresher_Tick(sender As Object, e As EventArgs) Handles tmr_RDPRefresher.Tick
        CheckRDP()
    End Sub

    Private Sub TrackingGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles TrackingGrid.CellDoubleClick
        Dim EntryUID = TrackingGrid.Item(GetColIndex(TrackingGrid, "GUID"), TrackingGrid.CurrentRow.Index).Value.ToString
        If Not FormIsOpenByUID(GetType(ViewTrackingForm), EntryUID) Then
            NewTrackingView(EntryUID)
        End If
    End Sub

    Private Sub TrackingGrid_Paint(sender As Object, e As PaintEventArgs) Handles TrackingGrid.Paint
        Try
            TrackingGrid.Columns("Check Type").DefaultCellStyle.Font = New Font(TrackingGrid.Font, FontStyle.Bold)
        Catch
        End Try
    End Sub

    Private Sub TrackingGrid_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles TrackingGrid.RowPrePaint
        Dim c1 As Color = ColorTranslator.FromHtml("#8BCEE8") 'highlight color
        TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.ForeColor = Color.Black
        TrackingGrid.Rows(e.RowIndex).Cells(GetColIndex(TrackingGrid, "Check Type")).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        If TrackingGrid.Rows(e.RowIndex).Cells(GetColIndex(TrackingGrid, "Check Type")).Value.ToString = CheckType.Checkin Then
            TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.BackColor = colCheckIn
            Dim c2 As Color = Color.FromArgb(colCheckIn.R, colCheckIn.G, colCheckIn.B)
            Dim BlendColor As Color
            BlendColor = Color.FromArgb(CInt((CInt(c1.A) + CInt(c2.A)) / 2),
                                                CInt((CInt(c1.R) + CInt(c2.R)) / 2),
                                                CInt((CInt(c1.G) + CInt(c2.G)) / 2),
                                                CInt((CInt(c1.B) + CInt(c2.B)) / 2))
            TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.SelectionBackColor = BlendColor
        ElseIf TrackingGrid.Rows(e.RowIndex).Cells(GetColIndex(TrackingGrid, "Check Type")).Value.ToString = CheckType.Checkout Then
            TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.BackColor = colCheckOut
            Dim c2 As Color = Color.FromArgb(colCheckOut.R, colCheckOut.G, colCheckOut.B)
            Dim BlendColor As Color
            BlendColor = Color.FromArgb(CInt((CInt(c1.A) + CInt(c2.A)) / 2),
                                                CInt((CInt(c1.R) + CInt(c2.R)) / 2),
                                                CInt((CInt(c1.G) + CInt(c2.G)) / 2),
                                                CInt((CInt(c1.B) + CInt(c2.B)) / 2))
            TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.SelectionBackColor = BlendColor
        End If
    End Sub

    Private Sub tsbDeleteDevice_Click(sender As Object, e As EventArgs) Handles tsbDeleteDevice.Click
        DeleteDevice()
    End Sub

    Private Sub tsbModify_Click(sender As Object, e As EventArgs) Handles tsbModify.Click
        ModifyDevice()
    End Sub

    Private Sub tsbNewNote_Click(sender As Object, e As EventArgs) Handles tsbNewNote.Click
        AddNewNote()
    End Sub

    Private Sub AddNewNote()
        Try
            If Not CheckForAccess(AccessGroup.ModifyDevice) Then Exit Sub
            Using UpdateDia As New UpdateDev(Me, True)
                If UpdateDia.DialogResult = DialogResult.OK Then
                    If Not ConcurrencyCheck() Then
                        CancelModify()
                        Exit Sub
                    End If
                    UpdateDevice(UpdateDia.UpdateInfo)
                Else
                    CancelModify()
                End If
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub tsmAssetInputForm_Click(sender As Object, e As EventArgs) Handles tsmAssetInputForm.Click
        If CurrentViewDevice.PO <> "" Then
            Dim PDFForm As New PdfFormFilling(Me, CurrentViewDevice, PdfFormType.InputForm)
        Else
            Message("Please add a valid PO number to this device.", vbOKOnly + vbExclamation, "Missing Info", Me)
        End If
    End Sub

    Private Sub tsmAssetTransferForm_Click(sender As Object, e As EventArgs) Handles tsmAssetTransferForm.Click
        Dim PDFForm As New PdfFormFilling(Me, CurrentViewDevice, PdfFormType.TransferForm)
    End Sub

    Private Sub txtAssetTag_View_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag_View_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub

    Private Sub txtCurUser_View_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtCurUser_View_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub

    Private Sub txtDescription_View_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtDescription_View_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub

    Private Sub txtPhoneNumber_Leave(sender As Object, e As EventArgs) Handles txtPhoneNumber.Leave
        If Trim(txtPhoneNumber.Text) <> "" And Not ValidPhoneNumber(txtPhoneNumber.Text) Then
            Message("Invalid phone number.", vbOKOnly + vbExclamation, "Error", Me)
            txtPhoneNumber.Focus()
        End If
    End Sub

    Private Sub txtPhoneNumber_TextChanged(sender As Object, e As EventArgs) Handles txtPhoneNumber.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub

    Private Sub txtSerial_View_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtSerial_View_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub

    Private Sub View_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Dim OKToClose As Boolean = True
        Dim ActiveTransfers As Boolean = CheckForActiveTransfers(Me)
        If ActiveTransfers Then OKToClose = False
        If EditMode AndAlso Not CancelModify() Then OKToClose = False
        e.Cancel = Not OKToClose
    End Sub

    Private Sub View_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        MyWindowList.Dispose()
        MyLiveBox.Dispose()
        MyMunisToolBar.Dispose()
        CloseChildren(Me)
        If MyPingVis IsNot Nothing Then MyPingVis.Dispose()
    End Sub

    Private Sub View_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        If Me.WindowState = FormWindowState.Minimized Then
            MinimizeChildren(Me)
        End If
    End Sub

    Private Function LoadHistoryAndFields(ByVal DeviceUID As String) As Boolean
        Try
            Using DeviceResults = DBFunc.GetDatabase.DataTableFromQueryString("Select * FROM " & DevicesCols.TableName & " WHERE " & DevicesCols.DeviceUID & " = '" & DeviceUID & "'"),
                    HistoricalResults = DBFunc.GetDatabase.DataTableFromQueryString("Select * FROM " & HistoricalDevicesCols.TableName & " WHERE " & HistoricalDevicesCols.DeviceUID & " = '" & DeviceUID & "' ORDER BY " & HistoricalDevicesCols.ActionDateTime & " DESC")
                If DeviceResults.Rows.Count < 1 Then
                    CloseChildren(Me)
                    CurrentViewDevice = Nothing
                    Message("That device was not found!  It may have been deleted.  Re-execute your search.", vbOKOnly + vbExclamation, "Not Found", Me)
                    Return False
                End If
                CurrentViewDevice = AssetFunc.CollectDeviceInfo(DeviceResults)
                DataParser.FillDBFields(DeviceResults)
                SetMunisEmpStatus()
                SendToHistGrid(DataGridHistory, HistoricalResults)
                DisableControls()
                SetAttachCount()
                SetADInfo()
                Return True
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

    Private Sub Waiting()
        SetWaitCursor(True, Me)
    End Sub

    Private Async Sub SetADInfo()
        Try
            If CurrentViewDevice.HostName <> "" Then
                Dim ADWrap As New ActiveDirectoryWrapper(CurrentViewDevice.HostName)
                If Await ADWrap.LoadResultsAsync Then
                    ADOUTextBox.Text = ADWrap.GetDeviceOU()
                    ADOSTextBox.Text = ADWrap.GetAttributeValue("operatingsystem")
                    ADOSVerTextBox.Text = ADWrap.GetAttributeValue("operatingsystemversion")
                    ADLastLoginTextBox.Text = ADWrap.GetAttributeValue("lastlogon")
                    ADCreatedTextBox.Text = ADWrap.GetAttributeValue("whencreated")
                    ADPanel.Visible = True
                Else
                    ADPanel.Visible = False
                End If
            Else
                ADPanel.Visible = False
            End If
        Catch
            ADPanel.Visible = False
        End Try
    End Sub

    Private Sub RefreshToolStripButton_Click(sender As Object, e As EventArgs) Handles RefreshToolStripButton.Click
        RefreshDevice()
    End Sub
#End Region

End Class