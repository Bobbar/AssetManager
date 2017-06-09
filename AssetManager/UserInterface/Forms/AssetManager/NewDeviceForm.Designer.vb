<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class NewDeviceForm
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(NewDeviceForm))
        Me.GroupBox2 = New System.Windows.Forms.GroupBox()
        Me.txtSerial_REQ = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtCurUser_REQ = New System.Windows.Forms.TextBox()
        Me.cmdUserSearch = New System.Windows.Forms.Button()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.Label12 = New System.Windows.Forms.Label()
        Me.txtAssetTag_REQ = New System.Windows.Forms.TextBox()
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
        Me.txtNotes = New System.Windows.Forms.TextBox()
        Me.cmdAdd = New System.Windows.Forms.Button()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.txtReplaceYear = New System.Windows.Forms.TextBox()
        Me.lbPurchaseDate = New System.Windows.Forms.Label()
        Me.dtPurchaseDate_REQ = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.cmbLocation_REQ = New System.Windows.Forms.ComboBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtDescription_REQ = New System.Windows.Forms.TextBox()
        Me.fieldErrorIcon = New System.Windows.Forms.ErrorProvider(Me.components)
        Me.GroupBox3 = New System.Windows.Forms.GroupBox()
        Me.GroupBox4 = New System.Windows.Forms.GroupBox()
        Me.GroupBox5 = New System.Windows.Forms.GroupBox()
        Me.Label13 = New System.Windows.Forms.Label()
        Me.GroupBox6 = New System.Windows.Forms.GroupBox()
        Me.GroupBox7 = New System.Windows.Forms.GroupBox()
        Me.txtPhoneNumber = New System.Windows.Forms.MaskedTextBox()
        Me.GroupBox2.SuspendLayout()
        CType(Me.fieldErrorIcon, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.GroupBox3.SuspendLayout()
        Me.GroupBox4.SuspendLayout()
        Me.GroupBox5.SuspendLayout()
        Me.GroupBox6.SuspendLayout()
        Me.GroupBox7.SuspendLayout()
        Me.SuspendLayout()
        '
        'GroupBox2
        '
        Me.GroupBox2.Controls.Add(Me.txtSerial_REQ)
        Me.GroupBox2.Controls.Add(Me.Label1)
        Me.GroupBox2.Controls.Add(Me.txtCurUser_REQ)
        Me.GroupBox2.Controls.Add(Me.cmdUserSearch)
        Me.GroupBox2.Controls.Add(Me.Label3)
        Me.GroupBox2.Controls.Add(Me.Label2)
        Me.GroupBox2.Controls.Add(Me.Label12)
        Me.GroupBox2.Controls.Add(Me.txtAssetTag_REQ)
        Me.GroupBox2.Location = New System.Drawing.Point(12, 12)
        Me.GroupBox2.Name = "GroupBox2"
        Me.GroupBox2.Size = New System.Drawing.Size(236, 294)
        Me.GroupBox2.TabIndex = 52
        Me.GroupBox2.TabStop = False
        Me.GroupBox2.Text = "Unique Info"
        '
        'txtSerial_REQ
        '
        Me.txtSerial_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerial_REQ.Location = New System.Drawing.Point(17, 46)
        Me.txtSerial_REQ.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.txtSerial_REQ.Name = "txtSerial_REQ"
        Me.txtSerial_REQ.Size = New System.Drawing.Size(178, 25)
        Me.txtSerial_REQ.TabIndex = 0
        Me.txtSerial_REQ.Text = "txtSerial"
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(14, 26)
        Me.Label1.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(43, 16)
        Me.Label1.TabIndex = 25
        Me.Label1.Text = "Serial"
        '
        'txtCurUser_REQ
        '
        Me.txtCurUser_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCurUser_REQ.Location = New System.Drawing.Point(17, 144)
        Me.txtCurUser_REQ.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.txtCurUser_REQ.Name = "txtCurUser_REQ"
        Me.txtCurUser_REQ.Size = New System.Drawing.Size(178, 25)
        Me.txtCurUser_REQ.TabIndex = 2
        Me.txtCurUser_REQ.Text = "txtCurUser"
        '
        'cmdUserSearch
        '
        Me.cmdUserSearch.Location = New System.Drawing.Point(36, 173)
        Me.cmdUserSearch.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.cmdUserSearch.Name = "cmdUserSearch"
        Me.cmdUserSearch.Size = New System.Drawing.Size(141, 23)
        Me.cmdUserSearch.TabIndex = 50
        Me.cmdUserSearch.Text = "Munis Search"
        Me.cmdUserSearch.UseVisualStyleBackColor = True
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(14, 124)
        Me.Label3.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(37, 16)
        Me.Label3.TabIndex = 29
        Me.Label3.Text = "User"
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(14, 75)
        Me.Label2.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(70, 16)
        Me.Label2.TabIndex = 26
        Me.Label2.Text = "Asset Tag"
        '
        'Label12
        '
        Me.Label12.Font = New System.Drawing.Font("Tahoma", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label12.ForeColor = System.Drawing.Color.Gray
        Me.Label12.Location = New System.Drawing.Point(92, 79)
        Me.Label12.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.Label12.Name = "Label12"
        Me.Label12.Size = New System.Drawing.Size(112, 13)
        Me.Label12.TabIndex = 51
        Me.Label12.Text = "(""NA"" if not available.)"
        Me.Label12.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
        '
        'txtAssetTag_REQ
        '
        Me.txtAssetTag_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAssetTag_REQ.Location = New System.Drawing.Point(17, 95)
        Me.txtAssetTag_REQ.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.txtAssetTag_REQ.Name = "txtAssetTag_REQ"
        Me.txtAssetTag_REQ.Size = New System.Drawing.Size(178, 25)
        Me.txtAssetTag_REQ.TabIndex = 1
        Me.txtAssetTag_REQ.Text = "txtAssetTag"
        '
        'chkNoClear
        '
        Me.chkNoClear.AutoSize = True
        Me.chkNoClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkNoClear.Location = New System.Drawing.Point(732, 392)
        Me.chkNoClear.Name = "chkNoClear"
        Me.chkNoClear.Size = New System.Drawing.Size(91, 20)
        Me.chkNoClear.TabIndex = 15
        Me.chkNoClear.Text = "Don't clear"
        Me.chkNoClear.UseVisualStyleBackColor = True
        '
        'chkTrackable
        '
        Me.chkTrackable.AutoSize = True
        Me.chkTrackable.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.chkTrackable.Location = New System.Drawing.Point(30, 95)
        Me.chkTrackable.Name = "chkTrackable"
        Me.chkTrackable.Size = New System.Drawing.Size(135, 20)
        Me.chkTrackable.TabIndex = 12
        Me.chkTrackable.Text = "Trackable Device"
        Me.chkTrackable.UseVisualStyleBackColor = True
        '
        'cmdClear
        '
        Me.cmdClear.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdClear.Location = New System.Drawing.Point(693, 427)
        Me.cmdClear.Name = "cmdClear"
        Me.cmdClear.Size = New System.Drawing.Size(170, 23)
        Me.cmdClear.TabIndex = 16
        Me.cmdClear.Text = "Clear"
        Me.cmdClear.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(14, 226)
        Me.Label11.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(91, 16)
        Me.Label11.TabIndex = 47
        Me.Label11.Text = "Device Status"
        '
        'cmbStatus_REQ
        '
        Me.cmbStatus_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbStatus_REQ.FormattingEnabled = True
        Me.cmbStatus_REQ.Location = New System.Drawing.Point(17, 246)
        Me.cmbStatus_REQ.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.cmbStatus_REQ.Name = "cmbStatus_REQ"
        Me.cmbStatus_REQ.Size = New System.Drawing.Size(251, 26)
        Me.cmbStatus_REQ.TabIndex = 7
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(14, 126)
        Me.Label10.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(115, 16)
        Me.Label10.TabIndex = 45
        Me.Label10.Text = "Operating System"
        '
        'cmbOSType_REQ
        '
        Me.cmbOSType_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbOSType_REQ.FormattingEnabled = True
        Me.cmbOSType_REQ.Location = New System.Drawing.Point(17, 146)
        Me.cmbOSType_REQ.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.cmbOSType_REQ.Name = "cmbOSType_REQ"
        Me.cmbOSType_REQ.Size = New System.Drawing.Size(251, 26)
        Me.cmbOSType_REQ.TabIndex = 5
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(11, 135)
        Me.Label9.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(78, 16)
        Me.Label9.TabIndex = 43
        Me.Label9.Text = "PO Number"
        '
        'txtPO
        '
        Me.txtPO.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPO.Location = New System.Drawing.Point(14, 154)
        Me.txtPO.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.txtPO.Name = "txtPO"
        Me.txtPO.Size = New System.Drawing.Size(170, 25)
        Me.txtPO.TabIndex = 10
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(14, 76)
        Me.Label8.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(107, 16)
        Me.Label8.TabIndex = 41
        Me.Label8.Text = "Equipment Type"
        '
        'cmbEquipType_REQ
        '
        Me.cmbEquipType_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmbEquipType_REQ.FormattingEnabled = True
        Me.cmbEquipType_REQ.Location = New System.Drawing.Point(17, 96)
        Me.cmbEquipType_REQ.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.cmbEquipType_REQ.Name = "cmbEquipType_REQ"
        Me.cmbEquipType_REQ.Size = New System.Drawing.Size(251, 26)
        Me.cmbEquipType_REQ.TabIndex = 4
        '
        'txtNotes
        '
        Me.txtNotes.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtNotes.Location = New System.Drawing.Point(6, 21)
        Me.txtNotes.MaxLength = 200
        Me.txtNotes.Multiline = True
        Me.txtNotes.Name = "txtNotes"
        Me.txtNotes.Size = New System.Drawing.Size(366, 86)
        Me.txtNotes.TabIndex = 13
        '
        'cmdAdd
        '
        Me.cmdAdd.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdAdd.Location = New System.Drawing.Point(693, 342)
        Me.cmdAdd.Name = "cmdAdd"
        Me.cmdAdd.Size = New System.Drawing.Size(170, 44)
        Me.cmdAdd.TabIndex = 14
        Me.cmdAdd.Text = "Add Device"
        Me.cmdAdd.UseVisualStyleBackColor = True
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(10, 81)
        Me.Label6.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(92, 16)
        Me.Label6.TabIndex = 36
        Me.Label6.Text = "Replace Year"
        '
        'txtReplaceYear
        '
        Me.txtReplaceYear.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtReplaceYear.Location = New System.Drawing.Point(14, 100)
        Me.txtReplaceYear.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.txtReplaceYear.Name = "txtReplaceYear"
        Me.txtReplaceYear.Size = New System.Drawing.Size(170, 25)
        Me.txtReplaceYear.TabIndex = 9
        Me.txtReplaceYear.Text = "txtReplaceYear"
        '
        'lbPurchaseDate
        '
        Me.lbPurchaseDate.AutoSize = True
        Me.lbPurchaseDate.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lbPurchaseDate.Location = New System.Drawing.Point(10, 27)
        Me.lbPurchaseDate.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
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
        Me.dtPurchaseDate_REQ.Location = New System.Drawing.Point(13, 46)
        Me.dtPurchaseDate_REQ.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.dtPurchaseDate_REQ.Name = "dtPurchaseDate_REQ"
        Me.dtPurchaseDate_REQ.Size = New System.Drawing.Size(171, 25)
        Me.dtPurchaseDate_REQ.TabIndex = 8
        Me.dtPurchaseDate_REQ.Value = New Date(2016, 4, 14, 0, 0, 0, 0)
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(14, 176)
        Me.Label5.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
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
        Me.cmbLocation_REQ.Location = New System.Drawing.Point(17, 196)
        Me.cmbLocation_REQ.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.cmbLocation_REQ.Name = "cmbLocation_REQ"
        Me.cmbLocation_REQ.Size = New System.Drawing.Size(251, 26)
        Me.cmbLocation_REQ.TabIndex = 6
        Me.cmbLocation_REQ.Text = "cmbLocation"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(14, 28)
        Me.Label4.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(76, 16)
        Me.Label4.TabIndex = 30
        Me.Label4.Text = "Description"
        '
        'txtDescription_REQ
        '
        Me.txtDescription_REQ.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription_REQ.Location = New System.Drawing.Point(17, 47)
        Me.txtDescription_REQ.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.txtDescription_REQ.Name = "txtDescription_REQ"
        Me.txtDescription_REQ.Size = New System.Drawing.Size(251, 25)
        Me.txtDescription_REQ.TabIndex = 3
        Me.txtDescription_REQ.Text = "Description"
        '
        'fieldErrorIcon
        '
        Me.fieldErrorIcon.BlinkStyle = System.Windows.Forms.ErrorBlinkStyle.NeverBlink
        Me.fieldErrorIcon.ContainerControl = Me
        Me.fieldErrorIcon.Icon = CType(resources.GetObject("fieldErrorIcon.Icon"), System.Drawing.Icon)
        '
        'GroupBox3
        '
        Me.GroupBox3.Controls.Add(Me.Label4)
        Me.GroupBox3.Controls.Add(Me.txtDescription_REQ)
        Me.GroupBox3.Controls.Add(Me.cmbLocation_REQ)
        Me.GroupBox3.Controls.Add(Me.Label5)
        Me.GroupBox3.Controls.Add(Me.cmbEquipType_REQ)
        Me.GroupBox3.Controls.Add(Me.Label11)
        Me.GroupBox3.Controls.Add(Me.Label8)
        Me.GroupBox3.Controls.Add(Me.cmbStatus_REQ)
        Me.GroupBox3.Controls.Add(Me.cmbOSType_REQ)
        Me.GroupBox3.Controls.Add(Me.Label10)
        Me.GroupBox3.Location = New System.Drawing.Point(254, 12)
        Me.GroupBox3.Name = "GroupBox3"
        Me.GroupBox3.Size = New System.Drawing.Size(305, 294)
        Me.GroupBox3.TabIndex = 53
        Me.GroupBox3.TabStop = False
        Me.GroupBox3.Text = "Add'l Info"
        '
        'GroupBox4
        '
        Me.GroupBox4.Controls.Add(Me.lbPurchaseDate)
        Me.GroupBox4.Controls.Add(Me.dtPurchaseDate_REQ)
        Me.GroupBox4.Controls.Add(Me.txtReplaceYear)
        Me.GroupBox4.Controls.Add(Me.Label6)
        Me.GroupBox4.Controls.Add(Me.txtPO)
        Me.GroupBox4.Controls.Add(Me.Label9)
        Me.GroupBox4.Location = New System.Drawing.Point(565, 12)
        Me.GroupBox4.Name = "GroupBox4"
        Me.GroupBox4.Size = New System.Drawing.Size(214, 294)
        Me.GroupBox4.TabIndex = 54
        Me.GroupBox4.TabStop = False
        Me.GroupBox4.Text = "Purchase Info"
        '
        'GroupBox5
        '
        Me.GroupBox5.Controls.Add(Me.txtPhoneNumber)
        Me.GroupBox5.Controls.Add(Me.Label13)
        Me.GroupBox5.Controls.Add(Me.chkTrackable)
        Me.GroupBox5.Location = New System.Drawing.Point(785, 12)
        Me.GroupBox5.Name = "GroupBox5"
        Me.GroupBox5.Size = New System.Drawing.Size(205, 294)
        Me.GroupBox5.TabIndex = 55
        Me.GroupBox5.TabStop = False
        Me.GroupBox5.Text = "Misc"
        '
        'Label13
        '
        Me.Label13.AutoSize = True
        Me.Label13.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label13.Location = New System.Drawing.Point(11, 29)
        Me.Label13.Margin = New System.Windows.Forms.Padding(2, 2, 40, 2)
        Me.Label13.Name = "Label13"
        Me.Label13.Size = New System.Drawing.Size(98, 16)
        Me.Label13.TabIndex = 50
        Me.Label13.Text = "Phone Number"
        '
        'GroupBox6
        '
        Me.GroupBox6.Controls.Add(Me.txtNotes)
        Me.GroupBox6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.GroupBox6.Location = New System.Drawing.Point(84, 19)
        Me.GroupBox6.Name = "GroupBox6"
        Me.GroupBox6.Size = New System.Drawing.Size(378, 119)
        Me.GroupBox6.TabIndex = 56
        Me.GroupBox6.TabStop = False
        Me.GroupBox6.Text = "Notes"
        '
        'GroupBox7
        '
        Me.GroupBox7.Controls.Add(Me.GroupBox6)
        Me.GroupBox7.Location = New System.Drawing.Point(12, 312)
        Me.GroupBox7.Name = "GroupBox7"
        Me.GroupBox7.Size = New System.Drawing.Size(547, 151)
        Me.GroupBox7.TabIndex = 57
        Me.GroupBox7.TabStop = False
        '
        'txtPhoneNumber
        '
        Me.txtPhoneNumber.Font = New System.Drawing.Font("Consolas", 11.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtPhoneNumber.Location = New System.Drawing.Point(14, 50)
        Me.txtPhoneNumber.Mask = "(999) 000-0000"
        Me.txtPhoneNumber.Name = "txtPhoneNumber"
        Me.txtPhoneNumber.Size = New System.Drawing.Size(169, 25)
        Me.txtPhoneNumber.TabIndex = 58
        Me.txtPhoneNumber.TextMaskFormat = System.Windows.Forms.MaskFormat.ExcludePromptAndLiterals
        '
        'AddNew
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(997, 472)
        Me.Controls.Add(Me.chkNoClear)
        Me.Controls.Add(Me.cmdClear)
        Me.Controls.Add(Me.GroupBox7)
        Me.Controls.Add(Me.cmdAdd)
        Me.Controls.Add(Me.GroupBox5)
        Me.Controls.Add(Me.GroupBox2)
        Me.Controls.Add(Me.GroupBox4)
        Me.Controls.Add(Me.GroupBox3)
        Me.DoubleBuffered = True
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.MaximizeBox = False
        Me.Name = "AddNew"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Add New Device"
        Me.GroupBox2.ResumeLayout(False)
        Me.GroupBox2.PerformLayout()
        CType(Me.fieldErrorIcon, System.ComponentModel.ISupportInitialize).EndInit()
        Me.GroupBox3.ResumeLayout(False)
        Me.GroupBox3.PerformLayout()
        Me.GroupBox4.ResumeLayout(False)
        Me.GroupBox4.PerformLayout()
        Me.GroupBox5.ResumeLayout(False)
        Me.GroupBox5.PerformLayout()
        Me.GroupBox6.ResumeLayout(False)
        Me.GroupBox6.PerformLayout()
        Me.GroupBox7.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents Label10 As Label
    Friend WithEvents cmbOSType_REQ As ComboBox
    Friend WithEvents Label9 As Label
    Friend WithEvents txtPO As TextBox
    Friend WithEvents Label8 As Label
    Friend WithEvents cmbEquipType_REQ As ComboBox
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
    Friend WithEvents Label12 As Label
    Friend WithEvents GroupBox2 As GroupBox
    Friend WithEvents GroupBox7 As GroupBox
    Friend WithEvents GroupBox6 As GroupBox
    Friend WithEvents GroupBox5 As GroupBox
    Friend WithEvents Label13 As Label
    Friend WithEvents GroupBox4 As GroupBox
    Friend WithEvents GroupBox3 As GroupBox
    Friend WithEvents txtPhoneNumber As MaskedTextBox
End Class
