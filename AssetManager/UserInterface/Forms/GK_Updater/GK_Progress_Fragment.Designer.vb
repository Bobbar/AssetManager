<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class GK_Progress_Fragment
    Inherits System.Windows.Forms.UserControl

    'UserControl overrides dispose to clean up the component list.
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
        Me.pbarProgress = New System.Windows.Forms.ProgressBar()
        Me.lblCurrentFile = New System.Windows.Forms.Label()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.rtbLog = New System.Windows.Forms.RichTextBox()
        Me.lblShowHide = New System.Windows.Forms.Label()
        Me.UI_Timer = New System.Windows.Forms.Timer(Me.components)
        Me.MyToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.pbRestart = New System.Windows.Forms.PictureBox()
        Me.pbCancelClose = New System.Windows.Forms.PictureBox()
        Me.lblSeq = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        CType(Me.pbRestart, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCancelClose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.SuspendLayout()
        '
        'pbarProgress
        '
        Me.pbarProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbarProgress.Location = New System.Drawing.Point(19, 25)
        Me.pbarProgress.Name = "pbarProgress"
        Me.pbarProgress.Size = New System.Drawing.Size(356, 31)
        Me.pbarProgress.TabIndex = 0
        '
        'lblCurrentFile
        '
        Me.lblCurrentFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCurrentFile.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblCurrentFile.Location = New System.Drawing.Point(0, 60)
        Me.lblCurrentFile.Name = "lblCurrentFile"
        Me.lblCurrentFile.Size = New System.Drawing.Size(395, 19)
        Me.lblCurrentFile.TabIndex = 1
        Me.lblCurrentFile.Text = "[Status/File]"
        Me.lblCurrentFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblInfo
        '
        Me.lblInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfo.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblInfo.Font = New System.Drawing.Font("Consolas", 9.75!, CType((System.Drawing.FontStyle.Bold Or System.Drawing.FontStyle.Underline), System.Drawing.FontStyle), System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfo.ForeColor = System.Drawing.Color.Blue
        Me.lblInfo.Location = New System.Drawing.Point(1, 3)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(397, 19)
        Me.lblInfo.TabIndex = 2
        Me.lblInfo.Text = "[Computer Info]"
        Me.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.MyToolTip.SetToolTip(Me.lblInfo, "Click to view device.")
        '
        'rtbLog
        '
        Me.rtbLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbLog.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.rtbLog.DetectUrls = False
        Me.rtbLog.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbLog.ForeColor = System.Drawing.Color.FromArgb(CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer), CType(CType(234, Byte), Integer))
        Me.rtbLog.Location = New System.Drawing.Point(4, 87)
        Me.rtbLog.Name = "rtbLog"
        Me.rtbLog.ReadOnly = True
        Me.rtbLog.Size = New System.Drawing.Size(391, 208)
        Me.rtbLog.TabIndex = 4
        Me.rtbLog.Text = ""
        '
        'lblShowHide
        '
        Me.lblShowHide.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblShowHide.AutoSize = True
        Me.lblShowHide.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblShowHide.Font = New System.Drawing.Font("Consolas", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowHide.Location = New System.Drawing.Point(373, 60)
        Me.lblShowHide.Name = "lblShowHide"
        Me.lblShowHide.Size = New System.Drawing.Size(22, 24)
        Me.lblShowHide.TabIndex = 5
        Me.lblShowHide.Text = "+"
        Me.MyToolTip.SetToolTip(Me.lblShowHide, "Show/Hide Log")
        '
        'UI_Timer
        '
        Me.UI_Timer.Enabled = True
        '
        'pbRestart
        '
        Me.pbRestart.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbRestart.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbRestart.Image = Global.AssetManager.My.Resources.Resources._012_restart_2_512
        Me.pbRestart.Location = New System.Drawing.Point(351, 1)
        Me.pbRestart.Name = "pbRestart"
        Me.pbRestart.Size = New System.Drawing.Size(20, 20)
        Me.pbRestart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbRestart.TabIndex = 7
        Me.pbRestart.TabStop = False
        Me.MyToolTip.SetToolTip(Me.pbRestart, "Restart Update")
        '
        'pbCancelClose
        '
        Me.pbCancelClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbCancelClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbCancelClose.Image = Global.AssetManager.My.Resources.Resources.close_delete_cancel_del_ui_round_512
        Me.pbCancelClose.Location = New System.Drawing.Point(377, 1)
        Me.pbCancelClose.Name = "pbCancelClose"
        Me.pbCancelClose.Size = New System.Drawing.Size(20, 20)
        Me.pbCancelClose.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbCancelClose.TabIndex = 3
        Me.pbCancelClose.TabStop = False
        Me.MyToolTip.SetToolTip(Me.pbCancelClose, "Cancel/Close")
        '
        'lblSeq
        '
        Me.lblSeq.AutoSize = True
        Me.lblSeq.Location = New System.Drawing.Point(1, 2)
        Me.lblSeq.Name = "lblSeq"
        Me.lblSeq.Size = New System.Drawing.Size(42, 14)
        Me.lblSeq.TabIndex = 6
        Me.lblSeq.Text = "[Seq]"
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.Silver
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.lblSeq)
        Me.Panel1.Controls.Add(Me.pbRestart)
        Me.Panel1.Controls.Add(Me.lblShowHide)
        Me.Panel1.Controls.Add(Me.pbCancelClose)
        Me.Panel1.Controls.Add(Me.rtbLog)
        Me.Panel1.Controls.Add(Me.lblCurrentFile)
        Me.Panel1.Controls.Add(Me.pbarProgress)
        Me.Panel1.Controls.Add(Me.lblInfo)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(400, 300)
        Me.Panel1.TabIndex = 8
        '
        'GK_Progress_Fragment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.Panel1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximumSize = New System.Drawing.Size(400, 300)
        Me.MinimumSize = New System.Drawing.Size(400, 87)
        Me.Name = "GK_Progress_Fragment"
        Me.Size = New System.Drawing.Size(400, 300)
        CType(Me.pbRestart, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCancelClose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pbarProgress As ProgressBar
    Friend WithEvents lblCurrentFile As Label
    Friend WithEvents lblInfo As Label
    Friend WithEvents pbCancelClose As PictureBox
    Friend WithEvents rtbLog As RichTextBox
    Friend WithEvents lblShowHide As Label
    Friend WithEvents UI_Timer As Timer
    Friend WithEvents MyToolTip As ToolTip
    Friend WithEvents lblSeq As Label
    Friend WithEvents pbRestart As PictureBox
    Friend WithEvents Panel1 As Panel
End Class
