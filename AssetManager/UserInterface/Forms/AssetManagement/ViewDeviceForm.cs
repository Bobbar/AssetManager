using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.NetworkInformation;
using PingVisLib;
using AssetManager.UserInterface.CustomControls;
using AssetManager.UserInterface.Forms.Attachments;
using AssetManager.UserInterface.Forms.Sibi;

namespace AssetManager.UserInterface.Forms.AssetManagement
{

    public partial class ViewDeviceForm : ExtendedForm
    {

        #region Fields

        public MunisEmployeeStruct MunisUser = new MunisEmployeeStruct();
        private bool bolCheckFields;
        private bool bolGridFilling = false;
        private string CurrentHash;
        private DeviceObject CurrentViewDevice = new DeviceObject();
        private DBControlParser DataParser;
        private string DeviceHostname = null;
        private bool EditMode = false;
        private int intFailedPings = 0;
        private LiveBox MyLiveBox;
        private MunisToolBar MyMunisToolBar;
        private PingVis MyPingVis;
        private WindowList MyWindowList;
        private SliderLabel StatusSlider;

        #endregion

        #region Delegates

        delegate void StatusVoidDelegate(string text);

        #endregion

        #region Constructors

        public ViewDeviceForm(ExtendedForm parentForm, string deviceGUID)
        {

            DataParser = new DBControlParser(this);
            MyLiveBox = new LiveBox(this);
            MyMunisToolBar = new MunisToolBar(this);
            MyWindowList = new WindowList(this);

            this.ParentForm = parentForm;
            FormUID = deviceGUID;
            InitializeComponent();

            StatusSlider = new SliderLabel();
            StatusStrip1.Items.Add(StatusSlider.ToToolStripControl(StatusStrip1));

            MyMunisToolBar.InsertMunisDropDown(ToolStrip1, 6);
            ImageCaching.CacheControlImages(this);
            MyWindowList.InsertWindowList(ToolStrip1);
            InitDBControls();
            MyLiveBox.AttachToControl(txtCurUser_View_REQ, DevicesCols.CurrentUser, LiveBoxType.UserSelect, DevicesCols.MunisEmpNum);
            MyLiveBox.AttachToControl(txtDescription_View_REQ, DevicesCols.Description, LiveBoxType.SelectValue);
            RefreshCombos();
            RemoteToolsBox.Visible = false;
            ExtendedMethods.DoubleBufferedDataGrid(DataGridHistory, true);
            ExtendedMethods.DoubleBufferedDataGrid(TrackingGrid, true);
            LoadDevice(deviceGUID);
        }

        #endregion

        #region Methods

