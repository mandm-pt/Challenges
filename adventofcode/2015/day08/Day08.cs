using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2015
{
    internal class Day08 : BaseDayChallenge
    {
        public override int Year => 2015;
        public override int Day => 8;

        private static readonly Regex AsciiEscaped = new Regex("\\\\x[0-9a-f]{2}", RegexOptions.Compiled);
        private readonly List<CodeBlock> Blocks = new List<CodeBlock>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            Blocks.Add(new("\"\""));
            Blocks.Add(new("\"abc\""));
            Blocks.Add(new("\"aaa\\\"aaa\""));
            Blocks.Add(new("\"\\x27\""));
            //foreach (string? line in inputLines)
            //{
            //    Blocks.Add(new(line));
            //}
        }

        protected override Task<string> Part1Async()
        {
            int codeSize = Blocks.Sum(b => b.Code.Length);
            int memorySize = Blocks.Sum(b => b.MemoryValue.Length);

            return Task.FromResult((codeSize - memorySize).ToString());
        }

        protected override Task<string> Part2Async()
        {
            // part 2 not working
            int codeSize = Blocks.Sum(b => b.Code.Length);
            int encodedSize = Blocks.Sum(b => b.EncodedValue.Length);

            return Task.FromResult((encodedSize - codeSize).ToString());
        }

        private record CodeBlock(string Code)
        {
            public string MemoryValue
            {
                get
                {
                    string unscaped = Code[1..^1];

                    foreach (Match match in AsciiEscaped.Matches(unscaped))
                    {
                        string hex = match.Value[2..];
                        uint ascii = Convert.ToUInt32(hex, 16);
                        char hexChar = Convert.ToChar(ascii);

                        unscaped = unscaped.Replace(match.Value, hexChar.ToString());
                    }

                    return unscaped.Replace("\\\"", "\"").Replace(@"\\", @"\");
                }
            }

            public string EncodedValue
            {
                get
                {
                    string encodedValue = Code;

                    foreach (Match match in AsciiEscaped.Matches(Code))
                    {
                        encodedValue = encodedValue.Replace(match.Value, $"\\{match.Value}");
                    }

                    encodedValue = "\"" + encodedValue.Replace("\\\"", "\\\\\"").Replace("\"", "\\\"") + "\"";

                    return encodedValue;
                }
            }
        }
    }
}
