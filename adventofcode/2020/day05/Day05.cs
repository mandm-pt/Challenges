using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Day05 : BaseDayChallenge
    {
        protected override int Day => 5;
        private List<Seat> seats = new List<Seat>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            seats = ProcessSeats(inputLines);
        }

        protected override Task Part1Async()
        {
            Console.WriteLine(seats.Max(s => s.Id));
            return Task.CompletedTask;
        }

        protected override Task Part2Async()
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

            Console.WriteLine(mySeatId);
            return Task.CompletedTask;
        }

        private List<Seat> ProcessSeats(string[] seatsToProcess)
        {
            var seats = new List<Seat>();
            foreach (string? processingSeat in seatsToProcess)
            {
                int upper = 127;
                int lower = 0;
                int left = 0;
                int right = 7;

                for (int i = 0; i < 7; i++)
                {
                    if (processingSeat[i] == 'F') upper = (int)Math.Floor(upper - (upper - lower) / 2m);
                    else lower = (int)Math.Ceiling(lower + (upper - lower) / 2m);
                }
                for (int i = 7; i < 7 + 3; i++)
                {
                    if (processingSeat[i] == 'L') right = (int)Math.Floor(right - (right - left) / 2m);
                    else left = (int)Math.Ceiling(left + (right - left) / 2m);
                }
                seats.Add(new(upper, right));
            }

            return seats;
        }
    }

    internal record Seat(int row, int column)
    {
        public int Id => row * 8 + column;
    }
}
