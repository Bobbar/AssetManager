using System;
using System.Windows.Forms;
namespace AssetManager
{
    public class CorrectedTreeView : TreeView
    {

        protected override void WndProc(ref Message m)
        {
            // Suppress WM_LBUTTONDBLCLK
            // Fixes bug that has existed since Vista...  (╯°□°)╯︵ ┻━┻
            if (m.Msg == 0x203)
            {
                m.Result = IntPtr.Zero;
            }
            else
            {
                base.WndProc(ref m);
            }
        }

    }
}
