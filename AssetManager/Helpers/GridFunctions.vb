Module GridFunctions

    Public Sub SetupGrid(Grid As DataGridView, Columns As List(Of DataGridColumnStruct))
        Try
            Grid.DataSource = Nothing
            Grid.Rows.Clear()
            Grid.Columns.Clear()
            Grid.AutoGenerateColumns = False

            For Each col As DataGridColumnStruct In Columns
                Grid.Columns.Add(GetColumn(col))
            Next
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Function GetColumn(Column As DataGridColumnStruct) As DataGridViewColumn
        Select Case Column.ColumnType
            Case GetType(String), GetType(Integer)
                Return GenericColumn(Column)
            Case GetType(ComboboxDataStruct)
                Return DataGridComboColumn(Column.ComboIndex, Column.ColumnCaption, Column.ColumnName)
        End Select
        Return Nothing
    End Function

    Private Function GenericColumn(Column As DataGridColumnStruct) As DataGridViewColumn
        Dim NewCol As New DataGridViewColumn
        NewCol.Name = Column.ColumnName
        NewCol.DataPropertyName = Column.ColumnName
        NewCol.HeaderText = Column.ColumnCaption
        NewCol.ValueType = Column.ColumnType
        NewCol.CellTemplate = New DataGridViewTextBoxCell
        NewCol.SortMode = DataGridViewColumnSortMode.Automatic
        NewCol.ReadOnly = Column.ColumnReadOnly
        NewCol.Visible = Column.ColumnVisible
        Return NewCol
    End Function

    Private Function DataGridComboColumn(IndexType() As ComboboxDataStruct, HeaderText As String, Name As String) As DataGridViewComboBoxColumn
        Dim NewCombo As New DataGridViewComboBoxColumn
        NewCombo.Items.Clear()
        NewCombo.HeaderText = HeaderText
        NewCombo.DataPropertyName = Name
        NewCombo.Name = Name
        NewCombo.Width = 200
        NewCombo.SortMode = DataGridViewColumnSortMode.Automatic
        NewCombo.DisplayMember = NameOf(ComboboxDataStruct.HumanReadable)
        NewCombo.ValueMember = NameOf(ComboboxDataStruct.Code)
        NewCombo.DataSource = IndexType
        Return NewCombo
    End Function

    ''' <summary>
    ''' Returns a comma separated string containing the DB columns within a List(Of ColumnStruct). For use in queries.
    ''' </summary>
    ''' <param name="Columns"></param>
    ''' <returns></returns>
    Public Function ColumnsString(Columns As List(Of DataGridColumnStruct)) As String
        Dim ColString As String = ""
        For Each col In Columns
            ColString += col.ColumnName
            If Columns.IndexOf(col) <> Columns.Count - 1 Then ColString += ","
        Next
        Return ColString
    End Function

    Public Function GetColIndex(ByVal Grid As DataGridView, ByVal strColName As String) As Integer
        Try
            Return Grid.Columns.Item(strColName).Index
        Catch ex As Exception
            Return -1
        End Try
    End Function

    Public Function GetCurrentCellValue(ByVal Grid As DataGridView, ColumnName As String) As String
        Return NoNull(Grid.Item(GetColIndex(Grid, ColumnName), Grid.CurrentRow.Index).Value.ToString)
    End Function

    Public Sub CopyToGridForm(Grid As DataGridView, Parent As ExtendedForm)
        If Grid IsNot Nothing Then
            Dim NewGridForm As New GridForm(Parent, Grid.Name & " Copy")
            NewGridForm.AddGrid(Grid.Name, Grid.Name, DirectCast(Grid.DataSource, DataTable))
            NewGridForm.Show()
        End If
    End Sub

    Public Sub CopySelectedGridData(Grid As DataGridView)
        Grid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableAlwaysIncludeHeaderText
        Clipboard.SetDataObject(Grid.GetClipboardContent())
        Grid.ClipboardCopyMode = DataGridViewClipboardCopyMode.EnableWithAutoHeaderText
    End Sub
End Module
