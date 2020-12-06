using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2015
{
    internal class Day07 : BaseDayChallenge
    {
        public override int Year => 2015;
        public override int Day => 7;

        private readonly WireState State = new WireState();
        private readonly List<Instruction> Instructions = new List<Instruction>();

        private readonly Regex outputsRegex = new Regex("[a-z]+$", RegexOptions.Compiled);
        private readonly Regex instructionsRegex = new Regex("^[ 0-9A-Za-z]+", RegexOptions.Compiled);

        protected override async Task LoadyAsync()
        {
            string contents = await File.ReadAllTextAsync(InputFilePath);

            foreach (string line in contents.Split(Environment.NewLine))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string rawInstruction = instructionsRegex.Match(line).Value.Trim();
                string output = outputsRegex.Match(line).Value;

                State.TryAdd(output, 0);

                if (rawInstruction.Contains(nameof(WireOp.AND))
                    || rawInstruction.Contains(nameof(WireOp.OR))
                    || rawInstruction.Contains(nameof(WireOp.LSHIFT))
                    || rawInstruction.Contains(nameof(WireOp.RSHIFT)))
                {
                    string[] param = rawInstruction.Split(' ');

                    var op = Enum.Parse<WireOp>(param[1]);

                    Instructions.Add(new(op, param[0], output, param[2]));
                }
                else if (rawInstruction.Contains(nameof(WireOp.NOT)))
                {
                    string[] param = rawInstruction.Split(' ');

                    Instructions.Add(new(WireOp.NOT, param[1], Output: output));
                }
                else
                {
                    Instructions.Add(new(WireOp.SET, rawInstruction, Output: output));
                }
            }
        }

        protected override Task<string> Part1Async()
        {
            RunInstructions(State, Instructions);
            State.Print();

            State.TryGetValue("a", out ushort solution);

            return Task.FromResult(solution.ToString());
        }

        private static void RunInstructions(WireState state, List<Instruction> instructions)
        {
            instructions.ForEach(i =>
            {
                state[i.Output] = i.Op switch
                {
                    WireOp.SET => state.GetValue(i.Param1),
                    WireOp.AND => (ushort)(state.GetValue(i.Param1) & state.GetValue(i.Param2!)),
                    WireOp.OR => (ushort)(state.GetValue(i.Param1) | state.GetValue(i.Param2!)),
                    WireOp.LSHIFT => (ushort)(state[i.Param1] << ushort.Parse(i.Param2!)),
                    WireOp.RSHIFT => (ushort)(state[i.Param1] >> ushort.Parse(i.Param2!)),
                    WireOp.NOT => (ushort)(~state[i.Param1]),
                    _ => throw new NotImplementedException()
                };
            });
        }

        private class WireState : Dictionary<string, ushort>
        {
            public ushort GetValue(string addressOrValue)
                => TryGetValue(addressOrValue, out ushort value) ? value : ushort.Parse(addressOrValue);

            public void Print()
            {
                foreach (string key in Keys.OrderBy(k => k))
                    Console.WriteLine($"{key}: {this[key]}");
            }
        }

        private record Instruction(WireOp Op, string Param1, string Output, string? Param2 = null)
        {
            public override string ToString()
            {
                return Op switch
                {
                    WireOp.SET => $"{Param1} -> {Output}",
                    WireOp.AND => $"{Param1} AND {Param2} -> {Output}",
                    WireOp.OR => $"{Param1} OR {Param2} -> {Output}",
                    WireOp.LSHIFT => $"{Param1} LSHIFT {Param2} -> {Output}",
                    WireOp.RSHIFT => $"{Param1} RSHIFT {Param2} -> {Output}",
                    WireOp.NOT => $"NOT {Param1} -> {Output}",
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private enum WireOp
        {
            SET,
            AND,
            OR,
            LSHIFT,
            RSHIFT,
            NOT,
        }
    }
}
