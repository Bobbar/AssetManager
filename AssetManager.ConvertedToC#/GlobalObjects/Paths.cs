using System;
using System.Deployment.Application;

namespace AssetManager
{
	static class Paths
	{

		//Application paths

		public static readonly string AppDir = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\AssetManager\\";
		public const string LogName = "log.log";
		public static readonly string LogPath = AppDir + LogName;

		public static readonly string DownloadPath = AppDir + "temp\\";
		//SQLite DB paths

		public static string SQLiteDBName {
			get { return "cache_" + ServerInfo.CurrentDataBase.ToString() + (!ApplicationDeployment.IsNetworkDeployed ? "_DEBUG" : "").ToString() + ".db"; }
		}

		public static string SQLitePath {
			get { return AppDir + "SQLiteCache\\" + SQLiteDBName; }
		}


		public static readonly string SQLiteDir = AppDir + "SQLiteCache\\";
		//Gatekeeper package paths

		public const string GKInstallDir = "C:\\PSi\\Gatekeeper";
		public const string GKPackFileName = "GatekeeperPack.gz";
		public const string GKPackHashName = "hash.md5";
		public static readonly string GKPackFileFDir = AppDir + "GKUpdateFiles\\PackFile\\";
		public static readonly string GKPackFileFullPath = GKPackFileFDir + GKPackFileName;
		public static readonly string GKExtractDir = AppDir + "GKUpdateFiles\\Gatekeeper\\";
		public const string GKRemotePackFileDir = "\\\\core.co.fairfield.oh.us\\dfs1\\fcdd\\files\\Information Technology\\Software\\Other\\GatekeeperPackFile\\";

		public const string GKRemotePackFilePath = GKRemotePackFileDir + GKPackFileName;
	}
}
