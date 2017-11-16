using System;
using System.Windows.Forms;
namespace AssetManager.UserInterface.CustomControls
{
    [Microsoft.VisualBasic.CompilerServices.DesignerGenerated()]
	partial class OneClickToolStrip : System.Windows.Forms.ToolStrip
	{

		//UserControl overrides dispose to clean up the component list.
		[System.Diagnostics.DebuggerNonUserCode()]
		protected override void Dispose(bool disposing)
		{
			try {
				if (disposing && components != null) {
					components.Dispose();
				}
			} finally {
				base.Dispose(disposing);
			}
		}
		const uint WM_LBUTTONDOWN = 0x201;
		const uint WM_LBUTTONUP = 0x202;

		private static bool down = false;
		protected override void WndProc(ref Message m)
		{
			if (m.Msg == WM_LBUTTONUP && !down) {
				m.Msg = Convert.ToInt32(WM_LBUTTONDOWN);
				base.WndProc(ref m);
				m.Msg = Convert.ToInt32(WM_LBUTTONUP);
			}

			if (m.Msg == WM_LBUTTONDOWN) {
				down = true;
			}
			if (m.Msg == WM_LBUTTONUP) {
				down = false;
			}

			base.WndProc(ref m);
		}
		//Required by the Windows Form Designer

		private System.ComponentModel.IContainer components;
		//NOTE: The following procedure is required by the Windows Form Designer
		//It can be modified using the Windows Form Designer.  
		//Do not modify it using the code editor.
		[System.Diagnostics.DebuggerStepThrough()]
		private void InitializeComponent()
		{
			components = new System.ComponentModel.Container();
			// Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
		}

	}
}
