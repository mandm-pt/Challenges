using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day17 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 17;

        private const char ActiveChar = '#';
        private const int MaxGridSize = 20;
        private bool[,,] Cube = new bool[MaxGridSize, MaxGridSize, MaxGridSize];

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            Cube = new bool[MaxGridSize, MaxGridSize, MaxGridSize];

            for (int x = 0; x < inputLines.Length; x++)
            {
                for (int y = 0; y < inputLines[x].Length; y++)
                {
                    Cube[y + 5, x + 5, 10] = inputLines[x][y] == ActiveChar;
                }
            }
        }

        protected override Task<string> Part1Async()
        {
            int totalActive = 0;
            bool[,,] grid = Cube;
            int count = 0;

            while (count++ < 6)
            {
                (grid, totalActive) = RunCycle(grid);
            }

            return Task.FromResult(totalActive.ToString());
        }

        private static (bool[,,], int) RunCycle(bool[,,] grid)
        {
            int totalActive = 0;

            bool[,,] newGrid = new bool[MaxGridSize, MaxGridSize, MaxGridSize];

            for (int x = 0; x < MaxGridSize; x++)
            {
                for (int y = 0; y < MaxGridSize; y++)
                {
                    for (int z = 0; z < MaxGridSize; z++)
                    {
                        int activeNeighbours = GetNumberOfActiveNeighbours(grid, x, y, z);

                        if (grid[x, y, z] && (activeNeighbours == 2 || activeNeighbours == 3))
                        {
                            totalActive++;
                            newGrid[x, y, z] = true;
                        }
                        else if (!grid[x, y, z] && activeNeighbours == 3)
                        {
                            totalActive++;
                            newGrid[x, y, z] = true;
                        }
                    }
                }
            }

            return (grid, totalActive);
        }

        private static int GetNumberOfActiveNeighbours(bool[,,] grid, int x, int y, int z)
        {
            int count = 0;

            for (int xn = x - 1; xn < x + 1; xn++)
            {
                for (int yn = y - 1; yn < y + 1; yn++)
                {
                    for (int zn = z - 1; zn < z + 1; zn++)
                    {
                        if (xn == x && yn == y && zn == z)
                            continue; // skip itself

                        if (PositionInsideBound(xn) && PositionInsideBound(yn) && PositionInsideBound(zn) &&
                            grid[xn, yn, zn])
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        private static bool PositionInsideBound(int pos) => pos >= 0 && pos < MaxGridSize;
    }
}
