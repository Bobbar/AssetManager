Option Explicit On
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
    Public Locations() As LocationsStruct
    Public Structure ChangeTypeStruct
        Public strChangeTypeLong As String
        Public strChangeTypeShort As String
        Public strChangeTypeID As String
    End Structure
    Public ChangeType() As ChangeTypeStruct
    Public Structure EquipTypeStruct
        Public strEquipTypeLong As String
        Public strEquipTypeShort As String
        Public strEquipTypeID As String
    End Structure
    Public EquipType() As EquipTypeStruct
    Public NotInheritable Class ComboType
        Public Const Location As String = "LOCATION"
        Public Const ChangeType As String = "CHANGETYPE"
        Public Const EquipType As String = "EQ_TYPE"
        Public Const OSType As String = "OS_TYPE"
    End Class


    Public Structure OSTypeStruct
        Public strOSTypeLong As String
        Public strOSTypeShort As String
        Public strOSTypeID As String
    End Structure
    Public OSType() As OSTypeStruct
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
                Locations(row).strLocationID = !combo_ID
                Locations(row).strLocationLong = !combo_data_human
                Locations(row).strLocationShort = !combo_data_db
            Loop
        End With
        cn_global.Close()
        Dim i
        For i = 0 To UBound(Locations)
            Debug.Print(i & " - " & Locations(i).strLocationLong)
        Next
    End Sub
    Public Function GetShortLocation(ByVal index As Integer) As String
        On Error GoTo errs
        Return Locations(index).strLocationShort
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
    Public Function GetShortValue(ByVal IndexType As String, ByVal index As Integer) As String
        On Error GoTo errs
        Select Case IndexType
            Case "LOCATION"
                Return Locations(index).strLocationShort
            Case "CHANGETYPE"
                Return ChangeType(index).strChangeTypeShort
            Case "EQ_TYPE"
                Return EquipType(index).strEquipTypeShort
            Case "OS_TYPE"
                Return OSType(index).strOSTypeShort
            Case Else
                Return ""



        End Select



        Exit Function
errs:
        Return ""

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
                ChangeType(row).strChangeTypeID = !combo_ID
                ChangeType(row).strChangeTypeLong = !combo_data_human
                ChangeType(row).strChangeTypeShort = !combo_data_db
            Loop
        End With
        cn_global.Close()
        Dim i
        For i = 0 To UBound(ChangeType)
            Debug.Print(i & " - " & ChangeType(i).strChangeTypeLong)
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
                EquipType(row).strEquipTypeID = !combo_ID
                EquipType(row).strEquipTypeLong = !combo_data_human
                EquipType(row).strEquipTypeShort = !combo_data_db
            Loop
        End With
        cn_global.Close()
        Dim i
        For i = 0 To UBound(EquipType)
            Debug.Print(i & " - " & EquipType(i).strEquipTypeLong)
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
                OSType(row).strOSTypeID = !combo_ID
                OSType(row).strOSTypeLong = !combo_data_human
                OSType(row).strOSTypeShort = !combo_data_db
            Loop
        End With
        cn_global.Close()
        Dim i
        For i = 0 To UBound(OSType)
            Debug.Print(i & " - " & OSType(i).strOSTypeLong)
        Next
    End Sub
    Public Function GetShortEquipType(ByVal index As Integer) As String
        On Error GoTo errs
        Return EquipType(index).strEquipTypeShort
        Exit Function
errs:
        Return ""
    End Function
End Module
