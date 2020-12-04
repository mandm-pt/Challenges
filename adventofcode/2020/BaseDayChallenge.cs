using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal abstract class BaseDayChallenge
    {
        protected abstract int Day { get; }
        protected string[] inputLines = Array.Empty<string>();

        protected string InputFilePath => Path.GetFullPath($"day{Day:D2}/input.txt");
        public string Name => $"Day {Day}";

        public async Task SolveAsync()
        {
            await LoadyAsync();

            Console.WriteLine($"{new string('=', 10)} Part 1 solution {new string('=', 10)}");
            await Part1Async();

            Console.WriteLine($"{new string('=', 10)} Part 2 solution {new string('=', 10)}");
            await Part2Async();
        }

        protected virtual async Task LoadyAsync()
        {
            if (!File.Exists(InputFilePath))
            {
                Console.WriteLine($"Input file not found at: {InputFilePath}");
                return;
            }

            inputLines = await File.ReadAllLinesAsync(InputFilePath);
        }

        protected virtual Task Part1Async()
        {
            System.Console.WriteLine("Not solved!");
            return Task.CompletedTask;
        }

        protected virtual Task Part2Async()
        {
            System.Console.WriteLine("Not solved!");
            return Task.CompletedTask;
        }
    }
}
