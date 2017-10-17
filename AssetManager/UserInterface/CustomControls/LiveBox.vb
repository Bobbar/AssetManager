Public Class LiveBox : Implements IDisposable

#Region "Fields"

    Private CurrentLiveBoxArgs As LiveBoxArgs
    Private LiveBox As ListBox
    Private LiveBoxControls As New List(Of LiveBoxArgs)
    Private QueryRunning As Boolean = False
    Private RowLimit As Integer = 30
    Private strPrevSearchString As String

#End Region

#Region "Constructors"

    Sub New(parentForm As Form)
        InitializeControl(parentForm)
    End Sub

#End Region

#Region "Methods"

    Public Sub AttachToControl(control As TextBox, type As LiveBoxType, displayMember As String, Optional valueMember As String = Nothing)
        Dim ControlArgs As New LiveBoxArgs
        ControlArgs.Control = control
        ControlArgs.Type = type
        ControlArgs.DisplayMember = displayMember
        If Not IsNothing(valueMember) Then ControlArgs.ValueMember = valueMember
        LiveBoxControls.Add(ControlArgs)
        AddHandler ControlArgs.Control.KeyUp, AddressOf Control_KeyUp
        AddHandler ControlArgs.Control.KeyDown, AddressOf Control_KeyDown
        AddHandler ControlArgs.Control.LostFocus, AddressOf Control_LostFocus
        AddHandler ControlArgs.Control.ReadOnlyChanged, AddressOf Control_LostFocus
    End Sub

    Public Sub GiveLiveBoxFocus()
        LiveBox.Focus()
        If LiveBox.SelectedIndex = -1 Then
            LiveBox.SelectedIndex = 0
        End If
    End Sub

    Public Sub HideLiveBox()
        Try
            LiveBox.Visible = False
            LiveBox.DataSource = Nothing
        Catch
        End Try
    End Sub

    Private Sub Control_KeyDown(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Down Then
            GiveLiveBoxFocus()
        End If
    End Sub

    Private Sub Control_KeyUp(sender As Object, e As KeyEventArgs)
        If e.KeyCode = Keys.Escape Then
            HideLiveBox()
        Else
            'don't respond to non-alpha keys
            Select Case e.KeyCode
                Case Keys.ShiftKey, Keys.Alt, Keys.ControlKey, Keys.Menu
                    'do nothing
                Case Else
                    Dim arg = GetSenderArgs(sender)
                    If Not arg.Control.ReadOnly Then
                        StartLiveSearch(arg)
                    End If
            End Select
        End If
    End Sub

    Private Sub Control_LostFocus(sender As Object, e As EventArgs)
        If Not IsNothing(CurrentLiveBoxArgs.Control) Then
            If Not CurrentLiveBoxArgs.Control.Focused And Not LiveBox.Focused Then
                If LiveBox.Visible Then HideLiveBox()
            End If
            If Not CurrentLiveBoxArgs.Control.Enabled Then
                If LiveBox.Visible Then HideLiveBox()
            End If
            If TypeOf CurrentLiveBoxArgs.Control Is TextBox Then
                Dim txt As TextBox = CurrentLiveBoxArgs.Control
                If txt.ReadOnly Then
                    If LiveBox.Visible Then HideLiveBox()
                End If
            End If
        End If
    End Sub

    Private Sub DrawLiveBox(dtResults As DataTable)
        Try
            If dtResults.Rows.Count > 0 Then
                LiveBox.DataSource = dtResults
                LiveBox.DisplayMember = CurrentLiveBoxArgs.DisplayMember
                LiveBox.ValueMember = CurrentLiveBoxArgs.ValueMember
                LiveBox.ClearSelected()
                PosistionLiveBox()
                LiveBox.Visible = True
                If strPrevSearchString <> Trim(CurrentLiveBoxArgs.Control.Text) Then
                    StartLiveSearch(CurrentLiveBoxArgs) 'if search string has changed since last completion, run again.
                End If
            Else
                LiveBox.Visible = False
            End If
        Catch
            HideLiveBox()
        End Try
    End Sub

    Private Function GetSenderArgs(sender As Object) As LiveBoxArgs
        For Each arg As LiveBoxArgs In LiveBoxControls
            If arg.Control Is DirectCast(sender, TextBox) Then
                Return arg
            End If
        Next
        Return Nothing
    End Function

    Private Sub InitializeControl(parentForm As Form)
        LiveBox = New ListBox
        LiveBox.Parent = parentForm
        LiveBox.BringToFront()
        'AddHandler LiveBox.MouseClick, AddressOf LiveBox_MouseClick
        AddHandler LiveBox.MouseDown, AddressOf LiveBox_MouseDown
        AddHandler LiveBox.MouseMove, AddressOf LiveBox_MouseMove
        AddHandler LiveBox.KeyDown, AddressOf LiveBox_KeyDown
        AddHandler LiveBox.LostFocus, AddressOf LiveBox_LostFocus
        ExtendedMethods.DoubleBufferedListBox(LiveBox, True)
        LiveBox.Visible = False
        SetStyle()
    End Sub

    Private Sub LiveBox_LostFocus(sender As Object, e As EventArgs)
        HideLiveBox()
    End Sub

    Private Sub LiveBox_KeyDown(sender As Object, e As KeyEventArgs)
        Select Case e.KeyCode
            Case Keys.Enter
                LiveBoxSelect()
            Case Keys.Escape
                HideLiveBox()
        End Select
    End Sub

    Private Sub LiveBox_MouseDown(sender As Object, e As MouseEventArgs)
        Select Case e.Button
            Case MouseButtons.Left
                LiveBoxSelect()
            Case MouseButtons.Right
                HideLiveBox()
        End Select
    End Sub

    Private Sub LiveBox_MouseMove(sender As Object, e As MouseEventArgs)
        LiveBox.SelectedIndex = LiveBox.IndexFromPoint(e.Location)
    End Sub

    Private Sub LiveBoxSelect()
        Select Case CurrentLiveBoxArgs.Type
            Case LiveBoxType.DynamicSearch
                CurrentLiveBoxArgs.Control.Text = LiveBox.Text
                MainForm.DynamicSearch()
            Case LiveBoxType.InstaLoad
                MainForm.LoadDevice(LiveBox.SelectedValue.ToString)
                CurrentLiveBoxArgs.Control.Text = ""
            Case LiveBoxType.SelectValue
                CurrentLiveBoxArgs.Control.Text = LiveBox.Text
            Case LiveBoxType.UserSelect
                CurrentLiveBoxArgs.Control.Text = LiveBox.Text
                If TypeOf CurrentLiveBoxArgs.Control.FindForm Is ViewDeviceForm Then
                    Dim FrmSetData As ViewDeviceForm = DirectCast(CurrentLiveBoxArgs.Control.FindForm, ViewDeviceForm)
                    FrmSetData.MunisUser.Name = LiveBox.Text
                    FrmSetData.MunisUser.Number = LiveBox.SelectedValue.ToString
                ElseIf TypeOf CurrentLiveBoxArgs.Control.FindForm Is NewDeviceForm Then
                    Dim FrmSetData As NewDeviceForm = DirectCast(CurrentLiveBoxArgs.Control.FindForm, NewDeviceForm)
                    FrmSetData.MunisUser.Name = LiveBox.Text
                    FrmSetData.MunisUser.Number = LiveBox.SelectedValue.ToString
                End If
        End Select
        HideLiveBox()
    End Sub

    Private Sub PosistionLiveBox()
        Dim ScreenPos As Point = LiveBox.Parent.PointToClient(CurrentLiveBoxArgs.Control.Parent.PointToScreen(CurrentLiveBoxArgs.Control.Location))
        ScreenPos.Y += CurrentLiveBoxArgs.Control.Height - 1
        LiveBox.Location = ScreenPos
        LiveBox.Width = PreferredWidth()
        Dim FormBounds As Rectangle = LiveBox.Parent.ClientRectangle
        If LiveBox.PreferredHeight + LiveBox.Top > FormBounds.Bottom Then
            LiveBox.Height = FormBounds.Bottom - LiveBox.Top - LiveBox.Padding.Bottom
        Else
            LiveBox.Height = LiveBox.PreferredHeight
        End If
    End Sub

    Private Function PreferredWidth() As Integer
        Using gfx As Graphics = LiveBox.CreateGraphics
            Dim BoxFont As Font = LiveBox.Font
            Dim MaxLen As Integer = 0
            For Each row As DataRowView In LiveBox.Items
                Dim ItemLen As Integer = CInt(gfx.MeasureString(row.Item(CurrentLiveBoxArgs.DisplayMember).ToString, BoxFont).Width)
                If ItemLen > MaxLen Then
                    MaxLen = ItemLen
                End If
            Next
            If MaxLen > CurrentLiveBoxArgs.Control.Width Then
                Return MaxLen
            Else
                Return CurrentLiveBoxArgs.Control.Width
            End If
        End Using
    End Function

    ''' <summary>
    ''' Runs the DB query Asynchronously.
    ''' </summary>
    ''' <param name="SearchString"></param>
    Private Async Sub ProcessSearch(searchString As String)
        strPrevSearchString = searchString
        If QueryRunning Then Exit Sub
        Try
            Dim Results = Await Task.Run(Function()
                                             QueryRunning = True
                                             Dim strQry As String
                                             strQry = "SELECT " & DevicesCols.DeviceUID & "," & IIf(IsNothing(CurrentLiveBoxArgs.ValueMember), CurrentLiveBoxArgs.DisplayMember, CurrentLiveBoxArgs.DisplayMember & "," & CurrentLiveBoxArgs.ValueMember).ToString & " FROM " & DevicesCols.TableName & " WHERE " & CurrentLiveBoxArgs.DisplayMember & " LIKE  @Search_Value  GROUP BY " & CurrentLiveBoxArgs.DisplayMember & " ORDER BY " & CurrentLiveBoxArgs.DisplayMember & " LIMIT " & RowLimit
                                             Using cmd = DBFactory.GetDatabase.GetCommand(strQry)
                                                 cmd.AddParameterWithValue("@Search_Value", "%" & searchString & "%")
                                                 Return DBFactory.GetDatabase.DataTableFromCommand(cmd)
                                             End Using
                                         End Function)
            DrawLiveBox(Results)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            QueryRunning = False
        End Try
    End Sub

    Private Sub SetStyle()
        Dim LiveBoxFont As Font = New Font("Consolas", 11.25, FontStyle.Bold)
        With LiveBox
            .BackColor = Color.FromArgb(255, 208, 99)
            .BorderStyle = BorderStyle.FixedSingle
            .Font = LiveBoxFont
            .ForeColor = Color.Black
            .Padding = New Padding(0, 0, 0, 10)
        End With
    End Sub

    Private Sub StartLiveSearch(args As LiveBoxArgs)
        CurrentLiveBoxArgs = args
        Dim strSearchString As String = Trim(CurrentLiveBoxArgs.Control.Text)
        If strSearchString <> "" Then
            ProcessSearch(strSearchString)
        Else
            HideLiveBox()
        End If
    End Sub

#End Region

#Region "Structs"

    Private Structure LiveBoxArgs

#Region "Fields"

        Public Control As TextBox
        Public DisplayMember As String
        Public Type As LiveBoxType
        Public ValueMember As String

#End Region

    End Structure

#End Region

#Region "IDisposable Support"

    Private disposedValue As Boolean ' To detect redundant calls

    ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
    'Protected Overrides Sub Finalize()
    '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
    '    Dispose(False)
    '    MyBase.Finalize()
    'End Sub
    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                LiveBox.Dispose()
                LiveBoxControls.Clear()
            End If
            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.
        End If
        disposedValue = True
    End Sub

#End Region

End Class