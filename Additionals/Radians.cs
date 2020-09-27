using System;
using System.Collections.Generic;
using System.Text;

namespace Additionals
{
    public class Radians
    {
        public static double RadiansToDegries(double radians)
        {
            return (radians * 180) / Math.PI;
        }

        public static double DegriesToRadians(double degries)
        {
            return (Math.PI * degries) / 180;
        }
    }
}
