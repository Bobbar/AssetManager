Imports MySql.Data.MySqlClient
Public Class clsMySQL_Comms
    Private Const strDatabase As String = "asset_manager"
    Private Const EncMySqlPass As String = "N9WzUK5qv2gOgB1odwfduM13ISneU/DG"
    Private MySQLConnectString As String = "server=" & strServerIP & ";uid=asset_mgr_usr;pwd=" & DecodePassword(EncMySqlPass) & ";database=" & strDatabase
    Public Connection As MySqlConnection = NewConnection()
    'Sub New()
    '    If Not OpenConnection() Then

    '    End If

    'End Sub
    Public Function Return_SQLTable(strSQLQry As String) As DataTable
        'Debug.Print("Table Hit " & Date.Now.Ticks)
        Dim ds As New DataSet
        Dim da As New MySqlDataAdapter
        Try
            da.SelectCommand = New MySqlCommand(strSQLQry)
            da.SelectCommand.Connection = Connection
            da.Fill(ds)
            da.Dispose()
            Return ds.Tables(0)
        Catch ex As Exception
            da.Dispose()
            ds.Dispose()
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function Return_SQLReader(strSQLQry As String) As MySqlDataReader
        'Debug.Print("Reader Hit " & Date.Now.Ticks)
        Try
            Dim cmd As New MySqlCommand(strSQLQry, Connection)
            Return cmd.ExecuteReader
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function Return_SQLCommand(strSQLQry As String) As MySqlCommand
        'Debug.Print("Command Hit " & Date.Now.Ticks)
        Try
            Dim cmd As New MySqlCommand
            cmd.Connection = Connection
            cmd.CommandText = strSQLQry
            Return cmd
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function Return_Adapter(strSQLQry As String) As MySqlDataAdapter
        'Debug.Print("Command Hit " & Date.Now.Ticks)
        Try
            Dim adapter As New MySqlDataAdapter(strSQLQry, MySQLConnectString)

            Return adapter
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function ConnectionReady() As Boolean
        Select Case SQLComms.Connection.State
            Case ConnectionState.Closed
                Return False
            Case ConnectionState.Open
                Return True
            Case ConnectionState.Connecting
                Return False
            Case Else
                Return False
        End Select
    End Function
    Public Function CheckConnection() As Boolean
        Try
            Dim ds As New DataSet
            Dim da As New MySqlDataAdapter
            Dim rows As Integer
            da.SelectCommand = New MySqlCommand("SELECT NOW()")
            da.SelectCommand.Connection = Connection
            da.Fill(ds)
            rows = ds.Tables(0).Rows.Count
            If rows > 0 Then
                Return True
            Else
                Return False
            End If
            Exit Function
        Catch ex As MySqlException
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
    End Function
    Public Function NewConnection() As MySqlConnection
        Return New MySqlConnection(MySQLConnectString)
    End Function
    Public Function OpenConnection() As Boolean
        Try
            If Connection.State <> ConnectionState.Open Then
                CloseConnection() '(Connection)
                Connection = NewConnection()
            End If
            Connection.Open()
            If Connection.State = ConnectionState.Open Then
                Return True
            Else
                Return False
            End If
        Catch ex As MySqlException
            Logger("ERROR:  MethodName=" & System.Reflection.MethodInfo.GetCurrentMethod().Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
            Return False
        End Try
    End Function
    Public Sub CloseConnection() 'ByRef conn As MySqlConnection)
        Connection.Close()
        Connection.Dispose()
        'conn.Close()
        'conn.Dispose()
    End Sub

End Class
