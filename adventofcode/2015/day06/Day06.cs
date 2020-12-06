using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2015
{
    internal class Day06 : BaseDayChallenge
    {
        public override int Year => 2015;
        public override int Day => 6;

        private readonly int xMax = 1000, yMax = 1000;
        private readonly Regex coordinatesRegex = new Regex(@"\d+,\d+", RegexOptions.Compiled);
        private readonly List<Instruction> instructions = new List<Instruction>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            foreach (string line in inputLines)
            {
                var op = line switch
                {
                    string when line.StartsWith("turn on") => LightOp.On,
                    string when line.StartsWith("turn off") => LightOp.Off,
                    _ => LightOp.Toggle
                };

                var matches = coordinatesRegex.Matches(line);
                int[] from = matches[0].Value.Split(',').Select(int.Parse).ToArray();
                int[] to = matches[1].Value.Split(',').Select(int.Parse).ToArray();

                instructions.Add(new(op, new(from[0], from[1]), new(to[0], to[1])));
            }
        }

        protected override Task<string> Part1Async()
        {
            bool[,] matrix = new bool[xMax, yMax];

            instructions.ForEach(i => ExecuteInstruction(ref matrix, i, (s, ins) => ins.Op switch
            {
                LightOp.On => true,
                LightOp.Off => false,
                LightOp.Toggle => !s,
                _ => throw new NotImplementedException(),
            }));

            int lightsOn = 0;
            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    lightsOn += matrix[x, y] ? 1 : 0;
                }
            }

            return Task.FromResult(lightsOn.ToString());
        }

        protected override Task<string> Part2Async()
        {
            int[,] matrix = new int[xMax, yMax];

            instructions.ForEach(i => ExecuteInstruction(ref matrix, i, (s, ins) => ins.Op switch
            {
                LightOp.On => s + 1,
                LightOp.Off => s + (s > 0 ? -1 : 0),
                LightOp.Toggle => s + 2,
                _ => throw new NotImplementedException(),
            }));

            int brightness = 0;
            for (int x = 0; x < xMax; x++)
            {
                for (int y = 0; y < yMax; y++)
                {
                    brightness += matrix[x, y];
                }
            }

            return Task.FromResult(brightness.ToString());
        }

        private static void ExecuteInstruction<T>(ref T[,] matrix, Instruction i, Func<T, Instruction, T> opLogic)
        {
            for (int x = i.From.X; x <= i.To.X; x++)
            {
                for (int y = i.From.Y; y <= i.To.Y; y++)
                {
                    matrix[x, y] = opLogic(matrix[x, y], i);
                }
            }
        }

        private record Instruction(LightOp Op, Position From, Position To);

        private enum LightOp
        {
            On,
            Off,
            Toggle
        }

        private record Position(int X, int Y);
    }
}
