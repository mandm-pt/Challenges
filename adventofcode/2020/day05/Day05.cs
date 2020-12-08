using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day05 : BaseDayChallenge
    {
        public override int Year => 2020;
        public override int Day => 5;

        private List<Seat> seats = new List<Seat>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            seats = ProcessSeats(inputLines);
        }

        protected override Task<string> Part1Async() => Task.FromResult(seats.Max(s => s.Id).ToString());

        protected override Task<string> Part2Async()
        {
            var sortedSeats = seats.OrderBy(s => s.Id).ToArray();
            int mySeatId = 0;

            int count = sortedSeats[0].Id;
            for (int i = 1; i < sortedSeats.Length; i++)
            {
                if (count + 1 != sortedSeats[i].Id)
                {
                    mySeatId = count + 1;
                    break;
                }
                count++;
            }

            return Task.FromResult(mySeatId.ToString());
        }

        private static List<Seat> ProcessSeats(string[] seatsToProcess)
        {
            var seats = new List<Seat>();
            foreach (string? processingSeat in seatsToProcess)
            {
                int row = Calculate(processingSeat[..^3].ToCharArray(), 'B');
                int collumn = Calculate(processingSeat[^3..].ToCharArray(), 'R');

                seats.Add(new(row, collumn));
            }

            return seats;
        }

        private static int Calculate(char[] chars, char valueOne)
        {
            chars = chars.Reverse().ToArray();

            int sum = 0;
            for (int i = chars.Length - 1; i >= 0; i--)
            {
                if (chars[i] == valueOne)
                    sum += (int)Math.Pow(2, i);
            }

            return sum;
        }

        private record Seat(int Row, int Column)
        {
            public int Id => Row * 8 + Column;
        }
    }
}
