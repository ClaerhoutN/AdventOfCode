using AOC.Util;
using System;
using System.Threading.Tasks;

namespace AOC_2022_8
{
    class Tree
    {
        public Tree? Top { get; init; }
        public Tree? Bottom { get; set; }
        public Tree? Left { get; init; }
        public Tree? Right { get; set; }
        public char Value { get; init; }
        public bool IsVisible => Top == null || Bottom == null || Left == null || Right == null
            || (Value > (HighestNeighbourTop?.Value ?? -1))
            || (Value > (HighestNeighbourBottom?.Value ?? -1))
            || (Value > (HighestNeighbourLeft?.Value ?? -1))
            || (Value > (HighestNeighbourRight?.Value ?? -1));
        private static Tree? GetHighestNeighbour(Tree? tree, Func<Tree, Tree?> highestNeighbourFunc)
        {
            if (tree == null) return tree;
            var highestNeighbour = highestNeighbourFunc(tree);
            return tree.Value >= (highestNeighbour?.Value ?? -1) ? tree : highestNeighbour;
        }
        private Tree? HighestNeighbourTop => GetHighestNeighbour(Top, x => x.HighestNeighbourTop);
        private Tree? HighestNeighbourBottom => GetHighestNeighbour(Bottom, x => x.HighestNeighbourBottom);
        private Tree? HighestNeighbourLeft => GetHighestNeighbour(Left, x => x.HighestNeighbourLeft);
        private Tree? HighestNeighbourRight => GetHighestNeighbour(Right, x => x.HighestNeighbourRight);
        private long ViewingDistanceTop(int treeToCompare) => Top == null ? 0 : (Top.Value < treeToCompare ? Top.ViewingDistanceTop(treeToCompare)+1 : 1);
        private long ViewingDistanceBottom(int treeToCompare) => Bottom == null ? 0 : (Bottom.Value < treeToCompare ? Bottom.ViewingDistanceBottom(treeToCompare)+1 : 1);
        private long ViewingDistanceLeft(int treeToCompare) => Left == null ? 0 : (Left.Value < treeToCompare ? Left.ViewingDistanceLeft(treeToCompare)+1 : 1);
        private long ViewingDistanceRight(int treeToCompare) => Right == null ? 0 : (Right.Value < treeToCompare ? Right.ViewingDistanceRight(treeToCompare)+1 : 1);
        public long ScenicScore => ViewingDistanceBottom(Value) * ViewingDistanceTop(Value) * ViewingDistanceLeft(Value) * ViewingDistanceRight(Value);
    }
    class Program
    {
        private static int GetVisibleTreeCount(Tree[,] trees)
        {
            int sum = 0;
            for (int i = 0; i < trees.GetLength(0); i++)
            {
                for (int j = 0; j < trees.GetLength(1); j++)
                {
                    if (trees[i, j].IsVisible)
                        ++sum;
                }
            }
            return sum;
        }
        private static long GetMaximumScenicScore(Tree[,] trees)
        {
            long maxScenicScore = 0;
            for (int i = 0; i < trees.GetLength(0); i++)
            {
                for (int j = 0; j < trees.GetLength(1); j++)
                {
                    long scenicScore = trees[i, j].ScenicScore;
                    maxScenicScore = scenicScore > maxScenicScore ? scenicScore : maxScenicScore;
                }
            }
            return maxScenicScore;
        }
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2022/day/8/input";
            var input = await InputHelper.GetInputLines<string>(inputUrl);
            
            int height = input.Count, width = input[0].Length;
            Tree[,] trees = new Tree[height, width];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    trees[i, j] = new Tree
                    {
                        Value = input[i][j], 
                        Top = i < 1 ? null : trees[i-1, j], 
                        Left = j < 1 ? null : trees[i, j-1]
                    };
                    if (j >= 1)
                        trees[i, j-1].Right = trees[i, j];
                    if (i >= 1)
                        trees[i-1, j].Bottom = trees[i, j];
                }
            }
            //part 1
            Console.WriteLine(GetVisibleTreeCount(trees));
            //part 2
            Console.WriteLine(GetMaximumScenicScore(trees));
        }
    }
}
