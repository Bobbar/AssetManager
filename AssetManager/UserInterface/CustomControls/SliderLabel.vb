Imports System.ComponentModel

Public Enum SlideDirection
    DefaultSlide
    Up
    Down
    Left
    Right

End Enum

Public Enum SlideState
    SlideIn
    SlideOut
    Paused
    Done
    Hold

End Enum

Public Class SliderLabel

#Region "Fields"

    Private Const _defaultDisplayTime As Integer = 4
    Private Const _defaultSlideInDirection As SlideDirection = SlideDirection.Up
    Private Const _defaultSlideOutDirection As SlideDirection = SlideDirection.Left
    '  Private _stepSize As Single = 0.25
    Private _acceleration As Single = 0.25

    Private _currentDirection As SlideDirection
    Private _currentSlideState As SlideState
    Private _currentSpeed As Single = 0
    Private _currentX As Single = 0
    Private _currentY As Single = 0
    Private _displayTime As Integer = 4
    Private _messageQueue As New List(Of MessageParameters)
    Private _movementInterval As Integer = 10
    Private _slideInDirection As SlideDirection
    Private _slideOutDirection As SlideDirection
    Private _slideTimer As Timer
    Private _text As String
    Private _textSize As SizeF
    Private _xStart, _xEnd, _yStart, _yEnd As Single

#End Region

#Region "Constructors"

    Sub New()
        InitializeComponent()

        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.ResizeRedraw, True)

        SetInMovements()
        _currentSlideState = SlideState.Done

        _slideTimer = New Timer()
        _slideTimer.Interval = _movementInterval
        _slideTimer.Enabled = False
        AddHandler _slideTimer.Tick, AddressOf Tick

        _slideInDirection = _defaultSlideInDirection
        _slideOutDirection = _defaultSlideOutDirection
    End Sub

#End Region

#Region "Properties"

    Public Property DistplayTime As Integer
        Get
            Return _displayTime
        End Get
        Set(value As Integer)
            _displayTime = value
        End Set
    End Property

    <Category("Appearance"), Browsable(True)> Public Property SlideText As String
        Get
            Return _text
        End Get
        Set(value As String)
            AddMessageToQueue(value, _defaultSlideInDirection, _defaultSlideOutDirection, _defaultDisplayTime)
        End Set
    End Property

#End Region

    'Sub New(text As String)
    '    InitializeComponent()

    '    ' _text = text
    '    _slideInDirection = SlideDirection.DefaultSlide
    '    _slideOutDirection = SlideDirection.DefaultSlide
    'End Sub

    'Sub New(text As String, displayTime As Integer)
    '    InitializeComponent()

    '    ' _text = text
    '    _displayTime = displayTime
    '    _slideInDirection = SlideDirection.DefaultSlide
    '    _slideOutDirection = SlideDirection.DefaultSlide
    'End Sub

    'Sub New(text As String, displayTime As Integer, slideInDirection As SlideDirection)
    '    InitializeComponent()

    '    ' _text = text
    '    _displayTime = displayTime
    '    _slideInDirection = slideInDirection
    '    _slideOutDirection = SlideDirection.DefaultSlide
    'End Sub

    'Sub New(text As String, displayTime As Integer, slideInDirection As SlideDirection, slideOutDirection As SlideDirection)
    '    InitializeComponent()

    '    ' _text = text
    '    _displayTime = displayTime
    '    _slideInDirection = slideInDirection
    '    slideOutDirection = slideOutDirection
    'End Sub

