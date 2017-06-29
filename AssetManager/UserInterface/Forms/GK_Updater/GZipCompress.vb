
Imports System.Text
Imports System.IO
Imports System.IO.Compression
Public Class GZipCompress
    Private Progress As ProgressCounter
    Sub New(ByRef ProgressDel As ProgressCounter)
        Progress = ProgressDel
    End Sub

    Private Sub CompressFile(sDir As String, sRelativePath As String, zipStream As GZipStream)
        'Compress file name
        Dim chars As Char() = sRelativePath.ToCharArray()
        zipStream.Write(BitConverter.GetBytes(chars.Length), 0, 4)
        For Each c As Char In chars
            zipStream.Write(BitConverter.GetBytes(c), 0, 2)
        Next

        'Compress file content
        Dim bytes As Byte() = File.ReadAllBytes(Path.Combine(sDir, sRelativePath))
        zipStream.Write(BitConverter.GetBytes(bytes.Length), 0, 4)
        zipStream.Write(bytes, 0, bytes.Length)
    End Sub


    Private Function DecompressFile(sDir As String, zipStream As GZipStream) As Boolean
        'Decompress file name
        Dim bytes As Byte() = New Byte(4 - 1) {}
        Dim Readed As Integer = zipStream.Read(bytes, 0, 4)
        If Readed < 4 Then
            Return False
        End If

        Dim iNameLen As Integer = BitConverter.ToInt32(bytes, 0)
        bytes = New Byte(2 - 1) {}
        Dim sb As New StringBuilder()
        For i As Integer = 0 To iNameLen - 1
            zipStream.Read(bytes, 0, 2)
            Dim c As Char = BitConverter.ToChar(bytes, 0)
            sb.Append(c)
        Next
        Dim sFileName As String = sb.ToString()

        'Decompress file content
        bytes = New Byte(4 - 1) {}
        zipStream.Read(bytes, 0, 4)
        Dim iFileLen As Integer = BitConverter.ToInt32(bytes, 0)


        bytes = New Byte(iFileLen - 1) {}
        zipStream.Read(bytes, 0, bytes.Length)

        Dim sFilePath As String = Path.Combine(sDir, sFileName)
        Dim sFinalDir As String = Path.GetDirectoryName(sFilePath)
        If Not Directory.Exists(sFinalDir) Then
            Directory.CreateDirectory(sFinalDir)
        End If

        Using outFile As New FileStream(sFilePath, FileMode.Create, FileAccess.Write, FileShare.None, 256000)
            outFile.Write(bytes, 0, iFileLen)
        End Using
        Progress.BytesMoved = iFileLen
        Return True
    End Function

    Public Sub CompressDirectory(sInDir As String, sOutFile As String)
        Dim sFiles As String() = Directory.GetFiles(sInDir, "*.*", SearchOption.AllDirectories)
        Dim iDirLen As Integer = If(sInDir(sInDir.Length - 1) = Path.DirectorySeparatorChar, sInDir.Length, sInDir.Length + 1)
        Progress.ResetProgress()
        Progress.BytesToTransfer = sFiles.Length - 1
        Using outFile As New FileStream(sOutFile, FileMode.Create, FileAccess.Write, FileShare.None, 256000)
            Using str As New GZipStream(outFile, CompressionLevel.Optimal)
                For Each sFilePath As String In sFiles
                    Progress.BytesMoved = 1
                    Dim sRelativePath As String = sFilePath.Substring(iDirLen)
                    CompressFile(sInDir, sRelativePath, str)
                Next
            End Using
        End Using
    End Sub
    Public Sub DecompressToDirectory(sCompressedFile As String, sDir As String)
        Progress.BytesToTransfer = GetGzOriginalFileSize(sCompressedFile)
        Using inFile As New FileStream(sCompressedFile, FileMode.Open, FileAccess.Read, FileShare.None, 256000)
            Using zipStream As New GZipStream(inFile, CompressionMode.Decompress, True)
                While DecompressFile(sDir, zipStream)

                End While
                zipStream.Close()
                inFile.Close()
            End Using
        End Using
    End Sub

    ''' <summary>
    ''' Extracts the original filesize of the compressed file.
    ''' </summary>
    ''' <param name="fi">GZip file to handle</param>
    ''' <returns>Size of the compressed file, when its decompressed.</returns>
    ''' <remarks>More information at <a href="http://tools.ietf.org/html/rfc1952">http://tools.ietf.org/html/rfc1952</a> section 2.3</remarks>
    Public Shared Function GetGzOriginalFileSize(fi As String) As Integer
        Return GetGzOriginalFileSize(New FileInfo(fi))
    End Function
    ''' <summary>
    ''' Extracts the original filesize of the compressed file.
    ''' </summary>
    ''' <param name="fi">GZip file to handle</param>
    ''' <returns>Size of the compressed file, when its decompressed.</returns>
    ''' <remarks>More information at <a href="http://tools.ietf.org/html/rfc1952">http://tools.ietf.org/html/rfc1952</a> section 2.3</remarks>
    Public Shared Function GetGzOriginalFileSize(fi As FileInfo) As Integer
        Try
            Using fs As FileStream = fi.OpenRead()
                Try
                    Dim fh As Byte() = New Byte(2) {}
                    fs.Read(fh, 0, 3)
                    If fh(0) = 31 AndAlso fh(1) = 139 AndAlso fh(2) = 8 Then
                        'If magic numbers are 31 and 139 and the deflation id is 8 then...
                        Dim ba As Byte() = New Byte(3) {}
                        fs.Seek(-4, SeekOrigin.[End])
                        fs.Read(ba, 0, 4)
                        Return BitConverter.ToInt32(ba, 0)
                    Else
                        Return -1
                    End If
                Finally
                    fs.Close()
                End Try
            End Using
        Catch generatedExceptionName As Exception
            Return -1
        End Try
    End Function

End Class


