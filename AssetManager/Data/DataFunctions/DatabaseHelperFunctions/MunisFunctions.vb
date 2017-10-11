Imports MyDialogLib
Imports System.Data.SqlClient

Public Class MunisFunctions 'Be warned. This whole class is a horrible bastard...
    Private Const intMaxResults As Integer = 100
    Private MunisComms As New MunisComms





    Public Function GetReqNumberFromPO(PO As String) As String
        If Not IsNothing(PO) Then
            If PO <> "" Then
                Return MunisComms.ReturnSqlValue("Requisitions", "PurchaseOrderNumber", PO, "RequisitionNumber").ToString
            End If
        End If
        Return Nothing
    End Function

    Private Async Function GetReqNumberFromPOAsync(PO As String) As Task(Of String)
        If PO <> "" Then
            Return Await MunisComms.ReturnSqlValueAsync("Requisitions", "PurchaseOrderNumber", PO, "RequisitionNumber")
        End If
        Return String.Empty
    End Function

    Public Async Function GetPOFromReqNumberAsync(reqNum As String, FY As String) As Task(Of String)
        If reqNum <> "" Then
            Return Await MunisComms.ReturnSqlValueAsync("rqdetail", "rqdt_req_no", reqNum, "rqdt_pur_no", "rqdt_fsc_yr", FY)
        End If
        Return String.Empty
    End Function

    Private Async Function GetPOFromDevice(device As DeviceObject) As Task(Of String)
        Dim PO As String
        If device.AssetTag IsNot Nothing AndAlso device.AssetTag <> "" Then
            PO = Await MunisComms.ReturnSqlValueAsync("famaster", "fama_tag", device.AssetTag, "fama_purch_memo")
            If PO <> "" Then
                Return Trim(PO.ToString)
            Else
                If device.Serial IsNot Nothing AndAlso device.Serial <> "" Then
                    PO = Await MunisComms.ReturnSqlValueAsync("famaster", "fama_serial", device.Serial, "fama_purch_memo")
                    Return Trim(PO.ToString)
                End If
            End If
        End If
        Return String.Empty
    End Function

    Private Function SelectedCellValue(ByRef gridRow As DataGridViewRow, Optional column As String = Nothing) As String
        For Each cell As DataGridViewCell In gridRow.Cells
            If column = "" Then
                If cell.Selected Then Return cell.Value.ToString
            Else
                If cell.OwningColumn.Name = column Then Return cell.Value.ToString
            End If
        Next
        Return String.Empty
    End Function

    Public Function GetSerialFromAsset(assetTag As String) As String
        Dim value = MunisComms.ReturnSqlValue("famaster", "fama_tag", assetTag, "fama_serial")
        If value IsNot Nothing Then
            Return Trim(value.ToString)
        End If
        Return String.Empty
    End Function

    Public Function GetAssetFromSerial(serial As String) As String
        Dim value = MunisComms.ReturnSqlValue("famaster", "fama_serial", serial, "fama_tag")
        If value IsNot Nothing Then
            Return Trim(value.ToString)
        End If
        Return String.Empty
    End Function

    Public Function GetFYFromAsset(assetTag As String) As String
        Return Trim(MunisComms.ReturnSqlValue("famaster", "fama_tag", assetTag, "fama_fisc_yr").ToString)
    End Function

    Public Function GetPODateFromPO(PO As String) As String
        Return YearFromDate(DateTime.Parse(Trim(MunisComms.ReturnSqlValue("RequisitionItems", "PurchaseOrderNumber", PO, "PurchaseOrderDate").ToString)))
    End Function

    Public Function GetVendorNameFromPO(PO As String) As String
        Dim VendorNumber = MunisComms.ReturnSqlValue("rqdetail", "rqdt_req_no", GetReqNumberFromPO(PO), "rqdt_sug_vn", "rqdt_fsc_yr", GetFYFromPO(PO))
        Return MunisComms.ReturnSqlValue("ap_vendor", "a_vendor_number", VendorNumber, "a_vendor_name").ToString
    End Function

    Public Async Function GetVendorNumberFromReqNumber(reqNum As String, FY As String) As Task(Of String)
        Dim VendorNum = Await MunisComms.ReturnSqlValueAsync("rqdetail", "rqdt_req_no", reqNum, "rqdt_sug_vn", "rqdt_fsc_yr", FY)
        If VendorNum IsNot Nothing Then
            Return VendorNum.ToString
        End If
        Return String.Empty
    End Function

    Public Function GetFYFromPO(PO As String) As String
        Dim TwoDigitYear As String = Left(PO, 2)
        Return "20" + TwoDigitYear
    End Function

    Public Async Function GetPOStatusFromPO(PO As Integer) As Task(Of String)
        Dim StatusString As String
        Dim StatusCode As String = Await MunisComms.ReturnSqlValueAsync("poheader", "pohd_pur_no", PO, "pohd_sta_cd")
        If StatusCode <> "" Then
            Dim ParseCode As Integer = -1
            If Not Int32.TryParse(StatusCode, ParseCode) Then Return Nothing
            StatusString = StatusCode.ToString & " - " & POStatusTextFromCode(ParseCode)
            Return StatusString
        End If
        Return Nothing
    End Function

    Public Async Function GetReqStatusFromReqNum(reqNum As String, FY As Integer) As Task(Of String)
        Dim StatusString As String
        Dim StatusCode As String = Await MunisComms.ReturnSqlValueAsync("rqheader", "rqhd_req_no", reqNum, "rqhd_sta_cd", "rqhd_fsc_yr", FY)
        If StatusCode <> "" Then
            Dim ParseCode As Integer = -1
            If Not Int32.TryParse(StatusCode, ParseCode) Then Return Nothing
            StatusString = StatusCode.ToString & " - " & ReqStatusTextFromCode(ParseCode)
            Return StatusString
        End If
        Return String.Empty
    End Function

    Private Function POStatusTextFromCode(code As Integer) As String
        Select Case code
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
        Return String.Empty
    End Function

    Private Function ReqStatusTextFromCode(code As Integer) As String
        Select Case code
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
        Return String.Empty
    End Function

    Public Sub AssetSearch(parentForm As ExtendedForm)
        Try
            Dim Device As New DeviceObject
            Device.PurchaseDate = Nothing
            Using NewDialog As New AdvancedDialog(parentForm)
                With NewDialog
                    .Text = "Asset Search"
                    .AddTextBox("txtAsset", "Asset:")
                    .AddTextBox("txtSerial", "Serial:")
                    .ShowDialog()
                    If .DialogResult = DialogResult.OK Then
                        Device.AssetTag = Trim(NewDialog.GetControlValue("txtAsset").ToString)
                        Device.Serial = Trim(NewDialog.GetControlValue("txtSerial").ToString)
                        LoadMunisInfoByDevice(Device, parentForm)
                    End If
                End With
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Public Sub NameSearch(parentForm As ExtendedForm)
        Try
            Using NewDialog As New AdvancedDialog(parentForm)
                With NewDialog
                    .Text = "Org/Object Code Search"
                    .AddTextBox("txtName", "First or Last Name:")
                    .ShowDialog()
                    If .DialogResult = DialogResult.OK Then
                        Dim strName = NewDialog.GetControlValue("txtName").ToString
                        If Trim(strName) IsNot "" Then
                            NewMunisEmployeeSearch(strName, parentForm)
                        End If
                    End If
                End With
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Public Sub POSearch(parentForm As ExtendedForm)
        Try
            Dim PO As String
            Using NewDialog As New AdvancedDialog(parentForm)
                With NewDialog
                    .Text = "PO Search"
                    .AddTextBox("txtPO", "PO #:")
                    .ShowDialog()
                    If .DialogResult = DialogResult.OK Then
                        PO = NewDialog.GetControlValue("txtPO").ToString
                        NewMunisPOSearch(PO, parentForm)
                    End If
                End With
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Public Async Sub ReqSearch(parentForm As ExtendedForm)
        Try
            Dim ReqNumber, FY As String
            Using NewDialog As New AdvancedDialog(parentForm)
                With NewDialog
                    .Text = "Req Search"
                    .AddTextBox("txtReqNum", "Requisition #:")
                    .AddTextBox("txtFY", "FY:")
                    .ShowDialog()
                    If .DialogResult = DialogResult.OK Then
                        ReqNumber = NewDialog.GetControlValue("txtReqNum").ToString
                        FY = NewDialog.GetControlValue("txtFY").ToString
                        If IsValidYear(FY) Then
                            SetWaitCursor(True, parentForm)
                            Dim blah = Await NewMunisReqSearch(ReqNumber, FY, parentForm)
                        Else
                            Message("Invalid year.", vbOKOnly + vbInformation, "Invalid", parentForm)
                        End If
                    End If
                End With
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            SetWaitCursor(False, parentForm)
        End Try
    End Sub

    Public Sub OrgObSearch(parentForm As ExtendedForm)
        Try
            Using NewDialog As New AdvancedDialog(parentForm)
                Dim strOrg, strObj, strFY As String
                With NewDialog
                    .Text = "Org/Object Code Search"
                    .AddTextBox("txtOrg", "Org Code:")
                    .AddTextBox("txtObj", "Object Code:")
                    .AddTextBox("txtFY", "Fiscal Year:")
                    .SetControlValue("txtFY", Now.Year)
                    .ShowDialog()
                    If .DialogResult = DialogResult.OK Then
                        strOrg = NewDialog.GetControlValue("txtOrg").ToString
                        strObj = NewDialog.GetControlValue("txtObj").ToString
                        strFY = NewDialog.GetControlValue("txtFY").ToString
                        If Trim(strOrg) IsNot "" And IsValidYear(strFY) Then
                            NewOrgObView(strOrg, strObj, strFY, parentForm)
                        End If
                    End If
                End With
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Public Function ListOfEmpsBySup(supEmpNum As String) As DataTable
        Dim strQRY As String = "SELECT TOP 100 a_employee_number FROM pr_employee_master WHERE e_supervisor='" & supEmpNum & "'"
        Return MunisComms.ReturnSqlTable(strQRY)
    End Function

    Public Async Sub NewOrgObView(org As String, obj As String, FY As String, parentForm As ExtendedForm)
        Try
            SetWaitCursor(True, parentForm)
            Dim NewGridForm As New GridForm(parentForm, "Org/Obj Info")
            Dim GLColumns As String = " glma_org, glma_obj, glma_desc, glma_seg5, glma_bud_yr, glma_orig_bud_cy, glma_rev_bud_cy, glma_encumb_cy, glma_memo_bal_cy, glma_rev_bud_cy-glma_encumb_cy-glma_memo_bal_cy AS 'Funds Available' "
            Dim GLMasterQry As String = "Select TOP " & intMaxResults & " " & GLColumns & "FROM glmaster"

            Dim GL_Params As New List(Of DBQueryParameter)
            GL_Params.Add(New DBQueryParameter("glma_org", org, True))

            If obj <> "" Then 'Show Rollup info for Object
                GL_Params.Add(New DBQueryParameter("glma_obj", obj, True))

                Dim RollUpCode As String = Await MunisComms.ReturnSqlValueAsync("gl_budget_rollup", "a_org", org, "a_rollup_code")
                Dim RollUpByCodeQry As String = "SELECT TOP " & intMaxResults & " * FROM gl_budget_rollup WHERE a_rollup_code = '" & RollUpCode & "'"
                Dim BudgetQry As String = "SELECT TOP " & intMaxResults & " a_projection_no,a_org,a_object,db_line,db_bud_desc_line1,db_bud_reason_desc,db_bud_req_qty5,db_bud_unit_cost,db_bud_req_amt5,a_account_id FROM gl_budget_detail_2" ' WHERE a_projection_no='" & FY & "' AND a_org='" & Org & "' AND a_object='" & Obj & "'"

                Dim Budget_Params As New List(Of DBQueryParameter)
                Budget_Params.Add(New DBQueryParameter("a_projection_no", FY, True))
                Budget_Params.Add(New DBQueryParameter("a_org", org, True))
                Budget_Params.Add(New DBQueryParameter("a_object", obj, True))

                NewGridForm.AddGrid("OrgGrid", "GL Info:", Await MunisComms.ReturnSqlTableFromCmdAsync(MunisComms.GetSqlCommandFromParams(GLMasterQry, GL_Params)))
                NewGridForm.AddGrid("RollupGrid", "Rollup Info:", Await MunisComms.ReturnSqlTableAsync(RollUpByCodeQry))
                NewGridForm.AddGrid("BudgetGrid", "Budget Info:", Await MunisComms.ReturnSqlTableFromCmdAsync(MunisComms.GetSqlCommandFromParams(BudgetQry, Budget_Params)))
            Else ' Show Rollup info for all Objects in Org
                Dim RollUpAllQry As String = "SELECT TOP " & intMaxResults & " * FROM gl_budget_rollup"

                Dim Roll_Params As New List(Of DBQueryParameter)
                Roll_Params.Add(New DBQueryParameter("a_org", org, True))

                NewGridForm.AddGrid("OrgGrid", "GL Info:", Await MunisComms.ReturnSqlTableFromCmdAsync(MunisComms.GetSqlCommandFromParams(GLMasterQry, GL_Params))) 'MunisComms.Return_MSSQLTableAsync(Qry))
                NewGridForm.AddGrid("RollupGrid", "Rollup Info:", Await MunisComms.ReturnSqlTableFromCmdAsync(MunisComms.GetSqlCommandFromParams(RollUpAllQry, Roll_Params))) 'MunisComms.Return_MSSQLTableAsync("SELECT TOP " & intMaxResults & " * FROM gl_budget_rollup WHERE a_org = '" & Org & "'"))
            End If
            NewGridForm.Show()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            SetWaitCursor(False, parentForm)
        End Try
    End Sub

    Private Async Sub NewMunisEmployeeSearch(name As String, parentForm As ExtendedForm)
        Try
            SetWaitCursor(True, parentForm)
            Dim strColumns As String = "e.a_employee_number,e.a_name_last,e.a_name_first,e.a_org_primary,e.a_object_primary,e.a_location_primary,e.a_location_p_desc,e.a_location_p_short,e.e_work_location,m.a_employee_number as sup_employee_number,m.a_name_first as sup_name_first,m.a_name_last as sup_name_last"
            Dim strQRY As String = "SELECT TOP " & intMaxResults & " " & strColumns & "
