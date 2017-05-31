Imports MySql.Data.MySqlClient
Public Class clsAssetManager_Functions

    Public Function Get_EmptyTable(Table As String) As DataTable
        Using SQLComms As New clsMySQL_Comms
            Return SQLComms.Return_SQLTable(Table) '"SELECT * FROM " & Table & " LIMIT 0")
        End Using
    End Function
    Public Function DeviceExists(AssetTag As String, Serial As String) As Boolean
        Dim bolAsset As Boolean
        Dim bolSerial As Boolean
        Try
            If AssetTag = "NA" Then 'Allow NA value because users do not always have an Asset Tag for new devices.
                bolAsset = False
            Else
                Dim CheckAsset As String = Get_SQLValue(devices.TableName, devices.AssetTag, AssetTag, devices.AssetTag)
                If CheckAsset <> "" Then
                    bolAsset = True
                Else
                    bolAsset = False
                End If
            End If

            Dim CheckSerial As String = Get_SQLValue(devices.TableName, devices.Serial, Serial, devices.Serial)
            If CheckSerial <> "" Then
                bolSerial = True
            Else
                bolSerial = False
            End If
            If bolSerial Or bolAsset Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            Throw ex
            Return False
        End Try
    End Function

    Public Function Update_SQLValue(table As String, fieldIN As String, valueIN As String, idField As String, idValue As String) As Integer
        Try
            Dim sqlUpdateQry As String = "UPDATE " & table & " SET " & fieldIN & "=@ValueIN  WHERE " & idField & "='" & idValue & "'"
            Using SQLComms As New clsMySQL_Comms, cmd As MySqlCommand = SQLComms.Return_SQLCommand(sqlUpdateQry)
                cmd.Parameters.AddWithValue("@ValueIN", valueIN)
                Return cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
    Public Function DeleteSQLAttachment(AttachUID As String, Type As Entry_Type) As Integer
        Try
            Dim rows As Integer
            Dim strDeviceID As String = ""
            Dim strSQLIDQry As String = ""
            If Type = Entry_Type.Device Then
                strSQLIDQry = "SELECT " & dev_attachments.FKey & " FROM " & dev_attachments.TableName & " WHERE " & dev_attachments.FileUID & "='" & AttachUID & "'"
            ElseIf Type = Entry_Type.Sibi Then
                strSQLIDQry = "SELECT " & sibi_attachments.FKey & " FROM " & sibi_attachments.TableName & " WHERE " & sibi_attachments.FileUID & "='" & AttachUID & "'"
            End If
            Using SQLComms As New clsMySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strSQLIDQry)
                For Each r As DataRow In results.Rows
                    If Type = Entry_Type.Device Then
                        strDeviceID = r.Item(dev_attachments.FKey).ToString
                    ElseIf Type = Entry_Type.Sibi Then
                        strDeviceID = r.Item(sibi_attachments.FKey).ToString
                    End If
                Next
                'Delete FTP Attachment
                If FTP.DeleteFTPAttachment(AttachUID, strDeviceID) Then
                    'delete SQL entry
                    Dim strSQLDelQry As String = ""
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
            End Using
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
                Exit Try
            Else
                EndProgram()
            End If
        End Try
        Return -1
    End Function
    Public Function Has_Attachments(strGUID As String, Type As Entry_Type) As Boolean
        Try
            Dim strQRY As String = ""
            Select Case Type
                Case Entry_Type.Device
                    strQRY = "SELECT " & dev_attachments.FKey & " FROM " & dev_attachments.TableName & " WHERE " & dev_attachments.FKey & "='" & strGUID & "'"
                Case Entry_Type.Sibi
                    strQRY = "SELECT " & sibi_attachments.FKey & " FROM " & sibi_attachments.TableName & " WHERE " & sibi_attachments.FKey & "='" & strGUID & "'"
            End Select
            Using SQLComms As New clsMySQL_Comms, reader As MySqlDataReader = SQLComms.Return_SQLReader(strQRY)
                Dim bolHasRows As Boolean = reader.HasRows
                reader.Close()
                Return bolHasRows
            End Using
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
            Else
                EndProgram()
            End If
            Return Nothing
        End Try
    End Function
    Public Function Get_SQLValue(table As String, fieldIN As String, valueIN As String, fieldOUT As String) As String
        Dim sqlQRY As String = "SELECT " & fieldOUT & " FROM " & table & " WHERE " & fieldIN & " = '" & valueIN & "' LIMIT 1"
        Try
            Using SQLComms As New clsMySQL_Comms, cmd As MySqlCommand = SQLComms.Return_SQLCommand(sqlQRY)
                Return Convert.ToString(cmd.ExecuteScalar)
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Throw ex
            Return Nothing
        End Try
    End Function
    Public Function Get_DeviceInfo_From_UID(GUID As String) As Device_Info
        Using SQLComms As New clsMySQL_Comms
            Return Asset.CollectDeviceInfo(SQLComms.Return_SQLTable("SELECT * FROM " & devices.TableName & " WHERE " & devices.DeviceUID & "='" & GUID & "'"))
        End Using
    End Function
    Public Function Get_EntryInfo(ByVal strGUID As String) As Device_Info
        Try
            Dim tmpInfo As New Device_Info
            Dim strQry = "SELECT * FROM " & historical_dev.TableName & " WHERE " & historical_dev.History_Entry_UID & "='" & strGUID & "'"
            Using SQLComms As New clsMySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strQry)
                For Each r As DataRow In results.Rows
                    tmpInfo.Historical.strChangeType = GetHumanValue(DeviceIndex.ChangeType, r.Item(historical_dev.ChangeType).ToString)
                    tmpInfo.strAssetTag = r.Item(historical_dev.AssetTag).ToString
                    tmpInfo.strCurrentUser = r.Item(historical_dev.CurrentUser).ToString
                    tmpInfo.strSerial = r.Item(historical_dev.Serial).ToString
                    tmpInfo.strDescription = r.Item(historical_dev.Description).ToString
                    tmpInfo.Historical.dtActionDateTime = DateTime.Parse(r.Item(historical_dev.ActionDateTime).ToString)
                    tmpInfo.Historical.strActionUser = r.Item(historical_dev.ActionUser).ToString
                Next
                Return tmpInfo
            End Using
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
            Else
                EndProgram()
            End If
            Return Nothing
        End Try
    End Function
    Public Function Get_DeviceUID(ByVal AssetTag As String, ByVal Serial As String) As String
        Dim UID As String
        Dim strQry = "SELECT " & devices.DeviceUID & " from " & devices.TableName & " WHERE " & devices.AssetTag & " = '" & AssetTag & "' AND " & devices.Serial & " = '" & Serial & "' ORDER BY " & devices.Input_DateTime & ""
        Using SQLComms As New clsMySQL_Comms, cmd As MySqlCommand = SQLComms.Return_SQLCommand(strQry)
            UID = cmd.ExecuteScalar.ToString
            Return UID
        End Using
    End Function
    Public Function Delete_SQLMasterEntry(ByVal strGUID As String, Type As Entry_Type) As Boolean
        Try
            Dim rows As Integer
            Dim strSQLQry As String = ""
            Select Case Type
                Case Entry_Type.Device
                    strSQLQry = "DELETE FROM " & devices.TableName & " WHERE " & devices.DeviceUID & "='" & strGUID & "'"
                Case Entry_Type.Sibi
                    strSQLQry = "DELETE FROM " & sibi_requests.TableName & " WHERE " & sibi_requests.UID & "='" & strGUID & "'"
            End Select
            Using SQLComms As New clsMySQL_Comms
                rows = SQLComms.Return_SQLCommand(strSQLQry).ExecuteNonQuery
                If rows > 0 Then
                    Return True
                End If
            End Using
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
                Return False
            Else
                EndProgram()
            End If
        End Try
        Return False
    End Function
    Public Function BuildIndex(CodeType As String, TypeName As String) As Combo_Data()
        Try
            Dim tmpArray() As Combo_Data
            Dim strQRY = "SELECT * FROM {OJ " & CodeType & " LEFT OUTER JOIN munis_codes on " & CodeType & ".db_value = munis_codes.asset_man_code} WHERE type_name ='" & TypeName & "' ORDER BY " & main_combocodes.HumanValue & ""
            Dim row As Integer
            Using SQLComms As New clsMySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strQRY)
                ReDim tmpArray(0)
                row = -1
                For Each r As DataRow In results.Rows
                    row += 1
                    ReDim Preserve tmpArray(row)
                    tmpArray(row).strID = r.Item(main_combocodes.ID).ToString
                    If r.Table.Columns.Contains("munis_code") Then
                        If Not IsDBNull(r.Item("munis_code")) Then
                            tmpArray(row).strLong = r.Item(main_combocodes.HumanValue).ToString & " - " & r.Item("munis_code").ToString
                        Else
                            tmpArray(row).strLong = r.Item(main_combocodes.HumanValue).ToString
                        End If
                    Else
                        tmpArray(row).strLong = r.Item(main_combocodes.HumanValue).ToString
                    End If
                    tmpArray(row).strShort = r.Item(main_combocodes.DB_Value).ToString
                Next
                Return tmpArray
            End Using
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
                Return Nothing
            Else
                EndProgram()
            End If
        End Try
        Return Nothing
    End Function
    Public Function BuildModuleIndex() As List(Of Access_Info)
        Dim tmpList As New List(Of Access_Info)
        Using SQLComms As New clsMySQL_Comms,
            ModuleTable As DataTable = SQLComms.Return_SQLTable("SELECT * FROM " & security.TableName & " ORDER BY " & security.AccessLevel & "")
            For Each row As DataRow In ModuleTable.Rows
                Dim tmpInfo As Access_Info
                With tmpInfo
                    .intLevel = CInt(row.Item(security.AccessLevel))
                    .strModule = row.Item(security.SecModule).ToString
                    .strDesc = row.Item(security.Description).ToString
                End With
                tmpList.Add(tmpInfo)
            Next
            Return tmpList
        End Using
    End Function
    Public Sub UpdateUser(ByRef User As User_Info, AccessLevel As Integer)
        User.intAccessLevel = AccessLevel
        Update_SQLValue(users.TableName, users.AccessLevel, User.intAccessLevel.ToString, users.UID, User.strUID)
    End Sub
    Public Function FindDevice(SearchVal As String, Type As FindDevType) As Device_Info
        Using SQLComms As New clsMySQL_Comms
            If Type = FindDevType.AssetTag Then
                Return CollectDeviceInfo(SQLComms.Return_SQLTable("SELECT * FROM " & devices.TableName & " WHERE " & devices.AssetTag & "='" & SearchVal & "'"))
            ElseIf Type = FindDevType.Serial Then
                Return CollectDeviceInfo(SQLComms.Return_SQLTable("SELECT * FROM " & devices.TableName & " WHERE " & devices.Serial & "='" & SearchVal & "'"))
            End If
        End Using
        Return Nothing
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
                Using SQLComms As New clsMySQL_Comms, cmd As MySqlCommand = SQLComms.Return_SQLCommand(strQRY)
                    cmd.Parameters.AddWithValue("@" & employees.Name, EmpInfo.Name)
                    cmd.Parameters.AddWithValue("@" & employees.Number, EmpInfo.Number)
                    cmd.Parameters.AddWithValue("@" & employees.UID, UID)
                    cmd.ExecuteNonQuery()
                End Using
            End If
        Catch ex As MySqlException
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
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
            Dim strQRY = "SELECT * FROM " & users.TableName & " WHERE " & users.UserName & "='" & strLocalUser & "'"
            Using SQLComms As New clsMySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strQRY)
                If results.Rows.Count > 0 Then
                    For Each r As DataRow In results.Rows
                        UserAccess.strUsername = r.Item(users.UserName).ToString
                        UserAccess.strFullname = r.Item(users.FullName).ToString
                        UserAccess.intAccessLevel = CInt(r.Item(users.AccessLevel))
                        UserAccess.strUID = r.Item(users.UID).ToString
                    Next
                Else
                    UserAccess.intAccessLevel = 0
                End If
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Public Sub GetAccessLevels()
        Try
            Dim strQRY = "SELECT * FROM " & security.TableName & " ORDER BY " & security.AccessLevel & "" ' WHERE usr_username='" & strLocalUser & "'"
            Dim rows As Integer
            Using SQLComms As New clsMySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strQRY)
                ReDim AccessLevels(0)
                rows = -1
                For Each r As DataRow In results.Rows
                    rows += 1
                    ReDim Preserve AccessLevels(rows)
                    AccessLevels(rows).intLevel = CInt(r.Item(security.AccessLevel))
                    AccessLevels(rows).strModule = r.Item(security.SecModule).ToString
                    AccessLevels(rows).strDesc = r.Item(security.Description).ToString
                Next
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Public Function DeleteMaster(ByVal strGUID As String, Type As Entry_Type) As Boolean
        Try
            If FTP.Has_FTPFolder(strGUID) Then
                If FTP.DeleteFTPFolder(strGUID, Type) Then Return Delete_SQLMasterEntry(strGUID, Type) ' if has attachments, delete ftp directory, then delete the sql records.
            Else
                Return Delete_SQLMasterEntry(strGUID, Type) 'delete sql records
            End If
        Catch ex As Exception
            Return ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
        Return False
    End Function
    Public Function DevicesBySup(ParentForm As Form) As DataTable
        Dim SupInfo As Emp_Info
        Dim NewMunisSearch As New frmMunisUser(ParentForm)
        If NewMunisSearch.DialogResult = DialogResult.Yes Then
            SupInfo = NewMunisSearch.EmployeeInfo
            Dim EmpList As DataTable = Munis.ListOfEmpBySup(SupInfo.Number)
            Dim DeviceList As New DataTable
            Using SQLComms As New clsMySQL_Comms
                For Each r As DataRow In EmpList.Rows
                    Dim strQRY As String = "SELECT * FROM " & devices.TableName & " WHERE " & devices.Munis_Emp_Num & "='" & r.Item("a_employee_number").ToString & "'"
                    Using tmpTable As DataTable = SQLComms.Return_SQLTable(strQRY)
                        DeviceList.Merge(tmpTable)
                    End Using
                Next
                Return DeviceList
            End Using
        Else
            Return Nothing
        End If
    End Function
    Public Function CollectDeviceInfo(DeviceTable As DataTable) As Device_Info
        Try
            Dim newDeviceInfo As New Device_Info
            With newDeviceInfo
                .strGUID = NoNull(DeviceTable.Rows(0).Item(devices.DeviceUID))
                .strDescription = NoNull(DeviceTable.Rows(0).Item(devices.Description))
                .strLocation = NoNull(DeviceTable.Rows(0).Item(devices.Location))
                .strCurrentUser = NoNull(DeviceTable.Rows(0).Item(devices.CurrentUser))
                .strCurrentUserEmpNum = NoNull(DeviceTable.Rows(0).Item(devices.Munis_Emp_Num))
                .strSerial = NoNull(DeviceTable.Rows(0).Item(devices.Serial))
                .strAssetTag = NoNull(DeviceTable.Rows(0).Item(devices.AssetTag))
                .dtPurchaseDate = DateTime.Parse(NoNull(DeviceTable.Rows(0).Item(devices.PurchaseDate)))
                .strReplaceYear = NoNull(DeviceTable.Rows(0).Item(devices.ReplacementYear))
                .strPO = NoNull(DeviceTable.Rows(0).Item(devices.PO))
                .strOSVersion = NoNull(DeviceTable.Rows(0).Item(devices.OSVersion))
                .strPhoneNumber = NoNull(DeviceTable.Rows(0).Item(devices.PhoneNumber))
                .strEqType = NoNull(DeviceTable.Rows(0).Item(devices.EQType))
                .strStatus = NoNull(DeviceTable.Rows(0).Item(devices.Status))
                .bolTrackable = CBool(DeviceTable.Rows(0).Item(devices.Trackable))
                .strSibiLink = NoNull(DeviceTable.Rows(0).Item(devices.Sibi_Link_UID))
                .Tracking.bolCheckedOut = CBool(DeviceTable.Rows(0).Item(devices.CheckedOut))
                .CheckSum = NoNull(DeviceTable.Rows(0).Item(devices.CheckSum))
            End With
            Return newDeviceInfo
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
    Public Function User_GetUserList() As List(Of User_Info)
        Dim Qry As String = "SELECT * FROM " & users.TableName
        Dim tmpList As New List(Of User_Info)
        Try
            Using SQLComms As New clsMySQL_Comms, Results As DataTable = SQLComms.Return_SQLTable(Qry)
                For Each r As DataRow In Results.Rows
                    Dim tmpItem As User_Info
                    tmpItem.strUsername = r.Item(users.UserName).ToString
                    tmpItem.strFullname = r.Item(users.FullName).ToString
                    tmpItem.intAccessLevel = CInt(r.Item(users.AccessLevel))
                    tmpItem.strUID = r.Item(users.UID).ToString
                    tmpList.Add(tmpItem)
                Next
                Return tmpList
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
        Return Nothing
    End Function
    Public Function GetAttachmentCount(AttachInfo As Object) As Integer
        Try
            Dim strQRY As String = ""
            If TypeOf AttachInfo Is Device_Info Then
                Dim Dev As Device_Info = DirectCast(AttachInfo, Device_Info)
                strQRY = "SELECT COUNT(*) FROM " & dev_attachments.TableName & " WHERE " & dev_attachments.FKey & "='" & Dev.strGUID & "'"
            ElseIf TypeOf AttachInfo Is Request_Info Then
                Dim Req As Request_Info = DirectCast(AttachInfo, Request_Info)
                strQRY = "SELECT COUNT(*) FROM " & sibi_attachments.TableName & " WHERE " & sibi_attachments.FKey & "='" & Req.strUID & "'"
            End If
            Using SQLComms As New clsMySQL_Comms, cmd As MySqlCommand = SQLComms.Return_SQLCommand(strQRY)
                Return CInt(cmd.ExecuteScalar)
            End Using
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()) Then
            Else
                EndProgram()
            End If
            Return Nothing
        End Try
    End Function
End Class
