Imports MySql.Data.MySqlClient
Public Class clsAssetManager_Functions
    Private SQLComms As New clsMySQL_Comms
    Private FPTComms As New clsFTP_Comms

    Public Function AddNewDevice(DeviceInfo As Device_Info, MunisEmp As Emp_Info) As Boolean
        Try
            Dim strUID As String = Guid.NewGuid.ToString
            Dim rows As Integer
            Dim strSqlQry1 = "INSERT INTO devices (dev_UID,dev_description,dev_location,dev_cur_user,dev_serial,dev_asset_tag,dev_purchase_date,dev_po,dev_replacement_year,dev_eq_type,dev_osversion,dev_status,dev_lastmod_user,dev_lastmod_date,dev_trackable,dev_cur_user_emp_num) VALUES(@dev_UID,@dev_description,@dev_location,@dev_cur_user,@dev_serial,@dev_asset_tag,@dev_purchase_date,@dev_po,@dev_replacement_year,@dev_eq_type,@dev_osversion,@dev_status,@dev_lastmod_user,@dev_lastmod_date,@dev_trackable,@dev_cur_user_emp_num)"
            Dim cmd As MySqlCommand = SQLComms.Return_SQLCommand(strSqlQry1)
            cmd.Parameters.AddWithValue("@dev_UID", strUID)
            cmd.Parameters.AddWithValue("@dev_description", DeviceInfo.strDescription)
            cmd.Parameters.AddWithValue("@dev_location", DeviceInfo.strLocation)
            cmd.Parameters.AddWithValue("@dev_cur_user", DeviceInfo.strCurrentUser)
            cmd.Parameters.AddWithValue("@dev_cur_user_emp_num", MunisEmp.Number)
            cmd.Parameters.AddWithValue("@dev_serial", DeviceInfo.strSerial)
            cmd.Parameters.AddWithValue("@dev_asset_tag", DeviceInfo.strAssetTag)
            cmd.Parameters.AddWithValue("@dev_purchase_date", DeviceInfo.dtPurchaseDate)
            cmd.Parameters.AddWithValue("@dev_po", DeviceInfo.strPO)
            cmd.Parameters.AddWithValue("@dev_replacement_year", DeviceInfo.strReplaceYear)
            cmd.Parameters.AddWithValue("@dev_eq_type", DeviceInfo.strEqType)
            cmd.Parameters.AddWithValue("@dev_osversion", DeviceInfo.strOSVersion)
            cmd.Parameters.AddWithValue("@dev_status", DeviceInfo.strStatus)
            cmd.Parameters.AddWithValue("@dev_lastmod_user", strLocalUser)
            cmd.Parameters.AddWithValue("@dev_lastmod_date", Now)
            cmd.Parameters.AddWithValue("@dev_trackable", Convert.ToInt32(DeviceInfo.bolTrackable))
            rows = rows + cmd.ExecuteNonQuery()
            Dim strSqlQry2 = "INSERT INTO dev_historical (hist_change_type, hist_notes, hist_serial, hist_description, hist_location, hist_cur_user, hist_asset_tag, hist_purchase_date, hist_replacement_year, hist_po, hist_osversion, hist_dev_UID, hist_action_user, hist_eq_type, hist_status, hist_trackable) VALUES(@hist_change_type, @hist_notes, @hist_serial, @hist_description, @hist_location, @hist_cur_user, @hist_asset_tag, @hist_purchase_date, @hist_replacement_year, @hist_po, @hist_osversion, @hist_dev_UID, @hist_action_user, @hist_eq_type, @hist_status, @hist_trackable)"
            cmd.Parameters.AddWithValue("@hist_change_type", "NEWD")
            cmd.Parameters.AddWithValue("@hist_notes", DeviceInfo.strNote)
            cmd.Parameters.AddWithValue("@hist_serial", DeviceInfo.strSerial)
            cmd.Parameters.AddWithValue("@hist_description", DeviceInfo.strDescription)
            cmd.Parameters.AddWithValue("@hist_location", DeviceInfo.strLocation)
            cmd.Parameters.AddWithValue("@hist_cur_user", DeviceInfo.strCurrentUser)
            cmd.Parameters.AddWithValue("@hist_asset_tag", DeviceInfo.strAssetTag)
            cmd.Parameters.AddWithValue("@hist_purchase_date", DeviceInfo.dtPurchaseDate)
            cmd.Parameters.AddWithValue("@hist_replacement_year", DeviceInfo.strReplaceYear)
            cmd.Parameters.AddWithValue("@hist_po", DeviceInfo.strPO)
            cmd.Parameters.AddWithValue("@hist_osversion", DeviceInfo.strOSVersion)
            cmd.Parameters.AddWithValue("@hist_dev_UID", strUID)
            cmd.Parameters.AddWithValue("@hist_action_user", strLocalUser)
            cmd.Parameters.AddWithValue("@hist_eq_type", DeviceInfo.strEqType)
            cmd.Parameters.AddWithValue("@hist_status", DeviceInfo.strStatus)
            cmd.Parameters.AddWithValue("@hist_trackable", Convert.ToInt32(DeviceInfo.bolTrackable))
            cmd.CommandText = strSqlQry2
            rows = rows + cmd.ExecuteNonQuery()
            cmd.Dispose()
            If rows = 2 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Exit Function
            Else
                EndProgram()
            End If
        End Try
    End Function
    Public Function Update_SQLValue(table As String, fieldIN As String, valueIN As String, idField As String, idValue As String) As Integer
        Try
            Dim sqlUpdateQry As String = "UPDATE " & table & " SET " & fieldIN & "=@ValueIN  WHERE " & idField & "='" & idValue & "'"
            Dim cmd As MySqlCommand = SQLComms.Return_SQLCommand(sqlUpdateQry)
            cmd.Parameters.AddWithValue("@ValueIN", valueIN)
            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function DeleteSQLAttachment(AttachUID As String, Type As String) As Integer
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Function
        End If
        Try
            Dim rows
            Dim reader As MySqlDataReader
            Dim strDeviceID As String
            Dim strSQLIDQry As String
            If Type = Entry_Type.Device Then
                strSQLIDQry = "SELECT attach_dev_UID FROM dev_attachments WHERE attach_file_UID='" & AttachUID & "'"
            ElseIf Type = Entry_Type.Sibi Then
                strSQLIDQry = "SELECT sibi_attach_uid FROM sibi_attachments WHERE sibi_attach_file_UID='" & AttachUID & "'"
            End If
            reader = SQLComms.Return_SQLReader(strSQLIDQry)
            With reader
                Do While .Read()
                    If Type = Entry_Type.Device Then
                        strDeviceID = !attach_dev_UID
                    ElseIf Type = Entry_Type.Sibi Then
                        strDeviceID = !sibi_attach_UID
                    End If
                Loop
            End With
            reader.Close()
            'Delete FTP Attachment
            If FTP.DeleteFTPAttachment(AttachUID, strDeviceID) Then
                'delete SQL entry
                Dim strSQLDelQry As String
                If Type = Entry_Type.Device Then
                    strSQLDelQry = "DELETE FROM dev_attachments WHERE attach_file_UID='" & AttachUID & "'"
                ElseIf Type = Entry_Type.Sibi Then
                    strSQLDelQry = "DELETE FROM sibi_attachments WHERE sibi_attach_file_UID='" & AttachUID & "'"
                End If
                rows = SQLComms.Return_SQLCommand(strSQLDelQry).ExecuteNonQuery
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
    Public Function Has_Attachments(strGUID As String, Type As String) As Boolean
        Try
            Dim reader As MySqlDataReader
            Dim strQRY As String
            Select Case Type
                Case Entry_Type.Device
                    strQRY = "SELECT attach_dev_UID FROM dev_attachments WHERE attach_dev_UID='" & strGUID & "'"
                Case Entry_Type.Sibi
                    strQRY = "SELECT sibi_attach_uid FROM sibi_attachments WHERE sibi_attach_uid='" & strGUID & "'"
            End Select
            reader = SQLComms.Return_SQLReader(strQRY)
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
    Public Function Get_SQLValue(table As String, fieldIN As String, valueIN As String, fieldOUT As String) As String
        Dim sqlQRY As String = "SELECT " & fieldOUT & " FROM " & table & " WHERE " & fieldIN & " = '" & valueIN & "' LIMIT 1"
        Try
            Dim cmd As New MySqlCommand
            cmd.Connection = GlobalConn
            cmd.CommandText = sqlQRY
            Return Convert.ToString(cmd.ExecuteScalar)
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return ""
        End Try
    End Function
    Public Function Get_EntryInfo(ByVal strGUID As String) As Device_Info
        Try
            If Not ConnectionReady() Then
                Exit Function
            End If
            Dim tmpInfo As Device_Info
            Dim reader As MySqlDataReader
            Dim strQry = "SELECT * FROM dev_historical WHERE hist_uid='" & strGUID & "'"
            reader = SQLComms.Return_SQLReader(strQry)
            With reader
                Do While .Read()
                    tmpInfo.Historical.strChangeType = GetHumanValue(DeviceIndex.ChangeType, !hist_change_type)
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
    Public Function Get_DeviceUID(ByVal AssetTag As String, ByVal Serial As String) As String
        Dim reader As MySqlDataReader
        Dim UID As String
        Dim strQry = "SELECT dev_UID from devices WHERE dev_asset_tag = '" & AssetTag & "' AND dev_serial = '" & Serial & "' ORDER BY dev_input_datetime"
        reader = SQLComms.Return_SQLReader(strQry)
        With reader
            Do While .Read()
                UID = (!dev_UID)
            Loop
        End With
        reader.Close()
        Return UID
    End Function
    Public Function Delete_SQLMasterEntry(ByVal strGUID As String, Type As String) As Integer
        Try
            Dim rows
            Dim strSQLQry As String
            Select Case Type
                Case Entry_Type.Device
                    strSQLQry = "DELETE FROM devices WHERE dev_UID='" & strGUID & "'"
                Case Entry_Type.Sibi
                    strSQLQry = "DELETE FROM sibi_requests WHERE sibi_uid='" & strGUID & "'"
            End Select
            rows = SQLComms.Return_SQLCommand(strSQLQry).ExecuteNonQuery
            Return rows
            Exit Function
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                EndProgram()
            End If
        End Try
    End Function
    Public Function BuildIndex(CodeType As String, TypeName As String) As Combo_Data()
        Try
            Dim tmpArray() As Combo_Data
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM " & CodeType & " WHERE type_name ='" & TypeName & "' ORDER BY human_value"
            Dim row As Integer
            reader = SQLComms.Return_SQLReader(strQRY)
            ReDim tmpArray(0)
            row = -1
            With reader
                Do While .Read()
                    row += 1
                    ReDim Preserve tmpArray(row)
                    tmpArray(row).strID = !id
                    tmpArray(row).strLong = !human_value
                    tmpArray(row).strShort = !db_value
                Loop
            End With
            reader.Close()
            Return tmpArray
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Return Nothing
            Else
                EndProgram()
            End If
        End Try
    End Function
    Public Function BuildModuleIndex() As List(Of Access_Info)
        Dim tmpList As New List(Of Access_Info)
        Dim ModuleTable As DataTable = SQLComms.Return_SQLTable("SELECT * FROM security ORDER BY sec_access_level")
        Dim i As Integer = 0
        'ReDim ModuleArray(ModuleTable.Rows.Count - 1)
        For Each row As DataRow In ModuleTable.Rows
            Dim tmpInfo As Access_Info
            With tmpInfo
                .intLevel = row.Item("sec_access_level")
                .strModule = row.Item("sec_module")
                .strDesc = row.Item("sec_desc")
            End With
            tmpList.Add(tmpInfo)
        Next
        Return tmpList
    End Function
    Public Sub UpdateUser(ByRef User As User_Info, AccessLevel As Integer)
        User.intAccessLevel = AccessLevel
        Update_SQLValue("users", "usr_access_level", User.intAccessLevel, "usr_UID", User.strUID)
    End Sub
    Public Function FindDevice(Optional AssetTag As String = "", Optional Serial As String = "") As Device_Info
        If AssetTag IsNot "" Then
            Return CollectDeviceInfo(SQLComms.Return_SQLTable("SELECT * FROM devices WHERE dev_asset_tag='" & AssetTag & "'"))
        ElseIf Serial IsNot "" Then
            Return CollectDeviceInfo(SQLComms.Return_SQLTable("SELECT * FROM devices WHERE dev_serial='" & Serial & "'"))
        End If
    End Function
    Public Sub AddNewEmp(EmpInfo As Emp_Info)
        Try
            If Not EmpIsInDB(EmpInfo.Number) Then
                Dim UID As String = Guid.NewGuid.ToString
                Dim strQRY As String = "INSERT INTO employees
