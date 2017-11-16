using System.Threading.Tasks;
using System;
using System.Drawing;
using System.Windows.Forms;
using System.Data;
using System.Collections.Generic;
using System.Diagnostics;
using AssetManager.UserInterface.CustomControls;
using AssetManager.UserInterface.Forms.AssetManagement;
using AssetManager.UserInterface.Forms.Attachments;

namespace AssetManager.UserInterface.Forms.Sibi
{
    public partial class SibiManageRequestForm : ExtendedForm
    {

        #region Fields

        private RequestObject CurrentRequest = new RequestObject();
        private string CurrentHash;
        private bool IsModifying = false;
        private bool IsNewRequest = false;
        private bool bolDragging = false;
        private bool bolFieldsValid;
        private bool bolGridFilling = false;
        private DBControlParser DataParser;
        private Point MouseStartPos;
        private MunisToolBar MyMunisToolBar;
        private string TitleText = "Manage Request";
        private WindowList MyWindowList;
        private FormWindowState PrevWindowState;
        private SliderLabel StatusSlider;

        #endregion

        #region Constructors

        public SibiManageRequestForm(ExtendedForm parentForm, string requestUID)
        {
            DataParser = new DBControlParser(this);
            MyMunisToolBar = new MunisToolBar(this);
            MyWindowList = new WindowList(this);

            InitializeComponent();
            InitForm(parentForm, requestUID);
            OpenRequest(requestUID);
        }

        public SibiManageRequestForm(ExtendedForm parentForm)
        {
            DataParser = new DBControlParser(this);
            MyMunisToolBar = new MunisToolBar(this);
            MyWindowList = new WindowList(this);

            InitializeComponent();
            InitForm(parentForm);
            Text += " - *New Request*";
            NewRequest();
        }

        #endregion

        #region Methods

        private bool CancelModify()
        {
            if (IsModifying)
            {
                this.WindowState = FormWindowState.Normal;
                this.Activate();
                var blah = OtherFunctions.Message("Are you sure you want to discard all changes?", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "Discard Changes?", this);
                if (blah == DialogResult.Yes)
                {
                    if (IsNewRequest)
                    {
                        return true;
                    }
                    else
                    {
                        HideEditControls();
                        OpenRequest(System.Convert.ToString(CurrentRequest.GUID));
                        return true;
                    }
                }
            }
            return false;
        }

        private void ClearAll()
        {
            ClearControls(this);
            ResetBackColors(this);
            HideEditControls();
            dgvNotes.DataSource = null;
            FillCombos();
            pnlCreate.Visible = false;
            CurrentRequest = null;
            DisableControls();
            ToolStrip.BackColor = Colors.SibiToolBarColor;
            IsModifying = false;
            IsNewRequest = false;
            fieldErrorIcon.Clear();
            SetAttachCount();
            SetMunisStatus();

        }