FROM pr_employee_master e
INNER JOIN pr_employee_master m on e.e_supervisor = m.a_employee_number"

            Dim Params As New List(Of DBQueryParameter)
            Params.Add(New DBQueryParameter("e.a_name_last", UCase(name), "OR"))
            Params.Add(New DBQueryParameter("e.a_name_first", UCase(name), "OR"))

            Dim NewGridForm As New GridForm(parentForm, "MUNIS Employee Info")
            Using cmd = MunisComms.GetSqlCommandFromParams(strQRY, Params),
                results = Await MunisComms.ReturnSqlTableFromCmdAsync(cmd)
                If HasResults(results, parentForm) Then
                    NewGridForm.AddGrid("EmpGrid", "MUNIS Info:", results)
                    NewGridForm.Show()
                End If
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            SetWaitCursor(False, parentForm)
        End Try
    End Sub

    Public Async Sub NewMunisPOSearch(PO As String, parentForm As ExtendedForm)
        Try
            SetWaitCursor(True, parentForm)
            If PO = "" Then Exit Sub
            Dim strQRY As String = "SELECT TOP " & intMaxResults & " pohd_pur_no, pohd_fsc_yr, pohd_req_no, pohd_gen_cm, pohd_buy_id, pohd_pre_dt, pohd_exp_dt, pohd_sta_cd, pohd_vnd_cd, pohd_dep_cd, pohd_shp_cd, pohd_tot_amt, pohd_serial
