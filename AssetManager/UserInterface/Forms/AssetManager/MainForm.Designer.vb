<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
    Inherits ThemedForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ResultGrid = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmAddGKUpdate = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmSendToGridForm = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.CopyTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.lblRecords = New System.Windows.Forms.Label()
        Me.InstantGroup = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtAssetTag = New System.Windows.Forms.TextBox()
        Me.txtSerial = New System.Windows.Forms.TextBox()
        Me.SearchGroup = New System.Windows.Forms.GroupBox()
        Me.SearchPanel = New System.Windows.Forms.Panel()
        Me.chkHistorical = New System.Windows.Forms.CheckBox()
        Me.cmdSupDevSearch = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtReplaceYear = New System.Windows.Forms.TextBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.cmbOSType = New System.Windows.Forms.ComboBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.chkTrackables = New System.Windows.Forms.CheckBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.cmbEquipType = New System.Windows.Forms.ComboBox()
        Me.txtSerialSearch = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.cmbLocation = New System.Windows.Forms.ComboBox()
        Me.txtCurUser = New System.Windows.Forms.TextBox()
        Me.txtAssetTagSearch = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdClear = New System.Windows.Forms.Button()
        Me.cmdShowAll = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StripSpinner = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ConnStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel4 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.DateTimeLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.ToolStrip1 = New AssetManager.OneClickToolStrip()
        Me.AddDeviceTool = New System.Windows.Forms.ToolStripButton()
        Me.AdminDropDown = New System.Windows.Forms.ToolStripDropDownButton()
        Me.txtGUID = New System.Windows.Forms.ToolStripTextBox()
        Me.tsmUserManager = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReEnterLACredentialsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TextEnCrypterToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ScanAttachmentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.tsmGKUpdater = New System.Windows.Forms.ToolStripMenuItem()
        Me.AdvancedSearchMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PSScriptMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.InstallChromeMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ViewLogToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmdSibi = New System.Windows.Forms.ToolStripButton()
        Me.GroupBox1.SuspendLayout()
        CType(Me.ResultGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.InstantGroup.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SearchGroup.SuspendLayout()
        Me.SearchPanel.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox1.Controls.Add(Me.ResultGrid)
        Me.GroupBox1.Controls.Add(Me.lblRecords)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 267)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1356, 513)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        '
        'ResultGrid
        '
        Me.ResultGrid.AllowUserToAddRows = False
        Me.ResultGrid.AllowUserToDeleteRows = False
        Me.ResultGrid.AllowUserToResizeRows = False
        Me.ResultGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ResultGrid.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ResultGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ResultGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle1
        Me.ResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ResultGrid.ContextMenuStrip = Me.ContextMenuStrip1
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.Padding = New System.Windows.Forms.Padding(5)
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(39, Byte), Integer))
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ResultGrid.DefaultCellStyle = DataGridViewCellStyle2
        Me.ResultGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.ResultGrid.Location = New System.Drawing.Point(9, 19)
        Me.ResultGrid.Name = "ResultGrid"
        Me.ResultGrid.ReadOnly = True
        Me.ResultGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ResultGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.ResultGrid.RowHeadersVisible = False
        Me.ResultGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.ResultGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.ResultGrid.ShowCellErrors = False
        Me.ResultGrid.ShowCellToolTips = False
        Me.ResultGrid.ShowEditingIcon = False
        Me.ResultGrid.Size = New System.Drawing.Size(1335, 475)
        Me.ResultGrid.TabIndex = 17
        Me.ResultGrid.VirtualMode = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem, Me.tsmAddGKUpdate, Me.tsmSendToGridForm, Me.ToolStripSeparator3, Me.CopyTool})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(180, 98)
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ViewToolStripMenuItem.Image = Global.AssetManager.My.Resources.Resources.DetailsIcon
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(179, 22)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'tsmAddGKUpdate
        '
        Me.tsmAddGKUpdate.Image = Global.AssetManager.My.Resources.Resources.GK_SmallIcon
        Me.tsmAddGKUpdate.Name = "tsmAddGKUpdate"
        Me.tsmAddGKUpdate.Size = New System.Drawing.Size(179, 22)
        Me.tsmAddGKUpdate.Text = "Enqueue GK Update"
        '
        'tsmSendToGridForm
        '
        Me.tsmSendToGridForm.Image = Global.AssetManager.My.Resources.Resources.TransferArrowsIcon
        Me.tsmSendToGridForm.Name = "tsmSendToGridForm"
        Me.tsmSendToGridForm.Size = New System.Drawing.Size(179, 22)
        Me.tsmSendToGridForm.Text = "Send to Grid Form"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(176, 6)
        '
        'CopyTool
        '
        Me.CopyTool.Image = Global.AssetManager.My.Resources.Resources.CopyIcon
        Me.CopyTool.Name = "CopyTool"
        Me.CopyTool.Size = New System.Drawing.Size(179, 22)
        Me.CopyTool.Text = "Copy Selected"
        '
        'lblRecords
        '
        Me.lblRecords.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblRecords.ForeColor = System.Drawing.Color.FromArgb(CType(CType(53, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(53, Byte), Integer))
        Me.lblRecords.Location = New System.Drawing.Point(15, 497)
        Me.lblRecords.Name = "lblRecords"
        Me.lblRecords.Size = New System.Drawing.Size(1329, 13)
        Me.lblRecords.TabIndex = 18
        Me.lblRecords.Text = "Records: 0"
        Me.lblRecords.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'InstantGroup
        '
        Me.InstantGroup.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.InstantGroup.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.InstantGroup.Controls.Add(Me.Panel1)
        Me.InstantGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InstantGroup.Location = New System.Drawing.Point(9, 19)
        Me.InstantGroup.Name = "InstantGroup"
        Me.InstantGroup.Size = New System.Drawing.Size(177, 201)
        Me.InstantGroup.TabIndex = 34
        Me.InstantGroup.TabStop = False
        Me.InstantGroup.Text = "Instant Lookup"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.txtAssetTag)
        Me.Panel1.Controls.Add(Me.txtSerial)
        Me.Panel1.Location = New System.Drawing.Point(6, 20)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(165, 173)
        Me.Panel1.TabIndex = 39
        '
        'Label9
        '
        Me.Label9.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(12, 80)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(46, 16)
        Me.Label9.TabIndex = 38
        Me.Label9.Text = "Serial:"
        '
        'Label8
        '
        Me.Label8.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(12, 31)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(73, 16)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "Asset Tag:"
        '
        'txtAssetTag
        '
        Me.txtAssetTag.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtAssetTag.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAssetTag.Location = New System.Drawing.Point(15, 50)
        Me.txtAssetTag.MaxLength = 45
        Me.txtAssetTag.Name = "txtAssetTag"
        Me.txtAssetTag.Size = New System.Drawing.Size(135, 23)
        Me.txtAssetTag.TabIndex = 36
        '
        'txtSerial
        '
        Me.txtSerial.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtSerial.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerial.Location = New System.Drawing.Point(15, 99)
        Me.txtSerial.MaxLength = 45
        Me.txtSerial.Name = "txtSerial"
        Me.txtSerial.Size = New System.Drawing.Size(135, 23)
        Me.txtSerial.TabIndex = 35
        '
        'SearchGroup
        '
        Me.SearchGroup.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SearchGroup.Controls.Add(Me.SearchPanel)
        Me.SearchGroup.Controls.Add(Me.cmdSearch)
        Me.SearchGroup.Controls.Add(Me.cmdClear)
        Me.SearchGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SearchGroup.Location = New System.Drawing.Point(192, 19)
        Me.SearchGroup.Name = "SearchGroup"
        Me.SearchGroup.Size = New System.Drawing.Size(862, 201)
        Me.SearchGroup.TabIndex = 31
        Me.SearchGroup.TabStop = False
        Me.SearchGroup.Text = "Custom Search"
        '
        'SearchPanel
        '
        Me.SearchPanel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SearchPanel.AutoScrollMargin = New System.Drawing.Size(10, 20)
        Me.SearchPanel.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.SearchPanel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.SearchPanel.Controls.Add(Me.chkHistorical)
        Me.SearchPanel.Controls.Add(Me.cmdSupDevSearch)
        Me.SearchPanel.Controls.Add(Me.Label6)
        Me.SearchPanel.Controls.Add(Me.Label4)
        Me.SearchPanel.Controls.Add(Me.txtReplaceYear)
        Me.SearchPanel.Controls.Add(Me.txtDescription)
        Me.SearchPanel.Controls.Add(Me.cmbOSType)
        Me.SearchPanel.Controls.Add(Me.Label2)
        Me.SearchPanel.Controls.Add(Me.Label5)
        Me.SearchPanel.Controls.Add(Me.Label1)
        Me.SearchPanel.Controls.Add(Me.Label10)
        Me.SearchPanel.Controls.Add(Me.cmbStatus)
        Me.SearchPanel.Controls.Add(Me.chkTrackables)
        Me.SearchPanel.Controls.Add(Me.Label12)
        Me.SearchPanel.Controls.Add(Me.cmbEquipType)
        Me.SearchPanel.Controls.Add(Me.txtSerialSearch)
        Me.SearchPanel.Controls.Add(Me.Label3)
        Me.SearchPanel.Controls.Add(Me.cmbLocation)
        Me.SearchPanel.Controls.Add(Me.txtCurUser)
        Me.SearchPanel.Controls.Add(Me.txtAssetTagSearch)
        Me.SearchPanel.Controls.Add(Me.Label11)
        Me.SearchPanel.Location = New System.Drawing.Point(11, 20)
        Me.SearchPanel.Name = "SearchPanel"
        Me.SearchPanel.Size = New System.Drawing.Size(732, 173)
        Me.SearchPanel.TabIndex = 52
        '
        'chkHistorical
        '
        Me.chkHistorical.AutoSize = True
        Me.chkHistorical.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkHistorical.Location = New System.Drawing.Point(444, 126)
        Me.chkHistorical.Name = "chkHistorical"
        Me.chkHistorical.Size = New System.Drawing.Size(89, 22)
        Me.chkHistorical.TabIndex = 56
        Me.chkHistorical.Text = "Historical"
        Me.chkHistorical.UseVisualStyleBackColor = True
        '
        'cmdSupDevSearch
        '
        Me.cmdSupDevSearch.Location = New System.Drawing.Point(577, 114)
        Me.cmdSupDevSearch.Name = "cmdSupDevSearch"
        Me.cmdSupDevSearch.Size = New System.Drawing.Size(125, 44)
        Me.cmdSupDevSearch.TabIndex = 55
        Me.cmdSupDevSearch.Text = "Devices By Supervisor"
        Me.cmdSupDevSearch.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(183, 108)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(95, 16)
        Me.Label6.TabIndex = 54
        Me.Label6.Text = "Replace Year:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(22, 12)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 16)
        Me.Label4.TabIndex = 48
        Me.Label4.Text = "Asset Tag:"
        '
        'txtReplaceYear
        '
        Me.txtReplaceYear.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReplaceYear.Location = New System.Drawing.Point(186, 127)
        Me.txtReplaceYear.MaxLength = 200
        Me.txtReplaceYear.Name = "txtReplaceYear"
        Me.txtReplaceYear.Size = New System.Drawing.Size(100, 23)
        Me.txtReplaceYear.TabIndex = 53
        Me.txtReplaceYear.TabStop = False
        '
        'txtDescription
        '
        Me.txtDescription.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.Location = New System.Drawing.Point(186, 78)
        Me.txtDescription.MaxLength = 200
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(330, 23)
        Me.txtDescription.TabIndex = 43
        Me.txtDescription.TabStop = False
        '
        'cmbOSType
        '
        Me.cmbOSType.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbOSType.FormattingEnabled = True
        Me.cmbOSType.Location = New System.Drawing.Point(25, 127)
        Me.cmbOSType.Name = "cmbOSType"
        Me.cmbOSType.Size = New System.Drawing.Size(135, 23)
        Me.cmbOSType.TabIndex = 51
        Me.cmbOSType.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(183, 59)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 16)
        Me.Label2.TabIndex = 44
        Me.Label2.Text = "Description:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(22, 108)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 16)
        Me.Label5.TabIndex = 52
        Me.Label5.Text = "OS Type:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(534, 59)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 16)
        Me.Label1.TabIndex = 42
        Me.Label1.Text = "Status:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(354, 12)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(110, 16)
        Me.Label10.TabIndex = 36
        Me.Label10.Text = "Equipment Type:"
        '
        'cmbStatus
        '
        Me.cmbStatus.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Location = New System.Drawing.Point(537, 78)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(165, 23)
        Me.cmbStatus.TabIndex = 41
        Me.cmbStatus.TabStop = False
        '
        'chkTrackables
        '
        Me.chkTrackables.AutoSize = True
        Me.chkTrackables.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTrackables.Location = New System.Drawing.Point(323, 126)
        Me.chkTrackables.Name = "chkTrackables"
        Me.chkTrackables.Size = New System.Drawing.Size(100, 22)
        Me.chkTrackables.TabIndex = 50
        Me.chkTrackables.Text = "Trackables"
        Me.chkTrackables.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(534, 12)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(62, 16)
        Me.Label12.TabIndex = 40
        Me.Label12.Text = "Location:"
        '
        'cmbEquipType
        '
        Me.cmbEquipType.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbEquipType.FormattingEnabled = True
        Me.cmbEquipType.Location = New System.Drawing.Point(357, 30)
        Me.cmbEquipType.Name = "cmbEquipType"
        Me.cmbEquipType.Size = New System.Drawing.Size(159, 23)
        Me.cmbEquipType.TabIndex = 35
        Me.cmbEquipType.TabStop = False
        '
        'txtSerialSearch
        '
        Me.txtSerialSearch.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerialSearch.Location = New System.Drawing.Point(25, 78)
        Me.txtSerialSearch.MaxLength = 45
        Me.txtSerialSearch.Name = "txtSerialSearch"
        Me.txtSerialSearch.Size = New System.Drawing.Size(135, 23)
        Me.txtSerialSearch.TabIndex = 46
        Me.txtSerialSearch.TabStop = False
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(22, 59)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 16)
        Me.Label3.TabIndex = 49
        Me.Label3.Text = "Serial:"
        '
        'cmbLocation
        '
        Me.cmbLocation.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLocation.FormattingEnabled = True
        Me.cmbLocation.Location = New System.Drawing.Point(537, 30)
        Me.cmbLocation.Name = "cmbLocation"
        Me.cmbLocation.Size = New System.Drawing.Size(165, 23)
        Me.cmbLocation.TabIndex = 39
        Me.cmbLocation.TabStop = False
        '
        'txtCurUser
        '
        Me.txtCurUser.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurUser.Location = New System.Drawing.Point(186, 30)
        Me.txtCurUser.MaxLength = 45
        Me.txtCurUser.Name = "txtCurUser"
        Me.txtCurUser.Size = New System.Drawing.Size(148, 23)
        Me.txtCurUser.TabIndex = 37
        Me.txtCurUser.TabStop = False
        '
        'txtAssetTagSearch
        '
        Me.txtAssetTagSearch.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAssetTagSearch.Location = New System.Drawing.Point(25, 30)
        Me.txtAssetTagSearch.MaxLength = 45
        Me.txtAssetTagSearch.Name = "txtAssetTagSearch"
        Me.txtAssetTagSearch.Size = New System.Drawing.Size(135, 23)
        Me.txtAssetTagSearch.TabIndex = 47
        Me.txtAssetTagSearch.TabStop = False
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(183, 12)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(85, 16)
        Me.Label11.TabIndex = 38
        Me.Label11.Text = "Current User:"
        '
        'cmdSearch
        '
        Me.cmdSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.Location = New System.Drawing.Point(758, 32)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(88, 56)
        Me.cmdSearch.TabIndex = 45
        Me.cmdSearch.Text = "Search"
        Me.cmdSearch.UseVisualStyleBackColor = True
        '
        'cmdClear
        '
        Me.cmdClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClear.Location = New System.Drawing.Point(758, 142)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(88, 32)
        Me.cmdClear.TabIndex = 18
        Me.cmdClear.Text = "Clear"
        Me.cmdClear.UseVisualStyleBackColor = True
        '
        'cmdShowAll
        '
        Me.cmdShowAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdShowAll.Location = New System.Drawing.Point(1069, 105)
        Me.cmdShowAll.Name = "cmdShowAll"
        Me.cmdShowAll.Size = New System.Drawing.Size(134, 35)
        Me.cmdShowAll.TabIndex = 27
        Me.cmdShowAll.Text = "Show All"
        Me.cmdShowAll.UseVisualStyleBackColor = True
        '
        'StatusStrip1
        '
        Me.StatusStrip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.StatusStrip1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel, Me.StripSpinner, Me.ToolStripStatusLabel1, Me.ConnStatusLabel, Me.ToolStripStatusLabel4, Me.DateTimeLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 787)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.ShowItemToolTips = True
        Me.StatusStrip1.Size = New System.Drawing.Size(1381, 22)
        Me.StatusStrip1.TabIndex = 5
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'StatusLabel
        '
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(101, 17)
        Me.StatusLabel.Text = "%StatusLabel%"
        '
        'StripSpinner
        '
        Me.StripSpinner.Image = Global.AssetManager.My.Resources.Resources.LoadingAni
        Me.StripSpinner.Name = "StripSpinner"
        Me.StripSpinner.Size = New System.Drawing.Size(16, 17)
        Me.StripSpinner.Visible = False
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(1104, 17)
        Me.ToolStripStatusLabel1.Spring = True
        '
        'ConnStatusLabel
        '
        Me.ConnStatusLabel.ForeColor = System.Drawing.Color.Green
        Me.ConnStatusLabel.Name = "ConnStatusLabel"
        Me.ConnStatusLabel.Size = New System.Drawing.Size(73, 17)
        Me.ConnStatusLabel.Text = "Connected"
        '
        'ToolStripStatusLabel4
        '
        Me.ToolStripStatusLabel4.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripStatusLabel4.Name = "ToolStripStatusLabel4"
        Me.ToolStripStatusLabel4.Size = New System.Drawing.Size(12, 17)
        Me.ToolStripStatusLabel4.Text = "|"
        '
        'DateTimeLabel
        '
        Me.DateTimeLabel.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeLabel.Name = "DateTimeLabel"
        Me.DateTimeLabel.Size = New System.Drawing.Size(76, 17)
        Me.DateTimeLabel.Text = "ServerTime"
        Me.DateTimeLabel.ToolTipText = "Server Time"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.SearchGroup)
        Me.GroupBox2.Controls.Add(Me.InstantGroup)
        Me.GroupBox2.Controls.Add(Me.cmdShowAll)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 40)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1215, 226)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        '
        'ToolStrip1
        '
        Me.ToolStrip1.AutoSize = False
        Me.ToolStrip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(249, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ToolStrip1.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(25, 25)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.AddDeviceTool, Me.AdminDropDown, Me.ToolStripSeparator5, Me.cmdSibi})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional
        Me.ToolStrip1.Size = New System.Drawing.Size(1381, 37)
        Me.ToolStrip1.Stretch = True
        Me.ToolStrip1.TabIndex = 6
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'AddDeviceTool
        '
        Me.AddDeviceTool.Image = Global.AssetManager.My.Resources.Resources.AddIcon
        Me.AddDeviceTool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AddDeviceTool.Name = "AddDeviceTool"
        Me.AddDeviceTool.Padding = New System.Windows.Forms.Padding(20, 0, 0, 0)
        Me.AddDeviceTool.Size = New System.Drawing.Size(137, 34)
        Me.AddDeviceTool.Text = "Add Device"
        '
        'AdminDropDown
        '
        Me.AdminDropDown.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.txtGUID, Me.tsmUserManager, Me.ReEnterLACredentialsToolStripMenuItem, Me.ViewLogToolStripMenuItem, Me.TextEnCrypterToolStripMenuItem, Me.ScanAttachmentToolStripMenuItem, Me.tsmGKUpdater, Me.AdvancedSearchMenuItem, Me.PSScriptMenuItem})
        Me.AdminDropDown.Image = Global.AssetManager.My.Resources.Resources.AdminIcon
        Me.AdminDropDown.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AdminDropDown.Name = "AdminDropDown"
        Me.AdminDropDown.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.AdminDropDown.Size = New System.Drawing.Size(143, 34)
        Me.AdminDropDown.Text = "Admin Tools"
        '
        'txtGUID
        '
        Me.txtGUID.AutoSize = False
        Me.txtGUID.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.txtGUID.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.txtGUID.Name = "txtGUID"
        Me.txtGUID.Size = New System.Drawing.Size(150, 23)
        Me.txtGUID.ToolTipText = "GUID Lookup. (Press Enter)"
        '
        'tsmUserManager
        '
        Me.tsmUserManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsmUserManager.Name = "tsmUserManager"
        Me.tsmUserManager.Size = New System.Drawing.Size(256, 26)
        Me.tsmUserManager.Text = "User Manager"
        '
        'ReEnterLACredentialsToolStripMenuItem
        '
        Me.ReEnterLACredentialsToolStripMenuItem.Name = "ReEnterLACredentialsToolStripMenuItem"
        Me.ReEnterLACredentialsToolStripMenuItem.Size = New System.Drawing.Size(256, 26)
        Me.ReEnterLACredentialsToolStripMenuItem.Text = "Re-Enter Credentials"
        '
        'TextEnCrypterToolStripMenuItem
        '
        Me.TextEnCrypterToolStripMenuItem.Name = "TextEnCrypterToolStripMenuItem"
        Me.TextEnCrypterToolStripMenuItem.Size = New System.Drawing.Size(256, 26)
        Me.TextEnCrypterToolStripMenuItem.Text = "Text Encrypter"
        '
        'ScanAttachmentToolStripMenuItem
        '
        Me.ScanAttachmentToolStripMenuItem.Name = "ScanAttachmentToolStripMenuItem"
        Me.ScanAttachmentToolStripMenuItem.Size = New System.Drawing.Size(256, 26)
        Me.ScanAttachmentToolStripMenuItem.Text = "Scan Attachments"
        '
        'tsmGKUpdater
        '
        Me.tsmGKUpdater.Name = "tsmGKUpdater"
        Me.tsmGKUpdater.Size = New System.Drawing.Size(256, 26)
        Me.tsmGKUpdater.Text = "GK Updater"
        '
        'AdvancedSearchMenuItem
        '
        Me.AdvancedSearchMenuItem.Name = "AdvancedSearchMenuItem"
        Me.AdvancedSearchMenuItem.Size = New System.Drawing.Size(256, 26)
        Me.AdvancedSearchMenuItem.Text = "Advanced Search"
        '
        'PSScriptMenuItem
        '
        Me.PSScriptMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.InstallChromeMenuItem})
        Me.PSScriptMenuItem.Name = "PSScriptMenuItem"
        Me.PSScriptMenuItem.Size = New System.Drawing.Size(256, 26)
        Me.PSScriptMenuItem.Text = "Execute Remote PS Script"
        '
        'InstallChromeMenuItem
        '
        Me.InstallChromeMenuItem.Name = "InstallChromeMenuItem"
        Me.InstallChromeMenuItem.Size = New System.Drawing.Size(237, 26)
        Me.InstallChromeMenuItem.Text = "Install/Update Chrome"
        '
        'ViewLogToolStripMenuItem
        '
        Me.ViewLogToolStripMenuItem.Name = "ViewLogToolStripMenuItem"
        Me.ViewLogToolStripMenuItem.Size = New System.Drawing.Size(256, 26)
        Me.ViewLogToolStripMenuItem.Text = "View Log"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 37)
        '
        'cmdSibi
        '
        Me.cmdSibi.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right
        Me.cmdSibi.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSibi.Image = Global.AssetManager.My.Resources.Resources.SibiIcon
        Me.cmdSibi.Name = "cmdSibi"
        Me.cmdSibi.Padding = New System.Windows.Forms.Padding(20, 0, 20, 0)
        Me.cmdSibi.Size = New System.Drawing.Size(232, 34)
        Me.cmdSibi.Text = "Sibi Acquisition Manager"
        Me.cmdSibi.TextAlign = System.Drawing.ContentAlignment.MiddleRight
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1381, 809)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupBox1)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(1256, 443)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Asset Manager - Main"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.ResultGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.InstantGroup.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.SearchGroup.ResumeLayout(False)
        Me.SearchPanel.ResumeLayout(False)
        Me.SearchPanel.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents cmdShowAll As Button
    Friend WithEvents cmdClear As Button
    Friend WithEvents ResultGrid As DataGridView
    Friend WithEvents cmdSearch As Button
    Friend WithEvents SearchGroup As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cmbStatus As ComboBox
    Friend WithEvents Label12 As Label
    Friend WithEvents cmbLocation As ComboBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtCurUser As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents cmbEquipType As ComboBox
    Friend WithEvents ContextMenuStrip1 As ContextMenuStrip
    Friend WithEvents ViewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents StatusLabel As ToolStripStatusLabel
    Friend WithEvents Label2 As Label
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents InstantGroup As GroupBox
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents txtAssetTag As TextBox
    Friend WithEvents txtSerial As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents txtAssetTagSearch As TextBox
    Friend WithEvents txtSerialSearch As TextBox
    Friend WithEvents StripSpinner As ToolStripStatusLabel
    Friend WithEvents ToolStrip1 As OneClickToolStrip
    Friend WithEvents AddDeviceTool As ToolStripButton
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents CopyTool As ToolStripMenuItem
    Friend WithEvents ConnStatusLabel As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents DateTimeLabel As ToolStripStatusLabel
    Friend WithEvents AdminDropDown As ToolStripDropDownButton
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents chkTrackables As CheckBox
    Friend WithEvents txtGUID As ToolStripTextBox
    Friend WithEvents cmbOSType As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents txtReplaceYear As TextBox
    Friend WithEvents SearchPanel As Panel
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents cmdSibi As ToolStripButton
    Friend WithEvents tsmUserManager As ToolStripMenuItem
    Friend WithEvents TextEnCrypterToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ToolStripStatusLabel4 As ToolStripStatusLabel
    Friend WithEvents lblRecords As Label
    Friend WithEvents ScanAttachmentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents cmdSupDevSearch As Button
    Friend WithEvents chkHistorical As CheckBox
    Friend WithEvents tsmAddGKUpdate As ToolStripMenuItem
    Friend WithEvents tsmGKUpdater As ToolStripMenuItem
    Friend WithEvents AdvancedSearchMenuItem As ToolStripMenuItem
    Friend WithEvents tsmSendToGridForm As ToolStripMenuItem
    Friend WithEvents PSScriptMenuItem As ToolStripMenuItem
    Friend WithEvents InstallChromeMenuItem As ToolStripMenuItem
    Friend WithEvents ReEnterLACredentialsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ViewLogToolStripMenuItem As ToolStripMenuItem
End Class
