using AOC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2020_1_2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2020/day/1/input";

            var numbers = await InputHelper.GetInputLines<long>(inputUrl);
            long[] numbersWithSum = FindWithSum(3, 2020, numbers);
            Console.WriteLine(numbersWithSum[0] * numbersWithSum[1] * numbersWithSum[2]);
            Console.ReadKey();
        }

        private static long[] FindWithSum(int targetNumberCount, long sum, IReadOnlyList<long> numbers)
        {
            for (int i = 0; i < numbers.Count(); ++i)
            {
                if (targetNumberCount == 1)
                {
                    if (numbers[i] == sum)
                        return new long[] { sum };
                }
                else
                {
                    long[] _numbersFound = FindWithSum(targetNumberCount - 1, sum - numbers[i], numbers);
                    if (_numbersFound != default)
                    {
                        long[] numbersFound = new long[targetNumberCount];
                        numbersFound[0] = numbers[i];
                        _numbersFound.CopyTo(numbersFound, 1);
                        return numbersFound;
                    }
                }
            }
            return default;
        }
    }
}
