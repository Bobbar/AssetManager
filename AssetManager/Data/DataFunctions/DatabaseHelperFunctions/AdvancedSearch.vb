Namespace AdvancedSearch

    Public Class Search

#Region "Fields"

        Private _searchString As String
        Private _searchTables As List(Of TableInfo)

#End Region

#Region "Constructors"

        Sub New(searchString As String, searchTables As List(Of TableInfo))
            _searchString = searchString
            _searchTables = searchTables
        End Sub

#End Region

#Region "Methods"

        Public Function GetColumns(table As String) As List(Of String)
            Dim colList As New List(Of String)
            Dim SQLQry = "SELECT COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_SCHEMA = '" & ServerInfo.CurrentDataBase.ToString & "' AND TABLE_NAME = '" & table & "'"
            Dim results = DBFactory.GetDatabase.DataTableFromQueryString(SQLQry) 'comms.ReturnMySqlTable(SQLQry)
            For Each row As DataRow In results.Rows
                colList.Add(row.Item("COLUMN_NAME").ToString)
            Next
            Return colList
        End Function

        Public Function GetResults() As List(Of DataTable)
            Dim resultsList As New List(Of DataTable)
            For Each table In _searchTables
                Dim qry As String = "SELECT " & BuildSelectString(table) & " FROM " & table.TableName & " WHERE "
                qry += BuildFieldString(table)
                Using MySQLDB As New MySQLDatabase,
                    cmd = MySQLDB.GetCommand(qry)
                    cmd.AddParameterWithValue("@" & "SEARCHVAL", _searchString)
                    Dim results = MySQLDB.DataTableFromCommand(cmd)
                    results.TableName = table.TableName
                    resultsList.Add(results)
                    results.Dispose()
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

#End Region

    End Class

#Region "Structs"

    Public Structure TableInfo

#Region "Fields"

        Public ReadOnly Property Columns As List(Of String)
        Public Property TableKey As String
        Public Property TableName As String

#End Region

#Region "Constructors"

        Sub New(name As String, cols As List(Of String))
            TableName = name
            Columns = cols
        End Sub

#End Region

    End Structure

#End Region

End Namespace