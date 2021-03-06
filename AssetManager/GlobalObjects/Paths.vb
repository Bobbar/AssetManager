﻿Imports System.Deployment.Application
Imports System.Environment

Namespace Paths
    Module Paths

        'Application paths
        Public ReadOnly AppDir As String = GetFolderPath(SpecialFolder.ApplicationData) & "\AssetManager\"

        Public Const LogName As String = "log.log"
        Public ReadOnly LogPath As String = AppDir & LogName
        Public ReadOnly DownloadPath As String = AppDir & "temp\"

        'SQLite DB paths

        Public ReadOnly Property SQLiteDBName() As String
            Get
                Return "cache_" & ServerInfo.CurrentDataBase.ToString & IIf(Not ApplicationDeployment.IsNetworkDeployed, "_DEBUG", "").ToString & ".db"
            End Get
        End Property

        Public ReadOnly Property SQLitePath() As String
            Get
                Return AppDir & "SQLiteCache\" & SQLiteDBName()
            End Get
        End Property

        Public ReadOnly SQLiteDir As String = AppDir & "SQLiteCache\"

        'Gatekeeper package paths
        Public Const GKInstallDir As String = "C:\PSi\Gatekeeper"

        Public Const GKPackFileName As String = "GatekeeperPack.gz"
        Public Const GKPackHashName As String = "hash.md5"
        Public ReadOnly GKPackFileFDir As String = AppDir & "GKUpdateFiles\PackFile\"
        Public ReadOnly GKPackFileFullPath As String = GKPackFileFDir & GKPackFileName
        Public ReadOnly GKExtractDir As String = AppDir & "GKUpdateFiles\Gatekeeper\"
        Public Const GKRemotePackFileDir As String = "\\core.co.fairfield.oh.us\dfs1\fcdd\files\Information Technology\Software\Other\GatekeeperPackFile\"
        Public Const GKRemotePackFilePath As String = GKRemotePackFileDir & GKPackFileName

    End Module
End Namespace