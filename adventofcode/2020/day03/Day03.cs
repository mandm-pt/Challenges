using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Day03 : BaseDayChallenge
    {
        protected override int Day => 3;

        protected override Task Part1Async()
        {
            System.Console.WriteLine(TraverseMap(3, 1));
            return Task.CompletedTask;
        }

        protected override Task Part2Async()
        {
            int slope1 = TraverseMap(1, 1);
            int slope2 = TraverseMap(3, 1);
            int slope3 = TraverseMap(5, 1);
            int slope4 = TraverseMap(7, 1);
            int slope5 = TraverseMap(1, 2);

            System.Console.WriteLine(slope1 * slope2 * slope3 * slope4 * slope5);
            return Task.CompletedTask;
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
