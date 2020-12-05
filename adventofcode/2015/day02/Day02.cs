using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2015
{
    internal class Day02 : BaseDayChallenge
    {
        public override int Year => 2015;
        public override int Day => 2;

        private List<Size> Sizes = new List<Size>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            Sizes = inputLines.Select(l =>
            {
                string[] parts = l.Split('x')!;
                return new Size(int.Parse(parts[0]), int.Parse(parts[1]), int.Parse(parts[2]));
            }).ToList();
        }

        protected override Task<string> Part1Async() => Task.FromResult(Sizes.Sum(s => s.TotalWrap).ToString());

        protected override Task<string> Part2Async() => Task.FromResult(Sizes.Sum(s => s.TotalRibbon).ToString());

        private record Size(int L, int W, int H)
        {
            private readonly int BiggestSize = Math.Max(Math.Max(L, W), H);

            public int WrappingPaper => 2 * L * W + 2 * W * H + 2 * H * L;
            public int Slack => Math.Min(Math.Min(L * W, W * H), H * L);
            public int TotalWrap => WrappingPaper + Slack;

            public int Ribbon => 2 * L + 2 * W + 2 * H - 2 * BiggestSize;
            public int Bow => L * W * H;
            public int TotalRibbon => Ribbon + Bow;
        }
    }
}
