using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;

namespace Additionals
{
    /// <summary>
    /// Создает GraphicsContainer, осуществляет сглаживание
    /// </summary>
    public class SmoothingContainer : IDisposable
    {
        Graphics Source = null;
        GraphicsContainer GC = null;

        public SmoothingContainer(Graphics g, SmoothingMode smoothingMode, TextRenderingHint textRenderingHint)
        {
            Source = g;
            GC = g.BeginContainer();
            g.SmoothingMode = smoothingMode;
            g.TextRenderingHint = textRenderingHint;
        }

        public SmoothingContainer(Graphics g)
            : this(g, SmoothingMode.HighQuality, TextRenderingHint.AntiAlias)
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
