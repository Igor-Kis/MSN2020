using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Additionals
{
    public static class IntegerExtended
    {
        public static bool GetBitValue(this int i, int bitNumber)
        {
            return (i & (1 << bitNumber)) != 0;
        }

        public static int SetBitValue(this int i, int bitNumber, bool bitValue)
        {
            if (bitValue)
                i = i | (1 << bitNumber);
            else
                i = i ^ (1 << bitNumber);
            return i;
        }

        public static string ToSeparatedString(this List<int> list, char separator = ';')
        {
            string result = "";
            foreach (var item in list)
            {
                result = result + ((result == "") ? "" : separator.ToString()) + item.ToString();
            }
            return result;
        }

        public static string ToSeparatedString(this int[] array, char separator = ';')
        {
            string result = "";
            foreach (var item in array)
            {
                result = result + ((result == "") ? "" : separator.ToString()) + item.ToString();
            }
            return result;
        }

    }
}
