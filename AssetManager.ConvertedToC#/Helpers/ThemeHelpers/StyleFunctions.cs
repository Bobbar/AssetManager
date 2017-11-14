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
    static class StyleFunctions
    {
        public static DataGridViewCellStyle GridStyles;

        public static Font GridFont = new Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));
        public static Color SetBarColor(string UID)
        {
            int hash = UID.GetHashCode();
            int r = 0;
            int g = 0;
            int b = 0;
            r = (hash & 0xff0000) >> 16;
            g = (hash & 0xff00) >> 8;
            b = hash & 0xff;
            return Color.FromArgb(r, g, b);
        }

        public static Color GetFontColor(Color color)
        {
            //get contrasting font color
            int d = 0;
            double a = 0;
            a = 1 - (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255;
            if (a < 0.5)
            {
                d = 0;
            }
            else
            {
                d = 255;
            }
            return Color.FromArgb(d, d, d);
        }

        public static void SetGridStyle(DataGridView Grid)
        {
            Grid.BackgroundColor = Colors.DefaultGridBackColor;
            Grid.DefaultCellStyle = GridStyles;
            Grid.DefaultCellStyle.Font = GridFont;
        }

        public static void HighlightRow(DataGridView Grid, GridTheme Theme, int Row)
        {
            try
            {
                Color BackColor = Theme.BackColor;
                //Colors.DefGridBC
                Color SelectColor = Theme.CellSelectColor;
                //Colors.DefGridSelCol
                Color c1 = Theme.RowHighlightColor;
                //Colors.colHighlightColor 'highlight color
                if (Row > -1)
                {
                    foreach (DataGridViewCell cell in Grid.Rows[Row].Cells)
                    {
                        Color c2 = Color.FromArgb(SelectColor.R, SelectColor.G, SelectColor.B);
                        Color BlendColor = default(Color);
                        BlendColor = Color.FromArgb(Convert.ToInt32((Convert.ToInt32(c1.A) + Convert.ToInt32(c2.A)) / 2), Convert.ToInt32((Convert.ToInt32(c1.R) + Convert.ToInt32(c2.R)) / 2), Convert.ToInt32((Convert.ToInt32(c1.G) + Convert.ToInt32(c2.G)) / 2), Convert.ToInt32((Convert.ToInt32(c1.B) + Convert.ToInt32(c2.B)) / 2));
                        cell.Style.SelectionBackColor = BlendColor;
                        c2 = Color.FromArgb(BackColor.R, BackColor.G, BackColor.B);
                        BlendColor = Color.FromArgb(Convert.ToInt32((Convert.ToInt32(c1.A) + Convert.ToInt32(c2.A)) / 2), Convert.ToInt32((Convert.ToInt32(c1.R) + Convert.ToInt32(c2.R)) / 2), Convert.ToInt32((Convert.ToInt32(c1.G) + Convert.ToInt32(c2.G)) / 2), Convert.ToInt32((Convert.ToInt32(c1.B) + Convert.ToInt32(c2.B)) / 2));
                        cell.Style.BackColor = BlendColor;
                    }
                }
            }
            catch
            {
            }
        }

        public static void LeaveRow(DataGridView Grid, GridTheme Theme, int Row)
        {
            Color BackColor = Theme.BackColor;
            Color SelectColor = Theme.CellSelectColor;
            if (Row > -1)
            {
                foreach (DataGridViewCell cell in Grid.Rows[Row].Cells)
                {
                    cell.Style.SelectionBackColor = SelectColor;
                    cell.Style.BackColor = BackColor;
                }
            }
        }

    }
}
