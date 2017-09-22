Imports System.Management.Automation
Imports System.Collections.ObjectModel
Imports System.Management.Automation.Runspaces
Imports System.Text
Imports System.IO
Imports System.Net
Imports System.ComponentModel


Public Class PowerShellWrapper
    ''' <summary>
    ''' Execute the specified PowerShell script on the specified host.
    ''' </summary>
    ''' <param name="hostname">Hostname of the remote computer.</param>
    ''' <param name="scriptBytes">PowerShell script as a byte array.</param>
    ''' <returns>Returns any error messages.</returns>
    Public Function ExecuteRemotePSScript(hostname As String, scriptBytes As Byte(), credentials As NetworkCredential) As String
        Try
            Dim psCreds = New PSCredential(credentials.UserName, credentials.SecurePassword)
            Dim scriptText = LoadScript(scriptBytes)
            Dim shellUri = "http://schemas.microsoft.com/powershell/Microsoft.PowerShell"
            Dim connInfo As WSManConnectionInfo = New WSManConnectionInfo(False, hostname, 5985, "/wsman", shellUri, psCreds)

            Using remoteRunSpace As Runspace = RunspaceFactory.CreateRunspace(connInfo)
                remoteRunSpace.Open()
                Using MyPipline As Pipeline = remoteRunSpace.CreatePipeline
                    MyPipline.Commands.AddScript(scriptText)
                    MyPipline.Commands.Add("Out-String")

                    Dim results As Collection(Of PSObject) = MyPipline.Invoke
                    Dim stringBuilder As New StringBuilder

                    For Each obj In results
                        stringBuilder.AppendLine(obj.ToString())
                    Next

                    Return CleanDBValue((stringBuilder.ToString)).ToString

                End Using

            End Using
        Catch ex As Exception
            'Check for incorrect username/password error and rethrow a Win32Exception to be caught in the error handler.
            'Makes sure that the global admin credentials variable is cleared so that a new prompt will be shown on the next attempt. See: VerifyAdminCreds method.
            If TypeOf ex Is Remoting.PSRemotingTransportException Then
                Dim transportEx = DirectCast(ex, Remoting.PSRemotingTransportException)
                If transportEx.ErrorCode = 1326 Then
                    Throw New Win32Exception(1326)
                End If
            End If
            Return ex.Message
        End Try
    End Function

    Private Function LoadScript(scriptBytes As Byte()) As String

        Try

            ' Create an instance of StreamReader to read from our file. 
            ' The using statement also closes the StreamReader. 
            ' Dim sr As New StreamReader(filename)
            Using sr As New StreamReader(New MemoryStream(scriptBytes), Encoding.ASCII)
                ' use a string builder to get all our lines from the file 
                Dim fileContents As New StringBuilder()

                ' string to hold the current line 
                Dim curLine As String = ""

                ' loop through our file and read each line into our 
                ' stringbuilder as we go along 
                Do
                    ' read each line and MAKE SURE YOU ADD BACK THE 
                    ' LINEFEED THAT IT THE ReadLine() METHOD STRIPS OFF 
                    curLine = sr.ReadLine()
                    fileContents.Append(curLine + vbCrLf)
                Loop Until curLine Is Nothing

                ' close our reader now that we are done 
                sr.Close()

                ' call RunScript and pass in our file contents 
                ' converted to a string 
                Return fileContents.ToString()
            End Using

        Catch e As Exception
            ' Let the user know what went wrong. 
            Dim errorText As String = "The file could not be read:"
            errorText += e.Message + "\n"
            Return errorText
        End Try

    End Function


End Class
