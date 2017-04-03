Imports System.Net
Imports System.IO
Public Class GK_Updater
    Private ReadOnly ServerName As String = "10.10.0.184" '"ddad-svr-fs01"
    Private ReadOnly ShareName As String = "\c$"
    ' Private ReadOnly Domain As String = "core.co.fairfield.oh.us\"
    Sub New(Username As String, Password As String) ', ComputerName As String)





    End Sub

    Private Sub ConnectServerShare()

        ' Dim theNetCache As New CredentialCache()

        Dim theFolders As String() = Directory.GetDirectories("\\" & ServerName & ShareName)




    End Sub




End Class
