Imports MySql.Data.MySqlClient
Public Class View_Tracking
    Private colTextBoxBG As Color = ColorTranslator.FromHtml("#D6D6D6")
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag.TextChanged
    End Sub
    Private Sub Waiting()
        Me.Cursor = Cursors.WaitCursor
    End Sub
    Private Sub DoneWaiting()
        Me.Cursor = Cursors.Default
    End Sub
    Public Sub ViewTrackingEntry(ByVal EntryUID As String)
        On Error GoTo errs
        Waiting()
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim strQry = "Select * FROM trackable WHERE  track_uid = '" & EntryUID & "'"
        Dim cmd As New MySqlCommand(strQry, GetConnection(ConnID).DBConnection)
        reader = cmd.ExecuteReader
        With reader
            Do While .Read()
                txtTimeStamp.Text = !track_datestamp
                'txtCheckUser.Text = !track_
                txtCheckType.Text = !track_check_type
                If txtCheckType.Text = "IN" Then
                    txtCheckType.BackColor = colCheckIn
                ElseIf txtCheckType.Text = "OUT" Then
                    txtCheckType.BackColor = colCheckOut
                End If
                txtDescription.Text = CurrentDevice.strDescription
                txtGUID.Text = !track_device_uid
                txtCheckOutUser.Text = !track_checkout_user
                txtCheckInUser.Text = !track_checkin_user
                txtLocation.Text = !track_use_location
                'txtPONumber.Text = !hist_po
                txtAssetTag.Text = CurrentDevice.strAssetTag
                txtCheckOutTime.Text = !track_checkout_time
                txtDueBack.Text = !track_dueback_date
                txtSerial.Text = CurrentDevice.strSerial
                txtCheckInTime.Text = !track_checkin_time
                'txtEQType.Text = GetHumanValue(ComboType.EquipType,!hist_eq_type)
                txtNotes.Text = !track_notes
                'txtStatus.Text = GetHumanValue(ComboType.StatusType,!hist_status)
                txtEntryGUID.Text = !track_uid
                'chkTrackable.Checked = CBool(!hist_trackable)
                Me.Text = Me.Text + " - " &!track_datestamp
            Loop
        End With
        CloseConnection(ConnID)
        DoneWaiting()
        Exit Sub
errs:
        If Err.Number = 13 Then Resume Next
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