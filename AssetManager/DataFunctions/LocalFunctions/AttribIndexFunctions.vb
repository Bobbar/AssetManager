Module AttribIndexFunctions

    Public Sub FillComboBox(IndexType() As Combo_Data, ByRef cmb As ComboBox)
        cmb.Items.Clear()
        cmb.Text = ""
        Dim i As Integer = 0
        For Each ComboItem As Combo_Data In IndexType
            cmb.Items.Insert(i, ComboItem.strLong)
            i += 1
        Next
    End Sub

    Public Sub FillToolComboBox(IndexType() As Combo_Data, ByRef cmb As ToolStripComboBox)
        cmb.Items.Clear()
        cmb.Text = ""
        Dim i As Integer = 0
        For Each ComboItem As Combo_Data In IndexType
            cmb.Items.Insert(i, ComboItem.strLong)
            i += 1
        Next
    End Sub

    Public Function GetDBValue(ByVal CodeIndex() As Combo_Data, ByVal index As Integer) As String
        Try
            If index > -1 Then
                Return CodeIndex(index).strShort
            End If
            Return Nothing
        Catch
            Return Nothing
        End Try
    End Function

    Public Function GetHumanValue(ByVal CodeIndex() As Combo_Data, ByVal ShortVal As String) As String
        For Each Code As Combo_Data In CodeIndex
            If Code.strShort = ShortVal Then Return Code.strLong
        Next
        Return Nothing
    End Function

    Public Function GetHumanValueFromIndex(ByVal CodeIndex() As Combo_Data, index As Integer) As String
        Return CodeIndex(index).strLong
    End Function

    Public Function GetComboIndexFromShort(ByVal CodeIndex() As Combo_Data, ByVal ShortVal As String) As Integer
        For i As Integer = 0 To UBound(CodeIndex)
            If CodeIndex(i).strShort = ShortVal Then Return i
        Next
        Return Nothing
    End Function

    Public Sub BuildIndexes()
        Dim BuildIdxs = Task.Run(Sub()
                                     With AssetFunc
                                         DeviceIndex.Locations = .BuildIndex(Attrib_Table.Device, Attrib_Type.Location)
                                         DeviceIndex.ChangeType = .BuildIndex(Attrib_Table.Device, Attrib_Type.ChangeType)
                                         DeviceIndex.EquipType = .BuildIndex(Attrib_Table.Device, Attrib_Type.EquipType)
                                         DeviceIndex.OSType = .BuildIndex(Attrib_Table.Device, Attrib_Type.OSType)
                                         DeviceIndex.StatusType = .BuildIndex(Attrib_Table.Device, Attrib_Type.StatusType)
                                         SibiIndex.StatusType = .BuildIndex(Attrib_Table.Sibi, Attrib_Type.SibiStatusType)
                                         SibiIndex.ItemStatusType = .BuildIndex(Attrib_Table.Sibi, Attrib_Type.SibiItemStatusType)
                                         SibiIndex.RequestType = .BuildIndex(Attrib_Table.Sibi, Attrib_Type.SibiRequestType)
                                         SibiIndex.AttachFolder = .BuildIndex(Attrib_Table.Sibi, Attrib_Type.SibiAttachFolder)
                                     End With
                                 End Sub)
        BuildIdxs.Wait()
    End Sub

End Module