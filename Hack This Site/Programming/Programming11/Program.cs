using System;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Programming11
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var client = GetHttpClient();

            string html = await client.GetHtml();

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

            await client.SendSolution(solution);
        }

        private static HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Cookie", $"PHPSESSID=lsg7ne1gv9681nt94ro1r0vvh0");

            return client;
        }
    }
}