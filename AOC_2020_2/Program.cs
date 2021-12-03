using AOC.Util;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2020_2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2020/day/2/input";

            int amountValid = 0;
            foreach ((string minMax, string character, string pw) 
                in await InputHelper.GetInputLines<(string, string, string)>(inputUrl))
            {
                var minMaxSplitted = minMax.Split("-");
                int min = int.Parse(minMaxSplitted[0]);
                int max = int.Parse(minMaxSplitted[1]);

                char c = character[0];

                int count = pw.Count(x => x == c);
                if (count >= min && count <= max)
                    ++amountValid;
            }
            Console.WriteLine(amountValid);
            Console.ReadKey();
        }
    }
}
