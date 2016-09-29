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
    Public strServerTime As String
    Public DeviceIndex As New Device_Indexes
    Public SibiIndex As New Sibi_Indexes
    Public MunisComms As New clsMunis_Comms
    Public Munis As New clsMunis_Functions
    Public MySQLDB As New clsMySQL_Comms
    Public UserAccess As User_Info
    Public NotInheritable Class Attrib_Type
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
    Public NotInheritable Class Attrib_Table
        Public Const Sibi As String = "sibi_codes"
        Public Const Device As String = "dev_codes"
    End Class
    Public NotInheritable Class Entry_Type
        Public Const Sibi As String = "sibi_"
        Public Const Device As String = "dev_"
    End Class

    Public GlobalConn As MySqlConnection = MySQLDB.NewConnection()
    Public Function OpenConnections() As Boolean
        Try
            If GlobalConn.State <> ConnectionState.Open Then
                MySQLDB.CloseConnection(GlobalConn)
                GlobalConn = MySQLDB.NewConnection
            End If
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
                .strCurrentUserEmpNum = NoNull(DeviceTable.Rows(0).Item("dev_cur_user_emp_num"))
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
    Public Function GetShortLocation(ByVal index As Integer) As String
        Try
            Return DeviceIndex.Locations(index).strShort
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
    Public Function GetHumanValue(ByVal CodeIndex() As Combo_Data, ByVal ShortVal As String) As String
        For Each Code As Combo_Data In CodeIndex
            If Code.strShort = ShortVal Then Return Code.strLong
        Next
        Return Nothing
    End Function
    Public Function GetHumanValueFromIndex(ByVal CodeIndex() As Combo_Data, index As Integer) As String
        Return CodeIndex(index).strLong
    End Function
    Public Function GetDBValueFromHuman(ByVal CodeIndex() As Combo_Data, ByVal LongVal As String) As String
        For Each i As Combo_Data In CodeIndex
            If i.strLong = LongVal Then Return i.strShort
        Next
        Return Nothing
    End Function
    Public Function GetComboIndexFromShort(ByVal CodeIndex() As Combo_Data, ByVal ShortVal As String) As Integer
        For i As Integer = 0 To UBound(CodeIndex)
            If CodeIndex(i).strShort = ShortVal Then Return i
        Next
        Return Nothing
    End Function
    Public Function Get_MunisCode_From_AssetCode(AssetCode As String) As String
        Return MySQLDB.Get_SQLValue("munis_codes", "asset_man_code", AssetCode, "munis_code")
    End Function
    Public Sub BuildIndexes()
        Logger("Building Indexes...")
        With MySQLDB
            DeviceIndex.Locations = .BuildIndex(Attrib_Table.Device, Attrib_Type.Location)
            DeviceIndex.ChangeType = .BuildIndex(Attrib_Table.Device, Attrib_Type.ChangeType)
            DeviceIndex.EquipType = .BuildIndex(Attrib_Table.Device, Attrib_Type.EquipType)
            DeviceIndex.OSType = .BuildIndex(Attrib_Table.Device, Attrib_Type.OSType)
            DeviceIndex.StatusType = .BuildIndex(Attrib_Table.Device, Attrib_Type.StatusType)
            SibiIndex.StatusType = .BuildIndex(Attrib_Table.Sibi, Attrib_Type.SibiStatusType)
            SibiIndex.ItemStatusType = .BuildIndex(Attrib_Table.Sibi, Attrib_Type.SibiItemStatusType)
            SibiIndex.RequestType = .BuildIndex(Attrib_Table.Sibi, Attrib_Type.SibiRequestType)
            SibiIndex.AttachFolder = .BuildIndex(Attrib_Table.Sibi, Attrib_Type.SibiAttachFolder)
        End With
        Logger("Building Indexes Done...")
    End Sub
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
    Public Function DevicesBySup() As DataTable
        Dim SupInfo As Emp_Info
        Dim NewMunisSearch As New frmMunisUser
        NewMunisSearch.ShowDialog()
        If NewMunisSearch.DialogResult = DialogResult.Yes Then
            SupInfo = NewMunisSearch.EmployeeInfo
            NewMunisSearch.Dispose()
        End If
        Dim EmpList As DataTable = ListOfEmpBySup(SupInfo.Number)
        Dim DeviceList As New DataTable
        For Each r As DataRow In EmpList.Rows
            Dim tmpTable As New DataTable
            Dim strQRY As String = "SELECT * FROM devices WHERE dev_cur_user_emp_num='" & r.Item("a_employee_number") & "'"
            tmpTable = MySQLDB.Return_SQLTable(strQRY)
            DeviceList.Merge(tmpTable)
        Next
        Return DeviceList
    End Function
    Private Function ListOfEmpBySup(SupEmpNum As String) As DataTable
        Dim strQRY As String = "SELECT TOP 100 a_employee_number FROM pr_employee_master WHERE e_supervisor='" & SupEmpNum & "'"
        Return MunisComms.Return_MSSQLTable(strQRY)
    End Function
End Module
