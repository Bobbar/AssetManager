Imports MySql.Data.MySqlClient
Public Class Tracking
    Private Structure CheckStruct
        Public strCheckOutTime As String
        Public strDueDate As String
        Public strUseLocation As String
        Public strUseReason As String
        Public strCheckInNotes As String
        Public strDeviceUID As String
        Public strCheckOutUser As String
        Public strCheckInUser As String
        Public strCheckInTime As String
    End Structure
    Private CheckData As CheckStruct
    Private Function GetCheckData() As Boolean
        'Dim bolEmptyFields As Boolean = False
        If Not CurrentDevice.Trackable.bolCheckedOut Then
            Dim c As Control
            For Each c In CheckOutBox.Controls
                If TypeOf c Is TextBox Then
                    If c.Visible Then
                        If Trim(c.Text) = "" Then
                            Dim blah = MsgBox("Please complete all fields.", vbOKOnly + vbExclamation, "Missing Data")
                            Return False
                        End If
                    End If
                End If
            Next
        Else
            Dim c As Control
            For Each c In CheckInBox.Controls
                If TypeOf c Is TextBox Then
                    If Trim(c.Text) = "" Then
                        Dim blah = MsgBox("Please complete all fields.", vbOKOnly + vbExclamation, "Missing Data")
                        Return False
                    End If
                End If
            Next
        End If
        With CheckData
            .strCheckOutTime = dtCheckOut.Value.ToString(strDBDateTimeFormat)
            .strDueDate = dtDueBack.Value.ToString(strDBDateTimeFormat)
            .strUseLocation = UCase(Trim(txtUseLocation.Text))
            .strUseReason = UCase(Trim(txtUseReason.Text))
            .strCheckInNotes = UCase(Trim(txtCheckInNotes.Text))
            .strDeviceUID = CurrentDevice.strGUID
            .strCheckOutUser = strLocalUser
            .strCheckInTime = dtCheckIn.Value.ToString(strDBDateTimeFormat)
            .strCheckInUser = strLocalUser
        End With
        Return True
    End Function
    Private Sub GetCurrentTracking(strGUID As String)
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim ds As New DataSet
        Dim da As New MySqlDataAdapter
        Dim dt As DataTable
        Dim dr As DataRow
        'Dim strQryRow As String
        da.SelectCommand = New MySqlCommand("SELECT * FROM trackable WHERE track_device_uid='" & strGUID & "' ORDER BY track_datestamp DESC LIMIT 1")
        da.SelectCommand.Connection = GetConnection(ConnID).DBConnection
        da.Fill(ds)
        CloseConnection(ConnID)
        dt = ds.Tables(0)
        Debug.Print(dt.Rows.Count)
        If dt.Rows.Count > 0 Then
            For Each dr In dt.Rows
                With dr
                    CurrentDevice.Trackable.strCheckOutTime = .Item("track_checkout_time").ToString
                    CurrentDevice.Trackable.strCheckInTime = .Item("track_checkin_time").ToString
                    CurrentDevice.Trackable.strUseLocation = .Item("track_use_location").ToString
                    CurrentDevice.Trackable.strCheckOutUser = .Item("track_checkout_user").ToString
                    CurrentDevice.Trackable.strCheckInUser = .Item("track_checkin_user").ToString
                    CurrentDevice.Trackable.strDueBackTime = .Item("track_dueback_date").ToString
                    CurrentDevice.Trackable.strUseReason = .Item("track_notes").ToString
                End With
            Next
        End If
    End Sub
    Private Sub Tracking_Load(sender As Object, e As EventArgs) Handles Me.Load
        ClearAll()
        SetDates()
        SetGroups()
        GetCurrentTracking(CurrentDevice.strGUID)
        LoadTracking()
    End Sub
    Private Sub LoadTracking()
        txtAssetTag.Text = CurrentDevice.strAssetTag
        txtDescription.Text = CurrentDevice.strDescription
        txtSerial.Text = CurrentDevice.strSerial
        txtDeviceType.Text = GetHumanValue(ComboType.EquipType, CurrentDevice.strEqType)
        If CurrentDevice.Trackable.bolCheckedOut Then
            dtCheckOut.Value = CurrentDevice.Trackable.strCheckOutTime
            dtDueBack.Value = CurrentDevice.Trackable.strDueBackTime
            txtUseLocation.Text = CurrentDevice.Trackable.strUseLocation
            txtUseReason.Text = CurrentDevice.Trackable.strUseReason
        End If
    End Sub
    Private Sub ClearAll()
        Dim c As Control
        For Each c In Me.Controls
            If TypeOf c Is TextBox Then
                Dim txt As TextBox = c
                txt.Text = ""
            End If
        Next
    End Sub
    Private Sub SetDates()
        dtCheckOut.Value = Now
        dtCheckIn.Value = Now
        dtCheckOut.Enabled = False
        dtCheckIn.Enabled = False
        Debug.Print(dtCheckOut.Value.ToString(strDBDateTimeFormat))
    End Sub
    Private Sub SetGroups()
        CheckInBox.Enabled = CurrentDevice.Trackable.bolCheckedOut
        CheckOutBox.Enabled = Not CurrentDevice.Trackable.bolCheckedOut
    End Sub
    Private Sub CheckOut()
        'On Error GoTo errs
        If Not GetCheckData() Then Exit Sub
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim rows As Integer
        Dim strSQLQry1 = "UPDATE devices SET dev_checkedout='1' WHERE dev_UID='" & CurrentDevice.strGUID & "'"
        Dim cmd As New MySqlCommand
        cmd.Connection = GetConnection(ConnID).DBConnection
        cmd.CommandText = strSQLQry1
        rows = rows + cmd.ExecuteNonQuery()
        Dim strSqlQry2 = "INSERT INTO trackable (track_check_type, track_checkout_time, track_dueback_date, track_checkout_user, track_use_location, track_notes, track_device_uid) VALUES ('" & strCheckOut & "','" & CheckData.strCheckOutTime & "','" & CheckData.strDueDate & "','" & CheckData.strCheckOutUser & "','" & CheckData.strUseLocation & "','" & CheckData.strUseReason & "','" & CheckData.strDeviceUID & "')"
        cmd.CommandText = strSqlQry2
        rows = rows + cmd.ExecuteNonQuery()
        CloseConnection(ConnID)
        UpdateDev.strNewNote = Nothing
        If rows = 2 Then
            Dim blah = MsgBox("Device Checked Out!", vbOKOnly + vbInformation, "Success")
        Else
            Dim blah = MsgBox("Unsuccessful! The number of affected rows was not what was expected.", vbOKOnly + vbAbort, "Unexpected Result")
        End If
        View.ViewTracking(CurrentDevice.strGUID)
        View.ViewDevice(CurrentDevice.strGUID)
        Me.Hide()
        Exit Sub
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Resume Next
        Else
            EndProgram()
        End If
    End Sub
    Private Sub CheckIn()
        'On Error GoTo errs
        If Not GetCheckData() Then Exit Sub
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim rows As Integer
        Dim strSQLQry1 = "UPDATE devices SET dev_checkedout='0' WHERE dev_UID='" & CurrentDevice.strGUID & "'"
        Dim cmd As New MySqlCommand
        cmd.Connection = GetConnection(ConnID).DBConnection
        cmd.CommandText = strSQLQry1
        rows = rows + cmd.ExecuteNonQuery()
        Dim strSqlQry2 = "INSERT INTO trackable (track_check_type, track_checkout_time, track_dueback_date, track_checkin_time, track_checkout_user, track_checkin_user, track_use_location, track_notes, track_device_uid) VALUES ('" & strCheckIn & "','" & CheckData.strCheckOutTime & "','" & CheckData.strDueDate & "','" & CheckData.strCheckInTime & "','" & CheckData.strCheckOutUser & "','" & CheckData.strCheckInUser & "','" & CheckData.strUseLocation & "','" & CheckData.strCheckInNotes & "','" & CheckData.strDeviceUID & "')"
        cmd.CommandText = strSqlQry2
        rows = rows + cmd.ExecuteNonQuery()
        CloseConnection(ConnID)
        UpdateDev.strNewNote = Nothing
        If rows = 2 Then
            Dim blah = MsgBox("Device Checked In!", vbOKOnly + vbInformation, "Success")
        Else
            Dim blah = MsgBox("Unsuccessful! The number of affected rows was not what was expected.", vbOKOnly + vbAbort, "Unexpected Result")
        End If
        View.ViewTracking(CurrentDevice.strGUID)
        View.ViewDevice(CurrentDevice.strGUID)
        Me.Hide()
        Exit Sub
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Resume Next
        Else
            EndProgram()
        End If
    End Sub
    Private Sub cmdCheckOut_Click(sender As Object, e As EventArgs) Handles cmdCheckOut.Click
        CheckOut()
    End Sub
    Private Sub cmdCheckIn_Click(sender As Object, e As EventArgs) Handles cmdCheckIn.Click
        CheckIn()
    End Sub
    Private Sub txtUseLocation_TextChanged(sender As Object, e As EventArgs) Handles txtUseLocation.TextChanged
    End Sub
    Private Sub txtUseLocation_LostFocus(sender As Object, e As EventArgs) Handles txtUseLocation.LostFocus
        txtUseLocation.Text = UCase(Trim(txtUseLocation.Text))
    End Sub
    Private Sub txtUseReason_TextChanged(sender As Object, e As EventArgs) Handles txtUseReason.TextChanged
    End Sub
    Private Sub txtUseReason_LostFocus(sender As Object, e As EventArgs) Handles txtUseReason.LostFocus
        txtUseReason.Text = UCase(Trim(txtUseReason.Text))
    End Sub
    Private Sub txtCheckInNotes_TextChanged(sender As Object, e As EventArgs) Handles txtCheckInNotes.TextChanged
    End Sub
    Private Sub txtCheckInNotes_LostFocus(sender As Object, e As EventArgs) Handles txtCheckInNotes.LostFocus
        txtCheckInNotes.Text = UCase(Trim(txtCheckInNotes.Text))
    End Sub
End Class