Public Class MunisToolsMenu
    Sub New(ParentForm As Form, ByRef TargetToolStrip As MyToolStrip, LocationIndex As Integer)
        Try
            InitializeComponent()
            Me.Tag = ParentForm
            TargetToolStrip.Items.Insert(LocationIndex, Me.MunisTools)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles tsmUserOrgObLookup.Click
        Munis.NameSearch(Me.Tag)
    End Sub
    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles tsmPOLookUp.Click
        Munis.POSearch(Me.Tag)
    End Sub
    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles tsmReqNumLookUp.Click
        Munis.ReqSearch(Me.Tag)
    End Sub
    Private Sub tsmDeviceLookUp_Click(sender As Object, e As EventArgs) Handles tsmDeviceLookUp.Click
        Munis.AssetSearch(Me.Tag)
    End Sub
End Class
