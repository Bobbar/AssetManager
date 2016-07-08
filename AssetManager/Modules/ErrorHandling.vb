Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Net.Sockets
Module ErrorHandling
    Public Function ErrHandleNew(ex As Exception, strOrigSub As String) As Boolean 'True = safe to continue. False = PANIC, BAD THINGS, THE SKY IS FALLING!
        'If Not IsNothing(ex.InnerException) Then
        '    Debug.Print("InnerEx: " & TypeName(ex.InnerException))
        'End If
        Select Case TypeName(ex)
            Case "WebException"
                Return handleWebException(ex, strOrigSub)
            Case "IndexOutOfRangeException"
                Dim handEx As IndexOutOfRangeException = ex
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & handEx.HResult & "  Message:" & handEx.Message)
                Return True
            Case "MySqlException"
                Return handleMySqlException(ex, strOrigSub)
            Case "InvalidCastException"
                Dim handEx As InvalidCastException = ex
                Select Case handEx.HResult
                    Case -2147467262 'DBNull to String type error. These are pretty ubiquitous and not a big deal. Move along.
                        Return True
                End Select
            Case "IOException"
                Return handleIOException(ex, strOrigSub)
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
    Private Function handleMySqlException(ex As MySqlException, strOrigSub As String) As Boolean
        Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
        Select Case ex.Number
            Case 1042
                ConnectionReady()
                Dim blah = MsgBox("Unable to connect to server.  Check connection and try again.", vbOKOnly + vbCritical, "Connection Lost")
                Return True
            Case 0
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
        Return False
    End Function
    Private Function handleWebException(ex As Net.WebException, strOrigSub As String) As Boolean
        Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Status & "  Message:" & ex.Message)
        Dim handResponse As Net.FtpWebResponse = ex.Response
        Select Case handResponse.StatusCode
            Case Net.FtpStatusCode.ActionNotTakenFileUnavailable
                Dim blah = MsgBox("FTP File was not found, or access was denied.", vbOKOnly + vbCritical, "Cannot Access FTP File")
                Return True
        End Select
        Return False
    End Function
    Private Function handleIOException(ex As IOException, strOrigSub As String) As Boolean
        If Not IsNothing(ex.InnerException) Then
            Select Case TypeName(ex.InnerException)
                Case "SocketException"
                    Return handleSocketException(ex.InnerException, strOrigSub)
                Case Else
                    Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex.InnerException) & "  #:" & ex.InnerException.HResult & "  Message:" & ex.InnerException.Message)
                    Return False
            End Select
        Else
        End If
        Return False
    End Function
    Private Function handleSocketException(ex As SocketException, strOrigSub As String) As Boolean
        Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.SocketErrorCode & "  Message:" & ex.Message)
        Select Case ex.SocketErrorCode
                                'FTPSocket timeout
            Case 10060
                Dim blah = MsgBox("Lost connection to the server or the server took too long to respond.  See Log.  '" & strLogPath & "'", vbOKOnly + vbExclamation, "Network Socket Timeout")
                Return True
            Case 10053
                Dim blah = MsgBox("Lost connection to the server or the server took too long to respond.  See Log.  '" & strLogPath & "'", vbOKOnly + vbExclamation, "Network Socket Disconnected")
                Return True
            Case Else
                Return False
        End Select
    End Function
End Module
