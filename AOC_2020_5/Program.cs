using AOC.Util;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2020_5
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2020/day/5/input";

            string maxSeatName = (await InputHelper.GetInputLines<string>(inputUrl))
                .Select(x => x
                    .Replace('F', '0')
                    .Replace('B', '1')
                    .Replace('L', '0')
                    .Replace('R', '1'))
                .Max();            
            long row = Convert.ToInt64(maxSeatName.Substring(0, 7), 2);
            long col = Convert.ToInt64(maxSeatName.Substring(7, 3), 2);
            long maxSeatId = row * 8 + col;

            Console.WriteLine(maxSeatId);
            Console.ReadKey();
        }
    }
}
