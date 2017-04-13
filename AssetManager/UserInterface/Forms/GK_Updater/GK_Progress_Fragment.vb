Imports System.ComponentModel
Public Class GK_Progress_Fragment
    Implements IDisposable

    Public WithEvents MyUpdater As GK_Updater
    Public ProgStatus As Progress_Status
    Private bolShow As Boolean = False
    Private CurrentStatus As GK_Updater.Status_Stats
    Private LogBuff As String = ""
    Private MyParentForm As Form
    Sub New(ParentForm As Form, ByRef Updater As GK_Updater, Optional Seq As Integer = 0)

        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Me.Size = Me.MinimumSize
        MyUpdater = Updater
        MyParentForm = ParentForm
        Me.DoubleBuffered = True
        lblInfo.Text = MyUpdater.CurDevice.strSerial & " - " & MyUpdater.CurDevice.strCurrentUser
        lblCurrentFile.Text = "Queued..."
        lblTransRate.Text = "0.00MB/s"
        SetStatus(Progress_Status.Queued)
        If Seq > 0 Then
            lblSeq.Text = "#" & Seq
        Else
            lblSeq.Text = ""
        End If
        AddHandler MyUpdater.LogEvent, AddressOf GKLogEvent
        AddHandler MyUpdater.StatusUpdate, AddressOf GKStatusUpdateEvent
        AddHandler MyUpdater.UpdateComplete, AddressOf GKUpdate_Complete
        AddHandler MyUpdater.UpdateCancelled, AddressOf GKUpdate_Cancelled
        DoubleBufferedPanel(Panel1, True)
    End Sub

    Public Event CriticalStopError As EventHandler
    Public Enum Progress_Status
        Starting
        Stopped
        Complete
        Running
        Cancelled
        Queued
        Errors
    End Enum

    Public Sub StartUpdate()
        Try
            If ProgStatus <> Progress_Status.Running Then
                LogBuff = ""
                SetStatus(Progress_Status.Starting)
                lblCurrentFile.Text = "Starting..."
                MyUpdater.StartUpdate()
            End If
        Catch ex As Exception
            SetStatus(Progress_Status.Errors)
            If TypeOf ex Is Win32Exception Then
                Dim err = DirectCast(ex, Win32Exception)
                'Check for invalid credentials error and fire critical stop event.
                'We want to stop all updates if the credtials are wrong as to avoid locking the account.
                If err.NativeErrorCode = 1326 Or err.NativeErrorCode = 86 Then
                    OnCriticalStopError(New EventArgs())
                End If
            Else
                ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            End If
        End Try
    End Sub

    Protected Overridable Sub OnCriticalStopError(e As EventArgs)
        RaiseEvent CriticalStopError(Me, e)
    End Sub
    Private Sub GK_Progress_Fragment_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        MyUpdater.Dispose()
    End Sub

    ''' <summary>
    ''' Log message event from GKUpdater.  This even can fire very rapidly. So the result is stored in a buffer to be added to the rtbLog control in a more controlled manner.
    ''' </summary>
    Private Sub GKLogEvent(sender As Object, e As GK_Updater.LogEvents)
        LogBuff += e.LogData.Message + vbCrLf
    End Sub
    Private Sub GKStatusUpdateEvent(sender As Object, e As GK_Updater.GKUpdateEvents)
        SetStatus(Progress_Status.Running)
        CurrentStatus = e.CurrentStatus
        pbarProgress.Maximum = CurrentStatus.TotFiles
        pbarProgress.Value = CurrentStatus.CurFileIdx
        Dim CurrentFile = CurrentStatus.CurFileName
        lblCurrentFile.Text = CurrentFile
        lblCurrentFile.Refresh()
    End Sub
    Private Sub GKUpdate_Cancelled(sender As Object, e As EventArgs)
        lblCurrentFile.Text = "Cancelled!"
        SetStatus(Progress_Status.Cancelled)
    End Sub

    Private Sub GKUpdate_Complete(sender As Object, e As GK_Updater.GKUpdateCompleteEvents)
        If e.HasErrors Then
            lblCurrentFile.Text = "ERROR!"
            SetStatus(Progress_Status.Errors)
            If TypeOf e.Errors Is Win32Exception Then
                Dim err = DirectCast(e.Errors, Win32Exception)
                'Check for invalid credentials error and fire critical stop event.
                'We want to stop all updates if the credentials are wrong as to avoid locking the account.
                If err.NativeErrorCode = 1326 Or err.NativeErrorCode = 86 Then
                    OnCriticalStopError(New EventArgs())
                End If
            End If
        Else
            lblCurrentFile.Text = "Complete! Errors: " & MyUpdater.ErrList.Count
            SetStatus(Progress_Status.Complete)
        End If
    End Sub
    Private Sub lblInfo_Click(sender As Object, e As EventArgs) Handles lblInfo.Click
        LookupDevice(MainForm, MyUpdater.CurDevice)
    End Sub

    Private Sub lblShowHide_Click(sender As Object, e As EventArgs) Handles lblShowHide.Click
        If Not bolShow Then
            ShowLog()
        Else
            HideLog()
        End If
    End Sub
    Private Sub ShowLog()
        UpdateLogBox()
        Me.Size = Me.MaximumSize
        bolShow = True
        lblShowHide.Text = "r" '"-"
    End Sub
    Private Sub HideLog()
        Me.Size = Me.MinimumSize
        bolShow = False
        lblShowHide.Text = "s" '"+"
    End Sub
    Private Sub pbCancelClose_Click(sender As Object, e As EventArgs) Handles pbCancelClose.Click
        If ProgStatus = Progress_Status.Running Then
            If Not MyUpdater.IsDisposed Then
                MyUpdater.CancelUpdate()
            Else
                Me.Dispose()
            End If
        Else
            Me.Dispose()
        End If

    End Sub
    Public Sub CancelUpdate()
        If Not MyUpdater.IsDisposed Then MyUpdater.CancelUpdate()
    End Sub

    Private Sub pbRestart_Click(sender As Object, e As EventArgs) Handles pbRestart.Click
        If ProgStatus <> Progress_Status.Queued Then
            StartUpdate()
        Else
            Dim blah = Message("This update is queued. Starting it may exceed the maximum concurrent updates. Are you sure you want to start it?", vbYesNo + vbQuestion, "Warning", MyParentForm)
            If blah = MsgBoxResult.Yes Then
                StartUpdate()
            End If
        End If
    End Sub
    ''' <summary>
    ''' Timer that updates the rtbLog control with chunks of data from the log buffer.
    ''' </summary>
    Private Sub UI_Timer_Tick(sender As Object, e As EventArgs) Handles UI_Timer.Tick
        If bolShow And LogBuff <> "" Then
            UpdateLogBox()
        End If
        If ProgStatus = Progress_Status.Running Then
            'pbarFileProgress.Value = MyUpdater.CurrentStatus.CurFileProgress
            'If MyUpdater.CurrentStatus.CurFileProgress > 1 Then pbarFileProgress.Value = pbarFileProgress.Value - 1 'doing this bypasses the progressbar control animation. This way it doesn't lag behind and fills completely
            ' Debug.Print(MyUpdater.CurrentStatus.CurFileProgress)
            pbarFileProgress.Value = MyUpdater.CurrentStatus.CurFileProgress
            pbarFileProgress.Refresh()
            lblTransRate.Text = MyUpdater.CurrentStatus.CurTransferRate.ToString("0.00") & "MB/s"
            lblTransRate.Refresh()
        End If
    End Sub
    Private Sub UpdateLogBox()
        rtbLog.AppendText(LogBuff)
        rtbLog.Refresh()
        LogBuff = ""
    End Sub
    Private Sub SetStatus(Status As Progress_Status)
        ProgStatus = Status
        SetStatusLight(Status)
    End Sub
    Private Sub SetStatusLight(Status As Progress_Status)
        Select Case Status
            Case Progress_Status.Running, Progress_Status.Starting
                DrawLight(Color.LimeGreen)
            Case Progress_Status.Queued
                DrawLight(Color.Yellow)
            Case Else
                DrawLight(Color.Red)
        End Select
    End Sub
    Private Sub DrawLight(Color As Color)
        Dim MyBrush As New SolidBrush(Color)
        Dim StrokePen As New Pen(Color.Black, 1.5)
        Dim bm As New Bitmap(pbStatus.Width, pbStatus.Height)
        Dim gr As Graphics = Graphics.FromImage(bm)
        gr.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
        Dim XLoc, YLoc, Size As Single
        Size = 20
        XLoc = pbStatus.Width / 2 - Size / 2
        YLoc = pbStatus.Height / 2 - Size / 2
        gr.FillEllipse(MyBrush, XLoc, YLoc, Size, Size)
        gr.DrawEllipse(StrokePen, XLoc, YLoc, Size, Size)
        pbStatus.Image = bm
    End Sub

End Class