        public void LoadDevice(string deviceGUID)
        {
            try
            {
                Waiting();
                bolGridFilling = true;
                if (LoadHistoryAndFields(deviceGUID))
                {
                    if (CurrentViewDevice.IsTrackable)
                    {
                        LoadTracking(CurrentViewDevice.GUID);
                    }
                    SetTracking(CurrentViewDevice.IsTrackable, CurrentViewDevice.Tracking.IsCheckedOut);
                    this.Text = this.Text + FormTitle(CurrentViewDevice);
                    DeviceHostname = CurrentViewDevice.HostName + "." + NetworkInfo.CurrentDomain;
                    CheckRDP();
                    tmr_RDPRefresher.Enabled = true;
                    this.Show();
                    DataGridHistory.ClearSelection();
                    bolGridFilling = false;
                }
                else
                {
                    this.Dispose();
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                DoneWaiting();
            }
        }

        public override bool OKToClose()
        {
            bool CanClose = true;
            if (!Helpers.ChildFormControl.OKToCloseChildren(this))
            {
                CanClose = false;
            }
            if (EditMode && !CancelModify())
            {
                CanClose = false;
            }
            return CanClose;
        }

        public override void RefreshData()
        {
            if (EditMode)
            {
                CancelModify();
            }
            else
            {
                ADPanel.Visible = false;
                RemoteToolsBox.Visible = false;
                if (MyPingVis != null)
                {
                    MyPingVis.Dispose();
                    MyPingVis = null;
                }
                LoadDevice(CurrentViewDevice.GUID);
            }
        }

        public void SetAttachCount()
        {
            if (!GlobalSwitches.CachedMode)
            {
                AttachmentTool.Text = "(" + GlobalInstances.AssetFunc.GetAttachmentCount(CurrentViewDevice.GUID, new DeviceAttachmentsCols()).ToString() + ")";
                AttachmentTool.ToolTipText = "Attachments " + AttachmentTool.Text;
            }

        }

        private void AcceptChanges()
        {
            try
            {
                if (!CheckFields())
                {
                    OtherFunctions.Message("Some required fields are missing or invalid.  Please check and fill all highlighted fields.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Missing Data", this);
                    bolCheckFields = true;
                    return;
                }
                using (UpdateDev UpdateDia = new UpdateDev(this))
                {
                    if (UpdateDia.DialogResult == DialogResult.OK)
                    {
                        if (!ConcurrencyCheck())
                        {
                            CancelModify();
                            return;
                        }
                        else
                        {
                            UpdateDevice(UpdateDia.UpdateInfo);
                        }
                    }
                    else
                    {
                        CancelModify();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void AddErrorIcon(Control ctl)
        {
            if (ReferenceEquals(fieldErrorIcon.GetError(ctl), string.Empty))
            {
                fieldErrorIcon.SetIconAlignment(ctl, ErrorIconAlignment.MiddleRight);
                fieldErrorIcon.SetIconPadding(ctl, 4);
                fieldErrorIcon.SetError(ctl, "Required or Invalid Field");
            }
        }

        private void AddNewNote()
        {
            try
            {
                if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifyDevice))
                {
                    return;
                }
                using (UpdateDev UpdateDia = new UpdateDev(this, true))
                {
                    if (UpdateDia.DialogResult == DialogResult.OK)
                    {
                        if (!ConcurrencyCheck())
                        {
                            RefreshData();
                        }
                        else
                        {
                            UpdateDevice(UpdateDia.UpdateInfo);
                        }
                    }
                    else
                    {
                        RefreshData();
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private async void BrowseFiles()
        {
            try
            {
                if (SecurityTools.VerifyAdminCreds())
                {
                    string FullPath = "\\\\" + CurrentViewDevice.HostName + "\\c$";
                    await Task.Run(() =>
                    {
                        using (NetworkConnection NetCon = new NetworkConnection(FullPath, SecurityTools.AdminCreds))
                        using (Process p = new Process())
                        {
                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.RedirectStandardOutput = true;
                            p.StartInfo.RedirectStandardError = true;
                            p.StartInfo.FileName = "explorer.exe";
                            p.StartInfo.Arguments = FullPath;
                            p.Start();
                            p.WaitForExit();
                        }
                    });
                }

            }
            catch (Exception ex)
            {

                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }


        private bool CancelModify()
        {
            if (EditMode)
            {
                this.WindowState = FormWindowState.Normal;
                this.Activate();
                var blah = OtherFunctions.Message("Are you sure you want to discard all changes?", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "Discard Changes?", this);
                if (blah == DialogResult.Yes)
                {
                    bolCheckFields = false;
                    fieldErrorIcon.Clear();
                    DisableControls();
                    ResetBackColors();
                    this.Refresh();
                    RefreshData();
                    return true;
                }
            }
            return false;
        }

        private bool CheckFields()
        {
            bool bolMissingField = false;
            bolMissingField = false;
            fieldErrorIcon.Clear();
            foreach (Control c in DataParser.GetDBControls(this))
            {
                DBControlInfo DBInfo = (DBControlInfo)c.Tag;
                if (c is TextBox)
                {
                    if (DBInfo.Required)
                    {
                        if (c.Text.Trim() == "")
                        {
                            bolMissingField = true;
                            c.BackColor = Colors.MissingField;
                            AddErrorIcon(c);
                        }
                        else
                        {
                            c.BackColor = Color.Empty;
                            ClearErrorIcon(c);
                        }
                    }
                }
                else if (c is ComboBox)
                {
                    ComboBox cmb = (ComboBox)c;
                    if (DBInfo.Required)
                    {
                        if (cmb.SelectedIndex == -1)
                        {
                            bolMissingField = true;
                            cmb.BackColor = Colors.MissingField;
                            AddErrorIcon(cmb);
                        }
                        else
                        {
                            cmb.BackColor = Color.Empty;
                            ClearErrorIcon(cmb);
                        }
                    }
                }
            }
            if (!DataConsistency.ValidPhoneNumber(txtPhoneNumber.Text))
            {
                bolMissingField = true;
                txtPhoneNumber.BackColor = Colors.MissingField;
                AddErrorIcon(txtPhoneNumber);
            }
            else
            {
                txtPhoneNumber.BackColor = Color.Empty;
                ClearErrorIcon(txtPhoneNumber);
            }
            return !bolMissingField; //if fields are missing return false to trigger a message if needed
        }

        private void CheckRDP()
        {
            try
            {
                if (CurrentViewDevice.OSVersion.Contains("WIN"))
                {
                    if (ReferenceEquals(MyPingVis, null))
                    {
                        MyPingVis = new PingVis((Control)cmdShowIP, DeviceHostname);
                    }
                    if (MyPingVis.CurrentResult != null)
                    {
                        if (MyPingVis.CurrentResult.Status == IPStatus.Success)
                        {
                            SetupNetTools(MyPingVis.CurrentResult);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void ClearErrorIcon(Control ctl)
        {
            fieldErrorIcon.SetError(ctl, string.Empty);
        }

        private void CollectCurrentTracking(DataTable results)
        {
            CurrentViewDevice.MapClassProperties(CurrentViewDevice.Tracking, results);
        }

        private bool ConcurrencyCheck()
        {
            using (var DeviceResults = GetDevicesTable(CurrentViewDevice.GUID))
            {
                using (var HistoricalResults = GetHistoricalTable(CurrentViewDevice.GUID))
                {
                    DeviceResults.TableName = DevicesCols.TableName;
                    HistoricalResults.TableName = HistoricalDevicesCols.TableName;
                    var DBHash = GetHash(DeviceResults, HistoricalResults);
                    if (DBHash != CurrentHash)
                    {
                        OtherFunctions.Message("This record appears to have been modified by someone else since the start of this modification.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Concurrency Error", this);
                        return false;
                    }
                    return true;
                }
            }

        }

        private void DeleteDevice()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.DeleteDevice))
            {
                return;
            }
            var blah = OtherFunctions.Message("Are you absolutely sure?  This cannot be undone and will delete all historical data, tracking and attachments.", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Exclamation, "WARNING", this);
            if (blah == DialogResult.Yes)
            {
                if (GlobalInstances.AssetFunc.DeleteFtpAndSql(CurrentViewDevice.GUID, EntryType.Device))
                {
                    OtherFunctions.Message("Device deleted successfully.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Device Deleted", this);
                    CurrentViewDevice = null;
                    Helpers.ChildFormControl.MainFormInstance().RefreshData();
                }
                else
                {
                    Logging.Logger("*****DELETION ERROR******: " + CurrentViewDevice.GUID);
                    OtherFunctions.Message("Failed to delete device succesfully!  Please let Bobby Lovell know about this.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Stop, "Delete Failed", this);
                    CurrentViewDevice = null;
                }
                this.Dispose();
            }
            else
            {
                return;
            }
        }

        private int DeleteHistoryEntry(string strGUID)
        {
            try
            {
                int rows = 0;
                string DeleteEntryQuery = "DELETE FROM " + HistoricalDevicesCols.TableName + " WHERE " + HistoricalDevicesCols.HistoryEntryUID + "='" + strGUID + "'";
                rows = DBFactory.GetDatabase().ExecuteQuery(DeleteEntryQuery);
                return rows;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return 0;
            }
        }

        private void DeleteSelectedHistoricalEntry()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifyDevice))
            {
                return;
            }
            string strGUID = GridFunctions.GetCurrentCellValue(DataGridHistory, HistoricalDevicesCols.HistoryEntryUID);
            DeviceObject Info = default(DeviceObject);
            var strQry = "SELECT * FROM " + HistoricalDevicesCols.TableName + " WHERE " + HistoricalDevicesCols.HistoryEntryUID + "='" + strGUID + "'";
            using (DataTable results = DBFactory.GetDatabase().DataTableFromQueryString(strQry))
            {
                Info = new DeviceObject(results);
            }

            var blah = OtherFunctions.Message("Are you absolutely sure?  This cannot be undone!" + "\r\n" + "\r\n" + "Entry info: " + Info.Historical.ActionDateTime + " - " + AttribIndexFunctions.GetDisplayValueFromCode(GlobalInstances.DeviceAttribute.ChangeType, Info.Historical.ChangeType) + " - " + strGUID, (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Exclamation, "WARNING", this);
            if (blah == DialogResult.Yes)
            {
                OtherFunctions.Message(DeleteHistoryEntry(strGUID) + " rows affected.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Deletion Results", this);
                LoadDevice(CurrentViewDevice.GUID);
            }
            else
            {
                return;
            }
        }

        private async void DeployTeamViewer(DeviceObject device)
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.IsAdmin))
            {
                return;
            }
            if (OtherFunctions.Message("Deploy TeamViewer to this device?", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "Are you sure?", this) != DialogResult.Yes)
            {
                return;
            }
            try
            {
                if (SecurityTools.VerifyAdminCreds("For remote runspace access."))
                {
                    TeamViewerDeploy NewTVDeploy = new TeamViewerDeploy();
                    StatusSlider.NewSlideMessage("Deploying TeamViewer...", 0);
                    if (await NewTVDeploy.DeployToDevice(this, device))
                    {
                        StatusSlider.NewSlideMessage("TeamViewer deployment complete!");
                    }
                    else
                    {
                        StatusSlider.NewSlideMessage("TeamViewer deployment failed...");
                    }
                }
            }
            catch (Exception ex)
            {
                StatusSlider.NewSlideMessage("TeamViewer deployment failed...");
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                DoneWaiting();
            }
        }

        private async void UpdateChrome(DeviceObject device)
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.IsAdmin))
            {
                return;
            }
            if (OtherFunctions.Message("Update/Install Chrome on this device?", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "Are you sure?", this) != DialogResult.Yes)
            {
                return;
            }
            try
            {
                if (SecurityTools.VerifyAdminCreds("For remote runspace access."))
                {
                    Waiting();
                    StatusSlider.NewSlideMessage("Installing Chrome...", 0);
                    PowerShellWrapper PSWrapper = new PowerShellWrapper();
                    if (await PSWrapper.ExecutePowerShellScript(device.HostName, Properties.Resources.UpdateChrome))
                    {
                        StatusSlider.NewSlideMessage("Chrome install complete!");
                        OtherFunctions.Message("Command successful.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Done", this);
                    }
                    else
                    {
                        StatusSlider.NewSlideMessage("Error while installing Chrome!");
                    }
                }
            }
            catch (Exception ex)
            {
                StatusSlider.NewSlideMessage("Error while installing Chrome!");
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                DoneWaiting();
            }
        }

        private void DisableControls()
        {
            EditMode = false;
            DisableControlsRecursive(this);
            pnlOtherFunctions.Visible = true;
            cmdMunisSearch.Visible = false;
            this.Text = "View";
            tsSaveModify.Visible = false;
            tsTracking.Visible = false;
        }

        private void DisableControlsRecursive(Control control)
        {
            foreach (Control c in control.Controls)
            {
                if (c is TextBox)
                {
                    TextBox txt = (TextBox)c;
                    txt.ReadOnly = true;
                }
                else if (c is MaskedTextBox)
                {
                    MaskedTextBox txt = (MaskedTextBox)c;
                    txt.ReadOnly = true;
                }
                else if (c is ComboBox)
                {
                    ComboBox cmb = (ComboBox)c;
                    cmb.Enabled = false;
                }
                else if (c is DateTimePicker)
                {
                    DateTimePicker dtp = (DateTimePicker)c;
                    dtp.Enabled = false;
                }
                else if (c is CheckBox)
                {
                    c.Enabled = false;
                }
                else if (c is Label)
                {
                    //do nut-zing
                }
                if (c.HasChildren)
                {
                    DisableControlsRecursive(c);
                }
            }
        }

        private void DisableSorting(DataGridView Grid)
        {
            foreach (DataGridViewColumn c in Grid.Columns)
            {
                c.SortMode = DataGridViewColumnSortMode.NotSortable;
            }
        }

        private void DoneWaiting()
        {
            OtherFunctions.SetWaitCursor(false, this);
        }

        private void EnableControls()
        {
            EditMode = true;
            EnableControlsRecursive(this);
            ADPanel.Visible = false;
            pnlOtherFunctions.Visible = false;
            cmdMunisSearch.Visible = true;
            this.Text = "View" + FormTitle(CurrentViewDevice) + "  *MODIFYING**";
            tsSaveModify.Visible = true;
        }

        private void EnableControlsRecursive(Control control)
        {
            foreach (Control c in control.Controls)
            {

                if (c is TextBox)
                {
                    TextBox txt = (TextBox)c;
                    txt.ReadOnly = false;
                }
                else if (c is MaskedTextBox)
                {
                    MaskedTextBox txt = (MaskedTextBox)c;
                    txt.ReadOnly = false;
                }
                else if (c is ComboBox)
                {
                    ComboBox cmb = (ComboBox)c;
                    cmb.Enabled = true;
                }
                else if (c is DateTimePicker)
                {
                    DateTimePicker dtp = (DateTimePicker)c;
                    dtp.Enabled = true;
                }
                else if (c is CheckBox)
                {
                    c.Enabled = true;
                }
                else if (c is Label)
                {
                    //do nut-zing
                }

                if (c.HasChildren)
                {
                    EnableControlsRecursive(c);
                }
            }

        }

        private void ExpandSplitter()
        {
            if (RemoteToolsBox.Visible || TrackingBox.Visible)
            {
                InfoDataSplitter.Panel2Collapsed = false;
            }
            else if (!RemoteToolsBox.Visible && !TrackingBox.Visible)
            {
                InfoDataSplitter.Panel2Collapsed = true;
            }
        }

        private void ExpandSplitter(bool shouldExpand)
        {
            InfoDataSplitter.Panel2Collapsed = !shouldExpand;
        }

        private void FillTrackingBox()
        {
            if (CurrentViewDevice.Tracking.IsCheckedOut)
            {
                txtCheckOut.BackColor = Colors.CheckOut;
                txtCheckLocation.Text = CurrentViewDevice.Tracking.UseLocation;
                lblCheckTime.Text = "CheckOut Time:";
                txtCheckTime.Text = CurrentViewDevice.Tracking.CheckoutTime.ToString();
                lblCheckUser.Text = "CheckOut User:";
                txtCheckUser.Text = CurrentViewDevice.Tracking.CheckoutUser;
                lblDueBack.Visible = true;
                txtDueBack.Visible = true;
                txtDueBack.Text = CurrentViewDevice.Tracking.DueBackTime.ToString();
            }
            else
            {
                txtCheckOut.BackColor = Colors.CheckIn;
                txtCheckLocation.Text = AttribIndexFunctions.GetDisplayValueFromCode(GlobalInstances.DeviceAttribute.Locations, CurrentViewDevice.Location);
                lblCheckTime.Text = "CheckIn Time:";
                txtCheckTime.Text = CurrentViewDevice.Tracking.CheckinTime.ToString();
                lblCheckUser.Text = "CheckIn User:";
                txtCheckUser.Text = CurrentViewDevice.Tracking.CheckinUser;
                lblDueBack.Visible = false;
                txtDueBack.Visible = false;
            }
            txtCheckOut.Text = (CurrentViewDevice.Tracking.IsCheckedOut ? "Checked Out" : "Checked In").ToString();
        }

        private string FormTitle(DeviceObject Device)
        {
            return " - " + Device.CurrentUser + " - " + Device.AssetTag + " - " + Device.Description;
        }

        private DataTable GetDevicesTable(string deviceUID)
        {
            return DBFactory.GetDatabase().DataTableFromQueryString("Select * FROM " + DevicesCols.TableName + " WHERE " + DevicesCols.DeviceUID + " = '" + deviceUID + "'");
        }

        private string GetHash(DataTable deviceTable, DataTable historicalTable)
        {
            return SecurityTools.GetSHAOfTable(deviceTable) + SecurityTools.GetSHAOfTable(historicalTable);
        }

        private DataTable GetHistoricalTable(string deviceUID)
        {
            return DBFactory.GetDatabase().DataTableFromQueryString("Select * FROM " + HistoricalDevicesCols.TableName + " WHERE " + HistoricalDevicesCols.DeviceUID + " = '" + deviceUID + "' ORDER BY " + HistoricalDevicesCols.ActionDateTime + " DESC");
        }

        private DataTable GetInsertTable(string selectQuery, DeviceUpdateInfoStruct UpdateInfo)
        {
            var tmpTable = DataParser.ReturnInsertTable(selectQuery);
            var DBRow = tmpTable.Rows[0];
            //Add Add'l info
            DBRow[HistoricalDevicesCols.ChangeType] = UpdateInfo.ChangeType;
            DBRow[HistoricalDevicesCols.Notes] = UpdateInfo.Note;
            DBRow[HistoricalDevicesCols.ActionUser] = GlobalConstants.LocalDomainUser;
            DBRow[HistoricalDevicesCols.DeviceUID] = CurrentViewDevice.GUID;
            return tmpTable;
        }

        private DataTable GetUpdateTable(string selectQuery)
        {
            var tmpTable = DataParser.ReturnUpdateTable(selectQuery);
            var DBRow = tmpTable.Rows[0];
            //Add Add'l info
            if (MunisUser.Number != null && MunisUser.Number != string.Empty)
            {
                DBRow[DevicesCols.CurrentUser] = MunisUser.Name;
                DBRow[DevicesCols.MunisEmpNum] = MunisUser.Number;
            }
            else
            {
                if (CurrentViewDevice.CurrentUser != txtCurUser_View_REQ.Text.Trim())
                {
                    DBRow[DevicesCols.CurrentUser] = txtCurUser_View_REQ.Text.Trim();
                    DBRow[DevicesCols.MunisEmpNum] = DBNull.Value;
                }
                else
                {
                    DBRow[DevicesCols.CurrentUser] = CurrentViewDevice.CurrentUser;
                    DBRow[DevicesCols.MunisEmpNum] = CurrentViewDevice.CurrentUserEmpNum;
                }
            }
            DBRow[DevicesCols.SibiLinkUID] = DataConsistency.CleanDBValue(CurrentViewDevice.SibiLink);
            DBRow[DevicesCols.LastModUser] = GlobalConstants.LocalDomainUser;
            DBRow[DevicesCols.LastModDate] = DateTime.Now;
            MunisUser = new MunisEmployeeStruct();//null;
            return tmpTable;
        }

        private List<DataGridColumn> HistoricalGridColumns()
        {
            List<DataGridColumn> ColList = new List<DataGridColumn>();
            ColList.Add(new DataGridColumn(HistoricalDevicesCols.ActionDateTime, "Time Stamp", typeof(DateTime)));
            ColList.Add(new DataGridColumn(HistoricalDevicesCols.ChangeType, "Change Type", GlobalInstances.DeviceAttribute.ChangeType, ColumnFormatTypes.AttributeDisplayMemberOnly));
            ColList.Add(new DataGridColumn(HistoricalDevicesCols.ActionUser, "Action User", typeof(string)));
            ColList.Add(new DataGridColumn(HistoricalDevicesCols.Notes, "Note Peek", typeof(string), ColumnFormatTypes.NotePreview));
            ColList.Add(new DataGridColumn(HistoricalDevicesCols.CurrentUser, "User", typeof(string)));
            ColList.Add(new DataGridColumn(HistoricalDevicesCols.AssetTag, "Asset ID", typeof(string)));
            ColList.Add(new DataGridColumn(HistoricalDevicesCols.Serial, "Serial", typeof(string)));
            ColList.Add(new DataGridColumn(HistoricalDevicesCols.Description, "Description", typeof(string)));
            ColList.Add(new DataGridColumn(HistoricalDevicesCols.Location, "Location", GlobalInstances.DeviceAttribute.Locations, ColumnFormatTypes.AttributeDisplayMemberOnly));
            ColList.Add(new DataGridColumn(HistoricalDevicesCols.PurchaseDate, "Purchase Date", typeof(DateTime)));
            ColList.Add(new DataGridColumn(HistoricalDevicesCols.HistoryEntryUID, "GUID", typeof(string)));
            return ColList;
        }

        private void InitDBControls()
        {
            //Required Fields
            txtAssetTag_View_REQ.Tag = new DBControlInfo(DevicesBaseCols.AssetTag, true);
            txtSerial_View_REQ.Tag = new DBControlInfo(DevicesBaseCols.Serial, true);
            txtCurUser_View_REQ.Tag = new DBControlInfo(DevicesBaseCols.CurrentUser, true);
            txtDescription_View_REQ.Tag = new DBControlInfo(DevicesBaseCols.Description, true);
            dtPurchaseDate_View_REQ.Tag = new DBControlInfo(DevicesBaseCols.PurchaseDate, true);
            cmbEquipType_View_REQ.Tag = new DBControlInfo(DevicesBaseCols.EQType, GlobalInstances.DeviceAttribute.EquipType, true);
            cmbLocation_View_REQ.Tag = new DBControlInfo(DevicesBaseCols.Location, GlobalInstances.DeviceAttribute.Locations, true);
            cmbOSVersion_REQ.Tag = new DBControlInfo(DevicesBaseCols.OSVersion, GlobalInstances.DeviceAttribute.OSType, true);
            cmbStatus_REQ.Tag = new DBControlInfo(DevicesBaseCols.Status, GlobalInstances.DeviceAttribute.StatusType, true);

            //Non-required and Misc Fields
            txtPONumber.Tag = new DBControlInfo(DevicesBaseCols.PO, false);
            txtReplacementYear_View.Tag = new DBControlInfo(DevicesBaseCols.ReplacementYear, false);
            txtPhoneNumber.Tag = new DBControlInfo(DevicesBaseCols.PhoneNumber, false);
            lblGUID.Tag = new DBControlInfo(DevicesBaseCols.DeviceUID, ParseType.DisplayOnly, false);
            chkTrackable.Tag = new DBControlInfo(DevicesBaseCols.Trackable, false);
            txtHostname.Tag = new DBControlInfo(DevicesBaseCols.HostName, false);
            iCloudTextBox.Tag = new DBControlInfo(DevicesBaseCols.iCloudAccount, false);
        }

        private void LaunchRDP()
        {
            ProcessStartInfo StartInfo = new ProcessStartInfo();
            StartInfo.FileName = "mstsc.exe";
            StartInfo.Arguments = "/v:" + CurrentViewDevice.HostName;
            Process.Start(StartInfo);
        }

        private void LinkSibi()
        {
            using (SibiSelectorForm f = new SibiSelectorForm(this))
            {
                if (f.DialogResult == DialogResult.OK)
                {
                    CurrentViewDevice.SibiLink = f.SibiUID;
                    OtherFunctions.Message("Sibi Link Set.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Success", this);
                }
            }

        }

        private bool LoadHistoryAndFields(string deviceUID)
        {
            try
            {
                using (var DeviceResults = GetDevicesTable(deviceUID))
                {
                    using (var HistoricalResults = GetHistoricalTable(deviceUID))
                    {
                        DeviceResults.TableName = DevicesCols.TableName;
                        HistoricalResults.TableName = HistoricalDevicesCols.TableName;
                        if (DeviceResults.Rows.Count < 1)
                        {
                            Helpers.ChildFormControl.CloseChildren(this);
                            CurrentViewDevice = null;
                            OtherFunctions.Message("That device was not found!  It may have been deleted.  Re-execute your search.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Not Found", this);
                            return false;
                        }
                        CurrentHash = GetHash(DeviceResults, HistoricalResults);
                        CurrentViewDevice = new DeviceObject(DeviceResults);
                        DataParser.FillDBFields(DeviceResults);
                        SetMunisEmpStatus();
                        SendToHistGrid(DataGridHistory, HistoricalResults);
                        DisableControls();
                        SetAttachCount();
                        SetADInfo();
                        return true;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return false;
            }
        }

        private void LoadTracking(string strGUID)
        {
            var strQry = "Select * FROM " + TrackablesCols.TableName + ", " + DevicesCols.TableName + " WHERE " + TrackablesCols.DeviceUID + " = " + DevicesCols.DeviceUID + " And " + TrackablesCols.DeviceUID + " = '" + strGUID + "' ORDER BY " + TrackablesCols.DateStamp + " DESC";
            using (DataTable Results = DBFactory.GetDatabase().DataTableFromQueryString(strQry))
            {
                if (Results.Rows.Count > 0)
                {
                    CollectCurrentTracking(Results);
                    SendToTrackGrid(TrackingGrid, Results);
                    DisableSorting(TrackingGrid);
                }
                else
                {
                    TrackingGrid.DataSource = null;
                }
                FillTrackingBox();
            }

        }

        private void ModifyDevice()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifyDevice))
            {
                return;
            }
            EnableControls();
        }

        private void NewEntryView(string entryGUID)
        {
            Waiting();
            ViewHistoryForm NewEntry = new ViewHistoryForm(this, entryGUID, CurrentViewDevice.GUID);
            DoneWaiting();
        }

        private void NewTrackingView(string GUID)
        {
            Waiting();
            ViewTrackingForm NewTracking = new ViewTrackingForm(this, GUID, CurrentViewDevice);
            DoneWaiting();
        }

        private void OpenSibiLink(DeviceObject LinkDevice)
        {
            try
            {
                if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ViewSibi))
                {
                    return;
                }
                string SibiUID = "";
                if (LinkDevice.SibiLink == "")
                {
                    if (LinkDevice.PO == "")
                    {
                        OtherFunctions.Message("A valid PO Number or Sibi Link is required.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Missing Info", this);
                        return;
                    }
                    else
                    {
                        SibiUID = GlobalInstances.AssetFunc.GetSqlValue(SibiRequestCols.TableName, SibiRequestCols.PO, LinkDevice.PO, SibiRequestCols.UID);
                    }
                }
                else
                {
                    SibiUID = LinkDevice.SibiLink;
                }
                if (string.IsNullOrEmpty(SibiUID))
                {
                    OtherFunctions.Message("No Sibi request found with matching PO number.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Not Found", this);
                }
                else
                {
                    if (!Helpers.ChildFormControl.FormIsOpenByUID(typeof(SibiManageRequestForm), SibiUID))
                    {
                        SibiManageRequestForm NewRequest = new SibiManageRequestForm(this, SibiUID);
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void RefreshCombos()
        {
            AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.EquipType, cmbEquipType_View_REQ);
            AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.Locations, cmbLocation_View_REQ);
            AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.OSType, cmbOSVersion_REQ);
            AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.StatusType, cmbStatus_REQ);
        }

