using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2023_5_CSharp
{
    internal class CategoryMap
    {
        readonly struct NumberRange(long source, long destination, long length)
        {
            public readonly long Source = source;
            public readonly long Destination = destination;
            public readonly long Length = length;
        }
        private readonly List<NumberRange> _numberRanges = new List<NumberRange>();
        public void Add(long source, long destination, long length)
        {
            _numberRanges.Add(new NumberRange(source, destination, length));
        }
        public long GetDestinationCategory(long sourceCategory)
        {
            foreach(var range in _numberRanges)
            {
                long diff = sourceCategory - range.Source;
                if (diff >= 0 && diff <= range.Length)
                    return range.Destination + diff;
            }
            return sourceCategory;
        }
    }
}
