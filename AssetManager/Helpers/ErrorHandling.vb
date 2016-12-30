Imports MySql.Data.MySqlClient
Imports System.IO
Imports System.Net.Sockets
Imports System.ComponentModel
Imports System.Data.SqlClient
Module ErrorHandling
    Public Function ErrHandle(ex As Exception, strOrigSub As String) As Boolean 'Recursive error handler. Returns False for undesired or dangerous errors, True if safe to continue.
        Dim ErrorResult As Boolean
        Select Case TypeName(ex)
            Case "BackgroundWorkerCancelledException"
                ErrorResult = True
            Case "WebException"
                ErrorResult = handleWebException(ex, strOrigSub)
            Case "IndexOutOfRangeException"
                Dim handEx As IndexOutOfRangeException = ex
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & handEx.HResult & "  Message:" & handEx.Message)
                ErrorResult = True
            Case "MySqlException"
                ErrorResult = handleMySqlException(ex, strOrigSub)
            Case "SqlException"
                ErrorResult = handleSQLException(ex, strOrigSub)
            Case "InvalidCastException"
                Dim handEx As InvalidCastException = ex
                Logger("CAST ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & handEx.HResult & "  Message:" & handEx.Message)
                Dim blah = Message("An object was cast to an unmatched type.  See log for details.  Log: " & strLogPath, vbOKOnly + vbExclamation, "Invalid Cast Error")
                Select Case handEx.HResult
                    Case -2147467262 'DBNull to String type error. These are pretty ubiquitous and not a big deal. Move along.
                        ErrorResult = True
                End Select
            Case "IOException"
                ErrorResult = handleIOException(ex, strOrigSub)
            Case "PingException"
                ErrorResult = handlePingException(ex, strOrigSub)
            Case "SocketException"
                ErrorResult = handleSocketException(ex, strOrigSub)
            Case "FormatException"
                ErrorResult = handleFormatException(ex, strOrigSub)
            Case "Win32Exception"
                ErrorResult = handleWin32Exception(ex, strOrigSub)
            Case "InvalidOperationException"
                ErrorResult = handleOperationException(ex, strOrigSub)
            Case Else
                Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Dim blah = Message("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbCritical, "ERROR")
                ErrorResult = False
                EndProgram()
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
                Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Dim blah = Message("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbCritical, "ERROR")
                EndProgram()
        End Select
        Return False
    End Function
    Private Function handleFormatException(ex As FormatException, strOrigSub As String) As Boolean
        Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
        Select Case ex.HResult
            Case -2146233033
                Return True
            Case Else
                Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Dim blah = Message("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbCritical, "ERROR")
                EndProgram()
        End Select
        Return False
    End Function
    Private Function handleMySqlException(ex As MySqlException, strOrigSub As String) As Boolean
        Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
        Select Case ex.Number
            Case 1042
                ConnectionReady()
                Dim blah = Message("Unable to connect to server.  Check connection and try again.", vbOKOnly + vbExclamation, "Connection Lost")
                Return True
            Case 0
                ConnectionReady()
                Dim blah = Message("Unable to connect to server.  Check connection and try again.", vbOKOnly + vbExclamation, "Connection Lost")
                Return True
            Case 1064
                Dim blah = Message("Something went wrong with the SQL command. See log for details.  Log: " & strLogPath, vbOKOnly + vbCritical, "SQL Syntax Error")
                Return True
            Case 1292
                Dim blah = Message("Something went wrong with the SQL command. See log for details.  Log: " & strLogPath, vbOKOnly + vbCritical, "SQL Syntax Error")
                Return True
            Case Else
                Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Dim blah = Message("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbCritical, "ERROR")
                EndProgram()
        End Select
        Return False
    End Function
    Private Function handlePingException(ex As Net.NetworkInformation.PingException, strOrigSub As String) As Boolean
        Select Case ex.HResult
            Case -2146233079
                Return False
            Case Else
                Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Dim blah = Message("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbCritical, "ERROR")
                EndProgram()
        End Select
    End Function
    Private Function handleOperationException(ex As InvalidOperationException, strOrigSub As String) As Boolean
        Select Case ex.HResult
            Case -2146233079
                Return False
            Case Else
                Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Dim blah = Message("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbCritical, "ERROR")
                EndProgram()
        End Select
    End Function
    Private Function handleWebException(ex As Net.WebException, strOrigSub As String) As Boolean
        Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Status & "  Message:" & ex.Message)
        Dim handResponse As Net.FtpWebResponse = ex.Response
        Select Case handResponse.StatusCode
            Case Net.FtpStatusCode.ActionNotTakenFileUnavailable
                Dim blah = Message("FTP File was not found, or access was denied.", vbOKOnly + vbExclamation, "Cannot Access FTP File")
                Return True
            Case Else
                Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Dim blah = Message("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbCritical, "ERROR")
                EndProgram()
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
                Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Dim blah = Message("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbCritical, "ERROR")
                EndProgram()
        End Select
        Return False
    End Function
    Private Function handleSocketException(ex As SocketException, strOrigSub As String) As Boolean
        Select Case ex.SocketErrorCode                               'FTPSocket timeout
            Case 10060
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.SocketErrorCode & "  Message:" & ex.Message)
                Dim blah = Message("Lost connection to the server or the server took too long to respond.  See Log.  '" & strLogPath & "'", vbOKOnly + vbExclamation, "Network Socket Timeout")
                Return True
            Case 10053
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.SocketErrorCode & "  Message:" & ex.Message)
                Dim blah = Message("Lost connection to the server or the server took too long to respond.  See Log.  '" & strLogPath & "'", vbOKOnly + vbExclamation, "Network Socket Disconnected")
                Return True
            Case 11001 'host not found.
                Return False
            Case Else
                Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Dim blah = Message("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbCritical, "ERROR")
                EndProgram()
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
            Case 245
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                'Dim blah = Message("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "ERROR")
                Return False
            Case 248
                Logger("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                Dim blah = Message("ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "ERROR")
                Return False
            Case Else
                Logger("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                Dim blah = Message("UNHANDLED ERROR:  MethodName=" & strOrigSub & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message, vbOKOnly + vbCritical, "ERROR")
                EndProgram()
        End Select
    End Function
End Module
