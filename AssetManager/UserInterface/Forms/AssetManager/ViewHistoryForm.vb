Option Explicit On

Public Class ViewHistoryForm
    Private DataParser As New DBControlParser(Me)

    Sub New(parentForm As Form, entryUID As String)
        InitializeComponent()
        InitDBControls()
        Tag = parentForm
        Icon = parentForm.Icon
        FormUID = entryUID
        ViewEntry(entryUID)
    End Sub

    Private Sub Waiting()
        SetWaitCursor(True)
    End Sub

    Private Sub DoneWaiting()
        SetWaitCursor(False)
    End Sub

    Private Sub InitDBControls()
        txtEntryTime.Tag = New DBControlInfo(HistoricalDevicesCols.ActionDateTime, ParseType.DisplayOnly, False)
        txtActionUser.Tag = New DBControlInfo(HistoricalDevicesCols.ActionUser, ParseType.DisplayOnly, False)
        txtChangeType.Tag = New DBControlInfo(HistoricalDevicesCols.ChangeType, DeviceIndex.ChangeType, ParseType.DisplayOnly, False)
        txtDescription.Tag = New DBControlInfo(HistoricalDevicesCols.Description, ParseType.DisplayOnly, False)
        txtGUID.Tag = New DBControlInfo(HistoricalDevicesCols.DeviceUID, ParseType.DisplayOnly, False)
        txtCurrentUser.Tag = New DBControlInfo(HistoricalDevicesCols.CurrentUser, ParseType.DisplayOnly, False)
        txtLocation.Tag = New DBControlInfo(HistoricalDevicesCols.Location, DeviceIndex.Locations, ParseType.DisplayOnly, False)
        txtPONumber.Tag = New DBControlInfo(HistoricalDevicesCols.PO, ParseType.DisplayOnly, False)
        txtAssetTag.Tag = New DBControlInfo(HistoricalDevicesCols.AssetTag, ParseType.DisplayOnly, False)
        txtPurchaseDate.Tag = New DBControlInfo(HistoricalDevicesCols.PurchaseDate, ParseType.DisplayOnly, False)
        txtOSVersion.Tag = New DBControlInfo(HistoricalDevicesCols.OSVersion, DeviceIndex.OSType, ParseType.DisplayOnly, False)
        txtSerial.Tag = New DBControlInfo(HistoricalDevicesCols.Serial, ParseType.DisplayOnly, False)
        txtReplaceYear.Tag = New DBControlInfo(HistoricalDevicesCols.ReplacementYear, ParseType.DisplayOnly, False)
        txtEQType.Tag = New DBControlInfo(HistoricalDevicesCols.EQType, DeviceIndex.EquipType, ParseType.DisplayOnly, False)
        txtNotes.Tag = New DBControlInfo(HistoricalDevicesCols.Notes, ParseType.DisplayOnly, False)
        txtStatus.Tag = New DBControlInfo(HistoricalDevicesCols.Status, DeviceIndex.StatusType, ParseType.DisplayOnly, False)
        txtEntryGUID.Tag = New DBControlInfo(HistoricalDevicesCols.HistoryEntryUID, ParseType.DisplayOnly, False)
        chkTrackable.Tag = New DBControlInfo(HistoricalDevicesCols.Trackable, ParseType.DisplayOnly, False)
        txtPhoneNumber.Tag = New DBControlInfo(HistoricalDevicesCols.PhoneNumber, ParseType.DisplayOnly, False)
        txtHostname.Tag = New DBControlInfo(HistoricalDevicesCols.HostName, ParseType.DisplayOnly, False)
    End Sub

    Private Sub FillControls(Data As DataTable)
        DataParser.FillDBFields(Data)
        Me.Text = Me.Text + " - " & NoNull(Data.Rows(0).Item(HistoricalDevicesCols.ActionDateTime))
    End Sub

    Private Sub ViewEntry(ByVal EntryUID As String)
        Waiting()
        Try
            Dim strQry = "Select * FROM " & HistoricalDevicesCols.TableName & " WHERE  " & HistoricalDevicesCols.HistoryEntryUID & " = '" & EntryUID & "'"
            Using results As DataTable = DBFunc.DataTableFromQueryString(strQry)
                FillControls(results)
                Show()
                Activate()
                DoneWaiting()
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub cmdClose_Click(sender As Object, e As EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub

End Class