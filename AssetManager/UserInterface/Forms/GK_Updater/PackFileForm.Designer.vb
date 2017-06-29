<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class PackFileForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(PackFileForm))
        Me.ProgressBar = New System.Windows.Forms.ProgressBar()
        Me.ProgressTimer = New System.Windows.Forms.Timer(Me.components)
        Me.StatusLabel = New System.Windows.Forms.Label()
        Me.VerifyPackButton = New System.Windows.Forms.Button()
        Me.NewPackButton = New System.Windows.Forms.Button()
        Me.FunctionPanel = New System.Windows.Forms.Panel()
        Me.TableLayoutPanel = New System.Windows.Forms.TableLayoutPanel()
        Me.StatusPanel = New System.Windows.Forms.Panel()
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.SpeedLabel = New System.Windows.Forms.Label()
        Me.FunctionPanel.SuspendLayout()
        Me.TableLayoutPanel.SuspendLayout()
        Me.StatusPanel.SuspendLayout()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'ProgressBar
        '
        Me.ProgressBar.Location = New System.Drawing.Point(19, 33)
        Me.ProgressBar.Name = "ProgressBar"
        Me.ProgressBar.Size = New System.Drawing.Size(419, 23)
        Me.ProgressBar.TabIndex = 0
        '
        'ProgressTimer
        '
        Me.ProgressTimer.Enabled = True
        '
        'StatusLabel
        '
        Me.StatusLabel.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.StatusLabel.Location = New System.Drawing.Point(19, 5)
        Me.StatusLabel.Name = "StatusLabel"
        Me.StatusLabel.Size = New System.Drawing.Size(419, 25)
        Me.StatusLabel.TabIndex = 1
        Me.StatusLabel.Text = "{STATUS}"
        Me.StatusLabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'VerifyPackButton
        '
        Me.VerifyPackButton.Location = New System.Drawing.Point(153, 6)
        Me.VerifyPackButton.Name = "VerifyPackButton"
        Me.VerifyPackButton.Size = New System.Drawing.Size(150, 30)
        Me.VerifyPackButton.TabIndex = 2
        Me.VerifyPackButton.Text = "Verify Pack File"
        Me.VerifyPackButton.UseVisualStyleBackColor = True
        '
        'NewPackButton
        '
        Me.NewPackButton.Location = New System.Drawing.Point(153, 42)
        Me.NewPackButton.Name = "NewPackButton"
        Me.NewPackButton.Size = New System.Drawing.Size(150, 26)
        Me.NewPackButton.TabIndex = 3
        Me.NewPackButton.Text = "Upload New Pack"
        Me.NewPackButton.UseVisualStyleBackColor = True
        '
        'FunctionPanel
        '
        Me.FunctionPanel.Controls.Add(Me.NewPackButton)
        Me.FunctionPanel.Controls.Add(Me.VerifyPackButton)
        Me.FunctionPanel.Location = New System.Drawing.Point(3, 83)
        Me.FunctionPanel.Name = "FunctionPanel"
        Me.FunctionPanel.Size = New System.Drawing.Size(459, 74)
        Me.FunctionPanel.TabIndex = 4
        '
        'TableLayoutPanel
        '
        Me.TableLayoutPanel.AutoSize = True
        Me.TableLayoutPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.TableLayoutPanel.ColumnCount = 1
        Me.TableLayoutPanel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel.Controls.Add(Me.StatusPanel, 0, 0)
        Me.TableLayoutPanel.Controls.Add(Me.FunctionPanel, 0, 1)
        Me.TableLayoutPanel.Location = New System.Drawing.Point(4, 14)
        Me.TableLayoutPanel.Margin = New System.Windows.Forms.Padding(0)
        Me.TableLayoutPanel.Name = "TableLayoutPanel"
        Me.TableLayoutPanel.RowCount = 2
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel.RowStyles.Add(New System.Windows.Forms.RowStyle())
        Me.TableLayoutPanel.Size = New System.Drawing.Size(465, 160)
        Me.TableLayoutPanel.TabIndex = 5
        '
        'StatusPanel
        '
        Me.StatusPanel.Controls.Add(Me.SpeedLabel)
        Me.StatusPanel.Controls.Add(Me.ProgressBar)
        Me.StatusPanel.Controls.Add(Me.StatusLabel)
        Me.StatusPanel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.StatusPanel.Location = New System.Drawing.Point(3, 3)
        Me.StatusPanel.Name = "StatusPanel"
        Me.StatusPanel.Size = New System.Drawing.Size(459, 74)
        Me.StatusPanel.TabIndex = 6
        '
        'GroupBox1
        '
        Me.GroupBox1.AutoSize = True
        Me.GroupBox1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.GroupBox1.Controls.Add(Me.TableLayoutPanel)
        Me.GroupBox1.Location = New System.Drawing.Point(6, 4)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(472, 193)
        Me.GroupBox1.TabIndex = 6
        Me.GroupBox1.TabStop = False
        '
        'SpeedLabel
        '
        Me.SpeedLabel.Location = New System.Drawing.Point(19, 59)
        Me.SpeedLabel.Name = "SpeedLabel"
        Me.SpeedLabel.Size = New System.Drawing.Size(419, 14)
        Me.SpeedLabel.TabIndex = 2
        Me.SpeedLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter
        '
        'PackFileForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 15.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.ClientSize = New System.Drawing.Size(491, 242)
        Me.Controls.Add(Me.GroupBox1)
        Me.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "PackFileForm"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Pack File Manager"
        Me.FunctionPanel.ResumeLayout(False)
        Me.TableLayoutPanel.ResumeLayout(False)
        Me.StatusPanel.ResumeLayout(False)
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents ProgressBar As ProgressBar
    Friend WithEvents ProgressTimer As Timer
    Friend WithEvents StatusLabel As Label
    Friend WithEvents VerifyPackButton As Button
    Friend WithEvents NewPackButton As Button
    Friend WithEvents FunctionPanel As Panel
    Friend WithEvents TableLayoutPanel As TableLayoutPanel
    Friend WithEvents StatusPanel As Panel
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents SpeedLabel As Label
End Class
