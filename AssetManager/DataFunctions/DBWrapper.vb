Imports MySql.Data.MySqlClient
Imports System.Data.SQLite
Imports System.Data.Common
Public Class DBWrapper
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
    Public Function DataTableFromCommand(Command As DbCommand) As DataTable
        Try
            Using da As DbDataAdapter = GetAdapter(), results As New DataTable
                da.SelectCommand = Command
                da.Fill(results)
                Return results
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Public Function GetCommand(Optional QryString As String = "") As DbCommand
        Try
            If OfflineMode Then
                Return New SQLiteCommand(QryString, DirectCast(GetConnection(), SQLiteConnection))
            Else
                Return New MySqlCommand(QryString, DirectCast(GetConnection(), MySqlConnection))
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
                Dim SQLiteComms As New SQLite_Comms
                Return SQLiteComms.Connection
            Else
                Using MySQLComms As New MySQL_Comms()
                    Return MySQLComms.Connection
                End Using
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
End Class
