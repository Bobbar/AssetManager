Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class AddNew
    Private Device As New Device_Info
    Private bolCheckFields As Boolean
    Private MyLiveBox As New clsLiveBox(Me)
    Public MunisUser As Emp_Info = Nothing
    Private DataParser As New DBControlParser
    Private NewUID As String
    Private Sub cmdAdd_Click(sender As Object, e As EventArgs) Handles cmdAdd.Click
        AddDevice()
    End Sub
    Private Sub AddDevice()
        Try
            If Not CheckFields(Me, True) Then
                Dim blah = Message("Some required fields are missing or invalid.  Please fill and/or verify all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data", Me)
                bolCheckFields = True
                Exit Sub
            Else
                If Asset.DeviceExists(Trim(txtAssetTag_REQ.Text), Trim(txtSerial_REQ.Text)) Then
                    Dim blah = Message("A device with that serial and/or asset tag already exists.", vbOKOnly + vbExclamation, "Duplicate Device", Me)
                    Exit Sub
                Else
                    'proceed
                End If
                Dim Success As Boolean = AddNewDevice()
                If Success Then
                    Dim blah = Message("New Device Added.   Add another?", vbYesNo + vbInformation, "Complete", Me)
                    If Not chkNoClear.Checked Then ClearAll()
                    If blah = vbNo Then Me.Hide()
                    MainForm.RefreshCurrent()
                Else
                    Dim blah = Message("Something went wrong while adding a new device.", vbOKOnly + vbExclamation, "Unexpected Result", Me)
                End If

                Exit Sub
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Dim blah = Message("Unable to add new device.", vbOKOnly + vbExclamation, "Error", Me)
        End Try
    End Sub
    Private Function DeviceInsertTable(Adapter As MySqlDataAdapter) As DataTable
        Dim tmpTable = DataParser.ReturnInsertTable(Me, Adapter.SelectCommand.CommandText)
        Dim DBRow = tmpTable.Rows(0)
        'Add Add'l info
        If MunisUser.Number IsNot Nothing Then
            DBRow(devices.CurrentUser) = MunisUser.Name
            DBRow(devices.Munis_Emp_Num) = MunisUser.Number
        End If
        DBRow(devices.LastMod_User) = strLocalUser
        DBRow(devices.LastMod_Date) = Now
        DBRow(devices.DeviceUID) = NewUID
        DBRow(devices.CheckedOut) = False
        Return tmpTable
    End Function
    Private Function HistoryInsertTable(Adapter As MySqlDataAdapter) As DataTable
        Dim tmpTable = DataParser.ReturnInsertTable(Me, Adapter.SelectCommand.CommandText)
        Dim DBRow = tmpTable.Rows(0)
        'Add Add'l info
        DBRow(historical_dev.ChangeType) = "NEWD"
        DBRow(historical_dev.Notes) = Trim(txtNotes.Text)
        DBRow(historical_dev.ActionUser) = strLocalUser
        DBRow(historical_dev.DeviceUID) = NewUID
        Return tmpTable
    End Function
    Private Function AddNewDevice() As Boolean
        Try
            NewUID = Guid.NewGuid.ToString
            Dim rows As Integer = 0
            Dim DeviceInsertQry As String = "SELECT * FROM " & devices.TableName & " LIMIT 0"
            Dim HistoryInsertQry As String = "SELECT * FROM " & historical_dev.TableName & " LIMIT 0"
            Using SQLComms As New clsMySQL_Comms,
                DeviceInsertAdapter As MySqlDataAdapter = SQLComms.Return_Adapter(DeviceInsertQry),
                 HistoryInsertAdapter As MySqlDataAdapter = SQLComms.Return_Adapter(HistoryInsertQry)
                rows += DeviceInsertAdapter.Update(DeviceInsertTable(DeviceInsertAdapter))
                rows += HistoryInsertAdapter.Update(HistoryInsertTable(HistoryInsertAdapter))
            End Using
            If rows = 2 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
                Return False
            Else
                EndProgram()
            End If
        End Try
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
                    Dim cmb As ComboBox = ctl
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
    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        ClearAll()
    End Sub

    Private Sub AddNew_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClearAll()
        InitDBControls()
        MyLiveBox.AddControl(txtCurUser_REQ, LiveBoxType.UserSelect, devices.CurrentUser, devices.Munis_Emp_Num)
        MyLiveBox.AddControl(txtDescription_REQ, LiveBoxType.SelectValue, devices.Description)
        Icon = MainForm.Icon
        Tag = MainForm
    End Sub
    Private Sub InitDBControls()
        txtDescription_REQ.Tag = New DBControlInfo(devices_main.Description, True)
        txtAssetTag_REQ.Tag = New DBControlInfo(devices_main.AssetTag, True)
        txtSerial_REQ.Tag = New DBControlInfo(devices_main.Serial, True)
        dtPurchaseDate_REQ.Tag = New DBControlInfo(devices_main.PurchaseDate, True)
        txtReplaceYear.Tag = New DBControlInfo(devices_main.ReplacementYear, False)
        cmbLocation_REQ.Tag = New DBControlInfo(devices_main.Location, DeviceIndex.Locations, True)
        txtCurUser_REQ.Tag = New DBControlInfo(devices_main.CurrentUser, True)
        ' txtNotes.Tag = New DBControlInfo(historical_dev.Notes, False)
        cmbOSType_REQ.Tag = New DBControlInfo(devices_main.OSVersion, DeviceIndex.OSType, True)
        txtPhoneNumber.Tag = New DBControlInfo(devices_main.PhoneNumber, False)
        cmbEquipType_REQ.Tag = New DBControlInfo(devices_main.EQType, DeviceIndex.EquipType, True)
        cmbStatus_REQ.Tag = New DBControlInfo(devices_main.Status, DeviceIndex.StatusType, True)
        chkTrackable.Tag = New DBControlInfo(devices_main.Trackable, False)
        txtPO.Tag = New DBControlInfo(devices_main.PO, False)
    End Sub
    Private Sub ClearAll()
        RefreshCombos()
        ClearFields(Me)
        dtPurchaseDate_REQ.Value = Now
        cmbStatus_REQ.SelectedIndex = GetComboIndexFromShort(DeviceIndex.StatusType, "INSRV")
        ResetBackColors(Me)
        chkTrackable.Checked = False
        chkNoClear.Checked = False
        bolCheckFields = False
        fieldErrorIcon.Clear()
    End Sub
    Private Sub RefreshCombos()
        FillComboBox(DeviceIndex.Locations, cmbLocation_REQ)
        FillComboBox(DeviceIndex.EquipType, cmbEquipType_REQ)
        FillComboBox(DeviceIndex.OSType, cmbOSType_REQ)
        FillComboBox(DeviceIndex.StatusType, cmbStatus_REQ)
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
    Private Sub ClearFields(Parent As Control)
        MunisUser = Nothing
        For Each ctl As Control In Parent.Controls
            If TypeOf ctl Is TextBox Then
                Dim txt As TextBox = ctl
                txt.Text = ""
                txt.ReadOnly = False
            End If
            If TypeOf ctl Is ComboBox Then
                Dim cmb As ComboBox = ctl
                cmb.SelectedIndex = -1
            End If
            If ctl.HasChildren Then ClearFields(ctl)
        Next
    End Sub
    Private Sub SetReplacementYear(PurDate As Date)
        Dim ReplaceYear As Integer = PurDate.Year + intReplacementSched
        txtReplaceYear.Text = ReplaceYear.ToString
    End Sub
    Private Sub txtSerial_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtSerial_REQ.TextChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub
    Private Sub txtCurUser_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtCurUser_REQ.TextChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub
    Private Sub cmbLocation_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLocation_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub
    Private Sub cmbEquipType_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEquipType_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub
    Private Sub cmbStatus_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatus_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub
    Private Sub txtAssetTag_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag_REQ.TextChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub
    Private Sub txtDescription_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtDescription_REQ.TextChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub
    Private Sub dtPurchaseDate_REQ_ValueChanged(sender As Object, e As EventArgs) Handles dtPurchaseDate_REQ.ValueChanged
        If bolCheckFields Then CheckFields(Me, False)
        SetReplacementYear(dtPurchaseDate_REQ.Value)
    End Sub
    Private Sub cmbOSType_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbOSType_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields(Me, False)
    End Sub
    Private Sub cmbLocation_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbLocation_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub
    Private Sub cmbOSType_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbOSType_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub
    Private Sub cmbStatus_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbStatus_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub
    Private Sub cmbEquipType_REQ_DropDown(sender As Object, e As EventArgs) Handles cmbEquipType_REQ.DropDown
        AdjustComboBoxWidth(sender, e)
    End Sub
    Private Sub AddNew_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        MyLiveBox.Dispose()
    End Sub
    Private Sub cmdUserSearch_Click(sender As Object, e As EventArgs) Handles cmdUserSearch.Click
        Dim NewMunisSearch As New frmMunisUser(Me)
        MunisUser = NewMunisSearch.EmployeeInfo
        If MunisUser.Number <> "" Then
            txtCurUser_REQ.Text = MunisUser.Name
            txtCurUser_REQ.ReadOnly = True
        End If
    End Sub
    Private Sub txtPhoneNumber_Leave(sender As Object, e As EventArgs) Handles txtPhoneNumber.Leave
        If Trim(txtPhoneNumber.Text) <> "" And Not ValidPhoneNumber(txtPhoneNumber.Text) Then
            Message("Invalid phone number.", vbOKOnly + vbExclamation, "Error", Me)
            txtPhoneNumber.Focus()
        End If
    End Sub
End Class
