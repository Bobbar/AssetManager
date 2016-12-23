<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class frmEncrypter
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
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.cmdClear = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtResult = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtKey = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtString = New System.Windows.Forms.TextBox()
        Me.cmdEncode = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmdClear)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtResult)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.txtKey)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtString)
        Me.GroupBox1.Controls.Add(Me.cmdEncode)
        Me.GroupBox1.Location = New System.Drawing.Point(10, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(490, 328)
        Me.GroupBox1.TabIndex = 0
        Me.GroupBox1.TabStop = False
        Me.GroupBox1.Text = "Encode Text"
        '
        'cmdClear
        '
        Me.cmdClear.Location = New System.Drawing.Point(169, 267)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(141, 30)
        Me.cmdClear.TabIndex = 7
        Me.cmdClear.Text = "Clear"
        Me.cmdClear.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(31, 159)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "Result"
        '
        'txtResult
        '
        Me.txtResult.Location = New System.Drawing.Point(34, 175)
        Me.txtResult.Name = "txtResult"
        Me.txtResult.Size = New System.Drawing.Size(421, 20)
        Me.txtResult.TabIndex = 5
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(31, 77)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(25, 13)
        Me.Label2.TabIndex = 4
        Me.Label2.Text = "Key"
        '
        'txtKey
        '
        Me.txtKey.Location = New System.Drawing.Point(34, 93)
        Me.txtKey.Name = "txtKey"
        Me.txtKey.Size = New System.Drawing.Size(421, 20)
        Me.txtKey.TabIndex = 3
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(31, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(34, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "String"
        '
        'txtString
        '
        Me.txtString.Location = New System.Drawing.Point(34, 42)
        Me.txtString.Name = "txtString"
        Me.txtString.Size = New System.Drawing.Size(421, 20)
        Me.txtString.TabIndex = 1
        '
        'cmdEncode
        '
        Me.cmdEncode.Location = New System.Drawing.Point(169, 212)
        Me.cmdEncode.Name = "cmdEncode"
        Me.cmdEncode.Size = New System.Drawing.Size(141, 49)
        Me.cmdEncode.TabIndex = 0
        Me.cmdEncode.Text = "Go!"
        Me.cmdEncode.UseVisualStyleBackColor = True
        '
        'frmEncrypter
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.AutoScroll = True
        Me.ClientSize = New System.Drawing.Size(515, 352)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Name = "frmEncrypter"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
        Me.Text = "Encoder"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtResult As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtKey As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtString As TextBox
    Friend WithEvents cmdEncode As Button
    Friend WithEvents cmdClear As Button
End Class
