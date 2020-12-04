using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal abstract class DayChallenge
    {
        protected abstract int day { get; }
        protected string[] inputLines;

        protected string InputFilePath => Path.GetFullPath($"{Program.inputFilesPath}day_{day:D2}.txt");
        public string Name => $"Day {day}";

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
