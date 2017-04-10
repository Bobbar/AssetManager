Imports System.ComponentModel
Imports System.Net
Public Class Get_Credentials
    Private Sub Accept()
        Dim Username, Password As String
        Username = Trim(txtUsername.Text)
        Password = Trim(txtPassword.Text)
        If Username <> "" And Password <> "" Then
            AdminCreds = New NetworkCredential(Username, Password, Environment.UserDomainName)
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