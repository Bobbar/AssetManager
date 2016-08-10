Public Class View_Munis
    Private intMaxResults As Integer = 50
    Private bolGridFilling As Boolean
    Private Sub LoadMunisInventoryGrid(Device As Device_Info)
        Try
            If NeededInfo(Device) Then
                Dim MunisTable As DataTable
                Dim strFields As String = "fama_asset,fama_status,fama_class,fama_subcl,fama_tag,fama_serial,fama_desc,fama_loc,fama_acq_dt,fama_fisc_yr,fama_pur_cost,fama_manuf,fama_model,fama_est_life,fama_repl_dt,fama_purch_memo"
                MunisTable = Return_MSSQLTable("SELECT TOP 1 " & strFields & " FROM famaster WHERE fama_serial='" & Device.strSerial & "'")
                bolGridFilling = True
                DataGridMunis_Inventory.DataSource = MunisTable
                bolGridFilling = False
            Else
                DataGridMunis_Inventory.DataSource = Nothing
            End If
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Sub LoadMunisRequisitionGridByPO(PO As String, FiscalYr As String)
        Try
            'If NeededInfo(Device) Then
            Dim strColumns As String = "rg_fiscal_year,a_requisition_no,LineNumber,rg_org,rg_object,rg_dollar_am,a_object_desc,a_org_description,RequisitionId,Quantity,UnitPrice,NetAmount,ItemDescription,SuggestedVendorId,PurchaseOrderNumber,PurchaseOrderDate"
            Dim strQRY As String = "SELECT TOP " & intMaxResults & " " & strColumns & " FROM rq_gl_info, RequisitionItems WHERE a_requisition_no='" & Munis_GetReqNumberFromPO(PO) & "' AND rg_fiscal_year='" & FiscalYr & "' AND  PurchaseOrderNumber='" & PO & "' AND rg_line_number = LineNumber"
            Debug.Print(strQRY)
            Dim results As DataTable
            results = Return_MSSQLTable(strQRY)
            bolGridFilling = True
            DataGridMunis_Requisition.DataSource = results
            DataGridMunis_Requisition.ClearSelection()
            bolGridFilling = False
            'Else
            '    DataGridMunis_Requisition.DataSource = Nothing
            'End If
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Sub LoadMunisRequisitionGridByReqNo(ReqNumber As String, FiscalYr As String)
        Try
            Dim strColumns As String = "rg_fiscal_year,a_requisition_no,rg_line_number,rg_org,rg_object,rg_dollar_am,a_object_desc,a_org_description,rqdt_des_ln,rqdt_sug_vn,rqdt_pur_no,rqdt_pur_dt"
            Dim strQRY As String = "SELECT TOP " & intMaxResults & " " & strColumns & " FROM rq_gl_info, rqdetail WHERE a_requisition_no='" & ReqNumber & "' AND rg_fiscal_year='" & FiscalYr & "' AND rg_line_number = rqdt_lin_no AND a_requisition_no = rqdt_req_no AND rg_fiscal_year = rqdt_fsc_yr"
            Debug.Print(strQRY)
            Dim results As DataTable
            results = Return_MSSQLTable(strQRY)
            bolGridFilling = True
            DataGridMunis_Requisition.DataSource = results
            DataGridMunis_Requisition.ClearSelection()
            DataGridMunis_Requisition.Refresh()
            bolGridFilling = False
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Sub LoadMunisEmployeeByLastName(Name As String)
        Try
            Dim strColumns As String = "a_employee_number,a_name_last,a_name_first,a_org_primary,a_object_primary,a_location_primary,a_location_p_desc,a_location_p_short"
            Dim strQRY As String = "SELECT TOP " & intMaxResults & " " & strColumns & " FROM pr_employee_master WHERE a_name_last LIKE '%" & UCase(Name) & "%' OR a_name_first LIKE '" & UCase(Name) & "'"
            Debug.Print(strQRY)
            Dim results As DataTable
            results = Return_MSSQLTable(strQRY)
            bolGridFilling = True
            DataGridMunis_Requisition.DataSource = results
            DataGridMunis_Requisition.ClearSelection()
            bolGridFilling = False
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Sub LoadMunisInfoByDevice(Device As Device_Info)
        If Device.strPO <> "" And YearFromDate(Device.dtPurchaseDate) <> "" Then 'if PO and Fiscal yr on record > load data using our records
            Device.strFiscalYear = YearFromDate(Device.dtPurchaseDate)
            LoadMunisInventoryGrid(Device)
            LoadMunisRequisitionGridByPO(Device.strPO, Device.strFiscalYear)
            Me.Show()
        Else
            If Device.strPO = "" Then
                Dim PO As String = Munis_GetPOFromAsset(Device.strAssetTag)
                If PO <> "" Then
                    Device.strPO = PO 'if some's missing > try to find it by other means
                Else
                    PO = Munis_GetPOFromSerial(Device.strSerial)
                    If PO <> "" Then Device.strPO = PO
                End If
            End If
            If YearFromDate(Device.dtPurchaseDate) = "" Then
                Dim FY As String = Munis_GetFYFromPO(Device.strPO) 'Munis_GetFYFromAsset(Device.strAssetTag)
                If FY <> "" Then
                    Device.strFiscalYear = FY
                Else
                    Device.strFiscalYear = Munis_GetFYFromAsset(Device.strAssetTag)
                End If
            Else
                Device.strFiscalYear = YearFromDate(Device.dtPurchaseDate)
            End If
            LoadMunisInventoryGrid(Device)
            LoadMunisRequisitionGridByPO(Device.strPO, Device.strFiscalYear)
            Me.Show()
        End If
    End Sub
    Private Function NeededInfo(Device As Device_Info) As Boolean
        If Trim(Device.strFiscalYear) <> "" Then
            If Trim(Device.strPO) <> "" Then Return True
        End If
        Return False
    End Function
    Private Sub View_Munis_Load(sender As Object, e As EventArgs) Handles Me.Load
        ExtendedMethods.DoubleBuffered(DataGridMunis_Inventory, True)
        ExtendedMethods.DoubleBuffered(DataGridMunis_Requisition, True)
        DataGridMunis_Inventory.DefaultCellStyle = GridStylez
        DataGridMunis_Requisition.DefaultCellStyle = GridStylez
    End Sub
    Private Sub cmdSearch_Click(sender As Object, e As EventArgs) Handles cmdSearch.Click
        Dim MunisTable As DataTable
        MunisTable = Return_MSSQLTable("SELECT TOP 10 * FROM famaster WHERE fama_serial='" & Trim(txtSerial.Text) & "'")
        Dim r As DataRow
        For Each r In MunisTable.Rows
            Debug.Print(r.Item("fama_asset"))
        Next
        DataGridMunis_Inventory.DataSource = MunisTable
    End Sub
    Public Sub HideFixedAssetGrid()
        pnlFixedAsset.Visible = False
        pnlRequisition.Top = pnlMaster.Top
        pnlRequisition.Height = pnlMaster.Height
    End Sub
    Private Sub HighlightCurrentRow(Row As Integer)
        On Error Resume Next
        If Not bolGridFilling Then
            Dim BackColor As Color = DefGridBC
            Dim SelectColor As Color = DefGridSelCol
            Dim c1 As Color = colHighlightColor 'highlight color
            If Row > -1 Then
                For Each cell As DataGridViewCell In DataGridMunis_Requisition.Rows(Row).Cells
                    Dim c2 As Color = Color.FromArgb(SelectColor.R, SelectColor.G, SelectColor.B)
                    Dim BlendColor As Color
                    BlendColor = Color.FromArgb((CInt(c1.A) + CInt(c2.A)) / 2,
                                                (CInt(c1.R) + CInt(c2.R)) / 2,
                                                (CInt(c1.G) + CInt(c2.G)) / 2,
                                                (CInt(c1.B) + CInt(c2.B)) / 2)
                    cell.Style.SelectionBackColor = BlendColor
                    c2 = Color.FromArgb(BackColor.R, BackColor.G, BackColor.B)
                    BlendColor = Color.FromArgb((CInt(c1.A) + CInt(c2.A)) / 2,
                                                (CInt(c1.R) + CInt(c2.R)) / 2,
                                                (CInt(c1.G) + CInt(c2.G)) / 2,
                                                (CInt(c1.B) + CInt(c2.B)) / 2)
                    cell.Style.BackColor = BlendColor
                Next
            End If
        End If
    End Sub
    Private Sub DataGridMunis_Requisition_CellLeave(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridMunis_Requisition.CellLeave
        Dim BackColor As Color = DefGridBC
        Dim SelectColor As Color = DefGridSelCol
        If e.RowIndex > -1 Then
            For Each cell As DataGridViewCell In DataGridMunis_Requisition.Rows(e.RowIndex).Cells
                cell.Style.SelectionBackColor = SelectColor
                cell.Style.BackColor = BackColor
            Next
        End If
    End Sub
    Private Sub DataGridMunis_Requisition_CellEnter(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridMunis_Requisition.CellEnter
        HighlightCurrentRow(e.RowIndex)
    End Sub
End Class