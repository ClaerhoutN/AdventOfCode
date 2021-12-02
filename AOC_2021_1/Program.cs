using AOC.Util;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2021_1
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2021/day/1/input";

            int lastDepth = -1;
            int depthIncreaseCount = 0;
            foreach(int depth in await InputHelper.GetInputLines<int>(inputUrl))
            {
                if(lastDepth >= 0 && depth > lastDepth)
                    ++depthIncreaseCount;
                lastDepth = depth;
            }
            Console.WriteLine(depthIncreaseCount);
            Console.ReadKey();
        }
    }
}
