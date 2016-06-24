Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Security.Cryptography
Imports System.Security
Imports System.Text
Module SecurityMod
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
    Public Sub GetAccessLevels()
        Try
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM security ORDER BY sec_access_level" ' WHERE usr_username='" & strLocalUser & "'"
            Dim rows As Integer
            reader = ReturnSQLReader(strQRY)
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
            reader = ReturnSQLReader(strQRY)
            With reader
                If .HasRows Then
                    Do While .Read()
                        UserAccess.strUsername = !usr_username
                        UserAccess.strFullname = !usr_fullname
                        UserAccess.bolIsAdmin = Convert.ToBoolean(reader("usr_isadmin"))
                        UserAccess.intAccessLevel = !usr_access_level
                        UserAccess.strUID = !usr_UID
                    Loop
                Else
                    UserAccess.intAccessLevel = 0
                    UserAccess.bolIsAdmin = False
                End If
            End With
            reader.Close()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Function IsAdmin() As Boolean
        Return UserAccess.bolIsAdmin
    End Function
    Public Function CheckForAdmin() As Boolean
        If Not UserAccess.bolIsAdmin Then
            Dim blah = MsgBox("Administrator rights required for this function.", vbOKOnly + vbExclamation, "Access Denied")
            Return False
        Else
            Return True
        End If
    End Function
    Public Function CanAccess(recModule As String) As Boolean 'bitwise access levels
        Dim mask As UInteger = 1
        Dim calc_level As UInteger
        Dim UsrLevel As UInteger = UserAccess.intAccessLevel
        Dim levels As Integer
        For levels = 0 To UBound(AccessLevels)
            calc_level = UsrLevel And mask
            If calc_level <> 0 Then
                If AccessLevels(levels + 1).strModule = recModule Then
                    Return True
                End If
            End If
            mask = mask << 1
        Next
        Return False
    End Function
    Public Function CheckForAccess(recModule As String) As Boolean
        If Not CanAccess(recModule) Then
            Dim blah = MsgBox("You do not have the required rights for this function. Must have access to '" & recModule & "'.", vbOKOnly + vbExclamation, "Access Denied")
            Return False
        Else
            Return True
        End If
    End Function
End Module
