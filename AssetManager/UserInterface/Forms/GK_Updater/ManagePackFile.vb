Imports System.IO
Imports System.Threading

Public Class ManagePackFile
    Public Progress As ProgressCounter
    Public Status As String = ""

    Sub New()
        Progress = New ProgressCounter
    End Sub

    ''' <summary>
    ''' Creates and cleans the pack file directories then downloads a new pack file from the server location.
    ''' </summary>
    ''' <returns></returns>
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

    ''' <summary>
    ''' Verifies directory structure, checks if pack file is present, then compares local and remote hashes of the pack file.
    '''
    ''' Returns False if directory or file is missing, or if the hashes mismatch.
    ''' </summary>
    ''' <returns></returns>
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

    ''' <summary>
    ''' Returns the contents of the hash text file located in <see cref="GKRemotePackFileDir"/>
    ''' </summary>
    ''' <returns></returns>
    Private Function GetRemoteHash() As String
        Using sr As New StreamReader(GKRemotePackFileDir & GKPackHashName)
            Return sr.ReadToEnd
        End Using
    End Function

    ''' <summary>
    ''' Copies a single file to the <paramref name="dest"/> path.
    ''' </summary>
    ''' <param name="source"></param>
    ''' Path of source file.
    ''' <param name="dest"></param>
    ''' Path of destination.
    ''' <returns></returns>
    Public Async Function CopyPackFile(source As String, dest As String) As Task(Of Boolean)
        If File.Exists(dest) Then
            File.Delete(dest)
        End If
        Return Await Task.Run(Function()
                                  Try
                                      CopyFile(source, dest)
                                      Return True
                                  Catch ex As Exception
                                      Return False
                                  End Try
                              End Function)
    End Function

    ''' <summary>
    ''' Performs a buffered file stream transfer.
    ''' </summary>
    ''' <param name="Source"></param>
    ''' <param name="Dest"></param>
    Private Sub CopyFile(Source As String, Dest As String)
        Dim BufferSize As Integer = 256000
        Dim buffer(BufferSize - 1) As Byte
        Dim bytesIn As Integer = 1
        Dim CurrentFile As New FileInfo(Source)
        Progress.ResetProgress()
        Using fStream As System.IO.FileStream = CurrentFile.OpenRead(),
                destFile As System.IO.FileStream = New FileStream(Dest, FileMode.OpenOrCreate, FileAccess.Write, FileShare.Write, BufferSize, FileOptions.None)
            Progress.BytesToTransfer = CInt(fStream.Length)
            Do Until bytesIn < 1
                bytesIn = fStream.Read(buffer, 0, BufferSize)
                If bytesIn > 0 Then
                    destFile.Write(buffer, 0, bytesIn)
                    Progress.BytesMoved = bytesIn
                End If
            Loop
        End Using
    End Sub

    ''' <summary>
    ''' Compresses the local Gatekeeper directory into a new pack file.
    ''' </summary>
    ''' <returns></returns>
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
            Await Task.Run(Sub()
                               CompDir.CompressDirectory(GKInstallDir, GKPackFileFullPath)
                           End Sub)
            Return True
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Decompresses the pack file into a local working directory.
    ''' </summary>
    ''' <returns></returns>
    Private Async Function UnPackGKDir() As Task(Of Boolean)
        Try
            Status = "Unpacking...."
            Progress = New ProgressCounter
            Dim CompDir As New GZipCompress(Progress)
            Await Task.Run(Sub()
                               CompDir.DecompressToDirectory(GKPackFileFullPath, GKExtractDir)
                           End Sub)
            Return True
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Copies the pack file and hash file to the server directory.
    ''' </summary>
    ''' <returns></returns>
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

    ''' <summary>
    ''' Verifies the local pack file and downloads a new one if needed.
    ''' </summary>
    ''' <returns></returns>
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

    ''' <summary>
    ''' Creates a new pack file and hash file and copies them to the server location.
    ''' </summary>
    ''' <returns></returns>
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

    ''' <summary>
    ''' Creates a text file containing the hash string of the pack file.
    ''' </summary>
    ''' <returns></returns>
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