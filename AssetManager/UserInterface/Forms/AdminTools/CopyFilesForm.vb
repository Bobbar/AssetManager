Public Class CopyFilesForm
    Private TargetDevice As DeviceObject
    Private SourceDirectory As String
    Private TargetDirectory As String
    Private PushFilesControl As GKProgressControl
    Private Cancel As Boolean = False
    Sub New(parentForm As ExtendedForm, targetDevice As DeviceObject, sourceDirectory As String, targetDirectory As String)
        InitializeComponent()
        ImageCaching.CacheControlImages(Me)
        Me.ParentForm = parentForm
        Me.TargetDevice = targetDevice
        Me.SourceDirectory = sourceDirectory
        Me.TargetDirectory = targetDirectory
        PushFilesControl = New GKProgressControl(Me, targetDevice, True, sourceDirectory, targetDirectory)
        Me.Controls.Add(PushFilesControl)

        Me.Show()

    End Sub

    Public Async Function StartCopy() As Task(Of Boolean)
        Try
            PushFilesControl.StartUpdate()

            Dim Done = Await Task.Run(Function()
                                          Do Until PushFilesControl.ProgStatus <> GKProgressControl.ProgressStatus.Running And PushFilesControl.ProgStatus <> GKProgressControl.ProgressStatus.Starting
                                              If Cancel Then Return False
                                              Task.Delay(1000).Wait()
                                          Loop
                                          If PushFilesControl.ProgStatus <> GKProgressControl.ProgressStatus.CompleteWithErrors And PushFilesControl.ProgStatus <> GKProgressControl.ProgressStatus.Complete Then
                                              Return False
                                          Else
                                              Return True
                                          End If
                                      End Function)
            Return Done
        Catch ex As Exception
            Debug.Print(ex.Message)
            Return False
        End Try

    End Function

    Private Sub CopyFilesForm_Disposed(sender As Object, e As EventArgs) Handles Me.Disposed
        If PushFilesControl IsNot Nothing Then PushFilesControl.Dispose()
        Cancel = True
    End Sub
End Class