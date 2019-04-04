using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace Programming11
{
    internal class Program
    {
        private const string ChallengeUri = "https://www.hackthissite.org/missions/prog/11/";
        private const string SubmitSolutionUri = "https://www.hackthissite.org/missions/prog/11/index.php";

        private static async Task Main(string[] args)
        {
            var client = Http.GetHttpClient();

            string html = await client.GetHtml(ChallengeUri);

            string randomString = new Regex(@"Generated String: (S+|\d|[^<])+")
                .Match(html).Value
                .Replace("Generated String: ", string.Empty);
            int shift = int.Parse(new Regex(@"Shift: [-]?(\d)+")
                .Match(html).Value
                .Replace("Shift: ", string.Empty));

            char separator = randomString[randomString.Length - 1];

            string solution = string.Join("", randomString
                .TrimEnd(separator)
                .Split(separator)
                .Select(i => Convert.ToChar(int.Parse(i) - shift)));

            string payload = $"solution={solution}";

            await client.SendSolution(SubmitSolutionUri, payload, ChallengeUri);
        }
    }
}