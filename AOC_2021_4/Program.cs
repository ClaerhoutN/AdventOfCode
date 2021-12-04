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
            int[][,] boards = boardLines.Select(line => line.ToMultiDim(rows)).ToArray();

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
                            boards[boardIndex][row, col] = -1;
                            if (Enumerable.Range(0, cols).All(x => !boards[boardIndex][row, x].ToBool(-1))
                            || Enumerable.Range(0, rows).All(x => !boards[boardIndex][x, col].ToBool(-1)))
                            {
                                finalScore = calledNumber * boards[boardIndex].SumWhere(x => x != -1);
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
