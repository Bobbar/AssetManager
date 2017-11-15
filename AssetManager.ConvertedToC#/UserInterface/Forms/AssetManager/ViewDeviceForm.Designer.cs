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

namespace AssetManager.UserInterface.Forms.AssetManager
{
	[Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class ViewDeviceForm
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
			System.Windows.Forms.DataGridViewCellStyle DataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ViewDeviceForm));
			this.txtHostname = new System.Windows.Forms.TextBox();
			this.Label15 = new System.Windows.Forms.Label();
			this.txtPhoneNumber = new System.Windows.Forms.MaskedTextBox();
			this.Label14 = new System.Windows.Forms.Label();
			this.lblGUID = new System.Windows.Forms.Label();
			this.cmdMunisSearch = new System.Windows.Forms.Button();
			this.pnlOtherFunctions = new System.Windows.Forms.Panel();
			this.cmdMunisInfo = new System.Windows.Forms.Button();
			this.cmdSibiLink = new System.Windows.Forms.Button();
			this.Label12 = new System.Windows.Forms.Label();
			this.txtPONumber = new System.Windows.Forms.TextBox();
			this.chkTrackable = new System.Windows.Forms.CheckBox();
			this.Label1 = new System.Windows.Forms.Label();
			this.txtAssetTag_View_REQ = new System.Windows.Forms.TextBox();
			this.Label10 = new System.Windows.Forms.Label();
			this.txtSerial_View_REQ = new System.Windows.Forms.TextBox();
			this.Label2 = new System.Windows.Forms.Label();
			this.Label9 = new System.Windows.Forms.Label();
			this.txtCurUser_View_REQ = new System.Windows.Forms.TextBox();
			this.cmbStatus_REQ = new System.Windows.Forms.ComboBox();
			this.Label3 = new System.Windows.Forms.Label();
			this.Label8 = new System.Windows.Forms.Label();
			this.txtDescription_View_REQ = new System.Windows.Forms.TextBox();
			this.cmbOSVersion_REQ = new System.Windows.Forms.ComboBox();
			this.Label4 = new System.Windows.Forms.Label();
			this.cmbLocation_View_REQ = new System.Windows.Forms.ComboBox();
			this.Label13 = new System.Windows.Forms.Label();
			this.Label5 = new System.Windows.Forms.Label();
			this.cmbEquipType_View_REQ = new System.Windows.Forms.ComboBox();
			this.dtPurchaseDate_View_REQ = new System.Windows.Forms.DateTimePicker();
			this.Label7 = new System.Windows.Forms.Label();
			this.Label6 = new System.Windows.Forms.Label();
			this.txtReplacementYear_View = new System.Windows.Forms.TextBox();
			this.RemoteToolsBox = new System.Windows.Forms.GroupBox();
			this.FlowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
			this.cmdGKUpdate = new System.Windows.Forms.Button();
			this.cmdBrowseFiles = new System.Windows.Forms.Button();
			this.cmdRestart = new System.Windows.Forms.PictureBox();
			this.cmdRDP = new System.Windows.Forms.Button();
			this.DeployTVButton = new System.Windows.Forms.Button();
			this.UpdateChromeButton = new System.Windows.Forms.Button();
			this.cmdShowIP = new System.Windows.Forms.Button();
			this.RightClickMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.DeleteEntryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.TabControl1 = new System.Windows.Forms.TabControl();
			this.HistoryTab = new System.Windows.Forms.TabPage();
			this.DataGridHistory = new System.Windows.Forms.DataGridView();
			this.TrackingTab = new System.Windows.Forms.TabPage();
			this.TrackingGrid = new System.Windows.Forms.DataGridView();
			this.TrackingBox = new System.Windows.Forms.GroupBox();
			this.Panel3 = new System.Windows.Forms.Panel();
			this.txtCheckLocation = new System.Windows.Forms.TextBox();
			this.Label16 = new System.Windows.Forms.Label();
			this.txtDueBack = new System.Windows.Forms.TextBox();
			this.lblDueBack = new System.Windows.Forms.Label();
			this.txtCheckUser = new System.Windows.Forms.TextBox();
			this.lblCheckUser = new System.Windows.Forms.Label();
			this.txtCheckTime = new System.Windows.Forms.TextBox();
			this.lblCheckTime = new System.Windows.Forms.Label();
			this.txtCheckOut = new System.Windows.Forms.TextBox();
			this.Label11 = new System.Windows.Forms.Label();
			this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
			this.StatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
			this.tmr_RDPRefresher = new System.Windows.Forms.Timer(this.components);
			this.fieldErrorIcon = new System.Windows.Forms.ErrorProvider(this.components);
			this.tsSaveModify = new OneClickToolStrip();
			this.cmdAccept_Tool = new System.Windows.Forms.ToolStripButton();
			this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.cmdCancel_Tool = new System.Windows.Forms.ToolStripButton();
			this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.FieldsPanel = new System.Windows.Forms.Panel();
			this.InfoDataSplitter = new System.Windows.Forms.SplitContainer();
			this.FieldTabs = new System.Windows.Forms.TabControl();
			this.AssetInfo = new System.Windows.Forms.TabPage();
			this.MiscInfo = new System.Windows.Forms.TabPage();
			this.ADPanel = new System.Windows.Forms.Panel();
			this.GroupBox1 = new System.Windows.Forms.GroupBox();
			this.Label22 = new System.Windows.Forms.Label();
			this.ADCreatedTextBox = new System.Windows.Forms.TextBox();
			this.Label21 = new System.Windows.Forms.Label();
			this.ADLastLoginTextBox = new System.Windows.Forms.TextBox();
			this.Label20 = new System.Windows.Forms.Label();
			this.ADOSVerTextBox = new System.Windows.Forms.TextBox();
			this.Label19 = new System.Windows.Forms.Label();
			this.ADOSTextBox = new System.Windows.Forms.TextBox();
			this.Label18 = new System.Windows.Forms.Label();
			this.ADOUTextBox = new System.Windows.Forms.TextBox();
			this.iCloudTextBox = new System.Windows.Forms.TextBox();
			this.Label17 = new System.Windows.Forms.Label();
			this.RemoteTrackingPanel = new System.Windows.Forms.Panel();
			this.ToolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
			this.tsTracking = new System.Windows.Forms.ToolStrip();
			this.ToolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
			this.CheckOutTool = new System.Windows.Forms.ToolStripMenuItem();
			this.CheckInTool = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolStrip1 = new OneClickToolStrip();
			this.tsbModify = new System.Windows.Forms.ToolStripButton();
			this.tsbNewNote = new System.Windows.Forms.ToolStripButton();
			this.tsbDeleteDevice = new System.Windows.Forms.ToolStripButton();
			this.RefreshToolStripButton = new System.Windows.Forms.ToolStripButton();
			this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.AttachmentTool = new System.Windows.Forms.ToolStripButton();
			this.ToolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.ToolStripDropDownButton2 = new System.Windows.Forms.ToolStripDropDownButton();
			this.tsmAssetInputForm = new System.Windows.Forms.ToolStripMenuItem();
			this.tsmAssetTransferForm = new System.Windows.Forms.ToolStripMenuItem();
			this.AssetDisposalForm = new System.Windows.Forms.ToolStripMenuItem();
			this.ToolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.pnlOtherFunctions.SuspendLayout();
			this.RemoteToolsBox.SuspendLayout();
			this.FlowLayoutPanel1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.cmdRestart).BeginInit();
			this.RightClickMenu.SuspendLayout();
			this.TabControl1.SuspendLayout();
			this.HistoryTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.DataGridHistory).BeginInit();
			this.TrackingTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.TrackingGrid).BeginInit();
			this.TrackingBox.SuspendLayout();
			this.Panel3.SuspendLayout();
			this.StatusStrip1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.fieldErrorIcon).BeginInit();
			this.tsSaveModify.SuspendLayout();
			this.FieldsPanel.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)this.InfoDataSplitter).BeginInit();
			this.InfoDataSplitter.Panel1.SuspendLayout();
			this.InfoDataSplitter.Panel2.SuspendLayout();
			this.InfoDataSplitter.SuspendLayout();
			this.FieldTabs.SuspendLayout();
			this.AssetInfo.SuspendLayout();
			this.MiscInfo.SuspendLayout();
			this.ADPanel.SuspendLayout();
			this.GroupBox1.SuspendLayout();
			this.RemoteTrackingPanel.SuspendLayout();
			this.ToolStripContainer1.ContentPanel.SuspendLayout();
			this.ToolStripContainer1.TopToolStripPanel.SuspendLayout();
			this.ToolStripContainer1.SuspendLayout();
			this.tsTracking.SuspendLayout();
			this.ToolStrip1.SuspendLayout();
			this.SuspendLayout();
			//
			//txtHostname
			//
			this.txtHostname.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtHostname.Location = new System.Drawing.Point(26, 42);
			this.txtHostname.Name = "txtHostname";
			this.txtHostname.Size = new System.Drawing.Size(177, 23);
			this.txtHostname.TabIndex = 58;
			//
			//Label15
			//
			this.Label15.AutoSize = true;
			this.Label15.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label15.Location = new System.Drawing.Point(23, 23);
			this.Label15.Name = "Label15";
			this.Label15.Size = new System.Drawing.Size(73, 16);
			this.Label15.TabIndex = 59;
			this.Label15.Text = "Hostname:";
			//
			//txtPhoneNumber
			//
			this.txtPhoneNumber.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtPhoneNumber.Location = new System.Drawing.Point(26, 94);
			this.txtPhoneNumber.Mask = "(999) 000-0000";
			this.txtPhoneNumber.Name = "txtPhoneNumber";
			this.txtPhoneNumber.Size = new System.Drawing.Size(177, 23);
			this.txtPhoneNumber.TabIndex = 57;
			this.txtPhoneNumber.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
			//
			//Label14
			//
			this.Label14.AutoSize = true;
			this.Label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label14.Location = new System.Drawing.Point(23, 75);
			this.Label14.Name = "Label14";
			this.Label14.Size = new System.Drawing.Size(60, 16);
			this.Label14.TabIndex = 56;
			this.Label14.Text = "Phone #:";
			//
			//lblGUID
			//
			this.lblGUID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.lblGUID.Cursor = System.Windows.Forms.Cursors.Default;
			this.lblGUID.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblGUID.Location = new System.Drawing.Point(26, 149);
			this.lblGUID.Name = "lblGUID";
			this.lblGUID.Size = new System.Drawing.Size(272, 23);
			this.lblGUID.TabIndex = 54;
			this.lblGUID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.ToolTip1.SetToolTip(this.lblGUID, "Click to copy GUID.");
			//
			//cmdMunisSearch
			//
			this.cmdMunisSearch.Font = new System.Drawing.Font("Microsoft Sans Serif", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdMunisSearch.Location = new System.Drawing.Point(20, 155);
			this.cmdMunisSearch.Name = "cmdMunisSearch";
			this.cmdMunisSearch.Size = new System.Drawing.Size(135, 23);
			this.cmdMunisSearch.TabIndex = 3;
			this.cmdMunisSearch.Text = "Munis Search";
			this.cmdMunisSearch.UseVisualStyleBackColor = true;
			this.cmdMunisSearch.Visible = false;
			//
			//pnlOtherFunctions
			//
			this.pnlOtherFunctions.Controls.Add(this.cmdMunisInfo);
			this.pnlOtherFunctions.Controls.Add(this.cmdSibiLink);
			this.pnlOtherFunctions.Location = new System.Drawing.Point(584, 168);
			this.pnlOtherFunctions.Name = "pnlOtherFunctions";
			this.pnlOtherFunctions.Size = new System.Drawing.Size(194, 61);
			this.pnlOtherFunctions.TabIndex = 51;
			//
			//cmdMunisInfo
			//
			this.cmdMunisInfo.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cmdMunisInfo.Location = new System.Drawing.Point(14, 3);
			this.cmdMunisInfo.Name = "cmdMunisInfo";
			this.cmdMunisInfo.Size = new System.Drawing.Size(170, 23);
			this.cmdMunisInfo.TabIndex = 46;
			this.cmdMunisInfo.Text = "View MUNIS";
			this.ToolTip1.SetToolTip(this.cmdMunisInfo, "View Munis");
			this.cmdMunisInfo.UseVisualStyleBackColor = true;
			//
			//cmdSibiLink
			//
			this.cmdSibiLink.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right);
			this.cmdSibiLink.Location = new System.Drawing.Point(14, 32);
			this.cmdSibiLink.Name = "cmdSibiLink";
			this.cmdSibiLink.Size = new System.Drawing.Size(170, 23);
			this.cmdSibiLink.TabIndex = 49;
			this.cmdSibiLink.Text = "View Sibi";
			this.cmdSibiLink.UseVisualStyleBackColor = true;
			//
			//Label12
			//
			this.Label12.AutoSize = true;
			this.Label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label12.Location = new System.Drawing.Point(595, 110);
			this.Label12.Name = "Label12";
			this.Label12.Size = new System.Drawing.Size(81, 16);
			this.Label12.TabIndex = 48;
			this.Label12.Text = "PO Number:";
			//
			//txtPONumber
			//
			this.txtPONumber.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtPONumber.Location = new System.Drawing.Point(598, 129);
			this.txtPONumber.Name = "txtPONumber";
			this.txtPONumber.Size = new System.Drawing.Size(169, 23);
			this.txtPONumber.TabIndex = 11;
			//
			//chkTrackable
			//
			this.chkTrackable.AutoSize = true;
			this.chkTrackable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.chkTrackable.Location = new System.Drawing.Point(26, 190);
			this.chkTrackable.Name = "chkTrackable";
			this.chkTrackable.Size = new System.Drawing.Size(89, 20);
			this.chkTrackable.TabIndex = 13;
			this.chkTrackable.Text = "Trackable";
			this.chkTrackable.UseVisualStyleBackColor = true;
			//
			//Label1
			//
			this.Label1.AutoSize = true;
			this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label1.Location = new System.Drawing.Point(18, 62);
			this.Label1.Name = "Label1";
			this.Label1.Size = new System.Drawing.Size(73, 16);
			this.Label1.TabIndex = 20;
			this.Label1.Text = "Asset Tag:";
			//
			//txtAssetTag_View_REQ
			//
			this.txtAssetTag_View_REQ.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtAssetTag_View_REQ.Location = new System.Drawing.Point(21, 81);
			this.txtAssetTag_View_REQ.Name = "txtAssetTag_View_REQ";
			this.txtAssetTag_View_REQ.Size = new System.Drawing.Size(134, 23);
			this.txtAssetTag_View_REQ.TabIndex = 1;
			//
			//Label10
			//
			this.Label10.AutoSize = true;
			this.Label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label10.Location = new System.Drawing.Point(22, 130);
			this.Label10.Name = "Label10";
			this.Label10.Size = new System.Drawing.Size(90, 16);
			this.Label10.TabIndex = 41;
			this.Label10.Text = "Device GUID:";
			//
			//txtSerial_View_REQ
			//
			this.txtSerial_View_REQ.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtSerial_View_REQ.Location = new System.Drawing.Point(21, 33);
			this.txtSerial_View_REQ.Name = "txtSerial_View_REQ";
			this.txtSerial_View_REQ.Size = new System.Drawing.Size(134, 23);
			this.txtSerial_View_REQ.TabIndex = 0;
			//
			//Label2
			//
			this.Label2.AutoSize = true;
			this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label2.Location = new System.Drawing.Point(18, 14);
			this.Label2.Name = "Label2";
			this.Label2.Size = new System.Drawing.Size(46, 16);
			this.Label2.TabIndex = 22;
			this.Label2.Text = "Serial:";
			//
			//Label9
			//
			this.Label9.AutoSize = true;
			this.Label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label9.Location = new System.Drawing.Point(390, 110);
			this.Label9.Name = "Label9";
			this.Label9.Size = new System.Drawing.Size(48, 16);
			this.Label9.TabIndex = 39;
			this.Label9.Text = "Status:";
			//
			//txtCurUser_View_REQ
			//
			this.txtCurUser_View_REQ.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtCurUser_View_REQ.Location = new System.Drawing.Point(21, 129);
			this.txtCurUser_View_REQ.Name = "txtCurUser_View_REQ";
			this.txtCurUser_View_REQ.Size = new System.Drawing.Size(134, 23);
			this.txtCurUser_View_REQ.TabIndex = 2;
			//
			//cmbStatus_REQ
			//
			this.cmbStatus_REQ.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmbStatus_REQ.FormattingEnabled = true;
			this.cmbStatus_REQ.Location = new System.Drawing.Point(393, 129);
			this.cmbStatus_REQ.Name = "cmbStatus_REQ";
			this.cmbStatus_REQ.Size = new System.Drawing.Size(177, 23);
			this.cmbStatus_REQ.TabIndex = 8;
			//
			//Label3
			//
			this.Label3.AutoSize = true;
			this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label3.Location = new System.Drawing.Point(17, 110);
			this.Label3.Name = "Label3";
			this.Label3.Size = new System.Drawing.Size(85, 16);
			this.Label3.TabIndex = 24;
			this.Label3.Text = "Current User:";
			//
			//Label8
			//
			this.Label8.AutoSize = true;
			this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label8.Location = new System.Drawing.Point(178, 110);
			this.Label8.Name = "Label8";
			this.Label8.Size = new System.Drawing.Size(79, 16);
			this.Label8.TabIndex = 37;
			this.Label8.Text = "OS Version:";
			//
			//txtDescription_View_REQ
			//
			this.txtDescription_View_REQ.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtDescription_View_REQ.Location = new System.Drawing.Point(181, 33);
			this.txtDescription_View_REQ.Name = "txtDescription_View_REQ";
			this.txtDescription_View_REQ.Size = new System.Drawing.Size(389, 23);
			this.txtDescription_View_REQ.TabIndex = 4;
			//
			//cmbOSVersion_REQ
			//
			this.cmbOSVersion_REQ.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmbOSVersion_REQ.FormattingEnabled = true;
			this.cmbOSVersion_REQ.Location = new System.Drawing.Point(181, 129);
			this.cmbOSVersion_REQ.Name = "cmbOSVersion_REQ";
			this.cmbOSVersion_REQ.Size = new System.Drawing.Size(177, 23);
			this.cmbOSVersion_REQ.TabIndex = 6;
			//
			//Label4
			//
			this.Label4.AutoSize = true;
			this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label4.Location = new System.Drawing.Point(176, 14);
			this.Label4.Name = "Label4";
			this.Label4.Size = new System.Drawing.Size(79, 16);
			this.Label4.TabIndex = 26;
			this.Label4.Text = "Description:";
			//
			//cmbLocation_View_REQ
			//
			this.cmbLocation_View_REQ.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmbLocation_View_REQ.FormattingEnabled = true;
			this.cmbLocation_View_REQ.Location = new System.Drawing.Point(393, 81);
			this.cmbLocation_View_REQ.Name = "cmbLocation_View_REQ";
			this.cmbLocation_View_REQ.Size = new System.Drawing.Size(177, 23);
			this.cmbLocation_View_REQ.TabIndex = 7;
			//
			//Label13
			//
			this.Label13.AutoSize = true;
			this.Label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label13.Location = new System.Drawing.Point(178, 62);
			this.Label13.Name = "Label13";
			this.Label13.Size = new System.Drawing.Size(110, 16);
			this.Label13.TabIndex = 34;
			this.Label13.Text = "Equipment Type:";
			//
			//Label5
			//
			this.Label5.AutoSize = true;
			this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label5.Location = new System.Drawing.Point(390, 62);
			this.Label5.Name = "Label5";
			this.Label5.Size = new System.Drawing.Size(62, 16);
			this.Label5.TabIndex = 28;
			this.Label5.Text = "Location:";
			//
			//cmbEquipType_View_REQ
			//
			this.cmbEquipType_View_REQ.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmbEquipType_View_REQ.FormattingEnabled = true;
			this.cmbEquipType_View_REQ.Location = new System.Drawing.Point(181, 81);
			this.cmbEquipType_View_REQ.Name = "cmbEquipType_View_REQ";
			this.cmbEquipType_View_REQ.Size = new System.Drawing.Size(177, 23);
			this.cmbEquipType_View_REQ.TabIndex = 5;
			//
			//dtPurchaseDate_View_REQ
			//
			this.dtPurchaseDate_View_REQ.CustomFormat = "yyyy-MM-dd";
			this.dtPurchaseDate_View_REQ.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.dtPurchaseDate_View_REQ.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
			this.dtPurchaseDate_View_REQ.Location = new System.Drawing.Point(600, 33);
			this.dtPurchaseDate_View_REQ.Name = "dtPurchaseDate_View_REQ";
			this.dtPurchaseDate_View_REQ.Size = new System.Drawing.Size(168, 23);
			this.dtPurchaseDate_View_REQ.TabIndex = 9;
			//
			//Label7
			//
			this.Label7.AutoSize = true;
			this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label7.Location = new System.Drawing.Point(595, 62);
			this.Label7.Name = "Label7";
			this.Label7.Size = new System.Drawing.Size(95, 16);
			this.Label7.TabIndex = 32;
			this.Label7.Text = "Replace Year:";
			//
			//Label6
			//
			this.Label6.AutoSize = true;
			this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label6.Location = new System.Drawing.Point(595, 14);
			this.Label6.Name = "Label6";
			this.Label6.Size = new System.Drawing.Size(100, 16);
			this.Label6.TabIndex = 30;
			this.Label6.Text = "Purchase Date:";
			//
			//txtReplacementYear_View
			//
			this.txtReplacementYear_View.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtReplacementYear_View.Location = new System.Drawing.Point(598, 81);
			this.txtReplacementYear_View.Name = "txtReplacementYear_View";
			this.txtReplacementYear_View.Size = new System.Drawing.Size(169, 23);
			this.txtReplacementYear_View.TabIndex = 10;
			//
			//RemoteToolsBox
			//
			this.RemoteToolsBox.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.RemoteToolsBox.BackColor = System.Drawing.SystemColors.Control;
			this.RemoteToolsBox.Controls.Add(this.FlowLayoutPanel1);
			this.RemoteToolsBox.Controls.Add(this.cmdShowIP);
			this.RemoteToolsBox.FlatStyle = System.Windows.Forms.FlatStyle.System;
			this.RemoteToolsBox.Font = new System.Drawing.Font("Consolas", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.RemoteToolsBox.Location = new System.Drawing.Point(3, 2);
			this.RemoteToolsBox.Name = "RemoteToolsBox";
			this.RemoteToolsBox.Size = new System.Drawing.Size(421, 108);
			this.RemoteToolsBox.TabIndex = 52;
			this.RemoteToolsBox.TabStop = false;
			this.RemoteToolsBox.Text = "Remote Tools";
			this.RemoteToolsBox.Visible = false;
			//
			//FlowLayoutPanel1
			//
			this.FlowLayoutPanel1.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.FlowLayoutPanel1.AutoScroll = true;
			this.FlowLayoutPanel1.Controls.Add(this.cmdGKUpdate);
			this.FlowLayoutPanel1.Controls.Add(this.cmdBrowseFiles);
			this.FlowLayoutPanel1.Controls.Add(this.cmdRestart);
			this.FlowLayoutPanel1.Controls.Add(this.cmdRDP);
			this.FlowLayoutPanel1.Controls.Add(this.DeployTVButton);
			this.FlowLayoutPanel1.Controls.Add(this.UpdateChromeButton);
			this.FlowLayoutPanel1.Location = new System.Drawing.Point(6, 16);
			this.FlowLayoutPanel1.Margin = new System.Windows.Forms.Padding(0);
			this.FlowLayoutPanel1.Name = "FlowLayoutPanel1";
			this.FlowLayoutPanel1.Size = new System.Drawing.Size(318, 86);
			this.FlowLayoutPanel1.TabIndex = 57;
			//
			//cmdGKUpdate
			//
			this.cmdGKUpdate.BackgroundImage = Properties.Resources.GK__UpdateIcon;
			this.cmdGKUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.cmdGKUpdate.Location = new System.Drawing.Point(1, 1);
			this.cmdGKUpdate.Margin = new System.Windows.Forms.Padding(1);
			this.cmdGKUpdate.Name = "cmdGKUpdate";
			this.cmdGKUpdate.Size = new System.Drawing.Size(45, 45);
			this.cmdGKUpdate.TabIndex = 55;
			this.ToolTip1.SetToolTip(this.cmdGKUpdate, "Enqueue GK Update");
			this.cmdGKUpdate.UseVisualStyleBackColor = true;
			//
			//cmdBrowseFiles
			//
			this.cmdBrowseFiles.BackgroundImage = Properties.Resources.FolderIcon;
			this.cmdBrowseFiles.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
			this.cmdBrowseFiles.Location = new System.Drawing.Point(48, 1);
			this.cmdBrowseFiles.Margin = new System.Windows.Forms.Padding(1);
			this.cmdBrowseFiles.Name = "cmdBrowseFiles";
			this.cmdBrowseFiles.Size = new System.Drawing.Size(45, 45);
			this.cmdBrowseFiles.TabIndex = 52;
			this.ToolTip1.SetToolTip(this.cmdBrowseFiles, "Browse Files");
			this.cmdBrowseFiles.UseVisualStyleBackColor = true;
			//
			//cmdRestart
			//
			this.cmdRestart.BackColor = System.Drawing.SystemColors.ControlLight;
			this.cmdRestart.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.cmdRestart.Image = Properties.Resources.RestartIcon;
			this.cmdRestart.Location = new System.Drawing.Point(95, 1);
			this.cmdRestart.Margin = new System.Windows.Forms.Padding(1);
			this.cmdRestart.Name = "cmdRestart";
			this.cmdRestart.Size = new System.Drawing.Size(45, 45);
			this.cmdRestart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.cmdRestart.TabIndex = 56;
			this.cmdRestart.TabStop = false;
			this.ToolTip1.SetToolTip(this.cmdRestart, "Reboot Device");
			//
			//cmdRDP
			//
			this.cmdRDP.BackgroundImage = Properties.Resources.RDPIcon;
			this.cmdRDP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.cmdRDP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.cmdRDP.Location = new System.Drawing.Point(142, 1);
			this.cmdRDP.Margin = new System.Windows.Forms.Padding(1);
			this.cmdRDP.Name = "cmdRDP";
			this.cmdRDP.Size = new System.Drawing.Size(45, 45);
			this.cmdRDP.TabIndex = 46;
			this.ToolTip1.SetToolTip(this.cmdRDP, "Launch Remote Desktop");
			this.cmdRDP.UseVisualStyleBackColor = true;
			//
			//DeployTVButton
			//
			this.DeployTVButton.BackgroundImage = Properties.Resources.TeamViewerIcon;
			this.DeployTVButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.DeployTVButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.DeployTVButton.Location = new System.Drawing.Point(189, 1);
			this.DeployTVButton.Margin = new System.Windows.Forms.Padding(1);
			this.DeployTVButton.Name = "DeployTVButton";
			this.DeployTVButton.Size = new System.Drawing.Size(45, 45);
			this.DeployTVButton.TabIndex = 57;
			this.ToolTip1.SetToolTip(this.DeployTVButton, "Deploy TeamViewer");
			this.DeployTVButton.UseVisualStyleBackColor = true;
			//
			//UpdateChromeButton
			//
			this.UpdateChromeButton.BackgroundImage = Properties.Resources.ChromeIcon;
			this.UpdateChromeButton.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.UpdateChromeButton.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
			this.UpdateChromeButton.Location = new System.Drawing.Point(236, 1);
			this.UpdateChromeButton.Margin = new System.Windows.Forms.Padding(1);
			this.UpdateChromeButton.Name = "UpdateChromeButton";
			this.UpdateChromeButton.Size = new System.Drawing.Size(45, 45);
			this.UpdateChromeButton.TabIndex = 58;
			this.ToolTip1.SetToolTip(this.UpdateChromeButton, "Update/Install Chrome");
			this.UpdateChromeButton.UseVisualStyleBackColor = true;
			//
			//cmdShowIP
			//
			this.cmdShowIP.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right);
			this.cmdShowIP.BackColor = System.Drawing.Color.Black;
			this.cmdShowIP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
			this.cmdShowIP.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
			this.cmdShowIP.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdShowIP.ForeColor = System.Drawing.Color.White;
			this.cmdShowIP.Location = new System.Drawing.Point(327, 12);
			this.cmdShowIP.Name = "cmdShowIP";
			this.cmdShowIP.Size = new System.Drawing.Size(90, 90);
			this.cmdShowIP.TabIndex = 53;
			this.cmdShowIP.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.cmdShowIP.UseVisualStyleBackColor = false;
			//
			//RightClickMenu
			//
			this.RightClickMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.DeleteEntryToolStripMenuItem });
			this.RightClickMenu.Name = "RightClickMenu";
			this.RightClickMenu.Size = new System.Drawing.Size(138, 26);
			//
			//DeleteEntryToolStripMenuItem
			//
			this.DeleteEntryToolStripMenuItem.Image = Properties.Resources.DeleteIcon;
			this.DeleteEntryToolStripMenuItem.Name = "DeleteEntryToolStripMenuItem";
			this.DeleteEntryToolStripMenuItem.Size = new System.Drawing.Size(137, 22);
			this.DeleteEntryToolStripMenuItem.Text = "Delete Entry";
			//
			//TabControl1
			//
			this.TabControl1.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.TabControl1.Controls.Add(this.HistoryTab);
			this.TabControl1.Controls.Add(this.TrackingTab);
			this.TabControl1.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.TabControl1.ItemSize = new System.Drawing.Size(69, 21);
			this.TabControl1.Location = new System.Drawing.Point(11, 301);
			this.TabControl1.Name = "TabControl1";
			this.TabControl1.SelectedIndex = 0;
			this.TabControl1.Size = new System.Drawing.Size(1268, 265);
			this.TabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.TabControl1.TabIndex = 40;
			//
			//HistoryTab
			//
			this.HistoryTab.Controls.Add(this.DataGridHistory);
			this.HistoryTab.Location = new System.Drawing.Point(4, 25);
			this.HistoryTab.Name = "HistoryTab";
			this.HistoryTab.Padding = new System.Windows.Forms.Padding(3);
			this.HistoryTab.Size = new System.Drawing.Size(1260, 236);
			this.HistoryTab.TabIndex = 0;
			this.HistoryTab.Text = "History";
			this.HistoryTab.UseVisualStyleBackColor = true;
			//
			//DataGridHistory
			//
			this.DataGridHistory.AllowUserToAddRows = false;
			this.DataGridHistory.AllowUserToDeleteRows = false;
			this.DataGridHistory.AllowUserToResizeColumns = false;
			this.DataGridHistory.AllowUserToResizeRows = false;
			this.DataGridHistory.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.DataGridHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.DataGridHistory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this.DataGridHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.DataGridHistory.ContextMenuStrip = this.RightClickMenu;
			DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
			DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window;
			DataGridViewCellStyle1.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText;
			DataGridViewCellStyle1.Padding = new System.Windows.Forms.Padding(10);
			DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
			DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
			DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
			this.DataGridHistory.DefaultCellStyle = DataGridViewCellStyle1;
			this.DataGridHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.DataGridHistory.Location = new System.Drawing.Point(6, 6);
			this.DataGridHistory.Name = "DataGridHistory";
			this.DataGridHistory.ReadOnly = true;
			this.DataGridHistory.RowHeadersVisible = false;
			this.DataGridHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect;
			this.DataGridHistory.ShowCellToolTips = false;
			this.DataGridHistory.ShowEditingIcon = false;
			this.DataGridHistory.Size = new System.Drawing.Size(1243, 224);
			this.DataGridHistory.TabIndex = 40;
			this.DataGridHistory.TabStop = false;
			this.DataGridHistory.VirtualMode = true;
			//
			//TrackingTab
			//
			this.TrackingTab.Controls.Add(this.TrackingGrid);
			this.TrackingTab.Location = new System.Drawing.Point(4, 25);
			this.TrackingTab.Name = "TrackingTab";
			this.TrackingTab.Padding = new System.Windows.Forms.Padding(3);
			this.TrackingTab.Size = new System.Drawing.Size(1260, 236);
			this.TrackingTab.TabIndex = 1;
			this.TrackingTab.Text = "Tracking";
			this.TrackingTab.UseVisualStyleBackColor = true;
			//
			//TrackingGrid
			//
			this.TrackingGrid.AllowUserToAddRows = false;
			this.TrackingGrid.AllowUserToDeleteRows = false;
			this.TrackingGrid.AllowUserToResizeColumns = false;
			this.TrackingGrid.AllowUserToResizeRows = false;
			this.TrackingGrid.Anchor = (System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.TrackingGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
			this.TrackingGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText;
			this.TrackingGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
			this.TrackingGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically;
			this.TrackingGrid.Location = new System.Drawing.Point(6, 6);
			this.TrackingGrid.MultiSelect = false;
			this.TrackingGrid.Name = "TrackingGrid";
			this.TrackingGrid.ReadOnly = true;
			this.TrackingGrid.RowHeadersVisible = false;
			this.TrackingGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
			this.TrackingGrid.ShowCellToolTips = false;
			this.TrackingGrid.ShowEditingIcon = false;
			this.TrackingGrid.Size = new System.Drawing.Size(1248, 224);
			this.TrackingGrid.TabIndex = 41;
			//
			//TrackingBox
			//
			this.TrackingBox.Anchor = (System.Windows.Forms.AnchorStyles)(System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right);
			this.TrackingBox.BackColor = System.Drawing.SystemColors.Control;
			this.TrackingBox.Controls.Add(this.Panel3);
			this.TrackingBox.Font = new System.Drawing.Font("Consolas", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.TrackingBox.Location = new System.Drawing.Point(3, 116);
			this.TrackingBox.Name = "TrackingBox";
			this.TrackingBox.Size = new System.Drawing.Size(421, 165);
			this.TrackingBox.TabIndex = 41;
			this.TrackingBox.TabStop = false;
			this.TrackingBox.Text = "Tracking Info";
			this.TrackingBox.Visible = false;
			//
			//Panel3
			//
			this.Panel3.AutoScroll = true;
			this.Panel3.Controls.Add(this.txtCheckLocation);
			this.Panel3.Controls.Add(this.Label16);
			this.Panel3.Controls.Add(this.txtDueBack);
			this.Panel3.Controls.Add(this.lblDueBack);
			this.Panel3.Controls.Add(this.txtCheckUser);
			this.Panel3.Controls.Add(this.lblCheckUser);
			this.Panel3.Controls.Add(this.txtCheckTime);
			this.Panel3.Controls.Add(this.lblCheckTime);
			this.Panel3.Controls.Add(this.txtCheckOut);
			this.Panel3.Controls.Add(this.Label11);
			this.Panel3.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Panel3.Location = new System.Drawing.Point(3, 18);
			this.Panel3.Name = "Panel3";
			this.Panel3.Padding = new System.Windows.Forms.Padding(0, 0, 5, 0);
			this.Panel3.Size = new System.Drawing.Size(415, 144);
			this.Panel3.TabIndex = 58;
			//
			//txtCheckLocation
			//
			this.txtCheckLocation.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.txtCheckLocation.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)));
			this.txtCheckLocation.Font = new System.Drawing.Font("Consolas", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtCheckLocation.Location = new System.Drawing.Point(224, 19);
			this.txtCheckLocation.Name = "txtCheckLocation";
			this.txtCheckLocation.ReadOnly = true;
			this.txtCheckLocation.Size = new System.Drawing.Size(134, 22);
			this.txtCheckLocation.TabIndex = 57;
			this.txtCheckLocation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			//
			//Label16
			//
			this.Label16.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.Label16.AutoSize = true;
			this.Label16.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label16.Location = new System.Drawing.Point(221, 0);
			this.Label16.Name = "Label16";
			this.Label16.Size = new System.Drawing.Size(62, 16);
			this.Label16.TabIndex = 56;
			this.Label16.Text = "Location:";
			//
			//txtDueBack
			//
			this.txtDueBack.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.txtDueBack.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)));
			this.txtDueBack.Font = new System.Drawing.Font("Consolas", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtDueBack.Location = new System.Drawing.Point(142, 108);
			this.txtDueBack.Name = "txtDueBack";
			this.txtDueBack.ReadOnly = true;
			this.txtDueBack.Size = new System.Drawing.Size(134, 22);
			this.txtDueBack.TabIndex = 55;
			this.txtDueBack.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			//
			//lblDueBack
			//
			this.lblDueBack.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblDueBack.AutoSize = true;
			this.lblDueBack.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblDueBack.Location = new System.Drawing.Point(139, 89);
			this.lblDueBack.Name = "lblDueBack";
			this.lblDueBack.Size = new System.Drawing.Size(70, 16);
			this.lblDueBack.TabIndex = 54;
			this.lblDueBack.Text = "Due Back:";
			//
			//txtCheckUser
			//
			this.txtCheckUser.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.txtCheckUser.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)));
			this.txtCheckUser.Font = new System.Drawing.Font("Consolas", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtCheckUser.Location = new System.Drawing.Point(224, 65);
			this.txtCheckUser.Name = "txtCheckUser";
			this.txtCheckUser.ReadOnly = true;
			this.txtCheckUser.Size = new System.Drawing.Size(134, 22);
			this.txtCheckUser.TabIndex = 53;
			this.txtCheckUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			//
			//lblCheckUser
			//
			this.lblCheckUser.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblCheckUser.AutoSize = true;
			this.lblCheckUser.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblCheckUser.Location = new System.Drawing.Point(221, 46);
			this.lblCheckUser.Name = "lblCheckUser";
			this.lblCheckUser.Size = new System.Drawing.Size(101, 16);
			this.lblCheckUser.TabIndex = 52;
			this.lblCheckUser.Text = "CheckOut User:";
			//
			//txtCheckTime
			//
			this.txtCheckTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.txtCheckTime.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)));
			this.txtCheckTime.Font = new System.Drawing.Font("Consolas", 9f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtCheckTime.Location = new System.Drawing.Point(74, 65);
			this.txtCheckTime.Name = "txtCheckTime";
			this.txtCheckTime.ReadOnly = true;
			this.txtCheckTime.Size = new System.Drawing.Size(134, 22);
			this.txtCheckTime.TabIndex = 51;
			this.txtCheckTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			//
			//lblCheckTime
			//
			this.lblCheckTime.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.lblCheckTime.AutoSize = true;
			this.lblCheckTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.lblCheckTime.Location = new System.Drawing.Point(71, 46);
			this.lblCheckTime.Name = "lblCheckTime";
			this.lblCheckTime.Size = new System.Drawing.Size(103, 16);
			this.lblCheckTime.TabIndex = 50;
			this.lblCheckTime.Text = "CheckOut Time:";
			//
			//txtCheckOut
			//
			this.txtCheckOut.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.txtCheckOut.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)), Convert.ToInt32(Convert.ToByte(224)));
			this.txtCheckOut.Font = new System.Drawing.Font("Consolas", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.txtCheckOut.Location = new System.Drawing.Point(74, 19);
			this.txtCheckOut.Name = "txtCheckOut";
			this.txtCheckOut.ReadOnly = true;
			this.txtCheckOut.Size = new System.Drawing.Size(134, 22);
			this.txtCheckOut.TabIndex = 49;
			this.txtCheckOut.Text = "STATUS";
			this.txtCheckOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			//
			//Label11
			//
			this.Label11.Anchor = System.Windows.Forms.AnchorStyles.Top;
			this.Label11.AutoSize = true;
			this.Label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label11.Location = new System.Drawing.Point(71, 0);
			this.Label11.Name = "Label11";
			this.Label11.Size = new System.Drawing.Size(48, 16);
			this.Label11.TabIndex = 48;
			this.Label11.Text = "Status:";
			//
			//ToolTip1
			//
			this.ToolTip1.AutoPopDelay = 5000;
			this.ToolTip1.InitialDelay = 100;
			this.ToolTip1.ReshowDelay = 100;
			//
			//StatusStrip1
			//
			this.StatusStrip1.AutoSize = false;
			this.StatusStrip1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.StatusStrip1.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { this.StatusLabel });
			this.StatusStrip1.Location = new System.Drawing.Point(0, 680);
			this.StatusStrip1.Name = "StatusStrip1";
			this.StatusStrip1.Size = new System.Drawing.Size(1291, 22);
			this.StatusStrip1.Stretch = false;
			this.StatusStrip1.TabIndex = 45;
			this.StatusStrip1.Text = "StatusStrip1";
			//
			//StatusLabel
			//
			this.StatusLabel.Font = new System.Drawing.Font("Segoe UI", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.StatusLabel.Name = "StatusLabel";
			this.StatusLabel.Size = new System.Drawing.Size(0, 17);
			//
			//tmr_RDPRefresher
			//
			this.tmr_RDPRefresher.Interval = 1000;
			//
			//fieldErrorIcon
			//
			this.fieldErrorIcon.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
			this.fieldErrorIcon.ContainerControl = this;
			this.fieldErrorIcon.Icon = (System.Drawing.Icon)resources.GetObject("fieldErrorIcon.Icon");
			//
			//tsSaveModify
			//
			this.tsSaveModify.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(88)), Convert.ToInt32(Convert.ToByte(237)), Convert.ToInt32(Convert.ToByte(118)));
			this.tsSaveModify.CanOverflow = false;
			this.tsSaveModify.Dock = System.Windows.Forms.DockStyle.None;
			this.tsSaveModify.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.tsSaveModify.ImageScalingSize = new System.Drawing.Size(25, 25);
			this.tsSaveModify.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.cmdAccept_Tool,
				this.ToolStripSeparator3,
				this.cmdCancel_Tool,
				this.ToolStripSeparator2
			});
			this.tsSaveModify.Location = new System.Drawing.Point(3, 0);
			this.tsSaveModify.Name = "tsSaveModify";
			this.tsSaveModify.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.tsSaveModify.Size = new System.Drawing.Size(312, 37);
			this.tsSaveModify.TabIndex = 44;
			this.tsSaveModify.Text = "ToolStrip1";
			//
			//cmdAccept_Tool
			//
			this.cmdAccept_Tool.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdAccept_Tool.Image = Properties.Resources.CheckedBoxIcon;
			this.cmdAccept_Tool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cmdAccept_Tool.Name = "cmdAccept_Tool";
			this.cmdAccept_Tool.Padding = new System.Windows.Forms.Padding(50, 5, 5, 0);
			this.cmdAccept_Tool.Size = new System.Drawing.Size(146, 34);
			this.cmdAccept_Tool.Text = "Accept";
			//
			//ToolStripSeparator3
			//
			this.ToolStripSeparator3.Name = "ToolStripSeparator3";
			this.ToolStripSeparator3.Size = new System.Drawing.Size(6, 37);
			//
			//cmdCancel_Tool
			//
			this.cmdCancel_Tool.Font = new System.Drawing.Font("Segoe UI Semibold", 12f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.cmdCancel_Tool.Image = Properties.Resources.CloseCancelDeleteIcon;
			this.cmdCancel_Tool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.cmdCancel_Tool.Name = "cmdCancel_Tool";
			this.cmdCancel_Tool.Padding = new System.Windows.Forms.Padding(50, 5, 5, 0);
			this.cmdCancel_Tool.Size = new System.Drawing.Size(142, 34);
			this.cmdCancel_Tool.Text = "Cancel";
			//
			//ToolStripSeparator2
			//
			this.ToolStripSeparator2.Name = "ToolStripSeparator2";
			this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 37);
			//
			//FieldsPanel
			//
			this.FieldsPanel.Anchor = (System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right);
			this.FieldsPanel.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.FieldsPanel.Controls.Add(this.InfoDataSplitter);
			this.FieldsPanel.Location = new System.Drawing.Point(11, 8);
			this.FieldsPanel.Name = "FieldsPanel";
			this.FieldsPanel.Size = new System.Drawing.Size(1268, 288);
			this.FieldsPanel.TabIndex = 53;
			//
			//InfoDataSplitter
			//
			this.InfoDataSplitter.BackColor = System.Drawing.SystemColors.Control;
			this.InfoDataSplitter.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.InfoDataSplitter.Dock = System.Windows.Forms.DockStyle.Fill;
			this.InfoDataSplitter.FixedPanel = System.Windows.Forms.FixedPanel.Panel2;
			this.InfoDataSplitter.Location = new System.Drawing.Point(0, 0);
			this.InfoDataSplitter.Name = "InfoDataSplitter";
			//
			//InfoDataSplitter.Panel1
			//
			this.InfoDataSplitter.Panel1.Controls.Add(this.FieldTabs);
			//
			//InfoDataSplitter.Panel2
			//
			this.InfoDataSplitter.Panel2.Controls.Add(this.RemoteTrackingPanel);
			this.InfoDataSplitter.Panel2MinSize = 327;
			this.InfoDataSplitter.Size = new System.Drawing.Size(1268, 288);
			this.InfoDataSplitter.SplitterDistance = 833;
			this.InfoDataSplitter.TabIndex = 55;
			//
			//FieldTabs
			//
			this.FieldTabs.Controls.Add(this.AssetInfo);
			this.FieldTabs.Controls.Add(this.MiscInfo);
			this.FieldTabs.Dock = System.Windows.Forms.DockStyle.Fill;
			this.FieldTabs.Font = new System.Drawing.Font("Consolas", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.FieldTabs.HotTrack = true;
			this.FieldTabs.Location = new System.Drawing.Point(0, 0);
			this.FieldTabs.Multiline = true;
			this.FieldTabs.Name = "FieldTabs";
			this.FieldTabs.SelectedIndex = 0;
			this.FieldTabs.Size = new System.Drawing.Size(829, 284);
			this.FieldTabs.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.FieldTabs.TabIndex = 53;
			//
			//AssetInfo
			//
			this.AssetInfo.AutoScroll = true;
			this.AssetInfo.BackColor = System.Drawing.SystemColors.Control;
			this.AssetInfo.Controls.Add(this.pnlOtherFunctions);
			this.AssetInfo.Controls.Add(this.Label2);
			this.AssetInfo.Controls.Add(this.Label3);
			this.AssetInfo.Controls.Add(this.txtCurUser_View_REQ);
			this.AssetInfo.Controls.Add(this.txtSerial_View_REQ);
			this.AssetInfo.Controls.Add(this.txtAssetTag_View_REQ);
			this.AssetInfo.Controls.Add(this.cmdMunisSearch);
			this.AssetInfo.Controls.Add(this.Label12);
			this.AssetInfo.Controls.Add(this.Label1);
			this.AssetInfo.Controls.Add(this.txtPONumber);
			this.AssetInfo.Controls.Add(this.Label4);
			this.AssetInfo.Controls.Add(this.cmbEquipType_View_REQ);
			this.AssetInfo.Controls.Add(this.Label5);
			this.AssetInfo.Controls.Add(this.dtPurchaseDate_View_REQ);
			this.AssetInfo.Controls.Add(this.Label9);
			this.AssetInfo.Controls.Add(this.Label7);
			this.AssetInfo.Controls.Add(this.Label13);
			this.AssetInfo.Controls.Add(this.Label6);
			this.AssetInfo.Controls.Add(this.txtReplacementYear_View);
			this.AssetInfo.Controls.Add(this.cmbStatus_REQ);
			this.AssetInfo.Controls.Add(this.cmbLocation_View_REQ);
			this.AssetInfo.Controls.Add(this.Label8);
			this.AssetInfo.Controls.Add(this.cmbOSVersion_REQ);
			this.AssetInfo.Controls.Add(this.txtDescription_View_REQ);
			this.AssetInfo.Location = new System.Drawing.Point(4, 23);
			this.AssetInfo.Name = "AssetInfo";
			this.AssetInfo.Padding = new System.Windows.Forms.Padding(3);
			this.AssetInfo.Size = new System.Drawing.Size(821, 257);
			this.AssetInfo.TabIndex = 0;
			this.AssetInfo.Text = "Asset Info.";
			//
			//MiscInfo
			//
			this.MiscInfo.AutoScroll = true;
			this.MiscInfo.BackColor = System.Drawing.SystemColors.Control;
			this.MiscInfo.Controls.Add(this.ADPanel);
			this.MiscInfo.Controls.Add(this.iCloudTextBox);
			this.MiscInfo.Controls.Add(this.Label17);
			this.MiscInfo.Controls.Add(this.lblGUID);
			this.MiscInfo.Controls.Add(this.txtPhoneNumber);
			this.MiscInfo.Controls.Add(this.Label10);
			this.MiscInfo.Controls.Add(this.Label14);
			this.MiscInfo.Controls.Add(this.chkTrackable);
			this.MiscInfo.Controls.Add(this.txtHostname);
			this.MiscInfo.Controls.Add(this.Label15);
			this.MiscInfo.Cursor = System.Windows.Forms.Cursors.Default;
			this.MiscInfo.Location = new System.Drawing.Point(4, 23);
			this.MiscInfo.Name = "MiscInfo";
			this.MiscInfo.Padding = new System.Windows.Forms.Padding(3);
			this.MiscInfo.Size = new System.Drawing.Size(821, 257);
			this.MiscInfo.TabIndex = 1;
			this.MiscInfo.Text = "Misc.";
			//
			//ADPanel
			//
			this.ADPanel.Controls.Add(this.GroupBox1);
			this.ADPanel.Location = new System.Drawing.Point(473, 7);
			this.ADPanel.Name = "ADPanel";
			this.ADPanel.Size = new System.Drawing.Size(305, 226);
			this.ADPanel.TabIndex = 65;
			//
			//GroupBox1
			//
			this.GroupBox1.Controls.Add(this.Label22);
			this.GroupBox1.Controls.Add(this.ADCreatedTextBox);
			this.GroupBox1.Controls.Add(this.Label21);
			this.GroupBox1.Controls.Add(this.ADLastLoginTextBox);
			this.GroupBox1.Controls.Add(this.Label20);
			this.GroupBox1.Controls.Add(this.ADOSVerTextBox);
			this.GroupBox1.Controls.Add(this.Label19);
			this.GroupBox1.Controls.Add(this.ADOSTextBox);
			this.GroupBox1.Controls.Add(this.Label18);
			this.GroupBox1.Controls.Add(this.ADOUTextBox);
			this.GroupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GroupBox1.Location = new System.Drawing.Point(0, 0);
			this.GroupBox1.Name = "GroupBox1";
			this.GroupBox1.Size = new System.Drawing.Size(305, 226);
			this.GroupBox1.TabIndex = 0;
			this.GroupBox1.TabStop = false;
			this.GroupBox1.Text = "Active Directory Info:";
			//
			//Label22
			//
			this.Label22.AutoSize = true;
			this.Label22.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label22.Location = new System.Drawing.Point(24, 163);
			this.Label22.Name = "Label22";
			this.Label22.Size = new System.Drawing.Size(59, 16);
			this.Label22.TabIndex = 72;
			this.Label22.Text = "Created:";
			//
			//ADCreatedTextBox
			//
			this.ADCreatedTextBox.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.ADCreatedTextBox.Location = new System.Drawing.Point(27, 182);
			this.ADCreatedTextBox.Name = "ADCreatedTextBox";
			this.ADCreatedTextBox.Size = new System.Drawing.Size(244, 23);
			this.ADCreatedTextBox.TabIndex = 71;
			//
			//Label21
			//
			this.Label21.AutoSize = true;
			this.Label21.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label21.Location = new System.Drawing.Point(24, 118);
			this.Label21.Name = "Label21";
			this.Label21.Size = new System.Drawing.Size(72, 16);
			this.Label21.TabIndex = 70;
			this.Label21.Text = "Last Login:";
			//
			//ADLastLoginTextBox
			//
			this.ADLastLoginTextBox.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.ADLastLoginTextBox.Location = new System.Drawing.Point(27, 137);
			this.ADLastLoginTextBox.Name = "ADLastLoginTextBox";
			this.ADLastLoginTextBox.Size = new System.Drawing.Size(244, 23);
			this.ADLastLoginTextBox.TabIndex = 69;
			//
			//Label20
			//
			this.Label20.AutoSize = true;
			this.Label20.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label20.Location = new System.Drawing.Point(149, 73);
			this.Label20.Name = "Label20";
			this.Label20.Size = new System.Drawing.Size(79, 16);
			this.Label20.TabIndex = 68;
			this.Label20.Text = "OS Version:";
			//
			//ADOSVerTextBox
			//
			this.ADOSVerTextBox.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.ADOSVerTextBox.Location = new System.Drawing.Point(152, 92);
			this.ADOSVerTextBox.Name = "ADOSVerTextBox";
			this.ADOSVerTextBox.Size = new System.Drawing.Size(119, 23);
			this.ADOSVerTextBox.TabIndex = 67;
			//
			//Label19
			//
			this.Label19.AutoSize = true;
			this.Label19.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label19.Location = new System.Drawing.Point(24, 73);
			this.Label19.Name = "Label19";
			this.Label19.Size = new System.Drawing.Size(30, 16);
			this.Label19.TabIndex = 66;
			this.Label19.Text = "OS:";
			//
			//ADOSTextBox
			//
			this.ADOSTextBox.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.ADOSTextBox.Location = new System.Drawing.Point(27, 92);
			this.ADOSTextBox.Name = "ADOSTextBox";
			this.ADOSTextBox.Size = new System.Drawing.Size(119, 23);
			this.ADOSTextBox.TabIndex = 65;
			//
			//Label18
			//
			this.Label18.AutoSize = true;
			this.Label18.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label18.Location = new System.Drawing.Point(24, 28);
			this.Label18.Name = "Label18";
			this.Label18.Size = new System.Drawing.Size(61, 16);
			this.Label18.TabIndex = 64;
			this.Label18.Text = "OU Path:";
			//
			//ADOUTextBox
			//
			this.ADOUTextBox.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.ADOUTextBox.Location = new System.Drawing.Point(27, 47);
			this.ADOUTextBox.Name = "ADOUTextBox";
			this.ADOUTextBox.Size = new System.Drawing.Size(244, 23);
			this.ADOUTextBox.TabIndex = 63;
			//
			//iCloudTextBox
			//
			this.iCloudTextBox.Font = new System.Drawing.Font("Consolas", 9.75f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.iCloudTextBox.Location = new System.Drawing.Point(232, 42);
			this.iCloudTextBox.Name = "iCloudTextBox";
			this.iCloudTextBox.Size = new System.Drawing.Size(219, 23);
			this.iCloudTextBox.TabIndex = 60;
			//
			//Label17
			//
			this.Label17.AutoSize = true;
			this.Label17.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.Label17.Location = new System.Drawing.Point(229, 23);
			this.Label17.Name = "Label17";
			this.Label17.Size = new System.Drawing.Size(100, 16);
			this.Label17.TabIndex = 61;
			this.Label17.Text = "iCloud Account:";
			//
			//RemoteTrackingPanel
			//
			this.RemoteTrackingPanel.Controls.Add(this.RemoteToolsBox);
			this.RemoteTrackingPanel.Controls.Add(this.TrackingBox);
			this.RemoteTrackingPanel.Dock = System.Windows.Forms.DockStyle.Fill;
			this.RemoteTrackingPanel.Location = new System.Drawing.Point(0, 0);
			this.RemoteTrackingPanel.Name = "RemoteTrackingPanel";
			this.RemoteTrackingPanel.Size = new System.Drawing.Size(427, 284);
			this.RemoteTrackingPanel.TabIndex = 54;
			//
			//ToolStripContainer1
			//
			this.ToolStripContainer1.BottomToolStripPanelVisible = false;
			//
			//ToolStripContainer1.ContentPanel
			//
			this.ToolStripContainer1.ContentPanel.Controls.Add(this.TabControl1);
			this.ToolStripContainer1.ContentPanel.Controls.Add(this.FieldsPanel);
			this.ToolStripContainer1.ContentPanel.Size = new System.Drawing.Size(1291, 569);
			this.ToolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.ToolStripContainer1.LeftToolStripPanelVisible = false;
			this.ToolStripContainer1.Location = new System.Drawing.Point(0, 0);
			this.ToolStripContainer1.Name = "ToolStripContainer1";
			this.ToolStripContainer1.RightToolStripPanelVisible = false;
			this.ToolStripContainer1.Size = new System.Drawing.Size(1291, 680);
			this.ToolStripContainer1.TabIndex = 54;
			this.ToolStripContainer1.Text = "ToolStripContainer1";
			//
			//ToolStripContainer1.TopToolStripPanel
			//
			this.ToolStripContainer1.TopToolStripPanel.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.ToolStripContainer1.TopToolStripPanel.Controls.Add(this.tsSaveModify);
			this.ToolStripContainer1.TopToolStripPanel.Controls.Add(this.tsTracking);
			this.ToolStripContainer1.TopToolStripPanel.Controls.Add(this.ToolStrip1);
			//
			//tsTracking
			//
			this.tsTracking.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.tsTracking.Dock = System.Windows.Forms.DockStyle.None;
			this.tsTracking.ImageScalingSize = new System.Drawing.Size(25, 25);
			this.tsTracking.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.ToolStripDropDownButton1,
				this.ToolStripSeparator4
			});
			this.tsTracking.Location = new System.Drawing.Point(16, 37);
			this.tsTracking.Name = "tsTracking";
			this.tsTracking.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.tsTracking.Size = new System.Drawing.Size(134, 37);
			this.tsTracking.TabIndex = 46;
			//
			//ToolStripDropDownButton1
			//
			this.ToolStripDropDownButton1.AutoSize = false;
			this.ToolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.CheckOutTool,
				this.CheckInTool
			});
			this.ToolStripDropDownButton1.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.ToolStripDropDownButton1.Image = Properties.Resources.CheckOutIcon;
			this.ToolStripDropDownButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1";
			this.ToolStripDropDownButton1.Padding = new System.Windows.Forms.Padding(10, 0, 0, 0);
			this.ToolStripDropDownButton1.Size = new System.Drawing.Size(116, 34);
			this.ToolStripDropDownButton1.Text = "Tracking";
			//
			//CheckOutTool
			//
			this.CheckOutTool.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.CheckOutTool.Image = Properties.Resources.CheckedBoxRedIcon;
			this.CheckOutTool.Name = "CheckOutTool";
			this.CheckOutTool.Size = new System.Drawing.Size(135, 22);
			this.CheckOutTool.Text = "Check Out";
			//
			//CheckInTool
			//
			this.CheckInTool.Font = new System.Drawing.Font("Segoe UI", 9f, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.CheckInTool.Image = Properties.Resources.CheckedBoxGreenIcon;
			this.CheckInTool.Name = "CheckInTool";
			this.CheckInTool.Size = new System.Drawing.Size(135, 22);
			this.CheckInTool.Text = "Check In";
			//
			//ToolStripSeparator4
			//
			this.ToolStripSeparator4.Name = "ToolStripSeparator4";
			this.ToolStripSeparator4.Size = new System.Drawing.Size(6, 37);
			//
			//ToolStrip1
			//
			this.ToolStrip1.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.ToolStrip1.Dock = System.Windows.Forms.DockStyle.None;
			this.ToolStrip1.Font = new System.Drawing.Font("Segoe UI", 12f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
			this.ToolStrip1.ImageScalingSize = new System.Drawing.Size(25, 25);
			this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.tsbModify,
				this.tsbNewNote,
				this.tsbDeleteDevice,
				this.RefreshToolStripButton,
				this.ToolStripSeparator1,
				this.AttachmentTool,
				this.ToolStripSeparator7,
				this.ToolStripDropDownButton2,
				this.ToolStripSeparator9
			});
			this.ToolStrip1.Location = new System.Drawing.Point(3, 74);
			this.ToolStrip1.Name = "ToolStrip1";
			this.ToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
			this.ToolStrip1.Size = new System.Drawing.Size(404, 37);
			this.ToolStrip1.TabIndex = 45;
			this.ToolStrip1.Text = "MyToolStrip1";
			//
			//tsbModify
			//
			this.tsbModify.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbModify.Image = Properties.Resources.EditIcon;
			this.tsbModify.Name = "tsbModify";
			this.tsbModify.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
			this.tsbModify.Size = new System.Drawing.Size(39, 34);
			this.tsbModify.Text = "Modify";
			//
			//tsbNewNote
			//
			this.tsbNewNote.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbNewNote.Image = Properties.Resources.AddNoteIcon;
			this.tsbNewNote.Name = "tsbNewNote";
			this.tsbNewNote.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
			this.tsbNewNote.Size = new System.Drawing.Size(39, 34);
			this.tsbNewNote.Text = "Add Note";
			//
			//tsbDeleteDevice
			//
			this.tsbDeleteDevice.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.tsbDeleteDevice.Image = Properties.Resources.DeleteRedIcon;
			this.tsbDeleteDevice.Name = "tsbDeleteDevice";
			this.tsbDeleteDevice.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
			this.tsbDeleteDevice.Size = new System.Drawing.Size(39, 34);
			this.tsbDeleteDevice.Text = "Delete Device";
			//
			//RefreshToolStripButton
			//
			this.RefreshToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
			this.RefreshToolStripButton.Image = Properties.Resources.RefreshIcon;
			this.RefreshToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
			this.RefreshToolStripButton.Name = "RefreshToolStripButton";
			this.RefreshToolStripButton.Size = new System.Drawing.Size(29, 34);
			this.RefreshToolStripButton.Text = "ToolStripButton1";
			this.RefreshToolStripButton.ToolTipText = "Refresh";
			//
			//ToolStripSeparator1
			//
			this.ToolStripSeparator1.Name = "ToolStripSeparator1";
			this.ToolStripSeparator1.Size = new System.Drawing.Size(6, 37);
			//
			//AttachmentTool
			//
			this.AttachmentTool.Image = Properties.Resources.PaperClipIcon;
			this.AttachmentTool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
			this.AttachmentTool.Name = "AttachmentTool";
			this.AttachmentTool.Padding = new System.Windows.Forms.Padding(5, 5, 5, 0);
			this.AttachmentTool.Size = new System.Drawing.Size(39, 34);
			//
			//ToolStripSeparator7
			//
			this.ToolStripSeparator7.Name = "ToolStripSeparator7";
			this.ToolStripSeparator7.Size = new System.Drawing.Size(6, 37);
			//
			//ToolStripDropDownButton2
			//
			this.ToolStripDropDownButton2.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
				this.tsmAssetInputForm,
				this.tsmAssetTransferForm,
				this.AssetDisposalForm
			});
			this.ToolStripDropDownButton2.Image = Properties.Resources.FormIcon;
			this.ToolStripDropDownButton2.Name = "ToolStripDropDownButton2";
			this.ToolStripDropDownButton2.Size = new System.Drawing.Size(189, 34);
			this.ToolStripDropDownButton2.Text = "Asset Control Forms";
			//
			//tsmAssetInputForm
			//
			this.tsmAssetInputForm.Image = Properties.Resources.ImportIcon;
			this.tsmAssetInputForm.Name = "tsmAssetInputForm";
			this.tsmAssetInputForm.Size = new System.Drawing.Size(230, 32);
			this.tsmAssetInputForm.Text = "Asset Input Form";
			//
			//tsmAssetTransferForm
			//
			this.tsmAssetTransferForm.Image = Properties.Resources.TransferArrowsIcon;
			this.tsmAssetTransferForm.Name = "tsmAssetTransferForm";
			this.tsmAssetTransferForm.Size = new System.Drawing.Size(230, 32);
			this.tsmAssetTransferForm.Text = "Asset Transfer Form";
			//
			//AssetDisposalForm
			//
			this.AssetDisposalForm.Image = Properties.Resources.TrashIcon;
			this.AssetDisposalForm.Name = "AssetDisposalForm";
			this.AssetDisposalForm.Size = new System.Drawing.Size(230, 32);
			this.AssetDisposalForm.Text = "Asset Disposal Form";
			//
			//ToolStripSeparator9
			//
			this.ToolStripSeparator9.Name = "ToolStripSeparator9";
			this.ToolStripSeparator9.Size = new System.Drawing.Size(6, 37);
			//
			//ViewDeviceForm
			//
			this.AutoScaleDimensions = new System.Drawing.SizeF(6f, 13f);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackColor = System.Drawing.Color.FromArgb(Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)), Convert.ToInt32(Convert.ToByte(232)));
			this.ClientSize = new System.Drawing.Size(1291, 702);
			this.Controls.Add(this.ToolStripContainer1);
			this.Controls.Add(this.StatusStrip1);
			this.DoubleBuffered = true;
			this.MinimumSize = new System.Drawing.Size(1161, 559);
			this.Name = "ViewDeviceForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "View";
			this.pnlOtherFunctions.ResumeLayout(false);
			this.RemoteToolsBox.ResumeLayout(false);
			this.FlowLayoutPanel1.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.cmdRestart).EndInit();
			this.RightClickMenu.ResumeLayout(false);
			this.TabControl1.ResumeLayout(false);
			this.HistoryTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.DataGridHistory).EndInit();
			this.TrackingTab.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.TrackingGrid).EndInit();
			this.TrackingBox.ResumeLayout(false);
			this.Panel3.ResumeLayout(false);
			this.Panel3.PerformLayout();
			this.StatusStrip1.ResumeLayout(false);
			this.StatusStrip1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)this.fieldErrorIcon).EndInit();
			this.tsSaveModify.ResumeLayout(false);
			this.tsSaveModify.PerformLayout();
			this.FieldsPanel.ResumeLayout(false);
			this.InfoDataSplitter.Panel1.ResumeLayout(false);
			this.InfoDataSplitter.Panel2.ResumeLayout(false);
			((System.ComponentModel.ISupportInitialize)this.InfoDataSplitter).EndInit();
			this.InfoDataSplitter.ResumeLayout(false);
			this.FieldTabs.ResumeLayout(false);
			this.AssetInfo.ResumeLayout(false);
			this.AssetInfo.PerformLayout();
			this.MiscInfo.ResumeLayout(false);
			this.MiscInfo.PerformLayout();
			this.ADPanel.ResumeLayout(false);
			this.GroupBox1.ResumeLayout(false);
			this.GroupBox1.PerformLayout();
			this.RemoteTrackingPanel.ResumeLayout(false);
			this.ToolStripContainer1.ContentPanel.ResumeLayout(false);
			this.ToolStripContainer1.TopToolStripPanel.ResumeLayout(false);
			this.ToolStripContainer1.TopToolStripPanel.PerformLayout();
			this.ToolStripContainer1.ResumeLayout(false);
			this.ToolStripContainer1.PerformLayout();
			this.tsTracking.ResumeLayout(false);
			this.tsTracking.PerformLayout();
			this.ToolStrip1.ResumeLayout(false);
			this.ToolStrip1.PerformLayout();
			this.ResumeLayout(false);

		}
		internal Label Label9;
		private ComboBox withEventsField_cmbStatus_REQ;
		internal ComboBox cmbStatus_REQ {
			get { return withEventsField_cmbStatus_REQ; }
			set {
				if (withEventsField_cmbStatus_REQ != null) {
					withEventsField_cmbStatus_REQ.DropDown -= cmbStatus_REQ_DropDown;
					withEventsField_cmbStatus_REQ.SelectedIndexChanged -= cmbStatus_REQ_SelectedIndexChanged;
				}
				withEventsField_cmbStatus_REQ = value;
				if (withEventsField_cmbStatus_REQ != null) {
					withEventsField_cmbStatus_REQ.DropDown += cmbStatus_REQ_DropDown;
					withEventsField_cmbStatus_REQ.SelectedIndexChanged += cmbStatus_REQ_SelectedIndexChanged;
				}
			}
		}
		internal Label Label8;
		private ComboBox withEventsField_cmbOSVersion_REQ;
		internal ComboBox cmbOSVersion_REQ {
			get { return withEventsField_cmbOSVersion_REQ; }
			set {
				if (withEventsField_cmbOSVersion_REQ != null) {
					withEventsField_cmbOSVersion_REQ.DropDown -= cmbOSVersion_REQ_DropDown;
					withEventsField_cmbOSVersion_REQ.SelectedIndexChanged -= cmbOSVersion_REQ_SelectedIndexChanged;
				}
				withEventsField_cmbOSVersion_REQ = value;
				if (withEventsField_cmbOSVersion_REQ != null) {
					withEventsField_cmbOSVersion_REQ.DropDown += cmbOSVersion_REQ_DropDown;
					withEventsField_cmbOSVersion_REQ.SelectedIndexChanged += cmbOSVersion_REQ_SelectedIndexChanged;
				}
			}
		}
		internal Label Label13;
		private ComboBox withEventsField_cmbEquipType_View_REQ;
		internal ComboBox cmbEquipType_View_REQ {
			get { return withEventsField_cmbEquipType_View_REQ; }
			set {
				if (withEventsField_cmbEquipType_View_REQ != null) {
					withEventsField_cmbEquipType_View_REQ.DropDown -= cmbEquipType_View_REQ_DropDown;
					withEventsField_cmbEquipType_View_REQ.SelectedIndexChanged -= cmbEquipType_View_REQ_SelectedIndexChanged;
				}
				withEventsField_cmbEquipType_View_REQ = value;
				if (withEventsField_cmbEquipType_View_REQ != null) {
					withEventsField_cmbEquipType_View_REQ.DropDown += cmbEquipType_View_REQ_DropDown;
					withEventsField_cmbEquipType_View_REQ.SelectedIndexChanged += cmbEquipType_View_REQ_SelectedIndexChanged;
				}
			}
		}
		internal Label Label7;
		internal TextBox txtReplacementYear_View;
		internal Label Label6;
		private DateTimePicker withEventsField_dtPurchaseDate_View_REQ;
		internal DateTimePicker dtPurchaseDate_View_REQ {
			get { return withEventsField_dtPurchaseDate_View_REQ; }
			set {
				if (withEventsField_dtPurchaseDate_View_REQ != null) {
					withEventsField_dtPurchaseDate_View_REQ.ValueChanged -= dtPurchaseDate_View_REQ_ValueChanged;
				}
				withEventsField_dtPurchaseDate_View_REQ = value;
				if (withEventsField_dtPurchaseDate_View_REQ != null) {
					withEventsField_dtPurchaseDate_View_REQ.ValueChanged += dtPurchaseDate_View_REQ_ValueChanged;
				}
			}
		}
		internal Label Label5;
		private ComboBox withEventsField_cmbLocation_View_REQ;
		internal ComboBox cmbLocation_View_REQ {
			get { return withEventsField_cmbLocation_View_REQ; }
			set {
				if (withEventsField_cmbLocation_View_REQ != null) {
					withEventsField_cmbLocation_View_REQ.DropDown -= cmbLocation_View_REQ_DropDown;
					withEventsField_cmbLocation_View_REQ.SelectedIndexChanged -= cmbLocation_View_REQ_SelectedIndexChanged;
				}
				withEventsField_cmbLocation_View_REQ = value;
				if (withEventsField_cmbLocation_View_REQ != null) {
					withEventsField_cmbLocation_View_REQ.DropDown += cmbLocation_View_REQ_DropDown;
					withEventsField_cmbLocation_View_REQ.SelectedIndexChanged += cmbLocation_View_REQ_SelectedIndexChanged;
				}
			}
		}
		internal Label Label4;
		private TextBox withEventsField_txtDescription_View_REQ;
		internal TextBox txtDescription_View_REQ {
			get { return withEventsField_txtDescription_View_REQ; }
			set {
				if (withEventsField_txtDescription_View_REQ != null) {
					withEventsField_txtDescription_View_REQ.TextChanged -= txtDescription_View_REQ_TextChanged;
				}
				withEventsField_txtDescription_View_REQ = value;
				if (withEventsField_txtDescription_View_REQ != null) {
					withEventsField_txtDescription_View_REQ.TextChanged += txtDescription_View_REQ_TextChanged;
				}
			}
		}
		internal Label Label3;
		private TextBox withEventsField_txtCurUser_View_REQ;
		internal TextBox txtCurUser_View_REQ {
			get { return withEventsField_txtCurUser_View_REQ; }
			set {
				if (withEventsField_txtCurUser_View_REQ != null) {
					withEventsField_txtCurUser_View_REQ.TextChanged -= txtCurUser_View_REQ_TextChanged;
				}
				withEventsField_txtCurUser_View_REQ = value;
				if (withEventsField_txtCurUser_View_REQ != null) {
					withEventsField_txtCurUser_View_REQ.TextChanged += txtCurUser_View_REQ_TextChanged;
				}
			}
		}
		internal Label Label2;
		private TextBox withEventsField_txtSerial_View_REQ;
		internal TextBox txtSerial_View_REQ {
			get { return withEventsField_txtSerial_View_REQ; }
			set {
				if (withEventsField_txtSerial_View_REQ != null) {
					withEventsField_txtSerial_View_REQ.TextChanged -= txtSerial_View_REQ_TextChanged;
				}
				withEventsField_txtSerial_View_REQ = value;
				if (withEventsField_txtSerial_View_REQ != null) {
					withEventsField_txtSerial_View_REQ.TextChanged += txtSerial_View_REQ_TextChanged;
				}
			}
		}
		internal Label Label1;
		private TextBox withEventsField_txtAssetTag_View_REQ;
		internal TextBox txtAssetTag_View_REQ {
			get { return withEventsField_txtAssetTag_View_REQ; }
			set {
				if (withEventsField_txtAssetTag_View_REQ != null) {
					withEventsField_txtAssetTag_View_REQ.TextChanged -= txtAssetTag_View_REQ_TextChanged;
				}
				withEventsField_txtAssetTag_View_REQ = value;
				if (withEventsField_txtAssetTag_View_REQ != null) {
					withEventsField_txtAssetTag_View_REQ.TextChanged += txtAssetTag_View_REQ_TextChanged;
				}
			}
		}
		internal Label Label10;
		internal ContextMenuStrip RightClickMenu;
		private ToolStripMenuItem withEventsField_DeleteEntryToolStripMenuItem;
		internal ToolStripMenuItem DeleteEntryToolStripMenuItem {
			get { return withEventsField_DeleteEntryToolStripMenuItem; }
			set {
				if (withEventsField_DeleteEntryToolStripMenuItem != null) {
					withEventsField_DeleteEntryToolStripMenuItem.Click -= DeleteEntryToolStripMenuItem_Click;
				}
				withEventsField_DeleteEntryToolStripMenuItem = value;
				if (withEventsField_DeleteEntryToolStripMenuItem != null) {
					withEventsField_DeleteEntryToolStripMenuItem.Click += DeleteEntryToolStripMenuItem_Click;
				}
			}
		}
		internal CheckBox chkTrackable;
		private TabControl withEventsField_TabControl1;
		internal TabControl TabControl1 {
			get { return withEventsField_TabControl1; }
			set {
				if (withEventsField_TabControl1 != null) {
					withEventsField_TabControl1.MouseDown -= TabControl1_MouseDown;
				}
				withEventsField_TabControl1 = value;
				if (withEventsField_TabControl1 != null) {
					withEventsField_TabControl1.MouseDown += TabControl1_MouseDown;
				}
			}
		}
		internal TabPage HistoryTab;
		private DataGridView withEventsField_DataGridHistory;
		internal DataGridView DataGridHistory {
			get { return withEventsField_DataGridHistory; }
			set {
				if (withEventsField_DataGridHistory != null) {
					withEventsField_DataGridHistory.CellDoubleClick -= DataGridHistory_CellDoubleClick;
					withEventsField_DataGridHistory.CellEnter -= DataGridHistory_CellEnter;
					withEventsField_DataGridHistory.CellLeave -= DataGridHistory_CellLeave;
					withEventsField_DataGridHistory.CellMouseDown -= DataGridHistory_CellMouseDown;
				}
				withEventsField_DataGridHistory = value;
				if (withEventsField_DataGridHistory != null) {
					withEventsField_DataGridHistory.CellDoubleClick += DataGridHistory_CellDoubleClick;
					withEventsField_DataGridHistory.CellEnter += DataGridHistory_CellEnter;
					withEventsField_DataGridHistory.CellLeave += DataGridHistory_CellLeave;
					withEventsField_DataGridHistory.CellMouseDown += DataGridHistory_CellMouseDown;
				}
			}
		}
		internal TabPage TrackingTab;
		private DataGridView withEventsField_TrackingGrid;
		internal DataGridView TrackingGrid {
			get { return withEventsField_TrackingGrid; }
			set {
				if (withEventsField_TrackingGrid != null) {
					withEventsField_TrackingGrid.CellDoubleClick -= TrackingGrid_CellDoubleClick;
					withEventsField_TrackingGrid.Paint -= TrackingGrid_Paint;
					withEventsField_TrackingGrid.RowPrePaint -= TrackingGrid_RowPrePaint;
				}
				withEventsField_TrackingGrid = value;
				if (withEventsField_TrackingGrid != null) {
					withEventsField_TrackingGrid.CellDoubleClick += TrackingGrid_CellDoubleClick;
					withEventsField_TrackingGrid.Paint += TrackingGrid_Paint;
					withEventsField_TrackingGrid.RowPrePaint += TrackingGrid_RowPrePaint;
				}
			}
		}
		internal GroupBox TrackingBox;
		internal TextBox txtCheckOut;
		internal Label Label11;
		internal TextBox txtCheckTime;
		internal Label lblCheckTime;
		internal TextBox txtCheckUser;
		internal Label lblCheckUser;
		internal TextBox txtCheckLocation;
		internal Label Label16;
		internal TextBox txtDueBack;
		internal Label lblDueBack;
		internal ToolTip ToolTip1;
		internal OneClickToolStrip tsSaveModify;
		internal ToolStripSeparator ToolStripSeparator3;
		internal StatusStrip StatusStrip1;
		private ToolStripButton withEventsField_cmdCancel_Tool;
		internal ToolStripButton cmdCancel_Tool {
			get { return withEventsField_cmdCancel_Tool; }
			set {
				if (withEventsField_cmdCancel_Tool != null) {
					withEventsField_cmdCancel_Tool.Click -= cmdCancel_Tool_Click;
				}
				withEventsField_cmdCancel_Tool = value;
				if (withEventsField_cmdCancel_Tool != null) {
					withEventsField_cmdCancel_Tool.Click += cmdCancel_Tool_Click;
				}
			}
		}
		private ToolStripButton withEventsField_cmdAccept_Tool;
		internal ToolStripButton cmdAccept_Tool {
			get { return withEventsField_cmdAccept_Tool; }
			set {
				if (withEventsField_cmdAccept_Tool != null) {
					withEventsField_cmdAccept_Tool.Click -= cmdAccept_Tool_Click;
				}
				withEventsField_cmdAccept_Tool = value;
				if (withEventsField_cmdAccept_Tool != null) {
					withEventsField_cmdAccept_Tool.Click += cmdAccept_Tool_Click;
				}
			}
		}
		private Button withEventsField_cmdMunisInfo;
		internal Button cmdMunisInfo {
			get { return withEventsField_cmdMunisInfo; }
			set {
				if (withEventsField_cmdMunisInfo != null) {
					withEventsField_cmdMunisInfo.Click -= cmdMunisInfo_Click;
				}
				withEventsField_cmdMunisInfo = value;
				if (withEventsField_cmdMunisInfo != null) {
					withEventsField_cmdMunisInfo.Click += cmdMunisInfo_Click;
				}
			}
		}
		private Button withEventsField_cmdRDP;
		internal Button cmdRDP {
			get { return withEventsField_cmdRDP; }
			set {
				if (withEventsField_cmdRDP != null) {
					withEventsField_cmdRDP.Click -= cmdRDP_Click;
				}
				withEventsField_cmdRDP = value;
				if (withEventsField_cmdRDP != null) {
					withEventsField_cmdRDP.Click += cmdRDP_Click;
				}
			}
		}
		internal Label Label12;
		internal TextBox txtPONumber;
		private Timer withEventsField_tmr_RDPRefresher;
		internal Timer tmr_RDPRefresher {
			get { return withEventsField_tmr_RDPRefresher; }
			set {
				if (withEventsField_tmr_RDPRefresher != null) {
					withEventsField_tmr_RDPRefresher.Tick -= tmr_RDPRefresher_Tick;
				}
				withEventsField_tmr_RDPRefresher = value;
				if (withEventsField_tmr_RDPRefresher != null) {
					withEventsField_tmr_RDPRefresher.Tick += tmr_RDPRefresher_Tick;
				}
			}
		}
		private Button withEventsField_cmdSibiLink;
		internal Button cmdSibiLink {
			get { return withEventsField_cmdSibiLink; }
			set {
				if (withEventsField_cmdSibiLink != null) {
					withEventsField_cmdSibiLink.Click -= cmdSibiLink_Click;
				}
				withEventsField_cmdSibiLink = value;
				if (withEventsField_cmdSibiLink != null) {
					withEventsField_cmdSibiLink.Click += cmdSibiLink_Click;
				}
			}
		}
		internal Panel pnlOtherFunctions;
		internal ErrorProvider fieldErrorIcon;
		private Button withEventsField_cmdBrowseFiles;
		internal Button cmdBrowseFiles {
			get { return withEventsField_cmdBrowseFiles; }
			set {
				if (withEventsField_cmdBrowseFiles != null) {
					withEventsField_cmdBrowseFiles.Click -= cmdBrowseFiles_Click;
				}
				withEventsField_cmdBrowseFiles = value;
				if (withEventsField_cmdBrowseFiles != null) {
					withEventsField_cmdBrowseFiles.Click += cmdBrowseFiles_Click;
				}
			}
		}
		internal GroupBox RemoteToolsBox;
		private Button withEventsField_cmdMunisSearch;
		internal Button cmdMunisSearch {
			get { return withEventsField_cmdMunisSearch; }
			set {
				if (withEventsField_cmdMunisSearch != null) {
					withEventsField_cmdMunisSearch.Click -= cmdMunisSearch_Click;
				}
				withEventsField_cmdMunisSearch = value;
				if (withEventsField_cmdMunisSearch != null) {
					withEventsField_cmdMunisSearch.Click += cmdMunisSearch_Click;
				}
			}
		}
		private Label withEventsField_lblGUID;
		internal Label lblGUID {
			get { return withEventsField_lblGUID; }
			set {
				if (withEventsField_lblGUID != null) {
					withEventsField_lblGUID.Click -= lblGUID_Click;
				}
				withEventsField_lblGUID = value;
				if (withEventsField_lblGUID != null) {
					withEventsField_lblGUID.Click += lblGUID_Click;
				}
			}
		}
		private Button withEventsField_cmdShowIP;
		internal Button cmdShowIP {
			get { return withEventsField_cmdShowIP; }
			set {
				if (withEventsField_cmdShowIP != null) {
					withEventsField_cmdShowIP.Click -= cmdShowIP_Click;
				}
				withEventsField_cmdShowIP = value;
				if (withEventsField_cmdShowIP != null) {
					withEventsField_cmdShowIP.Click += cmdShowIP_Click;
				}
			}
		}
		internal Label Label14;
		private Button withEventsField_cmdGKUpdate;
		internal Button cmdGKUpdate {
			get { return withEventsField_cmdGKUpdate; }
			set {
				if (withEventsField_cmdGKUpdate != null) {
					withEventsField_cmdGKUpdate.Click -= cmdGKUpdate_Click;
				}
				withEventsField_cmdGKUpdate = value;
				if (withEventsField_cmdGKUpdate != null) {
					withEventsField_cmdGKUpdate.Click += cmdGKUpdate_Click;
				}
			}
		}
		private MaskedTextBox withEventsField_txtPhoneNumber;
		internal MaskedTextBox txtPhoneNumber {
			get { return withEventsField_txtPhoneNumber; }
			set {
				if (withEventsField_txtPhoneNumber != null) {
					withEventsField_txtPhoneNumber.Leave -= txtPhoneNumber_Leave;
					withEventsField_txtPhoneNumber.TextChanged -= txtPhoneNumber_TextChanged;
				}
				withEventsField_txtPhoneNumber = value;
				if (withEventsField_txtPhoneNumber != null) {
					withEventsField_txtPhoneNumber.Leave += txtPhoneNumber_Leave;
					withEventsField_txtPhoneNumber.TextChanged += txtPhoneNumber_TextChanged;
				}
			}
		}
		internal Panel FieldsPanel;
		internal ToolStripContainer ToolStripContainer1;
		internal OneClickToolStrip ToolStrip1;
		private ToolStripButton withEventsField_tsbModify;
		internal ToolStripButton tsbModify {
			get { return withEventsField_tsbModify; }
			set {
				if (withEventsField_tsbModify != null) {
					withEventsField_tsbModify.Click -= tsbModify_Click;
				}
				withEventsField_tsbModify = value;
				if (withEventsField_tsbModify != null) {
					withEventsField_tsbModify.Click += tsbModify_Click;
				}
			}
		}
		private ToolStripButton withEventsField_tsbNewNote;
		internal ToolStripButton tsbNewNote {
			get { return withEventsField_tsbNewNote; }
			set {
				if (withEventsField_tsbNewNote != null) {
					withEventsField_tsbNewNote.Click -= tsbNewNote_Click;
				}
				withEventsField_tsbNewNote = value;
				if (withEventsField_tsbNewNote != null) {
					withEventsField_tsbNewNote.Click += tsbNewNote_Click;
				}
			}
		}
		private ToolStripButton withEventsField_tsbDeleteDevice;
		internal ToolStripButton tsbDeleteDevice {
			get { return withEventsField_tsbDeleteDevice; }
			set {
				if (withEventsField_tsbDeleteDevice != null) {
					withEventsField_tsbDeleteDevice.Click -= tsbDeleteDevice_Click;
				}
				withEventsField_tsbDeleteDevice = value;
				if (withEventsField_tsbDeleteDevice != null) {
					withEventsField_tsbDeleteDevice.Click += tsbDeleteDevice_Click;
				}
			}
		}
		private ToolStripButton withEventsField_AttachmentTool;
		internal ToolStripButton AttachmentTool {
			get { return withEventsField_AttachmentTool; }
			set {
				if (withEventsField_AttachmentTool != null) {
					withEventsField_AttachmentTool.Click -= AttachmentTool_Click;
				}
				withEventsField_AttachmentTool = value;
				if (withEventsField_AttachmentTool != null) {
					withEventsField_AttachmentTool.Click += AttachmentTool_Click;
				}
			}
		}
		internal ToolStripSeparator ToolStripSeparator7;
		internal ToolStripDropDownButton ToolStripDropDownButton2;
		private ToolStripMenuItem withEventsField_tsmAssetInputForm;
		internal ToolStripMenuItem tsmAssetInputForm {
			get { return withEventsField_tsmAssetInputForm; }
			set {
				if (withEventsField_tsmAssetInputForm != null) {
					withEventsField_tsmAssetInputForm.Click -= tsmAssetInputForm_Click;
				}
				withEventsField_tsmAssetInputForm = value;
				if (withEventsField_tsmAssetInputForm != null) {
					withEventsField_tsmAssetInputForm.Click += tsmAssetInputForm_Click;
				}
			}
		}
		private ToolStripMenuItem withEventsField_tsmAssetTransferForm;
		internal ToolStripMenuItem tsmAssetTransferForm {
			get { return withEventsField_tsmAssetTransferForm; }
			set {
				if (withEventsField_tsmAssetTransferForm != null) {
					withEventsField_tsmAssetTransferForm.Click -= tsmAssetTransferForm_Click;
				}
				withEventsField_tsmAssetTransferForm = value;
				if (withEventsField_tsmAssetTransferForm != null) {
					withEventsField_tsmAssetTransferForm.Click += tsmAssetTransferForm_Click;
				}
			}
		}
		private ToolStripMenuItem withEventsField_AssetDisposalForm;
		internal ToolStripMenuItem AssetDisposalForm {
			get { return withEventsField_AssetDisposalForm; }
			set {
				if (withEventsField_AssetDisposalForm != null) {
					withEventsField_AssetDisposalForm.Click -= AssetDisposalFormToolStripMenuItem_Click;
				}
				withEventsField_AssetDisposalForm = value;
				if (withEventsField_AssetDisposalForm != null) {
					withEventsField_AssetDisposalForm.Click += AssetDisposalFormToolStripMenuItem_Click;
				}
			}
		}
		internal ToolStripSeparator ToolStripSeparator9;
		internal ToolStripSeparator ToolStripSeparator1;
		internal ToolStrip tsTracking;
		internal ToolStripDropDownButton ToolStripDropDownButton1;
		private ToolStripMenuItem withEventsField_CheckOutTool;
		internal ToolStripMenuItem CheckOutTool {
			get { return withEventsField_CheckOutTool; }
			set {
				if (withEventsField_CheckOutTool != null) {
					withEventsField_CheckOutTool.Click -= CheckOutTool_Click;
				}
				withEventsField_CheckOutTool = value;
				if (withEventsField_CheckOutTool != null) {
					withEventsField_CheckOutTool.Click += CheckOutTool_Click;
				}
			}
		}
		private ToolStripMenuItem withEventsField_CheckInTool;
		internal ToolStripMenuItem CheckInTool {
			get { return withEventsField_CheckInTool; }
			set {
				if (withEventsField_CheckInTool != null) {
					withEventsField_CheckInTool.Click -= CheckInTool_Click;
				}
				withEventsField_CheckInTool = value;
				if (withEventsField_CheckInTool != null) {
					withEventsField_CheckInTool.Click += CheckInTool_Click;
				}
			}
		}
		internal ToolStripSeparator ToolStripSeparator2;
		internal ToolStripSeparator ToolStripSeparator4;
		private PictureBox withEventsField_cmdRestart;
		internal PictureBox cmdRestart {
			get { return withEventsField_cmdRestart; }
			set {
				if (withEventsField_cmdRestart != null) {
					withEventsField_cmdRestart.Click -= cmdRestart_Click;
				}
				withEventsField_cmdRestart = value;
				if (withEventsField_cmdRestart != null) {
					withEventsField_cmdRestart.Click += cmdRestart_Click;
				}
			}
		}
		internal TextBox txtHostname;
		internal Label Label15;
		internal TabControl FieldTabs;
		internal TabPage AssetInfo;
		internal TabPage MiscInfo;
		internal Panel RemoteTrackingPanel;
		internal TextBox iCloudTextBox;
		internal Label Label17;
		internal TextBox ADOUTextBox;
		internal Panel ADPanel;
		internal Label Label18;
		internal GroupBox GroupBox1;
		internal Label Label22;
		internal TextBox ADCreatedTextBox;
		internal Label Label21;
		internal TextBox ADLastLoginTextBox;
		internal Label Label20;
		internal TextBox ADOSVerTextBox;
		internal Label Label19;
		internal TextBox ADOSTextBox;
		private ToolStripButton withEventsField_RefreshToolStripButton;
		internal ToolStripButton RefreshToolStripButton {
			get { return withEventsField_RefreshToolStripButton; }
			set {
				if (withEventsField_RefreshToolStripButton != null) {
					withEventsField_RefreshToolStripButton.Click -= RefreshToolStripButton_Click;
				}
				withEventsField_RefreshToolStripButton = value;
				if (withEventsField_RefreshToolStripButton != null) {
					withEventsField_RefreshToolStripButton.Click += RefreshToolStripButton_Click;
				}
			}
		}
		internal ToolStripStatusLabel StatusLabel;
		internal FlowLayoutPanel FlowLayoutPanel1;
		private Button withEventsField_DeployTVButton;
		internal Button DeployTVButton {
			get { return withEventsField_DeployTVButton; }
			set {
				if (withEventsField_DeployTVButton != null) {
					withEventsField_DeployTVButton.Click -= DeployTVButton_Click;
				}
				withEventsField_DeployTVButton = value;
				if (withEventsField_DeployTVButton != null) {
					withEventsField_DeployTVButton.Click += DeployTVButton_Click;
				}
			}
		}
		private Button withEventsField_UpdateChromeButton;
		internal Button UpdateChromeButton {
			get { return withEventsField_UpdateChromeButton; }
			set {
				if (withEventsField_UpdateChromeButton != null) {
					withEventsField_UpdateChromeButton.Click -= UpdateChromeButton_Click;
				}
				withEventsField_UpdateChromeButton = value;
				if (withEventsField_UpdateChromeButton != null) {
					withEventsField_UpdateChromeButton.Click += UpdateChromeButton_Click;
				}
			}
		}
		internal SplitContainer InfoDataSplitter;
		internal Panel Panel3;
	}
}