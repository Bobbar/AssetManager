'TODO: Break up this class into separate files.
Public Structure ComboboxDataStruct
    Public Property HumanReadable As String
    Public Property Code As String
    Public Property ID As String
End Structure
<AttributeUsage(AttributeTargets.[Property])>
Public Class DataColumnNamesAttribute
    Inherits Attribute

    Private _columnName As String
    Public Property ColumnName As String
        Get
            Return _columnName
        End Get
        Set
            _columnName = Value
        End Set
    End Property

    Public Sub New()
        _columnName = String.Empty
    End Sub

    Public Sub New(columnName As String)
        _columnName = columnName
    End Sub
End Class

''' <summary>
''' Base type for classes that will hold information from database.
''' </summary>
Public Class DataStructure

    ''' <summary>
    ''' Uses reflection to recursively populate/map class properties that are marked with a <see cref="DataColumnNamesAttribute"/>.
    ''' </summary>
    ''' <param name="obj">Object to be populated.</param>
    ''' <param name="data">Datatable with columns matching the <see cref="DataColumnNamesAttribute"/> in the objects properties.</param>
    Public Sub PopulateClassProps(obj As Object, data As DataTable)
        'Collect list of all properties in the object class.
        Dim Props As List(Of Reflection.PropertyInfo) = (obj.GetType.GetProperties().ToList)
        'Get the first, and hopefully only relevent row of the DataTable.
        Dim row = data.Rows(0)
        'Iterate through the properties.
        For Each prop In Props

            'Check if the property contains a target attribute.
            If prop.GetCustomAttributes(GetType(DataColumnNamesAttribute), True).Length > 0 Then

                'Get the column name attached to the property.
                Dim propColumn = DirectCast(prop.GetCustomAttributes(False)(0), DataColumnNamesAttribute).ColumnName

                'Make sure the DataTable contains a matching column name.
                If row.Table.Columns.Contains(propColumn) Then

                    'Check the type of the propery and set its value accordingly.
                    Select Case prop.PropertyType
                        Case GetType(String)
                            prop.SetValue(obj, row(propColumn).ToString, Nothing)

                        Case GetType(DateTime)
                            Dim pDate As DateTime
                            If DateTime.TryParse(NoNull(row(propColumn).ToString), pDate) Then
                                prop.SetValue(obj, pDate)
                            Else
                                prop.SetValue(obj, Nothing)
                            End If

                        Case GetType(Boolean)
                            prop.SetValue(obj, CBool(row(propColumn)))

                        Case Else
                            'Throw an error if type is unexpected.
                            Debug.Print(prop.PropertyType.ToString)
                            Throw New Exception("Unexpected property type.")
                    End Select
                End If

            Else 'If the property does not contain a target attribute, check to see if it is a nested class inheriting the DataStructure base class.

                If GetType(DataStructure).IsAssignableFrom(prop.PropertyType) Then
                    'Recurse with nested DataStructure properties.
                    PopulateClassProps(prop.GetValue(obj, Nothing), data)
                End If
            End If
        Next
    End Sub
End Class


Public Class DeviceStruct
    Inherits DataStructure

    Sub New()

    End Sub

    Sub New(data As DataTable)
        Me.PopulateClassProps(Me, data)
    End Sub

    <DataColumnNames(DevicesCols.AssetTag)>
    Public Property AssetTag As String

    <DataColumnNames(DevicesCols.Description)>
    Public Property Description As String

    <DataColumnNames(DevicesCols.EQType)>
    Public Property EquipmentType As String

    <DataColumnNames(DevicesCols.Serial)>
    Public Property Serial As String

    <DataColumnNames(DevicesCols.Location)>
    Public Property Location As String

    <DataColumnNames(DevicesCols.CurrentUser)>
    Public Property CurrentUser As String

    <DataColumnNames(DevicesCols.MunisEmpNum)>
    Public Property CurrentUserEmpNum As String

    <DataColumnNames(DevicesCols.PurchaseDate)>
    Public Property PurchaseDate As Date

    <DataColumnNames(DevicesCols.ReplacementYear)>
    Public Property ReplaceYear As String

    <DataColumnNames(DevicesCols.OSVersion)>
    Public Property OSVersion As String

    <DataColumnNames(DevicesCols.PhoneNumber)>
    Public Property PhoneNumber As String

    <DataColumnNames(DevicesCols.DeviceUID)>
    Public Property GUID As String

    <DataColumnNames(DevicesCols.PO)>
    Public Property PO As String

    <DataColumnNames(DevicesCols.Status)>
    Public Property Status As String

    <DataColumnNames(DevicesCols.Trackable)>
    Public Property IsTrackable As Boolean

    <DataColumnNames(DevicesCols.SibiLinkUID)>
    Public Property SibiLink As String

    <DataColumnNames(DevicesCols.HostName)>
    Public Property HostName As String


    Public Property FiscalYear As String
    Public Property Note As String

    Public Property Tracking As New DeviceTrackingStruct
    Public Property Historical As New DeviceHistoricalStruct

