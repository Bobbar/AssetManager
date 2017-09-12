

Public Class DBWrapper
    'TODO: Rename/move this to something more appropriate
    Public Function GetDatabase() As IDataBase
        If GlobalSwitches.CachedMode Then
            Return New SQLiteDatabase(False)
        Else
            Return New MySQLDatabase
        End If
    End Function

End Class