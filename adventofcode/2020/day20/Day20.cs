using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day20 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 20;

        private Tile[] Tiles = Array.Empty<Tile>();
        private Puzzle PuzzleSolved = new Puzzle(0);

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

            char[,] content = new char[tileLines[0].Length, tileLines.Length - 1];

            for (int y = 1; y < tileLines.Length; y++)
            {
                for (int x = 0; x < tileLines[y].Length; x++)
                {
                    content[x, y - 1] = tileLines[y][x];
                }
            }

            return new Tile(id, tileLines.Length - 1, content);
        }

        protected override Task<string> Part1Async()
        {
            var puzzle = TrySolvePuzzle(Tiles);

            if (puzzle == null)
            {
                return Task.FromResult("Can't solve puzzle".ToString());
            }

            PuzzleSolved = puzzle;
            ulong result = puzzle.GetCorners().Aggregate((ulong)1, (a, b) => a * b.Id);

            return Task.FromResult(result.ToString());
        }

        protected override Task<string> Part2Async()
        {
            if (PuzzleSolved == null)
            {
                return Task.FromResult("No puzzle to work with".ToString());
            }

            var monsters = GetMonstersCount(PuzzleSolved);

            if (monsters.count == 0)
                return Task.FromResult("No solution found.".ToString());

            int result = monsters.image.Count(c => c == '#');
            result -= 15 * monsters.count;

            return Task.FromResult(result.ToString());
        }

        private static (int count, string image) GetMonstersCount(Puzzle puzzle)
        {
            var monsterBody = new Regex(@"#.{4}##.{4}##.{4}###", RegexOptions.Compiled | RegexOptions.Multiline);

            var image = GetImageMatrixFromPuzzle(puzzle);
            string plainImage = GetPlainImage(image.size, image.matrix);

            int count = 0;
            for (int i = 0; i < 8; i++)
            {
                foreach (Match matchBody in monsterBody.Matches(plainImage))
                {
                    var lines = plainImage.Split(Environment.NewLine);
                    var rowBody = plainImage[0..matchBody.Index].Count(c => c == '\n');
                    int startIdx = matchBody.Index - rowBody * lines[0].Length - (rowBody * 2);
                    int startBody = lines[rowBody].IndexOf(matchBody.Value, startIdx);

                    bool isMonster = lines[rowBody - 1][18 + startBody] == '#' &&
                        lines[rowBody + 1][1 + startBody] == '#' && lines[rowBody + 1][4 + startBody] == '#' &&
                        lines[rowBody + 1][7 + startBody] == '#' && lines[rowBody + 1][10 + startBody] == '#' &&
                        lines[rowBody + 1][13 + startBody] == '#' && lines[rowBody + 1][16 + startBody] == '#';

                    if (isMonster)
                    {
                        count++;
                    }
                }

                if (count > 0)
                    break;

                image.matrix = RotateLeftMatrix(image.size, image.matrix);
                plainImage = GetPlainImage(image.size, image.matrix);

                if (i == 3)
                    image.matrix = FlipMatrix(image.size, image.matrix);
            }

            if (count == 0)
                return (0, string.Empty);

            return (count, plainImage);

        }

        private static (int size, char[,] matrix) GetImageMatrixFromPuzzle(Puzzle puzzle)
        {

            var newPuzzleSize = (puzzle[0, 0].Size - 2) * puzzle.Size;
            var newPuzzleContent = new char[newPuzzleSize, newPuzzleSize];
            for (int y = 0; y < puzzle.Size; y++)
            {
                for (int x = 0; x < puzzle.Size; x++)
                {
                    for (int innerY = 0; innerY < puzzle[x, y].CenterSize; innerY++)
                    {
                        for (int innerX = 0; innerX < puzzle[x, y].CenterSize; innerX++)
                        {
                            int finalX = (x * puzzle[x, y].CenterSize) + innerX;
                            int finalY = (y * puzzle[x, y].CenterSize) + innerY;
                            newPuzzleContent[finalX, finalY] = puzzle[x, y].Center[innerX, innerY];
                        }
                    }
                }
            }

            return (newPuzzleSize, newPuzzleContent);
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

        private static string GetPlainImage(int size, char[,] matrix)
        {
            var sb = new StringBuilder();
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    sb.Append(matrix[x, y]);
                }
                sb.AppendLine();
            }

            return sb.ToString();
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
            private int x = 0;
            private int y = 1;

            public Puzzle(int size)
            {
                // +2 to not deal with index out of bounds. Border of -+1
                grid = new PieceCombination[size + 2, size + 2];
                Size = size;
            }

            public PieceCombination this[int x, int y] => grid[x + 1, y + 1];

            public int X => x - 1;
            public int Y => y - 1;

            public int Size { get; }

            public void Add(PieceCombination piece) => grid[x, y] = piece;

            public bool Next()
            {
                if (x < Size)
                {
                    x++;
                    return true;
                }

                if (y < Size)
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
                yield return this[0, Size - 1];
                yield return this[Size - 1, 0];
                yield return this[Size - 1, Size - 1];
            }
        }

        private record Tile
        {
            private readonly PieceCombination[] combinations = Array.Empty<PieceCombination>();

            public Tile(ulong id, int size, char[,] content)
            {
                Id = id;

                combinations = new PieceCombination[8];
                combinations[0] = new PieceCombination(id, size, content);
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
            private readonly char[,] content;

            public PieceCombination(ulong id, int size, char[,] content)
            {
                Id = id;
                Size = size;
                this.content = content;
            }

            public ulong Id { get; }
            public int Size { get; }
            public int CenterSize => Size - 2;

            public string Top => GetSide(Side.Top);
            public string Right => GetSide(Side.Right);
            public string Bottom => GetSide(Side.Bottom);
            public string Left => GetSide(Side.Left);
            public char[,] Center => GetCenter();

            public bool IsEmpty { get; }

            private string GetSide(Side side)
            {
                int x, y;
                var line = new char[Size];

                switch (side)
                {
                    case Side.Top:
                    case Side.Bottom:
                        y = side == Side.Top ? 0 : Size - 1;

                        for (x = 0; x < Size; x++)
                        {
                            line[x] = content[x, y];
                        }
                        break;
                    case Side.Right:
                    case Side.Left:

                        x = side == Side.Left ? 0 : Size - 1;

                        for (y = 0; y < Size; y++)
                        {
                            line[y] = content[x, y];
                        }
                        break;
                    default:
                        throw new ApplicationException();
                }

                return new string(line);
            }

            private char[,] GetCenter()
            {
                var center = new char[Size - 2, Size - 2];
                for (int y = 1; y < Size - 1; y++)
                {
                    for (int x = 1; x < Size - 1; x++)
                    {
                        center[x - 1, y - 1] = content[x, y];
                    }
                }

                return center;
            }

            public PieceCombination RotateRight() => new(Id, Size, RotateLeftMatrix(Size, content));

            public PieceCombination FlipVertical() => new(Id, Size, FlipMatrix(Size, content));

            private enum Side
            {
                Top,
                Right,
                Bottom,
                Left
            }
        }

        private static char[,] RotateLeftMatrix(int size, char[,] matrix)
        {
            char[,] newMatrix = new char[size, size];

            for (int i = 0; i < size; ++i)
            {
                for (int j = 0; j < size; ++j)
                {
                    newMatrix[i, j] = matrix[size - j - 1, i];
                }
            }

            return newMatrix;
        }

        private static char[,] FlipMatrix(int size, char[,] matrix)
        {
            char[,] newMatrix = new char[size, size];

            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    newMatrix[x, size - 1 - y] = matrix[x, y];
                }
            }

            return newMatrix;
        }
    }
}