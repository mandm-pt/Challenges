using System.Net.Http;

namespace Utils
{
    public static class Http
    {
        public static HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("Cookie", $"PHPSESSID=COOKIE_VALUE");

            return client;
        }
    }
}