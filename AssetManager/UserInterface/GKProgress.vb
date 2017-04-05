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

    Private Function ConnectClientDrive() As Boolean
        Try
            TopMost = True
            Dim pClient As Process = New Process
            pClient.StartInfo.UseShellExecute = False
            pClient.StartInfo.RedirectStandardOutput = True
            pClient.StartInfo.RedirectStandardError = True
            pClient.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
            pClient.StartInfo.FileName = "net.exe"
            pClient.StartInfo.Arguments = "USE " & ClientPath & " " & AdmPassword & " /USER:" & AdmUsername
            pClient.Start()
            Dim output As String
            output = pClient.StandardError.ReadToEnd
            pClient.WaitForExit()
            TopMost = False
            Debug.Print(output)
            If Trim(output) = "" Then
                Return True
            Else
                GKLog(output)
                Return False
            End If
            Return False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
    End Function

    Private Function ConnectServerDrive() As Boolean
        Try
            TopMost = True
            Dim pServer As Process = New Process
            pServer.StartInfo.UseShellExecute = False
            pServer.StartInfo.RedirectStandardOutput = True
            pServer.StartInfo.RedirectStandardError = True
            pServer.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
            pServer.StartInfo.FileName = "net.exe"
            pServer.StartInfo.Arguments = "USE " & ServerPath & " " & AdmPassword & " /USER:" & AdmUsername
            pServer.Start()
            Dim output As String
            output = pServer.StandardError.ReadToEnd
            pServer.WaitForExit()
            TopMost = False
            Debug.Print(output)
            If Trim(output) = "" Then
                Return True
            Else
                GKLog(output)
                Return False
            End If
            Return False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
    End Function

    Private Sub CopyWorker_DoWork(sender As Object, e As DoWorkEventArgs)

        Dim sourceDir As String = ServerPath & GKPath
        Dim targetDir As String = ClientPath & GKPath
        Dim Args As Worker_Args = DirectCast(e.Argument, Worker_Args)
        Dim StartIdx As Integer = Args.StartIndex
        Dim CurFileIdx As Integer = Args.StartIndex
        'Get array of full paths of all files in source dir and sub-dirs
        Dim files() = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories)

        'Loop through file array
        For i As Integer = StartIdx To UBound(files) ' In files
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

            'Copy source to target, overwritting
            File.Copy(file__1, cPath, True)


        Next



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
            GKLog(e.UserState.ToString)
        End If

    End Sub

    Private Sub CopyWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        If e.Error Is Nothing Then
            If Not e.Cancelled Then
                GKLog("Copy successful!")
                GKLog("Disconnecting Maps...")
                'GKLog("Server Map: Disconnecting...")
                'If RemoveServerDrive() Then
                '    GKLog("Server Map: Disconnected")
                'Else
                '    GKLog("Server Map Disconnect Failed!")
                'End If
                If Not RemoveClientDrive() Then Exit Sub

                GKLog("All done!")
                GKLog("------------------------------------------------")
                Text = Text + " - *COMPLETE*"
            Else
                GKLog("Cancelled by user!")
                GKLog("Disconnecting Maps...")
                If Not RemoveClientDrive() Then Exit Sub
            End If
        Else

            If e.Error.HResult = -2147024864 Then
                GKLog("******** File in-use error! Resuming next files.")
                ' Dim OldArgs As Worker_Args = DirectCast(e.Result, Worker_Args)
                Dim NewArgs As Worker_Args
                NewArgs.StartIndex = CurrentStatus.CurFileIdx ' + 1
                If Not CopyWorker.IsBusy Then CopyWorker.RunWorkerAsync(NewArgs)

            Else


                Text = Text + " - *ERRORS!*"
                GKLog("------------------------------------------------")
                GKLog("Errors during copy!")
                GKLog(e.Error.Message)
                GKLog("Disconnecting Maps...")
                If Not RemoveClientDrive() Then Exit Sub
            End If
        End If
    End Sub

    Private Sub GKLog(Message As String)
        lstLog.Items.Add(Message)
        Logger(Message)
        lstLog.TopIndex = lstLog.Items.Count - 1
        lstLog.Refresh()
        Invalidate()
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

    Private Function RemoveClientDrive() As Boolean
        Try
            GKLog("Client Map: Disconnecting...")
            TopMost = True
            Dim pClient As Process = New Process
            pClient.StartInfo.UseShellExecute = False
            pClient.StartInfo.RedirectStandardOutput = True
            pClient.StartInfo.RedirectStandardError = True
            pClient.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
            pClient.StartInfo.FileName = "net.exe"
            pClient.StartInfo.Arguments = "USE " & ClientPath & " /delete"
            pClient.Start()
            Dim output As String
            output = pClient.StandardError.ReadToEnd
            pClient.WaitForExit()
            TopMost = False
            Debug.Print(output)
            If Trim(output) = "" Then
                GKLog("Client Map: Disconnected")
                Return True
            Else
                GKLog("Client Map Disconnect Failed!")
                GKLog(output)
                Return False
            End If
            Return False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
    End Function

    Private Function RemoveServerDrive() As Boolean
        Try
            TopMost = True
            Dim pServer As Process = New Process
            pServer.StartInfo.UseShellExecute = False
            pServer.StartInfo.RedirectStandardOutput = True
            pServer.StartInfo.RedirectStandardError = True
            pServer.StartInfo.WindowStyle = ProcessWindowStyle.Minimized
            pServer.StartInfo.FileName = "net.exe"
            pServer.StartInfo.Arguments = "USE " & ServerPath & " /delete"
            pServer.Start()
            Dim output As String
            output = pServer.StandardError.ReadToEnd
            pServer.WaitForExit()
            TopMost = False
            Debug.Print(output)
            If Trim(output) = "" Then
                Return True
            Else
                GKLog(output)
                Return False
            End If
            Return False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return False
        End Try
    End Function

    Private Sub RunUpdate()
        Try
            GKLog("------------------------------------------------")
            GKLog("Starting GK Update to: " & MyDevice.strSerial)
            GKLog("Mapping drives...")
            txtUsername.ReadOnly = True
            txtPassword.ReadOnly = True
            cmdGo.Enabled = False
            AdmUsername = Trim(txtUsername.Text)
            AdmPassword = Trim(txtPassword.Text)
            'GKLog("Server Map: Connecting...")
            'If ConnectServerDrive() Then
            '    GKLog("Server Map: Connected!")
            'Else
            '    GKLog("Server Map Failed!")
            '    Exit Sub
            'End If
            GKLog("Client Map: Connecting...")
            If ConnectClientDrive() Then
                GKLog("Client Map: Connected!")
            Else
                GKLog("Client Map Failed!")
                Exit Sub
            End If
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
End Class