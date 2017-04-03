<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class GKProgress
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(GKProgress))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Button1 = New System.Windows.Forms.Button()
        Me.txtPassword = New System.Windows.Forms.TextBox()
        Me.txtUsername = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.lblCurrentFile = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.pbarProgress = New System.Windows.Forms.ProgressBar()
        Me.lstLog = New System.Windows.Forms.ListBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.lblServerDrive = New System.Windows.Forms.Label()
        Me.lblClientDrive = New System.Windows.Forms.Label()
        Me.tmrStatus = New System.Windows.Forms.Timer(Me.components)
        Me.GroupBox1.SuspendLayout()
        Me.GroupBox2.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.Button1)
        Me.GroupBox1.Controls.Add(Me.txtPassword)
        Me.GroupBox1.Controls.Add(Me.txtUsername)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Location = New System.Drawing.Point(13, 21)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(511, 172)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "LA Credentials"
        '
        'Button1
        '
        Me.Button1.Location = New System.Drawing.Point(289, 79)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(145, 35)
        Me.Button1.TabIndex = 4
        Me.Button1.Text = "Go"
        Me.Button1.UseVisualStyleBackColor = True
        '
        'txtPassword
        '
        Me.txtPassword.Location = New System.Drawing.Point(35, 107)
        Me.txtPassword.Name = "txtPassword"
        Me.txtPassword.Size = New System.Drawing.Size(162, 23)
        Me.txtPassword.TabIndex = 3
        Me.txtPassword.UseSystemPasswordChar = True
        '
        'txtUsername
        '
        Me.txtUsername.Location = New System.Drawing.Point(35, 54)
        Me.txtUsername.Name = "txtUsername"
        Me.txtUsername.Size = New System.Drawing.Size(162, 23)
        Me.txtUsername.TabIndex = 2
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(32, 89)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 15)
        Me.Label2.TabIndex = 1
        Me.Label2.Text = "Password:"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(32, 36)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 15)
        Me.Label1.TabIndex = 0
        Me.Label1.Text = "Username:"
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.lblClientDrive)
        Me.GroupBox2.Controls.Add(Me.lblServerDrive)
        Me.GroupBox2.Controls.Add(Me.Label4)
        Me.GroupBox2.Controls.Add(Me.lblCurrentFile)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.pbarProgress)
        Me.GroupBox2.Controls.Add(Me.lstLog)
        Me.GroupBox2.Location = New System.Drawing.Point(13, 199)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(511, 397)
        Me.GroupBox2.TabIndex = 1
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Status"
        '
        'lblCurrentFile
        '
        Me.lblCurrentFile.Location = New System.Drawing.Point(234, 73)
        Me.lblCurrentFile.Name = "lblCurrentFile"
        Me.lblCurrentFile.Size = New System.Drawing.Size(251, 21)
        Me.lblCurrentFile.TabIndex = 3
        Me.lblCurrentFile.Text = "[current file]"
        Me.lblCurrentFile.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(234, 24)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(63, 15)
        Me.Label3.TabIndex = 2
        Me.Label3.Text = "Progress"
        '
        'pbarProgress
        '
        Me.pbarProgress.Location = New System.Drawing.Point(234, 41)
        Me.pbarProgress.Name = "pbarProgress"
        Me.pbarProgress.Size = New System.Drawing.Size(251, 30)
        Me.pbarProgress.TabIndex = 1
        '
        'lstLog
        '
        Me.lstLog.FormattingEnabled = True
        Me.lstLog.ItemHeight = 15
        Me.lstLog.Location = New System.Drawing.Point(18, 130)
        Me.lstLog.Name = "lstLog"
        Me.lstLog.Size = New System.Drawing.Size(467, 244)
        Me.lstLog.TabIndex = 0
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(234, 108)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(28, 15)
        Me.Label4.TabIndex = 4
        Me.Label4.Text = "Log"
        '
        'lblServerDrive
        '
        Me.lblServerDrive.AutoSize = True
        Me.lblServerDrive.Location = New System.Drawing.Point(15, 41)
        Me.lblServerDrive.Name = "lblServerDrive"
        Me.lblServerDrive.Size = New System.Drawing.Size(182, 15)
        Me.lblServerDrive.TabIndex = 5
        Me.lblServerDrive.Text = "Server Map:  Disconnected"
        '
        'lblClientDrive
        '
        Me.lblClientDrive.AutoSize = True
        Me.lblClientDrive.Location = New System.Drawing.Point(15, 74)
        Me.lblClientDrive.Name = "lblClientDrive"
        Me.lblClientDrive.Size = New System.Drawing.Size(182, 15)
        Me.lblClientDrive.TabIndex = 6
        Me.lblClientDrive.Text = "Client Map:  Disconnected"
        '
        'tmrStatus
        '
        Me.tmrStatus.Enabled = True
        Me.tmrStatus.Interval = 200
        '
        'GKProgress
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(547, 631)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "GKProgress"
        Me.Text = "Gatekeeper Updater"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents txtPassword As TextBox
    Friend WithEvents txtUsername As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents Button1 As Button
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents lblCurrentFile As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents pbarProgress As ProgressBar
    Friend WithEvents lstLog As ListBox
    Friend WithEvents Label4 As Label
    Friend WithEvents lblServerDrive As Label
    Friend WithEvents lblClientDrive As Label
    Friend WithEvents tmrStatus As Timer
End Class
