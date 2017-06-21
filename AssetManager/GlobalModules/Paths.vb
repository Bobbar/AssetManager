Imports System.Environment
Module Paths
    Public strAppDir As String = GetFolderPath(SpecialFolder.ApplicationData) & "\AssetManager\"
    Public strLogName As String = "log.log"
    Public strLogPath As String = strAppDir & strLogName
    Public DownloadPath As String = strAppDir & "temp\"


    Public ReadOnly strSQLiteDBName As String = "cache.db"
    Public ReadOnly strSQLitePath As String = strAppDir & "SQLiteCache\" & strSQLiteDBName
    Public ReadOnly strSQLiteDir As String = strAppDir & "SQLiteCache\"

End Module
