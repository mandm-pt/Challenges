using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Day04 : BaseDayChallenge
    {
        protected override int Day => 4;

        private readonly List<Passaport> passaports = new List<Passaport>();

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            var passaportData = new List<string>();
            for (int i = 0; i < inputLines.Length; i++)
            {
                string line = inputLines[i];
                if (line.Length == 0 || i == inputLines.Length - 1)
                {
                    if (i == inputLines.Length - 1)
                    {
                        passaportData.Add(line);
                    }

                    var keyValues = string.Join(' ', passaportData)
                        .Split(' ')
                        .Select(kv =>
                        {
                            string[] kvParts = kv.Split(':');
                            return new
                            {
                                key = kvParts[0],
                                value = kvParts[1]
                            };
                        })
                        .ToDictionary(kv => kv.key, kv => kv.value);


                    passaports.Add(Passaport.CreateFromDictionary(keyValues));

                    passaportData.Clear();
                    continue;
                }

                passaportData.Add(line);
            }
        }

        protected override Task Part1Async()
        {
            System.Console.WriteLine(passaports.Count(p => p.IsValid()));
            return Task.CompletedTask;
        }

        protected override Task Part2Async()
        {
            System.Console.WriteLine(passaports.Count(p => p.IsStrictValid()));
            return Task.CompletedTask;
        }

        private record Passaport
        {
            private const string keyBirthYear = "byr";
            private const string keyIssueYear = "iyr";
            private const string keyExpirationYear = "eyr";
            private const string keyHeight = "hgt";
            private const string keyHairColor = "hcl";
            private const string keyEyeColor = "ecl";
            private const string keyPassportID = "pid";
            private const string keyCountryID = "cid";

            public string? RawBirthYear { get; init; }
            public string? RawIssueYear { get; init; }
            public string? RawExpirationYear { get; init; }
            public string? RawHeight { get; init; }
            public string? RawHairColor { get; init; }
            public string? RawEyeColor { get; init; }
            public string? RawPassportID { get; init; }
            public string? RawCountryID { get; init; }

            public static Passaport CreateFromDictionary(Dictionary<string, string> keyValues)
            {
                var passaport = new Passaport
                {
                    RawBirthYear = keyValues.GetValueOrDefault(keyBirthYear),
                    RawIssueYear = keyValues.GetValueOrDefault(keyIssueYear),
                    RawExpirationYear = keyValues.GetValueOrDefault(keyExpirationYear),
                    RawHeight = keyValues.GetValueOrDefault(keyHeight),
                    RawHairColor = keyValues.GetValueOrDefault(keyHairColor),
                    RawEyeColor = keyValues.GetValueOrDefault(keyEyeColor),
                    RawPassportID = keyValues.GetValueOrDefault(keyPassportID),
                    RawCountryID = keyValues.GetValueOrDefault(keyCountryID)
                };
                return passaport;
            }

            public bool IsValid()
            {
                return !string.IsNullOrWhiteSpace(RawBirthYear) &&
                    !string.IsNullOrWhiteSpace(RawIssueYear) &&
                    !string.IsNullOrWhiteSpace(RawExpirationYear) &&
                    !string.IsNullOrWhiteSpace(RawHeight) &&
                    !string.IsNullOrWhiteSpace(RawHairColor) &&
                    !string.IsNullOrWhiteSpace(RawEyeColor) &&
                    !string.IsNullOrWhiteSpace(RawPassportID);
            }

            private readonly Regex hairColorValidation = new Regex("#[0-9|a-f]{6}");
            private readonly Regex heightValidation = new Regex("[0-9]+(cm|in)");
            private readonly string[] validEyeColors = { "amb", "blu", "brn", "gry", "grn", "hzl", "oth" };

            public bool IsStrictValid()
            {
                bool birthYearIsValid = int.TryParse(RawBirthYear, out int birhday)
                    && birhday is >= 1920 and <= 2002;
                bool issueYearIsValid = int.TryParse(RawIssueYear, out int issueYear)
                    && issueYear is >= 2010 and <= 2020;
                bool expirationYearIsValid = int.TryParse(RawExpirationYear, out int expirationYear)
                    && expirationYear is >= 2020 and <= 2030;

                bool heightIsValid = heightValidation.IsMatch(RawHeight ?? "");
                if (heightIsValid)
                {
                    int height = int.Parse(RawHeight[..^2]);
                    heightIsValid = RawHeight.EndsWith("cm")
                        ? height is >= 150 and <= 193
                        : height is >= 59 and <= 76;
                }

                bool hairColorIsValid = hairColorValidation.IsMatch(RawHairColor ?? "");
                bool eyeColorIsValid = validEyeColors.Contains(RawEyeColor);
                bool idIsValid = RawPassportID?.Length == 9 && int.TryParse(RawPassportID, out _);

                return birthYearIsValid &&
                    issueYearIsValid &&
                    expirationYearIsValid &&
                    heightIsValid &&
                    hairColorIsValid &&
                    eyeColorIsValid &&
                    idIsValid;
            }
        }
    }
}
