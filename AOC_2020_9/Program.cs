using AOC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2020_9
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2020/day/9/input";

            var numbers = await InputHelper.GetInputLines<long>(inputUrl);

            long wrongNumber = numbers.SlidingWindows(26, 1).First(x =>
                ! x.Take(25).Permutations(2).Any(y => y.Sum() == x.Last())).Last();
            List<long> firstWindowSummingToWrongNumber = null;
            numbers.ForEach((i, n, @break) =>
            {
                if(numbers.Skip(i+1).SumWhile(out int count, sumCondition: (s => s < wrongNumber)) == wrongNumber)
                {
                    firstWindowSummingToWrongNumber = numbers.Skip(i + 1).Take(count).ToList();
                    @break();
                }

            });

            Console.WriteLine("number: " + wrongNumber);
            Console.WriteLine("encryption weakness: " 
                + (firstWindowSummingToWrongNumber.Min() + firstWindowSummingToWrongNumber.Max()));
            Console.ReadKey();
        }
    }
}
