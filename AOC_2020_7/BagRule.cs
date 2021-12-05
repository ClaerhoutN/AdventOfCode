using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AOC_2020_7
{
    [DebuggerDisplay("{Name,nq}")]
    class BagRule
    {
        public string Name { get; }
        private readonly Dictionary<string, int> _innerBags = new Dictionary<string, int>();
        private readonly BagRuleCollection _parentRuleCollection;
        public BagRule(BagRuleCollection parentRuleCollection, string name)
        {
            Name = name;
            _parentRuleCollection = parentRuleCollection;
        }
        public void AddInnerBag(string innerBagName, int amount)
        {
            _innerBags[innerBagName] = amount;
        }        
        public bool CanContainBagNested(string bagName)
        {
            foreach (var pair in _innerBags)
            {
                if (pair.Key == bagName || _parentRuleCollection[pair.Key].CanContainBagNested(bagName))
                    return true;
            }
            return false;
        }
        public int NestedInnerBagSum()
        {
            return _innerBags.Sum(x => x.Value + x.Value * _parentRuleCollection[x.Key].NestedInnerBagSum());
        }
    }
}
