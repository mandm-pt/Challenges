using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    class Day2 : DayChallenge
    {
        List<Policy> policies = new List<Policy>();

        public Day2(string inputFile)
            : base(inputFile)
        {
        }

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            foreach (var line in inputLines)
            {
                var parts = line.Split(" ");

                var subPart = parts[0].Split("-");

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
                    validCount++;
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
                    validCount++;
            }

            Console.WriteLine(validCount);
            return Task.CompletedTask;
        }
    }

    record Policy(int min, int max, char c, string text);
}
