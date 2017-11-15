using System.ComponentModel;
using System.IO;
using System.Net;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using Microsoft.VisualBasic;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data.Common;
using System.Diagnostics;
using AssetManager.UserInterface.CustomControls;

namespace AssetManager.UserInterface.Forms.AssetManagement
{
    public partial class NewDeviceForm : ExtendedForm
    {

        #region Fields
        private MunisEmployeeStruct _munisUser; //= null;
        public MunisEmployeeStruct MunisUser
        {
            get
            {
                return _munisUser;
            }
            set
            {
                _munisUser = value;
                LockUnlockUserField();
            }
        }

        private int intReplacementSched = 4;
        private bool bolCheckFields;
        private DBControlParser DataParser; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        private DeviceObject Device = new DeviceObject();
        private LiveBox MyLiveBox; // VBConversions Note: Initial value cannot be assigned here since it is non-static.  Assignment has been moved to the class constructors.
        private string NewUID;

        #endregion

        #region Methods

        public NewDeviceForm(ExtendedForm parentForm)
        {
            // VBConversions Note: Non-static class variable initialization is below.  Class variables cannot be initially assigned non-static values in C#.
            DataParser = new DBControlParser(this);
            MyLiveBox = new LiveBox(this);

            InitializeComponent();
            this.ParentForm = parentForm;
            this.Owner = parentForm;
            ClearAll();
            InitDBControls();
            MyLiveBox.AttachToControl(txtCurUser_REQ, DevicesCols.CurrentUser, LiveBoxType.UserSelect, DevicesCols.MunisEmpNum);
            MyLiveBox.AttachToControl(txtDescription_REQ, DevicesCols.Description, LiveBoxType.SelectValue);
            this.Show();
            this.Activate();
        }

        public void ImportFromSibi(string itemUID)
        {
            string itemQuery = "SELECT * FROM " + SibiRequestItemsCols.TableName + " INNER JOIN " + SibiRequestCols.TableName + " ON " + SibiRequestItemsCols.RequestUID + " = " + SibiRequestCols.UID + " WHERE " + SibiRequestItemsCols.ItemUID + "='" + itemUID + "'";
            DateTime POPurchaseDate = default(DateTime);
            using (var results = DBFactory.GetDatabase().DataTableFromQueryString(itemQuery))
            {
                DataParser.FillDBFields(results, ImportColumnRemaps());
                MunisUser = GlobalInstances.AssetFunc.SmartEmployeeSearch(results.Rows[0][SibiRequestItemsCols.User].ToString().ToUpper());
                POPurchaseDate = System.Convert.ToDateTime(GlobalInstances.MunisFunc.GetPODate(results.Rows[0][SibiRequestCols.PO].ToString()));
            }


            txtCurUser_REQ.Text = MunisUser.Name;
            CheckFields(this, false);
            dtPurchaseDate_REQ.Value = POPurchaseDate;
            bolCheckFields = true;
        }

        private List<DBRemappingInfo> ImportColumnRemaps()
        {
            List<DBRemappingInfo> newMap = new List<DBRemappingInfo>();
            newMap.Add(new DBRemappingInfo(SibiRequestItemsCols.User, DevicesCols.CurrentUser));
            newMap.Add(new DBRemappingInfo(SibiRequestItemsCols.NewAsset, DevicesCols.AssetTag));
            newMap.Add(new DBRemappingInfo(SibiRequestItemsCols.NewSerial, DevicesCols.Serial));
            newMap.Add(new DBRemappingInfo(SibiRequestItemsCols.Description, DevicesCols.Description));
            newMap.Add(new DBRemappingInfo(SibiRequestItemsCols.Location, DevicesCols.Location));
            newMap.Add(new DBRemappingInfo(SibiRequestCols.PO, DevicesCols.PO));
            return newMap;
        }

