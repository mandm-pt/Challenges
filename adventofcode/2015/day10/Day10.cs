using System.IO;
using System.Linq;
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

        protected override Task<string> Part1Async() => Task.FromResult(LookAndSay(40, Input).Length.ToString());

        protected override Task<string> Part2Async() => Task.FromResult(LookAndSay(50, Input).Length.ToString());

        private static string LookAndSay(byte rounds, string input)
        {
            string result = input;

            for (int i = 0; i < rounds; i++)
            {
                string currentInput = sbResult.ToString();

                result = string.Join("", RepeatedDigitsRegex.Matches(currentInput)
                    .Select(m => $"{m.Value.Length}{m.Value[0]}")
                    .ToArray());
            }

            return result;
        }
    }
}
