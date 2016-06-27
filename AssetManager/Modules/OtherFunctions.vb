Option Explicit On
Imports System.Environment
Imports System.IO
Imports MySql.Data.MySqlClient
Module OtherFunctions
    'paths
    Public strLogDir As String = GetFolderPath(SpecialFolder.ApplicationData) & "\AssetManager\"
    Public strLogName As String = "log.log"
    Public strLogPath As String = strLogDir & strLogName
    Public strTempPath As String = strLogDir & "temp\"
    'colors
    Public colCurrentEntry As Color = ColorTranslator.FromHtml("#7AD1FF") '"#7AD1FF"
    Public colMissingField As Color = ColorTranslator.FromHtml("#82C1FF") '"#FF9827") '"#75BAFF")
    Public colCheckIn As Color = ColorTranslator.FromHtml("#B6FCC0")
    Public colCheckOut As Color = ColorTranslator.FromHtml("#FCB6B6")
    Public colHighlightOrange As Color = ColorTranslator.FromHtml("#FF9A26") '"#FF9827")
    Public colHighlightBlue As Color = ColorTranslator.FromHtml("#8BCEE8")
    Public colHighlightColor As Color = ColorTranslator.FromHtml("#FF6600")
    Public colSelectColor As Color = ColorTranslator.FromHtml("#FFB917")
    Public colEditColor As Color = ColorTranslator.FromHtml("#81EAAA")
    Public colFormBackColor As Color = Color.FromArgb(232, 232, 232)
    Public colStatusBarProblem As Color = ColorTranslator.FromHtml("#FF9696")
    Public colToolBarColor As Color = Color.FromArgb(249, 226, 166)
    Public DefGridBC As Color, DefGridSelCol As Color
    'misc
    Public GridStylez As System.Windows.Forms.DataGridViewCellStyle ' = New System.Windows.Forms.DataGridViewCellStyle()
    Public GridFont As Font = New System.Drawing.Font("Consolas", 9.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
    Public stpw As New Stopwatch
    Public ProgramEnding As Boolean = False
    Public Function AdjustComboBoxWidth(ByVal sender As Object, ByVal e As EventArgs)
        Dim senderComboBox = DirectCast(sender, ComboBox)
        Dim width As Integer = senderComboBox.DropDownWidth
        Dim g As Graphics = senderComboBox.CreateGraphics()
        Dim font As Font = senderComboBox.Font
        Dim vertScrollBarWidth As Integer = If((senderComboBox.Items.Count > senderComboBox.MaxDropDownItems), SystemInformation.VerticalScrollBarWidth, 0)
        Dim newWidth As Integer
        For Each s As String In DirectCast(sender, ComboBox).Items
            newWidth = CInt(g.MeasureString(s, font).Width) + vertScrollBarWidth
            If width < newWidth Then
                width = newWidth
            End If
        Next
        senderComboBox.DropDownWidth = width
        Return False
    End Function
    Public Sub StartTimer()
        stpw.Stop()
        stpw.Reset()
        stpw.Start()
    End Sub
    Public Sub StopTimer()
        stpw.Stop()
        Debug.Print("Stopwatch: MS:" & stpw.ElapsedMilliseconds & " Ticks: " & stpw.ElapsedTicks)
    End Sub
    Public Sub Logger(Message As String)
        Dim MaxLogSizeKiloBytes As Short = 100
        Dim DateStamp As String = DateTime.Now
        Dim infoReader As FileInfo
        infoReader = My.Computer.FileSystem.GetFileInfo(strLogPath)
        If Not File.Exists(strLogPath) Then
            Dim di As DirectoryInfo = Directory.CreateDirectory(strLogDir)
            Using sw As StreamWriter = File.CreateText(strLogPath)
                sw.WriteLine(DateStamp & ": Log Created...")
                sw.WriteLine(DateStamp & ": " & Message)
            End Using
        Else
            If (infoReader.Length / 1000) < MaxLogSizeKiloBytes Then
                Using sw As StreamWriter = File.AppendText(strLogPath)
                    sw.WriteLine(DateStamp & ": " & Message)
                End Using
            Else
                If RotateLogs() Then
                    Using sw As StreamWriter = File.AppendText(strLogPath)
                        sw.WriteLine(DateStamp & ": " & Message)
                    End Using
                End If
            End If
        End If
    End Sub
    Private Function RotateLogs() As Boolean
        Try
            File.Copy(strLogPath, strLogPath + ".old", True)
            File.Delete(strLogPath)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function
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
    Public Function ErrHandleNew(ex As Exception, strOrigSub As String) As Boolean 'True = safe to continue. False = PANIC, BAD THINGS, THE SKY IS FALLING!
        If Not IsNothing(ex.InnerException) Then
            Debug.Print("InnerEx: " & TypeName(ex.InnerException))
        End If
        Select Case TypeName(ex)
            Case "WebException"
                Dim handEx As Net.WebException = ex
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & handEx.Status & "  Message:" & handEx.Message)
                Dim handResponse As Net.FtpWebResponse = handEx.Response
                Select Case handResponse.StatusCode
                    Case Net.FtpStatusCode.ActionNotTakenFileUnavailable
                        Dim blah = MsgBox("FTP File was not found, or access was denied.", vbOKOnly + vbCritical, "Cannot Access FTP File")
                        Return True
                End Select
            Case "IndexOutOfRangeException"
                Dim handEx As IndexOutOfRangeException = ex
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & handEx.HResult & "  Message:" & handEx.Message)
                Return True
            Case "MySqlException"
                Dim handEx As MySqlException = ex
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & handEx.Number & "  Message:" & handEx.Message)
                Select Case handEx.Number
                    Case 1042
                        ConnectionReady()
                        Dim blah = MsgBox("Unable to connect to server.  Check connection and try again.", vbOKOnly + vbCritical, "Connection Lost")
                        Return True
                    Case 1064
                        Dim blah = MsgBox("Something went wrong with the SQL command. See log for details.  Log= " & strLogPath, vbOKOnly + vbCritical, "SQL Syntax Error")
                        Return True
                    Case 1292
                        Dim blah = MsgBox("Something went wrong with the SQL command. See log for details.  Log= " & strLogPath, vbOKOnly + vbCritical, "SQL Syntax Error")
                        Return True
                End Select
            Case "InvalidCastException"
                Dim handEx As InvalidCastException = ex
                Select Case handEx.HResult
                    Case -2147467262 'DBNull to String type error. These are pretty ubiquitous and not a big deal. Move along.
                        Return True
                End Select
            Case "IOException"
                Dim handEx As IOException = ex
                If Not IsNothing(handEx.InnerException) Then
                    Select Case TypeName(handEx.InnerException)
                        Case "SocketException"
                            Dim innerEx As System.Net.Sockets.SocketException = handEx.InnerException
                            Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(innerEx) & "  #:" & innerEx.SocketErrorCode & "  Message:" & innerEx.Message)
                            Select Case innerEx.SocketErrorCode
                                'FTPSocket timeout
                                Case 10060
                                    Dim blah = MsgBox("Lost connection to the server or the server took too long to respond.  See Log.  '" & strLogPath & "'", vbOKOnly + vbExclamation, "Network Socket Timeout")
                                    Return True
                                Case Else
                                    Return False
                            End Select
                        Case Else
                            Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(handEx.InnerException) & "  #:" & handEx.InnerException.HResult & "  Message:" & handEx.InnerException.Message)
                    End Select
                Else
                End If
            Case Else
                Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                'Dim blah = MsgBox("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbCritical, "ERROR")
                Return False
        End Select
        If Not IsNothing(ex.InnerException) Then
            Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex.InnerException) & "  #:" & ex.InnerException.HResult & "  Message:" & ex.InnerException.Message)
            'Dim blah = MsgBox("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex.InnerException) & "  #:" & ex.InnerException.HResult & "  Message:" & ex.InnerException.Message, vbOKOnly + vbCritical, "ERROR")
        Else
            Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
            'Dim blah = MsgBox("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbCritical, "ERROR")
        End If
        Return False
    End Function
    Public Sub EndProgram()
        ProgramEnding = True
        Logger("Ending Program...")
        PurgeTempDir()
        GlobalConn.Close()
        LiveConn.Close()
        End
    End Sub
    Public Sub PurgeTempDir()
        On Error Resume Next
        Directory.Delete(strTempPath, True)
    End Sub
    Public Sub ConnectStatus(Message As String, FColor As Color)
        MainFrom.ConnStatusLabel.Text = Message
        MainFrom.ConnStatusLabel.ForeColor = FColor
        MainFrom.Refresh()
    End Sub
    Public Function NoNull(DBVal As Object) As String
        Try
            Return IIf(IsDBNull(DBVal), "", DBVal.ToString)
        Catch ex As Exception
            ErrHandleNew(ex, System.Reflection.MethodInfo.GetCurrentMethod().Name)
            Return ""
        End Try
    End Function
End Module
