using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2015
{
    internal class Day01 : BaseDayChallenge
    {
        public override int Year => 2015;
        public override int Day => 1;

        private List<int> Steps = new List<int>();

        protected override async Task LoadyAsync()
        {
            Steps = (await File.ReadAllTextAsync(InputFilePath))
                 .Select(c => c == '(' ? 1 : -1)
                 .ToList();
        }

        protected override Task<string> Part1Async() => Task.FromResult(Steps.Sum().ToString());

        protected override Task<string> Part2Async()
        {
            int i = 0, count = 0;

            for (; count >= 0; i++) count += Steps[i];

            return Task.FromResult(i.ToString());
        }
    }
}
