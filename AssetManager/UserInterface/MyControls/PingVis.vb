Option Explicit On
Imports System.Net.NetworkInformation
Imports System.Net
Imports System.ComponentModel
Imports System.Drawing.Drawing2D
Imports System.Drawing.Imaging
Imports System.Drawing
Public Class PingVis : Implements IDisposable
    Private MyPing As New Ping
    Private pngResults As New List(Of PingReply)
    Private MyPingHostname As String
    Private bolPingRunning As Boolean = False
    Private WithEvents PingTimer As New Timer
    Private MyControl As Control
    Private bm As Drawing.Bitmap
    Private gfx As Graphics
    Private intImgWidth As Integer
    Private intImgHeight As Integer
#Region "Ping Parameters"
    Private bolStopWhenNotFocused As Boolean = False 'Set to True to pause the pinging until focus is returned to the parent form.
    Private Timeout As Integer = 1000
#End Region

#Region "Bar Parameters"
    Private Const intBarGap As Integer = 0
    Private Const intMaxBars As Integer = 10
    Private Const intMinBarLen As Integer = 2
#Region "Colors"
    Private brushGoodPing As Brush = New SolidBrush(Color.FromArgb(15, 130, 21))
    Private brushOKPing As Brush = New SolidBrush(Color.FromArgb(175, 131, 0))
    Private brushBadPing As Brush = Brushes.DimGray
#End Region

