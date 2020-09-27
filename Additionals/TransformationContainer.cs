using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Additionals
{
    /// <summary>
    /// Создает GraphicsContainer, осуществляет смещение начала координат и поворачивает холст на заданный угол
    /// </summary>
    public class TransformationContainer : IDisposable
    {
        Graphics Source = null;
        GraphicsContainer GC = null;
        int X = 0;
        int Y = 0;
        float Angle = 0;

        public TransformationContainer(Graphics g, int dx, int dy, float angle, SmoothingMode smoothingMode, TextRenderingHint textRenderingHint)
        {
            Source = g;
            GC = g.BeginContainer();
            g.SmoothingMode = smoothingMode;
            g.TextRenderingHint = textRenderingHint;
            //            g.PixelOffsetMode = PixelOffsetMode.HighQuality;
            X = dx;
            Y = dy;
            g.TranslateTransform(X, Y);
            Angle = angle;
            g.RotateTransform(Angle);
        }

        public TransformationContainer(Graphics g, int dx, int dy, float angle)
            : this(g, dx, dy, angle, SmoothingMode.HighQuality, TextRenderingHint.AntiAlias)
        {
        }

        public TransformationContainer(Graphics g, Point p, float angle)
            : this(g, p.X, p.Y, angle, SmoothingMode.HighQuality, TextRenderingHint.AntiAlias)
        {
        }

        #region IDisposable Members

        public void Dispose()
        {
            Source.EndContainer(GC);
        }

        #endregion
    }
}
