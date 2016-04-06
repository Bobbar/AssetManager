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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(View))
        Me.Label14 = New System.Windows.Forms.Label()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.cmbEquipType_View = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtReplacementYear_View = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.dtPurchaseDate_View = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbLocation_View = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtDescription_View = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtCurUser_View = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSerial_View = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtAssetTag_View = New System.Windows.Forms.TextBox()
        Me.DataGridHistory = New System.Windows.Forms.DataGridView()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.ActionsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.EditToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.pnlViewControls = New System.Windows.Forms.Panel()
        Me.cmdUpdate = New System.Windows.Forms.Button()
        CType(Me.DataGridHistory, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.MenuStrip1.SuspendLayout()
        Me.pnlViewControls.SuspendLayout()
        Me.SuspendLayout()
        '
        'Label14
        '
        Me.Label14.AutoSize = True
        Me.Label14.Location = New System.Drawing.Point(26, 250)
        Me.Label14.Name = "Label14"
        Me.Label14.Size = New System.Drawing.Size(39, 13)
        Me.Label14.TabIndex = 35
        Me.Label14.Text = "History"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Location = New System.Drawing.Point(660, 26)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(87, 13)
        Me.Label13.TabIndex = 34
        Me.Label13.Text = "Equipment Type:"
        '
        'cmbEquipType_View
        '
        Me.cmbEquipType_View.Enabled = False
        Me.cmbEquipType_View.FormattingEnabled = True
        Me.cmbEquipType_View.Location = New System.Drawing.Point(663, 42)
        Me.cmbEquipType_View.Name = "cmbEquipType_View"
        Me.cmbEquipType_View.Size = New System.Drawing.Size(156, 21)
        Me.cmbEquipType_View.TabIndex = 33
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(436, 129)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(98, 13)
        Me.Label7.TabIndex = 32
        Me.Label7.Text = "Replacement Year:"
        '
        'txtReplacementYear_View
        '
        Me.txtReplacementYear_View.Enabled = False
        Me.txtReplacementYear_View.Location = New System.Drawing.Point(449, 145)
        Me.txtReplacementYear_View.Name = "txtReplacementYear_View"
        Me.txtReplacementYear_View.Size = New System.Drawing.Size(66, 20)
        Me.txtReplacementYear_View.TabIndex = 31
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(229, 129)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(81, 13)
        Me.Label6.TabIndex = 30
        Me.Label6.Text = "Purchase Date:"
        '
        'dtPurchaseDate_View
        '
        Me.dtPurchaseDate_View.CustomFormat = "yyyy-MM-dd"
        Me.dtPurchaseDate_View.Enabled = False
        Me.dtPurchaseDate_View.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtPurchaseDate_View.Location = New System.Drawing.Point(232, 145)
        Me.dtPurchaseDate_View.Name = "dtPurchaseDate_View"
        Me.dtPurchaseDate_View.Size = New System.Drawing.Size(182, 20)
        Me.dtPurchaseDate_View.TabIndex = 29
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(229, 79)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(51, 13)
        Me.Label5.TabIndex = 28
        Me.Label5.Text = "Location:"
        '
        'cmbLocation_View
        '
        Me.cmbLocation_View.Enabled = False
        Me.cmbLocation_View.FormattingEnabled = True
        Me.cmbLocation_View.Location = New System.Drawing.Point(230, 95)
        Me.cmbLocation_View.Name = "cmbLocation_View"
        Me.cmbLocation_View.Size = New System.Drawing.Size(168, 21)
        Me.cmbLocation_View.TabIndex = 27
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(229, 29)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(63, 13)
        Me.Label4.TabIndex = 26
        Me.Label4.Text = "Description:"
        '
        'txtDescription_View
        '
        Me.txtDescription_View.Enabled = False
        Me.txtDescription_View.Location = New System.Drawing.Point(230, 45)
        Me.txtDescription_View.Name = "txtDescription_View"
        Me.txtDescription_View.Size = New System.Drawing.Size(304, 20)
        Me.txtDescription_View.TabIndex = 25
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(26, 132)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(69, 13)
        Me.Label3.TabIndex = 24
        Me.Label3.Text = "Current User:"
        '
        'txtCurUser_View
        '
        Me.txtCurUser_View.Enabled = False
        Me.txtCurUser_View.Location = New System.Drawing.Point(29, 148)
        Me.txtCurUser_View.Name = "txtCurUser_View"
        Me.txtCurUser_View.Size = New System.Drawing.Size(132, 20)
        Me.txtCurUser_View.TabIndex = 23
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(26, 80)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(36, 13)
        Me.Label2.TabIndex = 22
        Me.Label2.Text = "Serial:"
        '
        'txtSerial_View
        '
        Me.txtSerial_View.Enabled = False
        Me.txtSerial_View.Location = New System.Drawing.Point(29, 96)
        Me.txtSerial_View.Name = "txtSerial_View"
        Me.txtSerial_View.Size = New System.Drawing.Size(133, 20)
        Me.txtSerial_View.TabIndex = 21
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(26, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(58, 13)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Asset Tag:"
        '
        'txtAssetTag_View
        '
        Me.txtAssetTag_View.Enabled = False
        Me.txtAssetTag_View.Location = New System.Drawing.Point(29, 45)
        Me.txtAssetTag_View.Name = "txtAssetTag_View"
        Me.txtAssetTag_View.Size = New System.Drawing.Size(134, 20)
        Me.txtAssetTag_View.TabIndex = 19
        '
        'DataGridHistory
        '
        Me.DataGridHistory.AllowUserToAddRows = False
        Me.DataGridHistory.AllowUserToDeleteRows = False
        Me.DataGridHistory.AllowUserToResizeRows = False
        Me.DataGridHistory.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        Me.DataGridHistory.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.DataGridHistory.Location = New System.Drawing.Point(30, 266)
        Me.DataGridHistory.MultiSelect = False
        Me.DataGridHistory.Name = "DataGridHistory"
        Me.DataGridHistory.ReadOnly = True
        Me.DataGridHistory.ShowEditingIcon = False
        Me.DataGridHistory.Size = New System.Drawing.Size(840, 237)
        Me.DataGridHistory.TabIndex = 18
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ActionsToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(983, 24)
        Me.MenuStrip1.TabIndex = 36
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'ActionsToolStripMenuItem
        '
        Me.ActionsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.EditToolStripMenuItem})
        Me.ActionsToolStripMenuItem.Name = "ActionsToolStripMenuItem"
        Me.ActionsToolStripMenuItem.Size = New System.Drawing.Size(59, 20)
        Me.ActionsToolStripMenuItem.Text = "Actions"
        '
        'EditToolStripMenuItem
        '
        Me.EditToolStripMenuItem.Name = "EditToolStripMenuItem"
        Me.EditToolStripMenuItem.Size = New System.Drawing.Size(152, 22)
        Me.EditToolStripMenuItem.Text = "Edit"
        '
        'pnlViewControls
        '
        Me.pnlViewControls.Controls.Add(Me.cmdUpdate)
        Me.pnlViewControls.Controls.Add(Me.Label13)
        Me.pnlViewControls.Controls.Add(Me.cmbEquipType_View)
        Me.pnlViewControls.Controls.Add(Me.Label7)
        Me.pnlViewControls.Controls.Add(Me.txtReplacementYear_View)
        Me.pnlViewControls.Controls.Add(Me.Label6)
        Me.pnlViewControls.Controls.Add(Me.dtPurchaseDate_View)
        Me.pnlViewControls.Controls.Add(Me.Label5)
        Me.pnlViewControls.Controls.Add(Me.cmbLocation_View)
        Me.pnlViewControls.Controls.Add(Me.Label4)
        Me.pnlViewControls.Controls.Add(Me.txtDescription_View)
        Me.pnlViewControls.Controls.Add(Me.Label3)
        Me.pnlViewControls.Controls.Add(Me.txtCurUser_View)
        Me.pnlViewControls.Controls.Add(Me.Label2)
        Me.pnlViewControls.Controls.Add(Me.txtSerial_View)
        Me.pnlViewControls.Controls.Add(Me.Label1)
        Me.pnlViewControls.Controls.Add(Me.txtAssetTag_View)
        Me.pnlViewControls.Location = New System.Drawing.Point(29, 27)
        Me.pnlViewControls.Name = "pnlViewControls"
        Me.pnlViewControls.Size = New System.Drawing.Size(841, 199)
        Me.pnlViewControls.TabIndex = 37
        '
        'cmdUpdate
        '
        Me.cmdUpdate.Location = New System.Drawing.Point(688, 118)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.Size = New System.Drawing.Size(119, 47)
        Me.cmdUpdate.TabIndex = 35
        Me.cmdUpdate.Text = "Update"
        Me.cmdUpdate.UseVisualStyleBackColor = True
        '
        'View
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(983, 541)
        Me.Controls.Add(Me.pnlViewControls)
        Me.Controls.Add(Me.Label14)
        Me.Controls.Add(Me.DataGridHistory)
        Me.Controls.Add(Me.MenuStrip1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "View"
        Me.Text = "View"
        CType(Me.DataGridHistory, System.ComponentModel.ISupportInitialize).EndInit()
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.pnlViewControls.ResumeLayout(False)
        Me.pnlViewControls.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents Label14 As Label
    Friend WithEvents Label13 As Label
    Friend WithEvents cmbEquipType_View As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtReplacementYear_View As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents dtPurchaseDate_View As DateTimePicker
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbLocation_View As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtDescription_View As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtCurUser_View As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtSerial_View As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtAssetTag_View As TextBox
    Friend WithEvents DataGridHistory As DataGridView
    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents ActionsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents EditToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents pnlViewControls As Panel
    Friend WithEvents cmdUpdate As Button
End Class
