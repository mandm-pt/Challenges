using System.IO;
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
        private string Input = "";

        protected override async Task LoadyAsync() => Input = await File.ReadAllTextAsync(InputFilePath);

        protected override Task<string> Part1Async()
        {
            int resultLength = LookAndSay(40, Input).Length;
            return Task.FromResult(resultLength.ToString());
        }

        protected override Task<string> Part2Async()
        {
            int resultLength = LookAndSay(50, Input).Length;
            return Task.FromResult(resultLength.ToString());
        }

        private static string LookAndSay(byte rounds, string input)
        {
            var sbResult = new StringBuilder(input);

            for (int i = 0; i < rounds; i++)
            {
                string currentInput = sbResult.ToString();
                sbResult.Clear();

                foreach (Match match in RepeatedDigitsRegex.Matches(currentInput))
                {
                    sbResult.Append($"{match.Value.Length}{match.Value[0]}");
                }
            }

            return sbResult.ToString();
        }
    }
}
