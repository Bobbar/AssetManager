Module DBFunctions
    Public Sub RefreshLocalDBCache()
        Try
            StartTimer()
            BuildingCache = True
            Using conn As New SQLite_Comms(False)
                conn.RefreshSQLCache()
            End Using
            BuildingCache = False
            StopTimer()
        Catch ex As Exception
            BuildingCache = False
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Sub

    Public Async Function RefreshLocalDBCacheAsync() As Task(Of Boolean)
        Try
            BuildingCache = True
            Dim Done As Boolean = False
            Done = Await Task.Run(Function()
                                      Try
                                          Using conn As New SQLite_Comms(False)
                                              conn.RefreshSQLCache()
                                          End Using
                                          Return True
                                      Catch ex As Exception
                                          BuildingCache = False
                                          Return False
                                          ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
                                      End Try
                                  End Function)
            BuildingCache = False
            Return Done
        Catch ex As Exception
            BuildingCache = False
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        End Try
    End Function
End Module
