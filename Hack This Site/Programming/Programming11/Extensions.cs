using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Programming11
{
    internal static class Extensions
    {
        private const string challengeUri = "https://www.hackthissite.org/missions/prog/11/";
        private const string submitSolutionUri = "https://www.hackthissite.org/missions/prog/11/index.php";

        internal static Task<string> GetHtml(this HttpClient client)
            => client.GetStringAsync(challengeUri);

        internal static async Task SendSolution(this HttpClient client, string solution)
        {
            client.DefaultRequestHeaders.Referrer = new Uri(challengeUri);
            var response = await client.PostAsync(submitSolutionUri,
                new StringContent($"solution={solution}&submitbutton=submit++++++++++++%28remaining+time%3A+1+seconds%29",
                UTF8Encoding.Default,
                "application/x-www-form-urlencoded"));

            string result = await response.Content.ReadAsStringAsync();
        }
    }
}