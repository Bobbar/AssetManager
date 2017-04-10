Imports System.Net
Imports System.IO
Imports System.ComponentModel
Public Class GKProgress
    Private WithEvents MyGKUpdater As GK_Updater
    Private AdmPassword As String
    Private AdmUsername As String
    Private CtlDown As Boolean
    Private CurrentStatus As GK_Updater.Status_Stats
    Private ErrList As New List(Of String)
    Private MyDevice As Device_Info
    Sub New(ParentForm As MyForm, Device As Device_Info)
        ' This call is required by the designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        Tag = ParentForm
        Icon = ParentForm.Icon
        MyDevice = Device
        Text = Text + " - " + MyDevice.strCurrentUser
        Show()
    End Sub
    Private Sub cmdCancel_Click(sender As Object, e As EventArgs) Handles cmdCancel.Click
        MyGKUpdater.CancelUpdate()
    End Sub

    Private Sub cmdGo_Click(sender As Object, e As EventArgs) Handles cmdGo.Click

        txtUsername.ReadOnly = True
        txtPassword.ReadOnly = True
        cmdGo.Enabled = False
        AdmUsername = Trim(txtUsername.Text)
        AdmPassword = Trim(txtPassword.Text)
        Dim creds As New NetworkCredential()
        creds.Password = AdmPassword
        creds.UserName = AdmUsername
        creds.Domain = Environment.UserDomainName
        MyGKUpdater = New GK_Updater(MyDevice, creds)
        GKUpdater_Form.AddUpdate(MyGKUpdater)
        GKUpdater_Form.Show()


        'RunUpdate()
    End Sub
    Private Sub GKLogEvent(sender As Object, e As GK_Updater.LogEvents)
        rtbLog.AppendText(e.LogData.Message + vbCrLf)
        rtbLog.Refresh()
        Logger(e.LogData.Message)
        Invalidate()
    End Sub

    Private Sub GKStatusUpdateEvent(sender As Object, e As GK_Updater.GKUpdateEvents)
        CurrentStatus = e.CurrentStatus
        pbarProgress.Maximum = CurrentStatus.TotFiles
        pbarProgress.Value = CurrentStatus.CurFileIdx
        Dim CurrentFile = CurrentStatus.CurFileName
        lblCurrentFile.Text = CurrentFile
        Invalidate()
    End Sub
    Private Sub GKUpdate_Complete(sender As Object, e As GK_Updater.GKUpdateCompleteEvents)
        If e.Errors Then
            Text = Text + " - *ERRORS!*"
            lblCurrentFile.Text = "ERROR"
        Else
            Text = Text + " - *COMPLETE*"
            lblCurrentFile.Text = "Complete!"
        End If
        MyGKUpdater.Dispose()
    End Sub
    Private Sub rtbLog_KeyDown(sender As Object, e As KeyEventArgs) Handles rtbLog.KeyDown
        If e.Control Then CtlDown = True
    End Sub

    Private Sub rtbLog_KeyUp(sender As Object, e As KeyEventArgs) Handles rtbLog.KeyUp
        CtlDown = False
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

    Private Sub RunUpdate()
        Try
            txtUsername.ReadOnly = True
            txtPassword.ReadOnly = True
            cmdGo.Enabled = False
            AdmUsername = Trim(txtUsername.Text)
            AdmPassword = Trim(txtPassword.Text)
            Dim creds As New NetworkCredential()
            creds.Password = AdmPassword
            creds.UserName = AdmUsername
            creds.Domain = Environment.UserDomainName
            MyGKUpdater = New GK_Updater(MyDevice, creds)
            AddHandler MyGKUpdater.LogEvent, AddressOf GKLogEvent
            AddHandler MyGKUpdater.StatusUpdate, AddressOf GKStatusUpdateEvent
            AddHandler MyGKUpdater.UpdateComplete, AddressOf GKUpdate_Complete
            MyGKUpdater.StartUpdate()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
        End Try
    End Sub

    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyDown
        If e.KeyCode = Keys.Return Then
            RunUpdate()
        End If
    End Sub
End Class