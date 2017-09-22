Public Class DeviceObject
    Inherits DataMapping

    Sub New()

    End Sub

    Sub New(data As Object)
        Me.MapClassProperties(Me, data)
    End Sub

    <DataColumnName(DevicesCols.AssetTag)>
    Public Property AssetTag As String

    <DataColumnName(DevicesCols.Description)>
    Public Property Description As String

    <DataColumnName(DevicesCols.EQType)>
    Public Property EquipmentType As String

    <DataColumnName(DevicesCols.Serial)>
    Public Property Serial As String

    <DataColumnName(DevicesCols.Location)>
    Public Property Location As String

    <DataColumnName(DevicesCols.CurrentUser)>
    Public Property CurrentUser As String

    <DataColumnName(DevicesCols.MunisEmpNum)>
    Public Property CurrentUserEmpNum As String

    <DataColumnName(DevicesCols.PurchaseDate)>
    Public Property PurchaseDate As Date

    <DataColumnName(DevicesCols.ReplacementYear)>
    Public Property ReplaceYear As String

    <DataColumnName(DevicesCols.OSVersion)>
    Public Property OSVersion As String

    <DataColumnName(DevicesCols.PhoneNumber)>
    Public Property PhoneNumber As String

    <DataColumnName(DevicesCols.DeviceUID)>
    Public Property GUID As String

    <DataColumnName(DevicesCols.PO)>
    Public Property PO As String

    <DataColumnName(DevicesCols.Status)>
    Public Property Status As String

    <DataColumnName(DevicesCols.Trackable)>
    Public Property IsTrackable As Boolean

    <DataColumnName(DevicesCols.SibiLinkUID)>
    Public Property SibiLink As String

    <DataColumnName(DevicesCols.HostName)>
    Public Property HostName As String


    Public Property FiscalYear As String
    Public Property Note As String

    Public Property Tracking As New DeviceTrackingObject
    Public Property Historical As New DeviceHistoricalObject

End Class

Public Class RequestObject
    Inherits DataMapping

    Sub New()
        Me.GUID = System.Guid.NewGuid.ToString
    End Sub

    Sub New(data As Object)
        Me.MapClassProperties(Me, data)
    End Sub


    <DataColumnName(SibiRequestCols.UID)>
    Public Property GUID As String
    <DataColumnName(SibiRequestCols.RequestUser)>
    Public Property RequestUser As String
    <DataColumnName(SibiRequestCols.Description)>
    Public Property Description As String
    <DataColumnName(SibiRequestCols.DateStamp)>
    Public Property DateStamp As Date
    <DataColumnName(SibiRequestCols.NeedBy)>
    Public Property NeedByDate As Date
    <DataColumnName(SibiRequestCols.Status)>
    Public Property Status As String
    <DataColumnName(SibiRequestCols.Type)>
    Public Property RequestType As String
    <DataColumnName(SibiRequestCols.PO)>
    Public Property PO As String
    <DataColumnName(SibiRequestCols.RequisitionNumber)>
    Public Property RequisitionNumber As String
    <DataColumnName(SibiRequestCols.ReplaceAsset)>
    Public Property ReplaceAsset As String
    <DataColumnName(SibiRequestCols.ReplaceSerial)>
    Public Property ReplaceSerial As String
    <DataColumnName(SibiRequestCols.RequestNumber)>
    Public Property RequestNumber As String
    <DataColumnName(SibiRequestCols.RTNumber)>
    Public Property RTNumber As String


    Public RequestItems As DataTable
End Class

Public Class DeviceHistoricalObject
    Inherits DataMapping

    Sub New()

    End Sub

    Sub New(data As Object)
        Me.MapClassProperties(Me, data)
    End Sub

    <DataColumnName(HistoricalDevicesCols.ChangeType)>
    Public Property ChangeType As String
    <DataColumnName(HistoricalDevicesCols.HistoryEntryUID)>
    Public Property GUID As String
    <DataColumnName(HistoricalDevicesCols.Notes)>
    Public Property Note As String
    <DataColumnName(HistoricalDevicesCols.ActionUser)>
    Public Property ActionUser As String
    <DataColumnName(HistoricalDevicesCols.ActionDateTime)>
    Public Property ActionDateTime As Date
End Class

Public Class DeviceTrackingObject
    Inherits DataMapping

    Sub New()

    End Sub

    Sub New(data As DataTable)
        Me.MapClassProperties(Me, data)
    End Sub

    <DataColumnName(TrackablesCols.CheckoutTime)>
    Public Property CheckoutTime As Date
    <DataColumnName(TrackablesCols.DueBackDate)>
    Public Property DueBackTime As Date
    <DataColumnName(TrackablesCols.CheckinTime)>
    Public Property CheckinTime As Date
    <DataColumnName(TrackablesCols.CheckoutUser)>
    Public Property CheckoutUser As String
    <DataColumnName(TrackablesCols.CheckinUser)>
    Public Property CheckinUser As String
    <DataColumnName(TrackablesCols.UseLocation)>
    Public Property UseLocation As String
    <DataColumnName(TrackablesCols.Notes)>
    Public Property UseReason As String
    <DataColumnName(DevicesCols.CheckedOut)>
    Public Property IsCheckedOut As Boolean
    <DataColumnName(TrackablesCols.Notes)>
    Public Property CheckinNotes As String
    <DataColumnName(TrackablesCols.DeviceUID)>
    Public Property DeviceGUID As String
End Class
