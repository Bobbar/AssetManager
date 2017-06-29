Imports System.ComponentModel
Imports System.IO
Imports System.Threading
Public Class PackFileForm
    Public Property PackVerified As Boolean = False
    Private Working As Boolean = False
    Private PackFunc As New ManagePackFile
    Sub New(ShowFunctions As Boolean)
        InitializeComponent()
        FunctionPanel.Visible = ShowFunctions
        If Not ShowFunctions Then
            CheckPackFile()
        End If
    End Sub
    Private Async Sub CheckPackFile()
        Working = True
        PackVerified = Await PackFunc.ProcessPackFile()
        Working = False
        If Me.Modal Then Me.Close()
    End Sub
    Private Sub ProgressTimer_Tick(sender As Object, e As EventArgs) Handles ProgressTimer.Tick
        If PackFunc.Progress.Percent > 0 Then
            ProgressBar.Value = PackFunc.Progress.Percent
            PackFunc.Progress.Tick()
            If PackFunc.Progress.Throughput > 0 And PackFunc.Progress.Percent < 100 Then
                If Not SpeedLabel.Visible Then SpeedLabel.Visible = True
                SpeedLabel.Text = PackFunc.Progress.Throughput.ToString & " MB/s"
            Else
                SpeedLabel.Visible = False
            End If
        End If
        StatusLabel.Text = PackFunc.Status
    End Sub
    Private Sub VerifyPackButton_Click(sender As Object, e As EventArgs) Handles VerifyPackButton.Click
        CheckPackFile()
    End Sub
    Private Async Sub NewPackFile()
        Working = True
        If Not Await PackFunc.CreateNewPackFile() Then
            Message("Error while creating a new pack file.", vbOKOnly + vbExclamation, "Error", Me)
        End If
        Working = False
    End Sub
    Private Sub NewPackButton_Click(sender As Object, e As EventArgs) Handles NewPackButton.Click
        NewPackFile()
    End Sub

    Private Sub PackFileForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        e.Cancel = Working
    End Sub
End Class