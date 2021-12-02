using AOC.Util;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2021_2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2021/day/2/input";

            long x = 0, y = 0;
            foreach ((string direction, int amount) in (await InputHelper.GetInputLines<string>(inputUrl))
                .Select(x =>
                {
                    string[] splittedCommand = x.Split(" ");
                    return (splittedCommand[0], int.Parse(splittedCommand[1]));
                }))
            {
                switch(direction)
                {
                    case "forward":
                        x += amount;
                        break;
                    case "down":
                        y -= amount;
                        break;
                    case "up":
                        y += amount;
                        break;
                }
            }
            Console.WriteLine(x * -y);
            Console.ReadKey();
        }
    }
}
