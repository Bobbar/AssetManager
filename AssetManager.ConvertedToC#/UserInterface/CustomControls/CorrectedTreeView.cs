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
	public class CorrectedTreeView : TreeView
	{

		protected override void WndProc(ref Message m)
		{
			// Suppress WM_LBUTTONDBLCLK
			// Fixes bug that has existed since Vista...  (╯°□°)╯︵ ┻━┻
			if (m.Msg == 0x203) {
				m.Result = IntPtr.Zero;
			} else {
				base.WndProc(ref m);
			}
		}

	}
}
