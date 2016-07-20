Option Explicit On
Imports MySql.Data.MySqlClient
Public Module DBFunctions
    Public ReadOnly Property strLocalUser As String = Environment.UserName
    'Public Const strServerIP As String = "192.168.1.122"
    'Public Const strServerIP As String = "10.10.80.232"
    Public Const strServerIP As String = "10.10.0.89"
    Public strDatabase As String = "asset_manager"
    Public MySQLConnectString As String = "server=" & strServerIP & ";uid=asset_mgr_usr;pwd=" & DecodePassword(EncMySqlPass) & ";database=" & strDatabase
    Public Const strDBDateTimeFormat As String = "yyyy-MM-dd HH:mm:ss"
    Public Const strDBDateFormat As String = "yyyy-MM-dd"
    Public Const strCommMessage As String = "Communicating..."
    Public Const strLoadingGridMessage As String = "Building Grid..."
    Public Const strCheckOut As String = "OUT"
    Public Const strCheckIn As String = "IN"
    Public strLastQry As String
    Private ConnCount As Integer = 0
    Public GlobalConn As New MySqlConnection(MySQLConnectString)
    Public LiveConn As New MySqlConnection(MySQLConnectString)
    Public strServerTime As String
    Public Const strFTPUser As String = "asset_manager"
    Public strFTPPass As String = DecodePassword(EncFTPUserPass)
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
        Public dtPurchaseDate ' As String
        Public strFiscalYear As String
        Public strReplaceYear As String
        Public strOSVersion As String
        Public strGUID As String
        Public strPO As String
        Public strStatus As String
        Public strNote As String
        Public bolTrackable As Boolean
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
        Public strReqNUmber As String
        Public strReplaceAsset As String
        Public strReplaceSerial As String



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
    Public AccessLevels() As Access_Info
    Public CurrentDevice As Device_Info
    Public Locations() As Combo_Data
    Public ChangeType() As Combo_Data
    Public EquipType() As Combo_Data
    Public OSType() As Combo_Data
    Public StatusType() As Combo_Data
    'sibi
    Public RequestData As Request_Info
    Public Sibi_StatusType As Combo_Data
    Public Sibi_ItemStatusType As Combo_Data

    Public Structure User_Info
        Public strUsername As String
        Public strFullname As String
        Public bolIsAdmin As Boolean
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
    End Class
    Public Function OpenConnections() As Boolean
        Try
            GlobalConn.Open()
            If GlobalConn.State = ConnectionState.Open Then
                LiveConn.Open()
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
            GlobalConn.Close()
            LiveConn.Close()
            GlobalConn.Dispose()
            LiveConn.Dispose()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
        Return True
    End Function
    Public Sub CollectDeviceInfo(DeviceTable As DataTable)
        Try
            With CurrentDevice
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
                .Tracking.bolCheckedOut = CBool(DeviceTable.Rows(0).Item("dev_checkedout"))
            End With
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Sub GetCurrentTracking(strGUID As String)
        Dim dt As DataTable
        Dim dr As DataRow
        dt = ReturnSQLTable("SELECT * FROM dev_trackable WHERE track_device_uid='" & strGUID & "' ORDER BY track_datestamp DESC LIMIT 1") 'ds.Tables(0)
        If dt.Rows.Count > 0 Then
            For Each dr In dt.Rows
                With dr
                    CurrentDevice.Tracking.strCheckOutTime = .Item("track_checkout_time").ToString
                    CurrentDevice.Tracking.strCheckInTime = .Item("track_checkin_time").ToString
                    CurrentDevice.Tracking.strUseLocation = .Item("track_use_location").ToString
                    CurrentDevice.Tracking.strCheckOutUser = .Item("track_checkout_user").ToString
                    CurrentDevice.Tracking.strCheckInUser = .Item("track_checkin_user").ToString
                    CurrentDevice.Tracking.strDueBackTime = .Item("track_dueback_date").ToString
                    CurrentDevice.Tracking.strUseReason = .Item("track_notes").ToString
                End With
            Next
        End If
    End Sub
    Public Function ReturnSQLTable(strSQLQry As String) As DataTable
        'Debug.Print("Table Hit " & Date.Now.Ticks)
        Dim ds As New DataSet
        Dim da As New MySqlDataAdapter
        Try
            da.SelectCommand = New MySqlCommand(strSQLQry)
            da.SelectCommand.Connection = GlobalConn
            da.Fill(ds)
            da.Dispose()
            Return ds.Tables(0)
        Catch ex As Exception
            da.Dispose()
            ds.Dispose()
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function ReturnSQLReader(strSQLQry As String) As MySqlDataReader
        'Debug.Print("Reader Hit " & Date.Now.Ticks)
        Try
            Dim cmd As New MySqlCommand(strSQLQry, GlobalConn)
            Return cmd.ExecuteReader
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function ReturnSQLCommand(strSQLQry As String) As MySqlCommand
        'Debug.Print("Command Hit " & Date.Now.Ticks)
        Try
            Dim cmd As New MySqlCommand
            cmd.Connection = GlobalConn
            cmd.CommandText = strSQLQry
            Return cmd
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function DeleteAttachment(AttachUID As String) As Integer
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Function
        End If
        Try
            Dim rows
            Dim reader As MySqlDataReader
            Dim strDeviceID As String
            Dim strSQLDevIDQry As String = "SELECT attach_dev_UID FROM dev_attachments WHERE attach_file_UID='" & AttachUID & "'"
            reader = ReturnSQLReader(strSQLDevIDQry)
            With reader
                Do While .Read()
                    strDeviceID = !attach_dev_UID
                Loop
            End With
            reader.Close()
            'Delete FTP Attachment
            If DeleteFTPAttachment(AttachUID, strDeviceID) Then
                'delete SQL entry
                Dim strSQLDelQry As String = "DELETE FROM dev_attachments WHERE attach_file_UID='" & AttachUID & "'"
                rows = ReturnSQLCommand(strSQLDelQry).ExecuteNonQuery
                Return rows
                'Else  'if file not found then we might as well remove the DB record.
                '    Dim strSQLDelQry As String = "DELETE FROM dev_attachments WHERE attach_file_UID='" & AttachUID & "'"
                '    cmd.Connection = GlobalConn
                '    cmd.CommandText = strSQLDelQry
                '    rows = cmd.ExecuteNonQuery()
                '    Return rows
            End If
            Exit Function
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Exit Try
            Else
                EndProgram()
            End If
        End Try
        Return -1
    End Function
    Public Function GetShortLocation(ByVal index As Integer) As String
        Try
            Return Locations(index).strShort
        Catch
            Return ""
        End Try
    End Function
    Public Function DeleteEntry(ByVal strGUID As String) As Integer
        Try
            Dim rows
            Dim strSQLQry As String = "DELETE FROM dev_historical WHERE hist_uid='" & strGUID & "'"
            rows = ReturnSQLCommand(strSQLQry).ExecuteNonQuery
            Return rows
            Exit Function
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                EndProgram()
            End If
        End Try
    End Function
    Public Function DeleteDevice(ByVal strGUID As String) As Boolean
        Try
            If HasAttachments(strGUID) Then
                If DeleteFTPDeviceFolder(strGUID) Then Return DeleteSQLDevice(strGUID) ' if has attachments, delete ftp directory, then delete the sql records.
            Else
                Return DeleteSQLDevice(strGUID) 'delete sql records
            End If
        Catch ex As Exception
            Return ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Function
    Public Function HasAttachments(strGUID As String) As Boolean
        Try
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT attach_dev_UID FROM dev_attachments WHERE attach_dev_UID='" & strGUID & "'"
            reader = ReturnSQLReader(strQRY)
            Dim bolHasRows As Boolean = reader.HasRows
            reader.Close()
            Return bolHasRows
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                EndProgram()
            End If
            Return Nothing
        End Try
    End Function
    Public Function DeleteSQLDevice(ByVal strGUID As String) As Integer
        Try
            Dim rows
            Dim strSQLQry As String = "DELETE FROM devices WHERE dev_UID='" & strGUID & "'"
            rows = ReturnSQLCommand(strSQLQry).ExecuteNonQuery
            Return rows
            Exit Function
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                EndProgram()
            End If
        End Try
    End Function
    Public Function GetEntryInfo(ByVal strGUID As String) As Device_Info
        Try
            If Not ConnectionReady() Then
                Exit Function
            End If
            Dim tmpInfo As Device_Info
            Dim reader As MySqlDataReader
            Dim strQry = "SELECT * FROM dev_historical WHERE hist_uid='" & strGUID & "'"
            reader = ReturnSQLReader(strQry)
            With reader
                Do While .Read()
                    tmpInfo.Historical.strChangeType = GetHumanValue(ComboType.ChangeType,!hist_change_type)
                    tmpInfo.strAssetTag = !hist_asset_tag
                    tmpInfo.strCurrentUser = !hist_cur_user
                    tmpInfo.strSerial = !hist_serial
                    tmpInfo.strDescription = !hist_description
                    tmpInfo.Historical.dtActionDateTime = !hist_action_datetime
                    tmpInfo.Historical.strActionUser = !hist_action_user
                Loop
            End With
            reader.Close()
            Return tmpInfo
            Exit Function
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Exit Try
            Else
                EndProgram()
            End If
        End Try
    End Function
    Public Function GetDeviceUID(ByVal AssetTag As String, ByVal Serial As String) As String
        Dim reader As MySqlDataReader
        Dim UID As String
        Dim strQry = "SELECT dev_UID from devices WHERE dev_asset_tag = '" & AssetTag & "' AND dev_serial = '" & Serial & "' ORDER BY dev_input_datetime"
        reader = ReturnSQLReader(strQry)
        With reader
            Do While .Read()
                UID = (!dev_UID)
            Loop
        End With
        reader.Close()
        Return UID
    End Function
    Public Function GetDBValue(ByVal IndexType As String, ByVal index As Integer) As Object
        Try
            If index > -1 Then
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
                        Return Nothing
                End Select
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
    Public Sub BuildLocationIndex()
        Try
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM dev_codes WHERE combo_type ='" & ComboType.Location & "' ORDER BY combo_data_human"
            Dim row As Integer
            reader = ReturnSQLReader(strQRY)
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
            reader.Close()
            Exit Sub
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Exit Try
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Public Sub BuildChangeTypeIndex()
        Try
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM dev_codes WHERE combo_type ='" & ComboType.ChangeType & "' ORDER BY combo_data_human"
            Dim row As Integer
            reader = ReturnSQLReader(strQRY)
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
            reader.Close()
            Exit Sub
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Exit Try
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Public Sub BuildEquipTypeIndex()
        Try
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM dev_codes WHERE combo_type ='" & ComboType.EquipType & "' ORDER BY combo_data_human"
            Dim row As Integer
            reader = ReturnSQLReader(strQRY)
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
            reader.Close()
            Exit Sub
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Exit Try
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Public Sub BuildOSTypeIndex()
        Try
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM dev_codes WHERE combo_type ='" & ComboType.OSType & "' ORDER BY combo_data_human"
            Dim row As Integer
            reader = ReturnSQLReader(strQRY)
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
            reader.Close()
            Exit Sub
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Exit Try
            Else
                EndProgram()
            End If
        End Try
    End Sub
    Public Sub BuildStatusTypeIndex()
        Try
            Dim reader As MySqlDataReader
            Dim strGetDevices = "SELECT * FROM dev_codes WHERE combo_type ='" & ComboType.StatusType & "' ORDER BY combo_data_human"
            Dim row As Integer
            reader = ReturnSQLReader(strGetDevices)
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
            reader.Close()
            Exit Sub
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Exit Try
            Else
                EndProgram()
            End If
        End Try
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
    Public Function CheckConnection() As Boolean
        Try
            Dim ds As New DataSet
            Dim da As New MySqlDataAdapter
            Dim rows As Integer
            da.SelectCommand = New MySqlCommand("SELECT NOW()")
            da.SelectCommand.Connection = GlobalConn
            da.Fill(ds)
            rows = ds.Tables(0).Rows.Count
            If rows > 0 Then
                Return True
            Else
                Return False
            End If
            Exit Function
        Catch ex As MySqlException
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
    End Function
End Module
