using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Globalization;

namespace Additionals
{
    public static class StringExtended
    {
        
        public static List<int> GetIntList(this string str, char splitter = ';')
        {
            List<int> ints = new List<int>();

            if (str != "")
            {
                string[] strs = str.Split(splitter);
                foreach (var item in strs)
                {
                    int value = -1;
                    if (int.TryParse(item, out value))
                    {
                        ints.Add(value);
                    }
                }
            }
            return ints;
        }
        public static double[] GetDoubleArray(this string str, char splitter = ';')
        {
            if (str != "")
            {
                string[] strs = str.Split(splitter);
                double[] ints = new double[strs.Length];
                int pos = 0;
                foreach (var item in strs)
                {
                    double value = -1;
                    if (double.TryParse(item, NumberStyles.Any, CultureInfo.InvariantCulture, out value))
                    {
                        ints[pos] = value;
                    }
                    pos++;
                }
                return ints;
            }
            else return null;
        }
    }
}
