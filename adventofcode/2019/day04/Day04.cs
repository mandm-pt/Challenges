using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2019
{
    internal class Day04 : BaseDayChallenge
    {
        public override int Year => 2019;
        public override int Day => 4;

        private int minRange = 0;
        private int maxRange = 0;

        protected override async Task LoadyAsync()
        {
            var ranges = (await File.ReadAllTextAsync(InputFilePath))
                .Split("-")
                .Select(int.Parse)
                .ToArray();

            minRange = ranges[0];
            maxRange = ranges[1];
        }

        protected override Task<string> Part1Async()
        {
            int countValidPasswords = 0;
            for (int i = minRange; i <= maxRange; i++)
            {
                string numberString = i.ToString();
                if (HasTwoAdjacentDigits(numberString) && IncreasingNumbersLTR(numberString))
                {
                    countValidPasswords++;
                }
            }

            return Task.FromResult(countValidPasswords.ToString());
        }

        protected override Task<string> Part2Async()
        {
            int countValidPasswords = 0;
            for (int i = minRange; i <= maxRange; i++)
            {
                string numberString = i.ToString();
                if (HasTwoAdjacentDigits(numberString) && RepeatedNumbers2DigitsLong(numberString) && IncreasingNumbersLTR(numberString))
                {
                    countValidPasswords++;
                }
            }

            return Task.FromResult(countValidPasswords.ToString());
        }

        private bool HasTwoAdjacentDigits(string number)
        {
            return number[0] == number[1] ||
                number[1] == number[2] ||
                number[2] == number[3] ||
                number[3] == number[4] ||
                number[4] == number[5];
        }

        private bool IncreasingNumbersLTR(string number)
        {
            return number[0] <= number[1] &&
                number[1] <= number[2] &&
                number[2] <= number[3] &&
                number[3] <= number[4] &&
                number[4] <= number[5];
        }

        private bool RepeatedNumbers2DigitsLong(string number)
        {
            return number
                .ToArray()
                .GroupBy(c => c)
                .Any(g => g.Count() == 2);
        }
    }
}
