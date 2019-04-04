using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public static class Extensions
    {
        public static Task<string> GetHtml(this HttpClient client, string uri)
            => client.GetStringAsync(uri);

        public static async Task SendSolution(this HttpClient client, string submitSolutionUri, string payload, string referrer)
        {
            client.DefaultRequestHeaders.Referrer = new Uri(referrer);
            var response = await client.PostAsync(submitSolutionUri,
                new StringContent(payload, Encoding.Default, "application/x-www-form-urlencoded"));

            string result = await response.Content.ReadAsStringAsync();
        }
    }
}