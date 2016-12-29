Public Class View_Munis
    Private intMaxResults As Integer = 50
    Private bolGridFilling As Boolean
    Public bolSelectMod As Boolean = False
    Private CurrentMunisDevice As Device_Info
    Private MunisComms As New clsMunis_Comms
    Sub New(ParentForm As Form, Optional Hide_FA As Boolean = False)
        InitializeComponent()
        Tag = ParentForm
        Icon = ParentForm.Icon
        If Hide_FA Then HideFixedAssetGrid()
    End Sub
    Public Sub LoadDevice(Device As Device_Info)
        CurrentMunisDevice = Device
    End Sub
    Private Sub LoadMunisInventoryGrid(Device As Device_Info)
        Dim intRows As Integer = 0
        Try
            Dim strFields As String = "fama_asset,fama_status,fama_class,fama_subcl,fama_tag,fama_serial,fama_desc,fama_dept,fama_loc,FixedAssetLocations.LongDescription,fama_acq_dt,fama_fisc_yr,fama_pur_cost,fama_manuf,fama_model,fama_est_life,fama_repl_dt,fama_purch_memo"
            If Device.strSerial <> "" Then
                intRows = ProcessMunisQuery(DataGridMunis_Inventory, "SELECT TOP 1 " & strFields & " FROM famaster INNER JOIN FixedAssetLocations ON FixedAssetLocations.Code = famaster.fama_loc WHERE fama_serial='" & Device.strSerial & "'")
            End If
            If intRows < 1 And Device.strAssetTag <> "" Then
                intRows = ProcessMunisQuery(DataGridMunis_Inventory, "SELECT TOP 1 " & strFields & " FROM famaster INNER JOIN FixedAssetLocations ON FixedAssetLocations.Code = famaster.fama_loc WHERE fama_tag='" & Device.strAssetTag & "'")
            End If
            If intRows < 1 Then Exit Sub
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Sub LoadMunisRequisitionGridByReqNo(ReqNumber As String, FiscalYr As String)
        Try
            If ReqNumber = "" Or FiscalYr = "" Then Exit Sub
            Dim strQRY As String = "SELECT TOP " & intMaxResults & " dbo.rq_gl_info.rg_fiscal_year, dbo.rq_gl_info.a_requisition_no, dbo.rq_gl_info.rg_org, dbo.rq_gl_info.rg_object, dbo.rq_gl_info.a_org_description, dbo.rq_gl_info.a_object_desc, 
                         VEN.a_vendor_name, VEN.a_vendor_number, dbo.rqdetail.rqdt_pur_no, dbo.rqdetail.rqdt_pur_dt, dbo.rqdetail.rqdt_lin_no, dbo.rqdetail.rqdt_uni_pr, dbo.rqdetail.rqdt_net_pr,
                         dbo.rqdetail.rqdt_qty_no, dbo.rqdetail.rqdt_des_ln
            From            dbo.rq_gl_info INNER JOIN
                         dbo.rqdetail ON dbo.rq_gl_info.rg_line_number = dbo.rqdetail.rqdt_lin_no AND dbo.rq_gl_info.a_requisition_no = dbo.rqdetail.rqdt_req_no AND dbo.rq_gl_info.rg_fiscal_year = dbo.rqdetail.rqdt_fsc_yr LEFT JOIN
                          (
						 SELECT TOP 1 a_vendor_number,a_vendor_name
						 FROM ap_vendor
						 WHERE a_vendor_number = " & Munis.Get_VendorNumber_From_ReqNumber(ReqNumber, FiscalYr) & "
						 ) VEN 
