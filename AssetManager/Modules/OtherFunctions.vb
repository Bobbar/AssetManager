Imports System.Environment
Imports System
Imports System.IO
Module OtherFunctions
    Public strLogDir As String = GetFolderPath(SpecialFolder.ApplicationData) & "\AssetManager\"
    Public strLogName As String = "log.log"
    Public strLogPath As String = strLogDir & strLogName
    Public strTempPath As String = strLogDir & "temp\"
    Public colCurrentEntry As Color = ColorTranslator.FromHtml("#7AD1FF") '"#7AD1FF"
    Public colMissingField As Color = ColorTranslator.FromHtml("#D4EA08") '"#FF9827") '"#75BAFF")
    Public colCheckIn As Color = ColorTranslator.FromHtml("#B6FCC0")
    Public colCheckOut As Color = ColorTranslator.FromHtml("#FCB6B6")
    Public colHighlightOrange As Color = ColorTranslator.FromHtml("#FF9A26") '"#FF9827")
    Public colHighlightBlue As Color = ColorTranslator.FromHtml("#8BCEE8")
    Public colEditColor As Color = ColorTranslator.FromHtml("#81EAAA")
    Public ViewFormIndex As Integer
    Public GridStylez As System.Windows.Forms.DataGridViewCellStyle ' = New System.Windows.Forms.DataGridViewCellStyle()
    Public GridFont As Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Public stpw As New Stopwatch
    Public Sub StartTimer()
        stpw.Stop()
        stpw.Reset()
        stpw.Start()
    End Sub
    Public Sub StopTimer()
        stpw.Stop()
        Debug.Print(stpw.ElapsedMilliseconds)
    End Sub
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
    Public Sub ConnectionNotReady()
        Dim blah = MsgBox("Not connected to server or connection is busy!", vbOKOnly + vbExclamation, "Cannot Connect")
    End Sub
    Public Function ErrHandle(lngErrNum As Long, strErrDescription As String, strOrigSub As String) As Boolean 'True = safe to continue. False = PANIC, BAD THINGS, THE SKY IS FALLING!
        Dim strErrMsg As String
        strErrMsg = "ERROR:  MethodName=" & strOrigSub & " - " & lngErrNum & " - " & strErrDescription
        Logger(strErrMsg)
        Select Case lngErrNum
            Case -2147467259
                Dim blah = MsgBox("There was an error while connecting." & vbCrLf & "Message: " & strErrDescription, vbOKOnly + vbExclamation, "Connection Error")
                ConnectionReady()
                Return True
            Case 13 'null value from DB, ok to continue
                Return True
            Case 1042
                StatusBar("Connection Lost!")
                Dim blah = MsgBox("Unable to connect to server.  Check connection and try again.", vbOKOnly + vbCritical, "Connection Lost")
                Return True
            Case Else 'unhandled errors
                StatusBar("ERROR")
                Dim blah = MsgBox("An unhandled error has occurred!" & vbCrLf & vbCrLf & "Message: " & vbCrLf & strErrMsg, vbOKOnly + vbCritical, "Yikes!")
                Return False
        End Select
        Return False
    End Function
    Public Sub EndProgram() 'I will add more stuff to this later.
        Logger("Ending Program...")
        PurgeTempDir()
        GlobalConn.Close()
        Liveconn.Close()
        End
    End Sub
    Public Sub PurgeTempDir()
        On Error Resume Next
        Directory.Delete(strTempPath, True)
    End Sub
    Public Sub ConnectStatus(Message As String, FColor As Color)
        AssetManager.ConnStatusLabel.Text = Message
        AssetManager.ConnStatusLabel.ForeColor = FColor
        AssetManager.Refresh()
    End Sub
    Public Sub StatusBar(Text As String)
        AssetManager.StatusLabel.Text = Text
        'Attachments.StatusLabel.Text = Text
        'Attachments.Refresh()
        AssetManager.Refresh()
    End Sub
End Module
