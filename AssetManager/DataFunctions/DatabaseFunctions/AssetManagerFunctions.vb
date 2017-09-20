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
        Catch ex As Exception
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
        Using ModuleTable As DataTable = DBFunc.GetDatabase.DataTableFromQueryString("SELECT * FROM " & SecurityCols.TableName & " ORDER BY " & SecurityCols.AccessLevel & "")
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
    Public Sub PopStructInfo(obj As Object, data As DataTable)
        Dim Props As List(Of Reflection.PropertyInfo) = (obj.GetType.GetProperties().Where(Function(x) x.GetCustomAttributes(GetType(DataNamesAttribute), True).Any()).ToList())
        Dim row = data.Rows(0)

        For Each prop In Props
            Dim propColumn = DirectCast(prop.GetCustomAttributes(False)(0), DataNamesAttribute).ValueName

            Select Case prop.PropertyType
                Case GetType(String)
                    prop.SetValue(obj, row(propColumn).ToString, Nothing)

                Case GetType(DateTime)
                    prop.SetValue(obj, DateTime.Parse(NoNull(row(propColumn).ToString)))

                Case GetType(Boolean)
                    prop.SetValue(obj, CBool(row(propColumn)))

                Case Else
                    Debug.Print(prop.PropertyType.ToString)
            End Select



        Next



    End Sub



    Public Function CollectDeviceInfo(deviceTable As DataTable) As DeviceStruct
        Try

            'Dim testDev As New DeviceStruct
            'PopStructInfo(testDev, deviceTable)





            Dim newDeviceInfo As New DeviceStruct

            PopStructInfo(newDeviceInfo, deviceTable)

            'With newDeviceInfo


            '    .GUID = NoNull(deviceTable.Rows(0).Item(DevicesCols.DeviceUID))
            '    .Description = NoNull(deviceTable.Rows(0).Item(DevicesCols.Description))
            '    .Location = NoNull(deviceTable.Rows(0).Item(DevicesCols.Location))
            '    .CurrentUser = NoNull(deviceTable.Rows(0).Item(DevicesCols.CurrentUser))
            '    .CurrentUserEmpNum = NoNull(deviceTable.Rows(0).Item(DevicesCols.MunisEmpNum))
            '    .Serial = NoNull(deviceTable.Rows(0).Item(DevicesCols.Serial))
            '    .AssetTag = NoNull(deviceTable.Rows(0).Item(DevicesCols.AssetTag))
            '    .PurchaseDate = DateTime.Parse(NoNull(deviceTable.Rows(0).Item(DevicesCols.PurchaseDate)))
            '    .ReplaceYear = NoNull(deviceTable.Rows(0).Item(DevicesCols.ReplacementYear))
            '    .PO = NoNull(deviceTable.Rows(0).Item(DevicesCols.PO))
            '    .OSVersion = NoNull(deviceTable.Rows(0).Item(DevicesCols.OSVersion))
            '    .PhoneNumber = NoNull(deviceTable.Rows(0).Item(DevicesCols.PhoneNumber))
            '    .EquipmentType = NoNull(deviceTable.Rows(0).Item(DevicesCols.EQType))
            '    .Status = NoNull(deviceTable.Rows(0).Item(DevicesCols.Status))
            '    .IsTrackable = CBool(deviceTable.Rows(0).Item(DevicesCols.Trackable))
            '    .SibiLink = NoNull(deviceTable.Rows(0).Item(DevicesCols.SibiLinkUID))
            '    .Tracking.IsCheckedOut = CBool(deviceTable.Rows(0).Item(DevicesCols.CheckedOut))
            '    .HostName = NoNull(deviceTable.Rows(0).Item(DevicesCols.HostName))
            'End With
            Return newDeviceInfo
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

    Public Function DeleteMasterSqlEntry(sqlGUID As String, type As EntryType) As Boolean
        Try
            Dim DeleteQuery As String = ""
            Select Case type
                Case EntryType.Device
                    DeleteQuery = "DELETE FROM " & DevicesCols.TableName & " WHERE " & DevicesCols.DeviceUID & "='" & sqlGUID & "'"
                Case EntryType.Sibi
                    DeleteQuery = "DELETE FROM " & SibiRequestCols.TableName & " WHERE " & SibiRequestCols.UID & "='" & sqlGUID & "'"
            End Select
            If DBFunc.GetDatabase.ExecuteQuery(DeleteQuery) > 0 Then
                Return True
            End If
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
            Dim AttachmentFolderID = GetSqlValue(attachment.AttachTable.TableName, attachment.AttachTable.FileUID, attachment.FileUID, attachment.AttachTable.FKey)
            'Delete FTP Attachment
            If FTPFunc.DeleteFtpAttachment(attachment.FileUID, AttachmentFolderID) Then
                'delete SQL entry
                Dim SQLDeleteQry = "DELETE FROM " & attachment.AttachTable.TableName & " WHERE " & attachment.AttachTable.FileUID & "='" & attachment.FileUID & "'"
                Return DBFunc.GetDatabase.ExecuteQuery(SQLDeleteQry)
            End If
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

    Public Function DevicesBySupervisor(parentForm As ExtendedForm) As DataTable
        Try
            Dim SupInfo As MunisEmployeeStruct
            Using NewMunisSearch As New MunisUserForm(parentForm)
                If NewMunisSearch.DialogResult = DialogResult.Yes Then
                    SetWaitCursor(True, parentForm)
                    SupInfo = NewMunisSearch.EmployeeInfo
                    Using DeviceList As New DataTable, EmpList As DataTable = MunisFunc.ListOfEmpsBySup(SupInfo.Number)
                        For Each r As DataRow In EmpList.Rows
                            Dim strQRY As String = "SELECT * FROM " & DevicesCols.TableName & " WHERE " & DevicesCols.MunisEmpNum & "='" & r.Item("a_employee_number").ToString & "'"
                            Using tmpTable As DataTable = DBFunc.GetDatabase.DataTableFromQueryString(strQRY)
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
                Return CollectDeviceInfo(DBFunc.GetDatabase.DataTableFromParameters("SELECT * FROM " & DevicesCols.TableName & " WHERE ", Params))
            ElseIf type = FindDevType.Serial Then
                Dim Params As New List(Of DBQueryParameter)
                Params.Add(New DBQueryParameter(DevicesCols.Serial, searchVal, True))
                Return CollectDeviceInfo(DBFunc.GetDatabase.DataTableFromParameters("SELECT * FROM " & DevicesCols.TableName & " WHERE ", Params))
            End If
            Return Nothing
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

    Public Function GetDeviceInfoFromGUID(deviceGUID As String) As DeviceStruct
        Return AssetFunc.CollectDeviceInfo(DBFunc.GetDatabase.DataTableFromQueryString("SELECT * FROM " & DevicesCols.TableName & " WHERE " & DevicesCols.DeviceUID & "='" & deviceGUID & "'"))
    End Function

    Public Function GetHistoricalEntryInfo(ByVal deviceHistEntryGUID As String) As DeviceStruct
        Try
            Dim tmpInfo As New DeviceStruct
            Dim strQry = "SELECT * FROM " & HistoricalDevicesCols.TableName & " WHERE " & HistoricalDevicesCols.HistoryEntryUID & "='" & deviceHistEntryGUID & "'"
            Using results As DataTable = DBFunc.GetDatabase.DataTableFromQueryString(strQry)
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
        Dim sqlQRY As String = "SELECT " & fieldOut & " FROM " & table & " WHERE "
        Dim Params As New List(Of DBQueryParameter)
        Params.Add(New DBQueryParameter(fieldIn, valueIn, True))
        Dim Result = DBFunc.GetDatabase.ExecuteScalarFromCommand(DBFunc.GetDatabase.GetCommandFromParams(sqlQRY, Params))
        If Result IsNot Nothing Then
            Return Result.ToString
        Else
            Return ""
        End If
    End Function

    Public Function GetAttachmentCount(attachFolderUID As String, attachTable As AttachmentsBaseCols) As Integer
        Try
            Return CInt(GetSqlValue(attachTable.TableName, attachTable.FKey, attachFolderUID, "COUNT(*)"))
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return 0
        End Try
    End Function

    Public Function UpdateSqlValue(table As String, fieldIn As String, valueIn As String, idField As String, idValue As String) As Integer
        Return DBFunc.GetDatabase.UpdateValue(table, fieldIn, valueIn, idField, idValue)
    End Function

#End Region

End Class