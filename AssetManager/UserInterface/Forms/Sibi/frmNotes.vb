Imports System.ComponentModel
Imports MySql.Data.MySqlClient
Public Class frmNotes
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
    Sub New(NoteUID As String)
        InitializeComponent()
        ViewNote(NoteUID)
    End Sub
    Private Sub ClearAll()
        rtbNotes.Clear()
    End Sub
    Private Sub ViewNote(NoteUID As String)
        cmdOK.Visible = False
        rtbNotes.Clear()
        rtbNotes.Text = Asset.Get_SQLValue(sibi_notes.TableName, sibi_notes.Note_UID, NoteUID, sibi_notes.Note)
        rtbNotes.ReadOnly = True
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
End Class