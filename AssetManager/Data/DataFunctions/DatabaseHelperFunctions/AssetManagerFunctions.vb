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
                DBFactory.GetDatabase.InsertFromParameters(EmployeesCols.TableName, InsertEmployeeParams)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    ''' <summary>
    ''' Searches the database for the best possible match to the specified search name using a Levenshtein distance algorithm.
    ''' </summary>
    ''' <param name="empSearchName"></param>
    ''' <param name="MinSearchDistance"></param>
    ''' <returns></returns>
    Public Function SmartEmployeeSearch(empSearchName As String, Optional MinSearchDistance As Integer = 10) As MunisEmployeeStruct

        If empSearchName.Trim <> "" Then
            Dim SplitName() = empSearchName.Split(Char.Parse(" "))
            ' Dim LevDist As New List(Of Integer)
            Dim Results As New List(Of SmartEmpSearchStruct)

            'Get results for complete name from employees table
            Results.AddRange(GetEmpSearchResults(EmployeesCols.TableName, empSearchName, EmployeesCols.Name, EmployeesCols.Number))

            'Get results for complete name from devices table
            Results.AddRange(GetEmpSearchResults(DevicesCols.TableName, empSearchName, DevicesCols.CurrentUser, DevicesCols.MunisEmpNum))

            For Each s In SplitName

                'Get results for partial name from employees table
                Results.AddRange(GetEmpSearchResults(EmployeesCols.TableName, s, EmployeesCols.Name, EmployeesCols.Number))

                'Get results for partial name from devices table
                Results.AddRange(GetEmpSearchResults(DevicesCols.TableName, s, DevicesCols.CurrentUser, DevicesCols.MunisEmpNum))

            Next

            If Results.Count > 0 Then
                Results = NarrowResults(Results)
                Dim BestMatch = FindBestSmartSearchMatch(Results)
                If BestMatch.MatchDistance < MinSearchDistance Then
                    Return BestMatch.SearchResult
                End If
            End If
        End If
        Return Nothing
    End Function

    ''' <summary>
    ''' Reprocesses the search results to obtain a more accurate Levenshtein distance calculation.
    ''' </summary>
    ''' <param name="results"></param>
    ''' <remarks>This is done because the initial calculations are performed against the full length
    ''' of the returned names (First and last name), and the distance between the search string and name string may be inaccurate.</remarks>
    ''' <returns></returns>
    Private Function NarrowResults(results As List(Of SmartEmpSearchStruct)) As List(Of SmartEmpSearchStruct)
        Dim newResults As New List(Of SmartEmpSearchStruct)
        'Iterate through results
        For Each result In results
            'Split the results returned string by spaces
            Dim resultSplit = result.SearchResult.Name.Split(Char.Parse(" "))
            If resultSplit.Count > 0 Then
                'Iterate through the separate strings
                For Each item In resultSplit
                    'Make sure the result string contains the search string
                    If item.Contains(result.SearchString) AndAlso item.StartsWith(result.SearchString) Then
                        'Get a new Levenshtein distance.
                        Dim NewDistance = Fastenshtein.Levenshtein.Distance(item, result.SearchString)
                        'If the strings are closer together, add the new data.
                        If NewDistance < result.MatchDistance Then
                            newResults.Add(New SmartEmpSearchStruct(result.SearchResult, result.SearchString, NewDistance))
                        Else
                            newResults.Add(result)
                        End If
                    Else
                        newResults.Add(result)
                    End If
                Next
            End If
        Next
        Return newResults
    End Function

    ''' <summary>
    ''' Finds the best match within the results. The item with shortest Levenshtein distance and the longest match length (string length) is preferred.
    ''' </summary>
    ''' <param name="results"></param>
    ''' <returns></returns>
    Private Function FindBestSmartSearchMatch(results As List(Of SmartEmpSearchStruct)) As SmartEmpSearchStruct
        'Initial minimum distance
        Dim MinDist As Integer = results.First.MatchDistance
        'Initial minimum match
        Dim MinMatch As SmartEmpSearchStruct = results.First
        Dim LongestMatch As New SmartEmpSearchStruct
        Dim DeDupDist As New List(Of SmartEmpSearchStruct)
        'Iterate through the results and determine the result with the shortest Levenshtein distance.
        For Each result In results
            If result.MatchDistance < MinDist Then
                MinDist = result.MatchDistance
                MinMatch = result

            End If
        Next
        'De-duplicate the results and iterate to determine which result of the Levenshtein shortest distances has the longest match length. (Greatest number of matches)
        DeDupDist = results.Distinct().ToList
        If DeDupDist.Count > 0 Then
            Dim MaxMatchLen As Integer = 0
            For Each dup In DeDupDist
                If dup.MatchDistance = MinDist Then
                    If dup.MatchLength > MaxMatchLen Then
                        MaxMatchLen = dup.MatchLength
                        LongestMatch = dup
                    End If
                End If
            Next
            'Return best match by length and Levenshtein distance.
            Return LongestMatch
        End If
        'Return best match by Levenshtein distance only. (If no duplicates)
        Return MinMatch
    End Function

    ''' <summary>
    ''' Queries the database for a list of results that contains the employee name result and computed Levenshtein distance to the search string.
    ''' </summary>
    ''' <param name="tableName"></param>
    ''' <param name="searchEmpName"></param>
    ''' <param name="empNameColumn"></param>
    ''' <param name="empNumColumn"></param>
    ''' <returns></returns>
    Private Function GetEmpSearchResults(tableName As String, searchEmpName As String, empNameColumn As String, empNumColumn As String) As List(Of SmartEmpSearchStruct)
        Dim tmpResults As New List(Of SmartEmpSearchStruct)
        Dim EmpSearchParams As New List(Of DBQueryParameter)
        EmpSearchParams.Add(New DBQueryParameter(empNameColumn, searchEmpName, False))
        Using data = DBFactory.GetDatabase.DataTableFromParameters("SELECT * FROM " & tableName & " WHERE", EmpSearchParams)
            For Each row As DataRow In data.Rows
                tmpResults.Add(New SmartEmpSearchStruct(New MunisEmployeeStruct(row.Item(empNameColumn).ToString, row.Item(empNumColumn).ToString), searchEmpName, Fastenshtein.Levenshtein.Distance(searchEmpName.ToUpper, row.Item(empNameColumn).ToString.ToUpper)))
            Next
        End Using
        Return tmpResults
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
            If DBFactory.GetDatabase.ExecuteQuery(DeleteQuery) > 0 Then
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
                Return DBFactory.GetDatabase.ExecuteQuery(SQLDeleteQry)
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
                            Using tmpTable As DataTable = DBFactory.GetDatabase.DataTableFromQueryString(strQRY)
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

    Public Function FindDeviceFromAssetOrSerial(searchVal As String, type As FindDevType) As DeviceObject
        Try
            If type = FindDevType.AssetTag Then
                Dim Params As New List(Of DBQueryParameter)
                Params.Add(New DBQueryParameter(DevicesCols.AssetTag, searchVal, True))
                Return New DeviceObject(DBFactory.GetDatabase.DataTableFromParameters("SELECT * FROM " & DevicesCols.TableName & " WHERE ", Params))
            ElseIf type = FindDevType.Serial Then
                Dim Params As New List(Of DBQueryParameter)
                Params.Add(New DBQueryParameter(DevicesCols.Serial, searchVal, True))
                Return New DeviceObject(DBFactory.GetDatabase.DataTableFromParameters("SELECT * FROM " & DevicesCols.TableName & " WHERE ", Params))
            End If
            Return Nothing
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

    Public Function GetTVApiToken() As String
        Using results = DBFactory.GetDatabase.DataTableFromQueryString("SELECT apitoken FROM teamviewer_info")
            Dim token = results.Rows(0).Item("apitoken").ToString
            Return token
        End Using
    End Function


    Public Function GetDeviceInfoFromGUID(deviceGUID As String) As DeviceObject
        Return New DeviceObject(DBFactory.GetDatabase.DataTableFromQueryString("SELECT * FROM " & DevicesCols.TableName & " WHERE " & DevicesCols.DeviceUID & "='" & deviceGUID & "'"))
    End Function

    Public Function GetMunisCodeFromAssetCode(assetCode As String) As String
        Return GetSqlValue("munis_codes", "asset_man_code", assetCode, "munis_code")
    End Function

    Public Function GetSqlValue(table As String, fieldIn As String, valueIn As String, fieldOut As String) As String
        Dim sqlQRY As String = "SELECT " & fieldOut & " FROM " & table & " WHERE "
        Dim Params As New List(Of DBQueryParameter)
        Params.Add(New DBQueryParameter(fieldIn, valueIn, True))
        Dim Result = DBFactory.GetDatabase.ExecuteScalarFromCommand(DBFactory.GetDatabase.GetCommandFromParams(sqlQRY, Params))
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
        Return DBFactory.GetDatabase.UpdateValue(table, fieldIn, valueIn, idField, idValue)
    End Function

#End Region

End Class