using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day08 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 8;

        private readonly Regex CaptureInstructions = new Regex(@"(nop|acc|jmp) [+-]\d+", RegexOptions.Compiled);
        private readonly List<Instruction> BootCode = new List<Instruction>();

        protected override async Task LoadyAsync()
        {
            string contents = await File.ReadAllTextAsync(InputFilePath);

            foreach (Match match in CaptureInstructions.Matches(contents))
            {
                string[] parts = match.Value.Split(' ');

                var code = Enum.Parse<OpCode>(parts[0], true);
                int arg = int.Parse(parts[1].Replace('+', ' '));

                BootCode.Add(new(code, arg));
            }
        }

        protected override Task<string> Part1Async()
        {
            (int result, bool success) = new GameConsole(BootCode.ToArray(),
                                            new InfiniteLoopValidator(),
                                            new OutOfBoundsValidator(BootCode.Count)
                                        ).Run();

            return Task.FromResult(result.ToString());
        }

        protected override Task<string> Part2Async()
        {
            for (int i = 0; ; i++)
            {
                Instruction[] newCode;
                if (BootCode[i].OpCode == OpCode.Jmp)
                {
                    newCode = (Instruction[])BootCode.ToArray().Clone();
                    newCode[i] = new(OpCode.Nop, BootCode[i].Argument);
                }
                else if (BootCode[i].OpCode == OpCode.Nop)
                {
                    newCode = (Instruction[])BootCode.ToArray().Clone();
                    newCode[i] = new(OpCode.Jmp, BootCode[i].Argument);
                }
                else
                    continue;

                (int result, bool success) = new GameConsole(newCode,
                                                new InfiniteLoopValidator(),
                                                new OutOfBoundsValidator(newCode.Length)
                                            ).Run();

                if (success)
                {
                    return Task.FromResult(result.ToString());
                }
            }

            throw new ApplicationException();
        }

        private class GameConsole
        {
            private readonly Instruction[] instructions;
            private readonly IInstructionValidation[] validators;

            public GameConsole(Instruction[] instructions, params IInstructionValidation[] validators)
            {
                this.instructions = instructions;
                this.validators = validators;
            }

            public (int, bool) Run()
            {
                int acc = 0, ip = 0;
                while (validators.All(v => v.CanRunInstruction(ip)))
                {
                    switch (instructions[ip].OpCode)
                    {
                        case OpCode.Nop: break;
                        case OpCode.Acc: acc += instructions[ip].Argument; break;
                        case OpCode.Jmp: ip += instructions[ip].Argument; continue;
                        default: throw new Exception();
                    }

                    ip++;
                }

                return (acc, ip == instructions.Length);
            }
        }

        private interface IInstructionValidation
        {
            bool CanRunInstruction(int ip);
        }

        private class InfiniteLoopValidator : IInstructionValidation
        {
            private readonly List<int> Tracker = new List<int>();

            public bool CanRunInstruction(int ip)
            {
                if (Tracker.Contains(ip))
                    return false;

                Tracker.Add(ip);
                return true;
            }
        }

        private class OutOfBoundsValidator : IInstructionValidation
        {
            private readonly int codeSize;

            public OutOfBoundsValidator(int codeSize)
            {
                this.codeSize = codeSize;
            }

            public bool CanRunInstruction(int ip) => ip < codeSize;
        }

        private record Instruction(OpCode OpCode, int Argument);

        private enum OpCode
        {
            Nop,
            Acc,
            Jmp,
        }
    }
}