(emp_name,
emp_number,
emp_UID)
VALUES
(@emp_name,
@emp_number,
@emp_UID)"
                Dim cmd As MySqlCommand = SQLComms.Return_SQLCommand(strQRY)
                cmd.Parameters.AddWithValue("@emp_name", EmpInfo.Name)
                cmd.Parameters.AddWithValue("@emp_number", EmpInfo.Number)
                cmd.Parameters.AddWithValue("@emp_UID", UID)
                cmd.ExecuteNonQuery()
                cmd.Dispose()
            End If
        Catch ex As MySqlException
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Function EmpIsInDB(EmpNum As String) As Boolean
        Dim EmpName As String = Get_SQLValue("employees", "emp_number", EmpNum, "emp_name")
        If EmpName <> "" Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub GetUserAccess()
        Try
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM users WHERE usr_username='" & strLocalUser & "'"
            reader = SQLComms.Return_SQLReader(strQRY)
            With reader
                If .HasRows Then
                    Do While .Read()
                        UserAccess.strUsername = !usr_username
                        UserAccess.strFullname = !usr_fullname
                        UserAccess.intAccessLevel = !usr_access_level
                        UserAccess.strUID = !usr_UID
                    Loop
                Else
                    UserAccess.intAccessLevel = 0
                End If
            End With
            reader.Close()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Sub GetAccessLevels()
        Try
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM security ORDER BY sec_access_level" ' WHERE usr_username='" & strLocalUser & "'"
            Dim rows As Integer
            reader = SQLComms.Return_SQLReader(strQRY)
            ReDim AccessLevels(0)
            rows = -1
            With reader
                Do While .Read()
                    rows += 1
                    ReDim Preserve AccessLevels(rows)
                    AccessLevels(rows).intLevel = !sec_access_level
                    AccessLevels(rows).strModule = !sec_module
                    AccessLevels(rows).strDesc = !sec_desc
                Loop
            End With
            reader.Close()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Function DeleteMaster(ByVal strGUID As String, Type As String) As Boolean
        Try
            If Has_Attachments(strGUID, Type) Then
                If FTP.DeleteFTPFolder(strGUID, Type) Then Return Delete_SQLMasterEntry(strGUID, Type) ' if has attachments, delete ftp directory, then delete the sql records.
            Else
                Return Delete_SQLMasterEntry(strGUID, Type) 'delete sql records
            End If
        Catch ex As Exception
            Return ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Function
    Public Function DevicesBySup() As DataTable
        Dim SupInfo As Emp_Info
        Dim NewMunisSearch As New frmMunisUser
        NewMunisSearch.ShowDialog()
        If NewMunisSearch.DialogResult = DialogResult.Yes Then
            SupInfo = NewMunisSearch.EmployeeInfo
            NewMunisSearch.Dispose()
        End If
        Dim EmpList As DataTable = Munis.ListOfEmpBySup(SupInfo.Number)
        Dim DeviceList As New DataTable
        For Each r As DataRow In EmpList.Rows
            Dim tmpTable As New DataTable
            Dim strQRY As String = "SELECT * FROM devices WHERE dev_cur_user_emp_num='" & r.Item("a_employee_number") & "'"
            tmpTable = SQLComms.Return_SQLTable(strQRY)
            DeviceList.Merge(tmpTable)
        Next
        Return DeviceList
    End Function
    Public Sub CloseConnection(ByRef conn As MySqlConnection)
        conn.Close()
        conn.Dispose()
    End Sub
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
End Class
