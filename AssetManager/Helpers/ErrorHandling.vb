Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Net.Sockets
Imports System.ComponentModel
Imports System.Data.SqlClient
Module ErrorHandling
    Public Function ErrHandle(ex As Exception, strOrigSub As String) As Boolean 'Recursive error handler. Returns False for undesired or dangerous errors, True if safe to continue.
        Dim ErrorResult As Boolean
        Select Case True
            Case TypeOf ex Is BackgroundWorkerCancelledException
                ErrorResult = True
            Case TypeOf ex Is Net.WebException
                ErrorResult = handleWebException(ex, strOrigSub)
            Case TypeOf ex Is IndexOutOfRangeException
                Dim handEx As IndexOutOfRangeException = ex
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & handEx.HResult & "  Message:" & handEx.Message)
                ErrorResult = True
            Case TypeOf ex Is MySqlException '"MySqlException"
                ErrorResult = handleMySqlException(ex, strOrigSub)
            Case TypeOf ex Is SqlException
                ErrorResult = handleSQLException(ex, strOrigSub)
            Case TypeOf ex Is InvalidCastException
                Dim handEx As InvalidCastException = ex
                Logger("CAST ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & handEx.HResult & "  Message:" & handEx.Message)
                Dim blah = Message("An object was cast to an unmatched type.  See log for details.  Log: " & strLogPath, vbOKOnly + vbExclamation, "Invalid Cast Error")
                Select Case handEx.HResult
                    Case -2147467262 'DBNull to String type error. These are pretty ubiquitous and not a big deal. Move along.
                        ErrorResult = True
                End Select
            Case TypeOf ex Is IOException
                ErrorResult = handleIOException(ex, strOrigSub)
            Case TypeOf ex Is Net.NetworkInformation.PingException
                ErrorResult = handlePingException(ex, strOrigSub)
            Case TypeOf ex Is SocketException
                ErrorResult = handleSocketException(ex, strOrigSub)
            Case TypeOf ex Is FormatException
                ErrorResult = handleFormatException(ex, strOrigSub)
            Case TypeOf ex Is Win32Exception
                ErrorResult = handleWin32Exception(ex, strOrigSub)
            Case TypeOf ex Is InvalidOperationException
                ErrorResult = handleOperationException(ex, strOrigSub)
            Case TypeOf ex Is NoPingException
                ErrorResult = handleNoPingException(ex, strOrigSub)
            Case Else
                UnHandledError(ex, ex.HResult, strOrigSub)
                ErrorResult = False
        End Select
        If Not IsNothing(ex.InnerException) Then ErrHandle(ex.InnerException, strOrigSub)
        Return ErrorResult
    End Function
    Private Function handleWin32Exception(ex As Win32Exception, strOrigSub As String) As Boolean
        Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
        Select Case ex.HResult
            Case -2147467259
                Dim blah = Message("Network path not found.", vbOKOnly + vbExclamation, "Network Error")
                Return True
            Case Else
                UnHandledError(ex, ex.HResult, strOrigSub)
        End Select
        Return False
    End Function
    Private Function handleFormatException(ex As FormatException, strOrigSub As String) As Boolean
        Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
        Select Case ex.HResult
            Case -2146233033
                Return True
            Case Else
                UnHandledError(ex, ex.HResult, strOrigSub)
        End Select
        Return False
    End Function
    Private Function handleMySqlException(ex As MySqlException, strOrigSub As String) As Boolean
        Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
        Select Case ex.Number
            Case 1042
                ' ConnectionReady()
                Dim blah = Message("Unable to connect to server.  Check connection and try again.", vbOKOnly + vbExclamation, "Connection Lost")
                Return True
            Case 0
                ' ConnectionReady()
                Dim blah = Message("Unable to connect to server.  Check connection and try again.", vbOKOnly + vbExclamation, "Connection Lost")
                Return True
            Case 1064
                Dim blah = Message("Something went wrong with the SQL command. See log for details.  Log: " & strLogPath, vbOKOnly + vbCritical, "SQL Syntax Error")
                Return True
            Case 1292
                Dim blah = Message("Something went wrong with the SQL command. See log for details.  Log: " & strLogPath, vbOKOnly + vbCritical, "SQL Syntax Error")
                Return True
            Case Else
                UnHandledError(ex, ex.Number, strOrigSub)
        End Select
        Return False
    End Function
    Private Function handlePingException(ex As Net.NetworkInformation.PingException, strOrigSub As String) As Boolean
        Select Case ex.HResult
            Case -2146233079
                Return False
            Case Else
                UnHandledError(ex, ex.HResult, strOrigSub)
        End Select
    End Function
    Private Function handleOperationException(ex As InvalidOperationException, strOrigSub As String) As Boolean
        Select Case ex.HResult
            Case -2146233079
                Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Return False
            Case Else
                UnHandledError(ex, ex.HResult, strOrigSub)
        End Select
    End Function
    Private Function handleWebException(ex As Net.WebException, strOrigSub As String) As Boolean
        Dim handResponse As Net.FtpWebResponse = ex.Response
        Select Case handResponse.StatusCode
            Case Net.FtpStatusCode.ActionNotTakenFileUnavailable
                Message("FTP File was not found, or access was denied.", vbOKOnly + vbExclamation, "Cannot Access FTP File")
                Return True
            Case Else
                Select Case ex.HResult
                    Case -2146233079
                        Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                        Message("Could not connect to FTP server.", vbOKOnly + vbExclamation, "Connection Failure")
                        Return True
                End Select
                UnHandledError(ex, ex.HResult, strOrigSub)
        End Select
        Return False
    End Function
    Private Function handleIOException(ex As IOException, strOrigSub As String) As Boolean
        Select Case ex.HResult
            Case -2147024864
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                'Message("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "IO Error")
                Return True
            Case -2146232800
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Return True
            Case Else
                UnHandledError(ex, ex.HResult, strOrigSub)
        End Select
        Return False
    End Function
    Private Function handleSocketException(ex As SocketException, strOrigSub As String) As Boolean
        Select Case ex.SocketErrorCode                               'FTPSocket timeout
            Case 10060
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.SocketErrorCode & "  Message:" & ex.Message)
                Dim blah = Message("Lost connection to the server or the server took too long to respond.  See Log.  '" & strLogPath & "'", vbOKOnly + vbExclamation, "Network Socket Timeout")
                Return True
            Case 10065 'host unreachable
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.SocketErrorCode & "  Message:" & ex.Message)
                'Dim blah = Message("Lost connection to the server or the server took too long to respond.  See Log.  '" & strLogPath & "'", vbOKOnly + vbExclamation, "Network Socket Timeout")
                Return True
            Case 10053
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.SocketErrorCode & "  Message:" & ex.Message)
                Dim blah = Message("Lost connection to the server or the server took too long to respond.  See Log.  '" & strLogPath & "'", vbOKOnly + vbExclamation, "Network Socket Disconnected")
                Return True
            Case 10054 'connection reset
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.SocketErrorCode & "  Message:" & ex.Message)
                Dim blah = Message("Lost connection to the server or the server took too long to respond.  See Log.  '" & strLogPath & "'", vbOKOnly + vbExclamation, "Network Socket Disconnected")
                Return True
            Case 11001 'host not found.
                Return False
            Case Else
                UnHandledError(ex, ex.SocketErrorCode, strOrigSub)
        End Select
    End Function
    Private Function handleSQLException(ex As SqlException, strOrigSub As String) As Boolean
        Select Case ex.Number
            Case 18456
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                Dim blah = Message("Error connecting to MUNIS Database.  Your username may not have access.", vbOKOnly + vbExclamation, "MUNIS Error")
                Return False
            Case 102
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                Dim blah = Message("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "ERROR")
                Return False
            Case 121
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                Dim blah = Message("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "ERROR")
                Return False
            Case 245
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                'Dim blah = Message("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "ERROR")
                Return False
            Case 248
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                Dim blah = Message("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "ERROR")
                Return False
            Case Else
                UnHandledError(ex, ex.Number, strOrigSub)
        End Select
    End Function
    Private Function handleNoPingException(ex As NoPingException, strOrigSub As String) As Boolean
        Select Case ex.HResult
            Case -2146233088
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Dim blah = Message("Unable to connect to server.  Check connection and try again.", vbOKOnly + vbExclamation, "Connection Lost")
                Return True
            Case Else
                UnHandledError(ex, ex.HResult, strOrigSub)
        End Select
    End Function
    Private Sub UnHandledError(ex As Exception, ErrorCode As Integer, strOrigSub As String)
        Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
        Dim blah = Message("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message & vbCrLf & vbCrLf & strLogPath, vbOKOnly + vbCritical, "ERROR")
        EndProgram()
    End Sub
End Module
