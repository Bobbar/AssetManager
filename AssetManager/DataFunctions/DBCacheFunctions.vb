Imports System.Threading
Module DBCacheFunctions

    Public Sub RefreshLocalDBCache()
        Try
            BuildingCache = True
            Using conn As New SQLite_Comms(False)
                conn.RefreshSQLCache()
            End Using
        Catch ex As Exception
            ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
        Finally
            BuildingCache = False
        End Try
    End Sub

    ''' <summary>
    ''' Builds local hash list and compares to previously built remote hash list. Returns False for mismatch.
    ''' </summary>
    ''' <returns></returns>
    Public Async Function VerifyLocalCacheHashOnly() As Task(Of Boolean)
        If RemoteTableHashes Is Nothing Then Return False
        Return Await Task.Run(Function()
                                  Dim LocalHashes As New List(Of String)
                                  Using SQLiteComms As New SQLite_Comms
                                      LocalHashes = SQLiteComms.LocalTableHashList
                                      Return SQLiteComms.CompareTableHashes(LocalHashes, RemoteTableHashes)
                                  End Using
                              End Function)
    End Function

    ''' <summary>
    ''' Builds hash lists for both local and remote tables and compares them.  Returns False for mismatch.
    ''' </summary>
    ''' <param name="OfflineMode"></param>
    ''' <returns></returns>
    Public Function VerifyCacheHashes(Optional OfflineMode As Boolean = False) As Boolean
        Try
            Using SQLiteComms As New SQLite_Comms
                If SQLiteComms.GetSchemaVersion > 0 Then
                    If Not OfflineMode Then
                        SQLiteTableHashes = SQLiteComms.LocalTableHashList
                        RemoteTableHashes = SQLiteComms.RemoteTableHashList
                        Return SQLiteComms.CompareTableHashes(SQLiteTableHashes, RemoteTableHashes)
                    Else
                        Return True
                    End If
                Else
                    Return False
                End If
            End Using
        Catch ex As Exception
            Return False
        End Try
    End Function

End Module