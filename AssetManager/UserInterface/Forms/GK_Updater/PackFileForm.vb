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
        Try
            Working = True
            PackVerified = Await PackFunc.ProcessPackFile()
            Working = False
            If Me.Modal Then Me.Close()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub ProgressTimer_Tick(sender As Object, e As EventArgs) Handles ProgressTimer.Tick
        Try
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
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Sub VerifyPackButton_Click(sender As Object, e As EventArgs) Handles VerifyPackButton.Click
        Try
            CheckPackFile()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Async Sub NewPackFile()
        Try
            Working = True
            If Not Await PackFunc.CreateNewPackFile() Then
                Message("Error while creating a new pack file.", vbOKOnly + vbExclamation, "Error", Me)
            End If
            Working = False
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub
    Private Async Sub NewPackButton_Click(sender As Object, e As EventArgs) Handles NewPackButton.Click
        Try
            NewPackFile()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub PackFileForm_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        e.Cancel = Working
    End Sub
End Class