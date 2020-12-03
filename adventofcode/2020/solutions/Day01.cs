using System;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Day01 : DayChallenge
    {
        protected override int day => 1;

        protected override Task Part1Async()
        {
            string solution = "";

            for (int i = 0; i < inputLines.Length; i++)
            {
                for (int j = 0; j < inputLines.Length; j++)
                {
                    int a = int.Parse(inputLines[i]);
                    int b = int.Parse(inputLines[j]);

                    if (a + b == 2020)
                    {
                        solution = (a * b).ToString();

                        Console.WriteLine(solution);
                        return Task.CompletedTask;
                    }
                }
            }

            return Task.CompletedTask;
        }

        protected override Task Part2Async()
        {
            string solution = "";

            for (int i = 0; i < inputLines.Length; i++)
            {
                for (int j = 0; j < inputLines.Length; j++)
                {
                    for (int k = 0; k < inputLines.Length; k++)
                    {
                        int a = int.Parse(inputLines[i]);
                        int b = int.Parse(inputLines[j]);
                        int c = int.Parse(inputLines[k]);

                        if (a + b + c == 2020)
                        {
                            solution = (a * b * c).ToString();

                            Console.WriteLine(solution);
                            return Task.CompletedTask;
                        }
                    }
                }
            }

            return Task.CompletedTask;
        }
    }
}
