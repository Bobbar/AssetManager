Option Explicit On
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Module SecurityMod
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
    End Class
    Public Const EncMySqlPass As String = "N9WzUK5qv2gOgB1odwfduM13ISneU/DG"
    Public Const EncFTPUserPass As String = "BzPOHPXLdGu9CxaHTAEUCXY4Oa5EVM2B/G7O9En28LQ="
    Private Const CryptKey As String = "r7L$aNjE6eiVj&zhap_@|Gz_"
    Public Function DecodePassword(strCypher As String) As String
        Dim wrapper As New Simple3Des(CryptKey)
        Return wrapper.DecryptData(strCypher)
    End Function
    Public Function GetHashOfFile(Path As String) As String
        Dim hash ' As MD5
        hash = MD5.Create
        Dim hashValue() As Byte
        Dim fileStream As FileStream = File.OpenRead(Path)
        fileStream.Position = 0
        hashValue = hash.ComputeHash(fileStream)
        Dim sBuilder As New StringBuilder
        Dim i As Integer
        For i = 0 To hashValue.Length - 1
            sBuilder.Append(hashValue(i).ToString("x2"))
        Next
        fileStream.Close()
        Return sBuilder.ToString
    End Function
    Public Function GetHashOfStream(ByRef MemStream As IO.FileStream) As String
        Dim md5Hash As MD5 = MD5.Create
        MemStream.Position = 0
        Dim hash As Byte() = md5Hash.ComputeHash(MemStream)
        Dim sBuilder As New StringBuilder
        Dim i As Integer
        For i = 0 To hash.Length - 1
            sBuilder.Append(hash(i).ToString("x2"))
        Next
        MemStream.Position = 0
        Return sBuilder.ToString
    End Function
    Public Function GetSecGroupValue(SecModule As String) As Integer
        For Each Group As Access_Info In AccessLevels
            If Group.strModule = SecModule Then Return Group.intLevel
        Next
    End Function
    Public Sub GetAccessLevels()
        Try
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM security ORDER BY sec_access_level" ' WHERE usr_username='" & strLocalUser & "'"
            Dim rows As Integer
            reader = MySQLDB.Return_SQLReader(strQRY)
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
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Sub GetUserAccess()
        Try
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM users WHERE usr_username='" & strLocalUser & "'"
            reader = MySQLDB.Return_SQLReader(strQRY)
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
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Function CanAccess(recModule As String, intAccessLevel As Integer) As Boolean 'bitwise access levels
        Dim mask As UInteger = 1
        Dim calc_level As UInteger
        Dim UsrLevel As UInteger = intAccessLevel 'UserAccess.intAccessLevel
        Dim levels As Integer
        For levels = 0 To UBound(AccessLevels)
            calc_level = UsrLevel And mask
            If calc_level <> 0 Then
                If AccessLevels(levels).strModule = recModule Then
                    Return True
                End If
            End If
            mask = mask << 1
        Next
        Return False
    End Function
    Public Function CheckForAccess(recModule As String) As Boolean
        If Not CanAccess(recModule, UserAccess.intAccessLevel) Then
            Dim blah = MsgBox("You do not have the required rights for this function. Must have access to '" & recModule & "'.", vbOKOnly + vbExclamation, "Access Denied")
            Return False
        Else
            Return True
        End If
    End Function
End Module
