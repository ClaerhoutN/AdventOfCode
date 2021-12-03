using AOC.Util;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;

namespace AOC_2021_3
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2021/day/3/input";

            var inputLines = await InputHelper.GetInputLines<string>(inputUrl);
            int[] bitwiseSums = new int[12];

            foreach(var input in inputLines)
            {
                for (int i = 0; i < bitwiseSums.Length; ++i)
                {
                    bitwiseSums[i] += input[i] == '0' ? -1 : 1;
                }
            }

            long gamma = 0;
            long epsilon = 0;
            for (int i = 0; i < bitwiseSums.Length; ++i)
            {
                gamma <<= 1;
                gamma += (bitwiseSums[i] > 0 ? 1 : 0);

                epsilon <<= 1;
                epsilon += (bitwiseSums[i] < 0 ? 1 : 0);
            }

            Console.WriteLine(gamma * epsilon);
            Console.ReadKey();
        }
    }
}
