using AOC.Util;
using AOC_2023_5_CSharp;

string inputUrl = "https://adventofcode.com/2023/day/5/input";

var inputLines = await InputHelper.GetInputLines<string[]>(inputUrl, argumentSeparatorRegex: " ");
IReadOnlyList<long> seeds = inputLines[0].Skip(1).Select(x => long.Parse(x)).ToList();

List<CategoryMap> maps = new List<CategoryMap>();
CategoryMap map = null;
foreach(var inputLine in inputLines.Skip(1))
{
    if (inputLine.Last() == "map:")
    {
        map = new CategoryMap();
        maps.Add(map);
        continue;
    }
    map.Add(long.Parse(inputLine[1]), long.Parse(inputLine[0]), long.Parse(inputLine[2]));
}

Console.WriteLine("lowest location number: " + seeds.Min(GetLocationNumber));
Console.ReadKey();

long GetLocationNumber(long seed)
{
    long destinationCategory = seed;
    foreach(var map in maps)
    {
        destinationCategory = map.GetDestinationCategory(destinationCategory);
    }
    return destinationCategory;
}