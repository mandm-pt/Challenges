using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2015
{
    internal class Day05 : BaseDayChallenge
    {
        public override int Year => 2015;
        public override int Day => 5;

        private readonly Regex repeatedLettersRegex = new Regex(@"(.)\1", RegexOptions.Compiled);
        private readonly char[] vowels = new[] { 'a', 'e', 'i', 'o', 'u' };
        private readonly string[] forbiden = new[] { "ab", "cd", "pq", "xy" };

        protected override Task<string> Part1Async()
        {
            int count = inputLines.Where(IsNice).Count();

            return Task.FromResult(count.ToString());
        }

        private readonly Regex repeatedLetters2Regex = new Regex(@"(.).\1", RegexOptions.Compiled);
        private readonly Regex pairRegex = new Regex(@"(..)[^\1]*\1", RegexOptions.Compiled);

        protected override Task<string> Part2Async()
        {
            int count = inputLines.Where(IsNice2).Count();

            return Task.FromResult(count.ToString());
        }

        private bool IsNice(string text)
        {
            return !forbiden.Any(f => text.Contains(f)) &&
                    text.ToCharArray().Where(vowels.Contains).Count() >= 3 &&
                    repeatedLettersRegex.IsMatch(text);
        }

        private bool IsNice2(string text)
        {
            return pairRegex.IsMatch(text) &&
                    repeatedLetters2Regex.IsMatch(text);
        }
    }
}
