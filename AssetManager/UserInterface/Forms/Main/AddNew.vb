Option Explicit On
Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class AddNew
    Private Device As Device_Info
    Private bolCheckFields As Boolean
    Private MyLiveBox As New clsLiveBox
    Public MunisUser As Emp_Info = Nothing
    Private Sub cmdAdd_Click(sender As Object, e As EventArgs) Handles cmdAdd.Click
        AddDevice()
    End Sub
    Private Sub AddDevice()
        If Not CheckFields() Then
            Dim blah = Message("Some required fields are missing.  Please fill in all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data")
            bolCheckFields = True
            Exit Sub
        Else
            Dim NewDevice As Device_Info = GetDBValues()
            Dim Success As Boolean = Asset.AddNewDevice(NewDevice, MunisUser)
            If Success Then
                Dim blah = Message("New Device Added.   Add another?", vbYesNo + vbInformation, "Complete")
                If Not chkNoClear.Checked Then ClearAll()
                If blah = vbNo Then Me.Hide()
                MainForm.RefreshCurrent()
            Else
                Dim blah = Message("Unsuccessful! The number of affected rows was not what was expected.", vbOKOnly + vbExclamation, "Unexpected Result")
            End If

            Exit Sub
        End If
    End Sub
    Private Function CheckFields() As Boolean
        Dim bolMissingField As Boolean
        bolMissingField = False
        Dim c As Control
        For Each c In GroupBox1.Controls
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
    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        ClearAll()
    End Sub
    Private Function GetDBValues() As Device_Info 'cleanup user input for db
        Dim tmpDevice As Device_Info
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
            .strEqType = GetDBValue(DeviceIndex.EquipType, cmbEquipType_REQ.SelectedIndex)
            .strStatus = GetDBValue(DeviceIndex.StatusType, cmbStatus_REQ.SelectedIndex)
            .bolTrackable = chkTrackable.Checked
            .strPO = Trim(txtPO.Text)
        End With
        Return tmpDevice
    End Function
    Private Sub AddNew_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClearAll()
    End Sub
    Private Sub ClearAll()
        RefreshCombos()
        ClearFields()
        dtPurchaseDate_REQ.Value = Now
        cmbStatus_REQ.SelectedIndex = GetComboIndexFromShort(DeviceIndex.StatusType, "INSRV")
        ResetBackColors()
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
    Private Sub ResetBackColors()
        Dim c As Control
        For Each c In GroupBox1.Controls
            Select Case True
                Case TypeOf c Is TextBox
                    c.BackColor = Color.Empty
                Case TypeOf c Is ComboBox
                    c.BackColor = Color.Empty
            End Select
        Next
    End Sub
    Private Sub ClearFields()
        MunisUser = Nothing
        Dim c As Control
        For Each c In GroupBox1.Controls
            If TypeOf c Is TextBox Then
                Dim txt As TextBox = c
                txt.Text = ""
                txt.ReadOnly = False
            End If
            If TypeOf c Is ComboBox Then
                Dim cmb As ComboBox = c
                cmb.SelectedIndex = -1
            End If
        Next
    End Sub
    Private Sub Button1_Click(sender As Object, e As EventArgs)
        If Not CheckFields() Then Dim blah = Message("Some required fields are missing.  Please fill in all highlighted fields.", vbOKOnly + vbExclamation, "Missing Data")
    End Sub
    Private Sub txtSerial_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtSerial_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub txtCurUser_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtCurUser_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmbLocation_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbLocation_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmbEquipType_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbEquipType_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmbStatus_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbStatus_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub txtAssetTag_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub txtDescription_REQ_TextChanged(sender As Object, e As EventArgs) Handles txtDescription_REQ.TextChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub dtPurchaseDate_REQ_ValueChanged(sender As Object, e As EventArgs) Handles dtPurchaseDate_REQ.ValueChanged
        If bolCheckFields Then CheckFields()
    End Sub
    Private Sub cmbOSType_REQ_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbOSType_REQ.SelectedIndexChanged
        If bolCheckFields Then CheckFields()
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
    Private Sub txtCurUser_REQ_KeyUp(sender As Object, e As KeyEventArgs) Handles txtCurUser_REQ.KeyUp
        MyLiveBox.StartLiveSearch(sender, MyLiveBox.LiveBoxType.UserSelect, "dev_cur_user", "dev_cur_user_emp_num")
    End Sub
    Private Sub txtDescription_REQ_KeyUp(sender As Object, e As KeyEventArgs) Handles txtDescription_REQ.KeyUp
        MyLiveBox.StartLiveSearch(sender, MyLiveBox.LiveBoxType.SelectValue, "dev_description")
    End Sub
    Private Sub AddNew_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        MyLiveBox.Unload()
    End Sub
    Private Sub cmdUserSearch_Click(sender As Object, e As EventArgs) Handles cmdUserSearch.Click
        Dim NewMunisSearch As New frmMunisUser
        NewMunisSearch.ShowDialog()
        If NewMunisSearch.DialogResult = DialogResult.Yes Then
            MunisUser = NewMunisSearch.EmployeeInfo
            NewMunisSearch.Dispose()
            txtCurUser_REQ.Text = MunisUser.Name
            txtCurUser_REQ.ReadOnly = True
        End If
    End Sub
End Class
