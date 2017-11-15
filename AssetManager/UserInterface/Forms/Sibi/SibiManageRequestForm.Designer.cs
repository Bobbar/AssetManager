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
using AssetManager.UserInterface.CustomControls;

namespace AssetManager.UserInterface.Forms.Sibi
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
    partial class SibiManageRequestForm
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SibiManageRequestForm));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.PopupMenuItems = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmPopFA = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmLookupDevice = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmGLBudget = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmCopyText = new System.Windows.Forms.ToolStripMenuItem();
            this.NewDeviceMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.tsmDeleteItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolTip = new System.Windows.Forms.ToolTip(this.components);
            this.txtRTNumber = new System.Windows.Forms.TextBox();
            this.txtCreateDate = new System.Windows.Forms.TextBox();
            this.PopupMenuNotes = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.cmdNewNote = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdDeleteNote = new System.Windows.Forms.ToolStripMenuItem();
            this.fieldErrorIcon = new System.Windows.Forms.ErrorProvider(this.components);
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.Panel4 = new System.Windows.Forms.Panel();
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.Panel3 = new System.Windows.Forms.Panel();
            this.pnlEditButtons = new System.Windows.Forms.Panel();
            this.cmdAccept = new System.Windows.Forms.Button();
            this.cmdDiscard = new System.Windows.Forms.Button();
            this.pnlCreate = new System.Windows.Forms.Panel();
            this.cmdAddNew = new System.Windows.Forms.Button();
            this.Label8 = new System.Windows.Forms.Label();
            this.txtRequestNum = new System.Windows.Forms.TextBox();
            this.cmbStatus = new System.Windows.Forms.ComboBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.lblReqStatus = new System.Windows.Forms.Label();
            this.lblPOStatus = new System.Windows.Forms.Label();
            this.Label9 = new System.Windows.Forms.Label();
            this.Label6 = new System.Windows.Forms.Label();
            this.txtReqNumber = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.txtPO = new System.Windows.Forms.TextBox();
            this.cmbType = new System.Windows.Forms.ComboBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.dtNeedBy = new System.Windows.Forms.DateTimePicker();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtUser = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.Panel2 = new System.Windows.Forms.Panel();
            this.dgvNotes = new System.Windows.Forms.DataGridView();
            this.Panel1 = new System.Windows.Forms.Panel();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.RequestItemsGrid = new System.Windows.Forms.DataGridView();
            this.chkAllowDrag = new System.Windows.Forms.CheckBox();
            this.ToolStrip = new AssetManager.UserInterface.CustomControls.OneClickToolStrip();
            this.cmdCreate = new System.Windows.Forms.ToolStripButton();
            this.ModifyButton = new System.Windows.Forms.ToolStripButton();
            this.cmdDelete = new System.Windows.Forms.ToolStripButton();
            this.cmdAddNote = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.cmdAttachments = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.tsbRefresh = new System.Windows.Forms.ToolStripButton();
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.PopupMenuItems.SuspendLayout();
            this.PopupMenuNotes.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldErrorIcon)).BeginInit();
            this.Panel4.SuspendLayout();
            this.GroupBox1.SuspendLayout();
            this.Panel3.SuspendLayout();
            this.pnlEditButtons.SuspendLayout();
            this.pnlCreate.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.GroupBox3.SuspendLayout();
            this.Panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotes)).BeginInit();
            this.Panel1.SuspendLayout();
            this.GroupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RequestItemsGrid)).BeginInit();
            this.ToolStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // PopupMenuItems
            // 
            this.PopupMenuItems.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.PopupMenuItems.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmPopFA,
            this.tsmLookupDevice,
            this.tsmGLBudget,
            this.tsmCopyText,
            this.NewDeviceMenuItem,
            this.tsmSeparator,
            this.tsmDeleteItem});
            this.PopupMenuItems.Name = "PopupMenu";
            this.PopupMenuItems.Size = new System.Drawing.Size(179, 166);
            // 
            // tsmPopFA
            // 
            this.tsmPopFA.Image = global::AssetManager.Properties.Resources.ImportIcon;
            this.tsmPopFA.Name = "tsmPopFA";
            this.tsmPopFA.Size = new System.Drawing.Size(178, 26);
            this.tsmPopFA.Text = "Populate From FA";
            this.tsmPopFA.Click += new System.EventHandler(this.tsmPopFA_Click);
            // 
            // tsmLookupDevice
            // 
            this.tsmLookupDevice.Image = global::AssetManager.Properties.Resources.SearchIcon;
            this.tsmLookupDevice.Name = "tsmLookupDevice";
            this.tsmLookupDevice.Size = new System.Drawing.Size(178, 26);
            this.tsmLookupDevice.Text = "Lookup Device";
            this.tsmLookupDevice.Visible = false;
            this.tsmLookupDevice.Click += new System.EventHandler(this.tsmLookupDevice_Click);
            // 
            // tsmGLBudget
            // 
            this.tsmGLBudget.Image = global::AssetManager.Properties.Resources.MoneyCircle2Icon;
            this.tsmGLBudget.Name = "tsmGLBudget";
            this.tsmGLBudget.Size = new System.Drawing.Size(178, 26);
            this.tsmGLBudget.Text = "Lookup GL/Budget";
            this.tsmGLBudget.Visible = false;
            this.tsmGLBudget.Click += new System.EventHandler(this.tsmGLBudget_Click);
            // 
            // tsmCopyText
            // 
            this.tsmCopyText.Image = global::AssetManager.Properties.Resources.CopyIcon;
            this.tsmCopyText.Name = "tsmCopyText";
            this.tsmCopyText.Size = new System.Drawing.Size(178, 26);
            this.tsmCopyText.Text = "Copy Selected";
            this.tsmCopyText.Click += new System.EventHandler(this.tsmCopyText_Click);
            // 
            // NewDeviceMenuItem
            // 
            this.NewDeviceMenuItem.Image = global::AssetManager.Properties.Resources.AddIcon;
            this.NewDeviceMenuItem.Name = "NewDeviceMenuItem";
            this.NewDeviceMenuItem.Size = new System.Drawing.Size(178, 26);
            this.NewDeviceMenuItem.Text = "Import New Asset";
            this.NewDeviceMenuItem.Click += new System.EventHandler(this.NewDeviceMenuItem_Click);
            // 
            // tsmSeparator
            // 
            this.tsmSeparator.Name = "tsmSeparator";
            this.tsmSeparator.Size = new System.Drawing.Size(175, 6);
            // 
            // tsmDeleteItem
            // 
            this.tsmDeleteItem.Image = global::AssetManager.Properties.Resources.DeleteRedIcon;
            this.tsmDeleteItem.Name = "tsmDeleteItem";
            this.tsmDeleteItem.Size = new System.Drawing.Size(178, 26);
            this.tsmDeleteItem.Text = "Delete Item";
            this.tsmDeleteItem.Click += new System.EventHandler(this.tsmDeleteItem_Click);
            // 
            // ToolTip
            // 
            this.ToolTip.AutomaticDelay = 0;
            this.ToolTip.AutoPopDelay = 5500;
            this.ToolTip.InitialDelay = 0;
            this.ToolTip.IsBalloon = true;
            this.ToolTip.ReshowDelay = 110;
            // 
            // txtRTNumber
            // 
            this.txtRTNumber.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txtRTNumber.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtRTNumber.Location = new System.Drawing.Point(17, 144);
            this.txtRTNumber.Name = "txtRTNumber";
            this.txtRTNumber.Size = new System.Drawing.Size(137, 22);
            this.txtRTNumber.TabIndex = 7;
            this.ToolTip.SetToolTip(this.txtRTNumber, "Click to open RT Ticket.");
            this.txtRTNumber.Click += new System.EventHandler(this.txtRTNumber_Click);
            // 
            // txtCreateDate
            // 
            this.txtCreateDate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtCreateDate.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.txtCreateDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtCreateDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCreateDate.ForeColor = System.Drawing.Color.Black;
            this.txtCreateDate.Location = new System.Drawing.Point(263, 71);
            this.txtCreateDate.Name = "txtCreateDate";
            this.txtCreateDate.ReadOnly = true;
            this.txtCreateDate.Size = new System.Drawing.Size(137, 21);
            this.txtCreateDate.TabIndex = 23;
            this.txtCreateDate.Text = "[CREATE DATE]";
            this.txtCreateDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.ToolTip.SetToolTip(this.txtCreateDate, "Create Date");
            this.txtCreateDate.WordWrap = false;
            // 
            // PopupMenuNotes
            // 
            this.PopupMenuNotes.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdNewNote,
            this.ToolStripSeparator1,
            this.cmdDeleteNote});
            this.PopupMenuNotes.Name = "PopupMenu";
            this.PopupMenuNotes.Size = new System.Drawing.Size(137, 54);
            // 
            // cmdNewNote
            // 
            this.cmdNewNote.Image = global::AssetManager.Properties.Resources.AddNoteIcon;
            this.cmdNewNote.Name = "cmdNewNote";
            this.cmdNewNote.Size = new System.Drawing.Size(136, 22);
            this.cmdNewNote.Text = "Add Note";
            this.cmdNewNote.Click += new System.EventHandler(this.cmdNewNote_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(133, 6);
            // 
            // cmdDeleteNote
            // 
            this.cmdDeleteNote.Image = global::AssetManager.Properties.Resources.DeleteRedIcon;
            this.cmdDeleteNote.Name = "cmdDeleteNote";
            this.cmdDeleteNote.Size = new System.Drawing.Size(136, 22);
            this.cmdDeleteNote.Text = "Delete Note";
            this.cmdDeleteNote.Click += new System.EventHandler(this.cmdDeleteNote_Click);
            // 
            // fieldErrorIcon
            // 
            this.fieldErrorIcon.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.fieldErrorIcon.ContainerControl = this;
            this.fieldErrorIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("fieldErrorIcon.Icon")));
            // 
            // ContentPanel
            // 
            this.ContentPanel.AutoScroll = true;
            this.ContentPanel.Size = new System.Drawing.Size(1014, 557);
            // 
            // Panel4
            // 
            this.Panel4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel4.Controls.Add(this.GroupBox1);
            this.Panel4.Controls.Add(this.GroupBox3);
            this.Panel4.Location = new System.Drawing.Point(8, 40);
            this.Panel4.Name = "Panel4";
            this.Panel4.Size = new System.Drawing.Size(1061, 272);
            this.Panel4.TabIndex = 5;
            // 
            // GroupBox1
            // 
            this.GroupBox1.Controls.Add(this.txtCreateDate);
            this.GroupBox1.Controls.Add(this.Panel3);
            this.GroupBox1.Controls.Add(this.Label8);
            this.GroupBox1.Controls.Add(this.txtRequestNum);
            this.GroupBox1.Controls.Add(this.cmbStatus);
            this.GroupBox1.Controls.Add(this.Label7);
            this.GroupBox1.Controls.Add(this.GroupBox2);
            this.GroupBox1.Controls.Add(this.cmbType);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.dtNeedBy);
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.Label2);
            this.GroupBox1.Controls.Add(this.txtUser);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Controls.Add(this.txtDescription);
            this.GroupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox1.Location = new System.Drawing.Point(5, 4);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(600, 264);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Request Info";
            // 
            // Panel3
            // 
            this.Panel3.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Panel3.AutoSize = true;
            this.Panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.Panel3.Controls.Add(this.pnlEditButtons);
            this.Panel3.Controls.Add(this.pnlCreate);
            this.Panel3.Location = new System.Drawing.Point(229, 102);
            this.Panel3.Name = "Panel3";
            this.Panel3.Size = new System.Drawing.Size(148, 148);
            this.Panel3.TabIndex = 22;
            // 
            // pnlEditButtons
            // 
            this.pnlEditButtons.Controls.Add(this.cmdAccept);
            this.pnlEditButtons.Controls.Add(this.cmdDiscard);
            this.pnlEditButtons.Location = new System.Drawing.Point(3, 4);
            this.pnlEditButtons.Name = "pnlEditButtons";
            this.pnlEditButtons.Size = new System.Drawing.Size(141, 78);
            this.pnlEditButtons.TabIndex = 20;
            this.pnlEditButtons.Visible = false;
            // 
            // cmdAccept
            // 
            this.cmdAccept.BackColor = System.Drawing.Color.PaleGreen;
            this.cmdAccept.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdAccept.Location = new System.Drawing.Point(9, 3);
            this.cmdAccept.Name = "cmdAccept";
            this.cmdAccept.Size = new System.Drawing.Size(119, 41);
            this.cmdAccept.TabIndex = 18;
            this.cmdAccept.Text = "Accept Changes";
            this.cmdAccept.UseVisualStyleBackColor = false;
            this.cmdAccept.Click += new System.EventHandler(this.cmdAccept_Click);
            // 
            // cmdDiscard
            // 
            this.cmdDiscard.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(128)))));
            this.cmdDiscard.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdDiscard.Location = new System.Drawing.Point(9, 50);
            this.cmdDiscard.Name = "cmdDiscard";
            this.cmdDiscard.Size = new System.Drawing.Size(119, 24);
            this.cmdDiscard.TabIndex = 19;
            this.cmdDiscard.Text = "Discard Changes";
            this.cmdDiscard.UseVisualStyleBackColor = false;
            this.cmdDiscard.Click += new System.EventHandler(this.cmdDiscard_Click);
            // 
            // pnlCreate
            // 
            this.pnlCreate.Controls.Add(this.cmdAddNew);
            this.pnlCreate.Location = new System.Drawing.Point(3, 90);
            this.pnlCreate.Name = "pnlCreate";
            this.pnlCreate.Size = new System.Drawing.Size(142, 55);
            this.pnlCreate.TabIndex = 21;
            this.pnlCreate.Visible = false;
            // 
            // cmdAddNew
            // 
            this.cmdAddNew.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdAddNew.Location = new System.Drawing.Point(9, 8);
            this.cmdAddNew.Name = "cmdAddNew";
            this.cmdAddNew.Size = new System.Drawing.Size(119, 41);
            this.cmdAddNew.TabIndex = 8;
            this.cmdAddNew.Text = "Create Request";
            this.cmdAddNew.UseVisualStyleBackColor = true;
            this.cmdAddNew.Click += new System.EventHandler(this.cmdAddNew_Click);
            // 
            // Label8
            // 
            this.Label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.Label8.AutoSize = true;
            this.Label8.Location = new System.Drawing.Point(484, 29);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(66, 15);
            this.Label8.TabIndex = 16;
            this.Label8.Text = "Request #:";
            // 
            // txtRequestNum
            // 
            this.txtRequestNum.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.txtRequestNum.Location = new System.Drawing.Point(487, 45);
            this.txtRequestNum.Name = "txtRequestNum";
            this.txtRequestNum.ReadOnly = true;
            this.txtRequestNum.Size = new System.Drawing.Size(86, 21);
            this.txtRequestNum.TabIndex = 15;
            this.txtRequestNum.TabStop = false;
            // 
            // cmbStatus
            // 
            this.cmbStatus.FormattingEnabled = true;
            this.cmbStatus.Location = new System.Drawing.Point(18, 227);
            this.cmbStatus.Margin = new System.Windows.Forms.Padding(2);
            this.cmbStatus.Name = "cmbStatus";
            this.cmbStatus.Size = new System.Drawing.Size(137, 23);
            this.cmbStatus.TabIndex = 3;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Location = new System.Drawing.Point(15, 208);
            this.Label7.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(44, 15);
            this.Label7.TabIndex = 13;
            this.Label7.Text = "Status:";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox2.Controls.Add(this.lblReqStatus);
            this.GroupBox2.Controls.Add(this.lblPOStatus);
            this.GroupBox2.Controls.Add(this.Label9);
            this.GroupBox2.Controls.Add(this.txtRTNumber);
            this.GroupBox2.Controls.Add(this.Label6);
            this.GroupBox2.Controls.Add(this.txtReqNumber);
            this.GroupBox2.Controls.Add(this.Label5);
            this.GroupBox2.Controls.Add(this.txtPO);
            this.GroupBox2.Location = new System.Drawing.Point(419, 82);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(175, 175);
            this.GroupBox2.TabIndex = 11;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Add\'l Info (Click to View)";
            // 
            // lblReqStatus
            // 
            this.lblReqStatus.AutoSize = true;
            this.lblReqStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblReqStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblReqStatus.Location = new System.Drawing.Point(16, 112);
            this.lblReqStatus.Name = "lblReqStatus";
            this.lblReqStatus.Size = new System.Drawing.Size(61, 12);
            this.lblReqStatus.TabIndex = 11;
            this.lblReqStatus.Text = "Status: NA";
            // 
            // lblPOStatus
            // 
            this.lblPOStatus.AutoSize = true;
            this.lblPOStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblPOStatus.ForeColor = System.Drawing.Color.DimGray;
            this.lblPOStatus.Location = new System.Drawing.Point(16, 59);
            this.lblPOStatus.Name = "lblPOStatus";
            this.lblPOStatus.Size = new System.Drawing.Size(61, 12);
            this.lblPOStatus.TabIndex = 10;
            this.lblPOStatus.Text = "Status: NA";
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Location = new System.Drawing.Point(14, 128);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(36, 15);
            this.Label9.TabIndex = 9;
            this.Label9.Text = "RT #:";
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Location = new System.Drawing.Point(14, 74);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(82, 15);
            this.Label6.TabIndex = 7;
            this.Label6.Text = "Requisition #:";
            // 
            // txtReqNumber
            // 
            this.txtReqNumber.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txtReqNumber.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReqNumber.Location = new System.Drawing.Point(17, 90);
            this.txtReqNumber.Name = "txtReqNumber";
            this.txtReqNumber.Size = new System.Drawing.Size(137, 22);
            this.txtReqNumber.TabIndex = 6;
            this.txtReqNumber.Click += new System.EventHandler(this.txtReqNumber_Click);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Location = new System.Drawing.Point(14, 21);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(37, 15);
            this.Label5.TabIndex = 5;
            this.Label5.Text = "PO #:";
            // 
            // txtPO
            // 
            this.txtPO.Cursor = System.Windows.Forms.Cursors.Hand;
            this.txtPO.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPO.ForeColor = System.Drawing.SystemColors.WindowText;
            this.txtPO.Location = new System.Drawing.Point(17, 37);
            this.txtPO.Name = "txtPO";
            this.txtPO.Size = new System.Drawing.Size(137, 22);
            this.txtPO.TabIndex = 5;
            this.txtPO.Click += new System.EventHandler(this.txtPO_Click);
            // 
            // cmbType
            // 
            this.cmbType.FormattingEnabled = true;
            this.cmbType.Location = new System.Drawing.Point(18, 135);
            this.cmbType.Margin = new System.Windows.Forms.Padding(2);
            this.cmbType.Name = "cmbType";
            this.cmbType.Size = new System.Drawing.Size(137, 23);
            this.cmbType.TabIndex = 2;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Location = new System.Drawing.Point(15, 116);
            this.Label4.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(36, 15);
            this.Label4.TabIndex = 8;
            this.Label4.Text = "Type:";
            // 
            // dtNeedBy
            // 
            this.dtNeedBy.CustomFormat = "MM/dd/yyyy";
            this.dtNeedBy.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtNeedBy.Location = new System.Drawing.Point(18, 182);
            this.dtNeedBy.Margin = new System.Windows.Forms.Padding(2);
            this.dtNeedBy.Name = "dtNeedBy";
            this.dtNeedBy.Size = new System.Drawing.Size(137, 21);
            this.dtNeedBy.TabIndex = 3;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Location = new System.Drawing.Point(15, 163);
            this.Label3.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(56, 15);
            this.Label3.TabIndex = 5;
            this.Label3.Text = "Need By:";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Location = new System.Drawing.Point(15, 71);
            this.Label2.Margin = new System.Windows.Forms.Padding(2, 3, 2, 2);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(85, 15);
            this.Label2.TabIndex = 3;
            this.Label2.Text = "Request User:";
            // 
            // txtUser
            // 
            this.txtUser.Location = new System.Drawing.Point(18, 90);
            this.txtUser.Margin = new System.Windows.Forms.Padding(2);
            this.txtUser.Name = "txtUser";
            this.txtUser.Size = new System.Drawing.Size(137, 21);
            this.txtUser.TabIndex = 1;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Location = new System.Drawing.Point(15, 27);
            this.Label1.Margin = new System.Windows.Forms.Padding(2);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(121, 15);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Request Description:";
            // 
            // txtDescription
            // 
            this.txtDescription.Location = new System.Drawing.Point(18, 45);
            this.txtDescription.Margin = new System.Windows.Forms.Padding(2);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.Size = new System.Drawing.Size(382, 21);
            this.txtDescription.TabIndex = 0;
            this.txtDescription.Tag = "";
            // 
            // GroupBox3
            // 
            this.GroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox3.Controls.Add(this.Panel2);
            this.GroupBox3.Location = new System.Drawing.Point(611, 4);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(447, 264);
            this.GroupBox3.TabIndex = 4;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Notes";
            // 
            // Panel2
            // 
            this.Panel2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel2.Controls.Add(this.dgvNotes);
            this.Panel2.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Panel2.Location = new System.Drawing.Point(6, 13);
            this.Panel2.Name = "Panel2";
            this.Panel2.Size = new System.Drawing.Size(436, 244);
            this.Panel2.TabIndex = 0;
            // 
            // dgvNotes
            // 
            this.dgvNotes.AllowUserToAddRows = false;
            this.dgvNotes.AllowUserToDeleteRows = false;
            this.dgvNotes.AllowUserToResizeRows = false;
            this.dgvNotes.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dgvNotes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.dgvNotes.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.dgvNotes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.dgvNotes.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
            this.dgvNotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvNotes.ContextMenuStrip = this.PopupMenuNotes;
            dataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle3.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(152)))), ((int)(((byte)(39)))));
            dataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dgvNotes.DefaultCellStyle = dataGridViewCellStyle3;
            this.dgvNotes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
            this.dgvNotes.Location = new System.Drawing.Point(3, 3);
            this.dgvNotes.Name = "dgvNotes";
            this.dgvNotes.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle4.Font = new System.Drawing.Font("Consolas", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dgvNotes.RowHeadersDefaultCellStyle = dataGridViewCellStyle4;
            this.dgvNotes.RowHeadersVisible = false;
            this.dgvNotes.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.dgvNotes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.dgvNotes.ShowCellErrors = false;
            this.dgvNotes.ShowCellToolTips = false;
            this.dgvNotes.Size = new System.Drawing.Size(430, 238);
            this.dgvNotes.TabIndex = 19;
            this.dgvNotes.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dgvNotes_CellDoubleClick);
            // 
            // Panel1
            // 
            this.Panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.Panel1.Controls.Add(this.GroupBox4);
            this.Panel1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Panel1.Location = new System.Drawing.Point(8, 314);
            this.Panel1.Name = "Panel1";
            this.Panel1.Size = new System.Drawing.Size(1061, 384);
            this.Panel1.TabIndex = 1;
            // 
            // GroupBox4
            // 
            this.GroupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox4.Controls.Add(this.RequestItemsGrid);
            this.GroupBox4.Controls.Add(this.chkAllowDrag);
            this.GroupBox4.Location = new System.Drawing.Point(0, 3);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(1058, 378);
            this.GroupBox4.TabIndex = 21;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Items";
            // 
            // RequestItemsGrid
            // 
            this.RequestItemsGrid.AllowDrop = true;
            this.RequestItemsGrid.AllowUserToResizeRows = false;
            this.RequestItemsGrid.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
            | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.RequestItemsGrid.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.RequestItemsGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.RequestItemsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.RequestItemsGrid.ContextMenuStrip = this.PopupMenuItems;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(205)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.RequestItemsGrid.DefaultCellStyle = dataGridViewCellStyle1;
            this.RequestItemsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter;
            this.RequestItemsGrid.Location = new System.Drawing.Point(6, 32);
            this.RequestItemsGrid.Name = "RequestItemsGrid";
            this.RequestItemsGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.RequestItemsGrid.RowHeadersDefaultCellStyle = dataGridViewCellStyle2;
            this.RequestItemsGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.RequestItemsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
            this.RequestItemsGrid.ShowCellToolTips = false;
            this.RequestItemsGrid.Size = new System.Drawing.Size(1046, 340);
            this.RequestItemsGrid.TabIndex = 18;
            this.RequestItemsGrid.CellEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.RequestItemsGrid_CellEnter);
            this.RequestItemsGrid.CellLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.RequestItemsGrid_CellLeave);
            this.RequestItemsGrid.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.RequestItemsGrid_CellMouseDown);
            this.RequestItemsGrid.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.RequestItemsGrid_DataError);
            this.RequestItemsGrid.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.RequestItemsGrid_DefaultValuesNeeded);
            this.RequestItemsGrid.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.RequestItemsGrid_RowEnter);
            this.RequestItemsGrid.RowLeave += new System.Windows.Forms.DataGridViewCellEventHandler(this.RequestItemsGrid_RowLeave);
            this.RequestItemsGrid.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.RequestItemsGrid_RowPostPaint);
            this.RequestItemsGrid.DragDrop += new System.Windows.Forms.DragEventHandler(this.RequestItemsGrid_DragDrop);
            this.RequestItemsGrid.DragEnter += new System.Windows.Forms.DragEventHandler(this.RequestItemsGrid_DragEnter);
            this.RequestItemsGrid.MouseDown += new System.Windows.Forms.MouseEventHandler(this.RequestItemsGrid_MouseDown);
            this.RequestItemsGrid.MouseMove += new System.Windows.Forms.MouseEventHandler(this.RequestItemsGrid_MouseMove);
            this.RequestItemsGrid.MouseUp += new System.Windows.Forms.MouseEventHandler(this.RequestItemsGrid_MouseUp);
            // 
            // chkAllowDrag
            // 
            this.chkAllowDrag.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkAllowDrag.AutoSize = true;
            this.chkAllowDrag.Location = new System.Drawing.Point(956, 13);
            this.chkAllowDrag.Name = "chkAllowDrag";
            this.chkAllowDrag.Size = new System.Drawing.Size(96, 19);
            this.chkAllowDrag.TabIndex = 20;
            this.chkAllowDrag.TabStop = false;
            this.chkAllowDrag.Text = "Allow Drag";
            this.chkAllowDrag.UseVisualStyleBackColor = true;
            this.chkAllowDrag.CheckedChanged += new System.EventHandler(this.chkAllowDrag_CheckedChanged);
            // 
            // ToolStrip
            // 
            this.ToolStrip.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(185)))), ((int)(((byte)(205)))), ((int)(((byte)(255)))));
            this.ToolStrip.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ToolStrip.ImageScalingSize = new System.Drawing.Size(25, 25);
            this.ToolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.cmdCreate,
            this.ModifyButton,
            this.cmdDelete,
            this.cmdAddNote,
            this.ToolStripSeparator2,
            this.cmdAttachments,
            this.ToolStripSeparator3,
            this.tsbRefresh});
            this.ToolStrip.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip.Name = "ToolStrip";
            this.ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.ToolStrip.Size = new System.Drawing.Size(1079, 37);
            this.ToolStrip.TabIndex = 6;
            this.ToolStrip.Text = "ToolStrip1";
            // 
            // cmdCreate
            // 
            this.cmdCreate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdCreate.Image = global::AssetManager.Properties.Resources.AddIcon;
            this.cmdCreate.Name = "cmdCreate";
            this.cmdCreate.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmdCreate.Size = new System.Drawing.Size(39, 34);
            this.cmdCreate.Text = "New Request";
            this.cmdCreate.Click += new System.EventHandler(this.cmdCreate_Click);
            // 
            // ModifyButton
            // 
            this.ModifyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ModifyButton.Image = global::AssetManager.Properties.Resources.EditIcon;
            this.ModifyButton.Name = "ModifyButton";
            this.ModifyButton.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.ModifyButton.Size = new System.Drawing.Size(39, 34);
            this.ModifyButton.Text = "Modify";
            this.ModifyButton.ToolTipText = "Modify";
            this.ModifyButton.Click += new System.EventHandler(this.ModifyButton_Click);
            // 
            // cmdDelete
            // 
            this.cmdDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdDelete.Image = global::AssetManager.Properties.Resources.DeleteRedIcon;
            this.cmdDelete.Name = "cmdDelete";
            this.cmdDelete.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmdDelete.Size = new System.Drawing.Size(39, 34);
            this.cmdDelete.Text = "Delete";
            this.cmdDelete.Click += new System.EventHandler(this.cmdDelete_Click);
            // 
            // cmdAddNote
            // 
            this.cmdAddNote.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.cmdAddNote.Image = global::AssetManager.Properties.Resources.AddNoteIcon;
            this.cmdAddNote.Name = "cmdAddNote";
            this.cmdAddNote.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmdAddNote.Size = new System.Drawing.Size(39, 34);
            this.cmdAddNote.Text = "Add Note";
            this.cmdAddNote.Click += new System.EventHandler(this.cmdAddNote_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 37);
            // 
            // cmdAttachments
            // 
            this.cmdAttachments.Image = global::AssetManager.Properties.Resources.PaperClipIcon;
            this.cmdAttachments.Name = "cmdAttachments";
            this.cmdAttachments.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
            this.cmdAttachments.Size = new System.Drawing.Size(136, 34);
            this.cmdAttachments.Text = "Attachments";
            this.cmdAttachments.Click += new System.EventHandler(this.cmdAttachments_Click);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(6, 37);
            // 
            // tsbRefresh
            // 
            this.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.tsbRefresh.Image = global::AssetManager.Properties.Resources.RefreshIcon;
            this.tsbRefresh.Name = "tsbRefresh";
            this.tsbRefresh.Size = new System.Drawing.Size(29, 34);
            this.tsbRefresh.ToolTipText = "Refresh";
            this.tsbRefresh.Click += new System.EventHandler(this.tsbRefresh_Click);
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.AutoSize = false;
            this.StatusStrip1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.StatusStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StatusStrip1.Location = new System.Drawing.Point(0, 701);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(1079, 22);
            this.StatusStrip1.TabIndex = 7;
            this.StatusStrip1.Text = "StatusStrip1";
            // 
            // SibiManageRequestForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ClientSize = new System.Drawing.Size(1079, 723);
            this.Controls.Add(this.ToolStrip);
            this.Controls.Add(this.Panel1);
            this.Controls.Add(this.Panel4);
            this.Controls.Add(this.StatusStrip1);
            this.MinimumSize = new System.Drawing.Size(771, 443);
            this.Name = "SibiManageRequestForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Manage Request";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.SibiManageRequestForm_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SibiManageRequestForm_FormClosed);
            this.Load += new System.EventHandler(this.SibiManageRequestForm_Load);
            this.ResizeBegin += new System.EventHandler(this.SibiManageRequestForm_ResizeBegin);
            this.Resize += new System.EventHandler(this.SibiManageRequestForm_Resize);
            this.PopupMenuItems.ResumeLayout(false);
            this.PopupMenuNotes.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.fieldErrorIcon)).EndInit();
            this.Panel4.ResumeLayout(false);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.Panel3.ResumeLayout(false);
            this.pnlEditButtons.ResumeLayout(false);
            this.pnlCreate.ResumeLayout(false);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            this.GroupBox3.ResumeLayout(false);
            this.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvNotes)).EndInit();
            this.Panel1.ResumeLayout(false);
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.RequestItemsGrid)).EndInit();
            this.ToolStrip.ResumeLayout(false);
            this.ToolStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal ContextMenuStrip PopupMenuItems;
        internal ToolStripMenuItem tsmDeleteItem;
        internal ToolTip ToolTip;
        internal ContextMenuStrip PopupMenuNotes;
        internal ToolStripMenuItem cmdDeleteNote;
        internal ToolStripMenuItem tsmLookupDevice;
        internal ErrorProvider fieldErrorIcon;
        internal ToolStripMenuItem cmdNewNote;
        internal ToolStripSeparator ToolStripSeparator1;
        internal OneClickToolStrip ToolStrip;
        internal ToolStripButton cmdCreate;
        internal ToolStripButton ModifyButton;
        internal ToolStripButton cmdDelete;
        internal ToolStripButton cmdAddNote;
        internal ToolStripButton cmdAttachments;
        internal Panel Panel1;
        internal DataGridView RequestItemsGrid;
        internal Panel Panel4;
        internal GroupBox GroupBox1;
        internal TextBox txtCreateDate;
        internal Panel Panel3;
        internal Panel pnlEditButtons;
        internal Button cmdAccept;
        internal Button cmdDiscard;
        internal Panel pnlCreate;
        internal Button cmdAddNew;
        internal Label Label8;
        internal TextBox txtRequestNum;
        internal ComboBox cmbStatus;
        internal Label Label7;
        internal GroupBox GroupBox2;
        internal Label Label9;
        internal TextBox txtRTNumber;
        internal Label Label6;
        internal TextBox txtReqNumber;
        internal Label Label5;
        internal TextBox txtPO;
        internal ComboBox cmbType;
        internal Label Label4;
        internal DateTimePicker dtNeedBy;
        internal Label Label3;
        internal Label Label2;
        internal TextBox txtUser;
        internal Label Label1;
        internal TextBox txtDescription;
        internal GroupBox GroupBox3;
        internal Panel Panel2;
        internal DataGridView dgvNotes;
        internal ToolStripContentPanel ContentPanel;
        internal CheckBox chkAllowDrag;
        internal ToolStripSeparator tsmSeparator;
        internal Label lblPOStatus;
        internal ToolStripSeparator ToolStripSeparator2;
        internal ToolStripSeparator ToolStripSeparator3;
        internal Label lblReqStatus;
        internal GroupBox GroupBox4;
        internal ToolStripMenuItem tsmCopyText;
        internal ToolStripButton tsbRefresh;
        internal ToolStripMenuItem tsmPopFA;
        internal ToolStripMenuItem tsmGLBudget;
        internal ToolStripMenuItem NewDeviceMenuItem;
        internal StatusStrip StatusStrip1;
    }
}
