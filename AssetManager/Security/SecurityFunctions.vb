Imports System.IO
Imports System.Net
Imports System.Runtime.Serialization
Imports System.Security.Cryptography
Imports System.Text
Imports System.DirectoryServices.AccountManagement

Namespace SecurityTools
    Module SecurityFunctions
        Public AdminCreds As NetworkCredential = Nothing
        Private AccessGroups As New Dictionary(Of String, AccessGroupObject)
        Private LocalUserAccess As LocalUserInfoStruct
        Private Const CryptKey As String = "r7L$aNjE6eiVj&zhap_@|Gz_"

        Public Function VerifyAdminCreds(Optional credentialDescription As String = "") As Boolean
            Dim ValidCreds As Boolean = False
            If AdminCreds Is Nothing Then
                Using NewGetCreds As New GetCredentialsForm(credentialDescription)
                    NewGetCreds.ShowDialog()
                    If NewGetCreds.DialogResult = DialogResult.OK Then
                        AdminCreds = NewGetCreds.Credentials
                    Else
                        ClearAdminCreds()
                        Return False
                    End If
                End Using
            End If
            ValidCreds = CredentialIsValid(AdminCreds)
            If Not ValidCreds Then
                ClearAdminCreds()
                If Message("Could not authenticate with provided credentials.  Do you with to re-enter?", vbOKCancel + vbExclamation, "Auth Error") = MsgBoxResult.Ok Then
                    Return VerifyAdminCreds(credentialDescription)
                Else
                    Return False
                End If
            Else
                Return ValidCreds
            End If
        End Function

        Private Function CredentialIsValid(creds As NetworkCredential) As Boolean
            Dim valid = False
            Using context As PrincipalContext = New PrincipalContext(ContextType.Domain, NetworkInfo.CurrentDomain)
                valid = context.ValidateCredentials(creds.UserName, creds.Password)
            End Using
            Return valid
        End Function

        Public Sub ClearAdminCreds()
            AdminCreds = Nothing
        End Sub

        Public Function DecodePassword(cypherString As String) As String
            Using wrapper As New Simple3Des(CryptKey)
                Return wrapper.DecryptData(cypherString)
            End Using
        End Function

        Public Function GetSHAOfTable(table As DataTable) As String
            Dim serializer = New DataContractSerializer(GetType(DataTable))
            Using memoryStream = New MemoryStream(), SHA = New SHA1CryptoServiceProvider()
                serializer.WriteObject(memoryStream, table)
                Dim serializedData As Byte() = memoryStream.ToArray()
                Dim hash As Byte() = SHA.ComputeHash(serializedData)
                Return Convert.ToBase64String(hash)
            End Using
        End Function

        Public Function GetMD5OfFile(filePath As String) As String
            Using fStream As New FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read, 16 * 1024 * 1024), hash = MD5.Create
                Return GetMD5OfStream(fStream)
            End Using
        End Function

        Public Function GetMD5OfStream(stream As Stream) As String
            Using md5Hash As MD5 = MD5.Create
                stream.Position = 0
                Dim hash As Byte() = md5Hash.ComputeHash(stream)
                Dim sBuilder As New StringBuilder
                Dim i As Integer
                For i = 0 To hash.Length - 1
                    sBuilder.Append(hash(i).ToString("x2"))
                Next
                stream.Position = 0
                Return sBuilder.ToString
            End Using
        End Function

        Public Function GetSecGroupValue(accessGroupName As String) As Integer
            Return AccessGroups(accessGroupName).Level
        End Function

        Public Sub GetUserAccess()
            Try
                Dim strQRY = "SELECT * FROM " & UsersCols.TableName & " WHERE " & UsersCols.UserName & "='" & LocalDomainUser & "' LIMIT 1"
                Using results As DataTable = DBFactory.GetDatabase.DataTableFromQueryString(strQRY)
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
                Using results As DataTable = DBFactory.GetDatabase.DataTableFromQueryString(strQRY)
                    For Each row As DataRow In results.Rows
                        AccessGroups.Add(row.Item(SecurityCols.SecModule).ToString, New AccessGroupObject(row))
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
            For Each group In AccessGroups.Values
                calc_level = UsrLevel And mask
                If calc_level <> 0 Then
                    If group.AccessModule = recModule Then
                        If GlobalSwitches.CachedMode Then
                            If group.AvailableOffline Then
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
            Public Const CanStartTransaction As String = "start_transaction"
        End Class

    End Module
End Namespace