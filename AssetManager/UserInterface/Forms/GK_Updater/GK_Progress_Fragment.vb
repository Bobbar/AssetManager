Public Class GK_Progress_Fragment
    Private WithEvents MyUpdater As GK_Updater
    Private CurrentStatus As GK_Updater.Status_Stats
    Private bolShow As Boolean = False
    Private LogBuff As String = ""
    Sub New(ByRef Updater As GK_Updater, Optional Seq As Integer = 0)

        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.
        Me.Size = Me.MinimumSize
        MyUpdater = Updater
        lblInfo.Text = MyUpdater.CurDevice.strSerial
        lblCurrentFile.Text = "Queued..."
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
    Public Sub StartUpdate()
        MyUpdater.StartUpdate()
    End Sub

    ''' <summary>
    ''' Log message event from GKUpdater.  This even can fire very rapidly. So the result is stored in a buffer to be added to the rtbLog control in a more controlled manner.
    ''' </summary>
    Private Sub GKLogEvent(sender As Object, e As GK_Updater.LogEvents)
        LogBuff += e.LogData.Message + vbCrLf
    End Sub
    Private Sub GKStatusUpdateEvent(sender As Object, e As GK_Updater.GKUpdateEvents)
        CurrentStatus = e.CurrentStatus
        pbarProgress.Maximum = CurrentStatus.TotFiles
        pbarProgress.Value = CurrentStatus.CurFileIdx
        Dim CurrentFile = CurrentStatus.CurFileName
        lblCurrentFile.Text = CurrentFile
        lblCurrentFile.Refresh()
        ' Invalidate()
    End Sub
    Private Sub GKUpdate_Complete(sender As Object, e As GK_Updater.GKUpdateCompleteEvents)
        If e.Errors Then
            ' Text = Text + " - *ERRORS!*"
            lblCurrentFile.Text = "ERROR"
        Else
            ' Text = Text + " - *COMPLETE*"
            lblCurrentFile.Text = "Complete!"
        End If
        MyUpdater.Dispose()
    End Sub
    Private Sub GKUpdate_Cancelled(sender As Object, e As EventArgs)
        lblCurrentFile.Text = "Cancelled!"
        MyUpdater.Dispose()
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

    Private Sub PictureBox1_Click(sender As Object, e As EventArgs) Handles PictureBox1.Click
        If Not MyUpdater.IsDisposed Then
            MyUpdater.CancelUpdate()
        Else
            Me.Dispose()
        End If

        'MyUpdater.Dispose()
        'Me.Dispose()
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

    Private Sub lblInfo_Click(sender As Object, e As EventArgs) Handles lblInfo.Click
        LookupDevice(MainForm, MyUpdater.CurDevice)
    End Sub
End Class
