using System.Windows.Forms;
namespace AssetManager.UserInterface.Forms.AssetManagement
{

    partial class NewDeviceForm
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
        private System.ComponentModel.IContainer components  = null;
        //NOTE: The following procedure is required by the Windows Form Designer
        //It can be modified using the Windows Form Designer.  
        //Do not modify it using the code editor.
        
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewDeviceForm));
            this.GroupBox2 = new System.Windows.Forms.GroupBox();
            this.txtSerial_REQ = new System.Windows.Forms.TextBox();
            this.Label1 = new System.Windows.Forms.Label();
            this.txtCurUser_REQ = new System.Windows.Forms.TextBox();
            this.cmdUserSearch = new System.Windows.Forms.Button();
            this.Label3 = new System.Windows.Forms.Label();
            this.Label2 = new System.Windows.Forms.Label();
            this.Label12 = new System.Windows.Forms.Label();
            this.txtAssetTag_REQ = new System.Windows.Forms.TextBox();
            this.chkNoClear = new System.Windows.Forms.CheckBox();
            this.chkTrackable = new System.Windows.Forms.CheckBox();
            this.cmdClear = new System.Windows.Forms.Button();
            this.Label11 = new System.Windows.Forms.Label();
            this.cmbStatus_REQ = new System.Windows.Forms.ComboBox();
            this.Label10 = new System.Windows.Forms.Label();
            this.cmbOSType_REQ = new System.Windows.Forms.ComboBox();
            this.Label9 = new System.Windows.Forms.Label();
            this.txtPO = new System.Windows.Forms.TextBox();
            this.Label8 = new System.Windows.Forms.Label();
            this.cmbEquipType_REQ = new System.Windows.Forms.ComboBox();
            this.txtNotes = new System.Windows.Forms.TextBox();
            this.cmdAdd = new System.Windows.Forms.Button();
            this.Label6 = new System.Windows.Forms.Label();
            this.txtReplaceYear = new System.Windows.Forms.TextBox();
            this.lbPurchaseDate = new System.Windows.Forms.Label();
            this.dtPurchaseDate_REQ = new System.Windows.Forms.DateTimePicker();
            this.Label5 = new System.Windows.Forms.Label();
            this.cmbLocation_REQ = new System.Windows.Forms.ComboBox();
            this.Label4 = new System.Windows.Forms.Label();
            this.txtDescription_REQ = new System.Windows.Forms.TextBox();
            this.fieldErrorIcon = new System.Windows.Forms.ErrorProvider(this.components);
            this.GroupBox3 = new System.Windows.Forms.GroupBox();
            this.GroupBox4 = new System.Windows.Forms.GroupBox();
            this.GroupBox5 = new System.Windows.Forms.GroupBox();
            this.iCloudTextBox = new System.Windows.Forms.TextBox();
            this.Label14 = new System.Windows.Forms.Label();
            this.txtHostname = new System.Windows.Forms.TextBox();
            this.Label7 = new System.Windows.Forms.Label();
            this.txtPhoneNumber = new System.Windows.Forms.MaskedTextBox();
            this.Label13 = new System.Windows.Forms.Label();
            this.GroupBox6 = new System.Windows.Forms.GroupBox();
            this.GroupBox7 = new System.Windows.Forms.GroupBox();
            this.ToolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.GroupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldErrorIcon)).BeginInit();
            this.GroupBox3.SuspendLayout();
            this.GroupBox4.SuspendLayout();
            this.GroupBox5.SuspendLayout();
            this.GroupBox6.SuspendLayout();
            this.GroupBox7.SuspendLayout();
            this.SuspendLayout();
            // 
            // GroupBox2
            // 
            this.GroupBox2.Controls.Add(this.txtSerial_REQ);
            this.GroupBox2.Controls.Add(this.Label1);
            this.GroupBox2.Controls.Add(this.txtCurUser_REQ);
            this.GroupBox2.Controls.Add(this.cmdUserSearch);
            this.GroupBox2.Controls.Add(this.Label3);
            this.GroupBox2.Controls.Add(this.Label2);
            this.GroupBox2.Controls.Add(this.Label12);
            this.GroupBox2.Controls.Add(this.txtAssetTag_REQ);
            this.GroupBox2.Location = new System.Drawing.Point(12, 12);
            this.GroupBox2.Name = "GroupBox2";
            this.GroupBox2.Size = new System.Drawing.Size(236, 294);
            this.GroupBox2.TabIndex = 52;
            this.GroupBox2.TabStop = false;
            this.GroupBox2.Text = "Unique Info";
            // 
            // txtSerial_REQ
            // 
            this.txtSerial_REQ.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtSerial_REQ.Location = new System.Drawing.Point(17, 46);
            this.txtSerial_REQ.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.txtSerial_REQ.Name = "txtSerial_REQ";
            this.txtSerial_REQ.Size = new System.Drawing.Size(178, 25);
            this.txtSerial_REQ.TabIndex = 0;
            this.txtSerial_REQ.Text = "txtSerial";
            this.txtSerial_REQ.TextChanged += new System.EventHandler(this.txtSerial_REQ_TextChanged);
            // 
            // Label1
            // 
            this.Label1.AutoSize = true;
            this.Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label1.Location = new System.Drawing.Point(14, 26);
            this.Label1.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label1.Name = "Label1";
            this.Label1.Size = new System.Drawing.Size(43, 16);
            this.Label1.TabIndex = 25;
            this.Label1.Text = "Serial";
            // 
            // txtCurUser_REQ
            // 
            this.txtCurUser_REQ.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtCurUser_REQ.Location = new System.Drawing.Point(17, 144);
            this.txtCurUser_REQ.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.txtCurUser_REQ.Name = "txtCurUser_REQ";
            this.txtCurUser_REQ.Size = new System.Drawing.Size(178, 25);
            this.txtCurUser_REQ.TabIndex = 2;
            this.txtCurUser_REQ.Text = "txtCurUser";
            this.txtCurUser_REQ.TextChanged += new System.EventHandler(this.txtCurUser_REQ_TextChanged);
            this.txtCurUser_REQ.DoubleClick += new System.EventHandler(this.txtCurUser_REQ_DoubleClick);
            // 
            // cmdUserSearch
            // 
            this.cmdUserSearch.Location = new System.Drawing.Point(36, 173);
            this.cmdUserSearch.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.cmdUserSearch.Name = "cmdUserSearch";
            this.cmdUserSearch.Size = new System.Drawing.Size(141, 23);
            this.cmdUserSearch.TabIndex = 50;
            this.cmdUserSearch.Text = "Munis Search";
            this.cmdUserSearch.UseVisualStyleBackColor = true;
            this.cmdUserSearch.Click += new System.EventHandler(this.cmdUserSearch_Click);
            // 
            // Label3
            // 
            this.Label3.AutoSize = true;
            this.Label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label3.Location = new System.Drawing.Point(14, 124);
            this.Label3.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label3.Name = "Label3";
            this.Label3.Size = new System.Drawing.Size(37, 16);
            this.Label3.TabIndex = 29;
            this.Label3.Text = "User";
            // 
            // Label2
            // 
            this.Label2.AutoSize = true;
            this.Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label2.Location = new System.Drawing.Point(14, 75);
            this.Label2.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label2.Name = "Label2";
            this.Label2.Size = new System.Drawing.Size(70, 16);
            this.Label2.TabIndex = 26;
            this.Label2.Text = "Asset Tag";
            // 
            // Label12
            // 
            this.Label12.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label12.ForeColor = System.Drawing.Color.Gray;
            this.Label12.Location = new System.Drawing.Point(92, 79);
            this.Label12.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label12.Name = "Label12";
            this.Label12.Size = new System.Drawing.Size(112, 13);
            this.Label12.TabIndex = 51;
            this.Label12.Text = "(\"NA\" if not available.)";
            this.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // txtAssetTag_REQ
            // 
            this.txtAssetTag_REQ.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtAssetTag_REQ.Location = new System.Drawing.Point(17, 95);
            this.txtAssetTag_REQ.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.txtAssetTag_REQ.Name = "txtAssetTag_REQ";
            this.txtAssetTag_REQ.Size = new System.Drawing.Size(178, 25);
            this.txtAssetTag_REQ.TabIndex = 1;
            this.txtAssetTag_REQ.Text = "txtAssetTag";
            this.txtAssetTag_REQ.TextChanged += new System.EventHandler(this.txtAssetTag_REQ_TextChanged);
            // 
            // chkNoClear
            // 
            this.chkNoClear.AutoSize = true;
            this.chkNoClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkNoClear.Location = new System.Drawing.Point(732, 392);
            this.chkNoClear.Name = "chkNoClear";
            this.chkNoClear.Size = new System.Drawing.Size(91, 20);
            this.chkNoClear.TabIndex = 16;
            this.chkNoClear.Text = "Don\'t clear";
            this.chkNoClear.UseVisualStyleBackColor = true;
            // 
            // chkTrackable
            // 
            this.chkTrackable.AutoSize = true;
            this.chkTrackable.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.chkTrackable.Location = new System.Drawing.Point(31, 196);
            this.chkTrackable.Name = "chkTrackable";
            this.chkTrackable.Size = new System.Drawing.Size(135, 20);
            this.chkTrackable.TabIndex = 12;
            this.chkTrackable.Text = "Trackable Device";
            this.chkTrackable.UseVisualStyleBackColor = true;
            // 
            // cmdClear
            // 
            this.cmdClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdClear.Location = new System.Drawing.Point(693, 427);
            this.cmdClear.Name = "cmdClear";
            this.cmdClear.Size = new System.Drawing.Size(170, 23);
            this.cmdClear.TabIndex = 17;
            this.cmdClear.Text = "Clear";
            this.cmdClear.UseVisualStyleBackColor = true;
            this.cmdClear.Click += new System.EventHandler(this.cmdClear_Click);
            // 
            // Label11
            // 
            this.Label11.AutoSize = true;
            this.Label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label11.Location = new System.Drawing.Point(14, 226);
            this.Label11.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label11.Name = "Label11";
            this.Label11.Size = new System.Drawing.Size(91, 16);
            this.Label11.TabIndex = 47;
            this.Label11.Text = "Device Status";
            // 
            // cmbStatus_REQ
            // 
            this.cmbStatus_REQ.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbStatus_REQ.FormattingEnabled = true;
            this.cmbStatus_REQ.Location = new System.Drawing.Point(17, 246);
            this.cmbStatus_REQ.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.cmbStatus_REQ.Name = "cmbStatus_REQ";
            this.cmbStatus_REQ.Size = new System.Drawing.Size(251, 26);
            this.cmbStatus_REQ.TabIndex = 7;
            this.cmbStatus_REQ.DropDown += new System.EventHandler(this.cmbStatus_REQ_DropDown);
            this.cmbStatus_REQ.SelectedIndexChanged += new System.EventHandler(this.cmbStatus_REQ_SelectedIndexChanged);
            // 
            // Label10
            // 
            this.Label10.AutoSize = true;
            this.Label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label10.Location = new System.Drawing.Point(14, 126);
            this.Label10.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label10.Name = "Label10";
            this.Label10.Size = new System.Drawing.Size(115, 16);
            this.Label10.TabIndex = 45;
            this.Label10.Text = "Operating System";
            // 
            // cmbOSType_REQ
            // 
            this.cmbOSType_REQ.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbOSType_REQ.FormattingEnabled = true;
            this.cmbOSType_REQ.Location = new System.Drawing.Point(17, 146);
            this.cmbOSType_REQ.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.cmbOSType_REQ.Name = "cmbOSType_REQ";
            this.cmbOSType_REQ.Size = new System.Drawing.Size(251, 26);
            this.cmbOSType_REQ.TabIndex = 5;
            this.cmbOSType_REQ.DropDown += new System.EventHandler(this.cmbOSType_REQ_DropDown);
            this.cmbOSType_REQ.SelectedIndexChanged += new System.EventHandler(this.cmbOSType_REQ_SelectedIndexChanged);
            // 
            // Label9
            // 
            this.Label9.AutoSize = true;
            this.Label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label9.Location = new System.Drawing.Point(11, 135);
            this.Label9.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label9.Name = "Label9";
            this.Label9.Size = new System.Drawing.Size(78, 16);
            this.Label9.TabIndex = 43;
            this.Label9.Text = "PO Number";
            // 
            // txtPO
            // 
            this.txtPO.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPO.Location = new System.Drawing.Point(14, 154);
            this.txtPO.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.txtPO.Name = "txtPO";
            this.txtPO.Size = new System.Drawing.Size(170, 25);
            this.txtPO.TabIndex = 10;
            // 
            // Label8
            // 
            this.Label8.AutoSize = true;
            this.Label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label8.Location = new System.Drawing.Point(14, 76);
            this.Label8.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label8.Name = "Label8";
            this.Label8.Size = new System.Drawing.Size(107, 16);
            this.Label8.TabIndex = 41;
            this.Label8.Text = "Equipment Type";
            // 
            // cmbEquipType_REQ
            // 
            this.cmbEquipType_REQ.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbEquipType_REQ.FormattingEnabled = true;
            this.cmbEquipType_REQ.Location = new System.Drawing.Point(17, 96);
            this.cmbEquipType_REQ.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.cmbEquipType_REQ.Name = "cmbEquipType_REQ";
            this.cmbEquipType_REQ.Size = new System.Drawing.Size(251, 26);
            this.cmbEquipType_REQ.TabIndex = 4;
            this.cmbEquipType_REQ.DropDown += new System.EventHandler(this.cmbEquipType_REQ_DropDown);
            this.cmbEquipType_REQ.SelectedIndexChanged += new System.EventHandler(this.cmbEquipType_REQ_SelectedIndexChanged);
            // 
            // txtNotes
            // 
            this.txtNotes.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtNotes.Location = new System.Drawing.Point(6, 21);
            this.txtNotes.MaxLength = 200;
            this.txtNotes.Multiline = true;
            this.txtNotes.Name = "txtNotes";
            this.txtNotes.Size = new System.Drawing.Size(366, 86);
            this.txtNotes.TabIndex = 14;
            // 
            // cmdAdd
            // 
            this.cmdAdd.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmdAdd.Location = new System.Drawing.Point(693, 342);
            this.cmdAdd.Name = "cmdAdd";
            this.cmdAdd.Size = new System.Drawing.Size(170, 44);
            this.cmdAdd.TabIndex = 15;
            this.cmdAdd.Text = "Add Device";
            this.cmdAdd.UseVisualStyleBackColor = true;
            this.cmdAdd.Click += new System.EventHandler(this.cmdAdd_Click);
            // 
            // Label6
            // 
            this.Label6.AutoSize = true;
            this.Label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label6.Location = new System.Drawing.Point(10, 81);
            this.Label6.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label6.Name = "Label6";
            this.Label6.Size = new System.Drawing.Size(92, 16);
            this.Label6.TabIndex = 36;
            this.Label6.Text = "Replace Year";
            // 
            // txtReplaceYear
            // 
            this.txtReplaceYear.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtReplaceYear.Location = new System.Drawing.Point(14, 100);
            this.txtReplaceYear.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.txtReplaceYear.Name = "txtReplaceYear";
            this.txtReplaceYear.Size = new System.Drawing.Size(170, 25);
            this.txtReplaceYear.TabIndex = 9;
            this.txtReplaceYear.Text = "txtReplaceYear";
            // 
            // lbPurchaseDate
            // 
            this.lbPurchaseDate.AutoSize = true;
            this.lbPurchaseDate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPurchaseDate.Location = new System.Drawing.Point(10, 27);
            this.lbPurchaseDate.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.lbPurchaseDate.Name = "lbPurchaseDate";
            this.lbPurchaseDate.Size = new System.Drawing.Size(97, 16);
            this.lbPurchaseDate.TabIndex = 34;
            this.lbPurchaseDate.Text = "Purchase Date";
            // 
            // dtPurchaseDate_REQ
            // 
            this.dtPurchaseDate_REQ.CustomFormat = "yyyy-MM-dd";
            this.dtPurchaseDate_REQ.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.dtPurchaseDate_REQ.Format = System.Windows.Forms.DateTimePickerFormat.Custom;
            this.dtPurchaseDate_REQ.Location = new System.Drawing.Point(13, 46);
            this.dtPurchaseDate_REQ.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.dtPurchaseDate_REQ.Name = "dtPurchaseDate_REQ";
            this.dtPurchaseDate_REQ.Size = new System.Drawing.Size(171, 25);
            this.dtPurchaseDate_REQ.TabIndex = 8;
            this.dtPurchaseDate_REQ.Value = new System.DateTime(2016, 4, 14, 0, 0, 0, 0);
            this.dtPurchaseDate_REQ.ValueChanged += new System.EventHandler(this.dtPurchaseDate_REQ_ValueChanged);
            // 
            // Label5
            // 
            this.Label5.AutoSize = true;
            this.Label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label5.Location = new System.Drawing.Point(14, 176);
            this.Label5.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label5.Name = "Label5";
            this.Label5.Size = new System.Drawing.Size(59, 16);
            this.Label5.TabIndex = 32;
            this.Label5.Text = "Location";
            // 
            // cmbLocation_REQ
            // 
            this.cmbLocation_REQ.DropDownWidth = 171;
            this.cmbLocation_REQ.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.cmbLocation_REQ.FormattingEnabled = true;
            this.cmbLocation_REQ.Location = new System.Drawing.Point(17, 196);
            this.cmbLocation_REQ.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.cmbLocation_REQ.Name = "cmbLocation_REQ";
            this.cmbLocation_REQ.Size = new System.Drawing.Size(251, 26);
            this.cmbLocation_REQ.TabIndex = 6;
            this.cmbLocation_REQ.Text = "cmbLocation";
            this.cmbLocation_REQ.DropDown += new System.EventHandler(this.cmbLocation_REQ_DropDown);
            this.cmbLocation_REQ.SelectedIndexChanged += new System.EventHandler(this.cmbLocation_REQ_SelectedIndexChanged);
            // 
            // Label4
            // 
            this.Label4.AutoSize = true;
            this.Label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label4.Location = new System.Drawing.Point(14, 28);
            this.Label4.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label4.Name = "Label4";
            this.Label4.Size = new System.Drawing.Size(76, 16);
            this.Label4.TabIndex = 30;
            this.Label4.Text = "Description";
            // 
            // txtDescription_REQ
            // 
            this.txtDescription_REQ.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtDescription_REQ.Location = new System.Drawing.Point(17, 47);
            this.txtDescription_REQ.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.txtDescription_REQ.Name = "txtDescription_REQ";
            this.txtDescription_REQ.Size = new System.Drawing.Size(251, 25);
            this.txtDescription_REQ.TabIndex = 3;
            this.txtDescription_REQ.Text = "Description";
            this.txtDescription_REQ.TextChanged += new System.EventHandler(this.txtDescription_REQ_TextChanged);
            // 
            // fieldErrorIcon
            // 
            this.fieldErrorIcon.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink;
            this.fieldErrorIcon.ContainerControl = this;
            this.fieldErrorIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("fieldErrorIcon.Icon")));
            // 
            // GroupBox3
            // 
            this.GroupBox3.Controls.Add(this.Label4);
            this.GroupBox3.Controls.Add(this.txtDescription_REQ);
            this.GroupBox3.Controls.Add(this.cmbLocation_REQ);
            this.GroupBox3.Controls.Add(this.Label5);
            this.GroupBox3.Controls.Add(this.cmbEquipType_REQ);
            this.GroupBox3.Controls.Add(this.Label11);
            this.GroupBox3.Controls.Add(this.Label8);
            this.GroupBox3.Controls.Add(this.cmbStatus_REQ);
            this.GroupBox3.Controls.Add(this.cmbOSType_REQ);
            this.GroupBox3.Controls.Add(this.Label10);
            this.GroupBox3.Location = new System.Drawing.Point(254, 12);
            this.GroupBox3.Name = "GroupBox3";
            this.GroupBox3.Size = new System.Drawing.Size(305, 294);
            this.GroupBox3.TabIndex = 53;
            this.GroupBox3.TabStop = false;
            this.GroupBox3.Text = "Add\'l Info";
            // 
            // GroupBox4
            // 
            this.GroupBox4.Controls.Add(this.lbPurchaseDate);
            this.GroupBox4.Controls.Add(this.dtPurchaseDate_REQ);
            this.GroupBox4.Controls.Add(this.txtReplaceYear);
            this.GroupBox4.Controls.Add(this.Label6);
            this.GroupBox4.Controls.Add(this.txtPO);
            this.GroupBox4.Controls.Add(this.Label9);
            this.GroupBox4.Location = new System.Drawing.Point(565, 12);
            this.GroupBox4.Name = "GroupBox4";
            this.GroupBox4.Size = new System.Drawing.Size(214, 294);
            this.GroupBox4.TabIndex = 54;
            this.GroupBox4.TabStop = false;
            this.GroupBox4.Text = "Purchase Info";
            // 
            // GroupBox5
            // 
            this.GroupBox5.Controls.Add(this.iCloudTextBox);
            this.GroupBox5.Controls.Add(this.Label14);
            this.GroupBox5.Controls.Add(this.txtHostname);
            this.GroupBox5.Controls.Add(this.Label7);
            this.GroupBox5.Controls.Add(this.txtPhoneNumber);
            this.GroupBox5.Controls.Add(this.Label13);
            this.GroupBox5.Controls.Add(this.chkTrackable);
            this.GroupBox5.Location = new System.Drawing.Point(785, 12);
            this.GroupBox5.Name = "GroupBox5";
            this.GroupBox5.Size = new System.Drawing.Size(205, 294);
            this.GroupBox5.TabIndex = 55;
            this.GroupBox5.TabStop = false;
            this.GroupBox5.Text = "Misc";
            // 
            // iCloudTextBox
            // 
            this.iCloudTextBox.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.iCloudTextBox.Location = new System.Drawing.Point(14, 154);
            this.iCloudTextBox.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.iCloudTextBox.Name = "iCloudTextBox";
            this.iCloudTextBox.Size = new System.Drawing.Size(178, 25);
            this.iCloudTextBox.TabIndex = 13;
            // 
            // Label14
            // 
            this.Label14.AutoSize = true;
            this.Label14.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label14.Location = new System.Drawing.Point(11, 134);
            this.Label14.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label14.Name = "Label14";
            this.Label14.Size = new System.Drawing.Size(97, 16);
            this.Label14.TabIndex = 62;
            this.Label14.Text = "iCloud Account";
            // 
            // txtHostname
            // 
            this.txtHostname.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtHostname.Location = new System.Drawing.Point(14, 100);
            this.txtHostname.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.txtHostname.Name = "txtHostname";
            this.txtHostname.Size = new System.Drawing.Size(178, 25);
            this.txtHostname.TabIndex = 12;
            this.txtHostname.Text = "txtHostname";
            // 
            // Label7
            // 
            this.Label7.AutoSize = true;
            this.Label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label7.Location = new System.Drawing.Point(11, 80);
            this.Label7.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label7.Name = "Label7";
            this.Label7.Size = new System.Drawing.Size(70, 16);
            this.Label7.TabIndex = 60;
            this.Label7.Text = "Hostname";
            // 
            // txtPhoneNumber
            // 
            this.txtPhoneNumber.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtPhoneNumber.Location = new System.Drawing.Point(14, 46);
            this.txtPhoneNumber.Mask = "(999) 000-0000";
            this.txtPhoneNumber.Name = "txtPhoneNumber";
            this.txtPhoneNumber.Size = new System.Drawing.Size(178, 25);
            this.txtPhoneNumber.TabIndex = 11;
            this.txtPhoneNumber.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals;
            this.txtPhoneNumber.Leave += new System.EventHandler(this.txtPhoneNumber_Leave);
            // 
            // Label13
            // 
            this.Label13.AutoSize = true;
            this.Label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Label13.Location = new System.Drawing.Point(11, 25);
            this.Label13.Margin = new System.Windows.Forms.Padding(2, 2, 40, 2);
            this.Label13.Name = "Label13";
            this.Label13.Size = new System.Drawing.Size(98, 16);
            this.Label13.TabIndex = 50;
            this.Label13.Text = "Phone Number";
            // 
            // GroupBox6
            // 
            this.GroupBox6.Controls.Add(this.txtNotes);
            this.GroupBox6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.GroupBox6.Location = new System.Drawing.Point(84, 19);
            this.GroupBox6.Name = "GroupBox6";
            this.GroupBox6.Size = new System.Drawing.Size(378, 119);
            this.GroupBox6.TabIndex = 56;
            this.GroupBox6.TabStop = false;
            this.GroupBox6.Text = "Notes";
            // 
            // GroupBox7
            // 
            this.GroupBox7.Controls.Add(this.GroupBox6);
            this.GroupBox7.Location = new System.Drawing.Point(12, 312);
            this.GroupBox7.Name = "GroupBox7";
            this.GroupBox7.Size = new System.Drawing.Size(547, 151);
            this.GroupBox7.TabIndex = 57;
            this.GroupBox7.TabStop = false;
            // 
            // NewDeviceForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(232)))), ((int)(((byte)(232)))), ((int)(((byte)(232)))));
            this.ClientSize = new System.Drawing.Size(997, 472);
            this.Controls.Add(this.chkNoClear);
            this.Controls.Add(this.cmdClear);
            this.Controls.Add(this.GroupBox7);
            this.Controls.Add(this.cmdAdd);
            this.Controls.Add(this.GroupBox5);
            this.Controls.Add(this.GroupBox2);
            this.Controls.Add(this.GroupBox4);
            this.Controls.Add(this.GroupBox3);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "NewDeviceForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Add New Device";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.NewDeviceForm_FormClosed);
            this.Load += new System.EventHandler(this.NewDeviceForm_Load);
            this.GroupBox2.ResumeLayout(false);
            this.GroupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.fieldErrorIcon)).EndInit();
            this.GroupBox3.ResumeLayout(false);
            this.GroupBox3.PerformLayout();
            this.GroupBox4.ResumeLayout(false);
            this.GroupBox4.PerformLayout();
            this.GroupBox5.ResumeLayout(false);
            this.GroupBox5.PerformLayout();
            this.GroupBox6.ResumeLayout(false);
            this.GroupBox6.PerformLayout();
            this.GroupBox7.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        internal Label Label10;
        internal ComboBox cmbOSType_REQ;
        internal Label Label9;
        internal TextBox txtPO;
        internal Label Label8;
        internal ComboBox cmbEquipType_REQ;
        internal TextBox txtNotes;
        internal Button cmdAdd;
        internal Label Label6;
        internal TextBox txtReplaceYear;
        internal Label lbPurchaseDate;
        internal DateTimePicker dtPurchaseDate_REQ;
        internal Label Label5;
        internal ComboBox cmbLocation_REQ;
        internal Label Label4;
        internal Label Label3;
        internal TextBox txtDescription_REQ;
        internal TextBox txtCurUser_REQ;
        internal Label Label2;
        internal Label Label1;
        internal TextBox txtAssetTag_REQ;
        internal TextBox txtSerial_REQ;
        internal Label Label11;
        internal ComboBox cmbStatus_REQ;
        internal Button cmdClear;
        internal CheckBox chkTrackable;
        internal CheckBox chkNoClear;
        internal ErrorProvider fieldErrorIcon;
        internal Button cmdUserSearch;
        internal Label Label12;
        internal GroupBox GroupBox2;
        internal GroupBox GroupBox7;
        internal GroupBox GroupBox6;
        internal GroupBox GroupBox5;
        internal Label Label13;
        internal GroupBox GroupBox4;
        internal GroupBox GroupBox3;
        internal MaskedTextBox txtPhoneNumber;
        internal TextBox txtHostname;
        internal Label Label7;
        internal TextBox iCloudTextBox;
        internal Label Label14;
        internal ToolTip ToolTip1;
    }
}
