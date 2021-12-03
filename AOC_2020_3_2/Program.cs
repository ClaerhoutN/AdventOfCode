using AOC.Util;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2020_3_2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2020/day/3/input";
            
            var lines = (await InputHelper.GetInputLines<string>(inputUrl));

            int[,] slopes = new int[,] { { 1, 1 }, { 3, 1 }, { 5, 1 }, { 7, 1 }, { 1, 2 } };
            long treesEncountered = 1;
            for(int i = 0; i < slopes.GetLength(0); ++i)
            {
                treesEncountered *= lines.Skip(1).Aggregate<string, long>((index, count, line) =>
                    {
                        return (index + 1) % slopes[i, 1] == 0 
                        && line[((index / slopes[i, 1] + 1) * slopes[i, 0]) % line.Length] == '#' 
                        ? count + 1 
                        : count;
                    });
            }
            Console.WriteLine(treesEncountered);
            Console.ReadKey();
        }
    }
}