#Region "Methods"

    ''' <summary>
    ''' Primary text renderer.
    ''' </summary>
    ''' <param name="canvas"></param>
    Public Sub DrawText(canvas As Graphics)
        Dim textSize = canvas.MeasureString(Me.SlideText, Me.Font)
        canvas.Clear(Me.BackColor)
        Using textBrush = New SolidBrush(Me.ForeColor)
            canvas.DrawString(Me.SlideText, Me.Font, textBrush, _currentX, _currentY)
        End Using
    End Sub

    ''' <summary>
    ''' Adds new message to queue.
    ''' </summary>
    ''' <param name="text">Text to be displayed.</param>
    ''' <param name="slideInDirection">Slide in direction.</param>
    ''' <param name="slideOutDirection">Slide out direction.</param>
    ''' <param name="displayTime">How long (in seconds) the text will be displayed before sliding out. 0 = forever.</param>
    Public Sub NewSlideMessage(text As String, Optional slideInDirection As SlideDirection = SlideDirection.Up, Optional slideOutDirection As SlideDirection = SlideDirection.Left, Optional displayTime As Integer = 4)
        AddMessageToQueue(text, slideInDirection, slideOutDirection, displayTime)
    End Sub

    ''' <summary>
    ''' Returns a <see cref="ToolStripControlHost"/> of this control for insertion into tool strips/status strips.
    ''' </summary>
    ''' <param name="parentControl">Target strip for this control. For inheriting font and color.</param>
    ''' <returns></returns>
    Public Function ToToolStripControl(Optional parentControl As Control = Nothing) As ToolStripControlHost
        Me.AutoSize = True
        If parentControl IsNot Nothing Then
            Me.Font = parentControl.Font
            Me.BackColor = parentControl.BackColor
        End If
        Dim stripSlider = New ToolStripControlHost(Me)
        stripSlider.AutoSize = False
        Return stripSlider
    End Function

    ''' <summary>
    ''' Adds a new text message to the queue.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <param name="slideInDirection"></param>
    ''' <param name="slideOutDirection"></param>
    ''' <param name="displayTime"></param>
    Private Sub AddMessageToQueue(text As String, slideInDirection As SlideDirection, slideOutDirection As SlideDirection, displayTime As Integer)
        _messageQueue.Add(New MessageParameters(text, slideInDirection, slideOutDirection, displayTime))
        ProcessQueue()
    End Sub

    ''' <summary>
    ''' Displays the specified text and starts the animation.
    ''' </summary>
    ''' <param name="message"></param>
    Private Sub DisplayText(message As MessageParameters)
        If message.Message <> "" Then
            _text = message.Message
            _displayTime = message.DisplayTime
            _textSize = GetTextSize(message.Message)
            _slideInDirection = message.SlideInDirection
            _slideOutDirection = message.SlideOutDirection
            SetControlSize()
            SetInMovements()
            _slideTimer.Enabled = True
            Me.Invalidate()
            Me.Update()
        End If

    End Sub

    ''' <summary>
    ''' Measures the graphical size of the specified text.
    ''' </summary>
    ''' <param name="text"></param>
    ''' <returns></returns>
    Private Function GetTextSize(text As String) As SizeF
        Using gfx = Me.CreateGraphics
            Return gfx.MeasureString(text, Me.Font)
        End Using
    End Function

    ''' <summary>
    ''' Async pause task.
    ''' </summary>
    ''' <param name="pauseTime"></param>
    ''' <returns></returns>
    Private Function Pause(pauseTime As Integer) As Task
        Return Task.Run(Sub()
                            Task.Delay(pauseTime * 1000).Wait()
                        End Sub)
    End Function

    ''' <summary>
    ''' Handles the message queue. Messages are queued until they their animation is complete. Messages with a display time of 0 are moved to the slide out animation status, then replaced with the next message when complete.
    ''' </summary>
    Private Sub ProcessQueue()
        If _messageQueue.Count > 0 Then
            If _currentSlideState = SlideState.Done Then
                DisplayText(_messageQueue.Last)
                _messageQueue.RemoveAt(_messageQueue.Count - 1)
            ElseIf _currentSlideState = SlideState.Hold Then
                SetOutMovements()
                _slideTimer.Enabled = True
            End If
        End If

    End Sub

    ''' <summary>
    ''' If autosize set to true, sets the control size to fit the text.
    ''' </summary>
    Private Sub SetControlSize()
        Me.BackColor = Me.Parent.BackColor
        If Me.AutoSize Then
            Me.Size = GetTextSize(_text).ToSize
        End If
    End Sub

    ''' <summary>
    ''' Sets states, current positions and ending positions for a slide-in animation.
    ''' </summary>
    Private Sub SetInMovements()
        _currentDirection = _slideInDirection
        _currentSlideState = SlideState.SlideIn
        _currentX = 0
        _currentY = 0
        _currentSpeed = 0
        Select Case _slideInDirection

            Case SlideDirection.DefaultSlide, SlideDirection.Up
                _yStart = _textSize.Height
                _currentY = _yStart
                _yEnd = 0

            Case SlideDirection.Down
                _yStart = -_textSize.Height
                _currentY = _yStart
                _yEnd = 0

            Case SlideDirection.Left
                _xStart = _textSize.Width
                _currentX = _xStart
                _xEnd = 0

            Case SlideDirection.Right
                _xStart = -_textSize.Width
                _currentX = _xStart
                _xEnd = 0
        End Select
    End Sub

    ''' <summary>
    ''' Sets states, current positions and ending positions for a slide-out animation.
    ''' </summary>
    Private Sub SetOutMovements()
        _currentDirection = _slideOutDirection
        _currentSlideState = SlideState.SlideOut
        _currentSpeed = 0

        Select Case _slideOutDirection

            Case SlideDirection.DefaultSlide, SlideDirection.Up
                _yEnd = -_textSize.Height

            Case SlideDirection.Down
                _yEnd = _textSize.Height

            Case SlideDirection.Left
                _xEnd = -_textSize.Width

            Case SlideDirection.Right
                _xEnd = _textSize.Width
        End Select
    End Sub

    Private Sub SliderLabel_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetControlSize()
    End Sub

    Private Sub SliderTextBox_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        DrawText(e.Graphics)
    End Sub

    ''' <summary>
    ''' Timer tick event for animation.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Tick(sender As Object, e As EventArgs)
        UpdateTextPosition()
    End Sub

    ''' <summary>
    ''' Primary animation routine. Messages are animated per their current state and specified directions.
    ''' </summary>
    Private Async Sub UpdateTextPosition()
        'Text position has moved, trigger redraw.
        Dim PositionChanged As Boolean = False
        'Current slide animation is complete, move to next state.
        Dim SlideComplete As Boolean = False

        'Check current direction and change X,Y positions/speeds as needed using an accumulating acceleration.
        Select Case _currentDirection
            Case SlideDirection.DefaultSlide, SlideDirection.Up
                If _currentY > _yEnd Then
                    _currentSpeed -= _acceleration
                    _currentY += _currentSpeed
                    PositionChanged = True
                Else
                    _currentY = _yEnd
                    SlideComplete = True
                End If
            Case SlideDirection.Down
                If _currentY < _yEnd Then
                    _currentSpeed += _acceleration
                    _currentY += _currentSpeed
                    PositionChanged = True
                Else
                    _currentY = _yEnd
                    SlideComplete = True
                End If
            Case SlideDirection.Left
                If _currentX > _xEnd Then
                    _currentSpeed -= _acceleration
                    _currentX += _currentSpeed
                    PositionChanged = True
                Else
                    _currentX = _xEnd
                    SlideComplete = True
                End If
            Case SlideDirection.Right
                If _currentX < _xEnd Then
                    _currentSpeed += _acceleration
                    _currentX += _currentSpeed
                    PositionChanged = True
                Else
                    _currentX = _xEnd
                    SlideComplete = True
                End If
        End Select

        'Trigger redraw.
        Me.Invalidate()
        Me.Update()

        'Current slide animation complete.
        If SlideComplete Then

            'Reset speed.
            _currentSpeed = 0

            'If current state is slide-in and display time is not forever.
            If _currentSlideState = SlideState.SlideIn And _displayTime > 0 Then

                'Stop the animation timer, change state to paused, and pause for the specified display time.
                _slideTimer.Enabled = False
                _currentSlideState = SlideState.Paused

                'Asynchronous wait task. (Keeps UI alive)
                Await Pause(_displayTime)

                'Once the wait is complete, set the next state (slide-out) and re-start the animation timer.
                SetOutMovements()
                _slideTimer.Enabled = True
            Else
                'If the display time is forever
                If _displayTime = 0 Then

                    'If the forever displayed message state is slide-out, change the state to done.
                    If _currentSlideState = SlideState.SlideOut Then
                        _currentSlideState = SlideState.Done
                    Else
                        'Otherwise, change the forever displayed message state to hold to keep it visible.
                        _currentSlideState = SlideState.Hold
                    End If
                Else
                    'If the message actually has a display time, set state to done.
                    _currentSlideState = SlideState.Done
                End If

                'Stop the animation timer.
                _slideTimer.Enabled = False

                'Add pause between messages if desired.
                'Await Pause(1)
            End If

        End If

        'Check the queue for new messages.
        ProcessQueue()
    End Sub

#End Region

#Region "Structs"

    ''' <summary>
    ''' Parameters for messages to be queued.
    ''' </summary>
    Private Structure MessageParameters

#Region "Fields"

        Public DisplayTime As Integer
        Public Message As String
        Public SlideInDirection As SlideDirection
        Public SlideOutDirection As SlideDirection

#End Region

#Region "Constructors"

        Sub New(message As String, slideInDirection As SlideDirection, slideOutDirection As SlideDirection, displayTime As Integer)
            Me.Message = message
            Me.DisplayTime = displayTime
            Me.SlideInDirection = slideInDirection
            Me.SlideOutDirection = slideOutDirection
        End Sub

#End Region

    End Structure

#End Region

End Class
