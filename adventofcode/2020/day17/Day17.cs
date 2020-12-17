using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day17 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 17;

        private const char ActiveChar = '#';
        private Cube3Position[] Universe = Array.Empty<Cube3Position>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            var newDimension = new List<Cube3Position>();

            for (int y = 0; y < inputLines.Length; y++)
            {
                for (int x = 0; x < inputLines[y].Length; x++)
                {
                    if (inputLines[y][x] == ActiveChar)
                    {
                        newDimension.Add(new(x, y, 0));
                    }
                }
            }

            Universe = newDimension.ToArray();
        }

        protected override Task<string> Part1Async()
        {
            int totalActive = 0;
            int count = 0;

            while (count++ < 6)
            {
                (Universe, totalActive) = RunCycle(Universe);
            }

            return Task.FromResult(totalActive.ToString());
        }

        protected override async Task<string> Part2Async()
        {
            await LoadyAsync();

            Cube4Position[] newUniverse = Universe
                                            .Select(p => new Cube4Position(p.X, p.Y, 0, 0))
                                            .ToArray();

            int totalActive = 0;
            int count = 0;

            while (count++ < 6)
            {
                (newUniverse, totalActive) = RunCycle(newUniverse);
            }

            return totalActive.ToString();
        }


        private static (Cube3Position[], int) RunCycle(Cube3Position[] dimensions)
        {
            int totalActive = 0;

            int startX = dimensions.Min(c => c.X) - 1;
            int startY = dimensions.Min(c => c.Y) - 1;
            int startZ = dimensions.Min(c => c.Z) - 1;

            int endX = dimensions.Max(c => c.X) + 1;
            int endY = dimensions.Max(c => c.Y) + 1;
            int endZ = dimensions.Max(c => c.Z) + 1;

            var newj = new List<Cube3Position>();

            for (int z = startZ; z <= endZ; z++)
            {
                for (int y = startY; y <= endY; y++)
                {
                    for (int x = startX; x <= endX; x++)
                    {
                        int activeNeighbours = GetNumberOfActiveNeighbours(dimensions, x, y, z);

                        bool cubeIsActive = dimensions.Any(c => c.X == x && c.Y == y && c.Z == z);

                        if (cubeIsActive && (activeNeighbours == 2 || activeNeighbours == 3))
                        {
                            totalActive++;
                            newj.Add(new(x, y, z));
                        }
                        else if (!cubeIsActive && activeNeighbours == 3)
                        {
                            totalActive++;
                            newj.Add(new(x, y, z));
                        }
                    }
                }
            }

            return (newj.ToArray(), totalActive);
        }

        private static (Cube4Position[], int) RunCycle(Cube4Position[] dimensions)
        {
            int totalActive = 0;

            int startX = dimensions.Min(c => c.X) - 1;
            int startY = dimensions.Min(c => c.Y) - 1;
            int startZ = dimensions.Min(c => c.Z) - 1;
            int startA = dimensions.Min(c => c.A) - 1;

            int endX = dimensions.Max(c => c.X) + 1;
            int endY = dimensions.Max(c => c.Y) + 1;
            int endZ = dimensions.Max(c => c.Z) + 1;
            int endA = dimensions.Max(c => c.A) + 1;

            var newj = new List<Cube4Position>(dimensions.Length * 3);

            for (int a = startA; a <= endA; a++)
            {
                for (int z = startZ; z <= endZ; z++)
                {
                    for (int y = startY; y <= endY; y++)
                    {
                        for (int x = startX; x <= endX; x++)
                        {
                            int activeNeighbours = GetNumberOfActiveNeighbours(dimensions, x, y, z, a);

                            bool cubeIsActive = dimensions.Any(c => c.X == x && c.Y == y && c.Z == z && c.A == a);

                            if (cubeIsActive && (activeNeighbours == 2 || activeNeighbours == 3))
                            {
                                totalActive++;
                                newj.Add(new(x, y, z, a));
                            }
                            else if (!cubeIsActive && activeNeighbours == 3)
                            {
                                totalActive++;
                                newj.Add(new(x, y, z, a));
                            }
                        }
                    }
                }
            }

            return (newj.ToArray(), totalActive);
        }

        private static int GetNumberOfActiveNeighbours(Cube3Position[] cubes, int x, int y, int z)
        {
            int count = 0;

            for (int zn = z - 1; zn <= z + 1; zn++)
            {
                for (int yn = y - 1; yn <= y + 1; yn++)
                {
                    for (int xn = x - 1; xn <= x + 1; xn++)
                    {
                        if (xn == x && yn == y && zn == z)
                            continue; // skip itself

                        if (cubes.Any(c => c.X == xn && c.Y == yn && c.Z == zn))
                        {
                            count++;
                        }
                    }
                }
            }

            return count;
        }

        private static int GetNumberOfActiveNeighbours(Cube4Position[] cubes, int x, int y, int z, int a)
        {
            int count = 0;

            for (int an = a - 1; an <= a + 1; an++)
            {
                for (int zn = z - 1; zn <= z + 1; zn++)
                {
                    for (int yn = y - 1; yn <= y + 1; yn++)
                    {
                        for (int xn = x - 1; xn <= x + 1; xn++)
                        {
                            if (xn == x && yn == y && zn == z && an == a)
                                continue; // skip itself

                            if (cubes.Any(c => c.X == xn &&
                                               c.Y == yn &&
                                               c.Z == zn &&
                                               c.A == an))
                            {
                                count++;
                            }
                        }
                    }
                }
            }

            return count;
        }

        private struct Cube3Position
        {
            public Cube3Position(int x, int y, int z)
            {
                X = x;
                Y = y;
                Z = z;
            }

            public int X { get; }
            public int Y { get; }
            public int Z { get; }

            public override string ToString() => $"X={X} Y={Y} Z={Z}";
        }

        private struct Cube4Position
        {
            public Cube4Position(int x, int y, int z, int a)
            {
                X = x;
                Y = y;
                Z = z;
                A = a;
            }

            public int X { get; }
            public int Y { get; }
            public int Z { get; }
            public int A { get; }

            public override string ToString() => $"X={X} Y={Y} Z={Z} A={A}";
        }
    }
}
