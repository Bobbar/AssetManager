Public Class NewDeviceForm

#Region "Fields"

    Public MunisUser As MunisEmployeeStruct = Nothing
    Private ReadOnly intReplacementSched As Integer = 4
    Private bolCheckFields As Boolean
    Private DataParser As New DBControlParser(Me)
    Private Device As New DeviceObject
    Private MyLiveBox As New LiveBox(Me)
    Private NewUID As String

#End Region

#Region "Methods"
    Sub New(parentForm As ExtendedForm)
        InitializeComponent()
        Me.ParentForm = parentForm
        Me.Owner = parentForm
        ClearAll()
        InitDBControls()
        MyLiveBox.AttachToControl(txtCurUser_REQ, LiveBoxType.UserSelect, DevicesCols.CurrentUser, DevicesCols.MunisEmpNum)
        MyLiveBox.AttachToControl(txtDescription_REQ, LiveBoxType.SelectValue, DevicesCols.Description)
        Me.Show()
        Me.Activate()
    End Sub

    Private Sub AddDevice()
        Try
            If Not CheckFields(Me, True) Then
                Message("Some required fields are missing or invalid.  Please fill and/or verify all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data", Me)
                bolCheckFields = True
                Exit Sub
            Else
                If AssetFunc.DeviceExists(Trim(txtAssetTag_REQ.Text), Trim(txtSerial_REQ.Text)) Then
                    Message("A device with that serial and/or asset tag already exists.", vbOKOnly + vbExclamation, "Duplicate Device", Me)
                    Exit Sub
                Else
                    'proceed
                End If
                Dim Success As Boolean = AddNewDevice()
                If Success Then
                    Dim blah = Message("New Device Added.   Add another?", vbYesNo + vbInformation, "Complete", Me)
                    If Not chkNoClear.Checked Then ClearAll()
                    If blah = vbNo Then Me.Dispose()
                    ParentForm.RefreshData()
                Else
                    Message("Something went wrong while adding a new device.", vbOKOnly + vbExclamation, "Unexpected Result", Me)
                End If

                Exit Sub
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Message("Unable to add new device.", vbOKOnly + vbExclamation, "Error", Me)
        End Try
    End Sub

    Private Sub AddErrorIcon(ctl As Control)
        If fieldErrorIcon.GetError(ctl) Is String.Empty Then
            fieldErrorIcon.SetIconAlignment(ctl, ErrorIconAlignment.MiddleRight)
            fieldErrorIcon.SetIconPadding(ctl, 4)
            fieldErrorIcon.SetError(ctl, "Required Field")
        End If
    End Sub

    Private Sub NewDeviceForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        MyLiveBox.Dispose()
    End Sub
    Private Function AddNewDevice() As Boolean
        Using trans = DBFactory.GetDatabase.StartTransaction, conn = trans.Connection
            Try
                NewUID = Guid.NewGuid.ToString
                Dim rows As Integer = 0
                Dim DeviceInsertQry As String = "SELECT * FROM " & DevicesCols.TableName & " LIMIT 0"
                Dim HistoryInsertQry As String = "SELECT * FROM " & HistoricalDevicesCols.TableName & " LIMIT 0"

                rows += DBFactory.GetDatabase.UpdateTable(DeviceInsertQry, DeviceInsertTable(DeviceInsertQry), trans)
                rows += DBFactory.GetDatabase.UpdateTable(HistoryInsertQry, HistoryInsertTable(HistoryInsertQry), trans)

                If rows = 2 Then
                    trans.Commit()
                    Return True
                Else
                    trans.Rollback()
                    Return False
                End If
            Catch ex As Exception
                trans.Rollback()
                ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
                Return False
            End Try
        End Using
    End Function

    Private Function CheckFields(Parent As Control, bolValidFields As Boolean) As Boolean
        For Each ctl As Control In Parent.Controls
            Dim DBInfo As New DBControlInfo
            If ctl.Tag IsNot Nothing Then DBInfo = DirectCast(ctl.Tag, DBControlInfo)
            Select Case True
                Case TypeOf ctl Is TextBox
                    If DBInfo.Required Then
                        If Trim(ctl.Text) = "" Then
                            bolValidFields = False
                            ctl.BackColor = colMissingField
                            AddErrorIcon(ctl)
                        Else
                            ctl.BackColor = Color.Empty
                            ClearErrorIcon(ctl)
                        End If
                    End If
                    If ctl Is txtPhoneNumber Then
                        If Trim(txtPhoneNumber.Text) <> "" And Not ValidPhoneNumber(txtPhoneNumber.Text) Then
                            bolValidFields = False
                            AddErrorIcon(ctl)
                            Message("Invalid phone number.", vbOKOnly + vbExclamation, "Error", Me)
                        End If
                    End If
                Case TypeOf ctl Is ComboBox
                    Dim cmb As ComboBox = DirectCast(ctl, ComboBox)
                    If DBInfo.Required Then
                        If cmb.SelectedIndex = -1 Then
                            bolValidFields = False
                            cmb.BackColor = colMissingField
                            AddErrorIcon(cmb)
                        Else
                            cmb.BackColor = Color.Empty
                            ClearErrorIcon(cmb)
                        End If
                    End If
            End Select
            If ctl.HasChildren Then
                bolValidFields = CheckFields(ctl, bolValidFields)
            End If
        Next
        Return bolValidFields 'if fields are missing return false to trigger a message if needed
    End Function

    Private Sub ClearAll()
        RefreshCombos()
        ClearFields(Me)
        dtPurchaseDate_REQ.Value = Now
        cmbStatus_REQ.SelectedIndex = GetComboIndexFromCode(DeviceAttribute.StatusType, "INSRV")
        ResetBackColors(Me)
        chkTrackable.Checked = False
        chkNoClear.Checked = False
        bolCheckFields = False
        fieldErrorIcon.Clear()
    End Sub

    Private Sub ClearErrorIcon(ctl As Control)
        fieldErrorIcon.SetError(ctl, String.Empty)
    End Sub

    Private Sub ClearFields(Parent As Control)
        MunisUser = Nothing
        For Each ctl As Control In Parent.Controls
            If TypeOf ctl Is TextBox Then
                Dim txt As TextBox = DirectCast(ctl, TextBox)
                txt.Text = ""
                txt.ReadOnly = False
            End If
            If TypeOf ctl Is ComboBox Then
                Dim cmb As ComboBox = DirectCast(ctl, ComboBox)
                cmb.SelectedIndex = -1
            End If
            If ctl.HasChildren Then ClearFields(ctl)
        Next
    End Sub

    Private Sub cmbEquipType_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbEquipType_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub

    Private Sub cmbEquipType_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEquipType_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub

    Private Sub cmbLocation_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbLocation_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub

    Private Sub cmbLocation_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLocation_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub

    Private Sub cmbOSType_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbOSType_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub

    Private Sub cmbOSType_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbOSType_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields(Me, False)
        SetHostname()
    End Sub

    Private Sub cmbStatus_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbStatus_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub

    Private Sub cmbStatus_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatus_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub

    Private Sub cmdAdd_Click(sender As Object, e As EventArgs) Handles cmdAdd.Click
        AddDevice()
    End Sub
    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        ClearAll()
    End Sub

    Private Sub cmdUserSearch_Click(sender As Object, e As EventArgs) Handles cmdUserSearch.Click
        Using NewMunisSearch As New MunisUserForm(Me)
            MunisUser = NewMunisSearch.EmployeeInfo
            If MunisUser.Number <> "" Then
                txtCurUser_REQ.Text = MunisUser.Name
                txtCurUser_REQ.ReadOnly = True
            End If
        End Using
    End Sub

    Private Function DeviceInsertTable(selectQuery As String) As DataTable
        Dim tmpTable = DataParser.ReturnInsertTable(selectQuery)
        Dim DBRow = tmpTable.Rows(0)
        'Add Add'l info
        If MunisUser.Number IsNot Nothing Then
            DBRow(DevicesCols.CurrentUser) = MunisUser.Name
            DBRow(DevicesCols.MunisEmpNum) = MunisUser.Number
        End If
        DBRow(DevicesCols.LastModUser) = LocalDomainUser
        DBRow(DevicesCols.LastModDate) = Now
        DBRow(DevicesCols.DeviceUID) = NewUID
        DBRow(DevicesCols.CheckedOut) = False
        Return tmpTable
    End Function

    Private Sub dtPurchaseDate_REQ_ValueChanged(sender As Object, e As EventArgs) Handles dtPurchaseDate_REQ.ValueChanged
        If bolCheckFields Then CheckFields(Me, False)
        SetReplacementYear(dtPurchaseDate_REQ.Value)
    End Sub

    Private Function HistoryInsertTable(selectQuery As String) As DataTable
        Dim tmpTable = DataParser.ReturnInsertTable(selectQuery)
        Dim DBRow = tmpTable.Rows(0)
        'Add Add'l info
        DBRow(HistoricalDevicesCols.ChangeType) = "NEWD"
        DBRow(HistoricalDevicesCols.Notes) = Trim(txtNotes.Text)
        DBRow(HistoricalDevicesCols.ActionUser) = LocalDomainUser
        DBRow(HistoricalDevicesCols.DeviceUID) = NewUID
        Return tmpTable
    End Function
    Private Sub InitDBControls()
        txtDescription_REQ.Tag = New DBControlInfo(DevicesBaseCols.Description, True)
        txtAssetTag_REQ.Tag = New DBControlInfo(DevicesBaseCols.AssetTag, True)
        txtSerial_REQ.Tag = New DBControlInfo(DevicesBaseCols.Serial, True)
        dtPurchaseDate_REQ.Tag = New DBControlInfo(DevicesBaseCols.PurchaseDate, True)
        txtReplaceYear.Tag = New DBControlInfo(DevicesBaseCols.ReplacementYear, False)
        cmbLocation_REQ.Tag = New DBControlInfo(DevicesBaseCols.Location, DeviceAttribute.Locations, True)
        txtCurUser_REQ.Tag = New DBControlInfo(DevicesBaseCols.CurrentUser, True)
        ' txtNotes.Tag = New DBControlInfo(historical_dev.Notes, False)
        cmbOSType_REQ.Tag = New DBControlInfo(DevicesBaseCols.OSVersion, DeviceAttribute.OSType, True)
        txtPhoneNumber.Tag = New DBControlInfo(DevicesBaseCols.PhoneNumber, False)
        cmbEquipType_REQ.Tag = New DBControlInfo(DevicesBaseCols.EQType, DeviceAttribute.EquipType, True)
        cmbStatus_REQ.Tag = New DBControlInfo(DevicesBaseCols.Status, DeviceAttribute.StatusType, True)
        chkTrackable.Tag = New DBControlInfo(DevicesBaseCols.Trackable, False)
        txtPO.Tag = New DBControlInfo(DevicesBaseCols.PO, False)
        txtHostname.Tag = New DBControlInfo(DevicesBaseCols.HostName, False)
        iCloudTextBox.Tag = New DBControlInfo(DevicesBaseCols.iCloudAccount, False)
    End Sub

    Private Sub RefreshCombos()
        FillComboBox(DeviceAttribute.Locations, cmbLocation_REQ)
        FillComboBox(DeviceAttribute.EquipType, cmbEquipType_REQ)
        FillComboBox(DeviceAttribute.OSType, cmbOSType_REQ)
        FillComboBox(DeviceAttribute.StatusType, cmbStatus_REQ)
    End Sub

    Private Sub ResetBackColors(Parent As Control)
        For Each ctl As Control In Parent.Controls
            Select Case True
                Case TypeOf ctl Is TextBox
                    ctl.BackColor = Color.Empty
                Case TypeOf ctl Is ComboBox
                    ctl.BackColor = Color.Empty
            End Select
            If ctl.HasChildren Then ResetBackColors(ctl)
        Next
    End Sub
    Private Sub SetReplacementYear(PurDate As Date)
        Dim ReplaceYear As Integer = PurDate.Year + intReplacementSched
        txtReplaceYear.Text = ReplaceYear.ToString
    End Sub

    Private Sub txtAssetTag_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag_REQ.TextChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub

    Private Sub txtCurUser_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtCurUser_REQ.TextChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub

    Private Sub txtDescription_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtDescription_REQ.TextChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub

    Private Sub txtPhoneNumber_Leave(sender As Object, e As EventArgs) Handles txtPhoneNumber.Leave
        If Trim(txtPhoneNumber.Text) <> "" And Not ValidPhoneNumber(txtPhoneNumber.Text) Then
            Message("Invalid phone number.", vbOKOnly + vbExclamation, "Error", Me)
            txtPhoneNumber.Focus()
        End If
    End Sub

    Private Sub txtSerial_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtSerial_REQ.TextChanged
        If bolCheckFields Then CheckFields(Me, False)
        SetHostname()
    End Sub

    Private Sub SetHostname()
        If txtSerial_REQ.Text <> "" AndAlso GetDBValue(DeviceAttribute.OSType, cmbOSType_REQ.SelectedIndex).Contains("WIN") Then
            txtHostname.Text = DeviceHostnameFormat(txtSerial_REQ.Text)
        Else
            txtHostname.Text = String.Empty
        End If
    End Sub


#End Region

End Class