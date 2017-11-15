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
	static class GlobalSwitches
	{
		public static bool BuildingCache = false;
		public static bool ProgramEnding = false;

		public static bool CachedMode = false;
	}
}
