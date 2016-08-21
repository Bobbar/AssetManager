<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmUserManager
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmUserManager))
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.UserGrid = New System.Windows.Forms.DataGridView()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pnlModule = New System.Windows.Forms.Panel()
        Me.clbModules = New System.Windows.Forms.CheckedListBox()
        Me.cmdUpdate = New System.Windows.Forms.Button()
        Me.lblAccessValue = New System.Windows.Forms.Label()
        Me.Panel1.SuspendLayout()
        CType(Me.UserGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.pnlModule.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.UserGrid)
        Me.Panel1.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(12, 195)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(611, 222)
        Me.Panel1.TabIndex = 0
        '
        'UserGrid
        '
        Me.UserGrid.AllowUserToAddRows = False
        Me.UserGrid.AllowUserToDeleteRows = False
        Me.UserGrid.AllowUserToResizeRows = False
        Me.UserGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UserGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.UserGrid.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.UserGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.UserGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.UserGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(39, Byte), Integer))
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.UserGrid.DefaultCellStyle = DataGridViewCellStyle1
        Me.UserGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.UserGrid.Location = New System.Drawing.Point(3, 3)
        Me.UserGrid.MultiSelect = False
        Me.UserGrid.Name = "UserGrid"
        Me.UserGrid.ReadOnly = True
        Me.UserGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle2.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle2.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle2.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.UserGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle2
        Me.UserGrid.RowHeadersVisible = False
        Me.UserGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.UserGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.UserGrid.ShowCellErrors = False
        Me.UserGrid.ShowCellToolTips = False
        Me.UserGrid.ShowEditingIcon = False
        Me.UserGrid.Size = New System.Drawing.Size(605, 216)
        Me.UserGrid.TabIndex = 18
        Me.UserGrid.VirtualMode = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pnlModule)
        Me.GroupBox1.Location = New System.Drawing.Point(15, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(377, 177)
        Me.GroupBox1.TabIndex = 1
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Module Access"
        '
        'pnlModule
        '
        Me.pnlModule.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlModule.Controls.Add(Me.clbModules)
        Me.pnlModule.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlModule.Location = New System.Drawing.Point(10, 17)
        Me.pnlModule.Name = "pnlModule"
        Me.pnlModule.Size = New System.Drawing.Size(356, 153)
        Me.pnlModule.TabIndex = 0
        '
        'clbModules
        '
        Me.clbModules.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.clbModules.CheckOnClick = True
        Me.clbModules.FormattingEnabled = True
        Me.clbModules.Location = New System.Drawing.Point(3, 3)
        Me.clbModules.MultiColumn = True
        Me.clbModules.Name = "clbModules"
        Me.clbModules.Size = New System.Drawing.Size(350, 140)
        Me.clbModules.Sorted = True
        Me.clbModules.TabIndex = 0
        '
        'cmdUpdate
        '
        Me.cmdUpdate.Location = New System.Drawing.Point(432, 61)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.Size = New System.Drawing.Size(135, 47)
        Me.cmdUpdate.TabIndex = 2
        Me.cmdUpdate.Text = "Update"
        Me.cmdUpdate.UseVisualStyleBackColor = True
        '
        'lblAccessValue
        '
        Me.lblAccessValue.AutoSize = True
        Me.lblAccessValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccessValue.Location = New System.Drawing.Point(412, 141)
        Me.lblAccessValue.Name = "lblAccessValue"
        Me.lblAccessValue.Size = New System.Drawing.Size(142, 13)
        Me.lblAccessValue.TabIndex = 3
        Me.lblAccessValue.Text = "Selected Access Value:"
        Me.lblAccessValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'frmUserManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(635, 429)
        Me.Controls.Add(Me.lblAccessValue)
        Me.Controls.Add(Me.cmdUpdate)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Panel1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "frmUserManager"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "User  Manager"
        Me.Panel1.ResumeLayout(False)
        CType(Me.UserGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.pnlModule.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()
    End Sub
    Friend WithEvents Panel1 As Panel
    Friend WithEvents UserGrid As DataGridView
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents pnlModule As Panel
    Friend WithEvents clbModules As CheckedListBox
    Friend WithEvents cmdUpdate As Button
    Friend WithEvents lblAccessValue As Label
End Class
