Imports System.Environment
Imports System
Imports System.IO
Module OtherFunctions
    Public strLogDir As String = GetFolderPath(SpecialFolder.ApplicationData) & "\AssetManager\"
    Public strLogName As String = "log.log"
    Public strLogPath As String = strLogDir & strLogName
    Public colCurrentEntry As Color = ColorTranslator.FromHtml("#7AD1FF") '"#7AD1FF"
    Public colMissingField As Color = ColorTranslator.FromHtml("#75BAFF")
    Public colCheckIn As Color = ColorTranslator.FromHtml("#B6FCC0")
    Public colCheckOut As Color = ColorTranslator.FromHtml("#FCB6B6")
    Public ViewFormIndex As Integer
    Public Sub Logger(Message As String)
        Dim DateStamp As String = DateTime.Now
        If Not File.Exists(strLogPath) Then
            Dim di As DirectoryInfo = Directory.CreateDirectory(strLogDir)
            Using sw As StreamWriter = File.CreateText(strLogPath)
                sw.WriteLine(DateStamp & ": Log Created...")
            End Using
        Else
            Using sw As StreamWriter = File.AppendText(strLogPath)
                sw.WriteLine(DateStamp & ": " & Message)
            End Using
        End If
    End Sub
    Public Function GetColIndex(ByVal Grid As DataGridView, ByVal strColName As String) As Integer
        Try
            Return Grid.Columns.Item(strColName).Index
        Catch ex As Exception
            Return -1
        End Try
    End Function
    Public Function ErrHandle(lngErrNum As Long, strErrDescription As String, strOrigSub As String) As Boolean 'True = safe to continue. False = PANIC, BAD THINGS, THE SKY IS FALLING!
        Dim strErrMsg As String
        strErrMsg = "ERROR:  MethodName=" & strOrigSub & " - " & lngErrNum & " - " & strErrDescription
        Logger(strErrMsg)
        Select Case lngErrNum
            Case 1042
                StatusBar("Connection Lost!")
                Dim blah = MsgBox("Unable to connect to server.  Check connection and try again.", vbOKOnly + vbCritical, "Connection Lost")
                Return True
            Case Else 'unhandled errors
                StatusBar("ERROR")
                Dim blah = MsgBox("An unhandled error has occurred!" & vbCrLf & vbCrLf & "Message:" & vbCrLf & strErrMsg, vbOKOnly + vbCritical, "Yikes!")
                Return False
        End Select
        Return False
    End Function
    Public Sub EndProgram() 'I will add more stuff to this later.
        Logger("Ending Program...")
        'cn_global.Close()
        End
    End Sub
End Module