        private void ResetBackColors()
        {
            foreach (Control c in DataParser.GetDBControls(this))
            {
                if (c is TextBox)
                {
                    c.BackColor = Color.Empty;
                }
                else if (c is ComboBox)
                {
                    c.BackColor = Color.Empty;
                }
            }
        }

        private async Task<string> SendRestart(string IP, string DeviceName)
        {
            var OrigButtonImage = cmdRestart.Image;
            try
            {
                if (SecurityTools.VerifyAdminCreds())
                {
                    cmdRestart.Image = Properties.Resources.LoadingAni;
                    string FullPath = "\\\\" + IP;
                    string output = await Task.Run(() =>
                    {
                        using (NetworkConnection NetCon = new NetworkConnection(FullPath, SecurityTools.AdminCreds))
                        using (Process p = new Process())
                        {
                            p.StartInfo.UseShellExecute = false;
                            p.StartInfo.RedirectStandardOutput = true;
                            p.StartInfo.RedirectStandardError = true;
                            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                            p.StartInfo.FileName = "shutdown.exe";
                            p.StartInfo.Arguments = "/m " + FullPath + " /f /r /t 0";
                            p.Start();
                            output = p.StandardError.ReadToEnd();
                            p.WaitForExit();
                            output = output.Trim();
                            return output;
                        }
                    });
                    return output;
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                cmdRestart.Image = OrigButtonImage;
            }
            return string.Empty;
        }

        private void SendToHistGrid(DataGridView Grid, DataTable results)
        {
            try
            {
                using (results)
                {
                    if (results.Rows.Count > 0)
                    {
                        GridFunctions.PopulateGrid(Grid, results, HistoricalGridColumns());
                    }
                    else
                    {
                        Grid.DataSource = null;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void SendToTrackGrid(DataGridView Grid, DataTable results)
        {
            try
            {
                using (results)
                {
                    if (results.Rows.Count > 0)
                    {
                        GridFunctions.PopulateGrid(Grid, results, TrackingGridColumns());
                    }
                    else
                    {
                        Grid.DataSource = null;
                    }
                }

            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private async void SetADInfo()
        {
            try
            {
                if (CurrentViewDevice.HostName != "")
                {
                    ActiveDirectoryWrapper ADWrap = new ActiveDirectoryWrapper(CurrentViewDevice.HostName);
                    if (await ADWrap.LoadResultsAsync())
                    {
                        ADOUTextBox.Text = ADWrap.GetDeviceOU();
                        ADOSTextBox.Text = ADWrap.GetAttributeValue("operatingsystem");
                        ADOSVerTextBox.Text = ADWrap.GetAttributeValue("operatingsystemversion");
                        ADLastLoginTextBox.Text = ADWrap.GetAttributeValue("lastlogon");
                        ADCreatedTextBox.Text = ADWrap.GetAttributeValue("whencreated");
                        ADPanel.Visible = true;
                    }
                    else
                    {
                        ADPanel.Visible = false;
                    }
                }
                else
                {
                    ADPanel.Visible = false;
                }
            }
            catch
            {
                ADPanel.Visible = false;
            }
        }

        private void SetMunisEmpStatus()
        {
            ToolTip1.SetToolTip(txtCurUser_View_REQ, "");
            if (CurrentViewDevice.CurrentUserEmpNum != "")
            {
                txtCurUser_View_REQ.BackColor = Colors.EditColor;
                ToolTip1.SetToolTip(txtCurUser_View_REQ, "Munis Linked Employee");
            }
        }

        private void SetStatusBar(string text)
        {
            if (StatusStrip1.InvokeRequired)
            {
                StatusVoidDelegate d = new StatusVoidDelegate(SetStatusBar);
                StatusStrip1.Invoke(d, new object[] { text });
            }
            else
            {
                // StatusLabel.Text = text
                StatusSlider.SlideText = text;
                StatusStrip1.Update();
            }
        }
        private void SetTracking(bool bolEnabled, bool bolCheckedOut)
        {
            if (bolEnabled)
            {
                if (!TabControl1.TabPages.Contains(TrackingTab))
                {
                    TabControl1.TabPages.Insert(1, TrackingTab);
                }
                ExpandSplitter(true);
                TrackingBox.Visible = true;
                tsTracking.Visible = bolEnabled;
                CheckOutTool.Visible = !bolCheckedOut;
                CheckInTool.Visible = bolCheckedOut;
            }
            else
            {
                tsTracking.Visible = bolEnabled;
                TabControl1.TabPages.Remove(TrackingTab);
                TrackingBox.Visible = false;
                ExpandSplitter();
            }
            StyleFunctions.SetGridStyle(DataGridHistory, GridTheme);
            StyleFunctions.SetGridStyle(TrackingGrid, GridTheme);
        }

        private void SetupNetTools(PingVis.PingInfo PingResults)
        {
            if (PingResults.Status != IPStatus.Success)
            {
                intFailedPings++;
            }
            else
            {
                intFailedPings = 0;
            }
            if (!RemoteToolsBox.Visible && PingResults.Status == IPStatus.Success)
            {
                intFailedPings = 0;
                cmdShowIP.Tag = PingResults.Address;
                ExpandSplitter(true);
                RemoteToolsBox.Visible = true;
            }
            if (intFailedPings > 10 && RemoteToolsBox.Visible)
            {
                RemoteToolsBox.Visible = false;
                ExpandSplitter();
            }
        }

        private void StartTrackDeviceForm()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.Tracking))
            {
                return;
            }
            Waiting();
            TrackDeviceForm NewTracking = new TrackDeviceForm(CurrentViewDevice, this);
            DoneWaiting();
        }

        private List<DataGridColumn> TrackingGridColumns()
        {
            List<DataGridColumn> ColList = new List<DataGridColumn>();
            ColList.Add(new DataGridColumn(TrackablesCols.DateStamp, "Date", typeof(DateTime)));
            ColList.Add(new DataGridColumn(TrackablesCols.CheckType, "Check Type", typeof(string)));
            ColList.Add(new DataGridColumn(TrackablesCols.CheckoutUser, "Check Out User", typeof(string)));
            ColList.Add(new DataGridColumn(TrackablesCols.CheckinUser, "Check In User", typeof(string)));
            ColList.Add(new DataGridColumn(TrackablesCols.CheckoutTime, "Check Out", typeof(DateTime)));
            ColList.Add(new DataGridColumn(TrackablesCols.CheckinTime, "Check In", typeof(DateTime)));
            ColList.Add(new DataGridColumn(TrackablesCols.DueBackDate, "Due Back", typeof(DateTime)));
            ColList.Add(new DataGridColumn(TrackablesCols.UseLocation, "Location", typeof(string)));
            ColList.Add(new DataGridColumn(TrackablesCols.UID, "GUID", typeof(string)));
            return ColList;
        }

        private void UpdateDevice(DeviceUpdateInfoStruct UpdateInfo)
        {
            int rows = 0;
            string SelectQry = "SELECT * FROM " + DevicesCols.TableName + " WHERE " + DevicesCols.DeviceUID + "='" + CurrentViewDevice.GUID + "'";
            string InsertQry = "SELECT * FROM " + HistoricalDevicesCols.TableName + " LIMIT 0";
            using (var trans = DBFactory.GetDatabase().StartTransaction())
            {
                using (var conn = trans.Connection)
                {
                    try
                    {
                        rows += DBFactory.GetDatabase().UpdateTable(SelectQry, GetUpdateTable(SelectQry), trans);
                        rows += DBFactory.GetDatabase().UpdateTable(InsertQry, GetInsertTable(InsertQry, UpdateInfo), trans);

                        if (rows == 2)
                        {
                            trans.Commit();
                            LoadDevice(CurrentViewDevice.GUID);
                            //OtherFunctions.Message("Update Added.", vbOKOnly + vbInformation, "Success", Me)
                            SetStatusBar("Update successful!");
                        }
                        else
                        {
                            trans.Rollback();
                            LoadDevice(CurrentViewDevice.GUID);
                            OtherFunctions.Message("Unsuccessful! The number of affected rows was not what was expected.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Unexpected Result", this);
                        }
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        if (ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod()))
                        {
                            LoadDevice(CurrentViewDevice.GUID);
                        }
                    }
                }
            }

        }
        private void ViewAttachments()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ViewAttachment))
            {
                return;
            }
            if (!Helpers.ChildFormControl.AttachmentsIsOpen(this))
            {
                AttachmentsForm NewAttachments = new AttachmentsForm(this, new DeviceAttachmentsCols(), CurrentViewDevice);
            }
        }
        private void Waiting()
        {
            OtherFunctions.SetWaitCursor(true, this);
        }
        #region Control Events

