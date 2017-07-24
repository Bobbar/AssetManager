Imports System.Data.Common
Imports System.Data.SQLite
Imports MySql.Data.MySqlClient

''' <summary>
''' This class handles basic DB functions while switching between local and remote databases on the fly based on connectivity
''' </summary>
Public Class DBWrapper

    Public Function DataTableFromQueryString(Query As String) As DataTable
        Using results As New DataTable, da As DbDataAdapter = GetAdapter(), cmd = GetCommand(Query), conn = GetConnection()
            cmd.Connection = conn
            da.SelectCommand = cmd
            da.Fill(results)
            Return results
            da.SelectCommand.Connection.Dispose()
        End Using
    End Function

    Public Function ExecuteScalarFromQueryString(Query As String) As Object
        Using cmd = GetCommand(Query), conn = GetConnection()
            cmd.Connection = conn
            cmd.Connection.Open()
            Return cmd.ExecuteScalar
        End Using
    End Function

    Public Function DataTableFromCommand(Command As DbCommand) As DataTable
        Using da As DbDataAdapter = GetAdapter(), results As New DataTable, conn = GetConnection()
            Command.Connection = conn
            da.SelectCommand = Command
            da.Fill(results)
            Command.Dispose()
            Return results
        End Using
    End Function

    Public Function GetCommand(Optional QryString As String = "") As DbCommand
        If OfflineMode Then
            Return New SQLiteCommand(QryString)
        Else
            Return New MySqlCommand(QryString)
        End If
    End Function

    Public Function GetAdapter() As DbDataAdapter
        If OfflineMode Then
            Return New SQLiteDataAdapter
        Else
            Return New MySqlDataAdapter
        End If
    End Function

    Public Function GetConnection() As DbConnection
        If OfflineMode Then
            Dim SQLiteComms As New SQLite_Comms(False)
            Return SQLiteComms.NewConnection
        Else
            Dim MySQLComms As New MySQL_Comms(False)
            Return MySQLComms.NewConnection
        End If
    End Function

End Class