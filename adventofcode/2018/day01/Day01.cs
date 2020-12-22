using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2018
{
    internal class Day01 : BaseDayChallenge
    {
        public override int Year => 2018;
        public override int Day => 1;

        private int[] Changes = Array.Empty<int>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            Changes = inputLines.Select(int.Parse).ToArray();
        }

        protected override Task<string> Part1Async()
        {
            int frequencyChange = Changes.Aggregate(0, (a, b) => a + b);

            return Task.FromResult(frequencyChange.ToString());
        }

        protected override Task<string> Part2Async()
        {
            int result = 0;
            
            var results = new List<int>();
            bool repeatedFrequency = false;

            do
            {
                foreach (var change in Changes)
                {
                    result = result + change;

                    if (results.Contains(result))
                    {
                        repeatedFrequency = true;
                        break;
                    }

                    results.Add(result);
                }
            } while (!repeatedFrequency);

            return Task.FromResult(result.ToString());
        }
    }
}
