Public Class View_Munis
    Public Sub LoadMunisInventoryGrid(results As DataTable)
        MainForm.CopyDefaultCellStyles()
        DataGridMunis_Inventory.DataSource = results
        LoadMunisRequisitionGrid(CurrentDevice)
    End Sub
    Public Sub LoadMunisRequisitionGrid(Device As Device_Info)
        Try
            Dim strColumns As String = "rg_fiscal_year,a_requisition_no,LineNumber,rg_org,rg_object,rg_dollar_am,a_object_desc,a_org_description,RequisitionId,Quantity,UnitPrice,NetAmount,ItemDescription,SuggestedVendorId,PurchaseOrderNumber,PurchaseOrderDate"
            Dim strQRY As String = "SELECT " & strColumns & " FROM rq_gl_info, RequisitionItems WHERE a_requisition_no='" & GetReqNumberFromPO(Device.strPO) & "' AND rg_fiscal_year='" & YearFromDate(Device.dtPurchaseDate) & "' AND  PurchaseOrderNumber='" & Device.strPO & "' AND rg_line_number = LineNumber"
            Debug.Print(strQRY)
            Dim results As DataTable
            results = ReturnMSSQLTable(strQRY)
            DataGridMunis_Requisition.DataSource = results
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub View_Munis_Load(sender As Object, e As EventArgs) Handles Me.Load
        ExtendedMethods.DoubleBuffered(DataGridMunis_Inventory, True)
        ExtendedMethods.DoubleBuffered(DataGridMunis_Requisition, True)
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