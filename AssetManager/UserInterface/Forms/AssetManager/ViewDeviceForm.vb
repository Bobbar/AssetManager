Imports System.ComponentModel
Imports System.Net
Imports System.Net.NetworkInformation
Imports PingVisLib

Public Class ViewDeviceForm

#Region "Fields"

    Public MunisUser As MunisEmployeeStruct = Nothing
    Private bolCheckFields As Boolean
    Private bolGridFilling As Boolean = False
    Private CurrentHash As String
    Private CurrentViewDevice As New DeviceObject
    Private DataParser As New DBControlParser(Me)
    Private DeviceHostname As String = Nothing
    Private EditMode As Boolean = False
    Private intFailedPings As Integer = 0
    Private MyLiveBox As New LiveBox(Me)
    Private MyMunisToolBar As New MunisToolBar(Me)
    Private MyPingVis As PingVis
    Private MyWindowList As New WindowList(Me)
    Private StatusSlider As SliderLabel

#End Region

#Region "Delegates"

    Delegate Sub StatusVoidDelegate(text As String)

#End Region

#Region "Constructors"

    Sub New(parentForm As ExtendedForm, deviceGUID As String)
        Me.ParentForm = parentForm
        FormUID = deviceGUID
        InitializeComponent()

        StatusSlider = New SliderLabel
        StatusStrip1.Items.Add(StatusSlider.ToToolStripControl(StatusStrip1))

        MyMunisToolBar.InsertMunisDropDown(ToolStrip1, 6)
        ImageCaching.CacheControlImages(Me)
        MyWindowList.InsertWindowList(ToolStrip1)
        InitDBControls()
        MyLiveBox.AttachToControl(txtCurUser_View_REQ, DevicesCols.CurrentUser, LiveBoxType.UserSelect, DevicesCols.MunisEmpNum)
        MyLiveBox.AttachToControl(txtDescription_View_REQ, DevicesCols.Description, LiveBoxType.SelectValue)
        RefreshCombos()
        RemoteToolsBox.Visible = False
        ExtendedMethods.DoubleBufferedDataGrid(DataGridHistory, True)
        ExtendedMethods.DoubleBufferedDataGrid(TrackingGrid, True)
        LoadDevice(deviceGUID)
    End Sub

#End Region

