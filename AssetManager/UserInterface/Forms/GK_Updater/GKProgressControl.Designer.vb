<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class GKProgressControl
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
        Me.pbarFileProgress = New System.Windows.Forms.ProgressBar()
        Me.lblStatus = New System.Windows.Forms.Label()
        Me.lblInfo = New System.Windows.Forms.Label()
        Me.rtbLog = New System.Windows.Forms.RichTextBox()
        Me.lblShowHide = New System.Windows.Forms.Label()
        Me.UI_Timer = New System.Windows.Forms.Timer(Me.components)
        Me.MyToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.pbRestart = New System.Windows.Forms.PictureBox()
        Me.pbCancelClose = New System.Windows.Forms.PictureBox()
        Me.lblSeq = New System.Windows.Forms.Label()
        Me.Panel1 = New System.Windows.Forms.Panel()
        Me.lblTransRate = New System.Windows.Forms.Label()
        Me.pbarProgress = New System.Windows.Forms.ProgressBar()
        Me.pbStatus = New System.Windows.Forms.PictureBox()
        CType(Me.pbRestart, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.pbCancelClose, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.pbStatus, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pbarFileProgress
        '
        Me.pbarFileProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbarFileProgress.Location = New System.Drawing.Point(75, 49)
        Me.pbarFileProgress.Name = "pbarFileProgress"
        Me.pbarFileProgress.Size = New System.Drawing.Size(254, 12)
        Me.pbarFileProgress.TabIndex = 0
        '
        'lblStatus
        '
        Me.lblStatus.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblStatus.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblStatus.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblStatus.Location = New System.Drawing.Point(2, 61)
        Me.lblStatus.Name = "lblStatus"
        Me.lblStatus.Size = New System.Drawing.Size(395, 14)
        Me.lblStatus.TabIndex = 1
        Me.lblStatus.Text = "[Status/File]"
        Me.lblStatus.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblInfo
        '
        Me.lblInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfo.BackColor = System.Drawing.Color.FromArgb(CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer), CType(CType(64, Byte), Integer))
        Me.lblInfo.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.lblInfo.Cursor = System.Windows.Forms.Cursors.Hand
        Me.lblInfo.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblInfo.ForeColor = System.Drawing.Color.White
        Me.lblInfo.ImageAlign = System.Drawing.ContentAlignment.TopCenter
        Me.lblInfo.Location = New System.Drawing.Point(75, 4)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(254, 17)
        Me.lblInfo.TabIndex = 2
        Me.lblInfo.Text = "[Computer Info]"
        Me.lblInfo.TextAlign = System.Drawing.ContentAlignment.TopCenter
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
        Me.lblShowHide.Font = New System.Drawing.Font("Wingdings 3", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(2, Byte))
        Me.lblShowHide.ImageAlign = System.Drawing.ContentAlignment.BottomCenter
        Me.lblShowHide.Location = New System.Drawing.Point(373, 72)
        Me.lblShowHide.Name = "lblShowHide"
        Me.lblShowHide.Size = New System.Drawing.Size(17, 12)
        Me.lblShowHide.TabIndex = 5
        Me.lblShowHide.Text = "s"
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
        Me.pbRestart.Image = Global.AssetManager.My.Resources.Resources.RestartIcon
        Me.pbRestart.Location = New System.Drawing.Point(350, 2)
        Me.pbRestart.Name = "pbRestart"
        Me.pbRestart.Size = New System.Drawing.Size(20, 20)
        Me.pbRestart.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbRestart.TabIndex = 7
        Me.pbRestart.TabStop = False
        Me.MyToolTip.SetToolTip(Me.pbRestart, "Start/Restart Update")
        '
        'pbCancelClose
        '
        Me.pbCancelClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbCancelClose.Cursor = System.Windows.Forms.Cursors.Hand
        Me.pbCancelClose.Image = Global.AssetManager.My.Resources.Resources.CloseCancelDeleteIcon
        Me.pbCancelClose.Location = New System.Drawing.Point(376, 2)
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
        Me.Panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Panel1.BackColor = System.Drawing.Color.Silver
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.lblTransRate)
        Me.Panel1.Controls.Add(Me.pbarProgress)
        Me.Panel1.Controls.Add(Me.pbStatus)
        Me.Panel1.Controls.Add(Me.lblSeq)
        Me.Panel1.Controls.Add(Me.pbRestart)
        Me.Panel1.Controls.Add(Me.lblShowHide)
        Me.Panel1.Controls.Add(Me.pbCancelClose)
        Me.Panel1.Controls.Add(Me.rtbLog)
        Me.Panel1.Controls.Add(Me.lblStatus)
        Me.Panel1.Controls.Add(Me.pbarFileProgress)
        Me.Panel1.Controls.Add(Me.lblInfo)
        Me.Panel1.Location = New System.Drawing.Point(0, 0)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(400, 300)
        Me.Panel1.TabIndex = 8
        '
        'lblTransRate
        '
        Me.lblTransRate.AutoSize = True
        Me.lblTransRate.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblTransRate.Location = New System.Drawing.Point(327, 48)
        Me.lblTransRate.Name = "lblTransRate"
        Me.lblTransRate.Size = New System.Drawing.Size(43, 13)
        Me.lblTransRate.TabIndex = 10
        Me.lblTransRate.Text = "[MBps]"
        '
        'pbarProgress
        '
        Me.pbarProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbarProgress.Location = New System.Drawing.Point(39, 27)
        Me.pbarProgress.Name = "pbarProgress"
        Me.pbarProgress.Size = New System.Drawing.Size(327, 20)
        Me.pbarProgress.TabIndex = 9
        '
        'pbStatus
        '
        Me.pbStatus.Location = New System.Drawing.Point(2, 25)
        Me.pbStatus.Name = "pbStatus"
        Me.pbStatus.Size = New System.Drawing.Size(37, 30)
        Me.pbStatus.TabIndex = 8
        Me.pbStatus.TabStop = False
        '
        'GKProgressControl
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.Controls.Add(Me.Panel1)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximumSize = New System.Drawing.Size(400, 300)
        Me.MinimumSize = New System.Drawing.Size(400, 87)
        Me.Name = "GKProgressControl"
        Me.Size = New System.Drawing.Size(400, 300)
        CType(Me.pbRestart, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.pbCancelClose, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.pbStatus, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents pbarFileProgress As ProgressBar
    Friend WithEvents lblStatus As Label
    Friend WithEvents lblInfo As Label
    Friend WithEvents pbCancelClose As PictureBox
    Friend WithEvents rtbLog As RichTextBox
    Friend WithEvents lblShowHide As Label
    Friend WithEvents UI_Timer As Timer
    Friend WithEvents MyToolTip As ToolTip
    Friend WithEvents lblSeq As Label
    Friend WithEvents pbRestart As PictureBox
    Friend WithEvents Panel1 As Panel
    Friend WithEvents pbStatus As PictureBox
    Friend WithEvents pbarProgress As ProgressBar
    Friend WithEvents lblTransRate As Label
End Class
