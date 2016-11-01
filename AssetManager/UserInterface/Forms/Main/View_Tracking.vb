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
            Dim results As New DataTable
            Dim strQry = "Select * FROM " & trackable.TableName & " WHERE  " & trackable.UID & " = '" & EntryUID & "'"
            results = SQLComms.Return_SQLTable(strQry)
            For Each r As DataRow In results.Rows
                txtTimeStamp.Text = NoNull(r.Item(trackable.DateStamp))
                txtCheckType.Text = NoNull(r.Item(trackable.CheckType))
                If txtCheckType.Text = "IN" Then
                    txtCheckType.BackColor = colCheckIn
                ElseIf txtCheckType.Text = "OUT" Then
                    txtCheckType.BackColor = colCheckOut
                End If
                txtDescription.Text = Device.strDescription
                txtGUID.Text = NoNull(r.Item(trackable.DeviceUID))
                txtCheckOutUser.Text = NoNull(r.Item(trackable.CheckOut_User))
                txtCheckInUser.Text = NoNull(r.Item(trackable.CheckIn_User))
                txtLocation.Text = NoNull(r.Item(trackable.UseLocation))
                txtAssetTag.Text = Device.strAssetTag
                txtCheckOutTime.Text = NoNull(r.Item(trackable.CheckOut_Time))
                txtDueBack.Text = NoNull(r.Item(trackable.DueBackDate))
                txtSerial.Text = Device.strSerial
                txtCheckInTime.Text = NoNull(r.Item(trackable.CheckIn_Time))
                txtNotes.Text = NoNull(r.Item(trackable.Notes))
                txtEntryGUID.Text = NoNull(r.Item(trackable.UID))
                Me.Text = Me.Text + " - " & NoNull(r.Item(trackable.DateStamp))
            Next
            results.Dispose()
            DoneWaiting()
            Exit Sub
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
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