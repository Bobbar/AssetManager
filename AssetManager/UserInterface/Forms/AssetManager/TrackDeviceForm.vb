Option Explicit On

Imports MySql.Data.MySqlClient

Public Class TrackDeviceForm
    Private CurrentTrackingDevice As New DeviceStruct
    Private MyParent As ViewDeviceForm
    Private CheckData As DeviceTrackingStruct

    Sub New(device As DeviceStruct, parentForm As Form)
        InitializeComponent()
        CurrentTrackingDevice = device
        Tag = parentForm
        MyParent = DirectCast(parentForm, ViewDeviceForm)
        Icon = parentForm.Icon
        ClearAll()
        SetDates()
        SetGroups()
        GetCurrentTracking(CurrentTrackingDevice.GUID)
        LoadTracking()
        Show()
    End Sub

    Private Function GetCheckData() As Boolean
        If Not CurrentTrackingDevice.Tracking.IsCheckedOut Then
            Dim c As Control
            For Each c In CheckOutBox.Controls
                If TypeOf c Is TextBox Then
                    If c.Visible Then
                        If Trim(c.Text) = "" Then
                            Message("Please complete all fields.", vbOKOnly + vbExclamation, "Missing Data", Me)
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
                        Message("Please complete all fields.", vbOKOnly + vbExclamation, "Missing Data", Me)
                        Return False
                    End If
                End If
            Next
        End If
        With CheckData
            .CheckoutTime = dtCheckOut.Value '.ToString(strDBDateTimeFormat)
            .DueBackTime = dtDueBack.Value '.ToString(strDBDateTimeFormat)
            .UseLocation = UCase(Trim(txtUseLocation.Text))
            .UseReason = UCase(Trim(txtUseReason.Text))
            .CheckinNotes = UCase(Trim(txtCheckInNotes.Text))
            .DeviceGUID = CurrentTrackingDevice.GUID
            .CheckoutUser = strLocalUser
            .CheckinTime = dtCheckIn.Value '.ToString(strDBDateTimeFormat)
            .CheckinUser = strLocalUser
        End With
        Return True
    End Function

    Private Sub GetCurrentTracking(strGUID As String)
        Dim dt As DataTable
        Using SQLComms As New MySqlComms
            dt = SQLComms.ReturnMySqlTable("SELECT * FROM " & TrackablesCols.TableName & " WHERE " & TrackablesCols.DeviceUID & "='" & strGUID & "' ORDER BY " & TrackablesCols.DateStamp & " DESC LIMIT 1") 'ds.Tables(0)
            If dt.Rows.Count > 0 Then
                For Each dr As DataRow In dt.Rows
                    With dr
                        DateTime.TryParse(.Item(TrackablesCols.CheckoutTime).ToString, CurrentTrackingDevice.Tracking.CheckoutTime)
                        DateTime.TryParse(.Item(TrackablesCols.CheckinTime).ToString, CurrentTrackingDevice.Tracking.CheckinTime)
                        CurrentTrackingDevice.Tracking.UseLocation = .Item(TrackablesCols.UseLocation).ToString
                        CurrentTrackingDevice.Tracking.CheckoutUser = .Item(TrackablesCols.CheckoutUser).ToString
                        CurrentTrackingDevice.Tracking.CheckinUser = .Item(TrackablesCols.CheckinUser).ToString
                        DateTime.TryParse(.Item(TrackablesCols.DueBackDate).ToString, CurrentTrackingDevice.Tracking.DueBackTime)
                        CurrentTrackingDevice.Tracking.UseReason = .Item(TrackablesCols.Notes).ToString
                    End With
                Next
            End If
        End Using
    End Sub

    Private Sub LoadTracking()
        txtAssetTag.Text = CurrentTrackingDevice.AssetTag
        txtDescription.Text = CurrentTrackingDevice.Description
        txtSerial.Text = CurrentTrackingDevice.Serial
        txtDeviceType.Text = GetHumanValue(DeviceIndex.EquipType, CurrentTrackingDevice.EquipmentType)
        If CurrentTrackingDevice.Tracking.IsCheckedOut Then
            dtCheckOut.Value = CurrentTrackingDevice.Tracking.CheckoutTime
            dtDueBack.Value = CurrentTrackingDevice.Tracking.DueBackTime
            txtUseLocation.Text = CurrentTrackingDevice.Tracking.UseLocation
            txtUseReason.Text = CurrentTrackingDevice.Tracking.UseReason
        End If
    End Sub

    Private Sub ClearAll()
        Dim c As Control
        For Each c In Me.Controls
            If TypeOf c Is GroupBox Then
                Dim gc As Control
                For Each gc In c.Controls
                    If TypeOf gc Is TextBox Then
                        Dim txt As TextBox = DirectCast(gc, TextBox)
                        txt.Text = ""
                    End If
                Next
            End If
        Next
    End Sub

    Private Sub SetDates()
        dtCheckOut.Value = Now
        dtCheckIn.Value = Now
        dtCheckOut.Enabled = False
        dtCheckIn.Enabled = False
    End Sub

    Private Sub SetGroups()
        CheckInBox.Enabled = CurrentTrackingDevice.Tracking.IsCheckedOut
        CheckOutBox.Enabled = Not CurrentTrackingDevice.Tracking.IsCheckedOut
    End Sub

    Private Sub CheckOut()
        Try
            If Not GetCheckData() Then Exit Sub
            SetWaitCursor(True, Me)
            Dim rows As Integer
            Dim strSQLQry1 = "UPDATE " & DevicesCols.TableName & " SET " & DevicesCols.CheckedOut & "='1' WHERE " & DevicesCols.DeviceUID & "='" & CurrentTrackingDevice.GUID & "'"
            Using SQLComms As New MySqlComms, cmd As MySqlCommand = SQLComms.ReturnMySqlCommand(strSQLQry1)
                'Dim cmd As MySqlCommand = SQLComms.Return_SQLCommand(strSQLQry1)
                rows = rows + cmd.ExecuteNonQuery()
                Dim strSqlQry2 = "INSERT INTO " & TrackablesCols.TableName & "
