Imports System.ComponentModel

Public Class ViewHistoryForm
    Private DataParser As New DBControlParser(Me)
    Private _DeviceGUID As String

    Sub New(parentForm As ExtendedForm, entryUID As String, deviceGUID As String)
        InitializeComponent()
        InitDBControls()
        Me.ParentForm = parentForm
        FormUID = entryUID
        _DeviceGUID = deviceGUID
        ViewEntry(entryUID)

    End Sub

    'TODO: Iterate through properties and dynamically generate controls at runtime.
    Private Sub InitDBControls()
        txtEntryTime.Tag = New DBControlInfo(HistoricalDevicesCols.ActionDateTime, ParseType.DisplayOnly, False)
        txtActionUser.Tag = New DBControlInfo(HistoricalDevicesCols.ActionUser, ParseType.DisplayOnly, False)
        txtChangeType.Tag = New DBControlInfo(HistoricalDevicesCols.ChangeType, DeviceAttribute.ChangeType, ParseType.DisplayOnly, False)
        txtDescription.Tag = New DBControlInfo(HistoricalDevicesCols.Description, ParseType.DisplayOnly, False)
        txtGUID.Tag = New DBControlInfo(HistoricalDevicesCols.DeviceUID, ParseType.DisplayOnly, False)
        txtCurrentUser.Tag = New DBControlInfo(HistoricalDevicesCols.CurrentUser, ParseType.DisplayOnly, False)
        txtLocation.Tag = New DBControlInfo(HistoricalDevicesCols.Location, DeviceAttribute.Locations, ParseType.DisplayOnly, False)
        txtPONumber.Tag = New DBControlInfo(HistoricalDevicesCols.PO, ParseType.DisplayOnly, False)
        txtAssetTag.Tag = New DBControlInfo(HistoricalDevicesCols.AssetTag, ParseType.DisplayOnly, False)
        txtPurchaseDate.Tag = New DBControlInfo(HistoricalDevicesCols.PurchaseDate, ParseType.DisplayOnly, False)
        txtOSVersion.Tag = New DBControlInfo(HistoricalDevicesCols.OSVersion, DeviceAttribute.OSType, ParseType.DisplayOnly, False)
        txtSerial.Tag = New DBControlInfo(HistoricalDevicesCols.Serial, ParseType.DisplayOnly, False)
        txtReplaceYear.Tag = New DBControlInfo(HistoricalDevicesCols.ReplacementYear, ParseType.DisplayOnly, False)
        txtEQType.Tag = New DBControlInfo(HistoricalDevicesCols.EQType, DeviceAttribute.EquipType, ParseType.DisplayOnly, False)
        NotesTextBox.Tag = New DBControlInfo(HistoricalDevicesCols.Notes, ParseType.DisplayOnly, False)
        txtStatus.Tag = New DBControlInfo(HistoricalDevicesCols.Status, DeviceAttribute.StatusType, ParseType.DisplayOnly, False)
        txtEntryGUID.Tag = New DBControlInfo(HistoricalDevicesCols.HistoryEntryUID, ParseType.DisplayOnly, False)
        chkTrackable.Tag = New DBControlInfo(HistoricalDevicesCols.Trackable, ParseType.DisplayOnly, False)
        txtPhoneNumber.Tag = New DBControlInfo(HistoricalDevicesCols.PhoneNumber, ParseType.DisplayOnly, False)
        txtHostname.Tag = New DBControlInfo(HistoricalDevicesCols.HostName, ParseType.DisplayOnly, False)
        iCloudTextBox.Tag = New DBControlInfo(HistoricalDevicesCols.iCloudAccount, ParseType.DisplayOnly, False)
    End Sub

    Private Sub FillControls(Data As DataTable)
        DataParser.FillDBFields(Data)
        Me.Text = Me.Text + " - " & NoNull(Data.Rows(0).Item(HistoricalDevicesCols.ActionDateTime))
    End Sub

    Private Sub ViewEntry(ByVal EntryUID As String)
        SetWaitCursor(True, Me)
        Try
            Dim strQry = "Select * FROM " & HistoricalDevicesCols.TableName & " WHERE  " & HistoricalDevicesCols.HistoryEntryUID & " = '" & EntryUID & "'"
            Using results As DataTable = DBFactory.GetDatabase.DataTableFromQueryString(strQry)
                HighlightChangedFields(results)
                FillControls(results)
                Show()
                Activate()
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            SetWaitCursor(False, Me)
        End Try
    End Sub

    ''' <summary>
    ''' Returns a list of controls whose value differs from the previous historical state.
    ''' </summary>
    ''' <param name="currentData"></param>
    ''' <returns></returns>
    Private Function GetChangedFields(currentData As DataTable) As List(Of Control)
        Dim ChangedControls As New List(Of Control)
        Dim CurrentTimeStamp As Date = DirectCast(currentData.Rows(0).Item(HistoricalDevicesCols.ActionDateTime), Date)
        'Query for all rows with a timestamp older than the current historical entry.
        Dim Query As String = "SELECT * FROM " & HistoricalDevicesCols.TableName & " WHERE " & HistoricalDevicesCols.DeviceUID & " = '" & _DeviceGUID & "' AND " & HistoricalDevicesCols.ActionDateTime & " < '" & CurrentTimeStamp.ToString(strDBDateTimeFormat) & "' ORDER BY " & HistoricalDevicesCols.ActionDateTime & " DESC"
        Using olderData = DBFactory.GetDatabase.DataTableFromQueryString(Query)
            If olderData.Rows.Count > 0 Then
                'Declare the current and previous DataRows.
                Dim PreviousRow As DataRow = olderData.Rows(0)
                Dim CurrentRow As DataRow = currentData.Rows(0)
                Dim ChangedColumns As New List(Of String)
                'Iterate through the CurrentRow item array and compare them to the PreviousRow items.
                For i As Integer = 0 To CurrentRow.ItemArray.Length - 1
                    If PreviousRow.Item(i).ToString <> CurrentRow.Item(i).ToString Then
                        'Add column names to a list if the item values don't match.
                        ChangedColumns.Add(PreviousRow.Table.Columns(i).ColumnName)
                    End If
                Next
                'Get a list of all the controls with DBControlInfo tags.
                Dim ControlList = DataParser.GetDBControls(Me)
                'Get a list of all the controls whose data columns match the ChangedColumns.
                For Each col In ChangedColumns
                    ChangedControls.Add(ControlList.Find(Function(c) DirectCast(c.Tag, DBControlInfo).DataColumn = col))
                Next
            End If
        End Using
        Return ChangedControls
    End Function

    Private Sub HighlightChangedFields(currentData As DataTable)
        'Iterate through the list of changed fields and set the background color to highlight them.
        For Each ctl In GetChangedFields(currentData)
            ctl.BackColor = Colors.CheckIn
        Next
        NotesTextBox.BackColor = Color.White
    End Sub

    Private Sub ViewHistoryForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        Me.Dispose()
    End Sub

End Class