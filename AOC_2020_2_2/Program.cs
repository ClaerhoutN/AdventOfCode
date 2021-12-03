using AOC.Util;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2020_2_2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2020/day/2/input";

            int amountValid = 0;
            foreach ((string positions, string character, string pw)
                in await InputHelper.GetInputLines<(string, string, string)>(inputUrl))
            {
                var positionsSplitted = positions.Split("-");
                int p1 = int.Parse(positionsSplitted[0]);
                int p2 = int.Parse(positionsSplitted[1]);

                char c = character[0];

                if ((pw[p1 - 1] == c && pw[p2 - 1] != c)
                    || (pw[p1 - 1] != c && pw[p2 - 1] == c))
                    ++amountValid;
            }
            Console.WriteLine(amountValid);
            Console.ReadKey();
        }
    }
}
