Module GridFunctions

    Public Sub PopulateGrid(grid As DataGridView, data As DataTable, columns As List(Of DataGridColumn))
        SetupGrid(grid, columns)
        Using data
            grid.DataSource = Nothing
            grid.DataSource = RebuildDataSource(data, columns)
        End Using
    End Sub
    Public Function RebuildDataSource(data As DataTable, columns As List(Of DataGridColumn)) As DataTable
        Dim NeedsRebuilt As Boolean = columns.Exists(Function(c) c.ComboDisplayMode <> ComboColumnDisplayMode.DefaultMode)
        If NeedsRebuilt Then
            Dim NewTable As New DataTable
            For Each col In columns
                NewTable.Columns.Add(col.ColumnName, data.Columns(col.ColumnName).DataType)
            Next
            For Each row As DataRow In data.Rows
                Dim NewRow As DataRow
                NewRow = NewTable.NewRow
                For Each col In columns

                    Select Case col.ComboDisplayMode
                        Case ComboColumnDisplayMode.DefaultMode
                            NewRow.Item(col.ColumnName) = row.Item(col.ColumnName)
                        Case ComboColumnDisplayMode.DisplayMemberOnly
                            NewRow.Item(col.ColumnName) = GetDisplayValueFromCode(col.ComboIndex, row.Item(col.ColumnName).ToString)
                    End Select
                Next
                NewTable.Rows.Add(NewRow)
            Next
            Return NewTable
        Else
            Return data
        End If
    End Function

    Public Sub SetupGrid(grid As DataGridView, columns As List(Of DataGridColumn))
        Try
            grid.DataSource = Nothing
            grid.Rows.Clear()
            grid.Columns.Clear()
            grid.AutoGenerateColumns = False

            For Each col As DataGridColumn In columns
                grid.Columns.Add(GetColumn(col))
            Next
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Function GetColumn(column As DataGridColumn) As DataGridViewColumn
        Select Case column.ColumnType
            Case GetType(String), GetType(Integer), GetType(Date)
                Return GenericColumn(column)
            Case GetType(ComboboxDataStruct)
                Select Case column.ComboDisplayMode
                    Case ComboColumnDisplayMode.DefaultMode
                        Return DataGridComboColumn(column.ComboIndex, column.ColumnCaption, column.ColumnName)
                    Case ComboColumnDisplayMode.DisplayMemberOnly
                        Return GenericColumn(column)
                End Select
            Case Else
                Throw New Exception("Unexpected column data type.")
        End Select
        Return Nothing
    End Function

    Private Function GenericColumn(column As DataGridColumn) As DataGridViewColumn
        Dim NewCol As New DataGridViewColumn
        NewCol.Name = column.ColumnName
        NewCol.DataPropertyName = column.ColumnName
        NewCol.HeaderText = column.ColumnCaption
        NewCol.ValueType = column.ColumnType
        NewCol.CellTemplate = New DataGridViewTextBoxCell
        NewCol.SortMode = DataGridViewColumnSortMode.Automatic
        NewCol.ReadOnly = column.ColumnReadOnly
        NewCol.Visible = column.ColumnVisible
        Return NewCol
    End Function

    Private Function DataGridComboColumn(indexType() As ComboboxDataStruct, headerText As String, name As String) As DataGridViewComboBoxColumn
        Dim NewCombo As New DataGridViewComboBoxColumn
        NewCombo.Items.Clear()
        NewCombo.HeaderText = headerText
        NewCombo.DataPropertyName = name
        NewCombo.Name = name
        NewCombo.Width = 200
        NewCombo.SortMode = DataGridViewColumnSortMode.Automatic
        NewCombo.DisplayMember = NameOf(ComboboxDataStruct.DisplayValue)
        NewCombo.ValueMember = NameOf(ComboboxDataStruct.Code)
        NewCombo.DataSource = indexType
        Return NewCombo
    End Function

    ''' <summary>
    ''' Returns a comma separated string containing the DB columns within a List(Of ColumnStruct). For use in queries.
    ''' </summary>
    ''' <param name="columns"></param>
    ''' <returns></returns>
    Public Function ColumnsString(columns As List(Of DataGridColumn)) As String
        Dim ColString As String = ""
        For Each col In columns
            ColString += col.ColumnName
            If columns.IndexOf(col) <> columns.Count - 1 Then ColString += ","
        Next
        Return ColString
    End Function

    Public Function GetColIndex(grid As DataGridView, ByVal columnName As String) As Integer
        Try
            Return grid.Columns.Item(columnName).Index
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Function GetCurrentCellValue(grid As DataGridView, columnName As String) As String
        Return NoNull(grid.Item(GetColIndex(grid, columnName), grid.CurrentRow.Index).Value.ToString)
    End Function

    Public Sub CopyToGridForm(grid As DataGridView, parentForm As ExtendedForm)
        If grid IsNot Nothing Then
            Dim NewGridForm As New GridForm(parentForm, grid.Name & " Copy")
            NewGridForm.AddGrid(grid.Name, grid.Name, DirectCast(grid.DataSource, DataTable).Copy())
            NewGridForm.Show()
        End If
    End Sub

    Public Sub CopySelectedGridData(grid As DataGridView)
        grid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
        Clipboard.SetDataObject(grid.GetClipboardContent())
        grid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText
    End Sub
End Module