        private void AssetDisposalFormToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PdfFormFilling PDFForm = new PdfFormFilling(this, CurrentViewDevice, PdfFormType.DisposeForm);
        }

        private void AttachmentTool_Click(object sender, EventArgs e)
        {
            ViewAttachments();
        }

        private void CheckInTool_Click(object sender, EventArgs e)
        {
            StartTrackDeviceForm();
        }

        private void CheckOutTool_Click(object sender, EventArgs e)
        {
            StartTrackDeviceForm();
        }

        private void cmbEquipType_View_REQ_DropDown(object sender, EventArgs e)
        {
            OtherFunctions.AdjustComboBoxWidth(sender, e);
        }

        private void cmbEquipType_View_REQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields();
            }
        }

        private void cmbLocation_View_REQ_DropDown(object sender, EventArgs e)
        {
            OtherFunctions.AdjustComboBoxWidth(sender, e);
        }

        private void cmbLocation_View_REQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields();
            }
        }

        private void cmbOSVersion_REQ_DropDown(object sender, EventArgs e)
        {
            OtherFunctions.AdjustComboBoxWidth(sender, e);
        }

        private void cmbOSVersion_REQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields();
            }
        }

        private void cmbStatus_REQ_DropDown(object sender, EventArgs e)
        {
            OtherFunctions.AdjustComboBoxWidth(sender, e);
        }

        private void cmbStatus_REQ_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields();
            }
        }

        private void cmdAccept_Tool_Click(object sender, EventArgs e)
        {
            AcceptChanges();
        }

        private void cmdBrowseFiles_Click(object sender, EventArgs e)
        {
            BrowseFiles();
        }

        private void cmdCancel_Tool_Click(object sender, EventArgs e)
        {
            CancelModify();
        }

        private void cmdGKUpdate_Click(object sender, EventArgs e)
        {
            if (SecurityTools.VerifyAdminCreds())
            {
                var GKInstance = Helpers.ChildFormControl.GKUpdaterInstance();
                GKInstance.AddUpdate(CurrentViewDevice);
                if (!GKInstance.Visible)
                {
                    GKInstance.Show();
                }
            }
        }

        private void cmdMunisInfo_Click(object sender, EventArgs e)
        {
            try
            {
                GlobalInstances.MunisFunc.LoadMunisInfoByDevice(CurrentViewDevice, this);
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }
        private void cmdMunisSearch_Click(object sender, EventArgs e)
        {
            using (MunisUserForm NewMunisSearch = new MunisUserForm(this))
            {
                MunisUser = NewMunisSearch.EmployeeInfo;
                if (MunisUser.Name != "")
                {
                    txtCurUser_View_REQ.Text = MunisUser.Name;
                    txtCurUser_View_REQ.ReadOnly = true;
                }
            }

        }

        private void cmdRDP_Click(object sender, EventArgs e)
        {
            LaunchRDP();
        }

        private async void cmdRestart_Click(object sender, EventArgs e)
        {
            var blah = OtherFunctions.Message("Click 'Yes' to reboot this device.", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "Are you sure?");
            if (blah == DialogResult.Yes)
            {
                string IP = MyPingVis.CurrentResult.Address.ToString();
                string DeviceName = CurrentViewDevice.HostName;
                var RestartOutput = await SendRestart(IP, DeviceName);
                if ((string)RestartOutput == "")
                {
                    OtherFunctions.Message("Success", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Restart Device", this);
                }
                else
                {
                    OtherFunctions.Message("Failed" + "\r\n" + "\r\n" + "Output: " + RestartOutput, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Restart Device", this);
                }
            }
        }

        private void cmdShowIP_Click(object sender, EventArgs e)
        {
            if (!ReferenceEquals(cmdShowIP.Tag, null))
            {
                var blah = OtherFunctions.Message(cmdShowIP.Tag.ToString() + " - " + NetworkInfo.LocationOfIP(cmdShowIP.Tag.ToString()) + "\r\n" + "\r\n" + "Press 'Yes' to copy to clipboard.", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Information, "IP Address", this);
                if (blah == DialogResult.Yes)
                {
                    Clipboard.SetText(cmdShowIP.Tag.ToString());
                }
            }
        }

        private void cmdSibiLink_Click(object sender, EventArgs e)
        {
            OpenSibiLink(CurrentViewDevice);
        }

        private void DataGridHistory_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            string EntryUID = GridFunctions.GetCurrentCellValue(DataGridHistory, HistoricalDevicesCols.HistoryEntryUID);
            if (!Helpers.ChildFormControl.FormIsOpenByUID(typeof(ViewHistoryForm), EntryUID))
            {
                NewEntryView(EntryUID);
            }
        }

        private void DataGridHistory_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!bolGridFilling)
            {
                StyleFunctions.HighlightRow(DataGridHistory, GridTheme, e.RowIndex);
            }
        }

        private void DataGridHistory_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            StyleFunctions.LeaveRow(DataGridHistory, GridTheme, e.RowIndex);
        }

        private void DataGridHistory_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && e.ColumnIndex > -1 && e.RowIndex > -1)
            {
                DataGridHistory.CurrentCell = DataGridHistory[e.ColumnIndex, e.RowIndex];
            }
        }

        private void DeleteEntryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedHistoricalEntry();
        }

        private void dtPurchaseDate_View_REQ_ValueChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields();
            }
        }

        private void lblGUID_Click(object sender, EventArgs e)
        {
            Clipboard.SetText(lblGUID.Text);
            OtherFunctions.Message("GUID Copied to clipboard.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Clipboard", this);
        }

        private void RefreshToolStripButton_Click(object sender, EventArgs e)
        {
            RefreshData();
        }

        private void TabControl1_MouseDown(object sender, MouseEventArgs e)
        {
            TrackingGrid.Refresh();
        }

        private void tmr_RDPRefresher_Tick(object sender, EventArgs e)
        {
            CheckRDP();
        }

        private void TrackingGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            var EntryUID = GridFunctions.GetCurrentCellValue(TrackingGrid, TrackablesCols.UID);
            if (!Helpers.ChildFormControl.FormIsOpenByUID(typeof(ViewTrackingForm), EntryUID))
            {
                NewTrackingView(EntryUID);
            }
        }

        private void TrackingGrid_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                TrackingGrid.Columns[TrackablesCols.CheckType].DefaultCellStyle.Font = new Font(TrackingGrid.Font, FontStyle.Bold);
            }
            catch
            {
            }
        }

        private void TrackingGrid_RowPrePaint(object sender, DataGridViewRowPrePaintEventArgs e)
        {
            Color c1 = ColorTranslator.FromHtml("#8BCEE8"); //highlight color
            TrackingGrid.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.Black;
            TrackingGrid.Rows[e.RowIndex].Cells[GridFunctions.GetColIndex(TrackingGrid, TrackablesCols.CheckType)].Style.Alignment = DataGridViewContentAlignment.MiddleCenter;
            if (TrackingGrid.Rows[e.RowIndex].Cells[GridFunctions.GetColIndex(TrackingGrid, TrackablesCols.CheckType)].Value.ToString() == CheckType.Checkin)
            {
                TrackingGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Colors.CheckIn;
                Color c2 = Color.FromArgb(Colors.CheckIn.R, Colors.CheckIn.G, Colors.CheckIn.B);
                Color BlendColor = default(Color);
                BlendColor = Color.FromArgb(Convert.ToInt32((Convert.ToInt32(c1.A) + Convert.ToInt32(c2.A)) / 2),
                         Convert.ToInt32((Convert.ToInt32(c1.R) + Convert.ToInt32(c2.R)) / 2),
                         Convert.ToInt32((Convert.ToInt32(c1.G) + Convert.ToInt32(c2.G)) / 2),
                         Convert.ToInt32((Convert.ToInt32(c1.B) + Convert.ToInt32(c2.B)) / 2));
                TrackingGrid.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = BlendColor;
            }
            else if (TrackingGrid.Rows[e.RowIndex].Cells[GridFunctions.GetColIndex(TrackingGrid, TrackablesCols.CheckType)].Value.ToString() == CheckType.Checkout)
            {
                TrackingGrid.Rows[e.RowIndex].DefaultCellStyle.BackColor = Colors.CheckOut;
                Color c2 = Color.FromArgb(Colors.CheckOut.R, Colors.CheckOut.G, Colors.CheckOut.B);
                Color BlendColor = default(Color);
                BlendColor = Color.FromArgb(Convert.ToInt32((Convert.ToInt32(c1.A) + Convert.ToInt32(c2.A)) / 2),
                         Convert.ToInt32((Convert.ToInt32(c1.R) + Convert.ToInt32(c2.R)) / 2),
                         Convert.ToInt32((Convert.ToInt32(c1.G) + Convert.ToInt32(c2.G)) / 2),
                         Convert.ToInt32((Convert.ToInt32(c1.B) + Convert.ToInt32(c2.B)) / 2));
                TrackingGrid.Rows[e.RowIndex].DefaultCellStyle.SelectionBackColor = BlendColor;
            }
        }

        private void tsbDeleteDevice_Click(object sender, EventArgs e)
        {
            DeleteDevice();
        }

        private void tsbModify_Click(object sender, EventArgs e)
        {
            ModifyDevice();
        }

        private void tsbNewNote_Click(object sender, EventArgs e)
        {
            AddNewNote();
        }

        private void tsmAssetInputForm_Click(object sender, EventArgs e)
        {
            if (CurrentViewDevice.PO != "")
            {
                PdfFormFilling PDFForm = new PdfFormFilling(this, CurrentViewDevice, PdfFormType.InputForm);
            }
            else
            {
                OtherFunctions.Message("Please add a valid PO number to this device.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Missing Info", this);
            }
        }

        private void tsmAssetTransferForm_Click(object sender, EventArgs e)
        {
            PdfFormFilling PDFForm = new PdfFormFilling(this, CurrentViewDevice, PdfFormType.TransferForm);
        }

        private void txtAssetTag_View_REQ_TextChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields();
            }
        }

        private void txtCurUser_View_REQ_TextChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields();
            }
        }

        private void txtDescription_View_REQ_TextChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields();
            }
        }

        private void txtPhoneNumber_Leave(object sender, EventArgs e)
        {
            if (txtPhoneNumber.Text.Trim() != "" && !DataConsistency.ValidPhoneNumber(txtPhoneNumber.Text))
            {
                OtherFunctions.Message("Invalid phone number.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Error", this);
                txtPhoneNumber.Focus();
            }
        }

        private void txtPhoneNumber_TextChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields();
            }
        }

        private void txtSerial_View_REQ_TextChanged(object sender, EventArgs e)
        {
            if (bolCheckFields)
            {
                CheckFields();
            }
        }

        private void View_Disposed(object sender, EventArgs e)
        {
            MyWindowList.Dispose();
            MyLiveBox.Dispose();
            MyMunisToolBar.Dispose();
            Helpers.ChildFormControl.CloseChildren(this);
            if (MyPingVis != null)
            {
                MyPingVis.Dispose();
            }

        }

        private void View_Resize(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                Helpers.ChildFormControl.MinimizeChildren(this);
            }
        }

        private void DeployTVButton_Click(object sender, EventArgs e)
        {
            DeployTeamViewer(CurrentViewDevice);
        }

        private void UpdateChromeButton_Click(object sender, EventArgs e)
        {
            UpdateChrome(CurrentViewDevice);
        }
        private void ViewDeviceForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!OKToClose())
            {
                e.Cancel = true;
            }
            else
            {
                //    DisposeImages(Me)
            }
        }

        private void ViewDeviceForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MyWindowList.Dispose();
            MyLiveBox.Dispose();
            MyMunisToolBar.Dispose();
            Helpers.ChildFormControl.CloseChildren(this);
            if (MyPingVis != null)
            {
                MyPingVis.Dispose();
            }
        }

        #endregion

        #endregion


    }
}