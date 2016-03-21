Option Explicit On

Imports MySql.Data.MySqlClient

Public Module DBFunctions
    Public cn_global As New MySqlConnection("server=localhost;uid=root;pwd=SQLR00tP455W0rd;database=asset_manager")
    Public Structure LocationsStruct
        Public strLocationLong As String
        Public strLocationShort As String
        Public strLocationID As String

    End Structure
    Public Locations() As LocationsStruct

    Public Function DBConnect()

        Debug.Print("DBState: " & cn_global.State)

        'cn_global.Open()


        Debug.Print("DBState: " & cn_global.State)

    End Function

    Public Sub BuildLocationIndex()


        Dim reader As MySqlDataReader
        cn_global.Open()
        Dim strGetDevices = "SELECT * FROM combo_data WHERE combo_type ='LOCATION' ORDER BY combo_data"
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
                Locations(row).strLocationLong = !combo_data
                Locations(row).strLocationShort = !combo_data2






            Loop
        End With
        cn_global.Close()


        Dim i
        For i = 0 To UBound(Locations)
            Debug.Print(i & " - " & Locations(i).strLocationLong)
        Next

    End Sub
    Public Function GetShortLocation(ByVal index As Integer) As String
        Return Locations(index).strLocationShort
    End Function

End Module
