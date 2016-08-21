Option Explicit On
Imports System.ComponentModel
Public Class UpdateDev
    Public ReadOnly Property UpdateInfo As Update_Info
        Get
            Return NewUpdateInfo
        End Get
    End Property
    Public strNewNote As String
    Private CurrentForm As Form
    Private NewUpdateInfo As Update_Info
    Public Sub FinishUpdate(SendingForm As Form)
        CurrentForm = SendingForm
    End Sub
    Private Sub cmdSubmit_Click(sender As Object, e As EventArgs) Handles cmdSubmit.Click
        If Not CheckFields() Then
            Dim blah = MsgBox("Please select a change type.", vbOKOnly + vbExclamation, "Missing Field")
            Exit Sub
        End If
        NewUpdateInfo.strNote = Trim(txtUpdate_Note.Text)
        NewUpdateInfo.strChangeType = GetDBValue(ChangeType, cmbUpdate_ChangeType.SelectedIndex)
        txtUpdate_Note.Text = ""
        cmbUpdate_ChangeType.Enabled = True
        Me.DialogResult = DialogResult.OK
    End Sub
    Private Function CheckFields() As Boolean
        If cmbUpdate_ChangeType.SelectedIndex = -1 Then
            Return False
        Else
            Return True
        End If
    End Function
End Class