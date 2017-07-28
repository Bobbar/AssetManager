Option Explicit On

Imports System.IO
Imports System.Net
Imports System.Runtime.Serialization
Imports System.Security.Cryptography
Imports System.Text

Module SecurityFunctions
    Private AccessLevels() As AccessGroupStruct
    Private UserAccess As LocalUserInfoStruct
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

    Public Function DecodePassword(strCypher As String) As String
        Using wrapper As New Simple3Des(CryptKey)
            Return wrapper.DecryptData(strCypher)
        End Using
    End Function

    Public Function GetHashOfTable(Table As DataTable) As String
        Dim serializer = New DataContractSerializer(GetType(DataTable))
        Using memoryStream = New MemoryStream(), SHA = New SHA1CryptoServiceProvider()
            serializer.WriteObject(memoryStream, Table)
            Dim serializedData As Byte() = memoryStream.ToArray()
            Dim hash As Byte() = SHA.ComputeHash(serializedData)
            Return Convert.ToBase64String(hash)
        End Using
    End Function

    Public Function GetHashOfFile(Path As String) As String
        Dim hashValue() As Byte
        Using fStream As New FileStream(Path, FileMode.Open, FileAccess.Read, FileShare.Read, 16 * 1024 * 1024), hash = MD5.Create
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

    Public Function GetHashOfFileStream(ByRef MemStream As IO.FileStream) As String
        Using md5Hash As MD5 = MD5.Create
            MemStream.Position = 0
            Dim hash As Byte() = md5Hash.ComputeHash(MemStream)
            Dim sBuilder As New StringBuilder
            Dim i As Integer
            For i = 0 To hash.Length - 1
                sBuilder.Append(hash(i).ToString("x2"))
            Next
            MemStream.Position = 0
            Return sBuilder.ToString
        End Using
    End Function

    Public Function GetHashOfIOStream(ByVal MemStream As IO.MemoryStream) As String
        Using md5Hash As MD5 = MD5.Create
            MemStream.Position = 0
            Dim hash As Byte() = md5Hash.ComputeHash(MemStream)
            Dim sBuilder As New StringBuilder
            Dim i As Integer
            For i = 0 To hash.Length - 1
                sBuilder.Append(hash(i).ToString("x2"))
            Next
            MemStream.Position = 0
            Return sBuilder.ToString
        End Using
    End Function

    Public Function GetSecGroupValue(SecModule As String) As Integer
        For Each Group As AccessGroupStruct In AccessLevels
            If Group.AccessModule = SecModule Then Return Group.Level
        Next
        Return -1
    End Function

    Public Sub GetUserAccess()
        Try
            Dim strQRY = "SELECT * FROM " & UsersCols.TableName & " WHERE " & UsersCols.UserName & "='" & strLocalUser & "' LIMIT 1"
            Using results As DataTable = DBFunc.DataTableFromQueryString(strQRY)
                If results.Rows.Count > 0 Then
                    Dim r As DataRow = results.Rows(0)
                    UserAccess.UserName = r.Item(UsersCols.UserName).ToString
                    UserAccess.Fullname = r.Item(UsersCols.FullName).ToString
                    UserAccess.AccessLevel = CInt(r.Item(UsersCols.AccessLevel))
                    UserAccess.GUID = r.Item(UsersCols.UID).ToString
                Else
                    UserAccess.AccessLevel = 0
                End If
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Public Sub GetAccessLevels()
        Try
            Dim strQRY = "SELECT * FROM " & SecurityCols.TableName & " ORDER BY " & SecurityCols.AccessLevel & "" ' WHERE usr_username='" & strLocalUser & "'"
            Dim rows As Integer
            Using results As DataTable = DBFunc.DataTableFromQueryString(strQRY)
                ReDim AccessLevels(0)
                rows = -1
                For Each r As DataRow In results.Rows
                    rows += 1
                    ReDim Preserve AccessLevels(rows)
                    AccessLevels(rows).Level = CInt(r.Item(SecurityCols.AccessLevel))
                    AccessLevels(rows).AccessModule = r.Item(SecurityCols.SecModule).ToString
                    AccessLevels(rows).Description = r.Item(SecurityCols.Description).ToString
                    AccessLevels(rows).AvailableOffline = CBool(r.Item(SecurityCols.AvailOffline))
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
            UsrLevel = UserAccess.AccessLevel
        Else
            UsrLevel = AccessLevel
        End If
        Dim levels As Integer
        For levels = 0 To UBound(AccessLevels)
            calc_level = UsrLevel And mask
            If calc_level <> 0 Then
                If AccessLevels(levels).AccessModule = recModule Then
                    If OfflineMode Then
                        If AccessLevels(levels).AvailableOffline Then
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
            If OfflineMode Then
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
        Public Const Add As String = "add"
        Public Const CanRun As String = "can_run"
        Public Const Delete As String = "delete"
        Public Const ManageAttachment As String = "manage_attach"
        Public Const Modify As String = "modify"
        Public Const Tracking As String = "track"
        Public Const ViewAttachment As String = "view_attach"
        Public Const Sibi_View As String = "sibi_view"
        Public Const Sibi_Add As String = "sibi_add"
        Public Const Sibi_Modify As String = "sibi_modify"
        Public Const Sibi_Delete As String = "sibi_delete"
        Public Const IsAdmin As String = "admin"
        Public Const AdvancedSearch As String = "advanced_search"
    End Class

End Module