Option Explicit On

Public Class ViewTrackingForm
    Private colTextBoxBG As Color = ColorTranslator.FromHtml("#D6D6D6")
    Private CurrentViewTrackDevice As New DeviceStruct
    Private Const strCheckOut As String = "OUT"
    Private Const strCheckIn As String = "IN"

    Sub New(parentForm As Form, entryGUID As String, device As DeviceStruct)
        InitializeComponent()
        Tag = parentForm
        Icon = parentForm.Icon
        ViewTrackingEntry(entryGUID, device)
        Show()
    End Sub

    Private Sub Waiting()
        SetWaitCursor(True)
    End Sub

    Private Sub DoneWaiting()
        SetWaitCursor(False)
    End Sub

    Private Sub ViewTrackingEntry(entryUID As String, device As DeviceStruct)
        Try
            Waiting()
            Dim strQry = "Select * FROM " & TrackablesCols.TableName & " WHERE  " & TrackablesCols.UID & " = '" & entryUID & "'"
            Using results As DataTable = DBFunc.DataTableFromQueryString(strQry)
                For Each r As DataRow In results.Rows
                    txtTimeStamp.Text = NoNull(r.Item(TrackablesCols.DateStamp))
                    txtCheckType.Text = NoNull(r.Item(TrackablesCols.CheckType))
                    If txtCheckType.Text = "IN" Then
                        txtCheckType.BackColor = colCheckIn
                    ElseIf txtCheckType.Text = "OUT" Then
                        txtCheckType.BackColor = colCheckOut
                    End If
                    txtDescription.Text = device.Description
                    txtGUID.Text = NoNull(r.Item(TrackablesCols.DeviceUID))
                    txtCheckOutUser.Text = NoNull(r.Item(TrackablesCols.CheckoutUser))
                    txtCheckInUser.Text = NoNull(r.Item(TrackablesCols.CheckinUser))
                    txtLocation.Text = NoNull(r.Item(TrackablesCols.UseLocation))
                    txtAssetTag.Text = device.AssetTag
                    txtCheckOutTime.Text = NoNull(r.Item(TrackablesCols.CheckoutTime))
                    txtDueBack.Text = NoNull(r.Item(TrackablesCols.DueBackDate))
                    txtSerial.Text = device.Serial
                    txtCheckInTime.Text = NoNull(r.Item(TrackablesCols.CheckinTime))
                    txtNotes.Text = NoNull(r.Item(TrackablesCols.Notes))
                    txtEntryGUID.Text = NoNull(r.Item(TrackablesCols.UID))
                    Me.Text = Me.Text + " - " & NoNull(r.Item(TrackablesCols.DateStamp))
                Next
            End Using
            DoneWaiting()
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
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