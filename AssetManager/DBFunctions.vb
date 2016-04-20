Option Explicit On
Imports MySql.Data.MySqlClient
Public Module DBFunctions
    Public ReadOnly Property strLocalUser As String = Environment.UserName
    Public cn_global As New MySqlConnection("server=localhost;uid=root;pwd=SQLR00tP455W0rd;database=asset_manager")
    Public cn_global2 As New MySqlConnection("server=localhost;uid=root;pwd=SQLR00tP455W0rd;database=asset_manager")
    Public Const strDBDateTimeFormat As String = "YYYY-MM-DD hh:mm:ss"
    Public Const strDBDateFormat As String = "yyyy-MM-dd"
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
        Public dtPurchaseDate ' As String
        Public strReplaceYear As String
        Public strOSVersion As String
        Public strGUID As String
        Public strPO As String
        Public strStatus As String
        Public strNote As String
    End Structure
    Public CurrentDevice As Device_Info
    Public Locations() As Combo_Data
    Public ChangeType() As Combo_Data
    Public EquipType() As Combo_Data
    Public OSType() As Combo_Data
    Public StatusType() As Combo_Data
    Public SearchResults() As Device_Info
    Public Structure User_Info
        Public strUsername As String
        Public strFullname As String
        Public bolIsAdmin As Boolean
        Public strUID As String
    End Structure
    Public UserAccess As User_Info
    Public Sub AddToResults(Info As Device_Info)
        ReDim Preserve SearchResults(UBound(SearchResults) + 1)
        SearchResults(UBound(SearchResults)) = Info
    End Sub
    Public NotInheritable Class ComboType
        Public Const Location As String = "LOCATION"
        Public Const ChangeType As String = "CHANGETYPE"
        Public Const EquipType As String = "EQ_TYPE"
        Public Const OSType As String = "OS_TYPE"
        Public Const StatusType As String = "STATUS_TYPE"
    End Class
    Public Sub CollectDeviceInfo(ByVal UID As String, ByVal Description As String, ByVal Location As String, ByVal CurrentUser As String, ByVal Serial As String, ByVal AssetTag As String, ByVal PurchaseDate As String, ByVal ReplaceYear As String, ByVal PO As String, ByVal OSVersion As String, ByVal EQType As String, ByVal Status As String)
        With CurrentDevice
            .strGUID = UID
            .strDescription = Description
            .strLocation = Location
            .strCurrentUser = CurrentUser
            .strSerial = Serial
            .strAssetTag = AssetTag
            .dtPurchaseDate = PurchaseDate
            .strReplaceYear = ReplaceYear
            .strPO = PO
            .strOSVersion = OSVersion
            .strEqType = EQType
            .strStatus = Status
        End With
    End Sub
    Public Sub GetUserAccess()
        Dim reader As MySqlDataReader
        cn_global.Open()
        Dim strQRY = "SELECT * FROM users WHERE usr_username='" & strLocalUser & "'"
        Dim cmd As New MySqlCommand(strQRY, cn_global)
        reader = cmd.ExecuteReader
        With reader
            Do While .Read()
                UserAccess.strUsername = !usr_username
                UserAccess.strFullname = !usr_fullname
                UserAccess.bolIsAdmin = Convert.ToBoolean(reader("usr_isadmin"))
                UserAccess.strUID = !usr_UID
            Loop
        End With
        cn_global.Close()
    End Sub
    Public Function IsAdmin() As Boolean
        Return UserAccess.bolIsAdmin
    End Function
    Public Function CheckForAdmin() As Boolean
        If Not UserAccess.bolIsAdmin Then
            Dim blah = MsgBox("Administrator rights required for this function.", vbOKOnly + vbExclamation, "Access Denied")
            Return False
        Else
            Return True
        End If
    End Function
    Public Function GetShortLocation(ByVal index As Integer) As String
        On Error GoTo errs
        Return Locations(index).strShort
        Exit Function
