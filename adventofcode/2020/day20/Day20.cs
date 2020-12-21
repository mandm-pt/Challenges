using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day20 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 20;

        private Tile[] Tiles = Array.Empty<Tile>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            var tiles = new List<Tile>();
            var tileLines = new List<string>();
            foreach (var line in inputLines)
            {
                if (string.IsNullOrWhiteSpace(line) && tileLines.Count > 0)
                {
                    tiles.Add(GetTile(tileLines.ToArray()));
                    tileLines.Clear();
                    continue;
                }

                tileLines.Add(line);
            }
            tiles.Add(GetTile(tileLines.ToArray()));

            Tiles = tiles.ToArray();
        }

        private static Tile GetTile(string[] tileLines)
        {
            var id = ulong.Parse(tileLines[0].Replace("Tile ", "").Replace(":", ""));

            string top = tileLines[1];
            string right = "";

            for (int i = 1; i < tileLines.Length; i++)
                right += tileLines[i][^1];

            string bottom = tileLines[^1];
            string left = "";

            for (int i = 1; i < tileLines.Length; i++)
                left += tileLines[i][0];

            return new Tile(id, top, right, bottom, left);
        }

        protected override Task<string> Part1Async()
        {
            var puzzle = TrySolvePuzzle(Tiles);

            if (puzzle == null)
            {
                return Task.FromResult("Can't solve puzzle".ToString());
            }

            ulong result = puzzle.GetCorners().Aggregate((ulong)1, (a, b) => a * b.Id);

            return Task.FromResult(result.ToString());
        }

        private static Puzzle? TrySolvePuzzle(Tile[] tiles)
        {
            var tilesPerLine = (int)Math.Sqrt(tiles.Length);

            foreach (var tile in tiles)
            {
                foreach (var piece in tile.Combinations)
                {
                    var puzzle = new Puzzle(tilesPerLine);
                    puzzle.Next();
                    puzzle.Add(piece);

                    if (PuzzleHasSolution(puzzle, tiles.Where(t => t.Id != piece.Id).ToList()))
                    {
                        return puzzle;
                    }
                }
            }

            return null;
        }

        private static bool PuzzleHasSolution(Puzzle puzzle, IList<Tile> remainingTiles)
        {
            while (puzzle.Next())
            {
                var compatibleTile = remainingTiles.Select(t =>
                            IsCompatible(t, puzzle[puzzle.X, puzzle.Y - 1], puzzle[puzzle.X + 1, puzzle.Y]
                                , puzzle[puzzle.X, puzzle.Y + 1], puzzle[puzzle.X - 1, puzzle.Y]))
                        .FirstOrDefault(p => p != null);

                if (compatibleTile == null)
                    return false;

                puzzle.Add(compatibleTile!);

                remainingTiles.Remove(remainingTiles.First(t => t.Id == compatibleTile.Id));
            }

            return true;
        }

        private static PieceCombination? IsCompatible(Tile tile, PieceCombination? topTile, PieceCombination? rightTile,
            PieceCombination? bottomTile, PieceCombination? leftTile)
        {
            foreach (var combination in tile.Combinations)
            {
                bool isCompatible = true;

                if (topTile != null)
                    isCompatible &= combination.Top == topTile.Bottom;

                if (rightTile != null)
                    isCompatible &= combination.Right == rightTile.Left;

                if (bottomTile != null)
                    isCompatible &= combination.Bottom == bottomTile.Top;

                if (leftTile != null)
                    isCompatible &= combination.Left == leftTile.Right;

                if (isCompatible)
                    return combination;
            }

            return null;

        }

        private class Puzzle
        {
            private readonly PieceCombination[,] grid;
            private readonly int size;
            private int x = 0;
            private int y = 1;

            public Puzzle(int size)
            {
                // +2 to not deal with index out of bounds. Border of -+1
                grid = new PieceCombination[size + 2, size + 2];
                this.size = size;
            }

            public PieceCombination this[int x, int y] => grid[x + 1, y + 1];

            public int X => x - 1;
            public int Y => y - 1;

            public void Add(PieceCombination piece) => grid[x, y] = piece;

            public bool Next()
            {
                if (x < size)
                {
                    x++;
                    return true;
                }

                if (y < size)
                {
                    y++;
                    x = 1;
                    return true;
                }

                return false;
            }

            public IEnumerable<PieceCombination> GetCorners()
            {
                yield return this[0, 0];
                yield return this[0, size - 1];
                yield return this[size - 1, 0];
                yield return this[size - 1, size - 1];
            }
        }

        private record Tile
        {
            private readonly PieceCombination[] combinations = Array.Empty<PieceCombination>();

            public Tile(ulong id, string top, string right, string bottom, string left)
            {
                Id = id;

                combinations = new PieceCombination[8];
                combinations[0] = new PieceCombination(id, top, right, bottom, left);
                combinations[1] = combinations[0].RotateRight();
                combinations[2] = combinations[1].RotateRight();
                combinations[3] = combinations[2].RotateRight();

                combinations[4] = combinations[0].FlipVertical();
                combinations[5] = combinations[1].FlipVertical();
                combinations[6] = combinations[2].FlipVertical();
                combinations[7] = combinations[3].FlipVertical();
            }

            public ulong Id { get; }

            public PieceCombination[] Combinations => combinations;
        }

        private record PieceCombination
        {
            public PieceCombination(ulong id, string top, string right, string bottom, string left)
            {
                Id = id;
                Top = top;
                Right = right;
                Bottom = bottom;
                Left = left;
            }

            public ulong Id { get; }
            public string Top { get; }
            public string Right { get; }
            public string Bottom { get; }
            public string Left { get; }
            public bool IsEmpty { get; }

            public PieceCombination RotateRight()
            {
                var tmp = Top;
                var newTop = new string(Left.Reverse().ToArray());
                var newLeft = Bottom;
                var newBottom = new string(Right.Reverse().ToArray());
                var newRight = tmp;

                return new(Id, newTop, newRight, newBottom, newLeft);
            }

            public PieceCombination FlipVertical()
            {
                var tmp = Top;
                var newTop = Bottom;
                var newBottom = tmp;

                return new(Id, newTop, new string(Right.Reverse().ToArray()), newBottom,
                    new string(Left.Reverse().ToArray()));
            }
        }
    }
}