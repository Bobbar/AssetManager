Imports System.Environment
Module Paths
    'Application paths
    Public ReadOnly strAppDir As String = GetFolderPath(SpecialFolder.ApplicationData) & "\AssetManager\"
    Public ReadOnly strLogName As String = "log.log"
    Public ReadOnly strLogPath As String = strAppDir & strLogName
    Public ReadOnly DownloadPath As String = strAppDir & "temp\"

    'SQLite DB paths
    Public ReadOnly strSQLiteDBName As String = "cache.db"
    Public ReadOnly strSQLitePath As String = strAppDir & "SQLiteCache\" & strSQLiteDBName
    Public ReadOnly strSQLiteDir As String = strAppDir & "SQLiteCache\"

    'Gatekeeper package paths
    Public ReadOnly GKInstallDir As String = "C:\PSi\Gatekeeper"
    Public ReadOnly GKPackFileName As String = "GatekeeperPack.gz"
    Public ReadOnly GKPackHashName As String = "hash.md5"
    Public ReadOnly GKPackFileFDir As String = strAppDir & "GKUpdateFiles\PackFile\"
    Public ReadOnly GKPackFileFullPath As String = GKPackFileFDir & GKPackFileName
    Public ReadOnly GKExtractDir As String = strAppDir & "GKUpdateFiles\Gatekeeper\"
    Public ReadOnly GKRemotePackFileDir As String = "\\core.co.fairfield.oh.us\dfs1\fcdd\files\Information Technology\Software\Other\GatekeeperPackFile\"
    Public ReadOnly GKRemotePackFilePath As String = GKRemotePackFileDir & GKPackFileName

End Module
