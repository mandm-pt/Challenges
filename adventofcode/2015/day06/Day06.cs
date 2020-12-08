using System;
using System.Collections.Generic;
using System.IO;
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
        private readonly Regex ParseInstructionsRegex = new Regex(@"[turn ]?(on|off|toggle)\s(\d+),(\d+)\sthrough\s(\d+),(\d+)", RegexOptions.Compiled);
        private List<Instruction> instructions = new List<Instruction>();

        protected override async Task LoadyAsync()
        {
            string contents = await File.ReadAllTextAsync(InputFilePath);

            instructions = ParseInstructionsRegex.Matches(contents).Select(m =>
            {
                var op = Enum.Parse<LightOp>(m.Groups[1].Value, true);
                var from = new Position(int.Parse(m.Groups[2].Value), int.Parse(m.Groups[3].Value));
                var to = new Position(int.Parse(m.Groups[4].Value), int.Parse(m.Groups[5].Value));

                return new Instruction(op, from, to);
            }).ToList();
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
