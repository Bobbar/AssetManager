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
        Dim DataGridViewCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Dim DataGridViewCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.UserGrid = New System.Windows.Forms.DataGridView()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.pnlModule = New System.Windows.Forms.Panel()
        Me.clbModules = New System.Windows.Forms.CheckedListBox()
        Me.lblAccessValue = New System.Windows.Forms.Label()
        Me.cmdUpdate = New System.Windows.Forms.Button()
        Me.cmdRefresh = New System.Windows.Forms.Button()
        Me.Panel2 = New System.Windows.Forms.Panel()
        Me.Panel1.SuspendLayout()
        CType(Me.UserGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.pnlModule.SuspendLayout()
        Me.Panel2.SuspendLayout()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.Controls.Add(Me.UserGrid)
        Me.Panel1.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel1.Location = New System.Drawing.Point(12, 222)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(647, 219)
        Me.Panel1.TabIndex = 0
        '
        'UserGrid
        '
        Me.UserGrid.AllowUserToResizeRows = False
        Me.UserGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.UserGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.UserGrid.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.UserGrid.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.UserGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.UserGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle7.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle7.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle7.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle7.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(152, Byte), Integer), CType(CType(39, Byte), Integer))
        DataGridViewCellStyle7.SelectionForeColor = System.Drawing.Color.Black
        DataGridViewCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.UserGrid.DefaultCellStyle = DataGridViewCellStyle7
        Me.UserGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.UserGrid.Location = New System.Drawing.Point(3, 3)
        Me.UserGrid.MultiSelect = False
        Me.UserGrid.Name = "UserGrid"
        Me.UserGrid.RowHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        DataGridViewCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
        DataGridViewCellStyle8.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        DataGridViewCellStyle8.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle8.ForeColor = System.Drawing.Color.White
        DataGridViewCellStyle8.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle8.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
        Me.UserGrid.RowHeadersDefaultCellStyle = DataGridViewCellStyle8
        Me.UserGrid.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.UserGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.UserGrid.ShowCellErrors = False
        Me.UserGrid.ShowCellToolTips = False
        Me.UserGrid.Size = New System.Drawing.Size(641, 213)
        Me.UserGrid.TabIndex = 18
        Me.UserGrid.VirtualMode = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.pnlModule)
        Me.GroupBox1.Location = New System.Drawing.Point(15, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(433, 204)
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
        Me.pnlModule.Controls.Add(Me.lblAccessValue)
        Me.pnlModule.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlModule.Location = New System.Drawing.Point(10, 17)
        Me.pnlModule.Name = "pnlModule"
        Me.pnlModule.Size = New System.Drawing.Size(412, 181)
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
        Me.clbModules.Size = New System.Drawing.Size(406, 157)
        Me.clbModules.Sorted = True
        Me.clbModules.TabIndex = 0
        '
        'lblAccessValue
        '
        Me.lblAccessValue.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.lblAccessValue.Font = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblAccessValue.Location = New System.Drawing.Point(28, 165)
        Me.lblAccessValue.Name = "lblAccessValue"
        Me.lblAccessValue.Size = New System.Drawing.Size(356, 16)
        Me.lblAccessValue.TabIndex = 3
        Me.lblAccessValue.Text = "Selected Access Value:"
        Me.lblAccessValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmdUpdate
        '
        Me.cmdUpdate.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.cmdUpdate.Location = New System.Drawing.Point(37, 28)
        Me.cmdUpdate.Name = "cmdUpdate"
        Me.cmdUpdate.Size = New System.Drawing.Size(134, 47)
        Me.cmdUpdate.TabIndex = 2
        Me.cmdUpdate.Text = "Commit Changes"
        Me.cmdUpdate.UseVisualStyleBackColor = True
        '
        'cmdRefresh
        '
        Me.cmdRefresh.Anchor = System.Windows.Forms.AnchorStyles.Top
        Me.cmdRefresh.Location = New System.Drawing.Point(37, 106)
        Me.cmdRefresh.Name = "cmdRefresh"
        Me.cmdRefresh.Size = New System.Drawing.Size(134, 23)
        Me.cmdRefresh.TabIndex = 4
        Me.cmdRefresh.Text = "Refresh/Undo"
        Me.cmdRefresh.UseVisualStyleBackColor = True
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.Controls.Add(Me.cmdUpdate)
        Me.Panel2.Controls.Add(Me.cmdRefresh)
        Me.Panel2.Location = New System.Drawing.Point(454, 40)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(205, 162)
        Me.Panel2.TabIndex = 5
        '
        'frmUserManager
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(671, 453)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Controls.Add(Me.Panel1)
        Me.MinimumSize = New System.Drawing.Size(634, 409)
        Me.Name = "frmUserManager"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "User  Manager"
        Me.Panel1.ResumeLayout(False)
        CType(Me.UserGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.pnlModule.ResumeLayout(False)
        Me.Panel2.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Panel1 As Panel
    Friend WithEvents UserGrid As DataGridView
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents pnlModule As Panel
    Friend WithEvents clbModules As CheckedListBox
    Friend WithEvents cmdUpdate As Button
    Friend WithEvents lblAccessValue As Label
    Friend WithEvents cmdRefresh As Button
    Friend WithEvents Panel2 As Panel
End Class
