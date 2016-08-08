<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class View
    Inherits System.Windows.Forms.Form
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(View))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ActionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.AddNoteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeleteDeviceToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem1 = New System.Windows.Forms.ToolStripSeparator()
        Me.TrackingToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckInMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckOutMenu = New System.Windows.Forms.ToolStripMenuItem()
        Me.DeviceInfoBox = New System.Windows.Forms.GroupBox()
        Me.pnlOtherFunctions = New System.Windows.Forms.Panel()
        Me.cmdRDP = New System.Windows.Forms.Button()
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
        Me.txtGUID = New System.Windows.Forms.TextBox()
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
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton2 = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripButton3 = New System.Windows.Forms.ToolStripButton()
        Me.AttachmentTool = New System.Windows.Forms.ToolStripButton()
        Me.TrackingTool = New System.Windows.Forms.ToolStripDropDownButton()
        Me.CheckOutTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.CheckInTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmdAccept_Tool = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmdCancel_Tool = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.PingWorker = New System.ComponentModel.BackgroundWorker()
        Me.BackgroundWorker1 = New System.ComponentModel.BackgroundWorker()
        Me.tmr_RDPRefresher = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip1.SuspendLayout()
        Me.DeviceInfoBox.SuspendLayout()
        Me.pnlOtherFunctions.SuspendLayout()
        Me.RightClickMenu.SuspendLayout()
        Me.TabControl1.SuspendLayout()
        Me.HistoryTab.SuspendLayout()
        CType(Me.DataGridHistory, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TrackingTab.SuspendLayout()
        CType(Me.TrackingGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.TrackingBox.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.BackColor = System.Drawing.SystemColors.Control
        Me.MenuStrip1.Dock = System.Windows.Forms.DockStyle.Bottom
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ActionsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 667)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1012, 24)
        Me.MenuStrip1.TabIndex = 36
        Me.MenuStrip1.Text = "MenuStrip1"
        Me.MenuStrip1.Visible = False
        '
        'ActionsToolStripMenuItem
        '
        Me.ActionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditToolStripMenuItem, Me.AddNoteToolStripMenuItem, Me.DeleteDeviceToolStripMenuItem, Me.ToolStripMenuItem1, Me.TrackingToolStripMenuItem})
        Me.ActionsToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ActionsToolStripMenuItem.Name = "ActionsToolStripMenuItem"
        Me.ActionsToolStripMenuItem.Size = New System.Drawing.Size(60, 20)
        Me.ActionsToolStripMenuItem.Text = "Actions"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.Image = Global.AssetManager.My.Resources.Resources.Edit
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.EditToolStripMenuItem.Text = "Modify Device"
        '
        'AddNoteToolStripMenuItem
        '
        Me.AddNoteToolStripMenuItem.Image = Global.AssetManager.My.Resources.Resources.Add
        Me.AddNoteToolStripMenuItem.Name = "AddNoteToolStripMenuItem"
        Me.AddNoteToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.AddNoteToolStripMenuItem.Text = "Add Note"
        '
        'DeleteDeviceToolStripMenuItem
        '
        Me.DeleteDeviceToolStripMenuItem.Image = Global.AssetManager.My.Resources.Resources.delete_icon
        Me.DeleteDeviceToolStripMenuItem.Name = "DeleteDeviceToolStripMenuItem"
        Me.DeleteDeviceToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.DeleteDeviceToolStripMenuItem.Text = "Delete Device"
        '
        'ToolStripMenuItem1
        '
        Me.ToolStripMenuItem1.Name = "ToolStripMenuItem1"
        Me.ToolStripMenuItem1.Size = New System.Drawing.Size(152, 6)
        '
        'TrackingToolStripMenuItem
        '
        Me.TrackingToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CheckInMenu, Me.CheckOutMenu})
        Me.TrackingToolStripMenuItem.Image = Global.AssetManager.My.Resources.Resources.check_out
        Me.TrackingToolStripMenuItem.Name = "TrackingToolStripMenuItem"
        Me.TrackingToolStripMenuItem.Size = New System.Drawing.Size(155, 22)
        Me.TrackingToolStripMenuItem.Text = "Tracking"
        '
        'CheckInMenu
        '
        Me.CheckInMenu.Name = "CheckInMenu"
        Me.CheckInMenu.Size = New System.Drawing.Size(132, 22)
        Me.CheckInMenu.Text = "Check In"
        '
        'CheckOutMenu
        '
        Me.CheckOutMenu.Name = "CheckOutMenu"
        Me.CheckOutMenu.Size = New System.Drawing.Size(132, 22)
        Me.CheckOutMenu.Text = "Check Out"
        '
        'DeviceInfoBox
        '
        Me.DeviceInfoBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.DeviceInfoBox.Controls.Add(Me.pnlOtherFunctions)
        Me.DeviceInfoBox.Controls.Add(Me.cmdSetSibi)
        Me.DeviceInfoBox.Controls.Add(Me.Label12)
        Me.DeviceInfoBox.Controls.Add(Me.txtPONumber)
        Me.DeviceInfoBox.Controls.Add(Me.chkTrackable)
        Me.DeviceInfoBox.Controls.Add(Me.Label1)
        Me.DeviceInfoBox.Controls.Add(Me.txtAssetTag_View_REQ)
        Me.DeviceInfoBox.Controls.Add(Me.Label10)
        Me.DeviceInfoBox.Controls.Add(Me.txtSerial_View_REQ)
        Me.DeviceInfoBox.Controls.Add(Me.txtGUID)
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
        Me.DeviceInfoBox.Size = New System.Drawing.Size(669, 242)
        Me.DeviceInfoBox.TabIndex = 39
        Me.DeviceInfoBox.TabStop = False
        Me.DeviceInfoBox.Text = "Current Info"
        '
        'pnlOtherFunctions
        '
        Me.pnlOtherFunctions.Controls.Add(Me.cmdRDP)
        Me.pnlOtherFunctions.Controls.Add(Me.cmdMunisInfo)
        Me.pnlOtherFunctions.Controls.Add(Me.cmdSibiLink)
        Me.pnlOtherFunctions.Location = New System.Drawing.Point(495, 175)
        Me.pnlOtherFunctions.Name = "pnlOtherFunctions"
        Me.pnlOtherFunctions.Size = New System.Drawing.Size(172, 61)
        Me.pnlOtherFunctions.TabIndex = 51
        '
        'cmdRDP
        '
        Me.cmdRDP.BackgroundImage = CType(resources.GetObject("cmdRDP.BackgroundImage"), System.Drawing.Image)
        Me.cmdRDP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch
        Me.cmdRDP.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdRDP.Location = New System.Drawing.Point(4, 7)
        Me.cmdRDP.Name = "cmdRDP"
        Me.cmdRDP.Size = New System.Drawing.Size(49, 45)
        Me.cmdRDP.TabIndex = 46
        Me.ToolTip1.SetToolTip(Me.cmdRDP, "Launch Remote Desktop")
        Me.cmdRDP.UseVisualStyleBackColor = True
        Me.cmdRDP.Visible = False
        '
        'cmdMunisInfo
        '
        Me.cmdMunisInfo.Location = New System.Drawing.Point(60, 4)
        Me.cmdMunisInfo.Name = "cmdMunisInfo"
        Me.cmdMunisInfo.Size = New System.Drawing.Size(106, 23)
        Me.cmdMunisInfo.TabIndex = 46
        Me.cmdMunisInfo.Text = "MUNIS Info"
        Me.cmdMunisInfo.UseVisualStyleBackColor = True
        '
        'cmdSibiLink
        '
        Me.cmdSibiLink.Location = New System.Drawing.Point(60, 33)
        Me.cmdSibiLink.Name = "cmdSibiLink"
        Me.cmdSibiLink.Size = New System.Drawing.Size(106, 23)
        Me.cmdSibiLink.TabIndex = 49
        Me.cmdSibiLink.Text = "Sibi Info"
        Me.cmdSibiLink.UseVisualStyleBackColor = True
        '
        'cmdSetSibi
        '
        Me.cmdSetSibi.Location = New System.Drawing.Point(110, 208)
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
        Me.Label12.Location = New System.Drawing.Point(492, 116)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(81, 16)
        Me.Label12.TabIndex = 48
        Me.Label12.Text = "PO Number:"
        '
        'txtPONumber
        '
        Me.txtPONumber.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPONumber.Location = New System.Drawing.Point(495, 135)
        Me.txtPONumber.Name = "txtPONumber"
        Me.txtPONumber.Size = New System.Drawing.Size(124, 23)
        Me.txtPONumber.TabIndex = 47
        '
        'chkTrackable
        '
        Me.chkTrackable.AutoSize = True
        Me.chkTrackable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTrackable.Location = New System.Drawing.Point(305, 178)
        Me.chkTrackable.Name = "chkTrackable"
        Me.chkTrackable.Size = New System.Drawing.Size(89, 20)
        Me.chkTrackable.TabIndex = 45
        Me.chkTrackable.Text = "Trackable"
        Me.chkTrackable.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(17, 23)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(73, 16)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Asset Tag:"
        '
        'txtAssetTag_View_REQ
        '
        Me.txtAssetTag_View_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAssetTag_View_REQ.Location = New System.Drawing.Point(20, 42)
        Me.txtAssetTag_View_REQ.Name = "txtAssetTag_View_REQ"
        Me.txtAssetTag_View_REQ.Size = New System.Drawing.Size(134, 23)
        Me.txtAssetTag_View_REQ.TabIndex = 19
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(17, 158)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(90, 16)
        Me.Label10.TabIndex = 41
        Me.Label10.Text = "Device GUID:"
        '
        'txtSerial_View_REQ
        '
        Me.txtSerial_View_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerial_View_REQ.Location = New System.Drawing.Point(20, 87)
        Me.txtSerial_View_REQ.Name = "txtSerial_View_REQ"
        Me.txtSerial_View_REQ.Size = New System.Drawing.Size(133, 23)
        Me.txtSerial_View_REQ.TabIndex = 21
        '
        'txtGUID
        '
        Me.txtGUID.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtGUID.Location = New System.Drawing.Point(20, 177)
        Me.txtGUID.Name = "txtGUID"
        Me.txtGUID.ReadOnly = True
        Me.txtGUID.Size = New System.Drawing.Size(268, 23)
        Me.txtGUID.TabIndex = 40
        Me.txtGUID.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(17, 68)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(46, 16)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "Serial:"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(492, 68)
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
        Me.txtCurUser_View_REQ.Size = New System.Drawing.Size(132, 23)
        Me.txtCurUser_View_REQ.TabIndex = 23
        '
        'cmbStatus_REQ
        '
        Me.cmbStatus_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStatus_REQ.FormattingEnabled = True
        Me.cmbStatus_REQ.Location = New System.Drawing.Point(495, 87)
        Me.cmbStatus_REQ.Name = "cmbStatus_REQ"
        Me.cmbStatus_REQ.Size = New System.Drawing.Size(124, 23)
        Me.cmbStatus_REQ.TabIndex = 38
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
        Me.Label8.Location = New System.Drawing.Point(353, 68)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(79, 16)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "OS Version:"
        '
        'txtDescription_View_REQ
        '
        Me.txtDescription_View_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription_View_REQ.Location = New System.Drawing.Point(172, 42)
        Me.txtDescription_View_REQ.Name = "txtDescription_View_REQ"
        Me.txtDescription_View_REQ.Size = New System.Drawing.Size(304, 23)
        Me.txtDescription_View_REQ.TabIndex = 25
        '
        'cmbOSVersion_REQ
        '
        Me.cmbOSVersion_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbOSVersion_REQ.FormattingEnabled = True
        Me.cmbOSVersion_REQ.Location = New System.Drawing.Point(356, 87)
        Me.cmbOSVersion_REQ.Name = "cmbOSVersion_REQ"
        Me.cmbOSVersion_REQ.Size = New System.Drawing.Size(120, 23)
        Me.cmbOSVersion_REQ.TabIndex = 36
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(171, 23)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(79, 16)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "Description:"
        '
        'cmbLocation_View_REQ
        '
        Me.cmbLocation_View_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLocation_View_REQ.FormattingEnabled = True
        Me.cmbLocation_View_REQ.Location = New System.Drawing.Point(172, 87)
        Me.cmbLocation_View_REQ.Name = "cmbLocation_View_REQ"
        Me.cmbLocation_View_REQ.Size = New System.Drawing.Size(168, 23)
        Me.cmbLocation_View_REQ.TabIndex = 27
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(487, 23)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(110, 16)
        Me.Label13.TabIndex = 34
        Me.Label13.Text = "Equipment Type:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(171, 68)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(62, 16)
        Me.Label5.TabIndex = 28
        Me.Label5.Text = "Location:"
        '
        'cmbEquipType_View_REQ
        '
        Me.cmbEquipType_View_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbEquipType_View_REQ.FormattingEnabled = True
        Me.cmbEquipType_View_REQ.Location = New System.Drawing.Point(490, 42)
        Me.cmbEquipType_View_REQ.Name = "cmbEquipType_View_REQ"
        Me.cmbEquipType_View_REQ.Size = New System.Drawing.Size(156, 23)
        Me.cmbEquipType_View_REQ.TabIndex = 33
        '
        'dtPurchaseDate_View_REQ
        '
        Me.dtPurchaseDate_View_REQ.CustomFormat = "yyyy-MM-dd"
        Me.dtPurchaseDate_View_REQ.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtPurchaseDate_View_REQ.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtPurchaseDate_View_REQ.Location = New System.Drawing.Point(172, 132)
        Me.dtPurchaseDate_View_REQ.Name = "dtPurchaseDate_View_REQ"
        Me.dtPurchaseDate_View_REQ.Size = New System.Drawing.Size(182, 23)
        Me.dtPurchaseDate_View_REQ.TabIndex = 29
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(369, 116)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(95, 16)
        Me.Label7.TabIndex = 32
        Me.Label7.Text = "Replace Year:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(168, 113)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(100, 16)
        Me.Label6.TabIndex = 30
        Me.Label6.Text = "Purchase Date:"
        '
        'txtReplacementYear_View
        '
        Me.txtReplacementYear_View.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReplacementYear_View.Location = New System.Drawing.Point(383, 135)
        Me.txtReplacementYear_View.Name = "txtReplacementYear_View"
        Me.txtReplacementYear_View.Size = New System.Drawing.Size(66, 23)
        Me.txtReplacementYear_View.TabIndex = 31
        '
        'RightClickMenu
        '
        Me.RightClickMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.DeleteEntryToolStripMenuItem})
        Me.RightClickMenu.Name = "RightClickMenu"
        Me.RightClickMenu.Size = New System.Drawing.Size(138, 26)
        '
        'DeleteEntryToolStripMenuItem
        '
        Me.DeleteEntryToolStripMenuItem.Image = Global.AssetManager.My.Resources.Resources.delete_icon
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
        Me.TabControl1.Location = New System.Drawing.Point(12, 303)
        Me.TabControl1.Name = "TabControl1"
        Me.TabControl1.SelectedIndex = 0
        Me.TabControl1.Size = New System.Drawing.Size(991, 363)
        Me.TabControl1.TabIndex = 40
        '
        'HistoryTab
        '
        Me.HistoryTab.Controls.Add(Me.DataGridHistory)
        Me.HistoryTab.Location = New System.Drawing.Point(4, 25)
        Me.HistoryTab.Name = "HistoryTab"
        Me.HistoryTab.Padding = New System.Windows.Forms.Padding(3)
        Me.HistoryTab.Size = New System.Drawing.Size(983, 334)
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
        Me.DataGridHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.DataGridHistory.Location = New System.Drawing.Point(6, 6)
        Me.DataGridHistory.MultiSelect = False
        Me.DataGridHistory.Name = "DataGridHistory"
        Me.DataGridHistory.ReadOnly = True
        Me.DataGridHistory.RowHeadersVisible = False
        Me.DataGridHistory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.DataGridHistory.ShowCellToolTips = False
        Me.DataGridHistory.ShowEditingIcon = False
        Me.DataGridHistory.Size = New System.Drawing.Size(971, 322)
        Me.DataGridHistory.TabIndex = 40
        '
        'TrackingTab
        '
        Me.TrackingTab.Controls.Add(Me.TrackingGrid)
        Me.TrackingTab.Location = New System.Drawing.Point(4, 25)
        Me.TrackingTab.Name = "TrackingTab"
        Me.TrackingTab.Padding = New System.Windows.Forms.Padding(3)
        Me.TrackingTab.Size = New System.Drawing.Size(983, 334)
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
        Me.TrackingGrid.Size = New System.Drawing.Size(971, 322)
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
        Me.TrackingBox.Location = New System.Drawing.Point(687, 46)
        Me.TrackingBox.Name = "TrackingBox"
        Me.TrackingBox.Size = New System.Drawing.Size(316, 242)
        Me.TrackingBox.TabIndex = 41
        Me.TrackingBox.TabStop = False
        Me.TrackingBox.Text = "Tracking Info"
        Me.TrackingBox.Visible = False
        '
        'txtCheckLocation
        '
        Me.txtCheckLocation.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtCheckLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCheckLocation.Location = New System.Drawing.Point(19, 90)
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
        Me.Label16.Location = New System.Drawing.Point(16, 71)
        Me.Label16.Name = "Label16"
        Me.Label16.Size = New System.Drawing.Size(62, 16)
        Me.Label16.TabIndex = 56
        Me.Label16.Text = "Location:"
        '
        'txtDueBack
        '
        Me.txtDueBack.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtDueBack.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDueBack.Location = New System.Drawing.Point(86, 191)
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
        Me.lblDueBack.Location = New System.Drawing.Point(83, 172)
        Me.lblDueBack.Name = "lblDueBack"
        Me.lblDueBack.Size = New System.Drawing.Size(70, 16)
        Me.lblDueBack.TabIndex = 54
        Me.lblDueBack.Text = "Due Back:"
        '
        'txtCheckUser
        '
        Me.txtCheckUser.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtCheckUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCheckUser.Location = New System.Drawing.Point(169, 132)
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
        Me.lblCheckUser.Location = New System.Drawing.Point(166, 113)
        Me.lblCheckUser.Name = "lblCheckUser"
        Me.lblCheckUser.Size = New System.Drawing.Size(101, 16)
        Me.lblCheckUser.TabIndex = 52
        Me.lblCheckUser.Text = "CheckOut User:"
        '
        'txtCheckTime
        '
        Me.txtCheckTime.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtCheckTime.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCheckTime.Location = New System.Drawing.Point(19, 132)
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
        Me.lblCheckTime.Location = New System.Drawing.Point(16, 113)
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
        Me.txtCheckOut.Size = New System.Drawing.Size(127, 26)
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
        Me.ToolTip1.IsBalloon = True
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(249, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ToolStrip1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(25, 25)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripButton1, Me.ToolStripButton2, Me.ToolStripButton3, Me.AttachmentTool, Me.TrackingTool, Me.ToolStripSeparator1, Me.cmdAccept_Tool, Me.ToolStripSeparator3, Me.cmdCancel_Tool, Me.ToolStripSeparator2})
        Me.ToolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ToolStrip1.Size = New System.Drawing.Size(1012, 32)
        Me.ToolStrip1.Stretch = True
        Me.ToolStrip1.TabIndex = 44
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.Image = Global.AssetManager.My.Resources.Resources.Edit
        Me.ToolStripButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Padding = New System.Windows.Forms.Padding(20, 0, 0, 0)
        Me.ToolStripButton1.Size = New System.Drawing.Size(98, 29)
        Me.ToolStripButton1.Text = "Modify"
        '
        'ToolStripButton2
        '
        Me.ToolStripButton2.Image = Global.AssetManager.My.Resources.Resources.note_icon_27942
        Me.ToolStripButton2.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton2.Name = "ToolStripButton2"
        Me.ToolStripButton2.Padding = New System.Windows.Forms.Padding(20, 0, 0, 0)
        Me.ToolStripButton2.Size = New System.Drawing.Size(114, 29)
        Me.ToolStripButton2.Text = "Add Note"
        '
        'ToolStripButton3
        '
        Me.ToolStripButton3.Image = Global.AssetManager.My.Resources.Resources.delete_icon_red
        Me.ToolStripButton3.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton3.Name = "ToolStripButton3"
        Me.ToolStripButton3.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.ToolStripButton3.Size = New System.Drawing.Size(126, 29)
        Me.ToolStripButton3.Text = "Delete Device"
        '
        'AttachmentTool
        '
        Me.AttachmentTool.Image = Global.AssetManager.My.Resources.Resources.clip_512
        Me.AttachmentTool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AttachmentTool.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.AttachmentTool.Name = "AttachmentTool"
        Me.AttachmentTool.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.AttachmentTool.Size = New System.Drawing.Size(118, 29)
        Me.AttachmentTool.Text = "Attachments"
        '
        'TrackingTool
        '
        Me.TrackingTool.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.CheckOutTool, Me.CheckInTool})
        Me.TrackingTool.Image = Global.AssetManager.My.Resources.Resources.check_out
        Me.TrackingTool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.TrackingTool.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.TrackingTool.Name = "TrackingTool"
        Me.TrackingTool.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.TrackingTool.Size = New System.Drawing.Size(104, 29)
        Me.TrackingTool.Text = "Tracking"
        '
        'CheckOutTool
        '
        Me.CheckOutTool.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckOutTool.Image = Global.AssetManager.My.Resources.Resources.checked_checkbox_red
        Me.CheckOutTool.Name = "CheckOutTool"
        Me.CheckOutTool.Size = New System.Drawing.Size(142, 32)
        Me.CheckOutTool.Text = "Check Out"
        '
        'CheckInTool
        '
        Me.CheckInTool.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.CheckInTool.Image = Global.AssetManager.My.Resources.Resources.checked_checkbox_green
        Me.CheckInTool.Name = "CheckInTool"
        Me.CheckInTool.Size = New System.Drawing.Size(142, 32)
        Me.CheckInTool.Text = "Check In"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(6, 32)
        Me.ToolStripSeparator1.Visible = False
        '
        'cmdAccept_Tool
        '
        Me.cmdAccept_Tool.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAccept_Tool.Image = Global.AssetManager.My.Resources.Resources.checked_checkbox
        Me.cmdAccept_Tool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdAccept_Tool.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdAccept_Tool.Name = "cmdAccept_Tool"
        Me.cmdAccept_Tool.Padding = New System.Windows.Forms.Padding(100, 0, 0, 0)
        Me.cmdAccept_Tool.Size = New System.Drawing.Size(178, 29)
        Me.cmdAccept_Tool.Text = "Accept"
        Me.cmdAccept_Tool.Visible = False
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 32)
        Me.ToolStripSeparator3.Visible = False
        '
        'cmdCancel_Tool
        '
        Me.cmdCancel_Tool.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel_Tool.Image = Global.AssetManager.My.Resources.Resources.close_delete_cancel_del_ui_round_512
        Me.cmdCancel_Tool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.cmdCancel_Tool.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdCancel_Tool.Name = "cmdCancel_Tool"
        Me.cmdCancel_Tool.Padding = New System.Windows.Forms.Padding(50, 0, 0, 0)
        Me.cmdCancel_Tool.Size = New System.Drawing.Size(126, 29)
        Me.cmdCancel_Tool.Text = "Cancel"
        Me.cmdCancel_Tool.Visible = False
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 32)
        Me.ToolStripSeparator2.Visible = False
        '
        'StatusStrip1
        '
        Me.StatusStrip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 669)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1012, 22)
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
        'PingWorker
        '
        Me.PingWorker.WorkerReportsProgress = True
        Me.PingWorker.WorkerSupportsCancellation = True
        '
        'tmr_RDPRefresher
        '
        Me.tmr_RDPRefresher.Enabled = True
        Me.tmr_RDPRefresher.Interval = 10000
        '
        'View
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1012, 691)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.TrackingBox)
        Me.Controls.Add(Me.TabControl1)
        Me.Controls.Add(Me.DeviceInfoBox)
        Me.Controls.Add(Me.MenuStrip1)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(1028, 500)
        Me.Name = "View"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "View"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.DeviceInfoBox.ResumeLayout(False)
        Me.DeviceInfoBox.PerformLayout()
        Me.pnlOtherFunctions.ResumeLayout(False)
        Me.RightClickMenu.ResumeLayout(False)
        Me.TabControl1.ResumeLayout(False)
        Me.HistoryTab.ResumeLayout(False)
        CType(Me.DataGridHistory, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TrackingTab.ResumeLayout(False)
        CType(Me.TrackingGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.TrackingBox.ResumeLayout(False)
        Me.TrackingBox.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ActionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents AddNoteToolStripMenuItem As ToolStripMenuItem
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
    Friend WithEvents txtGUID As TextBox
    Friend WithEvents DeleteDeviceToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RightClickMenu As ContextMenuStrip
    Friend WithEvents DeleteEntryToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem1 As ToolStripSeparator
    Friend WithEvents TrackingToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents chkTrackable As CheckBox
    Friend WithEvents CheckInMenu As ToolStripMenuItem
    Friend WithEvents CheckOutMenu As ToolStripMenuItem
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
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents ToolStripButton2 As ToolStripButton
    Friend WithEvents ToolStripButton3 As ToolStripButton
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
    Friend WithEvents PingWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents cmdRDP As Button
    Friend WithEvents BackgroundWorker1 As System.ComponentModel.BackgroundWorker
    Friend WithEvents tmrRDPRefresh As Timer
    Friend WithEvents Label12 As Label
    Friend WithEvents txtPONumber As TextBox
    Friend WithEvents tmr_RDPRefresher As Timer
    Friend WithEvents cmdSibiLink As Button
    Friend WithEvents cmdSetSibi As Button
    Friend WithEvents pnlOtherFunctions As Panel
End Class
