using AOC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AOC_2020_4_2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2020/day/4/input";

            string[] requiredPassportFields = new string[] { "byr", "iyr", "eyr", "hgt", "hcl", "ecl", "pid" };
            Dictionary<string, Predicate<string>> passportFieldRules = new Dictionary<string, Predicate<string>>
            {
                { "byr", v => int.TryParse(v, out int byr) && byr >= 1920 && byr <= 2002 },
                { "iyr", v => int.TryParse(v, out int iyr) && iyr >= 2010 && iyr <= 2020 },
                { "eyr", v => int.TryParse(v, out int eyr) && eyr >= 2020 && eyr <= 2030 },
                { "hgt", v =>
                    {
                        int nr = int.Parse(v.Substring(0, v.Length - 2));
                        string unit = v.Substring(v.Length - 2, 2);
                        return
                            (unit == "cm" && nr >= 150 && nr <= 193)
                            || (unit == "in" && nr >= 59 && nr <= 76);
                    } },
                { "hcl", v => Regex.IsMatch(v, "^#[0-9a-f]{6}$") },
                { "ecl", v => Regex.IsMatch(v, "^amb|blu|brn|gry|grn|hzl|oth$") },
                { "pid", v => Regex.IsMatch(v, "^0*\\d{9}$") }
            };

            int passportsValid = (await InputHelper.GetInputLines<string[]>(inputUrl,
                argumentSeparatorRegex: "[ \\n]", lineSeparatorRegex: "\\n\\n"))
                .Count(passportEntry =>
                {
                    return requiredPassportFields.All(x => passportEntry.Any(y =>
                    {
                        var (key, (value, _)) = y.Split(":");
                        return key == x && passportFieldRules[x](value);
                    }));
                });

            Console.WriteLine(passportsValid);
            Console.ReadKey();
        }
    }
}
