Imports System.Net
Imports System.IO
Imports System.ComponentModel
Public Class GKProgress
    Private WithEvents CopyWorker As BackgroundWorker
    Private ReadOnly GKPath As String = "\PSi\Gatekeeper"
    Private AdmPassword As String
    Private AdmUsername As String
    Private ClientPath As String
    Private CurrentStatus As Status_Stats
    Private MyDevice As Device_Info
    Private ServerPath As String
    Private CtlDown As Boolean
    Private ErrList As New List(Of String)
    Sub New(ParentForm As MyForm, Device As Device_Info)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Tag = ParentForm
        Icon = ParentForm.Icon
        MyDevice = Device
        ServerPath = "C:" '"\\" & ServerName & "\c$"
        ClientPath = "\\D" & MyDevice.strSerial & "\c$"
        Text = Text + " - " + MyDevice.strCurrentUser
        InitWorker()
        Show()
    End Sub
    Public Sub CopyFiles()
        Try
            Dim WorkArgs As New Worker_Args
            WorkArgs.StartIndex = 0
            If Not CopyWorker.IsBusy Then CopyWorker.RunWorkerAsync(WorkArgs)
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub
    Private Sub cmdGo_Click(sender As Object, e As EventArgs) Handles cmdGo.Click
        RunUpdate()
    End Sub
    Private Sub CopyWorker_DoWork(sender As Object, e As DoWorkEventArgs)
        Dim creds As New NetworkCredential()
        creds.Password = AdmPassword
        creds.UserName = AdmUsername
        creds.Domain = Environment.UserDomainName
        Using NetCon As New NetworkConnection(ClientPath, creds)
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
            pbarProgress.Maximum = CurrentStatus.TotFiles
            pbarProgress.Value = CurrentStatus.CurFileIdx
            Dim CurrentFile = CurrentStatus.CurFileName
            lblCurrentFile.Text = CurrentFile
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
                Text = Text + " - *COMPLETE*"
            Else
                GKLog("Cancelled by user!")
            End If
        Else
            If e.Error.HResult = -2147024864 Then
                GKLog("******** File in-use error! Resuming next files.", True)
                Dim NewArgs As Worker_Args
                NewArgs.StartIndex = CurrentStatus.CurFileIdx ' + 1
                If Not CopyWorker.IsBusy Then CopyWorker.RunWorkerAsync(NewArgs)
            Else
                Text = Text + " - *ERRORS!*"
                GKLog("------------------------------------------------")
                GKLog("Errors during copy!")
                GKLog(e.Error.Message, True)
            End If
        End If
    End Sub
    Private Sub GKLog(Message As String, Optional ToErrList As Boolean = False)
        rtbLog.AppendText(Message + vbCrLf)
        rtbLog.Refresh()
        Logger(Message)
        Invalidate()
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
    Private Sub RunUpdate()
        Try
            GKLog("------------------------------------------------")
            GKLog("Starting GK Update to: " & MyDevice.strSerial)
            txtUsername.ReadOnly = True
            txtPassword.ReadOnly = True
            cmdGo.Enabled = False
            AdmUsername = Trim(txtUsername.Text)
            AdmPassword = Trim(txtPassword.Text)
            GKLog("Copying files...")
            CopyFiles()
            BringToFront()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub

    Private Structure Status_Stats
        Public CurFileIdx As Integer
        Public SourceFileName As String
        Public CurFileName As String
        Public TotFiles As Integer
        Sub New(tFiles As Integer, CurFIdx As Integer, CurFName As String, sFileName As String)
            TotFiles = tFiles
            CurFileIdx = CurFIdx
            CurFileName = CurFName
            SourceFileName = sFileName
        End Sub
    End Structure

    Private Structure Worker_Args
        Public StartIndex As Integer
        Public CurrentIndex As Integer
    End Structure

    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyDown
        If e.KeyCode = Keys.Return Then
            RunUpdate()
        End If
    End Sub

    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        CopyWorker.CancelAsync()
    End Sub

    Private Sub rtbLog_MouseWheel(sender As Object, e As MouseEventArgs) Handles rtbLog.MouseWheel
        Dim Multi As Single = 0.001
        Dim Diff As Single = e.Delta * Multi
        Dim NewZoom As Single = rtbLog.ZoomFactor + Diff
        If CtlDown Then
            If NewZoom > 0 And NewZoom < 3 Then
                rtbLog.ZoomFactor = NewZoom
            End If
        End If

    End Sub

    Private Sub rtbLog_KeyDown(sender As Object, e As KeyEventArgs) Handles rtbLog.KeyDown
        If e.Control Then CtlDown = True
    End Sub

    Private Sub rtbLog_KeyUp(sender As Object, e As KeyEventArgs) Handles rtbLog.KeyUp
        CtlDown = False
    End Sub
End Class