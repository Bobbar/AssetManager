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
namespace AssetManager.UserInterface.Forms.Sibi
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class SibiSelectorForm
	{
		//Form overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing && components != null) {
					components.Dispose();
				}
			} finally {
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
			System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.Panel1 = new System.Windows.Forms.Panel();
			this.Label1 = new System.Windows.Forms.Label();
			this.cmdShowAll = new System.Windows.Forms.Button();
			this.ResultGrid = new System.Windows.Forms.DataGridView();
			this.Panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.ResultGrid).BeginInit();
			this.SuspendLayout();
			//
			//Panel1
			//
			this.Panel1.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.Panel1.Controls.Add(this.Label1);
			this.Panel1.Controls.Add(this.cmdShowAll);
			this.Panel1.Controls.Add(this.ResultGrid);
			this.Panel1.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Panel1.Location = new System.Drawing.Point(12, 12);
			this.Panel1.Name = "Panel1";
			this.Panel1.Size = new System.Drawing.Size(1080, 415);
			this.Panel1.TabIndex = 1;
			//
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label1.Location = new System.Drawing.Point(3, 34);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(70, 15);
			this.Label1.TabIndex = 3;
			this.Label1.Text = "Requests:";
			//
			//cmdShowAll
			//
			this.cmdShowAll.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cmdShowAll.Location = new System.Drawing.Point(977, 4);
			this.cmdShowAll.Name = "cmdShowAll";
			this.cmdShowAll.Size = new System.Drawing.Size(100, 30);
			this.cmdShowAll.TabIndex = 1;
			this.cmdShowAll.Text = "Refresh";
			this.cmdShowAll.UseVisualStyleBackColor = true;
			//
			//ResultGrid
			//
			this.ResultGrid.AllowUserToAddRows = false;
			this.ResultGrid.AllowUserToDeleteRows = false;
			this.ResultGrid.AllowUserToResizeRows = false;
			this.ResultGrid.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.ResultGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.ResultGrid.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)));
			this.ResultGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.ResultGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this.ResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)));
			DataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
			DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(255)), Convert.ToInt32(Convert.ToByte(152)), Convert.ToInt32(Convert.ToByte(39)));
			DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
			DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.ResultGrid.DefaultCellStyle = DataGridViewCellStyle1;
			this.ResultGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.ResultGrid.Location = new System.Drawing.Point(3, 52);
			this.ResultGrid.Name = "ResultGrid";
			this.ResultGrid.ReadOnly = true;
			this.ResultGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)));
			DataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
			DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.ResultGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle2;
			this.ResultGrid.RowHeadersVisible = false;
			this.ResultGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.ResultGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.ResultGrid.ShowCellErrors = false;
			this.ResultGrid.ShowCellToolTips = false;
			this.ResultGrid.ShowEditingIcon = false;
			this.ResultGrid.Size = new System.Drawing.Size(1074, 360);
			this.ResultGrid.TabIndex = 18;
			this.ResultGrid.VirtualMode = true;
			//
			//frmSibiSelector
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1104, 439);
			this.Controls.Add(this.Panel1);
			this.Name = "frmSibiSelector";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
			this.Text = "Select Request";
			this.Panel1.ResumeLayout(false);
			this.Panel1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.ResultGrid).EndInit();
			this.ResumeLayout(false);

		}
		internal Panel Panel1;
		internal Label Label1;
		internal Button cmdShowAll;
		private DataGridView withEventsField_ResultGrid;
		internal DataGridView ResultGrid {
			get { return withEventsField_ResultGrid; }
			set {
				if (withEventsField_ResultGrid != null) {
					withEventsField_ResultGrid.CellDoubleClick -= ResultGrid_CellDoubleClick;
				}
				withEventsField_ResultGrid = value;
				if (withEventsField_ResultGrid != null) {
					withEventsField_ResultGrid.CellDoubleClick += ResultGrid_CellDoubleClick;
				}
			}
		}
	}
}