End Class

Public Class RequestStruct
    Inherits DataStructure

    Sub New()
        Me.GUID = System.Guid.NewGuid.ToString
    End Sub

    Sub New(data As DataTable)
        Me.PopulateClassProps(Me, data)
    End Sub


    <DataColumnNames(SibiRequestCols.UID)>
    Public Property GUID As String
    <DataColumnNames(SibiRequestCols.RequestUser)>
    Public Property RequestUser As String
    <DataColumnNames(SibiRequestCols.Description)>
    Public Property Description As String
    <DataColumnNames(SibiRequestCols.DateStamp)>
    Public Property DateStamp As Date
    <DataColumnNames(SibiRequestCols.NeedBy)>
    Public Property NeedByDate As Date
    <DataColumnNames(SibiRequestCols.Status)>
    Public Property Status As String
    <DataColumnNames(SibiRequestCols.Type)>
    Public Property RequestType As String
    <DataColumnNames(SibiRequestCols.PO)>
    Public Property PO As String
    <DataColumnNames(SibiRequestCols.RequisitionNumber)>
    Public Property RequisitionNumber As String
    <DataColumnNames(SibiRequestCols.ReplaceAsset)>
    Public Property ReplaceAsset As String
    <DataColumnNames(SibiRequestCols.ReplaceSerial)>
    Public Property ReplaceSerial As String
    <DataColumnNames(SibiRequestCols.RequestNumber)>
    Public Property RequestNumber As String
    <DataColumnNames(SibiRequestCols.RTNumber)>
    Public Property RTNumber As String


    Public RequestItems As DataTable
End Class

Public Class DeviceHistoricalStruct
    Inherits DataStructure

    Sub New()

    End Sub

    Sub New(data As DataTable)
        Me.PopulateClassProps(Me, data)
    End Sub

    <DataColumnNames(HistoricalDevicesCols.ChangeType)>
    Public Property ChangeType As String
    <DataColumnNames(HistoricalDevicesCols.HistoryEntryUID)>
    Public Property GUID As String
    <DataColumnNames(HistoricalDevicesCols.Notes)>
    Public Property Note As String
    <DataColumnNames(HistoricalDevicesCols.ActionUser)>
    Public Property ActionUser As String
    <DataColumnNames(HistoricalDevicesCols.ActionDateTime)>
    Public Property ActionDateTime As Date
End Class

Public Class DeviceTrackingStruct
    Inherits DataStructure

    Sub New()

    End Sub

    Sub New(data As DataTable)
        Me.PopulateClassProps(Me, data)
    End Sub

    <DataColumnNames(TrackablesCols.CheckoutTime)>
    Public Property CheckoutTime As Date
    <DataColumnNames(TrackablesCols.DueBackDate)>
    Public Property DueBackTime As Date
    <DataColumnNames(TrackablesCols.CheckinTime)>
    Public Property CheckinTime As Date
    <DataColumnNames(TrackablesCols.CheckoutUser)>
    Public Property CheckoutUser As String
    <DataColumnNames(TrackablesCols.CheckinUser)>
    Public Property CheckinUser As String
    <DataColumnNames(TrackablesCols.UseLocation)>
    Public Property UseLocation As String
    <DataColumnNames(TrackablesCols.Notes)>
    Public Property UseReason As String
    <DataColumnNames(DevicesCols.CheckedOut)>
    Public Property IsCheckedOut As Boolean
    <DataColumnNames(TrackablesCols.Notes)>
    Public Property CheckinNotes As String
    <DataColumnNames(TrackablesCols.DeviceUID)>
    Public Property DeviceGUID As String
End Class

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