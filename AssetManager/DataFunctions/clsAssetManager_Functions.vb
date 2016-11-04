﻿Imports MySql.Data.MySqlClient
Public Class clsAssetManager_Functions
    Public Function AddNewDevice(DeviceInfo As Device_Info, MunisEmp As Emp_Info) As Boolean
        Try
            Dim strUID As String = Guid.NewGuid.ToString
            Dim rows As Integer
            Dim strSqlQry1 = "INSERT INTO " & devices.TableName &
" (" & devices.DeviceUID & ",
" & devices.Description & ",
" & devices.Location & ",
" & devices.CurrentUser & ",
" & devices.Serial & ",
" & devices.AssetTag & ",
" & devices.PurchaseDate & ",
" & devices.PO & ",
" & devices.ReplacementYear & ",
" & devices.EQType & ",
" & devices.OSVersion & ",
" & devices.Status & ",
" & devices.LastMod_User & ",
" & devices.LastMod_Date & ",
" & devices.Trackable & ",
" & devices.Munis_Emp_Num & ") 
VALUES(@" & devices.DeviceUID & ",
@" & devices.Description & ",
@" & devices.Location & ",
@" & devices.CurrentUser & ",
@" & devices.Serial & ",
@" & devices.AssetTag & ",
@" & devices.PurchaseDate & ",
@" & devices.PO & ",
@" & devices.ReplacementYear & ",
@" & devices.EQType & ",
@" & devices.OSVersion & ",
@" & devices.Status & ",
@" & devices.LastMod_User & ",
@" & devices.LastMod_Date & ",
@" & devices.Trackable & ",
@" & devices.Munis_Emp_Num & ")"
            Dim cmd As MySqlCommand = SQLComms.Return_SQLCommand(strSqlQry1)
            cmd.Parameters.AddWithValue("@" & devices.DeviceUID, strUID)
            cmd.Parameters.AddWithValue("@" & devices.Description, DeviceInfo.strDescription)
            cmd.Parameters.AddWithValue("@" & devices.Location, DeviceInfo.strLocation)
            cmd.Parameters.AddWithValue("@" & devices.CurrentUser, DeviceInfo.strCurrentUser)
            cmd.Parameters.AddWithValue("@" & devices.Munis_Emp_Num, MunisEmp.Number)
            cmd.Parameters.AddWithValue("@" & devices.Serial, DeviceInfo.strSerial)
            cmd.Parameters.AddWithValue("@" & devices.AssetTag, DeviceInfo.strAssetTag)
            cmd.Parameters.AddWithValue("@" & devices.PurchaseDate, DeviceInfo.dtPurchaseDate)
            cmd.Parameters.AddWithValue("@" & devices.PO, DeviceInfo.strPO)
            cmd.Parameters.AddWithValue("@" & devices.ReplacementYear, DeviceInfo.strReplaceYear)
            cmd.Parameters.AddWithValue("@" & devices.EQType, DeviceInfo.strEqType)
            cmd.Parameters.AddWithValue("@" & devices.OSVersion, DeviceInfo.strOSVersion)
            cmd.Parameters.AddWithValue("@" & devices.Status, DeviceInfo.strStatus)
            cmd.Parameters.AddWithValue("@" & devices.LastMod_User, strLocalUser)
            cmd.Parameters.AddWithValue("@" & devices.LastMod_Date, Now)
            cmd.Parameters.AddWithValue("@" & devices.Trackable, Convert.ToInt32(DeviceInfo.bolTrackable))
            rows = rows + cmd.ExecuteNonQuery()
            Dim strSqlQry2 = "INSERT INTO " & historical_dev.TableName & "
