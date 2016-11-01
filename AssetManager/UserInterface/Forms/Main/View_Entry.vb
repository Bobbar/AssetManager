Option Explicit On
Imports MySql.Data.MySqlClient
Public Class View_Entry
    Private colTextBoxBG As Color = ColorTranslator.FromHtml("#D6D6D6")
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
        Dim results As New DataTable
        Try
            Dim strQry = "Select * FROM " & historical_dev.TableName & " WHERE  " & historical_dev.History_Entry_UID & " = '" & EntryUID & "'"
            results = SQLComms.Return_SQLTable(strQry)
            For Each r As DataRow In results.Rows
                txtEntryTime.Text = NoNull(r.Item(historical_dev.ActionDateTime))
                txtActionUser.Text = NoNull(r.Item(historical_dev.ActionUser))
                txtChangeType.Text = GetHumanValue(DeviceIndex.ChangeType, r.Item(historical_dev.ChangeType))
                txtDescription.Text = NoNull(r.Item(historical_dev.Description))
                txtGUID.Text = NoNull(r.Item(historical_dev.DeviceUID))
                txtCurrentUser.Text = NoNull(r.Item(historical_dev.CurrentUser))
                txtLocation.Text = GetHumanValue(DeviceIndex.Locations, r.Item(historical_dev.Location))
                txtPONumber.Text = NoNull(r.Item(historical_dev.PO))
                txtAssetTag.Text = NoNull(r.Item(historical_dev.AssetTag))
                txtPurchaseDate.Text = NoNull(r.Item(historical_dev.PurchaseDate))
                txtOSVersion.Text = GetHumanValue(DeviceIndex.OSType, r.Item(historical_dev.OSVersion))
                txtSerial.Text = NoNull(r.Item(historical_dev.Serial))
                txtReplaceYear.Text = NoNull(r.Item(historical_dev.ReplacementYear))
                txtEQType.Text = GetHumanValue(DeviceIndex.EquipType, r.Item(historical_dev.EQType))
                txtNotes.Text = NoNull(r.Item(historical_dev.Notes))
                txtStatus.Text = GetHumanValue(DeviceIndex.StatusType, r.Item(historical_dev.Status))
                txtEntryGUID.Text = NoNull(r.Item(historical_dev.History_Entry_UID))
                chkTrackable.Checked = CBool(r.Item(historical_dev.Trackable))
                Me.Text = Me.Text + " - " & NoNull(r.Item(historical_dev.ActionDateTime))
            Next
            DoneWaiting()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub cmdClose_Click(sender As Object, e As EventArgs) Handles cmdClose.Click
        Me.Hide()
    End Sub
    Private Sub View_Entry_Layout(sender As Object, e As LayoutEventArgs) Handles Me.Layout
        'Dim c As Control
        'For Each c In GroupBox1.Controls
        '    If TypeOf c Is TextBox Then c.BackColor = colTextBoxBG
        'Next
    End Sub
End Class