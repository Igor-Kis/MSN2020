using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Additionals
{
    public class SecondScreen
    {
        public static Screen Get (IntPtr hWnd)
        {
            Screen mainWindowScreen = Screen.FromHandle(hWnd);
            Screen prcScreen = mainWindowScreen;
            if (Screen.AllScreens.Length > 1)
            {
                foreach (var screen in Screen.AllScreens)
                {
                    if (screen.GetHashCode() != mainWindowScreen.GetHashCode())
                    {
                        prcScreen = screen;
                        break;
                    }
                }
            }
            return prcScreen;
        }
    }
}
