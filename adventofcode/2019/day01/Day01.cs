using System;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2019
{
    internal class Day01 : BaseDayChallenge
    {
        public override int Year => 2019;
        public override int Day => 1;

        protected override Task<string> Part1Async()
        {
            int solution = inputLines
                            .Select(int.Parse)
                            .Select(m => m / 3 - 2)
                            .Sum();

            return Task.FromResult(solution.ToString());
        }

        protected override Task<string> Part2Async()
        {
            int solution = inputLines
                .Select(int.Parse)
                .Select(m =>
                {
                    int initialFuel = m / 3 - 2;

                    int extraFuel = 0;
                    int fuelForFuel = initialFuel;
                    while (fuelForFuel > 0)
                    {
                        fuelForFuel = fuelForFuel / 3 - 2;
                        extraFuel = extraFuel + Math.Max(fuelForFuel, 0);
                    }

                    return initialFuel + extraFuel;
                })
                .Sum();

            return Task.FromResult(solution.ToString());
        }
    }
}
