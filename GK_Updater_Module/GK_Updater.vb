﻿Imports System.ComponentModel
Imports System.IO
Imports System.Net
Imports System.Net.NetworkInformation
Imports System.Text
Imports System.Windows.Forms

Public Class GK_Updater : Implements IDisposable

#Region "Members"

    Private WithEvents CopyWorker As BackgroundWorker
    Private WithEvents SpeedTimer As Timer
    Private CurrentStatus As Status_Stats
    Private ErrList As New List(Of String)
    Private ReadOnly GKPath As String = "\PSi\Gatekeeper\"
    Private GKSourcePath As String
    Private DestinationPath As String
    Private ClientPath As String
    Private ClientHostName As String
    Private CurrentCreds As NetworkCredential
    Private CurrentFileIndex As Integer = 0
    Private ElapTime As New Stopwatch
    Private Progress As New ProgressCounter
    Private bolCreateMissingDirectory As Boolean = True
    Private bolPaused As Boolean = False

#End Region

#Region "Constructors"

    Sub New(ByVal HostName As String, SourcePath As String)
        GKSourcePath = SourcePath
        DestinationPath = GKPath
        ClientHostName = HostName
        ClientPath = "\\" & HostName & "\c$"
        InitWorker()
        InitializeTimer()
    End Sub

    Sub New(ByVal HostName As String, SourcePath As String, DestPath As String)
        GKSourcePath = SourcePath
        DestinationPath = DestPath
        ClientHostName = HostName
        ClientPath = "\\" & HostName & "\c$"
        InitWorker()
        InitializeTimer()
    End Sub


#End Region

#Region "Event Handlers"

    Public Event LogEvent As EventHandler

    Public Event StatusUpdate As EventHandler

    Public Event UpdateCanceled As EventHandler

    Public Event UpdateComplete As EventHandler

    Protected Overridable Sub OnLogEvent(e As LogEvents)
        RaiseEvent LogEvent(Me, e)
    End Sub

    Protected Overridable Sub OnStatusUpdate(e As GKUpdateEvents)
        RaiseEvent StatusUpdate(Me, e)
    End Sub

    Protected Overridable Sub OnUpdateCanceled(e As EventArgs)
        GKLog("Canceled by user!")
        RaiseEvent UpdateCanceled(Me, e)
    End Sub

    Protected Overridable Sub OnUpdateComplete(e As GKUpdateCompleteEvents)
        RaiseEvent UpdateComplete(Me, e)
    End Sub

#End Region

#Region "Properties"

    Public ReadOnly Property IsDisposed As Boolean
        Get
            Return disposedValue
        End Get
    End Property

    Public ReadOnly Property ErrorList As List(Of String)
        Get
            Return ErrList
        End Get
    End Property

    Public ReadOnly Property UpdateStatus As Status_Stats
        Get
            Return CurrentStatus
        End Get
    End Property

    Public Property CreateMissingDirectories As Boolean
        Get
            Return bolCreateMissingDirectory
        End Get
        Set(value As Boolean)
            bolCreateMissingDirectory = value
        End Set
    End Property

#End Region

