using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day12 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 12;

        private readonly Regex CaptureInstructions = new Regex(@"(\w)(\d+)", RegexOptions.Compiled);
        private List<Instruction> Instructions = new List<Instruction>();

        protected override async Task LoadyAsync()
        {
            string contents = await File.ReadAllTextAsync(InputFilePath);

            Instructions = CaptureInstructions
                            .Matches(contents)
                            .Select(m => new Instruction(Enum.Parse<Action>(m.Groups[1].Value, true), int.Parse(m.Groups[2].Value)))
                            .ToList();
        }

        protected override Task<string> Part1Async()
        {
            var facing = Action.E;
            int x = 0, y = 0;
            foreach (var instruction in Instructions)
            {
                if (instruction.Action is Action.L or Action.R)
                {
                    int diff = instruction.Value / 90;

                    int newFacing = ((int)facing) + 4 + (instruction.Action is Action.L
                        ? diff * -1
                        : diff);

                    facing = (Action)(newFacing % 4);
                }
                else if (instruction.Action == Action.F)
                {
                    if (facing == Action.N) y += instruction.Value;
                    else if (facing == Action.E) x += instruction.Value;
                    else if (facing == Action.S) y -= instruction.Value;
                    else if (facing == Action.W) x -= instruction.Value;
                }
                else
                {
                    if (instruction.Action == Action.N) y += instruction.Value;
                    else if (instruction.Action == Action.E) x += instruction.Value;
                    else if (instruction.Action == Action.S) y -= instruction.Value;
                    else if (instruction.Action == Action.W) x -= instruction.Value;
                }
            }

            int manDiff = GetManhattanDistance(0, x, 0, y);

            return Task.FromResult(manDiff.ToString());
        }

        protected override Task<string> Part2Async()
        {
            int x = 0, y = 0;
            int waypointX = 10, waypointY = 1;
            foreach (var instruction in Instructions)
            {
                if (instruction.Action is Action.L or Action.R)
                {
                    int diff = instruction.Value / 90;
                    while (diff > 0)
                    {
                        if (instruction.Action is Action.R)
                        {
                            int nextWaypointY = waypointX * -1;
                            waypointX = waypointY * 1;
                            waypointY = nextWaypointY;
                        }
                        else
                        {
                            int nextWaypointY = waypointX * 1;
                            waypointX = waypointY * -1;
                            waypointY = nextWaypointY;
                        }
                        diff--;
                    }
                }
                else if (instruction.Action == Action.F)
                {
                    x += instruction.Value * waypointX;
                    y += instruction.Value * waypointY;
                }
                else
                {
                    if (instruction.Action == Action.N) waypointY += instruction.Value;
                    else if (instruction.Action == Action.E) waypointX += instruction.Value;
                    else if (instruction.Action == Action.S) waypointY -= instruction.Value;
                    else if (instruction.Action == Action.W) waypointX -= instruction.Value;
                }
            }

            int manDiff = GetManhattanDistance(0, x, 0, y);

            return Task.FromResult(manDiff.ToString());
        }

        public static int GetManhattanDistance(int x1, int x2, int y1, int y2)
            => Math.Abs(x1 - x2) + Math.Abs(y1 - y2);

        private record Instruction(Action Action, int Value);

        private enum Action
        {
            N,
            E,
            S,
            W,
            L,
            R,
            F,
        }
    }
}
