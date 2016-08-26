<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class MyDialog
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(MyDialog))
        Me.tblOkCancel = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.Panel = New System.Windows.Forms.FlowLayoutPanel()
        Me.tblYesNo = New System.Windows.Forms.TableLayoutPanel()
        Me.Yes_Button = New System.Windows.Forms.Button()
        Me.No_Button = New System.Windows.Forms.Button()
        Me.TableLayoutPanel1 = New System.Windows.Forms.TableLayoutPanel()
        Me.pbIcon = New System.Windows.Forms.PictureBox()
        Me.tblOkCancel.SuspendLayout()
        Me.tblYesNo.SuspendLayout()
        Me.TableLayoutPanel1.SuspendLayout()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'tblOkCancel
        '
        Me.tblOkCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tblOkCancel.ColumnCount = 2
        Me.tblOkCancel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblOkCancel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblOkCancel.Controls.Add(Me.OK_Button, 0, 0)
        Me.tblOkCancel.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.tblOkCancel.Location = New System.Drawing.Point(319, 163)
        Me.tblOkCancel.Name = "tblOkCancel"
        Me.tblOkCancel.RowCount = 1
        Me.tblOkCancel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblOkCancel.Size = New System.Drawing.Size(146, 29)
        Me.tblOkCancel.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(67, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "&OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.No
        Me.Cancel_Button.Location = New System.Drawing.Point(76, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(67, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "&Cancel"
        '
        'Panel
        '
        Me.Panel.AutoScroll = True
        Me.Panel.AutoSize = True
        Me.Panel.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel.Dock = System.Windows.Forms.DockStyle.Fill
        Me.Panel.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.Panel.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Panel.Location = New System.Drawing.Point(3, 3)
        Me.Panel.Name = "Panel"
        Me.Panel.Padding = New System.Windows.Forms.Padding(10, 10, 10, 0)
        Me.Panel.Size = New System.Drawing.Size(376, 137)
        Me.Panel.TabIndex = 3
        '
        'tblYesNo
        '
        Me.tblYesNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tblYesNo.ColumnCount = 2
        Me.tblYesNo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblYesNo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblYesNo.Controls.Add(Me.Yes_Button, 0, 0)
        Me.tblYesNo.Controls.Add(Me.No_Button, 1, 0)
        Me.tblYesNo.Location = New System.Drawing.Point(12, 163)
        Me.tblYesNo.Name = "tblYesNo"
        Me.tblYesNo.RowCount = 1
        Me.tblYesNo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50.0!))
        Me.tblYesNo.Size = New System.Drawing.Size(146, 29)
        Me.tblYesNo.TabIndex = 4
        Me.tblYesNo.Visible = False
        '
        'Yes_Button
        '
        Me.Yes_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Yes_Button.Location = New System.Drawing.Point(3, 3)
        Me.Yes_Button.Name = "Yes_Button"
        Me.Yes_Button.Size = New System.Drawing.Size(67, 23)
        Me.Yes_Button.TabIndex = 0
        Me.Yes_Button.Text = "&Yes"
        '
        'No_Button
        '
        Me.No_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.No_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.No_Button.Location = New System.Drawing.Point(76, 3)
        Me.No_Button.Name = "No_Button"
        Me.No_Button.Size = New System.Drawing.Size(67, 23)
        Me.No_Button.TabIndex = 1
        Me.No_Button.Text = "&No"
        '
        'TableLayoutPanel1
        '
        Me.TableLayoutPanel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TableLayoutPanel1.AutoSize = True
        Me.TableLayoutPanel1.ColumnCount = 2
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.TableLayoutPanel1.Controls.Add(Me.pbIcon, 1, 0)
        Me.TableLayoutPanel1.Controls.Add(Me.Panel, 0, 0)
        Me.TableLayoutPanel1.Location = New System.Drawing.Point(12, 12)
        Me.TableLayoutPanel1.Name = "TableLayoutPanel1"
        Me.TableLayoutPanel1.RowCount = 1
        Me.TableLayoutPanel1.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.TableLayoutPanel1.Size = New System.Drawing.Size(453, 143)
        Me.TableLayoutPanel1.TabIndex = 5
        '
        'pbIcon
        '
        Me.pbIcon.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.pbIcon.Location = New System.Drawing.Point(385, 39)
        Me.pbIcon.MinimumSize = New System.Drawing.Size(65, 65)
        Me.pbIcon.Name = "pbIcon"
        Me.pbIcon.Size = New System.Drawing.Size(65, 65)
        Me.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbIcon.TabIndex = 4
        Me.pbIcon.TabStop = False
        '
        'MyDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(477, 204)
        Me.Controls.Add(Me.TableLayoutPanel1)
        Me.Controls.Add(Me.tblYesNo)
        Me.Controls.Add(Me.tblOkCancel)
        Me.DoubleBuffered = True
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(835, 452)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(467, 175)
        Me.Name = "MyDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.tblOkCancel.ResumeLayout(False)
        Me.tblYesNo.ResumeLayout(False)
        Me.TableLayoutPanel1.ResumeLayout(False)
        Me.TableLayoutPanel1.PerformLayout()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tblOkCancel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents Panel As FlowLayoutPanel
    Friend WithEvents tblYesNo As TableLayoutPanel
    Friend WithEvents Yes_Button As Button
    Friend WithEvents No_Button As Button
    Friend WithEvents TableLayoutPanel1 As TableLayoutPanel
    Friend WithEvents pbIcon As PictureBox
End Class