FROM poheader"

            Dim Params As New List(Of DBQueryParameter)
            Params.Add(New DBQueryParameter("pohd_pur_no", PO, True))

            Dim NewGridForm As New GridForm(parentForm, "MUNIS PO Info")
            Using cmd = MunisComms.GetSqlCommandFromParams(strQRY, Params),
                results = Await MunisComms.ReturnSqlTableFromCmdAsync(cmd)
                If HasResults(results, parentForm) Then
                    NewGridForm.AddGrid("POGrid", "PO Info:", results)
                    NewGridForm.Show()
                End If
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            SetWaitCursor(False, parentForm)
        End Try
    End Sub

    Public Async Function NewMunisReqSearch(reqNumber As String, FY As String, parentForm As ExtendedForm, Optional selectMode As Boolean = False) As Task(Of String)
        If reqNumber = "" Or FY = "" Then
            Return Nothing
        End If
        Dim NewGridForm As New GridForm(parentForm, "MUNIS Requisition Info")
        Using ReqLineItemsTable = Await GetReqLineItemsFromReqNum(reqNumber, FY)
            If HasResults(ReqLineItemsTable, parentForm) Then
                If Not selectMode Then
                    Using ReqHeaderTable = Await GetReqHeaderFromReqNum(reqNumber, FY)
                        NewGridForm.AddGrid("ReqHeaderGrid", "Requisition Header:", ReqHeaderTable)
                    End Using
                    NewGridForm.AddGrid("ReqLineGrid", "Requisition Line Items:", ReqLineItemsTable)
                    NewGridForm.Show()
                    Return Nothing
                Else
                    NewGridForm.AddGrid("ReqLineGrid", "Requisition Line Items:", ReqLineItemsTable)
                    NewGridForm.ShowDialog(parentForm)
                    If NewGridForm.DialogResult = DialogResult.OK Then
                        Return SelectedCellValue(NewGridForm.SelectedValue, "rqdt_uni_pr")
                    End If
                End If
            End If
        End Using
        Return Nothing
    End Function

    Private Function HasResults(results As DataTable, parentForm As Form) As Boolean
        If results IsNot Nothing AndAlso results.Rows.Count > 0 Then
            Return True
        Else
            Message("No results found.", vbOKOnly + vbInformation, "No results", parentForm)
            Return False
        End If
    End Function

    Private Async Function GetReqHeaderFromReqNum(reqNumber As String, fiscalYr As String) As Task(Of DataTable)
        If reqNumber = "" Or fiscalYr = "" Then Return Nothing
        Dim Query As String = "SELECT TOP " & intMaxResults & " * FROM rqheader"
        Dim Params As New List(Of DBQueryParameter)
        Params.Add(New DBQueryParameter("rqhd_req_no", reqNumber, True))
        Params.Add(New DBQueryParameter("rqhd_fsc_yr", fiscalYr, True))
        Return Await MunisComms.ReturnSqlTableFromCmdAsync(MunisComms.GetSqlCommandFromParams(Query, Params))
    End Function

    Private Async Function GetReqLineItemsFromReqNum(reqNumber As String, fiscalYr As String) As Task(Of DataTable)
        If reqNumber = "" Or fiscalYr = "" Then Return Nothing
        Dim VendorNum = Await MunisFunc.GetVendorNumberFromReqNumber(reqNumber, fiscalYr)
        If VendorNum = "" Then Return Nothing
        Dim VendorName As String = Await MunisComms.ReturnSqlValueAsync("ap_vendor", "a_vendor_number", VendorNum, "a_vendor_name")
        Dim strQRY As String = "SELECT TOP " & intMaxResults & " dbo.rq_gl_info.rg_fiscal_year, dbo.rq_gl_info.a_requisition_no, dbo.rq_gl_info.rg_org, dbo.rq_gl_info.rg_object, dbo.rq_gl_info.a_org_description, dbo.rq_gl_info.a_object_desc,
 '" & VendorName & "' AS a_vendor_name, '" & VendorNum & "' AS a_vendor_number, dbo.rqdetail.rqdt_pur_no, dbo.rqdetail.rqdt_pur_dt, dbo.rqdetail.rqdt_lin_no, dbo.rqdetail.rqdt_uni_pr, dbo.rqdetail.rqdt_net_pr, dbo.rqdetail.rqdt_qty_no, dbo.rqdetail.rqdt_des_ln, dbo.rqdetail.rqdt_vdr_part_no
