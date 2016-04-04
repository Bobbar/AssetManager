﻿Option Explicit On
Imports MySql.Data.MySqlClient
Public Module DBFunctions
    Public cn_global As New MySqlConnection("server=localhost;uid=root;pwd=SQLR00tP455W0rd;database=asset_manager")
    Public cn_global2 As New MySqlConnection("server=localhost;uid=root;pwd=SQLR00tP455W0rd;database=asset_manager")
    Public Const strDBDateTimeFormat As String = "YYYY-MM-DD hh:mm:ss"
    Public Const strDBDateFormat As String = "yyyy-MM-dd"
    Public Structure LocationsStruct
        Public strLocationLong As String
        Public strLocationShort As String
        Public strLocationID As String
    End Structure
    ' Public Locations() As LocationsStruct
    Public Structure ChangeTypeStruct
        Public strChangeTypeLong As String
        Public strChangeTypeShort As String
        Public strChangeTypeID As String
    End Structure
    ' Public ChangeType() As ChangeTypeStruct
    Public Structure EquipTypeStruct
        Public strEquipTypeLong As String
        Public strEquipTypeShort As String
        Public strEquipTypeID As String
    End Structure
    'Public EquipType() As EquipTypeStruct
    Public Structure OSTypeStruct
        Public strOSTypeLong As String
        Public strOSTypeShort As String
        Public strOSTypeID As String
    End Structure
    'Public OSType() As OSTypeStruct
    Public Structure Combo_Data
        Public strLong As String
        Public strShort As String
        Public strID As String
    End Structure
    Public Locations() As Combo_Data
    Public ChangeType() As Combo_Data
    Public EquipType() As Combo_Data
    Public OSType() As Combo_Data
    Public NotInheritable Class ComboType
        Public Const Location As String = "LOCATION"
        Public Const ChangeType As String = "CHANGETYPE"
        Public Const EquipType As String = "EQ_TYPE"
        Public Const OSType As String = "OS_TYPE"
    End Class
    Public Function DBConnect()
        Debug.Print("DBState: " & cn_global.State)
        'cn_global.Open()
        Debug.Print("DBState: " & cn_global.State)
    End Function
    Public Sub BuildLocationIndex()
        Dim reader As MySqlDataReader
        cn_global.Open()
        Dim strGetDevices = "SELECT * FROM combo_data WHERE combo_type ='LOCATION' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strGetDevices, cn_global)
        Dim row As Integer
        reader = cmd.ExecuteReader
        ReDim Locations(0)
        row = -1
        With reader
            Do While .Read()
                row = row + 1
                ReDim Preserve Locations(row)
                Locations(row).strID = !combo_ID
                Locations(row).strLong = !combo_data_human
                Locations(row).strShort = !combo_data_db
            Loop
        End With
        cn_global.Close()
        Dim i
        For i = 0 To UBound(Locations)
            Debug.Print(i & " - " & Locations(i).strLong)
        Next
    End Sub
    Public Function GetShortLocation(ByVal index As Integer) As String
        On Error GoTo errs
        Return Locations(index).strShort
        Exit Function
errs:
        Return ""
    End Function
    Public Function GetDeviceUID(ByVal AssetTag As String) As String
        Dim reader As MySqlDataReader
        Dim strQry = "SELECT dev_UID from devices WHERE dev_asset_tag = '" & AssetTag & "'"
        cn_global2.Open()
        Dim cmd As New MySqlCommand(strQry, cn_global2)
        reader = cmd.ExecuteReader
        With reader
            Do While .Read()
                Return (!dev_UID)
            Loop
        End With
        cn_global2.Close()
    End Function
    Public Function GetCMBShortValue(ByVal IndexType As String, ByVal index As Integer) As String
        On Error GoTo errs
        Select Case IndexType
            Case ComboType.Location
                Return Locations(index).strShort
            Case ComboType.ChangeType
                Return ChangeType(index).strShort
            Case ComboType.EquipType
                Return EquipType(index).strShort
            Case ComboType.OSType
                Return OSType(index).strShort
            Case Else
                Return ""
        End Select
        Exit Function