errs:
        Return ""
    End Function
    Public Function DeleteDevice(ByVal GUID As String) As Integer
        Dim cmd As New MySqlCommand
        Dim rows
        Dim strSQLQry As String = "DELETE FROM devices WHERE dev_UID='" & GUID & "'"
        cn_global.Open()
        cmd.Connection = cn_global
        cmd.CommandText = strSQLQry
        rows = cmd.ExecuteNonQuery()
        cn_global.Close()
        Return rows
    End Function
    Public Function GetDeviceUID(ByVal AssetTag As String, ByVal Serial As String) As String
        Dim reader As MySqlDataReader
        Dim strQry = "SELECT dev_UID from devices WHERE dev_asset_tag = '" & AssetTag & "' AND dev_serial = '" & Serial & "' ORDER BY dev_input_datetime"
        Dim devUID As String
        cn_global2.Open()
        Dim cmd As New MySqlCommand(strQry, cn_global2)
        reader = cmd.ExecuteReader
        With reader
            Do While .Read()
                devUID = (!dev_UID)
            Loop
        End With
        cn_global2.Close()
        Return devUID
    End Function
    Public Function GetDBValue(ByVal IndexType As String, ByVal index As Integer) As String
        On Error GoTo errs
        Select Case IndexType
            Case ComboType.Location
                Return Locations(index).strShort
            Case ComboType.ChangeType
                Return ChangeType(index).strShort
            Case ComboType.EquipType
                Return EquipType(index).strShort
            Case ComboType.OSType
                Return OSType(index).strShort
            Case ComboType.StatusType
                Return StatusType(index).strShort
            Case Else
                Return ""
        End Select
        Exit Function
errs:
        Return ""
    End Function
    Public Function GetHumanValue(ByVal Type As String, ByVal ShortVal As String) As String
        Dim SearchIndex() As Combo_Data
        Dim i As Integer
        SearchIndex = GetSearchIndex(Type)
        For i = 0 To UBound(SearchIndex)
            If SearchIndex(i).strShort = ShortVal Then Return SearchIndex(i).strLong
        Next
    End Function
    Private Function GetSearchIndex(ByVal Type As String) As Combo_Data()
        Select Case Type
            Case ComboType.Location
                GetSearchIndex = Locations
            Case ComboType.ChangeType
                GetSearchIndex = ChangeType
            Case ComboType.EquipType
                GetSearchIndex = EquipType
            Case ComboType.OSType
                GetSearchIndex = OSType
            Case ComboType.StatusType
                GetSearchIndex = StatusType
            Case Else
        End Select
    End Function
    Public Function GetComboIndexFromShort(ByVal Type As String, ByVal ShortVal As String) As Integer
        Dim SearchIndex() As Combo_Data
        Dim i As Integer
        SearchIndex = GetSearchIndex(Type)
        For i = 0 To UBound(SearchIndex)
            If SearchIndex(i).strShort = ShortVal Then Return i
        Next
    End Function
    Public Sub BuildLocationIndex()
        Dim reader As MySqlDataReader
        cn_global.Open()
        Dim strQRY = "SELECT * FROM combo_data WHERE combo_type ='" & ComboType.Location & "' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strQRY, cn_global)
        Dim row As Integer
        reader = cmd.ExecuteReader
        ReDim Locations(0)
        row = -1
        With reader
            Do While .Read()
                row = row + 1
                ReDim Preserve Locations(row)
                Locations(row).strID = !combo_ID
                Locations(row).strLong = !combo_data_human
                Locations(row).strShort = !combo_data_db
            Loop
        End With
        cn_global.Close()
    End Sub
    Public Sub BuildChangeTypeIndex()
        Dim reader As MySqlDataReader
        cn_global.Open()
        Dim strQRY = "SELECT * FROM combo_data WHERE combo_type ='" & ComboType.ChangeType & "' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strQRY, cn_global)
        Dim row As Integer
        reader = cmd.ExecuteReader
        ReDim ChangeType(0)
        row = -1
        With reader
            Do While .Read()
                row = row + 1
                ReDim Preserve ChangeType(row)
                ChangeType(row).strID = !combo_ID
                ChangeType(row).strLong = !combo_data_human
                ChangeType(row).strShort = !combo_data_db
            Loop
        End With
        cn_global.Close()
    End Sub
    Public Sub BuildEquipTypeIndex()
        Dim reader As MySqlDataReader
        cn_global.Open()
        Dim strQRY = "SELECT * FROM combo_data WHERE combo_type ='" & ComboType.EquipType & "' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strQRY, cn_global)
        Dim row As Integer
        reader = cmd.ExecuteReader
        ReDim EquipType(0)
        row = -1
        With reader
            Do While .Read()
                row = row + 1
                ReDim Preserve EquipType(row)
                EquipType(row).strID = !combo_ID
                EquipType(row).strLong = !combo_data_human
                EquipType(row).strShort = !combo_data_db
            Loop
        End With
        cn_global.Close()
    End Sub
    Public Sub BuildOSTypeIndex()
        Dim reader As MySqlDataReader
        cn_global.Open()
        Dim strQRY = "SELECT * FROM combo_data WHERE combo_type ='" & ComboType.OSType & "' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strQRY, cn_global)
        Dim row As Integer
        reader = cmd.ExecuteReader
        ReDim OSType(0)
        row = -1
        With reader
            Do While .Read()
                row = row + 1
                ReDim Preserve OSType(row)
                OSType(row).strID = !combo_ID
                OSType(row).strLong = !combo_data_human
                OSType(row).strShort = !combo_data_db
            Loop
        End With
        cn_global.Close()
    End Sub
    Public Sub BuildStatusTypeIndex()
        Dim reader As MySqlDataReader
        cn_global.Open()
        Dim strGetDevices = "SELECT * FROM combo_data WHERE combo_type ='" & ComboType.StatusType & "' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strGetDevices, cn_global)
        Dim row As Integer
        reader = cmd.ExecuteReader
        ReDim StatusType(0)
        row = -1
        With reader
            Do While .Read()
                row = row + 1
                ReDim Preserve StatusType(row)
                StatusType(row).strID = !combo_ID
                StatusType(row).strLong = !combo_data_human
                StatusType(row).strShort = !combo_data_db
            Loop
        End With
        cn_global.Close()
    End Sub
    Public Function GetShortEquipType(ByVal index As Integer) As String
        On Error GoTo errs
        Return EquipType(index).strShort
        Exit Function
