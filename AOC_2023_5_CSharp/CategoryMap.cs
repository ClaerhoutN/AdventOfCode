using AOC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static AOC_2023_5_CSharp.CategoryMap;

namespace AOC_2023_5_CSharp
{
    internal class CategoryMap
    {
        public readonly struct CategoryRange(long source, long destination, long length)
        {
            public readonly long Source = source;
            public readonly long Destination = destination;
            public readonly long Length = length;
        }
        private readonly List<CategoryRange> _categoryRanges = new List<CategoryRange>();
        public void Add(long source, long destination, long length)
        {
            _categoryRanges.Add(new CategoryRange(source, destination, length));
        }
        public long GetDestinationCategory(long sourceCategory)
        {
            foreach(var range in _categoryRanges)
            {
                long diff = sourceCategory - range.Source;
                if (diff >= 0 && diff < range.Length)
                    return range.Destination + diff;
            }
            return sourceCategory;
        }
        public IReadOnlyList<(long, long)> GetDestinationCategoryRanges((long sourceCategory, long sourceRange) sourceCategoryRange)
        {
            List<(long, long)> destinationCategoryRanges = new List<(long, long)>();
            long sourceCategory = sourceCategoryRange.sourceCategory;
            long sourceRange = sourceCategoryRange.sourceRange;

            var sortedCategoryRanges = _categoryRanges.OrderBy(x => x.Source).ToList();
            while (sourceRange > 0)
            {
                bool found = false;
                foreach (var categoryRange in sortedCategoryRanges)
                {
                    long diff = sourceCategory - categoryRange.Source;
                    if (diff >= 0 && diff < categoryRange.Length)
                    {
                        long destination = categoryRange.Destination + diff;
                        long length = Math.Min(categoryRange.Length - diff, sourceRange);
                        destinationCategoryRanges.Add((destination, length));
                        found = true;
                        sourceCategory += length;
                        sourceRange -= length;
                        break;
                    }
                }
                if(!found)
                {
                    long destination = sourceCategory, length;
                    int nextCategoryRangeIndex = sortedCategoryRanges.IndexWhere(x => x.Source > sourceCategory);
                    if(nextCategoryRangeIndex == -1) //no more number ranges will cover the remaining sourceCategoryRange
                        length = sourceRange;
                    else
                        length = sortedCategoryRanges[nextCategoryRangeIndex].Source - sourceCategory;
                    destinationCategoryRanges.Add((destination, length));
                    sourceCategory += length;
                    sourceRange -= length;
                }
            }
            return destinationCategoryRanges;
        }
    }
}
