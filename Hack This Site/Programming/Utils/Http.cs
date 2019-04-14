using System.Net.Http;

namespace Utils
{
    public static class Http
    {
        public static HttpClient GetHttpClient()
        {
            var client = new HttpClient();
            var authCookie = GetAuthCookie();
            client.DefaultRequestHeaders.Add("Cookie", $"{authCookie.name}={authCookie.value}");

            return client;
        }

        public static (string name, string value) GetAuthCookie()
            => (name: "PHPSESSID", value: "lsh34hd1f1iaai87rrinhrcie7");
    }
}