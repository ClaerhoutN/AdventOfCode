using AOC.Util;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2021_3_2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2021/day/3/input";

            var inputLines = await InputHelper.GetInputLines<string>(inputUrl);

            string oxygenGen = GetByBitwiseSum(inputLines, 0, x =>
            {
                if (x >= 0)
                    return '1';
                else return '0';
            }, 12);

            string co2Scrubber = GetByBitwiseSum(inputLines, 0, x =>
            {
                if (x >= 0)
                    return '0';
                else return '1';
            }, 12);
            
            Console.WriteLine(Convert.ToUInt64(oxygenGen, 2) * Convert.ToUInt64(co2Scrubber, 2));
            Console.ReadKey();
        }

        private static string GetByBitwiseSum(IEnumerable<string> numbers, int startingBitPosition, Func<int, char> fcKeep, int bitCount)
        {
            int bitwiseSum = numbers.Sum(x => x[startingBitPosition] == '0' ? -1 : 1);

            var remainingNumbers = numbers.Where(x => x[startingBitPosition] == fcKeep(bitwiseSum)).ToList();
            if (startingBitPosition >= bitCount-1 || remainingNumbers.Count() == 1)
                return remainingNumbers.FirstOrDefault();
            return GetByBitwiseSum(remainingNumbers
                , startingBitPosition + 1, fcKeep, bitCount);
        }
    }
}
