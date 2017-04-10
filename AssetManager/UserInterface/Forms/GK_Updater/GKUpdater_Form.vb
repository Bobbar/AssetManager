Public Class GKUpdater_Form
    Private MyUpdates As New List(Of GK_Progress_Fragment)
    Private MaxSimUpdates As Integer = 1
    Private bolRunQueue As Boolean = True
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        MaxUpdates.Value = MaxSimUpdates
        ' Add any initialization after the InitializeComponent() call.
        Show()

    End Sub
    Private Sub CriticalStop()
        bolRunQueue = False
        Message("The queue was stopped because of an access error. Please re-enter your credentials.", vbExclamation + vbOKOnly, "Queue Stopped", Me)
        AdminCreds = Nothing
        Dim NewGetCreds As New Get_Credentials
        NewGetCreds.ShowDialog()
        If NewGetCreds.DialogResult <> DialogResult.OK Then
            If AdminCreds IsNot Nothing Then
                NewGetCreds.Dispose()
                Exit Sub
                bolRunQueue = True
            End If
        End If
        NewGetCreds.Dispose()
    End Sub
    Public Sub AddUpdate(ByRef Updater As GK_Updater)

        Updater_Table.RowStyles.Clear()
        Dim NewProgCtl As New GK_Progress_Fragment(Updater, MyUpdates.Count + 1)
        Updater_Table.Controls.Add(NewProgCtl)
        MyUpdates.Add(NewProgCtl)
        AddHandler NewProgCtl.CriticalStopError, AddressOf CriticalStop

        ProcessUpdates()
    End Sub
    Private Sub ProcessUpdates()

        If CanRunMoreUpdates() Then StartNextUpdate()
        PruneQueue()
        SetStats()

    End Sub
    Private Sub SetStats()
        Dim intQueued, intRunning, intComplete As Integer
        For Each upd As GK_Progress_Fragment In MyUpdates
            Select Case upd.ProgStatus
                Case GK_Progress_Fragment.Progress_Status.Queued
                    intQueued += 1
                Case GK_Progress_Fragment.Progress_Status.Running
                    intRunning += 1
                Case GK_Progress_Fragment.Progress_Status.Complete
                    intComplete += 1
            End Select


        Next

        lblQueued.Text = "Queued: " & intQueued
        lblRunning.Text = "Running: " & intRunning
        lblComplete.Text = "Complete: " & intComplete
        lblTotUpdates.Text = "Tot Updates: " & MyUpdates.Count



    End Sub
    ''' <summary>
    ''' Removes disposed update fragments from list.
    ''' </summary>
    Private Sub PruneQueue()
        Dim tmpList As New List(Of GK_Progress_Fragment)
        For Each upd As GK_Progress_Fragment In MyUpdates
            If Not upd.IsDisposed Then
                tmpList.Add(upd)
            End If
        Next
        MyUpdates = tmpList
    End Sub

    ''' <summary>
    ''' Starts the next update that has a queued status.
    ''' </summary>
    Private Sub StartNextUpdate()
        For Each upd As GK_Progress_Fragment In MyUpdates
            If upd.ProgStatus = GK_Progress_Fragment.Progress_Status.Queued Then
                upd.StartUpdate()
                Exit Sub
            End If
        Next
    End Sub
    ''' <summary>
    ''' Returns True if number of running updates is less than the maximum simultaneous allowed updates and RunQueue is True.
    ''' </summary>
    ''' <returns></returns>
    Private Function CanRunMoreUpdates() As Boolean
        If bolRunQueue Then
            Dim RunningUpdates As Integer = 0
            For Each upd As GK_Progress_Fragment In MyUpdates
                If upd.ProgStatus = GK_Progress_Fragment.Progress_Status.Running Or upd.ProgStatus = GK_Progress_Fragment.Progress_Status.Starting Then
                    If Not upd.IsDisposed Then
                        RunningUpdates += 1
                    Else
                        MyUpdates.Remove(upd)
                    End If
                End If
            Next
            If RunningUpdates < MaxSimUpdates Then Return True
        End If
        Return False
    End Function

    Private Sub QueueChecker_Tick(sender As Object, e As EventArgs) Handles QueueChecker.Tick
        ProcessUpdates()
    End Sub

    Private Sub cmdPauseResume_Click(sender As Object, e As EventArgs) Handles cmdPauseResume.Click
        If bolRunQueue Then
            bolRunQueue = False
            cmdPauseResume.Text = "Resume Queue"
        Else
            bolRunQueue = True
            cmdPauseResume.Text = "Pause Queue"
        End If
    End Sub

    Private Sub MaxUpdates_ValueChanged(sender As Object, e As EventArgs) Handles MaxUpdates.ValueChanged
        MaxSimUpdates = MaxUpdates.Value
    End Sub
End Class