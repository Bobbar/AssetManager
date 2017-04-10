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
        Me.PictureBox1 = New System.Windows.Forms.PictureBox()
        Me.rtbLog = New System.Windows.Forms.RichTextBox()
        Me.lblShowHide = New System.Windows.Forms.Label()
        Me.UI_Timer = New System.Windows.Forms.Timer(Me.components)
        Me.MyToolTip = New System.Windows.Forms.ToolTip(Me.components)
        Me.lblSeq = New System.Windows.Forms.Label()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'pbarProgress
        '
        Me.pbarProgress.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pbarProgress.Location = New System.Drawing.Point(21, 27)
        Me.pbarProgress.Name = "pbarProgress"
        Me.pbarProgress.Size = New System.Drawing.Size(355, 31)
        Me.pbarProgress.TabIndex = 0
        '
        'lblCurrentFile
        '
        Me.lblCurrentFile.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblCurrentFile.Location = New System.Drawing.Point(0, 61)
        Me.lblCurrentFile.Name = "lblCurrentFile"
        Me.lblCurrentFile.Size = New System.Drawing.Size(400, 19)
        Me.lblCurrentFile.TabIndex = 1
        Me.lblCurrentFile.Text = "[Status/File]"
        Me.lblCurrentFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'lblInfo
        '
        Me.lblInfo.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblInfo.Location = New System.Drawing.Point(0, 4)
        Me.lblInfo.Name = "lblInfo"
        Me.lblInfo.Size = New System.Drawing.Size(400, 19)
        Me.lblInfo.TabIndex = 2
        Me.lblInfo.Text = "[Computer Info]"
        Me.lblInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        Me.MyToolTip.SetToolTip(Me.lblInfo, "Click to view device.")
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = Global.AssetManager.My.Resources.Resources.close_delete_cancel_del_ui_round_512
        Me.PictureBox1.Location = New System.Drawing.Point(380, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(20, 20)
        Me.PictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.PictureBox1.TabIndex = 3
        Me.PictureBox1.TabStop = False
        Me.MyToolTip.SetToolTip(Me.PictureBox1, "Cancel/Close")
        '
        'rtbLog
        '
        Me.rtbLog.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbLog.DetectUrls = False
        Me.rtbLog.Font = New System.Drawing.Font("Consolas", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbLog.Location = New System.Drawing.Point(3, 88)
        Me.rtbLog.Name = "rtbLog"
        Me.rtbLog.ReadOnly = True
        Me.rtbLog.Size = New System.Drawing.Size(394, 0)
        Me.rtbLog.TabIndex = 4
        Me.rtbLog.Text = ""
        '
        'lblShowHide
        '
        Me.lblShowHide.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lblShowHide.AutoSize = True
        Me.lblShowHide.Font = New System.Drawing.Font("Consolas", 15.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lblShowHide.Location = New System.Drawing.Point(375, 61)
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
        'lblSeq
        '
        Me.lblSeq.AutoSize = True
        Me.lblSeq.Location = New System.Drawing.Point(3, 4)
        Me.lblSeq.Name = "lblSeq"
        Me.lblSeq.Size = New System.Drawing.Size(42, 14)
        Me.lblSeq.TabIndex = 6
        Me.lblSeq.Text = "[Seq]"
        '
        'GK_Progress_Fragment
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.Controls.Add(Me.lblSeq)
        Me.Controls.Add(Me.lblShowHide)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.rtbLog)
        Me.Controls.Add(Me.lblCurrentFile)
        Me.Controls.Add(Me.pbarProgress)
        Me.Controls.Add(Me.lblInfo)
        Me.DoubleBuffered = True
        Me.Font = New System.Drawing.Font("Consolas", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.MaximumSize = New System.Drawing.Size(400, 300)
        Me.MinimumSize = New System.Drawing.Size(400, 87)
        Me.Name = "GK_Progress_Fragment"
        Me.Size = New System.Drawing.Size(400, 87)
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents pbarProgress As ProgressBar
    Friend WithEvents lblCurrentFile As Label
    Friend WithEvents lblInfo As Label
    Friend WithEvents PictureBox1 As PictureBox
    Friend WithEvents rtbLog As RichTextBox
    Friend WithEvents lblShowHide As Label
    Friend WithEvents UI_Timer As Timer
    Friend WithEvents MyToolTip As ToolTip
    Friend WithEvents lblSeq As Label
End Class
