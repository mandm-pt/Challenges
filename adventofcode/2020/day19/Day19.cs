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

        Regex CaptureRules = new Regex(@"(\d+): ""?([^""\n]+)""?", RegexOptions.Compiled);
        Regex CaptureText = new Regex(@"^[a-z]+$", RegexOptions.Compiled | RegexOptions.Multiline);
        Regex CaptureRuleId = new Regex(@"\d+", RegexOptions.Compiled);
        Rule[] Rules = Array.Empty<Rule>();
        string[] Text = Array.Empty<string>();
        protected override async Task LoadyAsync()
        {
            string contents = await File.ReadAllTextAsync(InputFilePath);

            Rules = CaptureRules
                    .Matches(contents)
                    .Select(m => new Rule(m.Groups[1].Value, m.Groups[2].Value))
                    .ToArray();

            Text = CaptureText
                    .Matches(contents)
                    .Select(m => m.Value)
                    .ToArray();
        }

        protected override Task<string> Part1Async()
        {
            ResolveRules(Rules);

            var zeroRule = Rules.First(r => r.Id == "0");
            zeroRule.Text = $"^{zeroRule.Text.Replace(" ", "")}$";

            var zeroRuleRegex = new Regex(zeroRule.Text, RegexOptions.Compiled);

            int matchCount = 0;
            foreach (var line in Text)
            {
                matchCount += zeroRuleRegex.IsMatch(line)
                    ? 1
                    : 0;
            }


            return Task.FromResult(matchCount.ToString());
        }

        private void ResolveRules(Rule[] rules)
        {
            do
            {
                for (int i = 0; i < rules.Length; i++)
                {
                    Rule? ruleToResolve = rules[i];
                    if (ruleToResolve.IsFinal)
                        continue;

                    var ids = CaptureRuleId
                                .Matches(ruleToResolve.Text)
                                .Select(m => m.Value)
                                .ToArray();

                    foreach (var id in ids)
                    {
                        var replaceRule = rules.FirstOrDefault(r => r.IsFinal && r.Id == id);
                        if (replaceRule == null)
                            continue;

                        ruleToResolve.Text = ruleToResolve.Text.Replace(id, $"({replaceRule.Text})");
                    }
                }
            } while (!rules.All(r => r.IsFinal));
        }

        class Rule
        {
            public Rule(string id, string text)
            {
                Id = id;
                Text = text;
            }

            public bool IsFinal => !Text.ToCharArray().Any(char.IsDigit);

            public string Id { get; }
            public string Text { get; set; }
        }
    }
}
