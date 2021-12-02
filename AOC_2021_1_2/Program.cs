using AOC.Util;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2021_1_2
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2021/day/1/input";

            int lastTotalDepth = -1;
            int depthIncreaseCount = 0;
            var depths = await InputHelper.GetInputLines<int>(inputUrl);
            for (int i = 2; i < depths.Count; ++i)
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
