Imports MySql.Data.MySqlClient
Imports System.Data.SQLite
Imports System.Data.Common
Public Class DBWrapper
    Public Function DataTableFromQueryString(Query As String) As DataTable
        Try
            Using results As New DataTable, da As DbDataAdapter = GetAdapter(), cmd = GetCommand(Query), conn = GetConnection()
                cmd.Connection = conn
                da.SelectCommand = cmd
                da.Fill(results)
                Return results
                da.SelectCommand.Connection.Dispose()
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Public Function ExecuteScalarFromQueryString(Query As String) As Object
        Try
            Using cmd = GetCommand(Query), conn = GetConnection()
                cmd.Connection = conn
                cmd.Connection.Open()
                Return cmd.ExecuteScalar
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Public Function DataTableFromCommand(ByRef Command As DbCommand) As DataTable
        Try
            Using da As DbDataAdapter = GetAdapter(), results As New DataTable, conn = GetConnection()
                Command.Connection = conn
                da.SelectCommand = Command
                da.Fill(results)
                Command.Dispose()
                Return results
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Public Function GetCommand(Optional QryString As String = "") As DbCommand
        Try
            If OfflineMode Then
                Return New SQLiteCommand(QryString)
            Else
                Return New MySqlCommand(QryString)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
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
                Dim SQLiteComms As New SQLite_Comms(False)
                Return SQLiteComms.NewConnection
            Else
                Dim MySQLComms As New MySQL_Comms(False)
                Return MySQLComms.NewConnection
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
End Class
