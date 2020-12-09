using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2015
{
    internal class Day12 : BaseDayChallenge
    {
        public override int Year => 2015;
        public override int Day => 12;

        private static readonly Regex DigitsRegex = new Regex(@"-?\d+", RegexOptions.Compiled);
        private List<int> AllDigits = new List<int>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();
            AllDigits = DigitsRegex.Matches(inputLines[0])
                            .Select(m => m.Value)
                            .Select(int.Parse)
                            .ToList();
        }

        protected override Task<string> Part1Async() => Task.FromResult(AllDigits.Sum().ToString());

        public override string ToString() => base.ToString();
    }
}
