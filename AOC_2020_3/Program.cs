using AOC.Util;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2020_3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2020/day/3/input";

            int treeCount = (await InputHelper.GetInputLines<string>(inputUrl))
                .Skip(1).Aggregate<string, int>((index, count, line) => line[((index + 1) * 3) % line.Length] == '#' ? count + 1 : count);
            Console.WriteLine(treeCount);
            Console.ReadKey();
        }
    }
}
