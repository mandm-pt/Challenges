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
        }

        protected override Task<string> Part1Async() => Task.FromResult(GetNumberFromTurn(2020).ToString());

        protected override Task<string> Part2Async() => Task.FromResult(GetNumberFromTurn(30000000).ToString());

        private int GetNumberFromTurn(int turn)
        {
            var memory = new Memory();

            int previousNumber = 0;
            int startingNumbersIdx = 0;

            int getNextNumber() => StartingNumbers[startingNumbersIdx++ % StartingNumbers.Length];

            for (int i = 0; i < StartingNumbers.Length; i++)
            {
                memory.Store(getNextNumber(), i + 1);
            }

            for (int i = StartingNumbers.Length + 1; i <= turn; i++)
            {
                int nextNumber = memory.ContainsKey(previousNumber)
                                    ? memory[previousNumber].Previous - memory[previousNumber].SecondPrevious
                                    : 0;

                memory.Store(nextNumber, i);

                previousNumber = nextNumber;
            }

            return previousNumber;
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

        private struct Pair
        {
            public readonly int Previous;
            public readonly int SecondPrevious;

            public Pair(int previous, int secondPrevious)
            {
                Previous = previous;
                SecondPrevious = secondPrevious;
            }
        }
    }
}
