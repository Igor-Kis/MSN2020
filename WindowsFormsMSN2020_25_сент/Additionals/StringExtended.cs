using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

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
    }
}
