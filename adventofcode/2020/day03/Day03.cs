using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day03 : BaseDayChallenge
    {
        public override int Year => 2020;
        public override int Day => 3;

        protected override Task<string> Part1Async() => Task.FromResult(TraverseMap(3, 1).ToString());

        protected override Task<string> Part2Async()
        {
            int slope1 = TraverseMap(1, 1);
            int slope2 = TraverseMap(3, 1);
            int slope3 = TraverseMap(5, 1);
            int slope4 = TraverseMap(7, 1);
            int slope5 = TraverseMap(1, 2);

            int solution = slope1 * slope2 * slope3 * slope4 * slope5;
            return Task.FromResult(solution.ToString());
        }

        private int TraverseMap(int xIncrement, int yIncrement)
        {
            int treeCount = 0;
            int x = xIncrement;
            int maxX = inputLines[0].Length - 1;
            for (int i = yIncrement; i < inputLines.Length; i += yIncrement)
            {
                if (inputLines[i][x] == '#')
                {
                    treeCount++;
                }

                x += xIncrement;

                if (x > maxX)
                {
                    x = x % maxX - 1;
                }
            }

            return treeCount;
        }
    }
}
