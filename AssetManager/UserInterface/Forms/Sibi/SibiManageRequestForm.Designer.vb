﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SibiManageRequestForm
    Inherits ExtendedForm
    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
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
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(SibiManageRequestForm))
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.PopupMenuItems = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.tsmPopFA = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmLookupDevice = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmGLBudget = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmCopyText = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewDeviceMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmSeparator = New System.Windows.Forms.ToolStripSeparator()
        Me.tsmDeleteItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.txtRTNumber = New System.Windows.Forms.TextBox()
        Me.txtCreateDate = New System.Windows.Forms.TextBox()
        Me.PopupMenuNotes = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.cmdNewNote = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmdDeleteNote = New System.Windows.Forms.ToolStripMenuItem()
        Me.fieldErrorIcon = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.ContentPanel = New System.Windows.Forms.ToolStripContentPanel()
        Me.Panel4 = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Panel3 = New System.Windows.Forms.Panel()
        Me.pnlEditButtons = New System.Windows.Forms.Panel()
        Me.cmdAccept = New System.Windows.Forms.Button()
        Me.cmdDiscard = New System.Windows.Forms.Button()
        Me.pnlCreate = New System.Windows.Forms.Panel()
        Me.cmdAddNew = New System.Windows.Forms.Button()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtRequestNum = New System.Windows.Forms.TextBox()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblReqStatus = New System.Windows.Forms.Label()
        Me.lblPOStatus = New System.Windows.Forms.Label()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtReqNumber = New System.Windows.Forms.TextBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtPO = New System.Windows.Forms.TextBox()
        Me.cmbType = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.dtNeedBy = New System.Windows.Forms.DateTimePicker()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtUser = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.dgvNotes = New System.Windows.Forms.DataGridView()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.RequestItemsGrid = New System.Windows.Forms.DataGridView()
        Me.chkAllowDrag = New System.Windows.Forms.CheckBox()
        Me.ToolStrip = New AssetManager.OneClickToolStrip()
        Me.cmdCreate = New System.Windows.Forms.ToolStripButton()
        Me.ModifyButton = New System.Windows.Forms.ToolStripButton()
        Me.cmdDelete = New System.Windows.Forms.ToolStripButton()
        Me.cmdAddNote = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmdAttachments = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.tsbRefresh = New System.Windows.Forms.ToolStripButton()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.PopupMenuItems.SuspendLayout()
        Me.PopupMenuNotes.SuspendLayout()
        CType(Me.fieldErrorIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel4.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.pnlEditButtons.SuspendLayout()
        Me.pnlCreate.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        Me.Panel2.SuspendLayout()
        CType(Me.dgvNotes, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        CType(Me.RequestItemsGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ToolStrip.SuspendLayout()
        Me.SuspendLayout()
        '
        'PopupMenuItems
        '
        Me.PopupMenuItems.ImageScalingSize = New System.Drawing.Size(20, 20)
        Me.PopupMenuItems.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsmPopFA, Me.tsmLookupDevice, Me.tsmGLBudget, Me.tsmCopyText, Me.NewDeviceMenuItem, Me.tsmSeparator, Me.tsmDeleteItem})
        Me.PopupMenuItems.Name = "PopupMenu"
        Me.PopupMenuItems.Size = New System.Drawing.Size(179, 166)
        '
        'tsmPopFA
        '
        Me.tsmPopFA.Image = Global.AssetManager.My.Resources.Resources.ImportIcon
        Me.tsmPopFA.Name = "tsmPopFA"
        Me.tsmPopFA.Size = New System.Drawing.Size(178, 26)
        Me.tsmPopFA.Text = "Populate From FA"
        '
        'tsmLookupDevice
        '
        Me.tsmLookupDevice.Image = Global.AssetManager.My.Resources.Resources.SearchIcon
        Me.tsmLookupDevice.Name = "tsmLookupDevice"
        Me.tsmLookupDevice.Size = New System.Drawing.Size(178, 26)
        Me.tsmLookupDevice.Text = "Lookup Device"
        Me.tsmLookupDevice.Visible = False
        '
        'tsmGLBudget
        '
        Me.tsmGLBudget.Image = Global.AssetManager.My.Resources.Resources.MoneyCircle2Icon
        Me.tsmGLBudget.Name = "tsmGLBudget"
        Me.tsmGLBudget.Size = New System.Drawing.Size(178, 26)
        Me.tsmGLBudget.Text = "Lookup GL/Budget"
        Me.tsmGLBudget.Visible = False
        '
        'tsmCopyText
        '
        Me.tsmCopyText.Image = Global.AssetManager.My.Resources.Resources.CopyIcon
        Me.tsmCopyText.Name = "tsmCopyText"
        Me.tsmCopyText.Size = New System.Drawing.Size(178, 26)
        Me.tsmCopyText.Text = "Copy Selected"
        '
        'NewDeviceMenuItem
        '
        Me.NewDeviceMenuItem.Image = Global.AssetManager.My.Resources.Resources.AddIcon
        Me.NewDeviceMenuItem.Name = "NewDeviceMenuItem"
        Me.NewDeviceMenuItem.Size = New System.Drawing.Size(178, 26)
        Me.NewDeviceMenuItem.Text = "Import New Asset"
        '
        'tsmSeparator
        '
        Me.tsmSeparator.Name = "tsmSeparator"
        Me.tsmSeparator.Size = New System.Drawing.Size(175, 6)
        '
        'tsmDeleteItem
        '
        Me.tsmDeleteItem.Image = Global.AssetManager.My.Resources.Resources.DeleteRedIcon
        Me.tsmDeleteItem.Name = "tsmDeleteItem"
        Me.tsmDeleteItem.Size = New System.Drawing.Size(178, 26)
        Me.tsmDeleteItem.Text = "Delete Item"
        '
        'ToolTip
        '
        Me.ToolTip.AutomaticDelay = 0
        Me.ToolTip.AutoPopDelay = 5500
        Me.ToolTip.InitialDelay = 0
        Me.ToolTip.IsBalloon = True
        Me.ToolTip.ReshowDelay = 110
        '
        'txtRTNumber
        '
        Me.txtRTNumber.Cursor = System.Windows.Forms.Cursors.Hand
        Me.txtRTNumber.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRTNumber.Location = New System.Drawing.Point(17, 144)
        Me.txtRTNumber.Name = "txtRTNumber"
        Me.txtRTNumber.Size = New System.Drawing.Size(137, 22)
        Me.txtRTNumber.TabIndex = 7
        Me.ToolTip.SetToolTip(Me.txtRTNumber, "Click to open RT Ticket.")
        '
        'txtCreateDate
        '
        Me.txtCreateDate.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtCreateDate.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.txtCreateDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtCreateDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCreateDate.ForeColor = System.Drawing.Color.Black
        Me.txtCreateDate.Location = New System.Drawing.Point(263, 71)
        Me.txtCreateDate.Name = "txtCreateDate"
        Me.txtCreateDate.ReadOnly = True
        Me.txtCreateDate.Size = New System.Drawing.Size(137, 21)
        Me.txtCreateDate.TabIndex = 23
        Me.txtCreateDate.Text = "[CREATE DATE]"
        Me.txtCreateDate.TextAlign = System.Windows.Forms.HorizontalAlignment.Center
        Me.ToolTip.SetToolTip(Me.txtCreateDate, "Create Date")
        Me.txtCreateDate.WordWrap = False
        '
        'PopupMenuNotes
        '
        Me.PopupMenuNotes.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmdNewNote, Me.ToolStripSeparator1, Me.cmdDeleteNote})
        Me.PopupMenuNotes.Name = "PopupMenu"
        Me.PopupMenuNotes.Size = New System.Drawing.Size(137, 54)
        '
        'cmdNewNote
        '
        Me.cmdNewNote.Image = Global.AssetManager.My.Resources.Resources.AddNoteIcon
        Me.cmdNewNote.Name = "cmdNewNote"
        Me.cmdNewNote.Size = New System.Drawing.Size(136, 22)
        Me.cmdNewNote.Text = "Add Note"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(133, 6)
        '
        'cmdDeleteNote
        '
        Me.cmdDeleteNote.Image = Global.AssetManager.My.Resources.Resources.DeleteRedIcon
        Me.cmdDeleteNote.Name = "cmdDeleteNote"
        Me.cmdDeleteNote.Size = New System.Drawing.Size(136, 22)
        Me.cmdDeleteNote.Text = "Delete Note"
        '
        'fieldErrorIcon
        '
        Me.fieldErrorIcon.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.fieldErrorIcon.ContainerControl = Me
        Me.fieldErrorIcon.Icon = CType(resources.GetObject("fieldErrorIcon.Icon"), System.Drawing.Icon)
        '
        'ContentPanel
        '
        Me.ContentPanel.AutoScroll = True
        Me.ContentPanel.Size = New System.Drawing.Size(1014, 557)
        '
        'Panel4
        '
        Me.Panel4.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel4.Controls.Add(Me.GroupBox1)
        Me.Panel4.Controls.Add(Me.GroupBox3)
        Me.Panel4.Location = New System.Drawing.Point(8, 40)
        Me.Panel4.Name = "Panel4"
        Me.Panel4.Size = New System.Drawing.Size(1061, 272)
        Me.Panel4.TabIndex = 5
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.txtCreateDate)
        Me.GroupBox1.Controls.Add(Me.Panel3)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.txtRequestNum)
        Me.GroupBox1.Controls.Add(Me.cmbStatus)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.GroupBox2)
        Me.GroupBox1.Controls.Add(Me.cmbType)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.dtNeedBy)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtUser)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtDescription)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(5, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(600, 264)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Request Info"
        '
        'Panel3
        '
        Me.Panel3.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.Panel3.AutoSize = True
        Me.Panel3.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Panel3.Controls.Add(Me.pnlEditButtons)
        Me.Panel3.Controls.Add(Me.pnlCreate)
        Me.Panel3.Location = New System.Drawing.Point(229, 102)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(148, 148)
        Me.Panel3.TabIndex = 22
        '
        'pnlEditButtons
        '
        Me.pnlEditButtons.Controls.Add(Me.cmdAccept)
        Me.pnlEditButtons.Controls.Add(Me.cmdDiscard)
        Me.pnlEditButtons.Location = New System.Drawing.Point(3, 4)
        Me.pnlEditButtons.Name = "pnlEditButtons"
        Me.pnlEditButtons.Size = New System.Drawing.Size(141, 78)
        Me.pnlEditButtons.TabIndex = 20
        Me.pnlEditButtons.Visible = False
        '
        'cmdAccept
        '
        Me.cmdAccept.BackColor = System.Drawing.Color.PaleGreen
        Me.cmdAccept.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAccept.Location = New System.Drawing.Point(9, 3)
        Me.cmdAccept.Name = "cmdAccept"
        Me.cmdAccept.Size = New System.Drawing.Size(119, 41)
        Me.cmdAccept.TabIndex = 18
        Me.cmdAccept.Text = "Accept Changes"
        Me.cmdAccept.UseVisualStyleBackColor = False
        '
        'cmdDiscard
        '
        Me.cmdDiscard.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(128, Byte), Integer), CType(CType(128, Byte), Integer))
        Me.cmdDiscard.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDiscard.Location = New System.Drawing.Point(9, 50)
        Me.cmdDiscard.Name = "cmdDiscard"
        Me.cmdDiscard.Size = New System.Drawing.Size(119, 24)
        Me.cmdDiscard.TabIndex = 19
        Me.cmdDiscard.Text = "Discard Changes"
        Me.cmdDiscard.UseVisualStyleBackColor = False
        '
        'pnlCreate
        '
        Me.pnlCreate.Controls.Add(Me.cmdAddNew)
        Me.pnlCreate.Location = New System.Drawing.Point(3, 90)
        Me.pnlCreate.Name = "pnlCreate"
        Me.pnlCreate.Size = New System.Drawing.Size(142, 55)
        Me.pnlCreate.TabIndex = 21
        Me.pnlCreate.Visible = False
        '
        'cmdAddNew
        '
        Me.cmdAddNew.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAddNew.Location = New System.Drawing.Point(9, 8)
        Me.cmdAddNew.Name = "cmdAddNew"
        Me.cmdAddNew.Size = New System.Drawing.Size(119, 41)
        Me.cmdAddNew.TabIndex = 8
        Me.cmdAddNew.Text = "Create Request"
        Me.cmdAddNew.UseVisualStyleBackColor = True
        '
        'Label8
        '
        Me.Label8.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(484, 29)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(66, 15)
        Me.Label8.TabIndex = 16
        Me.Label8.Text = "Request #:"
        '
        'txtRequestNum
        '
        Me.txtRequestNum.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.txtRequestNum.Location = New System.Drawing.Point(487, 45)
        Me.txtRequestNum.Name = "txtRequestNum"
        Me.txtRequestNum.ReadOnly = True
        Me.txtRequestNum.Size = New System.Drawing.Size(86, 21)
        Me.txtRequestNum.TabIndex = 15
        Me.txtRequestNum.TabStop = False
        '
        'cmbStatus
        '
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Location = New System.Drawing.Point(18, 227)
        Me.cmbStatus.Margin = New System.Windows.Forms.Padding(2)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(137, 23)
        Me.cmbStatus.TabIndex = 3
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(15, 208)
        Me.Label7.Margin = New System.Windows.Forms.Padding(2, 3, 2, 2)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(44, 15)
        Me.Label7.TabIndex = 13
        Me.Label7.Text = "Status:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.lblReqStatus)
        Me.GroupBox2.Controls.Add(Me.lblPOStatus)
        Me.GroupBox2.Controls.Add(Me.Label9)
        Me.GroupBox2.Controls.Add(Me.txtRTNumber)
        Me.GroupBox2.Controls.Add(Me.Label6)
        Me.GroupBox2.Controls.Add(Me.txtReqNumber)
        Me.GroupBox2.Controls.Add(Me.Label5)
        Me.GroupBox2.Controls.Add(Me.txtPO)
        Me.GroupBox2.Location = New System.Drawing.Point(419, 82)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(175, 175)
        Me.GroupBox2.TabIndex = 11
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Add'l Info (Click to View)"
        '
        'lblReqStatus
        '
        Me.lblReqStatus.AutoSize = True
        Me.lblReqStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblReqStatus.ForeColor = System.Drawing.Color.DimGray
        Me.lblReqStatus.Location = New System.Drawing.Point(16, 112)
        Me.lblReqStatus.Name = "lblReqStatus"
        Me.lblReqStatus.Size = New System.Drawing.Size(61, 12)
        Me.lblReqStatus.TabIndex = 11
        Me.lblReqStatus.Text = "Status: NA"
        '
        'lblPOStatus
        '
        Me.lblPOStatus.AutoSize = True
        Me.lblPOStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblPOStatus.ForeColor = System.Drawing.Color.DimGray
        Me.lblPOStatus.Location = New System.Drawing.Point(16, 59)
        Me.lblPOStatus.Name = "lblPOStatus"
        Me.lblPOStatus.Size = New System.Drawing.Size(61, 12)
        Me.lblPOStatus.TabIndex = 10
        Me.lblPOStatus.Text = "Status: NA"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(14, 128)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(36, 15)
        Me.Label9.TabIndex = 9
        Me.Label9.Text = "RT #:"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(14, 74)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(82, 15)
        Me.Label6.TabIndex = 7
        Me.Label6.Text = "Requisition #:"
        '
        'txtReqNumber
        '
        Me.txtReqNumber.Cursor = System.Windows.Forms.Cursors.Hand
        Me.txtReqNumber.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReqNumber.Location = New System.Drawing.Point(17, 90)
        Me.txtReqNumber.Name = "txtReqNumber"
        Me.txtReqNumber.Size = New System.Drawing.Size(137, 22)
        Me.txtReqNumber.TabIndex = 6
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(14, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(37, 15)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "PO #:"
        '
        'txtPO
        '
        Me.txtPO.Cursor = System.Windows.Forms.Cursors.Hand
        Me.txtPO.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPO.ForeColor = System.Drawing.SystemColors.WindowText
        Me.txtPO.Location = New System.Drawing.Point(17, 37)
        Me.txtPO.Name = "txtPO"
        Me.txtPO.Size = New System.Drawing.Size(137, 22)
        Me.txtPO.TabIndex = 5
        '
        'cmbType
        '
        Me.cmbType.FormattingEnabled = True
        Me.cmbType.Location = New System.Drawing.Point(18, 135)
        Me.cmbType.Margin = New System.Windows.Forms.Padding(2)
        Me.cmbType.Name = "cmbType"
        Me.cmbType.Size = New System.Drawing.Size(137, 23)
        Me.cmbType.TabIndex = 2
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(15, 116)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 3, 2, 2)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(36, 15)
        Me.Label4.TabIndex = 8
        Me.Label4.Text = "Type:"
        '
        'dtNeedBy
        '
        Me.dtNeedBy.CustomFormat = "MM/dd/yyyy"
        Me.dtNeedBy.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtNeedBy.Location = New System.Drawing.Point(18, 182)
        Me.dtNeedBy.Margin = New System.Windows.Forms.Padding(2)
        Me.dtNeedBy.Name = "dtNeedBy"
        Me.dtNeedBy.Size = New System.Drawing.Size(137, 21)
        Me.dtNeedBy.TabIndex = 3
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(15, 163)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 3, 2, 2)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(56, 15)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Need By:"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(15, 71)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 3, 2, 2)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(85, 15)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Request User:"
        '
        'txtUser
        '
        Me.txtUser.Location = New System.Drawing.Point(18, 90)
        Me.txtUser.Margin = New System.Windows.Forms.Padding(2)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.Size = New System.Drawing.Size(137, 21)
        Me.txtUser.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 27)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(121, 15)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Request Description:"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(18, 45)
        Me.txtDescription.Margin = New System.Windows.Forms.Padding(2)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(382, 21)
        Me.txtDescription.TabIndex = 0
        Me.txtDescription.Tag = ""
        '
        'GroupBox3
        '
        Me.GroupBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox3.Controls.Add(Me.Panel2)
        Me.GroupBox3.Location = New System.Drawing.Point(611, 4)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(447, 264)
        Me.GroupBox3.TabIndex = 4
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Notes"
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.Controls.Add(Me.dgvNotes)
        Me.Panel2.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel2.Location = New System.Drawing.Point(6, 13)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(436, 244)
        Me.Panel2.TabIndex = 0
        '
        'dgvNotes
        '
        Me.dgvNotes.AllowUserToAddRows = False
        Me.dgvNotes.AllowUserToDeleteRows = False
        Me.dgvNotes.AllowUserToResizeRows = False
        Me.dgvNotes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.dgvNotes.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.dgvNotes.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.dgvNotes.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.dgvNotes.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.dgvNotes.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.dgvNotes.ContextMenuStrip = Me.PopupMenuNotes
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(39, Byte), Integer))
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.dgvNotes.DefaultCellStyle = DataGridViewCellStyle3
        Me.dgvNotes.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.dgvNotes.Location = New System.Drawing.Point(3, 3)
        Me.dgvNotes.Name = "dgvNotes"
        Me.dgvNotes.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.dgvNotes.RowHeadersDefaultCellStyle = DataGridViewCellStyle4
        Me.dgvNotes.RowHeadersVisible = False
        Me.dgvNotes.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.dgvNotes.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.dgvNotes.ShowCellErrors = False
        Me.dgvNotes.ShowCellToolTips = False
        Me.dgvNotes.Size = New System.Drawing.Size(430, 238)
        Me.dgvNotes.TabIndex = 19
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.GroupBox4)
        Me.Panel1.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(8, 314)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(1061, 384)
        Me.Panel1.TabIndex = 1
        '
        'GroupBox4
        '
        Me.GroupBox4.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox4.Controls.Add(Me.RequestItemsGrid)
        Me.GroupBox4.Controls.Add(Me.chkAllowDrag)
        Me.GroupBox4.Location = New System.Drawing.Point(0, 3)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(1058, 378)
        Me.GroupBox4.TabIndex = 21
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Items"
        '
        'RequestItemsGrid
        '
        Me.RequestItemsGrid.AllowDrop = True
        Me.RequestItemsGrid.AllowUserToResizeRows = False
        Me.RequestItemsGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RequestItemsGrid.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.RequestItemsGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.RequestItemsGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.RequestItemsGrid.ContextMenuStrip = Me.PopupMenuItems
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(185, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(255, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.RequestItemsGrid.DefaultCellStyle = DataGridViewCellStyle1
        Me.RequestItemsGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditOnEnter
        Me.RequestItemsGrid.Location = New System.Drawing.Point(6, 32)
        Me.RequestItemsGrid.Name = "RequestItemsGrid"
        Me.RequestItemsGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.RequestItemsGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.RequestItemsGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.RequestItemsGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.RequestItemsGrid.ShowCellToolTips = False
        Me.RequestItemsGrid.Size = New System.Drawing.Size(1046, 340)
        Me.RequestItemsGrid.TabIndex = 18
        '
        'chkAllowDrag
        '
        Me.chkAllowDrag.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.chkAllowDrag.AutoSize = True
        Me.chkAllowDrag.Location = New System.Drawing.Point(956, 13)
        Me.chkAllowDrag.Name = "chkAllowDrag"
        Me.chkAllowDrag.Size = New System.Drawing.Size(96, 19)
        Me.chkAllowDrag.TabIndex = 20
        Me.chkAllowDrag.TabStop = False
        Me.chkAllowDrag.Text = "Allow Drag"
        Me.chkAllowDrag.UseVisualStyleBackColor = True
        '
        'ToolStrip
        '
        Me.ToolStrip.BackColor = System.Drawing.Color.FromArgb(CType(CType(185, Byte), Integer), CType(CType(205, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.ToolStrip.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStrip.ImageScalingSize = New System.Drawing.Size(25, 25)
        Me.ToolStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmdCreate, Me.ModifyButton, Me.cmdDelete, Me.cmdAddNote, Me.ToolStripSeparator2, Me.cmdAttachments, Me.ToolStripSeparator3, Me.tsbRefresh})
        Me.ToolStrip.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip.Name = "ToolStrip"
        Me.ToolStrip.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.ToolStrip.Size = New System.Drawing.Size(1079, 37)
        Me.ToolStrip.TabIndex = 6
        Me.ToolStrip.Text = "ToolStrip1"
        '
        'cmdCreate
        '
        Me.cmdCreate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdCreate.Image = Global.AssetManager.My.Resources.Resources.AddIcon
        Me.cmdCreate.Name = "cmdCreate"
        Me.cmdCreate.Padding = New System.Windows.Forms.Padding(5, 5, 5, 0)
        Me.cmdCreate.Size = New System.Drawing.Size(39, 34)
        Me.cmdCreate.Text = "New Request"
        '
        'ModifyButton
        '
        Me.ModifyButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.ModifyButton.Image = Global.AssetManager.My.Resources.Resources.EditIcon
        Me.ModifyButton.Name = "ModifyButton"
        Me.ModifyButton.Padding = New System.Windows.Forms.Padding(5, 5, 5, 0)
        Me.ModifyButton.Size = New System.Drawing.Size(39, 34)
        Me.ModifyButton.Text = "Modify"
        Me.ModifyButton.ToolTipText = "Modify"
        '
        'cmdDelete
        '
        Me.cmdDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdDelete.Image = Global.AssetManager.My.Resources.Resources.DeleteRedIcon
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Padding = New System.Windows.Forms.Padding(5, 5, 5, 0)
        Me.cmdDelete.Size = New System.Drawing.Size(39, 34)
        Me.cmdDelete.Text = "Delete"
        '
        'cmdAddNote
        '
        Me.cmdAddNote.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.cmdAddNote.Image = Global.AssetManager.My.Resources.Resources.AddNoteIcon
        Me.cmdAddNote.Name = "cmdAddNote"
        Me.cmdAddNote.Padding = New System.Windows.Forms.Padding(5, 5, 5, 0)
        Me.cmdAddNote.Size = New System.Drawing.Size(39, 34)
        Me.cmdAddNote.Text = "Add Note"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 37)
        '
        'cmdAttachments
        '
        Me.cmdAttachments.Image = Global.AssetManager.My.Resources.Resources.PaperClipIcon
        Me.cmdAttachments.Name = "cmdAttachments"
        Me.cmdAttachments.Padding = New System.Windows.Forms.Padding(5, 5, 5, 0)
        Me.cmdAttachments.Size = New System.Drawing.Size(136, 34)
        Me.cmdAttachments.Text = "Attachments"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(6, 37)
        '
        'tsbRefresh
        '
        Me.tsbRefresh.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image
        Me.tsbRefresh.Image = Global.AssetManager.My.Resources.Resources.RefreshIcon
        Me.tsbRefresh.Name = "tsbRefresh"
        Me.tsbRefresh.Size = New System.Drawing.Size(29, 34)
        Me.tsbRefresh.ToolTipText = "Refresh"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.AutoSize = False
        Me.StatusStrip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.StatusStrip1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 701)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1079, 22)
        Me.StatusStrip1.TabIndex = 7
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'SibiManageRequestForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1079, 723)
        Me.Controls.Add(Me.ToolStrip)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.Panel4)
        Me.Controls.Add(Me.StatusStrip1)
        Me.MinimumSize = New System.Drawing.Size(771, 443)
        Me.Name = "SibiManageRequestForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Manage Request"
        Me.PopupMenuItems.ResumeLayout(False)
        Me.PopupMenuNotes.ResumeLayout(False)
        CType(Me.fieldErrorIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel4.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.Panel3.ResumeLayout(False)
        Me.pnlEditButtons.ResumeLayout(False)
        Me.pnlCreate.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.GroupBox3.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        CType(Me.dgvNotes, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        CType(Me.RequestItemsGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ToolStrip.ResumeLayout(False)
        Me.ToolStrip.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents PopupMenuItems As ContextMenuStrip
    Friend WithEvents tsmDeleteItem As ToolStripMenuItem
    Friend WithEvents ToolTip As ToolTip
    Friend WithEvents PopupMenuNotes As ContextMenuStrip
    Friend WithEvents cmdDeleteNote As ToolStripMenuItem
    Friend WithEvents tsmLookupDevice As ToolStripMenuItem
    Friend WithEvents fieldErrorIcon As ErrorProvider
    Friend WithEvents cmdNewNote As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStrip As OneClickToolStrip
    Friend WithEvents cmdCreate As ToolStripButton
    Friend WithEvents ModifyButton As ToolStripButton
    Friend WithEvents cmdDelete As ToolStripButton
    Friend WithEvents cmdAddNote As ToolStripButton
    Friend WithEvents cmdAttachments As ToolStripButton
    Friend WithEvents Panel1 As Panel
    Friend WithEvents RequestItemsGrid As DataGridView
    Friend WithEvents Panel4 As Panel
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtCreateDate As TextBox
    Friend WithEvents Panel3 As Panel
    Friend WithEvents pnlEditButtons As Panel
    Friend WithEvents cmdAccept As Button
    Friend WithEvents cmdDiscard As Button
    Friend WithEvents pnlCreate As Panel
    Friend WithEvents cmdAddNew As Button
    Friend WithEvents Label8 As Label
    Friend WithEvents txtRequestNum As TextBox
    Friend WithEvents cmbStatus As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtRTNumber As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtReqNumber As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtPO As TextBox
    Friend WithEvents cmbType As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents dtNeedBy As DateTimePicker
    Friend WithEvents Label3 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtUser As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents dgvNotes As DataGridView
    Friend WithEvents ContentPanel As ToolStripContentPanel
    Friend WithEvents chkAllowDrag As CheckBox
    Friend WithEvents tsmSeparator As ToolStripSeparator
    Friend WithEvents lblPOStatus As Label
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents lblReqStatus As Label
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents tsmCopyText As ToolStripMenuItem
    Friend WithEvents tsbRefresh As ToolStripButton
    Friend WithEvents tsmPopFA As ToolStripMenuItem
    Friend WithEvents tsmGLBudget As ToolStripMenuItem
    Friend WithEvents NewDeviceMenuItem As ToolStripMenuItem
    Friend WithEvents StatusStrip1 As StatusStrip
End Class
