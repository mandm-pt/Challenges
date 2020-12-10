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

            Adapters = inputLines.Select(int.Parse).ToList();
        }

        protected override Task<string> Part1Async()
        {
            int deviceJoltRate = Adapters.Max() + 3;
            Adapters.Add(deviceJoltRate);
            int chargingOutlet = 0;

            var allDifferences = TestNextAdapter(chargingOutlet, new List<int>());

            int result = allDifferences
                            .GroupBy(n => n)
                            .Where(g => g.Key == 1 || g.Key == 3)
                            .Select(g => g.Count())
                            .Aggregate(1, (a, b) => a * b);

            return Task.FromResult(result.ToString());
        }

        private IEnumerable<int> TestNextAdapter(int currentJolt, IList<int> differences)
        {
            int compatibleAdapterJolt = Adapters.OrderBy(n => n).FirstOrDefault(n => n > currentJolt && n <= currentJolt + 3);

            if (compatibleAdapterJolt == 0)
                return differences;

            differences.Add(compatibleAdapterJolt - currentJolt);
            return TestNextAdapter(compatibleAdapterJolt, differences);
        }
    }
}
