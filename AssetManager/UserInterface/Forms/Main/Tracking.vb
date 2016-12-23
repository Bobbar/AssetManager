Option Explicit On
Imports MySql.Data.MySqlClient
Public Class Tracking
    Private CurrentTrackingDevice As Device_Info
    Private MyParent As frmView
    Private CheckData As CheckStruct
    Sub New(ByRef Device As Device_Info, ParentForm As Form)
        InitializeComponent()
        CurrentTrackingDevice = Device
        Tag = ParentForm
        MyParent = ParentForm
        Icon = ParentForm.Icon
        ClearAll()
        SetDates()
        SetGroups()
        GetCurrentTracking(CurrentTrackingDevice.strGUID)
        LoadTracking()
        Show()
    End Sub
    Private Function GetCheckData() As Boolean
        If Not CurrentTrackingDevice.Tracking.bolCheckedOut Then
            Dim c As Control
            For Each c In CheckOutBox.Controls
                If TypeOf c Is TextBox Then
                    If c.Visible Then
                        If Trim(c.Text) = "" Then
                            Dim blah = Message("Please complete all fields.", vbOKOnly + vbExclamation, "Missing Data")
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
                        Dim blah = Message("Please complete all fields.", vbOKOnly + vbExclamation, "Missing Data")
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
            .strDeviceUID = CurrentTrackingDevice.strGUID
            .strCheckOutUser = strLocalUser
            .strCheckInTime = dtCheckIn.Value.ToString(strDBDateTimeFormat)
            .strCheckInUser = strLocalUser
        End With
        Return True
    End Function

    Private Sub GetCurrentTracking(strGUID As String)
        Dim dt As DataTable
        Dim dr As DataRow
        dt = SQLComms.Return_SQLTable("SELECT * FROM " & trackable.TableName & " WHERE " & trackable.DeviceUID & "='" & strGUID & "' ORDER BY " & trackable.DateStamp & " DESC LIMIT 1") 'ds.Tables(0)
        If dt.Rows.Count > 0 Then
            For Each dr In dt.Rows
                With dr
                    CurrentTrackingDevice.Tracking.strCheckOutTime = .Item(trackable.CheckOut_Time).ToString
                    CurrentTrackingDevice.Tracking.strCheckInTime = .Item(trackable.CheckIn_Time).ToString
                    CurrentTrackingDevice.Tracking.strUseLocation = .Item(trackable.UseLocation).ToString
                    CurrentTrackingDevice.Tracking.strCheckOutUser = .Item(trackable.CheckOut_User).ToString
                    CurrentTrackingDevice.Tracking.strCheckInUser = .Item(trackable.CheckIn_User).ToString
                    CurrentTrackingDevice.Tracking.strDueBackTime = .Item(trackable.DueBackDate).ToString
                    CurrentTrackingDevice.Tracking.strUseReason = .Item(trackable.Notes).ToString
                End With
            Next
        End If
    End Sub
    Private Sub LoadTracking()
        txtAssetTag.Text = CurrentTrackingDevice.strAssetTag
        txtDescription.Text = CurrentTrackingDevice.strDescription
        txtSerial.Text = CurrentTrackingDevice.strSerial
        txtDeviceType.Text = GetHumanValue(DeviceIndex.EquipType, CurrentTrackingDevice.strEqType)
        If CurrentTrackingDevice.Tracking.bolCheckedOut Then
            dtCheckOut.Value = CurrentTrackingDevice.Tracking.strCheckOutTime
            dtDueBack.Value = CurrentTrackingDevice.Tracking.strDueBackTime
            txtUseLocation.Text = CurrentTrackingDevice.Tracking.strUseLocation
            txtUseReason.Text = CurrentTrackingDevice.Tracking.strUseReason
        End If
    End Sub
    Private Sub ClearAll()
        Dim c As Control
        For Each c In Me.Controls
            If TypeOf c Is GroupBox Then
                Dim gc As Control
                For Each gc In c.Controls
                    If TypeOf gc Is TextBox Then
                        Dim txt As TextBox = gc
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
        CheckInBox.Enabled = CurrentTrackingDevice.Tracking.bolCheckedOut
        CheckOutBox.Enabled = Not CurrentTrackingDevice.Tracking.bolCheckedOut
    End Sub
    Private Sub CheckOut()
        Try
            If Not GetCheckData() Then Exit Sub
            Waiting()
            Dim rows As Integer
            Dim strSQLQry1 = "UPDATE " & devices.TableName & " SET " & devices.CheckedOut & "='1' WHERE " & devices.DeviceUID & "='" & CurrentTrackingDevice.strGUID & "'"
            Dim cmd As MySqlCommand = SQLComms.Return_SQLCommand(strSQLQry1)
            rows = rows + cmd.ExecuteNonQuery()
            Dim strSqlQry2 = "INSERT INTO " & trackable.TableName & " 
