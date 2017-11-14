using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
namespace AssetManager
{
    public partial class SibiSelectorForm
    {

        public string SibiUID
        {
            get { return SelectedUID; }
        }


        private string SelectedUID;
        public SibiSelectorForm(Form parentForm)
        {
            Load += frmSibiSelector_Load;
            InitializeComponent();
            Icon = parentForm.Icon;
            ShowDialog(parentForm);
        }

        private void frmSibiSelector_Load(object sender, EventArgs e)
        {
            ExtendedMethods.DoubleBufferedDataGrid(ResultGrid, true);
            ShowAll();
        }

        // Data() As Device_Info)
        private void SendToGrid(DataTable results)
        {
            try
            {
                using (DataTable table = new DataTable())
                {
                    table.Columns.Add("Request #", typeof(string));
                    table.Columns.Add("Status", typeof(string));
                    table.Columns.Add("Description", typeof(string));
                    table.Columns.Add("Request User", typeof(string));
                    table.Columns.Add("Request Type", typeof(string));
                    table.Columns.Add("Need By", typeof(string));
                    table.Columns.Add("PO Number", typeof(string));
                    table.Columns.Add("Req. Number", typeof(string));
                    table.Columns.Add("UID", typeof(string));
                    foreach (DataRow r in results.Rows)
                    {
                        table.Rows.Add(DataConsistency.NoNull(r[SibiRequestCols.RequestNumber]), AttribIndexFunctions.GetDisplayValueFromCode(GlobalInstances.SibiAttribute.StatusType, r[SibiRequestCols.Status].ToString()), DataConsistency.NoNull(r[SibiRequestCols.Description]), DataConsistency.NoNull(r[SibiRequestCols.RequestUser]), AttribIndexFunctions.GetDisplayValueFromCode(GlobalInstances.SibiAttribute.RequestType, r[SibiRequestCols.Type].ToString()), DataConsistency.NoNull(r[SibiRequestCols.NeedBy]), DataConsistency.NoNull(r[SibiRequestCols.PO]), DataConsistency.NoNull(r[SibiRequestCols.RequisitionNumber]), DataConsistency.NoNull(r[SibiRequestCols.UID]));
                    }
                    ResultGrid.DataSource = table;
                    ResultGrid.ClearSelection();
                }
            }
            catch (Exception ex)
            {
                ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
            }
        }

        private void ShowAll()
        {
            SendToGrid(AssetManager.DBFactory.GetDatabase().DataTableFromQueryString("SELECT * FROM " + SibiRequestCols.TableName + " ORDER BY " + SibiRequestCols.NeedBy));
        }

        private void ResultGrid_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            SelectedUID = ResultGrid[GridFunctions.GetColIndex(ResultGrid, "UID"), ResultGrid.CurrentRow.Index].Value.ToString();
            this.DialogResult = DialogResult.OK;
        }

    }
}
