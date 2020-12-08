using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Solutions._2015
{
    internal class Day04 : BaseDayChallenge
    {
        public override int Year => 2015;
        public override int Day => 4;

        private string key = "";
        protected override async Task LoadyAsync() => key = await File.ReadAllTextAsync(InputFilePath);

        protected override Task<string> Part1Async()
        {
            int result = Mine(h => h.StartsWith("00000"));

            return Task.FromResult(result.ToString());
        }

        protected override Task<string> Part2Async()
        {
            int result = Mine(h => h.StartsWith("000000"));

            return Task.FromResult(result.ToString());
        }

        private int Mine(Func<string, bool> stopCondition)
        {
            var md5 = MD5.Create();
            int n = 0;

            string hexResult;
            do
            {
                byte[] byteResult = md5.ComputeHash(Encoding.UTF8.GetBytes($"{key + ++n}"));
                hexResult = BitConverter.ToString(byteResult).Replace("-", "");
            } while (!stopCondition(hexResult));

            return n;
        }
    }
}
