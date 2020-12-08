using System;
using System.Collections.Generic;
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

        private readonly Regex instructionsRegex = new Regex(@"(^[ 0-9A-Za-z]+)\s->\s([a-z]+)", RegexOptions.Compiled);

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            var instructions = inputLines
                .Where(line => !string.IsNullOrWhiteSpace(line))
                .Select(line =>
                {
                    var match = instructionsRegex.Match(line);

                    string rawInstruction = match.Groups[1].Value;
                    string output = match.Groups[2].Value;

                    if (rawInstruction.Contains(nameof(Gate.AND))
                        || rawInstruction.Contains(nameof(Gate.OR))
                        || rawInstruction.Contains(nameof(Gate.LSHIFT))
                        || rawInstruction.Contains(nameof(Gate.RSHIFT)))
                    {
                        string[] param = rawInstruction.Split(' ');

                        var op = Enum.Parse<Gate>(param[1]);

                        return new Instruction(op, param[0], output, param[2]);
                    }
                    else if (rawInstruction.Contains(nameof(Gate.NOT)))
                    {
                        string[] param = rawInstruction.Split(' ');

                        return new Instruction(Gate.NOT, param[1], Output: output);
                    }

                    return new Instruction(Gate.SET, rawInstruction, Output: output);
                });

            Circuit = new EletronicCircuit(instructions.ToArray());
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

            private EletronicCircuit() : this(Array.Empty<Instruction>()) { }

            public EletronicCircuit(IEnumerable<Instruction> instructions)
                : base()
            {
                this.instructions = instructions.ToList();
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
