<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class SibiNotesForm
    Inherits ExtendedForm
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
        Me.rtbNotes = New System.Windows.Forms.RichTextBox()
        Me.cmdClose = New System.Windows.Forms.Button()
        Me.cmdOK = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'rtbNotes
        '
        Me.rtbNotes.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtbNotes.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rtbNotes.Location = New System.Drawing.Point(12, 12)
        Me.rtbNotes.Name = "rtbNotes"
        Me.rtbNotes.Size = New System.Drawing.Size(448, 205)
        Me.rtbNotes.TabIndex = 0
        Me.rtbNotes.Text = ""
        '
        'cmdClose
        '
        Me.cmdClose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdClose.Location = New System.Drawing.Point(365, 234)
        Me.cmdClose.Name = "cmdClose"
        Me.cmdClose.Size = New System.Drawing.Size(95, 30)
        Me.cmdClose.TabIndex = 1
        Me.cmdClose.Text = "Close"
        Me.cmdClose.UseVisualStyleBackColor = True
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.Location = New System.Drawing.Point(12, 234)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(95, 30)
        Me.cmdOK.TabIndex = 2
        Me.cmdOK.Text = "OK"
        Me.cmdOK.UseVisualStyleBackColor = True
        '
        'SibiNotesForm
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(472, 276)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.cmdClose)
        Me.Controls.Add(Me.rtbNotes)
        Me.MinimumSize = New System.Drawing.Size(253, 160)
        Me.Name = "SibiNotesForm"
        Me.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Show
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Sibi Note"
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents rtbNotes As RichTextBox
    Friend WithEvents cmdClose As Button
    Friend WithEvents cmdOK As Button
End Class