        private void AddDevice()
        {
            try
            {
                if (!CheckFields(this, true))
                {
                    OtherFunctions.Message("Some required fields are missing or invalid.  Please fill and/or verify all highlighted fields.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Missing Data", this);
                    bolCheckFields = true;
                    return;
                }
                else
                {
                    if (GlobalInstances.AssetFunc.DeviceExists(Strings.Trim(System.Convert.ToString(txtAssetTag_REQ.Text)), Strings.Trim(System.Convert.ToString(txtSerial_REQ.Text))))
                    {
                        OtherFunctions.Message("A device with that serial and/or asset tag already exists.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Duplicate Device", this);
                        return;
                    }
                    else
                    {
                        //proceed
                    }
                    bool Success = AddNewDevice();
                    if (Success)
                    {
                        var blah = OtherFunctions.Message("New Device Added.   Add another?", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Information, "Complete", this);
                        if (!chkNoClear.Checked)
                        {
                            ClearAll();
                        }
                        if (blah == Constants.vbNo)
                        {
                            this.Dispose();
                        }
                        ParentForm.RefreshData();
                    }
                    else
                    {
                        OtherFunctions.Message("Something went wrong while adding a new device.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Unexpected Result", this);
                    }

                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                OtherFunctions.Message("Unable to add new device.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Error", this);
            }
        }

        private void AddErrorIcon(Control ctl)
        {
            ctl.BackColor = Colors.MissingField;
            if (ReferenceEquals(fieldErrorIcon.GetError(ctl), string.Empty))
            {
                fieldErrorIcon.SetIconAlignment(ctl, ErrorIconAlignment.MiddleRight);
                fieldErrorIcon.SetIconPadding(ctl, 4);
                fieldErrorIcon.SetError(ctl, "Required or invalid field");
            }
        }

        private void NewDeviceForm_Disposed(object sender, EventArgs e)
        {
            MyLiveBox.Dispose();
        }

        private bool AddNewDevice()
        {
            using (var trans = DBFactory.GetDatabase().StartTransaction())
            {
                using (var conn = trans.Connection)
                {
                    try
                    {
                        NewUID = System.Convert.ToString(Guid.NewGuid().ToString());
                        int rows = 0;
                        string DeviceInsertQry = "SELECT * FROM " + DevicesCols.TableName + " LIMIT 0";
                        string HistoryInsertQry = "SELECT * FROM " + HistoricalDevicesCols.TableName + " LIMIT 0";

                        rows += System.Convert.ToInt32(DBFactory.GetDatabase().UpdateTable(DeviceInsertQry, DeviceInsertTable(DeviceInsertQry), trans));
                        rows += System.Convert.ToInt32(DBFactory.GetDatabase().UpdateTable(HistoryInsertQry, HistoryInsertTable(HistoryInsertQry), trans));

                        if (rows == 2)
                        {
                            trans.Commit();
                            return true;
                        }
                        else
                        {
                            trans.Rollback();
                            return false;
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                        return false;
                    }
                }
            }

        }

        private bool CheckFields(Control Parent, bool bolValidFields)
        {
            foreach (Control ctl in Parent.Controls)
            {
                DBControlInfo DBInfo = new DBControlInfo();
                if (ctl.Tag != null)
                {
                    DBInfo = (DBControlInfo)ctl.Tag;
                }
                if (true)
                {
                    if (DBInfo.Required)
                    {
                        if (Strings.Trim(System.Convert.ToString(ctl.Text)) == "")
                        {
                            bolValidFields = false;
                            //  ctl.BackColor = Colors.MissingField
                            AddErrorIcon(ctl);
                        }
                        else
                        {
                            // ctl.BackColor = Color.Empty
                            ClearErrorIcon(ctl);

                            if (ReferenceEquals(ctl, txtCurUser_REQ))
                            {
                                LockUnlockUserField();
                            }

                        }
                    }
                    if (ReferenceEquals(ctl, txtPhoneNumber))
                    {
                        if (Strings.Trim(System.Convert.ToString(txtPhoneNumber.Text)) != "" && !DataConsistency.ValidPhoneNumber(txtPhoneNumber.Text))
                        {
                            bolValidFields = false;
                            AddErrorIcon(ctl);
                            OtherFunctions.Message("Invalid phone number.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Error", this);
                        }
                    }
                }
                else if (true)
                {
                    ComboBox cmb = (ComboBox)ctl;
                    if (DBInfo.Required)
                    {
                        if (cmb.SelectedIndex == -1)
                        {
                            bolValidFields = false;
                            // cmb.BackColor = Colors.MissingField
                            AddErrorIcon(cmb);
                        }
                        else
                        {
                            //  cmb.BackColor = Color.Empty
                            ClearErrorIcon(cmb);
                        }
                    }
                }
                if (ctl.HasChildren)
                {
                    bolValidFields = CheckFields(ctl, bolValidFields);
                }
            }
            return bolValidFields; //if fields are missing return false to trigger a message if needed
        }

        private void LockUnlockUserField()
        {
            if (MunisUser.Number != "")
            {
                txtCurUser_REQ.BackColor = Colors.EditColor;
                txtCurUser_REQ.ReadOnly = true;
                ToolTip1.SetToolTip(txtCurUser_REQ, "Munis Linked Employee - Double-Click to change.");
            }
            else
            {
                txtCurUser_REQ.BackColor = Color.Empty;
                txtCurUser_REQ.ReadOnly = false;
                ToolTip1.SetToolTip(txtCurUser_REQ, "");
            }
        }

        private void ClearAll()
        {
            RefreshCombos();
            ClearFields(this);
            dtPurchaseDate_REQ.Value = DateTime.Now;
            cmbStatus_REQ.SelectedIndex = AttribIndexFunctions.GetComboIndexFromCode(GlobalInstances.DeviceAttribute.StatusType, "INSRV");
            ResetBackColors(this);
            chkTrackable.Checked = false;
            chkNoClear.Checked = false;
            bolCheckFields = false;
            fieldErrorIcon.Clear();
        }

        private void ClearErrorIcon(Control ctl)
        {
            ctl.BackColor = Color.Empty;
            fieldErrorIcon.SetError(ctl, string.Empty);
        }

        private void ClearFields(Control Parent)
        {
            MunisUser = new MunisEmployeeStruct();  //null;
            foreach (Control ctl in Parent.Controls)
            {
                if (ctl is TextBox)
                {
                    TextBox txt = (TextBox)ctl;
                    txt.Text = "";
                    txt.ReadOnly = false;
                }
                if (ctl is ComboBox)
                {
                    ComboBox cmb = (ComboBox)ctl;
                    cmb.SelectedIndex = -1;
                }
                if (ctl.HasChildren)
                {
                    ClearFields(ctl);
                }
            }
        }

        private void cmbEquipType_REQ_DropDown(object sender, EventArgs e)
        {
            OtherFunctions.AdjustComboBoxWidth(sender, e);
        }

        private void cmbEquipType_REQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields(this, false);
            }
        }

