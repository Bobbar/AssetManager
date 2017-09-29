Option Explicit On
Imports System.Net.NetworkInformation
Imports System.Net
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Text

Public Class PingVis : Implements IDisposable
    Private MyPing As New Ping
    Private PingReplies As New List(Of PingInfo)
    Private MyPingHostname As String
    Private PingRunning As Boolean = False
    Private WithEvents PingTimer As New Timer
    Private DrawControl As Control
    Private ImageWidth As Integer
    Private ImageHeight As Integer
    Private MouseIsScrolling As Boolean = False
    Private TopBarIndex As Integer = 0
    Private MouseOverInfo As MouseOverInfoStruct
    Private LastDraw As Integer = 0
    Private ScrollingBars As List(Of PingBar)
    Private PrevScaleMax As Long = 1000

#Region "Ping Parameters"
    Private Const Timeout As Integer = 1000
    Private Const GoodPingInterval As Integer = 1000
    Private Const NoPingInterval As Integer = 5000
    Private CurrentPingInterval As Integer = GoodPingInterval
#End Region

#Region "Bar Parameters"
    Private Const BarGap As Single = 0
    Private Const MaxBars As Integer = 10
    Private Const MinBarLength As Integer = 2
    Private Const BarTopPadding As Single = 0
    Private Const BarBottomPadding As Single = 5
#End Region

#Region "Misc PingVis Variables"
    Private InitialScale As Single = 4
    Private Const MaxStoredResults As Integer = 1000000
    Private Const MaxDrawRate As Integer = 20
    Private ScaleMulti As Integer = 2
