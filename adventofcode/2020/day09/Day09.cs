using System;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day09 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 9;

        private long[] Numbers = Array.Empty<long>();
        private long Part1Solution = 0;

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            inputLines = new[]
            {
                "35",
                "20",
                "15",
                "25",
                "47",
                "40",
                "62",
                "55",
                "65",
                "95",
                "102",
                "117",
                "150",
                "182",
                "127",
                "219",
                "299",
                "277",
                "309",
                "576",
            };

            Numbers = inputLines.Select(long.Parse).ToArray();
        }

        protected override Task<string> Part1Async()
        {
            int take = 5;
            int startIdx = 0;
            foreach (long number in Numbers.Skip(take))
            {
                if (!IsValid(Numbers, startIdx, number, take))
                {
                    Part1Solution = number;
                    break;
                }

                startIdx++;
            }

            return Task.FromResult(Part1Solution.ToString());
        }

        protected override Task<string> Part2Async()
        {
            // todo
            //for (int i = 0; i < Numbers.Length; i++)
            //{
            //}

            return Task.FromResult(Part1Solution.ToString());
        }

        private bool IsValid(long[] arr, int idx, long number, int take)
        {
            for (int i = idx; i < idx + take; i++)
            {
                for (int j = idx; j < idx + take; j++)
                {
                    if (arr[i] + arr[j] == number)
                        return true;
                }
            }

            return false;
        }
    }
}