#Region "Methods"

    Public Sub LoadDevice(deviceGUID As String)
        Try
            Waiting()
            bolGridFilling = True
            If LoadHistoryAndFields(deviceGUID) Then
                If CurrentViewDevice.IsTrackable Then LoadTracking(CurrentViewDevice.GUID)
                SetTracking(CurrentViewDevice.IsTrackable, CurrentViewDevice.Tracking.IsCheckedOut)
                Me.Text = Me.Text + FormTitle(CurrentViewDevice)
                DeviceHostname = CurrentViewDevice.HostName & "." & NetworkInfo.CurrentDomain
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

    Public Overrides Function OKToClose() As Boolean
        Dim CanClose As Boolean = True
        If Not OKToCloseChildren(Me) Then CanClose = False
        If EditMode AndAlso Not CancelModify() Then CanClose = False
        Return CanClose
    End Function

    Public Overrides Sub RefreshData()
        If EditMode Then
            CancelModify()
        Else
            ADPanel.Visible = False
            RemoteToolsBox.Visible = False
            If MyPingVis IsNot Nothing Then
                MyPingVis.Dispose()
                MyPingVis = Nothing
            End If
            LoadDevice(CurrentViewDevice.GUID)
        End If
    End Sub

    Public Sub SetAttachCount()
        If Not GlobalSwitches.CachedMode Then
            AttachmentTool.Text = "(" + AssetFunc.GetAttachmentCount(CurrentViewDevice.GUID, New DeviceAttachmentsCols).ToString + ")"
            AttachmentTool.ToolTipText = "Attachments " + AttachmentTool.Text
        End If

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

    Private Sub AddErrorIcon(ctl As Control)
        If fieldErrorIcon.GetError(ctl) Is String.Empty Then
            fieldErrorIcon.SetIconAlignment(ctl, ErrorIconAlignment.MiddleRight)
            fieldErrorIcon.SetIconPadding(ctl, 4)
            fieldErrorIcon.SetError(ctl, "Required or Invalid Field")
        End If
    End Sub

    Private Sub AddNewNote()
        Try
            If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifyDevice) Then Exit Sub
            Using UpdateDia As New UpdateDev(Me, True)
                If UpdateDia.DialogResult = DialogResult.OK Then
                    If Not ConcurrencyCheck() Then
                        RefreshData()
                    Else
                        UpdateDevice(UpdateDia.UpdateInfo)
                    End If
                Else
                    RefreshData()
                End If
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Async Sub BrowseFiles()
        Try
            If SecurityTools.VerifyAdminCreds() Then
                Dim FullPath As String = "\\" & CurrentViewDevice.HostName & "\c$"
                Await Task.Run(Sub()
                                   Using NetCon As New NetworkConnection(FullPath, SecurityTools.AdminCreds), p As Process = New Process
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

    Private Function CancelModify() As Boolean
        If EditMode Then
            Me.WindowState = FormWindowState.Normal
            Me.Activate()
            Dim blah = Message("All changes will be lost.  Are you sure you want to cancel?", vbYesNo + vbQuestion, "Cancel Edit", Me)
            If blah = vbYes Then
                bolCheckFields = False
                fieldErrorIcon.Clear()
                DisableControls()
                ResetBackColors()
                Me.Refresh()
                RefreshData()
                Return True
            End If
        End If
        Return False
    End Function

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
                            c.BackColor = Colors.MissingField
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
                            cmb.BackColor = Colors.MissingField
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
            txtPhoneNumber.BackColor = Colors.MissingField
            AddErrorIcon(txtPhoneNumber)
        Else
            txtPhoneNumber.BackColor = Color.Empty
            ClearErrorIcon(txtPhoneNumber)
        End If
        Return Not bolMissingField 'if fields are missing return false to trigger a message if needed
    End Function

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

    Private Sub CollectCurrentTracking(results As DataTable)
        CurrentViewDevice.MapClassProperties(CurrentViewDevice.Tracking, results)
    End Sub

    Private Function ConcurrencyCheck() As Boolean
        Using DeviceResults = GetDevicesTable(CurrentViewDevice.GUID),
                HistoricalResults = GetHistoricalTable(CurrentViewDevice.GUID)
            DeviceResults.TableName = DevicesCols.TableName
            HistoricalResults.TableName = HistoricalDevicesCols.TableName
            Dim DBHash = GetHash(DeviceResults, HistoricalResults)
            If DBHash <> CurrentHash Then
                Message("This record appears to have been modified by someone else since the start of this modification.", vbOKOnly + vbExclamation, "Concurrency Error", Me)
                Return False
            End If
            Return True
        End Using
    End Function

    Private Sub DeleteDevice()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.DeleteDevice) Then Exit Sub
        Dim blah = Message("Are you absolutely sure?  This cannot be undone and will delete all historical data, tracking and attachments.", vbYesNo + vbExclamation, "WARNING", Me)
        If blah = vbYes Then
            If AssetFunc.DeleteFtpAndSql(CurrentViewDevice.GUID, EntryType.Device) Then
                Message("Device deleted successfully.", vbOKOnly + vbInformation, "Device Deleted", Me)
                CurrentViewDevice = Nothing
                MainForm.RefreshData()
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

    Private Function DeleteHistoryEntry(ByVal strGUID As String) As Integer
        Try
            Dim rows As Integer
            Dim DeleteEntryQuery As String = "DELETE FROM " & HistoricalDevicesCols.TableName & " WHERE " & HistoricalDevicesCols.HistoryEntryUID & "='" & strGUID & "'"
            rows = DBFactory.GetDatabase.ExecuteQuery(DeleteEntryQuery)
            Return rows
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return 0
        End Try
    End Function

    Private Sub DeleteSelectedHistoricalEntry()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifyDevice) Then Exit Sub
        Dim strGUID As String = GridFunctions.GetCurrentCellValue(DataGridHistory, HistoricalDevicesCols.HistoryEntryUID)
        Dim Info As DeviceObject
        Dim strQry = "SELECT * FROM " & HistoricalDevicesCols.TableName & " WHERE " & HistoricalDevicesCols.HistoryEntryUID & "='" & strGUID & "'"
        Using results As DataTable = DBFactory.GetDatabase.DataTableFromQueryString(strQry)
            Info = New DeviceObject(results)
        End Using
        Dim blah = Message("Are you absolutely sure?  This cannot be undone!" & vbCrLf & vbCrLf & "Entry info: " & Info.Historical.ActionDateTime & " - " & GetDisplayValueFromCode(DeviceAttribute.ChangeType, Info.Historical.ChangeType) & " - " & strGUID, vbYesNo + vbExclamation, "WARNING", Me)
        If blah = vbYes Then
            Message(DeleteHistoryEntry(strGUID) & " rows affected.", vbOKOnly + vbInformation, "Deletion Results", Me)
            LoadDevice(CurrentViewDevice.GUID)
        Else
            Exit Sub
        End If
    End Sub

    Private Async Sub DeployTeamViewer(device As DeviceObject)
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.IsAdmin) Then Exit Sub
        If Message("Deploy TeamViewer to this device?", vbYesNo + vbQuestion, "Are you sure?", Me) <> MsgBoxResult.Yes Then Exit Sub
        Try
            If SecurityTools.VerifyAdminCreds("For remote runspace access.") Then
                Dim NewTVDeploy As New TeamViewerDeploy
                StatusSlider.NewSlideMessage("Deploying TeamViewer...", 0)
                If Await NewTVDeploy.DeployToDevice(Me, device) Then
                    StatusSlider.NewSlideMessage("TeamViewer deployment complete!")
                Else
                    StatusSlider.NewSlideMessage("TeamViewer deployment failed...")
                End If
            End If
        Catch ex As Exception
            StatusSlider.NewSlideMessage("TeamViewer deployment failed...")
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            DoneWaiting()
        End Try
    End Sub

    Private Async Sub UpdateChrome(device As DeviceObject)
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.IsAdmin) Then Exit Sub
        If Message("Update/Install Chrome on this device?", vbYesNo + vbQuestion, "Are you sure?", Me) <> MsgBoxResult.Yes Then Exit Sub
        Try
            If SecurityTools.VerifyAdminCreds("For remote runspace access.") Then
                Waiting()
                StatusSlider.NewSlideMessage("Installing Chrome...", 0)
                Dim PSWrapper As New PowerShellWrapper
                If Await PSWrapper.ExecutePowerShellScript(device.HostName, My.Resources.UpdateChrome) Then
                    StatusSlider.NewSlideMessage("Chrome install complete!")
                    Message("Command successful.", vbOKOnly + vbInformation, "Done", Me)
                Else
                    StatusSlider.NewSlideMessage("Error while installing Chrome!")
                End If
            End If
        Catch ex As Exception
            StatusSlider.NewSlideMessage("Error while installing Chrome!")
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            DoneWaiting()
        End Try
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

    Private Sub DisableSorting(Grid As DataGridView)
        Dim c As DataGridViewColumn
        For Each c In Grid.Columns
            c.SortMode = DataGridViewColumnSortMode.NotSortable
        Next
    End Sub

    Private Sub DoneWaiting()
        SetWaitCursor(False, Me)
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

    Private Sub ExpandSplitter()
        If RemoteToolsBox.Visible Or TrackingBox.Visible Then
            InfoDataSplitter.Panel2Collapsed = False
        ElseIf Not RemoteToolsBox.Visible And Not TrackingBox.Visible Then
            InfoDataSplitter.Panel2Collapsed = True
        End If
    End Sub

    Private Sub ExpandSplitter(shouldExpand As Boolean)
        InfoDataSplitter.Panel2Collapsed = Not shouldExpand
    End Sub

    Private Sub FillTrackingBox()
        If CBool(CurrentViewDevice.Tracking.IsCheckedOut) Then
            txtCheckOut.BackColor = Colors.CheckOut
            txtCheckLocation.Text = CurrentViewDevice.Tracking.UseLocation
            lblCheckTime.Text = "CheckOut Time:"
            txtCheckTime.Text = CurrentViewDevice.Tracking.CheckoutTime.ToString
            lblCheckUser.Text = "CheckOut User:"
            txtCheckUser.Text = CurrentViewDevice.Tracking.CheckoutUser
            lblDueBack.Visible = True
            txtDueBack.Visible = True
            txtDueBack.Text = CurrentViewDevice.Tracking.DueBackTime.ToString
        Else
            txtCheckOut.BackColor = Colors.CheckIn
            txtCheckLocation.Text = GetDisplayValueFromCode(DeviceAttribute.Locations, CurrentViewDevice.Location)
            lblCheckTime.Text = "CheckIn Time:"
            txtCheckTime.Text = CurrentViewDevice.Tracking.CheckinTime.ToString
            lblCheckUser.Text = "CheckIn User:"
            txtCheckUser.Text = CurrentViewDevice.Tracking.CheckinUser
            lblDueBack.Visible = False
            txtDueBack.Visible = False
        End If
        txtCheckOut.Text = IIf(CurrentViewDevice.Tracking.IsCheckedOut, "Checked Out", "Checked In").ToString
    End Sub

    Private Function FormTitle(Device As DeviceObject) As String
        Return " - " + Device.CurrentUser + " - " + Device.AssetTag + " - " + Device.Description
    End Function

    Private Function GetDevicesTable(deviceUID As String) As DataTable
        Return DBFactory.GetDatabase.DataTableFromQueryString("Select * FROM " & DevicesCols.TableName & " WHERE " & DevicesCols.DeviceUID & " = '" & deviceUID & "'")
    End Function

    Private Function GetHash(deviceTable As DataTable, historicalTable As DataTable) As String
        Return SecurityTools.GetSHAOfTable(deviceTable) & SecurityTools.GetSHAOfTable(historicalTable)
    End Function

    Private Function GetHistoricalTable(deviceUID As String) As DataTable
        Return DBFactory.GetDatabase.DataTableFromQueryString("Select * FROM " & HistoricalDevicesCols.TableName & " WHERE " & HistoricalDevicesCols.DeviceUID & " = '" & deviceUID & "' ORDER BY " & HistoricalDevicesCols.ActionDateTime & " DESC")
    End Function

    Private Function GetInsertTable(selectQuery As String, UpdateInfo As DeviceUpdateInfoStruct) As DataTable
        Dim tmpTable = DataParser.ReturnInsertTable(selectQuery)
        Dim DBRow = tmpTable.Rows(0)
        'Add Add'l info
        DBRow(HistoricalDevicesCols.ChangeType) = UpdateInfo.ChangeType
        DBRow(HistoricalDevicesCols.Notes) = UpdateInfo.Note
        DBRow(HistoricalDevicesCols.ActionUser) = LocalDomainUser
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
        DBRow(DevicesCols.LastModUser) = LocalDomainUser
        DBRow(DevicesCols.LastModDate) = Now
        MunisUser = Nothing
        Return tmpTable
    End Function

    Private Function HistoricalGridColumns() As List(Of DataGridColumn)
        Dim ColList As New List(Of DataGridColumn)
        ColList.Add(New DataGridColumn(HistoricalDevicesCols.ActionDateTime, "Time Stamp", GetType(Date)))
        ColList.Add(New DataGridColumn(HistoricalDevicesCols.ChangeType, "Change Type", DeviceAttribute.ChangeType, ColumnFormatTypes.AttributeDisplayMemberOnly))
        ColList.Add(New DataGridColumn(HistoricalDevicesCols.ActionUser, "Action User", GetType(String)))
        ColList.Add(New DataGridColumn(HistoricalDevicesCols.Notes, "Note Peek", GetType(String), ColumnFormatTypes.NotePreview))
        ColList.Add(New DataGridColumn(HistoricalDevicesCols.CurrentUser, "User", GetType(String)))
        ColList.Add(New DataGridColumn(HistoricalDevicesCols.AssetTag, "Asset ID", GetType(String)))
        ColList.Add(New DataGridColumn(HistoricalDevicesCols.Serial, "Serial", GetType(String)))
        ColList.Add(New DataGridColumn(HistoricalDevicesCols.Description, "Description", GetType(String)))
        ColList.Add(New DataGridColumn(HistoricalDevicesCols.Location, "Location", DeviceAttribute.Locations, ColumnFormatTypes.AttributeDisplayMemberOnly))
        ColList.Add(New DataGridColumn(HistoricalDevicesCols.PurchaseDate, "Purchase Date", GetType(Date)))
        ColList.Add(New DataGridColumn(HistoricalDevicesCols.HistoryEntryUID, "GUID", GetType(String)))
        Return ColList
    End Function

    Private Sub InitDBControls()
        'Required Fields
        txtAssetTag_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.AssetTag, True)
        txtSerial_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.Serial, True)
        txtCurUser_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.CurrentUser, True)
        txtDescription_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.Description, True)
        dtPurchaseDate_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.PurchaseDate, True)
        cmbEquipType_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.EQType, DeviceAttribute.EquipType, True)
        cmbLocation_View_REQ.Tag = New DBControlInfo(DevicesBaseCols.Location, DeviceAttribute.Locations, True)
        cmbOSVersion_REQ.Tag = New DBControlInfo(DevicesBaseCols.OSVersion, DeviceAttribute.OSType, True)
        cmbStatus_REQ.Tag = New DBControlInfo(DevicesBaseCols.Status, DeviceAttribute.StatusType, True)

        'Non-required and Misc Fields
        txtPONumber.Tag = New DBControlInfo(DevicesBaseCols.PO, False)
        txtReplacementYear_View.Tag = New DBControlInfo(DevicesBaseCols.ReplacementYear, False)
        txtPhoneNumber.Tag = New DBControlInfo(DevicesBaseCols.PhoneNumber, False)
        lblGUID.Tag = New DBControlInfo(DevicesBaseCols.DeviceUID, ParseType.DisplayOnly, False)
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

    Private Sub LinkSibi()
        Using f As New SibiSelectorForm(Me)
            If f.DialogResult = DialogResult.OK Then
                CurrentViewDevice.SibiLink = f.SibiUID
                Message("Sibi Link Set.", vbOKOnly + vbInformation, "Success", Me)
            End If
        End Using
    End Sub

    Private Function LoadHistoryAndFields(deviceUID As String) As Boolean
        Try
            Using DeviceResults = GetDevicesTable(deviceUID),
                HistoricalResults = GetHistoricalTable(deviceUID)
                DeviceResults.TableName = DevicesCols.TableName
                HistoricalResults.TableName = HistoricalDevicesCols.TableName
                If DeviceResults.Rows.Count < 1 Then
                    CloseChildren(Me)
                    CurrentViewDevice = Nothing
                    Message("That device was not found!  It may have been deleted.  Re-execute your search.", vbOKOnly + vbExclamation, "Not Found", Me)
                    Return False
                End If
                CurrentHash = GetHash(DeviceResults, HistoricalResults)
                CurrentViewDevice = New DeviceObject(DeviceResults)
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

    Private Sub LoadTracking(strGUID As String)
        Dim strQry = "Select * FROM " & TrackablesCols.TableName & ", " & DevicesCols.TableName & " WHERE " & TrackablesCols.DeviceUID & " = " & DevicesCols.DeviceUID & " And " & TrackablesCols.DeviceUID & " = '" & strGUID & "' ORDER BY " & TrackablesCols.DateStamp & " DESC"
        Using Results As DataTable = DBFactory.GetDatabase.DataTableFromQueryString(strQry)
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

    Private Sub ModifyDevice()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifyDevice) Then Exit Sub
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

    Private Sub OpenSibiLink(LinkDevice As DeviceObject)
        Try
            If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ViewSibi) Then Exit Sub
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
        FillComboBox(DeviceAttribute.EquipType, cmbEquipType_View_REQ)
        FillComboBox(DeviceAttribute.Locations, cmbLocation_View_REQ)
        FillComboBox(DeviceAttribute.OSType, cmbOSVersion_REQ)
        FillComboBox(DeviceAttribute.StatusType, cmbStatus_REQ)
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
            If SecurityTools.VerifyAdminCreds() Then
                cmdRestart.Image = My.Resources.LoadingAni
                Dim FullPath As String = "\\" & IP
                Dim output As String = Await Task.Run(Function()
                                                          Using NetCon As New NetworkConnection(FullPath, SecurityTools.AdminCreds), p As Process = New Process
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

    Private Sub SendToHistGrid(Grid As DataGridView, results As DataTable)
        Try
            Using results
                If results.Rows.Count > 0 Then
                    GridFunctions.PopulateGrid(Grid, results, HistoricalGridColumns)
                Else
                    Grid.DataSource = Nothing
                End If
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub SendToTrackGrid(Grid As DataGridView, results As DataTable)
        Try
            Using results
                If results.Rows.Count > 0 Then
                    GridFunctions.PopulateGrid(Grid, results, TrackingGridColumns)
                Else
                    Grid.DataSource = Nothing
                End If
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
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

    Private Sub SetMunisEmpStatus()
        ToolTip1.SetToolTip(txtCurUser_View_REQ, "")
        If CurrentViewDevice.CurrentUserEmpNum <> "" Then
            txtCurUser_View_REQ.BackColor = Colors.EditColor
            ToolTip1.SetToolTip(txtCurUser_View_REQ, "Munis Linked Employee")
        End If
    End Sub

    Private Sub SetStatusBar(text As String)
        If StatusStrip1.InvokeRequired Then
            Dim d As New StatusVoidDelegate(AddressOf SetStatusBar)
            StatusStrip1.Invoke(d, New Object() {text})
        Else
            ' StatusLabel.Text = text
            StatusSlider.SlideText = text
            StatusStrip1.Update()
        End If
    End Sub
    Private Sub SetTracking(bolEnabled As Boolean, bolCheckedOut As Boolean)
        If bolEnabled Then
            If Not TabControl1.TabPages.Contains(TrackingTab) Then TabControl1.TabPages.Insert(1, TrackingTab)
            SetGridStyle(DataGridHistory)
            SetGridStyle(TrackingGrid)
            DataGridHistory.DefaultCellStyle.SelectionBackColor = GridTheme.CellSelectColor
            ExpandSplitter(True)
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
            ExpandSplitter()
        End If
    End Sub

    Private Sub SetupNetTools(PingResults As PingVis.PingInfo)
        If PingResults.Status <> IPStatus.Success Then
            intFailedPings += 1
        Else
            intFailedPings = 0
        End If
        If Not RemoteToolsBox.Visible And PingResults.Status = IPStatus.Success Then
            intFailedPings = 0
            cmdShowIP.Tag = PingResults.Address
            ExpandSplitter(True)
            RemoteToolsBox.Visible = True
        End If
        If intFailedPings > 10 And RemoteToolsBox.Visible Then
            RemoteToolsBox.Visible = False
            ExpandSplitter()
        End If
    End Sub

    Private Sub StartTrackDeviceForm()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.Tracking) Then Exit Sub
        Waiting()
        Dim NewTracking As New TrackDeviceForm(CurrentViewDevice, Me)
        DoneWaiting()
    End Sub

    Private Function TrackingGridColumns() As List(Of DataGridColumn)
        Dim ColList As New List(Of DataGridColumn)
        ColList.Add(New DataGridColumn(TrackablesCols.DateStamp, "Date", GetType(Date)))
        ColList.Add(New DataGridColumn(TrackablesCols.CheckType, "Check Type", GetType(String)))
        ColList.Add(New DataGridColumn(TrackablesCols.CheckoutUser, "Check Out User", GetType(String)))
        ColList.Add(New DataGridColumn(TrackablesCols.CheckinUser, "Check In User", GetType(String)))
        ColList.Add(New DataGridColumn(TrackablesCols.CheckoutTime, "Check Out", GetType(Date)))
        ColList.Add(New DataGridColumn(TrackablesCols.CheckinTime, "Check In", GetType(Date)))
        ColList.Add(New DataGridColumn(TrackablesCols.DueBackDate, "Due Back", GetType(Date)))
        ColList.Add(New DataGridColumn(TrackablesCols.UseLocation, "Location", GetType(String)))
        ColList.Add(New DataGridColumn(TrackablesCols.UID, "GUID", GetType(String)))
        Return ColList
    End Function

    Private Sub UpdateDevice(UpdateInfo As DeviceUpdateInfoStruct)
        Dim rows As Integer = 0
        Dim SelectQry As String = "SELECT * FROM " & DevicesCols.TableName & " WHERE " & DevicesCols.DeviceUID & "='" & CurrentViewDevice.GUID & "'"
        Dim InsertQry As String = "SELECT * FROM " & HistoricalDevicesCols.TableName & " LIMIT 0"
        Using trans = DBFactory.GetDatabase.StartTransaction, conn = trans.Connection
            Try
                rows += DBFactory.GetDatabase.UpdateTable(SelectQry, GetUpdateTable(SelectQry), trans)
                rows += DBFactory.GetDatabase.UpdateTable(InsertQry, GetInsertTable(InsertQry, UpdateInfo), trans)

                If rows = 2 Then
                    trans.Commit()
                    LoadDevice(CurrentViewDevice.GUID)
                    'Message("Update Added.", vbOKOnly + vbInformation, "Success", Me)
                    SetStatusBar("Update successful!")
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
    Private Sub ViewAttachments()
        If Not SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ViewAttachment) Then Exit Sub
        If Not AttachmentsIsOpen(Me) Then
            Dim NewAttachments As New AttachmentsForm(Me, New DeviceAttachmentsCols, CurrentViewDevice)
        End If
    End Sub
    Private Sub Waiting()
        SetWaitCursor(True, Me)
    End Sub
