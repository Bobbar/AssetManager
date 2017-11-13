Public Class TrackDeviceForm
    Private CurrentTrackingDevice As New DeviceObject
    Private CheckData As New DeviceTrackingObject

    Sub New(device As DeviceObject, parentForm As ExtendedForm)
        InitializeComponent()
        CurrentTrackingDevice = device
        Me.ParentForm = parentForm
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
            .CheckoutUser = LocalDomainUser
            .CheckinTime = dtCheckIn.Value '.ToString(strDBDateTimeFormat)
            .CheckinUser = LocalDomainUser
        End With
        Return True
    End Function

    Private Sub GetCurrentTracking(strGUID As String)
        Using results As DataTable = DBFactory.GetDatabase.DataTableFromQueryString("SELECT * FROM " & TrackablesCols.TableName & " WHERE " & TrackablesCols.DeviceUID & "='" & strGUID & "' ORDER BY " & TrackablesCols.DateStamp & " DESC LIMIT 1") 'ds.Tables(0)
            If results.Rows.Count > 0 Then
                CurrentTrackingDevice.MapClassProperties(CurrentTrackingDevice, results)
            End If
        End Using
    End Sub

    Private Sub LoadTracking()
        txtAssetTag.Text = CurrentTrackingDevice.AssetTag
        txtDescription.Text = CurrentTrackingDevice.Description
        txtSerial.Text = CurrentTrackingDevice.Serial
        txtDeviceType.Text = GetDisplayValueFromCode(DeviceAttribute.EquipType, CurrentTrackingDevice.EquipmentType)
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
        Using trans = DBFactory.GetDatabase.StartTransaction, conn = trans.Connection
            Try
                If Not GetCheckData() Then Exit Sub
                OtherFunctions.SetWaitCursor(True, Me)
                Dim rows As Integer
                rows += DBFactory.GetDatabase.UpdateValue(DevicesCols.TableName, DevicesCols.CheckedOut, 1, DevicesCols.DeviceUID, CurrentTrackingDevice.GUID, trans)

                Dim CheckOutParams As New List(Of DBParameter)
                CheckOutParams.Add(New DBParameter(TrackablesCols.CheckType, CheckType.Checkout))
                CheckOutParams.Add(New DBParameter(TrackablesCols.CheckoutTime, CheckData.CheckoutTime))
                CheckOutParams.Add(New DBParameter(TrackablesCols.DueBackDate, CheckData.DueBackTime))
                CheckOutParams.Add(New DBParameter(TrackablesCols.CheckoutUser, CheckData.CheckoutUser))
                CheckOutParams.Add(New DBParameter(TrackablesCols.UseLocation, CheckData.UseLocation))
                CheckOutParams.Add(New DBParameter(TrackablesCols.Notes, CheckData.UseReason))
                CheckOutParams.Add(New DBParameter(TrackablesCols.DeviceUID, CheckData.DeviceGUID))
                rows += DBFactory.GetDatabase.InsertFromParameters(TrackablesCols.TableName, CheckOutParams, trans)

                If rows = 2 Then
                    trans.Commit()
                    Message("Device Checked Out!", vbOKOnly + vbInformation, "Success", Me)
                Else
                    trans.Rollback()
                    Message("Unsuccessful! The number of affected rows was not expected.", vbOKOnly + vbExclamation, "Unexpected Result", Me)
                End If
                ParentForm.RefreshData()
            Catch ex As Exception
                trans.Rollback()
                ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Finally
                Me.Dispose()
            End Try
        End Using
    End Sub

    Private Sub CheckIn()
        Using trans = DBFactory.GetDatabase.StartTransaction, conn = trans.Connection
            Try
                If Not GetCheckData() Then Exit Sub
                OtherFunctions.SetWaitCursor(True, Me)
                Dim rows As Integer
                rows += DBFactory.GetDatabase.UpdateValue(DevicesCols.TableName, DevicesCols.CheckedOut, 0, DevicesCols.DeviceUID, CurrentTrackingDevice.GUID, trans)

                Dim CheckOutParams As New List(Of DBParameter)
                CheckOutParams.Add(New DBParameter(TrackablesCols.CheckType, CheckType.Checkin))
                CheckOutParams.Add(New DBParameter(TrackablesCols.CheckoutTime, CheckData.CheckoutTime))
                CheckOutParams.Add(New DBParameter(TrackablesCols.DueBackDate, CheckData.DueBackTime))
                CheckOutParams.Add(New DBParameter(TrackablesCols.CheckinTime, CheckData.CheckinTime))
                CheckOutParams.Add(New DBParameter(TrackablesCols.CheckoutUser, CheckData.CheckoutUser))
                CheckOutParams.Add(New DBParameter(TrackablesCols.CheckinUser, CheckData.CheckinUser))
                CheckOutParams.Add(New DBParameter(TrackablesCols.UseLocation, CheckData.UseLocation))
                CheckOutParams.Add(New DBParameter(TrackablesCols.Notes, CheckData.CheckinNotes))
                CheckOutParams.Add(New DBParameter(TrackablesCols.DeviceUID, CheckData.DeviceGUID))
                rows += DBFactory.GetDatabase.InsertFromParameters(TrackablesCols.TableName, CheckOutParams, trans)

                If rows = 2 Then
                    trans.Commit()
                    Message("Device Checked In!", vbOKOnly + vbInformation, "Success", Me)
                Else
                    trans.Rollback()
                    Message("Unsuccessful! The number of affected rows was not what was expected.", vbOKOnly + vbExclamation, "Unexpected Result", Me)
                End If
                ParentForm.RefreshData()
            Catch ex As Exception
                trans.Rollback()
                ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Finally
                Me.Dispose()
            End Try
        End Using
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