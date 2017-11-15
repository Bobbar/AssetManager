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
namespace AssetManager.UserInterface.Forms.AssetManagement
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    partial class MunisUserForm
    {

        //Form overrides dispose to clean up the component list.
        [System.Diagnostics.DebuggerNonUserCode()]
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
        [System.Diagnostics.DebuggerStepThrough()]
        private void InitializeComponent()
        {
            this.MunisResults = new System.Windows.Forms.DataGridView();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.lblSelectedEmp = new System.Windows.Forms.Label();
            this.cmdAccept = new System.Windows.Forms.Button();
            this.pnlSearch = new System.Windows.Forms.GroupBox();
            this.pbWorking = new System.Windows.Forms.PictureBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.cmdSearch = new System.Windows.Forms.Button();
            this.txtSearchName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.MunisResults)).BeginInit();
            this.GroupBox1.SuspendLayout();
            this.pnlSearch.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWorking)).BeginInit();
            this.SuspendLayout();
            // 
            // MunisResults
            // 
            this.MunisResults.AllowUserToAddRows = false;
            this.MunisResults.AllowUserToDeleteRows = false;
            this.MunisResults.AllowUserToResizeRows = false;
            this.MunisResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MunisResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells;
            this.MunisResults.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.MunisResults.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.MunisResults.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.MunisResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.MunisResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.MunisResults.EnableHeadersVisualStyles = false;
            this.MunisResults.Location = new System.Drawing.Point(6, 19);
            this.MunisResults.Name = "MunisResults";
            this.MunisResults.ReadOnly = true;
            this.MunisResults.RowHeadersVisible = false;
            this.MunisResults.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.MunisResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.MunisResults.ShowCellToolTips = false;
            this.MunisResults.ShowEditingIcon = false;
            this.MunisResults.Size = new System.Drawing.Size(672, 240);
            this.MunisResults.TabIndex = 42;
            this.MunisResults.VirtualMode = true;
            this.MunisResults.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MunisResults_CellClick);
            this.MunisResults.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.MunisResults_CellDoubleClick);
            // 
            // GroupBox1
            // 
            this.GroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox1.Controls.Add(this.lblSelectedEmp);
            this.GroupBox1.Controls.Add(this.cmdAccept);
            this.GroupBox1.Controls.Add(this.MunisResults);
            this.GroupBox1.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox1.Location = new System.Drawing.Point(12, 103);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(686, 360);
            this.GroupBox1.TabIndex = 43;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Results";
            // 
            // lblSelectedEmp
            // 
            this.lblSelectedEmp.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSelectedEmp.Location = new System.Drawing.Point(6, 277);
            this.lblSelectedEmp.Name = "lblSelectedEmp";
            this.lblSelectedEmp.Size = new System.Drawing.Size(672, 18);
            this.lblSelectedEmp.TabIndex = 44;
            this.lblSelectedEmp.Text = "Selected Emp:";
            this.lblSelectedEmp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmdAccept
            // 
            this.cmdAccept.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.cmdAccept.Location = new System.Drawing.Point(287, 310);
            this.cmdAccept.Name = "cmdAccept";
            this.cmdAccept.Size = new System.Drawing.Size(92, 29);
            this.cmdAccept.TabIndex = 43;
            this.cmdAccept.Text = "Accept";
            this.cmdAccept.UseVisualStyleBackColor = true;
            this.cmdAccept.Click += new System.EventHandler(this.cmdAccept_Click);
            // 
            // pnlSearch
            // 
            this.pnlSearch.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlSearch.Controls.Add(this.pbWorking);
            this.pnlSearch.Controls.Add(this.Label1);
            this.pnlSearch.Controls.Add(this.cmdSearch);
            this.pnlSearch.Controls.Add(this.txtSearchName);
            this.pnlSearch.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.pnlSearch.Location = new System.Drawing.Point(12, 12);
            this.pnlSearch.Name = "pnlSearch";
            this.pnlSearch.Size = new System.Drawing.Size(686, 85);
            this.pnlSearch.TabIndex = 44;
            this.pnlSearch.TabStop = false;
            this.pnlSearch.Text = "Search";
            // 
            // pbWorking
            // 
            this.pbWorking.Image = global::AssetManager.Properties.Resources.LoadingAni;
            this.pbWorking.Location = new System.Drawing.Point(178, 48);
            this.pbWorking.Name = "pbWorking";
            this.pbWorking.Size = new System.Drawing.Size(22, 22);
            this.pbWorking.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pbWorking.TabIndex = 0;
            this.pbWorking.TabStop = false;
            this.pbWorking.Visible = false;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(15, 32);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(140, 14);
            this.Label1.TabIndex = 2;
            this.Label1.Text = "First or Last Name:";
            // 
            // cmdSearch
            // 
            this.cmdSearch.Location = new System.Drawing.Point(213, 45);
            this.cmdSearch.Name = "cmdSearch";
            this.cmdSearch.Size = new System.Drawing.Size(131, 26);
            this.cmdSearch.TabIndex = 1;
            this.cmdSearch.Text = "Search";
            this.cmdSearch.UseVisualStyleBackColor = true;
            this.cmdSearch.Click += new System.EventHandler(this.cmdSearch_Click);
            // 
            // txtSearchName
            // 
            this.txtSearchName.Location = new System.Drawing.Point(18, 48);
            this.txtSearchName.Name = "txtSearchName";
            this.txtSearchName.Size = new System.Drawing.Size(157, 22);
            this.txtSearchName.TabIndex = 0;
            // 
            // MunisUserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(713, 477);
            this.Controls.Add(this.pnlSearch);
            this.Controls.Add(this.GroupBox1);
            this.MinimumSize = new System.Drawing.Size(391, 441);
            this.Name = "MunisUserForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Employee Search";
            this.Load += new System.EventHandler(this.MunisUser_Load);
            this.Shown += new System.EventHandler(this.MunisUserForm_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.MunisResults)).EndInit();
            this.GroupBox1.ResumeLayout(false);
            this.pnlSearch.ResumeLayout(false);
            this.pnlSearch.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pbWorking)).EndInit();
            this.ResumeLayout(false);

        }

        internal DataGridView MunisResults;
        internal GroupBox GroupBox1;
        internal Button cmdAccept;
        internal GroupBox pnlSearch;
        internal Label Label1;
        internal TextBox txtSearchName;
        internal Button cmdSearch;
        internal Label lblSelectedEmp;
        internal PictureBox pbWorking;
    }
}
