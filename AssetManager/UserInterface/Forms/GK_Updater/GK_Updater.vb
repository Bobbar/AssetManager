Imports System.Net
Imports System.Net.NetworkInformation
Imports System.IO
Imports System.ComponentModel
Imports System.Text

Public Class GK_Updater : Implements IDisposable
    Private WithEvents CopyWorker As BackgroundWorker
    Private WithEvents SpeedTimer As Timer
    Public CurrentStatus As Status_Stats
    Public ErrList As New List(Of String)
    Private ReadOnly GKPath As String = "\PSi\Gatekeeper"
    Private ClientPath As String
    Private CurrentFileIndex As Integer = 0
    Private lngBytesMoved As Integer
    Private progIts As Integer = 0
    Private ServerPath As String = "C:"
    Private stpSpeed As New Stopwatch
    Private UpdateDevice As Device_Info
    Sub New(ByVal Device As Device_Info)
        UpdateDevice = Device
        ClientPath = "\\D" & UpdateDevice.strSerial & "\c$"
        InitWorker()
        InitializeTimer()
    End Sub
    Public Event LogEvent As EventHandler
    Public Event StatusUpdate As EventHandler
    Public Event UpdateCancelled As EventHandler
    Public Event UpdateComplete As EventHandler
    Public ReadOnly Property CurDevice As Device_Info
        Get
            Return UpdateDevice
        End Get
    End Property
    Public ReadOnly Property IsDisposed As Boolean
        Get
            Return disposedValue
        End Get
    End Property
    Public Sub CancelUpdate()
        CopyWorker.CancelAsync()
    End Sub
    Public Sub StartUpdate()
        If AdminCreds Is Nothing Then
            Throw New Win32Exception(1326)
            Exit Sub
        End If
        GKLog("------------------------------------------------")
        GKLog("Starting GK Update to: " & UpdateDevice.strSerial)
        GKLog("Starting Update...")
        ErrList.Clear()
        Dim WorkArgs As New Worker_Args
        WorkArgs.StartIndex = 0
        If Not CopyWorker.IsBusy Then CopyWorker.RunWorkerAsync(WorkArgs)
    End Sub
    Protected Overridable Sub OnLogEvent(e As LogEvents)
        RaiseEvent LogEvent(Me, e)
    End Sub
    Protected Overridable Sub OnStatusUpdate(e As GKUpdateEvents)
        RaiseEvent StatusUpdate(Me, e)
    End Sub
    Protected Overridable Sub OnUpdateCancelled(e As EventArgs)
        RaiseEvent UpdateCancelled(Me, e)
    End Sub
    Protected Overridable Sub OnUpdateComplete(e As GKUpdateCompleteEvents)
        RaiseEvent UpdateComplete(Me, e)
    End Sub
    ''' <summary>
    ''' Pings the current device. Success returns True. All failures return False.
    ''' </summary>
    ''' <returns></returns>
    Private Function CanPing() As Boolean
        Try
            Dim MyPing As New Ping
            Dim options As New Net.NetworkInformation.PingOptions
            Dim Hostname As String = "D" & CurDevice.strSerial
            Dim Timeout As Integer = 1000
            Dim buff As Byte() = Encoding.ASCII.GetBytes("pingpingpingpingping")
            options.DontFragment = True
            Dim reply As PingReply = MyPing.Send(Hostname, Timeout, buff, options)
            If reply.Status = IPStatus.Success Then
                Return True
            End If
            Return False
        Catch ex As Exception
            Return False
        End Try
    End Function
    Private Sub CopyWorker_DoWork(sender As Object, e As DoWorkEventArgs)
        If Not CanPing() Then
            Throw New Exception("Cannot ping device.")
        End If
        Using NetCon As New NetworkConnection(ClientPath, AdminCreds)
            Dim sourceDir As String = ServerPath & GKPath
            Dim targetDir As String = ClientPath & GKPath
            Dim Args As Worker_Args = DirectCast(e.Argument, Worker_Args)
            Dim StartIdx As Integer = Args.StartIndex
            Dim CurFileIdx As Integer = Args.StartIndex
            'Get array of full paths of all files in source dir and sub-dirs
            Dim files() = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories)
            'Loop through file array
            lngBytesMoved = 0
            For i As Integer = StartIdx To UBound(files)
                Dim file__1 = files(i)
                If CopyWorker.CancellationPending Then
                    e.Cancel = True
                    Exit Sub
                End If
                'Counter for progress
                CurFileIdx += 1
                CurrentFileIndex = CurFileIdx
                Args.CurrentIndex = CurFileIdx
                'Modify source path to target path
                Dim cPath As String = Replace(file__1, sourceDir, targetDir)
                'Record status for UI updates
                Dim Status As New Status_Stats(files.Count, CurFileIdx, cPath, file__1, CurrentStatus.CurFileProgress, CurrentStatus.CurTransferRate)
                'Report status
                CopyWorker.ReportProgress(1, Status)
                e.Result = Args
                'Check is file extists on target. Then check if file is read-only and try to change attribs
                If File.Exists(cPath) Then
                    Dim FileAttrib As FileAttributes = File.GetAttributes(cPath)
                    If (FileAttrib And FileAttributes.ReadOnly) = FileAttrib.ReadOnly Then
                        CopyWorker.ReportProgress(99, "******* File is read-only. Changing attributes...")
                        FileAttrib = FileAttrib And (Not FileAttributes.ReadOnly)
                        File.SetAttributes(cPath, FileAttrib)
                    End If
                Else
                    If Not Directory.Exists(Path.GetDirectoryName(cPath)) Then
                        CopyWorker.ReportProgress(99, "******* Creating Missing Directory: " & Path.GetDirectoryName(cPath))
                        Directory.CreateDirectory(Path.GetDirectoryName(cPath))
                    End If
                End If
                'Copy source to target, overwriting
                Dim BufferSize As Integer = 256000
                Dim perc As Integer = 0
                Dim buffer(BufferSize) As Byte
                Dim bytesIn As Integer = 1
                Dim totalBytesIn As Integer
                Dim CurrentFile As New FileInfo(file__1)
                stpSpeed.Start()
                Using fStream As System.IO.FileStream = CurrentFile.OpenRead(),
                        destFile As System.IO.Stream = New FileStream(cPath, FileMode.OpenOrCreate) ',
                    ' bufStream As New BufferedStream(destFile, BufferSize)
                    CurrentStatus.CurFileProgress = 1
                    totalBytesIn = 0
                    Dim flLength As Integer = fStream.Length
                    Do Until bytesIn < 1 Or CopyWorker.CancellationPending
                        bytesIn = fStream.Read(buffer, 0, BufferSize)
                        If bytesIn > 0 Then

                            destFile.Write(buffer, 0, bytesIn)
                            ' bufStream.Write(buffer, 0, bytesIn)
                            totalBytesIn += bytesIn
                            lngBytesMoved += bytesIn
                            If flLength > 0 Then
                                perc = CInt((totalBytesIn / flLength) * 100)

                                CurrentStatus.CurFileProgress = perc
                                'If perc = 100 Then
                                '    Debug.Print(perc)
                                'End If
                                '  Debug.Print(perc)
                                ' Threading.Thread.Sleep(100)
                            End If
                        End If
                    Loop
                End Using
            Next
        End Using
    End Sub

    Private Sub CopyWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs)

        If e.ProgressPercentage = 1 Then
            CurrentStatus = DirectCast(e.UserState, Status_Stats)
            OnStatusUpdate(New GKUpdateEvents(CurrentStatus))
            GKLog(CurrentStatus.CurFileIdx & " of " & CurrentStatus.TotFiles)
            GKLog("Source: " & CurrentStatus.SourceFileName)
            GKLog("Dest: " & CurrentStatus.CurFileName)
        Else
            GKLog(e.UserState.ToString, True)
            stpSpeed.Stop()
            stpSpeed.Reset()
        End If
    End Sub

    Private Sub CopyWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        If e.Error Is Nothing Then
            If Not e.Cancelled Then
                GKLog("Copy successful!  Errors: " & ErrList.Count)
                If ErrList.Count > 0 Then
                    GKLog("Listing Errors: ")
                    For Each ErrMsg In ErrList
                        GKLog("---  " & (ErrList.IndexOf(ErrMsg) + 1) & " of " & ErrList.Count)
                        GKLog(ErrMsg)
                        GKLog("---")
                    Next
                End If
                GKLog("All done!")
                GKLog("------------------------------------------------")
                OnUpdateComplete(New GKUpdateCompleteEvents(False))
            Else
                OnUpdateCancelled(New EventArgs)
                GKLog("Cancelled by user!")
            End If
        Else
            If e.Error.HResult = -2147024864 Or e.Error.HResult = -2147024891 Then
                GKLog("******** File in-use error! Resuming next files.", True)
                Dim NewArgs As Worker_Args
                NewArgs.StartIndex = CurrentFileIndex
                If Not CopyWorker.IsBusy Then CopyWorker.RunWorkerAsync(NewArgs)
            Else
                GKLog("------------------------------------------------")
                GKLog("Unexpected erors during copy!")
                GKLog(e.Error.Message, True)
                OnUpdateComplete(New GKUpdateCompleteEvents(True, e.Error))
            End If
        End If
    End Sub
    Private Sub GKLog(Message As String, Optional ToErrList As Boolean = False)
        Dim NewLog As New GK_Log_Info(Message, ToErrList)
        OnLogEvent(New LogEvents(NewLog))
        If ToErrList Then

            Dim ErrMsg As String = "Error: " & Message & vbCrLf &
                "Info: " & vbCrLf &
            CurrentStatus.CurFileIdx & " of " & CurrentStatus.TotFiles & vbCrLf &
                "Source: " & CurrentStatus.SourceFileName & vbCrLf &
                "Dest: " & CurrentStatus.CurFileName
            ErrList.Add(ErrMsg)
        End If
    End Sub
    Private Sub InitializeTimer()
        SpeedTimer = New Timer
        SpeedTimer.Interval = 100
        SpeedTimer.Enabled = True
        AddHandler SpeedTimer.Tick, AddressOf SpeedTimer_Tick
    End Sub

    Private Sub InitWorker()
        CopyWorker = New BackgroundWorker
        AddHandler CopyWorker.DoWork, AddressOf CopyWorker_DoWork
        AddHandler CopyWorker.RunWorkerCompleted, AddressOf CopyWorker_RunWorkerCompleted
        AddHandler CopyWorker.ProgressChanged, AddressOf CopyWorker_ProgressChanged
        With CopyWorker
            .WorkerReportsProgress = True
            .WorkerSupportsCancellation = True
        End With
    End Sub

    Private Sub SpeedTimer_Tick()
        Dim BytesPerSecond As Single
        Dim ResetCounter As Integer = 40
        If lngBytesMoved > 0 Then
            progIts += 1
            BytesPerSecond = Math.Round((lngBytesMoved / stpSpeed.ElapsedMilliseconds) / 1000, 2)
            CurrentStatus.CurTransferRate = BytesPerSecond
            If progIts > ResetCounter Then
                progIts = 0
                lngBytesMoved = 0 'BytesPerSecond * stpSpeed.ElapsedMilliseconds * 1000
                stpSpeed.Restart()

            End If
        Else
            End If
    End Sub
    Public Structure GK_Complete_Stats
        Public Errors As Boolean
    End Structure

    Public Structure GK_Log_Info
        Public ErrList As List(Of String)
        Public Message As String
        Public ToErrList As Boolean
        Sub New(Msg As String, ToErrLst As Boolean)
            Message = Msg
            ToErrList = ToErrLst
        End Sub
    End Structure

    Public Structure Status_Stats
        Public CurFileIdx As Integer
        Public CurFileName As String
        Public CurFileProgress As Integer
        Public CurTransferRate As Single
        Public SourceFileName As String
        Public TotFiles As Integer
        Sub New(tFiles As Integer, CurFIdx As Integer, CurFName As String, sFileName As String, CurFileProg As Integer, CurTransRate As Single)
            TotFiles = tFiles
            CurFileIdx = CurFIdx
            CurFileName = CurFName
            SourceFileName = sFileName
            CurFileProgress = CurFileProg
            CurTransferRate = CurTransRate
        End Sub
    End Structure

    Private Structure Worker_Args
        Public CurrentIndex As Integer
        Public StartIndex As Integer
    End Structure
    Public Class GKUpdateCompleteEvents : Inherits EventArgs
        Private ErrExeption As Exception
        Private Errs As Boolean
        Public Sub New(ByVal Errs As Boolean, Optional ByVal Ex As Exception = Nothing)
            Me.Errs = Errs
            Me.ErrExeption = Ex
        End Sub
        Public ReadOnly Property Errors As Exception
            Get
                Return ErrExeption
            End Get
        End Property

        Public ReadOnly Property HasErrors As Boolean
            Get
                Return Errs
            End Get
        End Property
    End Class

    Public Class GKUpdateEvents : Inherits EventArgs
        Private eStatus As Status_Stats
        Public Sub New(ByVal Status As Status_Stats)
            eStatus = Status
        End Sub
        Public ReadOnly Property CurrentStatus As Status_Stats
            Get
                Return eStatus
            End Get
        End Property
    End Class

    Public Class LogEvents : Inherits EventArgs
        Private MyLogInfo As GK_Log_Info
        Public Sub New(ByVal LogInfo As GK_Log_Info)
            MyLogInfo = LogInfo
        End Sub
        Public ReadOnly Property LogData As GK_Log_Info
            Get
                Return MyLogInfo
            End Get
        End Property
    End Class
#Region "IDisposable Support"
    Private disposedValue As Boolean ' To detect redundant calls

    ' This code added by Visual Basic to correctly implement the disposable pattern.
    Public Sub Dispose() Implements IDisposable.Dispose
        ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        Dispose(True)
        ' TODO: uncomment the following line if Finalize() is overridden above.
        ' GC.SuppressFinalize(Me)
    End Sub

    ' IDisposable
    Protected Overridable Sub Dispose(disposing As Boolean)
        If Not disposedValue Then
            If disposing Then
                ' TODO: dispose managed state (managed objects).
            End If
            CopyWorker.Dispose()

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
#End Region

End Class