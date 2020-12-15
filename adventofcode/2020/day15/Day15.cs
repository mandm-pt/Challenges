using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day15 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 15;

        private int[] StartingNumbers = Array.Empty<int>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            StartingNumbers = inputLines[0].Split(',').Select(int.Parse).ToArray();
            //StartingNumbers = new[] { 0, 3, 6 };
        }

        protected override Task<string> Part1Async()
        {
            var memory = new Memory();

            int previousNumber = 0;
            int startingNumbersIdx = 0;

            int getNextNumber()
            {
                return StartingNumbers[startingNumbersIdx++ % StartingNumbers.Length];
            }

            for (int i = 1; i <= 2020; i++)
            {
                int nextNumber;

                if (i > StartingNumbers.Length)
                {
                    nextNumber = memory.ContainsKey(previousNumber)
                        ? memory[previousNumber].Previous - memory[previousNumber].SecondPrevious
                        : 0;
                }
                else
                    nextNumber = getNextNumber();

                memory.Store(nextNumber, i);

                previousNumber = nextNumber;
            }

            return Task.FromResult(previousNumber.ToString());
        }

        protected override Task<string> Part2Async()
        {
            var memory = new Memory();

            int previousNumber = 0;
            int startingNumbersIdx = 0;

            int getNextNumber()
            {
                return StartingNumbers[startingNumbersIdx++ % StartingNumbers.Length];
            }

            for (int i = 1; i <= 30000000; i++)
            {
                int nextNumber;

                if (i > StartingNumbers.Length)
                {
                    nextNumber = memory.ContainsKey(previousNumber)
                        ? memory[previousNumber].Previous - memory[previousNumber].SecondPrevious
                        : 0;
                }
                else
                    nextNumber = getNextNumber();

                memory.Store(nextNumber, i);

                previousNumber = nextNumber;
            }

            return Task.FromResult(previousNumber.ToString());
        }

        private class Memory : Dictionary<int, Pair>
        {
            public void Store(int number, int round)
            {
                if (base.ContainsKey(number))
                    this[number] = new(round, this[number].Previous);
                else
                    Add(number, new(round, 0));
            }

            public new bool ContainsKey(int key)
            {
                if (base.ContainsKey(key))
                {
                    var value = this[key];
                    return value.SecondPrevious != 0;
                }

                return false;
            }
        }

        private record Pair(int Previous, int SecondPrevious);
    }
}
