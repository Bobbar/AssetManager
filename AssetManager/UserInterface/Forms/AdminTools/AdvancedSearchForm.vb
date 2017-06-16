Public Class AdvancedSearchForm
    Private _parentForm As Form
    Sub New(ParentForm As Form)
        InitializeComponent()
        Tag = ParentForm
        Icon = ParentForm.Icon
        _parentForm = ParentForm
        PopulateTableList()
        Me.Show()
    End Sub
    Private Sub PopulateTableList()
        Dim Qry = "SHOW TABLES IN " & CurrentDB
        Dim Results As New DataTable
        Using comms As New MySQL_Comms
            Results = comms.Return_SQLTable(Qry)
        End Using
        TableListBox.Items.Clear()
        For Each row As DataRow In Results.Rows
            TableListBox.Items.Add(row.Item(Results.Columns(0).ColumnName).ToString)
        Next
    End Sub
    Private Sub StartSearch()
        Dim AdvSearch As New AdvancedSearch(SearchStringTextBox.Text, GetTables.ToArray)
        Dim DisplayGrid As New GridForm(_parentForm, "Advanced Search Results")
        Dim Tables As List(Of DataTable) = AdvSearch.GetResults
        For Each table In Tables
            DisplayGrid.AddGrid(table.TableName, table.TableName, table)
        Next
        DisplayGrid.Show()
    End Sub
    Private Function GetTables() As List(Of String)
        Dim tables As New List(Of String)

        For Each item As String In TableListBox.CheckedItems
            tables.Add(item)
        Next
        Return tables
    End Function
    Private Sub SearchButton_Click(sender As Object, e As EventArgs) Handles SearchButton.Click
        StartSearch()
    End Sub

    Private Sub SearchStringTextBox_KeyDown(sender As Object, e As KeyEventArgs) Handles SearchStringTextBox.KeyDown
        If e.KeyCode = Keys.Return Then
            StartSearch()
        End If
    End Sub
End Class