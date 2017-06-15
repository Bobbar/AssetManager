Option Explicit On
Imports System.Environment
Imports System.IO
Imports System.Runtime.InteropServices
Imports MyDialogLib
Module OtherFunctions
    Public stpw As New Stopwatch
    Private intTimerHits As Integer = 0
    Public Sub StartTimer()
        stpw.Stop()
        stpw.Reset()
        stpw.Start()
    End Sub
    Public Function StopTimer() As String
        stpw.Stop()
        intTimerHits += 1
        Dim Results As String = intTimerHits & "  Stopwatch: MS:" & stpw.ElapsedMilliseconds & " Ticks: " & stpw.ElapsedTicks
        Debug.Print(Results)
        Return Results
    End Function
    Public Sub Logger(Message As String)
        Dim MaxLogSizeKiloBytes As Short = 500
        Dim DateStamp As String = DateTime.Now.ToString
        Dim infoReader As FileInfo
        infoReader = My.Computer.FileSystem.GetFileInfo(strLogPath)
        If Not File.Exists(strLogPath) Then
            Dim di As DirectoryInfo = Directory.CreateDirectory(strLogDir)
            Using sw As StreamWriter = File.CreateText(strLogPath)
                sw.WriteLine(DateStamp & ": Log Created...")
                sw.WriteLine(DateStamp & ": " & Message)
            End Using
        Else
            If (infoReader.Length / 1000) < MaxLogSizeKiloBytes Then
                Using sw As StreamWriter = File.AppendText(strLogPath)
                    sw.WriteLine(DateStamp & ": " & Message)
                End Using
            Else
                If RotateLogs() Then
                    Using sw As StreamWriter = File.AppendText(strLogPath)
                        sw.WriteLine(DateStamp & ": " & Message)
                    End Using
                End If
            End If
        End If
    End Sub
    Private Function RotateLogs() As Boolean
        Try
            File.Copy(strLogPath, strLogPath + ".old", True)
            File.Delete(strLogPath)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
    Public Function GetColIndex(ByVal Grid As DataGridView, ByVal strColName As String) As Integer
        Try
            Return Grid.Columns.Item(strColName).Index
        Catch ex As Exception
            Return -1
        End Try
    End Function
    Public Function GetCellValue(ByVal Grid As DataGridView, ColumnName As String) As String
        Return NoNull(Grid.Item(GetColIndex(Grid, ColumnName), Grid.CurrentRow.Index).Value.ToString)
    End Function
    Public Sub EndProgram()
        If OKToEnd() Then
            ProgramEnding = True
            Logger("Ending Program...")
            PurgeTempDir()
            Application.Exit()
        End If
    End Sub
    Public Sub PurgeTempDir()
        Try
            Directory.Delete(DownloadPath, True)
        Catch
        End Try
    End Sub
    Public Sub AdjustComboBoxWidth(ByVal sender As Object, ByVal e As EventArgs)
        Dim senderComboBox = DirectCast(sender, ComboBox)
        Dim width As Integer = senderComboBox.DropDownWidth
        Dim g As Graphics = senderComboBox.CreateGraphics()
        Dim font As Font = senderComboBox.Font
        Dim vertScrollBarWidth As Integer = If((senderComboBox.Items.Count > senderComboBox.MaxDropDownItems), SystemInformation.VerticalScrollBarWidth, 0)
        Dim newWidth As Integer
        For Each s As String In DirectCast(sender, ComboBox).Items
            newWidth = CInt(g.MeasureString(s, font).Width) + vertScrollBarWidth
            If width < newWidth Then
                width = newWidth
            End If
        Next
        senderComboBox.DropDownWidth = width
    End Sub
    Public Function NotePreview(Note As String, Optional CharLimit As Integer = 50) As String
        If Note <> "" Then
            Return Strings.Left(Note, CharLimit) & IIf(Len(Note) > CharLimit, "...", "").ToString
        Else
            Return ""
        End If
    End Function
    Public Function Message(ByVal Prompt As String, Optional ByVal Buttons As Integer = vbOKOnly + vbInformation, Optional ByVal Title As String = Nothing, Optional ByVal ParentFrm As Form = Nothing) As MsgBoxResult
        Dim NewMessage As New MyDialog(ParentFrm)
        Return NewMessage.DialogMessage(Prompt, Buttons, Title, ParentFrm)
    End Function
    Public Function OKToEnd() As Boolean
        If CheckForActiveTransfers() Then Return False
        If GKUpdaterForm.UpdatesRunning() Then Return False
        Return True
    End Function
    Public Function CheckForActiveTransfers() As Boolean
        Dim ActiveTransfers As New List(Of AttachmentsForm)
        For Each frm As Form In My.Application.OpenForms
            If TypeOf frm Is AttachmentsForm Then
                Dim Attachments As AttachmentsForm = DirectCast(frm, AttachmentsForm)
                If Attachments.UploadWorker.IsBusy Or Attachments.DownloadWorker.IsBusy Then
                    ActiveTransfers.Add(Attachments)
                End If
            End If
        Next
        If ActiveTransfers.Count > 0 Then
            Dim blah = Message("There are " & ActiveTransfers.Count.ToString & " active uploads/downloads. Do you wish to cancel the current operations?", MessageBoxIcon.Warning + vbYesNo, "Worker Busy")
            If blah = vbYes Then
                For Each AttachForm As AttachmentsForm In ActiveTransfers
                    If AttachForm.UploadWorker.IsBusy Then AttachForm.UploadWorker.CancelAsync()
                    If AttachForm.DownloadWorker.IsBusy Then AttachForm.DownloadWorker.CancelAsync()
                Next
                Return False
            Else
                For Each AttachForm As AttachmentsForm In ActiveTransfers
                    AttachForm.WindowState = FormWindowState.Normal
                    AttachForm.Activate()
                Next
                Return True
            End If
        Else
            Return False
        End If
    End Function
    Public Sub SetWaitCursor(Waiting As Boolean)
        Application.UseWaitCursor = Waiting
        Application.DoEvents()
    End Sub
End Module
