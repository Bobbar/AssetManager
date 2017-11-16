using AssetManager.UserInterface.CustomControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Windows.Forms;

namespace AssetManager.UserInterface.Forms
{
    public partial class GridForm : ExtendedForm
    {
        #region "Fields"

        private bool bolGridFilling = true;
        private List<DataGridView> GridList = new List<DataGridView>();

        private DataGridViewRow LastDoubleClickRow;

        #endregion "Fields"

        #region "Constructors"

        public GridForm(ExtendedForm parentForm, string title = "")
        {
            Load += GridForm_Load;
            Disposed += GridForm_Disposed;
            Resize += GridForm_Resize;
            Closing += GridForm_Closing;
            this.ParentForm = parentForm;
            // This call is required by the designer.
            InitializeComponent();
            if (!string.IsNullOrEmpty(title))
                this.Text = title;
            // Add any initialization after the InitializeComponent() call.
            ExtendedMethods.DoubleBufferedTableLayout(GridPanel, true);
            ExtendedMethods.DoubleBufferedPanel(Panel1, true);
            GridPanel.RowStyles.Clear();
        }

        #endregion "Constructors"

        #region "Properties"

        public int GridCount
        {
            get { return GridList.Count; }
        }

        public DataGridViewRow SelectedValue
        {
            get { return LastDoubleClickRow; }
        }

        #endregion "Properties"

        #region "Methods"

        public void AddGrid(string name, string label, DataTable datatable)
        {
            var NewGrid = GetNewGrid(name, label + " (" + datatable.Rows.Count.ToString() + " rows)");
            FillGrid(NewGrid, datatable);
            GridList.Add(NewGrid);
        }

        private void CopySelectedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GridFunctions.CopySelectedGridData(GetActiveGrid());
        }

        private void AddGridsToForm()
        {
            this.SuspendLayout();
            Panel1.SuspendLayout();
            GridPanel.SuspendLayout();
            foreach (DataGridView grid in GridList)
            {
                GroupBox GridBox = new GroupBox();
                GridBox.Text = (string)grid.Tag;
                GridBox.Dock = DockStyle.Fill;
                GridBox.Controls.Add(grid);
                GridPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, GridHeight()));
                GridPanel.Controls.Add(GridBox);
            }
            this.ResumeLayout();
            Panel1.ResumeLayout();
            GridPanel.ResumeLayout();
        }

        private void FillGrid(DataGridView grid, DataTable datatable)
        {
            if (datatable != null)
                grid.DataSource = datatable;
        }

        private DataGridView GetActiveGrid()
        {
            if (this.ActiveControl is DataGridView)
            {
                return (DataGridView)this.ActiveControl;
            }
            return null;
        }

        private DataGridView GetNewGrid(string name, string label)
        {
            DataGridView NewGrid = new DataGridView();
            NewGrid.Name = name;
            NewGrid.Tag = label;
            NewGrid.Dock = DockStyle.Fill;
            NewGrid.RowHeadersVisible = false;
            NewGrid.EditMode = DataGridViewEditMode.EditProgrammatically;
            NewGrid.AllowUserToResizeRows = false;
            NewGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            NewGrid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.AllCells;
            NewGrid.AllowUserToAddRows = false;
            NewGrid.AllowUserToDeleteRows = false;
            NewGrid.Padding = new Padding(0, 0, 0, 10);
            NewGrid.ContextMenuStrip = PopUpMenu;
            StyleFunctions.SetGridStyle(NewGrid, ParentForm.GridTheme);
            NewGrid.CellLeave += GridLeaveCell;
            NewGrid.CellEnter += GridEnterCell;
            NewGrid.CellDoubleClick += GridDoubleClickCell;
            ExtendedMethods.DoubleBufferedDataGrid(NewGrid, true);
            return NewGrid;
        }

        private void GridDoubleClickCell(object sender, EventArgs e)
        {
            DataGridView SenderGrid = (DataGridView)sender;
            LastDoubleClickRow = SenderGrid.CurrentRow;
            this.DialogResult = DialogResult.OK;
        }

        private void GridEnterCell(object sender, DataGridViewCellEventArgs e)
        {
            if (!bolGridFilling)
            {
                StyleFunctions.HighlightRow((DataGridView)sender, this.GridTheme, e.RowIndex);
            }
        }

        private void GridForm_Closing(object sender, CancelEventArgs e)
        {
            if (!Modal)
                this.Dispose();
        }

        private void GridForm_Resize(object sender, EventArgs e)
        {
            if (!bolGridFilling)
                ResizeGridPanel();
        }

        private int GridHeight()
        {
            int MinHeight = 200;
            int CalcHeight = Convert.ToInt32((this.ClientSize.Height - 30) / GridList.Count);
            if (CalcHeight < MinHeight)
            {
                return MinHeight;
            }
            else
            {
                return CalcHeight;
            }
        }

        private void GridLeaveCell(object sender, DataGridViewCellEventArgs e)
        {
            StyleFunctions.LeaveRow((DataGridView)sender, this.GridTheme, e.RowIndex);
        }

        private void ResizeGridPanel()
        {
            var NewHeight = GridHeight();
            foreach (DataGridView grid in GridList)
            {
                var row = GridList.IndexOf(grid);
                GridPanel.RowStyles[row].Height = NewHeight;
            }
        }

        private void ResizeGrids()
        {
            foreach (DataGridView grid in GridList)
            {
                foreach (DataGridViewColumn c in grid.Columns)
                {
                    c.Width = c.GetPreferredWidth(DataGridViewAutoSizeColumnMode.AllCells, true);
                }
                grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
                grid.AllowUserToResizeColumns = true;
            }
        }

        private void SendToNewGridForm_Click(object sender, EventArgs e)
        {
            GridFunctions.CopyToGridForm(GetActiveGrid(), ParentForm);
        }

        private void GridForm_Disposed(object sender, EventArgs e)
        {
            foreach (DataGridView grid in GridList)
            {
                ((DataTable)grid.DataSource).Dispose();
                grid.Dispose();
            }
            if (LastDoubleClickRow != null)
                LastDoubleClickRow.Dispose();
        }

        private void GridForm_Load(object sender, EventArgs e)
        {
            AddGridsToForm();
            ResizeGrids();
            bolGridFilling = false;
        }

        #endregion "Methods"
    }
}