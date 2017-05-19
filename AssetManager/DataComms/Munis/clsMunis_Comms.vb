Imports System.Data.SqlClient
Public Class clsMunis_Comms
    Private Const MSSQLConnectString As String = "server=svr-munis5.core.co.fairfield.oh.us; database=mu_live; trusted_connection=True;"
    Public Function Return_MSSQLTable(strSQLQry As String) As DataTable
        Try
            Using conn As SqlConnection = New SqlConnection(MSSQLConnectString),
                    NewTable As New DataTable,
                    da As New SqlDataAdapter
                da.SelectCommand = New SqlCommand(strSQLQry)
                da.SelectCommand.Connection = conn
                da.Fill(NewTable)
                Return NewTable
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
    Public Async Function Return_MSSQLTableAsync(strSQLQry As String) As Task(Of DataTable)
        Try
            Using conn As SqlConnection = New SqlConnection(MSSQLConnectString),
                    NewTable As New DataTable,
                cmd As New SqlCommand(strSQLQry, conn)
                Await conn.OpenAsync
                Dim dr As SqlDataReader = Await cmd.ExecuteReaderAsync
                NewTable.Load(dr)
                Return NewTable
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
    Public Function Return_MSSQLValue(table As String, fieldIN As String, valueIN As String, fieldOUT As String) As String
        Dim sqlQRY As String = "SELECT TOP 1 " & fieldOUT & " FROM " & table & " WHERE " & fieldIN & " = '" & valueIN & "'"
        Try
            Using cmd As New SqlCommand,
                    conn As SqlConnection = New SqlConnection(MSSQLConnectString)
                cmd.Connection = conn
                cmd.CommandText = sqlQRY
                conn.Open()
                Return Convert.ToString(cmd.ExecuteScalar)
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
    Public Async Function Return_MSSQLValueAsync(table As String, fieldIN As Object, valueIN As Object, fieldOUT As String, Optional fieldIN2 As Object = Nothing, Optional ValueIN2 As Object = Nothing) As Task(Of String)
        Try
            Dim sqlQRY As String
            If fieldIN2 IsNot Nothing And ValueIN2 IsNot Nothing Then
                sqlQRY = "SELECT TOP 1 " & fieldOUT & " FROM " & table & " WHERE " & fieldIN & " = '" & valueIN & "' AND " & fieldIN2 & " = '" & ValueIN2 & "'"
            Else
                sqlQRY = "SELECT TOP 1 " & fieldOUT & " FROM " & table & " WHERE " & fieldIN & " = '" & valueIN & "'"
            End If
            Using conn As SqlConnection = New SqlConnection(MSSQLConnectString),
            cmd As New SqlCommand
                cmd.Connection = conn
                cmd.CommandText = sqlQRY
                Await conn.OpenAsync()
                Dim Value = Await cmd.ExecuteScalarAsync
                If Value IsNot Nothing Then Return Value.ToString
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
        Return Nothing
    End Function
End Class
