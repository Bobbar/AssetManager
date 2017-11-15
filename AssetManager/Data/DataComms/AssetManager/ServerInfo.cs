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
namespace AssetManager
{
	static class ServerInfo
	{
		public static bool ServerPinging = false;
		public const string MySQLServerIP = "10.10.0.89";

		private static Databases _currentDataBase = Databases.asset_manager;
		public static Databases CurrentDataBase {
			get { return _currentDataBase; }
			set {
				_currentDataBase = value;
				NetworkInfo.SetCurrentDomain(_currentDataBase);
			}
		}

	}
}
