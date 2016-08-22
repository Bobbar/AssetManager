Option Explicit On
Imports MySql.Data.MySqlClient
Public Module DBFunctions
    Public ReadOnly Property strLocalUser As String = Environment.UserName
    'Public Const strServerIP As String = "192.168.1.122"
    'Public Const strServerIP As String = "10.10.80.232"
    Public Const strServerIP As String = "10.10.0.89"
    Public Const strDBDateTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Public Const strDBDateFormat As String = "yyyy-MM-dd"
    Public Const strCommMessage As String = "Communicating..."
    Public Const strLoadingGridMessage As String = "Building Grid..."
    Public Const strCheckOut As String = "OUT"
    Public Const strCheckIn As String = "IN"
    Public strLastQry As String
    Private ConnCount As Integer = 0
    Public strServerTime As String
    Public Structure ConnectionData
        Public DBConnection As MySqlConnection
        Public ConnectionID As String
    End Structure
    Public Structure Combo_Data
        Public strLong As String
        Public strShort As String
        Public strID As String
    End Structure
    Public Structure Device_Info
        Public strAssetTag As String
        Public strDescription As String
        Public strEqType As String
        Public strSerial As String
        Public strLocation As String
        Public strCurrentUser As String
        Public strFiscalYear As String
        Public dtPurchaseDate As Date 'As Object
        Public strReplaceYear As String
        Public strOSVersion As String
        Public strGUID As String
        Public strPO As String
        Public strStatus As String
        Public strNote As String
        Public bolTrackable As Boolean
        Public strSibiLink As String
        Public Tracking As Track_Info
        Public Historical As Hist_Info
    End Structure
    Public Structure Request_Info
        Public strUID As String
        Public strUser As String
        Public strDescription As String
        Public dtDateStamp As Object
        Public dtNeedBy As Object
        Public strStatus As String
        Public strType As String
        Public strPO As String
        Public strRequisitionNumber As String
        Public strReplaceAsset As String
        Public strReplaceSerial As String
        Public strRequestNumber As String
        Public strRTNumber As String
        Public RequstItems As DataTable
    End Structure
    Public Structure Hist_Info
        Public strChangeType As String
        Public strHistUID As String
        Public strNote As String
        Public strActionUser As String
        Public dtActionDateTime As Date
    End Structure
    Public Structure Track_Info
        Public strCheckOutTime As String
        Public strDueBackTime As String
        Public strCheckInTime As String
        Public strCheckOutUser As String
        Public strCheckInUser As String
        Public strUseLocation As String
        Public strUseReason As String
        Public bolCheckedOut As Boolean
    End Structure
    Public Structure Access_Info
        Public strModule As String
        Public intLevel As Integer
        Public strDesc As String
    End Structure
    Public Structure Update_Info
        Public strNote As String
        Public strChangeType As String
    End Structure
    Public AccessLevels() As Access_Info
    Public Locations() As Combo_Data
    Public ChangeType() As Combo_Data
    Public EquipType() As Combo_Data
    Public OSType() As Combo_Data
    Public StatusType() As Combo_Data
    'sibi
    Public RequestData As Request_Info
    Public Sibi_StatusType() As Combo_Data
    Public Sibi_ItemStatusType() As Combo_Data
    Public Sibi_RequestType() As Combo_Data
    Public Sibi_AttachFolder() As Combo_Data
    Public CurrentRequest As Request_Info
    Public MunisComms As New Munis_Comms
    Public Munis As New Munis_Functions
    Public MySQLDB As New MySQL_Comms
    Public Structure User_Info
        Public strUsername As String
        Public strFullname As String
        'Public bolIsAdmin As Boolean
        Public intAccessLevel As Integer
        Public strUID As String
    End Structure
    Public UserAccess As User_Info
    Public NotInheritable Class ComboType
        Public Const Location As String = "LOCATION"
        Public Const ChangeType As String = "CHANGETYPE"
        Public Const EquipType As String = "EQ_TYPE"
        Public Const OSType As String = "OS_TYPE"
        Public Const StatusType As String = "STATUS_TYPE"
        Public Const SibiStatusType As String = "STATUS"
        Public Const SibiItemStatusType As String = "ITEM_STATUS"
        Public Const SibiRequestType As String = "REQ_TYPE"
        Public Const SibiAttachFolder As String = "ATTACH_FOLDER"
    End Class
    Public NotInheritable Class CodeType
        Public Const Sibi As String = "sibi_codes"
        Public Const Device As String = "dev_codes"
    End Class
    Public NotInheritable Class Entry_Type
        Public Const Sibi As String = "sibi_"
        Public Const Device As String = "dev_"
    End Class
    Public TotConnections As Integer = 0
    Public GlobalConn As MySqlConnection = MySQLDB.NewConnection()
    Public Function OpenConnections() As Boolean
        Try
            GlobalConn.Open()
            If GlobalConn.State = ConnectionState.Open Then
                Return True
            Else
                Return False
            End If
        Catch ex As MySqlException
            Logger("ERROR:  MethodName=" & System.Reflection.MethodInfo.GetCurrentMethod().Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
            Return False
        End Try
    End Function
    Public Function CloseConnections()
        Try
            MySQLDB.CloseConnection(GlobalConn)
            'GlobalConn.Close()
            'GlobalConn.Dispose()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
        Return True
    End Function
    Public Function CollectDeviceInfo(DeviceTable As DataTable) As Device_Info
        Try
            Dim newDeviceInfo As Device_Info
            With newDeviceInfo
                .strGUID = NoNull(DeviceTable.Rows(0).Item("dev_UID"))
                .strDescription = NoNull(DeviceTable.Rows(0).Item("dev_description"))
                .strLocation = NoNull(DeviceTable.Rows(0).Item("dev_location"))
                .strCurrentUser = NoNull(DeviceTable.Rows(0).Item("dev_cur_user"))
                .strSerial = NoNull(DeviceTable.Rows(0).Item("dev_serial"))
                .strAssetTag = NoNull(DeviceTable.Rows(0).Item("dev_asset_tag"))
                .dtPurchaseDate = NoNull(DeviceTable.Rows(0).Item("dev_purchase_date"))
                .strReplaceYear = NoNull(DeviceTable.Rows(0).Item("dev_replacement_year"))
                .strPO = NoNull(DeviceTable.Rows(0).Item("dev_po"))
                .strOSVersion = NoNull(DeviceTable.Rows(0).Item("dev_osversion"))
                .strEqType = NoNull(DeviceTable.Rows(0).Item("dev_eq_type"))
                .strStatus = NoNull(DeviceTable.Rows(0).Item("dev_status"))
                .bolTrackable = CBool(DeviceTable.Rows(0).Item("dev_trackable"))
                .strSibiLink = NoNull(DeviceTable.Rows(0).Item("dev_sibi_link"))
                .Tracking.bolCheckedOut = CBool(DeviceTable.Rows(0).Item("dev_checkedout"))
            End With
            Return newDeviceInfo
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Sub CollectRequestInfo(RequestResults As DataTable, RequestItemsResults As DataTable)
        Try
            With CurrentRequest
                .strUID = NoNull(RequestResults.Rows(0).Item("sibi_UID"))
                .strUser = NoNull(RequestResults.Rows(0).Item("sibi_request_user"))
                .strDescription = NoNull(RequestResults.Rows(0).Item("sibi_description"))
                .dtDateStamp = NoNull(RequestResults.Rows(0).Item("sibi_datestamp"))
                .dtNeedBy = NoNull(RequestResults.Rows(0).Item("sibi_need_by"))
                .strStatus = NoNull(RequestResults.Rows(0).Item("sibi_status"))
                .strType = NoNull(RequestResults.Rows(0).Item("sibi_type"))
                .strPO = NoNull(RequestResults.Rows(0).Item("sibi_PO")) '
                .strRequisitionNumber = NoNull(RequestResults.Rows(0).Item("sibi_requisition_number"))
                .strReplaceAsset = NoNull(RequestResults.Rows(0).Item("sibi_replace_asset"))
                .strReplaceSerial = NoNull(RequestResults.Rows(0).Item("sibi_replace_serial"))
                .strRequestNumber = NoNull(RequestResults.Rows(0).Item("sibi_request_number"))
                .RequstItems = RequestItemsResults
            End With
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Function GetShortLocation(ByVal index As Integer) As String
        Try
            Return Locations(index).strShort
        Catch
            Return ""
        End Try
    End Function
    Public Function DeleteMaster(ByVal strGUID As String, Type As String) As Boolean
        Try
            If MySQLDB.Has_Attachments(strGUID, Type) Then
                If DeleteFTPFolder(strGUID, Type) Then Return MySQLDB.Delete_SQLMasterEntry(strGUID, Type) ' if has attachments, delete ftp directory, then delete the sql records.
            Else
                Return MySQLDB.Delete_SQLMasterEntry(strGUID, Type) 'delete sql records
            End If
        Catch ex As Exception
            Return ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Function
    Public Function GetDBValue(ByVal CodeIndex() As Combo_Data, ByVal index As Integer) As Object
        Try
            If index > -1 Then
                Return CodeIndex(index).strShort
            End If
            Return Nothing
        Catch
            Return Nothing
        End Try
    End Function
    Public Function GetHumanValue(ByVal Type As String, ByVal ShortVal As String) As String
        Dim SearchIndex() As Combo_Data
        Dim i As Integer
        SearchIndex = GetSearchIndex(Type)
        For i = 0 To UBound(SearchIndex)
            If SearchIndex(i).strShort = ShortVal Then Return SearchIndex(i).strLong
        Next
        Return Nothing
    End Function
    Public Function GetDBValueFromHuman(ByVal Type As String, ByVal LongVal As String) As String
        Dim SearchIndex() As Combo_Data = GetSearchIndex(Type)
        For Each i As Combo_Data In SearchIndex
            If i.strLong = LongVal Then Return i.strShort
        Next
        Return Nothing
    End Function
    Private Function GetSearchIndex(ByVal Type As String) As Combo_Data()
        Select Case Type
            Case ComboType.Location
                Return Locations
            Case ComboType.ChangeType
                Return ChangeType
            Case ComboType.EquipType
                Return EquipType
            Case ComboType.OSType
                Return OSType
            Case ComboType.StatusType
                Return StatusType
            Case ComboType.SibiItemStatusType
                Return Sibi_ItemStatusType
            Case ComboType.SibiRequestType
                Return Sibi_RequestType
            Case ComboType.SibiStatusType
                Return Sibi_StatusType
            Case ComboType.SibiAttachFolder
                Return Sibi_AttachFolder
            Case Else
                Return Nothing
        End Select
    End Function
    Public Function GetComboIndexFromShort(ByVal Type As String, ByVal ShortVal As String) As Integer
        Dim SearchIndex() As Combo_Data
        Dim i As Integer
        SearchIndex = GetSearchIndex(Type)
        For i = 0 To UBound(SearchIndex)
            If SearchIndex(i).strShort = ShortVal Then Return i
        Next
        Return Nothing
    End Function
    Public Function Get_MunisCode_From_AssetCode(AssetCode As String) As String
        Return MySQLDB.Get_SQLValue("munis_codes", "asset_man_code", AssetCode, "munis_code")
    End Function
    Public Sub BuildIndexes()
        Logger("Building Indexes...")
        With MySQLDB
            Locations = .BuildIndex(CodeType.Device, ComboType.Location)
            ChangeType = .BuildIndex(CodeType.Device, ComboType.ChangeType)
            EquipType = .BuildIndex(CodeType.Device, ComboType.EquipType)
            OSType = .BuildIndex(CodeType.Device, ComboType.OSType)
            StatusType = .BuildIndex(CodeType.Device, ComboType.StatusType)
            Sibi_StatusType = .BuildIndex(CodeType.Sibi, ComboType.SibiStatusType)
            Sibi_ItemStatusType = .BuildIndex(CodeType.Sibi, ComboType.SibiItemStatusType)
            Sibi_RequestType = .BuildIndex(CodeType.Sibi, ComboType.SibiRequestType)
            Sibi_AttachFolder = .BuildIndex(CodeType.Sibi, ComboType.SibiAttachFolder)
        End With
        Logger("Building Indexes Done...")
    End Sub
    Public Function GetShortEquipType(ByVal index As Integer) As String
        Try
            Return EquipType(index).strShort
        Catch
            Return ""
        End Try
    End Function
    Public Function ConnectionReady() As Boolean
        Select Case GlobalConn.State
            Case ConnectionState.Closed
                Return False
            Case ConnectionState.Open
                Return True
            Case ConnectionState.Connecting
                Return False
            Case Else
                Return False
        End Select
    End Function
End Module
