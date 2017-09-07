Namespace ConnectionMonitoring
    Public Class ConnectionWatchdog : Implements IDisposable

        Sub New(cachedMode As Boolean)
            InCachedMode = cachedMode
        End Sub

        Public Event StatusChanged As EventHandler
        Public Event RebuildCache As EventHandler
        Public Event WatcherTick As EventHandler

        Protected Overridable Sub OnStatusChanged(e As WatchDogStatusEventArgs)
            RaiseEvent StatusChanged(Me, e)
        End Sub

        Protected Overridable Sub OnRebuildCache(e As EventArgs)
            RaiseEvent RebuildCache(Me, e)
        End Sub

        Protected Overridable Sub OnWatcherTick(e As EventArgs)
            RaiseEvent WatcherTick(Me, e)
        End Sub

        Private FailedPings As Integer = 0
        Private Cycles As Integer = 0

        Private ServerIsOnline As Boolean
        Private CacheIsAvailable As Boolean
        Private InCachedMode As Boolean

        Private CurrentWatchdogStatus As WatchDogConnectionStatus = WatchDogConnectionStatus.Online
        Private PreviousWatchdogStatus As WatchDogConnectionStatus

        Const MaxFailedPings As Integer = 2
        Const CyclesTillHashCheck As Integer = 60
        Const WatcherInterval As Integer = 5000

        Public Sub StartWatcher()
            Watcher()
        End Sub

        Private Async Sub Watcher()
            Do Until disposedValue

                ServerIsOnline = Await GetServerStatus()
                CacheIsAvailable = Await VerifyLocalCacheHashOnly(InCachedMode)

                Dim Status = GetWatchdogStatus()
                If Status <> CurrentWatchdogStatus Then
                    CurrentWatchdogStatus = Status
                    OnStatusChanged(New WatchDogStatusEventArgs(CurrentWatchdogStatus))
                End If

                CheckForCacheRebuild()

                Await Task.Delay(WatcherInterval)
            Loop
        End Sub

        Private Sub CheckForCacheRebuild()
            If CurrentWatchdogStatus = WatchDogConnectionStatus.Online Then
                If CacheIsAvailable Then
                    Cycles += 1
                    If Cycles >= CyclesTillHashCheck Then
                        Cycles = 0
                        OnRebuildCache(New EventArgs)
                    End If
                Else
                    OnRebuildCache(New EventArgs)
                End If
                If PreviousWatchdogStatus = WatchDogConnectionStatus.CachedMode Or PreviousWatchdogStatus = WatchDogConnectionStatus.Offline Then
                    OnRebuildCache(New EventArgs)
                End If
            End If
            PreviousWatchdogStatus = CurrentWatchdogStatus
        End Sub

        Private Async Function GetServerStatus() As Task(Of Boolean)
            Return Await Task.Run(Function()
                                      For i = 0 To MaxFailedPings
                                          If Not CanTalkToServer() Then
                                              FailedPings += 1
                                              If FailedPings >= MaxFailedPings Then
                                                  Return False
                                              End If
                                          Else
                                              FailedPings = 0
                                              Return True
                                          End If
                                          Task.Delay(1000)
                                      Next
                                      Return True
                                  End Function)
        End Function

        Private Function CanTalkToServer() As Boolean
            Try
                Dim CanPing As Boolean = My.Computer.Network.Ping(ServerInfo.MySQLServerIP)
                'If server pinging, try a simple SQL query.
                If CanPing Then
                    Using LocalSQLComm As New MySqlComms(True), cmd = LocalSQLComm.ReturnMySqlCommand("SELECT NOW()")
                        Dim ServerDateTime = cmd.ExecuteScalar.ToString
                        'Fire tick event to update server datatime.
                        OnWatcherTick(New WatchDogTickEventArgs(ServerDateTime))
                    End Using
                    'Query successful. Return true.
                    Return True
                Else
                    'Not pinging. Return false.
                    Return False
                End If
            Catch
                'Catch ping or SQL exceptions, and return false.
                Return False
            End Try
        End Function

        Private Function GetWatchdogStatus() As WatchDogConnectionStatus
            Select Case True

                Case ServerIsOnline And Not InCachedMode
                    Return WatchDogConnectionStatus.Online

                Case ServerIsOnline And InCachedMode
                    InCachedMode = False
                    Return WatchDogConnectionStatus.Online

                Case Not ServerIsOnline And CacheIsAvailable
                    InCachedMode = True
                    Return WatchDogConnectionStatus.CachedMode

                Case Not ServerIsOnline And Not CacheIsAvailable
                    InCachedMode = False
                    Return WatchDogConnectionStatus.Offline

                Case Else
                    Return WatchDogConnectionStatus.Offline

            End Select
        End Function

#Region "IDisposable Support"
        Private disposedValue As Boolean ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(disposing As Boolean)
            If Not disposedValue Then
                If disposing Then
                    ' TODO: dispose managed state (managed objects).
                End If

                ' TODO: free unmanaged resources (unmanaged objects) and override Finalize() below.
                ' TODO: set large fields to null.
            End If
            disposedValue = True
        End Sub

        ' TODO: override Finalize() only if Dispose(disposing As Boolean) above has code to free unmanaged resources.
        'Protected Overrides Sub Finalize()
        '    ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
        '    Dispose(False)
        '    MyBase.Finalize()
        'End Sub

        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(disposing As Boolean) above.
            Dispose(True)
            ' TODO: uncomment the following line if Finalize() is overridden above.
            ' GC.SuppressFinalize(Me)
        End Sub
#End Region
    End Class

    Public Class WatchDogTickEventArgs : Inherits EventArgs
        Public ReadOnly Property ServerTime As String

        Public Sub New(serverTime As String)
            Me.ServerTime = serverTime
        End Sub
    End Class

    Public Class WatchDogStatusEventArgs : Inherits EventArgs
        Public Property ConnectionStatus As WatchDogConnectionStatus
        Public Sub New(ByVal connectionStatus As WatchDogConnectionStatus)
            Me.ConnectionStatus = connectionStatus
        End Sub
    End Class

    Public Enum WatchDogConnectionStatus
        Online
        Offline
        CachedMode
    End Enum

End Namespace