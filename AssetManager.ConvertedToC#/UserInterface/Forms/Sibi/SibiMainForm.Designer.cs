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
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class SibiMainForm : ExtendedForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SibiMainForm));
			this.Panel1 = new System.Windows.Forms.Panel();
			this.GroupBox2 = new System.Windows.Forms.GroupBox();
			this.txtRTNum = new System.Windows.Forms.TextBox();
			this.txtReq = new System.Windows.Forms.TextBox();
			this.txtPO = new System.Windows.Forms.TextBox();
			this.Label5 = new System.Windows.Forms.Label();
			this.Label4 = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.Label3 = new System.Windows.Forms.Label();
			this.cmdShowAll = new System.Windows.Forms.Button();
			this.Label1 = new System.Windows.Forms.Label();
			this.cmbDisplayYear = new System.Windows.Forms.ComboBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.ResultGrid = new System.Windows.Forms.DataGridView();
			this.ToolStrip1 = new AssetManager.OneClickToolStrip();
			this.cmdManage = new System.Windows.Forms.ToolStripButton();
			this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.Panel1.SuspendLayout();
			this.GroupBox2.SuspendLayout();
			this.GroupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.ResultGrid).BeginInit();
			this.ToolStrip1.SuspendLayout();
			this.SuspendLayout();
			//
			//Panel1
			//
			this.Panel1.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.Panel1.Controls.Add(this.GroupBox2);
			this.Panel1.Controls.Add(this.GroupBox1);
			this.Panel1.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Panel1.Location = new System.Drawing.Point(3, 28);
			this.Panel1.Name = "Panel1";
			this.Panel1.Size = new System.Drawing.Size(1179, 596);
			this.Panel1.TabIndex = 0;
			//
			//GroupBox2
			//
			this.GroupBox2.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.GroupBox2.Controls.Add(this.txtRTNum);
			this.GroupBox2.Controls.Add(this.txtReq);
			this.GroupBox2.Controls.Add(this.txtPO);
			this.GroupBox2.Controls.Add(this.Label5);
			this.GroupBox2.Controls.Add(this.Label4);
			this.GroupBox2.Controls.Add(this.txtDescription);
			this.GroupBox2.Controls.Add(this.Label3);
			this.GroupBox2.Controls.Add(this.cmdShowAll);
			this.GroupBox2.Controls.Add(this.Label1);
			this.GroupBox2.Controls.Add(this.cmbDisplayYear);
			this.GroupBox2.Controls.Add(this.Label2);
			this.GroupBox2.Location = new System.Drawing.Point(6, 16);
			this.GroupBox2.Name = "GroupBox2";
			this.GroupBox2.Size = new System.Drawing.Size(1170, 59);
			this.GroupBox2.TabIndex = 22;
			this.GroupBox2.TabStop = false;
			this.GroupBox2.Text = "Filters:";
			//
			//txtRTNum
			//
			this.txtRTNum.Location = new System.Drawing.Point(844, 22);
			this.txtRTNum.Name = "txtRTNum";
			this.txtRTNum.Size = new System.Drawing.Size(77, 23);
			this.txtRTNum.TabIndex = 27;
			//
			//txtReq
			//
			this.txtReq.Location = new System.Drawing.Point(713, 22);
			this.txtReq.Name = "txtReq";
			this.txtReq.Size = new System.Drawing.Size(77, 23);
			this.txtReq.TabIndex = 23;
			//
			//txtPO
			//
			this.txtPO.Location = new System.Drawing.Point(546, 22);
			this.txtPO.Name = "txtPO";
			this.txtPO.Size = new System.Drawing.Size(121, 23);
			this.txtPO.TabIndex = 21;
			//
			//Label5
			//
			this.Label5.AutoSize = true;
			this.Label5.Location = new System.Drawing.Point(918, 25);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(42, 15);
			this.Label5.TabIndex = 28;
			this.Label5.Text = ":RT #";
			//
			//Label4
			//
			this.Label4.AutoSize = true;
			this.Label4.Location = new System.Drawing.Point(449, 25);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(91, 15);
			this.Label4.TabIndex = 26;
			this.Label4.Text = ":Description";
			//
			//txtDescription
			//
			this.txtDescription.Location = new System.Drawing.Point(265, 22);
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.Size = new System.Drawing.Size(184, 23);
			this.txtDescription.TabIndex = 25;
			//
			//Label3
			//
			this.Label3.AutoSize = true;
			this.Label3.Location = new System.Drawing.Point(789, 25);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(49, 15);
			this.Label3.TabIndex = 24;
			this.Label3.Text = ":Req #";
			//
			//cmdShowAll
			//
			this.cmdShowAll.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cmdShowAll.Location = new System.Drawing.Point(1050, 17);
			this.cmdShowAll.Name = "cmdShowAll";
			this.cmdShowAll.Size = new System.Drawing.Size(111, 30);
			this.cmdShowAll.TabIndex = 1;
			this.cmdShowAll.Text = "Refresh/Reset";
			this.cmdShowAll.UseVisualStyleBackColor = true;
			//
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.Location = new System.Drawing.Point(665, 25);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(42, 15);
			this.Label1.TabIndex = 22;
			this.Label1.Text = ":PO #";
			//
			//cmbDisplayYear
			//
			this.cmbDisplayYear.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmbDisplayYear.FormattingEnabled = true;
			this.cmbDisplayYear.Location = new System.Drawing.Point(16, 22);
			this.cmbDisplayYear.Name = "cmbDisplayYear";
			this.cmbDisplayYear.Size = new System.Drawing.Size(115, 23);
			this.cmbDisplayYear.TabIndex = 19;
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.Label2.Location = new System.Drawing.Point(137, 25);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(91, 15);
			this.Label2.TabIndex = 20;
			this.Label2.Text = "Display Year";
			//
			//GroupBox1
			//
			this.GroupBox1.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.GroupBox1.Controls.Add(this.ResultGrid);
			this.GroupBox1.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.GroupBox1.Location = new System.Drawing.Point(6, 81);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(1170, 512);
			this.GroupBox1.TabIndex = 21;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "Requests:";
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
			this.ResultGrid.Location = new System.Drawing.Point(6, 22);
			this.ResultGrid.MultiSelect = false;
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
			this.ResultGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.ResultGrid.ShowCellErrors = false;
			this.ResultGrid.ShowCellToolTips = false;
			this.ResultGrid.ShowEditingIcon = false;
			this.ResultGrid.Size = new System.Drawing.Size(1158, 484);
			this.ResultGrid.TabIndex = 18;
			this.ResultGrid.VirtualMode = true;
			//
			//ToolStrip1
			//
			this.ToolStrip1.AutoSize = false;
			this.ToolStrip1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(148)), Convert.ToInt32(Convert.ToByte(213)), Convert.ToInt32(Convert.ToByte(255)));
			this.ToolStrip1.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.ToolStrip1.ImageScalingSize = new System.Drawing.Size(25, 25);
			this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.cmdManage,
				this.ToolStripSeparator1
			});
			this.ToolStrip1.Location = new System.Drawing.Point(0, 0);
			this.ToolStrip1.Name = "ToolStrip1";
			this.ToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.ToolStrip1.Size = new System.Drawing.Size(1188, 37);
			this.ToolStrip1.Stretch = true;
			this.ToolStrip1.TabIndex = 2;
			this.ToolStrip1.Text = "ToolStrip1";
			//
			//cmdManage
			//
			this.cmdManage.Image = Properties.Resources.AddIcon;
			this.cmdManage.Name = "cmdManage";
			this.cmdManage.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.cmdManage.Size = new System.Drawing.Size(141, 34);
			this.cmdManage.Text = "New Request";
			this.cmdManage.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
			//
			//ToolStripSeparator1
			//
			this.ToolStripSeparator1.Name = "ToolStripSeparator1";
			this.ToolStripSeparator1.Size = new System.Drawing.Size(6, 37);
			//
			//frmSibiMain
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(1188, 636);
			this.Controls.Add(this.ToolStrip1);
			this.Controls.Add(this.Panel1);
			this.Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
			this.MinimumSize = new System.Drawing.Size(1121, 371);
			this.Name = "frmSibiMain";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Sibi Acquisition Manager - Main";
			this.Panel1.ResumeLayout(false);
			this.GroupBox2.ResumeLayout(false);
			this.GroupBox2.PerformLayout();
			this.GroupBox1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.ResultGrid).EndInit();
			this.ToolStrip1.ResumeLayout(false);
			this.ToolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}
		internal Panel Panel1;
		private DataGridView withEventsField_ResultGrid;
		internal DataGridView ResultGrid {
			get { return withEventsField_ResultGrid; }
			set {
				if (withEventsField_ResultGrid != null) {
					withEventsField_ResultGrid.CellDoubleClick -= ResultGrid_CellDoubleClick;
					withEventsField_ResultGrid.RowPostPaint -= ResultGrid_RowPostPaint;
					withEventsField_ResultGrid.CellEnter -= ResultGrid_CellEnter;
					withEventsField_ResultGrid.CellLeave -= ResultGrid_CellLeave;
				}
				withEventsField_ResultGrid = value;
				if (withEventsField_ResultGrid != null) {
					withEventsField_ResultGrid.CellDoubleClick += ResultGrid_CellDoubleClick;
					withEventsField_ResultGrid.RowPostPaint += ResultGrid_RowPostPaint;
					withEventsField_ResultGrid.CellEnter += ResultGrid_CellEnter;
					withEventsField_ResultGrid.CellLeave += ResultGrid_CellLeave;
				}
			}
		}
		private Button withEventsField_cmdShowAll;
		internal Button cmdShowAll {
			get { return withEventsField_cmdShowAll; }
			set {
				if (withEventsField_cmdShowAll != null) {
					withEventsField_cmdShowAll.Click -= cmdShowAll_Click;
				}
				withEventsField_cmdShowAll = value;
				if (withEventsField_cmdShowAll != null) {
					withEventsField_cmdShowAll.Click += cmdShowAll_Click;
				}
			}
		}
		internal OneClickToolStrip ToolStrip1;
		private ToolStripButton withEventsField_cmdManage;
		internal ToolStripButton cmdManage {
			get { return withEventsField_cmdManage; }
			set {
				if (withEventsField_cmdManage != null) {
					withEventsField_cmdManage.Click -= cmdManage_Click;
				}
				withEventsField_cmdManage = value;
				if (withEventsField_cmdManage != null) {
					withEventsField_cmdManage.Click += cmdManage_Click;
				}
			}
		}
		internal Label Label2;
		private ComboBox withEventsField_cmbDisplayYear;
		internal ComboBox cmbDisplayYear {
			get { return withEventsField_cmbDisplayYear; }
			set {
				if (withEventsField_cmbDisplayYear != null) {
					withEventsField_cmbDisplayYear.SelectedIndexChanged -= cmbDisplayYear_SelectedIndexChanged;
				}
				withEventsField_cmbDisplayYear = value;
				if (withEventsField_cmbDisplayYear != null) {
					withEventsField_cmbDisplayYear.SelectedIndexChanged += cmbDisplayYear_SelectedIndexChanged;
				}
			}
		}
		internal ToolStripSeparator ToolStripSeparator1;
		internal GroupBox GroupBox1;
		internal GroupBox GroupBox2;
		internal Label Label3;
		private TextBox withEventsField_txtReq;
		internal TextBox txtReq {
			get { return withEventsField_txtReq; }
			set {
				if (withEventsField_txtReq != null) {
					withEventsField_txtReq.TextChanged -= txtReq_TextChanged;
				}
				withEventsField_txtReq = value;
				if (withEventsField_txtReq != null) {
					withEventsField_txtReq.TextChanged += txtReq_TextChanged;
				}
			}
		}
		internal Label Label1;
		private TextBox withEventsField_txtPO;
		internal TextBox txtPO {
			get { return withEventsField_txtPO; }
			set {
				if (withEventsField_txtPO != null) {
					withEventsField_txtPO.TextChanged -= txtPO_TextChanged;
				}
				withEventsField_txtPO = value;
				if (withEventsField_txtPO != null) {
					withEventsField_txtPO.TextChanged += txtPO_TextChanged;
				}
			}
		}
		internal Label Label4;
		private TextBox withEventsField_txtDescription;
		internal TextBox txtDescription {
			get { return withEventsField_txtDescription; }
			set {
				if (withEventsField_txtDescription != null) {
					withEventsField_txtDescription.TextChanged -= txtDescription_TextChanged;
				}
				withEventsField_txtDescription = value;
				if (withEventsField_txtDescription != null) {
					withEventsField_txtDescription.TextChanged += txtDescription_TextChanged;
				}
			}
		}
		internal Label Label5;
		private TextBox withEventsField_txtRTNum;
		internal TextBox txtRTNum {
			get { return withEventsField_txtRTNum; }
			set {
				if (withEventsField_txtRTNum != null) {
					withEventsField_txtRTNum.TextChanged -= txtRTNum_TextChanged;
				}
				withEventsField_txtRTNum = value;
				if (withEventsField_txtRTNum != null) {
					withEventsField_txtRTNum.TextChanged += txtRTNum_TextChanged;
				}
			}
		}
	}
}