(" & trackable.CheckType & ", 
" & trackable.CheckOut_Time & ",
" & trackable.DueBackDate & ", 
" & trackable.CheckOut_User & ",
" & trackable.UseLocation & ",
" & trackable.Notes & ",
" & trackable.DeviceUID & ") 
VALUES(@" & trackable.CheckType & ",
@" & trackable.CheckOut_Time & ",
@" & trackable.DueBackDate & ",
@" & trackable.CheckOut_User & ",
@" & trackable.UseLocation & ",
@" & trackable.Notes & ",
@" & trackable.DeviceUID & ")"
            cmd.CommandText = strSqlQry2
            cmd.Parameters.AddWithValue("@" & trackable.CheckType, strCheckOut)
            cmd.Parameters.AddWithValue("@" & trackable.CheckOut_Time, CheckData.strCheckOutTime)
            cmd.Parameters.AddWithValue("@" & trackable.DueBackDate, CheckData.strDueDate)
            cmd.Parameters.AddWithValue("@" & trackable.CheckOut_User, CheckData.strCheckOutUser)
            cmd.Parameters.AddWithValue("@" & trackable.UseLocation, CheckData.strUseLocation)
            cmd.Parameters.AddWithValue("@" & trackable.Notes, CheckData.strUseReason)
            cmd.Parameters.AddWithValue("@" & trackable.DeviceUID, CheckData.strDeviceUID)
            rows = rows + cmd.ExecuteNonQuery()
            If rows = 2 Then
                Dim blah = Message("Device Checked Out!", vbOKOnly + vbInformation, "Success")
            Else
                Dim blah = Message("Unsuccessful! The number of affected rows was not expected.", vbOKOnly + vbAbort, "Unexpected Result")
            End If
            Me.Dispose()
            MyParent.ViewDevice(CurrentTrackingDevice.strGUID)
            cmd.Dispose()
            DoneWaiting()
            Exit Sub
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                DoneWaiting()
                Exit Try
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Private Sub CheckIn()
        Try
            If Not GetCheckData() Then Exit Sub
            Waiting()
            Dim rows As Integer
            Dim strSQLQry1 = "UPDATE " & devices.TableName & " SET " & devices.CheckedOut & "='0' WHERE " & devices.DeviceUID & "='" & CurrentTrackingDevice.strGUID & "'"
            Dim cmd As MySqlCommand = SQLComms.Return_SQLCommand(strSQLQry1)
            rows = rows + cmd.ExecuteNonQuery()
            Dim strSqlQry2 = "INSERT INTO " & trackable.TableName & " 
(" & trackable.CheckType & ", 
" & trackable.CheckOut_Time & ",
" & trackable.DueBackDate & ", 
" & trackable.CheckIn_Time & ", 
" & trackable.CheckOut_User & ",
" & trackable.CheckIn_User & ",
" & trackable.UseLocation & ",
" & trackable.Notes & ",
" & trackable.DeviceUID & ") 
VALUES (@" & trackable.CheckType & ",
@" & trackable.CheckOut_Time & ",
@" & trackable.DueBackDate & ",
@" & trackable.CheckIn_Time & ",
@" & trackable.CheckOut_User & ",
@" & trackable.CheckIn_User & ",
@" & trackable.UseLocation & ", 
@" & trackable.Notes & ",
@" & trackable.DeviceUID & ")"
            cmd.CommandText = strSqlQry2
            cmd.Parameters.AddWithValue("@" & trackable.CheckType, strCheckIn)
            cmd.Parameters.AddWithValue("@" & trackable.CheckOut_Time, CheckData.strCheckOutTime)
            cmd.Parameters.AddWithValue("@" & trackable.DueBackDate, CheckData.strDueDate)
            cmd.Parameters.AddWithValue("@" & trackable.CheckIn_Time, CheckData.strCheckInTime)
            cmd.Parameters.AddWithValue("@" & trackable.CheckOut_User, CheckData.strCheckOutUser)
            cmd.Parameters.AddWithValue("@" & trackable.CheckIn_User, CheckData.strCheckInUser)
            cmd.Parameters.AddWithValue("@" & trackable.UseLocation, CheckData.strUseLocation)
            cmd.Parameters.AddWithValue("@" & trackable.Notes, CheckData.strCheckInNotes)
            cmd.Parameters.AddWithValue("@" & trackable.DeviceUID, CheckData.strDeviceUID)
            rows = rows + cmd.ExecuteNonQuery()
            If rows = 2 Then
                Dim blah = Message("Device Checked In!", vbOKOnly + vbInformation, "Success")
            Else
                Dim blah = Message("Unsuccessful! The number of affected rows was not what was expected.", vbOKOnly + vbAbort, "Unexpected Result")
            End If
            Me.Dispose()
            cmd.Dispose()
            MyParent.ViewDevice(CurrentTrackingDevice.strGUID)
            DoneWaiting()
            Exit Sub
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                DoneWaiting()
                Exit Try
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Private Sub Waiting()
        Me.Cursor = Cursors.WaitCursor
    End Sub
    Private Sub DoneWaiting()
        Me.Cursor = Cursors.Default
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