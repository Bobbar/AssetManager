Option Explicit On
Imports System.Net.NetworkInformation
Imports System.Net
Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing
Imports System.Windows.Forms
Imports System.Net.Sockets
Public Class PingVis : Implements IDisposable
    Private MyPing As New Ping
    Private pngResults As New List(Of PingReply)
    Private MyPingHostname As String
    Private bolPingRunning As Boolean = False
    Private WithEvents PingTimer As New Timer
    Private MyControl As Control
    Private intImgWidth As Integer
    Private intImgHeight As Integer
    Private LastReply As PingReply
    Private bolScrolling As Boolean = False
    Private intTopIndex As Integer = 0
    Private mOverInfo As MouseOverInfoStruct
    Private LastDraw As Integer = 0
    Private ScrollingBars As List(Of PingBar)
    Private intPrevMax As Long = 1000
    Private CurrentAverageRoundTripTime As Integer = 0

#Region "Ping Parameters"
    Private Const bolStopWhenNotFocused As Boolean = False 'Set to True to pause the pinging until focus is returned to the parent form.
    Private Const Timeout As Integer = 1000
    Private Const PingInterval As Integer = 1000
    Private Const NoPingInterval As Integer = 5000
    Private CurrentPingInterval As Integer = PingInterval
#End Region

#Region "Bar Parameters"
    Private Const BarGap As Single = 0
    Private Const intMaxBars As Integer = 10
    Private Const intMinBarLen As Integer = 2
    Private Const BarTopPadding As Single = 0
    Private Const BarBottomPadding As Single = 5
#End Region

#Region "Misc PingVis Variables"
    Private intInitialScale As Single = 4
    Private Const intMaxStoredResults As Integer = 1000000
    Private Const MaxDrawRate As Integer = 20
    Private ScaleMulti As Integer = 2
