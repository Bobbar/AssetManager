Public Class SibiNotesForm
    Private MyRequest As RequestStruct

    Public ReadOnly Property Request As RequestStruct
        Get
            Return MyRequest
        End Get
    End Property

    Sub New(parentForm As Form, request As RequestStruct)
        InitializeComponent()
        Tag = parentForm
        Icon = parentForm.Icon
        MyRequest = request
        ShowDialog(parentForm)
    End Sub

    Sub New(parentForm As Form, noteUID As String)
        InitializeComponent()
        Tag = parentForm
        Icon = parentForm.Icon
        FormUID = noteUID
        ViewNote(noteUID)
    End Sub

    Private Sub ClearAll()
        rtbNotes.Clear()
    End Sub

    Private Sub ViewNote(noteUID As String)
        Try
            cmdOK.Visible = False
            rtbNotes.Clear()
            Dim NoteText As String = AssetFunc.GetSqlValue(SibiNotesCols.TableName, SibiNotesCols.NoteUID, noteUID, SibiNotesCols.Note)
            Dim NoteTimeStamp As String = AssetFunc.GetSqlValue(SibiNotesCols.TableName, SibiNotesCols.NoteUID, noteUID, SibiNotesCols.DateStamp)
            Me.Text += " - " & NoteTimeStamp
            Select Case GetStringFormat(NoteText)
                Case DataFormats.Rtf
                    rtbNotes.Rtf = NoteText
                Case DataFormats.Text
                    rtbNotes.Text = NoteText
            End Select
            rtbNotes.ReadOnly = True
            rtbNotes.BackColor = Color.White
            Show()
            Activate()
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        DialogResult = DialogResult.OK
        Me.Close()
    End Sub

    Private Sub cmdClose_Click(sender As Object, e As EventArgs) Handles cmdClose.Click
        DialogResult = DialogResult.Abort
        Me.Dispose()
    End Sub

    Private Sub rtbNotes_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles rtbNotes.LinkClicked
        Process.Start(e.LinkText)
    End Sub

    Private Function GetStringFormat(text As String) As String
        If text.StartsWith("{\rtf") Then
            Return DataFormats.Rtf
        Else
            Return DataFormats.Text
        End If
    End Function

End Class