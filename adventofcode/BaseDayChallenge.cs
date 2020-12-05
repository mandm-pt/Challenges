using System;
using System.Diagnostics;
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

            await RunAndPrintTaskAsync(Part1Async, 1);
            await RunAndPrintTaskAsync(Part2Async, 2);
        }

        protected virtual async Task LoadyAsync() => inputLines = await File.ReadAllLinesAsync(InputFilePath);

        protected virtual Task<string> Part1Async() => Task.FromResult("Not solved!");

        protected virtual Task<string> Part2Async() => Task.FromResult("Not solved!");

        private async Task RunAndPrintTaskAsync(Func<Task<string>> taskAsync, int part)
        {
            var sw = new Stopwatch();
            sw.Start();
            string solution = await taskAsync();
            sw.Stop();

            Write($"Year {Year} | ", ConsoleColor.White);
            Write($"Day {Day} | ", ConsoleColor.White);
            Write($"Part {part} | ", ConsoleColor.White);
            Write("Solution: ", ConsoleColor.White); WriteSuccess($"{solution} \t");
            Write("Time: ", ConsoleColor.White); WriteLineInfo($"{sw.ElapsedMilliseconds} ms"); // not really accurate
        }
    }
}
