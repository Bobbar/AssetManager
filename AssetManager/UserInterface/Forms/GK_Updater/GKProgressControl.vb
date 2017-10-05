Imports System.ComponentModel
Imports GKUpdaterLib

Public Class GKProgressControl
    Implements IDisposable

    Public WithEvents MyUpdater As GK_Updater
    Public ProgStatus As ProgressStatus
    Private bolShow As Boolean = False
    Private CurrentStatus As GK_Updater.Status_Stats
    Private CurDevice As New DeviceObject
    Private LogBuff As String = ""
    Private MyParentForm As Form
    Private PrevColor As Color

    Public ReadOnly Property Device As DeviceObject
        Get
            Return CurDevice
        End Get
    End Property

    Sub New(parentForm As Form, device As DeviceObject, createMissingDirs As Boolean, gkPath As String, Optional seq As Integer = 0)
        InitializeComponent()
        ImageCaching.CacheControlImages(Me)
        Me.Size = Me.MinimumSize
        MyParentForm = parentForm
        CurDevice = device
        MyUpdater = New GK_Updater(CurDevice.HostName, gkPath)
        MyUpdater.CreateMissingDirectories = createMissingDirs
        Me.DoubleBuffered = True
        lblInfo.Text = CurDevice.Serial & " - " & CurDevice.CurrentUser
        lblTransRate.Text = "0.00MB/s"
        SetStatus(ProgressStatus.Queued)
        If seq > 0 Then
            lblSeq.Text = "#" & seq
        Else
            lblSeq.Text = ""
        End If
        AddHandler MyUpdater.LogEvent, AddressOf GKLogEvent
        AddHandler MyUpdater.StatusUpdate, AddressOf GKStatusUpdateEvent
        AddHandler MyUpdater.UpdateComplete, AddressOf GKUpdate_Complete
        AddHandler MyUpdater.UpdateCanceled, AddressOf GKUpdate_Cancelled
        DoubleBufferedPanel(Panel1, True)
    End Sub

    Public Event CriticalStopError As EventHandler

    Public Enum ProgressStatus
        Starting
        Running
        Paused
        Queued
        Complete
        CompleteWithErrors
        Canceled
        Errors
    End Enum

    Public Sub CancelUpdate()
        If Not MyUpdater.IsDisposed Then MyUpdater.CancelUpdate()
    End Sub

    Public Sub StartUpdate()
        Try
            If ProgStatus <> ProgressStatus.Running Then
                LogBuff = ""
                SetStatus(ProgressStatus.Starting)
                MyUpdater.StartUpdate(SecurityTools.AdminCreds)
            End If
        Catch ex As Exception
            SetStatus(ProgressStatus.Errors)
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Protected Overridable Sub OnCriticalStopError(e As EventArgs)
        RaiseEvent CriticalStopError(Me, e)
    End Sub

    Private Sub DrawLight(Color As Color)
        If Color <> PrevColor Then
            PrevColor = Color
            Dim bm As New Bitmap(pbStatus.Width, pbStatus.Height)
            Using MyBrush As New SolidBrush(Color),
                StrokePen As New Pen(Color.Black, 1.5),
                           gr As Graphics = Graphics.FromImage(bm)
                gr.SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                Dim XLoc, YLoc, Size As Single
                Size = 20
                XLoc = Convert.ToSingle(pbStatus.Width / 2 - Size / 2)
                YLoc = Convert.ToSingle(pbStatus.Height / 2 - Size / 2)
                gr.FillEllipse(MyBrush, XLoc, YLoc, Size, Size)
                gr.DrawEllipse(StrokePen, XLoc, YLoc, Size, Size)
                pbStatus.Image = bm
            End Using
        End If
    End Sub

    Private Sub GK_Progress_Fragment_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        RemoveHandler MyUpdater.LogEvent, AddressOf GKLogEvent
        RemoveHandler MyUpdater.StatusUpdate, AddressOf GKStatusUpdateEvent
        RemoveHandler MyUpdater.UpdateComplete, AddressOf GKUpdate_Complete
        RemoveHandler MyUpdater.UpdateCanceled, AddressOf GKUpdate_Cancelled
        MyUpdater.Dispose()

    End Sub

    ''' <summary>
    ''' Log message event from GKUpdater.  This even can fire very rapidly. So the result is stored in a buffer to be added to the rtbLog control in a more controlled manner.
    ''' </summary>
    Private Sub GKLogEvent(sender As Object, e As EventArgs)
        Dim LogEvent = DirectCast(e, GK_Updater.LogEvents)
        Log(LogEvent.LogData.Message)
    End Sub

    Private Sub Log(Message As String)
        LogBuff += Message + vbCrLf
    End Sub

    Private Sub GKStatusUpdateEvent(sender As Object, e As EventArgs)
        Dim UpdateEvent = DirectCast(e, GK_Updater.GKUpdateEvents)
        SetStatus(ProgressStatus.Running)
        CurrentStatus = UpdateEvent.CurrentStatus
        pbarProgress.Maximum = CurrentStatus.TotFiles
        pbarProgress.Value = CurrentStatus.CurFileIdx
        lblStatus.Text = CurrentStatus.CurFileName
    End Sub

    Private Sub GKUpdate_Cancelled(sender As Object, e As EventArgs)
        SetStatus(ProgressStatus.Canceled)
    End Sub

    Private Sub GKUpdate_Complete(sender As Object, e As EventArgs)
        Dim CompleteEvent = DirectCast(e, GK_Updater.GKUpdateCompleteEvents)
        If CompleteEvent.HasErrors Then
            SetStatus(ProgressStatus.Errors)
            If TypeOf CompleteEvent.Errors Is Win32Exception Then
                Dim err = DirectCast(CompleteEvent.Errors, Win32Exception)
                'Check for invalid credentials error and fire critical stop event.
                'We want to stop all updates if the credentials are wrong as to avoid locking the account.
                If err.NativeErrorCode = 1326 Or err.NativeErrorCode = 86 Then
                    OnCriticalStopError(New EventArgs())
                End If
            Else
                Select Case True
                    Case TypeOf CompleteEvent.Errors Is GK_Updater.MissingDirectoryException
                        Log("Enable 'Create Missing Directories' option and re-enqueue this device to force creation.")
                End Select
            End If
        Else
            If MyUpdater.ErrorList.Count = 0 Then
                SetStatus(ProgressStatus.Complete)
            Else
                SetStatus(ProgressStatus.CompleteWithErrors)
            End If
        End If
    End Sub

    Private Sub HideLog()
        Me.Size = Me.MinimumSize
        bolShow = False
        lblShowHide.Text = "s" '"+"
    End Sub

    Private Sub ShowLog()
        UpdateLogBox()
        Me.Size = Me.MaximumSize
        bolShow = True
        lblShowHide.Text = "r" '"-"
    End Sub

    Private Sub lblInfo_Click(sender As Object, e As EventArgs) Handles lblInfo.Click
        LookupDevice(MainForm, CurDevice)
    End Sub

    Private Sub lblShowHide_Click(sender As Object, e As EventArgs) Handles lblShowHide.Click
        If Not bolShow Then
            ShowLog()
        Else
            HideLog()
        End If
    End Sub

    Private Sub pbCancelClose_Click(sender As Object, e As EventArgs) Handles pbCancelClose.Click
        If ProgStatus = ProgressStatus.Running Or ProgStatus = ProgressStatus.Paused Then
            If Not MyUpdater.IsDisposed Then
                MyUpdater.CancelUpdate()
                SetStatus(ProgressStatus.Canceled)
            Else
                Me.Dispose()
            End If
        Else
            Me.Dispose()
        End If
    End Sub

    Private Sub pbRestart_Click(sender As Object, e As EventArgs) Handles pbRestart.Click
        Select Case ProgStatus
            Case ProgressStatus.Paused
                MyUpdater.ResumeUpdate()
                SetStatus(ProgressStatus.Running)
            Case ProgressStatus.Running
                MyUpdater.PauseUpdate()
                SetStatus(ProgressStatus.Paused)
            Case ProgressStatus.Queued
                Dim blah = Message("This update is queued. Starting it may exceed the maximum concurrent updates. Are you sure you want to start it?", vbYesNo + vbQuestion, "Warning", MyParentForm)
                If blah = MsgBoxResult.Yes Then
                    StartUpdate()
                End If
            Case Else
                StartUpdate()
        End Select
    End Sub

    Private Sub SetStatus(Status As ProgressStatus)
        If ProgStatus <> Status Then
            ProgStatus = Status
            SetStatusLight(Status)
            SetButtons(Status)
            SetStatusLabel(Status)
        End If
    End Sub

    Private Sub SetStatusLight(Status As ProgressStatus)
        Select Case Status
            Case ProgressStatus.Running, ProgressStatus.Starting
                DrawLight(Color.LimeGreen)
            Case ProgressStatus.Queued, ProgressStatus.Paused
                DrawLight(Color.Yellow)
            Case Else
                DrawLight(Color.Red)
        End Select
    End Sub

    Private Sub SetButtons(Status As ProgressStatus)
        Select Case Status
            Case ProgressStatus.Running
                pbRestart.Image = ImageCaching.ImageCache("PauseIcon", My.Resources.PauseIcon)
                MyToolTip.SetToolTip(pbRestart, "Pause")
            Case ProgressStatus.Paused, ProgressStatus.Queued
                pbRestart.Image = ImageCaching.ImageCache("PlayIcon", My.Resources.PlayIcon)
                MyToolTip.SetToolTip(pbRestart, "Resume")
            Case Else
                pbRestart.Image = ImageCaching.ImageCache("RestartIcon", My.Resources.RestartIcon)
                MyToolTip.SetToolTip(pbRestart, "Restart")
        End Select
    End Sub

    Private Sub SetStatusLabel(Status As ProgressStatus)
        Select Case Status
            Case ProgressStatus.Queued
                lblStatus.Text = "Queued..."
            Case ProgressStatus.Canceled
                lblStatus.Text = "Cancelled!"
            Case ProgressStatus.Errors
                lblStatus.Text = "ERROR!"
            Case ProgressStatus.CompleteWithErrors
                lblStatus.Text = "Completed with errors: " & MyUpdater.ErrorList.Count
            Case ProgressStatus.Complete
                lblStatus.Text = "Complete!"
            Case ProgressStatus.Starting
                lblStatus.Text = "Starting..."
            Case ProgressStatus.Paused
                lblStatus.Text = "Paused."
        End Select
    End Sub

    ''' <summary>
    ''' Timer that updates the rtbLog control with chunks of data from the log buffer.
    ''' </summary>
    Private Sub UI_Timer_Tick(sender As Object, e As EventArgs) Handles UI_Timer.Tick
        If bolShow And LogBuff <> "" Then
            UpdateLogBox()
        End If
        If ProgStatus = ProgressStatus.Running Then
            pbarFileProgress.Value = MyUpdater.UpdateStatus.CurFileProgress
            If pbarFileProgress.Value > 1 Then pbarFileProgress.Value = pbarFileProgress.Value - 1 'doing this bypasses the progressbar control animation. This way it doesn't lag behind and fills completely
            pbarFileProgress.Value = MyUpdater.UpdateStatus.CurFileProgress
            lblTransRate.Text = MyUpdater.UpdateStatus.CurTransferRate.ToString("0.00") & "MB/s"
            Me.Update()
        End If
    End Sub

    Private Sub UpdateLogBox()
        rtbLog.AppendText(LogBuff)
        rtbLog.Refresh()
        LogBuff = ""
    End Sub

End Class