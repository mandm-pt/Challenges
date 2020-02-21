using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace Programming11
{
    internal class Program
    {
        private const byte min = 32;
        private const byte max = 126;

        private const string challengeUri = "https://www.hackthissite.org/missions/prog/11/";
        private const string submitSolutionUri = "https://www.hackthissite.org/missions/prog/11/index.php";

        private static async Task Main(string[] args)
        {
            var client = Http.GetHttpClient();

            string html = await client.GetHtml(challengeUri);

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
                .Select(int.Parse)
                .Select(b => b + -1 * shift)
                .Select(CheckAndFixNumberBoundaries)
                .Select(Convert.ToChar));

            solution = System.Web.HttpUtility.UrlEncode(solution);

            string payload = $"solution={solution}";

            await client.SendSolution(submitSolutionUri, payload, challengeUri);
        }

        private static int CheckAndFixNumberBoundaries(int i)
        {
            if (i < min)
            {
                int diff = min - i;
                return max - diff;
            }
            else if (i > max)
            {
                int diff = i - max;
                return min + diff;
            }

            return i;
        }
    }
}