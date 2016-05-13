<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AssetManager
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
        Dim DataGridViewCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AssetManager))
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.NewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ReportsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.YearsSincePurchaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.LiveBox = New System.Windows.Forms.ListBox()
        Me.InstantGroup = New System.Windows.Forms.GroupBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtAssetTag = New System.Windows.Forms.TextBox()
        Me.txtSerial = New System.Windows.Forms.TextBox()
        Me.SearchGroup = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtAssetTagSearch = New System.Windows.Forms.TextBox()
        Me.txtSerialSearch = New System.Windows.Forms.TextBox()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.cmbLocation = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtCurUser = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbEquipType = New System.Windows.Forms.ComboBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.cmdShowAll = New System.Windows.Forms.Button()
        Me.cmdClear = New System.Windows.Forms.Button()
        Me.ResultGrid = New System.Windows.Forms.DataGridView()
        Me.ContextMenuStrip1 = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ViewToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.StripSpinner = New System.Windows.Forms.ToolStripStatusLabel()
        Me.LiveQueryWorker = New System.ComponentModel.BackgroundWorker()
        Me.BigQueryWorker = New System.ComponentModel.BackgroundWorker()
        Me.MenuStrip1.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.InstantGroup.SuspendLayout()
        Me.SearchGroup.SuspendLayout()
        CType(Me.ResultGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.ContextMenuStrip1.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1230, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.NewToolStripMenuItem, Me.ReportsToolStripMenuItem})
        Me.EditToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(60, 20)
        Me.EditToolStripMenuItem.Text = "Actions"
        '
        'NewToolStripMenuItem
        '
        Me.NewToolStripMenuItem.Image = Global.AssetManager.My.Resources.Resources.Add
        Me.NewToolStripMenuItem.Name = "NewToolStripMenuItem"
        Me.NewToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.NewToolStripMenuItem.Text = "New Device"
        '
        'ReportsToolStripMenuItem
        '
        Me.ReportsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.YearsSincePurchaseToolStripMenuItem})
        Me.ReportsToolStripMenuItem.Image = Global.AssetManager.My.Resources.Resources.presentation_512
        Me.ReportsToolStripMenuItem.Name = "ReportsToolStripMenuItem"
        Me.ReportsToolStripMenuItem.Size = New System.Drawing.Size(142, 22)
        Me.ReportsToolStripMenuItem.Text = "Reports"
        '
        'YearsSincePurchaseToolStripMenuItem
        '
        Me.YearsSincePurchaseToolStripMenuItem.Name = "YearsSincePurchaseToolStripMenuItem"
        Me.YearsSincePurchaseToolStripMenuItem.Size = New System.Drawing.Size(190, 22)
        Me.YearsSincePurchaseToolStripMenuItem.Text = "Years Since Purchase"
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox1.Controls.Add(Me.LiveBox)
        Me.GroupBox1.Controls.Add(Me.InstantGroup)
        Me.GroupBox1.Controls.Add(Me.SearchGroup)
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.cmdShowAll)
        Me.GroupBox1.Controls.Add(Me.cmdClear)
        Me.GroupBox1.Controls.Add(Me.ResultGrid)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 29)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1201, 531)
        Me.GroupBox1.TabIndex = 4
        Me.GroupBox1.TabStop = False
        '
        'LiveBox
        '
        Me.LiveBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(207, Byte), Integer), CType(CType(207, Byte), Integer), CType(CType(207, Byte), Integer))
        Me.LiveBox.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.LiveBox.ForeColor = System.Drawing.SystemColors.WindowText
        Me.LiveBox.FormattingEnabled = True
        Me.LiveBox.ItemHeight = 18
        Me.LiveBox.Location = New System.Drawing.Point(1038, 148)
        Me.LiveBox.Name = "LiveBox"
        Me.LiveBox.Size = New System.Drawing.Size(134, 22)
        Me.LiveBox.TabIndex = 33
        '
        'InstantGroup
        '
        Me.InstantGroup.BackColor = System.Drawing.SystemColors.Control
        Me.InstantGroup.Controls.Add(Me.Label9)
        Me.InstantGroup.Controls.Add(Me.Label8)
        Me.InstantGroup.Controls.Add(Me.txtAssetTag)
        Me.InstantGroup.Controls.Add(Me.txtSerial)
        Me.InstantGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.InstantGroup.Location = New System.Drawing.Point(15, 19)
        Me.InstantGroup.Name = "InstantGroup"
        Me.InstantGroup.Size = New System.Drawing.Size(177, 149)
        Me.InstantGroup.TabIndex = 34
        Me.InstantGroup.TabStop = False
        Me.InstantGroup.Text = "Instant Lookup"
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(19, 77)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(46, 16)
        Me.Label9.TabIndex = 38
        Me.Label9.Text = "Serial:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(19, 30)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(73, 16)
        Me.Label8.TabIndex = 37
        Me.Label8.Text = "Asset Tag:"
        '
        'txtAssetTag
        '
        Me.txtAssetTag.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAssetTag.Location = New System.Drawing.Point(22, 47)
        Me.txtAssetTag.MaxLength = 45
        Me.txtAssetTag.Name = "txtAssetTag"
        Me.txtAssetTag.Size = New System.Drawing.Size(135, 22)
        Me.txtAssetTag.TabIndex = 36
        '
        'txtSerial
        '
        Me.txtSerial.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerial.Location = New System.Drawing.Point(22, 96)
        Me.txtSerial.MaxLength = 45
        Me.txtSerial.Name = "txtSerial"
        Me.txtSerial.Size = New System.Drawing.Size(135, 22)
        Me.txtSerial.TabIndex = 35
        '
        'SearchGroup
        '
        Me.SearchGroup.BackColor = System.Drawing.SystemColors.Control
        Me.SearchGroup.Controls.Add(Me.Label3)
        Me.SearchGroup.Controls.Add(Me.Label4)
        Me.SearchGroup.Controls.Add(Me.txtAssetTagSearch)
        Me.SearchGroup.Controls.Add(Me.txtSerialSearch)
        Me.SearchGroup.Controls.Add(Me.cmdSearch)
        Me.SearchGroup.Controls.Add(Me.Label2)
        Me.SearchGroup.Controls.Add(Me.txtDescription)
        Me.SearchGroup.Controls.Add(Me.Label1)
        Me.SearchGroup.Controls.Add(Me.cmbStatus)
        Me.SearchGroup.Controls.Add(Me.Label12)
        Me.SearchGroup.Controls.Add(Me.cmbLocation)
        Me.SearchGroup.Controls.Add(Me.Label11)
        Me.SearchGroup.Controls.Add(Me.txtCurUser)
        Me.SearchGroup.Controls.Add(Me.Label10)
        Me.SearchGroup.Controls.Add(Me.cmbEquipType)
        Me.SearchGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SearchGroup.Location = New System.Drawing.Point(198, 19)
        Me.SearchGroup.Name = "SearchGroup"
        Me.SearchGroup.Size = New System.Drawing.Size(823, 149)
        Me.SearchGroup.TabIndex = 31
        Me.SearchGroup.TabStop = False
        Me.SearchGroup.Text = "Search"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(17, 73)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(46, 16)
        Me.Label3.TabIndex = 49
        Me.Label3.Text = "Serial:"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(17, 26)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(73, 16)
        Me.Label4.TabIndex = 48
        Me.Label4.Text = "Asset Tag:"
        '
        'txtAssetTagSearch
        '
        Me.txtAssetTagSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAssetTagSearch.Location = New System.Drawing.Point(20, 43)
        Me.txtAssetTagSearch.MaxLength = 45
        Me.txtAssetTagSearch.Name = "txtAssetTagSearch"
        Me.txtAssetTagSearch.Size = New System.Drawing.Size(135, 22)
        Me.txtAssetTagSearch.TabIndex = 47
        '
        'txtSerialSearch
        '
        Me.txtSerialSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerialSearch.Location = New System.Drawing.Point(20, 92)
        Me.txtSerialSearch.MaxLength = 45
        Me.txtSerialSearch.Name = "txtSerialSearch"
        Me.txtSerialSearch.Size = New System.Drawing.Size(135, 22)
        Me.txtSerialSearch.TabIndex = 46
        '
        'cmdSearch
        '
        Me.cmdSearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdSearch.Location = New System.Drawing.Point(717, 49)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(88, 56)
        Me.cmdSearch.TabIndex = 45
        Me.cmdSearch.Text = "Search"
        Me.cmdSearch.UseVisualStyleBackColor = True
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(178, 74)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(79, 16)
        Me.Label2.TabIndex = 44
        Me.Label2.Text = "Description:"
        '
        'txtDescription
        '
        Me.txtDescription.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.Location = New System.Drawing.Point(181, 92)
        Me.txtDescription.MaxLength = 200
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(330, 22)
        Me.txtDescription.TabIndex = 43
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(529, 74)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(48, 16)
        Me.Label1.TabIndex = 42
        Me.Label1.Text = "Status:"
        '
        'cmbStatus
        '
        Me.cmbStatus.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Location = New System.Drawing.Point(532, 93)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(165, 24)
        Me.cmbStatus.TabIndex = 41
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.Location = New System.Drawing.Point(529, 25)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(62, 16)
        Me.Label12.TabIndex = 40
        Me.Label12.Text = "Location:"
        '
        'cmbLocation
        '
        Me.cmbLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLocation.FormattingEnabled = True
        Me.cmbLocation.Location = New System.Drawing.Point(532, 44)
        Me.cmbLocation.Name = "cmbLocation"
        Me.cmbLocation.Size = New System.Drawing.Size(165, 24)
        Me.cmbLocation.TabIndex = 39
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(178, 26)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(85, 16)
        Me.Label11.TabIndex = 38
        Me.Label11.Text = "Current User:"
        '
        'txtCurUser
        '
        Me.txtCurUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurUser.Location = New System.Drawing.Point(181, 44)
        Me.txtCurUser.MaxLength = 45
        Me.txtCurUser.Name = "txtCurUser"
        Me.txtCurUser.Size = New System.Drawing.Size(148, 22)
        Me.txtCurUser.TabIndex = 37
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(349, 25)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(110, 16)
        Me.Label10.TabIndex = 36
        Me.Label10.Text = "Equipment Type:"
        '
        'cmbEquipType
        '
        Me.cmbEquipType.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbEquipType.FormattingEnabled = True
        Me.cmbEquipType.Location = New System.Drawing.Point(352, 43)
        Me.cmbEquipType.Name = "cmbEquipType"
        Me.cmbEquipType.Size = New System.Drawing.Size(159, 24)
        Me.cmbEquipType.TabIndex = 35
        '
        'Button1
        '
        Me.Button1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Button1.Location = New System.Drawing.Point(1136, 0)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(106, 36)
        Me.Button1.TabIndex = 28
        Me.Button1.Text = "Start Import"
        Me.Button1.UseVisualStyleBackColor = True
        Me.Button1.Visible = False
        '
        'cmdShowAll
        '
        Me.cmdShowAll.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdShowAll.Location = New System.Drawing.Point(1051, 30)
        Me.cmdShowAll.Name = "cmdShowAll"
        Me.cmdShowAll.Size = New System.Drawing.Size(109, 31)
        Me.cmdShowAll.TabIndex = 27
        Me.cmdShowAll.Text = "Show All"
        Me.cmdShowAll.UseVisualStyleBackColor = True
        '
        'cmdClear
        '
        Me.cmdClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClear.Location = New System.Drawing.Point(1050, 113)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(110, 32)
        Me.cmdClear.TabIndex = 18
        Me.cmdClear.Text = "Clear"
        Me.cmdClear.UseVisualStyleBackColor = True
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
        Me.ResultGrid.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle3.BackColor = System.Drawing.SystemColors.Control
        DataGridViewCellStyle3.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle3.ForeColor = System.Drawing.SystemColors.WindowText
        DataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.ResultGrid.ColumnHeadersDefaultCellStyle = DataGridViewCellStyle3
        Me.ResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ResultGrid.ContextMenuStrip = Me.ContextMenuStrip1
        DataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle4.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle4.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle4.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(6, Byte), Integer), CType(CType(159, Byte), Integer), CType(CType(217, Byte), Integer))
        DataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.ResultGrid.DefaultCellStyle = DataGridViewCellStyle4
        Me.ResultGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.ResultGrid.Location = New System.Drawing.Point(15, 174)
        Me.ResultGrid.MultiSelect = False
        Me.ResultGrid.Name = "ResultGrid"
        Me.ResultGrid.ReadOnly = True
        Me.ResultGrid.RowHeadersVisible = False
        Me.ResultGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.ResultGrid.ShowEditingIcon = False
        Me.ResultGrid.Size = New System.Drawing.Size(1174, 343)
        Me.ResultGrid.TabIndex = 17
        '
        'ContextMenuStrip1
        '
        Me.ContextMenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ViewToolStripMenuItem})
        Me.ContextMenuStrip1.Name = "ContextMenuStrip1"
        Me.ContextMenuStrip1.Size = New System.Drawing.Size(106, 26)
        '
        'ViewToolStripMenuItem
        '
        Me.ViewToolStripMenuItem.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ViewToolStripMenuItem.Image = CType(resources.GetObject("ViewToolStripMenuItem.Image"), System.Drawing.Image)
        Me.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem"
        Me.ViewToolStripMenuItem.Size = New System.Drawing.Size(105, 22)
        Me.ViewToolStripMenuItem.Text = "View"
        '
        'StatusStrip1
        '
        Me.StatusStrip1.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel, Me.StripSpinner})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 563)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(1230, 22)
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
        'LiveQueryWorker
        '
        '
        'BigQueryWorker
        '
        Me.BigQueryWorker.WorkerReportsProgress = True
        '
        'AssetManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1230, 585)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.MinimumSize = New System.Drawing.Size(1222, 397)
        Me.Name = "AssetManager"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Asset Manager"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.GroupBox1.ResumeLayout(False)
        Me.InstantGroup.ResumeLayout(False)
        Me.InstantGroup.PerformLayout()
        Me.SearchGroup.ResumeLayout(False)
        Me.SearchGroup.PerformLayout()
        CType(Me.ResultGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ContextMenuStrip1.ResumeLayout(False)
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents cmdShowAll As Button
    Friend WithEvents cmdClear As Button
    Friend WithEvents ResultGrid As DataGridView
    Friend WithEvents cmdSearch As Button
    Friend WithEvents Button1 As Button
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
    Friend WithEvents NewToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ReportsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents YearsSincePurchaseToolStripMenuItem As ToolStripMenuItem
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
End Class