(" & TrackablesCols.CheckType & ",
" & TrackablesCols.CheckoutTime & ",
" & TrackablesCols.DueBackDate & ",
" & TrackablesCols.CheckoutUser & ",
" & TrackablesCols.UseLocation & ",
" & TrackablesCols.Notes & ",
" & TrackablesCols.DeviceUID & ")
VALUES(@" & TrackablesCols.CheckType & ",
@" & TrackablesCols.CheckoutTime & ",
@" & TrackablesCols.DueBackDate & ",
@" & TrackablesCols.CheckoutUser & ",
@" & TrackablesCols.UseLocation & ",
@" & TrackablesCols.Notes & ",
@" & TrackablesCols.DeviceUID & ")"
                cmd.CommandText = strSqlQry2
                cmd.Parameters.AddWithValue("@" & TrackablesCols.CheckType, CheckType.Checkout)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.CheckoutTime, CheckData.CheckoutTime)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.DueBackDate, CheckData.DueBackTime)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.CheckoutUser, CheckData.CheckoutUser)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.UseLocation, CheckData.UseLocation)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.Notes, CheckData.UseReason)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.DeviceUID, CheckData.DeviceGUID)
                rows = rows + cmd.ExecuteNonQuery()
                If rows = 2 Then
                    Message("Device Checked Out!", vbOKOnly + vbInformation, "Success", Me)
                Else
                    Message("Unsuccessful! The number of affected rows was not expected.", vbOKOnly + vbAbort, "Unexpected Result", Me)
                End If
            End Using
            MyParent.LoadDevice(CurrentTrackingDevice.GUID)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            Me.Dispose()
        End Try
    End Sub

    Private Sub CheckIn()
        Try
            If Not GetCheckData() Then Exit Sub
            SetWaitCursor(True, Me)
            Dim rows As Integer
            Dim strSQLQry1 = "UPDATE " & DevicesCols.TableName & " SET " & DevicesCols.CheckedOut & "='0' WHERE " & DevicesCols.DeviceUID & "='" & CurrentTrackingDevice.GUID & "'"
            Using SQLComms As New MySqlComms, cmd As MySqlCommand = SQLComms.ReturnMySqlCommand(strSQLQry1)
                rows = rows + cmd.ExecuteNonQuery()
                Dim strSqlQry2 = "INSERT INTO " & TrackablesCols.TableName & "
(" & TrackablesCols.CheckType & ",
" & TrackablesCols.CheckoutTime & ",
" & TrackablesCols.DueBackDate & ",
" & TrackablesCols.CheckinTime & ",
" & TrackablesCols.CheckoutUser & ",
" & TrackablesCols.CheckinUser & ",
" & TrackablesCols.UseLocation & ",
" & TrackablesCols.Notes & ",
" & TrackablesCols.DeviceUID & ")
VALUES (@" & TrackablesCols.CheckType & ",
@" & TrackablesCols.CheckoutTime & ",
@" & TrackablesCols.DueBackDate & ",
@" & TrackablesCols.CheckinTime & ",
@" & TrackablesCols.CheckoutUser & ",
@" & TrackablesCols.CheckinUser & ",
@" & TrackablesCols.UseLocation & ",
@" & TrackablesCols.Notes & ",
@" & TrackablesCols.DeviceUID & ")"
                cmd.CommandText = strSqlQry2
                cmd.Parameters.AddWithValue("@" & TrackablesCols.CheckType, CheckType.Checkin)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.CheckoutTime, CheckData.CheckoutTime)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.DueBackDate, CheckData.DueBackTime)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.CheckinTime, CheckData.CheckinTime)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.CheckoutUser, CheckData.CheckoutUser)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.CheckinUser, CheckData.CheckinUser)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.UseLocation, CheckData.UseLocation)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.Notes, CheckData.CheckinNotes)
                cmd.Parameters.AddWithValue("@" & TrackablesCols.DeviceUID, CheckData.DeviceGUID)
                rows = rows + cmd.ExecuteNonQuery()
                If rows = 2 Then
                    Message("Device Checked In!", vbOKOnly + vbInformation, "Success", Me)
                Else
                    Message("Unsuccessful! The number of affected rows was not what was expected.", vbOKOnly + vbAbort, "Unexpected Result", Me)
                End If
            End Using
            MyParent.LoadDevice(CurrentTrackingDevice.GUID)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            Me.Dispose()
        End Try
    End Sub

    Private Sub cmdCheckOut_Click(sender As Object, e As EventArgs) Handles cmdCheckOut.Click
        CheckOut()
    End Sub

    Private Sub cmdCheckIn_Click(sender As Object, e As EventArgs) Handles cmdCheckIn.Click
        CheckIn()
    End Sub

    Private Sub txtUseLocation_LostFocus(sender As Object, e As EventArgs) Handles txtUseLocation.LostFocus
        txtUseLocation.Text = UCase(Trim(txtUseLocation.Text))
    End Sub

    Private Sub txtUseReason_LostFocus(sender As Object, e As EventArgs) Handles txtUseReason.LostFocus
        txtUseReason.Text = UCase(Trim(txtUseReason.Text))
    End Sub

    Private Sub txtCheckInNotes_LostFocus(sender As Object, e As EventArgs) Handles txtCheckInNotes.LostFocus
        txtCheckInNotes.Text = UCase(Trim(txtCheckInNotes.Text))
    End Sub

End Class