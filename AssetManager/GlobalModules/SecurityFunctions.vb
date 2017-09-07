Option Explicit On

Imports System.IO
Imports System.Net
Imports System.Runtime.Serialization
Imports System.Security.Cryptography
Imports System.Text

Module SecurityFunctions
    Private AccessGroups() As AccessGroupStruct
    Private LocalUserAccess As LocalUserInfoStruct
    Private Const CryptKey As String = "r7L$aNjE6eiVj&zhap_@|Gz_"
    Public AdminCreds As NetworkCredential = Nothing

    Public Function VerifyAdminCreds() As Boolean
        If AdminCreds Is Nothing Then
            Using NewGetCreds As New GetCredentialsForm
                NewGetCreds.ShowDialog()
                If NewGetCreds.DialogResult = DialogResult.OK Then
                    AdminCreds = NewGetCreds.Credentials
                    Return True
                End If
            End Using
        Else
            Return True
        End If
        Return False
    End Function

    Public Function DecodePassword(cypherString As String) As String
        Using wrapper As New Simple3Des(CryptKey)
            Return wrapper.DecryptData(cypherString)
        End Using
    End Function

    Public Function GetHashOfTable(table As DataTable) As String
        Dim serializer = New DataContractSerializer(GetType(DataTable))
        Using memoryStream = New MemoryStream(), SHA = New SHA1CryptoServiceProvider()
            serializer.WriteObject(memoryStream, table)
            Dim serializedData As Byte() = memoryStream.ToArray()
            Dim hash As Byte() = SHA.ComputeHash(serializedData)
            Return Convert.ToBase64String(hash)
        End Using
    End Function

    Public Function GetHashOfFile(filePath As String) As String
        Dim hashValue() As Byte
        Using fStream As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 16 * 1024 * 1024), hash = MD5.Create
            fStream.Position = 0
            hashValue = hash.ComputeHash(fStream)
            Dim sBuilder As New StringBuilder
            Dim i As Integer
            For i = 0 To hashValue.Length - 1
                sBuilder.Append(hashValue(i).ToString("x2"))
            Next
            Return sBuilder.ToString
        End Using
    End Function

    Public Function GetHashOfFileStream(fileStream As IO.FileStream) As String
        Using md5Hash As MD5 = MD5.Create
            fileStream.Position = 0
            Dim hash As Byte() = md5Hash.ComputeHash(fileStream)
            Dim sBuilder As New StringBuilder
            Dim i As Integer
            For i = 0 To hash.Length - 1
                sBuilder.Append(hash(i).ToString("x2"))
            Next
            fileStream.Position = 0
            Return sBuilder.ToString
        End Using
    End Function

    Public Function GetHashOfIOStream(memStream As IO.MemoryStream) As String
        Using md5Hash As MD5 = MD5.Create
            memStream.Position = 0
            Dim hash As Byte() = md5Hash.ComputeHash(memStream)
            Dim sBuilder As New StringBuilder
            Dim i As Integer
            For i = 0 To hash.Length - 1
                sBuilder.Append(hash(i).ToString("x2"))
            Next
            memStream.Position = 0
            Return sBuilder.ToString
        End Using
    End Function

    Public Function GetSecGroupValue(accessGroupName As String) As Integer
        For Each Group As AccessGroupStruct In AccessGroups
            If Group.AccessModule = accessGroupName Then Return Group.Level
        Next
        Return -1
    End Function

    Public Sub GetUserAccess()
        Try
            Dim strQRY = "SELECT * FROM " & UsersCols.TableName & " WHERE " & UsersCols.UserName & "='" & strLocalUser & "' LIMIT 1"
            Using results As DataTable = DBFunc.DataTableFromQueryString(strQRY)
                If results.Rows.Count > 0 Then
                    Dim r As DataRow = results.Rows(0)
                    LocalUserAccess.UserName = r.Item(UsersCols.UserName).ToString
                    LocalUserAccess.Fullname = r.Item(UsersCols.FullName).ToString
                    LocalUserAccess.AccessLevel = CInt(r.Item(UsersCols.AccessLevel))
                    LocalUserAccess.GUID = r.Item(UsersCols.UID).ToString
                Else
                    LocalUserAccess.AccessLevel = 0
                End If
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Public Sub PopulateAccessGroups()
        Try
            Dim strQRY = "SELECT * FROM " & SecurityCols.TableName & " ORDER BY " & SecurityCols.AccessLevel & ""
            Dim rows As Integer = 0
            Using results As DataTable = DBFunc.DataTableFromQueryString(strQRY)
                ReDim AccessGroups(results.Rows.Count - 1)
                For Each r As DataRow In results.Rows
                    AccessGroups(rows).Level = CInt(r.Item(SecurityCols.AccessLevel))
                    AccessGroups(rows).AccessModule = r.Item(SecurityCols.SecModule).ToString
                    AccessGroups(rows).Description = r.Item(SecurityCols.Description).ToString
                    AccessGroups(rows).AvailableOffline = CBool(r.Item(SecurityCols.AvailOffline))
                    rows += 1
                Next
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Public Function CanAccess(recModule As String, Optional AccessLevel As Integer = -1) As Boolean 'bitwise access levels
        Dim mask As Integer = 1
        Dim calc_level As Integer
        Dim UsrLevel As Integer
        If AccessLevel = -1 Then
            UsrLevel = LocalUserAccess.AccessLevel
        Else
            UsrLevel = AccessLevel
        End If
        Dim levels As Integer
        For levels = 0 To UBound(AccessGroups)
            calc_level = UsrLevel And mask
            If calc_level <> 0 Then
                If AccessGroups(levels).AccessModule = recModule Then
                    If GlobalSwitches.CachedMode Then
                        If AccessGroups(levels).AvailableOffline Then
                            Return True
                        Else
                            Return False
                        End If
                    Else
                        Return True
                    End If
                End If
            End If
            mask = mask << 1
        Next
        Return False
    End Function

    Public Function CheckForAccess(recModule As String) As Boolean
        If Not CanAccess(recModule) Then
            If GlobalSwitches.CachedMode Then
                Message("You cannot access this function. Some features are disabled while running in cached mode.", vbOKOnly + vbExclamation, "Access Denied/Disabled")
            Else
                Message("You do not have the required rights for this function. Must have access to '" & recModule & "'.", vbOKOnly + vbExclamation, "Access Denied")
            End If
            Return False
        Else
            Return True
        End If
    End Function

    Public NotInheritable Class AccessGroup
        Public Const AddDevice As String = "add"
        Public Const CanRun As String = "can_run"
        Public Const DeleteDevice As String = "delete"
        Public Const ManageAttachment As String = "manage_attach"
        Public Const ModifyDevice As String = "modify"
        Public Const Tracking As String = "track"
        Public Const ViewAttachment As String = "view_attach"
        Public Const ViewSibi As String = "sibi_view"
        Public Const AddSibi As String = "sibi_add"
        Public Const ModifySibi As String = "sibi_modify"
        Public Const DeleteSibi As String = "sibi_delete"
        Public Const IsAdmin As String = "admin"
        Public Const AdvancedSearch As String = "advanced_search"
    End Class

End Module