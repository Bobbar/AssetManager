﻿Imports MyDialogLib
Public Class Munis_Functions
    Private Const intMaxResults As Integer = 100
    Private MunisComms As New Munis_Comms
    Public Function Get_ReqNumber_From_PO(PO As String) As String
        If Not IsNothing(PO) Then
            If PO <> "" Then
                Return MunisComms.Return_MSSQLValue("Requisitions", "PurchaseOrderNumber", PO, "RequisitionNumber").ToString
            End If
        End If
        Return Nothing
    End Function
    Private Async Function Get_ReqNumber_From_PO_Async(PO As String) As Task(Of String)
        If Not IsNothing(PO) Then
            If PO <> "" Then
                Return Await MunisComms.Return_MSSQLValueAsync("Requisitions", "PurchaseOrderNumber", PO, "RequisitionNumber")
            End If
        End If
        Return Nothing
    End Function
    Public Async Function Get_PO_From_ReqNumber_Async(ReqNum As String, FY As String) As Task(Of String)
        If Not IsNothing(ReqNum) Then
            If ReqNum <> "" Then
                Return Await MunisComms.Return_MSSQLValueAsync("rqdetail", "rqdt_req_no", ReqNum, "rqdt_pur_no", "rqdt_fsc_yr", FY)
            End If
        End If
        Return Nothing
    End Function
    Private Function Get_PO_From_Asset(AssetTag As String) As String
        Try
            If Not IsNothing(AssetTag) Then
                If AssetTag <> "" Then
                    Dim PO = MunisComms.Return_MSSQLValue("famaster", "fama_tag", AssetTag, "fama_purch_memo")
                    If PO IsNot Nothing Then
                        Return Trim(PO.ToString)
                    End If
                End If
            End If
            Return Nothing
        Catch
            Return Nothing
        End Try
    End Function
    Private Function Get_PO_From_Serial(Serial As String) As String
        Try
            If Not IsNothing(Serial) Then
                If Serial <> "" Then
                    Dim PO = MunisComms.Return_MSSQLValue("famaster", "fama_serial", Serial, "fama_purch_memo")
                    If PO IsNot Nothing Then
                        Return Trim(PO.ToString)
                    End If
                End If
            End If
            Return Nothing
        Catch
            Return Nothing
        End Try
    End Function
    Private Function SelectedCellValue(ByRef GridRow As DataGridViewRow, Optional Column As String = Nothing) As String
        For Each cell As DataGridViewCell In GridRow.Cells
            If Column = "" Then
                If cell.Selected Then Return cell.Value.ToString
            Else
                If cell.OwningColumn.Name = Column Then Return cell.Value.ToString
            End If
        Next
        Return Nothing
    End Function
    Public Function Get_SerialFromAsset(AssetTag As String) As String
        Dim value = MunisComms.Return_MSSQLValue("famaster", "fama_tag", AssetTag, "fama_serial").ToString
        If value IsNot Nothing Then
            Return Trim(value)
        End If
        Return ""
    End Function
    Public Function Get_AssetFromSerial(Serial As String) As String
        Dim value = MunisComms.Return_MSSQLValue("famaster", "fama_serial", Serial, "fama_tag") '.ToString
        If value IsNot Nothing Then
            Return Trim(value.ToString)
        End If
        Return ""
    End Function
    Public Function Get_FY_From_Asset(AssetTag As String) As String
        Return Trim(MunisComms.Return_MSSQLValue("famaster", "fama_tag", AssetTag, "fama_fisc_yr").ToString)
    End Function
    Public Function Get_PODT_From_PO(PO As String) As String
        Return YearFromDate(DateTime.Parse(Trim(MunisComms.Return_MSSQLValue("RequisitionItems", "PurchaseOrderNumber", PO, "PurchaseOrderDate").ToString)))
    End Function
    Public Function Get_VendorName_From_PO(PO As String) As String
        Dim VendorNumber = MunisComms.Return_MSSQLValue("rqdetail", "rqdt_req_no", Get_ReqNumber_From_PO(PO), "rqdt_sug_vn", "rqdt_fsc_yr", Get_FY_From_PO(PO))
        Return MunisComms.Return_MSSQLValue("ap_vendor", "a_vendor_number", VendorNumber, "a_vendor_name").ToString
    End Function
    Public Function Get_VendorNumber_From_ReqNumber(ReqNum As String, FY As String) As Integer
        Return CInt(MunisComms.Return_MSSQLValue("rqdetail", "rqdt_req_no", ReqNum, "rqdt_sug_vn", "rqdt_fsc_yr", FY).ToString)
    End Function
    Public Function Get_FY_From_PO(PO As String) As String
        Dim strFYyy As String = Left(PO, 2)
        Return "20" + strFYyy
    End Function
    Public Async Function Get_PO_Status(PO As Integer) As Task(Of String)
        Dim StatusString As String
        Dim StatusCode As String = Await MunisComms.Return_MSSQLValueAsync("poheader", "pohd_pur_no", PO, "pohd_sta_cd")
        If StatusCode <> "" Then
            Dim ParseCode As Integer = -1
            If Not Int32.TryParse(StatusCode, ParseCode) Then Return Nothing
            StatusString = StatusCode.ToString & " - " & POStatusCodeToLong(ParseCode)
            Return StatusString
        End If
        Return Nothing
    End Function
    Public Async Function Get_Req_Status(ReqNum As String, FY As Integer) As Task(Of String)
        Dim StatusString As String
        Dim StatusCode As String = Await MunisComms.Return_MSSQLValueAsync("rqheader", "rqhd_req_no", ReqNum, "rqhd_sta_cd", "rqhd_fsc_yr", FY)
        If StatusCode <> "" Then
            Dim ParseCode As Integer = -1
            If Not Int32.TryParse(StatusCode, ParseCode) Then Return Nothing
            StatusString = StatusCode.ToString & " - " & ReqStatusCodeToLong(ParseCode)
            Return StatusString
        End If
        Return Nothing
    End Function
    Private Function POStatusCodeToLong(Code As Integer) As String
        Select Case Code
            Case 0
                Return "Closed"
            Case 1
                Return "Rejected"
            Case 2
                Return "Created"
            Case 4
                Return "Allocated"
            Case 5
                Return "Released"
            Case 6
                Return "Posted"
            Case 8
                Return "Printed"
            Case 9
                Return "Carry Forward"
            Case 10
                Return "Canceled"
            Case 11
                Return "Closed"
        End Select
        Return Nothing
    End Function
    Private Function ReqStatusCodeToLong(Code As Integer) As String
        Select Case Code
            Case 0
                Return "Converted"
            Case 1
                Return "Rejected"
            Case 2
                Return "Created"
            Case 4
                Return "Allocated"
            Case 6
                Return "Released"
            Case Else
                Return "NA"
        End Select
        Return Nothing
    End Function
    Public Sub AssetSearch(Parent As Form)
        Try
            Dim Device As New Device_Info
            Device.dtPurchaseDate = Nothing
            Using NewDialog As New MyDialog(Parent)
                With NewDialog
                    .Text = "Asset Search"
                    .AddTextBox("txtAsset", "Asset:")
                    .AddTextBox("txtSerial", "Serial:")
                    .ShowDialog()
                    If .DialogResult = DialogResult.OK Then
                        Device.strAssetTag = Trim(NewDialog.GetControlValue("txtAsset").ToString)
                        Device.strSerial = Trim(NewDialog.GetControlValue("txtSerial").ToString)
                        LoadMunisInfoByDevice(Device, Parent)
                    End If
                End With
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Public Sub NameSearch(Parent As Form)
        Using NewDialog As New MyDialog(Parent)
            Dim strName As String
            With NewDialog
                .Text = "Org/Object Code Search"
                .AddTextBox("txtName", "First or Last Name:")
                .ShowDialog()
                If .DialogResult = DialogResult.OK Then strName = NewDialog.GetControlValue("txtName").ToString
            End With
            If Trim(strName) IsNot "" Then
                NewMunisView_NameSearch(strName, Parent)
            End If
        End Using
    End Sub
    Public Sub POSearch(Parent As Form)
        Try
            Dim PO, FY As String
            Using NewDialog As New MyDialog(Parent)
                With NewDialog
                    .Text = "PO Search"
                    .AddTextBox("txtPO", "PO #:")
                    .AddTextBox("txtFY", "FY:")
                    .ShowDialog()
                    If .DialogResult = DialogResult.OK Then
                        PO = NewDialog.GetControlValue("txtPO").ToString
                        FY = NewDialog.GetControlValue("txtFY").ToString
                        NewMunisView_POSearch(PO, Parent)
                    End If
                End With
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Public Sub ReqSearch(Parent As Form)
        Try
            Dim ReqNumber, FY As String
            Using NewDialog As New MyDialog(Parent)
                With NewDialog
                    .Text = "Req Search"
                    .AddTextBox("txtReqNum", "Requisition #:")
                    .AddTextBox("txtFY", "FY:")
                    .ShowDialog()
                    If .DialogResult = DialogResult.OK Then
                        ReqNumber = NewDialog.GetControlValue("txtReqNum").ToString
                        FY = NewDialog.GetControlValue("txtFY").ToString
                        NewMunisView_ReqSearch(ReqNumber, FY, Parent)
                    End If
                End With
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Public Sub OrgObSearch(Parent As Form)
        Using NewDialog As New MyDialog(Parent)
            Dim strOrg, strObj As String
            With NewDialog
                .Text = "Org/Object Code Search"
                .AddTextBox("txtOrg", "Org Code:")
                .AddTextBox("txtObj", "Object Code:")
                .ShowDialog()
                If .DialogResult = DialogResult.OK Then
                    strOrg = NewDialog.GetControlValue("txtOrg").ToString
                    strObj = NewDialog.GetControlValue("txtObj").ToString
                End If
            End With
            If Trim(strOrg) IsNot "" Then
                NewOrgObView(strOrg, strObj, Parent)
            End If
        End Using
    End Sub
    Public Function ListOfEmpBySup(SupEmpNum As String) As DataTable
        Dim strQRY As String = "SELECT TOP 100 a_employee_number FROM pr_employee_master WHERE e_supervisor='" & SupEmpNum & "'"
        Return MunisComms.Return_MSSQLTable(strQRY)
    End Function
    Private Async Sub NewOrgObView(Org As String, Optional Obj As String = Nothing, Optional Parent As Form = Nothing)
        Waiting()
        Dim NewGridForm As New GridForm(Parent, "Org/Obj Info")
        Dim Columns As String = " glma_org, glma_obj, glma_desc, glma_seg5, glma_bud_yr, glma_orig_bud_cy, glma_rev_bud_cy, glma_encumb_cy, glma_memo_bal_cy, glma_rev_bud_cy-glma_encumb_cy-glma_memo_bal_cy AS 'Funds Available' "

        If Obj <> "" Then 'Show Rollup info for Object

            Dim GLMasterQry As String = "Select TOP " & intMaxResults & " " & Columns & "FROM glmaster"
            Dim RollUpCode As String = Await MunisComms.Return_MSSQLValueAsync("gl_budget_rollup", "a_org", Org, "a_rollup_code")
            Dim RollUpQry As String = "SELECT TOP " & intMaxResults & " * FROM gl_budget_rollup WHERE a_rollup_code = '" & RollUpCode & "'"

            Dim Params As New List(Of SearchVal)
            Params.Add(New SearchVal("glma_org", Org,, True))
            Params.Add(New SearchVal("glma_obj", Obj,, True))

            NewGridForm.AddGrid("OrgGrid", "GL Info:", Await MunisComms.Return_MSSQLTableFromCmdAsync(MunisComms.SQLParamCommand(GLMasterQry, Params))) 'MunisComms.Return_MSSQLTableAsync(Qry))
            NewGridForm.AddGrid("RollupGrid", "Rollup Info:", Await MunisComms.Return_MSSQLTableAsync(RollUpQry))

        Else ' Show Rollup info for all Objects in Org

            Dim GLMasterQry As String = "Select TOP " & intMaxResults & " " & Columns & "FROM glmaster"
            Dim RollUpQry As String = "SELECT TOP " & intMaxResults & " * FROM gl_budget_rollup"

            Dim GL_Params As New List(Of SearchVal)
            GL_Params.Add(New SearchVal("glma_org", Org,, True))

            Dim Roll_Params As New List(Of SearchVal)
            Roll_Params.Add(New SearchVal("a_org", Org,, True))

            NewGridForm.AddGrid("OrgGrid", "GL Info:", Await MunisComms.Return_MSSQLTableFromCmdAsync(MunisComms.SQLParamCommand(GLMasterQry, GL_Params))) 'MunisComms.Return_MSSQLTableAsync(Qry))
            NewGridForm.AddGrid("RollupGrid", "Rollup Info:", Await MunisComms.Return_MSSQLTableFromCmdAsync(MunisComms.SQLParamCommand(RollUpQry, Roll_Params))) 'MunisComms.Return_MSSQLTableAsync("SELECT TOP " & intMaxResults & " * FROM gl_budget_rollup WHERE a_org = '" & Org & "'"))
        End If
        NewGridForm.Show()
        DoneWaiting()
    End Sub
    Private Async Sub NewMunisView_NameSearch(Name As String, Optional Parent As Form = Nothing)
        Waiting()
        Dim strColumns As String = "e.a_employee_number,e.a_name_last,e.a_name_first,e.a_org_primary,e.a_object_primary,e.a_location_primary,e.a_location_p_desc,e.a_location_p_short,e.e_work_location,m.a_employee_number as sup_employee_number,m.a_name_first as sup_name_first,m.a_name_last as sup_name_last"
        Dim strQRY As String = "SELECT TOP " & intMaxResults & " " & strColumns & " 
