Imports System.Windows.Forms

Public Class MyDialog
    Public ReadOnly Property ControlValues As List(Of Control)
        Get
            Return MyControls
        End Get
    End Property
    Private IsMessageBox As Boolean = False
    Private MyControls As New List(Of Control)
    Public Function Message(ByVal Prompt As Object, Optional ByVal Buttons As MsgBoxStyle = MsgBoxStyle.OkOnly, Optional ByVal Title As Object = Nothing) As DialogResult
        IsMessageBox = True
        Me.Text = Title
        AddLabel(Prompt)
        SetupButtons(Buttons)
        Me.Size = Me.MinimumSize
        ParseStyle(Buttons)
        Me.ShowDialog()
        Me.Dispose()
        Return Me.DialogResult



    End Function
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
            pbIcon.Image = My.Resources.err_critical
        ElseIf icons = MsgBoxStyle.Question Then
            pbIcon.Image = My.Resources.err_question
        ElseIf icons = MsgBoxStyle.Exclamation Then
            pbIcon.Image = My.Resources.err_exclamation

        ElseIf icons = MsgBoxStyle.Information Then
            pbIcon.Image = My.Resources.err_information
        Else ' Assume MBS.Information (MB_USERICON isn't wrapped)
            pbIcon.Image = Nothing
            pbIcon.Visible = False
        End If
    End Sub
    Private Sub SetupButtons(style As MsgBoxStyle)
        Select Case style
            Case MsgBoxStyle.OkCancel
                tblOkCancel.Visible = True


            Case MsgBoxStyle.OkOnly
                tblOkCancel.Visible = True
                Cancel_Button.Visible = False

            Case MsgBoxStyle.YesNo

                tblOkCancel.Visible = False
                tblYesNo.Visible = True


        End Select


    End Sub
    Private Sub OK_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OK_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.OK
        Me.Close()
    End Sub
    Private Sub Cancel_Button_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cancel_Button.Click
        Me.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.Close()
    End Sub
    Private Sub Yes_Button_Click(sender As Object, e As EventArgs) Handles Yes_Button.Click
        Me.DialogResult = DialogResult.Yes
        Me.Close()
    End Sub
    Private Sub No_Button_Click(sender As Object, e As EventArgs) Handles No_Button.Click
        Me.DialogResult = DialogResult.No
        Me.Close()
    End Sub
    Public Sub AddControl(c As Control)
        MyControls.Add(c)
    End Sub
    Private Sub Dialog1_Load(sender As Object, e As EventArgs) Handles Me.Load
        LoadControls(MyControls)
        If Not IsMessageBox Then pbIcon.Visible = False
    End Sub
    Private Sub LoadControls(lstControls As List(Of Control))
        For Each ctl As Control In lstControls
            Select Case True
                Case TypeOf ctl Is ComboBox
                    Dim cmb As ComboBox = ctl
                    Dim smPanel As FlowLayoutPanel = ControlPanel()
                    cmb.Size = cmb.PreferredSize
                    cmb.Padding = New Padding(5, 5, 5, 10)
                    smPanel.Controls.Add(NewControlLabel(cmb.Tag))
                    smPanel.Controls.Add(cmb)
                    Panel.Controls.Add(smPanel)
                Case TypeOf ctl Is TextBox
                    Dim txt As TextBox = ctl
                    Panel.Controls.Add(NewControlLabel(txt.Tag))
                    Panel.Controls.Add(txt)
                Case TypeOf ctl Is CheckBox
                    Dim chk As CheckBox = ctl
                    chk.AutoSize = True
                    chk.Text = chk.Tag
                    Panel.Controls.Add(chk)
                Case TypeOf ctl Is Label
                    Dim lbl As Label = ctl
                    lbl.AutoSize = True
                    If IsMessageBox Then

                        lbl.Anchor = AnchorStyles.Bottom + AnchorStyles.Top
                        lbl.TextAlign = ContentAlignment.MiddleLeft
                    End If
                    lbl.Padding = New Padding(5, 5, 5, 10)
                    Panel.Controls.Add(lbl)
                Case TypeOf ctl Is RichTextBox
                    Dim rtb As RichTextBox = ctl
                    Dim smPanel As FlowLayoutPanel = ControlPanel()
                    rtb.Width = 150
                    rtb.Height = 80
                    smPanel.Controls.Add(NewControlLabel(rtb.Tag))
                    smPanel.Controls.Add(rtb)
                    Panel.Controls.Add(smPanel)
            End Select
        Next
    End Sub
    Public Function ControlPanel() As FlowLayoutPanel
        Dim pnl As New FlowLayoutPanel
        pnl.FlowDirection = FlowDirection.TopDown
        pnl.AutoSize = True
        pnl.AutoSizeMode = AutoSizeMode.GrowOnly
        pnl.Padding = New Padding(0, 0, 5, 10)
        Return pnl
    End Function
    Public Function NewControlLabel(Text As String) As Label
        Dim lbl As New Label
        lbl.AutoSize = True
        lbl.Padding = New Padding(0, 10, 5, 0)
        lbl.Text = Text
        Return lbl
    End Function
    Public Sub AddComboBox(Name As String, Label As String, DataIndex() As Combo_Data)
        Dim cmb As New ComboBox
        cmb.Name = Name
        cmb.Tag = Label
        FillComboBox(DataIndex, cmb)
        AddControl(cmb)
    End Sub
    Public Sub AddCheckBox(Name As String, Label As String)
        Dim chk As New CheckBox
        chk.Name = Name
        chk.Tag = Label
        AddControl(chk)
    End Sub
    Public Sub AddLabel(LabelText As String, Optional Bold As Boolean = False)
        Dim lbl As New Label
        lbl.Text = LabelText
        If Bold Then lbl.Font = New Font(lbl.Font, FontStyle.Bold)
        AddControl(lbl)
    End Sub
    Public Sub AddRichTextBox(Name As String, Label As String)
        Dim rtb As New RichTextBox
        rtb.Name = Name
        rtb.Tag = Label
        AddControl(rtb)
    End Sub
    Public Function GetControlValue(ControlName As String) As Object
        For Each ctl As Control In MyControls
            If ctl.Name = ControlName Then
                Select Case True
                    Case TypeOf ctl Is ComboBox
                        Dim cmb As ComboBox = ctl
                        Return cmb.SelectedIndex
                    Case TypeOf ctl Is TextBox
                        Dim txt As TextBox = ctl
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
End Class
