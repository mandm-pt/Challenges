using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal abstract class DayChallenge
    {
        protected readonly string inputFilePath;
        protected string[] inputLines;

        public string Name => this.GetType().Name;

        public DayChallenge(string inputFilePath)
        {
            this.inputFilePath = inputFilePath;
        }

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
            inputLines = await File.ReadAllLinesAsync(inputFilePath);
        }

        protected abstract Task Part1Async();
        protected abstract Task Part2Async();
    }

    class Program
    {
        static string inputFilesPath = $"{Directory.GetCurrentDirectory()}/../inputs/";
        static DayChallenge[] Challenges = new DayChallenge[]
        { 
            new Day1($"{inputFilesPath}1.txt"),
            new Day2($"{inputFilesPath}2.txt")
        };

        static async Task Main()
        {
            int day = 0;
            do
            {
                Console.Clear();
                if (day >= 1 && day <= 25 && day <= Challenges.Length)
                {
                    await Challenges[day - 1].SolveAsync();
                }

                Console.WriteLine($"{new string('=', 10)} MENU {new string('=', 10)}");
                for (int i = 0; i < Challenges.Length; i++)
                {
                    Console.WriteLine($"{i + 1} - {Challenges[i].Name}");
                }
                Console.WriteLine($"{new string('-', 20)}{Environment.NewLine}99 - Exit");
                int.TryParse(Console.ReadLine(), out day);
            }
            while (day != 99);
        }
    }
}
