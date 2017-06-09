Public Class MunisToolBar
    Private MunisDropDown As New ToolStripDropDownButton
    Sub New(ParentForm As Form)
        InitDropDown()
        InitToolItems(ParentForm)
    End Sub
    ''' <summary>
    ''' Inserts the MunisToolBar into the specified toolstrip.
    ''' </summary>
    ''' <param name="TargetStrip"></param>
    ''' <param name="LocationIndex"></param>
    Public Sub InsertMunisDropDown(ByRef TargetStrip As MyToolStrip, Optional LocationIndex As Integer = -1)
        If LocationIndex >= 0 Then
            TargetStrip.Items.Insert(LocationIndex, MunisDropDown)
            AddSeperators(TargetStrip, LocationIndex)
        Else
            TargetStrip.Items.Add(MunisDropDown)
            AddSeperators(TargetStrip, TargetStrip.Items.Count - 1)
        End If
    End Sub
    Private Function NewToolItem(Name As String, Text As String, ClickMethod As Action) As ToolStripMenuItem
        Dim TSM As New ToolStripMenuItem
        TSM.Name = Name
        TSM.Text = Text
        TSM.Tag = ClickMethod
        Return TSM
    End Function
    Private Sub InitDropDown()
        MunisDropDown.Image = Global.AssetManager.My.Resources.Resources.Find
        MunisDropDown.Name = "MunisTools"
        MunisDropDown.Size = New System.Drawing.Size(87, 29)
        MunisDropDown.Text = "MUNIS Tools"
        MunisDropDown.AutoSize = True
    End Sub
    Private Sub InitToolItems(ParentForm As Form)
        Dim ToolItemList As New List(Of ToolStripMenuItem)
        ToolItemList.Add(NewToolItem("tsmUserOrgObLookup", "User Lookup", Sub() MunisFunc.NameSearch(ParentForm)))
        ToolItemList.Add(NewToolItem("tsmOrgObLookup", "Org/Obj Lookup", Sub() MunisFunc.OrgObSearch(ParentForm)))
        ToolItemList.Add(NewToolItem("tsmPOLookUp", "PO Lookup", Sub() MunisFunc.POSearch(ParentForm)))
        ToolItemList.Add(NewToolItem("tsmReqNumLookUp", "Requisition # Lookup", Sub() MunisFunc.ReqSearch(ParentForm)))
        ToolItemList.Add(NewToolItem("tsmDeviceLookUp", "Device Lookup", Sub() MunisFunc.AssetSearch(ParentForm)))
        For Each item In ToolItemList
            AddToolItem(item)
        Next
    End Sub
    Private Sub AddToolItem(ToolItem As ToolStripMenuItem)
        MunisDropDown.DropDownItems.Add(ToolItem)
        AddHandler ToolItem.Click, AddressOf ToolItemClicked
    End Sub
    Private Sub ToolItemClicked(sender As Object, e As EventArgs)
        Dim ClickedItem As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
        Dim ClickAction As Action = DirectCast(ClickedItem.Tag, Action)
        ClickAction()
    End Sub
    Private Sub AddSeperators(ByRef TargetToolStrip As MyToolStrip, LocationIndex As Integer)
        If TargetToolStrip.Items.Count - 1 >= LocationIndex + 1 Then
            If TargetToolStrip.Items(LocationIndex + 1).GetType IsNot GetType(ToolStripSeparator) Then
                TargetToolStrip.Items.Insert(LocationIndex + 1, New ToolStripSeparator)
            End If
            If TargetToolStrip.Items(LocationIndex - 1).GetType IsNot GetType(ToolStripSeparator) Then
                TargetToolStrip.Items.Insert(LocationIndex, New ToolStripSeparator)
            End If
        Else
            If TargetToolStrip.Items(LocationIndex).GetType IsNot GetType(ToolStripSeparator) Then
                TargetToolStrip.Items.Add(New ToolStripSeparator)
            End If
            If TargetToolStrip.Items(LocationIndex - 1).GetType IsNot GetType(ToolStripSeparator) Then
                TargetToolStrip.Items.Insert(LocationIndex, New ToolStripSeparator)
            End If
        End If
    End Sub
End Class
