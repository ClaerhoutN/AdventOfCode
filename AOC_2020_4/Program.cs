using AOC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC_2020_4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2020/day/4/input";

            string[] requiredPassportFields = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            int passportsValid = (await InputHelper.GetInputLines<string[]>(inputUrl,
                argumentSeparatorRegex: "[ \\n]", lineSeparatorRegex: "\\n\\n"))
                .Count(passportEntry =>
                {
                    return requiredPassportFields.All(x => passportEntry.Any(y => y.StartsWith(x + ":")));
                });

            Console.WriteLine(passportsValid);
            Console.ReadKey();
        }
    }
}
