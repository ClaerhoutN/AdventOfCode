using AOC.Util;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2021_5
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2021/day/5/input";

            var inputLines = await InputHelper.GetInputLines<string[]>(inputUrl,
                argumentSeparatorRegex: " -> ");

            var ventPositions = inputLines.Select(x =>
            {
                string[] p1 = x[0].Split(','), p2 = x[1].Split(',');
                int x1 = int.Parse(p1[0]),
                    y1 = int.Parse(p1[1]),
                    x2 = int.Parse(p2[0]),
                    y2 = int.Parse(p2[1]);

                return new int[,] { { x1, y1 }, { x2, y2 } };
            })
                .Where(x => x[0, 0] == x[1, 0] || x[0, 1] == x[1, 1])
                .ToArray();
            int[,] overlappedPositions = new int[
                ventPositions.Max(x => Math.Max(x[0, 0], x[1, 0])) + 1,
                ventPositions.Max(x => Math.Max(x[0, 1], x[1, 1])) + 1];
            CalculateOverlappedPositions(overlappedPositions, ventPositions);

            Console.WriteLine(
                AOC.Util.Range.FromDimensions(overlappedPositions.GetLength(0), overlappedPositions.GetLength(1)).Count(
                    x => overlappedPositions[x.Item1, x.Item2] >= 2
                ));
            Console.ReadKey();
        }

        private static void CalculateOverlappedPositions(int[,] overlappedPositions, int[][,] positions)
        {
            Array.ForEach<int[,]>(positions, p =>
            {
                if (p[0, 0] == p[1, 0]) //same x -> vertical line
                {
                    if (p[1, 1] >= p[0, 1])
                    {
                        for (int i = p[0, 1]; i <= p[1, 1]; ++i)
                            ++overlappedPositions[p[0, 0], i];
                    }
                    else
                    {
                        for (int i = p[1, 1]; i <= p[0, 1]; ++i)
                            ++overlappedPositions[p[0, 0], i];
                    }
                }
                else if (p[0, 1] == p[1, 1]) //same y -> horizontal line
                {
                    if (p[1, 0] >= p[0, 0])
                    {
                        for (int i = p[0, 0]; i <= p[1, 0]; ++i)
                            ++overlappedPositions[i, p[0, 1]];
                    }
                    else
                    {
                        for (int i = p[1, 0]; i <= p[0, 0]; ++i)
                            ++overlappedPositions[i, p[0, 1]];
                    }

                }
            });
        }
    }
}

//int[,] crossedPositions = new int[
//    ventPositions.Max(x => Math.Max(x[0, 0], x[1, 0])) + 1, 
//    ventPositions.Max(x => Math.Max(x[0, 1], x[1, 1])) + 1];

/*CROSSING*/
////p1 crossing p2 vertically
//if (
//    ((p1[0,1] <= p2[0,1] && p2[0,1] <= p1[1,1]) //compare y
//||   (p1[1,1] <= p2[0,1] && p2[0,1] <= p1[0,1]))
//&&  ((p2[0,0] <= p1[0,0] && p1[0,0] <= p2[1,0]) //compare x
//||   (p2[1,0] <= p1[0,0] && p1[0,0] <= p2[0,0])))
//    ++crossedPositions[p1[0,0], p2[0,1]];

////p1 crossing p2 horizontally
//else if (
//    ((p1[0,0] <= p2[0,0] && p2[0,0] <= p1[1,0]) //compare x
//||   (p1[1,0] <= p2[0,0] && p2[0,0] <= p1[0, 0]))
//&&  ((p2[0,1] <= p1[0,1] && p1[0,1] <= p2[1,1]) //compare y
//||   (p2[1,1] <= p1[0,1] && p1[0,1] <= p2[0,1])))
//    ++crossedPositions[p2[0,0], p1[0,1]];
