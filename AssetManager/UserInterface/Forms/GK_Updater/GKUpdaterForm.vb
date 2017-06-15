Imports System.ComponentModel
Imports System.Net.NetworkInformation
Public Class GKUpdaterForm
    Private bolRunQueue As Boolean = True
    Private MaxSimUpdates As Integer = 4
    Private MyUpdates As New List(Of GKProgressControl)
    Private bolStarting As Boolean = True
    Private bolCheckForDups As Boolean = True
    Private bolCreateMissingDirs As Boolean = False
    Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        bolStarting = False
        MaxUpdates.Value = MaxSimUpdates
        ' Add any initialization after the InitializeComponent() call.

        DoubleBufferedFlowLayout(Updater_Table, True)


    End Sub
    Public Sub AddUpdate(ByVal Device As Device_Info)
        If bolCheckForDups AndAlso Not Exists(Device) Then
            Dim NewProgCtl As New GKProgressControl(Me, Device, bolCreateMissingDirs, MyUpdates.Count + 1)
            Updater_Table.Controls.Add(NewProgCtl)
            MyUpdates.Add(NewProgCtl)
            AddHandler NewProgCtl.CriticalStopError, AddressOf CriticalStop
            ProcessUpdates()
        Else
            Dim blah = Message("An update for device " & Device.strSerial & " already exists.  Do you want to restart the update for this device?", vbOKCancel + vbExclamation, "Duplicate Update", Me)
            If blah = vbOK Then
                StartUpdateByDevice(Device)
            End If
        End If
    End Sub
    Private Sub StartUpdateByDevice(Device As Device_Info)
        For Each upd As GKProgressControl In MyUpdates
            If upd.Device.strGUID = Device.strGUID Then upd.StartUpdate()
        Next
    End Sub
    Private Function Exists(Device As Device_Info) As Boolean
        For Each upd As GKProgressControl In MyUpdates
            If upd.Device.strGUID = Device.strGUID Then Return True
        Next
        Return False
    End Function
    Private Function ActiveUpdates() As Boolean
        For Each upd As GKProgressControl In MyUpdates
            If upd.ProgStatus = GKProgressControl.Progress_Status.Running Then Return True
        Next
        Return False
    End Function

    Private Sub CancelAll()
        For Each upd As GKProgressControl In MyUpdates
            If upd.ProgStatus = GKProgressControl.Progress_Status.Running Then
                upd.CancelUpdate()
            End If
        Next
    End Sub
    Private Sub DisposeUpdates()
        For Each upd As GKProgressControl In MyUpdates
            If Not upd.IsDisposed Then
                upd.Dispose()
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
            For Each upd As GKProgressControl In MyUpdates
                If upd.ProgStatus = GKProgressControl.Progress_Status.Running Or upd.ProgStatus = GKProgressControl.Progress_Status.Starting Then
                    If Not upd.IsDisposed Then RunningUpdates += 1
                End If
            Next
            If RunningUpdates < MaxSimUpdates Then Return True
        End If
        Return False
    End Function

    Private Sub cmdCancelAll_Click(sender As Object, e As EventArgs) Handles cmdCancelAll.Click
        CancelAll()
        StopQueue()
    End Sub

    Private Sub cmdPauseResume_Click(sender As Object, e As EventArgs) Handles cmdPauseResume.Click
        If bolRunQueue Then
            StopQueue()
        Else
            StartQueue()
        End If
    End Sub
    Private Sub cmdSort_Click(sender As Object, e As EventArgs) Handles cmdSort.Click
        SortUpdates()
    End Sub

    Private Sub CriticalStop(sender As Object, e As EventArgs)
        StopQueue()
        Message("The queue was stopped because of an access error. Please re-enter your credentials.", vbExclamation + vbOKOnly, "Queue Stopped", Me)
        AdminCreds = Nothing
        If VerifyAdminCreds() Then
            bolRunQueue = True
        End If
    End Sub

    Private Sub GKUpdater_Form_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        If UpdatesRunning() Then
            e.Cancel = True
        Else
            DisposeUpdates()
            Me.Dispose()
        End If
    End Sub
    Public Function UpdatesRunning() As Boolean
        If ActiveUpdates() Then
            Me.Activate()
            Me.WindowState = FormWindowState.Normal
            Message("There are still updates running!  Cancel the updates or wait for them to finish.", vbOKOnly + vbExclamation, "Close Aborted", Me)
            Return True
        End If
        Return False
    End Function
    Private Sub MaxUpdates_ValueChanged(sender As Object, e As EventArgs) Handles MaxUpdates.ValueChanged
        If Not bolStarting Then MaxSimUpdates = CInt(MaxUpdates.Value)
    End Sub
    Private Sub ProcessUpdates()
        If CanRunMoreUpdates() Then StartNextUpdate()
        PruneQueue()
        SetStats()
    End Sub
    ''' <summary>
    ''' Removes disposed update fragments from list.
    ''' </summary>
    Private Sub PruneQueue()
        Dim tmpList As New List(Of GKProgressControl)
        For Each upd As GKProgressControl In MyUpdates
            If Not upd.IsDisposed Then
                tmpList.Add(upd)
            End If
        Next
        MyUpdates = tmpList
    End Sub

    Private Sub QueueChecker_Tick(sender As Object, e As EventArgs) Handles QueueChecker.Tick
        ProcessUpdates()
    End Sub

    Private Sub SetStats()
        Dim intQueued, intRunning, intComplete As Integer
        Dim TransferRateSum As Double
        For Each upd As GKProgressControl In MyUpdates
            Select Case upd.ProgStatus
                Case GKProgressControl.Progress_Status.Queued
                    intQueued += 1
                Case GKProgressControl.Progress_Status.Running
                    TransferRateSum += upd.MyUpdater.UpdateStatus.CurTransferRate
                    intRunning += 1
                Case GKProgressControl.Progress_Status.Complete
                    intComplete += 1
            End Select
        Next
        lblQueued.Text = "Queued: " & intQueued
        lblRunning.Text = "Running: " & intRunning
        lblComplete.Text = "Complete: " & intComplete
        lblTotUpdates.Text = "Tot Updates: " & MyUpdates.Count
        lblTransferRate.Text = "Transfer Rate: " & TransferRateSum.ToString("0.00") & " MB/s"
    End Sub

    Private Sub SortUpdates()

        Dim sortUpdates As New List(Of GKProgressControl)

        For Each upd As GKProgressControl In MyUpdates
            If upd.ProgStatus = GKProgressControl.Progress_Status.Running Then sortUpdates.Add(upd)
        Next

        For Each upd As GKProgressControl In MyUpdates
            If upd.ProgStatus = GKProgressControl.Progress_Status.Queued Then sortUpdates.Add(upd)
        Next
        For Each upd As GKProgressControl In MyUpdates
            Select Case upd.ProgStatus
                Case GKProgressControl.Progress_Status.Complete, GKProgressControl.Progress_Status.Errors, GKProgressControl.Progress_Status.Cancelled
                    sortUpdates.Add(upd)
            End Select
        Next
        Updater_Table.Controls.Clear()
        Updater_Table.Controls.AddRange(sortUpdates.ToArray)
    End Sub

    ''' <summary>
    ''' Starts the next update that has a queued status.
    ''' </summary>
    Private Sub StartNextUpdate()
        For Each upd As GKProgressControl In MyUpdates
            If upd.ProgStatus = GKProgressControl.Progress_Status.Queued Then
                upd.StartUpdate()
                Exit Sub
            End If
        Next
    End Sub

    Private Sub StartQueue()
        bolRunQueue = True
        cmdPauseResume.Text = "Pause Queue"
    End Sub

    Private Sub StopQueue()
        bolRunQueue = False
        cmdPauseResume.Text = "Resume Queue"
    End Sub
    Private Sub tsmCreateDirs_Click(sender As Object, e As EventArgs) Handles tsmCreateDirs.Click
        bolCreateMissingDirs = tsmCreateDirs.Checked
    End Sub
End Class