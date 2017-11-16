using System.Windows.Forms;
namespace AssetManager.UserInterface.Forms.AdminTools
{

    partial class UserManagerForm
    {
        //Form overrides dispose to clean up the component list.
        
        protected override void Dispose(bool disposing)
        {
            try
            {
                if (disposing && components != null)
                {
                    components.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
        //Required by the Windows Form Designer
        private System.ComponentModel.IContainer components;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle7 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle8 = new System.Windows.Forms.DataGridViewCellStyle();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.UserGrid = new System.Windows.Forms.DataGridView();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.pnlModule = new System.Windows.Forms.Panel();
            this.clbModules = new System.Windows.Forms.CheckedListBox();
            this.lblAccessValue = new System.Windows.Forms.Label();
            this.cmdUpdate = new System.Windows.Forms.Button();
            this.cmdRefresh = new System.Windows.Forms.Button();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.Panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.UserGrid)).BeginInit();
            this.GroupBox1.SuspendLayout();
            this.pnlModule.SuspendLayout();
            this.Panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel1
            // 
            this.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel1.Controls.Add(this.UserGrid);
            this.Panel1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Panel1.Location = new System.Drawing.Point(12, 222);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(647, 219);
            this.Panel1.TabIndex = 0;
            // 
            // UserGrid
            // 
            this.UserGrid.AllowUserToResizeRows = false;
            this.UserGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.UserGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.UserGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.UserGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.UserGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.UserGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle7.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle7.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.UserGrid.DefaultCellStyle = dataGridViewCellStyle7;
            this.UserGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.UserGrid.Location = new System.Drawing.Point(3, 3);
            this.UserGrid.MultiSelect = false;
            this.UserGrid.Name = "UserGrid";
            this.UserGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle8.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle8.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.UserGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle8;
            this.UserGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.UserGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.UserGrid.ShowCellErrors = false;
            this.UserGrid.ShowCellToolTips = false;
            this.UserGrid.Size = new System.Drawing.Size(641, 213);
            this.UserGrid.TabIndex = 18;
            this.UserGrid.VirtualMode = true;
            this.UserGrid.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.UserGrid_CellClick);
            this.UserGrid.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.UserGrid_CellDoubleClick);
            this.UserGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.UserGrid_KeyDown);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.pnlModule);
            this.GroupBox1.Location = new System.Drawing.Point(15, 12);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(433, 204);
            this.GroupBox1.TabIndex = 1;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Module Access";
            // 
            // pnlModule
            // 
            this.pnlModule.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlModule.Controls.Add(this.clbModules);
            this.pnlModule.Controls.Add(this.lblAccessValue);
            this.pnlModule.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlModule.Location = new System.Drawing.Point(10, 17);
            this.pnlModule.Name = "pnlModule";
            this.pnlModule.Size = new System.Drawing.Size(412, 181);
            this.pnlModule.TabIndex = 0;
            // 
            // clbModules
            // 
            this.clbModules.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.clbModules.CheckOnClick = true;
            this.clbModules.FormattingEnabled = true;
            this.clbModules.Location = new System.Drawing.Point(3, 3);
            this.clbModules.MultiColumn = true;
            this.clbModules.Name = "clbModules";
            this.clbModules.Size = new System.Drawing.Size(406, 157);
            this.clbModules.Sorted = true;
            this.clbModules.TabIndex = 0;
            this.clbModules.MouseUp += new System.Windows.Forms.MouseEventHandler(this.clbModules_MouseUp);
            // 
            // lblAccessValue
            // 
            this.lblAccessValue.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.lblAccessValue.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAccessValue.Location = new System.Drawing.Point(28, 165);
            this.lblAccessValue.Name = "lblAccessValue";
            this.lblAccessValue.Size = new System.Drawing.Size(356, 16);
            this.lblAccessValue.TabIndex = 3;
            this.lblAccessValue.Text = "Selected Access Value:";
            this.lblAccessValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdUpdate
            // 
            this.cmdUpdate.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmdUpdate.Location = new System.Drawing.Point(37, 28);
            this.cmdUpdate.Name = "cmdUpdate";
            this.cmdUpdate.Size = new System.Drawing.Size(134, 47);
            this.cmdUpdate.TabIndex = 2;
            this.cmdUpdate.Text = "Commit Changes";
            this.cmdUpdate.UseVisualStyleBackColor = true;
            this.cmdUpdate.Click += new System.EventHandler(this.cmdUpdate_Click);
            // 
            // cmdRefresh
            // 
            this.cmdRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top;
            this.cmdRefresh.Location = new System.Drawing.Point(37, 106);
            this.cmdRefresh.Name = "cmdRefresh";
            this.cmdRefresh.Size = new System.Drawing.Size(134, 23);
            this.cmdRefresh.TabIndex = 4;
            this.cmdRefresh.Text = "Refresh/Undo";
            this.cmdRefresh.UseVisualStyleBackColor = true;
            this.cmdRefresh.Click += new System.EventHandler(this.cmdRefresh_Click);
            // 
            // Panel2
            // 
            this.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel2.Controls.Add(this.cmdUpdate);
            this.Panel2.Controls.Add(this.cmdRefresh);
            this.Panel2.Location = new System.Drawing.Point(454, 40);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(205, 162);
            this.Panel2.TabIndex = 5;
            // 
            // UserManagerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 453);
            this.Controls.Add(this.Panel2);
            this.Controls.Add(this.GroupBox1);
            this.Controls.Add(this.Panel1);
            this.MinimumSize = new System.Drawing.Size(634, 409);
            this.Name = "UserManagerForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "User  Manager";
            this.Panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.UserGrid)).EndInit();
            this.GroupBox1.ResumeLayout(false);
            this.pnlModule.ResumeLayout(false);
            this.Panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }
        internal Panel Panel1;
        internal DataGridView UserGrid;
        internal GroupBox GroupBox1;
        internal Panel pnlModule;
        internal CheckedListBox clbModules;
        internal Button cmdUpdate;
        internal Label lblAccessValue;
        internal Button cmdRefresh;
        internal Panel Panel2;
    }
}
