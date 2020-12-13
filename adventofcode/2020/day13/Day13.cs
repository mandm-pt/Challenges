using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day13 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 13;

        private long EarliestTimestamp = 0;
        private int[] Buses = new int[0];

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            //inputLines = new[] { "939", "7,13,x,x,59,x,31,19" };

            EarliestTimestamp = long.Parse(inputLines[0]);
            Buses = inputLines[1]
                .Split(',')
                .Where(b => b != "x")
                .Select(b => int.Parse(b))
                .ToArray();
        }

        protected override Task<string> Part1Async()
        {
            var bus = Buses
                .Select(b => new
                {
                    Id = b,
                    NextTime = (EarliestTimestamp / b) * b + b
                })
                .OrderBy(b => b.NextTime)
                .First();

            var result = (bus.NextTime - EarliestTimestamp) * bus.Id;

            return Task.FromResult(result.ToString());
        }
    }
}
