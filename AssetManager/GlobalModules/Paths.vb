Imports System.Environment
Module Paths
    Public strLogDir As String = GetFolderPath(SpecialFolder.ApplicationData) & "\AssetManager\"
    Public strLogName As String = "log.log"
    Public strLogPath As String = strLogDir & strLogName
    Public DownloadPath As String = strLogDir & "temp\"
End Module
