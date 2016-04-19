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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddNew))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbOSType = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtPO = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbEquipType = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtReplaceYear = New System.Windows.Forms.TextBox()
        Me.lbPurchaseDate = New System.Windows.Forms.Label()
        Me.dtPurchaseDate = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbLocation = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.txtCurUser = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtAssetTag = New System.Windows.Forms.TextBox()
        Me.txtSerial = New System.Windows.Forms.TextBox()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cmbStatus = New System.Windows.Forms.ComboBox()
        Me.cmdClear = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.Controls.Add(Me.cmdClear)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.cmbStatus)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.cmbOSType)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.txtPO)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.cmbEquipType)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.txtNotes)
        Me.GroupBox1.Controls.Add(Me.cmdAdd)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtReplaceYear)
        Me.GroupBox1.Controls.Add(Me.lbPurchaseDate)
        Me.GroupBox1.Controls.Add(Me.dtPurchaseDate)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.cmbLocation)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtDescription)
        Me.GroupBox1.Controls.Add(Me.txtCurUser)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtAssetTag)
        Me.GroupBox1.Controls.Add(Me.txtSerial)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(965, 253)
        Me.GroupBox1.TabIndex = 23
        Me.GroupBox1.TabStop = False
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Location = New System.Drawing.Point(625, 76)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(90, 13)
        Me.Label10.TabIndex = 45
        Me.Label10.Text = "Operating System"
        '
        'cmbOSType
        '
        Me.cmbOSType.FormattingEnabled = True
        Me.cmbOSType.Location = New System.Drawing.Point(628, 92)
        Me.cmbOSType.Name = "cmbOSType"
        Me.cmbOSType.Size = New System.Drawing.Size(136, 21)
        Me.cmbOSType.TabIndex = 44
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Location = New System.Drawing.Point(786, 74)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(62, 13)
        Me.Label9.TabIndex = 43
        Me.Label9.Text = "PO Number"
        '
        'txtPO
        '
        Me.txtPO.Location = New System.Drawing.Point(789, 91)
        Me.txtPO.Name = "txtPO"
        Me.txtPO.Size = New System.Drawing.Size(122, 20)
        Me.txtPO.TabIndex = 42
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Location = New System.Drawing.Point(625, 21)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(84, 13)
        Me.Label8.TabIndex = 41
        Me.Label8.Text = "Equipment Type"
        '
        'cmbEquipType
        '
        Me.cmbEquipType.FormattingEnabled = True
        Me.cmbEquipType.Location = New System.Drawing.Point(628, 40)
        Me.cmbEquipType.Name = "cmbEquipType"
        Me.cmbEquipType.Size = New System.Drawing.Size(144, 21)
        Me.cmbEquipType.TabIndex = 40
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Location = New System.Drawing.Point(129, 152)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(38, 13)
        Me.Label7.TabIndex = 39
        Me.Label7.Text = "Notes:"
        '
        'txtNotes
        '
        Me.txtNotes.Location = New System.Drawing.Point(173, 133)
        Me.txtNotes.MaxLength = 200
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.Size = New System.Drawing.Size(551, 52)
        Me.txtNotes.TabIndex = 38
        '
        'cmdAdd
        '
        Me.cmdAdd.Location = New System.Drawing.Point(411, 191)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(97, 44)
        Me.cmdAdd.TabIndex = 37
        Me.cmdAdd.Text = "Add"
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Location = New System.Drawing.Point(511, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(72, 13)
        Me.Label6.TabIndex = 36
        Me.Label6.Text = "Replace Year"
        '
        'txtReplaceYear
        '
        Me.txtReplaceYear.Location = New System.Drawing.Point(514, 40)
        Me.txtReplaceYear.Name = "txtReplaceYear"
        Me.txtReplaceYear.Size = New System.Drawing.Size(88, 20)
        Me.txtReplaceYear.TabIndex = 35
        Me.txtReplaceYear.Text = "txtReplaceYear"
        '
        'lbPurchaseDate
        '
        Me.lbPurchaseDate.AutoSize = True
        Me.lbPurchaseDate.Location = New System.Drawing.Point(430, 74)
        Me.lbPurchaseDate.Name = "lbPurchaseDate"
        Me.lbPurchaseDate.Size = New System.Drawing.Size(78, 13)
        Me.lbPurchaseDate.TabIndex = 34
        Me.lbPurchaseDate.Text = "Purchase Date"
        '
        'dtPurchaseDate
        '
        Me.dtPurchaseDate.CustomFormat = "yyyy-MM-dd"
        Me.dtPurchaseDate.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtPurchaseDate.Location = New System.Drawing.Point(433, 91)
        Me.dtPurchaseDate.Name = "dtPurchaseDate"
        Me.dtPurchaseDate.Size = New System.Drawing.Size(169, 20)
        Me.dtPurchaseDate.TabIndex = 33
        Me.dtPurchaseDate.Value = New Date(2016, 4, 14, 0, 0, 0, 0)
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Location = New System.Drawing.Point(318, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(48, 13)
        Me.Label5.TabIndex = 32
        Me.Label5.Text = "Location"
        '
        'cmbLocation
        '
        Me.cmbLocation.FormattingEnabled = True
        Me.cmbLocation.Location = New System.Drawing.Point(321, 39)
        Me.cmbLocation.Name = "cmbLocation"
        Me.cmbLocation.Size = New System.Drawing.Size(171, 21)
        Me.cmbLocation.TabIndex = 31
        Me.cmbLocation.Text = "cmbLocation"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Location = New System.Drawing.Point(153, 71)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(60, 13)
        Me.Label4.TabIndex = 30
        Me.Label4.Text = "Description"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Location = New System.Drawing.Point(153, 19)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(29, 13)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "User"
        '
        'txtDescription
        '
        Me.txtDescription.Location = New System.Drawing.Point(156, 91)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.Size = New System.Drawing.Size(251, 20)
        Me.txtDescription.TabIndex = 28
        Me.txtDescription.Text = "Description"
        '
        'txtCurUser
        '
        Me.txtCurUser.Location = New System.Drawing.Point(156, 39)
        Me.txtCurUser.Name = "txtCurUser"
        Me.txtCurUser.Size = New System.Drawing.Size(121, 20)
        Me.txtCurUser.TabIndex = 27
        Me.txtCurUser.Text = "txtCurUser"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Location = New System.Drawing.Point(8, 71)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(55, 13)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "Asset Tag"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(8, 19)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(33, 13)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Serial"
        '
        'txtAssetTag
        '
        Me.txtAssetTag.Location = New System.Drawing.Point(11, 91)
        Me.txtAssetTag.Name = "txtAssetTag"
        Me.txtAssetTag.Size = New System.Drawing.Size(114, 20)
        Me.txtAssetTag.TabIndex = 24
        Me.txtAssetTag.Text = "txtAssetTag"
        '
        'txtSerial
        '
        Me.txtSerial.Location = New System.Drawing.Point(11, 39)
        Me.txtSerial.Name = "txtSerial"
        Me.txtSerial.Size = New System.Drawing.Size(115, 20)
        Me.txtSerial.TabIndex = 23
        Me.txtSerial.Text = "txtSerial"
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Location = New System.Drawing.Point(786, 19)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(74, 13)
        Me.Label11.TabIndex = 47
        Me.Label11.Text = "Device Status"
        '
        'cmbStatus
        '
        Me.cmbStatus.FormattingEnabled = True
        Me.cmbStatus.Location = New System.Drawing.Point(789, 38)
        Me.cmbStatus.Name = "cmbStatus"
        Me.cmbStatus.Size = New System.Drawing.Size(144, 21)
        Me.cmbStatus.TabIndex = 46
        '
        'cmdClear
        '
        Me.cmdClear.Location = New System.Drawing.Point(836, 202)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(97, 23)
        Me.cmdClear.TabIndex = 48
        Me.cmdClear.Text = "Clear"
        Me.cmdClear.UseVisualStyleBackColor = True
        '
        'AddNew
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(991, 272)
        Me.Controls.Add(Me.GroupBox1)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "AddNew"
        Me.Text = "Add New Device"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        Me.ResumeLayout(False)
    End Sub
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label10 As Label
    Friend WithEvents cmbOSType As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtPO As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents cmbEquipType As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtNotes As TextBox
    Friend WithEvents cmdAdd As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents txtReplaceYear As TextBox
    Friend WithEvents lbPurchaseDate As Label
    Friend WithEvents dtPurchaseDate As DateTimePicker
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbLocation As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents txtCurUser As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtAssetTag As TextBox
    Friend WithEvents txtSerial As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents cmbStatus As ComboBox
    Friend WithEvents cmdClear As Button
End Class
