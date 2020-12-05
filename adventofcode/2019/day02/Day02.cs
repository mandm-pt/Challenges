using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2019
{
    internal class Day02 : BaseDayChallenge
    {
        public override int Year => 2019;
        public override int Day => 2;

        private int[] instructions = Array.Empty<int>();

        protected override async Task LoadyAsync()
        {
            instructions = (await File.ReadAllTextAsync(InputFilePath))
                .Split(",")
                .Select(int.Parse)
                .ToArray();
        }

        protected override Task Part1Async()
        {
            int result = EcecProgram((int[])instructions.Clone(), 12, 2);

            Console.WriteLine(result);
            return Task.CompletedTask;
        }

        protected override Task Part2Async()
        {
            int result = 0;
            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    int programResult = EcecProgram((int[])instructions.Clone(), i, j);

                    if (programResult == 19690720)
                        result = 100 * i + j;
                }
            }

            Console.WriteLine(result);
            return Task.CompletedTask;
        }

        private int EcecProgram(int[] instructions, int initialAddress1, int initialAddress2)
        {
            try
            {
                int addressValue1 = 0, addressValue2 = 0, addressToStore = 0;

                instructions[1] = initialAddress1;
                instructions[2] = initialAddress2;

                for (int ip = 0; ip < instructions.Length; ip += 4)
                {
                    int opCode = instructions[ip];

                    if (opCode == 1)
                    {
                        addressValue1 = instructions[ip + 1];
                        addressValue2 = instructions[ip + 2];
                        addressToStore = instructions[ip + 3];

                        instructions[addressToStore] = instructions[addressValue1] + instructions[addressValue2];
                    }
                    else if (opCode == 2)
                    {
                        addressValue1 = instructions[ip + 1];
                        addressValue2 = instructions[ip + 2];
                        addressToStore = instructions[ip + 3];

                        instructions[addressToStore] = instructions[addressValue1] * instructions[addressValue2];
                    }
                    else if (opCode == 99)
                    {
                        break;
                    }

                }
            }
            catch
            {
                return -1;
            }

            return instructions[0];
        }
    }
}
