Option Explicit On
Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Security.Cryptography
Imports System.Text
Module modSecurityMod
    Public AccessLevels() As Access_Info
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
            Dim blah = Message("You do not have the required rights for this function. Must have access to '" & recModule & "'.", vbOKOnly + vbExclamation, "Access Denied")
            Return False
        Else
            Return True
        End If
    End Function
End Module
