Public Class frmEncrypter
    Private Sub cmdEncode_Click(sender As Object, e As EventArgs) Handles cmdEncode.Click
        Dim CryptKey As String = Trim(txtKey.Text)
        Dim CryptString As String = Trim(txtString.Text)
        Dim wrapper As New Simple3Des(CryptKey)
        txtResult.Text = wrapper.EncryptData(CryptString)
    End Sub
    Private Sub cmdClear_Click(sender As Object, e As EventArgs) Handles cmdClear.Click
        For Each ctl As Control In GroupBox1.Controls
            If TypeOf (ctl) Is TextBox Then
                Dim txt As TextBox = ctl
                txt.Clear()
            End If
        Next
    End Sub
End Class