Imports System.Net
Imports System.IO
Public Class GK_Updater
    Private ReadOnly ServerName As String = "10.10.0.184" '"ddad-svr-fs01"
    Private ReadOnly ShareName As String = "\c$"
    Private ServerPath As String
    Private ClientPath As String
    Private AdmUsername As String
    Private AdmPassword As String

    ' Private ReadOnly Domain As String = "core.co.fairfield.oh.us\"
    Sub New(Username As String, Password As String, ClientName As String) ', ComputerName As String)
        AdmUsername = Username
        AdmPassword = Password
        ServerPath = "\\" & ServerName & "\c$"
        ClientPath = "\\D" & ClientName & "\c$"



    End Sub
    Public Function ConnectServerDrive() As Boolean
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
        Debug.Print(output)
        If Trim(output) = "" Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function
    Public Function ConnectClientDrive() As Boolean
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
        Debug.Print(output)
        If Trim(output) = "" Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function
    Public Function RemoveServerDrive() As Boolean
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
        Debug.Print(output)
        If Trim(output) = "" Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function
    Public Function RemoveClientDrive() As Boolean
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
        Debug.Print(output)
        If Trim(output) = "" Then
            Return True
        Else
            Return False
        End If
        Return False
    End Function

    Public Function CopyFiles(ByRef CurFile As String, ByRef TotFiles As Integer, CurFileIdx As Integer) As Boolean
        Try
            Dim GKPath As String = "\PSi\Gatekeeper"

            Dim sourceDir As String = ServerPath & GKPath
            Dim targetDir As String = ClientPath & GKPath
            Dim files As String() = Directory.GetFiles(sourceDir)

            TotFiles = files.Count - 1
            For Each file__1 In files

                CurFileIdx += 1
                CurFile = file__1
                Debug.Print("Copying: " & file__1)
                File.Copy(file__1, Path.Combine(targetDir, Path.GetFileName(file__1)), True)
            Next
            Return True
        Catch
            Return False
        End Try
    End Function


End Class
