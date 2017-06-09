Imports System.Data.SqlClient
Public Class Munis_Comms
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
    Public Function Return_MSSQLTableFromCmd(cmd As SqlCommand) As DataTable
        Try
            Using NewTable As New DataTable,
                    da As New SqlDataAdapter(cmd)
                da.Fill(NewTable)
                cmd.Dispose()
                Return NewTable
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
    Public Async Function Return_MSSQLTableFromCmdAsync(cmd As SqlCommand) As Task(Of DataTable)
        Try
            Using conn = cmd.Connection,
                NewTable As New DataTable ',
                Await conn.OpenAsync
                Dim dr As SqlDataReader = Await cmd.ExecuteReaderAsync
                NewTable.Load(dr)
                cmd.Dispose()
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
    Public Function Return_MSSQLCommand(strSQLQry As String) As SqlCommand
        Try
            Dim conn As SqlConnection = New SqlConnection(MSSQLConnectString)
            Dim cmd As New SqlCommand
            cmd.Connection = conn
            cmd.CommandText = strSQLQry
            Return cmd
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function
    Public Function SQLParamCommand(PartialQuery As String, SearchVals As List(Of SearchVal)) As SqlCommand
        Dim cmd = Return_MSSQLCommand(PartialQuery)
        cmd.CommandText += " WHERE"
        Dim ParamString As String = ""
        Dim ValSeq As Integer = 1
        For Each fld In SearchVals
            If fld.IsExact Then
                ParamString += " " + fld.FieldName + "=@Value" & ValSeq & " " & fld.OperatorString
                cmd.Parameters.AddWithValue("@Value" & ValSeq, fld.Value)
            Else
                ParamString += " " + fld.FieldName + " LIKE CONCAT('%', @Value" & ValSeq & ", '%') " & fld.OperatorString
                cmd.Parameters.AddWithValue("@Value" & ValSeq, fld.Value)
            End If
            ValSeq += 1
        Next
        If Strings.Right(ParamString, 3) = "AND" Then 'remove trailing AND from query string
            ParamString = Strings.Left(ParamString, Strings.Len(ParamString) - 3)
        End If

        If Strings.Right(ParamString, 2) = "OR" Then 'remove trailing AND from query string
            ParamString = Strings.Left(ParamString, Strings.Len(ParamString) - 2)
        End If
        cmd.CommandText += ParamString
        Return cmd
    End Function
    Public Function Return_MSSQLValue(table As String, fieldIN As Object, valueIN As Object, fieldOUT As String, Optional fieldIN2 As Object = Nothing, Optional ValueIN2 As Object = Nothing) As Object
        Try
            Dim sqlQRY As String
            Dim Params As New List(Of SearchVal)
            If fieldIN2 IsNot Nothing And ValueIN2 IsNot Nothing Then
                sqlQRY = "SELECT TOP 1 " & fieldOUT & " FROM " & table  ' & fieldIN.ToString & " = '" & valueIN.ToString & "' AND " & fieldIN2.ToString & " = '" & ValueIN2.ToString & "'"
                Params.Add(New SearchVal(fieldIN.ToString, valueIN.ToString,, True))
                Params.Add(New SearchVal(fieldIN2.ToString, ValueIN2.ToString,, True))
            Else
                sqlQRY = "SELECT TOP 1 " & fieldOUT & " FROM " & table ' & fieldIN.ToString & " = '" & valueIN.ToString & "'"
                Params.Add(New SearchVal(fieldIN.ToString, valueIN.ToString,, True))
            End If
            Using cmd = SQLParamCommand(sqlQRY, Params)
                cmd.Connection.Open()
                Return cmd.ExecuteScalar
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
        Return Nothing
    End Function
    Public Async Function Return_MSSQLValueAsync(table As String, fieldIN As Object, valueIN As Object, fieldOUT As String, Optional fieldIN2 As Object = Nothing, Optional ValueIN2 As Object = Nothing) As Task(Of String)
        Try
            Dim sqlQRY As String
            Dim Params As New List(Of SearchVal)
            If fieldIN2 IsNot Nothing And ValueIN2 IsNot Nothing Then
                sqlQRY = "SELECT TOP 1 " & fieldOUT & " FROM " & table  ' & fieldIN.ToString & " = '" & valueIN.ToString & "' AND " & fieldIN2.ToString & " = '" & ValueIN2.ToString & "'"
                Params.Add(New SearchVal(fieldIN.ToString, valueIN.ToString,, True))
                Params.Add(New SearchVal(fieldIN2.ToString, ValueIN2.ToString,, True))
            Else
                sqlQRY = "SELECT TOP 1 " & fieldOUT & " FROM " & table ' & fieldIN.ToString & " = '" & valueIN.ToString & "'"
                Params.Add(New SearchVal(fieldIN.ToString, valueIN.ToString,, True))
            End If
            Using cmd = SQLParamCommand(sqlQRY, Params)
                Await cmd.Connection.OpenAsync()
                Dim Value = Await cmd.ExecuteScalarAsync
                If Value IsNot Nothing Then Return Value.ToString
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
        Return Nothing
    End Function
End Class
