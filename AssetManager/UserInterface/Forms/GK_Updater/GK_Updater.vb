Imports System.Net
Imports System.IO
Imports System.ComponentModel

Public Class GK_Updater : Implements IDisposable
    Private WithEvents CopyWorker As BackgroundWorker
    Private ReadOnly GKPath As String = "\PSi\Gatekeeper"
    Private AdmCredentials As NetworkCredential
    Private ClientPath As String
    Private CurrentFileIndex As Integer = 0
    Private CurrentStatus As Status_Stats
    Private ErrList As New List(Of String)
    Private ServerPath As String = "C:"
    Private UpdateDevice As Device_Info
    Sub New(Device As Device_Info, AdminCredentials As NetworkCredential)
        UpdateDevice = Device
        AdmCredentials = AdminCredentials
        ClientPath = "\\D" & UpdateDevice.strSerial & "\c$"
        InitWorker()

    End Sub
    Public Event LogEvent As EventHandler
    Public Event StatusUpdate As EventHandler
    Public Event UpdateComplete As EventHandler
    Public Event UpdateCancelled As EventHandler
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
        GKLog("------------------------------------------------")
        GKLog("Starting GK Update to: " & UpdateDevice.strSerial)
        GKLog("Copying files...")
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
    Protected Overridable Sub OnUpdateComplete(e As GKUpdateCompleteEvents)
        RaiseEvent UpdateComplete(Me, e)
    End Sub
    Protected Overridable Sub OnUpdateCancelled(e As EventArgs)
        RaiseEvent UpdateCancelled(Me, e)
    End Sub

    Private Sub CopyWorker_DoWork(sender As Object, e As DoWorkEventArgs)
        Using NetCon As New NetworkConnection(ClientPath, AdmCredentials)
            Dim sourceDir As String = ServerPath & GKPath
            Dim targetDir As String = ClientPath & GKPath
            Dim Args As Worker_Args = DirectCast(e.Argument, Worker_Args)
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
                Dim Status As New Status_Stats(files.Count, CurFileIdx, cPath, file__1)
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
                File.Copy(file__1, cPath, True)
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
            If e.Error.HResult = -2147024864 Then
                GKLog("******** File in-use error! Resuming next files.", True)
                Dim NewArgs As Worker_Args
                NewArgs.StartIndex = CurrentFileIndex
                If Not CopyWorker.IsBusy Then CopyWorker.RunWorkerAsync(NewArgs)
            Else
                GKLog("------------------------------------------------")
                GKLog("Unexpected erors during copy!")
                GKLog(e.Error.Message, True)
                OnUpdateComplete(New GKUpdateCompleteEvents(True))
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
        Public SourceFileName As String
        Public TotFiles As Integer
        Sub New(tFiles As Integer, CurFIdx As Integer, CurFName As String, sFileName As String)
            TotFiles = tFiles
            CurFileIdx = CurFIdx
            CurFileName = CurFName
            SourceFileName = sFileName
        End Sub
    End Structure

    Private Structure Worker_Args
        Public CurrentIndex As Integer
        Public StartIndex As Integer
    End Structure
    Public Class GKUpdateCompleteEvents : Inherits EventArgs
        Private Errs As Boolean
        Public Sub New(ByVal Errs As Boolean)
            Me.Errs = Errs
        End Sub
        Public ReadOnly Property Errors As Boolean
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
            AdmCredentials = Nothing

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