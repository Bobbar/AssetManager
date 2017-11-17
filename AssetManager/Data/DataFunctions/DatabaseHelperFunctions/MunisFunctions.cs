using AssetManager.UserInterface.CustomControls;
using AssetManager.UserInterface.Forms;
using MyDialogLib;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AssetManager
{
    public class MunisFunctions //Be warned. This whole class is a horrible bastard...
    {
        private const int intMaxResults = 100;
        private MunisComms MunisComms = new MunisComms();

        public string GetReqNumberFromPO(string PO)
        {
            if (!ReferenceEquals(PO, null))
            {
                if (PO != "")
                {
                    return MunisComms.ReturnSqlValue("Requisitions", "PurchaseOrderNumber", PO, "RequisitionNumber").ToString();
                }
            }
            return null;
        }

        private async Task<string> GetReqNumberFromPOAsync(string PO)
        {
            if (PO != "")
            {
                return await MunisComms.ReturnSqlValueAsync("Requisitions", "PurchaseOrderNumber", PO, "RequisitionNumber");
            }
            return string.Empty;
        }

        public async Task<string> GetPOFromReqNumberAsync(string reqNum, string FY)
        {
            if (reqNum != "")
            {
                return await MunisComms.ReturnSqlValueAsync("rqdetail", "rqdt_req_no", reqNum, "rqdt_pur_no", "rqdt_fsc_yr", FY);
            }
            return string.Empty;
        }

        public async Task<string> GetPOFromDevice(DeviceObject device)
        {
            string POFromAsset = "";
            string POFromSerial = "";
            string POFromAssetFromPurchaseHist = "";

            if (device.AssetTag != null && device.AssetTag != "")
            {
                POFromAsset = await MunisComms.ReturnSqlValueAsync("famaster", "fama_tag", device.AssetTag, "fama_purch_memo");
                POFromAssetFromPurchaseHist = await MunisComms.ReturnSqlValueAsync("fapurchh", "faph_asset", device.AssetTag, "faph_po_num");
            }

            if (device.Serial != null && device.Serial != "")
            {
                POFromSerial = await MunisComms.ReturnSqlValueAsync("famaster", "fama_serial", device.Serial, "fama_purch_memo");
            }

            POFromAsset = POFromAsset.Trim();
            POFromSerial = POFromSerial.Trim();

            if (!string.IsNullOrEmpty(POFromAsset))
            {
                return POFromAsset;
            }
            else if (!string.IsNullOrEmpty(POFromSerial))
            {
                return POFromSerial;
            }
            else if (!string.IsNullOrEmpty(POFromAssetFromPurchaseHist))
            {
                return POFromAssetFromPurchaseHist;
            }

            return string.Empty;
        }

        private string SelectedCellValue(DataGridViewRow gridRow, string column = null)
        {
            foreach (DataGridViewCell cell in gridRow.Cells)
            {
                if (column == "")
                {
                    if (cell.Selected)
                    {
                        return cell.Value.ToString();
                    }
                }
                else
                {
                    if (cell.OwningColumn.Name == column)
                    {
                        return cell.Value.ToString();
                    }
                }
            }
            return string.Empty;
        }

        public string GetSerialFromAsset(string assetTag)
        {
            var value = MunisComms.ReturnSqlValue("famaster", "fama_tag", assetTag, "fama_serial");
            if (value != null)
            {
                return value.ToString().Trim();
            }
            return string.Empty;
        }

        public string GetAssetFromSerial(string serial)
        {
            var value = MunisComms.ReturnSqlValue("famaster", "fama_serial", serial, "fama_tag");
            if (value != null)
            {
                return value.ToString().Trim();
            }
            return string.Empty;
        }

        public string GetFYFromAsset(string assetTag)
        {
            return MunisComms.ReturnSqlValue("famaster", "fama_tag", assetTag, "fama_fisc_yr").ToString().Trim();
        }

        public DateTime GetPODate(string PO)
        {
            try
            {
                return DateTime.Parse(MunisComms.ReturnSqlValue("RequisitionItems", "PurchaseOrderNumber", PO, "PurchaseOrderDate").ToString().Trim());
            }
            catch (Exception)
            {
                return DateTime.Now;
            }
        }

        public string GetVendorNameFromPO(string PO)
        {
            var VendorNumber = MunisComms.ReturnSqlValue("rqdetail", "rqdt_req_no", GetReqNumberFromPO(PO), "rqdt_sug_vn", "rqdt_fsc_yr", GetFYFromPO(PO));
            return MunisComms.ReturnSqlValue("ap_vendor", "a_vendor_number", VendorNumber, "a_vendor_name").ToString();
        }

        public async Task<string> GetVendorNumberFromReqNumber(string reqNum, string FY)
        {
            var VendorNum = await MunisComms.ReturnSqlValueAsync("rqdetail", "rqdt_req_no", reqNum, "rqdt_sug_vn", "rqdt_fsc_yr", FY);
            if (VendorNum != null)
            {
                return VendorNum.ToString();
            }
            return string.Empty;
        }

        public string GetFYFromPO(string PO)
        {
            string TwoDigitYear = PO.Substring(0, 2);
            return "20" + TwoDigitYear;
        }

        public async Task<string> GetPOStatusFromPO(int PO)
        {
            string StatusString = "";
            string StatusCode = await MunisComms.ReturnSqlValueAsync("poheader", "pohd_pur_no", PO, "pohd_sta_cd");
            if (!string.IsNullOrEmpty(StatusCode))
            {
                int ParseCode = -1;
                if (!int.TryParse(StatusCode, out ParseCode))
                {
                    return string.Empty;
                }
                StatusString = StatusCode.ToString() + " - " + POStatusTextFromCode(ParseCode);
                return StatusString;
            }
            return string.Empty;
        }

        public async Task<string> GetReqStatusFromReqNum(string reqNum, int FY)
        {
            string StatusString = "";
            string StatusCode = await MunisComms.ReturnSqlValueAsync("rqheader", "rqhd_req_no", reqNum, "rqhd_sta_cd", "rqhd_fsc_yr", FY);
            if (!string.IsNullOrEmpty(StatusCode))
            {
                int ParseCode = -1;
                if (!int.TryParse(StatusCode, out ParseCode))
                {
                    return string.Empty;
                }
                StatusString = StatusCode.ToString() + " - " + ReqStatusTextFromCode(ParseCode);
                return StatusString;
            }
            return string.Empty;
        }

        private string POStatusTextFromCode(int code)
        {
            switch (code)
            {
                case 0:
                    return "Closed";

                case 1:
                    return "Rejected";

                case 2:
                    return "Created";

                case 4:
                    return "Allocated";

                case 5:
                    return "Released";

                case 6:
                    return "Posted";

                case 8:
                    return "Printed";

                case 9:
                    return "Carry Forward";

                case 10:
                    return "Canceled";

                case 11:
                    return "Closed";
            }
            return string.Empty;
        }

        private string ReqStatusTextFromCode(int code)
        {
            switch (code)
            {
                case 0:
                    return "Converted";

                case 1:
                    return "Rejected";

                case 2:
                    return "Created";

                case 4:
                    return "Allocated";

                case 6:
                    return "Released";

                default:
                    return "NA";
            }
        }

        public void AssetSearch(ExtendedForm parentForm)
        {
            try
            {
                DeviceObject Device = new DeviceObject();
                using (AdvancedDialog NewDialog = new AdvancedDialog(parentForm))
                {
                    NewDialog.Text = "Asset Search";
                    NewDialog.AddTextBox("txtAsset", "Asset:");
                    NewDialog.AddTextBox("txtSerial", "Serial:");
                    NewDialog.ShowDialog();
                    if (NewDialog.DialogResult == DialogResult.OK)
                    {
                        Device.AssetTag = NewDialog.GetControlValue("txtAsset").ToString().Trim();
                        Device.Serial = NewDialog.GetControlValue("txtSerial").ToString().Trim();
                        LoadMunisInfoByDevice(Device, parentForm);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        public void NameSearch(ExtendedForm parentForm)
        {
            try
            {
                using (AdvancedDialog NewDialog = new AdvancedDialog(parentForm))
                {
                    NewDialog.Text = "Org/Object Code Search";
                    NewDialog.AddTextBox("txtName", "First or Last Name:");
                    NewDialog.ShowDialog();
                    if (NewDialog.DialogResult == DialogResult.OK)
                    {
                        var strName = NewDialog.GetControlValue("txtName").ToString();
                        if (strName.Trim() != "")
                        {
                            NewMunisEmployeeSearch(strName.Trim(), parentForm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        public void POSearch(ExtendedForm parentForm)
        {
            try
            {
                string PO = "";
                using (AdvancedDialog NewDialog = new AdvancedDialog(parentForm))
                {
                    NewDialog.Text = "PO Search";
                    NewDialog.AddTextBox("txtPO", "PO #:");
                    NewDialog.ShowDialog();
                    if (NewDialog.DialogResult == DialogResult.OK)
                    {
                        PO = NewDialog.GetControlValue("txtPO").ToString();
                        NewMunisPOSearch(PO, parentForm);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        public async void ReqSearch(ExtendedForm parentForm)
        {
            try
            {
                string ReqNumber = "";
                string FY = "";
                using (AdvancedDialog NewDialog = new AdvancedDialog(parentForm))
                {
                    NewDialog.Text = "Req Search";
                    NewDialog.AddTextBox("txtReqNum", "Requisition #:");
                    NewDialog.AddTextBox("txtFY", "FY:");
                    NewDialog.ShowDialog();
                    if (NewDialog.DialogResult == DialogResult.OK)
                    {
                        ReqNumber = NewDialog.GetControlValue("txtReqNum").ToString();
                        FY = NewDialog.GetControlValue("txtFY").ToString();
                        if (DataConsistency.IsValidYear(FY))
                        {
                            OtherFunctions.SetWaitCursor(true, parentForm);
                            var blah = await NewMunisReqSearch(ReqNumber, FY, parentForm);
                        }
                        else
                        {
                            OtherFunctions.Message("Invalid year.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Invalid", parentForm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                OtherFunctions.SetWaitCursor(false, parentForm);
            }
        }

        public void OrgObSearch(ExtendedForm parentForm)
        {
            try
            {
                using (AdvancedDialog NewDialog = new AdvancedDialog(parentForm))
                {
                    string strOrg = "";
                    string strObj = "";
                    string strFY = "";
                    NewDialog.Text = "Org/Object Code Search";
                    NewDialog.AddTextBox("txtOrg", "Org Code:");
                    NewDialog.AddTextBox("txtObj", "Object Code:");
                    NewDialog.AddTextBox("txtFY", "Fiscal Year:");
                    NewDialog.SetControlValue("txtFY", DateTime.Now.Year);
                    NewDialog.ShowDialog();
                    if (NewDialog.DialogResult == DialogResult.OK)
                    {
                        strOrg = NewDialog.GetControlValue("txtOrg").ToString();
                        strObj = NewDialog.GetControlValue("txtObj").ToString();
                        strFY = NewDialog.GetControlValue("txtFY").ToString();
                        if (strOrg.Trim() != "" && DataConsistency.IsValidYear(strFY))
                        {
                            NewOrgObView(strOrg, strObj, strFY, parentForm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        public DataTable ListOfEmpsBySup(string supEmpNum)
        {
            string strQRY = "SELECT TOP 100 a_employee_number FROM pr_employee_master WHERE e_supervisor='" + supEmpNum + "'";
            return MunisComms.ReturnSqlTable(strQRY);
        }

        public async void NewOrgObView(string org, string obj, string FY, ExtendedForm parentForm)
        {
            try
            {
                OtherFunctions.SetWaitCursor(true, parentForm);
                GridForm NewGridForm = new GridForm(parentForm, "Org/Obj Info");
                string GLColumns = " glma_org, glma_obj, glma_desc, glma_seg5, glma_bud_yr, glma_orig_bud_cy, glma_rev_bud_cy, glma_encumb_cy, glma_memo_bal_cy, glma_rev_bud_cy-glma_encumb_cy-glma_memo_bal_cy AS 'Funds Available' ";
                string GLMasterQry = "Select TOP " + intMaxResults + " " + GLColumns + "FROM glmaster";

                List<DBQueryParameter> GL_Params = new List<DBQueryParameter>();
                GL_Params.Add(new DBQueryParameter("glma_org", org, true));

                if (obj != "") //Show Rollup info for Object
                {
                    GL_Params.Add(new DBQueryParameter("glma_obj", obj, true));

                    string RollUpCode = await MunisComms.ReturnSqlValueAsync("gl_budget_rollup", "a_org", org, "a_rollup_code");
                    string RollUpByCodeQry = "SELECT TOP " + intMaxResults + " * FROM gl_budget_rollup WHERE a_rollup_code = '" + RollUpCode + "'";
                    string BudgetQry = "SELECT TOP " + intMaxResults + " a_projection_no,a_org,a_object,db_line,db_bud_desc_line1,db_bud_reason_desc,db_bud_req_qty5,db_bud_unit_cost,db_bud_req_amt5,a_account_id FROM gl_budget_detail_2"; // WHERE a_projection_no='" & FY & "' AND a_org='" & Org & "' AND a_object='" & Obj & "'"

                    List<DBQueryParameter> Budget_Params = new List<DBQueryParameter>();
                    Budget_Params.Add(new DBQueryParameter("a_projection_no", FY, true));
                    Budget_Params.Add(new DBQueryParameter("a_org", org, true));
                    Budget_Params.Add(new DBQueryParameter("a_object", obj, true));

                    NewGridForm.AddGrid("OrgGrid", "GL Info:", await MunisComms.ReturnSqlTableFromCmdAsync(MunisComms.GetSqlCommandFromParams(GLMasterQry, GL_Params)));
                    NewGridForm.AddGrid("RollupGrid", "Rollup Info:", await MunisComms.ReturnSqlTableAsync(RollUpByCodeQry));
                    NewGridForm.AddGrid("BudgetGrid", "Budget Info:", await MunisComms.ReturnSqlTableFromCmdAsync(MunisComms.GetSqlCommandFromParams(BudgetQry, Budget_Params)));
                }
                else // Show Rollup info for all Objects in Org
                {
                    string RollUpAllQry = "SELECT TOP " + intMaxResults + " * FROM gl_budget_rollup";

                    List<DBQueryParameter> Roll_Params = new List<DBQueryParameter>();
                    Roll_Params.Add(new DBQueryParameter("a_org", org, true));

                    NewGridForm.AddGrid("OrgGrid", "GL Info:", await MunisComms.ReturnSqlTableFromCmdAsync(MunisComms.GetSqlCommandFromParams(GLMasterQry, GL_Params))); //MunisComms.Return_MSSQLTableAsync(Qry))
                    NewGridForm.AddGrid("RollupGrid", "Rollup Info:", await MunisComms.ReturnSqlTableFromCmdAsync(MunisComms.GetSqlCommandFromParams(RollUpAllQry, Roll_Params))); //MunisComms.Return_MSSQLTableAsync("SELECT TOP " & intMaxResults & " * FROM gl_budget_rollup WHERE a_org = '" & Org & "'"))
                }
                NewGridForm.Show();
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                OtherFunctions.SetWaitCursor(false, parentForm);
            }
        }

        private async void NewMunisEmployeeSearch(string name, ExtendedForm parentForm)
        {
            try
            {
                OtherFunctions.SetWaitCursor(true, parentForm);
                string strColumns = "e.a_employee_number,e.a_name_last,e.a_name_first,e.a_org_primary,e.a_object_primary,e.a_location_primary,e.a_location_p_desc,e.a_location_p_short,e.e_work_location,m.a_employee_number as sup_employee_number,m.a_name_first as sup_name_first,m.a_name_last as sup_name_last";
                string strQRY = "SELECT TOP " + intMaxResults + " " + strColumns + @"
FROM pr_employee_master e
INNER JOIN pr_employee_master m on e.e_supervisor = m.a_employee_number";

                List<DBQueryParameter> Params = new List<DBQueryParameter>();
                @Params.Add(new DBQueryParameter("e.a_name_last", name.ToUpper(), "OR"));
                @Params.Add(new DBQueryParameter("e.a_name_first", name.ToUpper(), "OR"));

                GridForm NewGridForm = new GridForm(parentForm, "MUNIS Employee Info");
                using (var cmd = MunisComms.GetSqlCommandFromParams(strQRY, @Params))
                {
                    using (var results = await MunisComms.ReturnSqlTableFromCmdAsync(cmd))
                    {
                        if (HasResults(results, parentForm))
                        {
                            NewGridForm.AddGrid("EmpGrid", "MUNIS Info:", results);
                            NewGridForm.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                OtherFunctions.SetWaitCursor(false, parentForm);
            }
        }

        public async void NewMunisPOSearch(string PO, ExtendedForm parentForm)
        {
            try
            {
                OtherFunctions.SetWaitCursor(true, parentForm);
                if (PO == "")
                {
                    return;
                }
                string strQRY = "SELECT TOP " + intMaxResults + @" pohd_pur_no, pohd_fsc_yr, pohd_req_no, pohd_gen_cm, pohd_buy_id, pohd_pre_dt, pohd_exp_dt, pohd_sta_cd, pohd_vnd_cd, pohd_dep_cd, pohd_shp_cd, pohd_tot_amt, pohd_serial
FROM poheader";

                List<DBQueryParameter> Params = new List<DBQueryParameter>();
                @Params.Add(new DBQueryParameter("pohd_pur_no", PO, true));

                GridForm NewGridForm = new GridForm(parentForm, "MUNIS PO Info");
                using (var cmd = MunisComms.GetSqlCommandFromParams(strQRY, @Params))
                {
                    using (var results = await MunisComms.ReturnSqlTableFromCmdAsync(cmd))
                    {
                        if (HasResults(results, parentForm))
                        {
                            NewGridForm.AddGrid("POGrid", "PO Info:", results);
                            NewGridForm.Show();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                OtherFunctions.SetWaitCursor(false, parentForm);
            }
        }

        public async Task<string> NewMunisReqSearch(string reqNumber, string FY, ExtendedForm parentForm, bool selectMode = false)
        {
            if (reqNumber == "" || FY == "")
            {
                return string.Empty;
            }
            GridForm NewGridForm = new GridForm(parentForm, "MUNIS Requisition Info");
            using (var ReqLineItemsTable = await GetReqLineItemsFromReqNum(reqNumber, FY))
            {
                if (HasResults(ReqLineItemsTable, parentForm))
                {
                    if (!selectMode)
                    {
                        using (var ReqHeaderTable = await GetReqHeaderFromReqNum(reqNumber, FY))
                        {
                            NewGridForm.AddGrid("ReqHeaderGrid", "Requisition Header:", ReqHeaderTable);
                        }

                        NewGridForm.AddGrid("ReqLineGrid", "Requisition Line Items:", ReqLineItemsTable);
                        NewGridForm.Show();
                        return string.Empty;
                    }
                    else
                    {
                        NewGridForm.AddGrid("ReqLineGrid", "Requisition Line Items:", ReqLineItemsTable);
                        NewGridForm.ShowDialog(parentForm);
                        if (NewGridForm.DialogResult == DialogResult.OK)
                        {
                            return SelectedCellValue(NewGridForm.SelectedValue, "rqdt_uni_pr");
                        }
                    }
                }
            }

            return string.Empty;
        }

        private bool HasResults(DataTable results, Form parentForm)
        {
            if (results != null && results.Rows.Count > 0)
            {
                return true;
            }
            else
            {
                OtherFunctions.Message("No results found.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "No results", parentForm);
                return false;
            }
        }

        private async Task<DataTable> GetReqHeaderFromReqNum(string reqNumber, string fiscalYr)
        {
            if (reqNumber == "" || fiscalYr == "")
            {
                return null;
            }
            string Query = "SELECT TOP " + intMaxResults + " * FROM rqheader";
            List<DBQueryParameter> Params = new List<DBQueryParameter>();
            @Params.Add(new DBQueryParameter("rqhd_req_no", reqNumber, true));
            @Params.Add(new DBQueryParameter("rqhd_fsc_yr", fiscalYr, true));
            return await MunisComms.ReturnSqlTableFromCmdAsync(MunisComms.GetSqlCommandFromParams(Query, @Params));
        }

        private async Task<DataTable> GetReqLineItemsFromReqNum(string reqNumber, string fiscalYr)
        {
            if (reqNumber == "" || fiscalYr == "")
            {
                return null;
            }
            var VendorNum = await GetVendorNumberFromReqNumber(reqNumber, fiscalYr);
            if ((string)VendorNum == "")
            {
                return null;
            }
            string VendorName = await MunisComms.ReturnSqlValueAsync("ap_vendor", "a_vendor_number", VendorNum, "a_vendor_name");
            string strQRY = "SELECT TOP " + intMaxResults + @" dbo.rq_gl_info.rg_fiscal_year, dbo.rq_gl_info.a_requisition_no, dbo.rq_gl_info.rg_org, dbo.rq_gl_info.rg_object, dbo.rq_gl_info.a_org_description, dbo.rq_gl_info.a_object_desc,
'" + VendorName + "' AS a_vendor_name, '" + VendorNum + @"' AS a_vendor_number, dbo.rqdetail.rqdt_pur_no, dbo.rqdetail.rqdt_pur_dt, dbo.rqdetail.rqdt_lin_no, dbo.rqdetail.rqdt_uni_pr, dbo.rqdetail.rqdt_net_pr, dbo.rqdetail.rqdt_qty_no, dbo.rqdetail.rqdt_des_ln, dbo.rqdetail.rqdt_vdr_part_no
FROM dbo.rq_gl_info INNER JOIN
dbo.rqdetail ON dbo.rq_gl_info.rg_line_number = dbo.rqdetail.rqdt_lin_no AND dbo.rq_gl_info.a_requisition_no = dbo.rqdetail.rqdt_req_no AND dbo.rq_gl_info.rg_fiscal_year = dbo.rqdetail.rqdt_fsc_yr";
            List<DBQueryParameter> Params = new List<DBQueryParameter>();
            @Params.Add(new DBQueryParameter("dbo.rq_gl_info.a_requisition_no", reqNumber, true));
            @Params.Add(new DBQueryParameter("dbo.rq_gl_info.rg_fiscal_year", fiscalYr, true));
            var ReqTable = await MunisComms.ReturnSqlTableFromCmdAsync(MunisComms.GetSqlCommandFromParams(strQRY, @Params));
            if (ReqTable.Rows.Count > 0)
            {
                return ReqTable;
            }
            else
            {
                return null;
            }
        }

        public async void LoadMunisInfoByDevice(DeviceObject device, ExtendedForm parentForm)
        {
            try
            {
                OtherFunctions.SetWaitCursor(true, parentForm);
                DataTable ReqLinesTable = new DataTable();
                DataTable ReqHeaderTable = new DataTable();
                DataTable InventoryTable = new DataTable();

                if (device.PO == "" || device.PO == null)
                {
                    device.PO = await GetPOFromDevice(device);
                }

                if (device.PO != string.Empty)
                {
                    InventoryTable = await LoadMunisInventoryGrid(device);
                    ReqLinesTable = await GetReqLineItemsFromReqNum(await GetReqNumberFromPOAsync(device.PO), GetFYFromPO(device.PO));
                    ReqHeaderTable = await GetReqHeaderFromReqNum(await GetReqNumberFromPOAsync(device.PO), GetFYFromPO(device.PO));
                }
                else
                {
                    InventoryTable = await LoadMunisInventoryGrid(device);
                    ReqLinesTable = null;
                    ReqHeaderTable = null;
                }
                if (InventoryTable != null || ReqLinesTable != null)
                {
                    GridForm NewGridForm = new GridForm(parentForm, "MUNIS Info");
                    if (ReferenceEquals(InventoryTable, null))
                    {
                        OtherFunctions.Message("Could not pull Munis Fixed Asset info.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "No FA Record");
                    }
                    else
                    {
                        NewGridForm.AddGrid("InvGrid", "FA Info:", InventoryTable);
                    }
                    if (ReferenceEquals(ReqLinesTable, null))
                    {
                        OtherFunctions.Message("Could not resolve PO from Asset Tag or Serial. Please add a valid PO if possible.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "No Req. Record");
                    }
                    else
                    {
                        NewGridForm.AddGrid("ReqHeadGrid", "Requisition Header:", ReqHeaderTable);
                        NewGridForm.AddGrid("ReqLineGrid", "Requisition Line Items:", ReqLinesTable);
                    }
                    NewGridForm.Show();
                }
                else if (ReferenceEquals(InventoryTable, null) && ReferenceEquals(ReqLinesTable, null))
                {
                    OtherFunctions.Message("Could not resolve any Req. or FA info.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Nothing Found");
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                OtherFunctions.SetWaitCursor(false, parentForm);
            }
        }

        private async Task<DataTable> LoadMunisInventoryGrid(DeviceObject device)
        {
            string strFields = "fama_asset,fama_status,fama_class,fama_subcl,fama_tag,fama_serial,fama_desc,fama_dept,fama_loc,FixedAssetLocations.LongDescription,fama_acq_dt,fama_fisc_yr,fama_pur_cost,fama_manuf,fama_model,fama_est_life,fama_repl_dt,fama_purch_memo";
            string Query = "SELECT TOP 1 " + strFields + " FROM famaster INNER JOIN FixedAssetLocations ON FixedAssetLocations.Code = famaster.fama_loc WHERE fama_tag='" + device.AssetTag + "' AND fama_tag <> '' OR fama_serial='" + device.Serial + "' AND fama_serial <> ''";
            return await MunisComms.ReturnSqlTableAsync(Query);
        }
    }
}