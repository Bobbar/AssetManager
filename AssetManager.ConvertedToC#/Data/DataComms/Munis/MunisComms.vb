Imports System.Data.SqlClient

Public Class MunisComms

#Region "Fields"

    Private Const MSSQLConnectString As String = "server=svr-munis5.core.co.fairfield.oh.us; database=mu_live; trusted_connection=True;"

#End Region

#Region "Methods"

    Public Function ReturnSqlCommand(sqlQry As String) As SqlCommand
        Dim conn As SqlConnection = New SqlConnection(MSSQLConnectString)
        Dim cmd As New SqlCommand
        cmd.Connection = conn
        cmd.CommandText = sqlQry
        Return cmd
    End Function

    Public Function ReturnSqlTable(sqlQry As String) As DataTable
        Using conn As SqlConnection = New SqlConnection(MSSQLConnectString),
                    NewTable As New DataTable,
                    da As New SqlDataAdapter
            da.SelectCommand = New SqlCommand(sqlQry)
            da.SelectCommand.Connection = conn
            da.Fill(NewTable)
            Return NewTable
        End Using
    End Function

    Public Async Function ReturnSqlTableAsync(sqlQry As String) As Task(Of DataTable)
        Using conn As SqlConnection = New SqlConnection(MSSQLConnectString),
                    NewTable As New DataTable,
                cmd As New SqlCommand(sqlQry, conn)
            Await conn.OpenAsync
            Dim dr As SqlDataReader = Await cmd.ExecuteReaderAsync
            NewTable.Load(dr)
            Return NewTable
        End Using
    End Function

    Public Function ReturnSqlTableFromCmd(cmd As SqlCommand) As DataTable
        Using NewTable As New DataTable,
                    da As New SqlDataAdapter(cmd)
            da.Fill(NewTable)
            cmd.Dispose()
            Return NewTable
        End Using
    End Function

    Public Async Function ReturnSqlTableFromCmdAsync(cmd As SqlCommand) As Task(Of DataTable)
        Using conn = cmd.Connection,
                NewTable As New DataTable
            Await conn.OpenAsync
            Dim dr As SqlDataReader = Await cmd.ExecuteReaderAsync
            NewTable.Load(dr)
            cmd.Dispose()
            Return NewTable
        End Using
    End Function

    Public Function ReturnSqlValue(table As String, fieldIn As Object, valueIn As Object, fieldOut As String, Optional fieldIn2 As Object = Nothing, Optional valueIn2 As Object = Nothing) As Object
        Dim sqlQRY As String
        Dim Params As New List(Of DBQueryParameter)
        If fieldIn2 IsNot Nothing And valueIn2 IsNot Nothing Then
            sqlQRY = "SELECT TOP 1 " & fieldOut & " FROM " & table  ' & fieldIN.ToString & " = '" & valueIN.ToString & "' AND " & fieldIN2.ToString & " = '" & ValueIN2.ToString & "'"
            Params.Add(New DBQueryParameter(fieldIn.ToString, valueIn.ToString, True))
            Params.Add(New DBQueryParameter(fieldIn2.ToString, valueIn2.ToString, True))
        Else
            sqlQRY = "SELECT TOP 1 " & fieldOut & " FROM " & table ' & fieldIN.ToString & " = '" & valueIN.ToString & "'"
            Params.Add(New DBQueryParameter(fieldIn.ToString, valueIn.ToString, True))
        End If
        Using cmd = GetSqlCommandFromParams(sqlQRY, Params), conn = cmd.Connection
            cmd.Connection.Open()
            Return cmd.ExecuteScalar
        End Using
    End Function

    Public Async Function ReturnSqlValueAsync(table As String, fieldIn As Object, valueIn As Object, fieldOut As String, Optional fieldIn2 As Object = Nothing, Optional valueIn2 As Object = Nothing) As Task(Of String)
        Try
            Dim sqlQRY As String
            Dim Params As New List(Of DBQueryParameter)
            If fieldIn2 IsNot Nothing And valueIn2 IsNot Nothing Then
                sqlQRY = "SELECT TOP 1 " & fieldOut & " FROM " & table  ' & fieldIN.ToString & " = '" & valueIN.ToString & "' AND " & fieldIN2.ToString & " = '" & ValueIN2.ToString & "'"
                Params.Add(New DBQueryParameter(fieldIn.ToString, valueIn.ToString, True))
                Params.Add(New DBQueryParameter(fieldIn2.ToString, valueIn2.ToString, True))
            Else
                sqlQRY = "SELECT TOP 1 " & fieldOut & " FROM " & table ' & fieldIN.ToString & " = '" & valueIN.ToString & "'"
                Params.Add(New DBQueryParameter(fieldIn.ToString, valueIn.ToString, True))
            End If
            Using cmd = GetSqlCommandFromParams(sqlQRY, Params), conn = cmd.Connection
                Await cmd.Connection.OpenAsync()
                Dim Value = Await cmd.ExecuteScalarAsync
                'StopTimer()
                If Value IsNot Nothing Then Return Value.ToString
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
        Return String.Empty
    End Function

    ''' <summary>
    ''' Takes a partial query string without the WHERE operator, and a list of <see cref="DBQueryParameter"/> and returns a parameterized <see cref="SqlCommand"/>.
    ''' </summary>
    ''' <param name="partialQuery"></param>
    ''' <param name="parameters"></param>
    ''' <returns></returns>
    Public Function GetSqlCommandFromParams(partialQuery As String, parameters As List(Of DBQueryParameter)) As SqlCommand
        Dim cmd = ReturnSqlCommand(partialQuery)
        cmd.CommandText += " WHERE"
        Dim ParamString As String = ""
        Dim ValSeq As Integer = 1
        For Each fld In parameters
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

#End Region

End Class