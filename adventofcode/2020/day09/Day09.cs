using System;
using System.Collections.Generic;
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

            Numbers = inputLines.Select(long.Parse).ToArray();
        }

        protected override Task<string> Part1Async()
        {
            int take = 25;
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
            IEnumerable<long> set;

            for (int i = 0; i < Numbers.Length; i++)
            {
                long sum = 0;
                int take = 1;
                while (take < Numbers.Length - 1 && sum < Part1Solution)
                {
                    set = Numbers.Skip(i + 1).Take(take++);
                    sum = set.Sum();
                    if (sum == Part1Solution)
                    {
                        long min = set.Min();
                        long max = set.Max();
                        return Task.FromResult((min + max).ToString());
                    }
                }
            }

            return Task.FromResult("Not Solved");
        }

        private bool IsValid(long[] arr, int idx, long number, int take)
        {
            for (int i = idx; i < idx + take; i++)
            {
                for (int j = idx + 1; j < idx + take; j++)
                {
                    if (arr[i] + arr[j] == number)
                        return true;
                }
            }

            return false;
        }
    }
}
