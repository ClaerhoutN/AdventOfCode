using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC.Util
{
    public static class IEnumerableExtensions
    {
        public static void ForEach<T>(this IEnumerable<T> collection, Action<int, T, Action> action)
        {
            int i = 0;
            foreach(var el in collection)
            {
                bool @break = false;
                Action fcBreak = () => { @break = true; };
                action(i, el, fcBreak);
                if (@break)
                    break;
                ++i;
            }
        }
        public static U Aggregate<T, U>(this IEnumerable<T> collection, Func<int, U, T, U> func, U seed = default)
        {
            U result = seed;
            int i = 0;
            foreach (var el in collection)
            {
                result = func(i, result, el);
                ++i;
            }
            return result;
        }
        public static void Deconstruct<T>(this Span<T> span, out T first, out Span<T> rest)
        {
            first = span.Length > 0 ? span[0] : default(T);
            rest = span.Length > 0 ? span.Slice(1) : span.Slice(0);
        }

        public static void Deconstruct<T>(this T[] arr, out T first, out Span<T> rest)
        {
            first = arr.Length > 0 ? arr[0] : default(T);
            rest = arr.Length > 0 ? arr.AsSpan(1) : arr.AsSpan(0);
        }

        public static int Sum(this int[,] array)
        {
            int sum = 0;
            for (int i = 0; i < array.GetLength(0); ++i)
            {
                for (int j = 0; j < array.GetLength(1); ++j)
                {
                    sum += array[i, j];
                }
            }
            return sum;
        }
        public static int SumWhere(this int[,] array, Predicate<int> condition)
        {
            int sum = 0;
            for (int i = 0; i < array.GetLength(0); ++i)
            {
                for (int j = 0; j < array.GetLength(1); ++j)
                {
                    if(condition(array[i, j]))
                        sum += array[i, j];
                }
            }
            return sum;
        }
    }
    public static class Range
    {
        public static IEnumerable<(int, int)> FromDimensions(int a, int b)
        {
            return Enumerable.Range(0, a).SelectMany(row => Enumerable.Range(0, b).Select(col => (row, col)));
        }
    }
}
