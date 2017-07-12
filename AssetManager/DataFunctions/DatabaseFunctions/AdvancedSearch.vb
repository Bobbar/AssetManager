Imports MySql.Data.MySqlClient
Namespace AdvancedSearch


    Public Class Search

#Region "Fields"

        Private _searchString As String
        Private _searchTables As List(Of TableInfo)

#End Region

#Region "Constructors"

        Sub New(SearchString As String, SearchTables As List(Of TableInfo))
            _searchString = SearchString
            _searchTables = SearchTables
        End Sub

#End Region

#Region "Methods"

        Public Function GetColumns(table As String) As List(Of String)
            Dim colList As New List(Of String)
            Using comms As New MySQL_Comms
                Dim SQLQry = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" & CurrentDB & "' AND TABLE_NAME = '" & table & "'"
                Dim results = comms.Return_SQLTable(SQLQry)
                For Each row As DataRow In results.Rows
                    colList.Add(row.Item("COLUMN_NAME").ToString)
                Next
            End Using
            Return colList
        End Function

        Public Function GetResults() As List(Of DataTable)
            Dim resultsList As New List(Of DataTable)
            For Each table In _searchTables
                'Dim qry As String = "SELECT * FROM " & table.TableName & " WHERE "
                Dim qry As String = "SELECT " & BuildSelectString(table) & " FROM " & table.TableName & " WHERE "
                qry += BuildFieldString(table)
                Dim cmd As New MySqlCommand
                cmd.CommandText = qry
                cmd.Parameters.AddWithValue("@" & "SEARCHVAL", _searchString)
                Using LocalSQLComm As New MySQL_Comms,
                    ds As New DataSet,
                    da As New MySqlDataAdapter,
                    QryComm As MySqlCommand = cmd, results As New DataTable(table.TableName)
                    QryComm.Connection = LocalSQLComm.Connection
                    da.SelectCommand = QryComm
                    da.Fill(results)
                    resultsList.Add(results)
                End Using
            Next
            Return resultsList
        End Function

        Private Function BuildFieldString(table As TableInfo) As String
            Dim Fields As String = ""
            For Each col In table.Columns
                Fields += table.TableName & "." & col & " LIKE CONCAT('%', @SEARCHVAL, '%')"
                If table.Columns.IndexOf(col) <> table.Columns.Count - 1 Then Fields += " OR "
            Next
            Return Fields
        End Function

        Private Function BuildSelectString(table As TableInfo) As String
            Dim SelectString As String = ""
            For Each column In table.Columns
                SelectString += column
                If table.Columns.IndexOf(column) <> table.Columns.Count - 1 Then SelectString += ","
            Next
            Return SelectString
        End Function
        Private Function GetTableInfo(table As String) As TableInfo
            Dim col = GetColumns(table)
            Dim NewTable As New TableInfo(table, col)
            Return NewTable
        End Function

#End Region

    End Class

#Region "Structs"

    Public Structure TableInfo

#Region "Fields"

        Public Columns As List(Of String)
        Public TableKey As String
        Public TableName As String

#End Region

#Region "Constructors"

        Sub New(Name As String, Cols As List(Of String))
            TableName = Name
            Columns = Cols
        End Sub

#End Region

    End Structure

#End Region
End Namespace