Module AttribIndexFunctions

    Public Sub FillComboBox(IndexType() As ComboboxDataStruct, ByRef cmb As ComboBox)
        cmb.Items.Clear()
        cmb.Text = ""
        Dim i As Integer = 0
        For Each ComboItem As ComboboxDataStruct In IndexType
            cmb.Items.Insert(i, ComboItem.HumanReadable)
            i += 1
        Next
    End Sub

    Public Sub FillToolComboBox(IndexType() As ComboboxDataStruct, ByRef cmb As ToolStripComboBox)
        cmb.Items.Clear()
        cmb.Text = ""
        Dim i As Integer = 0
        For Each ComboItem As ComboboxDataStruct In IndexType
            cmb.Items.Insert(i, ComboItem.HumanReadable)
            i += 1
        Next
    End Sub

    Public Function GetDBValue(ByVal CodeIndex() As ComboboxDataStruct, ByVal index As Integer) As String
        Try
            If index > -1 Then
                Return CodeIndex(index).Code
            End If
            Return String.Empty
        Catch
            Return String.Empty
        End Try
    End Function

    Public Function GetHumanValue(ByVal CodeIndex() As ComboboxDataStruct, ByVal ShortVal As String) As String
        For Each Code As ComboboxDataStruct In CodeIndex
            If Code.Code = ShortVal Then Return Code.HumanReadable
        Next
        Return Nothing
    End Function

    Public Function GetHumanValueFromIndex(ByVal CodeIndex() As ComboboxDataStruct, index As Integer) As String
        Return CodeIndex(index).HumanReadable
    End Function

    Public Function GetComboIndexFromShort(ByVal CodeIndex() As ComboboxDataStruct, ByVal ShortVal As String) As Integer
        For i As Integer = 0 To UBound(CodeIndex)
            If CodeIndex(i).Code = ShortVal Then Return i
        Next
        Return Nothing
    End Function

    Public Sub BuildIndexes()
        Dim BuildIdxs = Task.Run(Sub()
                                     With AssetFunc
                                         DeviceIndex.Locations = .BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.Location)
                                         DeviceIndex.ChangeType = .BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.ChangeType)
                                         DeviceIndex.EquipType = .BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.EquipType)
                                         DeviceIndex.OSType = .BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.OSType)
                                         DeviceIndex.StatusType = .BuildIndex(DevicesBaseCols.AttribTable, DeviceAttribType.StatusType)
                                         SibiIndex.StatusType = .BuildIndex(SibiRequestCols.AttribTable, SibiAttribType.SibiStatusType)
                                         SibiIndex.ItemStatusType = .BuildIndex(SibiRequestCols.AttribTable, SibiAttribType.SibiItemStatusType)
                                         SibiIndex.RequestType = .BuildIndex(SibiRequestCols.AttribTable, SibiAttribType.SibiRequestType)
                                         SibiIndex.AttachFolder = .BuildIndex(SibiRequestCols.AttribTable, SibiAttribType.SibiAttachFolder)
                                     End With
                                 End Sub)
        BuildIdxs.Wait()
    End Sub

End Module