using System;
using System.IO;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Program
    {
        // When running with dotnet run
        internal static string inputFilesPath = $"{Directory.GetCurrentDirectory()}/../inputs/";
        // When running with VS
        //internal static string inputFilesPath = $"{Directory.GetCurrentDirectory()}/../../../../inputs/";
        private static readonly Func<DayChallenge>[] Challenges = new Func<DayChallenge>[]
        {
            () => new Day01(),() => new Day02(),() => new Day03(),() => new Day04(),() => new Day05(),
        };

        private static async Task Main()
        {
            int day = 0;
            do
            {
                Console.Clear();
                if (day >= 1 && day <= 25 && day <= Challenges.Length)
                {
                    Console.WriteLine($"Solutions for day:{day}");
                    await Challenges[day - 1]().SolveAsync();
                }

                Console.WriteLine($"{new string('=', 10)} MENU {new string('=', 10)}");
                for (int i = 0; i < Challenges.Length; i++)
                {
                    Console.WriteLine($"{i + 1} - {Challenges[i]().Name}");
                }
                Console.WriteLine($"{new string('-', 20)}{Environment.NewLine}99 - Exit");
                int.TryParse(Console.ReadLine(), out day);
            }
            while (day != 99);
        }
    }
}
