using AOC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2020_6
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2020/day/6/input";

            List<int> yesAnswers = (await InputHelper.GetInputLines<string>(inputUrl, lineSeparatorRegex: "\\n\\n"))
                .Select(x => x.Where(y => y != '\n').Distinct().Count())
                .ToList();


            Console.WriteLine(yesAnswers.Sum());
            Console.ReadKey();
        }
    }
}
