using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day02 : BaseDayChallenge
    {
        private readonly List<Policy> policies = new List<Policy>();

        public override int Year => 2020;
        public override int Day => 2;

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            foreach (string line in inputLines)
            {
                string[] parts = line.Split(" ");

                string[] subPart = parts[0].Split("-");

                int min = int.Parse(subPart[0]);
                int max = int.Parse(subPart[1]);
                char c = parts[1][0];
                string text = parts[2];

                policies.Add(new Policy(min, max, c, text));
            }
        }

        protected override Task Part1Async()
        {
            int validCount = 0;

            foreach (var p in policies)
            {
                int count = p.Text.Count(c => c == p.Char);

                if (count >= p.Min && count <= p.Max)
                {
                    validCount++;
                }
            }

            Console.WriteLine(validCount);
            return Task.CompletedTask;
        }

        protected override Task Part2Async()
        {
            int validCount = 0;

            foreach (var p in policies)
            {
                if (p.Text[p.Min - 1] != p.Text[p.Max - 1] && (
                    p.Text[p.Min - 1] == p.Char || p.Text[p.Max - 1] == p.Char))
                {
                    validCount++;
                }
            }

            Console.WriteLine(validCount);
            return Task.CompletedTask;
        }
    }

    internal record Policy(int Min, int Max, char Char, string Text);
}
