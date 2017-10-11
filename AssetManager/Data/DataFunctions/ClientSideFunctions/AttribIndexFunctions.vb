Module AttribIndexFunctions

    Public Sub FillComboBox(IndexType() As ComboboxDataStruct, ByRef cmb As ComboBox)
        cmb.Items.Clear()
        cmb.Text = ""
        For Each ComboItem As ComboboxDataStruct In IndexType
            cmb.Items.Add(ComboItem.DisplayValue)
        Next
    End Sub

    Public Sub FillToolComboBox(IndexType() As ComboboxDataStruct, ByRef cmb As ToolStripComboBox)
        cmb.Items.Clear()
        cmb.Text = ""
        Dim i As Integer = 0
        For Each ComboItem As ComboboxDataStruct In IndexType
            cmb.Items.Insert(i, ComboItem.DisplayValue)
            i += 1
        Next
    End Sub

    Public Function GetDBValue(codeIndex() As ComboboxDataStruct, index As Integer) As String
        Try
            If index > -1 Then
                Return codeIndex(index).Code
            End If
            Return String.Empty
        Catch
            Return String.Empty
        End Try
    End Function

    Public Function GetDisplayValueFromCode(codeIndex() As ComboboxDataStruct, code As String) As String
        For Each item In codeIndex
            If item.Code = code Then Return item.DisplayValue
        Next
        Return String.Empty
    End Function

    Public Function GetDisplayValueFromIndex(codeIndex() As ComboboxDataStruct, index As Integer) As String
        Return codeIndex(index).DisplayValue
    End Function

    Public Function GetComboIndexFromCode(codeIndex() As ComboboxDataStruct, code As String) As Integer
        For i = 0 To codeIndex.Length - 1
            If codeIndex(i).Code = code Then Return i
        Next
        Return -1
    End Function

    Public Sub BuildIndexes()
        Dim BuildIdxs = Task.Run(Sub()
                                     DeviceIndex.Locations = BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.Location)
                                     DeviceIndex.ChangeType = BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.ChangeType)
                                     DeviceIndex.EquipType = BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.EquipType)
                                     DeviceIndex.OSType = BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.OSType)
                                     DeviceIndex.StatusType = BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.StatusType)
                                     SibiIndex.StatusType = BuildIndex(SibiRequestCols.AttribTable, SibiAttribType.SibiStatusType)
                                     SibiIndex.ItemStatusType = BuildIndex(SibiRequestCols.AttribTable, SibiAttribType.SibiItemStatusType)
                                     SibiIndex.RequestType = BuildIndex(SibiRequestCols.AttribTable, SibiAttribType.SibiRequestType)
                                     SibiIndex.AttachFolder = BuildIndex(SibiRequestCols.AttribTable, SibiAttribType.SibiAttachFolder)
                                 End Sub)
        BuildIdxs.Wait()
    End Sub

    Public Function BuildIndex(codeType As String, typeName As String) As ComboboxDataStruct()
        Try
            Dim strQRY = "SELECT * FROM " & codeType & " LEFT OUTER JOIN munis_codes on " & codeType & ".db_value = munis_codes.asset_man_code WHERE type_name ='" & typeName & "' ORDER BY " & ComboCodesBaseCols.DisplayValue & ""
            Using results As DataTable = DBFactory.GetDatabase.DataTableFromQueryString(strQRY)
                Dim tmpArray As New List(Of ComboboxDataStruct)
                For Each r As DataRow In results.Rows
                    Dim DisplayValue As String = ""
                    If r.Table.Columns.Contains("munis_code") Then
                        If Not IsDBNull(r.Item("munis_code")) Then
                            DisplayValue = r.Item(ComboCodesBaseCols.DisplayValue).ToString & " - " & r.Item("munis_code").ToString
                        Else
                            DisplayValue = r.Item(ComboCodesBaseCols.DisplayValue).ToString
                        End If
                    Else
                        DisplayValue = r.Item(ComboCodesBaseCols.DisplayValue).ToString
                    End If
                    tmpArray.Add(New ComboboxDataStruct(DisplayValue, r.Item(ComboCodesBaseCols.CodeValue).ToString, CInt(r.Item(ComboCodesBaseCols.ID))))
                Next
                Return tmpArray.ToArray
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return Nothing
        End Try
    End Function

End Module