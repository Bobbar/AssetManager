Public Class MunisToolsMenu
    Sub New(ParentForm As Form, ByRef TargetToolStrip As MyToolStrip, Optional LocationIndex As Integer = -1)
        Try
            InitializeComponent()
            Me.Tag = ParentForm
            If LocationIndex >= 0 Then
                TargetToolStrip.Items.Insert(LocationIndex, Me.MunisTools)
                AddSeperators(TargetToolStrip, LocationIndex)
            Else
                TargetToolStrip.Items.Add(Me.MunisTools) 'Insert(TargetToolStrip.Items.Count - 1, Me.MunisTools)
                AddSeperators(TargetToolStrip, TargetToolStrip.Items.Count - 1)
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
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
    Private Sub tsmUserOrgObLookup_Click(sender As Object, e As EventArgs) Handles tsmUserOrgObLookup.Click
        Munis.NameSearch(Me.Tag)
    End Sub
    Private Sub tsmOrgObLookup_Click(sender As Object, e As EventArgs) Handles tsmOrgObLookup.Click
        Munis.OrgObSearch(Me.Tag)
    End Sub
    Private Sub tsmPOLookUp_Click(sender As Object, e As EventArgs) Handles tsmPOLookUp.Click
        Munis.POSearch(Me.Tag)
    End Sub
    Private Sub tsmReqNumLookUp_Click(sender As Object, e As EventArgs) Handles tsmReqNumLookUp.Click
        Munis.ReqSearch(Me.Tag)
    End Sub
    Private Sub tsmDeviceLookUp_Click(sender As Object, e As EventArgs) Handles tsmDeviceLookUp.Click
        Munis.AssetSearch(Me.Tag)
    End Sub
End Class
