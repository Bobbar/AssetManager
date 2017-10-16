Namespace DBFactory
    Public Module DBFactory

        Public Function GetDatabase() As IDataBase
            If GlobalSwitches.CachedMode Then
                Return New SQLiteDatabase(False)
            Else
                Return New MySQLDatabase
            End If
        End Function

    End Module
End Namespace