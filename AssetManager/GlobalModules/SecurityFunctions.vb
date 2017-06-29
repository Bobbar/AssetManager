Option Explicit On
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Imports System.Runtime.Serialization
Imports System.Net
Module SecurityFunctions
    Private AccessLevels() As Access_Info
    Private UserAccess As User_Info
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
        Dim wrapper As New Simple3Des(CryptKey)
        Return wrapper.DecryptData(strCypher)
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
            fStream.Close()
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
        For Each Group As Access_Info In AccessLevels
            If Group.strModule = SecModule Then Return Group.intLevel
        Next
    End Function
    Public Sub GetUserAccess()
        Try
            Dim strQRY = "SELECT * FROM " & users.TableName & " WHERE " & users.UserName & "='" & strLocalUser & "' LIMIT 1"
            Using results As DataTable = DBFunc.DataTableFromQueryString(strQRY)
                If results.Rows.Count > 0 Then
                    Dim r As DataRow = results.Rows(0)
                    UserAccess.strUsername = r.Item(users.UserName).ToString
                    UserAccess.strFullname = r.Item(users.FullName).ToString
                    UserAccess.intAccessLevel = CInt(r.Item(users.AccessLevel))
                    UserAccess.strUID = r.Item(users.UID).ToString
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
            Using results As DataTable = DBFunc.DataTableFromQueryString(strQRY)
                ReDim AccessLevels(0)
                rows = -1
                For Each r As DataRow In results.Rows
                    rows += 1
                    ReDim Preserve AccessLevels(rows)
                    AccessLevels(rows).intLevel = CInt(r.Item(security.AccessLevel))
                    AccessLevels(rows).strModule = r.Item(security.SecModule).ToString
                    AccessLevels(rows).strDesc = r.Item(security.Description).ToString
                    AccessLevels(rows).bolAvailOffline = CBool(r.Item(security.AvailOffline))
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
            UsrLevel = UserAccess.intAccessLevel
        Else
            UsrLevel = AccessLevel
        End If
        Dim levels As Integer
        For levels = 0 To UBound(AccessLevels)
            calc_level = UsrLevel And mask
            If calc_level <> 0 Then
                If AccessLevels(levels).strModule = recModule Then
                    If OfflineMode Then
                        If AccessLevels(levels).bolAvailOffline Then
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
