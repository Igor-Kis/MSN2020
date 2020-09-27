using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Additionals
{
    /// <summary>
    /// Быстрая инициализация массива значением или повторяемыми последовательностями значений
    /// </summary>
    public class FastArray
    {
        public static void ArrayFill<T>(T[] arrayToFill, T fillValue)
        {
            // if called with a single value, wrap the value in an array and call the main function
            ArrayFill(arrayToFill, new T[] { fillValue });
        }

        public static void ArrayFill<T>(T[] arrayToFill, T[] fillValue)
        {
            if (fillValue.Length >= arrayToFill.Length)
            {
                throw new ArgumentException("fillValue array length must be smaller than length of arrayToFill");
            }

            // set the initial array value
            Array.Copy(fillValue, arrayToFill, fillValue.Length);

            int arrayToFillHalfLength = arrayToFill.Length / 2;

            for (int i = fillValue.Length; i < arrayToFill.Length; i *= 2)
            {
                int copyLength = i;
                if (i > arrayToFillHalfLength)
                {
                    copyLength = arrayToFill.Length - i;
                }

                Array.Copy(arrayToFill, 0, arrayToFill, i, copyLength);
            }
        }
    }
}
