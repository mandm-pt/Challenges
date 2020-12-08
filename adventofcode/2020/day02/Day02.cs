using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day02 : BaseDayChallenge
    {
        private List<Policy> policies = new List<Policy>();

        public override int Year => 2020;
        public override int Day => 2;

        private readonly Regex CapturePoliciesRegex = new Regex(@"(\d+)-(\d+)\s(.):\s([a-z]+)", RegexOptions.Compiled);

        protected override async Task LoadyAsync()
        {
            string contents = await File.ReadAllTextAsync(InputFilePath);

            policies = CapturePoliciesRegex.Matches(contents)
                .Select(m =>
                {
                    int min = int.Parse(m.Groups[1].Value);
                    int max = int.Parse(m.Groups[2].Value);
                    char c = m.Groups[3].Value[0];
                    string text = m.Groups[4].Value;

                    return new Policy(min, max, c, text);
                })
                .ToList();
        }

        protected override Task<string> Part1Async()
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

            return Task.FromResult(validCount.ToString());
        }

        protected override Task<string> Part2Async()
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

            return Task.FromResult(validCount.ToString());
        }

        private record Policy(int Min, int Max, char Char, string Text);
    }
}
