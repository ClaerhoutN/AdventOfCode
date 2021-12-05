using AOC.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AOC_2020_7
{
    class Program
    {
        static async Task Main(string[] args)
        {
            string inputUrl = "https://adventofcode.com/2020/day/7/input";

            BagRuleCollection rules = new BagRuleCollection();
            foreach(var rule in await InputHelper.GetInputLines<string[]>(inputUrl, argumentSeparatorRegex: "s contain |s?, |s?\\."))
            {
                AddRule(rules, rule);
            }

            Console.WriteLine("part 1: " + rules.Count(x => x.CanContainBagNested("shiny gold bag")));
            Console.WriteLine("part 2: " + rules["shiny gold bag"].NestedInnerBagSum());
            Console.ReadKey();
        }

        private static void AddRule(BagRuleCollection rules, string[] rule)
        {
            if (!rules.TryGetValue(rule[0], out var bagRule))
            {
                bagRule = new BagRule(rules, rule[0]);
                rules.Add(bagRule);
            }

            for (int i = 1; i < rule.Length; ++i)
            {
                if (rule[i] == "no other bag")
                    continue;
                int spaceIndex = rule[i].IndexOf(' ');
                int amount = int.Parse(rule[i].Substring(0, spaceIndex));
                string bagName = rule[i].Substring(spaceIndex + 1);

                if (!rules.TryGetValue(bagName, out var innerBagRule))
                {
                    innerBagRule = new BagRule(rules, bagName);
                    rules.Add(innerBagRule);
                }
                bagRule.AddInnerBag(bagName, amount);
            }
        }
    }
}