FROM dbo.rq_gl_info INNER JOIN
dbo.rqdetail ON dbo.rq_gl_info.rg_line_number = dbo.rqdetail.rqdt_lin_no AND dbo.rq_gl_info.a_requisition_no = dbo.rqdetail.rqdt_req_no AND dbo.rq_gl_info.rg_fiscal_year = dbo.rqdetail.rqdt_fsc_yr"
        Dim Params As New List(Of DBQueryParameter)
        Params.Add(New DBQueryParameter("dbo.rq_gl_info.a_requisition_no", reqNumber, True))
        Params.Add(New DBQueryParameter("dbo.rq_gl_info.rg_fiscal_year", fiscalYr, True))
        Dim ReqTable = Await MunisComms.ReturnSqlTableFromCmdAsync(MunisComms.GetSqlCommandFromParams(strQRY, Params))
        If ReqTable.Rows.Count > 0 Then
            Return ReqTable
        Else
            Return Nothing
        End If
    End Function

    Public Async Sub LoadMunisInfoByDevice(device As DeviceObject, parentForm As ExtendedForm)
        Try
            SetWaitCursor(True, parentForm)
            Dim ReqLinesTable, ReqHeaderTable, InventoryTable As New DataTable

            If device.PO = "" Then device.PO = Await GetPOFromDevice(device)

            If device.PO <> "" Then
                InventoryTable = Await LoadMunisInventoryGrid(device)
                ReqLinesTable = Await GetReqLineItemsFromReqNum(Await GetReqNumberFromPOAsync(device.PO), MunisFunc.GetFYFromPO(device.PO))
                ReqHeaderTable = Await GetReqHeaderFromReqNum(Await GetReqNumberFromPOAsync(device.PO), MunisFunc.GetFYFromPO(device.PO))
            Else
                InventoryTable = Await LoadMunisInventoryGrid(device)
                ReqLinesTable = Nothing
                ReqHeaderTable = Nothing
            End If
            If InventoryTable IsNot Nothing Or ReqLinesTable IsNot Nothing Then
                Dim NewGridForm As New GridForm(parentForm, "MUNIS Info")
                If InventoryTable Is Nothing Then
                    Message("Could not pull Munis Fixed Asset info.", vbOKOnly + vbInformation, "No FA Record")
                Else
                    NewGridForm.AddGrid("InvGrid", "FA Info:", InventoryTable)
                End If
                If ReqLinesTable Is Nothing Then
                    Message("Could not resolve PO from Asset Tag or Serial. Please add a valid PO if possible.", vbOKOnly + vbInformation, "No Req. Record")
                Else
                    NewGridForm.AddGrid("ReqHeadGrid", "Requisition Header:", ReqHeaderTable)
                    NewGridForm.AddGrid("ReqLineGrid", "Requisition Line Items:", ReqLinesTable)
                End If
                NewGridForm.Show()
            ElseIf InventoryTable Is Nothing And ReqLinesTable Is Nothing Then
                Message("Could not resolve any Req. or FA info.", vbOKOnly + vbInformation, "Nothing Found")
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            SetWaitCursor(False, parentForm)
        End Try
    End Sub

    Private Async Function LoadMunisInventoryGrid(device As DeviceObject) As Task(Of DataTable)
        Dim strFields As String = "fama_asset,fama_status,fama_class,fama_subcl,fama_tag,fama_serial,fama_desc,fama_dept,fama_loc,FixedAssetLocations.LongDescription,fama_acq_dt,fama_fisc_yr,fama_pur_cost,fama_manuf,fama_model,fama_est_life,fama_repl_dt,fama_purch_memo"
        Dim Query As String = "SELECT TOP 1 " & strFields & " FROM famaster INNER JOIN FixedAssetLocations ON FixedAssetLocations.Code = famaster.fama_loc WHERE fama_tag='" & device.AssetTag & "' AND fama_tag <> '' OR fama_serial='" & device.Serial & "' AND fama_serial <> ''"
        Return Await MunisComms.ReturnSqlTableAsync(Query)
    End Function

End Class