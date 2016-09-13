Public Class MunisToolsMenu
    Private Sub MunisToolsMenu_Click(sender As Object, e As EventArgs) Handles Me.Click

    End Sub

    Private Sub ToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles tsmUserOrgObLookup.Click
        Munis.NameSearch()
    End Sub
    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles tsmPOLookUp.Click
        Munis.POSearch()
    End Sub
    Private Sub ToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles tsmReqNumLookUp.Click
        Munis.ReqSearch()
    End Sub

End Class