(" & historical_dev.ChangeType & ",
" & historical_dev.Notes & ",
" & historical_dev.Serial & ", 
" & historical_dev.Description & ", 
" & historical_dev.Location & ",
" & historical_dev.CurrentUser & ", 
" & historical_dev.AssetTag & ", 
" & historical_dev.PurchaseDate & ", 
" & historical_dev.ReplacementYear & ", 
" & historical_dev.PO & ",
" & historical_dev.OSVersion & ", 
" & historical_dev.DeviceUID & ",
" & historical_dev.ActionUser & ", 
" & historical_dev.EQType & ", 
" & historical_dev.Status & ",
" & historical_dev.Trackable & ") 
VALUES(@" & historical_dev.ChangeType & ",
@" & historical_dev.Notes & ",
@" & historical_dev.Serial & ",
@" & historical_dev.Description & ",
@" & historical_dev.Location & ",
@" & historical_dev.CurrentUser & ",
@" & historical_dev.AssetTag & ",
@" & historical_dev.PurchaseDate & ", 
@" & historical_dev.ReplacementYear & ",
@" & historical_dev.PO & ",
@" & historical_dev.OSVersion & ", 
@" & historical_dev.DeviceUID & ",
@" & historical_dev.ActionUser & ", 
@" & historical_dev.EQType & ",
@" & historical_dev.Status & ",
@" & historical_dev.Trackable & ")"
            cmd.Parameters.Clear()
            cmd.Parameters.AddWithValue("@" & historical_dev.ChangeType, "NEWD")
            cmd.Parameters.AddWithValue("@" & historical_dev.Notes, DeviceInfo.strNote)
            cmd.Parameters.AddWithValue("@" & historical_dev.Serial, DeviceInfo.strSerial)
            cmd.Parameters.AddWithValue("@" & historical_dev.Description, DeviceInfo.strDescription)
            cmd.Parameters.AddWithValue("@" & historical_dev.Location, DeviceInfo.strLocation)
            cmd.Parameters.AddWithValue("@" & historical_dev.CurrentUser, DeviceInfo.strCurrentUser)
            cmd.Parameters.AddWithValue("@" & historical_dev.AssetTag, DeviceInfo.strAssetTag)
            cmd.Parameters.AddWithValue("@" & historical_dev.PurchaseDate, DeviceInfo.dtPurchaseDate)
            cmd.Parameters.AddWithValue("@" & historical_dev.ReplacementYear, DeviceInfo.strReplaceYear)
            cmd.Parameters.AddWithValue("@" & historical_dev.PO, DeviceInfo.strPO)
            cmd.Parameters.AddWithValue("@" & historical_dev.OSVersion, DeviceInfo.strOSVersion)
            cmd.Parameters.AddWithValue("@" & historical_dev.DeviceUID, strUID)
            cmd.Parameters.AddWithValue("@" & historical_dev.ActionUser, strLocalUser)
            cmd.Parameters.AddWithValue("@" & historical_dev.EQType, DeviceInfo.strEqType)
            cmd.Parameters.AddWithValue("@" & historical_dev.Status, DeviceInfo.strStatus)
            cmd.Parameters.AddWithValue("@" & historical_dev.Trackable, Convert.ToInt32(DeviceInfo.bolTrackable))
            cmd.CommandText = strSqlQry2
            rows = rows + cmd.ExecuteNonQuery()
            cmd.Dispose()
            If rows = 2 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
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
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
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
            ' Dim reader As MySqlDataReader
            Dim results As New DataTable
            Dim strDeviceID As String
            Dim strSQLIDQry As String
            If Type = Entry_Type.Device Then
                strSQLIDQry = "SELECT " & dev_attachments.FKey & " FROM " & dev_attachments.TableName & " WHERE " & dev_attachments.FileUID & "='" & AttachUID & "'"
            ElseIf Type = Entry_Type.Sibi Then
                strSQLIDQry = "SELECT " & sibi_attachments.FKey & " FROM " & sibi_attachments.TableName & " WHERE " & sibi_attachments.FileUID & "='" & AttachUID & "'"
            End If
            'reader = SQLComms.Return_SQLReader(strSQLIDQry)
            results = SQLComms.Return_SQLTable(strSQLIDQry)
            For Each r As DataRow In results.Rows
                If Type = Entry_Type.Device Then
                    strDeviceID = r.Item(dev_attachments.FKey)
                ElseIf Type = Entry_Type.Sibi Then
                    strDeviceID = r.Item(sibi_attachments.FKey)
                End If
            Next
            results.Dispose()
            'Delete FTP Attachment
            If FTP.DeleteFTPAttachment(AttachUID, strDeviceID) Then
                'delete SQL entry
                Dim strSQLDelQry As String
                If Type = Entry_Type.Device Then
                    strSQLDelQry = "DELETE FROM " & dev_attachments.TableName & " WHERE " & dev_attachments.FileUID & "='" & AttachUID & "'"
                ElseIf Type = Entry_Type.Sibi Then
                    strSQLDelQry = "DELETE FROM " & sibi_attachments.TableName & " WHERE " & sibi_attachments.FileUID & "='" & AttachUID & "'"
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
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
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
                    strQRY = "SELECT " & dev_attachments.FKey & " FROM " & dev_attachments.TableName & " WHERE " & dev_attachments.FKey & "='" & strGUID & "'"
                Case Entry_Type.Sibi
                    strQRY = "SELECT " & sibi_attachments.FKey & " FROM " & sibi_attachments.TableName & " WHERE " & sibi_attachments.FKey & "='" & strGUID & "'"
            End Select
            reader = SQLComms.Return_SQLReader(strQRY)
            Dim bolHasRows As Boolean = reader.HasRows
            reader.Close()
            Return bolHasRows
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
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
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
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
            Dim strQry = "SELECT * FROM " & historical_dev.TableName & " WHERE " & historical_dev.History_Entry_UID & "='" & strGUID & "'"
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
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Exit Try
            Else
                EndProgram()
            End If
        End Try
    End Function
    Public Function Get_DeviceUID(ByVal AssetTag As String, ByVal Serial As String) As String
        Dim reader As MySqlDataReader
        Dim UID As String
        Dim strQry = "SELECT " & devices.DeviceUID & " from " & devices.TableName & " WHERE " & devices.AssetTag & " = '" & AssetTag & "' AND " & devices.Serial & " = '" & Serial & "' ORDER BY " & devices.Input_DateTime & ""
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
                    strSQLQry = "DELETE FROM " & devices.TableName & " WHERE " & devices.DeviceUID & "='" & strGUID & "'"
                Case Entry_Type.Sibi
                    strSQLQry = "DELETE FROM " & sibi_requests.TableName & " WHERE " & sibi_requests.UID & "='" & strGUID & "'"
            End Select
            rows = SQLComms.Return_SQLCommand(strSQLQry).ExecuteNonQuery
            Return rows
            Exit Function
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                EndProgram()
            End If
        End Try
    End Function
    Public Function BuildIndex(CodeType As String, TypeName As String) As Combo_Data()
        Try
            Dim tmpArray() As Combo_Data
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM " & CodeType & " WHERE type_name ='" & TypeName & "' ORDER BY " & main_combocodes.HumanValue & ""
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
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Return Nothing
            Else
                EndProgram()
            End If
        End Try
    End Function
    Public Function BuildModuleIndex() As List(Of Access_Info)
        Dim tmpList As New List(Of Access_Info)
        Dim ModuleTable As DataTable = SQLComms.Return_SQLTable("SELECT * FROM " & security.TableName & " ORDER BY " & security.AccessLevel & "")
        Dim i As Integer = 0
        'ReDim ModuleArray(ModuleTable.Rows.Count - 1)
        For Each row As DataRow In ModuleTable.Rows
            Dim tmpInfo As Access_Info
            With tmpInfo
                .intLevel = row.Item(security.AccessLevel)
                .strModule = row.Item(security.SecModule)
                .strDesc = row.Item(security.Description)
            End With
            tmpList.Add(tmpInfo)
        Next
        Return tmpList
    End Function
    Public Sub UpdateUser(ByRef User As User_Info, AccessLevel As Integer)
        User.intAccessLevel = AccessLevel
        Update_SQLValue(users.TableName, users.AccessLevel, User.intAccessLevel, users.UID, User.strUID)
    End Sub
    Public Function FindDevice(Optional AssetTag As String = "", Optional Serial As String = "") As Device_Info
        If AssetTag IsNot "" Then
            Return CollectDeviceInfo(SQLComms.Return_SQLTable("SELECT * FROM " & devices.TableName & " WHERE " & devices.AssetTag & "='" & AssetTag & "'"))
        ElseIf Serial IsNot "" Then
            Return CollectDeviceInfo(SQLComms.Return_SQLTable("SELECT * FROM " & devices.TableName & " WHERE " & devices.Serial & "='" & Serial & "'"))
        End If
    End Function
    Public Sub AddNewEmp(EmpInfo As Emp_Info)
        Try
            If Not EmpIsInDB(EmpInfo.Number) Then
                Dim UID As String = Guid.NewGuid.ToString
                Dim strQRY As String = "INSERT INTO " & employees.TableName & "