FROM pr_employee_master e
INNER JOIN pr_employee_master m on e.e_supervisor = m.a_employee_number"

        Dim Params As New List(Of SearchVal)
        Params.Add(New SearchVal("e.a_name_last", UCase(Name), "OR"))
        Params.Add(New SearchVal("e.a_name_first", UCase(Name), "OR"))

        Dim NewGridForm As New GridForm(Parent, "MUNIS Employee Info")
        NewGridForm.AddGrid("EmpGrid", "MUNIS Info:", Await MunisComms.Return_MSSQLTableFromCmdAsync(MunisComms.SQLParamCommand(strQRY, Params)))
        NewGridForm.Show()
        DoneWaiting()
    End Sub
    Public Async Sub NewMunisView_POSearch(PO As String, Optional Parent As Form = Nothing)
        Waiting()
        If PO = "" Then Exit Sub
        Dim strQRY As String = "SELECT TOP " & intMaxResults & " pohd_pur_no, pohd_fsc_yr, pohd_req_no, pohd_gen_cm, pohd_buy_id, pohd_pre_dt, pohd_exp_dt, pohd_sta_cd, pohd_vnd_cd, pohd_dep_cd, pohd_shp_cd, pohd_tot_amt, pohd_serial
FROM poheader"

        Dim Params As New List(Of SearchVal)
        Params.Add(New SearchVal("pohd_pur_no", PO,, True))

        Dim NewGridForm As New GridForm(Parent, "MUNIS PO Info")
        NewGridForm.AddGrid("POGrid", "PO Info:", Await MunisComms.Return_MSSQLTableFromCmdAsync(MunisComms.SQLParamCommand(strQRY, Params)))
        NewGridForm.Show()
        DoneWaiting()
    End Sub
    Public Async Function NewMunisView_ReqSearch(ReqNumber As String, FY As String, Optional Parent As Form = Nothing, Optional SelectMode As Boolean = False) As Task(Of String)
        Waiting()
        If ReqNumber = "" Or FY = "" Then
            DoneWaiting()
            Return Nothing
        End If
        If Not SelectMode Then
            Dim NewGridForm As New GridForm(Parent, "MUNIS Requisition Info")
            NewGridForm.AddGrid("ReqGrid", "Requisition Info:", Await LoadMunisRequisitionGridByReqNo(ReqNumber, FY))
            NewGridForm.Show()
            DoneWaiting()
            Return Nothing
        Else
            Using NewGridForm As New GridForm(Parent, "MUNIS Requisition Info")
                DoneWaiting()
                NewGridForm.ShowDialog(Parent)
                If NewGridForm.DialogResult = DialogResult.OK Then
                    Return SelectedCellValue(NewGridForm.SelectedValue, "rqdt_uni_pr")
                Else
                    Return Nothing
                End If
            End Using
        End If
        DoneWaiting()
    End Function
    Private Async Function LoadMunisRequisitionGridByReqNo(ReqNumber As String, FiscalYr As String) As Task(Of DataTable)
        Try
            If ReqNumber = "" Or FiscalYr = "" Then Return Nothing
            Dim strQRY As String = "SELECT TOP " & intMaxResults & " dbo.rq_gl_info.rg_fiscal_year, dbo.rq_gl_info.a_requisition_no, dbo.rq_gl_info.rg_org, dbo.rq_gl_info.rg_object, dbo.rq_gl_info.a_org_description, dbo.rq_gl_info.a_object_desc, 
                         VEN.a_vendor_name, VEN.a_vendor_number, dbo.rqdetail.rqdt_pur_no, dbo.rqdetail.rqdt_pur_dt, dbo.rqdetail.rqdt_lin_no, dbo.rqdetail.rqdt_uni_pr, dbo.rqdetail.rqdt_net_pr,
                         dbo.rqdetail.rqdt_qty_no, dbo.rqdetail.rqdt_des_ln, dbo.rqdetail.rqdt_vdr_part_no
            From            dbo.rq_gl_info INNER JOIN
                         dbo.rqdetail ON dbo.rq_gl_info.rg_line_number = dbo.rqdetail.rqdt_lin_no AND dbo.rq_gl_info.a_requisition_no = dbo.rqdetail.rqdt_req_no AND dbo.rq_gl_info.rg_fiscal_year = dbo.rqdetail.rqdt_fsc_yr LEFT JOIN
                          (
SELECT TOP 1 a_vendor_number,a_vendor_name
FROM ap_vendor
WHERE a_vendor_number = " & MunisFunc.Get_VendorNumber_From_ReqNumber(ReqNumber, FiscalYr) & "
) VEN 
ON dbo.rqdetail.rqdt_sug_vn = VEN.a_vendor_number"
            Dim Params As New List(Of SearchVal)
            Params.Add(New SearchVal("dbo.rq_gl_info.a_requisition_no", ReqNumber,, True))
            Params.Add(New SearchVal("dbo.rq_gl_info.rg_fiscal_year", FiscalYr,, True))
            Dim ReqTable = Await MunisComms.Return_MSSQLTableFromCmdAsync(MunisComms.SQLParamCommand(strQRY, Params))
            If ReqTable.Rows.Count > 0 Then
                Return ReqTable
            Else
                Return Nothing
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Public Async Sub LoadMunisInfoByDevice(Device As Device_Info, ParentForm As Form)
        Waiting()
        Dim ReqTable, InvTable As New DataTable
        If Device.strPO <> "" Then
            Device.strFiscalYear = YearFromDate(Device.dtPurchaseDate)
            InvTable = Await LoadMunisInventoryGrid(Device)
            ReqTable = Await LoadMunisRequisitionGridByReqNo(Await Get_ReqNumber_From_PO_Async(Device.strPO), MunisFunc.Get_FY_From_PO(Device.strPO))
        Else
            If Device.strPO = "" Then
                Dim PO As String = Get_PO_From_Asset(Device.strAssetTag)
                If PO <> "" Then
                    Device.strPO = PO 'if some's missing -> try to find it by other means
                Else
                    PO = Get_PO_From_Serial(Device.strSerial)
                    If PO <> "" Then Device.strPO = PO
                End If
            End If
            If Device.strPO <> "" Then
                InvTable = Await LoadMunisInventoryGrid(Device)
                ReqTable = Await LoadMunisRequisitionGridByReqNo(Await Get_ReqNumber_From_PO_Async(Device.strPO), MunisFunc.Get_FY_From_PO(Device.strPO))
            Else
                InvTable = Await LoadMunisInventoryGrid(Device)
                ReqTable = Nothing
            End If
        End If
        If InvTable IsNot Nothing Or ReqTable IsNot Nothing Then
            DoneWaiting()
            Dim NewGridForm As New GridForm(ParentForm, "MUNIS Info")
            If InvTable Is Nothing Then
                Message("Could not pull Munis Fixed Asset info.", vbOKOnly + vbInformation, "No FA Record")
            Else
                NewGridForm.AddGrid("InvGrid", "FA Info:", InvTable)
            End If
            If ReqTable Is Nothing Then
                Message("Could not resolve PO from Asset Tag or Serial. Please add a valid PO if possible.", vbOKOnly + vbInformation, "No Req. Record")
            Else
                NewGridForm.AddGrid("ReqGrid", "Requisition Info:", ReqTable)
            End If
            NewGridForm.Show()
        ElseIf InvTable Is Nothing And ReqTable Is Nothing Then
            DoneWaiting()
            Message("Could not resolve any Req. or FA info.", vbOKOnly + vbInformation, "Nothing Found")
        End If
    End Sub
    Private Async Function LoadMunisInventoryGrid(Device As Device_Info) As Task(Of DataTable) 'TODO: Make this cleaner. 
        Try
            Dim GridData As New DataTable
            Dim strFields As String = "fama_asset,fama_status,fama_class,fama_subcl,fama_tag,fama_serial,fama_desc,fama_dept,fama_loc,FixedAssetLocations.LongDescription,fama_acq_dt,fama_fisc_yr,fama_pur_cost,fama_manuf,fama_model,fama_est_life,fama_repl_dt,fama_purch_memo"
            If Device.strSerial <> "" Then 'if serial is available, search FA by serial. Else, search by asset
                GridData = Await MunisComms.Return_MSSQLTableAsync("SELECT TOP 1 " & strFields & " FROM famaster INNER JOIN FixedAssetLocations ON FixedAssetLocations.Code = famaster.fama_loc WHERE fama_serial='" & Device.strSerial & "'")
                If GridData.Rows.Count > 0 Then 'if serial returned results, return results. Else, try search by Asset
                    Return GridData
                ElseIf GridData.Rows.Count < 1 AndAlso Device.strAssetTag <> "" Then
                    GridData = Await MunisComms.Return_MSSQLTableAsync("SELECT TOP 1 " & strFields & " FROM famaster INNER JOIN FixedAssetLocations ON FixedAssetLocations.Code = famaster.fama_loc WHERE fama_tag='" & Device.strAssetTag & "'")
                    If GridData.Rows.Count < 1 Then
                        Return Nothing
                    Else
                        Return GridData
                    End If
                End If
            ElseIf Device.strSerial = "" AndAlso Device.strAssetTag <> "" Then
                GridData = Await MunisComms.Return_MSSQLTableAsync("SELECT TOP 1 " & strFields & " FROM famaster INNER JOIN FixedAssetLocations ON FixedAssetLocations.Code = famaster.fama_loc WHERE fama_tag='" & Device.strAssetTag & "'")
                If GridData.Rows.Count < 1 Then
                    Return Nothing
                Else
                    Return GridData
                End If
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
    Private Sub Waiting()
        ' Application.UseWaitCursor = True
        SetWaitCursor(True)
    End Sub
    Private Sub DoneWaiting()
        ' Application.UseWaitCursor = False
        SetWaitCursor(False)
    End Sub
End Class
