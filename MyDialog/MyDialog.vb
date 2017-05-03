Imports System.Windows.Forms
Imports System.Drawing
Public Class MyDialog
    Private FormControlSize As Size = New Size(600, 345)

    Private IsMessageBox As Boolean = False

    Private MyControls As New List(Of Control)

    Private StartFullSize As Boolean = False

    ''' <summary>
    ''' Instantiate new Dialog.
    ''' </summary>
    ''' <param name="ParentForm">Required. Specifies the form the modal dialog will be bound to.</param>
    ''' <param name="StartMaximized">Optional. Starts the dialog at its maximum size.</param>
    Sub New(ParentForm As Form, Optional StartMaximized As Boolean = False)
        InitializeComponent()
        StartFullSize = StartMaximized
        If Not IsNothing(ParentForm) Then
            Icon = ParentForm.Icon
        Else
            Icon = My.Resources.inventory_icon_orange
        End If
    End Sub
    ''' <summary>
    ''' List of controls added to this dialog instance.
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property ControlValues As List(Of Control)
        Get
            Return MyControls
        End Get
    End Property
    ''' <summary>
    ''' Adds new <see cref="Button"/> control to the dialog box.
    ''' </summary>
    ''' <param name="Name">Name of the control. To be referenced later via <see cref="GetControlValue(String)"/></param>
    ''' <param name="Text">Text to be displayed on the control. <see cref="Button.Text"/> </param>
    ''' <param name="ClickAction">Specifies a sub routine to execute on Click. (Example Value: AddressOf [Method])</param>
    Public Sub AddButton(Name As String, Text As String, ClickAction As Action)
        Dim but As New Button
        but.Name = Name
        but.Text = Text
        but.Tag = ClickAction
        AddControl(but)
    End Sub
    ''' <summary>
    ''' Adds new <see cref="CheckBox"/> control to the dialog box.
    ''' </summary>
    ''' <param name="Name">Name of the control. To be referenced later via <see cref="GetControlValue(String)"/></param>
    ''' <param name="Label">Text of label that will be positioned next to the control.</param>
    Public Sub AddCheckBox(Name As String, Label As String)
        Dim chk As New CheckBox
        chk.Name = Name
        chk.Tag = Label
        AddControl(chk)
    End Sub
    ''' <summary>
    ''' Adds control to the <see cref="MyControls"/> member.
    ''' </summary>
    ''' <param name="c"></param>
    Private Sub AddControl(c As Control)
        MyControls.Add(c)
    End Sub
    ''' <summary>
    ''' Add a previously instantiated control to the dialog box.
    ''' </summary>
    ''' <param name="Name">Name of the control. To be referenced later via <see cref="GetControlValue(String)"/></param>
    ''' <param name="Label">Text of label that will be positioned next to the control.</param>
    ''' <param name="control">Reference of pre-prepared control.</param>
    Public Sub AddCustomControl(Name As String, Label As String, ByRef control As Control)
        control.Name = Name
        control.Tag = Label
        AddControl(control)
    End Sub
    ''' <summary>
    ''' Adds a new <see cref="Label"/> control to the dialog box.
    ''' </summary>
    ''' <param name="LabelText">Text of the label.</param>
    ''' <param name="Bold">Optional. Use Bold font style?</param>
    Public Sub AddLabel(LabelText As String, Optional Bold As Boolean = False)
        Dim lbl As New Label
        lbl.Text = LabelText
        If Bold Then lbl.Font = New Font(lbl.Font, FontStyle.Bold)
        AddControl(lbl)
    End Sub

    ''' <summary>
    ''' Add a new <see cref="RichTextBox"/> control to the dialog box.
    ''' </summary>
    ''' <param name="Name">Name of the control. To be referenced later via <see cref="GetControlValue(String)"/></param>
    ''' <param name="Label">Text of label that will be positioned next to the control.</param>
    ''' <param name="Text">Optional. Text in the RichTextBox.</param>
    Public Sub AddRichTextBox(Name As String, Label As String, Optional Text As String = Nothing)
        Dim rtb As New RichTextBox
        rtb.Name = Name
        rtb.Tag = Label
        If Not IsNothing(Text) Then rtb.Text = Text
        AddControl(rtb)
    End Sub

    ''' <summary>
    ''' Add a new <see cref="TextBox"/> control to the dialog box.
    ''' </summary>
    ''' <param name="Name">Name of the control. To be referenced later via <see cref="GetControlValue(String)"/></param>
    ''' <param name="Label">Text of label that will be positioned next to the control.</param>
    ''' <param name="Text">Optional. Text in the TextBox.</param>
    Public Sub AddTextBox(Name As String, Label As String, Optional Text As String = Nothing)
        Dim txt As New TextBox
        txt.Name = Name
        txt.Tag = Label
        If Not IsNothing(Text) Then txt.Text = Text
        AddControl(txt)
    End Sub

    ''' <summary>
    ''' Instantiate new <see cref="FlowLayoutPanel"/>
    ''' </summary>
    ''' <returns><see cref="FlowLayoutPanel"/></returns>
    Private Function ControlPanel() As FlowLayoutPanel
        Dim pnl As New FlowLayoutPanel
        pnl.AutoSize = True
        pnl.WrapContents = False
        pnl.FlowDirection = FlowDirection.TopDown
        pnl.AutoSizeMode = AutoSizeMode.GrowAndShrink
        pnl.Padding = New Padding(0, 0, 10, 10)
        Return pnl
    End Function

    ''' <summary>
    ''' Instantiates and binds a new modal Dialog on the specified Form.
    ''' </summary>
    ''' <param name="Prompt">Required. Message to be displayed to user.</param>
    ''' <param name="Style">Optional. <see cref="MsgBoxStyle"/> of the dialog. (Default: vbOKOnly + vbInformation)</param>
    ''' <param name="Title">Optional. Title text of dialog.</param>
    ''' <param name="ParentFrm">Optional. Form to which the modal dialog will be bound. (Default: <see cref="ActiveForm"/></param>
    ''' <returns><see cref="MsgBoxResult"/></returns>
    Public Function DialogMessage(ByVal Prompt As String, Optional ByVal Style As Integer = vbOKOnly + vbInformation, Optional ByVal Title As String = Nothing, Optional ByVal ParentFrm As Form = Nothing) As MsgBoxResult
        IsMessageBox = True
        If IsNothing(Title) Then
            Me.Text = My.Application.Info.AssemblyName
        Else
            Me.Text = Title
        End If
        AddRichTextBox("MessageBox", "MessageBox", Prompt)
        Me.Size = Me.MinimumSize
        ParseStyle(CType(Style, MsgBoxStyle))
        If ParentFrm IsNot Nothing Then
            Me.ShowDialog(ParentFrm)
        Else
            Me.ShowDialog(ActiveForm)
        End If
        Me.Dispose()
        Return CType(Me.DialogResult, MsgBoxResult)
    End Function
    ''' <summary>
    ''' Returns the value of the specified control.
    ''' </summary>
    ''' <param name="ControlName">Name of the control.</param>
    ''' <returns>Dependent on control type. <see cref="Object"/></returns>
    Public Function GetControlValue(ControlName As String) As Object
        For Each ctl As Control In MyControls
            If ctl.Name = ControlName Then
                Select Case True
                    Case TypeOf ctl Is ComboBox
                        Dim cmb As ComboBox = ctl
                        Return cmb.SelectedIndex
                    Case TypeOf ctl Is TextBox
                        Dim txt As TextBox = ctl
                        Dim TxtVal As String = Trim(txt.Text)
                        If TxtVal = "" Then Return Nothing
                        Return txt.Text
                    Case TypeOf ctl Is RichTextBox
                        Dim txt As RichTextBox = ctl
                        Return txt.Text
                    Case TypeOf ctl Is CheckBox
                        Dim chk As CheckBox = ctl
                        Return chk.CheckState
                End Select
            End If
        Next
        Return Nothing
    End Function
    ''' <summary>
    ''' Instantiate new label.
    ''' </summary>
    ''' <param name="Text"></param>
    ''' <returns></returns>
    Private Function NewControlLabel(Text As String) As Label
        Dim lbl As New Label
        lbl.AutoSize = True
        lbl.Padding = New Padding(0, 10, 5, 0)
        lbl.Text = Text
        Return lbl
    End Function

    ''' <summary>
    ''' Sets the value of a control after the dialog has been displayed.
    ''' </summary>
    ''' <param name="ControlName">Name of control.</param>
    ''' <param name="Value">Value to set to control.</param>
    Public Sub SetControlValue(ControlName As String, Value As Object)
        For Each ctl As Control In MyControls
            If ctl.Name = ControlName Then
                Select Case True
                    Case TypeOf ctl Is ComboBox
                        Dim cmb As ComboBox = ctl
                        cmb.SelectedIndex = Value
                    Case TypeOf ctl Is TextBox
                        Dim txt As TextBox = ctl
                        txt.Text = Value
                    Case TypeOf ctl Is RichTextBox
                        Dim txt As RichTextBox = ctl
                        txt.Text = Value
                    Case TypeOf ctl Is CheckBox
                        Dim chk As CheckBox = ctl
                        chk.CheckState = Value
                End Select
            End If
        Next
    End Sub

    Private Sub ButtonClick(ByVal sender As Object, ByVal e As EventArgs)
        Dim btn As Button = DirectCast(sender, Button)
        Dim ClickAction As Action = CType(btn.Tag, Action)
        ClickAction()
    End Sub

    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub

    Private Sub Dialog1_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadControls(MyControls)
        If Not IsMessageBox Then
            pnlIcon.Visible = False
            pnlControls_Main.Width = pnlMaster.Width
            pnlControls_Main.Height = pnlMaster.Height - pnlButtons.Height - 10
        End If
        If StartFullSize Then MaximizeForm()
        pnlControls_Main.Refresh()
        pnlMaster.Refresh()
        Me.Update()
    End Sub

    Private Sub LoadControls(lstControls As List(Of Control))
        pnlControls.SuspendLayout()
        For Each ctl As Control In lstControls
            Select Case True
                Case TypeOf ctl Is ComboBox
                    Dim cmb As ComboBox = ctl
                    Dim smPanel As FlowLayoutPanel = ControlPanel()
                    cmb.Size = cmb.PreferredSize
                    cmb.Padding = New Padding(5, 5, 5, 10)
                    smPanel.Controls.Add(NewControlLabel(cmb.Tag))
                    smPanel.Controls.Add(cmb)
                    pnlControls.Controls.Add(smPanel)
                Case TypeOf ctl Is TextBox
                    Dim txt As TextBox = ctl
                    Dim smPanel As FlowLayoutPanel = ControlPanel()
                    If txt.Name.Contains("pass") Then
                        txt.UseSystemPasswordChar = True
                    End If
                    smPanel.Controls.Add(NewControlLabel(txt.Tag))
                    txt.Width = 150
                    smPanel.Controls.Add(txt)
                    pnlControls.Controls.Add(smPanel)
                Case TypeOf ctl Is CheckBox
                    Dim chk As CheckBox = ctl
                    chk.AutoSize = True
                    chk.Text = chk.Tag
                    pnlControls.Controls.Add(chk)
                Case TypeOf ctl Is Label
                    Dim lbl As Label = ctl
                    lbl.AutoSize = True
                    lbl.Padding = New Padding(5, 5, 5, 10)
                    pnlControls.Controls.Add(lbl)
                Case TypeOf ctl Is RichTextBox
                    Dim rtb As RichTextBox = ctl
                    Dim smPanel As FlowLayoutPanel = ControlPanel()
                    If IsMessageBox Then
                        pnlControls.Visible = False
                        rtb.ReadOnly = True '
                        rtb.Margin = New Padding(5, 10, 5, 0)
                        rtb.BackColor = pnlControls_Main.BackColor
                        rtb.Dock = DockStyle.Fill
                        rtb.TabStop = False
                        AddHandler rtb.LinkClicked, AddressOf ClickedRTBLink
                        pnlControls_Main.Controls.Add(rtb)
                    Else
                        rtb.Width = 150
                        rtb.Height = 80
                        If rtb.Tag <> "" Then smPanel.Controls.Add(NewControlLabel(rtb.Tag))
                        smPanel.Controls.Add(rtb)
                        pnlControls.Controls.Add(smPanel)
                    End If
                Case TypeOf ctl Is Button
                    Dim but As Button = ctl
                    but.AutoSize = True
                    but.Text = ctl.Text
                    but.Name = ctl.Name
                    AddHandler but.Click, AddressOf ButtonClick
                    pnlControls.Controls.Add(but)
            End Select
        Next
        pnlControls.ResumeLayout()
    End Sub

    Private Sub MaximizeForm()
        pnlControls_Main.AutoSize = False
        pnlMaster.AutoSize = False
        Me.AutoSize = False
        Me.Size = Me.MaximumSize
    End Sub

    Private Sub MyDialog_ResizeBegin(sender As Object, e As EventArgs) Handles Me.ResizeBegin
        pnlControls_Main.AutoSize = False
        pnlMaster.AutoSize = False
        Me.AutoSize = False
    End Sub

    Private Sub No_Button_Click(sender As Object, e As EventArgs) Handles No_Button.Click
        Me.DialogResult = DialogResult.No
        Me.Close()
    End Sub

    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub

    Private Sub ParseStyle(ByVal style As MsgBoxStyle)
        Dim types As Integer
        Dim icons As Integer
        Dim defs As Integer
        Dim modes As Integer
        Dim miscs As Integer
        Dim iStyle As Integer
        Const MB_TYPEMASK As Integer = &HF
        Const MB_ICONMASK As Integer = &HF0
        Const MB_DEFMASK As Integer = &HF00
        Const MB_MODEMASK As Integer = &H3000
        Const MB_MISCMASK As Integer = &HFFC000
        iStyle = style
        types = iStyle And MB_TYPEMASK
        icons = iStyle And MB_ICONMASK
        defs = iStyle And MB_DEFMASK
        modes = iStyle And MB_MODEMASK
        miscs = iStyle And MB_MISCMASK
        If icons = MsgBoxStyle.Critical Then
            pbIcon.Image = My.Resources.critical_funny_crop_2
        ElseIf icons = MsgBoxStyle.Question Then
            pbIcon.Image = My.Resources.err_question
        ElseIf icons = MsgBoxStyle.Exclamation Then
            pbIcon.Image = My.Resources.exclamation
        ElseIf icons = MsgBoxStyle.Information Then
            pbIcon.Image = My.Resources.err_information_crop
        Else ' Assume MBS.Information (MB_USERICON isn't wrapped)
            pbIcon.Image = Nothing
            pnlIcon.Visible = False
            pnlControls_Main.Dock = DockStyle.Fill
        End If
        If types = MsgBoxStyle.OkOnly Then
            tblOkCancel.Visible = True
            Cancel_Button.Visible = False
        ElseIf types = MsgBoxStyle.OkCancel Then
            tblOkCancel.Visible = True
        ElseIf types = MsgBoxStyle.YesNo Then
            tblOkCancel.Visible = False
            tblYesNo.Visible = True
        End If
    End Sub
    Private Sub Yes_Button_Click(sender As Object, e As EventArgs) Handles Yes_Button.Click
        Me.DialogResult = DialogResult.Yes
        Me.Close()
    End Sub
    Private Sub ClickedRTBLink(sender As Object, e As LinkClickedEventArgs)
        Process.Start(New Uri(e.LinkText).AbsolutePath)
    End Sub
End Class
