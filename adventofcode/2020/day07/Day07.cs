using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day07 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 7;

        private readonly Regex CaptureBags = new Regex("\\d+[ a-z]+ bags?|no other bags", RegexOptions.Compiled);
        private readonly List<Rule> Rules = new List<Rule>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            Rules.Clear();
            foreach (string? line in inputLines)
            {
                string[]? parts = line.Split("bags");

                var innerRules = new List<InnerRule>();
                foreach (Match match in CaptureBags.Matches(line))
                {
                    if (match.Value != "no other bags")
                    {
                        int idx = match.Value.IndexOf(" ");
                        int amount = int.Parse(match.Value[0..idx]);

                        string color = match.Value
                            .Replace("bags", "")
                            .Replace("bag", "")
                            .Replace(amount.ToString(), "")
                            .Trim();

                        innerRules.Add(new(color, amount));
                    }
                }

                Rules.Add(new(parts[0].Trim(), innerRules));
            }
        }

        protected override Task<string> Part1Async()
        {
            var compatibleRules = FindCompatibleRules(new[] { "shiny gold" }).Distinct();
            int count = compatibleRules.Count();

            return Task.FromResult(count.ToString());
        }

        protected override Task<string> Part2Async()
        {
            var shinyGold = Rules.First(r => r.BagColor == "shiny gold");
            int count = FindTotalBags(shinyGold.InnerRules) - 1; // initial value

            return Task.FromResult(count.ToString());
        }

        private IEnumerable<Rule> FindCompatibleRules(IEnumerable<string> colors)
        {
            var allCompatibleRules = new List<Rule>();

            foreach (string color in colors)
            {
                var compatibleRules = Rules.Where(r => r.InnerRules.Any(ir => ir.BagColor == color));
                allCompatibleRules.AddRange(compatibleRules);

                allCompatibleRules.AddRange(FindCompatibleRules(compatibleRules.Select(r => r.BagColor)));
            }

            return allCompatibleRules;
        }

        private int FindTotalBags(IEnumerable<InnerRule> innerRules)
        {
            int total = 1;

            foreach (var innerRule in innerRules)
            {
                var rule = Rules.First(r => r.BagColor == innerRule.BagColor);

                total += FindTotalBags(rule.InnerRules) * innerRule.Count;
            }

            return total;
        }

        private record Rule(string BagColor, IEnumerable<InnerRule> InnerRules);

        private record InnerRule(string BagColor, int Count);
    }
}
