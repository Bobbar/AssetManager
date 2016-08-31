Option Explicit On
Imports MySql.Data.MySqlClient
Module DataImport
    Private Device() As Device_Info
    Public Sub StartImport()
        ParseDevices()
        'Dim Connection As ConnectionData = GetConnection()
        Dim i As Integer
        For i = 0 To UBound(Device)
            'Connection.DBConnection.Open()
            Dim strSqlQry1 = "INSERT INTO devices (dev_description,dev_location,dev_cur_user,dev_serial,dev_asset_tag,dev_purchase_date,dev_replacement_year,dev_eq_type,dev_osversion,dev_lastmod_user,dev_status) VALUES ('" & Device(i).strDescription & "','" & Device(i).strLocation & "','" & Device(i).strCurrentUser & "','" & Device(i).strSerial & "','" & Device(i).strAssetTag & "','" & Device(i).dtPurchaseDate & "','" & Device(i).strReplaceYear & "','" & Device(i).strEqType & "','" & Device(i).strOSVersion & "','" & strLocalUser & "','" & Device(i).strStatus & "')"
            'Debug.Print(strSqlQry1)
            Dim cmd As New MySqlCommand
            cmd.Connection = GlobalConn
            cmd.CommandText = strSqlQry1
            cmd.ExecuteNonQuery()
            'Dim strSqlQry2 = "INSERT INTO historical (hist_change_type,hist_notes,hist_serial,hist_description,hist_location,hist_cur_user,hist_asset_tag,hist_purchase_date,hist_replacement_year,hist_po,hist_osversion,hist_action_user,hist_eq_type) VALUES ('NEWD','IMPORTED','" & Device(i).strSerial & "','" & Device(i).strDescription & "','" & Device(i).strLocation & "','" & Device(i).strCurrentUser & "','" & Device(i).strAssetTag & "','" & Device(i).dtPurchaseDate & "','" & Device(i).strReplaceYear & "','0000','" & Device(i).strOSVersion & "','" & strLocalUser & "','" & Device(i).strEqType & "')"
            'cmd.CommandText = strSqlQry2
            'cmd.ExecuteNonQuery()
            Debug.Print(i & " - " & Device(i).strCurrentUser)
        Next
        Message("Done?! Did it work?")
    End Sub
    Private Sub ParseDevices()
        Dim ConnID As String = Guid.NewGuid.ToString
        'Dim Connection As ConnectionData = GetConnection()
        Dim row As Integer
        Dim reader As MySqlDataReader
        Dim table As New DataTable
        'Connection.DBConnection.Open()
        Dim strQry = "SELECT * FROM device_import"
        Dim cmd As New MySqlCommand(strQry, GlobalConn)
        reader = cmd.ExecuteReader
        ReDim Device(0)
        row = -1
        With reader
            Do While .Read()
                row = row + 1
                ReDim Preserve Device(row)
                If IsDBNull(reader("current_user")) Then
                    Device(row).strCurrentUser = ""
                Else
                    Device(row).strCurrentUser = Trim(!current_user)
                End If
                'Device(row).strAssetTag = IIf(Not IsDBNull(reader("asset_tag")), Trim(!asset_tag), "")
                If IsDBNull(reader("asset_tag")) Then
                    Device(row).strAssetTag = ""
                Else
                    Device(row).strAssetTag = !asset_tag
                End If
                If IsDBNull(reader("serial")) Then
                    Device(row).strSerial = ""
                Else
                    Device(row).strSerial = Trim(!serial)
                End If
                If IsDBNull(reader("description")) Then
                    Device(row).strDescription = ""
                Else
                    Device(row).strDescription = Trim(!description)
                End If
                Dim tmpDesc As String = UCase(Device(row).strDescription)
                Select Case True
                    Case tmpDesc.Contains("OPTI")
                        Device(row).strEqType = "DESK"
                    Case tmpDesc.Contains("POWEREDGE")
                        Device(row).strEqType = "SVRD"
                    Case tmpDesc.Contains("LATITUDE")
                        Device(row).strEqType = "LAPT"
                    Case tmpDesc.Contains("IPAD")
                        Device(row).strEqType = "TAB"
                    Case tmpDesc.Contains("PROJECTOR")
                        Device(row).strEqType = "PROJ"
                    Case tmpDesc.Contains("LAPTOP")
                        Device(row).strEqType = "LAPT"
                    Case tmpDesc.Contains("VENUE")
                        Device(row).strEqType = "TAB"
                    Case Else
                        Device(row).strEqType = "OTHRD"
                End Select
                Dim Location As String = Trim(!location)
                Select Case Location
                    Case "Opportunity Center"
                        Device(row).strLocation = "OC"
                    Case "Forest Rose School"
                        Device(row).strLocation = "FRS"
                    Case "Pickerington Regional"
                        Device(row).strLocation = "PRO"
                    Case "Administration"
                        Device(row).strLocation = "ADM"
                    Case "County Courthouse"
                        Device(row).strLocation = "CCH"
                    Case "Discover U"
                        Device(row).strLocation = "DISU"
                    Case "Art & Clay"
                        Device(row).strLocation = "ANC"
                End Select
                Dim PurDate As Date
                If IsDBNull(reader("purchase_date")) Then
                    Device(row).dtPurchaseDate = "1900-01-01"
                ElseIf reader("purchase_date") = "" Then
                    Device(row).dtPurchaseDate = "1900-01-01"
                Else
                    PurDate = reader("purchase_date")
                    Device(row).dtPurchaseDate = PurDate.ToString(strDBDateFormat)
                End If
                'Device(row).dtPurchaseDate = PurDate.ToString(strDBDateFormat)
                Debug.Print(Device(row).dtPurchaseDate)
                Dim os = reader("osversion")
                If IsDBNull(os) Then
                    Device(row).strOSVersion = ""
                ElseIf os.Contains("iOS") Then
                    Device(row).strOSVersion = "IOSO"
                Else
                    Select Case os
                        Case "Windows"
                            Device(row).strOSVersion = "WINO"
                        Case "Windows 7"
                            Device(row).strOSVersion = "WIN7"
                        Case "Windows 8.1"
                            Device(row).strOSVersion = "WIN81"
                        Case "Windows 8"
                            Device(row).strOSVersion = "WIN8"
                        Case "Windows Server 2008R2"
                            Device(row).strOSVersion = "WSRR2"
                        Case "XP"
                            Device(row).strOSVersion = "WINX"
                        Case Else
                            Device(row).strOSVersion = "OTHR"
                    End Select
                End If
                If IsDBNull(reader("replace_year")) Then
                    Device(row).strReplaceYear = ""
                Else
                    Device(row).strReplaceYear = Trim(!replace_year)
                End If
                Device(row).strStatus = "INSRV"
            Loop
        End With
        'Connection.DBConnection.Close()
        Debug.Print(Device(UBound(Device)).strDescription)
    End Sub
End Module
