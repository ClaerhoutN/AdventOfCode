using AOC.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2020_1
{
    /*class SumComparer : IEqualityComparer<int>
    {
        private readonly int _targetSum;
        public SumComparer(int targetSum)
        {
            _targetSum = targetSum;
        }
        public bool Equals(int x, int y)
        {
            return x + y == _targetSum;
        }

        public int GetHashCode([DisallowNull] int obj)
        {
            return obj.GetHashCode();
        }
    }*/
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2020/day/1/input";

            var numbers = (await InputHelper.GetInputLines<int>(inputUrl)).ToArray();
            int n1 = 0, n2 = 0;
            for (int i = 1; i < numbers.Length; ++i)
            {
                n1 = numbers[i];
                n2 = new ArraySegment<int>(numbers, 0, i).FirstOrDefault(x => x + n1 == 2020);
                if(n2 != default)
                {
                    break;
                }
            }

            Console.WriteLine(n1 * n2);
            Console.ReadKey();
        }
    }
}
