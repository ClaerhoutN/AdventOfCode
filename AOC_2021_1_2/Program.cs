using System;
using System.IO;
using System.Linq;

namespace AOC_2021_1_2
{
    class Program
    {
        static void Main(string[] args)
        {
            int lastTotalDepth = -1;
            int depthIncreaseCount = 0;
            var depths = File.ReadAllLines("../../../../inputs/2021_1_2.txt")
                .Select(x => int.Parse(x))
                .ToList();
            for(int i = 2; i < depths.Count; ++i)
            {
                int totalDepth = 0;
                for (int j = i - 2; j <= i; ++j)
                {
                    totalDepth += depths[j];
                }
                if (lastTotalDepth >= 0 && totalDepth > lastTotalDepth)
                    ++depthIncreaseCount;
                lastTotalDepth = totalDepth;
            }
            Console.WriteLine(depthIncreaseCount);
            Console.ReadKey();
        }
    }
}
