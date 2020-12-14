using System.Collections.Generic;
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

            //inputLines = new[] { "1068781", "7,13,x,x,59,x,31,19" };
            //inputLines = new[] { "3417", "17,x,13,19" };
            //inputLines = new[] { "754018", "67,7,59,61" };
            //inputLines = new[] { "779210", "67,x,7,59,61" };
            //inputLines = new[] { "1261476", "67,7,x,59,61" };
            //inputLines = new[] { "1202161486", "1789,37,47,1889" };

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

            long result = (bus.NextTime - EarliestTimestamp) * bus.Id;

            return Task.FromResult(result.ToString());
        }

        protected override Task<string> Part2Async()
        {
            string[]? rawBuses = inputLines[1].Split(',');
            var bus = new List<BusTime>();
            for (int i = 0; i < rawBuses.Length; i++)
            {
                if (rawBuses[i] == "x")
                    continue;

                bus.Add(new(int.Parse(rawBuses[i]), i, 0));
            }

            bus[0].Number = 100000000000000;
            int busIdx = 0;
            do
            {
                if (busIdx == 0)
                {
                    bus[busIdx].Number = NextMultiple(bus[0].Number, bus[busIdx].Id);
                    bus[busIdx].StartTimestamp = bus[busIdx].Number - bus[busIdx].Id;
                    busIdx++;
                }
                else
                {
                    bus[busIdx].Number = (bus[0].StartTimestamp + bus[busIdx].Index);

                    if (bus[busIdx].Number % bus[busIdx].Id == 0)
                        busIdx++;
                    else
                        busIdx = 0;

                }
            } while (busIdx <= bus.Count - 1);

            return Task.FromResult(bus[0].StartTimestamp.ToString());
        }

        private static long NextMultiple(long n, long m)
            => n + (m - n % m);

        private class BusTime
        {
            public BusTime(int id, int index, long startTimestamp)
            {
                Id = id;
                Index = index;
                StartTimestamp = startTimestamp;
            }

            public int Id { get; }
            public int Index { get; }
            public long StartTimestamp { get; set; }
            public long Number { get; set; }
        }
    }
}
