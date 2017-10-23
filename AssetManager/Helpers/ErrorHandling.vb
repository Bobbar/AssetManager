Imports System.ComponentModel
Imports System.Data.SqlClient
Imports System.IO
Imports System.Net.Sockets
Imports System.Reflection
Imports MySql.Data.MySqlClient
Imports System.Data.SQLite

Module ErrorHandling

    'Suppress additional messages to user.
    Private SuppressAdditionalMessages As Boolean = False

    Public Function ErrHandle(ex As Exception, Method As MethodBase) As Boolean 'Recursive error handler. Returns False for undesired or dangerous errors, True if safe to continue.
        Try
            Logger("ERR STACK TRACE: " & ex.ToString)
            Dim ErrorResult As Boolean
            Select Case True
                Case TypeOf ex Is Net.WebException
                    Dim WebEx = DirectCast(ex, Net.WebException)
                    ErrorResult = handleWebException(WebEx, Method)

                Case TypeOf ex Is IndexOutOfRangeException
                    Dim handEx As IndexOutOfRangeException = DirectCast(ex, IndexOutOfRangeException)
                    Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & handEx.HResult & "  Message:" & handEx.Message)
                    ErrorResult = True

                Case TypeOf ex Is MySqlException
                    Dim MySQLEx = DirectCast(ex, MySqlException)
                    ErrorResult = handleMySQLException(MySQLEx, Method)

                Case TypeOf ex Is SqlException
                    Dim SQLEx = DirectCast(ex, SqlException)
                    ErrorResult = handleSQLException(SQLEx, Method)

                Case TypeOf ex Is SQLiteException
                    Dim SQLiteEx = DirectCast(ex, SQLiteException)
                    ErrorResult = handleSQLiteException(SQLiteEx, Method)

                Case TypeOf ex Is InvalidCastException
                    Dim handEx As InvalidCastException = DirectCast(ex, InvalidCastException)
                    Logger("CAST ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & handEx.HResult & "  Message:" & handEx.Message)
                    PromptUser("An object was cast to an unmatched type.  See log for details.  Log: " & Paths.LogPath, vbOKOnly + vbExclamation, "Invalid Cast Error")
                    Select Case handEx.HResult
                        Case -2147467262 'DBNull to String type error. These are pretty ubiquitous and not a big deal. Move along.
                            ErrorResult = True
                    End Select

                Case TypeOf ex Is IOException
                    Dim IOEx = DirectCast(ex, IOException)
                    ErrorResult = handleIOException(IOEx, Method)

                Case TypeOf ex Is Net.NetworkInformation.PingException
                    Dim PingEx = DirectCast(ex, Net.NetworkInformation.PingException)
                    ErrorResult = handlePingException(PingEx, Method)

                Case TypeOf ex Is SocketException
                    Dim SocketEx = DirectCast(ex, SocketException)
                    ErrorResult = handleSocketException(SocketEx, Method)

                Case TypeOf ex Is FormatException
                    Dim FormatEx = DirectCast(ex, FormatException)
                    ErrorResult = handleFormatException(FormatEx, Method)

                Case TypeOf ex Is Win32Exception
                    Dim Win32Ex = DirectCast(ex, Win32Exception)
                    ErrorResult = handleWin32Exception(Win32Ex, Method)

                Case TypeOf ex Is InvalidOperationException
                    Dim InvalidOpEx = DirectCast(ex, InvalidOperationException)
                    ErrorResult = handleOperationException(InvalidOpEx, Method)

                Case TypeOf ex Is NoPingException
                    Dim NoPingEx = DirectCast(ex, NoPingException)
                    ErrorResult = handleNoPingException(NoPingEx, Method)

                Case TypeOf ex Is NullReferenceException
                    Dim NullRefEx = DirectCast(ex, NullReferenceException)
                    ErrorResult = handleNullReferenceException(NullRefEx, Method)

                Case Else
                    UnHandledError(ex, ex.HResult, Method)
                    ErrorResult = False
            End Select
            If ex.InnerException IsNot Nothing Then
                ErrorResult = ErrHandle(ex.InnerException, Method)
            End If
            Return ErrorResult
        Finally
            SuppressAdditionalMessages = False
        End Try
    End Function

    Private Function handleWin32Exception(ex As Win32Exception, Method As MethodBase) As Boolean
        Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.NativeErrorCode & "  Message:" & ex.Message)
        Select Case ex.NativeErrorCode
            Case 1326 'Bad credentials error. Clear AdminCreds
                PromptUser("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.NativeErrorCode & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "Network Error")
                SecurityTools.ClearAdminCreds()
                Return True
            Case 86 'Bad credentials error. Clear AdminCreds
                PromptUser("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.NativeErrorCode & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "Network Error")
                SecurityTools.ClearAdminCreds()
                Return True
            Case 5 'Access denied error. Clear AdminCreds
                PromptUser("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.NativeErrorCode & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "Network Error")
                SecurityTools.ClearAdminCreds()
                Return True
            Case Else
                PromptUser("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.NativeErrorCode & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "Network Error")
                Return True
        End Select
    End Function

    Private Function handleFormatException(ex As FormatException, Method As MethodBase) As Boolean
        Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
        Select Case ex.HResult
            Case -2146233033
                Return True
            Case Else
                UnHandledError(ex, ex.HResult, Method)
        End Select
        Return False
    End Function

    Private Function handleMySQLException(ex As MySqlException, Method As MethodBase) As Boolean
        Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
        Select Case ex.Number
            Case 1042
                ' ConnectionReady()
                PromptUser("Unable to connect to server.  Check connection and try again.", vbOKOnly + vbExclamation, "Connection Lost")
                Return True
            Case 0
                ' ConnectionReady()
                PromptUser("Unable to connect to server.  Check connection and try again.", vbOKOnly + vbExclamation, "Connection Lost")
                Return True
            Case 1064
                PromptUser("Something went wrong with the SQL command. See log for details.  Log: " & Paths.LogPath, vbOKOnly + vbCritical, "SQL Syntax Error")
                Return True
            Case 1406
                PromptUser(ex.Message & vbCrLf & vbCrLf & "Log: " & Paths.LogPath, vbOKOnly + vbCritical, "SQL Error")
                Return True
            Case 1292
                PromptUser("Something went wrong with the SQL command. See log for details.  Log: " & Paths.LogPath, vbOKOnly + vbCritical, "SQL Syntax Error")
                Return True
            Case Else
                UnHandledError(ex, ex.Number, Method)
        End Select
        Return False
    End Function

    Private Function handlePingException(ex As Net.NetworkInformation.PingException, Method As MethodBase) As Boolean
        Select Case ex.HResult
            Case -2146233079
                Return False
            Case Else
                UnHandledError(ex, ex.HResult, Method)
                Return False
        End Select
    End Function

    Private Function handleOperationException(ex As InvalidOperationException, Method As MethodBase) As Boolean
        Select Case ex.HResult
            Case -2146233079
                Logger("UNHANDLED ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Return False
            Case Else
                UnHandledError(ex, ex.HResult, Method)
                Return False
        End Select
    End Function

    Private Function handleWebException(ex As Net.WebException, Method As MethodBase) As Boolean
        Dim handResponse As Net.FtpWebResponse = DirectCast(ex.Response, Net.FtpWebResponse)
        Select Case handResponse.StatusCode
            Case Net.FtpStatusCode.ActionNotTakenFileUnavailable
                PromptUser("FTP File was not found, or access was denied.", vbOKOnly + vbExclamation, "Cannot Access FTP File")
                Return True
            Case Else
                Select Case ex.HResult
                    Case -2146233079
                        Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                        PromptUser("Could not connect to FTP server.", vbOKOnly + vbExclamation, "Connection Failure")
                        Return True
                End Select
                UnHandledError(ex, ex.HResult, Method)
        End Select
        Return False
    End Function

    Private Function handleIOException(ex As IOException, Method As MethodBase) As Boolean
        Select Case ex.HResult
            Case -2147024864
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                'PromptUser("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "IO Error")
                Return True
            Case -2146232800
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                Return True
            Case Else
                UnHandledError(ex, ex.HResult, Method)
        End Select
        Return False
    End Function

    Private Function handleSocketException(ex As SocketException, Method As MethodBase) As Boolean
        Select Case ex.SocketErrorCode                               'FTPSocket timeout
            Case SocketError.TimedOut '10060
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.SocketErrorCode & "  Message:" & ex.Message)
                PromptUser("Lost connection to the server or the server took too long to respond.  See Log.  '" & Paths.LogPath & "'", vbOKOnly + vbExclamation, "Network Socket Timeout")
                Return True
            Case SocketError.HostUnreachable '10065 'host unreachable
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.SocketErrorCode & "  Message:" & ex.Message)
                'Dim blah = PromptUser("Lost connection to the server or the server took too long to respond.  See Log.  '" & strLogPath & "'", vbOKOnly + vbExclamation, "Network Socket Timeout")
                Return True
            Case SocketError.ConnectionAborted '10053
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.SocketErrorCode & "  Message:" & ex.Message)
                PromptUser("Lost connection to the server or the server took too long to respond.  See Log.  '" & Paths.LogPath & "'", vbOKOnly + vbExclamation, "Connection Aborted")
                Return True
            Case SocketError.ConnectionReset '10054 'connection reset
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.SocketErrorCode & "  Message:" & ex.Message)
                PromptUser("Lost connection to the server or the server took too long to respond.  See Log.  '" & Paths.LogPath & "'", vbOKOnly + vbExclamation, "Connection Reset")
                Return True
            Case SocketError.NetworkUnreachable
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.SocketErrorCode & "  Message:" & ex.Message)
                PromptUser("Could not connect to server.  See Log.  '" & Paths.LogPath & "'", vbOKOnly + vbExclamation, "Network Unreachable")
                Return True
            Case SocketError.HostNotFound '11001 'host not found.
                Return False
            Case Else
                UnHandledError(ex, ex.SocketErrorCode, Method)
                Return False
        End Select
    End Function

    Private Function handleSQLException(ex As SqlException, Method As MethodBase) As Boolean
        Select Case ex.Number
            Case 18456
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                PromptUser("Error connecting to MUNIS Database.  Your username may not have access.", vbOKOnly + vbExclamation, "MUNIS Error")
                Return False
            Case 102
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                PromptUser("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "ERROR")
                Return False
            Case 121
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                PromptUser("Could not connect to MUNIS database.", vbOKOnly + vbExclamation, "Network Error")
                Return False
            Case 245
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                'Dim blah = PromptUser("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "ERROR")
                Return False
            Case 248
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                PromptUser("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "ERROR")
                Return False
            Case 53
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.Number & "  Message:" & ex.Message)
                PromptUser("Could not connect to MUNIS database.", vbOKOnly + vbExclamation, "Network Error")
                SuppressAdditionalMessages = True
                Return True
            Case Else
                UnHandledError(ex, ex.Number, Method)
                Return False
        End Select
    End Function

    Private Function handleSQLiteException(ex As SqliteException, Method As MethodBase) As Boolean

        Select Case ex.ErrorCode
            Case 1
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.ErrorCode & "  Message:" & ex.Message)
                Return True
            Case Else
                UnHandledError(ex, ex.ErrorCode, Method)
                Return False
        End Select

    End Function


    Private Function handleNoPingException(ex As NoPingException, Method As MethodBase) As Boolean
        Select Case ex.HResult
            Case -2146233088
                Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
                PromptUser("Unable to connect to server.  Check connection and try again.", vbOKOnly + vbExclamation, "Connection Lost")
                Return True
            Case Else
                UnHandledError(ex, ex.HResult, Method)
                Return False
        End Select
    End Function

    Private Function handleNullReferenceException(ex As Exception, Method As MethodBase) As Boolean
        If ServerInfo.ServerPinging Then
            Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
            PromptUser("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message, vbOKOnly + vbExclamation, "ERROR")
            EndProgram()
            Return False
        Else
            Logger("ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ex.HResult & "  Message:" & ex.Message)
            Return True
        End If
    End Function

    Private Sub UnHandledError(ex As Exception, ErrorCode As Integer, Method As MethodBase)
        Logger("UNHANDLED ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ErrorCode & "  Message:" & ex.Message)
        PromptUser("UNHANDLED ERROR:  MethodName=" & Method.Name & "  Type: " & TypeName(ex) & "  #:" & ErrorCode & "  Message:" & ex.Message & vbCrLf & vbCrLf & "file://" & Paths.LogPath, vbOKOnly + vbCritical, "ERROR")
        EndProgram()
    End Sub

    Private Sub PromptUser(ByVal Prompt As String, Optional ByVal Buttons As Integer = vbOKOnly + vbInformation, Optional ByVal Title As String = Nothing, Optional ByVal ParentFrm As Form = Nothing)
        If Not SuppressAdditionalMessages Then
            Message(Prompt, Buttons, Title, ParentFrm)
        End If
    End Sub

End Module