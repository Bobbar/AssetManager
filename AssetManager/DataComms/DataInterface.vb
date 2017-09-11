Imports System.Data.Common
Imports MySql.Data.MySqlClient
Imports System.Data.SQLite
Imports System.IO

Public Interface IDataBase

    Function DataTableFromQueryString(query As String) As DataTable
    Function DataTableFromCommand(command As DbCommand) As DataTable
    Function DataTableFromParameters(query As String, params As List(Of DBQueryParameter)) As DataTable
    Function ExecuteScalarFromCommand(command As DbCommand) As Object
    Function InsertFromParameters(tableName As String, params As List(Of DBParameter)) As Integer
    Function UpdateTable(selectQuery As String, table As DataTable) As Integer
    Function UpdateValue(tableName As String, fieldIn As String, valueIn As Object, idField As String, idValue As String) As Integer
    Function GetCommand(Optional qryString As String = "") As DbCommand

End Interface
Public Class MySQLDatabase
    Implements IDataBase
    Implements IDisposable
#Region "Fields"
    Public Property Connection As MySqlConnection
    Private Const EncMySqlPass As String = "N9WzUK5qv2gOgB1odwfduM13ISneU/DG"
    Private Const strDatabase As String = "asset_manager"
    Private Const strTestDatabase As String = "test_db"
    Private ConnectionException As Exception
    Private MySQLConnectString As String = "server=" & ServerInfo.MySQLServerIP & ";uid=asset_mgr_usr;pwd=" & DecodePassword(EncMySqlPass) & ";ConnectionTimeout=5;TreatTinyAsBoolean=false;database="
#End Region

#Region "Constructors"

    Sub New()
        If ServerInfo.ServerPinging Then
            If Not OpenConnection() Then
                Throw ConnectionException 'If cannot connect, collect the exact exception and pass it to the referencing object
                Dispose()
            End If
        Else
            Throw New NoPingException
            '  Dispose()
        End If
    End Sub

    Sub New(openConnectionOnCall As Boolean)
        If openConnectionOnCall Then
            If Not OpenConnection(openConnectionOnCall) Then
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

    Public Function NewConnection(Optional overrideNoPing As Boolean = False) As MySqlConnection
        If ServerInfo.ServerPinging Or overrideNoPing Then
            Return New MySqlConnection(GetConnectString)
        Else
            Throw New NoPingException
        End If
    End Function

    Public Function OpenConnection(Optional overrideNoPing As Boolean = False) As Boolean
        Try
            If Connection Is Nothing Then
                Connection = NewConnection(overrideNoPing)
                Connection.Open()
            End If
            If Connection.State <> ConnectionState.Open Then
                CloseConnection()
                Connection = NewConnection(overrideNoPing)
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

    Private Function GetConnectString() As String
        If Not ServerInfo.UseTestDatabase Then
            ServerInfo.CurrentDataBase = strDatabase
            Return MySQLConnectString & strDatabase
        Else
            ServerInfo.CurrentDataBase = strTestDatabase
            Return MySQLConnectString & strTestDatabase
        End If
    End Function

#End Region


