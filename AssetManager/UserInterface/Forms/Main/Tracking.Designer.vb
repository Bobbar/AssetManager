<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class Tracking
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
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(Tracking))
        Me.CheckInBox = New System.Windows.Forms.GroupBox()
        Me.cmdCheckIn = New System.Windows.Forms.Button()
        Me.Label11 = New System.Windows.Forms.Label()
        Me.txtCheckInNotes = New System.Windows.Forms.TextBox()
        Me.Label10 = New System.Windows.Forms.Label()
        Me.dtCheckIn = New System.Windows.Forms.DateTimePicker()
        Me.CheckOutBox = New System.Windows.Forms.GroupBox()
        Me.cmdCheckOut = New System.Windows.Forms.Button()
        Me.Label9 = New System.Windows.Forms.Label()
        Me.txtUseReason = New System.Windows.Forms.TextBox()
        Me.Label8 = New System.Windows.Forms.Label()
        Me.txtUser = New System.Windows.Forms.TextBox()
        Me.Label7 = New System.Windows.Forms.Label()
        Me.txtUseLocation = New System.Windows.Forms.TextBox()
        Me.Label6 = New System.Windows.Forms.Label()
        Me.dtDueBack = New System.Windows.Forms.DateTimePicker()
        Me.Label5 = New System.Windows.Forms.Label()
        Me.dtCheckOut = New System.Windows.Forms.DateTimePicker()
        Me.DeviceInfoBox = New System.Windows.Forms.GroupBox()
        Me.Label4 = New System.Windows.Forms.Label()
        Me.txtDeviceType = New System.Windows.Forms.TextBox()
        Me.Label3 = New System.Windows.Forms.Label()
        Me.txtDescription = New System.Windows.Forms.TextBox()
        Me.Label2 = New System.Windows.Forms.Label()
        Me.txtSerial = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.txtAssetTag = New System.Windows.Forms.TextBox()
        Me.CheckInBox.SuspendLayout()
        Me.CheckOutBox.SuspendLayout()
        Me.DeviceInfoBox.SuspendLayout()
        Me.SuspendLayout()
        '
        'CheckInBox
        '
        Me.CheckInBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.CheckInBox.Controls.Add(Me.cmdCheckIn)
        Me.CheckInBox.Controls.Add(Me.Label11)
        Me.CheckInBox.Controls.Add(Me.txtCheckInNotes)
        Me.CheckInBox.Controls.Add(Me.Label10)
        Me.CheckInBox.Controls.Add(Me.dtCheckIn)
        Me.CheckInBox.Location = New System.Drawing.Point(12, 307)
        Me.CheckInBox.Name = "CheckInBox"
        Me.CheckInBox.Size = New System.Drawing.Size(794, 152)
        Me.CheckInBox.TabIndex = 0
        Me.CheckInBox.TabStop = False
        Me.CheckInBox.Text = "Check In"
        '
        'cmdCheckIn
        '
        Me.cmdCheckIn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCheckIn.Location = New System.Drawing.Point(601, 58)
        Me.cmdCheckIn.Name = "cmdCheckIn"
        Me.cmdCheckIn.Size = New System.Drawing.Size(160, 51)
        Me.cmdCheckIn.TabIndex = 13
        Me.cmdCheckIn.Text = "Check In"
        Me.cmdCheckIn.UseVisualStyleBackColor = True
        '
        'Label11
        '
        Me.Label11.AutoSize = True
        Me.Label11.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label11.Location = New System.Drawing.Point(354, 27)
        Me.Label11.Name = "Label11"
        Me.Label11.Size = New System.Drawing.Size(98, 16)
        Me.Label11.TabIndex = 12
        Me.Label11.Text = "Check In Notes"
        '
        'txtCheckInNotes
        '
        Me.txtCheckInNotes.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtCheckInNotes.Location = New System.Drawing.Point(270, 46)
        Me.txtCheckInNotes.MaxLength = 200
        Me.txtCheckInNotes.Multiline = True
        Me.txtCheckInNotes.Name = "txtCheckInNotes"
        Me.txtCheckInNotes.Size = New System.Drawing.Size(256, 68)
        Me.txtCheckInNotes.TabIndex = 11
        '
        'Label10
        '
        Me.Label10.AutoSize = True
        Me.Label10.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(28, 36)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(91, 16)
        Me.Label10.TabIndex = 4
        Me.Label10.Text = "Check In Date"
        '
        'dtCheckIn
        '
        Me.dtCheckIn.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtCheckIn.CustomFormat = "MM-dd-yyyy hh:mm tt"
        Me.dtCheckIn.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtCheckIn.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtCheckIn.Location = New System.Drawing.Point(31, 58)
        Me.dtCheckIn.Name = "dtCheckIn"
        Me.dtCheckIn.Size = New System.Drawing.Size(164, 22)
        Me.dtCheckIn.TabIndex = 3
        '
        'CheckOutBox
        '
        Me.CheckOutBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.CheckOutBox.Controls.Add(Me.cmdCheckOut)
        Me.CheckOutBox.Controls.Add(Me.Label9)
        Me.CheckOutBox.Controls.Add(Me.txtUseReason)
        Me.CheckOutBox.Controls.Add(Me.Label8)
        Me.CheckOutBox.Controls.Add(Me.txtUser)
        Me.CheckOutBox.Controls.Add(Me.Label7)
        Me.CheckOutBox.Controls.Add(Me.txtUseLocation)
        Me.CheckOutBox.Controls.Add(Me.Label6)
        Me.CheckOutBox.Controls.Add(Me.dtDueBack)
        Me.CheckOutBox.Controls.Add(Me.Label5)
        Me.CheckOutBox.Controls.Add(Me.dtCheckOut)
        Me.CheckOutBox.Location = New System.Drawing.Point(12, 120)
        Me.CheckOutBox.Name = "CheckOutBox"
        Me.CheckOutBox.Size = New System.Drawing.Size(794, 181)
        Me.CheckOutBox.TabIndex = 1
        Me.CheckOutBox.TabStop = False
        Me.CheckOutBox.Text = "Check Out"
        '
        'cmdCheckOut
        '
        Me.cmdCheckOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmdCheckOut.Location = New System.Drawing.Point(601, 109)
        Me.cmdCheckOut.Name = "cmdCheckOut"
        Me.cmdCheckOut.Size = New System.Drawing.Size(160, 51)
        Me.cmdCheckOut.TabIndex = 11
        Me.cmdCheckOut.Text = "Check Out"
        Me.cmdCheckOut.UseVisualStyleBackColor = True
        '
        'Label9
        '
        Me.Label9.AutoSize = True
        Me.Label9.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(353, 74)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(84, 16)
        Me.Label9.TabIndex = 10
        Me.Label9.Text = "Use Reason"
        '
        'txtUseReason
        '
        Me.txtUseReason.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUseReason.Location = New System.Drawing.Point(270, 93)
        Me.txtUseReason.MaxLength = 200
        Me.txtUseReason.Multiline = True
        Me.txtUseReason.Name = "txtUseReason"
        Me.txtUseReason.Size = New System.Drawing.Size(256, 68)
        Me.txtUseReason.TabIndex = 9
        Me.txtUseReason.Text = "test"
        '
        'Label8
        '
        Me.Label8.AutoSize = True
        Me.Label8.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(17, 116)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(37, 16)
        Me.Label8.TabIndex = 8
        Me.Label8.Text = "User"
        Me.Label8.Visible = False
        '
        'txtUser
        '
        Me.txtUser.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUser.Location = New System.Drawing.Point(15, 138)
        Me.txtUser.Name = "txtUser"
        Me.txtUser.ReadOnly = True
        Me.txtUser.Size = New System.Drawing.Size(175, 22)
        Me.txtUser.TabIndex = 7
        Me.txtUser.Visible = False
        '
        'Label7
        '
        Me.Label7.AutoSize = True
        Me.Label7.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(494, 16)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(87, 16)
        Me.Label7.TabIndex = 6
        Me.Label7.Text = "Use Location"
        '
        'txtUseLocation
        '
        Me.txtUseLocation.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtUseLocation.Location = New System.Drawing.Point(492, 38)
        Me.txtUseLocation.Name = "txtUseLocation"
        Me.txtUseLocation.Size = New System.Drawing.Size(184, 22)
        Me.txtUseLocation.TabIndex = 5
        '
        'Label6
        '
        Me.Label6.AutoSize = True
        Me.Label6.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(305, 16)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(99, 16)
        Me.Label6.TabIndex = 4
        Me.Label6.Text = "Due Back Date"
        '
        'dtDueBack
        '
        Me.dtDueBack.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtDueBack.CustomFormat = "MM-dd-yyyy hh:mm tt"
        Me.dtDueBack.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtDueBack.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtDueBack.Location = New System.Drawing.Point(308, 38)
        Me.dtDueBack.Name = "dtDueBack"
        Me.dtDueBack.Size = New System.Drawing.Size(167, 22)
        Me.dtDueBack.TabIndex = 3
        '
        'Label5
        '
        Me.Label5.AutoSize = True
        Me.Label5.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(128, 16)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(101, 16)
        Me.Label5.TabIndex = 2
        Me.Label5.Text = "Check Out Date"
        '
        'dtCheckOut
        '
        Me.dtCheckOut.CalendarFont = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtCheckOut.CustomFormat = "MM-dd-yyyy hh:mm tt"
        Me.dtCheckOut.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.dtCheckOut.Format = System.Windows.Forms.DateTimePickerFormat.Custom
        Me.dtCheckOut.Location = New System.Drawing.Point(131, 38)
        Me.dtCheckOut.Name = "dtCheckOut"
        Me.dtCheckOut.Size = New System.Drawing.Size(164, 22)
        Me.dtCheckOut.TabIndex = 0
        '
        'DeviceInfoBox
        '
        Me.DeviceInfoBox.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.DeviceInfoBox.Controls.Add(Me.Label4)
        Me.DeviceInfoBox.Controls.Add(Me.txtDeviceType)
        Me.DeviceInfoBox.Controls.Add(Me.Label3)
        Me.DeviceInfoBox.Controls.Add(Me.txtDescription)
        Me.DeviceInfoBox.Controls.Add(Me.Label2)
        Me.DeviceInfoBox.Controls.Add(Me.txtSerial)
        Me.DeviceInfoBox.Controls.Add(Me.Label1)
        Me.DeviceInfoBox.Controls.Add(Me.txtAssetTag)
        Me.DeviceInfoBox.Location = New System.Drawing.Point(12, 12)
        Me.DeviceInfoBox.Name = "DeviceInfoBox"
        Me.DeviceInfoBox.Size = New System.Drawing.Size(794, 93)
        Me.DeviceInfoBox.TabIndex = 2
        Me.DeviceInfoBox.TabStop = False
        Me.DeviceInfoBox.Text = "Device Info"
        '
        'Label4
        '
        Me.Label4.AutoSize = True
        Me.Label4.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(604, 27)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(86, 16)
        Me.Label4.TabIndex = 7
        Me.Label4.Text = "Device Type"
        '
        'txtDeviceType
        '
        Me.txtDeviceType.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDeviceType.Location = New System.Drawing.Point(607, 49)
        Me.txtDeviceType.Name = "txtDeviceType"
        Me.txtDeviceType.ReadOnly = True
        Me.txtDeviceType.Size = New System.Drawing.Size(139, 23)
        Me.txtDeviceType.TabIndex = 6
        '
        'Label3
        '
        Me.Label3.AutoSize = True
        Me.Label3.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(334, 27)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(76, 16)
        Me.Label3.TabIndex = 5
        Me.Label3.Text = "Description"
        '
        'txtDescription
        '
        Me.txtDescription.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtDescription.Location = New System.Drawing.Point(332, 49)
        Me.txtDescription.Name = "txtDescription"
        Me.txtDescription.ReadOnly = True
        Me.txtDescription.Size = New System.Drawing.Size(244, 23)
        Me.txtDescription.TabIndex = 4
        '
        'Label2
        '
        Me.Label2.AutoSize = True
        Me.Label2.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(167, 26)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(43, 16)
        Me.Label2.TabIndex = 3
        Me.Label2.Text = "Serial"
        '
        'txtSerial
        '
        Me.txtSerial.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtSerial.Location = New System.Drawing.Point(165, 48)
        Me.txtSerial.Name = "txtSerial"
        Me.txtSerial.ReadOnly = True
        Me.txtSerial.Size = New System.Drawing.Size(139, 23)
        Me.txtSerial.TabIndex = 2
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(28, 26)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(70, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Asset Tag"
        '
        'txtAssetTag
        '
        Me.txtAssetTag.Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.txtAssetTag.Location = New System.Drawing.Point(26, 48)
        Me.txtAssetTag.Name = "txtAssetTag"
        Me.txtAssetTag.ReadOnly = True
        Me.txtAssetTag.Size = New System.Drawing.Size(114, 23)
        Me.txtAssetTag.TabIndex = 0
        '
        'Tracking
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer), CType(CType(232, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(816, 470)
        Me.Controls.Add(Me.DeviceInfoBox)
        Me.Controls.Add(Me.CheckOutBox)
        Me.Controls.Add(Me.CheckInBox)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.Name = "Tracking"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Tracking"
        Me.CheckInBox.ResumeLayout(False)
        Me.CheckInBox.PerformLayout()
        Me.CheckOutBox.ResumeLayout(False)
        Me.CheckOutBox.PerformLayout()
        Me.DeviceInfoBox.ResumeLayout(False)
        Me.DeviceInfoBox.PerformLayout()
        Me.ResumeLayout(False)
    End Sub
    Friend WithEvents CheckInBox As GroupBox
    Friend WithEvents CheckOutBox As GroupBox
    Friend WithEvents DeviceInfoBox As GroupBox
    Friend WithEvents Label4 As Label
    Friend WithEvents txtDeviceType As TextBox
    Friend WithEvents Label3 As Label
    Friend WithEvents txtDescription As TextBox
    Friend WithEvents Label2 As Label
    Friend WithEvents txtSerial As TextBox
    Friend WithEvents Label1 As Label
    Friend WithEvents txtAssetTag As TextBox
    Friend WithEvents Label5 As Label
    Friend WithEvents dtCheckOut As DateTimePicker
    Friend WithEvents Label7 As Label
    Friend WithEvents txtUseLocation As TextBox
    Friend WithEvents Label6 As Label
    Friend WithEvents dtDueBack As DateTimePicker
    Friend WithEvents Label8 As Label
    Friend WithEvents txtUser As TextBox
    Friend WithEvents cmdCheckOut As Button
    Friend WithEvents Label9 As Label
    Friend WithEvents txtUseReason As TextBox
    Friend WithEvents Label10 As Label
    Friend WithEvents dtCheckIn As DateTimePicker
    Friend WithEvents Label11 As Label
    Friend WithEvents txtCheckInNotes As TextBox
    Friend WithEvents cmdCheckIn As Button
End Class