#End Region
    Public ReadOnly Property CurrentResult As PingInfo
        Get
            If PingReplies.Count > 0 Then
                Return PingReplies.Last
            Else
                Return Nothing
            End If
        End Get
    End Property
    Sub New(destControl As Control, hostName As String)
        InitMyControl(destControl)
        MyPingHostname = hostName
        InitPing()
    End Sub
    Private Sub InitMyControl(destControl As Control)
        DrawControl = destControl
        'Set image to double the size of the control
        ImageWidth = DrawControl.ClientSize.Width * ScaleMulti
        ImageHeight = DrawControl.ClientSize.Height * ScaleMulti
        AddHandler DrawControl.MouseWheel, AddressOf ControlMouseWheel
        AddHandler DrawControl.MouseLeave, AddressOf ControlMouseLeave
        AddHandler DrawControl.MouseMove, AddressOf ControlMouseMove
    End Sub
    Private Sub InitPing()
        ServicePointManager.DnsRefreshTimeout = 0
        InitTimer()
        StartPing()
    End Sub
    Private Sub InitTimer()
        If Not Me.disposedValue Then
            If PingTimer IsNot Nothing Then
                RemoveHandler PingTimer.Tick, AddressOf PingTimer_Tick
                PingTimer.Dispose()
                PingTimer = Nothing
                PingTimer = New Timer
            End If
            PingTimer.Interval = CurrentPingInterval
            PingTimer.Enabled = True
            AddHandler PingTimer.Tick, AddressOf PingTimer_Tick
        End If
    End Sub
    Private Sub PingTimer_Tick(sender As Object, e As EventArgs)
        StartPing()
        PingTimer.Interval = CurrentPingInterval
    End Sub
    Private Async Sub StartPing()
        Try
            If Not PingRunning Then
                Dim reply = Await GetPingReply(MyPingHostname)
                If reply.Status = IPStatus.Success Then
                    SetPingInterval(GoodPingInterval)
                Else
                    SetPingInterval(NoPingInterval)
                End If
                PingReplies.Add(New PingInfo(reply))
            End If
        Catch ex As Exception
            If Not Me.disposedValue Then
                PingReplies.Add(New PingInfo())
                SetPingInterval(NoPingInterval)
            Else
                Me.Dispose()
            End If
        Finally
            If Not MouseIsScrolling Then DrawBars(DrawControl, GetPingBars, MouseOverInfo)
        End Try
    End Sub
    Private Async Function GetPingReply(hostname As String) As Task(Of PingReply)
        Try
            PingRunning = True
            Dim options As New Net.NetworkInformation.PingOptions
            options.DontFragment = True
            Dim buff As Byte() = Encoding.ASCII.GetBytes("pingpingpingpingping")
            Return Await MyPing.SendPingAsync(hostname, Timeout, buff, options)
        Finally
            PingRunning = False
        End Try
    End Function
    Private Sub SetPingInterval(interval As Integer)
        If CurrentPingInterval <> interval Then
            CurrentPingInterval = interval
            InitTimer()
        End If
    End Sub
    Private Sub ControlMouseLeave(sender As Object, e As EventArgs)
        MouseIsScrolling = False
        MouseOverInfo = Nothing
        If ScrollingBars IsNot Nothing Then ScrollingBars.Clear()
        DrawBars(DrawControl, GetPingBars, MouseOverInfo)
    End Sub
    Private Sub ControlMouseWheel(sender As Object, e As MouseEventArgs)
        If PingReplies.Count > MaxBars Then
            MouseIsScrolling = True
            If e.Delta < 0 Then 'scroll up
                Dim NewIdx As Integer = TopBarIndex + 1
                If NewIdx > PingReplies.Count - MaxBars Then 'if the scroll index returns to the end (bottom) of the results, disable scrolling and return to normal display
                    TopBarIndex = PingReplies.Count - MaxBars
                    MouseIsScrolling = False
                Else
                    TopBarIndex = NewIdx
                End If
            ElseIf e.Delta > 0 Then 'scroll down
                Dim NewIdx As Integer = TopBarIndex - 1
                If NewIdx < 0 Then 'clamp the scroll index to always be greater than 0
                    TopBarIndex = 0
                Else
                    TopBarIndex = NewIdx
                End If
            End If
            DrawBars(DrawControl, GetPingBars)
        End If
    End Sub
    Private Sub ControlMouseMove(sender As Object, e As MouseEventArgs)
        If MouseIsScrolling Then
            MouseOverInfo = GetMouseOverPing(e.Location)
            DrawBars(DrawControl, ScrollingBars, MouseOverInfo)
        End If
    End Sub

    Private Function GetMouseOverPing(mPos As Point) As MouseOverInfoStruct
        Dim mScalePoint As New Point(mPos.X * ScaleMulti, mPos.Y * ScaleMulti)
        ScrollingBars = GetPingBars()
        For Each r As PingBar In ScrollingBars
            If r.Rectangle.Contains(mScalePoint) Then
                r.Brush = New SolidBrush(Color.Navy) 'Highlight MoveOver bar
                Return New MouseOverInfoStruct(mScalePoint, r.PingResult)
            End If
        Next
        Return Nothing
    End Function

    Private Sub DrawBars(ByRef DestControl As Control, ByRef Bars As List(Of PingBar), Optional MouseOverInfo As MouseOverInfoStruct = Nothing)
        If PingReplies.Count < 1 Or Not CanDraw(Environment.TickCount) Or DrawControl.FindForm.WindowState = FormWindowState.Minimized Then Exit Sub
        Try
            Using bm = New Drawing.Bitmap(ImageWidth, ImageHeight), gfx = Graphics.FromImage(bm)
                gfx.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                If Not MouseIsScrolling Then
                    gfx.Clear(DestControl.BackColor)
                Else
                    gfx.Clear(Color.FromArgb(48, 53, 61))
                End If
                SetScale()
                DrawScaleLines(gfx) 'Draw scale lines
                gfx.SmoothingMode = Drawing2D.SmoothingMode.None
                DrawPingBars(gfx, Bars) 'Draw ping bars
                gfx.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                DrawPingText(gfx, MouseOverInfo) 'Draw last ping round trip time
                DrawScrollBar(gfx)
                TrimPingList()
                'The bitmap is scaled back to control size using high quality transformations.
                'This improves the effect of anti-aliasing and therefore readability.
                '  bm.Save("C:\Temp\test.bmp")
                Dim bmRz = ResizeImage(bm, DestControl.ClientSize.Width, DestControl.ClientSize.Height)
                SetControlImage(DestControl, bmRz)
                DisposeBarList(Bars)
            End Using
        Catch
        Finally
        End Try
    End Sub

    Private Sub DisposeBarList(ByRef bars As List(Of PingBar))
        For Each bar In bars
            bar.Brush.Dispose()
        Next
        bars.Clear()
    End Sub

    Private Sub DrawScrollBar(ByRef gfx As Graphics)
        If MouseIsScrolling Then
            Using ScrollBrush As New SolidBrush(Color.White)
                Dim ScrollLocation As Integer = CInt(ImageHeight / (PingReplies.Count / TopBarIndex))
                gfx.FillRectangle(ScrollBrush, New RectangleF(ImageWidth - 15, ScrollLocation, 10, 5))
            End Using
        End If
    End Sub
    Private Function CanDraw(timeStamp As Integer) As Boolean
        Dim ElapTime As Integer = timeStamp - LastDraw
        If ElapTime >= MaxDrawRate Then
            LastDraw = timeStamp
            Return True
        End If
        Return False
    End Function
    Private Sub DrawPingText(ByRef gfx As Graphics, Optional mouseOverInfo As MouseOverInfoStruct = Nothing)
        Dim InfoFontSize As Single = 15
        Dim OverInfoFontSize As Single = 14
        If mouseOverInfo IsNot Nothing Then
            Dim OverInfoText As String = GetReplyStatusText(mouseOverInfo.PingReply)
            Using OverFont As Font = New Font("Tahoma", OverInfoFontSize, FontStyle.Regular)
                gfx.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                gfx.TextContrast = 0
                gfx.DrawString(OverInfoText, OverFont, Brushes.White, New PointF(mouseOverInfo.MouseLoc.X + 20, mouseOverInfo.MouseLoc.Y - 10)) '(intImgWidth / 2) - TextSize.Width / 2
            End Using
        End If
        If Not MouseIsScrolling Then
            Dim InfoText As String = GetReplyStatusText(PingReplies.Last)
            Using InfoFont As Font = New Font("Tahoma", InfoFontSize, FontStyle.Bold)
                Dim TextSize As SizeF = gfx.MeasureString(InfoText, InfoFont)
                gfx.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                gfx.TextContrast = 0
                gfx.DrawString(InfoText, InfoFont, Brushes.White, New PointF((ImageWidth - 5) - (TextSize.Width), (ImageHeight) - (TextSize.Height))) '(intImgWidth / 2) - TextSize.Width / 2
            End Using
        End If
    End Sub
    Private Function GetReplyStatusText(reply As PingInfo) As String
        Select Case reply.Status
            Case IPStatus.Success
                Return reply.RoundTripTime & "ms"
            Case IPStatus.TimedOut
                Return "T/O"
            Case Else
                Return "ERR"
        End Select
    End Function
    Private Sub DrawPingBars(ByRef gfx As Graphics, ByRef bars As List(Of PingBar))
        For Each bar As PingBar In bars
            gfx.FillRectangle(bar.Brush, bar.Rectangle)
            Using CapPen As Pen = New Pen(Color.ForestGreen, 2)
                gfx.DrawLine(CapPen, New PointF(bar.Length, bar.PositionY), New PointF(bar.Length, bar.PositionY + bar.Rectangle.Height))
            End Using
        Next
    End Sub
    Private Function GetPingBars() As List(Of PingBar)
        Dim NewPBars As New List(Of PingBar)
        Dim curPos As Single = BarTopPadding
        Dim BarHeight As Single = (ImageHeight - BarBottomPadding - BarTopPadding - (BarGap * MaxBars)) / MaxBars
        For Each result As PingInfo In CurrentDisplayResults()
            Dim BarRatio As Single
            Dim BarLen As Single
            Dim MyBrush As Brush
            If result.Status = Net.NetworkInformation.IPStatus.Success Then
                MyBrush = GetBarBrush(result.RoundTripTime)
                BarRatio = (Timeout / InitialScale) / result.RoundTripTime
                BarLen = (ImageWidth / BarRatio) + MinBarLength '* 2
            Else
                MyBrush = New SolidBrush(Color.Red)
                BarLen = ImageWidth - 2
            End If
            NewPBars.Add(New PingBar(BarLen, MyBrush, New Rectangle(1, CInt(curPos), CInt(BarLen), CInt(BarHeight)), curPos, result))
            curPos += BarHeight + BarGap
        Next
        Return NewPBars
    End Function

    Private Function FirstDrawIndex(barCount As Integer) As Integer
        If Not MouseIsScrolling Then
            If barCount <= MaxBars Then
                TopBarIndex = 0
                Return 0
            Else
                TopBarIndex = barCount - MaxBars
                Return barCount - MaxBars
            End If
        Else
            Return TopBarIndex
        End If
    End Function
    Private Function GetBarBrush(roundTrip As Long) As Brush
        'Alpha blending two colors. As ping times go up, the returned color becomes more red.
        Dim FadeColor As Integer
        Dim Color1, Color2 As Integer
        Dim r1, g1, b1, r2, g2, b2 As Long
        FadeColor = Color.Green.ToArgb 'low ping color
        Color1 = FadeColor
        r1 = Color1 And (Not &HFFFFFF00)
        g1 = (Color1 And (Not &HFFFF00FF)) \ &H100&
        b1 = (Color1 And (Not &HFF00FFFF)) \ &HFFFF&
        FadeColor = Color.Blue.ToArgb 'high ping color (invert of desired color Blue = Orange)
        Color2 = FadeColor
        r2 = Color2 And (Not &HFFFFFF00)
        g2 = (Color2 And (Not &HFFFF00FF)) \ &H100&
        b2 = (Color2 And (Not &HFF00FFFF)) \ &HFFFF&
        Dim iSteps As Integer = 255
        Dim iStep As Integer = CInt(255 / ((Timeout / 3) / roundTrip)) 'Convert ping time to ratio of 255. 255 being the maximum levels of blending.
        If iStep > iSteps Then iStep = iSteps
        FadeColor = RGB(CInt(r1 + (r2 - r1) / iSteps * iStep), CInt(g1 + (g2 - g1) / iSteps * iStep), CInt(b1 + (b2 - b1) / iSteps * iStep))
        Return New SolidBrush(ColorTranslator.FromOle(FadeColor))
    End Function
    Private Sub DrawScaleLines(ByRef gfx As Graphics)
        Dim ScaleLineLoc As Integer = 0
        Dim NumOfLines As Integer = CInt(Timeout / 10)
        Dim StepSize As Integer = 0
        For a As Integer = 0 To NumOfLines
            StepSize = CInt((((Timeout / 4)) / NumOfLines) * InitialScale)
            gfx.DrawLine(Pens.LightSlateGray, New Point(ScaleLineLoc, 0), New Point(ScaleLineLoc, ImageHeight))
            ScaleLineLoc += StepSize
        Next
    End Sub
    Private Sub TrimPingList()
        If PingReplies.Count > MaxStoredResults Then
            PingReplies = PingReplies.GetRange(PingReplies.Count - MaxStoredResults, MaxStoredResults)
        End If
    End Sub
    Private Sub SetScale()
        Dim MaxPing As Long = CurrentDisplayResults.OrderByDescending(Function(p) p.RoundTripTime).FirstOrDefault.RoundTripTime
        If MaxPing <> PrevScaleMax Then
            If MaxPing * 2 >= (Timeout / InitialScale) Then
                Do Until InitialScale <= 0 Or MaxPing * 2 < (Timeout / InitialScale)
                    InitialScale -= 0.1F
                Loop
            ElseIf MaxPing < ((Timeout / InitialScale) / 2) AndAlso InitialScale >= 0 Then
                Do Until InitialScale >= 15 Or MaxPing > ((Timeout / InitialScale) / 2)
                    InitialScale += 0.1F
                Loop
            End If
            PrevScaleMax = MaxPing
        End If
    End Sub
    Private Function CurrentDisplayResults() As List(Of PingInfo)
        If PingReplies.Count > MaxBars Then
            Return PingReplies.GetRange(FirstDrawIndex(PingReplies.Count), MaxBars)
        Else
            Return PingReplies
        End If
    End Function
    Private Sub SetControlImage(ByRef destControl As Control, ByRef image As Bitmap)
        Select Case True
            Case TypeOf destControl Is Form
                Dim frm As Form = DirectCast(destControl, Form)
                If frm.BackgroundImage IsNot Nothing Then frm.BackgroundImage.Dispose()
                frm.BackgroundImage = image
                frm.Invalidate()
            Case TypeOf destControl Is Button
                Dim but As Button = DirectCast(destControl, Button)
                If but.Image IsNot Nothing Then but.Image.Dispose()
                but.Image = image
                but.Invalidate()
            Case TypeOf destControl Is PictureBox
                Dim pic As PictureBox = DirectCast(destControl, PictureBox)
                If pic.Image IsNot Nothing Then pic.Image.Dispose()
                pic.Image = image
                pic.Refresh()
        End Select
    End Sub

    Public Shared Function ResizeImage(image As Image, width As Integer, height As Integer) As Bitmap
        Dim destRect = New Rectangle(0, 0, width, height)
        Dim destImage = New Bitmap(width, height)
        destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution)
        Using graphics__1 = Graphics.FromImage(destImage)
            graphics__1.CompositingMode = CompositingMode.SourceCopy
            graphics__1.CompositingQuality = CompositingQuality.HighQuality
            graphics__1.InterpolationMode = InterpolationMode.HighQualityBicubic
            '   graphics__1.SmoothingMode = SmoothingMode.HighQuality
            graphics__1.PixelOffsetMode = PixelOffsetMode.HighQuality
            Using wrapMode__2 = New ImageAttributes()
                wrapMode__2.SetWrapMode(WrapMode.TileFlipXY)
                graphics__1.DrawImage(image, destRect, 0, 0, image.Width, image.Height,
                GraphicsUnit.Pixel, wrapMode__2)
            End Using
        End Using
        Return destImage
    End Function
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
                PingTimer.Enabled = False
                RemoveHandler PingTimer.Tick, AddressOf PingTimer_Tick
                PingTimer.Dispose()
                PingTimer = Nothing
                RemoveHandler DrawControl.MouseWheel, AddressOf ControlMouseWheel
                RemoveHandler DrawControl.MouseLeave, AddressOf ControlMouseLeave
                RemoveHandler DrawControl.MouseMove, AddressOf ControlMouseMove
                MyPing.Dispose()
                PingReplies.Clear()
            End If

            ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
            ' TODO: set large fields to null.

        End If
        disposedValue = True
    End Sub

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
#End Region
    Public Class PingInfo
        Public Property Status As IPStatus
        Public Property RoundTripTime As Long
        Public Property Address As IPAddress
        Sub New()
            Status = IPStatus.Unknown
            RoundTripTime = 0
            Address = Nothing
        End Sub
        Sub New(reply As PingReply)
            Status = reply.Status
            RoundTripTime = reply.RoundtripTime
            Address = reply.Address
        End Sub
    End Class

    Private Class PingBar
        Private len As Single
        Private br As Brush
        Private rec As Rectangle
        Private pos As Single
        Private pInfo As PingInfo
        Public ReadOnly Property Length As Single
            Get
                Return len
            End Get
        End Property
        Public Property Brush As Brush
            Get
                Return br
            End Get
            Set(value As Brush)
                br = value
            End Set
        End Property
        Public ReadOnly Property Rectangle As Rectangle
            Get
                Return rec
            End Get
        End Property
        Public ReadOnly Property PositionY As Single
            Get
                Return pos
            End Get
        End Property
        Public ReadOnly Property PingResult As PingInfo
            Get
                Return pInfo
            End Get
        End Property
        Sub New(length As Single, ByRef brush As Brush, rect As Rectangle, posY As Single, ByVal pingInfo As PingInfo)
            len = length
            br = brush
            rec = rect
            pos = posY
            pInfo = pingInfo
        End Sub

    End Class
    Private Class MouseOverInfoStruct
        Public MouseLoc As Point
        Public PingReply As PingInfo
        Sub New(mLoc As Point, pReply As PingInfo)
            MouseLoc = mLoc
            PingReply = pReply
        End Sub
    End Class
End Class


