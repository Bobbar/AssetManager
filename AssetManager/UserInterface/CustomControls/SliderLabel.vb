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

    Private Const defaultDisplayTime As Integer = 4
    Private Const defaultSlideInDirection As SlideDirection = SlideDirection.Up
    Private Const defaultSlideOutDirection As SlideDirection = SlideDirection.Left
    '  Private stepSize As Single = 0.25
    Private Acceleration As Single = 0.5

    Private CurrentDirection As SlideDirection
    Private CurrentSlideState As SlideState = SlideState.Done
    Private CurrentSpeed As Single = 0
    Private DisplayTime As Integer = 4
    Private MessageQueue As New List(Of MessageParameters)
    Private AnimationTimerInterval As Integer = 15
    Private SlideInDirection As SlideDirection
    Private SlideOutDirection As SlideDirection
    Private SlideTimer As System.Timers.Timer
    Private TextSize As SizeF
    Private StartPosition As New PointF
    Private EndPosition As New PointF
    Private CurrentPosition As New PointF
    Private SlideComplete As Boolean = False
    Private lastPositionRect As RectangleF


#End Region

#Region "Constructors"

    Sub New()
        InitializeComponent()

        Me.SetStyle(ControlStyles.OptimizedDoubleBuffer, True)
        Me.SetStyle(ControlStyles.AllPaintingInWmPaint, True)
        Me.SetStyle(ControlStyles.UserPaint, True)
        Me.SetStyle(ControlStyles.ResizeRedraw, True)

        SlideTimer = New System.Timers.Timer
        SlideTimer.Interval = AnimationTimerInterval
        SlideTimer.Stop()
        AddHandler SlideTimer.Elapsed, AddressOf Tick

        SlideInDirection = defaultSlideInDirection
        SlideOutDirection = defaultSlideOutDirection
    End Sub

#End Region

#Region "Properties"

    Public Property DistplayTime As Integer
        Get
            Return DisplayTime
        End Get
        Set(value As Integer)
            DisplayTime = value
        End Set
    End Property

    <Category("Appearance"), Browsable(True)> Public Property SlideText As String
        Get
            Return Text
        End Get
        Set(value As String)
            AddMessageToQueue(value, defaultSlideInDirection, defaultSlideOutDirection, defaultDisplayTime)
        End Set
    End Property

#End Region

    'Sub New(text As String)
    '    InitializeComponent()

    '    ' text = text
    '    slideInDirection = SlideDirection.DefaultSlide
    '    slideOutDirection = SlideDirection.DefaultSlide
    'End Sub

    'Sub New(text As String, displayTime As Integer)
    '    InitializeComponent()

    '    ' text = text
    '    displayTime = displayTime
    '    slideInDirection = SlideDirection.DefaultSlide
    '    slideOutDirection = SlideDirection.DefaultSlide
    'End Sub

    'Sub New(text As String, displayTime As Integer, slideInDirection As SlideDirection)
    '    InitializeComponent()

    '    ' text = text
    '    displayTime = displayTime
    '    slideInDirection = slideInDirection
    '    slideOutDirection = SlideDirection.DefaultSlide
    'End Sub

    'Sub New(text As String, displayTime As Integer, slideInDirection As SlideDirection, slideOutDirection As SlideDirection)
    '    InitializeComponent()

    '    ' text = text
    '    displayTime = displayTime
    '    slideInDirection = slideInDirection
    '    slideOutDirection = slideOutDirection
    'End Sub