errs:
        Return ""
    End Function
    Public Function GetHumanValue(ByVal Type As String, ByVal ShortVal As String) As String
        Dim SearchIndex() As Combo_Data
        Dim i As Integer
        SearchIndex = GetSearchIndex(Type)
        For i = 0 To UBound(SearchIndex)
            If SearchIndex(i).strShort = ShortVal Then Return SearchIndex(i).strLong
        Next
    End Function
    Private Function GetSearchIndex(ByVal Type As String) As Combo_Data()
        Select Case Type
            Case ComboType.Location
                GetSearchIndex = Locations
            Case ComboType.ChangeType
                GetSearchIndex = ChangeType
            Case ComboType.EquipType
                GetSearchIndex = EquipType
            Case ComboType.OSType
                GetSearchIndex = OSType
            Case Else
        End Select
    End Function
    Public Function GetComboIndexFromShort(ByVal Type As String, ByVal ShortVal As String) As Integer
        Dim SearchIndex() As Combo_Data
        Dim i As Integer
        SearchIndex = GetSearchIndex(Type)
        For i = 0 To UBound(SearchIndex)
            If SearchIndex(i).strShort = ShortVal Then GetComboIndexFromShort = i
        Next
    End Function
    Public Sub BuildChangeTypeIndex()
        Dim reader As MySqlDataReader
        cn_global.Open()
        Dim strGetDevices = "SELECT * FROM combo_data WHERE combo_type ='CHANGETYPE' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strGetDevices, cn_global)
        Dim row As Integer
        reader = cmd.ExecuteReader
        ReDim ChangeType(0)
        row = -1
        With reader
            Do While .Read()
                row = row + 1
                ReDim Preserve ChangeType(row)
                ChangeType(row).strID = !combo_ID
                ChangeType(row).strLong = !combo_data_human
                ChangeType(row).strShort = !combo_data_db
            Loop
        End With
        cn_global.Close()
        Dim i
        For i = 0 To UBound(ChangeType)
            Debug.Print(i & " - " & ChangeType(i).strLong)
        Next
    End Sub
    Public Sub BuildEquipTypeIndex()
        Dim reader As MySqlDataReader
        cn_global.Open()
        Dim strGetDevices = "SELECT * FROM combo_data WHERE combo_type ='EQ_TYPE' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strGetDevices, cn_global)
        Dim row As Integer
        reader = cmd.ExecuteReader
        ReDim EquipType(0)
        row = -1
        With reader
            Do While .Read()
                row = row + 1
                ReDim Preserve EquipType(row)
                EquipType(row).strID = !combo_ID
                EquipType(row).strLong = !combo_data_human
                EquipType(row).strShort = !combo_data_db
            Loop
        End With
        cn_global.Close()
        Dim i
        For i = 0 To UBound(EquipType)
            Debug.Print(i & " - " & EquipType(i).strLong)
        Next
    End Sub
    Public Sub BuildOSTypeIndex()
        Dim reader As MySqlDataReader
        cn_global.Open()
        Dim strGetDevices = "SELECT * FROM combo_data WHERE combo_type ='OS_TYPE' ORDER BY combo_data_human"
        Dim cmd As New MySqlCommand(strGetDevices, cn_global)
        Dim row As Integer
        reader = cmd.ExecuteReader
        ReDim OSType(0)
        row = -1
        With reader
            Do While .Read()
                row = row + 1
                ReDim Preserve OSType(row)
                OSType(row).strID = !combo_ID
                OSType(row).strLong = !combo_data_human
                OSType(row).strShort = !combo_data_db
            Loop
        End With
        cn_global.Close()
        Dim i
        For i = 0 To UBound(OSType)
            Debug.Print(i & " - " & OSType(i).strLong)
        Next
    End Sub
    Public Function GetShortEquipType(ByVal index As Integer) As String
        On Error GoTo errs
        Return EquipType(index).strShort
        Exit Function
errs:
        Return ""
    End Function
End Module
