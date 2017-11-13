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
	public static class DBFactory
	{

		public static IDataBase GetDatabase()
		{
			if (AssetManager.GlobalSwitches.GlobalSwitchDeclarations.CachedMode) {
				return new SQLiteDatabase(false);
			} else {
				return new MySQLDatabase();
			}
		}

	}
}
