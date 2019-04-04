using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Utils;

namespace Programming12
{
    internal class Program
    {
        private const string ChallengeUri = "https://www.hackthissite.org/missions/prog/12/";
        private const string SubmitSolutionUri = "https://www.hackthissite.org/missions/prog/12/index.php";

        private static async Task Main(string[] args)
        {
            var client = Http.GetHttpClient();

            string html = await client.GetHtml(ChallengeUri);

            string givenString = new Regex("<input type=\"text\" value=\"[^\"]+\"")
                .Match(html).Value
                .Replace("<input type=\"text\" value=\"", string.Empty)
                .TrimEnd('"');

            var numbers = givenString
                .Where(char.IsDigit)
                .Select(c => c - '0')
                .Where(n => n > 1)
                .Select(n => n)
                .ToList();

            int primeNumbersSum = numbers
                .Where(IsPrime)
                .Select(n => n)
                .Sum();

            int compositeNumbersSum = numbers
                .Where(n => !IsPrime(n))
                .Select(n => n)
                .Sum();

            int product = primeNumbersSum * compositeNumbersSum;

            string first25Chars = string.Join("", givenString
                .Where(c => !char.IsDigit(c))
                .Take(25)
                .Select(c => Convert.ToChar(c + 1)));

            string payload = $"solution={first25Chars + product.ToString()}&submitbutton=Submit+%28remaining+time%3A+4+seconds%29";

            await client.SendSolution(SubmitSolutionUri, payload, ChallengeUri);
        }

        private static bool IsPrime(int n)
        {
            if (n <= 1) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;

            int boundary = (int)Math.Floor(Math.Sqrt(n));

            for (int i = 3; i <= boundary; i += 2)
                if (n % i == 0)
                    return false;

            return true;
        }
    }
}