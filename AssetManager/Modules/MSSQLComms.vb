﻿Imports System.Data.SqlClient
Module MSSQLComms
    Private Const MSSQLConnectString As String = "server=svr-munis5.core.co.fairfield.oh.us; database=mu_live; trusted_connection=True;"
    Public Function ReturnMSSQLTable(strSQLQry As String) As DataTable
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
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
    Public Function ReturnMSSQLValue(table As String, fieldIN As String, valueIN As String, fieldOUT As String) As String
        Dim sqlQRY As String = "SELECT TOP 1 " & fieldOUT & " FROM " & table & " WHERE " & fieldIN & " = '" & valueIN & "'"
        Debug.Print(sqlQRY)
        Dim conn As SqlConnection = New SqlConnection(MSSQLConnectString)
        Try
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandText = sqlQRY
            conn.Open()
            Return Convert.ToString(cmd.ExecuteScalar)
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return Nothing
        End Try
    End Function
End Module