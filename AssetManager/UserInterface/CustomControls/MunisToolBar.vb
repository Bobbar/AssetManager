Public Class MunisToolBar : Implements IDisposable

#Region "Fields"

    Private MunisDropDown As New ToolStripDropDownButton

#End Region

#Region "Constructors"

    Sub New(parentForm As Form)
        InitDropDown()
        InitToolItems(parentForm)
    End Sub

#End Region

#Region "Methods"

    ''' <summary>
    ''' Inserts the MunisToolBar into the specified toolstrip.
    ''' </summary>
    ''' <param name="TargetStrip"></param>
    ''' <param name="LocationIndex"></param>
    Public Sub InsertMunisDropDown(targetStrip As OneClickToolStrip, Optional locationIndex As Integer = -1)
        If locationIndex >= 0 Then
            targetStrip.Items.Insert(locationIndex, MunisDropDown)
            AddSeperators(targetStrip, locationIndex)
        Else
            targetStrip.Items.Add(MunisDropDown)
            AddSeperators(targetStrip, targetStrip.Items.Count - 1)
        End If
    End Sub

    Private Sub AddSeperators(ByRef targetToolStrip As OneClickToolStrip, locationIndex As Integer)
        If targetToolStrip.Items.Count - 1 >= locationIndex + 1 Then
            If targetToolStrip.Items(locationIndex + 1).GetType IsNot GetType(ToolStripSeparator) Then
                targetToolStrip.Items.Insert(locationIndex + 1, New ToolStripSeparator)
            End If
            If targetToolStrip.Items(locationIndex - 1).GetType IsNot GetType(ToolStripSeparator) Then
                targetToolStrip.Items.Insert(locationIndex, New ToolStripSeparator)
            End If
        Else
            If targetToolStrip.Items(locationIndex).GetType IsNot GetType(ToolStripSeparator) Then
                targetToolStrip.Items.Add(New ToolStripSeparator)
            End If
            If targetToolStrip.Items(locationIndex - 1).GetType IsNot GetType(ToolStripSeparator) Then
                targetToolStrip.Items.Insert(locationIndex, New ToolStripSeparator)
            End If
        End If
    End Sub

    Private Sub AddToolItem(toolItem As ToolStripMenuItem)
        MunisDropDown.DropDownItems.Add(toolItem)
        AddHandler toolItem.Click, AddressOf ToolItemClicked
    End Sub

    Private Sub InitDropDown()
        MunisDropDown.Image = Global.AssetManager.My.Resources.Resources.SearchIcon
        MunisDropDown.Name = "MunisTools"
        MunisDropDown.Size = New System.Drawing.Size(87, 29)
        MunisDropDown.Text = "MUNIS Tools"
        MunisDropDown.AutoSize = True
    End Sub

    Private Sub InitToolItems(parentForm As Form)
        Dim ToolItemList As New List(Of ToolStripMenuItem)
        ToolItemList.Add(NewToolItem("tsmUserOrgObLookup", "User Lookup", Sub() MunisFunc.NameSearch(parentForm)))
        ToolItemList.Add(NewToolItem("tsmOrgObLookup", "Org/Obj Lookup", Sub() MunisFunc.OrgObSearch(parentForm)))
        ToolItemList.Add(NewToolItem("tsmPOLookUp", "PO Lookup", Sub() MunisFunc.POSearch(parentForm)))
        ToolItemList.Add(NewToolItem("tsmReqNumLookUp", "Requisition # Lookup", Sub() MunisFunc.ReqSearch(parentForm)))
        ToolItemList.Add(NewToolItem("tsmDeviceLookUp", "Device Lookup", Sub() MunisFunc.AssetSearch(parentForm)))
        For Each item In ToolItemList
            AddToolItem(item)
        Next
    End Sub

    Private Function NewToolItem(name As String, text As String, clickMethod As Action) As ToolStripMenuItem
        Dim TSM As New ToolStripMenuItem
        TSM.Name = name
        TSM.Text = text
        TSM.Tag = clickMethod
        Return TSM
    End Function
    Private Sub ToolItemClicked(sender As Object, e As EventArgs)
        Dim ClickedItem As ToolStripMenuItem = DirectCast(sender, ToolStripMenuItem)
        Dim ClickAction As Action = DirectCast(ClickedItem.Tag, Action)
        ClickAction()
    End Sub

#End Region

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                MunisDropDown.Dispose()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub
#End Region

End Class