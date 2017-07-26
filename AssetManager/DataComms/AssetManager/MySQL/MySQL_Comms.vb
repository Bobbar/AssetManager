Imports MySql.Data.MySqlClient

Public Class MySQL_Comms : Implements IDisposable

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        GC.SuppressFinalize(Me)
    End Sub

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
#End Region

#Region "Fields"

    Public Property Connection As MySqlConnection
    Private Const EncMySqlPass As String = "N9WzUK5qv2gOgB1odwfduM13ISneU/DG"
    Private Const strDatabase As String = "asset_manager"
    Private Const strTestDatabase As String = "test_db"
    Private ConnectionException As Exception
    Private MySQLConnectString As String = "server=" & strServerIP & ";uid=asset_mgr_usr;pwd=" & DecodePassword(EncMySqlPass) & ";ConnectionTimeout=5;TreatTinyAsBoolean=false;database="

#End Region

#Region "Constructors"

    Sub New()
        If ServerPinging Then
            If Not OpenConnection() Then
                Throw ConnectionException 'If cannot connect, collect the exact exception and pass it to the referencing object
                Dispose()
            End If
        Else
            Throw New NoPingException
            '  Dispose()
        End If
    End Sub

    Sub New(OpenConnectionOnCall As Boolean)
        If OpenConnectionOnCall Then
            If Not OpenConnection() Then
                Throw ConnectionException 'If cannot connect, collect the exact exception and pass it to the referencing object
                Dispose()
            End If
        Else
        End If
    End Sub

#End Region

#Region "Methods"

    Public Sub CloseConnection()
        If Connection IsNot Nothing Then
            Connection.Close()
            Connection.Dispose()
        End If

    End Sub

    Public Function NewConnection() As MySqlConnection
        Return New MySqlConnection(GetConnectString)
    End Function

    Public Function OpenConnection() As Boolean
        Try
            If Connection Is Nothing Then
                Connection = NewConnection()
                Connection.Open()
            End If
            If Connection.State <> ConnectionState.Open Then
                CloseConnection()
                Connection = NewConnection()
                Connection.Open()
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

    Public Function Return_Adapter(strSQLQry As String) As MySqlDataAdapter
        '  Debug.Print("Adapter Hit " & Date.Now.Ticks)
        Dim adapter As New MySqlDataAdapter(strSQLQry, GetConnectString)
        Dim CmdBuilder As New MySqlCommandBuilder(adapter)
        Return adapter
    End Function

    Public Function Return_SQLCommand(Optional strSQLQry As String = "") As MySqlCommand
        ' Debug.Print("Command Hit " & Date.Now.Ticks)
        Using cmd As New MySqlCommand
            cmd.Connection = Connection
            cmd.CommandText = strSQLQry
            Return cmd
        End Using
    End Function

    Public Function Return_SQLReader(strSQLQry As String) As MySqlDataReader
        '  Debug.Print("Reader Hit " & Date.Now.Ticks)
        Using cmd As New MySqlCommand(strSQLQry, Connection)
            Return cmd.ExecuteReader
        End Using
    End Function

    Public Function Return_SQLTable(strSQLQry As String) As DataTable
        ' Debug.Print("Table Hit " & Date.Now.Ticks)
        Using da As New MySqlDataAdapter, tmpTable As New DataTable, cmd = New MySqlCommand(strSQLQry)
            da.SelectCommand = cmd
            da.SelectCommand.Connection = Connection
            da.Fill(tmpTable)
            Return tmpTable
        End Using
    End Function
    Private Function GetConnectString() As String
        If Not bolUseTestDatabase Then
            CurrentDB = strDatabase
            Return MySQLConnectString & strDatabase
        Else
            CurrentDB = strTestDatabase
            Return MySQLConnectString & strTestDatabase
        End If
    End Function

#End Region

End Class