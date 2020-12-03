using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal abstract class DayChallenge
    {
        protected abstract int day { get; }
        protected string[] inputLines;

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
            string path = Path.GetFullPath($"{Program.inputFilesPath}{day}.txt");

            if (!File.Exists(path))
            {
                Console.WriteLine($"Input file not found at: {path}");
                return;
            }

            inputLines = await File.ReadAllLinesAsync(path);
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
