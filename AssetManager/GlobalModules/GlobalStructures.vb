Public Structure ComboboxDataStruct
    Public Property strLong As String
    Public Property strShort As String
    Public Property strID As String
End Structure

Public Structure DeviceStruct
    Public AssetTag As String
    Public Description As String
    Public EquipmentType As String
    Public Serial As String
    Public Location As String
    Public CurrentUser As String
    Public CurrentUserEmpNum As String
    Public FiscalYear As String
    Public PurchaseDate As Date
    Public ReplaceYear As String
    Public OSVersion As String
    Public PhoneNumber As String
    Public GUID As String
    Public PO As String
    Public Status As String
    Public Note As String
    Public IsTrackable As Boolean
    Public SibiLink As String
    Public CheckSum As String
    Public Hostname As String
    Public Tracking As DeviceTrackingStruct
    Public Historical As DeviceHistoricalStruct
End Structure

Public Structure RequestStruct
    Public GUID As String
    Public RequestUser As String
    Public Description As String
    Public DateStamp As Date
    Public NeedByDate As Date
    Public Status As String
    Public Type As String
    Public PO As String
    Public RequisitionNumber As String
    Public ReplaceAsset As String
    Public ReplaceSerial As String
    Public RequestNumber As String
    Public RTNumber As String
    Public RequestItems As DataTable
End Structure

Public Structure DeviceHistoricalStruct
    Public ChangeType As String
    Public GUID As String
    Public Note As String
    Public ActionUser As String
    Public ActionDateTime As Date
End Structure

Public Structure DeviceTrackingStruct
    Public CheckoutTime As Date
    Public DueBackTime As Date
    Public CheckinTime As Date
    Public CheckoutUser As String
    Public CheckinUser As String
    Public UseLocation As String
    Public UseReason As String
    Public IsCheckedOut As Boolean
End Structure
'TODO: Combine these two redundant structs into one.
Public Structure CheckStruct
    Public CheckoutTime As String
    Public DueBackDate As String
    Public UseLocation As String
    Public UseReason As String
    Public CheckinNotes As String
    Public DeviceGUID As String
    Public CheckoutUser As String
    Public CheckinUser As String
    Public CheckinTime As String
End Structure

Public Structure AccessGroupStruct
    Public AccessModule As String
    Public Level As Integer
    Public Description As String
    Public AvailableOffline As Boolean
End Structure

Public Structure DeviceUpdateInfoStruct
    Public Note As String
    Public ChangeType As String
End Structure

Public Structure LocalUserInfoStruct
    Public Username As String
    Public Fullname As String

    'Public bolIsAdmin As Boolean
    Public AccessLevel As Integer

    Public GUID As String
End Structure

Public Structure MunisEmployeeStruct
    Public Number As String
    Public Name As String
    Public GUID As String
End Structure



Public Structure DataGridColumnStruct
    Public ColumnName As String
    Public ColumnCaption As String
    Public ColumnType As Type
    Public ColumnReadOnly As Boolean
    Public ColumnVisible As Boolean
    Public ComboIndex As ComboboxDataStruct()

    Sub New(Name As String, Caption As String, Type As Type)
        ColumnName = Name
        ColumnCaption = Caption
        ColumnType = Type
        ColumnReadOnly = False
        ColumnVisible = True
        ComboIndex = Nothing
    End Sub

    Sub New(Name As String, Caption As String, Type As Type, ComboIdx() As ComboboxDataStruct)
        ColumnName = Name
        ColumnCaption = Caption
        ColumnType = Type
        ColumnReadOnly = False
        ColumnVisible = True
        ComboIndex = ComboIdx
    End Sub

    Sub New(Name As String, Caption As String, Type As Type, IsReadOnly As Boolean, Visible As Boolean)
        ColumnName = Name
        ColumnCaption = Caption
        ColumnType = Type
        ColumnReadOnly = IsReadOnly
        ColumnVisible = Visible
        ComboIndex = Nothing
    End Sub

End Structure

Public Structure StatusColumnColorStruct
    Public StatusID As String
    Public StatusColor As Color

    Sub New(ID As String, Color As Color)
        StatusID = ID
        StatusColor = Color
    End Sub

End Structure