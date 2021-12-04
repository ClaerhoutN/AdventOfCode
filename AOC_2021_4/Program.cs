using AOC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2021_4
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2021/day/4/input";

            var inputLines = await InputHelper.GetInputLines<int[]>(inputUrl, 
                argumentSeparatorRegex: "[,\\s]+", lineSeparatorRegex: "\\n\\n");

            var calledNumbers = inputLines[0];
            var boardLines = inputLines.Skip(1).ToList();

            int rows = 5, cols = 5;
            int[][,] boards = new int[boardLines.Count][,]; //stackalloc?
            bool[][,] trackedBoardNumbers = new bool[boardLines.Count][,]; //stackalloc?
            int[] sumsOfUnmarked = new int[boardLines.Count];
            boardLines.ForEach((i, line, _) =>
            {
                boards[i] = new int[rows, cols];
                trackedBoardNumbers[i] = new bool[rows, cols];
                line.ForEach((i2, v, _) =>
                {
                    boards[i][i2 / 5, i2 % 5] = v;
                    sumsOfUnmarked[i] += v;
                });
            });

            int finalScore = -1;
            foreach (int calledNumber in calledNumbers)
            {
                boards.ForEach((boardIndex, board, @break) =>
                {
                    AOC.Util.Range.FromDimensions(rows, cols).ForEach((i, rc, @break2) =>
                    {
                        int row = rc.Item1, col = rc.Item2;
                        if (board[row, col] == calledNumber)
                        {
                            trackedBoardNumbers[boardIndex][row, col] = true;
                            sumsOfUnmarked[boardIndex] -= calledNumber;
                            if (Enumerable.Range(0, cols).All(x => trackedBoardNumbers[boardIndex][row, x])
                            || Enumerable.Range(0, rows).All(x => trackedBoardNumbers[boardIndex][x, col]))
                            {
                                finalScore = calledNumber * sumsOfUnmarked[boardIndex];
                                @break(); @break2();
                            }
                        }
                    });
                });
                if (finalScore >= 0)
                    break;
            }

            Console.WriteLine(finalScore);
            Console.ReadKey();
        }
    }
}
