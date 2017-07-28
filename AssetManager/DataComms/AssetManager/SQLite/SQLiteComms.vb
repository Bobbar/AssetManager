Imports System.Data.SQLite
Imports System.IO

Public Class SqliteComms : Implements IDisposable

#Region "Fields"
    Private Const EncSQLitePass As String = "X9ow0zCwpGKyVeFR6K3yB4A7lQ2HgOgU"
    Public Property Connection As SQLiteConnection
    Private ConnectionException As Exception
    Private SQLiteConnectString As String = "Data Source=" & strSQLitePath & ";Password=" & DecodePassword(EncSQLitePass)

#End Region

#Region "Constructors"

    Sub New(Optional openConnectionOnCall As Boolean = True)
        If openConnectionOnCall Then
            If Not OpenConnection() Then
                Throw ConnectionException 'If cannot connect, collect the exact exception and pass it to the referencing object
                Dispose()
            End If
        End If
    End Sub

#End Region

#Region "Methods"

    Public Function CheckLocalCacheHash() As Boolean
        Dim RemoteHashes As New List(Of String)
        RemoteHashes = RemoteTableHashList()
        Return CompareTableHashes(RemoteHashes, SQLiteTableHashes)
    End Function

    Public Sub CloseConnection()
        If Connection IsNot Nothing Then
            Connection.Close()
            Connection.Dispose()
        End If
    End Sub

    Public Function CompareTableHashes(tableHashesA As List(Of String), tableHashesB As List(Of String)) As Boolean
        Try
            If tableHashesA Is Nothing Or tableHashesB Is Nothing Then
                Return False
            End If
            For i As Integer = 0 To tableHashesA.Count - 1
                If tableHashesA(i) <> tableHashesB(i) Then Return False
            Next
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetSchemaVersion() As Integer
        Using cmd As New SQLiteCommand("pragma schema_version;")
            cmd.Connection = Connection
            Return CInt(cmd.ExecuteScalar)
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

    Public Function NewConnection() As SQLiteConnection
        Return New SQLiteConnection(SQLiteConnectString)
    End Function

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

    Public Sub RefreshSqlCache()
        Try
            If SQLiteTableHashes IsNot Nothing AndAlso CheckLocalCacheHash() Then Exit Sub

            Logger("Rebuilding local DB cache...")
            CloseConnection()
            GC.Collect()
            If Not File.Exists(strSQLiteDir) Then
                Directory.CreateDirectory(strSQLiteDir)
            End If
            If File.Exists(strSQLitePath) Then
                File.Delete(strSQLitePath)
            End If
            SQLiteConnection.CreateFile(strSQLitePath)
            Connection = NewConnection()
            Connection.SetPassword(DecodePassword(EncSQLitePass))
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

    Public Function RemoteTableHashList() As List(Of String)
        Dim hashList As New List(Of String)
        Using MySQLConn As New MySqlComms
            For Each table In TableList()
                Using results = ToStringTable(MySQLConn.ReturnMySqlTable("SELECT * FROM " & table))
                    results.TableName = table
                    hashList.Add(GetHashOfTable(results))
                End Using
            Next
            Return hashList
        End Using
    End Function

    Private Sub AddTable(tableName As String, transaction As SQLiteTransaction)
        CreateCacheTable(tableName, transaction)
        ImportDatabase(tableName, transaction)
    End Sub

    ''' <summary>
    ''' Converts MySQL Create statement into a SQL compatible one.
    ''' </summary>
    ''' <param name="createStatement"></param>
    ''' <returns></returns>
    Private Function ConvertStatement(createStatement As String) As String
        Dim ColumnDefs As New List(Of String)
        Dim RemoveItems As New List(Of String)
        Dim key As String = ""
        Dim NewStatement As String = ""
        Dim NewKeyString As String = ""
        Dim KeyStringIndex As Integer

        'Remove incompatible defs
        createStatement = Replace(createStatement, "AUTO_INCREMENT", "")

        'Split the statement by commas
        ColumnDefs = Split(createStatement, ",").ToList

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

    Private Sub CreateCacheTable(tableName As String, transaction As SQLiteTransaction)
        Dim Statement = GetTableCreateStatement(tableName)
        Dim qry As String = ConvertStatement(Statement)
        Using cmd As New SQLiteCommand(qry, Connection)
            cmd.Transaction = transaction
            cmd.ExecuteNonQuery()
        End Using
    End Sub

    Private Function GetRemoteDBTable(tableName As String) As DataTable
        Dim qry As String = "SELECT * FROM " & tableName
        Using conn As New MySqlComms, results As New DataTable, adapter = conn.ReturnMySqlAdapter(qry)
            adapter.AcceptChangesDuringFill = False
            adapter.Fill(results)
            results.TableName = tableName
            Return results
        End Using
    End Function

    Private Function GetTableCreateStatement(tableName As String) As String
        Dim qry As String = "SHOW CREATE TABLE " & tableName
        Using conn As New MySqlComms, results = conn.ReturnMySqlTable(qry)
            Return results.Rows(0).Item(1).ToString
        End Using
    End Function

    Private Sub ImportDatabase(tableName As String, transaction As SQLiteTransaction)
        OpenConnection()
        Using cmd = Connection.CreateCommand, adapter = New SQLiteDataAdapter(cmd), builder As New SQLiteCommandBuilder(adapter)
            cmd.Transaction = transaction
            cmd.CommandText = "SELECT * FROM " & tableName
            adapter.Update(GetRemoteDBTable(tableName))
        End Using
    End Sub

    Private Function Return_SQLTable(SqlQry As String) As DataTable
        Using da As New SQLiteDataAdapter, tmpTable As New DataTable
            da.SelectCommand = New SQLiteCommand(SqlQry)
            da.SelectCommand.Connection = Connection
            da.Fill(tmpTable)
            Return tmpTable
        End Using
    End Function
    Private Function TableList() As List(Of String)
        Dim list As New List(Of String)
        list.Add(DevicesCols.TableName)
        list.Add(HistoricalDevicesCols.TableName)
        list.Add(TrackablesCols.TableName)
        list.Add(SibiRequestCols.TableName)
        list.Add(SibiRequestItemsCols.TableName)
        list.Add(SibiNotesCols.TableName)
        list.Add(DeviceComboCodesCols.TableName)
        list.Add(SibiComboCodesCols.TableName)
        list.Add("munis_codes")
        list.Add(SecurityCols.TableName)
        list.Add(UsersCols.TableName)
        Return list
    End Function
    Private Function ToStringTable(ByRef table As DataTable) As DataTable
        Dim tmpTable As DataTable = table.Clone
        For i = 0 To tmpTable.Columns.Count - 1
            tmpTable.Columns(i).DataType = GetType(String)
        Next
        For Each row As DataRow In table.Rows
            tmpTable.ImportRow(row)
        Next
        table.Dispose()
        Return tmpTable
    End Function

#End Region

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub

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
#End Region

End Class