using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day11 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 11;

        private int xMax = 0;
        private int yMax = 0;
        private Seat[,] Seats = new Seat[0, 0];

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            xMax = inputLines[0].Length;
            yMax = inputLines.Length;

            Seats = new Seat[xMax, yMax];


            for (int y = 0; y < yMax; y++)
            {
                for (int x = 0; x < xMax; x++)
                {
                    Seats[x, y] = new Seat(inputLines[y][x]);
                }
            }
        }

        protected override Task<string> Part1Async()
        {
            bool seatsChanged;
            var newSeatLayout = (Seat[,])Seats.Clone();

            do
            {
                seatsChanged = false;
                var currentSeatLayout = (Seat[,])newSeatLayout.Clone();

                //// Debug
                //PrintGrid(currentSeatLayout);

                for (int x = 0; x < xMax; x++)
                {
                    for (int y = 0; y < yMax; y++)
                    {
                        if (currentSeatLayout[x, y].IsFloor)
                            continue;

                        if (currentSeatLayout[x, y].IsEmpty &&
                            !GetAdjacents(new(x, y)).Any(p => currentSeatLayout[p.X, p.Y].IsOccupied))
                        {
                            newSeatLayout[x, y] = currentSeatLayout[x, y].Occupy();
                            seatsChanged = true;
                        }
                        else if (currentSeatLayout[x, y].IsOccupied &&
                            GetAdjacents(new(x, y)).Count(p => currentSeatLayout[p.X, p.Y].IsOccupied) >= 4)
                        {
                            newSeatLayout[x, y] = currentSeatLayout[x, y].Empty();
                            seatsChanged = true;
                        }
                    }
                }
            } while (seatsChanged);

            int totalOccupied = 0;
            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    totalOccupied += newSeatLayout[x, y].IsOccupied ? 1 : 0;
                }
            }

            return Task.FromResult(totalOccupied.ToString());
        }

        protected override Task<string> Part2Async()
        {
            bool seatsChanged;
            var newSeatLayout = (Seat[,])Seats.Clone();

            do
            {
                seatsChanged = false;
                var currentSeatLayout = (Seat[,])newSeatLayout.Clone();

                //// Debug
                //PrintGrid(currentSeatLayout);

                for (int x = 0; x < xMax; x++)
                {
                    for (int y = 0; y < yMax; y++)
                    {
                        if (currentSeatLayout[x, y].IsFloor)
                            continue;

                        if (currentSeatLayout[x, y].IsEmpty &&
                            !GetFarAdjacents(new(x, y)).Any(p => currentSeatLayout[p.X, p.Y].IsOccupied))
                        {
                            newSeatLayout[x, y] = currentSeatLayout[x, y].Occupy();
                            seatsChanged = true;
                        }
                        else if (currentSeatLayout[x, y].IsOccupied &&
                            GetFarAdjacents(new(x, y)).Count(p => currentSeatLayout[p.X, p.Y].IsOccupied) >= 5)
                        {
                            newSeatLayout[x, y] = currentSeatLayout[x, y].Empty();
                            seatsChanged = true;
                        }
                    }
                }
            } while (seatsChanged);

            int totalOccupied = 0;
            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    totalOccupied += newSeatLayout[x, y].IsOccupied ? 1 : 0;
                }
            }

            return Task.FromResult(totalOccupied.ToString());
        }

        private IEnumerable<Position> GetAdjacents(Position pos)
        {
            var adjacents = new List<Position>
            {
                new(pos.X - 1, pos.Y - 1),
                new(pos.X, pos.Y - 1),
                new(pos.X + 1, pos.Y - 1),

                new(pos.X - 1, pos.Y),
                new(pos.X + 1, pos.Y),

                new(pos.X - 1, pos.Y + 1),
                new(pos.X, pos.Y + 1),
                new(pos.X + 1, pos.Y + 1)
            }
            .Where(IsInsideBounds)
            .ToArray();

            return adjacents;
        }

        private IEnumerable<Position> GetFarAdjacents(Position pos)
        {
            var adjacents = new List<Position>();

            int i = 0, maxRounds = Math.Max(xMax, yMax);

            int x = pos.X, y = pos.Y;
            bool hasTopLeft, hasTop, hasTopRight, hasLeft, hasRight, hasBottomLeft,
                hasBottom, hasBottomRight;

            hasTopLeft = hasTop = hasTopRight = hasLeft = hasRight = hasBottomLeft =
                hasBottom = hasBottomRight = false;

            for (i = 1; i < maxRounds; i++)
            {
                var topLeft = new Position(x - i, y - i);
                if (!hasTopLeft && (hasTopLeft = AddSeat(topLeft)))
                    adjacents.Add(new(topLeft.X, topLeft.Y));

                var top = new Position(x, y - i);
                if (!hasTop && (hasTop = AddSeat(top)))
                    adjacents.Add(new(top.X, top.Y));

                var topRight = new Position(x + i, y - i);
                if (!hasTopRight && (hasTopRight = AddSeat(topRight)))
                    adjacents.Add(new(topRight.X, topRight.Y));

                var left = new Position(x - i, y);
                if (!hasLeft && (hasLeft = AddSeat(left)))
                    adjacents.Add(new(left.X, left.Y));

                var right = new Position(x + i, y);
                if (!hasRight && (hasRight = AddSeat(right)))
                    adjacents.Add(new(right.X, right.Y));

                var bottomLeft = new Position(x - i, y + i);
                if (!hasBottomLeft && (hasBottomLeft = AddSeat(bottomLeft)))
                    adjacents.Add(new(bottomLeft.X, bottomLeft.Y));

                var bottom = new Position(x, y + i);
                if (!hasBottom && (hasBottom = AddSeat(bottom)))
                    adjacents.Add(new(bottom.X, bottom.Y));

                var bottomRight = new Position(x + i, y + i);
                if (!hasBottomRight && (hasBottomRight = AddSeat(bottomRight)))
                    adjacents.Add(new(bottomRight.X, bottomRight.Y));
            }

            return adjacents;
        }

        private bool IsInsideBounds(Position p) => p.X >= 0 && p.Y >= 0
                && p.X < xMax && p.Y < yMax;

        private bool AddSeat(Position p)
            => IsInsideBounds(p) && (Seats[p.X, p.Y].IsEmpty || Seats[p.X, p.Y].IsOccupied);

        private void PrintGrid(Seat[,] seats)
        {
            // Debug
            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    Console.Write(seats[x, y].C);
                }
                Console.WriteLine();
            }
            Console.WriteLine("========== New Round ===============");
        }

        private record Seat(char C)
        {
            public bool IsFloor => C == '.';
            public bool IsEmpty => C == 'L';
            public bool IsOccupied => C == '#';

            public Seat Occupy() => new('#');
            public Seat Empty() => new('L');
        }

        private record Position(int X, int Y);
    }
}
