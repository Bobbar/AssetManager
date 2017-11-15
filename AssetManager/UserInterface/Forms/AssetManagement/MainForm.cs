using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Data.Common;
using System.Deployment.Application;
using System.Management.Automation.Runspaces;
using MyDialogLib;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.VisualBasic;
using System.Data;
using System.Diagnostics;
using AssetManager.UserInterface.CustomControls;
using AssetManager.UserInterface.Forms.AdminTools;

namespace AssetManager.UserInterface.Forms.AssetManagement
{


    public partial class MainForm : ExtendedForm
    {

        #region "Fields"

        private const string strShowAllQry = "SELECT * FROM " + DevicesCols.TableName + " ORDER BY " + DevicesCols.InputDateTime + " DESC";
        private bool bolGridFilling = false;
        private DbCommand LastCommand;

        private LiveBox MyLiveBox;// = new LiveBox(this);
        private CustomControls.MunisToolBar MyMunisToolBar;// = new MunisToolBar(this);
        private CustomControls.WindowList MyWindowList;// = new WindowList(this);
        private bool QueryRunning = false;
        private ConnectionWatchdog WatchDog;

        private DbTransaction CurrentTransaction = null;
        #endregion

        #region "Delegates"

        delegate void ConnectStatusVoidDelegate(string text, Color foreColor, Color backColor, string toolTipText);

        delegate void StatusVoidDelegate(string text, int timeOut = 0);

        delegate void ServerTimeVoidDelegate(string serverTime);

        #endregion

