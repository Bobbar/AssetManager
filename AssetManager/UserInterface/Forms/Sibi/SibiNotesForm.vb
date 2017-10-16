Public Class SibiNotesForm
    Private MyRequest As RequestObject

    Public ReadOnly Property Request As RequestObject
        Get
            Return MyRequest
        End Get
    End Property

    Sub New(parentForm As ExtendedForm, request As RequestObject)
        InitializeComponent()
        Me.ParentForm = parentForm
        MyRequest = request
        ShowDialog(parentForm)
    End Sub

    Sub New(parentForm As ExtendedForm, noteUID As String)
        InitializeComponent()
        Me.ParentForm = parentForm
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
            SetRichTextBox(rtbNotes, NoteText)
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

End Class