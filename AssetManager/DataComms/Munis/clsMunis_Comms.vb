Imports System.Data.SqlClient
Public Class clsMunis_Comms
    Private Const MSSQLConnectString As String = "server=svr-munis5.core.co.fairfield.oh.us; database=mu_live; trusted_connection=True;"
    Public Function Return_MSSQLTable(strSQLQry As String) As DataTable
        Dim ds As New DataSet
        Dim da As New SqlDataAdapter
        Dim conn As SqlConnection = New SqlConnection(MSSQLConnectString)
        Try
            da.SelectCommand = New SqlCommand(strSQLQry)
            da.SelectCommand.Connection = conn
            da.Fill(ds)
            da.Dispose()
            Return ds.Tables(0)
        Catch ex As Exception
            da.Dispose()
            ds.Dispose()
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
    Public Function Return_MSSQLValue(table As String, fieldIN As String, valueIN As String, fieldOUT As String) As String
        Dim sqlQRY As String = "SELECT TOP 1 " & fieldOUT & " FROM " & table & " WHERE " & fieldIN & " = '" & valueIN & "'"
        Dim conn As SqlConnection = New SqlConnection(MSSQLConnectString)
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandText = sqlQRY
            conn.Open()
            Return Convert.ToString(cmd.ExecuteScalar)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
    Public Async Function Return_MSSQLValueAsync(table As String, fieldIN As String, valueIN As String, fieldOUT As String) As Task(Of String)
        Dim sqlQRY As String = "SELECT TOP 1 " & fieldOUT & " FROM " & table & " WHERE " & fieldIN & " = '" & valueIN & "'"
        Dim conn As SqlConnection = New SqlConnection(MSSQLConnectString)
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandText = sqlQRY
            conn.Open()
            Dim Value = Await cmd.ExecuteScalarAsync
            Return Value.ToString
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
End Class
