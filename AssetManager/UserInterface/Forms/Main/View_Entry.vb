Option Explicit On
Imports MySql.Data.MySqlClient
Public Class View_Entry
    Private MyEntryGUID As String
    Private DataParser As New DBControlParser(Me)
    Public ReadOnly Property EntryGUID As String
        Get
            Return MyEntryGUID
        End Get
    End Property
    Sub New(ParentForm As Form, EntryUID As String)
        InitializeComponent()
        InitDBControls()
        MyEntryGUID = EntryUID
        Tag = ParentForm
        Icon = ParentForm.Icon
        ViewEntry(EntryUID)
    End Sub
    Private Sub Waiting()
        SetWaitCursor(True)
    End Sub
    Private Sub DoneWaiting()
        SetWaitCursor(False)
    End Sub
    Private Sub InitDBControls()
        txtEntryTime.Tag = New DBControlInfo(historical_dev.ActionDateTime, ParseType.DisplayOnly, False)
        txtActionUser.Tag = New DBControlInfo(historical_dev.ActionUser, ParseType.DisplayOnly, False)
        txtChangeType.Tag = New DBControlInfo(historical_dev.ChangeType, DeviceIndex.ChangeType, ParseType.DisplayOnly, False)
        txtDescription.Tag = New DBControlInfo(historical_dev.Description, ParseType.DisplayOnly, False)
        txtGUID.Tag = New DBControlInfo(historical_dev.DeviceUID, ParseType.DisplayOnly, False)
        txtCurrentUser.Tag = New DBControlInfo(historical_dev.CurrentUser, ParseType.DisplayOnly, False)
        txtLocation.Tag = New DBControlInfo(historical_dev.Location, DeviceIndex.Locations, ParseType.DisplayOnly, False)
        txtPONumber.Tag = New DBControlInfo(historical_dev.PO, ParseType.DisplayOnly, False)
        txtAssetTag.Tag = New DBControlInfo(historical_dev.AssetTag, ParseType.DisplayOnly, False)
        txtPurchaseDate.Tag = New DBControlInfo(historical_dev.PurchaseDate, ParseType.DisplayOnly, False)
        txtOSVersion.Tag = New DBControlInfo(historical_dev.OSVersion, DeviceIndex.OSType, ParseType.DisplayOnly, False)
        txtSerial.Tag = New DBControlInfo(historical_dev.Serial, ParseType.DisplayOnly, False)
        txtReplaceYear.Tag = New DBControlInfo(historical_dev.ReplacementYear, ParseType.DisplayOnly, False)
        txtEQType.Tag = New DBControlInfo(historical_dev.EQType, DeviceIndex.EquipType, ParseType.DisplayOnly, False)
        txtNotes.Tag = New DBControlInfo(historical_dev.Notes, ParseType.DisplayOnly, False)
        txtStatus.Tag = New DBControlInfo(historical_dev.Status, DeviceIndex.StatusType, ParseType.DisplayOnly, False)
        txtEntryGUID.Tag = New DBControlInfo(historical_dev.History_Entry_UID, ParseType.DisplayOnly, False)
        chkTrackable.Tag = New DBControlInfo(historical_dev.Trackable, ParseType.DisplayOnly, False)
        txtPhoneNumber.Tag = New DBControlInfo(historical_dev.PhoneNumber, ParseType.DisplayOnly, False)
    End Sub
    Private Sub FillControls(Data As DataTable)
        DataParser.FillDBFields(Data)
        Me.Text = Me.Text + " - " & NoNull(Data.Rows(0).Item(historical_dev.ActionDateTime))
    End Sub
    Private Sub ViewEntry(ByVal EntryUID As String)
        Waiting()
        Try
            Dim strQry = "Select * FROM " & historical_dev.TableName & " WHERE  " & historical_dev.History_Entry_UID & " = '" & EntryUID & "'"
            Using SQLComms As New clsMySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strQry)
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