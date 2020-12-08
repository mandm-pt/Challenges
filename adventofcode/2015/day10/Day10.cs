using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2015
{
    internal class Day10 : BaseDayChallenge
    {
        public override int Year => 2015;
        public override int Day => 10;

        private static readonly Regex RepeatedDigitsRegex = new Regex(@"(\d)\1+|\d", RegexOptions.Compiled);
        private readonly string key = "1321131112";

        protected override Task LoadyAsync() => Task.CompletedTask;

        protected override Task<string> Part1Async()
        {
            int resultLength = LookAndSay(40, key).Length;
            return Task.FromResult(resultLength.ToString());
        }

        protected override Task<string> Part2Async()
        {
            int resultLength = LookAndSay(50, key).Length;
            return Task.FromResult(resultLength.ToString());
        }

        private static string LookAndSay(byte rounds, string startingText)
        {
            var sbResult = new StringBuilder(startingText);

            for (int i = 0; i < rounds; i++)
            {
                string currentRun = sbResult.ToString();
                sbResult.Clear();

                foreach (Match match in RepeatedDigitsRegex.Matches(currentRun))
                {
                    sbResult.Append($"{match.Value.Length}{match.Value[0]}");
                }
            }

            return sbResult.ToString();
        }
    }
}
