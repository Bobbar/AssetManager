Module GlobalVariables
    Public ProgramEnding As Boolean = False
    Public ServerPinging As Boolean = True
    Public CurrentDB As String
    Public OfflineMode As Boolean = False
    Public BuildingCache As Boolean = False
    Public CacheAvailable As Boolean = False
    Public SQLiteTableHashes As List(Of String)
    Public RemoteTableHashes As List(Of String)
End Module