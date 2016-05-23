Imports MySql.Data.MySqlClient
Public Class View_Entry
    Private colTextBoxBG As Color = ColorTranslator.FromHtml("#D6D6D6")
    Private Sub TextBox1_TextChanged(sender As Object, e As EventArgs) Handles txtAssetTag.TextChanged
    End Sub
    Private Sub Waiting()
        Me.Cursor = Cursors.WaitCursor
    End Sub
    Private Sub DoneWaiting()
        Me.Cursor = Cursors.Default
    End Sub
    Public Sub ViewEntry(ByVal EntryUID As String)
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Sub
        End If
        Waiting()
        'Dim ConnID As String = Guid.NewGuid.ToString
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        Dim strQry = "Select * FROM historical WHERE  hist_UID = '" & EntryUID & "'"
        Dim cmd As New MySqlCommand(strQry, GlobalConn)
        reader = cmd.ExecuteReader
        With reader
            Do While .Read()
                txtEntryTime.Text = !hist_action_datetime
                txtActionUser.Text = !hist_action_user
                txtChangeType.Text = GetHumanValue(ComboType.ChangeType,!hist_change_type)
                txtDescription.Text = !hist_description
                txtGUID.Text = !hist_dev_UID
                txtCurrentUser.Text = !hist_cur_user
                txtLocation.Text = GetHumanValue(ComboType.Location,!hist_location)
                txtPONumber.Text = !hist_po
                txtAssetTag.Text = !hist_asset_tag
                txtPurchaseDate.Text = !hist_purchase_date
                txtOSVersion.Text = GetHumanValue(ComboType.OSType,!hist_osversion)
                txtSerial.Text = !hist_serial
                txtReplaceYear.Text = !hist_replacement_year
                txtEQType.Text = GetHumanValue(ComboType.EquipType,!hist_eq_type)
                txtNotes.Text = !hist_notes
                txtStatus.Text = GetHumanValue(ComboType.StatusType,!hist_status)
                txtEntryGUID.Text = !hist_uid
                chkTrackable.Checked = CBool(!hist_trackable)
                Me.Text = Me.Text + " - " &!hist_action_datetime
            Loop
        End With
        reader.Close()
        'CloseConnection(ConnID)
        DoneWaiting()
    End Sub
    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
    End Sub
    Private Sub cmdClose_Click(sender As Object, e As EventArgs) Handles cmdClose.Click
        Me.Hide()
    End Sub
    Private Sub GroupBox1_Enter(sender As Object, e As EventArgs) Handles GroupBox1.Enter
    End Sub
    Private Sub View_Entry_Layout(sender As Object, e As LayoutEventArgs) Handles Me.Layout
        Dim c As Control
        For Each c In GroupBox1.Controls
            If TypeOf c Is TextBox Then c.BackColor = colTextBoxBG
        Next
    End Sub
End Class