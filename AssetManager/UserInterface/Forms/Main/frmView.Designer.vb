<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmView
    Inherits MyForm
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub
    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer
    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmView))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.DeviceInfoBox = New System.Windows.Forms.GroupBox()
        Me.txtPhoneNumber = New System.Windows.Forms.TextBox()
        Me.Label14 = New System.Windows.Forms.Label()
        Me.lblGUID = New System.Windows.Forms.Label()
        Me.cmdMunisSearch = New System.Windows.Forms.Button()
        Me.pnlOtherFunctions = New System.Windows.Forms.Panel()
        Me.cmdMunisInfo = New System.Windows.Forms.Button()
        Me.cmdSibiLink = New System.Windows.Forms.Button()
        Me.cmdSetSibi = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtPONumber = New System.Windows.Forms.TextBox()
        Me.chkTrackable = New System.Windows.Forms.CheckBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtAssetTag_View_REQ = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.txtSerial_View_REQ = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtCurUser_View_REQ = New System.Windows.Forms.TextBox()
        Me.cmbStatus_REQ = New System.Windows.Forms.ComboBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtDescription_View_REQ = New System.Windows.Forms.TextBox()
        Me.cmbOSVersion_REQ = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbLocation_View_REQ = New System.Windows.Forms.ComboBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbEquipType_View_REQ = New System.Windows.Forms.ComboBox()
        Me.dtPurchaseDate_View_REQ = New System.Windows.Forms.DateTimePicker()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtReplacementYear_View = New System.Windows.Forms.TextBox()
        Me.grpNetTools = New System.Windows.Forms.GroupBox()
        Me.cmdGKUpdate = New System.Windows.Forms.Button()
        Me.cmdRestart = New System.Windows.Forms.Button()
        Me.cmdShowIP = New System.Windows.Forms.Button()
        Me.cmdBrowseFiles = New System.Windows.Forms.Button()
        Me.cmdRDP = New System.Windows.Forms.Button()
        Me.RightClickMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.DeleteEntryToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TabControl1 = New System.Windows.Forms.TabControl()
        Me.HistoryTab = New System.Windows.Forms.TabPage()
        Me.DataGridHistory = New System.Windows.Forms.DataGridView()
        Me.TrackingTab = New System.Windows.Forms.TabPage()
        Me.TrackingGrid = New System.Windows.Forms.DataGridView()
        Me.TrackingBox = New System.Windows.Forms.GroupBox()
        Me.txtCheckLocation = New System.Windows.Forms.TextBox()
        Me.Label16 = New System.Windows.Forms.Label()
        Me.txtDueBack = New System.Windows.Forms.TextBox()
        Me.lblDueBack = New System.Windows.Forms.Label()
        Me.txtCheckUser = New System.Windows.Forms.TextBox()
        Me.lblCheckUser = New System.Windows.Forms.Label()
        Me.txtCheckTime = New System.Windows.Forms.TextBox()
        Me.lblCheckTime = New System.Windows.Forms.Label()
        Me.txtCheckOut = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.tmr_RDPRefresher = New System.Windows.Forms.Timer(Me.components)
        Me.fieldErrorIcon = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ToolStrip1 = New AssetManager.MyToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.tsbNewNote = New System.Windows.Forms.ToolStripButton()
        Me.tsbDeleteDevice = New System.Windows.Forms.ToolStripButton()
        Me.AttachmentTool = New System.Windows.Forms.ToolStripButton()
        Me.TrackingTool = New System.Windows.Forms.ToolStripDropDownButton()
        Me.CheckOutTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckInTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsdAssetControl = New System.Windows.Forms.ToolStripDropDownButton()
        Me.tsmAssetInputForm = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmAssetTransferForm = New System.Windows.Forms.ToolStripMenuItem()
        Me.AssetDisposalFormToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmdAccept_Tool = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmdCancel_Tool = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeviceInfoBox.SuspendLayout()
        Me.pnlOtherFunctions.SuspendLayout()
        Me.grpNetTools.SuspendLayout()
        Me.RightClickMenu.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.HistoryTab.SuspendLayout()
        CType(Me.DataGridHistory, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TrackingTab.SuspendLayout()
        CType(Me.TrackingGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TrackingBox.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        CType(Me.fieldErrorIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'DeviceInfoBox
        '
        Me.DeviceInfoBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.DeviceInfoBox.Controls.Add(Me.txtPhoneNumber)
        Me.DeviceInfoBox.Controls.Add(Me.Label14)
        Me.DeviceInfoBox.Controls.Add(Me.lblGUID)
        Me.DeviceInfoBox.Controls.Add(Me.cmdMunisSearch)
        Me.DeviceInfoBox.Controls.Add(Me.pnlOtherFunctions)
        Me.DeviceInfoBox.Controls.Add(Me.cmdSetSibi)
        Me.DeviceInfoBox.Controls.Add(Me.Label12)
        Me.DeviceInfoBox.Controls.Add(Me.txtPONumber)
        Me.DeviceInfoBox.Controls.Add(Me.chkTrackable)
        Me.DeviceInfoBox.Controls.Add(Me.Label1)
        Me.DeviceInfoBox.Controls.Add(Me.txtAssetTag_View_REQ)
        Me.DeviceInfoBox.Controls.Add(Me.Label10)
        Me.DeviceInfoBox.Controls.Add(Me.txtSerial_View_REQ)
        Me.DeviceInfoBox.Controls.Add(Me.Label2)
        Me.DeviceInfoBox.Controls.Add(Me.Label9)
        Me.DeviceInfoBox.Controls.Add(Me.txtCurUser_View_REQ)
        Me.DeviceInfoBox.Controls.Add(Me.cmbStatus_REQ)
        Me.DeviceInfoBox.Controls.Add(Me.Label3)
        Me.DeviceInfoBox.Controls.Add(Me.Label8)
        Me.DeviceInfoBox.Controls.Add(Me.txtDescription_View_REQ)
        Me.DeviceInfoBox.Controls.Add(Me.cmbOSVersion_REQ)
        Me.DeviceInfoBox.Controls.Add(Me.Label4)
        Me.DeviceInfoBox.Controls.Add(Me.cmbLocation_View_REQ)
        Me.DeviceInfoBox.Controls.Add(Me.Label13)
        Me.DeviceInfoBox.Controls.Add(Me.Label5)
        Me.DeviceInfoBox.Controls.Add(Me.cmbEquipType_View_REQ)
        Me.DeviceInfoBox.Controls.Add(Me.dtPurchaseDate_View_REQ)
        Me.DeviceInfoBox.Controls.Add(Me.Label7)
        Me.DeviceInfoBox.Controls.Add(Me.Label6)
        Me.DeviceInfoBox.Controls.Add(Me.txtReplacementYear_View)
        Me.DeviceInfoBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DeviceInfoBox.Location = New System.Drawing.Point(12, 46)
        Me.DeviceInfoBox.Name = "DeviceInfoBox"
        Me.DeviceInfoBox.Size = New System.Drawing.Size(810, 295)
        Me.DeviceInfoBox.TabIndex = 39
        Me.DeviceInfoBox.TabStop = False
        Me.DeviceInfoBox.Text = "Current Info"
        '
        'txtPhoneNumber
        '
        Me.txtPhoneNumber.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhoneNumber.Location = New System.Drawing.Point(612, 183)
        Me.txtPhoneNumber.Name = "txtPhoneNumber"
        Me.txtPhoneNumber.Size = New System.Drawing.Size(169, 23)
        Me.txtPhoneNumber.TabIndex = 12
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label14.Location = New System.Drawing.Point(609, 164)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(60, 16)
        Me.Label14.TabIndex = 56
        Me.Label14.Text = "Phone #:"
        '
        'lblGUID
        '
        Me.lblGUID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.lblGUID.Cursor = System.Windows.Forms.Cursors.Default
        Me.lblGUID.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblGUID.Location = New System.Drawing.Point(22, 213)
        Me.lblGUID.Name = "lblGUID"
        Me.lblGUID.Size = New System.Drawing.Size(272, 34)
        Me.lblGUID.TabIndex = 54
        Me.lblGUID.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.ToolTip1.SetToolTip(Me.lblGUID, "Click to copy GUID.")
        '
        'cmdMunisSearch
        '
        Me.cmdMunisSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdMunisSearch.Location = New System.Drawing.Point(20, 160)
        Me.cmdMunisSearch.Name = "cmdMunisSearch"
        Me.cmdMunisSearch.Size = New System.Drawing.Size(134, 23)
        Me.cmdMunisSearch.TabIndex = 3
        Me.cmdMunisSearch.Text = "Munis Search"
        Me.cmdMunisSearch.UseVisualStyleBackColor = True
        Me.cmdMunisSearch.Visible = False
        '
        'pnlOtherFunctions
        '
        Me.pnlOtherFunctions.Controls.Add(Me.cmdMunisInfo)
        Me.pnlOtherFunctions.Controls.Add(Me.cmdSibiLink)
        Me.pnlOtherFunctions.Location = New System.Drawing.Point(433, 213)
        Me.pnlOtherFunctions.Name = "pnlOtherFunctions"
        Me.pnlOtherFunctions.Size = New System.Drawing.Size(116, 61)
        Me.pnlOtherFunctions.TabIndex = 51
        '
        'cmdMunisInfo
        '
        Me.cmdMunisInfo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdMunisInfo.Location = New System.Drawing.Point(5, 4)
        Me.cmdMunisInfo.Name = "cmdMunisInfo"
        Me.cmdMunisInfo.Size = New System.Drawing.Size(106, 23)
        Me.cmdMunisInfo.TabIndex = 46
        Me.cmdMunisInfo.Text = "MUNIS Info"
        Me.cmdMunisInfo.UseVisualStyleBackColor = True
        '
        'cmdSibiLink
        '
        Me.cmdSibiLink.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSibiLink.Location = New System.Drawing.Point(5, 33)
        Me.cmdSibiLink.Name = "cmdSibiLink"
        Me.cmdSibiLink.Size = New System.Drawing.Size(106, 23)
        Me.cmdSibiLink.TabIndex = 49
        Me.cmdSibiLink.Text = "Sibi Info"
        Me.cmdSibiLink.UseVisualStyleBackColor = True
        '
        'cmdSetSibi
        '
        Me.cmdSetSibi.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSetSibi.Location = New System.Drawing.Point(100, 250)
        Me.cmdSetSibi.Name = "cmdSetSibi"
        Me.cmdSetSibi.Size = New System.Drawing.Size(106, 23)
        Me.cmdSetSibi.TabIndex = 50
        Me.cmdSetSibi.Text = "Set Sibi Link"
        Me.cmdSetSibi.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(609, 119)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(81, 16)
        Me.Label12.TabIndex = 48
        Me.Label12.Text = "PO Number:"
        '
        'txtPONumber
        '
        Me.txtPONumber.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPONumber.Location = New System.Drawing.Point(612, 138)
        Me.txtPONumber.Name = "txtPONumber"
        Me.txtPONumber.Size = New System.Drawing.Size(169, 23)
        Me.txtPONumber.TabIndex = 11
        '
        'chkTrackable
        '
        Me.chkTrackable.AutoSize = True
        Me.chkTrackable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTrackable.Location = New System.Drawing.Point(651, 216)
        Me.chkTrackable.Name = "chkTrackable"
        Me.chkTrackable.Size = New System.Drawing.Size(89, 20)
        Me.chkTrackable.TabIndex = 13
        Me.chkTrackable.Text = "Trackable"
        Me.chkTrackable.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(18, 68)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 16)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Asset Tag:"
        '
        'txtAssetTag_View_REQ
        '
        Me.txtAssetTag_View_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAssetTag_View_REQ.Location = New System.Drawing.Point(21, 87)
        Me.txtAssetTag_View_REQ.Name = "txtAssetTag_View_REQ"
        Me.txtAssetTag_View_REQ.Size = New System.Drawing.Size(134, 23)
        Me.txtAssetTag_View_REQ.TabIndex = 1
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(18, 193)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(90, 16)
        Me.Label10.TabIndex = 41
        Me.Label10.Text = "Device GUID:"
        '
        'txtSerial_View_REQ
        '
        Me.txtSerial_View_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerial_View_REQ.Location = New System.Drawing.Point(21, 42)
        Me.txtSerial_View_REQ.Name = "txtSerial_View_REQ"
        Me.txtSerial_View_REQ.Size = New System.Drawing.Size(134, 23)
        Me.txtSerial_View_REQ.TabIndex = 0
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(18, 23)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 16)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "Serial:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(395, 116)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(48, 16)
        Me.Label9.TabIndex = 39
        Me.Label9.Text = "Status:"
        '
        'txtCurUser_View_REQ
        '
        Me.txtCurUser_View_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurUser_View_REQ.Location = New System.Drawing.Point(21, 132)
        Me.txtCurUser_View_REQ.Name = "txtCurUser_View_REQ"
        Me.txtCurUser_View_REQ.Size = New System.Drawing.Size(134, 23)
        Me.txtCurUser_View_REQ.TabIndex = 2
        '
        'cmbStatus_REQ
        '
        Me.cmbStatus_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStatus_REQ.FormattingEnabled = True
        Me.cmbStatus_REQ.Location = New System.Drawing.Point(398, 135)
        Me.cmbStatus_REQ.Name = "cmbStatus_REQ"
        Me.cmbStatus_REQ.Size = New System.Drawing.Size(177, 23)
        Me.cmbStatus_REQ.TabIndex = 8
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(17, 113)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(85, 16)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "Current User:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(183, 116)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(79, 16)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "OS Version:"
        '
        'txtDescription_View_REQ
        '
        Me.txtDescription_View_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription_View_REQ.Location = New System.Drawing.Point(186, 42)
        Me.txtDescription_View_REQ.Name = "txtDescription_View_REQ"
        Me.txtDescription_View_REQ.Size = New System.Drawing.Size(389, 23)
        Me.txtDescription_View_REQ.TabIndex = 4
        '
        'cmbOSVersion_REQ
        '
        Me.cmbOSVersion_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbOSVersion_REQ.FormattingEnabled = True
        Me.cmbOSVersion_REQ.Location = New System.Drawing.Point(186, 135)
        Me.cmbOSVersion_REQ.Name = "cmbOSVersion_REQ"
        Me.cmbOSVersion_REQ.Size = New System.Drawing.Size(177, 23)
        Me.cmbOSVersion_REQ.TabIndex = 6
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(181, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 16)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "Description:"
        '
        'cmbLocation_View_REQ
        '
        Me.cmbLocation_View_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLocation_View_REQ.FormattingEnabled = True
        Me.cmbLocation_View_REQ.Location = New System.Drawing.Point(398, 90)
        Me.cmbLocation_View_REQ.Name = "cmbLocation_View_REQ"
        Me.cmbLocation_View_REQ.Size = New System.Drawing.Size(177, 23)
        Me.cmbLocation_View_REQ.TabIndex = 7
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(183, 68)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(110, 16)
        Me.Label13.TabIndex = 34
        Me.Label13.Text = "Equipment Type:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(395, 71)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 16)
        Me.Label5.TabIndex = 28
        Me.Label5.Text = "Location:"
        '
        'cmbEquipType_View_REQ
        '
        Me.cmbEquipType_View_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbEquipType_View_REQ.FormattingEnabled = True
        Me.cmbEquipType_View_REQ.Location = New System.Drawing.Point(186, 87)
        Me.cmbEquipType_View_REQ.Name = "cmbEquipType_View_REQ"
        Me.cmbEquipType_View_REQ.Size = New System.Drawing.Size(177, 23)
        Me.cmbEquipType_View_REQ.TabIndex = 5
        '
        'dtPurchaseDate_View_REQ
        '
        Me.dtPurchaseDate_View_REQ.CustomFormat = "yyyy-MM-dd"
        Me.dtPurchaseDate_View_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtPurchaseDate_View_REQ.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtPurchaseDate_View_REQ.Location = New System.Drawing.Point(614, 43)
        Me.dtPurchaseDate_View_REQ.Name = "dtPurchaseDate_View_REQ"
        Me.dtPurchaseDate_View_REQ.Size = New System.Drawing.Size(168, 23)
        Me.dtPurchaseDate_View_REQ.TabIndex = 9
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(609, 71)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(95, 16)
        Me.Label7.TabIndex = 32
        Me.Label7.Text = "Replace Year:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(609, 24)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 16)
        Me.Label6.TabIndex = 30
        Me.Label6.Text = "Purchase Date:"
        '
        'txtReplacementYear_View
        '
        Me.txtReplacementYear_View.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReplacementYear_View.Location = New System.Drawing.Point(612, 90)
        Me.txtReplacementYear_View.Name = "txtReplacementYear_View"
        Me.txtReplacementYear_View.Size = New System.Drawing.Size(169, 23)
        Me.txtReplacementYear_View.TabIndex = 10
        '
        'grpNetTools
        '
        Me.grpNetTools.Controls.Add(Me.cmdGKUpdate)
        Me.grpNetTools.Controls.Add(Me.cmdRestart)
        Me.grpNetTools.Controls.Add(Me.cmdShowIP)
        Me.grpNetTools.Controls.Add(Me.cmdBrowseFiles)
        Me.grpNetTools.Controls.Add(Me.cmdRDP)
        Me.grpNetTools.FlatStyle = System.Windows.Forms.FlatStyle.System
        Me.grpNetTools.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.grpNetTools.Location = New System.Drawing.Point(828, 46)
        Me.grpNetTools.Name = "grpNetTools"
        Me.grpNetTools.Size = New System.Drawing.Size(320, 113)
        Me.grpNetTools.TabIndex = 52
        Me.grpNetTools.TabStop = False
        Me.grpNetTools.Text = "Remote Mgmt"
        Me.grpNetTools.Visible = False
        '
        'cmdGKUpdate
        '
        Me.cmdGKUpdate.BackgroundImage = CType(resources.GetObject("cmdGKUpdate.BackgroundImage"), System.Drawing.Image)
        Me.cmdGKUpdate.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.cmdGKUpdate.Location = New System.Drawing.Point(12, 34)
        Me.cmdGKUpdate.Margin = New System.Windows.Forms.Padding(1)
        Me.cmdGKUpdate.Name = "cmdGKUpdate"
        Me.cmdGKUpdate.Size = New System.Drawing.Size(50, 50)
        Me.cmdGKUpdate.TabIndex = 55
        Me.ToolTip1.SetToolTip(Me.cmdGKUpdate, "Update Gatekeeper")
        Me.cmdGKUpdate.UseVisualStyleBackColor = True
        '
        'cmdRestart
        '
        Me.cmdRestart.BackgroundImage = CType(resources.GetObject("cmdRestart.BackgroundImage"), System.Drawing.Image)
        Me.cmdRestart.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.cmdRestart.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdRestart.Location = New System.Drawing.Point(116, 34)
        Me.cmdRestart.Margin = New System.Windows.Forms.Padding(1)
        Me.cmdRestart.Name = "cmdRestart"
        Me.cmdRestart.Size = New System.Drawing.Size(50, 50)
        Me.cmdRestart.TabIndex = 54
        Me.ToolTip1.SetToolTip(Me.cmdRestart, "Restart Device")
        Me.cmdRestart.UseVisualStyleBackColor = True
        '
        'cmdShowIP
        '
        Me.cmdShowIP.BackColor = System.Drawing.Color.Black
        Me.cmdShowIP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom
        Me.cmdShowIP.FlatStyle = System.Windows.Forms.FlatStyle.Popup
        Me.cmdShowIP.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdShowIP.ForeColor = System.Drawing.Color.White
        Me.cmdShowIP.Location = New System.Drawing.Point(224, 15)
        Me.cmdShowIP.Name = "cmdShowIP"
        Me.cmdShowIP.Size = New System.Drawing.Size(90, 90)
        Me.cmdShowIP.TabIndex = 53
        Me.cmdShowIP.TextAlign = System.Drawing.ContentAlignment.TopRight
        Me.ToolTip1.SetToolTip(Me.cmdShowIP, "Show IP")
        Me.cmdShowIP.UseVisualStyleBackColor = False
        '
        'cmdBrowseFiles
        '
        Me.cmdBrowseFiles.BackgroundImage = CType(resources.GetObject("cmdBrowseFiles.BackgroundImage"), System.Drawing.Image)
        Me.cmdBrowseFiles.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.cmdBrowseFiles.Location = New System.Drawing.Point(64, 34)
        Me.cmdBrowseFiles.Margin = New System.Windows.Forms.Padding(1)
        Me.cmdBrowseFiles.Name = "cmdBrowseFiles"
        Me.cmdBrowseFiles.Size = New System.Drawing.Size(50, 50)
        Me.cmdBrowseFiles.TabIndex = 52
        Me.ToolTip1.SetToolTip(Me.cmdBrowseFiles, "Browse Files")
        Me.cmdBrowseFiles.UseVisualStyleBackColor = True
        '
        'cmdRDP
        '
        Me.cmdRDP.BackgroundImage = CType(resources.GetObject("cmdRDP.BackgroundImage"), System.Drawing.Image)
        Me.cmdRDP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.cmdRDP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdRDP.Location = New System.Drawing.Point(168, 34)
        Me.cmdRDP.Margin = New System.Windows.Forms.Padding(1)
        Me.cmdRDP.Name = "cmdRDP"
        Me.cmdRDP.Size = New System.Drawing.Size(50, 50)
        Me.cmdRDP.TabIndex = 46
        Me.ToolTip1.SetToolTip(Me.cmdRDP, "Launch Remote Desktop")
        Me.cmdRDP.UseVisualStyleBackColor = True
        '
        'RightClickMenu
        '
        Me.RightClickMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteEntryToolStripMenuItem})
        Me.RightClickMenu.Name = "RightClickMenu"
        Me.RightClickMenu.Size = New System.Drawing.Size(138, 26)
        '
        'DeleteEntryToolStripMenuItem
        '
        Me.DeleteEntryToolStripMenuItem.Image = CType(resources.GetObject("DeleteEntryToolStripMenuItem.Image"), System.Drawing.Image)
        Me.DeleteEntryToolStripMenuItem.Name = "DeleteEntryToolStripMenuItem"
        Me.DeleteEntryToolStripMenuItem.Size = New System.Drawing.Size(137, 22)
        Me.DeleteEntryToolStripMenuItem.Text = "Delete Entry"
        '
        'TabControl1
        '
        Me.TabControl1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TabControl1.Controls.Add(Me.HistoryTab)
        Me.TabControl1.Controls.Add(Me.TrackingTab)
        Me.TabControl1.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TabControl1.ItemSize = New System.Drawing.Size(61, 21)
        Me.TabControl1.Location = New System.Drawing.Point(12, 347)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(1140, 304)
        Me.TabControl1.TabIndex = 40
        '
        'HistoryTab
        '
        Me.HistoryTab.Controls.Add(Me.DataGridHistory)
        Me.HistoryTab.Location = New System.Drawing.Point(4, 25)
        Me.HistoryTab.Name = "HistoryTab"
        Me.HistoryTab.Padding = New System.Windows.Forms.Padding(3)
        Me.HistoryTab.Size = New System.Drawing.Size(1132, 275)
        Me.HistoryTab.TabIndex = 0
        Me.HistoryTab.Text = "History"
        Me.HistoryTab.UseVisualStyleBackColor = True
        '
        'DataGridHistory
        '
        Me.DataGridHistory.AllowUserToAddRows = False
        Me.DataGridHistory.AllowUserToDeleteRows = False
        Me.DataGridHistory.AllowUserToResizeColumns = False
        Me.DataGridHistory.AllowUserToResizeRows = False
        Me.DataGridHistory.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridHistory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.DataGridHistory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.DataGridHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridHistory.ContextMenuStrip = Me.RightClickMenu
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.Padding = New System.Windows.Forms.Padding(10)
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.DataGridHistory.DefaultCellStyle = DataGridViewCellStyle1
        Me.DataGridHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.DataGridHistory.Location = New System.Drawing.Point(6, 6)
        Me.DataGridHistory.Name = "DataGridHistory"
        Me.DataGridHistory.ReadOnly = True
        Me.DataGridHistory.RowHeadersVisible = False
        Me.DataGridHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.DataGridHistory.ShowCellToolTips = False
        Me.DataGridHistory.ShowEditingIcon = False
        Me.DataGridHistory.Size = New System.Drawing.Size(1120, 263)
        Me.DataGridHistory.TabIndex = 40
        Me.DataGridHistory.TabStop = False
        Me.DataGridHistory.VirtualMode = True
        '
        'TrackingTab
        '
        Me.TrackingTab.Controls.Add(Me.TrackingGrid)
        Me.TrackingTab.Location = New System.Drawing.Point(4, 25)
        Me.TrackingTab.Name = "TrackingTab"
        Me.TrackingTab.Padding = New System.Windows.Forms.Padding(3)
        Me.TrackingTab.Size = New System.Drawing.Size(1132, 275)
        Me.TrackingTab.TabIndex = 1
        Me.TrackingTab.Text = "Tracking"
        Me.TrackingTab.UseVisualStyleBackColor = True
        '
        'TrackingGrid
        '
        Me.TrackingGrid.AllowUserToAddRows = False
        Me.TrackingGrid.AllowUserToDeleteRows = False
        Me.TrackingGrid.AllowUserToResizeColumns = False
        Me.TrackingGrid.AllowUserToResizeRows = False
        Me.TrackingGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TrackingGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.TrackingGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.TrackingGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.TrackingGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.TrackingGrid.Location = New System.Drawing.Point(6, 6)
        Me.TrackingGrid.MultiSelect = False
        Me.TrackingGrid.Name = "TrackingGrid"
        Me.TrackingGrid.ReadOnly = True
        Me.TrackingGrid.RowHeadersVisible = False
        Me.TrackingGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.TrackingGrid.ShowCellToolTips = False
        Me.TrackingGrid.ShowEditingIcon = False
        Me.TrackingGrid.Size = New System.Drawing.Size(1274, 586)
        Me.TrackingGrid.TabIndex = 41
        '
        'TrackingBox
        '
        Me.TrackingBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.TrackingBox.Controls.Add(Me.txtCheckLocation)
        Me.TrackingBox.Controls.Add(Me.Label16)
        Me.TrackingBox.Controls.Add(Me.txtDueBack)
        Me.TrackingBox.Controls.Add(Me.lblDueBack)
        Me.TrackingBox.Controls.Add(Me.txtCheckUser)
        Me.TrackingBox.Controls.Add(Me.lblCheckUser)
        Me.TrackingBox.Controls.Add(Me.txtCheckTime)
        Me.TrackingBox.Controls.Add(Me.lblCheckTime)
        Me.TrackingBox.Controls.Add(Me.txtCheckOut)
        Me.TrackingBox.Controls.Add(Me.Label11)
        Me.TrackingBox.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.TrackingBox.Location = New System.Drawing.Point(828, 161)
        Me.TrackingBox.Name = "TrackingBox"
        Me.TrackingBox.Size = New System.Drawing.Size(320, 180)
        Me.TrackingBox.TabIndex = 41
        Me.TrackingBox.TabStop = False
        Me.TrackingBox.Text = "Tracking Info"
        Me.TrackingBox.Visible = False
        '
        'txtCheckLocation
        '
        Me.txtCheckLocation.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtCheckLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCheckLocation.Location = New System.Drawing.Point(169, 42)
        Me.txtCheckLocation.Name = "txtCheckLocation"
        Me.txtCheckLocation.ReadOnly = True
        Me.txtCheckLocation.Size = New System.Drawing.Size(134, 22)
        Me.txtCheckLocation.TabIndex = 57
        Me.txtCheckLocation.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label16
        '
        Me.Label16.AutoSize = True
        Me.Label16.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label16.Location = New System.Drawing.Point(166, 23)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(62, 16)
        Me.Label16.TabIndex = 56
        Me.Label16.Text = "Location:"
        '
        'txtDueBack
        '
        Me.txtDueBack.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtDueBack.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDueBack.Location = New System.Drawing.Point(86, 141)
        Me.txtDueBack.Name = "txtDueBack"
        Me.txtDueBack.ReadOnly = True
        Me.txtDueBack.Size = New System.Drawing.Size(134, 22)
        Me.txtDueBack.TabIndex = 55
        Me.txtDueBack.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblDueBack
        '
        Me.lblDueBack.AutoSize = True
        Me.lblDueBack.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblDueBack.Location = New System.Drawing.Point(83, 122)
        Me.lblDueBack.Name = "lblDueBack"
        Me.lblDueBack.Size = New System.Drawing.Size(70, 16)
        Me.lblDueBack.TabIndex = 54
        Me.lblDueBack.Text = "Due Back:"
        '
        'txtCheckUser
        '
        Me.txtCheckUser.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtCheckUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCheckUser.Location = New System.Drawing.Point(169, 93)
        Me.txtCheckUser.Name = "txtCheckUser"
        Me.txtCheckUser.ReadOnly = True
        Me.txtCheckUser.Size = New System.Drawing.Size(134, 22)
        Me.txtCheckUser.TabIndex = 53
        Me.txtCheckUser.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblCheckUser
        '
        Me.lblCheckUser.AutoSize = True
        Me.lblCheckUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCheckUser.Location = New System.Drawing.Point(166, 74)
        Me.lblCheckUser.Name = "lblCheckUser"
        Me.lblCheckUser.Size = New System.Drawing.Size(101, 16)
        Me.lblCheckUser.TabIndex = 52
        Me.lblCheckUser.Text = "CheckOut User:"
        '
        'txtCheckTime
        '
        Me.txtCheckTime.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtCheckTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCheckTime.Location = New System.Drawing.Point(19, 93)
        Me.txtCheckTime.Name = "txtCheckTime"
        Me.txtCheckTime.ReadOnly = True
        Me.txtCheckTime.Size = New System.Drawing.Size(134, 22)
        Me.txtCheckTime.TabIndex = 51
        Me.txtCheckTime.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'lblCheckTime
        '
        Me.lblCheckTime.AutoSize = True
        Me.lblCheckTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCheckTime.Location = New System.Drawing.Point(16, 74)
        Me.lblCheckTime.Name = "lblCheckTime"
        Me.lblCheckTime.Size = New System.Drawing.Size(103, 16)
        Me.lblCheckTime.TabIndex = 50
        Me.lblCheckTime.Text = "CheckOut Time:"
        '
        'txtCheckOut
        '
        Me.txtCheckOut.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtCheckOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCheckOut.Location = New System.Drawing.Point(19, 42)
        Me.txtCheckOut.Name = "txtCheckOut"
        Me.txtCheckOut.ReadOnly = True
        Me.txtCheckOut.Size = New System.Drawing.Size(134, 26)
        Me.txtCheckOut.TabIndex = 49
        Me.txtCheckOut.Text = "STATUS"
        Me.txtCheckOut.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(16, 23)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(48, 16)
        Me.Label11.TabIndex = 48
        Me.Label11.Text = "Status:"
        '
        'ToolTip1
        '
        Me.ToolTip1.AutoPopDelay = 5000
        Me.ToolTip1.InitialDelay = 100
        Me.ToolTip1.ReshowDelay = 100
        '
        'StatusStrip1
        '
        Me.StatusStrip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 654)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1161, 22)
        Me.StatusStrip1.TabIndex = 45
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'StatusLabel
        '
        Me.StatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(76, 17)
        Me.StatusLabel.Text = "%STATUS%"
        '
        'tmr_RDPRefresher
        '
        Me.tmr_RDPRefresher.Interval = 250
        '
        'fieldErrorIcon
        '
        Me.fieldErrorIcon.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.fieldErrorIcon.ContainerControl = Me
        Me.fieldErrorIcon.Icon = CType(resources.GetObject("fieldErrorIcon.Icon"), System.Drawing.Icon)
        '
        'ToolStrip1
        '
        Me.ToolStrip1.AutoSize = False
        Me.ToolStrip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(249, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ToolStrip1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(25, 25)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.tsbNewNote, Me.tsbDeleteDevice, Me.AttachmentTool, Me.TrackingTool, Me.tsdAssetControl, Me.ToolStripSeparator1, Me.cmdAccept_Tool, Me.ToolStripSeparator3, Me.cmdCancel_Tool, Me.ToolStripSeparator2})
        Me.ToolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.ToolStrip1.Size = New System.Drawing.Size(1161, 37)
        Me.ToolStrip1.Stretch = True
        Me.ToolStrip1.TabIndex = 44
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ToolStripButton1.Image = CType(resources.GetObject("ToolStripButton1.Image"), System.Drawing.Image)
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Padding = New System.Windows.Forms.Padding(5, 5, 5, 0)
        Me.ToolStripButton1.Size = New System.Drawing.Size(39, 34)
        Me.ToolStripButton1.Text = "Modify"
        '
        'tsbNewNote
        '
        Me.tsbNewNote.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbNewNote.Image = Global.AssetManager.My.Resources.Resources.add_note_icon
        Me.tsbNewNote.Name = "tsbNewNote"
        Me.tsbNewNote.Padding = New System.Windows.Forms.Padding(5, 5, 5, 0)
        Me.tsbNewNote.Size = New System.Drawing.Size(39, 34)
        Me.tsbNewNote.Text = "Add Note"
        '
        'tsbDeleteDevice
        '
        Me.tsbDeleteDevice.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbDeleteDevice.Image = CType(resources.GetObject("tsbDeleteDevice.Image"), System.Drawing.Image)
        Me.tsbDeleteDevice.Name = "tsbDeleteDevice"
        Me.tsbDeleteDevice.Padding = New System.Windows.Forms.Padding(5, 5, 5, 0)
        Me.tsbDeleteDevice.Size = New System.Drawing.Size(39, 34)
        Me.tsbDeleteDevice.Text = "Delete Device"
        '
        'AttachmentTool
        '
        Me.AttachmentTool.Image = CType(resources.GetObject("AttachmentTool.Image"), System.Drawing.Image)
        Me.AttachmentTool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AttachmentTool.Name = "AttachmentTool"
        Me.AttachmentTool.Padding = New System.Windows.Forms.Padding(5, 5, 5, 0)
        Me.AttachmentTool.Size = New System.Drawing.Size(136, 34)
        Me.AttachmentTool.Text = "Attachments"
        '
        'TrackingTool
        '
        Me.TrackingTool.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CheckOutTool, Me.CheckInTool})
        Me.TrackingTool.Image = CType(resources.GetObject("TrackingTool.Image"), System.Drawing.Image)
        Me.TrackingTool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.TrackingTool.Name = "TrackingTool"
        Me.TrackingTool.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.TrackingTool.Size = New System.Drawing.Size(116, 34)
        Me.TrackingTool.Text = "Tracking"
        '
        'CheckOutTool
        '
        Me.CheckOutTool.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckOutTool.Image = CType(resources.GetObject("CheckOutTool.Image"), System.Drawing.Image)
        Me.CheckOutTool.Name = "CheckOutTool"
        Me.CheckOutTool.Size = New System.Drawing.Size(144, 32)
        Me.CheckOutTool.Text = "Check Out"
        '
        'CheckInTool
        '
        Me.CheckInTool.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckInTool.Image = CType(resources.GetObject("CheckInTool.Image"), System.Drawing.Image)
        Me.CheckInTool.Name = "CheckInTool"
        Me.CheckInTool.Size = New System.Drawing.Size(144, 32)
        Me.CheckInTool.Text = "Check In"
        '
        'tsdAssetControl
        '
        Me.tsdAssetControl.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmAssetInputForm, Me.tsmAssetTransferForm, Me.AssetDisposalFormToolStripMenuItem})
        Me.tsdAssetControl.Image = CType(resources.GetObject("tsdAssetControl.Image"), System.Drawing.Image)
        Me.tsdAssetControl.Name = "tsdAssetControl"
        Me.tsdAssetControl.Size = New System.Drawing.Size(189, 34)
        Me.tsdAssetControl.Text = "Asset Control Forms"
        '
        'tsmAssetInputForm
        '
        Me.tsmAssetInputForm.Image = CType(resources.GetObject("tsmAssetInputForm.Image"), System.Drawing.Image)
        Me.tsmAssetInputForm.Name = "tsmAssetInputForm"
        Me.tsmAssetInputForm.Size = New System.Drawing.Size(230, 32)
        Me.tsmAssetInputForm.Text = "Asset Input Form"
        '
        'tsmAssetTransferForm
        '
        Me.tsmAssetTransferForm.Image = CType(resources.GetObject("tsmAssetTransferForm.Image"), System.Drawing.Image)
        Me.tsmAssetTransferForm.Name = "tsmAssetTransferForm"
        Me.tsmAssetTransferForm.Size = New System.Drawing.Size(230, 32)
        Me.tsmAssetTransferForm.Text = "Asset Transfer Form"
        '
        'AssetDisposalFormToolStripMenuItem
        '
        Me.AssetDisposalFormToolStripMenuItem.Image = CType(resources.GetObject("AssetDisposalFormToolStripMenuItem.Image"), System.Drawing.Image)
        Me.AssetDisposalFormToolStripMenuItem.Name = "AssetDisposalFormToolStripMenuItem"
        Me.AssetDisposalFormToolStripMenuItem.Size = New System.Drawing.Size(230, 32)
        Me.AssetDisposalFormToolStripMenuItem.Text = "Asset Disposal Form"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 37)
        Me.ToolStripSeparator1.Visible = False
        '
        'cmdAccept_Tool
        '
        Me.cmdAccept_Tool.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAccept_Tool.Image = CType(resources.GetObject("cmdAccept_Tool.Image"), System.Drawing.Image)
        Me.cmdAccept_Tool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdAccept_Tool.Name = "cmdAccept_Tool"
        Me.cmdAccept_Tool.Padding = New System.Windows.Forms.Padding(100, 0, 0, 0)
        Me.cmdAccept_Tool.Size = New System.Drawing.Size(178, 34)
        Me.cmdAccept_Tool.Text = "Accept"
        Me.cmdAccept_Tool.Visible = False
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 37)
        Me.ToolStripSeparator3.Visible = False
        '
        'cmdCancel_Tool
        '
        Me.cmdCancel_Tool.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel_Tool.Image = CType(resources.GetObject("cmdCancel_Tool.Image"), System.Drawing.Image)
        Me.cmdCancel_Tool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdCancel_Tool.Name = "cmdCancel_Tool"
        Me.cmdCancel_Tool.Padding = New System.Windows.Forms.Padding(50, 0, 0, 0)
        Me.cmdCancel_Tool.Size = New System.Drawing.Size(126, 34)
        Me.cmdCancel_Tool.Text = "Cancel"
        Me.cmdCancel_Tool.Visible = False
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 37)
        Me.ToolStripSeparator2.Visible = False
        '
        'frmView
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1161, 676)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.grpNetTools)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.TrackingBox)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.DeviceInfoBox)
        Me.DoubleBuffered = True
        Me.MinimumSize = New System.Drawing.Size(1177, 478)
        Me.Name = "frmView"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "View"
        Me.DeviceInfoBox.ResumeLayout(False)
        Me.DeviceInfoBox.PerformLayout()
        Me.pnlOtherFunctions.ResumeLayout(False)
        Me.grpNetTools.ResumeLayout(False)
        Me.RightClickMenu.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.HistoryTab.ResumeLayout(False)
        CType(Me.DataGridHistory, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TrackingTab.ResumeLayout(False)
        CType(Me.TrackingGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TrackingBox.ResumeLayout(False)
        Me.TrackingBox.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        CType(Me.fieldErrorIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents DeviceInfoBox As GroupBox
    Friend WithEvents Label9 As Label
    Friend WithEvents cmbStatus_REQ As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents cmbOSVersion_REQ As ComboBox
    Friend WithEvents Label13 As Label
    Friend WithEvents cmbEquipType_View_REQ As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtReplacementYear_View As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents dtPurchaseDate_View_REQ As DateTimePicker
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbLocation_View_REQ As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtDescription_View_REQ As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtCurUser_View_REQ As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtSerial_View_REQ As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtAssetTag_View_REQ As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents RightClickMenu As ContextMenuStrip
    Friend WithEvents DeleteEntryToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents chkTrackable As CheckBox
    Friend WithEvents TabControl1 As TabControl
    Friend WithEvents HistoryTab As TabPage
    Friend WithEvents DataGridHistory As DataGridView
    Friend WithEvents TrackingTab As TabPage
    Friend WithEvents TrackingGrid As DataGridView
    Friend WithEvents TrackingBox As GroupBox
    Friend WithEvents txtCheckOut As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtCheckTime As TextBox
    Friend WithEvents lblCheckTime As Label
    Friend WithEvents txtCheckUser As TextBox
    Friend WithEvents lblCheckUser As Label
    Friend WithEvents txtCheckLocation As TextBox
    Friend WithEvents Label16 As Label
    Friend WithEvents txtDueBack As TextBox
    Friend WithEvents lblDueBack As Label
    Friend WithEvents ToolTip1 As ToolTip
    Friend WithEvents ToolStrip1 As MyToolStrip
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents tsbNewNote As ToolStripButton
    Friend WithEvents tsbDeleteDevice As ToolStripButton
    Friend WithEvents TrackingTool As ToolStripDropDownButton
    Friend WithEvents CheckOutTool As ToolStripMenuItem
    Friend WithEvents CheckInTool As ToolStripMenuItem
    Friend WithEvents AttachmentTool As ToolStripButton
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents StatusLabel As ToolStripStatusLabel
    Friend WithEvents cmdCancel_Tool As ToolStripButton
    Friend WithEvents cmdAccept_Tool As ToolStripButton
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents cmdMunisInfo As Button
    Friend WithEvents cmdRDP As Button
    Friend WithEvents Label12 As Label
    Friend WithEvents txtPONumber As TextBox
    Friend WithEvents tmr_RDPRefresher As Timer
    Friend WithEvents cmdSibiLink As Button
    Friend WithEvents cmdSetSibi As Button
    Friend WithEvents pnlOtherFunctions As Panel
    Friend WithEvents fieldErrorIcon As ErrorProvider
    Friend WithEvents cmdBrowseFiles As Button
    Friend WithEvents grpNetTools As GroupBox
    Friend WithEvents tsdAssetControl As ToolStripDropDownButton
    Friend WithEvents tsmAssetInputForm As ToolStripMenuItem
    Friend WithEvents tsmAssetTransferForm As ToolStripMenuItem
    Friend WithEvents cmdMunisSearch As Button
    Friend WithEvents lblGUID As Label
    Friend WithEvents AssetDisposalFormToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents cmdShowIP As Button
    Friend WithEvents txtPhoneNumber As TextBox
    Friend WithEvents Label14 As Label
    Friend WithEvents cmdRestart As Button
    Friend WithEvents cmdGKUpdate As Button
End Class
