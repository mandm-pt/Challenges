using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using static AoC.Solutions.ConsoleUtils;

namespace AoC.Solutions
{
    internal class Program
    {
        public static IEnumerable<BaseDayChallenge> GetSolutions()
        {
            var challenges = new List<BaseDayChallenge>();

            var challengeType = typeof(BaseDayChallenge);

            foreach (var dayChallengeType in
                Assembly.GetExecutingAssembly().GetTypes()
                .Where(t => t.IsClass &&
                        !t.IsAbstract &&
                        t.IsSubclassOf(challengeType)))
            {
                challenges.Add((BaseDayChallenge)Activator.CreateInstance(dayChallengeType)!);
            }

            return challenges;
        }

        private static async Task Main()
        {
            Console.ForegroundColor = ConsoleColor.Gray;

            var solutionPerYear = GetSolutions().GroupBy(s => s.Year).OrderBy(y => y.Key).ToList();
            while (true)
            {
                WriteLineInfo("CTRL + C to exit");
                WriteLineInfo("============= Choose a year ============= ");
                solutionPerYear.ForEach(y => WriteMenuOption(y.Key, $"Solutions for AoC {y.Key}"));

                if (!int.TryParse(Console.ReadLine(), out int choosenYear))
                {
                    WriteLineWarning($"Invalid option. Choose a number from the menu options");
                    continue;
                }

                var choosenYearSolutions = solutionPerYear
                    .FirstOrDefault(s => s.Key == choosenYear)
                    ?.OrderBy(s => s.Day)
                    .ToList();
                if (choosenYearSolutions == null)
                {
                    WriteLineWarning($"No solutions found for year {choosenYear}");
                    continue;
                }

                WriteLineInfo("============= Choose the day ============= ");
                choosenYearSolutions.ForEach(s => WriteMenuOption(s.Day, s.Name));

                if (!int.TryParse(Console.ReadLine(), out int choosenDay))
                {
                    WriteLineWarning($"Invalid option. Choose a number from the menu options");
                    continue;
                }

                var choosenSolution = choosenYearSolutions.FirstOrDefault(s => s.Day == choosenDay);
                if (choosenSolution == null)
                {
                    WriteLineWarning($"Solution for day {choosenDay} not found.");
                    continue;
                }

                Console.WriteLine();
                await choosenSolution.SolveAsync();
                Console.WriteLine();
            }
        }
    }
}
