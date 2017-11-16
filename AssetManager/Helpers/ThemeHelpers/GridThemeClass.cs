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
    public class GridTheme
    {

        public GridTheme(Color highlightCol, Color cellSelCol, Color backCol)
        {
            RowHighlightColor = highlightCol;
            CellSelectColor = cellSelCol;
            BackColor = backCol;
        }

        public GridTheme(Color highlightCol, Color cellSelCol, Color cellAltSelCol, Color backCol)
        {
            RowHighlightColor = highlightCol;
            CellSelectColor = cellSelCol;
            CellAltSelectColor = cellAltSelCol;
            BackColor = backCol;
        }

        public GridTheme()
        {
        }

        public Color RowHighlightColor { get; set; }
        public Color CellSelectColor { get; set; }
        public Color CellAltSelectColor { get; set; }
        public Color BackColor { get; set; }

    }
}
