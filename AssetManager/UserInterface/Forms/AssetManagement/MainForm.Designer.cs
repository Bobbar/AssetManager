using AssetManager.UserInterface.CustomControls;

namespace AssetManager.UserInterface.Forms.AssetManagement
{
    //
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        //
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.ResultGrid = new System.Windows.Forms.DataGridView();
            this.ContextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.ViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmAddGKUpdate = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSendToGridForm = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.CopyTool = new System.Windows.Forms.ToolStripMenuItem();
            this.lblRecords = new System.Windows.Forms.Label();
            this.InstantGroup = new System.Windows.Forms.GroupBox();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label8 = new System.Windows.Forms.Label();
            this.txtAssetTag = new System.Windows.Forms.TextBox();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.SearchGroup = new System.Windows.Forms.GroupBox();
            this.SearchPanel = new System.Windows.Forms.Panel();
            this.chkHistorical = new System.Windows.Forms.CheckBox();
            this.cmdSupDevSearch = new System.Windows.Forms.Button();
            this.Label6 = new System.Windows.Forms.Label();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtReplaceYear = new System.Windows.Forms.TextBox();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.cmbOSType = new System.Windows.Forms.ComboBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label5 = new System.Windows.Forms.Label();
            this.Label1 = new System.Windows.Forms.Label();
            this.Label10 = new System.Windows.Forms.Label();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.chkTrackables = new System.Windows.Forms.CheckBox();
            this.Label12 = new System.Windows.Forms.Label();
            this.cmbEquipType = new System.Windows.Forms.ComboBox();
            this.txtSerialSearch = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.cmbLocation = new System.Windows.Forms.ComboBox();
            this.txtCurUser = new System.Windows.Forms.TextBox();
            this.txtAssetTagSearch = new System.Windows.Forms.TextBox();
            this.Label11 = new System.Windows.Forms.Label();
            this.cmdSearch = new System.Windows.Forms.Button();
            this.cmdClear = new System.Windows.Forms.Button();
            this.cmdShowAll = new System.Windows.Forms.Button();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.StripSpinner = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ConnStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripStatusLabel4 = new System.Windows.Forms.ToolStripStatusLabel();
            this.DateTimeLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.TransactionBox = new System.Windows.Forms.GroupBox();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.CommitButton = new System.Windows.Forms.Button();
            this.RollbackButton = new System.Windows.Forms.Button();
            this.ToolStrip1 = new AssetManager.UserInterface.CustomControls.OneClickToolStrip();
            this.AddDeviceTool = new System.Windows.Forms.ToolStripButton();
            this.AdminDropDown = new System.Windows.Forms.ToolStripDropDownButton();
            this.txtGUID = new System.Windows.Forms.ToolStripTextBox();
            this.DatabaseToolCombo = new System.Windows.Forms.ToolStripComboBox();
            this.tsmUserManager = new System.Windows.Forms.ToolStripMenuItem();
            this.ReEnterLACredentialsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewLogToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.TextEnCrypterToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ScanAttachmentToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmGKUpdater = new System.Windows.Forms.ToolStripMenuItem();
            this.AdvancedSearchMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.StartTransactionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.PSScriptMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.InstallChromeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdSibi = new System.Windows.Forms.ToolStripButton();
            this.GroupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).BeginInit();
            this.ContextMenuStrip1.SuspendLayout();
            this.InstantGroup.SuspendLayout();
            this.Panel1.SuspendLayout();
            this.SearchGroup.SuspendLayout();
            this.SearchPanel.SuspendLayout();
            this.StatusStrip1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.TransactionBox.SuspendLayout();
            this.ToolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.GroupBox1.Controls.Add(this.ResultGrid);
            this.GroupBox1.Controls.Add(this.lblRecords);
            this.GroupBox1.Location = new System.Drawing.Point(12, 267);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(1356, 513);
            this.GroupBox1.TabIndex = 4;
            this.GroupBox1.TabStop = false;
            // 
            // ResultGrid
            // 
            this.ResultGrid.AllowUserToAddRows = false;
            this.ResultGrid.AllowUserToDeleteRows = false;
            this.ResultGrid.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(30)))));
            this.ResultGrid.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.ResultGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.ResultGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.ResultGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ResultGrid.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.ResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ResultGrid.ContextMenuStrip = this.ContextMenuStrip1;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.ResultGrid.DefaultCellStyle = dataGridViewCellStyle3;
            this.ResultGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.ResultGrid.Location = new System.Drawing.Point(9, 19);
            this.ResultGrid.Name = "ResultGrid";
            this.ResultGrid.ReadOnly = true;
            this.ResultGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ResultGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.ResultGrid.RowHeadersVisible = false;
            this.ResultGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.ResultGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.ResultGrid.ShowCellErrors = false;
            this.ResultGrid.ShowCellToolTips = false;
            this.ResultGrid.ShowEditingIcon = false;
            this.ResultGrid.Size = new System.Drawing.Size(1335, 475);
            this.ResultGrid.TabIndex = 17;
            this.ResultGrid.VirtualMode = true;
            this.ResultGrid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.ResultGrid_CellEnter);
            this.ResultGrid.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.ResultGrid_CellLeave);
            this.ResultGrid.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ResultGrid_CellMouseDoubleClick);
            this.ResultGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.ResultGrid_CellMouseDown);
            this.ResultGrid.KeyDown += new System.Windows.Forms.KeyEventHandler(this.ResultGrid_KeyDown);
            // 
            // ContextMenuStrip1
            // 
            this.ContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ViewToolStripMenuItem,
            this.tsmAddGKUpdate,
            this.tsmSendToGridForm,
            this.ToolStripSeparator3,
            this.CopyTool});
            this.ContextMenuStrip1.Name = "ContextMenuStrip1";
            this.ContextMenuStrip1.Size = new System.Drawing.Size(180, 98);
            // 
            // ViewToolStripMenuItem
            // 
            this.ViewToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ViewToolStripMenuItem.Image = global::AssetManager.Properties.Resources.DetailsIcon;
            this.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem";
            this.ViewToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.ViewToolStripMenuItem.Text = "View";
            this.ViewToolStripMenuItem.Click += new System.EventHandler(this.ViewToolStripMenuItem_Click);
            // 
            // tsmAddGKUpdate
            // 
            this.tsmAddGKUpdate.Image = global::AssetManager.Properties.Resources.GK_SmallIcon;
            this.tsmAddGKUpdate.Name = "tsmAddGKUpdate";
            this.tsmAddGKUpdate.Size = new System.Drawing.Size(179, 22);
            this.tsmAddGKUpdate.Text = "Enqueue GK Update";
            this.tsmAddGKUpdate.Click += new System.EventHandler(this.tsmAddGKUpdate_Click);
            // 
            // tsmSendToGridForm
            // 
            this.tsmSendToGridForm.Image = global::AssetManager.Properties.Resources.TransferArrowsIcon;
            this.tsmSendToGridForm.Name = "tsmSendToGridForm";
            this.tsmSendToGridForm.Size = new System.Drawing.Size(179, 22);
            this.tsmSendToGridForm.Text = "Send to Grid Form";
            this.tsmSendToGridForm.Click += new System.EventHandler(this.tsmSendToGridForm_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(176, 6);
            // 
            // CopyTool
            // 
            this.CopyTool.Image = global::AssetManager.Properties.Resources.CopyIcon;
            this.CopyTool.Name = "CopyTool";
            this.CopyTool.Size = new System.Drawing.Size(179, 22);
            this.CopyTool.Text = "Copy Selected";
            this.CopyTool.Click += new System.EventHandler(this.CopyTool_Click);
            // 
            // lblRecords
            // 
            this.lblRecords.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblRecords.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(53)))), ((int)(((byte)(53)))), ((int)(((byte)(53)))));
            this.lblRecords.Location = new System.Drawing.Point(15, 497);
            this.lblRecords.Name = "lblRecords";
            this.lblRecords.Size = new System.Drawing.Size(1329, 13);
            this.lblRecords.TabIndex = 18;
            this.lblRecords.Text = "Records: 0";
            this.lblRecords.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // InstantGroup
            // 
            this.InstantGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.InstantGroup.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.InstantGroup.Controls.Add(this.Panel1);
            this.InstantGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.InstantGroup.Location = new System.Drawing.Point(9, 19);
            this.InstantGroup.Name = "InstantGroup";
            this.InstantGroup.Size = new System.Drawing.Size(177, 201);
            this.InstantGroup.TabIndex = 34;
            this.InstantGroup.TabStop = false;
            this.InstantGroup.Text = "Instant Lookup";
            // 
            // Panel1
            // 
            this.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.Panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Panel1.Controls.Add(this.Label9);
            this.Panel1.Controls.Add(this.Label8);
            this.Panel1.Controls.Add(this.txtAssetTag);
            this.Panel1.Controls.Add(this.txtSerial);
            this.Panel1.Location = new System.Drawing.Point(6, 20);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(165, 173);
            this.Panel1.TabIndex = 39;
            // 
            // Label9
            // 
            this.Label9.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label9.AutoSize = true;
            this.Label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.Location = new System.Drawing.Point(12, 80);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(46, 16);
            this.Label9.TabIndex = 38;
            this.Label9.Text = "Serial:";
            // 
            // Label8
            // 
            this.Label8.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.Location = new System.Drawing.Point(12, 31);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(73, 16);
            this.Label8.TabIndex = 37;
            this.Label8.Text = "Asset Tag:";
            // 
            // txtAssetTag
            // 
            this.txtAssetTag.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtAssetTag.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAssetTag.Location = new System.Drawing.Point(15, 50);
            this.txtAssetTag.MaxLength = 45;
            this.txtAssetTag.Name = "txtAssetTag";
            this.txtAssetTag.Size = new System.Drawing.Size(135, 23);
            this.txtAssetTag.TabIndex = 36;
            // 
            // txtSerial
            // 
            this.txtSerial.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.txtSerial.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerial.Location = new System.Drawing.Point(15, 99);
            this.txtSerial.MaxLength = 45;
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.Size = new System.Drawing.Size(135, 23);
            this.txtSerial.TabIndex = 35;
            // 
            // SearchGroup
            // 
            this.SearchGroup.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.SearchGroup.Controls.Add(this.SearchPanel);
            this.SearchGroup.Controls.Add(this.cmdSearch);
            this.SearchGroup.Controls.Add(this.cmdClear);
            this.SearchGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SearchGroup.Location = new System.Drawing.Point(192, 19);
            this.SearchGroup.Name = "SearchGroup";
            this.SearchGroup.Size = new System.Drawing.Size(862, 201);
            this.SearchGroup.TabIndex = 31;
            this.SearchGroup.TabStop = false;
            this.SearchGroup.Text = "Custom Search";
            // 
            // SearchPanel
            // 
            this.SearchPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)));
            this.SearchPanel.AutoScrollMargin = new System.Drawing.Size(10, 20);
            this.SearchPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.SearchPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.SearchPanel.Controls.Add(this.chkHistorical);
            this.SearchPanel.Controls.Add(this.cmdSupDevSearch);
            this.SearchPanel.Controls.Add(this.Label6);
            this.SearchPanel.Controls.Add(this.Label4);
            this.SearchPanel.Controls.Add(this.txtReplaceYear);
            this.SearchPanel.Controls.Add(this.txtDescription);
            this.SearchPanel.Controls.Add(this.cmbOSType);
            this.SearchPanel.Controls.Add(this.Label2);
            this.SearchPanel.Controls.Add(this.Label5);
            this.SearchPanel.Controls.Add(this.Label1);
            this.SearchPanel.Controls.Add(this.Label10);
            this.SearchPanel.Controls.Add(this.cmbStatus);
            this.SearchPanel.Controls.Add(this.chkTrackables);
            this.SearchPanel.Controls.Add(this.Label12);
            this.SearchPanel.Controls.Add(this.cmbEquipType);
            this.SearchPanel.Controls.Add(this.txtSerialSearch);
            this.SearchPanel.Controls.Add(this.Label3);
            this.SearchPanel.Controls.Add(this.cmbLocation);
            this.SearchPanel.Controls.Add(this.txtCurUser);
            this.SearchPanel.Controls.Add(this.txtAssetTagSearch);
            this.SearchPanel.Controls.Add(this.Label11);
            this.SearchPanel.Location = new System.Drawing.Point(11, 20);
            this.SearchPanel.Name = "SearchPanel";
            this.SearchPanel.Size = new System.Drawing.Size(732, 173);
            this.SearchPanel.TabIndex = 52;
            this.SearchPanel.Scroll += new System.Windows.Forms.ScrollEventHandler(this.PanelNoScrollOnFocus1_Scroll);
            // 
            // chkHistorical
            // 
            this.chkHistorical.AutoSize = true;
            this.chkHistorical.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkHistorical.Location = new System.Drawing.Point(444, 126);
            this.chkHistorical.Name = "chkHistorical";
            this.chkHistorical.Size = new System.Drawing.Size(89, 22);
            this.chkHistorical.TabIndex = 56;
            this.chkHistorical.Text = "Historical";
            this.chkHistorical.UseVisualStyleBackColor = true;
            // 
            // cmdSupDevSearch
            // 
            this.cmdSupDevSearch.Location = new System.Drawing.Point(577, 114);
            this.cmdSupDevSearch.Name = "cmdSupDevSearch";
            this.cmdSupDevSearch.Size = new System.Drawing.Size(125, 44);
            this.cmdSupDevSearch.TabIndex = 55;
            this.cmdSupDevSearch.Text = "Devices By Supervisor";
            this.cmdSupDevSearch.UseVisualStyleBackColor = true;
            this.cmdSupDevSearch.Click += new System.EventHandler(this.cmdSupDevSearch_Click);
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(183, 108);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(95, 16);
            this.Label6.TabIndex = 54;
            this.Label6.Text = "Replace Year:";
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(22, 12);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(73, 16);
            this.Label4.TabIndex = 48;
            this.Label4.Text = "Asset Tag:";
            // 
            // txtReplaceYear
            // 
            this.txtReplaceYear.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReplaceYear.Location = new System.Drawing.Point(186, 127);
            this.txtReplaceYear.MaxLength = 200;
            this.txtReplaceYear.Name = "txtReplaceYear";
            this.txtReplaceYear.Size = new System.Drawing.Size(100, 23);
            this.txtReplaceYear.TabIndex = 53;
            this.txtReplaceYear.TabStop = false;
            // 
            // txtDescription
            // 
            this.txtDescription.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(186, 78);
            this.txtDescription.MaxLength = 200;
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(330, 23);
            this.txtDescription.TabIndex = 43;
            this.txtDescription.TabStop = false;
            // 
            // cmbOSType
            // 
            this.cmbOSType.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbOSType.FormattingEnabled = true;
            this.cmbOSType.Location = new System.Drawing.Point(25, 127);
            this.cmbOSType.Name = "cmbOSType";
            this.cmbOSType.Size = new System.Drawing.Size(135, 23);
            this.cmbOSType.TabIndex = 51;
            this.cmbOSType.TabStop = false;
            this.cmbOSType.DropDown += new System.EventHandler(this.cmbOSType_DropDown);
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(183, 59);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(79, 16);
            this.Label2.TabIndex = 44;
            this.Label2.Text = "Description:";
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(22, 108);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(65, 16);
            this.Label5.TabIndex = 52;
            this.Label5.Text = "OS Type:";
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(534, 59);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(48, 16);
            this.Label1.TabIndex = 42;
            this.Label1.Text = "Status:";
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label10.Location = new System.Drawing.Point(354, 12);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(110, 16);
            this.Label10.TabIndex = 36;
            this.Label10.Text = "Equipment Type:";
            // 
            // cmbStatus
            // 
            this.cmbStatus.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(537, 78);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(165, 23);
            this.cmbStatus.TabIndex = 41;
            this.cmbStatus.TabStop = false;
            // 
            // chkTrackables
            // 
            this.chkTrackables.AutoSize = true;
            this.chkTrackables.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTrackables.Location = new System.Drawing.Point(323, 126);
            this.chkTrackables.Name = "chkTrackables";
            this.chkTrackables.Size = new System.Drawing.Size(100, 22);
            this.chkTrackables.TabIndex = 50;
            this.chkTrackables.Text = "Trackables";
            this.chkTrackables.UseVisualStyleBackColor = true;
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label12.Location = new System.Drawing.Point(534, 12);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(62, 16);
            this.Label12.TabIndex = 40;
            this.Label12.Text = "Location:";
            // 
            // cmbEquipType
            // 
            this.cmbEquipType.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEquipType.FormattingEnabled = true;
            this.cmbEquipType.Location = new System.Drawing.Point(357, 30);
            this.cmbEquipType.Name = "cmbEquipType";
            this.cmbEquipType.Size = new System.Drawing.Size(159, 23);
            this.cmbEquipType.TabIndex = 35;
            this.cmbEquipType.TabStop = false;
            this.cmbEquipType.Click += new System.EventHandler(this.cmbEquipType_DropDown);
            // 
            // txtSerialSearch
            // 
            this.txtSerialSearch.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerialSearch.Location = new System.Drawing.Point(25, 78);
            this.txtSerialSearch.MaxLength = 45;
            this.txtSerialSearch.Name = "txtSerialSearch";
            this.txtSerialSearch.Size = new System.Drawing.Size(135, 23);
            this.txtSerialSearch.TabIndex = 46;
            this.txtSerialSearch.TabStop = false;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(22, 59);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(46, 16);
            this.Label3.TabIndex = 49;
            this.Label3.Text = "Serial:";
            // 
            // cmbLocation
            // 
            this.cmbLocation.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbLocation.FormattingEnabled = true;
            this.cmbLocation.Location = new System.Drawing.Point(537, 30);
            this.cmbLocation.Name = "cmbLocation";
            this.cmbLocation.Size = new System.Drawing.Size(165, 23);
            this.cmbLocation.TabIndex = 39;
            this.cmbLocation.TabStop = false;
            this.cmbLocation.Click += new System.EventHandler(this.cmbLocation_DropDown);
            // 
            // txtCurUser
            // 
            this.txtCurUser.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurUser.Location = new System.Drawing.Point(186, 30);
            this.txtCurUser.MaxLength = 45;
            this.txtCurUser.Name = "txtCurUser";
            this.txtCurUser.Size = new System.Drawing.Size(148, 23);
            this.txtCurUser.TabIndex = 37;
            this.txtCurUser.TabStop = false;
            // 
            // txtAssetTagSearch
            // 
            this.txtAssetTagSearch.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAssetTagSearch.Location = new System.Drawing.Point(25, 30);
            this.txtAssetTagSearch.MaxLength = 45;
            this.txtAssetTagSearch.Name = "txtAssetTagSearch";
            this.txtAssetTagSearch.Size = new System.Drawing.Size(135, 23);
            this.txtAssetTagSearch.TabIndex = 47;
            this.txtAssetTagSearch.TabStop = false;
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label11.Location = new System.Drawing.Point(183, 12);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(85, 16);
            this.Label11.TabIndex = 38;
            this.Label11.Text = "Current User:";
            // 
            // cmdSearch
            // 
            this.cmdSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSearch.Location = new System.Drawing.Point(758, 32);
            this.cmdSearch.Name = "cmdSearch";
            this.cmdSearch.Size = new System.Drawing.Size(88, 56);
            this.cmdSearch.TabIndex = 45;
            this.cmdSearch.Text = "Search";
            this.cmdSearch.UseVisualStyleBackColor = true;
            this.cmdSearch.Click += new System.EventHandler(this.cmdSearch_Click);
            // 
            // cmdClear
            // 
            this.cmdClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdClear.Location = new System.Drawing.Point(758, 142);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(88, 32);
            this.cmdClear.TabIndex = 18;
            this.cmdClear.Text = "Clear";
            this.cmdClear.UseVisualStyleBackColor = true;
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // cmdShowAll
            // 
            this.cmdShowAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdShowAll.Location = new System.Drawing.Point(1066, 63);
            this.cmdShowAll.Name = "cmdShowAll";
            this.cmdShowAll.Size = new System.Drawing.Size(134, 35);
            this.cmdShowAll.TabIndex = 27;
            this.cmdShowAll.Text = "Show All";
            this.cmdShowAll.UseVisualStyleBackColor = true;
            this.cmdShowAll.Click += new System.EventHandler(this.cmdShowAll_Click);
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.StatusStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.StatusLabel,
            this.StripSpinner,
            this.ToolStripStatusLabel1,
            this.ConnStatusLabel,
            this.ToolStripStatusLabel4,
            this.DateTimeLabel});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 787);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.ShowItemToolTips = true;
            this.StatusStrip1.Size = new System.Drawing.Size(1381, 22);
            this.StatusStrip1.TabIndex = 5;
            this.StatusStrip1.Text = "StatusStrip1";
            // 
            // StatusLabel
            // 
            this.StatusLabel.Name = "StatusLabel";
            this.StatusLabel.Size = new System.Drawing.Size(101, 17);
            this.StatusLabel.Text = "%StatusLabel%";
            // 
            // StripSpinner
            // 
            this.StripSpinner.Image = global::AssetManager.Properties.Resources.LoadingAni;
            this.StripSpinner.Name = "StripSpinner";
            this.StripSpinner.Size = new System.Drawing.Size(16, 17);
            this.StripSpinner.Visible = false;
            // 
            // ToolStripStatusLabel1
            // 
            this.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1";
            this.ToolStripStatusLabel1.Size = new System.Drawing.Size(1104, 17);
            this.ToolStripStatusLabel1.Spring = true;
            // 
            // ConnStatusLabel
            // 
            this.ConnStatusLabel.ForeColor = System.Drawing.Color.Green;
            this.ConnStatusLabel.Name = "ConnStatusLabel";
            this.ConnStatusLabel.Size = new System.Drawing.Size(73, 17);
            this.ConnStatusLabel.Text = "Connected";
            // 
            // ToolStripStatusLabel4
            // 
            this.ToolStripStatusLabel4.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToolStripStatusLabel4.Name = "ToolStripStatusLabel4";
            this.ToolStripStatusLabel4.Size = new System.Drawing.Size(12, 17);
            this.ToolStripStatusLabel4.Text = "|";
            // 
            // DateTimeLabel
            // 
            this.DateTimeLabel.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DateTimeLabel.Name = "DateTimeLabel";
            this.DateTimeLabel.Size = new System.Drawing.Size(76, 17);
            this.DateTimeLabel.Text = "ServerTime";
            this.DateTimeLabel.ToolTipText = "Server Time";
            this.DateTimeLabel.Click += new System.EventHandler(this.DateTimeLabel_Click);
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.TransactionBox);
            this.GroupBox2.Controls.Add(this.SearchGroup);
            this.GroupBox2.Controls.Add(this.cmdShowAll);
            this.GroupBox2.Controls.Add(this.InstantGroup);
            this.GroupBox2.Location = new System.Drawing.Point(12, 40);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(1215, 226);
            this.GroupBox2.TabIndex = 7;
            this.GroupBox2.TabStop = false;
            // 
            // TransactionBox
            // 
            this.TransactionBox.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.TransactionBox.Controls.Add(this.UpdateButton);
            this.TransactionBox.Controls.Add(this.CommitButton);
            this.TransactionBox.Controls.Add(this.RollbackButton);
            this.TransactionBox.Location = new System.Drawing.Point(1066, 118);
            this.TransactionBox.Name = "TransactionBox";
            this.TransactionBox.Size = new System.Drawing.Size(134, 92);
            this.TransactionBox.TabIndex = 38;
            this.TransactionBox.TabStop = false;
            this.TransactionBox.Text = "Transaction";
            this.TransactionBox.Visible = false;
            // 
            // UpdateButton
            // 
            this.UpdateButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.UpdateButton.Location = new System.Drawing.Point(6, 18);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(122, 22);
            this.UpdateButton.TabIndex = 38;
            this.UpdateButton.Text = "Apply Changes";
            this.UpdateButton.UseVisualStyleBackColor = false;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // CommitButton
            // 
            this.CommitButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
            this.CommitButton.Location = new System.Drawing.Point(30, 41);
            this.CommitButton.Name = "CommitButton";
            this.CommitButton.Size = new System.Drawing.Size(76, 22);
            this.CommitButton.TabIndex = 36;
            this.CommitButton.Text = "Commit";
            this.CommitButton.UseVisualStyleBackColor = false;
            this.CommitButton.Click += new System.EventHandler(this.CommitButton_Click);
            // 
            // RollbackButton
            // 
            this.RollbackButton.BackColor = System.Drawing.Color.Red;
            this.RollbackButton.Location = new System.Drawing.Point(31, 64);
            this.RollbackButton.Name = "RollbackButton";
            this.RollbackButton.Size = new System.Drawing.Size(76, 22);
            this.RollbackButton.TabIndex = 37;
            this.RollbackButton.Text = "Rollback";
            this.RollbackButton.UseVisualStyleBackColor = false;
            this.RollbackButton.Click += new System.EventHandler(this.RollbackButton_Click);
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.AutoSize = false;
            this.ToolStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(249)))), ((int)(((byte)(226)))), ((int)(((byte)(166)))));
            this.ToolStrip1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToolStrip1.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.AddDeviceTool,
            this.AdminDropDown,
            this.ToolStripSeparator5,
            this.cmdSibi});
            this.ToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.ToolStrip1.Size = new System.Drawing.Size(1381, 37);
            this.ToolStrip1.Stretch = true;
            this.ToolStrip1.TabIndex = 6;
            this.ToolStrip1.Text = "ToolStrip1";
            // 
            // AddDeviceTool
            // 
            this.AddDeviceTool.Image = global::AssetManager.Properties.Resources.AddIcon;
            this.AddDeviceTool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AddDeviceTool.Name = "AddDeviceTool";
            this.AddDeviceTool.Padding = new System.Windows.Forms.Padding(20, 0, 0, 0);
            this.AddDeviceTool.Size = new System.Drawing.Size(137, 34);
            this.AddDeviceTool.Text = "Add Device";
            this.AddDeviceTool.Click += new System.EventHandler(this.AddDeviceTool_Click);
            // 
            // AdminDropDown
            // 
            this.AdminDropDown.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.txtGUID,
            this.DatabaseToolCombo,
            this.tsmUserManager,
            this.ReEnterLACredentialsToolStripMenuItem,
            this.ViewLogToolStripMenuItem,
            this.TextEnCrypterToolStripMenuItem,
            this.ScanAttachmentToolStripMenuItem,
            this.tsmGKUpdater,
            this.AdvancedSearchMenuItem,
            this.StartTransactionToolStripMenuItem,
            this.PSScriptMenuItem});
            this.AdminDropDown.Image = global::AssetManager.Properties.Resources.AdminIcon;
            this.AdminDropDown.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AdminDropDown.Name = "AdminDropDown";
            this.AdminDropDown.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
            this.AdminDropDown.Size = new System.Drawing.Size(143, 34);
            this.AdminDropDown.Text = "Admin Tools";
            // 
            // txtGUID
            // 
            this.txtGUID.AutoSize = false;
            this.txtGUID.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.txtGUID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtGUID.Name = "txtGUID";
            this.txtGUID.Size = new System.Drawing.Size(150, 23);
            this.txtGUID.ToolTipText = "GUID Lookup. (Press Enter)";
            this.txtGUID.Visible = false;
            this.txtGUID.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtGUID_KeyDown);
            // 
            // DatabaseToolCombo
            // 
            this.DatabaseToolCombo.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.DatabaseToolCombo.Name = "DatabaseToolCombo";
            this.DatabaseToolCombo.Size = new System.Drawing.Size(121, 25);
            this.DatabaseToolCombo.ToolTipText = "Change Database";
            this.DatabaseToolCombo.DropDownClosed += new System.EventHandler(this.DatabaseToolCombo_DropDownClosed);
            // 
            // tsmUserManager
            // 
            this.tsmUserManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.tsmUserManager.Name = "tsmUserManager";
            this.tsmUserManager.Size = new System.Drawing.Size(256, 26);
            this.tsmUserManager.Text = "User Manager";
            this.tsmUserManager.Click += new System.EventHandler(this.tsmUserManager_Click);
            // 
            // ReEnterLACredentialsToolStripMenuItem
            // 
            this.ReEnterLACredentialsToolStripMenuItem.Name = "ReEnterLACredentialsToolStripMenuItem";
            this.ReEnterLACredentialsToolStripMenuItem.Size = new System.Drawing.Size(256, 26);
            this.ReEnterLACredentialsToolStripMenuItem.Text = "Re-Enter Credentials";
            this.ReEnterLACredentialsToolStripMenuItem.Click += new System.EventHandler(this.ReEnterLACredentialsToolStripMenuItem_Click);
            // 
            // ViewLogToolStripMenuItem
            // 
            this.ViewLogToolStripMenuItem.Name = "ViewLogToolStripMenuItem";
            this.ViewLogToolStripMenuItem.Size = new System.Drawing.Size(256, 26);
            this.ViewLogToolStripMenuItem.Text = "View Log";
            this.ViewLogToolStripMenuItem.Click += new System.EventHandler(this.ViewLogToolStripMenuItem_Click);
            // 
            // TextEnCrypterToolStripMenuItem
            // 
            this.TextEnCrypterToolStripMenuItem.Name = "TextEnCrypterToolStripMenuItem";
            this.TextEnCrypterToolStripMenuItem.Size = new System.Drawing.Size(256, 26);
            this.TextEnCrypterToolStripMenuItem.Text = "Text Encrypter";
            this.TextEnCrypterToolStripMenuItem.Click += new System.EventHandler(this.TextEnCrypterToolStripMenuItem_Click);
            // 
            // ScanAttachmentToolStripMenuItem
            // 
            this.ScanAttachmentToolStripMenuItem.Name = "ScanAttachmentToolStripMenuItem";
            this.ScanAttachmentToolStripMenuItem.Size = new System.Drawing.Size(256, 26);
            this.ScanAttachmentToolStripMenuItem.Text = "Scan Attachments";
            this.ScanAttachmentToolStripMenuItem.Click += new System.EventHandler(this.ScanAttachmentToolStripMenuItem_Click);
            // 
            // tsmGKUpdater
            // 
            this.tsmGKUpdater.Name = "tsmGKUpdater";
            this.tsmGKUpdater.Size = new System.Drawing.Size(256, 26);
            this.tsmGKUpdater.Text = "GK Updater";
            this.tsmGKUpdater.Click += new System.EventHandler(this.tsmGKUpdater_Click);
            // 
            // AdvancedSearchMenuItem
            // 
            this.AdvancedSearchMenuItem.Name = "AdvancedSearchMenuItem";
            this.AdvancedSearchMenuItem.Size = new System.Drawing.Size(256, 26);
            this.AdvancedSearchMenuItem.Text = "Advanced Search";
            this.AdvancedSearchMenuItem.Click += new System.EventHandler(this.AdvancedSearchMenuItem_Click);
            // 
            // StartTransactionToolStripMenuItem
            // 
            this.StartTransactionToolStripMenuItem.Name = "StartTransactionToolStripMenuItem";
            this.StartTransactionToolStripMenuItem.Size = new System.Drawing.Size(256, 26);
            this.StartTransactionToolStripMenuItem.Text = "Start Manual Edit Mode";
            this.StartTransactionToolStripMenuItem.Click += new System.EventHandler(this.StartTransactionToolStripMenuItem_Click);
            // 
            // PSScriptMenuItem
            // 
            this.PSScriptMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.InstallChromeMenuItem});
            this.PSScriptMenuItem.Name = "PSScriptMenuItem";
            this.PSScriptMenuItem.Size = new System.Drawing.Size(256, 26);
            this.PSScriptMenuItem.Text = "Execute Remote PS Script";
            // 
            // InstallChromeMenuItem
            // 
            this.InstallChromeMenuItem.Name = "InstallChromeMenuItem";
            this.InstallChromeMenuItem.Size = new System.Drawing.Size(237, 26);
            this.InstallChromeMenuItem.Text = "Install/Update Chrome";
            this.InstallChromeMenuItem.Click += new System.EventHandler(this.InstallChromeMenuItem_Click);
            // 
            // ToolStripSeparator5
            // 
            this.ToolStripSeparator5.Name = "ToolStripSeparator5";
            this.ToolStripSeparator5.Size = new System.Drawing.Size(6, 37);
            // 
            // cmdSibi
            // 
            this.cmdSibi.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.cmdSibi.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdSibi.Image = global::AssetManager.Properties.Resources.SibiIcon;
            this.cmdSibi.Name = "cmdSibi";
            this.cmdSibi.Padding = new System.Windows.Forms.Padding(20, 0, 20, 0);
            this.cmdSibi.Size = new System.Drawing.Size(232, 34);
            this.cmdSibi.Text = "Sibi Acquisition Manager";
            this.cmdSibi.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.cmdSibi.Click += new System.EventHandler(this.cmdSibi_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ClientSize = new System.Drawing.Size(1381, 809);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.ToolStrip1);
            this.Controls.Add(this.StatusStrip1);
            this.Controls.Add(this.GroupBox1);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(1256, 443);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Asset Manager - Main";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MainForm_FormClosed);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.GroupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ResultGrid)).EndInit();
            this.ContextMenuStrip1.ResumeLayout(false);
            this.InstantGroup.ResumeLayout(false);
            this.Panel1.ResumeLayout(false);
            this.Panel1.PerformLayout();
            this.SearchGroup.ResumeLayout(false);
            this.SearchPanel.ResumeLayout(false);
            this.SearchPanel.PerformLayout();
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.TransactionBox.ResumeLayout(false);
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal System.Windows.Forms.GroupBox GroupBox1;
        internal System.Windows.Forms.Button cmdShowAll;
        internal System.Windows.Forms.Button cmdClear;
        internal System.Windows.Forms.DataGridView ResultGrid;
        internal System.Windows.Forms.Button cmdSearch;
        internal System.Windows.Forms.GroupBox SearchGroup;
        internal System.Windows.Forms.Label Label1;
        internal System.Windows.Forms.ComboBox cmbStatus;
        internal System.Windows.Forms.Label Label12;
        internal System.Windows.Forms.ComboBox cmbLocation;
        internal System.Windows.Forms.Label Label11;
        internal System.Windows.Forms.TextBox txtCurUser;
        internal System.Windows.Forms.Label Label10;
        internal System.Windows.Forms.ComboBox cmbEquipType;
        internal System.Windows.Forms.ContextMenuStrip ContextMenuStrip1;
        internal System.Windows.Forms.ToolStripMenuItem ViewToolStripMenuItem;
        internal System.Windows.Forms.StatusStrip StatusStrip1;
        internal System.Windows.Forms.ToolStripStatusLabel StatusLabel;
        internal System.Windows.Forms.Label Label2;
        internal System.Windows.Forms.TextBox txtDescription;
        internal System.Windows.Forms.GroupBox InstantGroup;
        internal System.Windows.Forms.Label Label9;
        internal System.Windows.Forms.Label Label8;
        internal System.Windows.Forms.TextBox txtAssetTag;
        internal System.Windows.Forms.TextBox txtSerial;
        internal System.Windows.Forms.Label Label3;
        internal System.Windows.Forms.Label Label4;
        internal System.Windows.Forms.TextBox txtAssetTagSearch;
        internal System.Windows.Forms.TextBox txtSerialSearch;
        internal System.Windows.Forms.ToolStripStatusLabel StripSpinner;
        internal OneClickToolStrip ToolStrip1;
        internal System.Windows.Forms.ToolStripButton AddDeviceTool;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator3;
        internal System.Windows.Forms.ToolStripMenuItem CopyTool;
        internal System.Windows.Forms.ToolStripStatusLabel ConnStatusLabel;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel1;
        internal System.Windows.Forms.ToolStripStatusLabel DateTimeLabel;
        internal System.Windows.Forms.ToolStripDropDownButton AdminDropDown;
        internal System.Windows.Forms.GroupBox GroupBox2;
        internal System.Windows.Forms.CheckBox chkTrackables;
        internal System.Windows.Forms.ToolStripTextBox txtGUID;
        internal System.Windows.Forms.ComboBox cmbOSType;
        internal System.Windows.Forms.Label Label5;
        internal System.Windows.Forms.Label Label6;
        internal System.Windows.Forms.TextBox txtReplaceYear;
        internal System.Windows.Forms.Panel SearchPanel;
        internal System.Windows.Forms.Panel Panel1;
        internal System.Windows.Forms.ToolStripSeparator ToolStripSeparator5;
        internal System.Windows.Forms.ToolStripButton cmdSibi;
        internal System.Windows.Forms.ToolStripMenuItem tsmUserManager;
        internal System.Windows.Forms.ToolStripMenuItem TextEnCrypterToolStripMenuItem;
        internal System.Windows.Forms.ToolStripStatusLabel ToolStripStatusLabel4;
        internal System.Windows.Forms.Label lblRecords;
        internal System.Windows.Forms.ToolStripMenuItem ScanAttachmentToolStripMenuItem;
        internal System.Windows.Forms.Button cmdSupDevSearch;
        internal System.Windows.Forms.CheckBox chkHistorical;
        internal System.Windows.Forms.ToolStripMenuItem tsmAddGKUpdate;
        internal System.Windows.Forms.ToolStripMenuItem tsmGKUpdater;
        internal System.Windows.Forms.ToolStripMenuItem AdvancedSearchMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem tsmSendToGridForm;
        internal System.Windows.Forms.ToolStripMenuItem PSScriptMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem InstallChromeMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ReEnterLACredentialsToolStripMenuItem;
        internal System.Windows.Forms.ToolStripMenuItem ViewLogToolStripMenuItem;
        internal System.Windows.Forms.ToolStripComboBox DatabaseToolCombo;
        internal System.Windows.Forms.Button RollbackButton;
        internal System.Windows.Forms.Button CommitButton;
        internal System.Windows.Forms.ToolStripMenuItem StartTransactionToolStripMenuItem;
        internal System.Windows.Forms.GroupBox TransactionBox;
        internal System.Windows.Forms.Button UpdateButton;
    }
}