#Region "Control Events"

    Private Sub AssetDisposalFormToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AssetDisposalForm.Click
        Dim PDFForm As New PdfFormFilling(Me, CurrentViewDevice, PdfFormType.DisposeForm)
    End Sub

    Private Sub AttachmentTool_Click(sender As Object, e As EventArgs) Handles AttachmentTool.Click
        ViewAttachments()
    End Sub

    Private Sub CheckInTool_Click(sender As Object, e As EventArgs) Handles CheckInTool.Click
        StartTrackDeviceForm()
    End Sub

    Private Sub CheckOutTool_Click(sender As Object, e As EventArgs) Handles CheckOutTool.Click
        StartTrackDeviceForm()
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

    Private Sub cmdBrowseFiles_Click(sender As Object, e As EventArgs) Handles cmdBrowseFiles.Click
        BrowseFiles()
    End Sub

    Private Sub cmdCancel_Tool_Click(sender As Object, e As EventArgs) Handles cmdCancel_Tool.Click
        CancelModify()
    End Sub

    Private Sub cmdGKUpdate_Click(sender As Object, e As EventArgs) Handles cmdGKUpdate.Click
        If SecurityTools.VerifyAdminCreds() Then
            GKUpdaterForm.AddUpdate(CurrentViewDevice)
            If Not GKUpdaterForm.Visible Then GKUpdaterForm.Show()
        End If
    End Sub

    Private Sub cmdMunisInfo_Click(sender As Object, e As EventArgs) Handles cmdMunisInfo.Click
        Try
            MunisFunc.LoadMunisInfoByDevice(CurrentViewDevice, Me)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
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

    Private Sub cmdShowIP_Click(sender As Object, e As EventArgs) Handles cmdShowIP.Click
        If Not IsNothing(cmdShowIP.Tag) Then
            Dim blah = Message(cmdShowIP.Tag.ToString & " - " & NetworkInfo.LocationOfIP(cmdShowIP.Tag.ToString) & vbCrLf & vbCrLf & "Press 'Yes' to copy to clipboard.", vbInformation + vbYesNo, "IP Address", Me)
            If blah = vbYes Then
                Clipboard.SetText(cmdShowIP.Tag.ToString)
            End If
        End If
    End Sub

    Private Sub cmdSibiLink_Click(sender As Object, e As EventArgs) Handles cmdSibiLink.Click
        OpenSibiLink(CurrentViewDevice)
    End Sub

    Private Sub DataGridHistory_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridHistory.CellDoubleClick
        Dim EntryUID As String = GridFunctions.GetCurrentCellValue(DataGridHistory, HistoricalDevicesCols.HistoryEntryUID)
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

    Private Sub DeleteEntryToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DeleteEntryToolStripMenuItem.Click
        DeleteSelectedHistoricalEntry()
    End Sub

    Private Sub dtPurchaseDate_View_REQ_ValueChanged(sender As Object, e As EventArgs) Handles dtPurchaseDate_View_REQ.ValueChanged
        If bolCheckFields Then CheckFields()
    End Sub

    Private Sub lblGUID_Click(sender As Object, e As EventArgs) Handles lblGUID.Click
        Clipboard.SetText(lblGUID.Text)
        Message("GUID Copied to clipboard.", vbInformation + vbOKOnly,, Me)
    End Sub

    Private Sub RefreshToolStripButton_Click(sender As Object, e As EventArgs) Handles RefreshToolStripButton.Click
        RefreshData()
    End Sub

    Private Sub TabControl1_MouseDown(sender As Object, e As MouseEventArgs) Handles TabControl1.MouseDown
        TrackingGrid.Refresh()
    End Sub

    Private Sub tmr_RDPRefresher_Tick(sender As Object, e As EventArgs) Handles tmr_RDPRefresher.Tick
        CheckRDP()
    End Sub

    Private Sub TrackingGrid_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles TrackingGrid.CellDoubleClick
        Dim EntryUID = GridFunctions.GetCurrentCellValue(TrackingGrid, TrackablesCols.UID)
        If Not FormIsOpenByUID(GetType(ViewTrackingForm), EntryUID) Then
            NewTrackingView(EntryUID)
        End If
    End Sub

    Private Sub TrackingGrid_Paint(sender As Object, e As PaintEventArgs) Handles TrackingGrid.Paint
        Try
            TrackingGrid.Columns(TrackablesCols.CheckType).DefaultCellStyle.Font = New Font(TrackingGrid.Font, FontStyle.Bold)
        Catch
        End Try
    End Sub

    Private Sub TrackingGrid_RowPrePaint(sender As Object, e As DataGridViewRowPrePaintEventArgs) Handles TrackingGrid.RowPrePaint
        Dim c1 As Color = ColorTranslator.FromHtml("#8BCEE8") 'highlight color
        TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.ForeColor = Color.Black
        TrackingGrid.Rows(e.RowIndex).Cells(GridFunctions.GetColIndex(TrackingGrid, TrackablesCols.CheckType)).Style.Alignment = DataGridViewContentAlignment.MiddleCenter
        If TrackingGrid.Rows(e.RowIndex).Cells(GridFunctions.GetColIndex(TrackingGrid, TrackablesCols.CheckType)).Value.ToString = CheckType.Checkin Then
            TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Colors.CheckIn
            Dim c2 As Color = Color.FromArgb(Colors.CheckIn.R, Colors.CheckIn.G, Colors.CheckIn.B)
            Dim BlendColor As Color
            BlendColor = Color.FromArgb(CInt((CInt(c1.A) + CInt(c2.A)) / 2),
                                                CInt((CInt(c1.R) + CInt(c2.R)) / 2),
                                                CInt((CInt(c1.G) + CInt(c2.G)) / 2),
                                                CInt((CInt(c1.B) + CInt(c2.B)) / 2))
            TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.SelectionBackColor = BlendColor
        ElseIf TrackingGrid.Rows(e.RowIndex).Cells(GridFunctions.GetColIndex(TrackingGrid, TrackablesCols.CheckType)).Value.ToString = CheckType.Checkout Then
            TrackingGrid.Rows(e.RowIndex).DefaultCellStyle.BackColor = Colors.CheckOut
            Dim c2 As Color = Color.FromArgb(Colors.CheckOut.R, Colors.CheckOut.G, Colors.CheckOut.B)
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
        If Not OKToClose() Then
            e.Cancel = True
        Else
            '    DisposeImages(Me)
        End If

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

    Private Sub DeployTVButton_Click(sender As Object, e As EventArgs) Handles DeployTVButton.Click
        DeployTeamViewer(CurrentViewDevice)
    End Sub

    Private Sub UpdateChromeButton_Click(sender As Object, e As EventArgs) Handles UpdateChromeButton.Click
        UpdateChrome(CurrentViewDevice)
    End Sub

#End Region

#End Region

End Class