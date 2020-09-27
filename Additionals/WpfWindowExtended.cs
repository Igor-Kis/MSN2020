using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Forms;
using System.Windows.Interop;

namespace Additionals
{
    public static class WpfWindowExtended
    {
        public enum ScreenPosition
        {
            LeftTop,
            LeftMiddle,
            LeftBottom,
            RightTop,
            RightMiddle,
            RightBottom,
            TopCenter,
            BottomCenter
        }

        static Point RealPixelsToWpf(Window w, Point p)
        {
            return p;

            //var t = PresentationSource.FromVisual(w).CompositionTarget.TransformFromDevice;
            //return t.Transform(p);
        }

        public static void SetWindowPosition(Window parent, Window window, ScreenPosition position, int padding)
        {
            double k = parent.PointToScreen(new Point(1, 0)).X - parent.PointToScreen(new Point(0, 0)).X;

            var screen = Screen.FromHandle(new WindowInteropHelper(parent).Handle);
            Point p = (parent.WindowState == WindowState.Maximized) ? new Point(screen.Bounds.Left / k, screen.Bounds.Top / k) : new Point(parent.Left, parent.Top);
            var workingArea = new System.Drawing.Rectangle((int)p.X, (int)p.Y, (int)parent.ActualWidth, (int)parent.ActualHeight);
            Point corner = new Point(0, 0);

            switch (position)
            {
                case ScreenPosition.LeftTop:
                    corner = RealPixelsToWpf(window, new Point(workingArea.Left + padding, workingArea.Top + padding));
                    break;
                case ScreenPosition.LeftMiddle:
                    corner = RealPixelsToWpf(window, new Point(workingArea.Left + padding, workingArea.Top + (double)workingArea.Height / 2 - window.Width / 2));
                    break;
                case ScreenPosition.LeftBottom:
                    corner = RealPixelsToWpf(window, new Point(workingArea.Left + padding, workingArea.Bottom - window.Width - padding));
                    break;
                case ScreenPosition.RightTop:
                    corner = RealPixelsToWpf(window, new Point(workingArea.Right - window.Width - padding, workingArea.Top + padding));
                    break;
                case ScreenPosition.RightMiddle:
                    corner = RealPixelsToWpf(window, new Point(workingArea.Right - window.Width - padding, workingArea.Top + (double)workingArea.Height / 2 - window.Height / 2));
                    break;
                case ScreenPosition.RightBottom:
                    corner = RealPixelsToWpf(window, new Point(workingArea.Right - window.Width - padding, workingArea.Bottom - window.Height - padding));
                    break;
                case ScreenPosition.TopCenter:
                    corner = RealPixelsToWpf(window, new Point((double)workingArea.Width / 2 - window.Width / 2, workingArea.Top + padding));
                    break;
                case ScreenPosition.BottomCenter:
                    corner = RealPixelsToWpf(window, new Point(workingArea.Left + (double)workingArea.Width / 2 - window.Width / 2, workingArea.Bottom - window.Height - padding));
                    break;
                default:
                    break;
            }
            window.Left = corner.X;
            window.Top = corner.Y;
        }
    }
}
