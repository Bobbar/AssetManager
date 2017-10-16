Imports System.Data.Common
Imports MySql.Data.MySqlClient

Public Class MySQLDatabase
    Implements IDisposable
    Implements IDataBase

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        '    GC.SuppressFinalize(Me)
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
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

#End Region

#Region "Fields"

    Private Const EncMySqlPass As String = "N9WzUK5qv2gOgB1odwfduM13ISneU/DG"
    Private MySQLConnectString As String = "server=" & ServerInfo.MySQLServerIP & ";uid=asset_mgr_usr;pwd=" & SecurityTools.DecodePassword(EncMySqlPass) & ";ConnectionTimeout=5;TreatTinyAsBoolean=false;database="

#End Region

#Region "Constructors"

    Sub New()

    End Sub

#End Region

#Region "Methods"

    Public Function NewConnection() As MySqlConnection
        Return New MySqlConnection(GetConnectString)
    End Function

    Public Function OpenConnection(connection As MySqlConnection, Optional overrideNoPing As Boolean = False) As Boolean
        If Not ServerInfo.ServerPinging Then 'Server not pinging.
            If overrideNoPing Then 'Ignore server not pinging, try to open anyway.
                Return TryOpenConnection(connection)
            Else 'Throw exception.
                Throw New NoPingException
                Return False
            End If
        Else 'Server is pinging, try to open connection.
            Return TryOpenConnection(connection)
        End If

    End Function

    Private Function TryOpenConnection(connection As MySqlConnection) As Boolean
        If connection Is Nothing Then 'Instantiate new connection.
            connection = NewConnection()
        End If
        If connection.State <> ConnectionState.Open Then 'Try to open connection.
            connection.Open()
        End If
        If connection.State = ConnectionState.Open Then 'Check if connection is open.
            Return True
        Else
            Return False
        End If
    End Function

    Private Function GetConnectString() As String
        Return MySQLConnectString & ServerInfo.CurrentDataBase.ToString
    End Function

    Public Function ReturnMySqlAdapter(sqlQry As String, connection As MySqlConnection) As MySqlDataAdapter
        Return New MySqlDataAdapter(sqlQry, connection)
    End Function

#End Region

#Region "IDataBase"

    Public Function StartTransaction() As DbTransaction Implements IDataBase.StartTransaction
        Dim conn = NewConnection()
        OpenConnection(conn)
        Dim trans = conn.BeginTransaction
        Return trans
    End Function

    Public Function DataTableFromQueryString(query As String) As DataTable Implements IDataBase.DataTableFromQueryString
        Using results As New DataTable, da = New MySqlDataAdapter, cmd = New MySqlCommand(query), conn = NewConnection()
            OpenConnection(conn)
            cmd.Connection = conn
            da.SelectCommand = cmd
            da.Fill(results)
            Return results
        End Using
    End Function

    Public Function DataTableFromCommand(command As DbCommand) As DataTable Implements IDataBase.DataTableFromCommand
        Using da As DbDataAdapter = New MySqlDataAdapter, results As New DataTable, conn = NewConnection()
            OpenConnection(conn)
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
                OpenConnection(conn)
                command.Connection = conn
                Return command.ExecuteScalar
            End Using
        Finally
            command.Dispose()
        End Try
    End Function

    Public Function ExecuteScalarFromQueryString(query As String) As Object Implements IDataBase.ExecuteScalarFromQueryString
        Using conn = NewConnection(), cmd As New MySqlCommand(query, conn)
            OpenConnection(conn)
            Return cmd.ExecuteScalar
        End Using
    End Function

    Public Function ExecuteQuery(query As String) As Integer Implements IDataBase.ExecuteQuery
        Using conn = NewConnection(), cmd As New MySqlCommand(query, conn)
            OpenConnection(conn)
            Return cmd.ExecuteNonQuery
        End Using
    End Function

    Public Function InsertFromParameters(tableName As String, params As List(Of DBParameter), Optional transaction As DbTransaction = Nothing) As Integer Implements IDataBase.InsertFromParameters
        Dim SelectQuery As String = "SELECT * FROM " & tableName & " LIMIT 0"
        If transaction IsNot Nothing Then
            Dim conn = DirectCast(transaction.Connection, MySqlConnection)
            Using cmd = New MySqlCommand(SelectQuery, conn, DirectCast(transaction, MySqlTransaction)), Adapter = New MySqlDataAdapter(cmd), Builder = New MySqlCommandBuilder(Adapter)
                Dim table = DataTableFromQueryString(SelectQuery)
                table.Rows.Add()
                For Each param In params
                    table.Rows(0)(param.FieldName) = param.Value
                Next
                Return Adapter.Update(table)
            End Using
        Else
            Using conn = NewConnection(), Adapter = New MySqlDataAdapter(SelectQuery, conn), Builder = New MySqlCommandBuilder(Adapter)
                OpenConnection(conn)
                Dim table = DataTableFromQueryString(SelectQuery)
                table.Rows.Add()
                For Each param In params
                    table.Rows(0)(param.FieldName) = param.Value
                Next
                Return Adapter.Update(table)
            End Using
        End If
    End Function

    Public Function UpdateTable(selectQuery As String, table As DataTable, Optional transaction As DbTransaction = Nothing) As Integer Implements IDataBase.UpdateTable
        If transaction IsNot Nothing Then
            Dim conn = DirectCast(transaction.Connection, MySqlConnection)
            Using cmd = New MySqlCommand(selectQuery, conn, DirectCast(transaction, MySqlTransaction)), Adapter = New MySqlDataAdapter(cmd), Builder = New MySqlCommandBuilder(Adapter)
                Return Adapter.Update(table)
            End Using
        Else
            Using conn = NewConnection(), Adapter = New MySqlDataAdapter(selectQuery, conn), Builder = New MySqlCommandBuilder(Adapter)
                OpenConnection(conn)
                Return Adapter.Update(table)
            End Using
        End If
    End Function

    Public Function UpdateValue(tableName As String, fieldIn As String, valueIn As Object, idField As String, idValue As String, Optional transaction As DbTransaction = Nothing) As Integer Implements IDataBase.UpdateValue
        Dim sqlUpdateQry As String = "UPDATE " & tableName & " SET " & fieldIn & "=@ValueIN  WHERE " & idField & "='" & idValue & "'"
        If transaction IsNot Nothing Then
            Dim conn = DirectCast(transaction.Connection, MySqlConnection)
            Using cmd = New MySqlCommand(sqlUpdateQry, conn, DirectCast(transaction, MySqlTransaction))
                cmd.Parameters.AddWithValue("@ValueIN", valueIn)
                Return cmd.ExecuteNonQuery()
            End Using
        Else
            Using conn = NewConnection(), cmd As MySqlCommand = New MySqlCommand(sqlUpdateQry, conn)
                OpenConnection(conn)
                cmd.Parameters.AddWithValue("@ValueIN", valueIn)
                Return cmd.ExecuteNonQuery()
            End Using
        End If

    End Function

    Public Function GetCommand(Optional qryString As String = "") As DbCommand Implements IDataBase.GetCommand
        Return New MySqlCommand(qryString)
    End Function

    Public Function GetCommandFromParams(query As String, params As List(Of DBQueryParameter)) As DbCommand Implements IDataBase.GetCommandFromParams
        Dim cmd = New MySqlCommand
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

End Class