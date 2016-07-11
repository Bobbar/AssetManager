Public Class View_Munis
    Public Sub LoadMunisGrid(results As DataTable)
        MainFrom.CopyDefaultCellStyles()

        DataGridMunis.DataSource = results



    End Sub

    Private Sub View_Munis_Load(sender As Object, e As EventArgs) Handles Me.Load
        ExtendedMethods.DoubleBuffered(DataGridMunis, True)
    End Sub
End Class