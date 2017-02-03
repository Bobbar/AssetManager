Imports MySql.Data.MySqlClient
Public Class clsMySQL_Comms : Implements IDisposable
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).

            End If
            CloseConnection()
            MySQLConnectString = vbNullString
            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    Protected Overrides Sub Finalize()
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(False)
        MyBase.Finalize()
    End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        GC.SuppressFinalize(Me)
    End Sub
#End Region
    Private Const strDatabase As String = "test_db" ' "asset_manager"
    Private Const EncMySqlPass As String = "N9WzUK5qv2gOgB1odwfduM13ISneU/DG"
    Private MySQLConnectString As String = "server=" & strServerIP & ";uid=asset_mgr_usr;pwd=" & DecodePassword(EncMySqlPass) & ";database=" & strDatabase & ";ConnectionTimeout=5"
    Private ConnectionException As Exception
    Public Connection As MySqlConnection = NewConnection()
    Sub New()
        If bolServerPinging Then
            If Not OpenConnection() Then
                Throw ConnectionException 'If cannot connect, collect the exact exception and pass it to the referencing object
                Dispose()
            End If
        Else
            Throw New NoPingException
            '  Dispose()
        End If
    End Sub
    Public Function Return_SQLTable(strSQLQry As String) As DataTable
        'Debug.Print("Table Hit " & Date.Now.Ticks)
        Try
            Using ds As New DataSet, da As New MySqlDataAdapter
                da.SelectCommand = New MySqlCommand(strSQLQry)
                da.SelectCommand.Connection = Connection
                da.Fill(ds)
                da.Dispose()
                Return ds.Tables(0)
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function Return_SQLReader(strSQLQry As String) As MySqlDataReader
        'Debug.Print("Reader Hit " & Date.Now.Ticks)
        Try
            Using cmd As New MySqlCommand(strSQLQry, Connection)
                Return cmd.ExecuteReader
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function Return_SQLCommand(strSQLQry As String) As MySqlCommand
        'Debug.Print("Command Hit " & Date.Now.Ticks)
        Try
            Using cmd As New MySqlCommand
                cmd.Connection = Connection
                cmd.CommandText = strSQLQry
                Return cmd
            End Using
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
        Select Case Connection.State
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
            Using ds As New DataSet, da As New MySqlDataAdapter
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
            End Using
        Catch ex As MySqlException
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
    End Function
    Private Function NewConnection() As MySqlConnection
        Return New MySqlConnection(MySQLConnectString)
    End Function
    Public Function OpenConnection() As Boolean
        Try
            If Connection.State <> ConnectionState.Open Then
                CloseConnection()
                Connection = NewConnection()
                Connection.Open()
                '  Else

            End If
            If Connection.State = ConnectionState.Open Then
                Return True
            Else
                Return False
            End If
        Catch ex As MySqlException
            ConnectionException = ex
            Return False
        End Try
    End Function
    Public Sub CloseConnection()
        Connection.Close()
        Connection.Dispose()
    End Sub
End Class

