<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class Form1
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Form1))
        Me.Tabs = New System.Windows.Forms.TabControl()
        Me.TabPage1 = New System.Windows.Forms.TabPage()
        Me.cmbShowAll = New System.Windows.Forms.Button()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.cmbLocation = New System.Windows.Forms.ComboBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtCurUser = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbEquipType = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmdClear = New System.Windows.Forms.Button()
        Me.ResultGrid = New System.Windows.Forms.DataGridView()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.txtAssetTag = New System.Windows.Forms.TextBox()
        Me.txtSerial = New System.Windows.Forms.TextBox()
        Me.TabPage2 = New System.Windows.Forms.TabPage()
        Me.TabPage3 = New System.Windows.Forms.TabPage()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.BlahToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Tabs.SuspendLayout()
        Me.TabPage1.SuspendLayout()
        CType(Me.ResultGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Tabs
        '
        Me.Tabs.Controls.Add(Me.TabPage1)
        Me.Tabs.Controls.Add(Me.TabPage2)
        Me.Tabs.Controls.Add(Me.TabPage3)
        Me.Tabs.Location = New System.Drawing.Point(12, 42)
        Me.Tabs.Name = "Tabs"
        Me.Tabs.SelectedIndex = 0
        Me.Tabs.Size = New System.Drawing.Size(1158, 634)
        Me.Tabs.TabIndex = 0
        '
        'TabPage1
        '
        Me.TabPage1.Controls.Add(Me.cmbShowAll)
        Me.TabPage1.Controls.Add(Me.Label12)
        Me.TabPage1.Controls.Add(Me.cmbLocation)
        Me.TabPage1.Controls.Add(Me.Label11)
        Me.TabPage1.Controls.Add(Me.txtCurUser)
        Me.TabPage1.Controls.Add(Me.Label10)
        Me.TabPage1.Controls.Add(Me.cmbEquipType)
        Me.TabPage1.Controls.Add(Me.Label9)
        Me.TabPage1.Controls.Add(Me.Label8)
        Me.TabPage1.Controls.Add(Me.cmdClear)
        Me.TabPage1.Controls.Add(Me.ResultGrid)
        Me.TabPage1.Controls.Add(Me.cmdSearch)
        Me.TabPage1.Controls.Add(Me.txtAssetTag)
        Me.TabPage1.Controls.Add(Me.txtSerial)
        Me.TabPage1.Location = New System.Drawing.Point(4, 22)
        Me.TabPage1.Name = "TabPage1"
        Me.TabPage1.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage1.Size = New System.Drawing.Size(1150, 608)
        Me.TabPage1.TabIndex = 0
        Me.TabPage1.Text = "Search"
        Me.TabPage1.UseVisualStyleBackColor = True
        '
        'cmbShowAll
        '
        Me.cmbShowAll.Location = New System.Drawing.Point(1021, 10)
        Me.cmbShowAll.Name = "cmbShowAll"
        Me.cmbShowAll.Size = New System.Drawing.Size(109, 31)
        Me.cmbShowAll.TabIndex = 13
        Me.cmbShowAll.Text = "Show All"
        Me.cmbShowAll.UseVisualStyleBackColor = True
        '
        'Label12
        '
        Me.Label12.AutoSize = True
        Me.Label12.Location = New System.Drawing.Point(379, 29)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(51, 13)
        Me.Label12.TabIndex = 12
        Me.Label12.Text = "Location:"
        '
        'cmbLocation
        '
        Me.cmbLocation.FormattingEnabled = True
        Me.cmbLocation.Location = New System.Drawing.Point(382, 48)
        Me.cmbLocation.Name = "cmbLocation"
        Me.cmbLocation.Size = New System.Drawing.Size(182, 21)
        Me.cmbLocation.TabIndex = 11
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(188, 79)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(69, 13)
        Me.Label11.TabIndex = 10
        Me.Label11.Text = "Current User:"
        '
        'txtCurUser
        '
        Me.txtCurUser.Location = New System.Drawing.Point(191, 95)
        Me.txtCurUser.Name = "txtCurUser"
        Me.txtCurUser.Size = New System.Drawing.Size(159, 20)
        Me.txtCurUser.TabIndex = 9
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(188, 31)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(87, 13)
        Me.Label10.TabIndex = 8
        Me.Label10.Text = "Equipment Type:"
        '
        'cmbEquipType
        '
        Me.cmbEquipType.FormattingEnabled = True
        Me.cmbEquipType.Location = New System.Drawing.Point(191, 47)
        Me.cmbEquipType.Name = "cmbEquipType"
        Me.cmbEquipType.Size = New System.Drawing.Size(159, 21)
        Me.cmbEquipType.TabIndex = 7
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(33, 79)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(36, 13)
        Me.Label9.TabIndex = 6
        Me.Label9.Text = "Serial:"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(33, 32)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(58, 13)
        Me.Label8.TabIndex = 5
        Me.Label8.Text = "Asset Tag:"
        '
        'cmdClear
        '
        Me.cmdClear.Location = New System.Drawing.Point(1021, 111)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(110, 32)
        Me.cmdClear.TabIndex = 4
        Me.cmdClear.Text = "Clear"
        Me.cmdClear.UseVisualStyleBackColor = True
        '
        'ResultGrid
        '
        Me.ResultGrid.AllowUserToAddRows = False
        Me.ResultGrid.AllowUserToDeleteRows = False
        Me.ResultGrid.AllowUserToResizeRows = False
        Me.ResultGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.ResultGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.ResultGrid.Location = New System.Drawing.Point(25, 160)
        Me.ResultGrid.MultiSelect = False
        Me.ResultGrid.Name = "ResultGrid"
        Me.ResultGrid.ReadOnly = True
        Me.ResultGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.ResultGrid.ShowEditingIcon = False
        Me.ResultGrid.Size = New System.Drawing.Size(1106, 442)
        Me.ResultGrid.TabIndex = 3
        '
        'cmdSearch
        '
        Me.cmdSearch.Location = New System.Drawing.Point(1021, 47)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(110, 56)
        Me.cmdSearch.TabIndex = 2
        Me.cmdSearch.Text = "Search"
        Me.cmdSearch.UseVisualStyleBackColor = True
        '
        'txtAssetTag
        '
        Me.txtAssetTag.Location = New System.Drawing.Point(36, 48)
        Me.txtAssetTag.Name = "txtAssetTag"
        Me.txtAssetTag.Size = New System.Drawing.Size(117, 20)
        Me.txtAssetTag.TabIndex = 1
        Me.txtAssetTag.Text = "%ASSET_TAG%"
        '
        'txtSerial
        '
        Me.txtSerial.Location = New System.Drawing.Point(36, 95)
        Me.txtSerial.Name = "txtSerial"
        Me.txtSerial.Size = New System.Drawing.Size(117, 20)
        Me.txtSerial.TabIndex = 0
        Me.txtSerial.Text = "%SERIAL%"
        '
        'TabPage2
        '
        Me.TabPage2.Location = New System.Drawing.Point(4, 22)
        Me.TabPage2.Name = "TabPage2"
        Me.TabPage2.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage2.Size = New System.Drawing.Size(1150, 608)
        Me.TabPage2.TabIndex = 1
        Me.TabPage2.Text = "View"
        Me.TabPage2.UseVisualStyleBackColor = True
        '
        'TabPage3
        '
        Me.TabPage3.Location = New System.Drawing.Point(4, 22)
        Me.TabPage3.Name = "TabPage3"
        Me.TabPage3.Padding = New System.Windows.Forms.Padding(3)
        Me.TabPage3.Size = New System.Drawing.Size(1150, 608)
        Me.TabPage3.TabIndex = 2
        Me.TabPage3.Text = "Manage"
        Me.TabPage3.UseVisualStyleBackColor = True
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(1182, 24)
        Me.MenuStrip1.TabIndex = 3
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.BlahToolStripMenuItem})
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.EditToolStripMenuItem.Text = "Actions"
        '
        'BlahToolStripMenuItem
        '
        Me.BlahToolStripMenuItem.Name = "BlahToolStripMenuItem"
        Me.BlahToolStripMenuItem.Size = New System.Drawing.Size(98, 22)
        Me.BlahToolStripMenuItem.Text = "New"
        '
        'Form1
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1182, 688)
        Me.Controls.Add(Me.Tabs)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "Form1"
        Me.Text = "Asset Manager"
        Me.Tabs.ResumeLayout(False)
        Me.TabPage1.ResumeLayout(False)
        Me.TabPage1.PerformLayout()
        CType(Me.ResultGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Tabs As TabControl
    Friend WithEvents TabPage1 As TabPage
    Friend WithEvents TabPage2 As TabPage
    Friend WithEvents TabPage3 As TabPage
    Friend WithEvents cmdSearch As Button
    Friend WithEvents txtAssetTag As TextBox
    Friend WithEvents txtSerial As TextBox
    Friend WithEvents ResultGrid As DataGridView
    Friend WithEvents cmdClear As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents Label8 As Label
    Friend WithEvents Label10 As Label
    Friend WithEvents cmbEquipType As ComboBox
    Friend WithEvents Label11 As Label
    Friend WithEvents txtCurUser As TextBox
    Friend WithEvents Label12 As Label
    Friend WithEvents cmbLocation As ComboBox
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents BlahToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents cmbShowAll As Button
End Class
