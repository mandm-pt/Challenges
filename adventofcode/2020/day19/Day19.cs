using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day19 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 19;

        private readonly Regex CaptureRules = new Regex(@"(\d+): ""?([^""\n]+)""?", RegexOptions.Compiled);
        private readonly Regex CaptureText = new Regex(@"^[a-z]+", RegexOptions.Compiled | RegexOptions.Multiline);
        private readonly Regex CaptureRuleId = new Regex(@"\d+(?!,})", RegexOptions.Compiled);
        private Rule[] Rules = Array.Empty<Rule>();
        private string[] Text = Array.Empty<string>();
        protected override async Task LoadyAsync()
        {
            string contents = await File.ReadAllTextAsync(InputFilePath);

            Rules = CaptureRules
                    .Matches(contents)
                    .Select(m => new Rule(int.Parse(m.Groups[1].Value), m.Groups[2].Value.Trim()))
                    .OrderBy(o => o.Id)
                    .ToArray();

            Text = CaptureText
                    .Matches(contents)
                    .Select(m => m.Value)
                    .ToArray();
        }

        protected override Task<string> Part1Async()
        {
            ResolveRules(Rules);

            var zeroRule = Rules.First(r => r.Id == 0);
            var rexgexString = $"^{zeroRule.FinalRule.Replace(" ", "")}$";

            var zeroRuleRegex = new Regex(rexgexString, RegexOptions.Compiled);

            int matchCount = Text.Count(line => zeroRuleRegex.IsMatch(line));

            return Task.FromResult(matchCount.ToString());
        }

        protected override async Task<string> Part2Async()
        {
            // DOES NOT WORK

            await LoadyAsync(); // reload

            Rules.First(r => r.Id == 8).Or = new[] { "42", "42 8" };
            Rules.First(r => r.Id == 11).Or = new[] { "42 31", "42 11 31" };

            ResolveRules(Rules);

            var zeroRule = Rules.First(r => r.Id == 0);
            var rexgexString = $"^{zeroRule.FinalRule.Replace(" ", "")}$";

            var zeroRuleRegex = new Regex(rexgexString, RegexOptions.Compiled);

            int matchCount = Text.Count(line => zeroRuleRegex.IsMatch(line));

            return matchCount.ToString();
        }

        private void ResolveRules(Rule[] rules)
        {
            do
            {
                for (int i = 0; i < rules.Length; i++)
                {
                    var ruleToResolve = rules[i];
                    if (ruleToResolve.IsFinal)
                        continue;

                    for (int j = 0; j < ruleToResolve.Or.Length; j++)
                    {
                        var ids = CaptureRuleId
                                .Matches(ruleToResolve.Or[j])
                                .ToArray();

                        foreach (Match idMatch in ids)
                        {
                            // recursive rule
                            if (idMatch.Value == ruleToResolve.Id.ToString())
                            {
                                string itSelf = string.Join(null, ruleToResolve.Or.Where(o => o != ruleToResolve.Or[j]).ToArray());

                                ruleToResolve.Or[j] = CaptureRuleId.Replace(ruleToResolve.Or[j]
                                                        , $"({itSelf}){{1,}}"
                                                        , 1
                                                        , idMatch.Index == 0 ? 0 : idMatch.Index - 1);
                                continue;
                            }

                            var replaceRule = rules.FirstOrDefault(r => r.IsFinal && r.Id == int.Parse(idMatch.Value));
                            if (replaceRule == null)
                                continue;

                            ruleToResolve.Or[j] = CaptureRuleId.Replace(ruleToResolve.Or[j], replaceRule.FinalRule, 1, idMatch.Index == 0 ? 0 : idMatch.Index - 1);
                        }
                    }
                }
            } while (!rules.All(r => r.IsFinal));
        }

        private class Rule
        {
            public Rule(int id, string text)
            {
                Id = id;
                Or = text.Split("|");
            }

            public bool IsFinal => !FinalRule
                        .Replace("{1,}", "") // remove recursive rules
                        .ToCharArray()
                        .Any(char.IsDigit);

            public int Id { get; }
            public string[] Or { get; set; }

            public string FinalRule => Or.Length > 1 ? $"({string.Join("|", Or)})" : Or[0];
        }
    }
}
