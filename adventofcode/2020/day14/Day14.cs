using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day14 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 14;

        private readonly Regex InstructionRegex = new Regex(@"mask = ([10X]+)|mem\[(\d+)] = (\d+)", RegexOptions.Compiled);
        private List<Instruction> Instructions = new List<Instruction>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            Instructions = inputLines
                .Select<string, Instruction>(line =>
                {
                    var m = InstructionRegex.Match(line);

                    return line.StartsWith("mask")
                        ? new Mask(m.Groups[1].Value)
                        : new Write(ulong.Parse(m.Groups[3].Value), ulong.Parse(m.Groups[2].Value));
                })
                .ToList();
        }

        protected override Task<string> Part1Async()
        {
            var memory = new Dictionary<ulong, ulong>();
            string currentMask = EmptyMask;
            foreach (var instruction in Instructions)
            {
                if (instruction is Mask m)
                {
                    currentMask = m.Value;
                }
                else if (instruction is Write w)
                {
                    string binaryValue = Decoder.ApplyMask(currentMask, w.BinaryValue);
                    ulong decimalValue = Convert.ToUInt64(new string(binaryValue), 2);

                    if (memory.ContainsKey(w.Address))
                        memory[w.Address] = decimalValue;
                    else
                        memory.Add(w.Address, decimalValue);
                }
            }

            ulong result = 0;
            foreach (var pair in memory)
            {
                result += pair.Value;
            }

            return Task.FromResult(result.ToString());
        }

        protected override Task<string> Part2Async()
        {
            var memory = new Dictionary<ulong, ulong>();
            string currentMask = EmptyMask;
            foreach (var instruction in Instructions)
            {
                if (instruction is Mask m)
                {
                    currentMask = m.Value;
                }
                else if (instruction is Write w)
                {
                    string binaryValue = Decoder.ApplyMask2(currentMask, w.BinaryAddress);
                    ulong[] addresses = Decoder.GetAddresses(binaryValue);

                    foreach (ulong address in addresses)
                    {
                        if (memory.ContainsKey(address))
                            memory[address] = w.Value;
                        else
                            memory.Add(address, w.Value);
                    }
                }
            }

            ulong result = 0;
            foreach (var pair in memory)
            {
                result += pair.Value;
            }

            return Task.FromResult(result.ToString());
        }

        private abstract record Instruction();

        private static class Decoder
        {
            private static char[] EmptyValue => new string('0', 36).ToCharArray();

            public static string ApplyMask(string mask, string binaryValue)
            {
                char[] binaryResult = EmptyValue;

                for (int i = mask.Length - 1; i >= 0; i--)
                {
                    binaryResult[i] = mask[i] switch
                    {
                        '0' => '0',
                        '1' => '1',
                        _ => binaryValue[i],
                    };
                }

                return new string(binaryResult);
            }

            public static string ApplyMask2(string mask, string binaryValue)
            {
                char[] binaryResult = EmptyValue;

                for (int i = mask.Length - 1; i >= 0; i--)
                {
                    binaryResult[i] = mask[i] switch
                    {
                        'X' => 'X',
                        '1' => '1',
                        _ => binaryValue[i],
                    };
                }

                return new string(binaryResult);
            }

            public static ulong[] GetAddresses(string binaryValue)
            {
                int count = binaryValue.Count(c => c == 'X');
                int maxValue = (int)Math.Pow(2, count);

                ulong[] addresses = new ulong[maxValue];

                for (int i = 0; i < maxValue; i++)
                {
                    string binaryI = Convert.ToString(i, 2).PadLeft(count, '0');
                    char[] address = binaryValue.ToCharArray();

                    int idx = -1;
                    for (int j = 0; j < binaryI.Length; j++)
                    {
                        idx = binaryValue.IndexOf('X', idx + 1);
                        address[idx] = binaryI[j];
                    }

                    addresses[i] = Convert.ToUInt64(new string(address), 2);
                }

                return addresses;
            }
        }

        private record Mask(string Value) : Instruction;

        private record Write(ulong Value, ulong Address) : Instruction
        {
            public string BinaryValue => Convert.ToString((long)Value, 2).PadLeft(36, '0');

            public string BinaryAddress => Convert.ToString((long)Address, 2).PadLeft(36, '0');
        }

        private string EmptyMask => new string('X', 36);
    }
}
