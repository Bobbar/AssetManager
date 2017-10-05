Imports System.Net
Imports System.Security

Public Class GetCredentialsForm
    Private MyCreds As NetworkCredential
    Private SecurePwd As New SecureString
    Public ReadOnly Property Credentials As NetworkCredential
        Get
            Return MyCreds
        End Get
    End Property
    Sub New()
        InitializeComponent()
    End Sub

    Sub New(credentialDescription As String)
        InitializeComponent()
        CredDescriptionLabel.Text = credentialDescription
    End Sub

    Private Sub Accept()
        Dim Username As String
        Username = Trim(txtUsername.Text)
        SecurePwd.MakeReadOnly()
        If Username <> "" And SecurePwd.Length > 0 Then
            MyCreds = New NetworkCredential(Username, SecurePwd, NetworkInfo.CurrentDomain)
            SecurePwd.Dispose()
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
        If e.KeyCode = Keys.Back Then
            If SecurePwd.Length > 0 Then
                SecurePwd.RemoveAt(SecurePwd.Length - 1)
            End If
        End If
    End Sub

    Private Sub txtPassword_KeyPress(sender As Object, e As KeyPressEventArgs) Handles txtPassword.KeyPress
        If e.KeyChar <> Convert.ToChar(Keys.Back) And e.KeyChar <> Convert.ToChar(Keys.Enter) Then
            SecurePwd.AppendChar(e.KeyChar)
        End If
    End Sub

    Private Sub txtPassword_KeyUp(sender As Object, e As KeyEventArgs) Handles txtPassword.KeyUp
        SetText()
    End Sub

    Private Sub SetText()
        txtPassword.Text = BlankText(SecurePwd.Length)
        txtPassword.SelectionStart = txtPassword.Text.Length + 1
    End Sub

    Private Function BlankText(length As Integer) As String
        If length > 0 Then
            Return New String(Char.Parse("*"), length)
        End If
        Return String.Empty
    End Function

End Class