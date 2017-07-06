Imports System.Net

Public Class GetCredentialsForm
    Private MyCreds As NetworkCredential

    Public ReadOnly Property Credentials As NetworkCredential
        Get
            Return MyCreds
        End Get
    End Property

    Private Sub Accept()
        Dim Username, Password As String
        Username = Trim(txtUsername.Text)
        Password = Trim(txtPassword.Text)
        If Username <> "" And Password <> "" Then
            MyCreds = New NetworkCredential(Username, Password, Environment.UserDomainName)
            DialogResult = DialogResult.OK
        Else
            Message("Username or Password incomplete.", vbExclamation + vbOKOnly, "Missing Info", Me)
        End If
    End Sub

    Private Sub cmdAccept_Click(sender As Object, e As EventArgs) Handles cmdAccept.Click
        Accept()
    End Sub

    Private Sub txtPassword_KeyDown(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyDown
        If e.KeyCode = Keys.Enter Then
            Accept()
        End If
    End Sub

End Class