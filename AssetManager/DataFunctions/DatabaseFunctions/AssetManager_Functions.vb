Imports MySql.Data.MySqlClient

Public Class AssetManager_Functions

#Region "Methods"

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
                Using SQLComms As New MySQL_Comms, cmd As MySqlCommand = SQLComms.Return_SQLCommand(strQRY)
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

    Public Function BuildIndex(CodeType As String, TypeName As String) As Combo_Data()
        Try
            Dim tmpArray() As Combo_Data
            'Dim strQRY = "SELECT * FROM {OJ " & CodeType & " LEFT OUTER JOIN munis_codes on " & CodeType & ".db_value = munis_codes.asset_man_code} WHERE type_name ='" & TypeName & "' ORDER BY " & main_combocodes.HumanValue & ""
            Dim strQRY = "SELECT * FROM " & CodeType & " LEFT OUTER JOIN munis_codes on " & CodeType & ".db_value = munis_codes.asset_man_code WHERE type_name ='" & TypeName & "' ORDER BY " & main_combocodes.HumanValue & ""
            Dim row As Integer
            Using results As DataTable = DBFunc.DataTableFromQueryString(strQRY)
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
        Using SQLComms As New MySQL_Comms,
            ModuleTable As DataTable = SQLComms.Return_SQLTable("SELECT * FROM " & security_columns.TableName & " ORDER BY " & security_columns.AccessLevel & "")
            For Each row As DataRow In ModuleTable.Rows
                Dim tmpInfo As Access_Info
                With tmpInfo
                    .intLevel = CInt(row.Item(security_columns.AccessLevel))
                    .strModule = row.Item(security_columns.SecModule).ToString
                    .strDesc = row.Item(security_columns.Description).ToString
                End With
                tmpList.Add(tmpInfo)
            Next
            Return tmpList
        End Using
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
            Using SQLComms As New MySQL_Comms
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

    Public Function DeleteMaster(ByVal strGUID As String, Type As Entry_Type) As Boolean
        Try
            If FTPFunc.Has_FTPFolder(strGUID) Then
                If FTPFunc.DeleteFTPFolder(strGUID) Then Return Delete_SQLMasterEntry(strGUID, Type) ' if has attachments, delete ftp directory, then delete the sql records.
            Else
                Return Delete_SQLMasterEntry(strGUID, Type) 'delete sql records
            End If
        Catch ex As Exception
            Return ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
        Return False
    End Function

    Public Function DeleteSQLAttachment(Attachment As Attachment) As Integer 'AttachUID As String, AttachTable As main_attachments) As Integer 'TODO: Change this to use Attachment class as propery
        Try
            Dim rows As Integer
            Dim strDeviceID As String = ""
            Dim strSQLIDQry As String = ""
            strSQLIDQry = "SELECT " & Attachment.AttachTable.FKey & " FROM " & Attachment.AttachTable.TableName & " WHERE " & Attachment.AttachTable.FileUID & "='" & Attachment.FileUID & "'"
            Using SQLComms As New MySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strSQLIDQry)
                For Each r As DataRow In results.Rows
                    strDeviceID = r.Item(Attachment.AttachTable.FKey).ToString
                Next
                'Delete FTP Attachment
                If FTPFunc.DeleteFTPAttachment(Attachment.FileUID, strDeviceID) Then
                    'delete SQL entry
                    Dim strSQLDelQry As String = ""
                    strSQLDelQry = "DELETE FROM " & Attachment.AttachTable.TableName & " WHERE " & Attachment.AttachTable.FileUID & "='" & Attachment.FileUID & "'"
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

    Public Function DevicesBySup(ParentForm As Form) As DataTable
        Dim SupInfo As Emp_Info
        Using NewMunisSearch As New MunisUserForm(ParentForm)
            If NewMunisSearch.DialogResult = DialogResult.Yes Then
                SetWaitCursor(True)
                SupInfo = NewMunisSearch.EmployeeInfo
                Dim EmpList As DataTable = MunisFunc.ListOfEmpBySup(SupInfo.Number)
                Dim DeviceList As New DataTable
                Using SQLComms As New MySQL_Comms
                    For Each r As DataRow In EmpList.Rows
                        Dim strQRY As String = "SELECT * FROM " & devices.TableName & " WHERE " & devices.Munis_Emp_Num & "='" & r.Item("a_employee_number").ToString & "'"
                        Using tmpTable As DataTable = SQLComms.Return_SQLTable(strQRY)
                            DeviceList.Merge(tmpTable)
                        End Using
                    Next
                    Return DeviceList
                End Using
                SetWaitCursor(False)
            Else
                SetWaitCursor(False)
                Return Nothing
            End If
        End Using
    End Function

    Public Function EmpIsInDB(EmpNum As String) As Boolean
        Dim EmpName As String = Get_SQLValue(employees.TableName, employees.Number, EmpNum, employees.Name)
        If EmpName <> "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function FindDevice(SearchVal As String, Type As FindDevType) As Device_Info
        Try
            If Type = FindDevType.AssetTag Then
                Return CollectDeviceInfo(DBFunc.DataTableFromQueryString("SELECT * FROM " & devices.TableName & " WHERE " & devices.AssetTag & "='" & SearchVal & "'"))
            ElseIf Type = FindDevType.Serial Then
                Return CollectDeviceInfo(DBFunc.DataTableFromQueryString("SELECT * FROM " & devices.TableName & " WHERE " & devices.Serial & "='" & SearchVal & "'"))
            End If
            Return Nothing
        Catch ex As MySqlException
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function

    Public Function Get_DeviceInfo_From_UID(GUID As String) As Device_Info
        Using SQLComms As New MySQL_Comms
            Return AssetFunc.CollectDeviceInfo(SQLComms.Return_SQLTable("SELECT * FROM " & devices.TableName & " WHERE " & devices.DeviceUID & "='" & GUID & "'"))
        End Using
    End Function

    Public Function Get_DeviceUID(ByVal AssetTag As String, ByVal Serial As String) As String
        Dim UID As String
        Dim strQry = "SELECT " & devices.DeviceUID & " from " & devices.TableName & " WHERE " & devices.AssetTag & " = '" & AssetTag & "' AND " & devices.Serial & " = '" & Serial & "' ORDER BY " & devices.Input_DateTime & ""
        Using SQLComms As New MySQL_Comms, cmd As MySqlCommand = SQLComms.Return_SQLCommand(strQry)
            UID = cmd.ExecuteScalar.ToString
            Return UID
        End Using
    End Function

    Public Function Get_EntryInfo(ByVal strGUID As String) As Device_Info
        Try
            Dim tmpInfo As New Device_Info
            Dim strQry = "SELECT * FROM " & historical_dev.TableName & " WHERE " & historical_dev.History_Entry_UID & "='" & strGUID & "'"
            Using SQLComms As New MySQL_Comms, results As DataTable = SQLComms.Return_SQLTable(strQry)
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

    Public Function Get_MunisCode_From_AssetCode(AssetCode As String) As String
        Return Get_SQLValue("munis_codes", "asset_man_code", AssetCode, "munis_code")
    End Function
    Public Function Get_SQLValue(table As String, fieldIN As String, valueIN As String, fieldOUT As String) As String
        Dim sqlQRY As String = "SELECT " & fieldOUT & " FROM " & table & " WHERE " & fieldIN & " = '" & valueIN & "' LIMIT 1"
        Try
            Dim Result = DBFunc.ExecuteScalarFromQueryString(sqlQRY)
            If Result IsNot Nothing Then
                Return Result.ToString
            Else
                Return ""
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Throw ex
            Return Nothing
        End Try
    End Function

    Public Function GetAttachmentCount(AttachFolderUID As String, AttachTable As main_attachments) As Integer
        Try
            Dim strQRY As String = ""
            strQRY = "SELECT COUNT(*) FROM " & AttachTable.TableName & " WHERE " & AttachTable.FKey & "='" & AttachFolderUID & "'"
            Using SQLComms As New MySQL_Comms, cmd As MySqlCommand = SQLComms.Return_SQLCommand(strQRY)
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

    Public Function Update_SQLValue(table As String, fieldIN As String, valueIN As String, idField As String, idValue As String) As Integer
        Try
            Dim sqlUpdateQry As String = "UPDATE " & table & " SET " & fieldIN & "=@ValueIN  WHERE " & idField & "='" & idValue & "'"
            Using SQLComms As New MySQL_Comms, cmd As MySqlCommand = SQLComms.Return_SQLCommand(sqlUpdateQry)
                cmd.Parameters.AddWithValue("@ValueIN", valueIN)
                Return cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
    Public Function User_GetUserList() As List(Of User_Info)
        Dim Qry As String = "SELECT * FROM " & users.TableName
        Dim tmpList As New List(Of User_Info)
        Try
            Using SQLComms As New MySQL_Comms, Results As DataTable = SQLComms.Return_SQLTable(Qry)
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

#End Region

End Class