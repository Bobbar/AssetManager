Imports System.Net
Public Class Get_Credentials
    Private Sub cmdAccept_Click(sender As Object, e As EventArgs) Handles cmdAccept.Click
        Dim Username, Password As String
        Username = Trim(txtUsername.Text)
        Password = Trim(txtPassword.Text)
        If Username <> "" And Password <> "" Then
            AdminCreds = New NetworkCredential(Username, Password, Environment.UserDomainName)
        End If
        Me.Dispose()
    End Sub
End Class