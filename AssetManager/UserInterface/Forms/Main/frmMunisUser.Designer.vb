<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMunisUser
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
        Me.MunisResults = New System.Windows.Forms.DataGridView()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.lblSelectedEmp = New System.Windows.Forms.Label()
        Me.cmdAccept = New System.Windows.Forms.Button()
        Me.pnlSearch = New System.Windows.Forms.GroupBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdSearch = New System.Windows.Forms.Button()
        Me.txtSearchName = New System.Windows.Forms.TextBox()
        CType(Me.MunisResults, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox1.SuspendLayout()
        Me.pnlSearch.SuspendLayout()
        Me.SuspendLayout()
        '
        'MunisResults
        '
        Me.MunisResults.AllowUserToAddRows = False
        Me.MunisResults.AllowUserToDeleteRows = False
        Me.MunisResults.AllowUserToResizeRows = False
        Me.MunisResults.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.MunisResults.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.DisplayedCells
        Me.MunisResults.BackgroundColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.MunisResults.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.MunisResults.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.[Single]
        Me.MunisResults.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        Me.MunisResults.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.MunisResults.EnableHeadersVisualStyles = False
        Me.MunisResults.Location = New System.Drawing.Point(6, 19)
        Me.MunisResults.Name = "MunisResults"
        Me.MunisResults.ReadOnly = True
        Me.MunisResults.RowHeadersVisible = False
        Me.MunisResults.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing
        Me.MunisResults.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
        Me.MunisResults.ShowCellToolTips = False
        Me.MunisResults.ShowEditingIcon = False
        Me.MunisResults.Size = New System.Drawing.Size(672, 240)
        Me.MunisResults.TabIndex = 42
        Me.MunisResults.VirtualMode = True
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.lblSelectedEmp)
        Me.GroupBox1.Controls.Add(Me.cmdAccept)
        Me.GroupBox1.Controls.Add(Me.MunisResults)
        Me.GroupBox1.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox1.Location = New System.Drawing.Point(12, 103)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(686, 360)
        Me.GroupBox1.TabIndex = 43
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Results"
        '
        'lblSelectedEmp
        '
        Me.lblSelectedEmp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblSelectedEmp.Location = New System.Drawing.Point(6, 277)
        Me.lblSelectedEmp.Name = "lblSelectedEmp"
        Me.lblSelectedEmp.Size = New System.Drawing.Size(672, 18)
        Me.lblSelectedEmp.TabIndex = 44
        Me.lblSelectedEmp.Text = "Selected Emp:"
        Me.lblSelectedEmp.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'cmdAccept
        '
        Me.cmdAccept.Anchor = System.Windows.Forms.AnchorStyles.Bottom
        Me.cmdAccept.Location = New System.Drawing.Point(287, 310)
        Me.cmdAccept.Name = "cmdAccept"
        Me.cmdAccept.Size = New System.Drawing.Size(92, 29)
        Me.cmdAccept.TabIndex = 43
        Me.cmdAccept.Text = "Accept"
        Me.cmdAccept.UseVisualStyleBackColor = True
        '
        'pnlSearch
        '
        Me.pnlSearch.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlSearch.Controls.Add(Me.Label1)
        Me.pnlSearch.Controls.Add(Me.cmdSearch)
        Me.pnlSearch.Controls.Add(Me.txtSearchName)
        Me.pnlSearch.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlSearch.Location = New System.Drawing.Point(12, 12)
        Me.pnlSearch.Name = "pnlSearch"
        Me.pnlSearch.Size = New System.Drawing.Size(686, 85)
        Me.pnlSearch.TabIndex = 44
        Me.pnlSearch.TabStop = False
        Me.pnlSearch.Text = "Search"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(15, 32)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(140, 14)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "First or Last Name:"
        '
        'cmdSearch
        '
        Me.cmdSearch.Location = New System.Drawing.Point(201, 45)
        Me.cmdSearch.Name = "cmdSearch"
        Me.cmdSearch.Size = New System.Drawing.Size(131, 26)
        Me.cmdSearch.TabIndex = 1
        Me.cmdSearch.Text = "Search"
        Me.cmdSearch.UseVisualStyleBackColor = True
        '
        'txtSearchName
        '
        Me.txtSearchName.Location = New System.Drawing.Point(18, 48)
        Me.txtSearchName.Name = "txtSearchName"
        Me.txtSearchName.Size = New System.Drawing.Size(157, 22)
        Me.txtSearchName.TabIndex = 0
        '
        'frmMunisUser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(713, 477)
        Me.Controls.Add(Me.pnlSearch)
        Me.Controls.Add(Me.GroupBox1)
        Me.MinimumSize = New System.Drawing.Size(391, 441)
        Me.Name = "frmMunisUser"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Employee Search"
        CType(Me.MunisResults, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox1.ResumeLayout(False)
        Me.pnlSearch.ResumeLayout(False)
        Me.pnlSearch.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents MunisResults As DataGridView
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents cmdAccept As Button
    Friend WithEvents pnlSearch As GroupBox
    Friend WithEvents Label1 As Label
    Friend WithEvents cmdSearch As Button
    Friend WithEvents txtSearchName As TextBox
    Friend WithEvents lblSelectedEmp As Label
End Class
