using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day10 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 10;

        private List<int> Adapters = new List<int>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            Adapters = inputLines.Select(int.Parse).OrderBy(n => n).ToList();
        }

        protected override Task<string> Part1Async()
        {
            int deviceJoltRate = Adapters.Max() + 3;
            Adapters.Insert(0, 0);
            Adapters.Add(deviceJoltRate);

            int result = GetDifferences(Adapters.ToArray())
                            .GroupBy(n => n)
                            .Where(g => g.Key == 1 || g.Key == 3)
                            .Select(g => g.Count())
                            .Aggregate(1, (a, b) => a * b);

            return Task.FromResult(result.ToString());
        }

        private int[] GetDifferences(int[] adapters)
        {
            var diffs = new List<int>();
            for (int i = 1; i < adapters.Length; i++)
            {
                diffs.Add(adapters[i] - adapters[i - 1]);
            }

            return diffs.ToArray();
        }
    }
}
