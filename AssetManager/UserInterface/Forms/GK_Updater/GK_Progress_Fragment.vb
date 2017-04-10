Imports System.ComponentModel
Public Class GK_Progress_Fragment
    Implements IDisposable

    Private WithEvents MyUpdater As GK_Updater
    Public ProgStatus As Progress_Status
    Private bolShow As Boolean = False
    Private CurrentStatus As GK_Updater.Status_Stats
    Private LogBuff As String = ""
    Sub New(ByRef Updater As GK_Updater, Optional Seq As Integer = 0)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Size = Me.MinimumSize
        MyUpdater = Updater
        lblInfo.Text = MyUpdater.CurDevice.strSerial & " - " & MyUpdater.CurDevice.strCurrentUser
        lblCurrentFile.Text = "Queued..."
        ProgStatus = Progress_Status.Queued
        If Seq > 0 Then
            lblSeq.Text = "#" & Seq
        Else
            lblSeq.Text = ""
        End If
        AddHandler MyUpdater.LogEvent, AddressOf GKLogEvent
        AddHandler MyUpdater.StatusUpdate, AddressOf GKStatusUpdateEvent
        AddHandler MyUpdater.UpdateComplete, AddressOf GKUpdate_Complete
        AddHandler MyUpdater.UpdateCancelled, AddressOf GKUpdate_Cancelled
        'MyUpdater.StartUpdate()

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
        If ProgStatus <> Progress_Status.Running Then
            LogBuff = ""
            ProgStatus = Progress_Status.Starting
            lblCurrentFile.Text = "Starting..."
            MyUpdater.StartUpdate()
        End If
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
        ProgStatus = Progress_Status.Running
        CurrentStatus = e.CurrentStatus
        pbarProgress.Maximum = CurrentStatus.TotFiles
        pbarProgress.Value = CurrentStatus.CurFileIdx
        Dim CurrentFile = CurrentStatus.CurFileName
        lblCurrentFile.Text = CurrentFile
        lblCurrentFile.Refresh()
        ' Invalidate()
    End Sub
    Private Sub GKUpdate_Cancelled(sender As Object, e As EventArgs)
        lblCurrentFile.Text = "Cancelled!"
        ProgStatus = Progress_Status.Cancelled
        ' MyUpdater.Dispose()
    End Sub

    Private Sub GKUpdate_Complete(sender As Object, e As GK_Updater.GKUpdateCompleteEvents)
        If e.HasErrors Then
            lblCurrentFile.Text = "ERROR!"
            ProgStatus = Progress_Status.Errors
            Dim err = DirectCast(e.Errors, Win32Exception)

            'Check for invalid credentials error and fire critical stop event.
            'We want to stop all updates if the credtials are wrong as to avoid locking the account.
            If err.NativeErrorCode = 1326 Or err.NativeErrorCode = 86 Then
                OnCriticalStopError(New EventArgs())
            End If
        Else
            lblCurrentFile.Text = "Complete! Errors: " & MyUpdater.ErrList.Count
            ProgStatus = Progress_Status.Complete
        End If
        ' MyUpdater.Dispose()
    End Sub
    Private Sub lblInfo_Click(sender As Object, e As EventArgs) Handles lblInfo.Click
        LookupDevice(MainForm, MyUpdater.CurDevice)
    End Sub

    Private Sub lblShowHide_Click(sender As Object, e As EventArgs) Handles lblShowHide.Click
        If Not bolShow Then
            Me.Size = Me.MaximumSize
            bolShow = True
            lblShowHide.Text = "-"
        Else
            Me.Size = Me.MinimumSize
            bolShow = False
            lblShowHide.Text = "+"
        End If
    End Sub

    Private Sub pbRestart_Click(sender As Object, e As EventArgs) Handles pbRestart.Click
        StartUpdate()
    End Sub

    Private Sub pbCancelClose_Click(sender As Object, e As EventArgs) Handles pbCancelClose.Click

        If ProgStatus = Progress_Status.Running Then
            If Not MyUpdater.IsDisposed Then
                MyUpdater.CancelUpdate()
            Else
                Me.Dispose()
            End If
        Else
            ' MyUpdater.Dispose()
            Me.Dispose()
        End If

    End Sub

    ''' <summary>
    ''' Timer that updates the rtbLog control with chunks of data from the log buffer.
    ''' </summary>
    Private Sub UI_Timer_Tick(sender As Object, e As EventArgs) Handles UI_Timer.Tick
        If LogBuff <> "" Then
            rtbLog.AppendText(LogBuff)
            rtbLog.Refresh()
            LogBuff = ""
        End If
    End Sub
End Class