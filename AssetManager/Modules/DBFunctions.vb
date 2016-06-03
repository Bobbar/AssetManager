Option Explicit On
Imports MySql.Data.MySqlClient
Public Module DBFunctions
    Public ReadOnly Property strLocalUser As String = Environment.UserName
    'Public Const strServerIP As String = "192.168.1.122"
    'Public Const strServerIP As String = "10.10.80.232"
    Public Const strServerIP As String = "10.10.0.89"
    Public strDatabase As String = "asset_manager"
    'Private MySQLConnectString As String = "server=df8xlbs1;port=3306;uid=asset_manager_user;pwd=A553tP455;database=asset_manager"
    Public MySQLConnectString As String = "server=" & strServerIP & ";uid=asset_mgr_usr;pwd=A553tP455;database=" & strDatabase
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
    Public Structure ConnectionData
        Public DBConnection As MySqlConnection
        Public ConnectionID As String
    End Structure
    Public CurrentConnections() As ConnectionData
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
        Public bolTrackable As Boolean
        Public Tracking As Track_Info
        Public Historical As Hist_Info
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
    Public Function OpenConnections() As Boolean
        Try
            GlobalConn.Open()
            If GlobalConn.State = ConnectionState.Open Then
                LiveConn.Open()
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            ErrHandle(ex.HResult, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            'GlobalConn.Close()
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
            ErrHandle(ex.HResult, ex.Message, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
        Return True
    End Function
    Public Sub CollectDeviceInfo(ByVal UID As String, ByVal Description As String, ByVal Location As String, ByVal CurrentUser As String, ByVal Serial As String, ByVal AssetTag As String, ByVal PurchaseDate As String, ByVal ReplaceYear As String, ByVal PO As String, ByVal OSVersion As String, ByVal EQType As String, ByVal Status As String, ByVal Trackable As Boolean, ByVal CheckedOut As Boolean)
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
            .bolTrackable = Trackable
            .Tracking.bolCheckedOut = CheckedOut
        End With
    End Sub
    Private Sub ListConnections(num As Integer)
        Debug.Print("")
        For i As Integer = 0 To UBound(CurrentConnections)
            Debug.Print(i & " => " & num & " - " & CurrentConnections(i).ConnectionID & " - " & CurrentConnections(i).DBConnection.State)
        Next
        Debug.Print("")
    End Sub
    Public Sub GetCurrentTracking(strGUID As String)
        Dim ds As New DataSet
        Dim da As New MySqlDataAdapter
        Dim dt As DataTable
        Dim dr As DataRow
        'Dim strQryRow As String
        da.SelectCommand = New MySqlCommand("SELECT * FROM trackable WHERE track_device_uid='" & strGUID & "' ORDER BY track_datestamp DESC LIMIT 1")
        da.SelectCommand.Connection = GlobalConn
        da.Fill(ds)
        dt = ds.Tables(0)
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
    Public Sub GetUserAccess()
        On Error Resume Next
        Dim reader As MySqlDataReader
        Dim strQRY = "SELECT * FROM users WHERE usr_username='" & strLocalUser & "'"
        Dim cmd As New MySqlCommand(strQRY, GlobalConn)
        reader = cmd.ExecuteReader
        With reader
            Do While .Read()
                UserAccess.strUsername = !usr_username
                UserAccess.strFullname = !usr_fullname
                UserAccess.bolIsAdmin = Convert.ToBoolean(reader("usr_isadmin"))
                UserAccess.strUID = !usr_UID
            Loop
        End With
        reader.Close()
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
    Public Function DeleteEntry(ByVal strGUID As String) As Integer
        On Error GoTo errs
        Dim cmd As New MySqlCommand
        Dim rows
        Dim strSQLQry As String = "DELETE FROM historical WHERE hist_uid='" & strGUID & "'"
        cmd.Connection = GlobalConn
        cmd.CommandText = strSQLQry
        rows = cmd.ExecuteNonQuery()
        Return rows
        Exit Function
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Resume Next
        Else
            EndProgram()
        End If
    End Function
    Public Function DeleteDevice(ByVal strGUID As String) As Integer
        On Error GoTo errs
        Dim cmd As New MySqlCommand
        Dim rows
        Dim strSQLQry As String = "DELETE FROM devices WHERE dev_UID='" & strGUID & "'"
        cmd.Connection = GlobalConn
        cmd.CommandText = strSQLQry
        rows = cmd.ExecuteNonQuery()
        Return rows
        Exit Function
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Resume Next
        Else
            EndProgram()
        End If
    End Function
    Public Function GetEntryInfo(ByVal strGUID As String) As Device_Info
        On Error GoTo errs
        If Not ConnectionReady() Then
            Exit Function
        End If
        Dim tmpInfo As Device_Info
        Dim reader As MySqlDataReader
        Dim UID As String
        Dim strQry = "SELECT * FROM historical WHERE hist_uid='" & strGUID & "'"
        Dim cmd As New MySqlCommand(strQry, GlobalConn)
        reader = cmd.ExecuteReader
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
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Resume Next
        Else
            EndProgram()
        End If
    End Function
    Public Function GetDeviceUID(ByVal AssetTag As String, ByVal Serial As String) As String
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim reader As MySqlDataReader
        Dim UID As String
        Dim strQry = "SELECT dev_UID from devices WHERE dev_asset_tag = '" & AssetTag & "' AND dev_serial = '" & Serial & "' ORDER BY dev_input_datetime"
        Dim cmd As New MySqlCommand(strQry, GlobalConn)
        reader = cmd.ExecuteReader
        With reader
            Do While .Read()
                UID = (!dev_UID)
            Loop
        End With
        reader.Close()
        Return UID
    End Function
    Public Function GetDBValue(ByVal IndexType As String, ByVal index As Integer) As Object
        On Error GoTo errs
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
        Exit Function
errs:
        Return Nothing
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
        On Error GoTo errs
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim reader As MySqlDataReader
        Dim strQRY = "SELECT * FROM combo_data WHERE combo_type ='" & ComboType.Location & "' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strQRY, GlobalConn)
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
        reader.Close()
        Exit Sub
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Resume Next
        Else
            EndProgram()
        End If
    End Sub
    Public Sub BuildChangeTypeIndex()
        On Error GoTo errs
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim reader As MySqlDataReader
        Dim strQRY = "SELECT * FROM combo_data WHERE combo_type ='" & ComboType.ChangeType & "' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strQRY, GlobalConn)
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
        'CloseConnection(ConnID)
        reader.Close()
        Exit Sub
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Resume Next
        Else
            EndProgram()
        End If
    End Sub
    Public Sub BuildEquipTypeIndex()
        On Error GoTo errs
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim reader As MySqlDataReader
        Dim strQRY = "SELECT * FROM combo_data WHERE combo_type ='" & ComboType.EquipType & "' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strQRY, GlobalConn)
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
        'CloseConnection(ConnID)
        reader.Close()
        Exit Sub
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Resume Next
        Else
            EndProgram()
        End If
    End Sub
    Public Sub BuildOSTypeIndex()
        On Error GoTo errs
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim reader As MySqlDataReader
        Dim strQRY = "SELECT * FROM combo_data WHERE combo_type ='" & ComboType.OSType & "' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strQRY, GlobalConn)
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
        reader.Close()
        Exit Sub
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Resume Next
        Else
            EndProgram()
        End If
    End Sub
    Public Sub BuildStatusTypeIndex()
        On Error GoTo errs
        Dim ConnID As String = Guid.NewGuid.ToString
        Dim reader As MySqlDataReader
        Dim strGetDevices = "SELECT * FROM combo_data WHERE combo_type ='" & ComboType.StatusType & "' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strGetDevices, GlobalConn)
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
        reader.Close()
        Exit Sub
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Resume Next
        Else
            EndProgram()
        End If
    End Sub
    Public Function GetShortEquipType(ByVal index As Integer) As String
        On Error GoTo errs
        Return EquipType(index).strShort
        Exit Function
errs:
        Return ""
    End Function
    Public Function ConnectionReady() As Boolean
        Select Case GlobalConn.State
            Case ConnectionState.Closed
                If Not AssetManager.ReconnectThread.IsBusy Then AssetManager.ReconnectThread.RunWorkerAsync()
                Return False
            Case ConnectionState.Open
                Return True
            Case ConnectionState.Connecting
                Return False
            Case Else
                If Not AssetManager.ReconnectThread.IsBusy Then AssetManager.ReconnectThread.RunWorkerAsync()
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
            'AssetManager.DateTimeLabel.Text = ds.Tables(0).Rows(0).Item(0).ToString
            If rows > 0 Then
                Return True
            Else
                Return False
            End If
            Exit Function
        Catch ex As MySqlException
            'ErrHandle(ex, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
    End Function
    Public Sub UpdateDevice()
        On Error GoTo errs
        Dim rows As Integer
        Dim strSQLQry1 = "UPDATE devices SET dev_description='" & View.NewData.strDescription & "', dev_location='" & View.NewData.strLocation & "', dev_cur_user='" & View.NewData.strCurrentUser & "', dev_serial='" & View.NewData.strSerial & "', dev_asset_tag='" & View.NewData.strAssetTag & "', dev_purchase_date='" & View.NewData.dtPurchaseDate & "', dev_replacement_year='" & View.NewData.strReplaceYear & "', dev_osversion='" & View.NewData.strOSVersion & "', dev_eq_type='" & View.NewData.strEqType & "', dev_status='" & View.NewData.strStatus & "', dev_trackable='" & Convert.ToInt32(View.NewData.bolTrackable) & "' WHERE dev_UID='" & CurrentDevice.strGUID & "'"
        Dim cmd As New MySqlCommand
        cmd.Connection = GlobalConn
        cmd.CommandText = strSQLQry1
        rows = rows + cmd.ExecuteNonQuery()
        Dim strSqlQry2 = "INSERT INTO historical (hist_change_type,hist_notes,hist_serial,hist_description,hist_location,hist_cur_user,hist_asset_tag,hist_purchase_date,hist_replacement_year,hist_osversion,hist_dev_UID,hist_action_user,hist_eq_type,hist_status,hist_trackable) VALUES ('" & GetDBValue(ComboType.ChangeType, UpdateDev.cmbUpdate_ChangeType.SelectedIndex) & "','" & View.NewData.strNote & "','" & View.NewData.strSerial & "','" & View.NewData.strDescription & "','" & View.NewData.strLocation & "','" & View.NewData.strCurrentUser & "','" & View.NewData.strAssetTag & "','" & View.NewData.dtPurchaseDate & "','" & View.NewData.strReplaceYear & "','" & View.NewData.strOSVersion & "','" & CurrentDevice.strGUID & "','" & strLocalUser & "','" & View.NewData.strEqType & "','" & View.NewData.strStatus & "','" & Convert.ToInt32(View.NewData.bolTrackable) & "')"
        cmd.CommandText = strSqlQry2
        rows = rows + cmd.ExecuteNonQuery()
        UpdateDev.strNewNote = Nothing
        If rows = 2 Then
            View.ViewDevice(CurrentDevice.strGUID)
            Dim blah = MsgBox("Update Added.", vbOKOnly + vbInformation, "Success")
        Else
            Dim blah = MsgBox("Unsuccessful! The number of affected rows was not what was expected.", vbOKOnly + vbAbort, "Unexpected Result")
        End If
        Exit Sub
errs:
        If ErrHandle(Err.Number, Err.Description, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Resume Next
        Else
            EndProgram()
        End If
    End Sub
End Module
