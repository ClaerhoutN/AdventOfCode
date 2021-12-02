using System;
using System.IO;
using System.Linq;

namespace AOC_2021_2
{
    class Program
    {
        static void Main(string[] args)
        {
            long x = 0, y = 0;
            foreach ((string direction, int amount) in File.ReadAllLines("../../../../inputs/2021_2.txt")
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
