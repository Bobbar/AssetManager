Option Explicit On
Imports MySql.Data.MySqlClient
Public Class View_Tracking
    Private colTextBoxBG As Color = ColorTranslator.FromHtml("#D6D6D6")
    Private CurrentViewTrackDevice As Device_Info
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag.TextChanged
    End Sub
    Private Sub Waiting()
        Me.Cursor = Cursors.WaitCursor
    End Sub
    Private Sub DoneWaiting()
        Me.Cursor = Cursors.Default
    End Sub
    Public Sub ViewTrackingEntry(ByVal EntryUID As String, ByRef Device As Device_Info)
        Try
            If Not ConnectionReady() Then
                ConnectionNotReady()
                Exit Sub
            End If
            Waiting()
            Dim reader As MySqlDataReader
            Dim strQry = "Select * FROM dev_trackable WHERE  track_uid = '" & EntryUID & "'"
            reader = Return_SQLReader(strQry)
            With reader
                Do While .Read()
                    txtTimeStamp.Text = !track_datestamp
                    txtCheckType.Text = !track_check_type
                    If txtCheckType.Text = "IN" Then
                        txtCheckType.BackColor = colCheckIn
                    ElseIf txtCheckType.Text = "OUT" Then
                        txtCheckType.BackColor = colCheckOut
                    End If
                    txtDescription.Text = Device.strDescription
                    txtGUID.Text = NoNull(!track_device_uid)
                    txtCheckOutUser.Text = NoNull(!track_checkout_user)
                    txtCheckInUser.Text = NoNull(!track_checkin_user)
                    txtLocation.Text = NoNull(!track_use_location)
                    txtAssetTag.Text = Device.strAssetTag
                    txtCheckOutTime.Text = NoNull(!track_checkout_time)
                    txtDueBack.Text = NoNull(!track_dueback_date)
                    txtSerial.Text = Device.strSerial
                    txtCheckInTime.Text = NoNull(!track_checkin_time)
                    txtNotes.Text = NoNull(!track_notes)
                    txtEntryGUID.Text = NoNull(!track_uid)
                    Me.Text = Me.Text + " - " & NoNull(!track_datestamp)
                Loop
            End With
            reader.Close()
            DoneWaiting()
            Exit Sub
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                Exit Sub
            End If
        End Try
    End Sub
    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
    End Sub
    Private Sub cmdClose_Click(sender As Object, e As EventArgs) Handles cmdClose.Click
        Me.Dispose()
        Me.Hide()
    End Sub
    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter
    End Sub
    Private Sub View_Entry_Layout(sender As Object, e As LayoutEventArgs) Handles Me.Layout
    End Sub
    Private Sub txtChangeType_TextChanged(sender As Object, e As EventArgs) Handles txtCheckType.TextChanged
    End Sub
    Private Sub Label1_Click(sender As Object, e As EventArgs) Handles Label1.Click
    End Sub
    Private Sub View_Tracking_Load(sender As Object, e As EventArgs) Handles Me.Load
        Dim c As Control
        For Each c In GroupBox1.Controls
            If TypeOf c Is TextBox Then
                If c.Name <> "txtCheckType" Then c.BackColor = colTextBoxBG
            End If
        Next
    End Sub
End Class