        #region "Methods"
        private void StartTransaction()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.CanStartTransaction))
                return;
            if (OtherFunctions.Message("This will allow unchecked changes to the database. Incorrect inputs WILL BREAK THINGS! \r\n" + Environment.NewLine + "Changes must be 'applied' and 'committed' before they will be permanently stored in the database.", (int)MessageBoxButtons.OKCancel + (int)MessageBoxIcon.Exclamation, "WARNING", this) == MsgBoxResult.Ok)
            {
                CurrentTransaction = DBFactory.GetDatabase().StartTransaction();
                RefreshData();
                GridEditMode(true);
                DoneWaiting();
            }
        }

        private void CommitTransaction()
        {
            if (CurrentTransaction != null)
            {
                if (OtherFunctions.Message("Are you sure? This will permanently apply the changes to the database.", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "Commit Transaction", this) == MsgBoxResult.Yes)
                {
                    CurrentTransaction.Commit();
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                    RefreshData();
                    GridEditMode(false);
                    DoneWaiting();
                }
            }
        }

        private void RollbackTransaction()
        {
            if (CurrentTransaction != null)
            {
                if (OtherFunctions.Message("Restore database to original state?", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "Rollback Transaction", this) == MsgBoxResult.Yes)
                {
                    CurrentTransaction.Rollback();
                    CurrentTransaction.Dispose();
                    CurrentTransaction = null;
                    RefreshData();
                    GridEditMode(false);
                    DoneWaiting();
                }
            }
        }

        private void UpdateRecords()
        {
            if (CurrentTransaction != null)
            {
                DBFactory.GetDatabase().UpdateTable(strShowAllQry, (DataTable)ResultGrid.DataSource, CurrentTransaction);
                RefreshData();
                DoneWaiting();
            }
        }

        private void GridEditMode(bool canEdit)
        {
            if (canEdit)
            {
                ResultGrid.ReadOnly = false;
                ResultGrid.EditMode = DataGridViewEditMode.EditOnEnter;
                TransactionBox.Visible = true;
            }
            else
            {
                ResultGrid.ReadOnly = true;
                TransactionBox.Visible = false;
                ResultGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            }
        }

        //dynamically creates sql query using any combination of search filters the users wants
        public void DynamicSearch()
        {
            try
            {
                string strStartQry;
                if (chkHistorical.Checked)
                {
                    strStartQry = "SELECT * FROM " + HistoricalDevicesCols.TableName + " WHERE ";
                }
                else
                {
                    strStartQry = "SELECT * FROM " + DevicesCols.TableName + " WHERE ";
                }
                string strDynaQry = "";
                List<DBQueryParameter> SearchValCol = BuildSearchList();
                StartBigQuery(DBFactory.GetDatabase().GetCommandFromParams(strStartQry, SearchValCol));
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        public void LoadDevice(string deviceGUID)
        {
            if (!Helpers.ChildFormControl.FormIsOpenByUID(typeof(ViewDeviceForm), deviceGUID))
            {
                Waiting();
                ViewDeviceForm NewView = new ViewDeviceForm(this, deviceGUID);
                DoneWaiting();
            }
        }

        public override void RefreshData()
        {
            StartBigQuery(LastCommand);
        }

        private void AddNewDevice()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.AddDevice))
                return;
            var NewDevForm = Helpers.ChildFormControl.GetChildOfType(this, typeof(NewDeviceForm));
            if (NewDevForm == null)
            {
                NewDeviceForm NewDev = new NewDeviceForm(this);
            }
            else
            {
                Helpers.ChildFormControl.ActivateForm(NewDevForm);
            }
        }

        private List<DBQueryParameter> BuildSearchList()
        {
            List<DBQueryParameter> tmpList = new List<DBQueryParameter>();
            DBControlParser DataParser = new DBControlParser(this);
            foreach (Control ctl in DataParser.GetDBControls(this))
            {
                object CtlValue = DataParser.GetDBControlValue(ctl);
                if (!object.ReferenceEquals(CtlValue, DBNull.Value) && CtlValue.ToString() != "")
                {
                    var DBInfo = (DBControlInfo)ctl.Tag;
                    bool IsExact = false;
                    switch (DBInfo.DataColumn)
                    {
                        case DevicesCols.OSVersion:
                            IsExact = true;
                            break;
                        case DevicesCols.EQType:
                            IsExact = true;
                            break;
                        case DevicesCols.Location:
                            IsExact = true;
                            break;
                        case DevicesCols.Status:
                            IsExact = true;
                            break;
                        default:
                            IsExact = false;
                            break;
                    }
                    tmpList.Add(new DBQueryParameter(DBInfo.DataColumn, CtlValue, IsExact));
                }
            }
            return tmpList;
        }

        private void Clear_All()
        {
            txtAssetTag.Clear();
            txtAssetTagSearch.Clear();
            txtSerial.Clear();
            txtSerialSearch.Clear();
            cmbEquipType.Items.Clear();
            cmbOSType.Items.Clear();
            cmbLocation.Items.Clear();
            txtCurUser.Clear();
            txtDescription.Clear();
            txtReplaceYear.Clear();
            chkTrackables.Checked = false;
            chkHistorical.Checked = false;
            RefreshCombos();
        }

        private async void WatchDogRebuildCache(object sender, EventArgs e)
        {
            if (GlobalSwitches.BuildingCache)
                return;
            GlobalSwitches.BuildingCache = true;
            try
            {
                SetStatusBar("Rebuilding DB Cache...");
                await Task.Run(() =>
                {
                    if (!DBCacheFunctions.VerifyCacheHashes())
                    {
                        DBCacheFunctions.RefreshLocalDBCache();
                    }
                });
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                GlobalSwitches.BuildingCache = false;
                DoneWaiting();
            }
        }

        private void WatchDogTick(object sender, EventArgs e)
        {
            var TickEvent = (WatchDogTickEventArgs)e;
            if (DateTimeLabel.Text != TickEvent.ServerTime)
            {
                SetServerTime(TickEvent.ServerTime);
            }
        }

        private void WatchDogStatusChanged(object sender, EventArgs e)
        {
            var ConnectionEventArgs = (WatchDogStatusEventArgs)e;
            Logging.Logger("Connection Status changed to: " + ConnectionEventArgs.ConnectionStatus.ToString());
            switch (ConnectionEventArgs.ConnectionStatus)
            {

                case WatchDogConnectionStatus.Online:
                    ConnectStatus("Connected", Color.Green, Colors.DefaultFormBackColor, "Connection OK");
                    GlobalSwitches.CachedMode = false;
                    ServerInfo.ServerPinging = true;

                    break;
                case WatchDogConnectionStatus.Offline:
                    ConnectStatus("Offline", Color.Red, Colors.StatusBarProblem, "No connection. Cache unavailable.");
                    GlobalSwitches.CachedMode = false;
                    ServerInfo.ServerPinging = false;

                    break;
                case WatchDogConnectionStatus.CachedMode:
                    ConnectStatus("Cached Mode", Color.Black, Colors.StatusBarProblem, "Server Offline. Using Local DB Cache.");
                    GlobalSwitches.CachedMode = true;
                    ServerInfo.ServerPinging = false;

                    break;
            }
        }

        private void ConnectStatus(string text, Color foreColor, Color backColor, string toolTipText)
        {
            if (StatusStrip1.InvokeRequired)
            {
                ConnectStatusVoidDelegate d = new ConnectStatusVoidDelegate(ConnectStatus);
                StatusStrip1.Invoke(d, new object[] {
                text,
                foreColor,
                backColor,
                toolTipText
            });
            }
            else
            {
                ConnStatusLabel.Text = text;
                ConnStatusLabel.ToolTipText = toolTipText;
                ConnStatusLabel.ForeColor = foreColor;
                StatusStrip1.BackColor = backColor;
                StatusStrip1.Update();
            }
        }

        private void DisplayRecords(int NumberOf)
        {
            lblRecords.Text = "Records: " + NumberOf;
        }

        private void DoneWaiting()
        {
            OtherFunctions.SetWaitCursor(false, this);
            StripSpinner.Visible = false;
            SetStatusBar("Idle...");
        }

        private void EnqueueGKUpdate()
        {
            if (SecurityTools.VerifyAdminCreds())
            {
                List<DeviceObject> SelectedDevices = new List<DeviceObject>();
                HashSet<int> Rows = new HashSet<int>();
                //Iterate selected cells and collect unique row indexes via a HashSet.(HashSet only allows unique values to be added to collection).
                foreach (DataGridViewCell cell in ResultGrid.SelectedCells)
                {
                    Rows.Add(cell.RowIndex);
                }

                foreach (var row in Rows)
                {
                    string DevUID = GridFunctions.GetCurrentCellValue(ResultGrid, DevicesCols.DeviceUID);
                    SelectedDevices.Add(GlobalInstances.AssetFunc.GetDeviceInfoFromGUID(DevUID));
                }

               // GKUpdaterForm.AddMultipleUpdates(SelectedDevices); //TODO: Start an actual instance of GKUpdaterForm.
            }
        }

        // ERROR: Handles clauses are not supported in C#
        private void Form1_Closing(object sender, CancelEventArgs e)
        {
            if (CurrentTransaction != null)
            {
                OtherFunctions.Message("There is currently an active transaction. Please commit or rollback before closing.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Cannot Close");
            }

            if (!OtherFunctions.OKToEnd() | !Helpers.ChildFormControl.OKToCloseChildren(this) | CurrentTransaction != null)
            {
                e.Cancel = true;
            }
            else
            {
                LastCommand.Dispose();
                MyLiveBox.Dispose();
                MyWindowList.Dispose();
                MyMunisToolBar.Dispose();
                OtherFunctions.EndProgram();
            }
        }

        private void GetGridStyles()
        {
            //set colors

            Colors.DefaultGridBackColor = ResultGrid.DefaultCellStyle.BackColor;
            Colors.DefaultGridSelectColor = ResultGrid.DefaultCellStyle.SelectionBackColor;

            ResultGrid.DefaultCellStyle.SelectionBackColor = Colors.OrangeSelectColor;

            this.GridTheme = new GridTheme(Colors.OrangeHighlightColor, ResultGrid.DefaultCellStyle.SelectionBackColor, ResultGrid.DefaultCellStyle.BackColor);

            System.Windows.Forms.DataGridViewCellStyle tmpStyle = new System.Windows.Forms.DataGridViewCellStyle();
            tmpStyle.Alignment = ResultGrid.DefaultCellStyle.Alignment;
            tmpStyle.BackColor = ResultGrid.DefaultCellStyle.BackColor;
            tmpStyle.Font = ResultGrid.DefaultCellStyle.Font;
            tmpStyle.ForeColor = ResultGrid.DefaultCellStyle.ForeColor;
            tmpStyle.SelectionBackColor = ResultGrid.DefaultCellStyle.SelectionBackColor;
            tmpStyle.SelectionForeColor = ResultGrid.DefaultCellStyle.SelectionForeColor;
            tmpStyle.WrapMode = ResultGrid.DefaultCellStyle.WrapMode;
            StyleFunctions.GridStyles = tmpStyle;
        }

        private void InitDBControls()
        {
            txtSerialSearch.Tag = new DBControlInfo(DevicesCols.Serial);
            txtAssetTagSearch.Tag = new DBControlInfo(DevicesCols.AssetTag);
            txtDescription.Tag = new DBControlInfo(DevicesCols.Description);
            cmbEquipType.Tag = new DBControlInfo(DevicesCols.EQType, GlobalInstances.DeviceAttribute.EquipType);
            txtReplaceYear.Tag = new DBControlInfo(DevicesCols.ReplacementYear);
            cmbOSType.Tag = new DBControlInfo(DevicesCols.OSVersion, GlobalInstances.DeviceAttribute.OSType);
            cmbLocation.Tag = new DBControlInfo(DevicesCols.Location, GlobalInstances.DeviceAttribute.Locations);
            txtCurUser.Tag = new DBControlInfo(DevicesCols.CurrentUser);
            cmbStatus.Tag = new DBControlInfo(DevicesCols.Status, GlobalInstances.DeviceAttribute.StatusType);
            chkTrackables.Tag = new DBControlInfo(DevicesCols.Trackable);
        }

        private void InitLiveBox()
        {
            MyLiveBox.AttachToControl(txtDescription, DevicesCols.Description, LiveBoxType.DynamicSearch);
            MyLiveBox.AttachToControl(txtCurUser, DevicesCols.CurrentUser, LiveBoxType.DynamicSearch);
            MyLiveBox.AttachToControl(txtSerial, DevicesCols.Serial, LiveBoxType.InstaLoad, DevicesCols.DeviceUID);
            MyLiveBox.AttachToControl(txtAssetTag, DevicesCols.AssetTag, LiveBoxType.InstaLoad, DevicesCols.DeviceUID);
        }

        private void InitDBCombo()
        {
            foreach (var item in Enum.GetValues(typeof(Databases)))
            {
                DatabaseToolCombo.Items.Add(item.ToString());
            }
            DatabaseToolCombo.SelectedIndex = (int)ServerInfo.CurrentDataBase;
        }

        private void LoadProgram()
        {
            try
            {

                MyLiveBox = new LiveBox(this);
                MyMunisToolBar = new CustomControls.MunisToolBar(this);
                MyWindowList = new CustomControls.WindowList(this);


                ShowAll();
                DateTimeLabel.Text = DateAndTime.Now.ToString();
                ToolStrip1.BackColor = Colors.AssetToolBarColor;
                ExtendedMethods.DoubleBufferedDataGrid(ResultGrid, true);
                if (SecurityTools.CanAccess(SecurityTools.AccessGroup.IsAdmin))
                {
                    AdminDropDown.Visible = true;
                }
                else
                {
                    AdminDropDown.Visible = false;
                }
                GetGridStyles();
                StyleFunctions.SetGridStyle(ResultGrid);

                WatchDog = new ConnectionWatchdog(GlobalSwitches.CachedMode);
                WatchDog.StatusChanged += WatchDogStatusChanged;
                WatchDog.RebuildCache += WatchDogRebuildCache;
                WatchDog.WatcherTick += WatchDogTick;
                WatchDog.StartWatcher();

                MyMunisToolBar.InsertMunisDropDown(ToolStrip1, 2);
                MyWindowList.InsertWindowList(ToolStrip1);
                ImageCaching.CacheControlImages(this);
                InitLiveBox();
                InitDBControls();
                Clear_All();
                ShowTestDBWarning();
                InitDBCombo();
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                OtherFunctions.EndProgram();
            }
        }

        // ERROR: Handles clauses are not supported in C#
        private void MainForm_Shown(object sender, EventArgs e)
        {
            //SplashScreenForm.Dispose(); //TODO: Figure out how to find and dispose this.
        }

        private void NewTextCrypterForm()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.IsAdmin))
                return;
            CrypterForm NewEncryp = new CrypterForm(this);
        }

        private void OpenSibiMainForm()
        {
            try
            {
                if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ViewSibi))
                    return;
                Waiting();
                var SibiForm = Helpers.ChildFormControl.GetChildOfType(this, typeof(Sibi.SibiMainForm));
                if (SibiForm == null)
                {
                    Sibi.SibiMainForm NewSibi = new Sibi.SibiMainForm(this);
                }
                else
                {
                    Helpers.ChildFormControl.ActivateForm(SibiForm);
                }
            }
            finally
            {
                DoneWaiting();
            }
        }

        private void RefreshCombos()
        {
            AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.EquipType, cmbEquipType);
            AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.Locations, cmbLocation);
            AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.StatusType, cmbStatus);
            AttribIndexFunctions.FillComboBox(GlobalInstances.DeviceAttribute.OSType, cmbOSType);
        }

        private List<DataGridColumn> ResultGridColumns()
        {
            ColumnFormatTypes AttribColumnType;
            if (CurrentTransaction != null)
            {
                AttribColumnType = ColumnFormatTypes.AttributeCombo;
            }
            else
            {
                AttribColumnType = ColumnFormatTypes.AttributeDisplayMemberOnly;
            }
            List<DataGridColumn> ColList = new List<DataGridColumn>();
            ColList.Add(new DataGridColumn(DevicesCols.CurrentUser, "User", typeof(string)));
            ColList.Add(new DataGridColumn(DevicesCols.AssetTag, "Asset ID", typeof(string)));
            ColList.Add(new DataGridColumn(DevicesCols.Serial, "Serial", typeof(string)));
            ColList.Add(new DataGridColumn(DevicesCols.EQType, "Device Type", GlobalInstances.DeviceAttribute.EquipType, AttribColumnType));
            ColList.Add(new DataGridColumn(DevicesCols.Description, "Description", typeof(string)));
            ColList.Add(new DataGridColumn(DevicesCols.OSVersion, "OS Version", GlobalInstances.DeviceAttribute.OSType, AttribColumnType));
            ColList.Add(new DataGridColumn(DevicesCols.Location, "Location", GlobalInstances.DeviceAttribute.Locations, AttribColumnType));
            ColList.Add(new DataGridColumn(DevicesCols.PO, "PO Number", typeof(string)));
            ColList.Add(new DataGridColumn(DevicesCols.PurchaseDate, "Purchase Date", typeof(System.DateTime)));
            ColList.Add(new DataGridColumn(DevicesCols.ReplacementYear, "Replace Year", typeof(string)));
            ColList.Add(new DataGridColumn(DevicesCols.LastModDate, "Modified", typeof(System.DateTime)));
            ColList.Add(new DataGridColumn(DevicesCols.DeviceUID, "GUID", typeof(string)));
            return ColList;
        }

        private void SendToGrid(ref DataTable results)
        {
            if (results == null)
                return;
            using (results)
            {
                SetStatusBar("Building Grid...");
                bolGridFilling = true;
                if (CurrentTransaction != null)
                {
                    GridFunctions.PopulateGrid(ResultGrid, results, ResultGridColumns(), true);
                }
                else
                {
                    GridFunctions.PopulateGrid(ResultGrid, results, ResultGridColumns());
                }
                ResultGrid.ClearSelection();
                ResultGrid.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCells);
                bolGridFilling = false;
                DisplayRecords(ResultGrid.Rows.Count);
            }
        }

        private void ShowAll()
        {
            var cmd = DBFactory.GetDatabase().GetCommand(strShowAllQry);
            StartBigQuery(cmd);
        }

        private void StartAdvancedSearch()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.AdvancedSearch))
                return;
            AdvancedSearchForm NewAdvancedSearch = new AdvancedSearchForm(this);
        }

        private void StartAttachScan()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ManageAttachment))
                return;
            GlobalInstances.FTPFunc.ScanAttachements();
        }

        private async void StartBigQuery(DbCommand QryCommand)
        {
            try
            {
                if (!QueryRunning)
                {
                    OtherFunctions.SetWaitCursor(true, this);
                    StripSpinner.Visible = true;
                    SetStatusBar("Background query running...");
                    QueryRunning = true;
                    var Results = await Task.Run(() =>
                    {
                        LastCommand = QryCommand;
                    // If CurrentTransaction IsNot Nothing Then
                    return DBFactory.GetDatabase().DataTableFromCommand(QryCommand, CurrentTransaction);
                    //Else
                    //    Return DBFactory.GetDatabase().DataTableFromCommand(QryCommand)
                    //End If
                });
                    QryCommand.Dispose();
                    SendToGrid(ref Results);
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                QueryRunning = false;
                DoneWaiting();
            }
        }

        private void StartUserManager()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.IsAdmin))
                return;
            UserManagerForm NewUserMan = new UserManagerForm(this);
        }

        private void SetStatusBar(string text, int timeOut = 0)
        {
            if (StatusStrip1.InvokeRequired)
            {
                StatusVoidDelegate d = new StatusVoidDelegate(SetStatusBar);
                StatusStrip1.Invoke(d, new object[] { text, timeOut });
            }
            else
            {
                StatusLabel.Text = text;
                StatusStrip1.Update();
            }
        }

        private void SetServerTime(string serverTime)
        {
            if (StatusStrip1.InvokeRequired)
            {
                ServerTimeVoidDelegate d = new ServerTimeVoidDelegate(SetServerTime);
                StatusStrip1.Invoke(d, new object[] { serverTime });
            }
            else
            {
                DateTimeLabel.Text = serverTime;
                StatusStrip1.Update();
            }
        }

        private void ChangeDatabase(Databases database)
        {
            try
            {
                if (CurrentTransaction == null)
                {
                    if (!GlobalSwitches.CachedMode & ServerInfo.ServerPinging)
                    {
                        if (database != ServerInfo.CurrentDataBase)
                        {
                            var blah = OtherFunctions.Message("Are you sure? This will close all open forms.", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "Change Database", this);
                            if (blah == MsgBoxResult.Yes)
                            {
                                if (Helpers.ChildFormControl.OKToCloseChildren(this))
                                {
                                    Helpers.ChildFormControl.CloseChildren(this);
                                    ServerInfo.CurrentDataBase = database;
                                    AttribIndexFunctions.PopulateAttributeIndexes();
                                    RefreshCombos();
                                    SecurityTools.GetUserAccess();
                                    InitDBControls();
                                    GlobalSwitches.BuildingCache = true;
                                    Task.Run(() => DBCacheFunctions.RefreshLocalDBCache());
                                    ShowTestDBWarning();
                                    SetDatabaseTitleText();
                                    ShowAll();
                                }
                            }
                        }
                    }
                    else
                    {
                        OtherFunctions.Message("Cannot switch database while Offline or in Cached Mode.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Unavailable", this);
                    }
                }
                else
                {
                    OtherFunctions.Message("There is currently an active transaction. Please commit or rollback before switching databases.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Stop");
                }

            }
            finally
            {
                DatabaseToolCombo.SelectedIndex = (int)ServerInfo.CurrentDataBase;
            }
        }

        private void SetDatabaseTitleText()
        {
            switch (ServerInfo.CurrentDataBase)
            {
                case Databases.asset_manager:
                    this.Text = "Asset Manager - Main";
                    break;
                case Databases.test_db:
                    this.Text = "Asset Manager - Main - ****TEST DATABASE****";
                    break;
                case Databases.vintondd:
                    this.Text = "Asset Manager - Main - Vinton DD";
                    break;
            }

        }

        private void ShowTestDBWarning()
        {
            if (ServerInfo.CurrentDataBase == Databases.test_db)
            {
                OtherFunctions.Message("TEST DATABASE IN USE", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "WARNING", this);
                this.BackColor = Color.DarkRed;
                //Me.Text += " - ****TEST DATABASE****"
            }
            else
            {
                this.BackColor = Color.FromArgb(232, 232, 232);
                //  Me.Text = "Asset Manager - Main"
            }
        }

        private void Waiting()
        {
            OtherFunctions.SetWaitCursor(true, this);
            SetStatusBar("Processing...");
        }

        private async Task<bool> StartPowerShellScript(byte[] scriptByte, string hostName = "")
        {
            try
            {
                //Dim Hostname As String
                if (hostName == "")
                {
                    using (AdvancedDialog GetHostnameDialog = new AdvancedDialog(this))
                    {
                        {
                            GetHostnameDialog.Text = "Remote Computer Hostname";
                            GetHostnameDialog.AddTextBox("HostnameText", "Hostname:");
                            GetHostnameDialog.ShowDialog();
                            if (GetHostnameDialog.DialogResult == DialogResult.OK)
                            {
                                hostName = GetHostnameDialog.GetControlValue("HostnameText").ToString().Trim();
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }

                if (hostName != "")
                {
                    if (SecurityTools.VerifyAdminCreds())
                    {
                        Waiting();
                        PowerShellWrapper PSWrapper = new PowerShellWrapper();
                        if (await PSWrapper.ExecutePowerShellScript(hostName, scriptByte))
                        {
                            OtherFunctions.Message("Command successful.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Done", this);
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return false;
            }
            finally
            {
                DoneWaiting();
            }
        }

        private async Task<bool> StartPowerShellCommand(Command PScommand, string hostName = "")
        {
            try
            {
                //Dim Hostname As String
                if (hostName == "")
                {
                    using (AdvancedDialog GetHostnameDialog = new AdvancedDialog(this))
                    {
                        {
                            GetHostnameDialog.Text = "Remote Computer Hostname";
                            GetHostnameDialog.AddTextBox("HostnameText", "Hostname:");
                            GetHostnameDialog.ShowDialog();
                            if (GetHostnameDialog.DialogResult == DialogResult.OK)
                            {
                                hostName = GetHostnameDialog.GetControlValue("HostnameText").ToString().Trim();
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }

                if (hostName != "")
                {
                    if (SecurityTools.VerifyAdminCreds())
                    {
                        Waiting();
                        PowerShellWrapper PSWrapper = new PowerShellWrapper();
                        if (await PSWrapper.InvokePowerShellCommand(hostName, PScommand))
                        {
                            //  OtherFunctions.Message("Command successful.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Done", Me)
                            return true;
                        }
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return false;
            }
            finally
            {
                DoneWaiting();
            }
        }


        #region "Control Event Methods"

        // ERROR: Handles clauses are not supported in C#
        private void AdvancedSearchMenuItem_Click(object sender, EventArgs e)
        {
            StartAdvancedSearch();
        }

        // ERROR: Handles clauses are not supported in C#
        private void AddDeviceTool_Click(object sender, EventArgs e)
        {
            AddNewDevice();
        }

        // ERROR: Handles clauses are not supported in C#
        private void cmbEquipType_DropDown(object sender, EventArgs e)
        {
            OtherFunctions.AdjustComboBoxWidth(sender, e);
        }

        // ERROR: Handles clauses are not supported in C#
        private void cmbLocation_DropDown(object sender, EventArgs e)
        {
            OtherFunctions.AdjustComboBoxWidth(sender, e);
        }

        // ERROR: Handles clauses are not supported in C#
        private void cmbOSType_DropDown(object sender, EventArgs e)
        {
            OtherFunctions.AdjustComboBoxWidth(sender, e);
        }

        // ERROR: Handles clauses are not supported in C#
        private void cmdClear_Click(object sender, EventArgs e)
        {
            Clear_All();
        }

        // ERROR: Handles clauses are not supported in C#
        private void cmdSearch_Click(object sender, EventArgs e)
        {
            DynamicSearch();
        }

        // ERROR: Handles clauses are not supported in C#
        private void cmdShowAll_Click(object sender, EventArgs e)
        {
            ShowAll();
        }

        // ERROR: Handles clauses are not supported in C#
        private void cmdSibi_Click(object sender, EventArgs e)
        {
            OpenSibiMainForm();
        }

        // ERROR: Handles clauses are not supported in C#
        private void cmdSupDevSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable results = GlobalInstances.AssetFunc.DevicesBySupervisor(this);
                if (results != null)
                {
                    SendToGrid(ref results);
                }
                else
                {
                    //do nutzing
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

        // ERROR: Handles clauses are not supported in C#
        private void CopyTool_Click(object sender, EventArgs e)
        {
            GridFunctions.CopySelectedGridData(ResultGrid);
        }

        // ERROR: Handles clauses are not supported in C#
        private void DateTimeLabel_Click(object sender, EventArgs e)
        {
            if (ApplicationDeployment.IsNetworkDeployed)
            {
                OtherFunctions.Message(ApplicationDeployment.CurrentDeployment.CurrentVersion.ToString(), (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information,"Version", this);
            }
            else
            {
                OtherFunctions.Message("Debug", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Version", this);
            }
        }

        // ERROR: Handles clauses are not supported in C#
        private void PanelNoScrollOnFocus1_MouseWheel(object sender, MouseEventArgs e)
        {
            MyLiveBox.HideLiveBox();
        }

        // ERROR: Handles clauses are not supported in C#
        private void PanelNoScrollOnFocus1_Scroll(object sender, ScrollEventArgs e)
        {
            MyLiveBox.HideLiveBox();
        }

        // ERROR: Handles clauses are not supported in C#
        private void ResultGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!bolGridFilling)
            {
                //NOTE: Not sure if this will work.
                //DataGridView grid = ResultGrid;
                StyleFunctions.HighlightRow(ResultGrid, GridTheme, e.RowIndex);
            }
        }

        // ERROR: Handles clauses are not supported in C#
        private void ResultGrid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            ////NOTE: Not sure if this will work.
            //DataGridView grid = ResultGrid;
            StyleFunctions.LeaveRow(ResultGrid, GridTheme, e.RowIndex);
        }

        // ERROR: Handles clauses are not supported in C#
        private void ResultGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (e.ColumnIndex >= 0 & e.RowIndex >= 0)
            {
                if (e.Button == MouseButtons.Right & !ResultGrid[e.ColumnIndex, e.RowIndex].Selected)
                {
                    ResultGrid.Rows[e.RowIndex].Selected = true;
                    ResultGrid.CurrentCell = ResultGrid[e.ColumnIndex, e.RowIndex];
                }
            }
        }

        //// ERROR: Handles clauses are not supported in C#
        //private void ResultGrid_CellDoubleClick(object sender, EventArgs e)
        //{
        //    LoadDevice(GridFunctions.GetCurrentCellValue(ResultGrid, DevicesCols.DeviceUID));
        //}

        // ERROR: Handles clauses are not supported in C#
        private void ResultGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                LoadDevice(GridFunctions.GetCurrentCellValue(ResultGrid, DevicesCols.DeviceUID));
                e.SuppressKeyPress = true;
            }
        }

        // ERROR: Handles clauses are not supported in C#
        private void ScanAttachmentToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartAttachScan();
        }

        // ERROR: Handles clauses are not supported in C#
        private void TextEnCrypterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewTextCrypterForm();
        }

        // ERROR: Handles clauses are not supported in C#
        private void tsmAddGKUpdate_Click(object sender, EventArgs e)
        {
            EnqueueGKUpdate();
        }

        // ERROR: Handles clauses are not supported in C#
        private void tsmGKUpdater_Click(object sender, EventArgs e)
        {

            var GKUpdater = Helpers.ChildFormControl.GKUpdaterInstance();
            if (!GKUpdater.Visible)
            {
                GKUpdater.Show();
            }
            else
            {
                GKUpdater.WindowState = FormWindowState.Normal;
                GKUpdater.Activate();
            }
                      
        }

        // ERROR: Handles clauses are not supported in C#
        private void tsmSendToGridForm_Click(object sender, EventArgs e)
        {
            GridFunctions.CopyToGridForm(ResultGrid, this);
        }

        // ERROR: Handles clauses are not supported in C#
        private void tsmUserManager_Click(object sender, EventArgs e)
        {
            StartUserManager();
        }

        // ERROR: Handles clauses are not supported in C#
        private void txtGUID_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                LoadDevice(txtGUID.Text.Trim());
                txtGUID.Clear();
            }
        }

        // ERROR: Handles clauses are not supported in C#
        private void ViewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            LoadDevice(GridFunctions.GetCurrentCellValue(ResultGrid, DevicesCols.DeviceUID));
        }

        // ERROR: Handles clauses are not supported in C#
        private void InstallChromeMenuItem_Click(object sender, EventArgs e)
        {
          
            StartPowerShellScript(Properties.Resources.UpdateChrome);
        }

        // ERROR: Handles clauses are not supported in C#
        private void MainForm_Disposed(object sender, EventArgs e)
        {
            LastCommand.Dispose();
            MyLiveBox.Dispose();
            MyMunisToolBar.Dispose();
            MyWindowList.Dispose();
            WatchDog.Dispose();
        }

        // ERROR: Handles clauses are not supported in C#
        private void ReEnterLACredentialsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SecurityTools.ClearAdminCreds();
            SecurityTools.VerifyAdminCreds();
        }

        // ERROR: Handles clauses are not supported in C#
        private void ViewLogToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessStartInfo StartInfo = new ProcessStartInfo();
            StartInfo.FileName = Paths.LogPath;
            Process.Start(StartInfo);
        }

        // ERROR: Handles clauses are not supported in C#
        private void DatabaseToolCombo_DropDownClosed(object sender, EventArgs e)
        {
            ChangeDatabase((Databases)DatabaseToolCombo.SelectedIndex);
        }

        // ERROR: Handles clauses are not supported in C#
        private void MainForm_Load(object sender, EventArgs e)
        {
            LoadProgram();
            Application.DoEvents();
        }
        // ERROR: Handles clauses are not supported in C#
        private void CommitButton_Click(object sender, EventArgs e)
        {
            CommitTransaction();
        }

        // ERROR: Handles clauses are not supported in C#
        private void RollbackButton_Click(object sender, EventArgs e)
        {
            RollbackTransaction();
        }

        // ERROR: Handles clauses are not supported in C#
        private void StartTransactionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            StartTransaction();
        }

        // ERROR: Handles clauses are not supported in C#
        private void UpdateButton_Click(object sender, EventArgs e)
        {
            UpdateRecords();
        }

        #endregion

        #endregion

        private void ResultGrid_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            LoadDevice(GridFunctions.GetCurrentCellValue(ResultGrid, DevicesCols.DeviceUID));
        }
    }
}