#Region "IDataBase"
    Public Function DataTableFromCommand(command As DbCommand) As DataTable Implements IDataBase.DataTableFromCommand
        Using da As DbDataAdapter = New MySqlDataAdapter, results As New DataTable, conn = NewConnection()
            command.Connection = conn
            da.SelectCommand = command
            da.Fill(results)
            command.Dispose()
            Return results
        End Using
        'Throw New NotImplementedException()
    End Function

    Public Function DataTableFromQueryString(query As String) As DataTable Implements IDataBase.DataTableFromQueryString
        Using results As New DataTable, da As DbDataAdapter = New MySqlDataAdapter, cmd = New MySqlCommand(query), conn = NewConnection()
            cmd.Connection = conn
            da.SelectCommand = cmd
            da.Fill(results)
            Return results
            da.SelectCommand.Connection.Dispose()
        End Using
        ' Throw New NotImplementedException()
    End Function

    Public Function DataTableFromParameters(query As String, params As List(Of DBQueryParameter)) As DataTable Implements IDataBase.DataTableFromParameters
        Using conn = NewConnection(), cmd As New MySqlCommand, da = New MySqlDataAdapter(cmd), results As New DataTable
            cmd.Connection = conn
            'Build query from params
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
            da.Fill(results)
            Return results
        End Using
    End Function

    Public Function InsertFromParameters(tableName As String, params As List(Of DBParameter)) As Integer Implements IDataBase.InsertFromParameters
        Dim SelectQuery As String = "SELECT * FROM " & tableName & " LIMIT 0"
        Using Adapter = New MySqlDataAdapter(SelectQuery, NewConnection), Builder = New MySqlCommandBuilder(Adapter)
            Dim table = DataTableFromQueryString(SelectQuery)
            table.Rows.Add()
            For Each param In params
                table.Rows(0)(param.FieldName) = param.Value
            Next
            Return Adapter.Update(table)
        End Using
    End Function

    Function UpdateValue(tableName As String, fieldIn As String, valueIn As Object, idField As String, idValue As String) As Integer Implements IDataBase.UpdateValue
        Dim sqlUpdateQry As String = "UPDATE " & tableName & " SET " & fieldIn & "=@ValueIN  WHERE " & idField & "='" & idValue & "'"
        Using conn = NewConnection(), cmd As MySqlCommand = New MySqlCommand(sqlUpdateQry, conn)
            conn.Open()
            cmd.Parameters.AddWithValue("@ValueIN", valueIn)
            Return cmd.ExecuteNonQuery()
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

    Public Function GetCommand(Optional qryString As String = "") As DbCommand Implements IDataBase.GetCommand
        Return New MySqlCommand(qryString)
    End Function

    Function UpdateTable(selectQuery As String, table As DataTable) As Integer Implements IDataBase.UpdateTable
        Using Adapter = New MySqlDataAdapter(selectQuery, NewConnection), Builder = New MySqlCommandBuilder(Adapter)
            Return Adapter.Update(table)
        End Using
    End Function


#End Region



#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).

                CloseConnection()
                MySQLConnectString = vbNullString
            End If

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

Public Class SQLiteDatabase
    Implements IDataBase
    Implements IDisposable
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

    Public Sub CloseConnection()
        If Connection IsNot Nothing Then
            Connection.Close()
            Connection.Dispose()
        End If
    End Sub

#End Region


#Region "IDataBase"
    Public Function DataTableFromCommand(command As DbCommand) As DataTable Implements IDataBase.DataTableFromCommand
        Using da As DbDataAdapter = New SQLiteDataAdapter, results As New DataTable, conn = NewConnection()
            command.Connection = conn
            da.SelectCommand = command
            da.Fill(results)
            command.Dispose()
            Return results
        End Using
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

    Public Function DataTableFromParameters(query As String, params As List(Of DBQueryParameter)) As DataTable Implements IDataBase.DataTableFromParameters
        Using conn = NewConnection(), cmd As New SQLiteCommand, da = New SQLiteDataAdapter(cmd), results As New DataTable
            cmd.Connection = conn
            'Build query from params
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
            da.Fill(results)
            Return results
        End Using
    End Function

    Public Function InsertFromParameters(query As String, params As List(Of DBParameter)) As Integer Implements IDataBase.InsertFromParameters
        Throw New NotImplementedException()
    End Function

    Function UpdateValue(tableName As String, fieldIn As String, valueIn As Object, idField As String, idValue As String) As Integer Implements IDataBase.UpdateValue
        Throw New NotImplementedException()
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

    Public Function GetCommand(Optional qryString As String = "") As DbCommand Implements IDataBase.GetCommand
        Return New SQLiteCommand(qryString)
    End Function

    Function UpdateTable(selectQuery As String, table As DataTable) As Integer Implements IDataBase.UpdateTable
        Throw New NotImplementedException()
    End Function
#End Region


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