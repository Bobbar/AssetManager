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
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GKUpdater_Form))
        Me.Updater_Table = New System.Windows.Forms.TableLayoutPanel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.lblTotUpdates = New System.Windows.Forms.Label()
        Me.lblComplete = New System.Windows.Forms.Label()
        Me.lblRunning = New System.Windows.Forms.Label()
        Me.lblQueued = New System.Windows.Forms.Label()
        Me.QueueChecker = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
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
        Me.Updater_Table.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 464.0!))
        Me.Updater_Table.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 464.0!))
        Me.Updater_Table.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 464.0!))
        Me.Updater_Table.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 464.0!))
        Me.Updater_Table.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 464.0!))
        Me.Updater_Table.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 464.0!))
        Me.Updater_Table.Size = New System.Drawing.Size(729, 525)
        Me.Updater_Table.TabIndex = 1
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.Controls.Add(Me.Updater_Table)
        Me.GroupBox1.Location = New System.Drawing.Point(336, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(741, 550)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Current Updates"
        '
        'GroupBox2
        '
        Me.GroupBox2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.GroupBox2.Controls.Add(Me.GroupBox3)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(316, 550)
        Me.GroupBox2.TabIndex = 3
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Manage Updates"
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.lblTotUpdates)
        Me.GroupBox3.Controls.Add(Me.lblComplete)
        Me.GroupBox3.Controls.Add(Me.lblRunning)
        Me.GroupBox3.Controls.Add(Me.lblQueued)
        Me.GroupBox3.Location = New System.Drawing.Point(15, 31)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(285, 147)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Stats"
        '
        'lblTotUpdates
        '
        Me.lblTotUpdates.AutoSize = True
        Me.lblTotUpdates.Location = New System.Drawing.Point(22, 27)
        Me.lblTotUpdates.Name = "lblTotUpdates"
        Me.lblTotUpdates.Size = New System.Drawing.Size(63, 13)
        Me.lblTotUpdates.TabIndex = 3
        Me.lblTotUpdates.Text = "[# Updates]"
        '
        'lblComplete
        '
        Me.lblComplete.AutoSize = True
        Me.lblComplete.Location = New System.Drawing.Point(22, 94)
        Me.lblComplete.Name = "lblComplete"
        Me.lblComplete.Size = New System.Drawing.Size(57, 13)
        Me.lblComplete.TabIndex = 2
        Me.lblComplete.Text = "[Complete]"
        '
        'lblRunning
        '
        Me.lblRunning.AutoSize = True
        Me.lblRunning.Location = New System.Drawing.Point(22, 71)
        Me.lblRunning.Name = "lblRunning"
        Me.lblRunning.Size = New System.Drawing.Size(53, 13)
        Me.lblRunning.TabIndex = 1
        Me.lblRunning.Text = "[Running]"
        '
        'lblQueued
        '
        Me.lblQueued.AutoSize = True
        Me.lblQueued.Location = New System.Drawing.Point(22, 49)
        Me.lblQueued.Name = "lblQueued"
        Me.lblQueued.Size = New System.Drawing.Size(51, 13)
        Me.lblQueued.TabIndex = 0
        Me.lblQueued.Text = "[Queued]"
        '
        'QueueChecker
        '
        Me.QueueChecker.Enabled = True
        Me.QueueChecker.Interval = 500
        '
        'GKUpdater_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1089, 574)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(868, 400)
        Me.Name = "GKUpdater_Form"
        Me.Text = "Gatekeeper Updater"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents Updater_Table As TableLayoutPanel
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents QueueChecker As Timer
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents lblQueued As Label
    Friend WithEvents lblRunning As Label
    Friend WithEvents lblComplete As Label
    Friend WithEvents lblTotUpdates As Label
End Class
