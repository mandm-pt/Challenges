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

        private EletronicCircuit Circuit = EletronicCircuit.Empty;

        private readonly Regex outputsRegex = new Regex("[a-z]+$", RegexOptions.Compiled);
        private readonly Regex instructionsRegex = new Regex("^[ 0-9A-Za-z]+", RegexOptions.Compiled);

        protected override async Task LoadyAsync()
        {
            string contents = await File.ReadAllTextAsync(InputFilePath);

            var instructions = new List<Instruction>();
            foreach (string line in contents.Split(Environment.NewLine))
            {
                if (string.IsNullOrWhiteSpace(line))
                    continue;

                string rawInstruction = instructionsRegex.Match(line).Value.Trim();
                string output = outputsRegex.Match(line).Value;

                if (rawInstruction.Contains(nameof(Gate.AND))
                    || rawInstruction.Contains(nameof(Gate.OR))
                    || rawInstruction.Contains(nameof(Gate.LSHIFT))
                    || rawInstruction.Contains(nameof(Gate.RSHIFT)))
                {
                    string[] param = rawInstruction.Split(' ');

                    var op = Enum.Parse<Gate>(param[1]);

                    instructions.Add(new(op, param[0], output, param[2]));
                }
                else if (rawInstruction.Contains(nameof(Gate.NOT)))
                {
                    string[] param = rawInstruction.Split(' ');

                    instructions.Add(new(Gate.NOT, param[1], Output: output));
                }
                else
                {
                    instructions.Add(new(Gate.SET, rawInstruction, Output: output));
                }
            }

            Circuit = new EletronicCircuit(instructions);
        }

        protected override Task<string> Part1Async()
        {
            Circuit.Run();
            Circuit.TryGetValue("a", out ushort solution);

            return Task.FromResult(solution.ToString());
        }

        protected override Task<string> Part2Async()
        {
            Circuit.TryGetValue("a", out ushort part1State);

            Circuit.Reset();
            Circuit["b"] = part1State;
            Circuit.Run();

            Circuit.TryGetValue("a", out ushort solution);

            return Task.FromResult(solution.ToString());
        }

        private class EletronicCircuit : Dictionary<string, ushort>
        {
            public static EletronicCircuit Empty = new EletronicCircuit();
            private readonly List<Instruction> instructions;

            private EletronicCircuit() : this(new List<Instruction>()) { }

            public EletronicCircuit(List<Instruction> instructions)
                : base()
            {
                this.instructions = instructions;
            }

            public void Run() => instructions.ForEach(RunInstruction);

            public void Reset() => Keys.ToList().ForEach(k => Remove(k));

            public void Print() => Keys.OrderBy(k => k).ToList().ForEach(k => Console.WriteLine($"{k}: {this[k]}"));

            private void RunInstruction(Instruction i)
            {
                this[i.Output] = i.Op switch
                {
                    Gate.SET => GetValue(i.Param1),
                    Gate.AND => (ushort)(GetValue(i.Param1) & GetValue(i.Param2!)),
                    Gate.OR => (ushort)(GetValue(i.Param1) | GetValue(i.Param2!)),
                    Gate.LSHIFT => (ushort)(GetValue(i.Param1) << ushort.Parse(i.Param2!)),
                    Gate.RSHIFT => (ushort)(GetValue(i.Param1) >> ushort.Parse(i.Param2!)),
                    Gate.NOT => (ushort)(~GetValue(i.Param1)),
                    _ => throw new NotImplementedException()
                };
            }

            private ushort GetValue(string addressOrValue)
            {
                bool success = TryGetValue(addressOrValue, out ushort value);

                if (!success)
                {
                    var i = instructions.FirstOrDefault(i => i.Output == addressOrValue);

                    if (i != null)
                    {
                        RunInstruction(i);
                        return this[i.Output];
                    }

                    return ushort.Parse(addressOrValue);
                }

                return value;
            }
        }

        private record Instruction(Gate Op, string Param1, string Output, string? Param2 = null)
        {
            public override string ToString()
            {
                return Op switch
                {
                    Gate.SET => $"{Param1} -> {Output}",
                    Gate.AND => $"{Param1} AND {Param2} -> {Output}",
                    Gate.OR => $"{Param1} OR {Param2} -> {Output}",
                    Gate.LSHIFT => $"{Param1} LSHIFT {Param2} -> {Output}",
                    Gate.RSHIFT => $"{Param1} RSHIFT {Param2} -> {Output}",
                    Gate.NOT => $"NOT {Param1} -> {Output}",
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private enum Gate
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
