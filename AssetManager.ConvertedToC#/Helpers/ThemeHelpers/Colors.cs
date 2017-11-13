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
	static class Colors
	{
		public static Color MissingField { get; }
		//"#82C1FF") '"#FF9827") '"#75BAFF")
		public static Color CheckIn { get; }
		public static Color CheckOut { get; }
		public static Color HighlightBlue { get; }
		//ColorTranslator.FromHtml("#8BCEE8")
		public static Color SibiSelectColor { get; }
		//(146, 148, 255) '(31, 47, 155)
		public static Color OrangeHighlightColor { get; }
		public static Color OrangeSelectColor { get; }
		public static Color EditColor { get; }
		public static Color DefaultFormBackColor { get; }
		public static Color StatusBarProblem { get; }
		public static Color AssetToolBarColor { get; }
		public static Color SibiToolBarColor { get; }
		//(148, 213, 255)
		public static Color DefaultGridBackColor { get; set; }
		public static Color DefaultGridSelectColor { get; set; }
	}
}
