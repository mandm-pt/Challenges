﻿using System;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day01 : BaseDayChallenge
    {
        public override int Year => 2020;
        public override int Day => 1;

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