#End Region
    Public ReadOnly Property CurrentResult As PingReply
        Get
            Return LastReply
        End Get
    End Property
    Public ReadOnly Property AvgPingTime As Single
        Get
            Return CurrentAverageRoundTripTime
        End Get
    End Property
    Sub New(ByRef DestControl As Control, HostName As String)
        InitMyControl(DestControl)
        MyPingHostname = HostName
        InitPing()
    End Sub
    Private Sub InitMyControl(DestControl As Control)
        MyControl = DestControl
        'Set image to double the size of the control
        intImgWidth = MyControl.ClientSize.Width * ScaleMulti
        intImgHeight = MyControl.ClientSize.Height * ScaleMulti
        AddHandler MyControl.MouseWheel, AddressOf ControlMouseWheel
        AddHandler MyControl.MouseLeave, AddressOf ControlMouseLeave
        AddHandler MyControl.MouseMove, AddressOf ControlMouseMove
    End Sub
    Private Sub InitPing()
        AddHandler MyPing.PingCompleted, AddressOf PingComplete
        ServicePointManager.DnsRefreshTimeout = 0
        PingTimer.Interval = PingInterval
        PingTimer.Enabled = True
        AddHandler PingTimer.Tick, AddressOf PingTimer_Tick
        StartPing()
    End Sub
    Private Sub PingTimer_Tick(sender As Object, e As EventArgs)
        Try
            StartPing()
            PingTimer.Interval = CurrentPingInterval
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub
    Private Async Sub StartPing()
        Try
            Dim options As New Net.NetworkInformation.PingOptions
            options.DontFragment = True
            Dim IP = ReturnIPv4Address(Await Dns.GetHostAddressesAsync(MyPingHostname))
            If Not bolPingRunning Then
                If Not bolStopWhenNotFocused Then
                    MyPing.SendAsync(IP, Timeout, options)
                    bolPingRunning = True
                Else
                    If Form.ActiveForm Is MyControl.FindForm Then
                        MyPing.SendAsync(IP, Timeout, options)
                        bolPingRunning = True
                    End If
                End If
            End If
        Catch ex As SocketException
            CurrentPingInterval = NoPingInterval
            Exit Sub
        End Try
    End Sub
    Private Sub PingComplete(ByVal sender As Object, ByVal e As System.Net.NetworkInformation.PingCompletedEventArgs)
        bolPingRunning = False
        If Not e.Cancelled Then
            If e.Error Is Nothing Then
                If e.Reply.Status = IPStatus.Success Then
                    CurrentPingInterval = PingInterval
                Else
                    CurrentPingInterval = NoPingInterval
                End If
                pngResults.Add(e.Reply)
                LastReply = e.Reply
            Else
                CurrentPingInterval = NoPingInterval
                'Debug.Print(e.Error.Message)
            End If
            If Not bolScrolling Then DrawBars(MyControl, GetPingBars, mOverInfo)
        End If
    End Sub
    Private Function ReturnIPv4Address(IPs() As IPAddress) As String
        For Each ip In IPs
            If ip.AddressFamily = Sockets.AddressFamily.InterNetwork Then Return ip.ToString
        Next
        Return Nothing
    End Function
    Private Sub ControlMouseLeave(sender As Object, e As EventArgs)
        bolScrolling = False
        mOverInfo = Nothing
        DrawBars(MyControl, GetPingBars, mOverInfo)
    End Sub
    Private Sub ControlMouseWheel(sender As Object, e As MouseEventArgs)
        If pngResults.Count > intMaxBars Then
            bolScrolling = True
            If e.Delta < 0 Then 'scroll up
                Dim NewIdx As Integer = intTopIndex + 1
                If NewIdx > pngResults.Count - intMaxBars Then 'if the scroll index returns to the end (bottom) of the results, disable scrolling and return to normal display
                    intTopIndex = pngResults.Count - intMaxBars
                    bolScrolling = False
                Else
                    intTopIndex = NewIdx
                End If
            ElseIf e.Delta > 0 Then 'scroll down
                Dim NewIdx As Integer = intTopIndex - 1
                If NewIdx < 0 Then 'clamp the scroll index to always be greater than 0
                    intTopIndex = 0
                Else
                    intTopIndex = NewIdx
                End If
            End If
            DrawBars(MyControl, GetPingBars)
        End If
    End Sub
    Private Sub ControlMouseMove(sender As Object, e As MouseEventArgs)
        If bolScrolling Then
            mOverInfo = GetMouseOverPing(e.Location)
            DrawBars(MyControl, ScrollingBars, mOverInfo)
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
        If pngResults.Count < 1 Or Not CanDraw(Environment.TickCount) Then Exit Sub
        Try
            Using bm = New Drawing.Bitmap(intImgWidth, intImgHeight), gfx = Graphics.FromImage(bm)
                gfx.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                If Not bolScrolling Then
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
                SetAvgPing() 'Set public property containing current average round trip time
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
    Private Sub DisposeBarList(ByRef Bars As List(Of PingBar))
        For Each bar In Bars
            bar.Brush.Dispose()
        Next
        Bars.Clear()
    End Sub
    Private Sub DrawScrollBar(ByRef gfx As Graphics)
        If bolScrolling Then
            Using ScrollBrush As New SolidBrush(Color.White)
                Dim ScrollLocation As Integer = CInt(intImgHeight / (pngResults.Count / intTopIndex))
                gfx.FillRectangle(ScrollBrush, New RectangleF(intImgWidth - 15, ScrollLocation, 10, 5))
            End Using
        End If
    End Sub
    Private Function CanDraw(TimeStamp As Integer) As Boolean
        Dim ElapTime As Integer = TimeStamp - LastDraw
        If ElapTime >= MaxDrawRate Then
            LastDraw = TimeStamp
            Return True
        End If
        Return False
    End Function
    Private Sub DrawPingText(ByRef gfx As Graphics, Optional MouseOverInfo As MouseOverInfoStruct = Nothing)
        Dim InfoFontSize As Single = 15
        Dim OverInfoFontSize As Single = 14
        If MouseOverInfo IsNot Nothing Then
            Dim OverInfoText As String
            If MouseOverInfo.PingReply.Status = IPStatus.Success Then
                OverInfoText = MouseOverInfo.PingReply.RoundtripTime & "ms"
            Else
                OverInfoText = "T/O"
            End If
            Dim OverFont As Font = New Font("Tahoma", OverInfoFontSize, FontStyle.Regular)
            gfx.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            gfx.TextContrast = 0
            gfx.DrawString(OverInfoText, OverFont, Brushes.White, New PointF(MouseOverInfo.MouseLoc.X + 20, MouseOverInfo.MouseLoc.Y - 10)) '(intImgWidth / 2) - TextSize.Width / 2
        End If
        If Not bolScrolling Then
            Dim InfoText As String
            If pngResults.Last.Status = IPStatus.Success Then
                InfoText = pngResults.Last.RoundtripTime & "ms"
            Else
                InfoText = "T/O" '"Fail"
            End If
            Dim InfoFont As Font = New Font("Tahoma", InfoFontSize, FontStyle.Bold)
            Dim TextSize As SizeF = gfx.MeasureString(InfoText, InfoFont)
            gfx.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
            gfx.TextContrast = 0
            gfx.DrawString(InfoText, InfoFont, Brushes.White, New PointF((intImgWidth - 5) - (TextSize.Width), (intImgHeight) - (TextSize.Height))) '(intImgWidth / 2) - TextSize.Width / 2
        End If
    End Sub
    Private Sub DrawPingBars(ByRef gfx As Graphics, ByRef Bars As List(Of PingBar))
        For Each bar As PingBar In Bars
            gfx.FillRectangle(bar.Brush, bar.Rectangle)
            Dim CapPen As Pen = New Pen(Color.ForestGreen, 2)
            gfx.DrawLine(CapPen, New PointF(bar.Length, bar.PositionY), New PointF(bar.Length, bar.PositionY + bar.Rectangle.Height))
            CapPen.Dispose()
        Next
    End Sub
    Private Function GetPingBars() As List(Of PingBar)
        Dim NewPBars As New List(Of PingBar)
        Dim curPos As Single = BarTopPadding
        Dim BarHeight As Single = (intImgHeight - BarBottomPadding - BarTopPadding - (BarGap * intMaxBars)) / intMaxBars
        For Each result As PingReply In CurrentDisplayResults()
            Dim BarRatio As Single
            Dim BarLen As Single
            Dim MyBrush As Brush
            If result.Status <> Net.NetworkInformation.IPStatus.Success Then
                Dim brushBadPing = New SolidBrush(Color.Red)
                MyBrush = brushBadPing
                BarLen = intImgWidth - 2
            Else
                MyBrush = GetBarBrush(result.RoundtripTime)
                BarRatio = (Timeout / intInitialScale) / result.RoundtripTime
                BarLen = (intImgWidth / BarRatio) + intMinBarLen '* 2
            End If
            NewPBars.Add(New PingBar(BarLen, MyBrush, New Rectangle(1, CInt(curPos), CInt(BarLen), CInt(BarHeight)), curPos, result))
            curPos += BarHeight + BarGap
        Next
        Return NewPBars
    End Function
    Private Function LastDrawIndex(BarCount As Integer) As Integer
        If Not bolScrolling Then
            Return BarCount - 1
        Else
            Dim NewIdx As Integer = intTopIndex + intMaxBars
            If NewIdx <= BarCount - 1 Then
                Return NewIdx
            Else
                Return BarCount - 1
            End If
        End If
    End Function
    Private Function FirstDrawIndex(BarCount As Integer) As Integer
        If Not bolScrolling Then
            If BarCount <= intMaxBars Then
                intTopIndex = 0
                Return 0
            Else
                intTopIndex = BarCount - intMaxBars
                Return BarCount - intMaxBars
            End If
        Else
            Return intTopIndex
        End If
    End Function
    Private Function GetBarBrush(RoundTrip As Long) As Brush
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
        Dim iStep As Integer = CInt(255 / ((Timeout / 3) / RoundTrip)) 'Convert ping time to ratio of 255. 255 being the maximum levels of blending.
        If iStep > iSteps Then iStep = iSteps
        FadeColor = RGB(CInt(r1 + (r2 - r1) / iSteps * iStep), CInt(g1 + (g2 - g1) / iSteps * iStep), CInt(b1 + (b2 - b1) / iSteps * iStep))
        Return New SolidBrush(ColorTranslator.FromOle(FadeColor))
    End Function
    Private Sub DrawScaleLines(ByRef gfx As Graphics)
        Dim ScaleLineLoc As Integer = 0
        Dim NumOfLines As Integer = CInt(Timeout / 10)
        Dim StepSize As Integer = 0
        For a As Integer = 0 To NumOfLines
            StepSize = CInt((((Timeout / 4)) / NumOfLines) * intInitialScale)
            gfx.DrawLine(Pens.LightSlateGray, New Point(ScaleLineLoc, 0), New Point(ScaleLineLoc, intImgHeight))
            ScaleLineLoc += StepSize
        Next
    End Sub
    Private Sub TrimPingList()
        If pngResults.Count > intMaxStoredResults Then
            pngResults = pngResults.GetRange(pngResults.Count - intMaxStoredResults, intMaxStoredResults)
        End If
    End Sub
    Private Sub SetScale()
        Dim MaxPing As Long = CurrentDisplayResults.OrderByDescending(Function(p) p.RoundtripTime).FirstOrDefault.RoundtripTime
        If MaxPing <> intPrevMax Then
            If MaxPing * 2 >= (Timeout / intInitialScale) Then
                Do Until intInitialScale <= 0 Or MaxPing * 2 < (Timeout / intInitialScale)
                    intInitialScale -= 0.1F
                Loop
            ElseIf MaxPing < ((Timeout / intInitialScale) / 2) AndAlso intInitialScale >= 0 Then
                Do Until intInitialScale >= 15 Or MaxPing > ((Timeout / intInitialScale) / 2)
                    intInitialScale += 0.1F
                Loop
            End If
            intPrevMax = MaxPing
        End If
    End Sub
    Private Function CurrentDisplayResults() As List(Of PingReply)
        If pngResults.Count > intMaxBars Then
            Return pngResults.GetRange(FirstDrawIndex(pngResults.Count), intMaxBars)
        Else
            Return pngResults
        End If
    End Function
    Private Sub SetControlImage(ByRef DestControl As Control, ByRef Image As Bitmap)
        Select Case True
            Case TypeOf DestControl Is Form
                Dim frm As Form = DirectCast(DestControl, Form)
                If frm.BackgroundImage IsNot Nothing Then frm.BackgroundImage.Dispose()
                frm.BackgroundImage = Image
                frm.Invalidate()
            Case TypeOf DestControl Is Button
                Dim but As Button = DirectCast(DestControl, Button)
                If but.Image IsNot Nothing Then but.Image.Dispose()
                but.Image = Image
                but.Invalidate()
            Case TypeOf DestControl Is PictureBox
                Dim pic As PictureBox = DirectCast(DestControl, PictureBox)
                If pic.Image IsNot Nothing Then pic.Image.Dispose()
                pic.Image = Image
                pic.Refresh()
        End Select
    End Sub
    Private Sub SetAvgPing()
        Dim TotalPingTime As Long = 0
        For Each reply As PingReply In pngResults
            If reply.Status = IPStatus.Success Then
                TotalPingTime += reply.RoundtripTime
            Else
                TotalPingTime += Timeout
            End If
        Next
        CurrentAverageRoundTripTime = CInt(Math.Round((TotalPingTime / pngResults.Count), 0))
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
                PingTimer.Dispose()
                MyPing.SendAsyncCancel()
                MyPing.Dispose()
                ' MyControl.Dispose()
                pngResults.Clear()
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
    Private Class PingBar
        Private len As Single
        Private br As Brush
        Private rec As Rectangle
        Private pos As Single
        Private pInfo As PingReply
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
        Public ReadOnly Property PingResult As PingReply
            Get
                Return pInfo
            End Get
        End Property
        Sub New(Length As Single, ByRef Brush As Brush, Rect As Rectangle, PosY As Single, ByVal PingInfo As PingReply)
            len = Length
            br = Brush
            rec = Rect
            pos = PosY
            pInfo = PingInfo
        End Sub
    End Class
    Private Class MouseOverInfoStruct
        Public MouseLoc As Point
        Public PingReply As PingReply
        Sub New(mLoc As Point, pReply As PingReply)
            MouseLoc = mLoc
            PingReply = pReply
        End Sub
    End Class
End Class


