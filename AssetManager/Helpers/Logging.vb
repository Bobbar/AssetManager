Imports System.IO
Module Logging
    Public Sub Logger(Message As String)
        Dim MaxLogSizeKiloBytes As Short = 500
        Dim DateStamp As String = DateTime.Now.ToString
        Dim infoReader As FileInfo
        infoReader = My.Computer.FileSystem.GetFileInfo(Paths.LogPath)
        If Not File.Exists(Paths.LogPath) Then
            Directory.CreateDirectory(Paths.AppDir)
            Using sw As StreamWriter = File.CreateText(Paths.LogPath)
                sw.WriteLine(DateStamp & ": Log Created...")
                sw.WriteLine(DateStamp & ": " & Message)
            End Using
        Else
            If (infoReader.Length / 1000) < MaxLogSizeKiloBytes Then
                Using sw As StreamWriter = File.AppendText(Paths.LogPath)
                    sw.WriteLine(DateStamp & ": " & Message)
                End Using
            Else
                If RotateLogs() Then
                    Using sw As StreamWriter = File.AppendText(Paths.LogPath)
                        sw.WriteLine(DateStamp & ": " & Message)
                    End Using
                End If
            End If
        End If
    End Sub

    Private Function RotateLogs() As Boolean
        Try
            File.Copy(Paths.LogPath, Paths.LogPath + ".old", True)
            File.Delete(Paths.LogPath)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
End Module
