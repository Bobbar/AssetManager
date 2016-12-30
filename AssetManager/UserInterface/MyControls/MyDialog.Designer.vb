<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class MyDialog
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
        Me.tblOkCancel = New System.Windows.Forms.TableLayoutPanel()
        Me.OK_Button = New System.Windows.Forms.Button()
        Me.Cancel_Button = New System.Windows.Forms.Button()
        Me.pnlControls = New System.Windows.Forms.FlowLayoutPanel()
        Me.tblYesNo = New System.Windows.Forms.TableLayoutPanel()
        Me.Yes_Button = New System.Windows.Forms.Button()
        Me.No_Button = New System.Windows.Forms.Button()
        Me.pnlIcon = New System.Windows.Forms.Panel()
        Me.pbIcon = New System.Windows.Forms.PictureBox()
        Me.pnlControls_Main = New System.Windows.Forms.Panel()
        Me.pnlMaster = New System.Windows.Forms.Panel()
        Me.pnlButtons = New System.Windows.Forms.Panel()
        Me.tblOkCancel.SuspendLayout()
        Me.tblYesNo.SuspendLayout()
        Me.pnlIcon.SuspendLayout()
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.pnlControls_Main.SuspendLayout()
        Me.pnlMaster.SuspendLayout()
        Me.pnlButtons.SuspendLayout()
        Me.SuspendLayout()
        '
        'tblOkCancel
        '
        Me.tblOkCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tblOkCancel.AutoSize = True
        Me.tblOkCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tblOkCancel.ColumnCount = 2
        Me.tblOkCancel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tblOkCancel.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tblOkCancel.Controls.Add(Me.OK_Button, 0, 0)
        Me.tblOkCancel.Controls.Add(Me.Cancel_Button, 1, 0)
        Me.tblOkCancel.Location = New System.Drawing.Point(165, 6)
        Me.tblOkCancel.Name = "tblOkCancel"
        Me.tblOkCancel.RowCount = 1
        Me.tblOkCancel.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblOkCancel.Size = New System.Drawing.Size(162, 29)
        Me.tblOkCancel.TabIndex = 0
        '
        'OK_Button
        '
        Me.OK_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.OK_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.OK_Button.Location = New System.Drawing.Point(3, 3)
        Me.OK_Button.Name = "OK_Button"
        Me.OK_Button.Size = New System.Drawing.Size(75, 23)
        Me.OK_Button.TabIndex = 0
        Me.OK_Button.Text = "&OK"
        '
        'Cancel_Button
        '
        Me.Cancel_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Cancel_Button.DialogResult = System.Windows.Forms.DialogResult.No
        Me.Cancel_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cancel_Button.Location = New System.Drawing.Point(84, 3)
        Me.Cancel_Button.Name = "Cancel_Button"
        Me.Cancel_Button.Size = New System.Drawing.Size(75, 23)
        Me.Cancel_Button.TabIndex = 1
        Me.Cancel_Button.Text = "&Cancel"
        '
        'pnlControls
        '
        Me.pnlControls.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlControls.AutoScroll = True
        Me.pnlControls.AutoSize = True
        Me.pnlControls.BackColor = System.Drawing.SystemColors.Control
        Me.pnlControls.FlowDirection = System.Windows.Forms.FlowDirection.TopDown
        Me.pnlControls.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlControls.ForeColor = System.Drawing.Color.Black
        Me.pnlControls.Location = New System.Drawing.Point(3, 3)
        Me.pnlControls.Name = "pnlControls"
        Me.pnlControls.Padding = New System.Windows.Forms.Padding(10, 10, 10, 0)
        Me.pnlControls.Size = New System.Drawing.Size(336, 83)
        Me.pnlControls.TabIndex = 3
        '
        'tblYesNo
        '
        Me.tblYesNo.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.tblYesNo.AutoSize = True
        Me.tblYesNo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tblYesNo.ColumnCount = 2
        Me.tblYesNo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tblYesNo.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle())
        Me.tblYesNo.Controls.Add(Me.Yes_Button, 0, 0)
        Me.tblYesNo.Controls.Add(Me.No_Button, 1, 0)
        Me.tblYesNo.Location = New System.Drawing.Point(3, 6)
        Me.tblYesNo.Name = "tblYesNo"
        Me.tblYesNo.RowCount = 1
        Me.tblYesNo.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tblYesNo.Size = New System.Drawing.Size(162, 29)
        Me.tblYesNo.TabIndex = 4
        Me.tblYesNo.Visible = False
        '
        'Yes_Button
        '
        Me.Yes_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.Yes_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Yes_Button.Location = New System.Drawing.Point(3, 3)
        Me.Yes_Button.Name = "Yes_Button"
        Me.Yes_Button.Size = New System.Drawing.Size(75, 23)
        Me.Yes_Button.TabIndex = 0
        Me.Yes_Button.Text = "&Yes"
        '
        'No_Button
        '
        Me.No_Button.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.No_Button.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.No_Button.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.No_Button.Location = New System.Drawing.Point(84, 3)
        Me.No_Button.Name = "No_Button"
        Me.No_Button.Size = New System.Drawing.Size(75, 23)
        Me.No_Button.TabIndex = 1
        Me.No_Button.Text = "&No"
        '
        'pnlIcon
        '
        Me.pnlIcon.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlIcon.Controls.Add(Me.pbIcon)
        Me.pnlIcon.Location = New System.Drawing.Point(345, 3)
        Me.pnlIcon.Name = "pnlIcon"
        Me.pnlIcon.Size = New System.Drawing.Size(100, 90)
        Me.pnlIcon.TabIndex = 6
        '
        'pbIcon
        '
        Me.pbIcon.Anchor = System.Windows.Forms.AnchorStyles.None
        Me.pbIcon.BackColor = System.Drawing.SystemColors.Control
        Me.pbIcon.Location = New System.Drawing.Point(17, 10)
        Me.pbIcon.MinimumSize = New System.Drawing.Size(65, 65)
        Me.pbIcon.Name = "pbIcon"
        Me.pbIcon.Size = New System.Drawing.Size(70, 70)
        Me.pbIcon.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage
        Me.pbIcon.TabIndex = 4
        Me.pbIcon.TabStop = False
        '
        'pnlControls_Main
        '
        Me.pnlControls_Main.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlControls_Main.AutoSize = True
        Me.pnlControls_Main.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlControls_Main.BackColor = System.Drawing.SystemColors.Control
        Me.pnlControls_Main.Controls.Add(Me.pnlControls)
        Me.pnlControls_Main.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.pnlControls_Main.Location = New System.Drawing.Point(3, 4)
        Me.pnlControls_Main.MinimumSize = New System.Drawing.Size(330, 90)
        Me.pnlControls_Main.Name = "pnlControls_Main"
        Me.pnlControls_Main.Size = New System.Drawing.Size(340, 90)
        Me.pnlControls_Main.TabIndex = 7
        '
        'pnlMaster
        '
        Me.pnlMaster.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlMaster.AutoSize = True
        Me.pnlMaster.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlMaster.Controls.Add(Me.pnlButtons)
        Me.pnlMaster.Controls.Add(Me.pnlControls_Main)
        Me.pnlMaster.Controls.Add(Me.pnlIcon)
        Me.pnlMaster.Location = New System.Drawing.Point(9, 8)
        Me.pnlMaster.MinimumSize = New System.Drawing.Size(444, 135)
        Me.pnlMaster.Name = "pnlMaster"
        Me.pnlMaster.Size = New System.Drawing.Size(444, 137)
        Me.pnlMaster.TabIndex = 8
        '
        'pnlButtons
        '
        Me.pnlButtons.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.pnlButtons.AutoSize = True
        Me.pnlButtons.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.pnlButtons.Controls.Add(Me.tblYesNo)
        Me.pnlButtons.Controls.Add(Me.tblOkCancel)
        Me.pnlButtons.Cursor = System.Windows.Forms.Cursors.Default
        Me.pnlButtons.Location = New System.Drawing.Point(111, 96)
        Me.pnlButtons.Name = "pnlButtons"
        Me.pnlButtons.Size = New System.Drawing.Size(330, 38)
        Me.pnlButtons.TabIndex = 9
        '
        'MyDialog
        '
        Me.AcceptButton = Me.OK_Button
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoSize = True
        Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.CancelButton = Me.Cancel_Button
        Me.ClientSize = New System.Drawing.Size(461, 151)
        Me.Controls.Add(Me.pnlMaster)
        Me.DoubleBuffered = True
        Me.MaximizeBox = False
        Me.MaximumSize = New System.Drawing.Size(1068, 678)
        Me.MinimizeBox = False
        Me.MinimumSize = New System.Drawing.Size(477, 190)
        Me.Name = "MyDialog"
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.tblOkCancel.ResumeLayout(False)
        Me.tblYesNo.ResumeLayout(False)
        Me.pnlIcon.ResumeLayout(False)
        CType(Me.pbIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.pnlControls_Main.ResumeLayout(False)
        Me.pnlControls_Main.PerformLayout()
        Me.pnlMaster.ResumeLayout(False)
        Me.pnlMaster.PerformLayout()
        Me.pnlButtons.ResumeLayout(False)
        Me.pnlButtons.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tblOkCancel As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents OK_Button As System.Windows.Forms.Button
    Friend WithEvents Cancel_Button As System.Windows.Forms.Button
    Friend WithEvents pnlControls As FlowLayoutPanel
    Friend WithEvents tblYesNo As TableLayoutPanel
    Friend WithEvents Yes_Button As Button
    Friend WithEvents No_Button As Button
    Friend WithEvents pbIcon As PictureBox
    Friend WithEvents pnlIcon As Panel
    Friend WithEvents pnlControls_Main As Panel
    Friend WithEvents pnlMaster As Panel
    Friend WithEvents pnlButtons As Panel
End Class
