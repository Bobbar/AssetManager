<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MainForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MainForm))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.ResultGrid = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator3 = New System.Windows.Forms.ToolStripSeparator()
        Me.CopyTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.LiveBox = New System.Windows.Forms.ListBox()
        Me.InstantGroup = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtAssetTag = New System.Windows.Forms.TextBox()
        Me.txtSerial = New System.Windows.Forms.TextBox()
        Me.SearchGroup = New System.Windows.Forms.GroupBox()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.cmdClear = New System.Windows.Forms.Button()
        Me.cmdShowAll = New System.Windows.Forms.Button()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StripSpinner = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ConnStatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.DateTimeLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LiveQueryWorker = New System.ComponentModel.BackgroundWorker()
        Me.BigQueryWorker = New System.ComponentModel.BackgroundWorker()
        Me.ToolStrip1 = New System.Windows.Forms.ToolStrip()
        Me.ToolStripSeparator2 = New System.Windows.Forms.ToolStripSeparator()
        Me.AddDeviceTool = New System.Windows.Forms.ToolStripButton()
        Me.ToolStripDropDownButton1 = New System.Windows.Forms.ToolStripDropDownButton()
        Me.YearsSincePurchaseToolStripMenuItem1 = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripButton1 = New System.Windows.Forms.ToolStripButton()
        Me.AdminDropDown = New System.Windows.Forms.ToolStripDropDownButton()
        Me.cmbDBs = New System.Windows.Forms.ToolStripComboBox()
        Me.ManageAttachmentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.txtGUID = New System.Windows.Forms.ToolStripTextBox()
        Me.tsmUserManager = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator5 = New System.Windows.Forms.ToolStripSeparator()
        Me.cmdSibi = New System.Windows.Forms.ToolStripButton()
        Me.ConnectionWatcher = New System.Windows.Forms.Timer(Me.components)
        Me.ConnectionWatchDog = New System.ComponentModel.BackgroundWorker()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.PanelNoScrollOnFocus1 = New AssetManager.PanelNoScrollOnFocus()
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
        Me.GroupBox1.SuspendLayout()
        CType(Me.ResultGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.InstantGroup.SuspendLayout()
        Me.Panel1.SuspendLayout()
        Me.SearchGroup.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.ToolStrip1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.PanelNoScrollOnFocus1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox1.Controls.Add(Me.ResultGrid)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 222)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1215, 403)
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
        Me.ResultGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.ResultGrid.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.ResultGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.ResultGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
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
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(39, Byte), Integer))
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ResultGrid.DefaultCellStyle = DataGridViewCellStyle2
        Me.ResultGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.ResultGrid.Location = New System.Drawing.Point(15, 19)
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
        Me.ResultGrid.Size = New System.Drawing.Size(1188, 369)
        Me.ResultGrid.TabIndex = 17
        Me.ResultGrid.VirtualMode = True
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem, Me.ToolStripSeparator3, Me.CopyTool})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(127, 54)
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ViewToolStripMenuItem.Image = CType(resources.GetObject("ViewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(126, 22)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'ToolStripSeparator3
        '
        Me.ToolStripSeparator3.Name = "ToolStripSeparator3"
        Me.ToolStripSeparator3.Size = New System.Drawing.Size(123, 6)
        '
        'CopyTool
        '
        Me.CopyTool.Name = "CopyTool"
        Me.CopyTool.Size = New System.Drawing.Size(126, 22)
        Me.CopyTool.Text = "Copy Text"
        '
        'LiveBox
        '
        Me.LiveBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(208, Byte), Integer), CType(CType(99, Byte), Integer))
        Me.LiveBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.LiveBox.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LiveBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.LiveBox.FormattingEnabled = True
        Me.LiveBox.ItemHeight = 18
        Me.LiveBox.Location = New System.Drawing.Point(-21, 207)
        Me.LiveBox.Name = "LiveBox"
        Me.LiveBox.Size = New System.Drawing.Size(134, 20)
        Me.LiveBox.TabIndex = 33
        '
        'InstantGroup
        '
        Me.InstantGroup.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.InstantGroup.Controls.Add(Me.Panel1)
        Me.InstantGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InstantGroup.Location = New System.Drawing.Point(9, 19)
        Me.InstantGroup.Name = "InstantGroup"
        Me.InstantGroup.Size = New System.Drawing.Size(177, 149)
        Me.InstantGroup.TabIndex = 34
        Me.InstantGroup.TabStop = False
        Me.InstantGroup.Text = "Instant Lookup"
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(202, Byte), Integer), CType(CType(202, Byte), Integer), CType(CType(202, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.Label9)
        Me.Panel1.Controls.Add(Me.Label8)
        Me.Panel1.Controls.Add(Me.txtAssetTag)
        Me.Panel1.Controls.Add(Me.txtSerial)
        Me.Panel1.Location = New System.Drawing.Point(6, 20)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(165, 116)
        Me.Panel1.TabIndex = 39
        '
        'Label9
        '
        Me.Label9.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(11, 51)
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
        Me.Label8.Location = New System.Drawing.Point(11, 4)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(73, 16)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "Asset Tag:"
        '
        'txtAssetTag
        '
        Me.txtAssetTag.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtAssetTag.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAssetTag.Location = New System.Drawing.Point(14, 21)
        Me.txtAssetTag.MaxLength = 45
        Me.txtAssetTag.Name = "txtAssetTag"
        Me.txtAssetTag.Size = New System.Drawing.Size(135, 23)
        Me.txtAssetTag.TabIndex = 36
        '
        'txtSerial
        '
        Me.txtSerial.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.txtSerial.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerial.Location = New System.Drawing.Point(14, 70)
        Me.txtSerial.MaxLength = 45
        Me.txtSerial.Name = "txtSerial"
        Me.txtSerial.Size = New System.Drawing.Size(135, 23)
        Me.txtSerial.TabIndex = 35
        '
        'SearchGroup
        '
        Me.SearchGroup.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.SearchGroup.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.SearchGroup.Controls.Add(Me.PanelNoScrollOnFocus1)
        Me.SearchGroup.Controls.Add(Me.cmdSearch)
        Me.SearchGroup.Controls.Add(Me.cmdClear)
        Me.SearchGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SearchGroup.Location = New System.Drawing.Point(192, 19)
        Me.SearchGroup.Name = "SearchGroup"
        Me.SearchGroup.Size = New System.Drawing.Size(862, 149)
        Me.SearchGroup.TabIndex = 31
        Me.SearchGroup.TabStop = False
        Me.SearchGroup.Text = "Search"
        '
        'cmdSearch
        '
        Me.cmdSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.Location = New System.Drawing.Point(759, 29)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(88, 56)
        Me.cmdSearch.TabIndex = 45
        Me.cmdSearch.Text = "Search"
        Me.cmdSearch.UseVisualStyleBackColor = True
        '
        'cmdClear
        '
        Me.cmdClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClear.Location = New System.Drawing.Point(759, 104)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(88, 32)
        Me.cmdClear.TabIndex = 18
        Me.cmdClear.Text = "Clear"
        Me.cmdClear.UseVisualStyleBackColor = True
        '
        'cmdShowAll
        '
        Me.cmdShowAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdShowAll.Location = New System.Drawing.Point(1069, 78)
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
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel, Me.StripSpinner, Me.ToolStripStatusLabel1, Me.ConnStatusLabel, Me.ToolStripStatusLabel2, Me.DateTimeLabel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 632)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1240, 22)
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
        Me.StripSpinner.Image = Global.AssetManager.My.Resources.Resources.loading
        Me.StripSpinner.Name = "StripSpinner"
        Me.StripSpinner.Size = New System.Drawing.Size(16, 17)
        Me.StripSpinner.Visible = False
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(963, 17)
        Me.ToolStripStatusLabel1.Spring = True
        '
        'ConnStatusLabel
        '
        Me.ConnStatusLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(0, Byte), Integer), CType(CType(192, Byte), Integer), CType(CType(0, Byte), Integer))
        Me.ConnStatusLabel.Name = "ConnStatusLabel"
        Me.ConnStatusLabel.Size = New System.Drawing.Size(73, 17)
        Me.ConnStatusLabel.Text = "Connected"
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(12, 17)
        Me.ToolStripStatusLabel2.Text = "|"
        '
        'DateTimeLabel
        '
        Me.DateTimeLabel.Font = New System.Drawing.Font("Segoe UI Semibold", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimeLabel.Name = "DateTimeLabel"
        Me.DateTimeLabel.Size = New System.Drawing.Size(76, 17)
        Me.DateTimeLabel.Text = "ServerTime"
        '
        'LiveQueryWorker
        '
        '
        'BigQueryWorker
        '
        Me.BigQueryWorker.WorkerReportsProgress = True
        '
        'ToolStrip1
        '
        Me.ToolStrip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(249, Byte), Integer), CType(CType(226, Byte), Integer), CType(CType(166, Byte), Integer))
        Me.ToolStrip1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.ToolStrip1.ImageScalingSize = New System.Drawing.Size(25, 25)
        Me.ToolStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripSeparator2, Me.AddDeviceTool, Me.ToolStripDropDownButton1, Me.ToolStripButton1, Me.AdminDropDown, Me.ToolStripSeparator5, Me.cmdSibi})
        Me.ToolStrip1.Location = New System.Drawing.Point(0, 0)
        Me.ToolStrip1.Name = "ToolStrip1"
        Me.ToolStrip1.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ToolStrip1.Size = New System.Drawing.Size(1240, 32)
        Me.ToolStrip1.Stretch = True
        Me.ToolStrip1.TabIndex = 6
        Me.ToolStrip1.Text = "ToolStrip1"
        '
        'ToolStripSeparator2
        '
        Me.ToolStripSeparator2.Name = "ToolStripSeparator2"
        Me.ToolStripSeparator2.Size = New System.Drawing.Size(6, 32)
        '
        'AddDeviceTool
        '
        Me.AddDeviceTool.Image = Global.AssetManager.My.Resources.Resources.Add
        Me.AddDeviceTool.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AddDeviceTool.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.AddDeviceTool.Name = "AddDeviceTool"
        Me.AddDeviceTool.Padding = New System.Windows.Forms.Padding(20, 0, 0, 0)
        Me.AddDeviceTool.Size = New System.Drawing.Size(123, 29)
        Me.AddDeviceTool.Text = "Add Device"
        '
        'ToolStripDropDownButton1
        '
        Me.ToolStripDropDownButton1.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.YearsSincePurchaseToolStripMenuItem1})
        Me.ToolStripDropDownButton1.Image = Global.AssetManager.My.Resources.Resources.presentation_512
        Me.ToolStripDropDownButton1.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.ToolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripDropDownButton1.Name = "ToolStripDropDownButton1"
        Me.ToolStripDropDownButton1.Padding = New System.Windows.Forms.Padding(20, 0, 0, 0)
        Me.ToolStripDropDownButton1.Size = New System.Drawing.Size(112, 29)
        Me.ToolStripDropDownButton1.Text = "Reports"
        '
        'YearsSincePurchaseToolStripMenuItem1
        '
        Me.YearsSincePurchaseToolStripMenuItem1.Name = "YearsSincePurchaseToolStripMenuItem1"
        Me.YearsSincePurchaseToolStripMenuItem1.Size = New System.Drawing.Size(197, 22)
        Me.YearsSincePurchaseToolStripMenuItem1.Text = "Years Since Purchase"
        '
        'ToolStripButton1
        '
        Me.ToolStripButton1.Image = Global.AssetManager.My.Resources.Resources.view
        Me.ToolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.ToolStripButton1.Name = "ToolStripButton1"
        Me.ToolStripButton1.Size = New System.Drawing.Size(150, 29)
        Me.ToolStripButton1.Text = "Org/Object Lookup"
        '
        'AdminDropDown
        '
        Me.AdminDropDown.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmbDBs, Me.ManageAttachmentsToolStripMenuItem, Me.txtGUID, Me.tsmUserManager})
        Me.AdminDropDown.Image = CType(resources.GetObject("AdminDropDown.Image"), System.Drawing.Image)
        Me.AdminDropDown.ImageAlign = System.Drawing.ContentAlignment.MiddleRight
        Me.AdminDropDown.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.AdminDropDown.Name = "AdminDropDown"
        Me.AdminDropDown.Padding = New System.Windows.Forms.Padding(50, 0, 0, 0)
        Me.AdminDropDown.Size = New System.Drawing.Size(163, 29)
        Me.AdminDropDown.Text = "Admin Stuff"
        '
        'cmbDBs
        '
        Me.cmbDBs.Items.AddRange(New Object() {"asset_manager", "test_db"})
        Me.cmbDBs.Name = "cmbDBs"
        Me.cmbDBs.Size = New System.Drawing.Size(121, 23)
        Me.cmbDBs.Text = "asset_manager"
        Me.cmbDBs.ToolTipText = "Change DB"
        Me.cmbDBs.Visible = False
        '
        'ManageAttachmentsToolStripMenuItem
        '
        Me.ManageAttachmentsToolStripMenuItem.Name = "ManageAttachmentsToolStripMenuItem"
        Me.ManageAttachmentsToolStripMenuItem.Size = New System.Drawing.Size(210, 22)
        Me.ManageAttachmentsToolStripMenuItem.Text = "Manage Attachments"
        Me.ManageAttachmentsToolStripMenuItem.ToolTipText = "Manage ALL Attachments"
        '
        'txtGUID
        '
        Me.txtGUID.AutoSize = False
        Me.txtGUID.Name = "txtGUID"
        Me.txtGUID.Size = New System.Drawing.Size(150, 23)
        Me.txtGUID.ToolTipText = "GUID Lookup. (Press Enter)"
        '
        'tsmUserManager
        '
        Me.tsmUserManager.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        Me.tsmUserManager.Name = "tsmUserManager"
        Me.tsmUserManager.Size = New System.Drawing.Size(210, 22)
        Me.tsmUserManager.Text = "User Manager"
        '
        'ToolStripSeparator5
        '
        Me.ToolStripSeparator5.Name = "ToolStripSeparator5"
        Me.ToolStripSeparator5.Size = New System.Drawing.Size(6, 32)
        '
        'cmdSibi
        '
        Me.cmdSibi.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSibi.Image = CType(resources.GetObject("cmdSibi.Image"), System.Drawing.Image)
        Me.cmdSibi.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdSibi.Name = "cmdSibi"
        Me.cmdSibi.Size = New System.Drawing.Size(186, 29)
        Me.cmdSibi.Text = "Sibi Aquisition Manager"
        '
        'ConnectionWatcher
        '
        Me.ConnectionWatcher.Enabled = True
        Me.ConnectionWatcher.Interval = 500
        '
        'ConnectionWatchDog
        '
        Me.ConnectionWatchDog.WorkerReportsProgress = True
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.SearchGroup)
        Me.GroupBox2.Controls.Add(Me.InstantGroup)
        Me.GroupBox2.Controls.Add(Me.cmdShowAll)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 35)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(1215, 181)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        '
        'PanelNoScrollOnFocus1
        '
        Me.PanelNoScrollOnFocus1.AutoScroll = True
        Me.PanelNoScrollOnFocus1.AutoScrollMargin = New System.Drawing.Size(10, 20)
        Me.PanelNoScrollOnFocus1.BackColor = System.Drawing.Color.FromArgb(CType(CType(202, Byte), Integer), CType(CType(202, Byte), Integer), CType(CType(202, Byte), Integer))
        Me.PanelNoScrollOnFocus1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.Label6)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.Label4)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.txtReplaceYear)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.txtDescription)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.cmbOSType)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.Label2)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.Label5)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.Label1)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.Label10)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.cmbStatus)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.chkTrackables)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.Label12)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.cmbEquipType)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.txtSerialSearch)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.Label3)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.cmbLocation)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.txtCurUser)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.txtAssetTagSearch)
        Me.PanelNoScrollOnFocus1.Controls.Add(Me.Label11)
        Me.PanelNoScrollOnFocus1.Location = New System.Drawing.Point(11, 19)
        Me.PanelNoScrollOnFocus1.Name = "PanelNoScrollOnFocus1"
        Me.PanelNoScrollOnFocus1.Size = New System.Drawing.Size(732, 121)
        Me.PanelNoScrollOnFocus1.TabIndex = 52
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(176, 111)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(95, 16)
        Me.Label6.TabIndex = 54
        Me.Label6.Text = "Replace Year:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(15, 9)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 16)
        Me.Label4.TabIndex = 48
        Me.Label4.Text = "Asset Tag:"
        '
        'txtReplaceYear
        '
        Me.txtReplaceYear.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReplaceYear.Location = New System.Drawing.Point(179, 129)
        Me.txtReplaceYear.MaxLength = 200
        Me.txtReplaceYear.Name = "txtReplaceYear"
        Me.txtReplaceYear.Size = New System.Drawing.Size(100, 23)
        Me.txtReplaceYear.TabIndex = 53
        Me.txtReplaceYear.TabStop = False
        '
        'txtDescription
        '
        Me.txtDescription.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.Location = New System.Drawing.Point(179, 75)
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
        Me.cmbOSType.Location = New System.Drawing.Point(18, 129)
        Me.cmbOSType.Name = "cmbOSType"
        Me.cmbOSType.Size = New System.Drawing.Size(135, 23)
        Me.cmbOSType.TabIndex = 51
        Me.cmbOSType.TabStop = False
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(176, 57)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 16)
        Me.Label2.TabIndex = 44
        Me.Label2.Text = "Description:"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(15, 110)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(65, 16)
        Me.Label5.TabIndex = 52
        Me.Label5.Text = "OS Type:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(527, 57)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 16)
        Me.Label1.TabIndex = 42
        Me.Label1.Text = "Status:"
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(347, 8)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(110, 16)
        Me.Label10.TabIndex = 36
        Me.Label10.Text = "Equipment Type:"
        '
        'cmbStatus
        '
        Me.cmbStatus.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Location = New System.Drawing.Point(530, 76)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(165, 23)
        Me.cmbStatus.TabIndex = 41
        Me.cmbStatus.TabStop = False
        '
        'chkTrackables
        '
        Me.chkTrackables.AutoSize = True
        Me.chkTrackables.Font = New System.Drawing.Font("Microsoft Sans Serif", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTrackables.Location = New System.Drawing.Point(324, 128)
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
        Me.Label12.Location = New System.Drawing.Point(527, 8)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(62, 16)
        Me.Label12.TabIndex = 40
        Me.Label12.Text = "Location:"
        '
        'cmbEquipType
        '
        Me.cmbEquipType.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbEquipType.FormattingEnabled = True
        Me.cmbEquipType.Location = New System.Drawing.Point(350, 26)
        Me.cmbEquipType.Name = "cmbEquipType"
        Me.cmbEquipType.Size = New System.Drawing.Size(159, 23)
        Me.cmbEquipType.TabIndex = 35
        Me.cmbEquipType.TabStop = False
        '
        'txtSerialSearch
        '
        Me.txtSerialSearch.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerialSearch.Location = New System.Drawing.Point(18, 75)
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
        Me.Label3.Location = New System.Drawing.Point(15, 56)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 16)
        Me.Label3.TabIndex = 49
        Me.Label3.Text = "Serial:"
        '
        'cmbLocation
        '
        Me.cmbLocation.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLocation.FormattingEnabled = True
        Me.cmbLocation.Location = New System.Drawing.Point(530, 27)
        Me.cmbLocation.Name = "cmbLocation"
        Me.cmbLocation.Size = New System.Drawing.Size(165, 23)
        Me.cmbLocation.TabIndex = 39
        Me.cmbLocation.TabStop = False
        '
        'txtCurUser
        '
        Me.txtCurUser.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurUser.Location = New System.Drawing.Point(179, 27)
        Me.txtCurUser.MaxLength = 45
        Me.txtCurUser.Name = "txtCurUser"
        Me.txtCurUser.Size = New System.Drawing.Size(148, 23)
        Me.txtCurUser.TabIndex = 37
        Me.txtCurUser.TabStop = False
        '
        'txtAssetTagSearch
        '
        Me.txtAssetTagSearch.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAssetTagSearch.Location = New System.Drawing.Point(18, 26)
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
        Me.Label11.Location = New System.Drawing.Point(176, 9)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(85, 16)
        Me.Label11.TabIndex = 38
        Me.Label11.Text = "Current User:"
        '
        'MainForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1240, 654)
        Me.Controls.Add(Me.LiveBox)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.ToolStrip1)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupBox1)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(1256, 443)
        Me.Name = "MainForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Asset Manager"
        Me.GroupBox1.ResumeLayout(False)
        CType(Me.ResultGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.InstantGroup.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.SearchGroup.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ToolStrip1.ResumeLayout(False)
        Me.ToolStrip1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.PanelNoScrollOnFocus1.ResumeLayout(False)
        Me.PanelNoScrollOnFocus1.PerformLayout()
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
    Friend WithEvents LiveQueryWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents LiveBox As ListBox
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
    Friend WithEvents BigQueryWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents StripSpinner As ToolStripStatusLabel
    Friend WithEvents ToolStrip1 As ToolStrip
    Friend WithEvents AddDeviceTool As ToolStripButton
    Friend WithEvents ToolStripDropDownButton1 As ToolStripDropDownButton
    Friend WithEvents YearsSincePurchaseToolStripMenuItem1 As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator2 As ToolStripSeparator
    Friend WithEvents ToolStripSeparator3 As ToolStripSeparator
    Friend WithEvents CopyTool As ToolStripMenuItem
    Friend WithEvents ConnectionWatcher As Timer
    Friend WithEvents ConnStatusLabel As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents DateTimeLabel As ToolStripStatusLabel
    Friend WithEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WithEvents ConnectionWatchDog As System.ComponentModel.BackgroundWorker
    Friend WithEvents AdminDropDown As ToolStripDropDownButton
    Friend WithEvents cmbDBs As ToolStripComboBox
    Friend WithEvents ManageAttachmentsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents chkTrackables As CheckBox
    Friend WithEvents txtGUID As ToolStripTextBox
    Friend WithEvents cmbOSType As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents Label6 As Label
    Friend WithEvents txtReplaceYear As TextBox
    Friend WithEvents PanelNoScrollOnFocus1 As AssetManager.PanelNoScrollOnFocus
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ToolStripSeparator5 As ToolStripSeparator
    Friend WithEvents cmdSibi As ToolStripButton
    Friend WithEvents ToolStripButton1 As ToolStripButton
    Friend WithEvents tsmUserManager As ToolStripMenuItem
End Class
