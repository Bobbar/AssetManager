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
            AddTable(devices.TableName)
            AddTable(historical_dev.TableName)
            AddTable(trackable.TableName)
            AddTable(sibi_requests.TableName)
            AddTable(sibi_request_items.TableName)
            AddTable(sibi_notes.TableName)
            AddTable(dev_codes.TableName)
            AddTable(sibi_codes.TableName)
            AddTable("munis_codes")
            AddTable(security.TableName)
            AddTable(users.TableName)
            Logger("Local DB cache complete...")
        Catch ex As Exception
            Logger("Errors during cache rebuild!")
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub AddTable(TableName As String)
        CreateCacheTable(TableName)
        ImportDatabase(TableName)

    End Sub
    Private Sub CreateCacheTable(TableName As String)
        Try
            Dim Statement = GetTableCreateStatement(TableName)
            ' Debug.Print(Statement)

            Dim qry As String = ConvertStatement(Statement)
            Using cmd As New SQLiteCommand(qry, Connection)
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
    Private Sub ImportDatabase(TableName As String)
        Try
            OpenConnection()
            Using cmd = Connection.CreateCommand, adapter = New SQLiteDataAdapter(cmd), builder As New SQLiteCommandBuilder(adapter), trans = Connection.BeginTransaction
                cmd.Transaction = trans
                cmd.CommandText = "SELECT * FROM " & TableName
                adapter.Update(GetRemoteDBTable(TableName))
                trans.Commit()
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
