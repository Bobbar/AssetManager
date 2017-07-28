﻿Imports System.Data.Common
Imports System.Data.SQLite
Imports MySql.Data.MySqlClient

''' <summary>
''' This class handles basic DB functions while switching between local and remote databases on the fly based on connectivity
''' </summary>
Public Class DBWrapper

    Public Function DataTableFromQueryString(query As String) As DataTable
        Using results As New DataTable, da As DbDataAdapter = GetAdapter(), cmd = GetCommand(query), conn = GetConnection()
            cmd.Connection = conn
            da.SelectCommand = cmd
            da.Fill(results)
            Return results
            da.SelectCommand.Connection.Dispose()
        End Using
    End Function

    Public Function ExecuteScalarFromQueryString(query As String) As Object
        Using cmd = GetCommand(query), conn = GetConnection()
            cmd.Connection = conn
            cmd.Connection.Open()
            Return cmd.ExecuteScalar
        End Using
    End Function

    Public Function ExecuteScalarFromCommand(command As DbCommand) As Object
        Try
            Using conn = GetConnection()
                command.Connection = conn
                command.Connection.Open()
                Return command.ExecuteScalar
            End Using
        Finally
            command.Dispose()
        End Try
    End Function

    Public Function DataTableFromCommand(command As DbCommand) As DataTable
        Using da As DbDataAdapter = GetAdapter(), results As New DataTable, conn = GetConnection()
            command.Connection = conn
            da.SelectCommand = command
            da.Fill(results)
            command.Dispose()
            Return results
        End Using
    End Function

    Public Function GetCommand(Optional qryString As String = "") As DbCommand
        If OfflineMode Then
            Return New SQLiteCommand(qryString)
        Else
            Return New MySqlCommand(qryString)
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
            Dim SQLiteComms As New SqliteComms(False)
            Return SQLiteComms.NewConnection
        Else
            Dim MySQLComms As New MySqlComms(False)
            Return MySQLComms.NewConnection
        End If
    End Function

End Class