errs:
        Return ""
    End Function
    Public Sub UpdateDevice()
        cn_global.Open()
        Dim strSQLQry1 = "UPDATE devices set dev_description='" & View.NewData.strDescription & "', dev_location='" & View.NewData.strLocation & "', dev_cur_user='" & View.NewData.strCurrentUser & "', dev_serial='" & View.NewData.strSerial & "', dev_asset_tag='" & View.NewData.strAssetTag & "', dev_purchase_date='" & View.NewData.dtPurchaseDate & "', dev_replacement_year='" & View.NewData.strReplaceYear & "', dev_osversion='" & View.NewData.strOSVersion & "', dev_eq_type='" & View.NewData.strEqType & "', dev_status='" & View.NewData.strStatus & "' WHERE dev_UID='" & CurrentDevice.strGUID & "'"
        Dim cmd As New MySqlCommand
        cmd.Connection = cn_global
        cmd.CommandText = strSQLQry1
        cmd.ExecuteNonQuery()
        Dim strSqlQry2 = "INSERT INTO historical (hist_change_type,hist_notes,hist_serial,hist_description,hist_location,hist_cur_user,hist_asset_tag,hist_purchase_date,hist_replacement_year,hist_osversion,hist_dev_UID,hist_action_user,hist_eq_type,hist_status) VALUES ('" & GetDBValue(ComboType.ChangeType, UpdateDev.cmbUpdate_ChangeType.SelectedIndex) & "','" & View.NewData.strNote & "','" & View.NewData.strSerial & "','" & View.NewData.strDescription & "','" & View.NewData.strLocation & "','" & View.NewData.strCurrentUser & "','" & View.NewData.strAssetTag & "','" & View.NewData.dtPurchaseDate & "','" & View.NewData.strReplaceYear & "','" & View.NewData.strOSVersion & "','" & CurrentDevice.strGUID & "','" & strLocalUser & "','" & View.NewData.strEqType & "','" & View.NewData.strStatus & "')"
        cmd.CommandText = strSqlQry2
        cmd.ExecuteNonQuery()
        cn_global.Close()
        UpdateDev.strNewNote = Nothing
        Dim blah
        blah = MsgBox("Update Added", vbOKOnly, "Complete")
        View.ViewDevice(CurrentDevice.strGUID)
    End Sub
End Module
