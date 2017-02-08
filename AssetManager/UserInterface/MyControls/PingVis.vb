Option Explicit On
Imports System.Net.NetworkInformation
Imports System.Net
Imports System.ComponentModel
Public Class PingVis : Implements IDisposable
    Private MyPing As New Ping
    Private pngResults As New List(Of PingReply)
    Private MyPingHostname As String
    Private bolPingRunning As Boolean = False
    Private WithEvents PingTimer As New Timer
    Private MyControl As Control
    Private Timeout As Integer = 1000
    Private intBarGap As Integer = 0
    Private intBarHeight As Integer = 4
    Private intMinBarLen As Integer = 2
    Private curPos As Integer = 1
    Private intInitialScale As Single = 4
    Private intPrevMax As Integer = 0
    Public AvgPingTime As Single = 0
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
    End Sub
    Private Sub PingTimer_Tick(sender As Object, e As EventArgs)
        Dim options As New Net.NetworkInformation.PingOptions
        options.DontFragment = True
        If Not bolPingRunning Then
            '   If Form.ActiveForm Is MyControl.FindForm Then
            MyPing.SendAsync(MyPingHostname, Timeout, options)
                bolPingRunning = True
            '  End If
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
        Dim bm As Drawing.Bitmap = New Drawing.Bitmap(DestControl.Width, DestControl.Height)
        Dim gfx As Graphics = Graphics.FromImage(bm)
        ' gfx.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Try
            Dim intImgHeight As Integer = DestControl.ClientSize.Height
            Dim intImgWidth As Integer = DestControl.ClientSize.Width
            Dim intMaxBars As Integer = CInt((intImgHeight - 1) / (intBarHeight + intBarGap))
            Dim brushGreen As Brush = New SolidBrush(Color.FromArgb(15, 130, 21)) '94, 178, 98)) ' Brushes.Green
            Dim MyBrush As Brush
            Dim LB, UB As Integer
            If pngResults.Count > intMaxBars Then
                pngResults = pngResults.GetRange(pngResults.Count - intMaxBars, intMaxBars)
            End If
            LB = 0
            UB = pngResults.Count - 1
            gfx.Clear(DestControl.BackColor)
            For i As Integer = LB To UB
                Dim BarRatio As Single
                Dim BarLen As Single
                If pngResults(i).Status <> Net.NetworkInformation.IPStatus.Success Then
                    MyBrush = Brushes.Red
                    BarLen = intImgWidth - 2
                Else
                    MyBrush = brushGreen
                    BarRatio = (Timeout / intInitialScale) / pngResults(i).RoundtripTime
                    BarLen = (intImgWidth / BarRatio) + intMinBarLen '* 2
                End If

                gfx.FillRectangle(MyBrush, 1, curPos, BarLen, intBarHeight)
                '  gfx.DrawLine(Pens.Gray, New Point(1, curPos), New Point(BarLen, curPos))
                curPos += intBarHeight + intBarGap
            Next
            Dim InfoText As String = AvgPingTime & "ms" '"P" & vbCrLf & "i" & vbCrLf & "n" & vbCrLf & "g" '"Ping" & vbCrLf & "Vis"
            Dim InfoFont As Font = New Font("Consolas", 7, FontStyle.Regular)
            Dim TextSize As SizeF = gfx.MeasureString(InfoText, InfoFont)
            gfx.DrawString(InfoText, InfoFont, Brushes.White, New Point((intImgWidth / 2) - TextSize.Width / 2 - 4, (intImgHeight) - (TextSize.Height + 3))) '(intImgWidth / 2) - TextSize.Width / 2
            curPos = 1
            SetAvgPing()
            SetScale()
            SetControlImage(DestControl, bm)
        Catch
        Finally
            ' GC.Collect()
        End Try
    End Sub
    Private Sub SetScale()
        Dim MaxPing As Integer = pngResults.OrderByDescending(Function(p) p.RoundtripTime).FirstOrDefault.RoundtripTime
        Debug.Print(MaxPing)
        If MaxPing <> intPrevMax Then
            If MaxPing * 2 >= (Timeout / intInitialScale) Then
                'intInitialScale -= 1
                Do Until intInitialScale <= 0 Or MaxPing * 2 < (Timeout / intInitialScale)
                    intInitialScale -= 0.1
                Loop

            ElseIf MaxPing < ((Timeout / intInitialScale) / 2) AndAlso intInitialScale >= 0 Then
                '  intInitialScale += 1
                Do Until intInitialScale >= 15 Or MaxPing > ((Timeout / intInitialScale) / 2)
                    intInitialScale += 0.1
                Loop




            End If
            intPrevMax = MaxPing

        End If


        Debug.Print(intInitialScale)
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
        AvgPingTime = Math.Round((TotalPingTime / pngResults.Count), 0)
    End Sub
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

