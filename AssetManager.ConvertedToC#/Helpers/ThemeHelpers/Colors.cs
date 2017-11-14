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
        public static Color MissingField { get; } = ColorTranslator.FromHtml("#ffcccc");
        //"#82C1FF") '"#FF9827") '"#75BAFF")
        public static Color CheckIn { get; } = ColorTranslator.FromHtml("#B6FCC0");
        public static Color CheckOut { get; } = ColorTranslator.FromHtml("#FCB6B6");
        public static Color HighlightBlue { get; } = Color.FromArgb(46, 112, 255);
        //ColorTranslator.FromHtml("#8BCEE8")
        public static Color SibiSelectColor { get; } = Color.FromArgb(185, 205, 255);
        //(146, 148, 255) '(31, 47, 155)
        public static Color OrangeHighlightColor { get; } = ColorTranslator.FromHtml("#FF6600");
        public static Color OrangeSelectColor { get; } = ColorTranslator.FromHtml("#FFB917");
        public static Color EditColor { get; } = ColorTranslator.FromHtml("#81EAAA");
        public static Color DefaultFormBackColor { get; } = Color.FromArgb(232, 232, 232);
        public static Color StatusBarProblem { get; } = ColorTranslator.FromHtml("#FF9696");
        public static Color AssetToolBarColor { get; } = Color.FromArgb(249, 226, 166);
        public static Color SibiToolBarColor { get; } = Color.FromArgb(185, 205, 255);
        //(148, 213, 255)
        public static Color DefaultGridBackColor { get; set; }
        public static Color DefaultGridSelectColor { get; set; }
    }
}
