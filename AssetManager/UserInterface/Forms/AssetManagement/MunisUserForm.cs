using AssetManager.UserInterface.CustomControls;
using System;
using System.Data;
using System.Windows.Forms;

namespace AssetManager.UserInterface.Forms.AssetManagement
{
    public partial class MunisUserForm : ExtendedForm
    {
        public MunisEmployeeStruct EmployeeInfo
        {
            get
            {
                using (this)
                {
                    if (DialogResult == DialogResult.Yes)
                    {
                        return SelectedEmpInfo;
                    }
                    return new MunisEmployeeStruct();
                }
            }
        }

        private MunisEmployeeStruct SelectedEmpInfo = new MunisEmployeeStruct();

        private const int intMaxResults = 50;

        public MunisUserForm(ExtendedForm parentForm)
        {
            Shown += MunisUserForm_Shown;
            Load += MunisUser_Load;
            InitializeComponent();
            this.ParentForm = parentForm;
            ShowDialog(parentForm);
        }

        private async void EmpNameSearch(string Name)
        {
            try
            {
                MunisResults.DataSource = null;
                string strColumns = "a_employee_number,a_name_last,a_name_first,a_org_primary,a_object_primary,a_location_primary,a_location_p_desc,a_location_p_short";
                string strQRY = "SELECT TOP " + intMaxResults + " " + strColumns + " FROM pr_employee_master WHERE a_name_last LIKE '%" + Name.ToUpper() + "%' OR a_name_first LIKE '" + Name.ToUpper() + "'";
                MunisComms MunisComms = new MunisComms();
                SetWorking(true);
                using (DataTable results = await MunisComms.ReturnSqlTableAsync(strQRY))
                {
                    if (results.Rows.Count < 1)
                        return;
                    MunisResults.DataSource = results;
                    MunisResults.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
            finally
            {
                SetWorking(false);
            }
        }

        private void SetWorking(bool Working)
        {
            pbWorking.Visible = Working;
        }

        private void MunisUser_Load(object sender, EventArgs e)
        {
            MunisResults.DefaultCellStyle = StyleFunctions.DefaultGridStyles;
        }

        private void MunisResults_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedEmpInfo.Name = GridFunctions.GetCurrentCellValue(MunisResults, "a_name_first") + " " + GridFunctions.GetCurrentCellValue(MunisResults, "a_name_last");
            SelectedEmpInfo.Number = GridFunctions.GetCurrentCellValue(MunisResults, "a_employee_number");
            lblSelectedEmp.Text = "Selected Emp: " + SelectedEmpInfo.Name + " - " + SelectedEmpInfo.Number;
        }

        private void cmdSearch_Click(object sender, EventArgs e)
        {
            this.Cursor = Cursors.WaitCursor;
            EmpNameSearch(txtSearchName.Text.Trim());
            this.Cursor = Cursors.Default;
        }

        private void SelectEmp()
        {
            if (!string.IsNullOrEmpty(SelectedEmpInfo.Name) && !string.IsNullOrEmpty(SelectedEmpInfo.Number))
            {
                GlobalInstances.AssetFunc.AddNewEmp(SelectedEmpInfo);
                this.DialogResult = DialogResult.Yes;
                this.Close();
            }
            else
            {
                this.DialogResult = DialogResult.Abort;
                this.Close();
            }
        }

        private void cmdAccept_Click(object sender, EventArgs e)
        {
            SelectEmp();
        }

        private void txtSearchName_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Cursor = Cursors.WaitCursor;
                EmpNameSearch(txtSearchName.Text.Trim());
                this.Cursor = Cursors.Default;
            }
        }

        private void MunisResults_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectEmp();
        }

        private void MunisUserForm_Shown(object sender, EventArgs e)
        {
            txtSearchName.Focus();
        }
    }
}