#End Region
    Private intInitialScale As Single = 4
    Private intPrevMax As Integer = 1000
    Private CurrentAverageRoundTripTime As Integer = 0
    Public ReadOnly Property AvgPingTime As Single
        Get
            Return CurrentAverageRoundTripTime
        End Get
    End Property
    Sub New(ByRef DestControl As Control, HostName As String)
        MyControl = DestControl
        MyPingHostname = HostName
        Init()
    End Sub
    Private Sub Init()
        AddHandler MyPing.PingCompleted, AddressOf PingComplete
        PingTimer.Interval = 1000
        PingTimer.Enabled = True
        AddHandler PingTimer.Tick, AddressOf PingTimer_Tick
        StartPing()
    End Sub
    Private Sub PingTimer_Tick(sender As Object, e As EventArgs)
        Try
            StartPing()
        Catch ex As Exception
            Debug.Print(ex.Message)
        End Try
    End Sub
    Private Sub StartPing()
        Dim options As New Net.NetworkInformation.PingOptions
        options.DontFragment = True
        If Not bolPingRunning Then
            If Not bolStopWhenNotFocused Then
                MyPing.SendAsync(MyPingHostname, Timeout, options)
                bolPingRunning = True
            Else
                If Form.ActiveForm Is MyControl.FindForm Then
                    MyPing.SendAsync(MyPingHostname, Timeout, options)
                    bolPingRunning = True
                End If
            End If
        End If
    End Sub
    Private Sub PingComplete(ByVal sender As Object, ByVal e As System.Net.NetworkInformation.PingCompletedEventArgs)
        bolPingRunning = False
        If e.Error Is Nothing Then
            Dim MyPingResults As Net.NetworkInformation.PingReply = e.Reply
            If MyPingResults.Status = Net.NetworkInformation.IPStatus.Success Then
                pngResults.Add(e.Reply)
            Else
                pngResults.Add(e.Reply)
            End If
        Else
            Debug.Print(e.Reply.ToString)
        End If
        DrawBars(MyControl)
    End Sub
    Private Sub DrawBars(ByRef DestControl As Control)
        'Set image to double the size of the control
        intImgWidth = DestControl.ClientSize.Width * 2
        intImgHeight = DestControl.ClientSize.Height * 2
        bm = New Drawing.Bitmap(intImgWidth, intImgHeight)
        gfx = Graphics.FromImage(bm)
        gfx.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Try
            TrimPingList() 'Trim list to intMaxBars
            gfx.Clear(DestControl.BackColor)
            DrawScaleLines() 'Draw scale lines
            DrawPingBars() 'Draw ping bars
            DrawPingText() 'Draw last ping round trip time
            SetAvgPing() 'Set public property containing current average round trip time
            SetScale()
            'The bitmap is scaled back to control size using high quality transformations.
            'This improves the effect of anti-aliasing and therefore readability.
            '  bm.Save("C:\Temp\test.bmp")
            bm = ResizeImage(bm, DestControl.ClientSize.Width, DestControl.ClientSize.Height)
            SetControlImage(DestControl, bm)
        Catch
        Finally
        End Try
    End Sub
    Private Sub DrawPingText()
        Dim InfoFontSize As Single = 20
        Dim InfoText As String
        If pngResults.Last.Status = IPStatus.Success Then
            InfoText = pngResults.Last.RoundtripTime & "ms"
        Else
            InfoText = "Fail"
        End If
        Dim InfoFont As Font = New Font("Tahoma", InfoFontSize, FontStyle.Bold)
        Dim TextSize As SizeF = gfx.MeasureString(InfoText, InfoFont)
        gfx.TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
        gfx.TextContrast = 0
        gfx.DrawString(InfoText, InfoFont, Brushes.White, New PointF((intImgWidth / 2) - (TextSize.Width / 2) - 2, (intImgHeight / 2) - (TextSize.Height / 2))) '(intImgWidth / 2) - TextSize.Width / 2
    End Sub
    Private Sub DrawPingBars()
        Dim MyBrush As Brush
        Dim curPos As Single = -1 '0
        Dim BarHeight As Single = intImgHeight / intMaxBars
        For i As Integer = 0 To pngResults.Count - 1
            Dim BarRatio As Single
            Dim BarLen As Single
            If pngResults(i).Status <> Net.NetworkInformation.IPStatus.Success Then
                MyBrush = brushBadPing
                BarLen = intImgWidth - 2
            Else
                MyBrush = GetBarBrush(pngResults(i).RoundtripTime)
                BarRatio = (Timeout / intInitialScale) / pngResults(i).RoundtripTime
                BarLen = (intImgWidth / BarRatio) + intMinBarLen '* 2
            End If
            gfx.FillRectangle(MyBrush, 1, curPos, BarLen, BarHeight)
            Dim CapPen As Pen = New Pen(Color.ForestGreen, 2)
            gfx.DrawLine(CapPen, New PointF(BarLen, curPos), New PointF(BarLen, curPos + BarHeight))
            curPos += BarHeight + intBarGap
        Next
    End Sub
    Private Function GetBarBrush(RoundTrip As Integer) As Brush
        'Alpha blending two colors. As ping times go up, the returned color becomes more red.
        Dim FadeColor As Long
        Dim Color1, Color2
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
        FadeColor = RGB(r1 + (r2 - r1) / iSteps * iStep, g1 + (g2 - g1) / iSteps * iStep, b1 + (b2 - b1) / iSteps * iStep)
        Return New SolidBrush(ColorTranslator.FromOle(FadeColor))
    End Function
    Private Sub DrawScaleLines()
        Dim ScaleLineLoc As Integer = 0
        Dim NumOfLines As Integer = Timeout / 10
        Dim StepSize As Integer = 0
        For a As Integer = 0 To NumOfLines
            StepSize = (((Timeout / 4)) / NumOfLines) * intInitialScale
            gfx.DrawLine(Pens.LightSlateGray, New Point(ScaleLineLoc, 0), New Point(ScaleLineLoc, intImgHeight))
            ScaleLineLoc += StepSize
        Next
    End Sub
    Private Sub TrimPingList()
        If pngResults.Count > intMaxBars Then
            pngResults = pngResults.GetRange(pngResults.Count - intMaxBars, intMaxBars)
        End If
    End Sub
    Private Sub SetScale()
        Dim MaxPing As Integer = pngResults.OrderByDescending(Function(p) p.RoundtripTime).FirstOrDefault.RoundtripTime
        If MaxPing <> intPrevMax Then
            If MaxPing * 2 >= (Timeout / intInitialScale) Then
                Do Until intInitialScale <= 0 Or MaxPing * 2 < (Timeout / intInitialScale)
                    intInitialScale -= 0.1
                Loop
            ElseIf MaxPing < ((Timeout / intInitialScale) / 2) AndAlso intInitialScale >= 0 Then
                Do Until intInitialScale >= 15 Or MaxPing > ((Timeout / intInitialScale) / 2)
                    intInitialScale += 0.1
                Loop
            End If
            intPrevMax = MaxPing
        End If
    End Sub
    Private Sub SetControlImage(ByRef DestControl As Control, Image As Bitmap)
        Select Case True
            Case TypeOf DestControl Is Form
                Dim frm As Form = DestControl
                frm.BackgroundImage = Image
                frm.Invalidate()
            Case TypeOf DestControl Is Button
                Dim but As Button = DestControl
                but.Image = Image
                but.Invalidate()
            Case TypeOf DestControl Is PictureBox
                Dim pic As PictureBox = DestControl
                pic.Image = Image
                pic.Refresh()
        End Select
    End Sub
    Private Sub SetAvgPing()
        Dim TotalPingTime As Integer = 0
        For Each reply As PingReply In pngResults
                If reply.Status = IPStatus.Success Then
                    TotalPingTime += reply.RoundtripTime
                Else
                    TotalPingTime += Timeout
                End If
            Next
            CurrentAverageRoundTripTime = Math.Round((TotalPingTime / pngResults.Count), 0)
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
End Class

