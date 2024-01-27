using AOC.Util;
using AOC_2023_5_CSharp;

string inputUrl = "https://adventofcode.com/2023/day/5/input";
var inputLines = await InputHelper.GetInputLines<string[]>(inputUrl, argumentSeparatorRegex: " ");

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

#region part 1
IReadOnlyList<long> seeds = inputLines[0].Skip(1).Select(long.Parse).ToList();
Console.WriteLine("Part 1 - lowest location number: " + seeds.Min(GetLocationNumber));
#endregion part 1

#region part 2
List<(long, long)> seedRanges = new List<(long, long)>();
for(int i = 0; i < seeds.Count-1; i+= 2)
{
    seedRanges.Add((seeds[i], seeds[i + 1]));
}
Console.WriteLine("Part 2 - lowest location number: " 
    + seedRanges.Min(x => GetLocationNumberRanges(x).Select(y => y.Item1).Min()));
#endregion part 2


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
IReadOnlyList<(long, long)> GetLocationNumberRanges((long, long) seedRange)
{
    List<(long, long)> destinationCategoryRanges = new List<(long, long)>() { seedRange };
    int i = 0;
    foreach (var map in maps)
    {
        ++i;

        var destinationCategoryRangesCopy = new List<(long, long)>(destinationCategoryRanges);
        destinationCategoryRanges.Clear();
        foreach (var destinationCategoryRange in destinationCategoryRangesCopy)
        {
            var t = map.GetDestinationCategoryRanges(destinationCategoryRange);
            destinationCategoryRanges.AddRange(t);
        }
    }

    return destinationCategoryRanges;
}