#Region "Methods"

    ''' <summary>
    ''' Primary text renderer.
    ''' </summary>
    ''' <param name="canvas"></param>
    Public Sub DrawText(canvas As Graphics)
        canvas.Clear(Me.BackColor)
        Using textBrush = New SolidBrush(Me.ForeColor)
            canvas.DrawString(Me.SlideText, Me.Font, textBrush, CurrentPosition)
        End Using
        lastPositionRect = New RectangleF(CurrentPosition.X, CurrentPosition.Y, TextSize.Width, TextSize.Height)
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
        MessageQueue.Add(New MessageParameters(text, slideInDirection, slideOutDirection, displayTime))
        ProcessQueue()
    End Sub

    ''' <summary>
    ''' Displays the specified text and starts the animation.
    ''' </summary>
    ''' <param name="message"></param>
    Private Sub DisplayText(message As MessageParameters)
        If message.Message <> "" Then
            Text = message.Message
            DisplayTime = message.DisplayTime
            TextSize = GetTextSize(message.Message)
            SlideInDirection = message.SlideInDirection
            SlideOutDirection = message.SlideOutDirection
            SetControlSize()
            SetSlideInAnimation()
            SlideTimer.Start()
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
        Try
            Using gfx = Me.CreateGraphics
                Return gfx.MeasureString(text, Me.Font)
            End Using
        Catch ex As ObjectDisposedException
            'We've been disposed. Do nothing.
        End Try
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
        If MessageQueue.Count > 0 Then
            'If state is done, then we can display the next message
            If CurrentSlideState = SlideState.Done Then
                DisplayText(MessageQueue.Last)
                MessageQueue.RemoveAt(MessageQueue.Count - 1)
                'If the state is hold, then a permanent message is currently displayed. Trigger a slide out animation, which will change the state to done once complete.
            ElseIf CurrentSlideState = SlideState.Hold Then
                SetSlideOutAnimation()
                SlideTimer.Start()
            End If
        End If

    End Sub

    ''' <summary>
    ''' If autosize set to true, sets the control size to fit the text.
    ''' </summary>
    Private Sub SetControlSize()
        Me.BackColor = Me.Parent.BackColor
        If Me.AutoSize Then
            Me.Size = GetTextSize(Text).ToSize
        End If
    End Sub

    ''' <summary>
    ''' Sets states, current positions and ending positions for a slide-in animation.
    ''' </summary>
    Private Sub SetSlideInAnimation()
        CurrentDirection = SlideInDirection
        CurrentSlideState = SlideState.SlideIn
        CurrentPosition = New PointF(0, 0)
        CurrentSpeed = 0
        SlideComplete = False
        Select Case SlideInDirection

            Case SlideDirection.DefaultSlide, SlideDirection.Up
                StartPosition.Y = TextSize.Height
                CurrentPosition.Y = StartPosition.Y
                EndPosition.Y = 0

            Case SlideDirection.Down
                StartPosition.Y = -TextSize.Height
                CurrentPosition.Y = StartPosition.Y
                EndPosition.Y = 0

            Case SlideDirection.Left
                StartPosition.X = TextSize.Width
                CurrentPosition.X = StartPosition.X
                EndPosition.X = 0

            Case SlideDirection.Right
                StartPosition.X = -TextSize.Width
                CurrentPosition.X = StartPosition.X
                EndPosition.X = 0

        End Select
    End Sub

    ''' <summary>
    ''' Sets states, current positions and ending positions for a slide-out animation.
    ''' </summary>
    Private Sub SetSlideOutAnimation()
        CurrentDirection = SlideOutDirection
        CurrentSlideState = SlideState.SlideOut
        CurrentSpeed = 0
        SlideComplete = False
        Select Case SlideOutDirection

            Case SlideDirection.DefaultSlide, SlideDirection.Up
                EndPosition.Y = -TextSize.Height

            Case SlideDirection.Down
                EndPosition.Y = TextSize.Height

            Case SlideDirection.Left
                EndPosition.X = -TextSize.Width

            Case SlideDirection.Right
                EndPosition.X = TextSize.Width

        End Select
    End Sub

    Private Sub SliderLabelLoad(sender As Object, e As EventArgs) Handles Me.Load
        SetControlSize()
    End Sub

    Private Sub SliderTextBoxPaint(sender As Object, e As PaintEventArgs) Handles Me.Paint
        DrawText(e.Graphics)
    End Sub

    Delegate Sub UpdateTextDelegate()

    ''' <summary>
    ''' Timer tick event for animation.
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Tick(sender As Object, e As EventArgs)
        Try
            If Not Me.Disposing AndAlso Not Me.IsDisposed Then
                Dim d As New UpdateTextDelegate(AddressOf UpdateTextPosition)
                Me.Invoke(d)
            Else
                SlideTimer.Stop()
                SlideTimer.Dispose()
            End If
        Catch ex As ObjectDisposedException
            'We've been disposed. Do nothing.
        End Try
    End Sub



    ''' <summary>
    ''' Primary animation routine. Messages are animated per their current state and specified directions.
    ''' </summary>
    Private Async Sub UpdateTextPosition()

        'Check current direction and change X,Y positions/speeds as needed using an accumulating acceleration.
        Select Case CurrentDirection
            Case SlideDirection.DefaultSlide, SlideDirection.Up
                If CurrentPosition.Y + CurrentSpeed > EndPosition.Y Then
                    CurrentSpeed -= Acceleration
                    CurrentPosition.Y += CurrentSpeed
                Else
                    CurrentPosition.Y = EndPosition.Y
                    SlideComplete = True
                End If
            Case SlideDirection.Down
                If CurrentPosition.Y + CurrentSpeed < EndPosition.Y Then
                    CurrentSpeed += Acceleration
                    CurrentPosition.Y += CurrentSpeed
                Else
                    CurrentPosition.Y = EndPosition.Y
                    SlideComplete = True
                End If
            Case SlideDirection.Left
                If CurrentPosition.X + CurrentSpeed > EndPosition.X Then
                    CurrentSpeed -= Acceleration
                    CurrentPosition.X += CurrentSpeed
                Else
                    CurrentPosition.X = EndPosition.X
                    SlideComplete = True
                End If
            Case SlideDirection.Right
                If CurrentPosition.X + CurrentSpeed < EndPosition.X Then
                    CurrentSpeed += Acceleration
                    CurrentPosition.X += CurrentSpeed
                Else
                    CurrentPosition.X = EndPosition.X
                    SlideComplete = True
                End If
        End Select

        'Trigger redraw.
        lastPositionRect.Inflate(10, 5)
        Dim updateRegion As Region = New Region(lastPositionRect)
        Me.Invalidate(updateRegion)
        Me.Update()

        'Current slide animation complete.
        If SlideComplete Then

            'Reset speed.
            CurrentSpeed = 0

            'If current state is slide-in and display time is not forever.
            If CurrentSlideState = SlideState.SlideIn And DisplayTime > 0 Then

                'Stop the animation timer, change state to paused, and pause for the specified display time.
                SlideTimer.Stop()
                CurrentSlideState = SlideState.Paused

                'Asynchronous wait task. (Keeps UI alive)
                Await Pause(DisplayTime)

                'Once the wait is complete, set the next state (slide-out) and re-start the animation timer.
                SetSlideOutAnimation()
                SlideTimer.Start()
            Else
                'If the display time is forever
                If DisplayTime = 0 Then

                    'If the forever displayed message state is slide-out, then the forever message is being replaced with a new message, so change the state to done.
                    If CurrentSlideState = SlideState.SlideOut Then
                        CurrentSlideState = SlideState.Done
                    Else
                        'Otherwise, change the forever displayed message state to hold to keep it visible.
                        CurrentSlideState = SlideState.Hold
                    End If
                Else
                    'If the message has a display time, set state to done.
                    CurrentSlideState = SlideState.Done
                End If

                'Stop the animation timer.
                SlideTimer.Stop()

                'Add pause between messages if desired.
                'Await Pause(1)
            End If

        End If

        'Check the queue for new messages.
        ProcessQueue()
    End Sub


    Private Sub SliderLabel_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        MessageQueue.Clear()
        SlideTimer.Stop()
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
