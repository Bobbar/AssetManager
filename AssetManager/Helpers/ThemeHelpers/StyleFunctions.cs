using System;
using System.Drawing;
using System.Windows.Forms;
namespace AssetManager
{
    static class StyleFunctions
    {
        public static DataGridViewCellStyle DefaultGridStyles;
        public static DataGridViewCellStyle AlternatingRowDefaultStyles;

        public static Font DefaultGridFont = new Font("Consolas", 9.75f, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, Convert.ToByte(0));

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

        public static void SetGridStyle(DataGridView grid)
        {
            grid.BackgroundColor = Colors.DefaultGridBackColor;
            grid.DefaultCellStyle = new DataGridViewCellStyle(DefaultGridStyles);
            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle(AlternatingRowDefaultStyles);
        }

        public static void SetGridStyle(DataGridView grid, GridTheme theme)
        {
            grid.BackgroundColor = Colors.DefaultGridBackColor;
            grid.DefaultCellStyle = new DataGridViewCellStyle(DefaultGridStyles);
            grid.AlternatingRowsDefaultCellStyle = new DataGridViewCellStyle(AlternatingRowDefaultStyles);
            grid.AlternatingRowsDefaultCellStyle.SelectionBackColor = theme.CellAltSelectColor;
            grid.DefaultCellStyle.SelectionBackColor = theme.CellSelectColor;
            grid.Font = DefaultGridStyles.Font;
        }

        public static void HighlightRow(DataGridView Grid, GridTheme Theme, int Row)
        {
            try
            {
                Color BackColor = Grid.CurrentRow.InheritedStyle.BackColor;
                Color SelectColor = Grid.CurrentRow.InheritedStyle.SelectionBackColor;

                if (Row > -1)
                {
                    Color c1 = Theme.RowHighlightColor;
                    Color c2 = SelectColor;

                    var BlendColorSelect = Color.FromArgb(Convert.ToInt32((Convert.ToInt32(c1.A) + Convert.ToInt32(c2.A)) / 2),
                         Convert.ToInt32((Convert.ToInt32(c1.R) + Convert.ToInt32(c2.R)) / 2),
                         Convert.ToInt32((Convert.ToInt32(c1.G) + Convert.ToInt32(c2.G)) / 2),
                         Convert.ToInt32((Convert.ToInt32(c1.B) + Convert.ToInt32(c2.B)) / 2));

                    c2 = Color.FromArgb(BackColor.R, BackColor.G, BackColor.B);
                    var BlendColorBack = Color.FromArgb(Convert.ToInt32((Convert.ToInt32(c1.A) + Convert.ToInt32(c2.A)) / 2),
                         Convert.ToInt32((Convert.ToInt32(c1.R) + Convert.ToInt32(c2.R)) / 2),
                         Convert.ToInt32((Convert.ToInt32(c1.G) + Convert.ToInt32(c2.G)) / 2),
                         Convert.ToInt32((Convert.ToInt32(c1.B) + Convert.ToInt32(c2.B)) / 2));

                    foreach (DataGridViewCell cell in Grid.Rows[Row].Cells)
                    {
                        cell.Style.SelectionBackColor = BlendColorSelect;
                        cell.Style.BackColor = BlendColorBack;
                    }

                }
            }
            catch
            {
            }
        }

        public static void LeaveRow(DataGridView Grid, GridTheme Theme, int Row)
        {
            Color BackColor = Grid.Rows[Row].InheritedStyle.BackColor;
            Color SelectColor = Grid.CurrentRow.InheritedStyle.SelectionBackColor;

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
