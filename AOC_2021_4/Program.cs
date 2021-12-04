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

            var inputLines = await InputHelper.GetInputLines<string[]>(inputUrl, 
                argumentSeparatorRegex: "[,\\s]+", lineSeparatorRegex: "\\n\\n");

            var calledNumbers = inputLines[0].Select(x => int.Parse(x)).ToList();
            var boardLines = inputLines.Skip(1).ToList();

            int rows = 5, cols = 5;
            int[][,] boards = new int[boardLines.Count][,]; //stackalloc?
            bool[][,] trackedBoardNumbers = new bool[boardLines.Count][,]; //stackalloc?
            int[] sumsOfUnmarked = new int[boardLines.Count];
            boardLines.ForEach((i, line, _) =>
            {
                boards[i] = new int[rows, cols];
                trackedBoardNumbers[i] = new bool[rows, cols];
                line.Where(x => ! string.IsNullOrWhiteSpace(x))
                    .ToList() //prevent rerunning iteration multiple times
                    .ForEach((i2, v, _) =>
                {
                    int nr = int.Parse(v);
                    boards[i][i2 / 5, i2 % 5] = nr;
                    sumsOfUnmarked[i] += nr;
                });

            });

            int finalScore = -1;
            foreach (int calledNumber in calledNumbers)
            {
                boards.ForEach((boardIndex, board, @break) =>
                {
                    for (int row = 0; row < rows; ++row)
                    {
                        for (int col = 0; col < cols; ++col)
                        {
                            if (board[row, col] == calledNumber)
                            {
                                trackedBoardNumbers[boardIndex][row, col] = true;
                                sumsOfUnmarked[boardIndex] -= calledNumber;
                                for (int i = 0; i < cols; ++i)
                                {
                                    if (trackedBoardNumbers[boardIndex][row, i] == false)
                                        break;
                                    else if (i == cols - 1)
                                    {
                                        finalScore = calledNumber * sumsOfUnmarked[boardIndex];
                                        @break();
                                        goto end;
                                    }
                                }
                                for (int i = 0; i < rows; ++i)
                                {
                                    if (trackedBoardNumbers[boardIndex][i, col] == false)
                                        break;
                                    else if (i == rows - 1)
                                    {
                                        finalScore = calledNumber * sumsOfUnmarked[boardIndex];
                                        @break();
                                        goto end;
                                    }
                                }
                            }
                        }
                    }
                    end:
                    byte _;
                });
                if (finalScore >= 0)
                    break;
            }

            Console.WriteLine(finalScore);
            Console.ReadKey();
        }
    }
}
