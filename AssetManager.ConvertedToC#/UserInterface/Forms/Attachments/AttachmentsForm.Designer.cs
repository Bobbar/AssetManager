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

	partial class AttachmentsForm : ExtendedForm
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AttachmentsForm));
			System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
			this.cmdUpload = new System.Windows.Forms.Button();
			this.RightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.OpenTool = new System.Windows.Forms.ToolStripMenuItem();
			this.SaveToMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.CopyTextTool = new System.Windows.Forms.ToolStripMenuItem();
			this.NewFolderMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.RenameStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.DeleteAttachmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.cmdDelete = new System.Windows.Forms.Button();
			this.cmdOpen = new System.Windows.Forms.Button();
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.Panel1 = new System.Windows.Forms.Panel();
			this.AttachContainer = new System.Windows.Forms.SplitContainer();
			this.FolderListView = new System.Windows.Forms.ListView();
			this.ColumnHeader1 = (System.Windows.Forms.ColumnHeader)new System.Windows.Forms.ColumnHeader();
			this.imageList1 = new System.Windows.Forms.ImageList(this.components);
			this.AttachGrid = new System.Windows.Forms.DataGridView();
			this.AllowDragCheckBox = new System.Windows.Forms.CheckBox();
			this.SibiGroup = new System.Windows.Forms.GroupBox();
			this.Label4 = new System.Windows.Forms.Label();
			this.ReqNumberTextBox = new System.Windows.Forms.TextBox();
			this.Label3 = new System.Windows.Forms.Label();
			this.txtDescription = new System.Windows.Forms.TextBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.txtRequestNum = new System.Windows.Forms.TextBox();
			this.Label1 = new System.Windows.Forms.Label();
			this.ReqPO = new System.Windows.Forms.TextBox();
			this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
			this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.ProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
			this.ToolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
			this.Spinner = new System.Windows.Forms.ToolStripStatusLabel();
			this.statMBPS = new System.Windows.Forms.ToolStripStatusLabel();
			this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
			this.cmdCancel = new System.Windows.Forms.ToolStripDropDownButton();
			this.ProgTimer = new System.Windows.Forms.Timer(this.components);
			this.DeviceGroup = new System.Windows.Forms.GroupBox();
			this.Label5 = new System.Windows.Forms.Label();
			this.txtDeviceDescription = new System.Windows.Forms.TextBox();
			this.Label6 = new System.Windows.Forms.Label();
			this.txtSerial = new System.Windows.Forms.TextBox();
			this.Label7 = new System.Windows.Forms.Label();
			this.txtAssetTag = new System.Windows.Forms.TextBox();
			this.Panel2 = new System.Windows.Forms.Panel();
			this.RightClickMenu.SuspendLayout();
			this.GroupBox1.SuspendLayout();
			this.Panel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.AttachContainer).BeginInit();
			this.AttachContainer.Panel1.SuspendLayout();
			this.AttachContainer.Panel2.SuspendLayout();
			this.AttachContainer.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.AttachGrid).BeginInit();
			this.SibiGroup.SuspendLayout();
			this.StatusStrip1.SuspendLayout();
			this.DeviceGroup.SuspendLayout();
			this.Panel2.SuspendLayout();
			this.SuspendLayout();
			//
			//cmdUpload
			//
			this.cmdUpload.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdUpload.Location = new System.Drawing.Point(29, 46);
			this.cmdUpload.Name = "cmdUpload";
			this.cmdUpload.Size = new System.Drawing.Size(92, 46);
			this.cmdUpload.TabIndex = 0;
			this.cmdUpload.Text = "Upload";
			this.cmdUpload.UseVisualStyleBackColor = true;
			//
			//RightClickMenu
			//
			this.RightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.OpenTool,
				this.SaveToMenuItem,
				this.CopyTextTool,
				this.NewFolderMenuItem,
				this.RenameStripMenuItem,
				this.ToolStripSeparator1,
				this.DeleteAttachmentToolStripMenuItem
			});
			this.RightClickMenu.Name = "RightClickMenu";
			this.RightClickMenu.Size = new System.Drawing.Size(178, 142);
			//
			//OpenTool
			//
			this.OpenTool.Name = "OpenTool";
			this.OpenTool.Size = new System.Drawing.Size(177, 22);
			this.OpenTool.Text = "Open";
			//
			//SaveToMenuItem
			//
			this.SaveToMenuItem.Name = "SaveToMenuItem";
			this.SaveToMenuItem.Size = new System.Drawing.Size(177, 22);
			this.SaveToMenuItem.Text = "Save To";
			//
			//CopyTextTool
			//
			this.CopyTextTool.Name = "CopyTextTool";
			this.CopyTextTool.Size = new System.Drawing.Size(177, 22);
			this.CopyTextTool.Text = "Copy Text";
			//
			//NewFolderMenuItem
			//
			this.NewFolderMenuItem.Name = "NewFolderMenuItem";
			this.NewFolderMenuItem.Size = new System.Drawing.Size(177, 22);
			this.NewFolderMenuItem.Text = "Move to new folder";
			//
			//RenameStripMenuItem
			//
			this.RenameStripMenuItem.Name = "RenameStripMenuItem";
			this.RenameStripMenuItem.Size = new System.Drawing.Size(177, 22);
			this.RenameStripMenuItem.Text = "Rename";
			//
			//ToolStripSeparator1
			//
			this.ToolStripSeparator1.Name = "ToolStripSeparator1";
			this.ToolStripSeparator1.Size = new System.Drawing.Size(174, 6);
			//
			//DeleteAttachmentToolStripMenuItem
			//
			this.DeleteAttachmentToolStripMenuItem.Image = Properties.Resources.DeleteRedIcon;
			this.DeleteAttachmentToolStripMenuItem.Name = "DeleteAttachmentToolStripMenuItem";
			this.DeleteAttachmentToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
			this.DeleteAttachmentToolStripMenuItem.Text = "Delete Attachment";
			//
			//cmdDelete
			//
			this.cmdDelete.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cmdDelete.Location = new System.Drawing.Point(29, 160);
			this.cmdDelete.Name = "cmdDelete";
			this.cmdDelete.Size = new System.Drawing.Size(92, 25);
			this.cmdDelete.TabIndex = 4;
			this.cmdDelete.Text = "Delete";
			this.cmdDelete.UseVisualStyleBackColor = true;
			//
			//cmdOpen
			//
			this.cmdOpen.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdOpen.Location = new System.Drawing.Point(29, 98);
			this.cmdOpen.Name = "cmdOpen";
			this.cmdOpen.Size = new System.Drawing.Size(92, 23);
			this.cmdOpen.TabIndex = 5;
			this.cmdOpen.Text = "Open";
			this.cmdOpen.UseVisualStyleBackColor = true;
			//
			//GroupBox1
			//
			this.GroupBox1.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.GroupBox1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.GroupBox1.Controls.Add(this.Panel1);
			this.GroupBox1.Controls.Add(this.cmdOpen);
			this.GroupBox1.Controls.Add(this.cmdDelete);
			this.GroupBox1.Controls.Add(this.cmdUpload);
			this.GroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.GroupBox1.Location = new System.Drawing.Point(12, 113);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(793, 440);
			this.GroupBox1.TabIndex = 6;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "Attachments";
			//
			//Panel1
			//
			this.Panel1.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.Panel1.Controls.Add(this.AttachContainer);
			this.Panel1.Controls.Add(this.AllowDragCheckBox);
			this.Panel1.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Panel1.Location = new System.Drawing.Point(150, 21);
			this.Panel1.Name = "Panel1";
			this.Panel1.Size = new System.Drawing.Size(637, 413);
			this.Panel1.TabIndex = 19;
			//
			//AttachContainer
			//
			this.AttachContainer.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.AttachContainer.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
			this.AttachContainer.Location = new System.Drawing.Point(3, 22);
			this.AttachContainer.Name = "AttachContainer";
			//
			//AttachContainer.Panel1
			//
			this.AttachContainer.Panel1.Controls.Add(this.FolderListView);
			//
			//AttachContainer.Panel2
			//
			this.AttachContainer.Panel2.Controls.Add(this.AttachGrid);
			this.AttachContainer.Size = new System.Drawing.Size(629, 386);
			this.AttachContainer.SplitterDistance = 122;
			this.AttachContainer.TabIndex = 23;
			//
			//FolderListView
			//
			this.FolderListView.AllowDrop = true;
			this.FolderListView.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)));
			this.FolderListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.FolderListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { this.ColumnHeader1 });
			this.FolderListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FolderListView.ForeColor = System.Drawing.Color.White;
			this.FolderListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.Nonclickable;
			this.FolderListView.Location = new System.Drawing.Point(0, 0);
			this.FolderListView.MultiSelect = false;
			this.FolderListView.Name = "FolderListView";
			this.FolderListView.Size = new System.Drawing.Size(122, 386);
			this.FolderListView.StateImageList = this.imageList1;
			this.FolderListView.TabIndex = 0;
			this.FolderListView.UseCompatibleStateImageBehavior = false;
			this.FolderListView.View = System.Windows.Forms.View.Details;
			//
			//ColumnHeader1
			//
			this.ColumnHeader1.Text = "Folder";
			this.ColumnHeader1.Width = 115;
			//
			//imageList1
			//
			this.imageList1.ImageStream = (System.Windows.Forms.ImageListStreamer)resources.GetObject("imageList1.ImageStream");
			this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
			this.imageList1.Images.SetKeyName(0, "icons8-Folder-48.png");
			this.imageList1.Images.SetKeyName(1, "icons8-Open-48.png");
			//
			//AttachGrid
			//
			this.AttachGrid.AllowDrop = true;
			this.AttachGrid.AllowUserToAddRows = false;
			this.AttachGrid.AllowUserToDeleteRows = false;
			this.AttachGrid.AllowUserToResizeRows = false;
			this.AttachGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.AttachGrid.BackgroundColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)));
			this.AttachGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
			this.AttachGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this.AttachGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.AttachGrid.ContextMenuStrip = this.RightClickMenu;
			DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)));
			DataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
			DataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(10, 0, 10, 0);
			DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(255)), Convert.ToInt32(Convert.ToByte(152)), Convert.ToInt32(Convert.ToByte(39)));
			DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
			DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.AttachGrid.DefaultCellStyle = DataGridViewCellStyle1;
			this.AttachGrid.Dock = System.Windows.Forms.DockStyle.Fill;
			this.AttachGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.AttachGrid.Location = new System.Drawing.Point(0, 0);
			this.AttachGrid.MultiSelect = false;
			this.AttachGrid.Name = "AttachGrid";
			this.AttachGrid.ReadOnly = true;
			this.AttachGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
			DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
			DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)), Convert.ToInt32(Convert.ToByte(64)));
			DataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
			DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
			this.AttachGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle2;
			this.AttachGrid.RowHeadersVisible = false;
			this.AttachGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
			this.AttachGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.AttachGrid.ShowCellErrors = false;
			this.AttachGrid.ShowCellToolTips = false;
			this.AttachGrid.ShowEditingIcon = false;
			this.AttachGrid.Size = new System.Drawing.Size(503, 386);
			this.AttachGrid.TabIndex = 18;
			this.AttachGrid.VirtualMode = true;
			//
			//AllowDragCheckBox
			//
			this.AllowDragCheckBox.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.AllowDragCheckBox.AutoSize = true;
			this.AllowDragCheckBox.Checked = true;
			this.AllowDragCheckBox.CheckState = System.Windows.Forms.CheckState.Checked;
			this.AllowDragCheckBox.Location = new System.Drawing.Point(543, 3);
			this.AllowDragCheckBox.Name = "AllowDragCheckBox";
			this.AllowDragCheckBox.Size = new System.Drawing.Size(89, 19);
			this.AllowDragCheckBox.TabIndex = 21;
			this.AllowDragCheckBox.Text = "Drag-Drop";
			this.AllowDragCheckBox.UseVisualStyleBackColor = true;
			//
			//SibiGroup
			//
			this.SibiGroup.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.SibiGroup.Controls.Add(this.Label4);
			this.SibiGroup.Controls.Add(this.ReqNumberTextBox);
			this.SibiGroup.Controls.Add(this.Label3);
			this.SibiGroup.Controls.Add(this.txtDescription);
			this.SibiGroup.Controls.Add(this.Label2);
			this.SibiGroup.Controls.Add(this.txtRequestNum);
			this.SibiGroup.Controls.Add(this.Label1);
			this.SibiGroup.Controls.Add(this.ReqPO);
			this.SibiGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.SibiGroup.Location = new System.Drawing.Point(0, 3);
			this.SibiGroup.Name = "SibiGroup";
			this.SibiGroup.Size = new System.Drawing.Size(793, 95);
			this.SibiGroup.TabIndex = 7;
			this.SibiGroup.TabStop = false;
			this.SibiGroup.Text = "Request Info";
			//
			//Label4
			//
			this.Label4.AutoSize = true;
			this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label4.Location = new System.Drawing.Point(598, 29);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(47, 16);
			this.Label4.TabIndex = 7;
			this.Label4.Text = "Req #:";
			//
			//ReqNumberTextBox
			//
			this.ReqNumberTextBox.Font = new System.Drawing.Font("Consolas", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.ReqNumberTextBox.Location = new System.Drawing.Point(601, 49);
			this.ReqNumberTextBox.Name = "ReqNumberTextBox";
			this.ReqNumberTextBox.ReadOnly = true;
			this.ReqNumberTextBox.Size = new System.Drawing.Size(120, 25);
			this.ReqNumberTextBox.TabIndex = 6;
			//
			//Label3
			//
			this.Label3.AutoSize = true;
			this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label3.Location = new System.Drawing.Point(20, 29);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(79, 16);
			this.Label3.TabIndex = 5;
			this.Label3.Text = "Description:";
			//
			//txtDescription
			//
			this.txtDescription.Font = new System.Drawing.Font("Consolas", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtDescription.Location = new System.Drawing.Point(23, 49);
			this.txtDescription.Name = "txtDescription";
			this.txtDescription.ReadOnly = true;
			this.txtDescription.Size = new System.Drawing.Size(259, 25);
			this.txtDescription.TabIndex = 4;
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label2.Location = new System.Drawing.Point(317, 29);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(72, 16);
			this.Label2.TabIndex = 3;
			this.Label2.Text = "Request #:";
			//
			//txtRequestNum
			//
			this.txtRequestNum.Font = new System.Drawing.Font("Consolas", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtRequestNum.Location = new System.Drawing.Point(320, 49);
			this.txtRequestNum.Name = "txtRequestNum";
			this.txtRequestNum.ReadOnly = true;
			this.txtRequestNum.Size = new System.Drawing.Size(83, 25);
			this.txtRequestNum.TabIndex = 2;
			//
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label1.Location = new System.Drawing.Point(439, 29);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(40, 16);
			this.Label1.TabIndex = 1;
			this.Label1.Text = "PO #:";
			//
			//ReqPO
			//
			this.ReqPO.Font = new System.Drawing.Font("Consolas", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.ReqPO.Location = new System.Drawing.Point(442, 49);
			this.ReqPO.Name = "ReqPO";
			this.ReqPO.ReadOnly = true;
			this.ReqPO.Size = new System.Drawing.Size(120, 25);
			this.ReqPO.TabIndex = 0;
			//
			//StatusStrip1
			//
			this.StatusStrip1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.StatusLabel,
				this.ProgressBar1,
				this.ToolStripStatusLabel2,
				this.Spinner,
				this.statMBPS,
				this.ToolStripStatusLabel1,
				this.cmdCancel
			});
			this.StatusStrip1.Location = new System.Drawing.Point(0, 556);
			this.StatusStrip1.Name = "StatusStrip1";
			this.StatusStrip1.Size = new System.Drawing.Size(817, 22);
			this.StatusStrip1.TabIndex = 8;
			this.StatusStrip1.Text = "StatusStrip1";
			//
			//StatusLabel
			//
			this.StatusLabel.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.StatusLabel.Name = "StatusLabel";
			this.StatusLabel.Padding = new System.Windows.Forms.Padding(0, 0, 10, 0);
			this.StatusLabel.Size = new System.Drawing.Size(86, 17);
			this.StatusLabel.Text = "%STATUS%";
			this.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
			//
			//ProgressBar1
			//
			this.ProgressBar1.AutoSize = false;
			this.ProgressBar1.Name = "ProgressBar1";
			this.ProgressBar1.Size = new System.Drawing.Size(150, 16);
			this.ProgressBar1.Step = 1;
			this.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.ProgressBar1.Visible = false;
			//
			//ToolStripStatusLabel2
			//
			this.ToolStripStatusLabel2.AutoSize = false;
			this.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2";
			this.ToolStripStatusLabel2.Size = new System.Drawing.Size(10, 17);
			//
			//Spinner
			//
			this.Spinner.Image = Properties.Resources.LoadingAni;
			this.Spinner.Name = "Spinner";
			this.Spinner.Size = new System.Drawing.Size(16, 17);
			this.Spinner.Visible = false;
			//
			//statMBPS
			//
			this.statMBPS.Font = new System.Drawing.Font("Segoe UI Semibold", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.statMBPS.Name = "statMBPS";
			this.statMBPS.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.statMBPS.Size = new System.Drawing.Size(10, 17);
			//
			//ToolStripStatusLabel1
			//
			this.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
			this.ToolStripStatusLabel1.Size = new System.Drawing.Size(696, 17);
			this.ToolStripStatusLabel1.Spring = true;
			//
			//cmdCancel
			//
			this.cmdCancel.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdCancel.Image = Properties.Resources.CloseCancelDeleteIcon;
			this.cmdCancel.Name = "cmdCancel";
			this.cmdCancel.ShowDropDownArrow = false;
			this.cmdCancel.Size = new System.Drawing.Size(63, 20);
			this.cmdCancel.Text = "Cancel";
			this.cmdCancel.Visible = false;
			//
			//ProgTimer
			//
			//
			//DeviceGroup
			//
			this.DeviceGroup.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.DeviceGroup.Controls.Add(this.Label5);
			this.DeviceGroup.Controls.Add(this.txtDeviceDescription);
			this.DeviceGroup.Controls.Add(this.Label6);
			this.DeviceGroup.Controls.Add(this.txtSerial);
			this.DeviceGroup.Controls.Add(this.Label7);
			this.DeviceGroup.Controls.Add(this.txtAssetTag);
			this.DeviceGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.DeviceGroup.Location = new System.Drawing.Point(817, 3);
			this.DeviceGroup.Name = "DeviceGroup";
			this.DeviceGroup.Size = new System.Drawing.Size(633, 95);
			this.DeviceGroup.TabIndex = 9;
			this.DeviceGroup.TabStop = false;
			this.DeviceGroup.Text = "Device Info";
			//
			//Label5
			//
			this.Label5.AutoSize = true;
			this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label5.Location = new System.Drawing.Point(331, 29);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(79, 16);
			this.Label5.TabIndex = 5;
			this.Label5.Text = "Description:";
			//
			//txtDeviceDescription
			//
			this.txtDeviceDescription.Font = new System.Drawing.Font("Consolas", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtDeviceDescription.Location = new System.Drawing.Point(334, 49);
			this.txtDeviceDescription.Name = "txtDeviceDescription";
			this.txtDeviceDescription.ReadOnly = true;
			this.txtDeviceDescription.Size = new System.Drawing.Size(259, 25);
			this.txtDeviceDescription.TabIndex = 4;
			//
			//Label6
			//
			this.Label6.AutoSize = true;
			this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label6.Location = new System.Drawing.Point(175, 29);
			this.Label6.Name = "Label6";
			this.Label6.Size = new System.Drawing.Size(46, 16);
			this.Label6.TabIndex = 3;
			this.Label6.Text = "Serial:";
			//
			//txtSerial
			//
			this.txtSerial.Font = new System.Drawing.Font("Consolas", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtSerial.Location = new System.Drawing.Point(178, 49);
			this.txtSerial.Name = "txtSerial";
			this.txtSerial.ReadOnly = true;
			this.txtSerial.Size = new System.Drawing.Size(105, 25);
			this.txtSerial.TabIndex = 2;
			//
			//Label7
			//
			this.Label7.AutoSize = true;
			this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label7.Location = new System.Drawing.Point(30, 29);
			this.Label7.Name = "Label7";
			this.Label7.Size = new System.Drawing.Size(73, 16);
			this.Label7.TabIndex = 1;
			this.Label7.Text = "Asset Tag:";
			//
			//txtAssetTag
			//
			this.txtAssetTag.Font = new System.Drawing.Font("Consolas", 11.25f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtAssetTag.Location = new System.Drawing.Point(33, 49);
			this.txtAssetTag.Name = "txtAssetTag";
			this.txtAssetTag.ReadOnly = true;
			this.txtAssetTag.Size = new System.Drawing.Size(105, 25);
			this.txtAssetTag.TabIndex = 0;
			//
			//Panel2
			//
			this.Panel2.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.Panel2.Controls.Add(this.DeviceGroup);
			this.Panel2.Controls.Add(this.SibiGroup);
			this.Panel2.Location = new System.Drawing.Point(12, 12);
			this.Panel2.Name = "Panel2";
			this.Panel2.Size = new System.Drawing.Size(793, 116);
			this.Panel2.TabIndex = 10;
			//
			//AttachmentsForm
			//
			this.AllowDrop = true;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.ClientSize = new System.Drawing.Size(817, 578);
			this.Controls.Add(this.StatusStrip1);
			this.Controls.Add(this.GroupBox1);
			this.Controls.Add(this.Panel2);
			this.DoubleBuffered = true;
			this.MinimumSize = new System.Drawing.Size(833, 370);
			this.Name = "AttachmentsForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Attachments";
			this.RightClickMenu.ResumeLayout(false);
			this.GroupBox1.ResumeLayout(false);
			this.Panel1.ResumeLayout(false);
			this.Panel1.PerformLayout();
			this.AttachContainer.Panel1.ResumeLayout(false);
			this.AttachContainer.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.AttachContainer).EndInit();
			this.AttachContainer.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.AttachGrid).EndInit();
			this.SibiGroup.ResumeLayout(false);
			this.SibiGroup.PerformLayout();
			this.StatusStrip1.ResumeLayout(false);
			this.StatusStrip1.PerformLayout();
			this.DeviceGroup.ResumeLayout(false);
			this.DeviceGroup.PerformLayout();
			this.Panel2.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}
		private Button withEventsField_cmdUpload;
		internal Button cmdUpload {
			get { return withEventsField_cmdUpload; }
			set {
				if (withEventsField_cmdUpload != null) {
					withEventsField_cmdUpload.Click -= cmdUpload_Click;
				}
				withEventsField_cmdUpload = value;
				if (withEventsField_cmdUpload != null) {
					withEventsField_cmdUpload.Click += cmdUpload_Click;
				}
			}
		}
		internal ContextMenuStrip RightClickMenu;
		private ToolStripMenuItem withEventsField_DeleteAttachmentToolStripMenuItem;
		internal ToolStripMenuItem DeleteAttachmentToolStripMenuItem {
			get { return withEventsField_DeleteAttachmentToolStripMenuItem; }
			set {
				if (withEventsField_DeleteAttachmentToolStripMenuItem != null) {
					withEventsField_DeleteAttachmentToolStripMenuItem.Click -= DeleteAttachmentToolStripMenuItem_Click;
				}
				withEventsField_DeleteAttachmentToolStripMenuItem = value;
				if (withEventsField_DeleteAttachmentToolStripMenuItem != null) {
					withEventsField_DeleteAttachmentToolStripMenuItem.Click += DeleteAttachmentToolStripMenuItem_Click;
				}
			}
		}
		private Button withEventsField_cmdDelete;
		internal Button cmdDelete {
			get { return withEventsField_cmdDelete; }
			set {
				if (withEventsField_cmdDelete != null) {
					withEventsField_cmdDelete.Click -= cmdDelete_Click;
				}
				withEventsField_cmdDelete = value;
				if (withEventsField_cmdDelete != null) {
					withEventsField_cmdDelete.Click += cmdDelete_Click;
				}
			}
		}
		private Button withEventsField_cmdOpen;
		internal Button cmdOpen {
			get { return withEventsField_cmdOpen; }
			set {
				if (withEventsField_cmdOpen != null) {
					withEventsField_cmdOpen.Click -= cmdOpen_Click;
				}
				withEventsField_cmdOpen = value;
				if (withEventsField_cmdOpen != null) {
					withEventsField_cmdOpen.Click += cmdOpen_Click;
				}
			}
		}
		internal GroupBox GroupBox1;
		internal GroupBox SibiGroup;
		internal Label Label3;
		internal TextBox txtDescription;
		internal Label Label2;
		internal TextBox txtRequestNum;
		internal Label Label1;
		internal TextBox ReqPO;
		internal StatusStrip StatusStrip1;
		internal ToolStripStatusLabel StatusLabel;
		internal ToolStripStatusLabel Spinner;
		private ToolStripMenuItem withEventsField_OpenTool;
		internal ToolStripMenuItem OpenTool {
			get { return withEventsField_OpenTool; }
			set {
				if (withEventsField_OpenTool != null) {
					withEventsField_OpenTool.Click -= OpenTool_Click;
				}
				withEventsField_OpenTool = value;
				if (withEventsField_OpenTool != null) {
					withEventsField_OpenTool.Click += OpenTool_Click;
				}
			}
		}
		internal ToolStripSeparator ToolStripSeparator1;
		internal ToolStripProgressBar ProgressBar1;
		private Timer withEventsField_ProgTimer;
		internal Timer ProgTimer {
			get { return withEventsField_ProgTimer; }
			set {
				if (withEventsField_ProgTimer != null) {
					withEventsField_ProgTimer.Tick -= ProgTimer_Tick;
				}
				withEventsField_ProgTimer = value;
				if (withEventsField_ProgTimer != null) {
					withEventsField_ProgTimer.Tick += ProgTimer_Tick;
				}
			}
		}
		internal ToolStripStatusLabel ToolStripStatusLabel2;
		internal ToolStripStatusLabel statMBPS;
		private DataGridView withEventsField_AttachGrid;
		internal DataGridView AttachGrid {
			get { return withEventsField_AttachGrid; }
			set {
				if (withEventsField_AttachGrid != null) {
					withEventsField_AttachGrid.CellDoubleClick -= AttachGrid_CellDoubleClick;
					withEventsField_AttachGrid.CellEnter -= AttachGrid_CellEnter;
					withEventsField_AttachGrid.CellLeave -= AttachGrid_CellLeave;
					withEventsField_AttachGrid.CellMouseDown -= AttachGrid_CellMouseDown;
					withEventsField_AttachGrid.CellMouseUp -= AttachGrid_CellMouseUp;
					withEventsField_AttachGrid.DragDrop -= AttachGrid_DragDrop;
					withEventsField_AttachGrid.DragLeave -= AttachGrid_DragLeave;
					withEventsField_AttachGrid.DragOver -= AttachGrid_DragOver;
					withEventsField_AttachGrid.MouseDown -= AttachGrid_MouseDown;
					withEventsField_AttachGrid.MouseMove -= AttachGrid_MouseMove;
					withEventsField_AttachGrid.KeyDown -= AttachGrid_KeyDown;
					withEventsField_AttachGrid.QueryContinueDrag -= AttachGrid_QueryContinueDrag;
				}
				withEventsField_AttachGrid = value;
				if (withEventsField_AttachGrid != null) {
					withEventsField_AttachGrid.CellDoubleClick += AttachGrid_CellDoubleClick;
					withEventsField_AttachGrid.CellEnter += AttachGrid_CellEnter;
					withEventsField_AttachGrid.CellLeave += AttachGrid_CellLeave;
					withEventsField_AttachGrid.CellMouseDown += AttachGrid_CellMouseDown;
					withEventsField_AttachGrid.CellMouseUp += AttachGrid_CellMouseUp;
					withEventsField_AttachGrid.DragDrop += AttachGrid_DragDrop;
					withEventsField_AttachGrid.DragLeave += AttachGrid_DragLeave;
					withEventsField_AttachGrid.DragOver += AttachGrid_DragOver;
					withEventsField_AttachGrid.MouseDown += AttachGrid_MouseDown;
					withEventsField_AttachGrid.MouseMove += AttachGrid_MouseMove;
					withEventsField_AttachGrid.KeyDown += AttachGrid_KeyDown;
					withEventsField_AttachGrid.QueryContinueDrag += AttachGrid_QueryContinueDrag;
				}
			}
		}
		private ToolStripMenuItem withEventsField_CopyTextTool;
		internal ToolStripMenuItem CopyTextTool {
			get { return withEventsField_CopyTextTool; }
			set {
				if (withEventsField_CopyTextTool != null) {
					withEventsField_CopyTextTool.Click -= CopyTextTool_Click;
				}
				withEventsField_CopyTextTool = value;
				if (withEventsField_CopyTextTool != null) {
					withEventsField_CopyTextTool.Click += CopyTextTool_Click;
				}
			}
		}
		internal Panel Panel1;
		internal ToolStripStatusLabel ToolStripStatusLabel1;
		private ToolStripDropDownButton withEventsField_cmdCancel;
		internal ToolStripDropDownButton cmdCancel {
			get { return withEventsField_cmdCancel; }
			set {
				if (withEventsField_cmdCancel != null) {
					withEventsField_cmdCancel.Click -= cmdCancel_Click;
				}
				withEventsField_cmdCancel = value;
				if (withEventsField_cmdCancel != null) {
					withEventsField_cmdCancel.Click += cmdCancel_Click;
				}
			}
		}
		private ToolStripMenuItem withEventsField_RenameStripMenuItem;
		internal ToolStripMenuItem RenameStripMenuItem {
			get { return withEventsField_RenameStripMenuItem; }
			set {
				if (withEventsField_RenameStripMenuItem != null) {
					withEventsField_RenameStripMenuItem.Click -= RenameStripMenuItem_Click;
				}
				withEventsField_RenameStripMenuItem = value;
				if (withEventsField_RenameStripMenuItem != null) {
					withEventsField_RenameStripMenuItem.Click += RenameStripMenuItem_Click;
				}
			}
		}
		private CheckBox withEventsField_AllowDragCheckBox;
		internal CheckBox AllowDragCheckBox {
			get { return withEventsField_AllowDragCheckBox; }
			set {
				if (withEventsField_AllowDragCheckBox != null) {
					withEventsField_AllowDragCheckBox.Click -= AllowDragCheckBox_Click;
				}
				withEventsField_AllowDragCheckBox = value;
				if (withEventsField_AllowDragCheckBox != null) {
					withEventsField_AllowDragCheckBox.Click += AllowDragCheckBox_Click;
				}
			}
		}
		internal GroupBox DeviceGroup;
		internal Label Label5;
		internal TextBox txtDeviceDescription;
		internal Label Label6;
		internal TextBox txtSerial;
		internal Label Label7;
		internal TextBox txtAssetTag;
		internal Panel Panel2;
		private ToolStripMenuItem withEventsField_SaveToMenuItem;
		internal ToolStripMenuItem SaveToMenuItem {
			get { return withEventsField_SaveToMenuItem; }
			set {
				if (withEventsField_SaveToMenuItem != null) {
					withEventsField_SaveToMenuItem.Click -= SaveToMenuItem_Click;
				}
				withEventsField_SaveToMenuItem = value;
				if (withEventsField_SaveToMenuItem != null) {
					withEventsField_SaveToMenuItem.Click += SaveToMenuItem_Click;
				}
			}
		}
		internal Label Label4;
		internal TextBox ReqNumberTextBox;
		internal SplitContainer AttachContainer;
		private ListView withEventsField_FolderListView;
		internal ListView FolderListView {
			get { return withEventsField_FolderListView; }
			set {
				if (withEventsField_FolderListView != null) {
					withEventsField_FolderListView.ItemActivate -= FolderListView_ItemActivate;
					withEventsField_FolderListView.ItemSelectionChanged -= FolderListView_ItemSelectionChanged;
					withEventsField_FolderListView.DragOver -= FolderListView_DragOver;
					withEventsField_FolderListView.DragDrop -= FolderListView_DragDrop;
					withEventsField_FolderListView.DragLeave -= FolderListView_DragLeave;
				}
				withEventsField_FolderListView = value;
				if (withEventsField_FolderListView != null) {
					withEventsField_FolderListView.ItemActivate += FolderListView_ItemActivate;
					withEventsField_FolderListView.ItemSelectionChanged += FolderListView_ItemSelectionChanged;
					withEventsField_FolderListView.DragOver += FolderListView_DragOver;
					withEventsField_FolderListView.DragDrop += FolderListView_DragDrop;
					withEventsField_FolderListView.DragLeave += FolderListView_DragLeave;
				}
			}
		}
		internal ColumnHeader ColumnHeader1;
		internal ImageList imageList1;
		private ToolStripMenuItem withEventsField_NewFolderMenuItem;
		internal ToolStripMenuItem NewFolderMenuItem {
			get { return withEventsField_NewFolderMenuItem; }
			set {
				if (withEventsField_NewFolderMenuItem != null) {
					withEventsField_NewFolderMenuItem.Click -= NewFolderMenuItem_Click;
				}
				withEventsField_NewFolderMenuItem = value;
				if (withEventsField_NewFolderMenuItem != null) {
					withEventsField_NewFolderMenuItem.Click += NewFolderMenuItem_Click;
				}
			}
		}
	}
}
