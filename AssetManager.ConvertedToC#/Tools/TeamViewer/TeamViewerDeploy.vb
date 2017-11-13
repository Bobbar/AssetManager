Imports System.ComponentModel
Imports System.IO
Imports System.Management.Automation.Runspaces

Public Class TeamViewerDeploy

#Region "Fields"

    Private Const DeploymentFilesDirectory As String = "\\core.co.fairfield.oh.us\dfs1\fcdd\files\Information Technology\Software\Tools\TeamViewer\Deploy"
    Private Const DeployTempDirectory As String = "\Temp\TVDeploy"
    Private CancelOperation As Boolean = False
    Private Finished As Boolean = False
    Private LastActivity As Long
    Private LogView As ExtendedForm
    Private ParentForm As ExtendedForm
    Private PSWrapper As New PowerShellWrapper
    Private RTBLog As RichTextBox
    Private TimeoutSeconds As Integer = 120
    Private WatchDogTask As New Task(Sub() WatchDog())
#End Region

#Region "Delegates"

    Delegate Sub RTBLogDelegate(message As String)

#End Region

#Region "Methods"

    Public Async Function DeployToDevice(parentForm As ExtendedForm, targetDevice As DeviceObject) As Task(Of Boolean)
        Try
            If targetDevice IsNot Nothing AndAlso targetDevice.HostName <> "" Then
                ActivityTick()
                WatchDogTask.Start()
                InitLogWindow(parentForm)

                DepLog("Starting new TeamViewer deployment to " & targetDevice.HostName)
                DepLog("-------------------")
                Using PushForm As New CopyFilesForm(parentForm, targetDevice, DeploymentFilesDirectory, DeployTempDirectory)
                    Dim TVExists As Boolean = False

                    DepLog("Pushing files to target computer...")
                    ActivityTick()
                    If Await PushForm.StartCopy Then
                        DepLog("Push successful!")
                        PushForm.Dispose()
                    Else
                        Finished = True
                        DepLog("Push failed!")
                        Message("Error occurred while pushing deployment files to device!")
                        Return False
                    End If

                    ActivityTick()
                    DepLog("Checking for previous installation...")
                    TVExists = Await TeamViewerInstalled(targetDevice)
                    If TVExists Then
                        DepLog("TeamViewer already installed.")
                    Else
                        DepLog("TeamViewer not installed.")
                    End If

                    ActivityTick()
                    If TVExists Then
                        DepLog("Reinstalling TeamViewer...")

                        If Await PSWrapper.InvokePowerShellCommand(targetDevice.HostName, GetTVReinstallCommand) Then
                            DepLog("Deployment complete!")
                        Else
                            Finished = True
                            DepLog("Deployment failed!")
                            Message("Error occurred while executing deployment command!")
                            Return False
                        End If

                    Else
                        DepLog("Starting TeamViewer deployment...")
                        If Await PSWrapper.InvokePowerShellCommand(targetDevice.HostName, GetTVInstallCommand) Then
                            DepLog("Deployment complete!")
                        Else
                            Finished = True
                            DepLog("Deployment failed!")
                            Message("Error occurred while executing deployment command!")
                            Return False
                        End If
                    End If

                    DepLog("Waiting 10 seconds.")
                    For i = 10 To 1 Step -1
                        ActivityTick()
                        Await Task.Delay(1000)
                        DepLog(i & "...")
                    Next

                    ActivityTick()
                    DepLog("Starting TeamViewer assignment...")
                    If Await PSWrapper.InvokePowerShellCommand(targetDevice.HostName, GetTVAssignCommand) Then
                        DepLog("Assignment complete!")
                    Else
                        Finished = True
                        DepLog("Assignment failed!")
                        Message("Error occurred while executing assignment command!")
                        Return False
                    End If

                    ActivityTick()
                    DepLog("Deleting temp files...")
                    Dim ClientPath As String = "\\" & targetDevice.HostName & "\c$"
                    Using NetCon As New NetworkConnection(ClientPath, SecurityTools.AdminCreds)
                        Directory.Delete(ClientPath & DeployTempDirectory, True)
                    End Using
                    Finished = True
                    DepLog("Done.")

                End Using
                DepLog("-------------------")
                DepLog("TeamView deployment is complete!")
                DepLog("NOTE: The target computer may need rebooted or the user may need to open the application before TeamViewer will connect.")
                Return True
            End If
            Finished = True
            Message("The target device is null or does not have a hostname.", vbOKOnly + vbInformation, "Missing Info", parentForm)
            Return False
        Catch ex As Exception
            Return False
        Finally
            Finished = True
        End Try
    End Function

    Private Sub DepLog(message As String)
        If RTBLog.InvokeRequired Then
            Dim d As New RTBLogDelegate(AddressOf DepLog)
            RTBLog.Invoke(d, message)
        Else
            RTBLog.AppendText(Now.ToString & ": " & message & vbCrLf)
        End If
    End Sub

    Private Function GetTVAssignCommand() As Command
        Dim ApiToken As String = AssetFunc.GetTVApiToken
        Dim cmd = New Command("Start-Process", False, True)
        cmd.Parameters.Add("FilePath", "C:\Temp\TVDeploy\Assignment\TeamViewer_Assignment.exe")
        cmd.Parameters.Add("ArgumentList", "-apitoken " & ApiToken & " -datafile ${ProgramFiles}\TeamViewer\AssignmentData.json")
        cmd.Parameters.Add("Wait")
        cmd.Parameters.Add("NoNewWindow")
        Return cmd
    End Function

    Private Function GetTVReinstallCommand() As Command
        Dim cmd = New Command("Start-Process", False, True)
        cmd.Parameters.Add("FilePath", "msiexec.exe")
        cmd.Parameters.Add("ArgumentList", "/i C:\Temp\TVDeploy\TeamViewer_Host-idcjnfzfgb.msi REINSTALL=ALL REINSTALLMODE=omus /qn")
        cmd.Parameters.Add("Wait")
        cmd.Parameters.Add("NoNewWindow")
        Return cmd
    End Function

    Private Function GetTVInstallCommand() As Command
        Dim cmd = New Command("Start-Process", False, True)
        cmd.Parameters.Add("FilePath", "msiexec.exe")
        cmd.Parameters.Add("ArgumentList", "/i C:\Temp\TVDeploy\TeamViewer_Host-idcjnfzfgb.msi /qn")
        cmd.Parameters.Add("Wait")
        cmd.Parameters.Add("NoNewWindow")
        Return cmd
    End Function

    Private Async Function TeamViewerInstalled(targetDevice As DeviceObject) As Task(Of Boolean)
        Try
            Dim resultString = Await Task.Run(Function()
                                                  Return PSWrapper.ExecuteRemotePSScript(targetDevice.HostName, My.Resources.CheckForTVRegistryValue, SecurityTools.AdminCreds)
                                              End Function)
            Dim result = Convert.ToBoolean(resultString)
            Return result
        Catch ex As FormatException
            Return False
        End Try
    End Function

    Private Sub InitLogWindow(parentForm As ExtendedForm)
        Me.ParentForm = parentForm
        LogView = New ExtendedForm(parentForm)
        AddHandler LogView.FormClosing, AddressOf LogClosed
        LogView.Text = "Deployment Log (Close to cancel)"
        LogView.Width = 500
        LogView.Owner = parentForm
        RTBLog = New RichTextBox
        RTBLog.Dock = DockStyle.Fill
        RTBLog.Font = GridFont
        LogView.Controls.Add(RTBLog)
        LogView.Show()
    End Sub

    Private Sub LogClosed(sender As Object, e As CancelEventArgs)
        If Not Finished Then
            If Not CancelOperation Then
                If Message("Cancel the current operation?", vbYesNo + vbQuestion, "Cancel?", ParentForm) = MsgBoxResult.Yes Then
                    CancelOperation = True
                    PSWrapper.StopPowerShellCommand()
                    PSWrapper.StopPiplineCommand()
                End If
                e.Cancel = True
            Else
                If Finished Then
                    e.Cancel = False
                Else
                    If SecondsSinceLastActivity() > TimeoutSeconds Then
                        PSWrapper.StopPowerShellCommand()
                        PSWrapper.StopPiplineCommand()
                    End If

                    e.Cancel = True
                End If
            End If
        Else
            e.Cancel = False
        End If
    End Sub

    Private Sub ActivityTick()
        LastActivity = Now.Ticks
        If CancelOperation Then
            DepLog("The deployment has been canceled!")
            Throw New DeploymentCanceledException
        End If
    End Sub

    Private Function SecondsSinceLastActivity() As Integer
        Return CInt(((Now.Ticks - LastActivity) / 10000) / 1000)
    End Function

    Private Sub WatchDog()
        Dim TimeoutMessageSent As Boolean = False
        Dim CancelMessageSent As Boolean = False
        Do While Not Finished
            If SecondsSinceLastActivity() > TimeoutSeconds Then
                If Not TimeoutMessageSent Then
                    DepLog("The operation is taking a long time...")
                    TimeoutMessageSent = True
                End If
            Else
                TimeoutMessageSent = False
            End If
            If CancelOperation And Not CancelMessageSent Then
                DepLog("Cancelling the operation...")
                CancelMessageSent = True
            End If
            Task.Delay(1000).Wait()
        Loop
    End Sub

#End Region
    Private Class DeploymentCanceledException
        Inherits Exception

        Public Sub New()
        End Sub

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(message As String, inner As Exception)
            MyBase.New(message, inner)
        End Sub

    End Class


End Class