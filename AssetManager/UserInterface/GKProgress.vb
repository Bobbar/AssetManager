Imports System.Net
Imports System.IO
Imports System.ComponentModel
Public Class GKProgress
    Private WithEvents CopyWorker As BackgroundWorker
    Private ReadOnly GKPath As String = "\PSi\Gatekeeper"
    Private ReadOnly ServerName As String = "10.10.0.184"
    '"ddad-svr-fs01"
    Private ReadOnly ShareName As String = "\c$"

    Private AdmPassword As String
    Private AdmUsername As String
    Private ClientPath As String
    Private CurrentFileIdx As Integer = 0
    Private CurrentStatus As Status_Stats
    Private MaxFiles As Integer = 1
    Private MyDevice As Device_Info
    Private PrevFile As String = ""
    Private ServerPath As String
    Sub New(ParentForm As MyForm, Device As Device_Info)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Tag = ParentForm
        Icon = ParentForm.Icon
        MyDevice = Device
        ServerPath = "\\" & ServerName & "\c$"
        ClientPath = "\\D" & MyDevice.strSerial & "\c$"
        Text = Text + " - " + MyDevice.strCurrentUser
        InitWorker()
        Show()
    End Sub

    Public Sub CopyFiles()
        Try
            If Not CopyWorker.IsBusy Then CopyWorker.RunWorkerAsync()
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
        Dim CurFileIdx As Integer = 0
        Dim sourceDir As String = ServerPath & GKPath
        Dim targetDir As String = ClientPath & GKPath
        Dim files As String() = Directory.GetFiles(sourceDir)
        For Each file__1 In files
            CurFileIdx += 1
            Dim Status As New Status_Stats(files.Count, CurFileIdx, file__1)
            CopyWorker.ReportProgress(1, Status)
            'Debug.Print("Copying: " & file__1)
            File.Copy(file__1, Path.Combine(targetDir, Path.GetFileName(file__1)), True)
        Next
    End Sub

    Private Sub CopyWorker_ProgressChanged(sender As Object, e As ProgressChangedEventArgs)
        CurrentStatus = DirectCast(e.UserState, Status_Stats)
        pbarProgress.Maximum = CurrentStatus.TotFiles
        pbarProgress.Value = CurrentStatus.CurFileIdx
        Dim CurrentFile = CurrentStatus.CurFileName
        lblCurrentFile.Text = CurrentFile
        If PrevFile <> CurrentFile Then
            GKLog(CurrentStatus.CurFileIdx & " of " & CurrentStatus.TotFiles & ": " & CurrentFile)
            PrevFile = CurrentFile
        End If
        Application.DoEvents()
    End Sub

    Private Sub CopyWorker_RunWorkerCompleted(sender As Object, e As RunWorkerCompletedEventArgs)
        If e.Error Is Nothing Then
            GKLog("Copy successful!")
            GKLog("Disconnecting Maps...")
            GKLog("Server Map: Disconnecting...")
            If RemoveServerDrive() Then
                GKLog("Server Map: Disconnected")
            Else
                GKLog("Server Map Disconnect Failed!")
            End If
            GKLog("Client Map: Disconnecting...")
            If RemoveClientDrive() Then
                GKLog("Client Map: Disconnected")
            Else
                GKLog("Client Map Disconnect Failed!")
            End If
            GKLog("All done!")
            GKLog("------------------------------------------------")
            Text = Text + " - *COMPLETE*"
        Else
            Text = Text + " - *ERRORS!*"
            GKLog("------------------------------------------------")
            GKLog("Errors during copy!")
            GKLog(e.Error.Message)
        End If
    End Sub

    Private Sub GKLog(Message As String)
        lstLog.Items.Add(Message)
        lstLog.TopIndex = lstLog.Items.Count - 1
        Logger(Message)
        Application.DoEvents()
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
            GKLog("Server Map: Connecting...")
            If ConnectServerDrive() Then
                GKLog("Server Map: Connected!")
            Else
                GKLog("Server Map Failed!")
                Exit Sub
            End If
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
        Public CurFileName As String
        Public TotFiles As Integer
        Sub New(tFiles As Integer, CurFIdx As Integer, CurFName As String)
            TotFiles = tFiles
            CurFileIdx = CurFIdx
            CurFileName = CurFName
        End Sub
    End Structure
End Class