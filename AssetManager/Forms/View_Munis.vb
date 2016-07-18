Public Class View_Munis
    Public Sub LoadMunisInventoryGrid(results As DataTable)
        MainForm.CopyDefaultCellStyles()

        DataGridMunis_Inventory.DataSource = results

        LoadMunisRequisitionGrid()

    End Sub
    Public Sub LoadMunisRequisitionGrid()


        Debug.Print(GetReqNumberFromPO(CurrentDevice.strPO))


    End Sub


    Private Sub View_Munis_Load(sender As Object, e As EventArgs) Handles Me.Load
        ExtendedMethods.DoubleBuffered(DataGridMunis_Inventory, True)
    End Sub

    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        Dim MunisTable As DataTable

        MunisTable = ReturnMSSQLTable("SELECT TOP 10 * FROM famaster WHERE fama_serial='" & Trim(txtSerial.Text) & "'")

        Dim r As DataRow
        For Each r In MunisTable.Rows
            Debug.Print(r.Item("fama_asset"))
        Next
        LoadMunisInventoryGrid(MunisTable)

    End Sub
End Class