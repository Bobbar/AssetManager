Namespace DBCache
    Module DBCacheFunctions

        Public SQLiteTableHashes As List(Of String)
        Public RemoteTableHashes As List(Of String)

        Public Sub RefreshLocalDBCache()
            Try
                ' GlobalSwitches.BuildingCache = True
                Using conn As New SQLiteDatabase(False)
                    conn.RefreshSqlCache()
                End Using
            Catch ex As Exception
                ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod())
            Finally
                GlobalSwitches.BuildingCache = False
            End Try
        End Sub

        ''' <summary>
        ''' Builds local hash list and compares to previously built remote hash list. Returns False for mismatch.
        ''' </summary>
        ''' <param name="cachedMode">When true, only checks for Schema Version since a remote table hash will likely be unavailabe.</param>
        ''' <returns></returns>
        Public Async Function VerifyLocalCacheHashOnly(cachedMode As Boolean) As Task(Of Boolean)
            If Not cachedMode Then
                If RemoteTableHashes Is Nothing Then Return False
                Return Await Task.Run(Function()
                                          Dim LocalHashes As New List(Of String)
                                          Using SQLiteComms As New SQLiteDatabase
                                              LocalHashes = SQLiteComms.LocalTableHashList
                                              Return SQLiteComms.CompareTableHashes(LocalHashes, RemoteTableHashes)
                                          End Using
                                      End Function)
            Else
                Using SQLiteComms As New SQLiteDatabase
                    If SQLiteComms.GetSchemaVersion > 0 Then Return True
                End Using
            End If
            Return False
        End Function

        ''' <summary>
        ''' Builds hash lists for both local and remote tables and compares them.  Returns False for mismatch.
        ''' </summary>
        ''' <param name="connectedToDB"></param>
        ''' <returns></returns>
        Public Function VerifyCacheHashes(Optional connectedToDB As Boolean = True) As Boolean
            Try
                Using SQLiteComms As New SQLiteDatabase
                    If SQLiteComms.GetSchemaVersion > 0 Then
                        If connectedToDB Then
                            SQLiteTableHashes = SQLiteComms.LocalTableHashList
                            RemoteTableHashes = SQLiteComms.RemoteTableHashList
                            Return SQLiteComms.CompareTableHashes(SQLiteTableHashes, RemoteTableHashes)
                        Else
                            Return True
                        End If
                    Else
                        SQLiteTableHashes = Nothing
                        Return False
                    End If
                End Using
            Catch ex As Exception
                Return False
            End Try
        End Function

    End Module
End Namespace