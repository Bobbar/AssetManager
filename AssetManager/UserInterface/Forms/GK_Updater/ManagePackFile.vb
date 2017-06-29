Imports System.ComponentModel
Imports System.IO
Imports System.Threading
Public Class ManagePackFile
    Public Progress As ProgressCounter
    Public Status As String = ""
    Sub New()
        Progress = New ProgressCounter
    End Sub
    Public Async Function DownloadPack() As Task(Of Boolean)
        If Not Directory.Exists(GKPackFileFDir) Then
            Directory.CreateDirectory(GKPackFileFDir)
        End If

        If Directory.Exists(GKExtractDir) Then
            Directory.Delete(GKExtractDir, True)
        End If

        If File.Exists(GKPackFileFullPath) Then
            File.Delete(GKPackFileFullPath)
        End If
        Progress = New ProgressCounter
        Return Await CopyPackFile(GKRemotePackFilePath, GKPackFileFullPath)
    End Function

    Public Async Function VerifyPackFile() As Task(Of Boolean)
        Try
            If Not Directory.Exists(GKPackFileFDir) Then
                Return False
            End If
            If Not Directory.Exists(GKExtractDir) Then
                Return False
            End If
            If Not File.Exists(GKPackFileFullPath) Then
                Return False
            Else

                Dim LocalHash = Await Task.Run(Function()
                                                   Return GetHashOfFile(GKPackFileFullPath)
                                               End Function)

                Dim RemoteHash = Await Task.Run(Function()
                                                    Return GetRemoteHash()
                                                End Function)

                If LocalHash = RemoteHash Then
                    Return True
                Else
                    Return False
                End If
            End If
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function
    Private Function GetRemoteHash() As String
        Using sr As New StreamReader(GKRemotePackFileDir & GKPackHashName)
            Return sr.ReadToEnd
        End Using
    End Function
    Public Async Function CopyPackFile(Source As String, Dest As String) As Task(Of Boolean)
        If File.Exists(Dest) Then
            File.Delete(Dest)
        End If
        Return Await Task.Run(Function()
                                  Try
                                      CopyFile(Source, Dest)
                                      Return True
                                  Catch ex As Exception
                                      Return False
                                  End Try
                              End Function)
    End Function
    Private Sub CopyFile(Source As String, Dest As String)
        Dim BufferSize As Integer = 256000
        Dim perc As Integer = 0
        Dim buffer(BufferSize - 1) As Byte
        Dim bytesIn As Integer = 1
        Dim totalBytesIn As Integer
        Dim CurrentFile As New FileInfo(Source)
        Progress.ResetProgress()
        Using fStream As System.IO.FileStream = CurrentFile.OpenRead(),
                destFile As System.IO.FileStream = New FileStream(Dest, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, BufferSize, FileOptions.WriteThrough)
            totalBytesIn = 0
            Progress.BytesToTransfer = CInt(fStream.Length)
            Dim flLength As Long = fStream.Length
            Do Until bytesIn < 1
                bytesIn = fStream.Read(buffer, 0, BufferSize)
                If bytesIn > 0 Then
                    destFile.Write(buffer, 0, bytesIn)
                    Progress.BytesMoved = bytesIn
                End If
            Loop
            fStream.Close()
        End Using
    End Sub

    Public Async Function PackGKDir() As Task(Of Boolean)
        Try
            Progress = New ProgressCounter
            Dim CompDir As New GZipCompress(Progress)
            If Not Directory.Exists(GKPackFileFDir) Then
                Directory.CreateDirectory(GKPackFileFDir)
            End If

            If File.Exists(GKPackFileFullPath) Then
                File.Delete(GKPackFileFullPath)
            End If
            Dim Errs As Exception
            Dim PackTaskOK = Await Task.Run(Function()
                                                Try
                                                    CompDir.CompressDirectory("C:\PSi\Gatekeeper", GKPackFileFullPath)
                                                    Return True
                                                Catch ex As Exception
                                                    Errs = ex
                                                    Return False
                                                End Try
                                            End Function)
            If Errs IsNot Nothing Then
                ErrHandle(Errs, System.Reflection.MethodInfo.GetCurrentMethod())
            End If
            Return PackTaskOK
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

    Private Async Function UnPackGKDir() As Task(Of Boolean)
        Try
            Status = "Unpacking...."
            Dim Errs As Exception
            Progress = New ProgressCounter
            Dim CompDir As New GZipCompress(Progress)
            Await Task.Run(Sub()
                               Try
                                   CompDir.DecompressToDirectory(GKPackFileFullPath, GKExtractDir)
                               Catch ex As Exception
                                   Errs = ex
                               End Try
                           End Sub)
            If Errs Is Nothing Then
                Return True
            Else
                Return False
            End If
        Catch
            Return False
        End Try
    End Function
    Private Async Function UploadPackFiles() As Task(Of Boolean)
        Dim Done As Boolean = False
        Status = "Uploading Pack File..."
        Progress = New ProgressCounter
        Done = Await CopyPackFile(GKPackFileFullPath, GKRemotePackFilePath)

        Status = "Uploading Hash File..."
        Progress = New ProgressCounter
        Done = Await CopyPackFile(GKPackFileFDir & GKPackHashName, GKRemotePackFileDir & GKPackHashName)
        Return Done
    End Function

    Public Async Function ProcessPackFile() As Task(Of Boolean)
        Dim PackFileOK As Boolean = False
        Status = "Verifying Pack File..."
        If Await VerifyPackFile() Then
            PackFileOK = Await UnPackGKDir()
        Else
            Status = "Downloading Pack File..."
            If Await DownloadPack() Then
                PackFileOK = Await UnPackGKDir()
            End If
        End If
        If PackFileOK Then
            Status = "Done."
            Await Task.Run(Sub()
                               Thread.Sleep(1000)
                           End Sub)
            Return True
        Else
            Status = "ERROR!"
            Return False
        End If

    End Function

    Public Async Function CreateNewPackFile() As Task(Of Boolean)
        Try
            Dim OK As Boolean = False
            Status = "Creating Pack File..."
            Progress = New ProgressCounter
            OK = Await PackGKDir()

            Status = "Generating Hash..."
            OK = Await CreateHashFile()

            OK = Await UploadPackFiles()

            If OK Then
                Status = "Done."
            Else
                Status = "Something went wrong..."
            End If
            Return OK
        Catch ex As Exception
            Status = "ERROR!"
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

    Private Async Function CreateHashFile() As Task(Of Boolean)
        If File.Exists(GKPackFileFDir & GKPackHashName) Then
            File.Delete(GKPackFileFDir & GKPackHashName)
        End If
        Dim Hash = Await Task.Run(Function()
                                      Return GetHashOfFile(GKPackFileFullPath)
                                  End Function)
        Using sw As StreamWriter = File.CreateText(GKPackFileFDir & GKPackHashName)
            sw.Write(Hash)
        End Using
        Return True
    End Function
End Class
