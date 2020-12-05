using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace AoC.Solutions._2015
{
    internal class Day03 : BaseDayChallenge
    {
        public override int Year => 2015;
        public override int Day => 3;

        private char[] Moves = Array.Empty<char>();

        protected override async Task LoadyAsync() => Moves = (await File.ReadAllTextAsync(InputFilePath)).ToCharArray();

        protected override Task<string> Part1Async()
        {
            var houses = new Dictionary<Position, bool>();
            int x = 0, y = 0;

            foreach (char direction in Moves)
            {
                houses.TryAdd(new(x, y), true);

                if (direction == '^') y++;
                else if (direction == 'v') y--;
                else if (direction == '>') x++;
                else if (direction == '<') x--;
            }

            return Task.FromResult(houses.Count.ToString());
        }

        protected override Task<string> Part2Async()
        {
            var houses = new Dictionary<Position, bool>();
            int xSanta = 0, ySanta = 0;
            int xRobo = 0, yRobo = 0;

            ref int x = ref xSanta, y = ref ySanta;
            for (int i = 0; i < Moves.Length; i++)
            {
                houses.TryAdd(new(x, y), true);

                if (i % 2 == 0)
                {
                    x = ref xSanta;
                    y = ref ySanta;
                }
                else
                {
                    x = ref xRobo;
                    y = ref yRobo;
                }

                if (Moves[i] == '^') y++;
                else if (Moves[i] == 'v') y--;
                else if (Moves[i] == '>') x++;
                else if (Moves[i] == '<') x--;
            }

            return Task.FromResult(houses.Count.ToString());
        }

        private record Position(int X, int Y);
    }
}
