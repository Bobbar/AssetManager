Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class SibiNotesForm
    Private MyRequest As Request_Info
    Public ReadOnly Property Request As Request_Info
        Get
            Return MyRequest
        End Get
    End Property
    Sub New(ParentForm As Form, Request As Request_Info)
        InitializeComponent()
        Tag = ParentForm
        Icon = ParentForm.Icon
        MyRequest = Request
        ShowDialog(ParentForm)
    End Sub
    Sub New(ParentForm As Form, NoteUID As String)
        InitializeComponent()
        Tag = ParentForm
        Icon = ParentForm.Icon
        ViewNote(NoteUID)
    End Sub
    Private Sub ClearAll()
        rtbNotes.Clear()
    End Sub
    Private Sub ViewNote(NoteUID As String)
        cmdOK.Visible = False
        rtbNotes.Clear()
        Dim NoteText As String = AssetFunc.Get_SQLValue(sibi_notes.TableName, sibi_notes.Note_UID, NoteUID, sibi_notes.Note)
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