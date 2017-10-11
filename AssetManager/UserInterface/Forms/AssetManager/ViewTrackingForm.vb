Option Explicit On

Public Class ViewTrackingForm
    Sub New(parentForm As ExtendedForm, entryGUID As String, device As DeviceObject)
        InitializeComponent()
        Me.ParentForm = parentForm
        FormUID = entryGUID
        ViewTrackingEntry(entryGUID, device)
        Show()
    End Sub

    Private Sub ViewTrackingEntry(entryUID As String, device As DeviceObject)
        Try
            SetWaitCursor(True, Me)
            Dim strQry = "Select * FROM " & TrackablesCols.TableName & " WHERE  " & TrackablesCols.UID & " = '" & entryUID & "'"
            Using results As DataTable = DBFactory.GetDatabase.DataTableFromQueryString(strQry)
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
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            SetWaitCursor(False, Me)
        End Try
    End Sub

    Private Sub cmdClose_Click(sender As Object, e As EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

End Class