<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class LicenseManagementForm
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
        Dim DataGridViewCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
        Me.LicensesGrid = New System.Windows.Forms.DataGridView()
        CType(Me.LicensesGrid, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'LicensesGrid
        '
        Me.LicensesGrid.AllowUserToAddRows = False
        Me.LicensesGrid.AllowUserToDeleteRows = False
        Me.LicensesGrid.AllowUserToResizeColumns = False
        Me.LicensesGrid.AllowUserToResizeRows = False
        Me.LicensesGrid.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.LicensesGrid.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells
        Me.LicensesGrid.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.EnableWithoutHeaderText
        Me.LicensesGrid.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
        DataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter
        DataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Window
        DataGridViewCellStyle1.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        DataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.ControlText
        DataGridViewCellStyle1.Padding = New System.Windows.Forms.Padding(10)
        DataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
        DataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
        DataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
        Me.LicensesGrid.DefaultCellStyle = DataGridViewCellStyle1
        Me.LicensesGrid.EditMode = System.Windows.Forms.DataGridViewEditMode.EditProgrammatically
        Me.LicensesGrid.Location = New System.Drawing.Point(40, 164)
        Me.LicensesGrid.Name = "LicensesGrid"
        Me.LicensesGrid.ReadOnly = True
        Me.LicensesGrid.RowHeadersVisible = False
        Me.LicensesGrid.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.CellSelect
        Me.LicensesGrid.ShowCellToolTips = False
        Me.LicensesGrid.ShowEditingIcon = False
        Me.LicensesGrid.Size = New System.Drawing.Size(752, 379)
        Me.LicensesGrid.TabIndex = 41
        Me.LicensesGrid.TabStop = False
        Me.LicensesGrid.VirtualMode = True
        '
        'LicenseManagementForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(873, 569)
        Me.Controls.Add(Me.LicensesGrid)
        Me.Name = "LicenseManagementForm"
        Me.Text = "LicenseManagementForm"
        CType(Me.LicensesGrid, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents LicensesGrid As DataGridView
End Class
