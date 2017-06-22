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
End Module
