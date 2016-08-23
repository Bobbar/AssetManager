Imports MySql.Data.MySqlClient
Public Class frmNotes
    Private NoteRequest As Request_Info
    Private Sub ClearAll()
        rtbNotes.Clear()
    End Sub
    Private Sub frmNotes_Load(sender As Object, e As EventArgs) Handles Me.Load

        'ClearAll()
    End Sub
    Public Sub LoadNote(Request As Request_Info)
        NoteRequest = Request
        Me.Show()
    End Sub
    Private Function AddNewNote(RequestUID As String, Note As String) As Boolean
        Dim strNoteUID As String = Guid.NewGuid.ToString
        Try
            Dim strAddNoteQry As String = "INSERT INTO `asset_manager`.`sibi_notes`
(`sibi_request_uid`,
`sibi_note_uid`,
`sibi_note`)
VALUES
(@sibi_request_uid,
@sibi_note_uid,
@sibi_note)"
            Dim cmd As MySqlCommand = MySQLDB.Return_SQLCommand(strAddNoteQry)
            cmd.Parameters.AddWithValue("@sibi_request_uid", RequestUID)
            cmd.Parameters.AddWithValue("@sibi_note_uid", strNoteUID)
            cmd.Parameters.AddWithValue("@sibi_note", Note)
            If cmd.ExecuteNonQuery() > 0 Then
                Return True
            Else
                Return False
            End If
        Catch ex As Exception
            If ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name) Then
                Return False
            End If
        End Try
    End Function
    Public Sub ViewNote(NoteUID As String)
        cmdOK.Visible = False
        rtbNotes.Clear()
        rtbNotes.Text = MySQLDB.Get_SQLValue("sibi_notes", "sibi_note_uid", NoteUID, "sibi_note")
        rtbNotes.ReadOnly = True
        Me.Show()
    End Sub
    Private Sub cmdOK_Click(sender As Object, e As EventArgs) Handles cmdOK.Click
        If AddNewNote(NoteRequest.strUID, Trim(rtbNotes.Text)) Then
            'MsgBox("Success!")
            Me.Dispose()
            frmManageRequest.OpenRequest(NoteRequest.strUID)
        Else
            MsgBox("Failed!")
        End If
    End Sub
    Private Sub cmdClose_Click(sender As Object, e As EventArgs) Handles cmdClose.Click
        Me.Dispose()
    End Sub
    Private Sub rtbNotes_LinkClicked(sender As Object, e As LinkClickedEventArgs) Handles rtbNotes.LinkClicked
        Process.Start(e.LinkText)
    End Sub
End Class