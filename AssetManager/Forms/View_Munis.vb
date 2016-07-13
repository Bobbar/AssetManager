Public Class View_Munis
    Public Sub LoadMunisGrid(results As DataTable)
        MainForm.CopyDefaultCellStyles()

        DataGridMunis.DataSource = results



    End Sub

    Private Sub View_Munis_Load(sender As Object, e As EventArgs) Handles Me.Load
        ExtendedMethods.DoubleBuffered(DataGridMunis, True)
    End Sub

    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        Dim MunisTable As DataTable

        MunisTable = ReturnMSSQLTable("SELECT TOP 10 * FROM famaster WHERE fama_serial='" & Trim(txtSerial.Text) & "'")

        Dim r As DataRow
        For Each r In MunisTable.Rows
            Debug.Print(r.Item("fama_asset"))
        Next
        LoadMunisGrid(MunisTable)

    End Sub
End Class