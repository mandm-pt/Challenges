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
            => (name: "PHPSESSID", value: "b1433odb2eejfe3g9hrsnsk360");
    }
}
