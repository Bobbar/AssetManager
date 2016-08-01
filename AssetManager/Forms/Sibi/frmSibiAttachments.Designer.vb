<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmSibiAttachments
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmSibiAttachments))
        Me.cmdUpload = New System.Windows.Forms.Button()
        Me.RightClickMenu = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.OpenTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.CopyTextTool = New System.Windows.Forms.ToolStripMenuItem()
        Me.MoveStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmbMoveFolder = New System.Windows.Forms.ToolStripComboBox()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.DeleteAttachmentToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.cmdDelete = New System.Windows.Forms.Button()
        Me.cmdOpen = New System.Windows.Forms.Button()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbFolder = New System.Windows.Forms.ComboBox()
        Me.AttachGrid = New System.Windows.Forms.DataGridView()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtRequestNum = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtNeedBy = New System.Windows.Forms.TextBox()
        Me.StatusStrip1 = New System.Windows.Forms.StatusStrip()
        Me.StatusLabel = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ProgressBar1 = New System.Windows.Forms.ToolStripProgressBar()
        Me.ToolStripStatusLabel2 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.Spinner = New System.Windows.Forms.ToolStripStatusLabel()
        Me.statMBPS = New System.Windows.Forms.ToolStripStatusLabel()
        Me.ToolStripStatusLabel1 = New System.Windows.Forms.ToolStripStatusLabel()
        Me.cmdCancel = New System.Windows.Forms.ToolStripDropDownButton()
        Me.UploadWorker = New System.ComponentModel.BackgroundWorker()
        Me.DownloadWorker = New System.ComponentModel.BackgroundWorker()
        Me.ProgTimer = New System.Windows.Forms.Timer(Me.components)
        Me.RenameStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RightClickMenu.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.Panel1.SuspendLayout()
        CType(Me.AttachGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox2.SuspendLayout()
        Me.StatusStrip1.SuspendLayout()
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
        Me.RightClickMenu.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.OpenTool, Me.CopyTextTool, Me.MoveStripMenuItem, Me.RenameStripMenuItem, Me.ToolStripSeparator1, Me.DeleteAttachmentToolStripMenuItem})
        Me.RightClickMenu.Name = "RightClickMenu"
        Me.RightClickMenu.Size = New System.Drawing.Size(174, 142)
        '
        'OpenTool
        '
        Me.OpenTool.Name = "OpenTool"
        Me.OpenTool.Size = New System.Drawing.Size(173, 22)
        Me.OpenTool.Text = "Open"
        '
        'CopyTextTool
        '
        Me.CopyTextTool.Name = "CopyTextTool"
        Me.CopyTextTool.Size = New System.Drawing.Size(173, 22)
        Me.CopyTextTool.Text = "Copy Text"
        '
        'MoveStripMenuItem
        '
        Me.MoveStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.cmbMoveFolder})
        Me.MoveStripMenuItem.Name = "MoveStripMenuItem"
        Me.MoveStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.MoveStripMenuItem.Text = "Move"
        '
        'cmbMoveFolder
        '
        Me.cmbMoveFolder.Name = "cmbMoveFolder"
        Me.cmbMoveFolder.Size = New System.Drawing.Size(121, 23)
        Me.cmbMoveFolder.Text = "Select a folder"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(170, 6)
        '
        'DeleteAttachmentToolStripMenuItem
        '
        Me.DeleteAttachmentToolStripMenuItem.Name = "DeleteAttachmentToolStripMenuItem"
        Me.DeleteAttachmentToolStripMenuItem.Size = New System.Drawing.Size(173, 22)
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
        Me.GroupBox1.Size = New System.Drawing.Size(913, 409)
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
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.cmbFolder)
        Me.Panel1.Controls.Add(Me.AttachGrid)
        Me.Panel1.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(150, 21)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(757, 382)
        Me.Panel1.TabIndex = 19
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(152, 8)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(49, 15)
        Me.Label4.TabIndex = 20
        Me.Label4.Text = "Folder"
        '
        'cmbFolder
        '
        Me.cmbFolder.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbFolder.FormattingEnabled = True
        Me.cmbFolder.Items.AddRange(New Object() {"Quotes", "POs", "Invoices"})
        Me.cmbFolder.Location = New System.Drawing.Point(3, 3)
        Me.cmbFolder.Name = "cmbFolder"
        Me.cmbFolder.Size = New System.Drawing.Size(144, 23)
        Me.cmbFolder.TabIndex = 19
        '
        'AttachGrid
        '
        Me.AttachGrid.AllowUserToAddRows = False
        Me.AttachGrid.AllowUserToDeleteRows = False
        Me.AttachGrid.AllowUserToResizeRows = False
        Me.AttachGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AttachGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.AttachGrid.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.AttachGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
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
        Me.AttachGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.AttachGrid.Location = New System.Drawing.Point(3, 32)
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
        Me.AttachGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.AttachGrid.ShowCellErrors = False
        Me.AttachGrid.ShowCellToolTips = False
        Me.AttachGrid.ShowEditingIcon = False
        Me.AttachGrid.Size = New System.Drawing.Size(749, 345)
        Me.AttachGrid.TabIndex = 18
        Me.AttachGrid.VirtualMode = True
        '
        'GroupBox2
        '
        Me.GroupBox2.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.txtDescription)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.txtRequestNum)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.txtNeedBy)
        Me.GroupBox2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox2.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(633, 95)
        Me.GroupBox2.TabIndex = 7
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Request Info"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(24, 29)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Description"
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
        Me.Label2.Location = New System.Drawing.Point(321, 29)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(69, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Request #"
        '
        'txtRequestNum
        '
        Me.txtRequestNum.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtRequestNum.Location = New System.Drawing.Point(320, 49)
        Me.txtRequestNum.Name = "txtRequestNum"
        Me.txtRequestNum.ReadOnly = True
        Me.txtRequestNum.Size = New System.Drawing.Size(105, 25)
        Me.txtRequestNum.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(460, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(61, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Need By"
        '
        'txtNeedBy
        '
        Me.txtNeedBy.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNeedBy.Location = New System.Drawing.Point(459, 49)
        Me.txtNeedBy.Name = "txtNeedBy"
        Me.txtNeedBy.ReadOnly = True
        Me.txtNeedBy.Size = New System.Drawing.Size(105, 25)
        Me.txtNeedBy.TabIndex = 0
        '
        'StatusStrip1
        '
        Me.StatusStrip1.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.StatusStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.StatusLabel, Me.ProgressBar1, Me.ToolStripStatusLabel2, Me.Spinner, Me.statMBPS, Me.ToolStripStatusLabel1, Me.cmdCancel})
        Me.StatusStrip1.Location = New System.Drawing.Point(0, 525)
        Me.StatusStrip1.Name = "StatusStrip1"
        Me.StatusStrip1.Size = New System.Drawing.Size(937, 22)
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
        Me.Spinner.Image = Global.AssetManager.My.Resources.Resources.loading
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
        Me.ToolStripStatusLabel1.Size = New System.Drawing.Size(816, 17)
        Me.ToolStripStatusLabel1.Spring = True
        '
        'cmdCancel
        '
        Me.cmdCancel.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancel.Image = Global.AssetManager.My.Resources.Resources.close_delete_cancel_del_ui_round_512
        Me.cmdCancel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.ShowDropDownArrow = False
        Me.cmdCancel.Size = New System.Drawing.Size(63, 20)
        Me.cmdCancel.Text = "Cancel"
        Me.cmdCancel.Visible = False
        '
        'UploadWorker
        '
        Me.UploadWorker.WorkerReportsProgress = True
        Me.UploadWorker.WorkerSupportsCancellation = True
        '
        'DownloadWorker
        '
        Me.DownloadWorker.WorkerReportsProgress = True
        Me.DownloadWorker.WorkerSupportsCancellation = True
        '
        'ProgTimer
        '
        Me.ProgTimer.Interval = 50
        '
        'RenameStripMenuItem
        '
        Me.RenameStripMenuItem.Name = "RenameStripMenuItem"
        Me.RenameStripMenuItem.Size = New System.Drawing.Size(173, 22)
        Me.RenameStripMenuItem.Text = "Rename"
        '
        'frmSibiAttachments
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(937, 547)
        Me.Controls.Add(Me.StatusStrip1)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(673, 585)
        Me.Name = "frmSibiAttachments"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sibi Attachments"
        Me.RightClickMenu.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.AttachGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.StatusStrip1.ResumeLayout(False)
        Me.StatusStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents cmdUpload As Button
    Friend WithEvents RightClickMenu As ContextMenuStrip
    Friend WithEvents DeleteAttachmentToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents cmdDelete As Button
    Friend WithEvents cmdOpen As Button
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtRequestNum As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtNeedBy As TextBox
    Friend WithEvents StatusStrip1 As StatusStrip
    Friend WithEvents StatusLabel As ToolStripStatusLabel
    Friend WithEvents Spinner As ToolStripStatusLabel
    Friend WithEvents UploadWorker As System.ComponentModel.BackgroundWorker
    Friend WithEvents DownloadWorker As System.ComponentModel.BackgroundWorker
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
    Friend WithEvents cmbFolder As ComboBox
    Friend WithEvents MoveStripMenuItem As ToolStripMenuItem
    Friend WithEvents cmbMoveFolder As ToolStripComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents RenameStripMenuItem As ToolStripMenuItem
End Class
