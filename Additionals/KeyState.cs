using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Additionals
{
    public class KeyState
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true)]
        internal static extern short GetKeyState(int virtualKeyCode);
        public static bool GetKeyState(Keys key)
        {
            if ((GetKeyState((int)key) & 0xfffe) != 0)
            {
                return true;
            }
            return false;
        }
    }
}
