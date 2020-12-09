using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2015
{
    internal class Day12 : BaseDayChallenge
    {
        public override int Year => 2015;
        public override int Day => 12;

        private static readonly Regex DigitsRegex = new Regex(@"-?\d+", RegexOptions.Compiled);
        private List<int> AllDigits = new List<int>();
        private string RawJson = "";

        protected override async Task LoadyAsync()
        {
            RawJson = await File.ReadAllTextAsync(InputFilePath);
            AllDigits = DigitsRegex.Matches(RawJson)
                            .Select(m => m.Value)
                            .Select(int.Parse)
                            .ToList();
        }

        protected override Task<string> Part1Async() => Task.FromResult(AllDigits.Sum().ToString());

        protected override Task<string> Part2Async()
        {
            var jsonDoc = JsonDocument.Parse(RawJson);
            int sum = GetNumbers(jsonDoc.RootElement).Sum();

            return Task.FromResult(sum.ToString());
        }

        private IEnumerable<int> GetNumbers(JsonElement rootElement)
        {
            var validNumbers = new List<int>();

            if (rootElement.ValueKind == JsonValueKind.Array)
            {
                foreach (var element in rootElement.EnumerateArray())
                {
                    (var numbers, bool valid) = ProcessElement(element, true);

                    if (!valid)
                        return Array.Empty<int>();

                    validNumbers.AddRange(numbers);
                }
            }
            else if (rootElement.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in rootElement.EnumerateObject())
                {
                    (var numbers, bool valid) = ProcessElement(property.Value);

                    if (!valid)
                        return Array.Empty<int>();

                    validNumbers.AddRange(numbers);
                }
            }
            return validNumbers;
        }

        private (IEnumerable<int>, bool) ProcessElement(JsonElement element, bool inArray = false)
        {
            var currentObject = new List<int>();

            switch (element.ValueKind)
            {
                case JsonValueKind.String when element.GetString() == "red":
                    return (Array.Empty<int>(), inArray);
                case JsonValueKind.Number:
                    currentObject.Add(element.GetInt32());
                    break;
                case JsonValueKind.Object or JsonValueKind.Array:
                    currentObject.AddRange(GetNumbers(element));
                    break;
            }

            return (currentObject, true);
        }
    }
}