        private void cmbLocation_REQ_DropDown(object sender, EventArgs e)
        {
            OtherFunctions.AdjustComboBoxWidth(sender, e);
        }

        private void cmbLocation_REQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields(this, false);
            }
        }

        private void cmbOSType_REQ_DropDown(object sender, EventArgs e)
        {
            OtherFunctions.AdjustComboBoxWidth(sender, e);
        }

        private void cmbOSType_REQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields(this, false);
            }
            SetHostname();
        }

        private void cmbStatus_REQ_DropDown(object sender, EventArgs e)
        {
            OtherFunctions.AdjustComboBoxWidth(sender, e);
        }

        private void cmbStatus_REQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields(this, false);
            }
        }

        private void cmdAdd_Click(object sender, EventArgs e)
        {
            AddDevice();
        }

        private void cmdClear_Click(object sender, EventArgs e)
        {
            ClearAll();
        }

        private void cmdUserSearch_Click(object sender, EventArgs e)
        {
            using (MunisUserForm NewMunisSearch = new MunisUserForm(this))
            {
                MunisUser = NewMunisSearch.EmployeeInfo;
                if (MunisUser.Number != "")
                {
                    txtCurUser_REQ.Text = MunisUser.Name;
                    txtCurUser_REQ.ReadOnly = true;
                }
            }

        }

        private DataTable DeviceInsertTable(string selectQuery)
        {
            var tmpTable = DataParser.ReturnInsertTable(selectQuery);
            var DBRow = tmpTable.Rows[0];
            //Add Add'l info
            if (MunisUser.Number != null)
            {
                DBRow[DevicesCols.CurrentUser] = MunisUser.Name;
                DBRow[DevicesCols.MunisEmpNum] = MunisUser.Number;
            }
            DBRow[DevicesCols.LastModUser] = GlobalConstants.LocalDomainUser;
            DBRow[DevicesCols.LastModDate] = DateTime.Now;
            DBRow[DevicesCols.DeviceUID] = NewUID;
            DBRow[DevicesCols.CheckedOut] = false;
            return tmpTable;
        }

        private void dtPurchaseDate_REQ_ValueChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields(this, false);
            }
            SetReplacementYear(System.Convert.ToDateTime(dtPurchaseDate_REQ.Value));
        }

        private DataTable HistoryInsertTable(string selectQuery)
        {
            var tmpTable = DataParser.ReturnInsertTable(selectQuery);
            var DBRow = tmpTable.Rows[0];
            //Add Add'l info
            DBRow[HistoricalDevicesCols.ChangeType] = "NEWD";
            DBRow[HistoricalDevicesCols.Notes] = Strings.Trim(System.Convert.ToString(txtNotes.Text));
            DBRow[HistoricalDevicesCols.ActionUser] = GlobalConstants.LocalDomainUser;
            DBRow[HistoricalDevicesCols.DeviceUID] = NewUID;
            return tmpTable;
        }

        private void InitDBControls()
        {
            txtDescription_REQ.Tag = new DBControlInfo(DevicesBaseCols.Description, true);
            txtAssetTag_REQ.Tag = new DBControlInfo(DevicesBaseCols.AssetTag, true);
            txtSerial_REQ.Tag = new DBControlInfo(DevicesBaseCols.Serial, true);
            dtPurchaseDate_REQ.Tag = new DBControlInfo(DevicesBaseCols.PurchaseDate, true);
            txtReplaceYear.Tag = new DBControlInfo(DevicesBaseCols.ReplacementYear, false);
            cmbLocation_REQ.Tag = new DBControlInfo(DevicesBaseCols.Location, GlobalInstances.DeviceAttribute.Locations, true);
            txtCurUser_REQ.Tag = new DBControlInfo(DevicesBaseCols.CurrentUser, true);
            // txtNotes.Tag = New DBControlInfo(historical_dev.Notes, False)
            cmbOSType_REQ.Tag = new DBControlInfo(DevicesBaseCols.OSVersion, GlobalInstances.DeviceAttribute.OSType, true);
            txtPhoneNumber.Tag = new DBControlInfo(DevicesBaseCols.PhoneNumber, false);
            cmbEquipType_REQ.Tag = new DBControlInfo(DevicesBaseCols.EQType, GlobalInstances.DeviceAttribute.EquipType, true);
            cmbStatus_REQ.Tag = new DBControlInfo(DevicesBaseCols.Status, GlobalInstances.DeviceAttribute.StatusType, true);
            chkTrackable.Tag = new DBControlInfo(DevicesBaseCols.Trackable, false);
            txtPO.Tag = new DBControlInfo(DevicesBaseCols.PO, false);
            txtHostname.Tag = new DBControlInfo(DevicesBaseCols.HostName, false);
            iCloudTextBox.Tag = new DBControlInfo(DevicesBaseCols.iCloudAccount, false);
        }

        private void RefreshCombos()
        {
            AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.Locations, cmbLocation_REQ);
            AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.EquipType, cmbEquipType_REQ);
            AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.OSType, cmbOSType_REQ);
            AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.StatusType, cmbStatus_REQ);
        }

        private void ResetBackColors(Control Parent)
        {
            foreach (Control ctl in Parent.Controls)
            {
                if (true)
                {
                    ctl.BackColor = Color.Empty;
                }
                else if (true)
                {
                    ctl.BackColor = Color.Empty;
                }
                if (ctl.HasChildren)
                {
                    ResetBackColors(ctl);
                }
            }
        }

        private void SetReplacementYear(DateTime PurDate)
        {
            int ReplaceYear = PurDate.Year + intReplacementSched;
            txtReplaceYear.Text = ReplaceYear.ToString();
        }

        private void txtAssetTag_REQ_TextChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields(this, false);
            }
        }

        private void txtCurUser_REQ_TextChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields(this, false);
            }
        }

        private void txtCurUser_REQ_DoubleClick(object sender, EventArgs e)
        {
            txtCurUser_REQ.ReadOnly = false;
            MunisUser = new MunisEmployeeStruct();  //null;
            txtCurUser_REQ.SelectAll();
        }

        private void txtDescription_REQ_TextChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields(this, false);
            }
        }

        private void txtPhoneNumber_Leave(object sender, EventArgs e)
        {
            if (Strings.Trim(System.Convert.ToString(txtPhoneNumber.Text)) != "" && !DataConsistency.ValidPhoneNumber(txtPhoneNumber.Text))
            {
                OtherFunctions.Message("Invalid phone number.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Error", this);
                txtPhoneNumber.Focus();
            }
        }

        private void txtSerial_REQ_TextChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields(this, false);
            }
            SetHostname();
        }

        private void SetHostname()
        {
            if (txtSerial_REQ.Text != "" && AttribIndexFunctions.GetDBValue(GlobalInstances.DeviceAttribute.OSType, cmbOSType_REQ.SelectedIndex).Contains("WIN"))
            {
                txtHostname.Text = DataConsistency.DeviceHostnameFormat(txtSerial_REQ.Text);
            }
            else
            {
                txtHostname.Text = string.Empty;
            }
        }


        #endregion

    }
}