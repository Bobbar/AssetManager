using AssetManager.UserInterface.CustomControls;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace AssetManager.UserInterface.Forms.AdminTools
{
    public partial class UserManagerForm : ExtendedForm
    {
        private List<AccessGroupObject> ModuleIndex = new List<AccessGroupObject>();
        private LocalUserInfoStruct CurrentUser;
        private string UserDataQuery = "SELECT * FROM " + UsersCols.TableName;

        private int SelectedRow;

        public UserManagerForm(ExtendedForm parentForm)
        {
            Load += frmUserManager_Load;
            InitializeComponent();
            this.ParentForm = parentForm;
            StyleFunctions.SetGridStyle(UserGrid, GridTheme);
            Show();
        }

        private void frmUserManager_Load(object sender, EventArgs e)
        {
            LoadUserData();
        }

        private void LoadUserData()
        {
            ListUsers();
            ModuleIndex = BuildModuleIndex();
            LoadModuleBoxes();
            UpdateAccessLabel();
        }

        private void ListUsers()
        {
            SendToGrid();
        }

        private void SendToGrid()
        {
            UserGrid.DataSource = DBFactory.GetDatabase().DataTableFromQueryString(UserDataQuery);
            UserGrid.Columns[UsersCols.UID].ReadOnly = true;
        }

        private void DisplayAccess(int intAccessLevel)
        {
            CheckState[] clbItemStates = new CheckState[clbModules.Items.Count];
            foreach (CheckBox chkBox in clbModules.Items)
            {
                if (SecurityTools.CanAccess(chkBox.Name, intAccessLevel))
                {
                    clbItemStates[clbModules.Items.IndexOf(chkBox)] = CheckState.Checked;
                }
                else
                {
                    clbItemStates[clbModules.Items.IndexOf(chkBox)] = CheckState.Unchecked;
                }
            }
            for (int i = 0; i <= clbItemStates.Count() - 1; i++)
            {
                clbModules.SetItemCheckState(i, clbItemStates[i]);
            }
            UpdateAccessLabel();
            AutoSizeCLBColumns(clbModules);
        }

        public List<AccessGroupObject> BuildModuleIndex()
        {
            List<AccessGroupObject> tmpList = new List<AccessGroupObject>();
            using (DataTable ModuleTable = DBFactory.GetDatabase().DataTableFromQueryString("SELECT * FROM " + SecurityCols.TableName + " ORDER BY " + SecurityCols.AccessLevel + ""))
            {
                foreach (DataRow row in ModuleTable.Rows)
                {
                    tmpList.Add(new AccessGroupObject(row));
                }
                return tmpList;
            }
        }

        private void LoadModuleBoxes()
        {
            CheckBox chkModuleBox = null;
            clbModules.Items.Clear();
            foreach (AccessGroupObject ModuleBox in ModuleIndex)
            {
                chkModuleBox = new CheckBox();
                var _with1 = chkModuleBox;
                _with1.Text = ModuleBox.Description;
                _with1.Name = ModuleBox.AccessModule;
                clbModules.DisplayMember = "Text";
                clbModules.Items.Add(chkModuleBox);
            }
            AutoSizeCLBColumns(clbModules);
        }

        private int CalcAccessLevel()
        {
            int intAccessLevel = 0;
            foreach (CheckBox chkBox in clbModules.Items)
            {
                if (clbModules.GetItemCheckState(clbModules.Items.IndexOf(chkBox)) == CheckState.Checked)
                {
                    intAccessLevel += SecurityTools.GetSecGroupValue(chkBox.Name);
                }
            }
            return intAccessLevel;
        }

        private void UserGrid_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (UserGrid[GridFunctions.GetColIndex(UserGrid, UsersCols.AccessLevel), UserGrid.CurrentRow.Index].Value is int)
            {
                DisplayAccess(Convert.ToInt32(UserGrid[GridFunctions.GetColIndex(UserGrid, UsersCols.AccessLevel), UserGrid.CurrentRow.Index].Value));
                GetUserInfo();
            }
            else
            {
                DisplayAccess(0);
            }
            SelectedRow = e.RowIndex;
        }

        private void GetUserInfo()
        {
            var _with2 = CurrentUser;
            _with2.AccessLevel = Convert.ToInt32(GridFunctions.GetCurrentCellValue(UserGrid, UsersCols.AccessLevel));
            _with2.UserName = GridFunctions.GetCurrentCellValue(UserGrid, UsersCols.UserName);
            _with2.GUID = GridFunctions.GetCurrentCellValue(UserGrid, UsersCols.UID);
            _with2.Fullname = GridFunctions.GetCurrentCellValue(UserGrid, UsersCols.FullName);
        }

        private void cmdUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                var blah = OtherFunctions.Message("Are you sure?  Committed changes cannot be undone.", (int)MessageBoxButtons.YesNo + (int)MessageBoxIcon.Question, "Commit Changes", this);
                if (blah == DialogResult.Yes)
                {
                    UserGrid.EndEdit();
                    AddGUIDs();
                    DBFactory.GetDatabase().UpdateTable(UserDataQuery, (DataTable)UserGrid.DataSource);
                    ListUsers();
                    SecurityTools.GetUserAccess();
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

        private void AddGUIDs()
        {
            foreach (DataGridViewRow rows in UserGrid.Rows)
            {
                if (string.IsNullOrEmpty(rows.Cells[GridFunctions.GetColIndex(UserGrid, UsersCols.UID)].EditedFormattedValue.ToString()))
                {
                    string UserUID = Guid.NewGuid().ToString();
                    rows.Cells[GridFunctions.GetColIndex(UserGrid, UsersCols.UID)].Value = UserUID;
                }
            }
        }

        private void AddAccessLevelToGrid()
        {
            UserGrid.Rows[SelectedRow].Cells[GridFunctions.GetColIndex(UserGrid, UsersCols.AccessLevel)].Selected = true;
            UserGrid.BeginEdit(false);
            UserGrid.Rows[SelectedRow].Cells[GridFunctions.GetColIndex(UserGrid, UsersCols.AccessLevel)].Value = CalcAccessLevel();
            UserGrid.EndEdit();
        }

        private void AutoSizeCLBColumns(CheckedListBox CLB)
        {
            CLB.ColumnWidth = Convert.ToInt32(CLB.Width / 2);
        }

        private void UpdateAccessLabel()
        {
            lblAccessValue.Text = "Selected Access Level: " + CalcAccessLevel();
        }

        private void clbModules_MouseUp(object sender, MouseEventArgs e)
        {
            UpdateAccessLabel();
            AddAccessLevelToGrid();
        }

        private void UserGrid_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                UserGrid.Rows.RemoveAt(SelectedRow);
            }
        }

        private void UserGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            UserGrid.BeginEdit(false);
        }

        private void cmdRefresh_Click(object sender, EventArgs e)
        {
            LoadUserData();
        }
    }
}