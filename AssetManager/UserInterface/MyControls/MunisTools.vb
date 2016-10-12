Public Class MunisToolsMenu
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
