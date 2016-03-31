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
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.cmbEquipType = New System.Windows.Forms.ComboBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtPO = New System.Windows.Forms.TextBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.cmbOSType = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.SuspendLayout()
        '
        'txtSerial
        '
        Me.txtSerial.Location = New System.Drawing.Point(15, 29)
        Me.txtSerial.Name = "txtSerial"
        Me.txtSerial.Size = New System.Drawing.Size(115, 20)
        Me.txtSerial.TabIndex = 0
        Me.txtSerial.Text = "txtSerial"
        '
        'txtAssetTag
        '
        Me.txtAssetTag.Location = New System.Drawing.Point(15, 81)
        Me.txtAssetTag.Name = "txtAssetTag"
        Me.txtAssetTag.Size = New System.Drawing.Size(114, 20)
        Me.txtAssetTag.TabIndex = 1
        Me.txtAssetTag.Text = "txtAssetTag"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(12, 9)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 13)
        Me.Label1.TabIndex = 2
        Me.Label1.Text = "Serial"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(12, 61)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Asset Tag"
        '
        'txtCurUser
        '
        Me.txtCurUser.Location = New System.Drawing.Point(160, 29)
        Me.txtCurUser.Name = "txtCurUser"
        Me.txtCurUser.Size = New System.Drawing.Size(121, 20)
        Me.txtCurUser.TabIndex = 4
        Me.txtCurUser.Text = "txtCurUser"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(160, 81)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(251, 20)
        Me.txtDescription.TabIndex = 5
        Me.txtDescription.Text = "Description"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(157, 9)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 13)
        Me.Label3.TabIndex = 6
        Me.Label3.Text = "User"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(157, 61)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 13)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Description"
        '
        'cmbLocation
        '
        Me.cmbLocation.FormattingEnabled = True
        Me.cmbLocation.Location = New System.Drawing.Point(325, 29)
        Me.cmbLocation.Name = "cmbLocation"
        Me.cmbLocation.Size = New System.Drawing.Size(171, 21)
        Me.cmbLocation.TabIndex = 8
        Me.cmbLocation.Text = "cmbLocation"
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(322, 11)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(48, 13)
        Me.Label5.TabIndex = 9
        Me.Label5.Text = "Location"
        '
        'dtPurchaseDate
        '
        Me.dtPurchaseDate.CustomFormat = "yyyy-MM-dd"
        Me.dtPurchaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtPurchaseDate.Location = New System.Drawing.Point(437, 81)
        Me.dtPurchaseDate.Name = "dtPurchaseDate"
        Me.dtPurchaseDate.Size = New System.Drawing.Size(169, 20)
        Me.dtPurchaseDate.TabIndex = 10
        '
        'lbPurchaseDate
        '
        Me.lbPurchaseDate.AutoSize = True
        Me.lbPurchaseDate.Location = New System.Drawing.Point(434, 64)
        Me.lbPurchaseDate.Name = "lbPurchaseDate"
        Me.lbPurchaseDate.Size = New System.Drawing.Size(78, 13)
        Me.lbPurchaseDate.TabIndex = 11
        Me.lbPurchaseDate.Text = "Purchase Date"
        '
        'txtReplaceYear
        '
        Me.txtReplaceYear.Location = New System.Drawing.Point(537, 30)
        Me.txtReplaceYear.Name = "txtReplaceYear"
        Me.txtReplaceYear.Size = New System.Drawing.Size(88, 20)
        Me.txtReplaceYear.TabIndex = 12
        Me.txtReplaceYear.Text = "txtReplaceYear"
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(534, 9)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(72, 13)
        Me.Label6.TabIndex = 13
        Me.Label6.Text = "Replace Year"
        '
        'cmdAdd
        '
        Me.cmdAdd.Location = New System.Drawing.Point(325, 285)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(97, 44)
        Me.cmdAdd.TabIndex = 14
        Me.cmdAdd.Text = "Add"
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'txtNotes
        '
        Me.txtNotes.Location = New System.Drawing.Point(74, 214)
        Me.txtNotes.MaxLength = 200
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.Size = New System.Drawing.Size(551, 52)
        Me.txtNotes.TabIndex = 15
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(30, 226)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(38, 13)
        Me.Label7.TabIndex = 16
        Me.Label7.Text = "Notes:"
        '
        'cmbEquipType
        '
        Me.cmbEquipType.FormattingEnabled = True
        Me.cmbEquipType.Location = New System.Drawing.Point(669, 30)
        Me.cmbEquipType.Name = "cmbEquipType"
        Me.cmbEquipType.Size = New System.Drawing.Size(144, 21)
        Me.cmbEquipType.TabIndex = 17
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(666, 11)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(84, 13)
        Me.Label8.TabIndex = 18
        Me.Label8.Text = "Equipment Type"
        '
        'txtPO
        '
        Me.txtPO.Location = New System.Drawing.Point(793, 81)
        Me.txtPO.Name = "txtPO"
        Me.txtPO.Size = New System.Drawing.Size(122, 20)
        Me.txtPO.TabIndex = 19
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(790, 64)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 13)
        Me.Label9.TabIndex = 20
        Me.Label9.Text = "PO Number"
        '
        'cmbOSType
        '
        Me.cmbOSType.FormattingEnabled = True
        Me.cmbOSType.Location = New System.Drawing.Point(632, 82)
        Me.cmbOSType.Name = "cmbOSType"
        Me.cmbOSType.Size = New System.Drawing.Size(136, 21)
        Me.cmbOSType.TabIndex = 21
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(629, 66)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(90, 13)
        Me.Label10.TabIndex = 22
        Me.Label10.Text = "Operating System"
        '
        'AddNew
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(940, 341)
        Me.Controls.Add(Me.Label10)
        Me.Controls.Add(Me.cmbOSType)
        Me.Controls.Add(Me.Label9)
        Me.Controls.Add(Me.txtPO)
        Me.Controls.Add(Me.Label8)
        Me.Controls.Add(Me.cmbEquipType)
        Me.Controls.Add(Me.Label7)
        Me.Controls.Add(Me.txtNotes)
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
    Friend WithEvents txtNotes As TextBox
    Friend WithEvents Label7 As Label
    Friend WithEvents cmbEquipType As ComboBox
    Friend WithEvents Label8 As Label
    Friend WithEvents txtPO As TextBox
    Friend WithEvents Label9 As Label
    Friend WithEvents cmbOSType As ComboBox
    Friend WithEvents Label10 As Label
End Class
