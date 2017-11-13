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
using System.Runtime.InteropServices;
namespace AssetManager
{
	public class FileIcon
	{
		private const Int32 MAX_PATH = 260;
		private const Int32 SHGFI_ICON = 0x100;
		private const Int32 SHGFI_USEFILEATTRIBUTES = 0x10;

		private const Int32 FILE_ATTRIBUTE_NORMAL = 0x80;
		private struct SHFILEINFO
		{
			public IntPtr hIcon;
			public Int32 iIcon;

			public Int32 dwAttributes;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = FileIcon.MAX_PATH)]

			public string szDisplayName;
			[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]

			public string szTypeName;
		}

		private enum IconSize
		{
			SHGFI_LARGEICON = 0,
			SHGFI_SMALLICON = 1
		}
		[DllImport("shell32.dll", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]

		private static extern IntPtr SHGetFileInfo(string pszPath, Int32 dwFileAttributes, ref SHFILEINFO psfi, Int32 cbFileInfo, Int32 uFlags);

		[DllImport("user32.dll", SetLastError = true)]
		private static extern bool DestroyIcon(IntPtr hIcon);

		public static Bitmap GetFileIcon(string fileExt)
		{
			//, Optional ByVal ICOsize As IconSize = IconSize.SHGFI_SMALLICON
			try {
				IconSize ICOSize = IconSize.SHGFI_SMALLICON;
				SHFILEINFO shinfo = new SHFILEINFO();
				shinfo.szDisplayName = new string(Strings.Chr(0), MAX_PATH);
				shinfo.szTypeName = new string(Strings.Chr(0), 80);
				SHGetFileInfo(fileExt, FILE_ATTRIBUTE_NORMAL, ref shinfo, Marshal.SizeOf(shinfo), SHGFI_ICON | ICOSize | SHGFI_USEFILEATTRIBUTES);
				Bitmap bmp = System.Drawing.Icon.FromHandle(shinfo.hIcon).ToBitmap();
				DestroyIcon(shinfo.hIcon);
				// must destroy icon to avoid GDI leak!
				return bmp;
				// return icon as a bitmap
			} catch (Exception ex) {
				ErrorHandling.ErrHandle(ex, System.Reflection.MethodInfo.GetCurrentMethod());
			}
			return null;
		}

	}
}
