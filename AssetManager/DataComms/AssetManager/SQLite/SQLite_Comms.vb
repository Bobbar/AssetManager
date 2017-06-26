Imports System.Data.SQLite
Imports System.IO
Public Class SQLite_Comms : Implements IDisposable
    Private SQLiteConnectString As String = "Data Source=" & strSQLitePath
    Private ConnectionException As Exception
    Public Connection As SQLiteConnection ' = NewConnection()
    Sub New(Optional OpenConnectionOnCall As Boolean = True)
        If OpenConnectionOnCall Then
            If Not OpenConnection() Then
                Throw ConnectionException 'If cannot connect, collect the exact exception and pass it to the referencing object
                Dispose()
            End If
        End If
    End Sub
    Public Function GetSchemaVersion() As Integer
        Using cmd As New SQLiteCommand("pragma schema_version;")
            cmd.Connection = Connection
            Return CInt(cmd.ExecuteScalar)
        End Using
    End Function
    Private Function Return_SQLTable(strSQLQry As String) As DataTable
        Try
            Using da As New SQLiteDataAdapter, tmpTable As New DataTable
                da.SelectCommand = New SQLiteCommand(strSQLQry)
                da.SelectCommand.Connection = Connection
                da.Fill(tmpTable)
                Return tmpTable
            End Using
        Catch ex As Exception
            '  ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
    Private Function GetRemoteDBTable(TableName As String) As DataTable
        Try
            Dim qry As String = "SELECT * FROM " & TableName
            Using conn As New MySQL_Comms, results As New DataTable, adapter = conn.Return_Adapter(qry)
                adapter.AcceptChangesDuringFill = False
                adapter.Fill(results)
                results.TableName = TableName
                Return results
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Public Sub RefreshSQLCache()
        Try
            If SQLiteTableHashes IsNot Nothing AndAlso CheckLocalCacheHash() Then Exit Sub

            Logger("Rebuilding local DB cache...")
            CloseConnection()
            GC.Collect()
            If Not File.Exists(strSQLiteDir) Then
                Dim di As DirectoryInfo = Directory.CreateDirectory(strSQLiteDir)
            End If
            If File.Exists(strSQLitePath) Then
                File.Delete(strSQLitePath)
            End If
            SQLiteConnection.CreateFile(strSQLitePath)
            Connection = NewConnection()
            OpenConnection()
            Using trans = Connection.BeginTransaction
                For Each table In TableList()
                    AddTable(table, trans)
                Next
                trans.Commit()
            End Using
            SQLiteTableHashes = LocalTableHashList()
            RemoteTableHashes = RemoteTableHashList()
            Logger("Local DB cache complete...")
        Catch ex As Exception
            Logger("Errors during cache rebuild!")
            Logger("STACK TRACE: " & ex.ToString)
        End Try
    End Sub
    Public Function CheckLocalCacheHash() As Boolean
        Dim RemoteHashes As New List(Of String)
        RemoteHashes = RemoteTableHashList()
        Return CompareTableHashes(RemoteHashes, SQLiteTableHashes)
    End Function
    Public Function CompareTableHashes(TableHashesA As List(Of String), TableHashesB As List(Of String)) As Boolean
        Try
            If TableHashesA Is Nothing Or TableHashesB Is Nothing Then
                Return False
            End If
            For i As Integer = 0 To TableHashesA.Count - 1
                If TableHashesA(i) <> TableHashesB(i) Then Return False
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Function TableList() As List(Of String)
        Dim list As New List(Of String)
        list.Add(devices.TableName)
        list.Add(historical_dev.TableName)
        list.Add(trackable.TableName)
        list.Add(sibi_requests.TableName)
        list.Add(sibi_request_items.TableName)
        list.Add(sibi_notes.TableName)
        list.Add(dev_codes.TableName)
        list.Add(sibi_codes.TableName)
        list.Add("munis_codes")
        list.Add(security.TableName)
        list.Add(users.TableName)
        Return list
    End Function
    Public Function RemoteTableHashList() As List(Of String)
        Dim hashList As New List(Of String)
        Using MySQLConn As New MySQL_Comms
            For Each table In TableList()
                Using results = ToStringTable(MySQLConn.Return_SQLTable("SELECT * FROM " & table))
                    results.TableName = table
                    hashList.Add(GetHashOfTable(results))
                End Using
            Next
            Return hashList
        End Using
    End Function
    Public Function LocalTableHashList() As List(Of String)
        Try
            Dim hashList As New List(Of String)
            For Each table In TableList()
                Using results = ToStringTable(Return_SQLTable("SELECT * FROM " & table))
                    results.TableName = table
                    hashList.Add(GetHashOfTable(results))
                End Using
            Next
            Return hashList
        Catch ex As Exception
            Return Nothing
        End Try
    End Function
    Private Function ToStringTable(ByRef Table As DataTable) As DataTable
        Dim tmpTable As DataTable = Table.Clone
        For i = 0 To tmpTable.Columns.Count - 1
            tmpTable.Columns(i).DataType = GetType(String)
        Next
        For Each row As DataRow In Table.Rows
            tmpTable.ImportRow(row)
        Next
        Table.Dispose()
        Return tmpTable
    End Function
    Private Sub AddTable(TableName As String, Transaction As SQLiteTransaction)
        CreateCacheTable(TableName, Transaction)
        ImportDatabase(TableName, Transaction)
    End Sub
    Private Sub CreateCacheTable(TableName As String, Transaction As SQLiteTransaction)
        Try
            Dim Statement = GetTableCreateStatement(TableName)
            Dim qry As String = ConvertStatement(Statement)
            Using cmd As New SQLiteCommand(qry, Connection)
                cmd.Transaction = Transaction
                cmd.ExecuteNonQuery()
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Function GetTableCreateStatement(TableName As String) As String
        Dim qry As String = "SHOW CREATE TABLE " & TableName
        Using conn As New MySQL_Comms, results = conn.Return_SQLTable(qry)
            Return results.Rows(0).Item(1).ToString
        End Using
    End Function
    Private Sub ImportDatabase(TableName As String, Transaction As SQLiteTransaction)
        Try
            OpenConnection()
            Using cmd = Connection.CreateCommand, adapter = New SQLiteDataAdapter(cmd), builder As New SQLiteCommandBuilder(adapter)
                cmd.Transaction = Transaction
                cmd.CommandText = "SELECT * FROM " & TableName
                adapter.Update(GetRemoteDBTable(TableName))
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Public Function OpenConnection() As Boolean
        Try
            If Connection Is Nothing Then
                Connection = NewConnection()
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
        Catch ex As Exception
            ConnectionException = ex
            Debug.Print(ex.Message)
            Return False
        End Try
    End Function
    Public Sub CloseConnection()
        If Connection IsNot Nothing Then
            Connection.Close()
            Connection.Dispose()
        End If
    End Sub
    Public Function NewConnection() As SQLiteConnection
        Return New SQLiteConnection(SQLiteConnectString)
    End Function
    ''' <summary>
    ''' Converts MySQL Create statement into a SQL compatible one.
    ''' </summary>
    ''' <param name="Input"></param>
    ''' <returns></returns>
    Private Function ConvertStatement(Input As String) As String
        Dim ColumnDefs As New List(Of String)
        Dim RemoveItems As New List(Of String)
        Dim key As String = ""
        Dim NewStatement As String = ""
        Dim NewKeyString As String = ""
        Dim KeyStringIndex As Integer

        'Remove incompatible defs
        Input = Replace(Input, "AUTO_INCREMENT", "")



        'Split the statement by commas
        ColumnDefs = Split(Input, ",").ToList

        'Find the primary key column name
        For Each item In ColumnDefs
            If item.Contains("PRIMARY KEY") Then
                key = Split(item, "`")(1)
            End If
        Next


        'Find incompatible elements
        For Each item In ColumnDefs
            If item.Contains("PRIMARY") Or item.Contains("UNIQUE") Or item.Contains("ENGINE") Then
                RemoveItems.Add(item)
            End If
        Next


        'Remove incompatible elements
        For Each item In RemoveItems
            ColumnDefs.Remove(item)
        Next




        For Each item In ColumnDefs
            If item.Contains(key) Then 'Find primary key location
                KeyStringIndex = ColumnDefs.IndexOf(item)
                If item.Contains("CREATE") Then 'If the key is at the start of the statement, add all the correct syntax
                    Dim firstDef = Split(Replace(item, vbLf, ""), " ")
                    NewKeyString = firstDef(0) & " " & firstDef(1) & " " & firstDef(2) & " " & firstDef(3) & " " & firstDef(5) & " " & firstDef(6) & " PRIMARY KEY"
                Else
                    Dim keyString = Split(Replace(item, vbLf, ""), " ")
                    NewKeyString = " " & keyString(2) & " " & keyString(3) & " PRIMARY KEY"
                End If
            End If
        Next

        'Modify the key element with corrected syntax
        ColumnDefs(KeyStringIndex) = NewKeyString

        'Rebuild the statement with new syntax
        For Each item In ColumnDefs
            NewStatement += item
            If ColumnDefs.IndexOf(item) <> ColumnDefs.Count - 1 Then NewStatement += ","
        Next

        'Add closing parentheses
        NewStatement += ")"

        '  Debug.Print(NewStatement)

        Return NewStatement
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