#Region "Methods"

    Public Sub CancelUpdate()
        bolPaused = False
        If CopyWorker.IsBusy Then
            CopyWorker.CancelAsync()
        Else
            OnUpdateCanceled(New EventArgs)
        End If
        CurrentFileIndex = 0
    End Sub

    Public Sub PauseUpdate()
        bolPaused = True
        Progress = New ProgressCounter
        CopyWorker.CancelAsync()
    End Sub

    Public Sub ResumeUpdate()
        bolPaused = False
        Dim NewArgs As Worker_Args
        NewArgs.StartIndex = CurrentFileIndex
        NewArgs.Credentials = CurrentCreds
        If Not CopyWorker.IsBusy Then CopyWorker.RunWorkerAsync(NewArgs)
    End Sub

    Public Sub StartUpdate(Creds As NetworkCredential)
        If Creds Is Nothing Then
            Throw New Win32Exception(1326)
            Exit Sub
        End If
        GKLog("------------------------------------------------")
        GKLog("Starting GK Update to: " & ClientHostName)
        GKLog("Starting Update...")
        ErrList.Clear()
        Dim WorkArgs As New Worker_Args
        WorkArgs.StartIndex = 0
        WorkArgs.Credentials = Creds
        CurrentCreds = Creds
        Progress = New ProgressCounter
        ElapTime = New Stopwatch
        ElapTime.Start()
        If Not CopyWorker.IsBusy Then CopyWorker.RunWorkerAsync(WorkArgs)
    End Sub

    ''' <summary>
    ''' Pings the current device. Success returns True. All failures return False.
    ''' </summary>
    ''' <returns></returns>
    Private Function CanPing() As Boolean
        Try
            Using MyPing As New Ping
                Dim options As New Net.NetworkInformation.PingOptions
                Dim Hostname As String = ClientHostName
                Dim Timeout As Integer = 1000
                Dim buff As Byte() = Encoding.ASCII.GetBytes("pingpingpingpingping")
                options.DontFragment = True
                Dim reply As PingReply = MyPing.Send(Hostname, Timeout, buff, options)
                If reply.Status = IPStatus.Success Then
                    Return True
                End If
                Return False
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function

    Private Sub CopyFile(Source As String, Dest As String)
        Dim BufferSize As Integer = 256000
        Dim buffer(BufferSize - 1) As Byte
        Dim bytesIn As Integer = 1
        Dim CurrentFile As New FileInfo(Source)
        Progress.ResetProgress()
        Using fStream As System.IO.FileStream = CurrentFile.OpenRead(),
                destFile As System.IO.FileStream = New FileStream(Dest, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, BufferSize, FileOptions.None)
            CurrentStatus.CurFileProgress = 1
            Progress.BytesToTransfer = CInt(fStream.Length)
            Do Until bytesIn < 1 Or CopyWorker.CancellationPending
                bytesIn = fStream.Read(buffer, 0, BufferSize)
                If bytesIn > 0 Then
                    destFile.Write(buffer, 0, bytesIn)
                    Progress.BytesMoved = bytesIn
                End If
            Loop
        End Using
    End Sub

    Private Sub CopyWorker_DoWork(sender As Object, e As DoWorkEventArgs)
        If Not CanPing() Then
            Throw New Exception("Cannot ping device.")
        End If
        Dim Args As Worker_Args = DirectCast(e.Argument, Worker_Args)
        Using NetCon As New NetworkConnection(ClientPath, Args.Credentials)
            Dim sourceDir As String = GKSourcePath
            Dim targetDir As String = ClientPath & DestinationPath 'GKPath
            Dim StartIdx As Integer = Args.StartIndex
            Dim CurFileIdx As Integer = Args.StartIndex
            'Get array of full paths of all files in source dir and sub-dirs
            Dim files() = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories)
            'Loop through file array
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
                'Check if file extists on target. Then check if file is read-only and try to change attribs
                If File.Exists(cPath) Then
                    Dim FileAttrib As FileAttributes = File.GetAttributes(cPath)
                    If (FileAttrib And FileAttributes.ReadOnly) = FileAttributes.ReadOnly Then
                        CopyWorker.ReportProgress(99, "******* File is read-only. Changing attributes...")
                        FileAttrib = FileAttrib And (Not FileAttributes.ReadOnly)
                        File.SetAttributes(cPath, FileAttrib)
                    End If
                Else
                    If Not Directory.Exists(Path.GetDirectoryName(cPath)) Then
                        If bolCreateMissingDirectory Then
                            CopyWorker.ReportProgress(99, "******* Creating Missing Directory: " & Path.GetDirectoryName(cPath))
                            Directory.CreateDirectory(Path.GetDirectoryName(cPath))
                        Else
                            Throw New MissingDirectoryException
                        End If
                    End If
                End If
                'Copy source to target, overwriting
                CopyFile(file__1, cPath)
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
        End If
    End Sub

    Private Sub CopyWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        If e.Error Is Nothing Then
            If Not e.Cancelled Then
                ElapTime.Stop()
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
                GKLog("Elapsed time: " & (ElapTime.ElapsedMilliseconds / 1000) & "s")
                GKLog("------------------------------------------------")
                OnUpdateComplete(New GKUpdateCompleteEvents(False))
            Else
                If Not bolPaused Then
                    OnUpdateCanceled(New EventArgs)
                End If
            End If
        Else
            If e.Error.HResult = -2147024864 Or e.Error.HResult = -2147024891 Then
                GKLog("******** File in-use error! Resuming next files.", True)
                ResumeUpdate()
            Else
                GKLog("------------------------------------------------")
                GKLog("Unexpected errors during copy!")
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

    Private Sub SpeedTimer_Tick(sender As Object, e As EventArgs)
        If Not bolPaused Then Progress.Tick()
        If Progress.BytesMoved > 0 Then
            CurrentStatus.CurTransferRate = Progress.Throughput
            CurrentStatus.CurFileProgress = Progress.Percent
        Else
        End If
    End Sub

#End Region

#Region "Structures And Classes"

    Public Structure GK_Log_Info
        Public Property Message As String
        Public Property ToErrList As Boolean

        Sub New(Msg As String, ToErrLst As Boolean)
            Message = Msg
            ToErrList = ToErrLst
        End Sub

    End Structure

    Public Structure Status_Stats
        Public Property CurFileIdx As Integer
        Public Property CurFileName As String
        Public Property CurFileProgress As Integer
        Public Property CurTransferRate As Double
        Public Property SourceFileName As String
        Public Property TotFiles As Integer

        Sub New(tFiles As Integer, CurFIdx As Integer, CurFName As String, sFileName As String, CurFileProg As Integer, CurTransRate As Double)
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
        Public Credentials As NetworkCredential
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

    Public Class MissingDirectoryException
        Inherits Exception

        Public Sub New()
            MyBase.New("Directory not found on target.")
        End Sub

    End Class

#End Region

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
                CopyWorker.Dispose()
                SpeedTimer.Dispose()
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

#End Region

End Class