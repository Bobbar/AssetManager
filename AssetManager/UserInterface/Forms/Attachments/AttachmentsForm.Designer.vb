<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AttachmentsForm

    Inherits ExtendedForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AttachmentsForm))
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.cmdUpload = New System.Windows.Forms.Button()
        Me.RightClickMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.SaveToMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyTextTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.RenameStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeleteAttachmentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdOpen = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.AttachContainer = New System.Windows.Forms.SplitContainer()
        Me.FolderListView = New System.Windows.Forms.ListView()
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.imageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.AttachGrid = New System.Windows.Forms.DataGridView()
        Me.AllowDragCheckBox = New System.Windows.Forms.CheckBox()
        Me.SibiGroup = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.ReqNumberTextBox = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtRequestNum = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.ReqPO = New System.Windows.Forms.TextBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Spinner = New System.Windows.Forms.ToolStripStatusLabel()
        Me.statMBPS = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.cmdCancel = New System.Windows.Forms.ToolStripDropDownButton()
        Me.ProgTimer = New System.Windows.Forms.Timer(Me.components)
        Me.DeviceGroup = New System.Windows.Forms.GroupBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.txtDeviceDescription = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtSerial = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtAssetTag = New System.Windows.Forms.TextBox()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.NewFolderMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RightClickMenu.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.AttachContainer, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.AttachContainer.Panel1.SuspendLayout()
        Me.AttachContainer.Panel2.SuspendLayout()
        Me.AttachContainer.SuspendLayout()
        CType(Me.AttachGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SibiGroup.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
        Me.DeviceGroup.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'cmdUpload
        '
        Me.cmdUpload.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdUpload.Location = New System.Drawing.Point(23, 29)
        Me.cmdUpload.Name = "cmdUpload"
        Me.cmdUpload.Size = New System.Drawing.Size(92, 46)
        Me.cmdUpload.TabIndex = 0
        Me.cmdUpload.Text = "Upload"
        Me.cmdUpload.UseVisualStyleBackColor = True
        '
        'RightClickMenu
        '
        Me.RightClickMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenTool, Me.SaveToMenuItem, Me.CopyTextTool, Me.NewFolderMenuItem, Me.RenameStripMenuItem, Me.ToolStripSeparator1, Me.DeleteAttachmentToolStripMenuItem})
        Me.RightClickMenu.Name = "RightClickMenu"
        Me.RightClickMenu.Size = New System.Drawing.Size(178, 164)
        '
        'OpenTool
        '
        Me.OpenTool.Name = "OpenTool"
        Me.OpenTool.Size = New System.Drawing.Size(177, 22)
        Me.OpenTool.Text = "Open"
        '
        'SaveToMenuItem
        '
        Me.SaveToMenuItem.Name = "SaveToMenuItem"
        Me.SaveToMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.SaveToMenuItem.Text = "Save To"
        '
        'CopyTextTool
        '
        Me.CopyTextTool.Name = "CopyTextTool"
        Me.CopyTextTool.Size = New System.Drawing.Size(177, 22)
        Me.CopyTextTool.Text = "Copy Text"
        '
        'RenameStripMenuItem
        '
        Me.RenameStripMenuItem.Name = "RenameStripMenuItem"
        Me.RenameStripMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.RenameStripMenuItem.Text = "Rename"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(174, 6)
        '
        'DeleteAttachmentToolStripMenuItem
        '
        Me.DeleteAttachmentToolStripMenuItem.Image = Global.AssetManager.My.Resources.Resources.DeleteRedIcon
        Me.DeleteAttachmentToolStripMenuItem.Name = "DeleteAttachmentToolStripMenuItem"
        Me.DeleteAttachmentToolStripMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.DeleteAttachmentToolStripMenuItem.Text = "Delete Attachment"
        '
        'cmdDelete
        '
        Me.cmdDelete.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdDelete.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft
        Me.cmdDelete.Location = New System.Drawing.Point(23, 143)
        Me.cmdDelete.Name = "cmdDelete"
        Me.cmdDelete.Size = New System.Drawing.Size(92, 25)
        Me.cmdDelete.TabIndex = 4
        Me.cmdDelete.Text = "Delete"
        Me.cmdDelete.UseVisualStyleBackColor = True
        '
        'cmdOpen
        '
        Me.cmdOpen.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdOpen.Location = New System.Drawing.Point(23, 81)
        Me.cmdOpen.Name = "cmdOpen"
        Me.cmdOpen.Size = New System.Drawing.Size(92, 23)
        Me.cmdOpen.TabIndex = 5
        Me.cmdOpen.Text = "Open"
        Me.cmdOpen.UseVisualStyleBackColor = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.GroupBox1.Controls.Add(Me.Panel1)
        Me.GroupBox1.Controls.Add(Me.cmdOpen)
        Me.GroupBox1.Controls.Add(Me.cmdDelete)
        Me.GroupBox1.Controls.Add(Me.cmdUpload)
        Me.GroupBox1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 113)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(793, 440)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Manage Attachments"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.AttachContainer)
        Me.Panel1.Controls.Add(Me.AllowDragCheckBox)
        Me.Panel1.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(150, 21)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(637, 413)
        Me.Panel1.TabIndex = 19
        '
        'AttachContainer
        '
        Me.AttachContainer.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AttachContainer.Location = New System.Drawing.Point(3, 22)
        Me.AttachContainer.Name = "AttachContainer"
        '
        'AttachContainer.Panel1
        '
        Me.AttachContainer.Panel1.Controls.Add(Me.FolderListView)
        '
        'AttachContainer.Panel2
        '
        Me.AttachContainer.Panel2.Controls.Add(Me.AttachGrid)
        Me.AttachContainer.Size = New System.Drawing.Size(629, 386)
        Me.AttachContainer.SplitterDistance = 130
        Me.AttachContainer.TabIndex = 23
        '
        'FolderListView
        '
        Me.FolderListView.AllowDrop = True
        Me.FolderListView.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.FolderListView.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.FolderListView.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1})
        Me.FolderListView.Dock = System.Windows.Forms.DockStyle.Fill
        Me.FolderListView.ForeColor = System.Drawing.Color.White
        Me.FolderListView.Location = New System.Drawing.Point(0, 0)
        Me.FolderListView.MultiSelect = False
        Me.FolderListView.Name = "FolderListView"
        Me.FolderListView.Size = New System.Drawing.Size(130, 386)
        Me.FolderListView.StateImageList = Me.imageList1
        Me.FolderListView.TabIndex = 0
        Me.FolderListView.UseCompatibleStateImageBehavior = False
        Me.FolderListView.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Folder"
        Me.ColumnHeader1.Width = 115
        '
        'imageList1
        '
        Me.imageList1.ImageStream = CType(resources.GetObject("imageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.imageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.imageList1.Images.SetKeyName(0, "icons8-Folder-48.png")
        Me.imageList1.Images.SetKeyName(1, "icons8-Open-48.png")
        '
        'AttachGrid
        '
        Me.AttachGrid.AllowDrop = True
        Me.AttachGrid.AllowUserToAddRows = False
        Me.AttachGrid.AllowUserToDeleteRows = False
        Me.AttachGrid.AllowUserToResizeRows = False
        Me.AttachGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.AttachGrid.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.AttachGrid.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None
        Me.AttachGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.AttachGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.AttachGrid.ContextMenuStrip = Me.RightClickMenu
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.Padding = New System.Windows.Forms.Padding(10, 0, 10, 0)
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(39, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.AttachGrid.DefaultCellStyle = DataGridViewCellStyle1
        Me.AttachGrid.Dock = System.Windows.Forms.DockStyle.Fill
        Me.AttachGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.AttachGrid.Location = New System.Drawing.Point(0, 0)
        Me.AttachGrid.MultiSelect = False
        Me.AttachGrid.Name = "AttachGrid"
        Me.AttachGrid.ReadOnly = True
        Me.AttachGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.AttachGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.AttachGrid.RowHeadersVisible = False
        Me.AttachGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.AttachGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.AttachGrid.ShowCellErrors = False
        Me.AttachGrid.ShowCellToolTips = False
        Me.AttachGrid.ShowEditingIcon = False
        Me.AttachGrid.Size = New System.Drawing.Size(495, 386)
        Me.AttachGrid.TabIndex = 18
        Me.AttachGrid.VirtualMode = True
        '
        'AllowDragCheckBox
        '
        Me.AllowDragCheckBox.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AllowDragCheckBox.AutoSize = True
        Me.AllowDragCheckBox.Checked = True
        Me.AllowDragCheckBox.CheckState = System.Windows.Forms.CheckState.Checked
        Me.AllowDragCheckBox.Location = New System.Drawing.Point(543, 3)
        Me.AllowDragCheckBox.Name = "AllowDragCheckBox"
        Me.AllowDragCheckBox.Size = New System.Drawing.Size(89, 19)
        Me.AllowDragCheckBox.TabIndex = 21
        Me.AllowDragCheckBox.Text = "Drag-Drop"
        Me.AllowDragCheckBox.UseVisualStyleBackColor = True
        '
        'SibiGroup
        '
        Me.SibiGroup.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.SibiGroup.Controls.Add(Me.Label4)
        Me.SibiGroup.Controls.Add(Me.ReqNumberTextBox)
        Me.SibiGroup.Controls.Add(Me.Label3)
        Me.SibiGroup.Controls.Add(Me.txtDescription)
        Me.SibiGroup.Controls.Add(Me.Label2)
        Me.SibiGroup.Controls.Add(Me.txtRequestNum)
        Me.SibiGroup.Controls.Add(Me.Label1)
        Me.SibiGroup.Controls.Add(Me.ReqPO)
        Me.SibiGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.SibiGroup.Location = New System.Drawing.Point(0, 3)
        Me.SibiGroup.Name = "SibiGroup"
        Me.SibiGroup.Size = New System.Drawing.Size(793, 95)
        Me.SibiGroup.TabIndex = 7
        Me.SibiGroup.TabStop = False
        Me.SibiGroup.Text = "Request Info"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(598, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(47, 16)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Req #:"
        '
        'ReqNumberTextBox
        '
        Me.ReqNumberTextBox.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ReqNumberTextBox.Location = New System.Drawing.Point(601, 49)
        Me.ReqNumberTextBox.Name = "ReqNumberTextBox"
        Me.ReqNumberTextBox.ReadOnly = True
        Me.ReqNumberTextBox.Size = New System.Drawing.Size(120, 25)
        Me.ReqNumberTextBox.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(20, 29)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(79, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Description:"
        '
        'txtDescription
        '
        Me.txtDescription.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.Location = New System.Drawing.Point(23, 49)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.Size = New System.Drawing.Size(259, 25)
        Me.txtDescription.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(317, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(72, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Request #:"
        '
        'txtRequestNum
        '
        Me.txtRequestNum.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRequestNum.Location = New System.Drawing.Point(320, 49)
        Me.txtRequestNum.Name = "txtRequestNum"
        Me.txtRequestNum.ReadOnly = True
        Me.txtRequestNum.Size = New System.Drawing.Size(83, 25)
        Me.txtRequestNum.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(439, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(40, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "PO #:"
        '
        'ReqPO
        '
        Me.ReqPO.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ReqPO.Location = New System.Drawing.Point(442, 49)
        Me.ReqPO.Name = "ReqPO"
        Me.ReqPO.ReadOnly = True
        Me.ReqPO.Size = New System.Drawing.Size(120, 25)
        Me.ReqPO.TabIndex = 0
        '
        'StatusStrip1
        '
        Me.StatusStrip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel, Me.ProgressBar1, Me.ToolStripStatusLabel2, Me.Spinner, Me.statMBPS, Me.ToolStripStatusLabel1, Me.cmdCancel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 556)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(817, 22)
        Me.StatusStrip1.TabIndex = 8
        Me.StatusStrip1.Text = "StatusStrip1"
        '
        'StatusLabel
        '
        Me.StatusLabel.Font = New System.Drawing.Font("Segoe UI", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Padding = New System.Windows.Forms.Padding(0, 0, 10, 0)
        Me.StatusLabel.Size = New System.Drawing.Size(86, 17)
        Me.StatusLabel.Text = "%STATUS%"
        Me.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
        '
        'ProgressBar1
        '
        Me.ProgressBar1.AutoSize = False
        Me.ProgressBar1.Name = "ProgressBar1"
        Me.ProgressBar1.Size = New System.Drawing.Size(150, 16)
        Me.ProgressBar1.Step = 1
        Me.ProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Continuous
        Me.ProgressBar1.Visible = False
        '
        'ToolStripStatusLabel2
        '
        Me.ToolStripStatusLabel2.AutoSize = False
        Me.ToolStripStatusLabel2.Name = "ToolStripStatusLabel2"
        Me.ToolStripStatusLabel2.Size = New System.Drawing.Size(10, 17)
        '
        'Spinner
        '
        Me.Spinner.Image = Global.AssetManager.My.Resources.Resources.LoadingAni
        Me.Spinner.Name = "Spinner"
        Me.Spinner.Size = New System.Drawing.Size(16, 17)
        Me.Spinner.Visible = False
        '
        'statMBPS
        '
        Me.statMBPS.Font = New System.Drawing.Font("Segoe UI Semibold", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.statMBPS.Name = "statMBPS"
        Me.statMBPS.Padding = New System.Windows.Forms.Padding(10, 0, 0, 0)
        Me.statMBPS.Size = New System.Drawing.Size(10, 17)
        '
        'ToolStripStatusLabel1
        '
        Me.ToolStripStatusLabel1.Name = "ToolStripStatusLabel1"
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(696, 17)
        Me.ToolStripStatusLabel1.Spring = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Image = Global.AssetManager.My.Resources.Resources.CloseCancelDeleteIcon
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.ShowDropDownArrow = False
        Me.cmdCancel.Size = New System.Drawing.Size(63, 20)
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.Visible = False
        '
        'ProgTimer
        '
        '
        'DeviceGroup
        '
        Me.DeviceGroup.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.DeviceGroup.Controls.Add(Me.Label5)
        Me.DeviceGroup.Controls.Add(Me.txtDeviceDescription)
        Me.DeviceGroup.Controls.Add(Me.Label6)
        Me.DeviceGroup.Controls.Add(Me.txtSerial)
        Me.DeviceGroup.Controls.Add(Me.Label7)
        Me.DeviceGroup.Controls.Add(Me.txtAssetTag)
        Me.DeviceGroup.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DeviceGroup.Location = New System.Drawing.Point(817, 3)
        Me.DeviceGroup.Name = "DeviceGroup"
        Me.DeviceGroup.Size = New System.Drawing.Size(633, 95)
        Me.DeviceGroup.TabIndex = 9
        Me.DeviceGroup.TabStop = False
        Me.DeviceGroup.Text = "Device Info"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(331, 29)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(79, 16)
        Me.Label5.TabIndex = 5
        Me.Label5.Text = "Description:"
        '
        'txtDeviceDescription
        '
        Me.txtDeviceDescription.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDeviceDescription.Location = New System.Drawing.Point(334, 49)
        Me.txtDeviceDescription.Name = "txtDeviceDescription"
        Me.txtDeviceDescription.ReadOnly = True
        Me.txtDeviceDescription.Size = New System.Drawing.Size(259, 25)
        Me.txtDeviceDescription.TabIndex = 4
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(175, 29)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(46, 16)
        Me.Label6.TabIndex = 3
        Me.Label6.Text = "Serial:"
        '
        'txtSerial
        '
        Me.txtSerial.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerial.Location = New System.Drawing.Point(178, 49)
        Me.txtSerial.Name = "txtSerial"
        Me.txtSerial.ReadOnly = True
        Me.txtSerial.Size = New System.Drawing.Size(105, 25)
        Me.txtSerial.TabIndex = 2
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(30, 29)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(73, 16)
        Me.Label7.TabIndex = 1
        Me.Label7.Text = "Asset Tag:"
        '
        'txtAssetTag
        '
        Me.txtAssetTag.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAssetTag.Location = New System.Drawing.Point(33, 49)
        Me.txtAssetTag.Name = "txtAssetTag"
        Me.txtAssetTag.ReadOnly = True
        Me.txtAssetTag.Size = New System.Drawing.Size(105, 25)
        Me.txtAssetTag.TabIndex = 0
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.Controls.Add(Me.DeviceGroup)
        Me.Panel2.Controls.Add(Me.SibiGroup)
        Me.Panel2.Location = New System.Drawing.Point(12, 12)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(793, 116)
        Me.Panel2.TabIndex = 10
        '
        'NewFolderMenuItem
        '
        Me.NewFolderMenuItem.Name = "NewFolderMenuItem"
        Me.NewFolderMenuItem.Size = New System.Drawing.Size(177, 22)
        Me.NewFolderMenuItem.Text = "Move to new folder"
        '
        'AttachmentsForm
        '
        Me.AllowDrop = True
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(817, 578)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Panel2)
        Me.DoubleBuffered = True
        Me.MinimumSize = New System.Drawing.Size(833, 370)
        Me.Name = "AttachmentsForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Attachments"
        Me.RightClickMenu.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.AttachContainer.Panel1.ResumeLayout(False)
        Me.AttachContainer.Panel2.ResumeLayout(False)
        CType(Me.AttachContainer, System.ComponentModel.ISupportInitialize).EndInit()
        Me.AttachContainer.ResumeLayout(False)
        CType(Me.AttachGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.SibiGroup.ResumeLayout(False)
        Me.SibiGroup.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.DeviceGroup.ResumeLayout(False)
        Me.DeviceGroup.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdUpload As Button
    Friend WithEvents RightClickMenu As ContextMenuStrip
    Friend WithEvents DeleteAttachmentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents cmdDelete As Button
    Friend WithEvents cmdOpen As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents SibiGroup As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtRequestNum As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents ReqPO As TextBox
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents StatusLabel As ToolStripStatusLabel
    Friend WithEvents Spinner As ToolStripStatusLabel
    Friend WithEvents OpenTool As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ProgressBar1 As ToolStripProgressBar
    Friend WithEvents ProgTimer As Timer
    Friend WithEvents ToolStripStatusLabel2 As ToolStripStatusLabel
    Friend WithEvents statMBPS As ToolStripStatusLabel
    Friend WithEvents AttachGrid As DataGridView
    Friend WithEvents CopyTextTool As ToolStripMenuItem
    Friend WithEvents Panel1 As Panel
    Friend WithEvents ToolStripStatusLabel1 As ToolStripStatusLabel
    Friend WithEvents cmdCancel As ToolStripDropDownButton
    Friend WithEvents RenameStripMenuItem As ToolStripMenuItem
    Friend WithEvents AllowDragCheckBox As CheckBox
    Friend WithEvents DeviceGroup As GroupBox
    Friend WithEvents Label5 As Label
    Friend WithEvents txtDeviceDescription As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents txtSerial As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtAssetTag As TextBox
    Friend WithEvents Panel2 As Panel
    Friend WithEvents SaveToMenuItem As ToolStripMenuItem
    Friend WithEvents Label4 As Label
    Friend WithEvents ReqNumberTextBox As TextBox
    Friend WithEvents AttachContainer As SplitContainer
    Friend WithEvents FolderListView As ListView
    Friend WithEvents ColumnHeader1 As ColumnHeader
    Friend WithEvents imageList1 As ImageList
    Friend WithEvents NewFolderMenuItem As ToolStripMenuItem
End Class
