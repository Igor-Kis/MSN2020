using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;

namespace Additionals
{

    public static class GirColors
    {

        public static System.Windows.Media.SolidColorBrush GetBrushFromColor(System.Drawing.Color color)
        {
            System.Windows.Media.Color colorB = new System.Windows.Media.Color();
            colorB.A = color.A;
            colorB.R = color.R;
            colorB.G = color.G;
            colorB.B = color.B;
            System.Windows.Media.SolidColorBrush brush = new System.Windows.Media.SolidColorBrush(colorB);
            return brush;
        }
        public static System.Drawing.Color GetWFColorFromWPFColor(System.Windows.Media.Color color)
        {
            System.Drawing.Color colorWF =  System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B);
            return colorWF;
        }

        public static System.Windows.Media.Color ColorToGray(System.Windows.Media.Color clr, double sh, bool isDarkIfLight)
        {
            //сдвигаем по перпендикуляру к точке на диагонали RGB (0 - цвет тот же, 1 - серый)
            double shift = Math.Max(Math.Min(sh, 1), 0);
            double gr = 1.0 * (clr.R + clr.G + clr.B) / 3;

            if (isDarkIfLight && gr > 200)
                gr = gr-100;

            byte r = (byte)(gr * shift + clr.R * (1 - shift));
            byte g = (byte)(gr * shift + clr.G * (1 - shift)); 
            byte b = (byte)(gr * shift + clr.B * (1 - shift));
            return System.Windows.Media.Color.FromArgb(clr.A, r, g, b);
        }

        public static System.Drawing.Color ColorToGray(System.Drawing.Color clr, double sh, bool isDarkIfLight)
        {
            //сдвигаем по перпендикуляру к точке на диагонали RGB (0 - цвет тот же, 1 - серый)
            double shift = Math.Max(Math.Min(sh, 1), 0);
            double gr = 1.0 * (clr.R + clr.G + clr.B) / 3;

            if (isDarkIfLight && gr > 200)
                gr = gr - 100;

            byte r = (byte)(gr * shift + clr.R * (1 - shift));
            byte g = (byte)(gr * shift + clr.G * (1 - shift));
            byte b = (byte)(gr * shift + clr.B * (1 - shift));
            return System.Drawing.Color.FromArgb(clr.A, r, g, b);
        }

        public static System.Windows.Media.Color ColorToLightGray(System.Windows.Media.Color clr, double sh)
        {
            //сдвигаем по перпендикуляру к точке на диагонали RGB (0 - цвет тот же, 1 - серый)
            double shift = Math.Max(Math.Min(sh, 1), 0);
            double gr = 1.0 * (clr.R + clr.G + clr.B) / 3;

            gr = Math.Min(255, gr + 100);
            
            byte r = (byte)(gr * shift + clr.R * (1 - shift));
            byte g = (byte)(gr * shift + clr.G * (1 - shift));
            byte b = (byte)(gr * shift + clr.B * (1 - shift));
            return System.Windows.Media.Color.FromArgb(clr.A, r, g, b);
        }

        public static Color GetColorA(Color c, int a)
        {
            return Color.FromArgb(a, c.R, c.G, c.B);
        }
    }
}
