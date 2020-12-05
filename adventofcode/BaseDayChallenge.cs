using System;
using System.IO;
using System.Threading.Tasks;
using static AoC.Solutions.ConsoleUtils;

namespace AoC.Solutions
{
    internal abstract class BaseDayChallenge
    {
        protected string[] inputLines = Array.Empty<string>();
        protected string InputFilePath => Path.GetFullPath($"{Year}/day{Day:D2}/input.txt");

        public abstract int Year { get; }
        public abstract int Day { get; }
        public string Name => $"Day {Day}";

        public async Task SolveAsync()
        {
            if (!File.Exists(InputFilePath))
            {
                WriteLineError($"Input file not found at: {InputFilePath}");
                return;
            }
            await LoadyAsync();

            WriteSuccess($"{Year}-{Day} Part 1 solution: ");
            await Part1Async();

            WriteSuccess($"{Year}-{Day} Part 2 solution: ");
            await Part2Async();
        }

        protected virtual async Task LoadyAsync() => inputLines = await File.ReadAllLinesAsync(InputFilePath);

        protected virtual Task Part1Async()
        {
            WriteLineWarning("Not solved!");
            return Task.CompletedTask;
        }

        protected virtual Task Part2Async()
        {
            WriteLineWarning("Not solved!");
            return Task.CompletedTask;
        }
    }
}