ON dbo.rqdetail.rqdt_sug_vn = VEN.a_vendor_number
WHERE        (dbo.rq_gl_info.a_requisition_no = " & ReqNumber & ") AND (dbo.rq_gl_info.rg_fiscal_year = " & FiscalYr & ")" ' AND (dbo.ap_vendor.a_vendor_remit_seq = 0)"
            ProcessMunisQuery(DataGridMunis_Requisition, strQRY)
            Me.Show()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Public Sub LoadMunisEmployeeByLastName(Name As String)
        Try
            lblReqInfo.Text = "MUNIS Info:"
            HideFixedAssetGrid()
            Dim strColumns As String = "a_employee_number,a_name_last,a_name_first,a_org_primary,a_object_primary,a_location_primary,a_location_p_desc,a_location_p_short"
            Dim strQRY As String = "SELECT TOP " & intMaxResults & " " & strColumns & " FROM pr_employee_master WHERE a_name_last LIKE '%" & UCase(Name) & "%' OR a_name_first LIKE '" & UCase(Name) & "'"
            ProcessMunisQuery(DataGridMunis_Requisition, strQRY)
            Me.Show()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Function ProcessMunisQuery(Grid As DataGridView, Query As String) As Integer
        Dim results As New DataTable
        Dim intRows As Integer
        Try
            Debug.Print(Query)
            results = MunisComms.Return_MSSQLTable(Query)
            If IsNothing(results) Then Return 0
            intRows = results.Rows.Count
            bolGridFilling = True
            Grid.DataSource = results
            Grid.ClearSelection()
            Grid.Refresh()
            results.Dispose()
            Return intRows
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Function
    Public Sub LoadMunisInfoByDevice(Device As Device_Info)
        CurrentMunisDevice = Device
        If Device.strPO <> "" Then
            Device.strFiscalYear = YearFromDate(Device.dtPurchaseDate)
            LoadMunisInventoryGrid(Device)
            LoadMunisRequisitionGridByReqNo(Munis.Get_ReqNumber_From_PO(Device.strPO), Munis.Get_FY_From_PO(Device.strPO))
            Me.Show()
        Else
            If Device.strPO = "" Then
                Dim PO As String = Munis.Get_PO_From_Asset(Device.strAssetTag)
                If PO <> "" Then
                    Device.strPO = PO 'if some's missing -> try to find it by other means
                Else
                    PO = Munis.Get_PO_From_Serial(Device.strSerial)
                    If PO <> "" Then Device.strPO = PO
                End If
            End If
            If Device.strPO <> "" Then
                LoadMunisInventoryGrid(Device)
                LoadMunisRequisitionGridByReqNo(Munis.Get_ReqNumber_From_PO(Device.strPO), Munis.Get_FY_From_PO(Device.strPO))
                Me.Show()
            Else
                Message("Could not pull all Munis info. No FA info and/or no PO", vbOKOnly + vbInformation, "Nothing Found")
                LoadMunisInventoryGrid(Device)
                LoadMunisRequisitionGridByReqNo(Munis.Get_ReqNumber_From_PO(Device.strPO), Munis.Get_FY_From_PO(Device.strPO))
                Me.Show()
            End If
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
        DataGridMunis_Inventory.DefaultCellStyle = GridStyles
        DataGridMunis_Requisition.DefaultCellStyle = GridStyles
        If IsNothing(CurrentMunisDevice.strGUID) Then
        Else
            Me.Text = Me.Text + FormTitle(CurrentMunisDevice)
        End If
        bolGridFilling = False
    End Sub
    Private Sub HideFixedAssetGrid()
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
    Public ReadOnly Property UnitPrice As String
        Get
            Return SelectedUnitPrice
        End Get
    End Property
    Private SelectedUnitPrice As String
    Private Sub DataGridMunis_Requisition_CellMouseDoubleClick(sender As Object, e As DataGridViewCellMouseEventArgs) Handles DataGridMunis_Requisition.CellMouseDoubleClick
        If Me.Modal Then
            SelectedUnitPrice = DataGridMunis_Requisition.Item(GetColIndex(DataGridMunis_Requisition, "rqdt_uni_pr"), DataGridMunis_Requisition.CurrentRow.Index).Value.ToString
            Dim decPrice As Decimal = Convert.ToDecimal(SelectedUnitPrice)
            SelectedUnitPrice = decPrice.ToString("C")
            Me.DialogResult = DialogResult.OK
        End If
    End Sub
End Class