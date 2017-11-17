using System.Windows.Forms;
namespace AssetManager.UserInterface.Forms.AssetManagement
{

    partial class ViewHistoryForm
	{
		//Form overrides dispose to clean up the component list.
		
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
		private System.ComponentModel.IContainer components  = null;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		
		private void InitializeComponent()
		{
            this.GroupBox1 = new System.Windows.Forms.GroupBox();
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.NotesTextBox = new System.Windows.Forms.RichTextBox();
            this.Label20 = new System.Windows.Forms.Label();
            this.iCloudTextBox = new System.Windows.Forms.TextBox();
            this.Label19 = new System.Windows.Forms.Label();
            this.txtHostname = new System.Windows.Forms.TextBox();
            this.Label18 = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new System.Windows.Forms.TextBox();
            this.chkTrackable = new System.Windows.Forms.CheckBox();
            this.Label17 = new System.Windows.Forms.Label();
            this.txtEntryGUID = new System.Windows.Forms.TextBox();
            this.Label16 = new System.Windows.Forms.Label();
            this.txtStatus = new System.Windows.Forms.TextBox();
            this.Label14 = new System.Windows.Forms.Label();
            this.txtActionUser = new System.Windows.Forms.TextBox();
            this.Label13 = new System.Windows.Forms.Label();
            this.txtEntryTime = new System.Windows.Forms.TextBox();
            this.Label12 = new System.Windows.Forms.Label();
            this.txtGUID = new System.Windows.Forms.TextBox();
            this.Label11 = new System.Windows.Forms.Label();
            this.txtEQType = new System.Windows.Forms.TextBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.txtOSVersion = new System.Windows.Forms.TextBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.txtPONumber = new System.Windows.Forms.TextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.txtReplaceYear = new System.Windows.Forms.TextBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.txtPurchaseDate = new System.Windows.Forms.TextBox();
            this.Label6 = new System.Windows.Forms.Label();
            this.txtLocation = new System.Windows.Forms.TextBox();
            this.Label5 = new System.Windows.Forms.Label();
            this.txtDescription = new System.Windows.Forms.TextBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtSerial = new System.Windows.Forms.TextBox();
            this.Label3 = new System.Windows.Forms.Label();
            this.txtAssetTag = new System.Windows.Forms.TextBox();
            this.Label2 = new System.Windows.Forms.Label();
            this.txtCurrentUser = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtChangeType = new System.Windows.Forms.TextBox();
            this.GroupBox1.SuspendLayout();
            this.GroupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox1
            // 
            this.GroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.GroupBox1.Controls.Add(this.GroupBox2);
            this.GroupBox1.Controls.Add(this.Label20);
            this.GroupBox1.Controls.Add(this.iCloudTextBox);
            this.GroupBox1.Controls.Add(this.Label19);
            this.GroupBox1.Controls.Add(this.txtHostname);
            this.GroupBox1.Controls.Add(this.Label18);
            this.GroupBox1.Controls.Add(this.txtPhoneNumber);
            this.GroupBox1.Controls.Add(this.chkTrackable);
            this.GroupBox1.Controls.Add(this.Label17);
            this.GroupBox1.Controls.Add(this.txtEntryGUID);
            this.GroupBox1.Controls.Add(this.Label16);
            this.GroupBox1.Controls.Add(this.txtStatus);
            this.GroupBox1.Controls.Add(this.Label14);
            this.GroupBox1.Controls.Add(this.txtActionUser);
            this.GroupBox1.Controls.Add(this.Label13);
            this.GroupBox1.Controls.Add(this.txtEntryTime);
            this.GroupBox1.Controls.Add(this.Label12);
            this.GroupBox1.Controls.Add(this.txtGUID);
            this.GroupBox1.Controls.Add(this.Label11);
            this.GroupBox1.Controls.Add(this.txtEQType);
            this.GroupBox1.Controls.Add(this.Label10);
            this.GroupBox1.Controls.Add(this.txtOSVersion);
            this.GroupBox1.Controls.Add(this.Label9);
            this.GroupBox1.Controls.Add(this.txtPONumber);
            this.GroupBox1.Controls.Add(this.Label8);
            this.GroupBox1.Controls.Add(this.txtReplaceYear);
            this.GroupBox1.Controls.Add(this.Label7);
            this.GroupBox1.Controls.Add(this.txtPurchaseDate);
            this.GroupBox1.Controls.Add(this.Label6);
            this.GroupBox1.Controls.Add(this.txtLocation);
            this.GroupBox1.Controls.Add(this.Label5);
            this.GroupBox1.Controls.Add(this.txtDescription);
            this.GroupBox1.Controls.Add(this.Label4);
            this.GroupBox1.Controls.Add(this.txtSerial);
            this.GroupBox1.Controls.Add(this.Label3);
            this.GroupBox1.Controls.Add(this.txtAssetTag);
            this.GroupBox1.Controls.Add(this.Label2);
            this.GroupBox1.Controls.Add(this.txtCurrentUser);
            this.GroupBox1.Controls.Add(this.Label1);
            this.GroupBox1.Controls.Add(this.txtChangeType);
            this.GroupBox1.Location = new System.Drawing.Point(13, 14);
            this.GroupBox1.Name = "GroupBox1";
            this.GroupBox1.Size = new System.Drawing.Size(807, 407);
            this.GroupBox1.TabIndex = 0;
            this.GroupBox1.TabStop = false;
            this.GroupBox1.Text = "Device Info Snapshot";
            // 
            // GroupBox2
            // 
            this.GroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.GroupBox2.Controls.Add(this.NotesTextBox);
            this.GroupBox2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox2.Location = new System.Drawing.Point(6, 230);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(795, 171);
            this.GroupBox2.TabIndex = 43;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Notes";
            // 
            // NotesTextBox
            // 
            this.NotesTextBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.NotesTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.NotesTextBox.Location = new System.Drawing.Point(3, 19);
            this.NotesTextBox.Name = "NotesTextBox";
            this.NotesTextBox.ReadOnly = true;
            this.NotesTextBox.Size = new System.Drawing.Size(789, 149);
            this.NotesTextBox.TabIndex = 42;
            this.NotesTextBox.Text = "";
            // 
            // Label20
            // 
            this.Label20.AutoSize = true;
            this.Label20.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label20.Location = new System.Drawing.Point(560, 162);
            this.Label20.Name = "Label20";
            this.Label20.Size = new System.Drawing.Size(84, 15);
            this.Label20.TabIndex = 41;
            this.Label20.Text = "iCloud Acct";
            // 
            // iCloudTextBox
            // 
            this.iCloudTextBox.BackColor = System.Drawing.SystemColors.Window;
            this.iCloudTextBox.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iCloudTextBox.Location = new System.Drawing.Point(563, 181);
            this.iCloudTextBox.Name = "iCloudTextBox";
            this.iCloudTextBox.ReadOnly = true;
            this.iCloudTextBox.Size = new System.Drawing.Size(228, 23);
            this.iCloudTextBox.TabIndex = 40;
            // 
            // Label19
            // 
            this.Label19.AutoSize = true;
            this.Label19.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label19.Location = new System.Drawing.Point(14, 162);
            this.Label19.Name = "Label19";
            this.Label19.Size = new System.Drawing.Size(70, 15);
            this.Label19.TabIndex = 39;
            this.Label19.Text = "Hostname:";
            // 
            // txtHostname
            // 
            this.txtHostname.BackColor = System.Drawing.SystemColors.Window;
            this.txtHostname.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHostname.Location = new System.Drawing.Point(17, 181);
            this.txtHostname.Name = "txtHostname";
            this.txtHostname.ReadOnly = true;
            this.txtHostname.Size = new System.Drawing.Size(155, 23);
            this.txtHostname.TabIndex = 38;
            // 
            // Label18
            // 
            this.Label18.AutoSize = true;
            this.Label18.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label18.Location = new System.Drawing.Point(14, 119);
            this.Label18.Name = "Label18";
            this.Label18.Size = new System.Drawing.Size(49, 15);
            this.Label18.TabIndex = 37;
            this.Label18.Text = "Phone:";
            // 
            // txtPhoneNumber
            // 
            this.txtPhoneNumber.BackColor = System.Drawing.SystemColors.Window;
            this.txtPhoneNumber.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhoneNumber.Location = new System.Drawing.Point(17, 138);
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.ReadOnly = true;
            this.txtPhoneNumber.Size = new System.Drawing.Size(155, 23);
            this.txtPhoneNumber.TabIndex = 36;
            // 
            // chkTrackable
            // 
            this.chkTrackable.AutoSize = true;
            this.chkTrackable.Enabled = false;
            this.chkTrackable.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTrackable.Location = new System.Drawing.Point(563, 210);
            this.chkTrackable.Name = "chkTrackable";
            this.chkTrackable.Size = new System.Drawing.Size(89, 19);
            this.chkTrackable.TabIndex = 35;
            this.chkTrackable.Text = "Trackable";
            this.chkTrackable.UseVisualStyleBackColor = true;
            // 
            // Label17
            // 
            this.Label17.AutoSize = true;
            this.Label17.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label17.Location = new System.Drawing.Point(560, 75);
            this.Label17.Name = "Label17";
            this.Label17.Size = new System.Drawing.Size(70, 15);
            this.Label17.TabIndex = 34;
            this.Label17.Text = "Entry UID";
            // 
            // txtEntryGUID
            // 
            this.txtEntryGUID.BackColor = System.Drawing.SystemColors.Window;
            this.txtEntryGUID.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEntryGUID.Location = new System.Drawing.Point(563, 94);
            this.txtEntryGUID.Name = "txtEntryGUID";
            this.txtEntryGUID.ReadOnly = true;
            this.txtEntryGUID.Size = new System.Drawing.Size(228, 23);
            this.txtEntryGUID.TabIndex = 33;
            // 
            // Label16
            // 
            this.Label16.AutoSize = true;
            this.Label16.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label16.Location = new System.Drawing.Point(560, 119);
            this.Label16.Name = "Label16";
            this.Label16.Size = new System.Drawing.Size(49, 15);
            this.Label16.TabIndex = 32;
            this.Label16.Text = "Status";
            // 
            // txtStatus
            // 
            this.txtStatus.BackColor = System.Drawing.SystemColors.Window;
            this.txtStatus.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtStatus.Location = new System.Drawing.Point(563, 138);
            this.txtStatus.Name = "txtStatus";
            this.txtStatus.ReadOnly = true;
            this.txtStatus.Size = new System.Drawing.Size(108, 23);
            this.txtStatus.TabIndex = 31;
            // 
            // Label14
            // 
            this.Label14.AutoSize = true;
            this.Label14.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label14.Location = new System.Drawing.Point(14, 75);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(84, 15);
            this.Label14.TabIndex = 27;
            this.Label14.Text = "Action User";
            // 
            // txtActionUser
            // 
            this.txtActionUser.BackColor = System.Drawing.SystemColors.Window;
            this.txtActionUser.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtActionUser.Location = new System.Drawing.Point(17, 94);
            this.txtActionUser.Name = "txtActionUser";
            this.txtActionUser.ReadOnly = true;
            this.txtActionUser.Size = new System.Drawing.Size(155, 23);
            this.txtActionUser.TabIndex = 26;
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label13.Location = new System.Drawing.Point(14, 28);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(77, 15);
            this.Label13.TabIndex = 25;
            this.Label13.Text = "Time Stamp";
            // 
            // txtEntryTime
            // 
            this.txtEntryTime.BackColor = System.Drawing.SystemColors.Window;
            this.txtEntryTime.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEntryTime.Location = new System.Drawing.Point(17, 47);
            this.txtEntryTime.Name = "txtEntryTime";
            this.txtEntryTime.ReadOnly = true;
            this.txtEntryTime.Size = new System.Drawing.Size(155, 23);
            this.txtEntryTime.TabIndex = 24;
            // 
            // Label12
            // 
            this.Label12.AutoSize = true;
            this.Label12.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label12.Location = new System.Drawing.Point(560, 28);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(77, 15);
            this.Label12.TabIndex = 23;
            this.Label12.Text = "Device UID";
            // 
            // txtGUID
            // 
            this.txtGUID.BackColor = System.Drawing.SystemColors.Window;
            this.txtGUID.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtGUID.Location = new System.Drawing.Point(563, 47);
            this.txtGUID.Name = "txtGUID";
            this.txtGUID.ReadOnly = true;
            this.txtGUID.Size = new System.Drawing.Size(228, 23);
            this.txtGUID.TabIndex = 22;
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label11.Location = new System.Drawing.Point(438, 162);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(56, 15);
            this.Label11.TabIndex = 21;
            this.Label11.Text = "EQ Type";
            // 
            // txtEQType
            // 
            this.txtEQType.BackColor = System.Drawing.SystemColors.Window;
            this.txtEQType.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtEQType.Location = new System.Drawing.Point(441, 181);
            this.txtEQType.Name = "txtEQType";
            this.txtEQType.ReadOnly = true;
            this.txtEQType.Size = new System.Drawing.Size(108, 23);
            this.txtEQType.TabIndex = 20;
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label10.Location = new System.Drawing.Point(438, 119);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(77, 15);
            this.Label10.TabIndex = 19;
            this.Label10.Text = "OS Version";
            // 
            // txtOSVersion
            // 
            this.txtOSVersion.BackColor = System.Drawing.SystemColors.Window;
            this.txtOSVersion.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtOSVersion.Location = new System.Drawing.Point(441, 138);
            this.txtOSVersion.Name = "txtOSVersion";
            this.txtOSVersion.ReadOnly = true;
            this.txtOSVersion.Size = new System.Drawing.Size(108, 23);
            this.txtOSVersion.TabIndex = 18;
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.Location = new System.Drawing.Point(438, 75);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(70, 15);
            this.Label9.TabIndex = 17;
            this.Label9.Text = "PO Number";
            // 
            // txtPONumber
            // 
            this.txtPONumber.BackColor = System.Drawing.SystemColors.Window;
            this.txtPONumber.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPONumber.Location = new System.Drawing.Point(441, 94);
            this.txtPONumber.Name = "txtPONumber";
            this.txtPONumber.ReadOnly = true;
            this.txtPONumber.Size = new System.Drawing.Size(108, 23);
            this.txtPONumber.TabIndex = 16;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.Location = new System.Drawing.Point(317, 162);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(119, 15);
            this.Label8.TabIndex = 15;
            this.Label8.Text = "Replacement Year";
            // 
            // txtReplaceYear
            // 
            this.txtReplaceYear.BackColor = System.Drawing.SystemColors.Window;
            this.txtReplaceYear.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReplaceYear.Location = new System.Drawing.Point(320, 181);
            this.txtReplaceYear.Name = "txtReplaceYear";
            this.txtReplaceYear.ReadOnly = true;
            this.txtReplaceYear.Size = new System.Drawing.Size(108, 23);
            this.txtReplaceYear.TabIndex = 14;
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(317, 119);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(98, 15);
            this.Label7.TabIndex = 13;
            this.Label7.Text = "Purchase Date";
            // 
            // txtPurchaseDate
            // 
            this.txtPurchaseDate.BackColor = System.Drawing.SystemColors.Window;
            this.txtPurchaseDate.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPurchaseDate.Location = new System.Drawing.Point(320, 138);
            this.txtPurchaseDate.Name = "txtPurchaseDate";
            this.txtPurchaseDate.ReadOnly = true;
            this.txtPurchaseDate.Size = new System.Drawing.Size(108, 23);
            this.txtPurchaseDate.TabIndex = 12;
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(317, 75);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(63, 15);
            this.Label6.TabIndex = 11;
            this.Label6.Text = "Location";
            // 
            // txtLocation
            // 
            this.txtLocation.BackColor = System.Drawing.SystemColors.Window;
            this.txtLocation.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtLocation.Location = new System.Drawing.Point(320, 94);
            this.txtLocation.Name = "txtLocation";
            this.txtLocation.ReadOnly = true;
            this.txtLocation.Size = new System.Drawing.Size(108, 23);
            this.txtLocation.TabIndex = 10;
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(317, 28);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(84, 15);
            this.Label5.TabIndex = 9;
            this.Label5.Text = "Description";
            // 
            // txtDescription
            // 
            this.txtDescription.BackColor = System.Drawing.SystemColors.Window;
            this.txtDescription.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription.Location = new System.Drawing.Point(320, 47);
            this.txtDescription.Name = "txtDescription";
            this.txtDescription.ReadOnly = true;
            this.txtDescription.Size = new System.Drawing.Size(229, 23);
            this.txtDescription.TabIndex = 8;
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(191, 162);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(49, 15);
            this.Label4.TabIndex = 7;
            this.Label4.Text = "Serial";
            // 
            // txtSerial
            // 
            this.txtSerial.BackColor = System.Drawing.SystemColors.Window;
            this.txtSerial.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerial.Location = new System.Drawing.Point(194, 181);
            this.txtSerial.Name = "txtSerial";
            this.txtSerial.ReadOnly = true;
            this.txtSerial.Size = new System.Drawing.Size(108, 23);
            this.txtSerial.TabIndex = 6;
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(191, 119);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(70, 15);
            this.Label3.TabIndex = 5;
            this.Label3.Text = "Asset Tag";
            // 
            // txtAssetTag
            // 
            this.txtAssetTag.BackColor = System.Drawing.SystemColors.Window;
            this.txtAssetTag.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAssetTag.Location = new System.Drawing.Point(194, 138);
            this.txtAssetTag.Name = "txtAssetTag";
            this.txtAssetTag.ReadOnly = true;
            this.txtAssetTag.Size = new System.Drawing.Size(108, 23);
            this.txtAssetTag.TabIndex = 4;
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(189, 75);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(35, 15);
            this.Label2.TabIndex = 3;
            this.Label2.Text = "User";
            // 
            // txtCurrentUser
            // 
            this.txtCurrentUser.BackColor = System.Drawing.SystemColors.Window;
            this.txtCurrentUser.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurrentUser.Location = new System.Drawing.Point(192, 94);
            this.txtCurrentUser.Name = "txtCurrentUser";
            this.txtCurrentUser.ReadOnly = true;
            this.txtCurrentUser.Size = new System.Drawing.Size(108, 23);
            this.txtCurrentUser.TabIndex = 2;
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(189, 28);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(84, 15);
            this.Label1.TabIndex = 1;
            this.Label1.Text = "Change Type";
            // 
            // txtChangeType
            // 
            this.txtChangeType.BackColor = System.Drawing.SystemColors.Window;
            this.txtChangeType.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtChangeType.Location = new System.Drawing.Point(192, 47);
            this.txtChangeType.Name = "txtChangeType";
            this.txtChangeType.ReadOnly = true;
            this.txtChangeType.Size = new System.Drawing.Size(110, 23);
            this.txtChangeType.TabIndex = 0;
            // 
            // ViewHistoryForm
            // 
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ClientSize = new System.Drawing.Size(832, 431);
            this.Controls.Add(this.GroupBox1);
            this.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.MinimumSize = new System.Drawing.Size(848, 385);
            this.Name = "ViewHistoryForm";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "View Entry";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ViewHistoryForm_FormClosing);
            this.GroupBox1.ResumeLayout(false);
            this.GroupBox1.PerformLayout();
            this.GroupBox2.ResumeLayout(false);
            this.ResumeLayout(false);

		}
		internal GroupBox GroupBox1;
		internal Label Label2;
		internal TextBox txtCurrentUser;
		internal Label Label1;
		internal TextBox txtChangeType;
		internal Label Label3;
		internal TextBox txtAssetTag;
		internal Label Label12;
		internal TextBox txtGUID;
		internal Label Label11;
		internal TextBox txtEQType;
		internal Label Label10;
		internal TextBox txtOSVersion;
		internal Label Label9;
		internal TextBox txtPONumber;
		internal Label Label8;
		internal TextBox txtReplaceYear;
		internal Label Label7;
		internal TextBox txtPurchaseDate;
		internal Label Label6;
		internal TextBox txtLocation;
		internal Label Label5;
		internal TextBox txtDescription;
		internal Label Label4;
		internal TextBox txtSerial;
		internal Label Label13;
		internal TextBox txtEntryTime;
		internal Label Label14;
		internal TextBox txtActionUser;
		internal Label Label16;
		internal TextBox txtStatus;
		internal Label Label17;
		internal TextBox txtEntryGUID;
		internal CheckBox chkTrackable;
		internal Label Label18;
		internal TextBox txtPhoneNumber;
		internal Label Label19;
		internal TextBox txtHostname;
		internal Label Label20;
		internal TextBox iCloudTextBox;
		internal GroupBox GroupBox2;
		internal RichTextBox NotesTextBox;
	}
}
