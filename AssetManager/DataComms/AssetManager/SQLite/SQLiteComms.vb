Imports System.Data.Common
Imports System.Data.SQLite
Imports System.IO

Public Class SQLiteDatabase
    Implements IDisposable
    Implements IDataBase

#Region "Fields"
    Private Const EncSQLitePass As String = "X9ow0zCwpGKyVeFR6K3yB4A7lQ2HgOgU"
    Private Property Connection As SQLiteConnection
    Private SQLiteConnectString As String = "Data Source=" & strSQLitePath & ";Password=" & DecodePassword(EncSQLitePass)

#End Region

#Region "Constructors"

    Sub New(Optional openConnectionOnCall As Boolean = True)
        If openConnectionOnCall Then
            If Not OpenConnection() Then
            End If
        End If
    End Sub

#End Region

#Region "Methods"

#Region "Connection Methods"
    Public Sub CloseConnection()
        If Connection IsNot Nothing Then
            Connection.Close()
            Connection.Dispose()
        End If
    End Sub

    Public Function NewConnection() As SQLiteConnection
        Return New SQLiteConnection(SQLiteConnectString)
    End Function

    Public Function OpenConnection() As Boolean
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
    End Function
#End Region

#Region "CacheManagement"

    Public Function CheckLocalCacheHash() As Boolean
        Dim RemoteHashes As New List(Of String)
        RemoteHashes = RemoteTableHashList()
        Return CompareTableHashes(RemoteHashes, SQLiteTableHashes)
    End Function

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

    Public Function LocalTableHashList() As List(Of String)
        Try
            Dim hashList As New List(Of String)
            For Each table In TableList()
                Using results = ToStringTable(DataTableFromQueryString("SELECT * FROM " & table))
                    results.TableName = table
                    hashList.Add(GetHashOfTable(results))
                End Using
            Next
            Return hashList
        Catch ex As Exception
            Return Nothing
        End Try
    End Function

    Public Function RemoteTableHashList() As List(Of String)
        Dim hashList As New List(Of String)
        Using MySQLDB As New MySQLDatabase
            For Each table In TableList()
                Using results = ToStringTable(MySQLDB.DataTableFromQueryString("SELECT * FROM " & table))
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
        Using MySQLDB As New MySQLDatabase, results As New DataTable, conn = MySQLDB.NewConnection, adapter = MySQLDB.ReturnMySqlAdapter(qry, conn)
            adapter.AcceptChangesDuringFill = False
            adapter.Fill(results)
            results.TableName = tableName
            Return results
        End Using
    End Function

    Private Function GetTableCreateStatement(tableName As String) As String
        Dim qry As String = "SHOW CREATE TABLE " & tableName
        Using MySQLDB As New MySQLDatabase, results = MySQLDB.DataTableFromQueryString(qry)
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


#Region "IDataBase"
    Public Function StartTransaction() As DbTransaction Implements IDataBase.StartTransaction
        Throw New NotImplementedException()
    End Function

    Public Function DataTableFromQueryString(query As String) As DataTable Implements IDataBase.DataTableFromQueryString
        Using results As New DataTable, da As DbDataAdapter = New SQLiteDataAdapter, cmd = New SQLiteCommand(query), conn = NewConnection()
            cmd.Connection = conn
            da.SelectCommand = cmd
            da.Fill(results)
            Return results
            da.SelectCommand.Connection.Dispose()
        End Using
    End Function

    Public Function DataTableFromCommand(command As DbCommand) As DataTable Implements IDataBase.DataTableFromCommand
        Using da As DbDataAdapter = New SQLiteDataAdapter, results As New DataTable, conn = NewConnection()
            command.Connection = conn
            da.SelectCommand = command
            da.Fill(results)
            command.Dispose()
            Return results
        End Using
    End Function

    Public Function DataTableFromParameters(query As String, params As List(Of DBQueryParameter)) As DataTable Implements IDataBase.DataTableFromParameters
        Using cmd = GetCommandFromParams(query, params), results = DataTableFromCommand(cmd)
            Return results
        End Using
    End Function

    Public Function ExecuteScalarFromCommand(command As DbCommand) As Object Implements IDataBase.ExecuteScalarFromCommand
        Try
            Using conn = NewConnection()
                command.Connection = conn
                command.Connection.Open()
                Return command.ExecuteScalar
            End Using
        Finally
            command.Dispose()
        End Try
    End Function

    Public Function ExecuteScalarFromQueryString(query As String) As Object Implements IDataBase.ExecuteScalarFromQueryString
        Using conn = NewConnection(), cmd As New SQLiteCommand(query, conn)
            cmd.Connection.Open()
            Return cmd.ExecuteScalar
        End Using
    End Function

    Public Function ExecuteQuery(query As String) As Integer Implements IDataBase.ExecuteQuery
        Throw New NotImplementedException()
    End Function

    Public Function InsertFromParameters(tableName As String, params As List(Of DBParameter), Optional transaction As DbTransaction = Nothing) As Integer Implements IDataBase.InsertFromParameters
        Throw New NotImplementedException()
    End Function

    Public Function UpdateTable(selectQuery As String, table As DataTable, Optional transaction As DbTransaction = Nothing) As Integer Implements IDataBase.UpdateTable
        Throw New NotImplementedException()
    End Function

    Function UpdateValue(tableName As String, fieldIn As String, valueIn As Object, idField As String, idValue As String, Optional transaction As DbTransaction = Nothing) As Integer Implements IDataBase.UpdateValue
        Throw New NotImplementedException()
    End Function

    Public Function GetCommand(Optional qryString As String = "") As DbCommand Implements IDataBase.GetCommand
        Return New SQLiteCommand(qryString)
    End Function

    Public Function GetCommandFromParams(query As String, params As List(Of DBQueryParameter)) As DbCommand Implements IDataBase.GetCommandFromParams
        Dim cmd = New SQLiteCommand
        Dim ParamQuery As String = ""
        For Each param In params
            If TypeOf param.Value Is Boolean Then
                ParamQuery += " " & param.FieldName & "=@" & param.FieldName
                cmd.Parameters.AddWithValue("@" & param.FieldName, Convert.ToInt32(param.Value))
            Else
                If param.IsExact Then
                    ParamQuery += " " & param.FieldName & "=@" & param.FieldName
                    cmd.Parameters.AddWithValue("@" & param.FieldName, param.Value)
                Else
                    ParamQuery += " " & param.FieldName & " LIKE @" & param.FieldName
                    cmd.Parameters.AddWithValue("@" & param.FieldName, "%" & param.Value.ToString & "%")
                End If
            End If
            'Add operator if we are not on the last entry
            If params.IndexOf(param) < params.Count - 1 Then ParamQuery += " " & param.OperatorString
        Next
        cmd.CommandText = query & ParamQuery
        Return cmd
    End Function


#End Region

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