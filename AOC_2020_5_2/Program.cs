using AOC.Util;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2020_5_2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2020/day/5/input";

            long maxSeatId = -1;
            foreach (long seatId in (await InputHelper.GetInputLines<string>(inputUrl))
                .Select(x =>
                {
                    x = x
                    .Replace('F', '0')
                    .Replace('B', '1')
                    .Replace('L', '0')
                    .Replace('R', '1');                    
                    long row = Convert.ToInt64(x.Substring(0, 7), 2);
                    long col = Convert.ToInt64(x.Substring(7, 3), 2);                    
                    return row * 8 + col;
                })
                .OrderBy(x => x))
            {
                if (maxSeatId != -1 && seatId - maxSeatId == 2)
                {
                    maxSeatId = seatId - 1;
                    break;
                }
                else
                    maxSeatId = seatId;
            }

            Console.WriteLine(maxSeatId);
            Console.ReadKey();
        }
    }
}
