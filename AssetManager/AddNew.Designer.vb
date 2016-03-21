<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class AddNew
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
        Me.txtSerial = New System.Windows.Forms.TextBox()
        Me.txtAssetTag = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtCurUser = New System.Windows.Forms.TextBox()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.cmbLocation = New System.Windows.Forms.ComboBox()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtPurchaseDate = New System.Windows.Forms.DateTimePicker()
        Me.lbPurchaseDate = New System.Windows.Forms.Label()
        Me.txtReplaceYear = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.SuspendLayout()
        '
        'txtSerial
        '
        Me.txtSerial.Location = New System.Drawing.Point(35, 49)
        Me.txtSerial.Name = "txtSerial"
        Me.txtSerial.Size = New System.Drawing.Size(115, 20)
        Me.txtSerial.TabIndex = 0
        Me.txtSerial.Text = "txtSerial"
        '
        'txtAssetTag
        '
        Me.txtAssetTag.Location = New System.Drawing.Point(35, 101)
        Me.txtAssetTag.Name = "txtAssetTag"
        Me.txtAssetTag.Size = New System.Drawing.Size(114, 20)
        Me.txtAssetTag.TabIndex = 1
        Me.txtAssetTag.Text = "txtAssetTag"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(32, 29)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Serial"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(32, 81)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Asset Tag"
        '
        'txtCurUser
        '
        Me.txtCurUser.Location = New System.Drawing.Point(196, 49)
        Me.txtCurUser.Name = "txtCurUser"
        Me.txtCurUser.Size = New System.Drawing.Size(121, 20)
        Me.txtCurUser.TabIndex = 4
        Me.txtCurUser.Text = "txtCurUser"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(196, 101)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(251, 20)
        Me.txtDescription.TabIndex = 5
        Me.txtDescription.Text = "Description"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(193, 29)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "User"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(193, 81)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Description"
        '
        'cmbLocation
        '
        Me.cmbLocation.FormattingEnabled = True
        Me.cmbLocation.Location = New System.Drawing.Point(377, 49)
        Me.cmbLocation.Name = "cmbLocation"
        Me.cmbLocation.Size = New System.Drawing.Size(171, 21)
        Me.cmbLocation.TabIndex = 8
        Me.cmbLocation.Text = "cmbLocation"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(374, 29)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(48, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Location"
        '
        'dtPurchaseDate
        '
        Me.dtPurchaseDate.Format = System.Windows.Forms.DateTimePickerFormat.[Short]
        Me.dtPurchaseDate.Location = New System.Drawing.Point(499, 101)
        Me.dtPurchaseDate.Name = "dtPurchaseDate"
        Me.dtPurchaseDate.Size = New System.Drawing.Size(169, 20)
        Me.dtPurchaseDate.TabIndex = 10
        '
        'lbPurchaseDate
        '
        Me.lbPurchaseDate.AutoSize = True
        Me.lbPurchaseDate.Location = New System.Drawing.Point(496, 81)
        Me.lbPurchaseDate.Name = "lbPurchaseDate"
        Me.lbPurchaseDate.Size = New System.Drawing.Size(78, 13)
        Me.lbPurchaseDate.TabIndex = 11
        Me.lbPurchaseDate.Text = "Purchase Date"
        '
        'txtReplaceYear
        '
        Me.txtReplaceYear.Location = New System.Drawing.Point(602, 50)
        Me.txtReplaceYear.Name = "txtReplaceYear"
        Me.txtReplaceYear.Size = New System.Drawing.Size(158, 20)
        Me.txtReplaceYear.TabIndex = 12
        Me.txtReplaceYear.Text = "txtReplaceYear"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(599, 29)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(72, 13)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Replace Year"
        '
        'cmdAdd
        '
        Me.cmdAdd.Location = New System.Drawing.Point(350, 164)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(97, 44)
        Me.cmdAdd.TabIndex = 14
        Me.cmdAdd.Text = "Add"
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'AddNew
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(812, 261)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.Label6)
        Me.Controls.Add(Me.txtReplaceYear)
        Me.Controls.Add(Me.lbPurchaseDate)
        Me.Controls.Add(Me.dtPurchaseDate)
        Me.Controls.Add(Me.Label5)
        Me.Controls.Add(Me.cmbLocation)
        Me.Controls.Add(Me.Label4)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.txtDescription)
        Me.Controls.Add(Me.txtCurUser)
        Me.Controls.Add(Me.Label2)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtAssetTag)
        Me.Controls.Add(Me.txtSerial)
        Me.Name = "AddNew"
        Me.Text = "AddNew"
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents txtSerial As TextBox
    Friend WithEvents txtAssetTag As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents Label2 As Label
    Friend WithEvents txtCurUser As TextBox
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents Label4 As Label
    Friend WithEvents cmbLocation As ComboBox
    Friend WithEvents Label5 As Label
    Friend WithEvents dtPurchaseDate As DateTimePicker
    Friend WithEvents lbPurchaseDate As Label
    Friend WithEvents txtReplaceYear As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents cmdAdd As Button
End Class
