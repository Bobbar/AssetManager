﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class GKUpdaterForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GKUpdaterForm))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Updater_Table = New System.Windows.Forms.FlowLayoutPanel()
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.MaxUpdates = New System.Windows.Forms.NumericUpDown()
        Me.cmdCancelAll = New System.Windows.Forms.Button()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.cmdPauseResume = New System.Windows.Forms.Button()
        Me.lblTotUpdates = New System.Windows.Forms.Label()
        Me.lblComplete = New System.Windows.Forms.Label()
        Me.lblRunning = New System.Windows.Forms.Label()
        Me.lblQueued = New System.Windows.Forms.Label()
        Me.QueueChecker = New System.Windows.Forms.Timer(Me.components)
        Me.lblTransferRate = New System.Windows.Forms.Label()
        Me.cmdSort = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox3.SuspendLayout()
        CType(Me.MaxUpdates, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.GroupBox1.BackColor = System.Drawing.SystemColors.Control
        Me.GroupBox1.Controls.Add(Me.Updater_Table)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 90)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1068, 528)
        Me.GroupBox1.TabIndex = 2
        Me.GroupBox1.TabStop = False
        '
        'Updater_Table
        '
        Me.Updater_Table.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Updater_Table.AutoScroll = True
        Me.Updater_Table.BackColor = System.Drawing.SystemColors.Control
        Me.Updater_Table.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Updater_Table.Location = New System.Drawing.Point(9, 21)
        Me.Updater_Table.Margin = New System.Windows.Forms.Padding(10)
        Me.Updater_Table.Name = "Updater_Table"
        Me.Updater_Table.Size = New System.Drawing.Size(1051, 497)
        Me.Updater_Table.TabIndex = 2
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.MaxUpdates)
        Me.GroupBox3.Controls.Add(Me.cmdCancelAll)
        Me.GroupBox3.Controls.Add(Me.Label1)
        Me.GroupBox3.Controls.Add(Me.cmdPauseResume)
        Me.GroupBox3.Controls.Add(Me.lblTotUpdates)
        Me.GroupBox3.Controls.Add(Me.lblComplete)
        Me.GroupBox3.Controls.Add(Me.lblRunning)
        Me.GroupBox3.Controls.Add(Me.lblQueued)
        Me.GroupBox3.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(577, 79)
        Me.GroupBox3.TabIndex = 0
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Queue Control"
        '
        'MaxUpdates
        '
        Me.MaxUpdates.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaxUpdates.Location = New System.Drawing.Point(302, 40)
        Me.MaxUpdates.Maximum = New Decimal(New Integer() {10, 0, 0, 0})
        Me.MaxUpdates.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
        Me.MaxUpdates.Name = "MaxUpdates"
        Me.MaxUpdates.Size = New System.Drawing.Size(59, 22)
        Me.MaxUpdates.TabIndex = 5
        Me.MaxUpdates.Value = New Decimal(New Integer() {1, 0, 0, 0})
        '
        'cmdCancelAll
        '
        Me.cmdCancelAll.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCancelAll.Location = New System.Drawing.Point(441, 45)
        Me.cmdCancelAll.Name = "cmdCancelAll"
        Me.cmdCancelAll.Size = New System.Drawing.Size(120, 27)
        Me.cmdCancelAll.TabIndex = 7
        Me.cmdCancelAll.Text = "Cancel All"
        Me.cmdCancelAll.UseVisualStyleBackColor = True
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(277, 18)
        Me.Label1.Name = "Label1"
        Me.Label1.Padding = New System.Windows.Forms.Padding(5)
        Me.Label1.Size = New System.Drawing.Size(115, 24)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Max Concurrent"
        '
        'cmdPauseResume
        '
        Me.cmdPauseResume.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdPauseResume.Location = New System.Drawing.Point(441, 12)
        Me.cmdPauseResume.Name = "cmdPauseResume"
        Me.cmdPauseResume.Size = New System.Drawing.Size(120, 27)
        Me.cmdPauseResume.TabIndex = 4
        Me.cmdPauseResume.Text = "Pause Queue"
        Me.cmdPauseResume.UseVisualStyleBackColor = True
        '
        'lblTotUpdates
        '
        Me.lblTotUpdates.AutoSize = True
        Me.lblTotUpdates.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTotUpdates.Location = New System.Drawing.Point(6, 18)
        Me.lblTotUpdates.Name = "lblTotUpdates"
        Me.lblTotUpdates.Padding = New System.Windows.Forms.Padding(5)
        Me.lblTotUpdates.Size = New System.Drawing.Size(94, 24)
        Me.lblTotUpdates.TabIndex = 3
        Me.lblTotUpdates.Text = "[# Updates]"
        '
        'lblComplete
        '
        Me.lblComplete.AutoSize = True
        Me.lblComplete.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblComplete.Location = New System.Drawing.Point(145, 42)
        Me.lblComplete.Name = "lblComplete"
        Me.lblComplete.Padding = New System.Windows.Forms.Padding(5)
        Me.lblComplete.Size = New System.Drawing.Size(87, 24)
        Me.lblComplete.TabIndex = 2
        Me.lblComplete.Text = "[Complete]"
        '
        'lblRunning
        '
        Me.lblRunning.AutoSize = True
        Me.lblRunning.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblRunning.Location = New System.Drawing.Point(145, 18)
        Me.lblRunning.Name = "lblRunning"
        Me.lblRunning.Padding = New System.Windows.Forms.Padding(5)
        Me.lblRunning.Size = New System.Drawing.Size(80, 24)
        Me.lblRunning.TabIndex = 1
        Me.lblRunning.Text = "[Running]"
        '
        'lblQueued
        '
        Me.lblQueued.AutoSize = True
        Me.lblQueued.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblQueued.Location = New System.Drawing.Point(6, 42)
        Me.lblQueued.Name = "lblQueued"
        Me.lblQueued.Padding = New System.Windows.Forms.Padding(5)
        Me.lblQueued.Size = New System.Drawing.Size(73, 24)
        Me.lblQueued.TabIndex = 0
        Me.lblQueued.Text = "[Queued]"
        '
        'QueueChecker
        '
        Me.QueueChecker.Enabled = True
        Me.QueueChecker.Interval = 250
        '
        'lblTransferRate
        '
        Me.lblTransferRate.AutoSize = True
        Me.lblTransferRate.Location = New System.Drawing.Point(633, 47)
        Me.lblTransferRate.Name = "lblTransferRate"
        Me.lblTransferRate.Size = New System.Drawing.Size(112, 14)
        Me.lblTransferRate.TabIndex = 3
        Me.lblTransferRate.Text = "[Transfer Rate]"
        '
        'cmdSort
        '
        Me.cmdSort.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdSort.Location = New System.Drawing.Point(994, 67)
        Me.cmdSort.Name = "cmdSort"
        Me.cmdSort.Size = New System.Drawing.Size(86, 24)
        Me.cmdSort.TabIndex = 4
        Me.cmdSort.Text = "Sort"
        Me.cmdSort.UseVisualStyleBackColor = True
        '
        'GKUpdater_Form
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 14.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(1094, 638)
        Me.Controls.Add(Me.cmdSort)
        Me.Controls.Add(Me.lblTransferRate)
        Me.Controls.Add(Me.GroupBox3)
        Me.Controls.Add(Me.GroupBox1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MinimumSize = New System.Drawing.Size(1010, 428)
        Me.Name = "GKUpdater_Form"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Gatekeeper Updater"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        CType(Me.MaxUpdates, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents QueueChecker As Timer
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents lblQueued As Label
    Friend WithEvents lblRunning As Label
    Friend WithEvents lblComplete As Label
    Friend WithEvents lblTotUpdates As Label
    Friend WithEvents cmdPauseResume As Button
    Friend WithEvents Label1 As Label
    Friend WithEvents MaxUpdates As NumericUpDown
    Friend WithEvents cmdCancelAll As Button
    Friend WithEvents Updater_Table As FlowLayoutPanel
    Friend WithEvents lblTransferRate As Label
    Friend WithEvents cmdSort As Button
End Class