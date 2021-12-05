using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2020_7
{
    class BagRuleCollection : KeyedCollection<string, BagRule>
    {
        protected override string GetKeyForItem(BagRule item)
        {
            return item.Name;
        }
    }
}
