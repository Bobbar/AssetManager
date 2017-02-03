Option Explicit On
Imports MySql.Data.MySqlClient
Public Class View_Tracking
    Private colTextBoxBG As Color = ColorTranslator.FromHtml("#D6D6D6")
    Private CurrentViewTrackDevice As Device_Info
    Sub New(ParentForm As Form, EntryGUID As String, Device As Device_Info)
        InitializeComponent()
        Tag = ParentForm
        Icon = ParentForm.Icon
        ViewTrackingEntry(EntryGUID, Device)
        Show()
    End Sub
    Private Sub Waiting()
        SetCursor(Cursors.WaitCursor)
    End Sub
    Private Sub DoneWaiting()
        SetCursor(Cursors.Default)
    End Sub
    Private Sub ViewTrackingEntry(ByVal EntryUID As String, ByRef Device As Device_Info)
        Try
            Waiting()
            Dim strQry = "Select * FROM " & trackable.TableName & " WHERE  " & trackable.UID & " = '" & EntryUID & "'"
            Using SQLComms As New clsMySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strQry)
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
            End Using
            DoneWaiting()
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                Exit Sub
            End If
        End Try
    End Sub
    Private Sub cmdClose_Click(sender As Object, e As EventArgs) Handles cmdClose.Click
        Me.Dispose()
        Me.Hide()
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