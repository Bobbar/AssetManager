Public Class UpdateDev
    Public strNewNote As String
    Private Sub cmbUpdate_ChangeType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbUpdate_ChangeType.SelectedIndexChanged
    End Sub
    Private Sub cmdSubmit_Click(sender As Object, e As EventArgs) Handles cmdSubmit.Click
        If Not CheckFields() Then
            Dim blah = MsgBox("Please select a change type.", vbOKOnly + vbExclamation, "Missing Field")
            Exit Sub
        End If
        strNewNote = Trim(txtUpdate_Note.Text)

        txtUpdate_Note.Text = ""
        Me.Hide()

    End Sub
    Private Function CheckFields() As Boolean
        If cmbUpdate_ChangeType.SelectedIndex = -1 Then
            Return False
        Else
            Return True
        End If
    End Function
End Class