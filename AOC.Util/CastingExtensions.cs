using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC.Util
{
    public static class CastingExtensions
    {
        public static bool ToBool(this int i)
        {
            return i == 0 ? false : true;
        }

        public static T[,] ToMultiDim<T>(this T[] array, int rows)
        {
            T[,] multiDimArray = new T[rows, array.Length / rows];
            for(int i = 0; i < array.Length; ++i)
            {
                multiDimArray[i / rows, i % rows] = array[i];
            }
            return multiDimArray;
        }
    }
}
