<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class View_Munis
    Inherits System.Windows.Forms.Form
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(View_Munis))
        Me.DataGridMunis_Inventory = New System.Windows.Forms.DataGridView()
        Me.pnlFixedAsset = New System.Windows.Forms.Panel()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSerial = New System.Windows.Forms.TextBox()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.pnlRequisition = New System.Windows.Forms.Panel()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.DataGridMunis_Requisition = New System.Windows.Forms.DataGridView()
        Me.pnlMaster = New System.Windows.Forms.Panel()
        CType(Me.DataGridMunis_Inventory, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlFixedAsset.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.pnlRequisition.SuspendLayout()
        CType(Me.DataGridMunis_Requisition, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlMaster.SuspendLayout()
        Me.SuspendLayout()
        '
        'DataGridMunis_Inventory
        '
        Me.DataGridMunis_Inventory.AllowUserToAddRows = False
        Me.DataGridMunis_Inventory.AllowUserToDeleteRows = False
        Me.DataGridMunis_Inventory.AllowUserToResizeRows = False
        Me.DataGridMunis_Inventory.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridMunis_Inventory.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
        Me.DataGridMunis_Inventory.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.DataGridMunis_Inventory.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.DataGridMunis_Inventory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridMunis_Inventory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.DataGridMunis_Inventory.Location = New System.Drawing.Point(3, 19)
        Me.DataGridMunis_Inventory.Name = "DataGridMunis_Inventory"
        Me.DataGridMunis_Inventory.ReadOnly = True
        Me.DataGridMunis_Inventory.RowHeadersVisible = False
        Me.DataGridMunis_Inventory.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.DataGridMunis_Inventory.ShowCellToolTips = False
        Me.DataGridMunis_Inventory.ShowEditingIcon = False
        Me.DataGridMunis_Inventory.Size = New System.Drawing.Size(842, 134)
        Me.DataGridMunis_Inventory.TabIndex = 41
        '
        'pnlFixedAsset
        '
        Me.pnlFixedAsset.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlFixedAsset.Controls.Add(Me.DataGridMunis_Inventory)
        Me.pnlFixedAsset.Controls.Add(Me.Label2)
        Me.pnlFixedAsset.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlFixedAsset.Location = New System.Drawing.Point(0, 12)
        Me.pnlFixedAsset.Name = "pnlFixedAsset"
        Me.pnlFixedAsset.Size = New System.Drawing.Size(848, 159)
        Me.pnlFixedAsset.TabIndex = 42
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(0, 0)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(122, 16)
        Me.Label2.TabIndex = 47
        Me.Label2.Text = "Fixed Asset Info:"
        '
        'txtSerial
        '
        Me.txtSerial.Location = New System.Drawing.Point(28, 19)
        Me.txtSerial.Name = "txtSerial"
        Me.txtSerial.Size = New System.Drawing.Size(125, 20)
        Me.txtSerial.TabIndex = 43
        Me.txtSerial.Visible = False
        '
        'cmdSearch
        '
        Me.cmdSearch.Location = New System.Drawing.Point(28, 45)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(129, 24)
        Me.cmdSearch.TabIndex = 44
        Me.cmdSearch.Text = "Search"
        Me.cmdSearch.UseVisualStyleBackColor = True
        Me.cmdSearch.Visible = False
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(25, 3)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 13)
        Me.Label1.TabIndex = 45
        Me.Label1.Text = "Serial"
        Me.Label1.Visible = False
        '
        'Panel2
        '
        Me.Panel2.Controls.Add(Me.cmdSearch)
        Me.Panel2.Controls.Add(Me.Label1)
        Me.Panel2.Controls.Add(Me.txtSerial)
        Me.Panel2.Location = New System.Drawing.Point(969, 12)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(227, 72)
        Me.Panel2.TabIndex = 46
        Me.Panel2.Visible = False
        '
        'pnlRequisition
        '
        Me.pnlRequisition.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlRequisition.Controls.Add(Me.Label3)
        Me.pnlRequisition.Controls.Add(Me.DataGridMunis_Requisition)
        Me.pnlRequisition.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlRequisition.Location = New System.Drawing.Point(0, 177)
        Me.pnlRequisition.Name = "pnlRequisition"
        Me.pnlRequisition.Size = New System.Drawing.Size(851, 394)
        Me.pnlRequisition.TabIndex = 48
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(0, 0)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(89, 16)
        Me.Label3.TabIndex = 49
        Me.Label3.Text = "MUNIS Info:"
        '
        'DataGridMunis_Requisition
        '
        Me.DataGridMunis_Requisition.AllowUserToAddRows = False
        Me.DataGridMunis_Requisition.AllowUserToDeleteRows = False
        Me.DataGridMunis_Requisition.AllowUserToResizeRows = False
        Me.DataGridMunis_Requisition.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.DataGridMunis_Requisition.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
        Me.DataGridMunis_Requisition.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.DataGridMunis_Requisition.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.DataGridMunis_Requisition.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.DataGridMunis_Requisition.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.DataGridMunis_Requisition.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.DataGridMunis_Requisition.EnableHeadersVisualStyles = False
        Me.DataGridMunis_Requisition.Location = New System.Drawing.Point(3, 19)
        Me.DataGridMunis_Requisition.Name = "DataGridMunis_Requisition"
        Me.DataGridMunis_Requisition.ReadOnly = True
        Me.DataGridMunis_Requisition.RowHeadersVisible = False
        Me.DataGridMunis_Requisition.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.DataGridMunis_Requisition.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.DataGridMunis_Requisition.ShowCellToolTips = False
        Me.DataGridMunis_Requisition.ShowEditingIcon = False
        Me.DataGridMunis_Requisition.Size = New System.Drawing.Size(845, 372)
        Me.DataGridMunis_Requisition.TabIndex = 41
        Me.DataGridMunis_Requisition.VirtualMode = True
        '
        'pnlMaster
        '
        Me.pnlMaster.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlMaster.Controls.Add(Me.pnlRequisition)
        Me.pnlMaster.Controls.Add(Me.pnlFixedAsset)
        Me.pnlMaster.Location = New System.Drawing.Point(12, 3)
        Me.pnlMaster.Name = "pnlMaster"
        Me.pnlMaster.Size = New System.Drawing.Size(854, 574)
        Me.pnlMaster.TabIndex = 49
        '
        'View_Munis
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(880, 589)
        Me.Controls.Add(Me.pnlMaster)
        Me.Controls.Add(Me.Panel2)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "View_Munis"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Munis Info"
        CType(Me.DataGridMunis_Inventory, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlFixedAsset.ResumeLayout(False)
        Me.pnlFixedAsset.PerformLayout()
        Me.Panel2.ResumeLayout(False)
        Me.Panel2.PerformLayout()
        Me.pnlRequisition.ResumeLayout(False)
        Me.pnlRequisition.PerformLayout()
        CType(Me.DataGridMunis_Requisition, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlMaster.ResumeLayout(False)
        Me.ResumeLayout(False)
    End Sub
    Friend WithEvents DataGridMunis_Inventory As DataGridView
    Friend WithEvents pnlFixedAsset As Panel
    Friend WithEvents txtSerial As TextBox
    Friend WithEvents cmdSearch As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents Panel2 As Panel
    Friend WithEvents Label2 As Label
    Friend WithEvents pnlRequisition As Panel
    Friend WithEvents DataGridMunis_Requisition As DataGridView
    Friend WithEvents Label3 As Label
    Friend WithEvents pnlMaster As Panel
End Class
