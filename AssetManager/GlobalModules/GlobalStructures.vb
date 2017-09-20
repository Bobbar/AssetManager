Imports System.Linq
Imports System.Collections.Generic
Imports System
Public Structure ComboboxDataStruct
    Public Property HumanReadable As String
    Public Property Code As String
    Public Property ID As String
End Structure
<AttributeUsage(AttributeTargets.[Property])>
Public Class DataNamesAttribute
    Inherits Attribute
    'Protected Property _valueNames() As List(Of String)
    '    Get
    '        Return m__valueNames
    '    End Get
    '    Set
    '        m__valueNames = Value
    '    End Set
    'End Property
    ' Private m__valueNames As List(Of String)
    Private _valueName As String
    Public Property ValueName As String
        Get
            Return _valueName
        End Get
        Set
            _valueName = Value
        End Set
    End Property

    Public Sub New()
        _valueName = String.Empty
    End Sub

    Public Sub New(valueName As String)
        _valueName = valueName
    End Sub
End Class

Public Class Test
    <DataNames("dev_asset_tag")>
    Public Property AssetTag As String

    <DataNames("dev_description")>
    Public Property Description As String

End Class

Public Class TestingToo

    Public props As List(Of Reflection.PropertyInfo) = (GetType(DeviceStruct).GetProperties().Where(Function(x) x.GetCustomAttributes(GetType(DataNamesAttribute), True).Any()).ToList())




End Class

Public Class DeviceStruct
    <DataNames("dev_asset_tag")>
    Public Property AssetTag As String

    <DataNames("dev_description")>
    Public Property Description As String

    <DataNames("dev_eq_type")>
    Public Property EquipmentType As String

    <DataNames("dev_serial")>
    Public Property Serial As String

    <DataNames("dev_location")>
    Public Property Location As String

    <DataNames("dev_cur_user")>
    Public Property CurrentUser As String

    <DataNames("dev_cur_user_emp_num")>
    Public Property CurrentUserEmpNum As String

    Public Property FiscalYear As String

    <DataNames("dev_purchase_date")>
    Public Property PurchaseDate As Date

    <DataNames("dev_replacement_year")>
    Public Property ReplaceYear As String

    <DataNames("dev_osversion")>
    Public Property OSVersion As String

    <DataNames("dev_phone_number")>
    Public Property PhoneNumber As String

    <DataNames("dev_UID")>
    Public Property GUID As String

    <DataNames("dev_po")>
    Public Property PO As String

    <DataNames("dev_status")>
    Public Property Status As String

    Public Property Note As String

    <DataNames("dev_trackable")>
    Public Property IsTrackable As Boolean

    <DataNames("dev_sibi_link")>
    Public Property SibiLink As String

    <DataNames("dev_hostname")>
    Public Property HostName As String

    Public Tracking As DeviceTrackingStruct
    Public Historical As DeviceHistoricalStruct
End Class

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

Public Class DeviceHistoricalStruct
    <DataNames("hist_change_type")>
    Public Property ChangeType As String
    <DataNames("hist_uid")>
    Public Property GUID As String
    <DataNames("hist_notes")>
    Public Property Note As String
    <DataNames("hist_action_user")>
    Public Property ActionUser As String
    <DataNames("dev_lastmod_date")>
    Public Property ActionDateTime As Date
End Class

Public Structure DeviceTrackingStruct
    Public CheckoutTime As Date
    Public DueBackTime As Date
    Public CheckinTime As Date
    Public CheckoutUser As String
    Public CheckinUser As String
    Public UseLocation As String
    Public UseReason As String
    Public IsCheckedOut As Boolean
    Public CheckinNotes As String
    Public DeviceGUID As String
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
    Public UserName As String
    Public Fullname As String
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

    Sub New(name As String, caption As String, type As Type)
        ColumnName = name
        ColumnCaption = caption
        ColumnType = type
        ColumnReadOnly = False
        ColumnVisible = True
        ComboIndex = Nothing
    End Sub

    Sub New(name As String, caption As String, type As Type, comboIndex() As ComboboxDataStruct)
        ColumnName = name
        ColumnCaption = caption
        ColumnType = type
        ColumnReadOnly = False
        ColumnVisible = True
        Me.ComboIndex = comboIndex
    End Sub

    Sub New(name As String, caption As String, type As Type, isReadOnly As Boolean, visible As Boolean)
        ColumnName = name
        ColumnCaption = caption
        ColumnType = type
        ColumnReadOnly = isReadOnly
        ColumnVisible = visible
        ComboIndex = Nothing
    End Sub

End Structure

Public Structure StatusColumnColorStruct
    Public StatusID As String
    Public StatusColor As Color

    Sub New(id As String, color As Color)
        StatusID = id
        StatusColor = color
    End Sub

End Structure