        private void NewRequest()
        {
            try
            {
                OtherFunctions.SetWaitCursor(true, this);
                if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.AddSibi))
                {
                    if (!this.Visible)
                    {
                        this.Dispose();
                    }
                    return;
                }
                if (IsModifying)
                {
                    var blah = OtherFunctions.Message("All current changes will be lost. Are you sure you want to create a new request?", (int)MessageBoxButtons.OKCancel + (int)MessageBoxIcon.Question, "Create New Request", this);
                    if (blah != DialogResult.OK)
                    {
                        return;
                    }
                }
                ClearAll();
                IsNewRequest = true;
                SetTitle(true);
                CurrentRequest = new RequestObject();
                this.FormUID = CurrentRequest.GUID;
                IsModifying = true;
                //Set the datasource to a new empty DB table.
                var EmptyTable = DBFactory.GetDatabase().DataTableFromQueryString("SELECT " + GridFunctions.ColumnsString(RequestItemsColumns()) + " FROM " + SibiRequestItemsCols.TableName + " LIMIT 0");
                GridFunctions.PopulateGrid(RequestItemsGrid, EmptyTable, RequestItemsColumns());
                EnableControls();
                pnlCreate.Visible = true;
                this.Show();
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                this.Dispose();
            }
            finally
            {
                OtherFunctions.SetWaitCursor(false, this);
            }
        }

        private void OpenRequest(string RequestUID)
        {
            OtherFunctions.SetWaitCursor(true, this);
            try
            {
                string strRequestQRY = "SELECT * FROM " + SibiRequestCols.TableName + " WHERE " + SibiRequestCols.UID + "='" + RequestUID + "'";
                string strRequestItemsQRY = "SELECT " + GridFunctions.ColumnsString(RequestItemsColumns()) + " FROM " + SibiRequestItemsCols.TableName + " WHERE " + SibiRequestItemsCols.RequestUID + "='" + RequestUID + "' ORDER BY " + SibiRequestItemsCols.Timestamp;
                using (DataTable RequestResults = DBFactory.GetDatabase().DataTableFromQueryString(strRequestQRY))
                {
                    using (DataTable RequestItemsResults = DBFactory.GetDatabase().DataTableFromQueryString(strRequestItemsQRY))
                    {
                        RequestResults.TableName = SibiRequestCols.TableName;
                        RequestItemsResults.TableName = SibiRequestItemsCols.TableName;
                        CurrentHash = GetHash(RequestResults, RequestItemsResults);
                        ClearAll();
                        CollectRequestInfo(RequestResults, RequestItemsResults);
                        DataParser.FillDBFields(RequestResults);
                        SendToGrid(RequestItemsResults);
                        LoadNotes(System.Convert.ToString(CurrentRequest.GUID));
                        DisableControls();
                        SetTitle(false);
                        SetAttachCount();
                        this.Show();
                        this.Activate();
                        SetMunisStatus();
                        bolGridFilling = false;
                    }
                }

            }
            catch (Exception ex)
            {
                OtherFunctions.Message("An error occurred while opening the request. It may have been deleted.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Error", this);
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                this.Dispose();
            }
            finally
            {
                OtherFunctions.SetWaitCursor(false, this);
            }
        }

        private string GetHash(DataTable RequestTable, DataTable ItemsTable)
        {
            string RequestHash = System.Convert.ToString(SecurityTools.GetSHAOfTable(RequestTable));
            string ItemHash = System.Convert.ToString(SecurityTools.GetSHAOfTable(ItemsTable));
            return RequestHash + ItemHash;
        }

        private bool ConcurrencyCheck()
        {
            try
            {
                using (var RequestTable = DBFactory.GetDatabase().DataTableFromQueryString("SELECT * FROM " + SibiRequestCols.TableName + " WHERE " + SibiRequestCols.UID + "='" + CurrentRequest.GUID + "'"))
                {
                    using (var ItemTable = DBFactory.GetDatabase().DataTableFromQueryString("SELECT " + GridFunctions.ColumnsString(RequestItemsColumns()) + " FROM " + SibiRequestItemsCols.TableName + " WHERE " + SibiRequestItemsCols.RequestUID + "='" + CurrentRequest.GUID + "' ORDER BY " + SibiRequestItemsCols.Timestamp))
                    {
                        RequestTable.TableName = SibiRequestCols.TableName;
                        ItemTable.TableName = SibiRequestItemsCols.TableName;
                        string DBHash = GetHash(RequestTable, ItemTable);
                        if (DBHash != CurrentHash)
                        {
                            return false;
                        }
                        return true;
                    }
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public void SetAttachCount()
        {
            if (!GlobalSwitches.CachedMode)
            {

                if (CurrentRequest != null)
                {
                    cmdAttachments.Text = "(" + GlobalInstances.AssetFunc.GetAttachmentCount(CurrentRequest.GUID, new SibiAttachmentsCols()).ToString() + ")";
                    cmdAttachments.ToolTipText = "Attachments " + cmdAttachments.Text;
                }
                else
                {
                    cmdAttachments.Text = "(0)";
                    cmdAttachments.ToolTipText = "Attachments " + cmdAttachments.Text;
                }

            }
        }

        private void AddErrorIcon(Control ctl)
        {
            if (ReferenceEquals(fieldErrorIcon.GetError(ctl), string.Empty))
            {
                fieldErrorIcon.SetIconAlignment(ctl, ErrorIconAlignment.MiddleRight);
                fieldErrorIcon.SetIconPadding(ctl, 4);
                fieldErrorIcon.SetError(ctl, "Required Field");
            }
        }

        private bool AddNewNote(string RequestUID, string Note)
        {
            string NoteUID = System.Convert.ToString(Guid.NewGuid().ToString());
            try
            {
                List<DBParameter> NewNoteParams = new List<DBParameter>();
                NewNoteParams.Add(new DBParameter(SibiNotesCols.RequestUID, RequestUID));
                NewNoteParams.Add(new DBParameter(SibiNotesCols.NoteUID, NoteUID));
                NewNoteParams.Add(new DBParameter(SibiNotesCols.Note, Note));
                if (DBFactory.GetDatabase().InsertFromParameters(SibiNotesCols.TableName, NewNoteParams) > 0)
                {
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return false;
            }
        }

        private void AddNewRequest()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.AddSibi))
            {
                return;
            }
            if (!ValidateFields())
            {
                return;
            }
            RequestObject RequestData = CollectData();
            using (var trans = DBFactory.GetDatabase().StartTransaction())
            {
                using (var conn = trans.Connection)
                {
                    try
                    {
                        string InsertRequestQry = "SELECT * FROM " + SibiRequestCols.TableName + " LIMIT 0";
                        string InsertRequestItemsQry = "SELECT " + GridFunctions.ColumnsString(RequestItemsColumns()) + " FROM " + SibiRequestItemsCols.TableName + " LIMIT 0";
                        DBFactory.GetDatabase().UpdateTable(InsertRequestQry, GetInsertTable(InsertRequestQry, System.Convert.ToString(CurrentRequest.GUID)), trans);
                        DBFactory.GetDatabase().UpdateTable(InsertRequestItemsQry, RequestData.RequestItems, trans);
                        pnlCreate.Visible = false;
                        trans.Commit();
                        IsModifying = false;
                        IsNewRequest = false;
                        OpenRequest(System.Convert.ToString(CurrentRequest.GUID));
                        ParentForm.RefreshData();
                        OtherFunctions.Message("New Request Added.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Complete", this);
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                    }
                }
            }

        }

        private void AddNote()
        {
            try
            {
                if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifySibi))
                {
                    return;
                }
                if (CurrentRequest.GUID != "" && !IsNewRequest)
                {
                    SibiNotesForm NewNote = new SibiNotesForm(this, CurrentRequest);
                    if (NewNote.DialogResult == DialogResult.OK)
                    {
                        AddNewNote(System.Convert.ToString(NewNote.Request.GUID), NewNote.rtbNotes.Rtf.Trim());
                        LoadNotes(System.Convert.ToString(CurrentRequest.GUID));
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private bool CheckFields(Control parent)
        {
            Control c = default(Control);
            foreach (Control tempLoopVar_c in parent.Controls)
            {
                c = tempLoopVar_c;
                DBControlInfo DBInfo = new DBControlInfo();
                if (c.Tag != null)
                {
                    DBInfo = (DBControlInfo)c.Tag;
                }
                if (DBInfo.Required)
                {
                    if (true)
                    {
                        if (c.Text.Trim() == "")
                        {
                            bolFieldsValid = false;
                            c.BackColor = Colors.MissingField;
                            AddErrorIcon(c);
                        }
                        else
                        {
                            c.BackColor = Color.Empty;
                            ClearErrorIcon(c);
                        }
                    }
                    else if (true)
                    {
                        var cmb = (ComboBox)c;
                        if (cmb.SelectedIndex == -1)
                        {
                            bolFieldsValid = false;
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
                if (c.HasChildren)
                {
                    CheckFields(c);
                }
            }
            return bolFieldsValid; //if fields are missing return false to trigger a message if needed
        }

        private async void CheckForPO()
        {
            if (CurrentRequest.RequisitionNumber != "" && CurrentRequest.PO == "")
            {
                string GetPO = System.Convert.ToString(await GlobalInstances.MunisFunc.GetPOFromReqNumberAsync(CurrentRequest.RequisitionNumber, CurrentRequest.DateStamp.Year.ToString()));
                if (GetPO != null && GetPO.Length > 1)
                {
                    var blah = OtherFunctions.Message("PO Number " + GetPO + " was detected in the Requisition. Do you wish to add it to this request?", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "New PO Detected", this);
                    if (blah == DialogResult.Yes)
                    {
                        InsertPONumber(GetPO);
                        OpenRequest(System.Convert.ToString(CurrentRequest.GUID));
                    }
                    else
                    {
                        return;
                    }
                }
            }
        }

        private void chkAllowDrag_CheckedChanged(object sender, EventArgs e)
        {
            if (chkAllowDrag.Checked)
            {
                RequestItemsGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
                RequestItemsGrid.MultiSelect = false;
            }
            else
            {
                RequestItemsGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
                RequestItemsGrid.MultiSelect = true;
            }
        }

        private void ClearCheckBox(Control control)
        {
            if (control is CheckBox)
            {
                var chk = (CheckBox)control;
                chk.Checked = false;
            }
        }

        private void ClearCombos(Control control)
        {
            if (control is ComboBox)
            {
                var cmb = (ComboBox)control;
                cmb.SelectedIndex = -1;
                cmb.Text = null;
            }
        }

        private void ClearControls(Control control)
        {
            foreach (Control c in control.Controls)
            {
                ClearTextBoxes(c);
                ClearCombos(c);
                ClearDTPicker(c);
                ClearCheckBox(c);
                if (c.HasChildren)
                {
                    ClearControls(c);
                }
            }
        }

        private void ClearDTPicker(Control control)
        {
            if (control is DateTimePicker)
            {
                var dtp = (DateTimePicker)control;
                dtp.Value = DateTime.Now;
            }
        }

        private void ClearErrorIcon(Control ctl)
        {
            fieldErrorIcon.SetError(ctl, string.Empty);
        }

        private void ClearTextBoxes(Control control)
        {
            if (control is TextBox)
            {
                var txt = (TextBox)control;
                txt.Clear();
            }
        }

        private void cmdAccept_Click(object sender, EventArgs e)
        {
            RequestItemsGrid.EndEdit();
            if (!ValidateFields())
            {
                return;
            }
            DisableControls();
            ToolStrip.BackColor = Colors.SibiToolBarColor;
            HideEditControls();
            UpdateRequest();
            IsModifying = false;
        }

        private void cmdAddNew_Click(object sender, EventArgs e)
        {
            AddNewRequest();
        }

        private void cmdAddNote_Click(object sender, EventArgs e)
        {
            AddNote();
        }

        private void ViewAttachments()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ViewAttachment))
            {
                return;
            }
            if (!Helpers.ChildFormControl.AttachmentsIsOpen(this))
            {
                if (CurrentRequest.GUID != "" && !IsNewRequest)
                {
                    AttachmentsForm NewAttach = new AttachmentsForm(this, new SibiAttachmentsCols(), CurrentRequest);
                }
            }
        }

        private void cmdAttachments_Click(object sender, EventArgs e)
        {
            ViewAttachments();
        }

        private void cmdClearAll_Click(object sender, EventArgs e)
        {
            ClearAll();
            CurrentRequest = null;
        }

        private void cmdCreate_Click(object sender, EventArgs e)
        {
            NewRequest();
        }

        private void DeleteCurrentSibiReqest()
        {
            try
            {
                if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.DeleteSibi))
                {
                    return;
                }
                if (ReferenceEquals(CurrentRequest.RequestItems, null))
                {
                    return;
                }
                var blah = OtherFunctions.Message("Are you absolutely sure?  This cannot be undone and will delete all data including attachments.", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Exclamation, "WARNING", this);
                if (blah == DialogResult.Yes)
                {
                    OtherFunctions.SetWaitCursor(true, this);
                    if (GlobalInstances.AssetFunc.DeleteFtpAndSql(CurrentRequest.GUID, EntryType.Sibi))
                    {
                        OtherFunctions.Message("Sibi Request deleted successfully.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Device Deleted", this);
                        CurrentRequest = null;
                        ParentForm.RefreshData();
                        this.Dispose();
                    }
                    else
                    {
                        Logging.Logger("*****DELETION ERROR******: " + CurrentRequest.GUID);
                        OtherFunctions.Message("Failed to delete request successfully!  Please let Bobby Lovell know about this.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Stop, "Delete Failed", this);
                        CurrentRequest = null;
                        this.Dispose();
                    }
                }
                else
                {
                    return;
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                OtherFunctions.SetWaitCursor(false, this);
            }
        }

        private void cmdDelete_Click(object sender, EventArgs e)
        {
            DeleteCurrentSibiReqest();
        }

        private void DeleteCurrentNote()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifySibi))
            {
                return;
            }
            if (dgvNotes.CurrentRow != null && dgvNotes.CurrentRow.Index > -1)
            {
                var blah = OtherFunctions.Message("Are you sure?", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "Delete Note", this);
                if (blah == DialogResult.Yes)
                {
                    string NoteUID = System.Convert.ToString(GridFunctions.GetCurrentCellValue(dgvNotes, SibiNotesCols.NoteUID));
                    if (!string.IsNullOrEmpty(NoteUID))
                    {
                        OtherFunctions.Message(DeleteItem_FromSQL(NoteUID, System.Convert.ToString(SibiNotesCols.NoteUID), System.Convert.ToString(SibiNotesCols.TableName)) + " Rows affected.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Delete Item", this);
                        OpenRequest(System.Convert.ToString(CurrentRequest.GUID));
                    }
                }
            }
        }

        private void cmdDeleteNote_Click(object sender, EventArgs e)
        {
            DeleteCurrentNote();
        }

        private void cmdDiscard_Click(object sender, EventArgs e)
        {
            CancelModify();
        }

        private void cmdNewNote_Click(object sender, EventArgs e)
        {
            AddNote();
        }

        private void ModifyRequest()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifySibi))
            {
                return;
            }
            if (CurrentRequest.GUID != "" && !IsModifying)
            {
                SetModifyMode(IsModifying);
            }
        }

        private void ModifyButton_Click(object sender, EventArgs e)
        {
            ModifyRequest();
        }

        private RequestObject CollectData()
        {
            try
            {
                RequestObject info = new RequestObject();
                info.Description = txtDescription.Text.Trim();
                info.RequestUser = txtUser.Text.Trim();
                info.RequestType = AttribIndexFunctions.GetDBValue(GlobalInstances.SibiAttribute.RequestType, cmbType.SelectedIndex);
                info.NeedByDate = dtNeedBy.Value;
                info.Status = AttribIndexFunctions.GetDBValue(GlobalInstances.SibiAttribute.StatusType, cmbStatus.SelectedIndex);
                info.PO = txtPO.Text.Trim();
                info.RequisitionNumber = txtReqNumber.Text.Trim();
                info.RTNumber = txtRTNumber.Text.Trim();
                RequestItemsGrid.EndEdit();
                foreach (DataGridViewRow row in RequestItemsGrid.Rows)
                {
                    foreach (DataGridViewCell dcell in row.Cells)
                    {
                        if (dcell.Value != null)
                        {
                            dcell.Value = dcell.Value.ToString().Trim();
                        }
                        else
                        {
                            if (dcell.OwningColumn.Name == SibiRequestItemsCols.ItemUID)
                            {
                                dcell.Value = Guid.NewGuid().ToString();
                            }
                            else
                            {
                                dcell.Value = DBNull.Value;
                            }
                        }
                    }
                }
                info.RequestItems = (DataTable)RequestItemsGrid.DataSource;
                return info;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return null;
            }
        }

        private void CollectRequestInfo(DataTable RequestResults, DataTable RequestItemsResults)
        {
            try
            {
                CurrentRequest = new RequestObject(RequestResults);
                CurrentRequest.RequestItems = RequestItemsResults;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private bool DeleteItem_FromLocal(int RowIndex)
        {
            try
            {
                if (!RequestItemsGrid.Rows[RowIndex].IsNewRow)
                {
                    RequestItemsGrid.Rows.Remove(RequestItemsGrid.Rows[RowIndex]);
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return false;
            }
        }

        private int DeleteItem_FromSQL(string ItemUID, string ItemColumnName, string Table)
        {
            try
            {
                int rows = 0;
                string DeleteItemQuery = "DELETE FROM " + Table + " WHERE " + ItemColumnName + "='" + ItemUID + "'";
                rows = System.Convert.ToInt32(DBFactory.GetDatabase().ExecuteQuery(DeleteItemQuery));
                return rows;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return -1;
            }
        }

        private void dgvNotes_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            ViewNote();
        }

        private void ViewNote()
        {
            try
            {
                var NoteUID = GridFunctions.GetCurrentCellValue(dgvNotes, SibiNotesCols.NoteUID);
                if (!Helpers.ChildFormControl.FormIsOpenByUID(typeof(SibiNotesForm), NoteUID))
                {
                    SibiNotesForm ViewNote = new SibiNotesForm(this, NoteUID);
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void DisableControlsRecursive(Control control)
        {
            foreach (Control c in control.Controls)
            {

                if (c is TextBox)
                {
                    var txt = (TextBox)c;
                    txt.ReadOnly = true;
                }
                else if (c is ComboBox)
                {
                    var cmb = (ComboBox)c;
                    cmb.Enabled = false;
                }
                else if (c is DateTimePicker)
                {
                    var dtp = (DateTimePicker)c;
                    dtp.Enabled = false;
                }
                else if (c is CheckBox)
                {
                    if (c != chkAllowDrag)
                    {
                        c.Enabled = false;
                    }
                }

                if (c.HasChildren)
                {
                    DisableControlsRecursive(c);
                }
            }
        }

        private void DisableControls()
        {
            DisableControlsRecursive(this);
            DisableGrid();
        }

        private void DisableGrid()
        {
            RequestItemsGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            RequestItemsGrid.AllowUserToAddRows = false;
            RequestItemsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
        }

        private void EnableControlsRecursive(Control control)
        {
            foreach (Control c in control.Controls)
            {

                if (c is TextBox)
                {
                    var txt = (TextBox)c;
                    if (txt != txtRequestNum && txt != txtCreateDate)
                    {
                        txt.ReadOnly = false;
                    }
                }
                else if (c is ComboBox)
                {
                    var cmb = (ComboBox)c;
                    cmb.Enabled = true;
                }
                else if (c is DateTimePicker)
                {
                    var dtp = (DateTimePicker)c;
                    dtp.Enabled = true;
                }
                else if (c is CheckBox)
                {
                    c.Enabled = true;
                }

                if (c.HasChildren)
                {
                    EnableControlsRecursive(c);
                }
            }
        }

        private void EnableControls()
        {
            EnableControlsRecursive(this);
            EnableGrid();
        }

        private void EnableGrid()
        {
            RequestItemsGrid.EditMode = DataGridViewEditMode.EditOnEnter;
            RequestItemsGrid.AllowUserToAddRows = true;
            SetColumnWidths();
        }

        private void FillCombos()
        {
            AttribIndexFunctions.FillComboBox(GlobalInstances.SibiAttribute.StatusType, cmbStatus);
            AttribIndexFunctions.FillComboBox(GlobalInstances.SibiAttribute.RequestType, cmbType);
        }

        public override bool OKToClose()
        {
            bool CanClose = true;
            if (!Helpers.ChildFormControl.OKToCloseChildren(this))
            {
                CanClose = false;
            }
            if (IsModifying && !CancelModify())
            {
                CanClose = false;
            }
            return CanClose;
        }

        private DataTable GetInsertTable(string selectQuery, string UID)
        {
            try
            {
                var tmpTable = DataParser.ReturnInsertTable(selectQuery);
                var DBRow = tmpTable.Rows[0];
                //Add Add'l info
                DBRow[SibiRequestCols.UID] = UID;
                return tmpTable;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return null;
            }
        }

        private DataTable GetUpdateTable(string selectQuery)
        {
            try
            {
                var tmpTable = DataParser.ReturnUpdateTable(selectQuery);
                //Add Add'l info
                return tmpTable;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return null;
            }
        }

        private void HideEditControls()
        {
            pnlEditButtons.Visible = false;
        }

        private void HighlightCurrentRow(int Row)
        {
            try
            {
                if (!bolGridFilling)
                {
                    //TODO: See if this works.
                    // var grid = RequestItemsGrid;
                    StyleFunctions.HighlightRow(RequestItemsGrid, GridTheme, Row);
                }
            }
            catch
            {
            }
        }

        private void InitDBControls()
        {
            txtDescription.Tag = new DBControlInfo(SibiRequestCols.Description, true);
            txtUser.Tag = new DBControlInfo(SibiRequestCols.RequestUser, true);
            cmbType.Tag = new DBControlInfo(SibiRequestCols.Type, GlobalInstances.SibiAttribute.RequestType, true);
            dtNeedBy.Tag = new DBControlInfo(SibiRequestCols.NeedBy, true);
            cmbStatus.Tag = new DBControlInfo(SibiRequestCols.Status, GlobalInstances.SibiAttribute.StatusType, true);
            txtPO.Tag = new DBControlInfo(SibiRequestCols.PO, false);
            txtReqNumber.Tag = new DBControlInfo(SibiRequestCols.RequisitionNumber, false);
            txtRequestNum.Tag = new DBControlInfo(SibiRequestCols.RequestNumber, ParseType.DisplayOnly, false);
            txtRTNumber.Tag = new DBControlInfo(SibiRequestCols.RTNumber, false);
            txtCreateDate.Tag = new DBControlInfo(SibiRequestCols.DateStamp, ParseType.DisplayOnly, false);

        }

        private void InitForm(ExtendedForm ParentForm, string UID = "")
        {
            StatusSlider = new SliderLabel();
            StatusStrip1.Items.Insert(0, StatusSlider.ToToolStripControl(StatusStrip1));

            InitDBControls();
            ExtendedMethods.DoubleBufferedDataGrid(RequestItemsGrid, true);
            MyMunisToolBar.InsertMunisDropDown(ToolStrip);
            this.ParentForm = ParentForm;
            this.FormUID = UID;
            ImageCaching.CacheControlImages(this);
            MyWindowList.InsertWindowList(ToolStrip);
            StyleFunctions.SetGridStyle(RequestItemsGrid, GridTheme);
            StyleFunctions.SetGridStyle(dgvNotes, GridTheme);
            ToolStrip.BackColor = Colors.SibiToolBarColor;
        }

        private void InsertPONumber(string PO)
        {
            try
            {
                GlobalInstances.AssetFunc.UpdateSqlValue(SibiRequestCols.TableName, SibiRequestCols.PO, PO, SibiRequestCols.UID, CurrentRequest.GUID);
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void LoadNotes(string RequestUID)
        {
            try
            {
                string NotesQry = "SELECT * FROM " + SibiNotesCols.TableName + " WHERE " + SibiNotesCols.RequestUID + "='" + RequestUID + "' ORDER BY " + SibiNotesCols.DateStamp + " DESC";
                using (DataTable Results = DBFactory.GetDatabase().DataTableFromQueryString(NotesQry))
                {
                    GridFunctions.PopulateGrid(dgvNotes, Results, NotesGridColumns());
                }


                dgvNotes.ClearSelection();
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private bool MouseIsDragging(Point CurrentPos)
        {
            int intMouseMoveThreshold = 50;
            //if (NewStartPos != null)
            //{
            //    MouseStartPos = NewStartPos;
            //}
            //else
            //{
            var intDistanceMoved = Math.Sqrt(Math.Pow((MouseStartPos.X - CurrentPos.X), 2) + Math.Pow((MouseStartPos.Y - CurrentPos.Y), 2));
            if (System.Convert.ToInt32(intDistanceMoved) > intMouseMoveThreshold)
            {
                return true;
            }
            else
            {
                return false;
            }
            // }
            return false;
        }

        private async void PopulateFromFA(string ColumnName)
        {
            try
            {
                OtherFunctions.SetWaitCursor(true, this);
                if (ColumnName == SibiRequestItemsCols.NewSerial)
                {
                    var ItemUID = GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ItemUID);
                    var Serial = await Task.Run(() =>
                    {
                        return GlobalInstances.MunisFunc.GetSerialFromAsset(GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.NewAsset));
                    });
                    if (Serial != "")
                    {
                        GlobalInstances.AssetFunc.UpdateSqlValue(SibiRequestItemsCols.TableName, SibiRequestItemsCols.NewSerial, Serial, SibiRequestItemsCols.ItemUID, ItemUID);
                        RefreshRequest();
                    }
                }
                else if (ColumnName == SibiRequestItemsCols.NewAsset)
                {
                    var ItemUID = GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ItemUID);
                    var Asset = await Task.Run(() =>
                    {
                        return GlobalInstances.MunisFunc.GetAssetFromSerial(GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.NewSerial));
                    });
                    if (Asset != "")
                    {
                        GlobalInstances.AssetFunc.UpdateSqlValue(SibiRequestItemsCols.TableName, SibiRequestItemsCols.NewAsset, Asset, SibiRequestItemsCols.ItemUID, ItemUID);
                        RefreshRequest();
                    }
                }
                else if (ColumnName == SibiRequestItemsCols.ReplaceSerial)
                {
                    var ItemUID = GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ItemUID);
                    var Serial = await Task.Run(() =>
                    {
                        return GlobalInstances.MunisFunc.GetSerialFromAsset(GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ReplaceAsset));
                    });
                    if (Serial != "")
                    {
                        GlobalInstances.AssetFunc.UpdateSqlValue(SibiRequestItemsCols.TableName, SibiRequestItemsCols.ReplaceSerial, Serial, SibiRequestItemsCols.ItemUID, ItemUID);
                        RefreshRequest();
                    }
                }
                else if (ColumnName == SibiRequestItemsCols.ReplaceAsset)
                {
                    var ItemUID = GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ItemUID);
                    var Asset = await Task.Run(() =>
                    {
                        return GlobalInstances.MunisFunc.GetAssetFromSerial(GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ReplaceSerial));
                    });
                    if (Asset != "")
                    {
                        GlobalInstances.AssetFunc.UpdateSqlValue(SibiRequestItemsCols.TableName, SibiRequestItemsCols.ReplaceAsset, Asset, SibiRequestItemsCols.ItemUID, ItemUID);
                        RefreshRequest();
                    }
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                OtherFunctions.SetWaitCursor(false, this);
            }
        }

        private void RefreshRequest()
        {
            OpenRequest(System.Convert.ToString(CurrentRequest.GUID));
        }

        private List<DataGridColumn> NotesGridColumns()
        {
            List<DataGridColumn> ColList = new List<DataGridColumn>();
            ColList.Add(new DataGridColumn(SibiNotesCols.Note, "Note", typeof(string), ColumnFormatTypes.NotePreview));
            ColList.Add(new DataGridColumn(SibiNotesCols.DateStamp, "Date Stamp", typeof(DateTime)));
            ColList.Add(new DataGridColumn(SibiNotesCols.NoteUID, "UID", typeof(string), true, false));
            return ColList;
        }

        private List<DataGridColumn> RequestItemsColumns()
        {
            List<DataGridColumn> ColList = new List<DataGridColumn>();
            ColList.Add(new DataGridColumn(SibiRequestItemsCols.User, "User", typeof(string)));
            ColList.Add(new DataGridColumn(SibiRequestItemsCols.Description, "Description", typeof(string)));
            ColList.Add(new DataGridColumn(SibiRequestItemsCols.Qty, "Qty", typeof(int)));
            ColList.Add(new DataGridColumn(SibiRequestItemsCols.Location, "Location", GlobalInstances.DeviceAttribute.Locations));
            ColList.Add(new DataGridColumn(SibiRequestItemsCols.Status, "Status", GlobalInstances.SibiAttribute.ItemStatusType));
            ColList.Add(new DataGridColumn(SibiRequestItemsCols.ReplaceAsset, "Replace Asset", typeof(string)));
            ColList.Add(new DataGridColumn(SibiRequestItemsCols.ReplaceSerial, "Replace Serial", typeof(string)));
            ColList.Add(new DataGridColumn(SibiRequestItemsCols.NewAsset, "New Asset", typeof(string)));
            ColList.Add(new DataGridColumn(SibiRequestItemsCols.NewSerial, "New Serial", typeof(string)));
            ColList.Add(new DataGridColumn(SibiRequestItemsCols.OrgCode, "Org Code", typeof(string)));
            ColList.Add(new DataGridColumn(SibiRequestItemsCols.ObjectCode, "Object Code", typeof(string)));
            ColList.Add(new DataGridColumn(SibiRequestItemsCols.ItemUID, "Item UID", typeof(string), true, true));
            ColList.Add(new DataGridColumn(SibiRequestItemsCols.RequestUID, "Request UID", typeof(string), true, false));
            return ColList;
        }

        private void RequestItemsGrid_CellEnter(object sender, DataGridViewCellEventArgs e)
        {
            HighlightCurrentRow(System.Convert.ToInt32(e.RowIndex));
        }

        private void RequestItemsGrid_CellLeave(object sender, DataGridViewCellEventArgs e)
        {
            //TODO: See if this works.
            //var grid = RequestItemsGrid;
            StyleFunctions.LeaveRow(RequestItemsGrid, GridTheme, e.RowIndex);
        }

        private void RequestItemsGrid_CellMouseDown(object sender, DataGridViewCellMouseEventArgs e)
        {
            try
            {
                if (e.ColumnIndex >= -1 && e.RowIndex >= 0)
                {
                    int ColIndex = System.Convert.ToInt32(System.Convert.ToInt32(e.ColumnIndex == -1 ? 0 : e.ColumnIndex));
                    if (!RequestItemsGrid[ColIndex, e.RowIndex].Selected)
                    {
                        RequestItemsGrid.Rows[e.RowIndex].Selected = true;
                        RequestItemsGrid.CurrentCell = RequestItemsGrid[ColIndex, e.RowIndex];
                    }
                    SetToolStripItems();
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void RequestItemsGrid_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            OtherFunctions.Message("DataGrid Error: " + "\u0022" + e.Exception.Message + "\u0022" + "   Col/Row:" + e.ColumnIndex + "/" + e.RowIndex, (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "DataGrid Error", this);
        }

        private void RequestItemsGrid_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells[SibiRequestItemsCols.Qty].Value = 1;
        }

        private void RequestItemsGrid_DragDrop(object sender, DragEventArgs e)
        {
            //Drag-drop rows from other data grids, adding new UIDs and the current FKey (RequestUID).
            //This was a tough nut to crack. In the end I just ended up building an item array and adding it to the receiving grids datasource.
            try
            {
                if (IsModifying)
                {
                    var R = (DataGridViewRow)(e.Data.GetData(typeof(DataGridViewRow))); //Cast the DGVRow
                    if (R.DataBoundItem != null)
                    {
                        DataRow NewDataRow = ((DataRowView)R.DataBoundItem).Row; //Get the databound row
                        List<object> ItemArr = new List<object>();
                        foreach (DataColumn col in NewDataRow.Table.Columns) //Iterate through columns and build a new item list
                        {
                            if (col.ColumnName == SibiRequestItemsCols.ItemUID)
                            {
                                ItemArr.Add(Guid.NewGuid().ToString());
                            }
                            else if (col.ColumnName == SibiRequestItemsCols.RequestUID)
                            {
                                ItemArr.Add(CurrentRequest.GUID);
                            }
                            else
                            {
                                ItemArr.Add(NewDataRow[col]);
                            }
                        }
                        ((DataTable)RequestItemsGrid.DataSource).Rows.Add(ItemArr.ToArray()); //Add the item list as an array
                    }
                    bolDragging = false;
                }
                else
                {
                    if (!bolDragging)
                    {
                        OtherFunctions.Message("You must be modifying this request before you can drag-drop rows from another request.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Not Allowed", this);
                    }
                }
            }
            catch (Exception)
            {
                bolDragging = false;
            }
        }

        private void RequestItemsGrid_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Copy;
        }

        private void RequestItemsGrid_DragLeave(object sender, EventArgs e)
        {
            bolDragging = false;
        }

        private void RequestItemsGrid_MouseDown(object sender, MouseEventArgs e)
        {
            // MouseIsDragging(e.Location);
            MouseStartPos = e.Location;
        }

        private void RequestItemsGrid_MouseMove(object sender, MouseEventArgs e)
        {
            if (RequestItemsGrid.SelectedRows.Count > 0)
            {
                if (chkAllowDrag.Checked && !bolDragging)
                {
                    if (e.Button == MouseButtons.Left)
                    {
                        if (MouseIsDragging(CurrentPos: e.Location))
                        {
                            bolDragging = true;
                            RequestItemsGrid.DoDragDrop(RequestItemsGrid.SelectedRows[0], DragDropEffects.All);
                        }
                    }
                }
            }
        }

        private void RequestItemsGrid_MouseUp(object sender, MouseEventArgs e)
        {
            bolDragging = false;
        }

        private void RequestItemsGrid_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (!bolGridFilling)
            {
                RequestItemsGrid.Rows[e.RowIndex].Cells[SibiRequestItemsCols.RequestUID].Value = CurrentRequest.GUID;
                if (ReferenceEquals(RequestItemsGrid.Rows[e.RowIndex].Cells[SibiRequestItemsCols.ItemUID].Value, null))
                {
                    RequestItemsGrid.Rows[e.RowIndex].Cells[SibiRequestItemsCols.ItemUID].Value = Guid.NewGuid().ToString();
                }
            }
        }

        private void RequestItemsGrid_RowLeave(object sender, DataGridViewCellEventArgs e)
        {
            if (IsModifying)
            {
                ValidateRequestItems();
            }
        }

        private void RequestItemsGrid_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            using (SolidBrush b = new SolidBrush(Color.Black))
            {
                e.Graphics.DrawString((e.RowIndex + 1).ToString(),
                    RequestItemsGrid.DefaultCellStyle.Font,
                    b,
                    e.RowBounds.Location.X + 20,
                    e.RowBounds.Location.Y + 4);
            }

        }

        private void ResetBackColors(Control parent)
        {
            foreach (Control c in parent.Controls)
            {

                if (c is TextBox)
                {
                    c.BackColor = Color.Empty;
                }
                else if (c is ComboBox)
                {
                    c.BackColor = Color.Empty;
                }
                if (c.HasChildren)
                {
                    ResetBackColors(c);
                }
            }
        }

        private void SendToGrid(DataTable Results)
        {
            try
            {
                bolGridFilling = true;
                GridFunctions.PopulateGrid(RequestItemsGrid, Results, RequestItemsColumns(), true);
                RequestItemsGrid.ClearSelection();
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void SetColumnWidths()
        {
            RequestItemsGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
            RequestItemsGrid.Columns[1].Width = 200;
            RequestItemsGrid.Columns[2].Width = 50;
            RequestItemsGrid.Columns[3].Width = 200;
            RequestItemsGrid.Columns[4].Width = 200;
            RequestItemsGrid.RowHeadersWidth = 57;
        }

        private void SetGLBudgetContextMenu()
        {
            if (RequestItemsGrid.CurrentCell != null)
            {
                int ColIndex = System.Convert.ToInt32(RequestItemsGrid.CurrentCell.ColumnIndex);
                if ((true) || (true))
                {
                    if (GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ObjectCode) != "" && GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.OrgCode) != "")
                    {
                        tsmGLBudget.Visible = true;
                    }
                    else
                    {
                        tsmGLBudget.Visible = false;
                    }
                }
                else
                {
                    tsmGLBudget.Visible = false;
                }
            }
        }

        private void SetMunisStatus()
        {
            if (!GlobalSwitches.CachedMode)
            {
                if (CurrentRequest != null)
                {
                    SetReqStatus(System.Convert.ToString(CurrentRequest.RequisitionNumber), System.Convert.ToInt32(CurrentRequest.DateStamp.Year));
                    CheckForPO();
                    SetPOStatus(System.Convert.ToString(CurrentRequest.PO));
                }
                else
                {
                    SetReqStatus(string.Empty, -1);
                    SetPOStatus(string.Empty);
                }
            }
        }

        private async void SetPOStatus(string PO)
        {
            int intPO = 0;
            lblPOStatus.Text = "Status: NA";
            if (PO != "" && int.TryParse(PO, out intPO))
            {
                string GetStatusString = System.Convert.ToString(await GlobalInstances.MunisFunc.GetPOStatusFromPO(intPO));
                if (!string.IsNullOrEmpty(GetStatusString))
                {
                    lblPOStatus.Text = "Status: " + GetStatusString;
                }
            }
        }

        private async void SetReqStatus(string ReqNum, int FY)
        {
            int intReq = 0;
            lblReqStatus.Text = "Status: NA";
            if (FY > 0)
            {
                if (ReqNum != "" && int.TryParse(ReqNum, out intReq))
                {
                    string GetStatusString = System.Convert.ToString(await GlobalInstances.MunisFunc.GetReqStatusFromReqNum(ReqNum, FY));
                    if (!string.IsNullOrEmpty(GetStatusString))
                    {
                        lblReqStatus.Text = "Status: " + GetStatusString;
                    }
                }
            }
        }

        private void SetTitle(bool NewRequest = false)
        {
            if (!NewRequest)
            {
                this.Text = TitleText + " - " + CurrentRequest.Description;
            }
            else
            {
                this.Text = TitleText + " - *New Request*";
            }
        }

        private void SetToolStripItems()
        {
            if (RequestItemsGrid.CurrentCell != null)
            {
                if (ValidColumn())
                {
                    tsmPopFA.Visible = true;
                    tsmSeparator.Visible = true;
                    if (RequestItemsGrid.CurrentCell.Value != null && RequestItemsGrid.CurrentCell.Value.ToString() != "")
                    {
                        tsmLookupDevice.Visible = true;
                    }
                    else
                    {
                        tsmLookupDevice.Visible = false;
                    }
                }
                else
                {
                    tsmPopFA.Visible = false;
                    tsmSeparator.Visible = false;
                    tsmLookupDevice.Visible = false;
                }
                if (IsModifying)
                {
                    tsmDeleteItem.Visible = true;
                }
                else
                {
                    tsmDeleteItem.Visible = false;
                }
            }
            SetGLBudgetContextMenu();
        }

        private void ShowEditControls()
        {
            pnlEditButtons.Visible = true;
        }

        private void tsbRefresh_Click(object sender, EventArgs e)
        {
            if (!IsNewRequest)
            {
                OpenRequest(System.Convert.ToString(CurrentRequest.GUID));
            }
        }

        private void tsmCopyText_Click(object sender, EventArgs e)
        {
            RequestItemsGrid.RowHeadersVisible = false;
            GridFunctions.CopySelectedGridData(RequestItemsGrid);
            RequestItemsGrid.RowHeadersVisible = true;
        }

        private void DeleteSelectedRequestItem()
        {
            try
            {
                if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifySibi))
                {
                    return;
                }
                var blah = OtherFunctions.Message("Delete selected row?", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "Delete Item Row", this);
                if (blah == DialogResult.Yes)
                {
                    if (!DeleteItem_FromLocal(System.Convert.ToInt32(RequestItemsGrid.CurrentRow.Index)))
                    {
                        blah = OtherFunctions.Message("Failed to delete row.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Error", this);
                    }
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void tsmDeleteItem_Click(object sender, EventArgs e)
        {
            DeleteSelectedRequestItem();
        }

        private void tsmGLBudget_Click(object sender, EventArgs e)
        {
            try
            {
                var Org = GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.OrgCode);
                var Obj = GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ObjectCode);
                var FY = CurrentRequest.DateStamp.Year.ToString();
                GlobalInstances.MunisFunc.NewOrgObView(Org, Obj, FY, this);
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void tsmLookupDevice_Click(object sender, EventArgs e)
        {
            try
            {
                int colIndex = System.Convert.ToInt32(RequestItemsGrid.CurrentCell.ColumnIndex);
                if (colIndex == GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.ReplaceAsset))
                {
                    Helpers.ChildFormControl.LookupDevice(this, GlobalInstances.AssetFunc.FindDeviceFromAssetOrSerial(RequestItemsGrid[colIndex, RequestItemsGrid.CurrentRow.Index].Value.ToString(), FindDevType.AssetTag));
                }
                else if (colIndex == GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.ReplaceSerial))
                {
                    Helpers.ChildFormControl.LookupDevice(this, GlobalInstances.AssetFunc.FindDeviceFromAssetOrSerial(RequestItemsGrid[colIndex, RequestItemsGrid.CurrentRow.Index].Value.ToString(), FindDevType.Serial));
                }
                else if (colIndex == GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.NewAsset))
                {
                    Helpers.ChildFormControl.LookupDevice(this, GlobalInstances.AssetFunc.FindDeviceFromAssetOrSerial(RequestItemsGrid[colIndex, RequestItemsGrid.CurrentRow.Index].Value.ToString(), FindDevType.AssetTag));
                }
                else if (colIndex == GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.NewSerial))
                {
                    Helpers.ChildFormControl.LookupDevice(this, GlobalInstances.AssetFunc.FindDeviceFromAssetOrSerial(RequestItemsGrid[colIndex, RequestItemsGrid.CurrentRow.Index].Value.ToString(), FindDevType.Serial));
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void PopulateCurrentFAItem()
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.ModifySibi))
            {
                return;
            }
            try
            {
                if (ValidColumn())
                {
                    PopulateFromFA(System.Convert.ToString(RequestItemsGrid.CurrentCell.OwningColumn.Name));
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void tsmPopFA_Click(object sender, EventArgs e)
        {
            PopulateCurrentFAItem();
        }

        private void txtPO_Click(object sender, EventArgs e)
        {
            string PO = txtPO.Text.Trim();
            if (!IsModifying && !string.IsNullOrEmpty(PO))
            {
                GlobalInstances.MunisFunc.NewMunisPOSearch(PO, this);
            }
        }

        private void txtReqNumber_Click(object sender, EventArgs e)
        {
            try
            {
                OtherFunctions.SetWaitCursor(true, this);
                string ReqNum = txtReqNumber.Text.Trim();
                if (!IsModifying && !string.IsNullOrEmpty(ReqNum))
                {
                    GlobalInstances.MunisFunc.NewMunisReqSearch(ReqNum, DataConsistency.YearFromDate(CurrentRequest.DateStamp), this);
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                OtherFunctions.SetWaitCursor(false, this);
            }
        }

        private void txtRTNumber_Click(object sender, EventArgs e)
        {
            try
            {
                string RTNum = txtRTNumber.Text.Trim();
                if (!IsModifying && !string.IsNullOrEmpty(RTNum))
                {
                    Process.Start("http://rt.co.fairfield.oh.us/rt/Ticket/Display.html?id=" + RTNum);
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void SetModifyMode(bool Enable)
        {
            if (!Enable)
            {
                if (!ConcurrencyCheck())
                {
                    RefreshRequest();
                    OtherFunctions.Message("This request has been modified since it's been open and has been refreshed with the current data.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Information, "Concurrency Check", this);
                }
                EnableControls();
                ToolStrip.BackColor = Colors.EditColor;
                ShowEditControls();
                IsModifying = true;
            }
            else
            {
                DisableControls();
                ToolStrip.BackColor = Colors.SibiToolBarColor;
                HideEditControls();
                UpdateRequest();
                IsModifying = false;
            }
        }

        private void UpdateRequest()
        {
            using (var trans = DBFactory.GetDatabase().StartTransaction())
            {
                using (var conn = trans.Connection)
                {
                    try
                    {
                        if (!ConcurrencyCheck())
                        {
                            OtherFunctions.Message("It appears that someone else has modified this request. Please refresh and try again.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Concurrency Failure", this);
                            return;
                        }
                        RequestObject RequestData = CollectData();
                        RequestData.GUID = CurrentRequest.GUID;
                        if (ReferenceEquals(RequestData.RequestItems, null))
                        {
                            return;
                        }
                        string RequestUpdateQry = "SELECT * FROM " + SibiRequestCols.TableName + " WHERE " + SibiRequestCols.UID + " = '" + CurrentRequest.GUID + "'";
                        string RequestItemsUpdateQry = "SELECT " + GridFunctions.ColumnsString(RequestItemsColumns()) + " FROM " + SibiRequestItemsCols.TableName + " WHERE " + SibiRequestItemsCols.RequestUID + " = '" + CurrentRequest.GUID + "'";

                        DBFactory.GetDatabase().UpdateTable(RequestUpdateQry, GetUpdateTable(RequestUpdateQry), trans);
                        DBFactory.GetDatabase().UpdateTable(RequestItemsUpdateQry, RequestData.RequestItems, trans);

                        trans.Commit();

                        ParentForm.RefreshData();
                        OpenRequest(System.Convert.ToString(CurrentRequest.GUID));
                        StatusSlider.NewSlideMessage("Update successful!");
                    }
                    catch (Exception ex)
                    {
                        trans.Rollback();
                        ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                    }
                }
            }

        }

        private bool ValidateFields()
        {
            bolFieldsValid = true;
            bool ValidateResults = CheckFields(this);
            if (ValidateResults)
            {
                ValidateResults = ValidateRequestItems();
            }
            if (!ValidateResults)
            {
                OtherFunctions.Message("Some required fields are missing. Please enter data into all require fields.", (int)MessageBoxButtons.OK + (int)MessageBoxIcon.Exclamation, "Missing Data", this);
            }
            return ValidateResults;
        }

        private bool ValidateRequestItems()
        {
            bool RowsValid = true;
            foreach (DataGridViewRow row in RequestItemsGrid.Rows)
            {
                if (!row.IsNewRow)
                {
                    foreach (DataGridViewCell dcell in row.Cells)
                    {
                        string CellString = "";
                        if (dcell.Value != null)
                        {
                            CellString = System.Convert.ToString(dcell.Value.ToString());
                        }
                        else
                        {
                            CellString = "";
                        }
                        if (ReferenceEquals(dcell.OwningColumn.CellType, typeof(DataGridViewComboBoxCell)))
                        {
                            if (ReferenceEquals(dcell.Value, null) || string.IsNullOrEmpty(CellString))
                            {
                                RowsValid = false;
                                dcell.ErrorText = "Required Field!";
                            }
                            else
                            {
                                dcell.ErrorText = null;
                            }
                        }
                        if (dcell.OwningColumn.Name == SibiRequestItemsCols.Qty)
                        {
                            if (ReferenceEquals(dcell.Value, null) || string.IsNullOrEmpty(CellString))
                            {
                                RowsValid = false;
                                dcell.ErrorText = "Required Field!";
                            }
                            else
                            {
                                dcell.ErrorText = null;
                            }
                        }
                    }
                }
            }
            return RowsValid;
        }

        private bool ValidColumn()
        {
            try
            {

                var colIndex = RequestItemsGrid.CurrentCell.ColumnIndex;
                if (colIndex == GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.ReplaceAsset))
                {
                    return true;
                }
                else if (colIndex == GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.ReplaceSerial))
                {
                    return true;
                }
                else if (colIndex == GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.NewAsset))
                {
                    return true;
                }
                else if (colIndex == GridFunctions.GetColIndex(RequestItemsGrid, SibiRequestItemsCols.NewSerial))
                {
                    return true;
                }
                else
                {
                    return false;
                }
                return false;
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
                return false;
            }
        }

        private void NewDeviceMenuItem_Click(object sender, EventArgs e)
        {
            if (!SecurityTools.CheckForAccess(SecurityTools.AccessGroup.AddDevice))
            {
                return;
            }
            NewDeviceForm NewDev = new NewDeviceForm(this);
            NewDev.ImportFromSibi(GridFunctions.GetCurrentCellValue(RequestItemsGrid, SibiRequestItemsCols.ItemUID));

        }

        private void SibiManageRequestForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!OKToClose())
            {
                e.Cancel = true;
            }
        }

        private void SibiManageRequestForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            MyMunisToolBar.Dispose();
            MyWindowList.Dispose();
            Helpers.ChildFormControl.CloseChildren(this);
        }

        private void SibiManageRequestForm_Resize(object sender, EventArgs e)
        {
            //Form f = (Form)sender;
            if (this.WindowState == FormWindowState.Minimized)
            {
                Helpers.ChildFormControl.MinimizeChildren(this);
                PrevWindowState = this.WindowState;
            }
            else if (this.WindowState != PrevWindowState && this.WindowState == FormWindowState.Normal)
            {
                if (PrevWindowState != FormWindowState.Maximized)
                {
                    Helpers.ChildFormControl.RestoreChildren(this);
                }
            }
        }

        private void SibiManageRequestForm_ResizeBegin(object sender, EventArgs e)
        {
            //Form f = (Form)sender;
            PrevWindowState = this.WindowState;
        }

        private void SibiManageRequestForm_Load(object sender, EventArgs e)
        {
            // InitForm();
        }

        #endregion


    }
}