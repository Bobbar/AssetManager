Imports MySql.Data.MySqlClient
Public Class frmNotes
    Private NoteRequest As Request_Info
    Private CallingForm As frmManageRequest
    Private Sub ClearAll()
        rtbNotes.Clear()
    End Sub
    Private Sub frmNotes_Load(sender As Object, e As EventArgs) Handles Me.Load
        'ClearAll()
    End Sub
    Public Sub LoadNote(Request As Request_Info, Sender As frmManageRequest)
        NoteRequest = Request
        CallingForm = Sender
        Me.Show()
    End Sub
    Private Function AddNewNote(RequestUID As String, Note As String) As Boolean
        Dim strNoteUID As String = Guid.NewGuid.ToString
        Try
            Dim strAddNoteQry As String = "INSERT INTO " & sibi_notes.TableName & "
(" & sibi_notes.Request_UID & ",
" & sibi_notes.Note_UID & ",
" & sibi_notes.Note & ")
VALUES
(@" & sibi_notes.Request_UID & ",
@" & sibi_notes.Note_UID & ",
@" & sibi_notes.Note & ")"
            Dim cmd As MySqlCommand = SQLComms.Return_SQLCommand(strAddNoteQry)
            cmd.Parameters.AddWithValue("@" & sibi_notes.Request_UID, RequestUID)
            cmd.Parameters.AddWithValue("@" & sibi_notes.Note_UID, strNoteUID)
            cmd.Parameters.AddWithValue("@" & sibi_notes.Note, Note)
            If cmd.ExecuteNonQuery() > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            If ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Return False
            End If
        End Try
    End Function
    Public Sub ViewNote(NoteUID As String)
        cmdOK.Visible = False
        rtbNotes.Clear()
        rtbNotes.Text = Asset.Get_SQLValue(sibi_notes.TableName, sibi_notes.Note_UID, NoteUID, sibi_notes.Note)
        rtbNotes.ReadOnly = True
        Me.Show()
    End Sub
    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        If AddNewNote(NoteRequest.strUID, Trim(rtbNotes.Text)) Then
            'Message("Success!")
            Me.Dispose()
            CallingForm.OpenRequest(NoteRequest.strUID)
        Else
            Message("Failed!")
        End If
    End Sub
    Private Sub cmdClose_Click(sender As Object, e As EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub
    Private Sub rtbNotes_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles rtbNotes.LinkClicked
        Process.Start(e.LinkText)
    End Sub
End Class