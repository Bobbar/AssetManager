<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GKUpdater_Form
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
        Me.Updater_Table = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'Updater_Table
        '
        Me.Updater_Table.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Updater_Table.AutoScroll = True
        Me.Updater_Table.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Updater_Table.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble
        Me.Updater_Table.ColumnCount = 2
        Me.Updater_Table.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.Updater_Table.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.Updater_Table.Location = New System.Drawing.Point(6, 19)
        Me.Updater_Table.Name = "Updater_Table"
        Me.Updater_Table.RowCount = 1
        Me.Updater_Table.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.Updater_Table.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 416.0!))
        Me.Updater_Table.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 416.0!))
        Me.Updater_Table.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 416.0!))
        Me.Updater_Table.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 416.0!))
        Me.Updater_Table.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 416.0!))
        Me.Updater_Table.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 416.0!))
        Me.Updater_Table.Size = New System.Drawing.Size(816, 418)
        Me.Updater_Table.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Updater_Table)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(828, 443)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Current Updates"
        '
        'GKUpdater_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(852, 467)
        Me.Controls.Add(Me.GroupBox1)
        Me.DoubleBuffered = True
        Me.MinimumSize = New System.Drawing.Size(868, 400)
        Me.Name = "GKUpdater_Form"
        Me.Text = "Updater_Test"
        Me.GroupBox1.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Updater_Table As TableLayoutPanel
    Friend WithEvents GroupBox1 As GroupBox
End Class
