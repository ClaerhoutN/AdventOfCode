using System;
using System.IO;
using System.Linq;

namespace AOC_2021_1
{
    class Program
    {
        static void Main(string[] args)
        {
            int lastDepth = -1;
            int depthIncreaseCount = 0;
            foreach(int depth in File.ReadAllLines("../../../../inputs/2021_1.txt")
                .Select(x => int.Parse(x)))
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