(" & employees.Name & ",
" & employees.Number & ",
" & employees.UID & ")
VALUES
(@" & employees.Name & ",
@" & employees.Number & ",
@" & employees.UID & ")"
                Dim cmd As MySqlCommand = SQLComms.Return_SQLCommand(strQRY)
                cmd.Parameters.AddWithValue("@" & employees.Name, EmpInfo.Name)
                cmd.Parameters.AddWithValue("@" & employees.Number, EmpInfo.Number)
                cmd.Parameters.AddWithValue("@" & employees.UID, UID)
                cmd.ExecuteNonQuery()
                cmd.Dispose()
            End If
        Catch ex As MySqlException
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Function EmpIsInDB(EmpNum As String) As Boolean
        Dim EmpName As String = Get_SQLValue(employees.TableName, employees.Number, EmpNum, employees.Name)
        If EmpName <> "" Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Sub GetUserAccess()
        Try
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM " & users.TableName & " WHERE " & users.UserName & "='" & strLocalUser & "'"
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
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Sub GetAccessLevels()
        Try
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM " & security.TableName & " ORDER BY " & security.AccessLevel & "" ' WHERE usr_username='" & strLocalUser & "'"
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
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
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
            Return ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
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
            Dim strQRY As String = "SELECT * FROM " & devices.TableName & " WHERE " & devices.Munis_Emp_Num & "='" & r.Item("a_employee_number") & "'"
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
                .strGUID = NoNull(DeviceTable.Rows(0).Item(devices.DeviceUID))
                .strDescription = NoNull(DeviceTable.Rows(0).Item(devices.Description))
                .strLocation = NoNull(DeviceTable.Rows(0).Item(devices.Location))
                .strCurrentUser = NoNull(DeviceTable.Rows(0).Item(devices.CurrentUser))
                .strCurrentUserEmpNum = NoNull(DeviceTable.Rows(0).Item(devices.Munis_Emp_Num))
                .strSerial = NoNull(DeviceTable.Rows(0).Item(devices.Serial))
                .strAssetTag = NoNull(DeviceTable.Rows(0).Item(devices.AssetTag))
                .dtPurchaseDate = NoNull(DeviceTable.Rows(0).Item(devices.PurchaseDate))
                .strReplaceYear = NoNull(DeviceTable.Rows(0).Item(devices.ReplacementYear))
                .strPO = NoNull(DeviceTable.Rows(0).Item(devices.PO))
                .strOSVersion = NoNull(DeviceTable.Rows(0).Item(devices.OSVersion))
                .strEqType = NoNull(DeviceTable.Rows(0).Item(devices.EQType))
                .strStatus = NoNull(DeviceTable.Rows(0).Item(devices.Status))
                .bolTrackable = CBool(DeviceTable.Rows(0).Item(devices.Trackable))
                .strSibiLink = NoNull(DeviceTable.Rows(0).Item(devices.Sibi_Link_UID))
                .Tracking.bolCheckedOut = CBool(DeviceTable.Rows(0).Item(devices.CheckedOut))
            End With
            Return newDeviceInfo
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function User_GetUserList() As List(Of User_Info)
        Dim Qry As String = "SELECT * FROM " & users.TableName
        Dim tmpList As New List(Of User_Info)
        Try
            Dim Results As DataTable = SQLComms.Return_SQLTable(Qry)
            For Each r As DataRow In Results.Rows
                Dim tmpItem As User_Info
                tmpItem.strUsername = r.Item(users.UserName)
                tmpItem.strFullname = r.Item(users.FullName)
                tmpItem.intAccessLevel = r.Item(users.AccessLevel)
                tmpItem.strUID = r.Item(users.UID)
                tmpList.Add(tmpItem)
            Next
            Return tmpList
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Function
    Public Function CheckConnection() As Boolean
        Try
            Dim ds As New DataSet
            Dim da As New MySqlDataAdapter
            Dim rows As Integer
            Dim result As String
            da.SelectCommand = New MySqlCommand("SELECT NOW()")
            da.SelectCommand.Connection = GlobalConn
            da.Fill(ds)
            rows = ds.Tables(0).Rows.Count
            result = ds.Tables(0).Rows(0).Item(0).ToString
            da.Dispose()
            ds.Dispose()
            If result <> "" Then Return True
            Return False
        Catch
            Return False
        End Try
    End Function
End Class