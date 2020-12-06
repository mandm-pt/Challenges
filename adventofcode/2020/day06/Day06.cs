using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day06 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 6;

        private string[] groups = Array.Empty<string>();

        protected override async Task LoadyAsync()
        {
            string fileContents = await File.ReadAllTextAsync(InputFilePath);

            groups = fileContents.Split(Environment.NewLine + Environment.NewLine);
        }

        protected override Task<string> Part1Async()
            => Task.FromResult(groups.Sum(g => g.Replace(Environment.NewLine, "").ToCharArray().Distinct().Count()).ToString());

        protected override Task<string> Part2Async()
        {
            int count = 0;
            foreach (string[]? group in groups.Select(g => g.Split(Environment.NewLine)))
            {
                if (group.Length == 1)
                    count += group[0].Length;
                else
                {
                    count += group.SelectMany(t => t.ToCharArray())
                        .GroupBy(c => c)
                        .Where(g => g.Count() == group.Length)
                        .Count();
                }
            }

            return Task.FromResult(count.ToString());
        }
    }
}
