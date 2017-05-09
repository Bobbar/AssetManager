Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class AddNew
    Private Device As New Device_Info
    Private bolCheckFields As Boolean
    Private MyLiveBox As New clsLiveBox(Me)
    Public MunisUser As Emp_Info = Nothing
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
                Dim NewDevice As Device_Info = GetDBValues()
                If Asset.DeviceExists(NewDevice) Then
                    Dim blah = Message("A device with that serial and/or asset tag already exists.", vbOKOnly + vbExclamation, "Duplicate Device", Me)
                    Exit Sub
                Else
                    'proceed
                End If
                Dim Success As Boolean = Asset.AddNewDevice(NewDevice, MunisUser)
                If Success Then
                    Dim blah = Message("New Device Added.   Add another?", vbYesNo + vbInformation, "Complete", Me)
                    If Not chkNoClear.Checked Then ClearAll()
                    If blah = vbNo Then Me.Hide()
                    MainForm.RefreshCurrent()
                Else
                    Dim blah = Message("Unsuccessful! The number of affected rows was not what was expected.", vbOKOnly + vbExclamation, "Unexpected Result", Me)
                End If

                Exit Sub
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Dim blah = Message("Unable to add new device.", vbOKOnly + vbExclamation, "Error", Me)
        End Try
    End Sub
    Private Function CheckFields(Parent As Control, bolValidFields As Boolean) As Boolean
        For Each ctl As Control In Parent.Controls
            Select Case True
                Case TypeOf ctl Is TextBox
                    If ctl.Tag = True Then
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
                    If ctl.Tag = True Then
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
    Private Function GetDBValues() As Device_Info 'cleanup user input for db
        Dim tmpDevice As New Device_Info
        With tmpDevice
            .strSerial = Trim(txtSerial_REQ.Text)
            .strDescription = Trim(txtDescription_REQ.Text)
            .strAssetTag = Trim(txtAssetTag_REQ.Text)
            .dtPurchaseDate = dtPurchaseDate_REQ.Text
            .strReplaceYear = Trim(txtReplaceYear.Text)
            .strLocation = GetDBValue(DeviceIndex.Locations, cmbLocation_REQ.SelectedIndex)
            If IsNothing(MunisUser.Number) Then
                .strCurrentUser = Trim(txtCurUser_REQ.Text)
            Else
                .strCurrentUser = MunisUser.Name
            End If
            .strNote = Trim(txtNotes.Text)
            .strOSVersion = GetDBValue(DeviceIndex.OSType, cmbOSType_REQ.SelectedIndex)
            .strPhoneNumber = PhoneNumberToDB(txtPhoneNumber.Text)
            .strEqType = GetDBValue(DeviceIndex.EquipType, cmbEquipType_REQ.SelectedIndex)
            .strStatus = GetDBValue(DeviceIndex.StatusType, cmbStatus_REQ.SelectedIndex)
            .bolTrackable = chkTrackable.Checked
            .strPO = Trim(txtPO.Text)
        End With
        Return tmpDevice
    End Function
    Private Sub AddNew_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClearAll()
        MyLiveBox.AddControl(txtCurUser_REQ, LiveBoxType.UserSelect, devices.CurrentUser, devices.Munis_Emp_Num)
        MyLiveBox.AddControl(txtDescription_REQ, LiveBoxType.SelectValue, devices.Description)
        Icon = MainForm.Icon
        Tag = MainForm
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
    Private Sub txtPhoneNumber_LostFocus(sender As Object, e As EventArgs) Handles txtPhoneNumber.LostFocus
        txtPhoneNumber.Text = FormatPhoneNumber(txtPhoneNumber.Text)
    End Sub
End Class
