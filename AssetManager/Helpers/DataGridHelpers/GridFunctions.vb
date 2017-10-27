Namespace GridFunctions
    Module GridFunctions

        Public Sub PopulateGrid(grid As DataGridView, data As DataTable, columns As List(Of DataGridColumn), Optional forceRawData As Boolean = False)
            SetupGrid(grid, columns)
            Using data
                grid.DataSource = Nothing
                grid.DataSource = BuildDataSource(data, columns, forceRawData)
            End Using
        End Sub

        Private Function BuildDataSource(data As DataTable, columns As List(Of DataGridColumn), forceRawData As Boolean) As DataTable
            Dim NeedsRebuilt = ColumnsRequireRebuild(columns)
            If NeedsRebuilt And Not forceRawData Then
                Dim NewTable As New DataTable
                For Each col In columns
                    NewTable.Columns.Add(col.ColumnName, col.ColumnType)
                Next
                For Each row As DataRow In data.Rows
                    Dim NewRow As DataRow
                    NewRow = NewTable.NewRow
                    For Each col In columns

                        Select Case col.ColumnFormatType
                            Case ColumnFormatTypes.DefaultFormat, ColumnFormatTypes.AttributeCombo
                                NewRow.Item(col.ColumnName) = row.Item(col.ColumnName)

                            Case ColumnFormatTypes.AttributeDisplayMemberOnly
                                NewRow.Item(col.ColumnName) = GetDisplayValueFromCode(col.AttributeIndex, row.Item(col.ColumnName).ToString)

                            Case ColumnFormatTypes.NotePreview
                                Dim NoteText = RTFToPlainText(row.Item(col.ColumnName).ToString)
                                NewRow.Item(col.ColumnName) = NotePreview(NoteText)

                            Case ColumnFormatTypes.FileSize
                                Dim HumanFileSize As String = Math.Round((CInt(row.Item(col.ColumnName)) / 1024), 1) & " KB"
                                NewRow.Item(col.ColumnName) = HumanFileSize

                            Case ColumnFormatTypes.Image
                                NewRow.Item(col.ColumnName) = FileIcon.GetFileIcon(row.Item(col.ColumnName).ToString)

                        End Select

                    Next
                    NewTable.Rows.Add(NewRow)
                Next
                Return NewTable
            Else
                Return data
            End If
        End Function

        Private Function ColumnsRequireRebuild(columns As List(Of DataGridColumn)) As Boolean
            Dim RebuildRequired As Boolean = False
            For Each col In columns
                Select Case col.ColumnFormatType
                    Case ColumnFormatTypes.AttributeDisplayMemberOnly, ColumnFormatTypes.NotePreview, ColumnFormatTypes.FileSize, ColumnFormatTypes.Image
                        RebuildRequired = True
                End Select
            Next
            Return RebuildRequired
        End Function

        Private Sub SetupGrid(grid As DataGridView, columns As List(Of DataGridColumn))
            grid.DataSource = Nothing
            grid.Rows.Clear()
            grid.Columns.Clear()
            grid.AutoGenerateColumns = False
            For Each col As DataGridColumn In columns
                grid.Columns.Add(GetColumn(col))
            Next
        End Sub

        Private Function GetColumn(column As DataGridColumn) As DataGridViewColumn
            Select Case column.ColumnFormatType
                Case ColumnFormatTypes.DefaultFormat, ColumnFormatTypes.AttributeDisplayMemberOnly, ColumnFormatTypes.NotePreview, ColumnFormatTypes.FileSize
                    Return GenericColumn(column)
                Case ColumnFormatTypes.AttributeCombo
                    Return DataGridComboColumn(column.AttributeIndex, column.ColumnCaption, column.ColumnName)
                Case ColumnFormatTypes.Image
                    Return DataGridImageColumn(column)
            End Select
            Return Nothing
        End Function

        Private Function DataGridImageColumn(column As DataGridColumn) As DataGridViewColumn
            Dim NewCol As New DataGridViewImageColumn
            NewCol.Name = column.ColumnName
            NewCol.DataPropertyName = column.ColumnName
            NewCol.HeaderText = column.ColumnCaption
            NewCol.ValueType = column.ColumnType
            NewCol.CellTemplate = New DataGridViewImageCell
            NewCol.SortMode = DataGridViewColumnSortMode.Automatic
            NewCol.ReadOnly = column.ColumnReadOnly
            NewCol.Visible = column.ColumnVisible
            NewCol.Width = 40
            Return NewCol
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

        Private Function DataGridComboColumn(indexType() As AttributeDataStruct, headerText As String, name As String) As DataGridViewComboBoxColumn
            Dim NewCombo As New DataGridViewComboBoxColumn
            NewCombo.Items.Clear()
            NewCombo.HeaderText = headerText
            NewCombo.DataPropertyName = name
            NewCombo.Name = name
            NewCombo.Width = 200
            NewCombo.SortMode = DataGridViewColumnSortMode.Automatic
            NewCombo.DisplayMember = NameOf(AttributeDataStruct.DisplayValue)
            NewCombo.ValueMember = NameOf(AttributeDataStruct.Code)
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
End Namespace