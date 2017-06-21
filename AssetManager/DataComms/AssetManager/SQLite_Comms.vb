Imports System.Data.SQLite
Imports System.IO
Public Class SQLite_Comms : Implements IDisposable
    Private SQLiteConnectString As String = "Data Source=" & strSQLitePath
    Private ConnectionException As Exception
    Public Connection As SQLiteConnection = NewConnection()
    Sub New(Optional OpenConnectionOnCall As Boolean = True)
        If OpenConnectionOnCall Then
            If Not OpenConnection() Then
                Throw ConnectionException 'If cannot connect, collect the exact exception and pass it to the referencing object
                Dispose()
            End If
        End If
    End Sub
    Private Function GetRemoteDBTable() As DataTable
        Try
            Dim qry As String = "SELECT * FROM devices"
            Using conn As New MySQL_Comms, results As New DataTable, adapter = conn.Return_Adapter(qry)
                adapter.AcceptChangesDuringFill = False
                adapter.Fill(results)
                results.TableName = "devices"
                Return results
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Public Sub RefreshSQLCache()
        Try
            Logger("Rebuilding local DB cache...")
            CloseConnection()
            If File.Exists(strSQLiteDir) Then
                File.Delete(strSQLitePath)
            Else
                Dim di As DirectoryInfo = Directory.CreateDirectory(strSQLiteDir)
            End If
            SQLiteConnection.CreateFile(strSQLitePath)
            Connection = NewConnection()
            OpenConnection()
            CreateCacheTables()
            ImportDatabase()
            Logger("Local DB cache complete...")
        Catch ex As Exception
            Logger("Errors during cache rebuild!")
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub CreateCacheTables()
        Try
            Dim qry As String = "CREATE TABLE `devices` (
  `dev_UID` varchar(50) PRIMARY KEY,
  `dev_description` varchar(45) NOT NULL,
  `dev_location` varchar(5) NOT NULL,
  `dev_cur_user` varchar(45) NOT NULL,
  `dev_serial` varchar(45) NOT NULL,
  `dev_asset_tag` varchar(45) NOT NULL,
  `dev_purchase_date` date NOT NULL,
  `dev_replacement_year` varchar(4) DEFAULT NULL,
  `dev_po` varchar(45) DEFAULT '0000',
  `dev_osversion` varchar(7) NOT NULL,
  `dev_phone_number` varchar(10) DEFAULT NULL,
  `dev_lastmod_user` varchar(45) DEFAULT NULL,
  `dev_lastmod_date` datetime DEFAULT NULL,
  `dev_eq_type` varchar(5) NOT NULL,
  `dev_input_datetime` timestamp NOT NULL DEFAULT CURRENT_TIMESTAMP,
  `dev_status` varchar(5) NOT NULL,
  `dev_trackable` tinyint(1) NOT NULL DEFAULT '0',
  `dev_checkedout` tinyint(1) NOT NULL DEFAULT '0',
  `dev_sibi_link` varchar(45) DEFAULT NULL,
  `dev_cur_user_emp_num` varchar(45) DEFAULT NULL,
  `dev_checksum` varchar(45) NOT NULL DEFAULT '0')"
            Using cmd As New SQLiteCommand(qry, Connection)
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub ImportDatabase()
        Try
            OpenConnection()
            Using cmd = Connection.CreateCommand, adapter = New SQLiteDataAdapter(cmd), builder As New SQLiteCommandBuilder(adapter), trans = Connection.BeginTransaction
                cmd.Transaction = trans
                cmd.CommandText = "SELECT * FROM devices"
                adapter.Update(GetRemoteDBTable)
                trans.Commit()
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Public Function OpenConnection() As Boolean
        Try
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
        Catch ex As Exception
            Debug.Print(ex.Message)
            Return False
        End Try
    End Function
    Public Sub CloseConnection()
        Connection.Close()
        Connection.Dispose()
    End Sub
    Private Function NewConnection() As SQLiteConnection
        Return New SQLiteConnection(SQLiteConnectString)
    End Function
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If
            CloseConnection()

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub
#End Region
End Class
