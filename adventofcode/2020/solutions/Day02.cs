using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Day02 : DayChallenge
    {
        private readonly List<Policy> policies = new List<Policy>();

        protected override int day => 2;

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
                int count = p.text.Count(c => c == p.c);

                if (count >= p.min && count <= p.max)
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
                if (p.text[p.min - 1] != p.text[p.max - 1] && (
                    p.text[p.min - 1] == p.c || p.text[p.max - 1] == p.c))
                {
                    validCount++;
                }
            }

            Console.WriteLine(validCount);
            return Task.CompletedTask;
        }
    }

    internal record Policy(int min, int max, char c, string text);
}
