Imports MySql.Data.MySqlClient
Public Module MySQLComms
    Public strDatabase As String = "asset_manager"
    Public MySQLConnectString As String = "server=" & strServerIP & ";uid=asset_mgr_usr;pwd=" & DecodePassword(EncMySqlPass) & ";database=" & strDatabase
    Public GlobalConn As New MySqlConnection(MySQLConnectString)
    Public LiveConn As New MySqlConnection(MySQLConnectString)
    Public NotInheritable Class Entry_Type
        Public Const Sibi As String = "sibi_"
        Public Const Device As String = "dev_"
    End Class
    Public Function Return_SQLTable(strSQLQry As String) As DataTable
        'Debug.Print("Table Hit " & Date.Now.Ticks)
        Dim ds As New DataSet
        Dim da As New MySqlDataAdapter
        Try
            da.SelectCommand = New MySqlCommand(strSQLQry)
            da.SelectCommand.Connection = GlobalConn
            da.Fill(ds)
            da.Dispose()
            Return ds.Tables(0)
        Catch ex As Exception
            da.Dispose()
            ds.Dispose()
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function Return_SQLReader(strSQLQry As String) As MySqlDataReader
        'Debug.Print("Reader Hit " & Date.Now.Ticks)
        Try
            Dim cmd As New MySqlCommand(strSQLQry, GlobalConn)
            Return cmd.ExecuteReader
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function Return_SQLCommand(strSQLQry As String) As MySqlCommand
        'Debug.Print("Command Hit " & Date.Now.Ticks)
        Try
            Dim cmd As New MySqlCommand
            cmd.Connection = GlobalConn
            cmd.CommandText = strSQLQry
            Return cmd
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function Update_SQLValue(table As String, fieldIN As String, valueIN As String, idField As String, idValue As String) As Integer
        Try
            Dim sqlUpdateQry As String = "UPDATE " & table & " SET " & fieldIN & "=@ValueIN  WHERE " & idField & "='" & idValue & "'"
            Dim cmd As MySqlCommand = Return_SQLCommand(sqlUpdateQry)
            cmd.Parameters.AddWithValue("@ValueIN", valueIN)
            Return cmd.ExecuteNonQuery()
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function DeleteAttachment(AttachUID As String, Type As String) As Integer
        If Not ConnectionReady() Then
            ConnectionNotReady()
            Exit Function
        End If
        Try
            Dim rows
            Dim reader As MySqlDataReader
            Dim strDeviceID As String
            Dim strSQLIDQry As String
            If Type = Entry_Type.Device Then
                strSQLIDQry = "SELECT attach_dev_UID FROM dev_attachments WHERE attach_file_UID='" & AttachUID & "'"
            ElseIf Type = Entry_Type.Sibi Then
                strSQLIDQry = "SELECT sibi_attach_uid FROM sibi_attachments WHERE sibi_attach_file_UID='" & AttachUID & "'"
            End If
            reader = Return_SQLReader(strSQLIDQry)
            With reader
                Do While .Read()
                    If Type = Entry_Type.Device Then
                        strDeviceID = !attach_dev_UID
                    ElseIf Type = Entry_Type.Sibi Then
                        strDeviceID = !sibi_attach_UID
                    End If
                Loop
            End With
            reader.Close()
            'Delete FTP Attachment
            If DeleteFTPAttachment(AttachUID, strDeviceID) Then
                'delete SQL entry
                Dim strSQLDelQry As String
                If Type = Entry_Type.Device Then
                    strSQLDelQry = "DELETE FROM dev_attachments WHERE attach_file_UID='" & AttachUID & "'"
                ElseIf Type = Entry_Type.Sibi Then
                    strSQLDelQry = "DELETE FROM sibi_attachments WHERE sibi_attach_file_UID='" & AttachUID & "'"
                End If
                rows = Return_SQLCommand(strSQLDelQry).ExecuteNonQuery
                Return rows
                'Else  'if file not found then we might as well remove the DB record.
                '    Dim strSQLDelQry As String = "DELETE FROM dev_attachments WHERE attach_file_UID='" & AttachUID & "'"
                '    cmd.Connection = GlobalConn
                '    cmd.CommandText = strSQLDelQry
                '    rows = cmd.ExecuteNonQuery()
                '    Return rows
            End If
            Exit Function
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Exit Try
            Else
                EndProgram()
            End If
        End Try
        Return -1
    End Function
    Public Function Has_Attachments(strGUID As String, Type As String) As Boolean
        Try
            Dim reader As MySqlDataReader
            Dim strQRY As String
            Select Case Type
                Case Entry_Type.Device
                    strQRY = "SELECT attach_dev_UID FROM dev_attachments WHERE attach_dev_UID='" & strGUID & "'"
                Case Entry_Type.Sibi
                    strQRY = "SELECT sibi_attach_uid FROM sibi_attachments WHERE sibi_attach_uid='" & strGUID & "'"
            End Select
            reader = Return_SQLReader(strQRY)
            Dim bolHasRows As Boolean = reader.HasRows
            reader.Close()
            Return bolHasRows
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                EndProgram()
            End If
            Return Nothing
        End Try
    End Function
    Public Function Get_SQLValue(table As String, fieldIN As String, valueIN As String, fieldOUT As String) As String
        Dim sqlQRY As String = "SELECT " & fieldOUT & " FROM " & table & " WHERE " & fieldIN & " = '" & valueIN & "' LIMIT 1"
        Debug.Print(sqlQRY)
        Try
            Dim cmd As New MySqlCommand
            cmd.Connection = GlobalConn
            cmd.CommandText = sqlQRY
            Return Convert.ToString(cmd.ExecuteScalar)
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function Get_EntryInfo(ByVal strGUID As String) As Device_Info
        Try
            If Not ConnectionReady() Then
                Exit Function
            End If
            Dim tmpInfo As Device_Info
            Dim reader As MySqlDataReader
            Dim strQry = "SELECT * FROM dev_historical WHERE hist_uid='" & strGUID & "'"
            reader = Return_SQLReader(strQry)
            With reader
                Do While .Read()
                    tmpInfo.Historical.strChangeType = GetHumanValue(ComboType.ChangeType, !hist_change_type)
                    tmpInfo.strAssetTag = !hist_asset_tag
                    tmpInfo.strCurrentUser = !hist_cur_user
                    tmpInfo.strSerial = !hist_serial
                    tmpInfo.strDescription = !hist_description
                    tmpInfo.Historical.dtActionDateTime = !hist_action_datetime
                    tmpInfo.Historical.strActionUser = !hist_action_user
                Loop
            End With
            reader.Close()
            Return tmpInfo
            Exit Function
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Exit Try
            Else
                EndProgram()
            End If
        End Try
    End Function
    Public Function Get_DeviceUID(ByVal AssetTag As String, ByVal Serial As String) As String
        Dim reader As MySqlDataReader
        Dim UID As String
        Dim strQry = "SELECT dev_UID from devices WHERE dev_asset_tag = '" & AssetTag & "' AND dev_serial = '" & Serial & "' ORDER BY dev_input_datetime"
        reader = Return_SQLReader(strQry)
        With reader
            Do While .Read()
                UID = (!dev_UID)
            Loop
        End With
        reader.Close()
        Return UID
    End Function
    Public Function Delete_SQLMasterEntry(ByVal strGUID As String, Type As String) As Integer
        Try
            Dim rows
            Dim strSQLQry As String
            Select Case Type
                Case Entry_Type.Device
                    strSQLQry = "DELETE FROM devices WHERE dev_UID='" & strGUID & "'"
                Case Entry_Type.Sibi
                    strSQLQry = "DELETE FROM sibi_requests WHERE sibi_uid='" & strGUID & "'"
            End Select
            rows = Return_SQLCommand(strSQLQry).ExecuteNonQuery
            Return rows
            Exit Function
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
            Else
                EndProgram()
            End If
        End Try
    End Function
    Public Function BuildIndex(CodeType As String, TypeName As String) As Combo_Data()
        Try
            Dim tmpArray() As Combo_Data
            Dim reader As MySqlDataReader
            Dim strQRY = "SELECT * FROM " & CodeType & " WHERE type_name ='" & TypeName & "' ORDER BY human_value"
            Dim row As Integer
            reader = Return_SQLReader(strQRY)
            ReDim tmpArray(0)
            row = -1
            With reader
                Do While .Read()
                    row += 1
                    ReDim Preserve tmpArray(row)
                    tmpArray(row).strID = !id
                    tmpArray(row).strLong = !human_value
                    tmpArray(row).strShort = !db_value
                Loop
            End With
            reader.Close()
            Return tmpArray
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Return Nothing
            Else
                EndProgram()
            End If
        End Try
    End Function
    Public Function CheckConnection() As Boolean
        Try
            Dim ds As New DataSet
            Dim da As New MySqlDataAdapter
            Dim rows As Integer
            da.SelectCommand = New MySqlCommand("SELECT NOW()")
            da.SelectCommand.Connection = GlobalConn
            da.Fill(ds)
            rows = ds.Tables(0).Rows.Count
            If rows > 0 Then
                Return True
            Else
                Return False
            End If
            Exit Function
        Catch ex As MySqlException
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
    End Function
    Public Function FindDevice(Optional AssetTag As String = "", Optional Serial As String = "") As Device_Info
        If AssetTag IsNot "" Then
            Return CollectDeviceInfo(Return_SQLTable("SELECT * FROM devices WHERE dev_asset_tag='" & AssetTag & "'"))
        ElseIf Serial IsNot "" Then
            Return CollectDeviceInfo(Return_SQLTable("SELECT * FROM devices WHERE dev_serial='" & Serial & "'"))
        End If
    End Function
End Module
