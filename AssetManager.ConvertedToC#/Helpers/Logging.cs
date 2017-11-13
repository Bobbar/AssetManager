using Microsoft.VisualBasic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Diagnostics;
using System.Windows.Forms;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
namespace AssetManager
{

	static class Logging
	{

		public static void Logger(string Message)
		{
			short MaxLogSizeKiloBytes = 500;
			string DateStamp = DateTime.Now.ToString();
			FileInfo infoReader = null;
			infoReader = AssetManager.My.MyProject.Computer.FileSystem.GetFileInfo(AssetManager.Paths.Paths.LogPath);
			if (!File.Exists(AssetManager.Paths.Paths.LogPath)) {
				Directory.CreateDirectory(AssetManager.Paths.Paths.AppDir);
				using (StreamWriter sw = File.CreateText(AssetManager.Paths.Paths.LogPath)) {
					sw.WriteLine(DateStamp + ": Log Created...");
					sw.WriteLine(DateStamp + ": " + Message);
				}
			} else {
				if ((infoReader.Length / 1000) < MaxLogSizeKiloBytes) {
					using (StreamWriter sw = File.AppendText(AssetManager.Paths.Paths.LogPath)) {
						sw.WriteLine(DateStamp + ": " + Message);
					}
				} else {
					if (RotateLogs()) {
						using (StreamWriter sw = File.AppendText(AssetManager.Paths.Paths.LogPath)) {
							sw.WriteLine(DateStamp + ": " + Message);
						}
					}
				}
			}
		}

		private static bool RotateLogs()
		{
			try {
				File.Copy(AssetManager.Paths.Paths.LogPath, AssetManager.Paths.Paths.LogPath + ".old", true);
				File.Delete(AssetManager.Paths.Paths.LogPath);
				return true;
			} catch (Exception ex) {
				return false;
			}
		}

	}
}
