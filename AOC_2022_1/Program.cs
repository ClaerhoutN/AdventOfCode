using AOC.Util;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2022_1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2022/day/1/input";

            int maxCaloriesSum = (await InputHelper.GetInputLines<int[]>(inputUrl, "\n", "\n\n"))
                .Select(x => x.Sum())
                .OrderByDescending(x => x)
                .Take(3)
                .Sum();
            Console.WriteLine(maxCaloriesSum);
        }
    }
}
