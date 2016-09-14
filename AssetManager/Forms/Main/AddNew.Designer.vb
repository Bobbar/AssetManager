<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class AddNew
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(AddNew))
        Me.GroupBox1 = New System.Windows.Forms.GroupBox()
        Me.chkNoClear = New System.Windows.Forms.CheckBox()
        Me.chkTrackable = New System.Windows.Forms.CheckBox()
        Me.cmdClear = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.cmbStatus_REQ = New System.Windows.Forms.ComboBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.cmbOSType_REQ = New System.Windows.Forms.ComboBox()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtPO = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.cmbEquipType_REQ = New System.Windows.Forms.ComboBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtReplaceYear = New System.Windows.Forms.TextBox()
        Me.lbPurchaseDate = New System.Windows.Forms.Label()
        Me.dtPurchaseDate_REQ = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbLocation_REQ = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDescription_REQ = New System.Windows.Forms.TextBox()
        Me.txtCurUser_REQ = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtAssetTag_REQ = New System.Windows.Forms.TextBox()
        Me.txtSerial_REQ = New System.Windows.Forms.TextBox()
        Me.fieldErrorIcon = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.cmdUserSearch = New System.Windows.Forms.Button()
        Me.GroupBox1.SuspendLayout()
        CType(Me.fieldErrorIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'GroupBox1
        '
        Me.GroupBox1.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.GroupBox1.Controls.Add(Me.cmdUserSearch)
        Me.GroupBox1.Controls.Add(Me.chkNoClear)
        Me.GroupBox1.Controls.Add(Me.chkTrackable)
        Me.GroupBox1.Controls.Add(Me.cmdClear)
        Me.GroupBox1.Controls.Add(Me.Label11)
        Me.GroupBox1.Controls.Add(Me.cmbStatus_REQ)
        Me.GroupBox1.Controls.Add(Me.Label10)
        Me.GroupBox1.Controls.Add(Me.cmbOSType_REQ)
        Me.GroupBox1.Controls.Add(Me.Label9)
        Me.GroupBox1.Controls.Add(Me.txtPO)
        Me.GroupBox1.Controls.Add(Me.Label8)
        Me.GroupBox1.Controls.Add(Me.cmbEquipType_REQ)
        Me.GroupBox1.Controls.Add(Me.Label7)
        Me.GroupBox1.Controls.Add(Me.txtNotes)
        Me.GroupBox1.Controls.Add(Me.cmdAdd)
        Me.GroupBox1.Controls.Add(Me.Label6)
        Me.GroupBox1.Controls.Add(Me.txtReplaceYear)
        Me.GroupBox1.Controls.Add(Me.lbPurchaseDate)
        Me.GroupBox1.Controls.Add(Me.dtPurchaseDate_REQ)
        Me.GroupBox1.Controls.Add(Me.Label5)
        Me.GroupBox1.Controls.Add(Me.cmbLocation_REQ)
        Me.GroupBox1.Controls.Add(Me.Label4)
        Me.GroupBox1.Controls.Add(Me.Label3)
        Me.GroupBox1.Controls.Add(Me.txtDescription_REQ)
        Me.GroupBox1.Controls.Add(Me.txtCurUser_REQ)
        Me.GroupBox1.Controls.Add(Me.Label2)
        Me.GroupBox1.Controls.Add(Me.Label1)
        Me.GroupBox1.Controls.Add(Me.txtAssetTag_REQ)
        Me.GroupBox1.Controls.Add(Me.txtSerial_REQ)
        Me.GroupBox1.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox1.Name = "GroupBox1"
        Me.GroupBox1.Size = New System.Drawing.Size(1023, 333)
        Me.GroupBox1.TabIndex = 23
        Me.GroupBox1.TabStop = False
        '
        'chkNoClear
        '
        Me.chkNoClear.AutoSize = True
        Me.chkNoClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNoClear.Location = New System.Drawing.Point(543, 267)
        Me.chkNoClear.Name = "chkNoClear"
        Me.chkNoClear.Size = New System.Drawing.Size(91, 20)
        Me.chkNoClear.TabIndex = 49
        Me.chkNoClear.Text = "Don't clear"
        Me.chkNoClear.UseVisualStyleBackColor = True
        '
        'chkTrackable
        '
        Me.chkTrackable.AutoSize = True
        Me.chkTrackable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTrackable.Location = New System.Drawing.Point(739, 174)
        Me.chkTrackable.Name = "chkTrackable"
        Me.chkTrackable.Size = New System.Drawing.Size(89, 20)
        Me.chkTrackable.TabIndex = 48
        Me.chkTrackable.Text = "Trackable"
        Me.chkTrackable.UseVisualStyleBackColor = True
        '
        'cmdClear
        '
        Me.cmdClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClear.Location = New System.Drawing.Point(872, 275)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(97, 23)
        Me.cmdClear.TabIndex = 13
        Me.cmdClear.Text = "Clear"
        Me.cmdClear.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(822, 20)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(91, 16)
        Me.Label11.TabIndex = 47
        Me.Label11.Text = "Device Status"
        '
        'cmbStatus_REQ
        '
        Me.cmbStatus_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStatus_REQ.FormattingEnabled = True
        Me.cmbStatus_REQ.Location = New System.Drawing.Point(825, 39)
        Me.cmbStatus_REQ.Name = "cmbStatus_REQ"
        Me.cmbStatus_REQ.Size = New System.Drawing.Size(144, 26)
        Me.cmbStatus_REQ.TabIndex = 5
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(639, 102)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(115, 16)
        Me.Label10.TabIndex = 45
        Me.Label10.Text = "Operating System"
        '
        'cmbOSType_REQ
        '
        Me.cmbOSType_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbOSType_REQ.FormattingEnabled = True
        Me.cmbOSType_REQ.Location = New System.Drawing.Point(642, 120)
        Me.cmbOSType_REQ.Name = "cmbOSType_REQ"
        Me.cmbOSType_REQ.Size = New System.Drawing.Size(144, 26)
        Me.cmbOSType_REQ.TabIndex = 9
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(822, 102)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(78, 16)
        Me.Label9.TabIndex = 43
        Me.Label9.Text = "PO Number"
        '
        'txtPO
        '
        Me.txtPO.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPO.Location = New System.Drawing.Point(825, 121)
        Me.txtPO.Name = "txtPO"
        Me.txtPO.Size = New System.Drawing.Size(122, 25)
        Me.txtPO.TabIndex = 10
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(639, 22)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(107, 16)
        Me.Label8.TabIndex = 41
        Me.Label8.Text = "Equipment Type"
        '
        'cmbEquipType_REQ
        '
        Me.cmbEquipType_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbEquipType_REQ.FormattingEnabled = True
        Me.cmbEquipType_REQ.Location = New System.Drawing.Point(642, 39)
        Me.cmbEquipType_REQ.Name = "cmbEquipType_REQ"
        Me.cmbEquipType_REQ.Size = New System.Drawing.Size(144, 26)
        Me.cmbEquipType_REQ.TabIndex = 4
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(251, 190)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(47, 16)
        Me.Label7.TabIndex = 39
        Me.Label7.Text = "Notes:"
        '
        'txtNotes
        '
        Me.txtNotes.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNotes.Location = New System.Drawing.Point(304, 161)
        Me.txtNotes.MaxLength = 200
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.Size = New System.Drawing.Size(330, 69)
        Me.txtNotes.TabIndex = 11
        '
        'cmdAdd
        '
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.Location = New System.Drawing.Point(419, 254)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(97, 44)
        Me.cmdAdd.TabIndex = 12
        Me.cmdAdd.Text = "Add"
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(539, 20)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(92, 16)
        Me.Label6.TabIndex = 36
        Me.Label6.Text = "Replace Year"
        '
        'txtReplaceYear
        '
        Me.txtReplaceYear.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReplaceYear.Location = New System.Drawing.Point(543, 39)
        Me.txtReplaceYear.Name = "txtReplaceYear"
        Me.txtReplaceYear.Size = New System.Drawing.Size(88, 25)
        Me.txtReplaceYear.TabIndex = 3
        Me.txtReplaceYear.Text = "txtReplaceYear"
        '
        'lbPurchaseDate
        '
        Me.lbPurchaseDate.AutoSize = True
        Me.lbPurchaseDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbPurchaseDate.Location = New System.Drawing.Point(436, 102)
        Me.lbPurchaseDate.Name = "lbPurchaseDate"
        Me.lbPurchaseDate.Size = New System.Drawing.Size(97, 16)
        Me.lbPurchaseDate.TabIndex = 34
        Me.lbPurchaseDate.Text = "Purchase Date"
        '
        'dtPurchaseDate_REQ
        '
        Me.dtPurchaseDate_REQ.CustomFormat = "yyyy-MM-dd"
        Me.dtPurchaseDate_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtPurchaseDate_REQ.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtPurchaseDate_REQ.Location = New System.Drawing.Point(439, 121)
        Me.dtPurchaseDate_REQ.Name = "dtPurchaseDate_REQ"
        Me.dtPurchaseDate_REQ.Size = New System.Drawing.Size(169, 25)
        Me.dtPurchaseDate_REQ.TabIndex = 8
        Me.dtPurchaseDate_REQ.Value = New Date(2016, 4, 14, 0, 0, 0, 0)
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(331, 21)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(59, 16)
        Me.Label5.TabIndex = 32
        Me.Label5.Text = "Location"
        '
        'cmbLocation_REQ
        '
        Me.cmbLocation_REQ.DropDownWidth = 171
        Me.cmbLocation_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbLocation_REQ.FormattingEnabled = True
        Me.cmbLocation_REQ.Location = New System.Drawing.Point(334, 38)
        Me.cmbLocation_REQ.Name = "cmbLocation_REQ"
        Me.cmbLocation_REQ.Size = New System.Drawing.Size(171, 26)
        Me.cmbLocation_REQ.TabIndex = 2
        Me.cmbLocation_REQ.Text = "cmbLocation"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(153, 102)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 16)
        Me.Label4.TabIndex = 30
        Me.Label4.Text = "Description"
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(153, 20)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 16)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "User"
        '
        'txtDescription_REQ
        '
        Me.txtDescription_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription_REQ.Location = New System.Drawing.Point(156, 121)
        Me.txtDescription_REQ.Name = "txtDescription_REQ"
        Me.txtDescription_REQ.Size = New System.Drawing.Size(251, 25)
        Me.txtDescription_REQ.TabIndex = 7
        Me.txtDescription_REQ.Text = "Description"
        '
        'txtCurUser_REQ
        '
        Me.txtCurUser_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurUser_REQ.Location = New System.Drawing.Point(156, 39)
        Me.txtCurUser_REQ.Name = "txtCurUser_REQ"
        Me.txtCurUser_REQ.Size = New System.Drawing.Size(142, 25)
        Me.txtCurUser_REQ.TabIndex = 1
        Me.txtCurUser_REQ.Text = "txtCurUser"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(9, 102)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 16)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "Asset Tag"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 20)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 16)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Serial"
        '
        'txtAssetTag_REQ
        '
        Me.txtAssetTag_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAssetTag_REQ.Location = New System.Drawing.Point(12, 121)
        Me.txtAssetTag_REQ.Name = "txtAssetTag_REQ"
        Me.txtAssetTag_REQ.Size = New System.Drawing.Size(114, 25)
        Me.txtAssetTag_REQ.TabIndex = 6
        Me.txtAssetTag_REQ.Text = "txtAssetTag"
        '
        'txtSerial_REQ
        '
        Me.txtSerial_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerial_REQ.Location = New System.Drawing.Point(12, 39)
        Me.txtSerial_REQ.Name = "txtSerial_REQ"
        Me.txtSerial_REQ.Size = New System.Drawing.Size(115, 25)
        Me.txtSerial_REQ.TabIndex = 0
        Me.txtSerial_REQ.Text = "txtSerial"
        '
        'fieldErrorIcon
        '
        Me.fieldErrorIcon.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.fieldErrorIcon.ContainerControl = Me
        Me.fieldErrorIcon.Icon = CType(resources.GetObject("fieldErrorIcon.Icon"), System.Drawing.Icon)
        '
        'cmdUserSearch
        '
        Me.cmdUserSearch.Location = New System.Drawing.Point(156, 69)
        Me.cmdUserSearch.Name = "cmdUserSearch"
        Me.cmdUserSearch.Size = New System.Drawing.Size(141, 23)
        Me.cmdUserSearch.TabIndex = 50
        Me.cmdUserSearch.Text = "Munis Link"
        Me.cmdUserSearch.UseVisualStyleBackColor = True
        '
        'AddNew
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(1047, 357)
        Me.Controls.Add(Me.GroupBox1)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "AddNew"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add New Device"
        Me.GroupBox1.ResumeLayout(False)
        Me.GroupBox1.PerformLayout()
        CType(Me.fieldErrorIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub
    Friend WithEvents GroupBox1 As GroupBox
    Friend WithEvents Label10 As Label
    Friend WithEvents cmbOSType_REQ As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtPO As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents cmbEquipType_REQ As ComboBox
    Friend WithEvents Label7 As Label
    Friend WithEvents txtNotes As TextBox
    Friend WithEvents cmdAdd As Button
    Friend WithEvents Label6 As Label
    Friend WithEvents txtReplaceYear As TextBox
    Friend WithEvents lbPurchaseDate As Label
    Friend WithEvents dtPurchaseDate_REQ As DateTimePicker
    Friend WithEvents Label5 As Label
    Friend WithEvents cmbLocation_REQ As ComboBox
    Friend WithEvents Label4 As Label
    Friend WithEvents Label3 As Label
    Friend WithEvents txtDescription_REQ As TextBox
    Friend WithEvents txtCurUser_REQ As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents Label1 As Label
    Friend WithEvents txtAssetTag_REQ As TextBox
    Friend WithEvents txtSerial_REQ As TextBox
    Friend WithEvents Label11 As Label
    Friend WithEvents cmbStatus_REQ As ComboBox
    Friend WithEvents cmdClear As Button
    Friend WithEvents chkTrackable As CheckBox
    Friend WithEvents chkNoClear As CheckBox
    Friend WithEvents fieldErrorIcon As ErrorProvider
    Friend WithEvents cmdUserSearch As Button
End Class
