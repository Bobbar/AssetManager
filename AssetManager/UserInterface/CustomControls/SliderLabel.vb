
Imports System.ComponentModel
Imports System.Drawing

Public Class SliderLabel
    Private _text As String
    Private _displayTime As Integer = 4
    Private _slideInDirection As SlideDirection
    Private _slideOutDirection As SlideDirection
    Private _xStart, _xEnd, _yStart, _yEnd As Single
    Private _currentY As Single = 0
    Private _currentX As Single = 0
    Private _currentSpeed As Single = 0
    Private _currentDirection As SlideDirection
    Private _currentSlideState As SlideState
    '  Private _stepSize As Single = 0.25
    Private _acceleration As Single = 0.25
    Private _movementInterval As Integer = 10
    Private _textSize As SizeF

    Private _slideTimer As Timer

    Private _messageQueue As New List(Of MessageParameters)


    <Category("Appearance"), Browsable(True)> Public Property SlideText As String
        Get
            Return _text
        End Get
        Set(value As String)
            _text = value
            If _text <> "" Then
                _textSize = GetTextSize(_text)
                SetSize()
                SetInMovements()
                _slideTimer.Enabled = True
                Me.Invalidate()
                Me.Update()
            End If

        End Set
    End Property

    Public Property DistplayTime As Integer
        Get
            Return _displayTime
        End Get
        Set(value As Integer)
            _displayTime = value
        End Set
    End Property

    Sub New()
        InitializeComponent()


        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.ResizeRedraw, True)


        SetInMovements()

        _currentSlideState = SlideState.Paused


        '_yStart = Me.Height
        '_yEnd = 0
        '_currentY = Me.Height
        _slideTimer = New Timer()
        _slideTimer.Interval = _movementInterval
        _slideTimer.Enabled = True
        AddHandler _slideTimer.Tick, AddressOf Tick

        ' _text = ""
        _slideInDirection = SlideDirection.Up
        _slideOutDirection = SlideDirection.Left
    End Sub

    Sub New(text As String)
        InitializeComponent()

        ' _text = text
        _slideInDirection = SlideDirection.DefaultSlide
        _slideOutDirection = SlideDirection.DefaultSlide
    End Sub

    Sub New(text As String, displayTime As Integer)
        InitializeComponent()

        ' _text = text
        _displayTime = displayTime
        _slideInDirection = SlideDirection.DefaultSlide
        _slideOutDirection = SlideDirection.DefaultSlide
    End Sub

    Sub New(text As String, displayTime As Integer, slideInDirection As SlideDirection)
        InitializeComponent()

        ' _text = text
        _displayTime = displayTime
        _slideInDirection = slideInDirection
        _slideOutDirection = SlideDirection.DefaultSlide
    End Sub

    Sub New(text As String, displayTime As Integer, slideInDirection As SlideDirection, slideOutDirection As SlideDirection)
        InitializeComponent()

        ' _text = text
        _displayTime = displayTime
        _slideInDirection = slideInDirection
        slideOutDirection = slideOutDirection
    End Sub

    'Private Sub GetPaths(text As String)
    '    Using gfx = Me.CreateGraphics

    '        Dim textSize = gfx.MeasureString(text, Me.Font)
    '        _yEnd = 


    '    End Using
    'End Sub

    Private Sub Tick(sender As Object, e As EventArgs)
        '   Debug.Print(_currentY.ToString)
        UpdateTextPosition()


    End Sub

    Private Sub AddMessageToQueue(text As String, Optional displayTime As Integer =  )


    Private Async Sub UpdateTextPosition()
        Dim PositionChanged As Boolean = False
        Dim SlideComplete As Boolean = False
        Dim SlideOutComplete As Boolean = False
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

        '  If PositionChanged Then
        Me.Invalidate()
        Me.Update()
        ' End If

        If SlideComplete Then
            _currentSpeed = 0
            If _currentSlideState = SlideState.SlideIn Then
                _slideTimer.Enabled = False

                Await Task.Run(Sub()
                                   Task.Delay(_displayTime * 1000).Wait()
                               End Sub)

                SetOutMovements()
                _slideTimer.Enabled = True
            Else
                _slideTimer.Enabled = False
            End If

        End If


    End Sub

    Private Sub SetInMovements()
        _currentDirection = _slideInDirection
        _currentSlideState = SlideState.SlideIn
        _currentX = 0
        _currentY = 0
        _currentSpeed = 0
        Select Case _slideInDirection
            Case SlideDirection.DefaultSlide, SlideDirection.Up
                '_yStart = Me.Height
                _yStart = _textSize.Height
                _currentY = _yStart
                _yEnd = 0
            Case SlideDirection.Down
                ' _yStart = -Me.Height
                _yStart = -_textSize.Height
                _currentY = _yStart
                _yEnd = 0
            Case SlideDirection.Left
                '  _xStart = Me.Width
                _xStart = _textSize.Width
                _currentX = _xStart
                _xEnd = 0
            Case SlideDirection.Right
                '  _xStart = -Me.Width
                _xStart = -_textSize.Width
                _currentX = _xStart
                _xEnd = 0
        End Select
    End Sub

    Private Sub SetOutMovements()
        _currentDirection = _slideOutDirection
        _currentSlideState = SlideState.SlideOut
        _currentSpeed = 0

        Select Case _slideOutDirection
            Case SlideDirection.DefaultSlide, SlideDirection.Up

                ' _yEnd = -Me.Height
                _yEnd = -_textSize.Height
            Case SlideDirection.Down

                ' _yEnd = Me.Height
                _yEnd = _textSize.Height
            Case SlideDirection.Left

                '_xEnd = -Me.Width
                _xEnd = -_textSize.Width
            Case SlideDirection.Right

                ' _xEnd = Me.Width
                _xEnd = _textSize.Width
        End Select

    End Sub
    Private Function GetTextSize(text As String) As SizeF
        Using gfx = Me.CreateGraphics
            Return gfx.MeasureString(text, Me.Font)
        End Using
    End Function

    Public Sub Slide(text As String)


        'Using gfx = Me.CreateGraphics, textBrush = New SolidBrush(Me.ForeColor)
        '    gfx.DrawString(Me.Text, Me.Font, textBrush, 0, 0)
        'End Using


    End Sub

    Public Sub DrawText(canvas As Graphics)
        Dim textSize = canvas.MeasureString(Me.SlideText, Me.Font)

        ' Debug.Print(_currentY.ToString)
        '   For y = 0 To textSize.Height
        canvas.Clear(Me.BackColor)
        Using textBrush = New SolidBrush(Me.ForeColor)
            canvas.DrawString(Me.SlideText, Me.Font, textBrush, _currentX, _currentY)
        End Using



        ' Next




        'canvas.Clear(Me.BackColor)
        'Using textBrush = New SolidBrush(Me.ForeColor)
        '    canvas.DrawString(Me.SlideText, Me.Font, textBrush, ClientRectangle)
        'End Using



    End Sub

    Private Sub SliderTextBox_Paint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        DrawText(e.Graphics)
    End Sub

    Private Sub SliderLabel_Load(sender As Object, e As EventArgs) Handles Me.Load
        SetSize()
    End Sub

    Private Sub SetSize()
        If Me.AutoSize Then
            Me.Size = _textSize.ToSize
        End If
    End Sub

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


    Private Structure MessageParameters
        Public Message As String
        Public DisplayTime As Integer

        Sub New(message As String, displayTime As Integer)
            Me.Message = message
            Me.DisplayTime = displayTime

        End Sub
    End Structure


End Class

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
End Enum




