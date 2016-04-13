Public Class UpdateDev
    Private Sub cmbUpdate_ChangeType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbUpdate_ChangeType.SelectedIndexChanged
    End Sub
    Private Sub cmdSubmit_Click(sender As Object, e As EventArgs) Handles cmdSubmit.Click
        Me.Hide()
        View.UpdateDevice()
        txtUpdate_Note.Text = ""
    End Sub
    Private Sub UpdateDev_Load(sender As Object, e As EventArgs) Handles Me.Load
    End Sub
End Class