Imports MySql.Data.MySqlClient
Imports System.Data.SQLite
Imports System.Data.Common
Public Class DBWrapper
    Public Function GetCommand() As DbCommand
        If OfflineMode Then
            Return New SQLiteCommand
        Else
            Return New MySqlCommand
        End If
    End Function
    Public Function DataTableFromQueryString(Query As String) As DataTable
        Try
            Using conn = GetConnection(), results As New DataTable, da As DbDataAdapter = GetAdapter()
                da.SelectCommand = GetCommand(Query)
                da.Fill(results)
                Return results
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Public Function DataTableFromCommand(ByRef Command As DbCommand) As DataTable
        Try
            Using conn = GetConnection(), results As New DataTable, da As DbDataAdapter = GetAdapter()
                Command.Connection = conn
                da.SelectCommand = Command
                da.Fill(results)
                da.Dispose()
                Command.Dispose()
                Return results
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Public Function GetCommand(QryString As String) As DbCommand
        If OfflineMode Then
            Return New SQLiteCommand(QryString, DirectCast(GetConnection(), SQLiteConnection))
        Else
            Return New MySqlCommand(QryString, DirectCast(GetConnection(), MySqlConnection))
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
        Try
            If OfflineMode Then
                Dim SQLiteComms As New SQLite_Comms
                Return SQLiteComms.Connection
            Else
                Dim MySQLComms As New MySQL_Comms
                Return MySQLComms.Connection
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function

End Class
