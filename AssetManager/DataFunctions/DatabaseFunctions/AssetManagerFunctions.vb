Imports MySql.Data.MySqlClient
Imports System.Data.Common

Public Class AssetManagerFunctions

#Region "Methods"

    Public Sub AddNewEmp(empInfo As MunisEmployeeStruct)
        Try
            If Not IsEmployeeInDB(empInfo.Number) Then
                Dim UID As String = Guid.NewGuid.ToString
                Dim InsertEmployeeParams As New List(Of DBParameter)
                InsertEmployeeParams.Add(New DBParameter(EmployeesCols.Name, empInfo.Name))
                InsertEmployeeParams.Add(New DBParameter(EmployeesCols.Number, empInfo.Number))
                InsertEmployeeParams.Add(New DBParameter(EmployeesCols.UID, UID))
                DBFunc.GetDatabase.InsertFromParameters(EmployeesCols.TableName, InsertEmployeeParams)
            End If
        Catch ex As MySqlException
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Public Function BuildIndex(codeType As String, typeName As String) As ComboboxDataStruct()
        Try
            Dim strQRY = "SELECT * FROM " & codeType & " LEFT OUTER JOIN munis_codes on " & codeType & ".db_value = munis_codes.asset_man_code WHERE type_name ='" & typeName & "' ORDER BY " & ComboCodesBaseCols.HumanValue & ""
            Dim row As Integer = 0
            Using results As DataTable = DBFunc.GetDatabase.DataTableFromQueryString(strQRY) 'DBFunc.GetDatabase.DataTableFromQueryString(strQRY)
                Dim tmpArray(results.Rows.Count - 1) As ComboboxDataStruct
                For Each r As DataRow In results.Rows
                    tmpArray(row).ID = r.Item(ComboCodesBaseCols.ID).ToString
                    If r.Table.Columns.Contains("munis_code") Then
                        If Not IsDBNull(r.Item("munis_code")) Then
                            tmpArray(row).HumanReadable = r.Item(ComboCodesBaseCols.HumanValue).ToString & " - " & r.Item("munis_code").ToString
                        Else
                            tmpArray(row).HumanReadable = r.Item(ComboCodesBaseCols.HumanValue).ToString
                        End If
                    Else
                        tmpArray(row).HumanReadable = r.Item(ComboCodesBaseCols.HumanValue).ToString
                    End If
                    tmpArray(row).Code = r.Item(ComboCodesBaseCols.DBValue).ToString
                    row += 1
                Next
                Return tmpArray
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

    Public Function BuildModuleIndex() As List(Of AccessGroupStruct)
        Dim tmpList As New List(Of AccessGroupStruct)
        Using SQLComms As New MySqlComms,
            ModuleTable As DataTable = SQLComms.ReturnMySqlTable("SELECT * FROM " & SecurityCols.TableName & " ORDER BY " & SecurityCols.AccessLevel & "")
            For Each row As DataRow In ModuleTable.Rows
                Dim tmpInfo As AccessGroupStruct
                With tmpInfo
                    .Level = CInt(row.Item(SecurityCols.AccessLevel))
                    .AccessModule = row.Item(SecurityCols.SecModule).ToString
                    .Description = row.Item(SecurityCols.Description).ToString
                End With
                tmpList.Add(tmpInfo)
            Next
            Return tmpList
        End Using
    End Function

    Public Function CollectDeviceInfo(deviceTable As DataTable) As DeviceStruct
        Try
            Dim newDeviceInfo As New DeviceStruct
            With newDeviceInfo
                .GUID = NoNull(deviceTable.Rows(0).Item(DevicesCols.DeviceUID))
                .Description = NoNull(deviceTable.Rows(0).Item(DevicesCols.Description))
                .Location = NoNull(deviceTable.Rows(0).Item(DevicesCols.Location))
                .CurrentUser = NoNull(deviceTable.Rows(0).Item(DevicesCols.CurrentUser))
                .CurrentUserEmpNum = NoNull(deviceTable.Rows(0).Item(DevicesCols.MunisEmpNum))
                .Serial = NoNull(deviceTable.Rows(0).Item(DevicesCols.Serial))
                .AssetTag = NoNull(deviceTable.Rows(0).Item(DevicesCols.AssetTag))
                .PurchaseDate = DateTime.Parse(NoNull(deviceTable.Rows(0).Item(DevicesCols.PurchaseDate)))
                .ReplaceYear = NoNull(deviceTable.Rows(0).Item(DevicesCols.ReplacementYear))
                .PO = NoNull(deviceTable.Rows(0).Item(DevicesCols.PO))
                .OSVersion = NoNull(deviceTable.Rows(0).Item(DevicesCols.OSVersion))
                .PhoneNumber = NoNull(deviceTable.Rows(0).Item(DevicesCols.PhoneNumber))
                .EquipmentType = NoNull(deviceTable.Rows(0).Item(DevicesCols.EQType))
                .Status = NoNull(deviceTable.Rows(0).Item(DevicesCols.Status))
                .IsTrackable = CBool(deviceTable.Rows(0).Item(DevicesCols.Trackable))
                .SibiLink = NoNull(deviceTable.Rows(0).Item(DevicesCols.SibiLinkUID))
                .Tracking.IsCheckedOut = CBool(deviceTable.Rows(0).Item(DevicesCols.CheckedOut))
                .Checksum = NoNull(deviceTable.Rows(0).Item(DevicesCols.Checksum))
                .HostName = NoNull(deviceTable.Rows(0).Item(DevicesCols.HostName))
            End With
            Return newDeviceInfo
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

    Public Function DeleteMasterSqlEntry(sqlGUID As String, type As EntryType) As Boolean
        Try
            Dim rows As Integer
            Dim strSQLQry As String = ""
            Select Case type
                Case EntryType.Device
                    strSQLQry = "DELETE FROM " & DevicesCols.TableName & " WHERE " & DevicesCols.DeviceUID & "='" & sqlGUID & "'"
                Case EntryType.Sibi
                    strSQLQry = "DELETE FROM " & SibiRequestCols.TableName & " WHERE " & SibiRequestCols.UID & "='" & sqlGUID & "'"
            End Select
            Using SQLComms As New MySqlComms
                rows = SQLComms.ReturnMySqlCommand(strSQLQry).ExecuteNonQuery
                If rows > 0 Then
                    Return True
                End If
            End Using
            Return False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

    Public Function DeleteFtpAndSql(sqlGUID As String, type As EntryType) As Boolean
        Try
            If FTPFunc.HasFtpFolder(sqlGUID) Then
                If FTPFunc.DeleteFtpFolder(sqlGUID) Then Return DeleteMasterSqlEntry(sqlGUID, type) ' if has attachments, delete ftp directory, then delete the sql records.
            Else
                Return DeleteMasterSqlEntry(sqlGUID, type) 'delete sql records
            End If
        Catch ex As Exception
            Return ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
        Return False
    End Function

    Public Function DeleteSqlAttachment(attachment As Attachment) As Integer
        Try
            Dim rows As Integer
            Dim strDeviceID As String = ""
            Dim strSQLIDQry As String = ""
            strSQLIDQry = "SELECT " & attachment.AttachTable.FKey & " FROM " & attachment.AttachTable.TableName & " WHERE " & attachment.AttachTable.FileUID & "='" & attachment.FileUID & "'"
            Using SQLComms As New MySqlComms, results As DataTable = SQLComms.ReturnMySqlTable(strSQLIDQry)
                For Each r As DataRow In results.Rows
                    strDeviceID = r.Item(attachment.AttachTable.FKey).ToString
                Next
                'Delete FTP Attachment
                If FTPFunc.DeleteFtpAttachment(attachment.FileUID, strDeviceID) Then
                    'delete SQL entry
                    Dim strSQLDelQry As String = ""
                    strSQLDelQry = "DELETE FROM " & attachment.AttachTable.TableName & " WHERE " & attachment.AttachTable.FileUID & "='" & attachment.FileUID & "'"
                    rows = SQLComms.ReturnMySqlCommand(strSQLDelQry).ExecuteNonQuery
                    Return rows
                    'Else  'if file not found then we might as well remove the DB record.
                    '    Dim strSQLDelQry As String = "DELETE FROM dev_attachments WHERE attach_file_UID='" & AttachUID & "'"
                    '    cmd.Connection = GlobalConn
                    '    cmd.CommandText = strSQLDelQry
                    '    rows = cmd.ExecuteNonQuery()
                    '    Return rows
                End If
            End Using
            Return -1
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return -1
        End Try
    End Function

    Public Function DeviceExists(assetTag As String, serial As String) As Boolean
        Dim bolAsset As Boolean
        Dim bolSerial As Boolean
        If assetTag = "NA" Then 'Allow NA value because users do not always have an Asset Tag for new devices.
            bolAsset = False
        Else
            Dim CheckAsset As String = GetSqlValue(DevicesCols.TableName, DevicesCols.AssetTag, assetTag, DevicesCols.AssetTag)
            If CheckAsset <> "" Then
                bolAsset = True
            Else
                bolAsset = False
            End If
        End If

        Dim CheckSerial As String = GetSqlValue(DevicesCols.TableName, DevicesCols.Serial, serial, DevicesCols.Serial)
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
    End Function

    Public Function DevicesBySupervisor(parentForm As Form) As DataTable
        Try
            Dim SupInfo As MunisEmployeeStruct
            Using NewMunisSearch As New MunisUserForm(parentForm)
                If NewMunisSearch.DialogResult = DialogResult.Yes Then
                    SetWaitCursor(True, parentForm)
                    SupInfo = NewMunisSearch.EmployeeInfo
                    Using SQLComms As New MySqlComms, DeviceList As New DataTable, EmpList As DataTable = MunisFunc.ListOfEmpsBySup(SupInfo.Number)
                        For Each r As DataRow In EmpList.Rows
                            Dim strQRY As String = "SELECT * FROM " & DevicesCols.TableName & " WHERE " & DevicesCols.MunisEmpNum & "='" & r.Item("a_employee_number").ToString & "'"
                            Using tmpTable As DataTable = SQLComms.ReturnMySqlTable(strQRY)
                                DeviceList.Merge(tmpTable)
                            End Using
                        Next
                        Return DeviceList
                    End Using
                Else
                    Return Nothing
                End If
            End Using
        Finally
            SetWaitCursor(False, parentForm)
        End Try
    End Function

    Public Function IsEmployeeInDB(empNum As String) As Boolean
        Dim EmpName As String = GetSqlValue(EmployeesCols.TableName, EmployeesCols.Number, empNum, EmployeesCols.Name)
        If EmpName <> "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function FindDeviceFromAssetOrSerial(searchVal As String, type As FindDevType) As DeviceStruct
        Try
            If type = FindDevType.AssetTag Then
                Dim Params As New List(Of DBQueryParameter)
                Params.Add(New DBQueryParameter(DevicesCols.AssetTag, searchVal, True))
                Return CollectDeviceInfo(DBFunc.GetDatabase.DataTableFromCommand(GetSQLCommandFromParams("SELECT * FROM " & DevicesCols.TableName, Params)))
            ElseIf type = FindDevType.Serial Then
                Dim Params As New List(Of DBQueryParameter)
                Params.Add(New DBQueryParameter(DevicesCols.Serial, searchVal, True))
                Return CollectDeviceInfo(DBFunc.GetDatabase.DataTableFromCommand(GetSQLCommandFromParams("SELECT * FROM " & DevicesCols.TableName, Params)))
            End If
            Return Nothing
        Catch ex As MySqlException
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

    Public Function GetDeviceInfoFromGUID(deviceGUID As String) As DeviceStruct
        Using SQLComms As New MySqlComms
            Return AssetFunc.CollectDeviceInfo(SQLComms.ReturnMySqlTable("SELECT * FROM " & DevicesCols.TableName & " WHERE " & DevicesCols.DeviceUID & "='" & deviceGUID & "'"))
        End Using
    End Function

    Public Function GetHistoricalEntryInfo(ByVal deviceHistEntryGUID As String) As DeviceStruct
        Try
            Dim tmpInfo As New DeviceStruct
            Dim strQry = "SELECT * FROM " & HistoricalDevicesCols.TableName & " WHERE " & HistoricalDevicesCols.HistoryEntryUID & "='" & deviceHistEntryGUID & "'"
            Using SQLComms As New MySqlComms, results As DataTable = SQLComms.ReturnMySqlTable(strQry)
                For Each r As DataRow In results.Rows
                    tmpInfo.Historical.ChangeType = GetHumanValue(DeviceIndex.ChangeType, r.Item(HistoricalDevicesCols.ChangeType).ToString)
                    tmpInfo.AssetTag = r.Item(HistoricalDevicesCols.AssetTag).ToString
                    tmpInfo.CurrentUser = r.Item(HistoricalDevicesCols.CurrentUser).ToString
                    tmpInfo.Serial = r.Item(HistoricalDevicesCols.Serial).ToString
                    tmpInfo.Description = r.Item(HistoricalDevicesCols.Description).ToString
                    tmpInfo.Historical.ActionDateTime = DateTime.Parse(r.Item(HistoricalDevicesCols.ActionDateTime).ToString)
                    tmpInfo.Historical.ActionUser = r.Item(HistoricalDevicesCols.ActionUser).ToString
                Next
                Return tmpInfo
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

    Public Function GetMunisCodeFromAssetCode(assetCode As String) As String
        Return GetSqlValue("munis_codes", "asset_man_code", assetCode, "munis_code")
    End Function

    Public Function GetSqlValue(table As String, fieldIn As String, valueIn As String, fieldOut As String) As String
        Dim sqlQRY As String = "SELECT " & fieldOut & " FROM " & table ' & " WHERE " & fieldIN & " = '" & valueIN & "' LIMIT 1"
        Dim Params As New List(Of DBQueryParameter)
        Params.Add(New DBQueryParameter(fieldIn, valueIn, True))
        Dim Result = DBFunc.GetDatabase.ExecuteScalarFromCommand(GetSQLCommandFromParams(sqlQRY, Params))
        If Result IsNot Nothing Then
            Return Result.ToString
        Else
            Return ""
        End If
    End Function

    ''' <summary>
    ''' Takes a partial query string without the WHERE operator, and a list of <see cref="DBQueryParameter"/> and returns a parameterized <see cref="DBCommand"/>.
    ''' </summary>
    ''' <param name="partialQuery"></param>
    ''' <param name="parameters"></param>
    ''' <returns></returns>
    Private Function GetSQLCommandFromParams(partialQuery As String, parameters As List(Of DBQueryParameter)) As DbCommand
        Dim cmd = DBFunc.GetDatabase.GetCommand(partialQuery)
        cmd.CommandText += " WHERE"
        Dim ParamString As String = ""
        Dim ValSeq As Integer = 1
        For Each fld In parameters
            Dim ValueName As String = "@Value" & ValSeq
            If fld.IsExact Then
                ParamString += " " + fld.FieldName + "=" & ValueName & " " & fld.OperatorString
                cmd.AddParameterWithValue(ValueName, fld.Value)
            Else
                ParamString += " " + fld.FieldName + " LIKE " & ValueName & " " & fld.OperatorString
                Dim Value As String = "%" & fld.Value.ToString & "%"
                cmd.AddParameterWithValue(ValueName, Value)
            End If
            ValSeq += 1
        Next
        If Strings.Right(ParamString, 3) = "AND" Then 'remove trailing AND from query string
            ParamString = Strings.Left(ParamString, Strings.Len(ParamString) - 3)
        End If

        If Strings.Right(ParamString, 2) = "OR" Then 'remove trailing AND from query string
            ParamString = Strings.Left(ParamString, Strings.Len(ParamString) - 2)
        End If
        cmd.CommandText += ParamString
        Return cmd
    End Function

    Public Function GetAttachmentCount(attachFolderUID As String, attachTable As AttachmentsBaseCols) As Integer
        Try
            Dim strQRY As String = ""
            strQRY = "SELECT COUNT(*) FROM " & attachTable.TableName & " WHERE " & attachTable.FKey & "='" & attachFolderUID & "'"
            Using SQLComms As New MySqlComms, cmd As MySqlCommand = SQLComms.ReturnMySqlCommand(strQRY)
                Return CInt(cmd.ExecuteScalar)
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return 0
        End Try
    End Function

    Public Function UpdateSqlValue(table As String, fieldIn As String, valueIn As String, idField As String, idValue As String) As Integer
        Dim sqlUpdateQry As String = "UPDATE " & table & " SET " & fieldIn & "=@ValueIN  WHERE " & idField & "='" & idValue & "'"
        Using SQLComms As New MySqlComms, cmd As MySqlCommand = SQLComms.ReturnMySqlCommand(sqlUpdateQry)
            cmd.Parameters.AddWithValue("@ValueIN", valueIn)
            Return cmd.ExecuteNonQuery()
        End Using
    End Function

#End Region

End Class