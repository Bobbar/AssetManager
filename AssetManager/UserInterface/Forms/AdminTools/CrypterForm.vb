Public Class CrypterForm

    Sub New(ParentForm As Form)
        InitializeComponent()
        Tag = ParentForm
        Icon = ParentForm.Icon
        Show()
    End Sub

    Private Sub cmdEncode_Click(sender As Object, e As EventArgs) Handles cmdEncode.Click
        If Trim(txtString.Text) <> "" Then
            Dim CryptKey As String = Trim(txtKey.Text)
            Dim CryptString As String = Trim(txtString.Text)
            Using wrapper As New Simple3Des(CryptKey)
                txtResult.Text = wrapper.EncryptData(CryptString)
            End Using
        ElseIf Trim(txtResult.Text) <> "" Then
            Using wrapper As New Simple3Des(Trim(txtKey.Text))
                txtString.Text = wrapper.DecryptData(Trim(txtResult.Text))
            End Using
        End If
    End Sub

    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        For Each ctl As Control In GroupBox1.Controls
            If TypeOf (ctl) Is TextBox Then
                Dim txt = DirectCast(ctl, TextBox)
                txt.Clear()
            End If
        Next
    End Sub

End Class