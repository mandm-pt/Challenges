using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day18 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 18;

        private readonly Regex CapturePriority = new Regex(@"\(\d+ ( ?[+*] \d+)+\)", RegexOptions.Compiled);
        private readonly Regex CaptureSum = new Regex(@"\d+( ?[+] \d+)+", RegexOptions.Compiled);
        private readonly Regex ExtractTokens = new Regex(@"\d+|\+|\*|\(|\)", RegexOptions.Compiled);

        protected override Task<string> Part1Async()
        {
            ulong sum = 0;
            for (int i = 0; i < inputLines.Length; i++)
            {
                string line = inputLines[i];

                sum += ProcessExpression(line);
            }

            return Task.FromResult(sum.ToString());
        }

        protected override Task<string> Part2Async()
        {
            ulong sum = 0;
            for (int i = 0; i < inputLines.Length; i++)
            {
                string line = inputLines[i];

                sum += ProcessExpression(line, InjectParenteses);
            }

            return Task.FromResult(sum.ToString());
        }

        private string InjectParenteses(string stringExpression)
        {
            foreach (Match sumExpression in CaptureSum.Matches(stringExpression))
            {
                stringExpression = CaptureSum.Replace(stringExpression, $"({sumExpression.Value})", 1, sumExpression.Index);
            }

            return stringExpression;
        }

        protected ulong ProcessExpression(string line, Func<string, string>? expModifier = null)
        {
            var expressions = new List<MathExpression[]>();

            ulong result = 0;
            var matches = CapturePriority.Matches(line);
            while (matches.Count > 0)
            {
                foreach (string parenthesesMatch in matches.Select(m => m.Value))
                {
                    string modifiedLine = parenthesesMatch;
                    if (expModifier != null)
                        modifiedLine = expModifier(parenthesesMatch);

                    var tokens = ParseExpression(modifiedLine);
                    (_, result) = ProcessExpression(tokens);
                    line = line.Replace(parenthesesMatch, result.ToString());
                }

                matches = CapturePriority.Matches(line);
            }

            if (expModifier != null)
                line = expModifier(line);

            var lineTokens = ParseExpression(line);
            (_, result) = ProcessExpression(lineTokens);

            return result;
        }

        private static (int, ulong) ProcessExpression(MathExpression[] expressions)
        {
            var queue = new Queue<ulong>();
            for (int i = 0; i < expressions.Length; i++)
            {
                var exp = expressions[i];

                switch (expressions[i].Type)
                {
                    case TokenType.Lp:
                        (int newIdx, ulong innerResult) = ProcessExpression(expressions[++i..]);
                        queue.Enqueue(innerResult);
                        i += newIdx;
                        continue;
                    case TokenType.Rp:
                        queue.Enqueue(ProcessQueue(queue));
                        return (i, queue.Dequeue());
                    case TokenType.Numner:
                        queue.Enqueue(exp.Value!.Value);
                        break;
                    case TokenType.Star:
                        queue.Enqueue('*');
                        break;
                    case TokenType.Plus:
                        queue.Enqueue('+');
                        break;
                    default:
                        break;
                }
            }

            return (0, ProcessQueue(queue));
        }

        private static ulong ProcessQueue(Queue<ulong> queue)
        {
            var a = queue.Dequeue();
            while (queue.Count >= 2)
            {
                char op = (char)queue.Dequeue();
                var b = queue.Dequeue();

                a = op switch
                {
                    '+' => a + b,
                    '*' => a * b,
                    _ => throw new Exception()
                };
            }

            return a;
        }

        private MathExpression[] ParseExpression(string text)
        {
            var symbols = new List<MathExpression>();
            foreach (var token in ExtractTokens.Matches(text).Select(m => m.Value))
            {
                if (ulong.TryParse(token, out ulong number)) symbols.Add(new(TokenType.Numner, number));
                else if (token[0] is '*') symbols.Add(new(TokenType.Star));
                else if (token[0] is '+') symbols.Add(new(TokenType.Plus));
                else if (token[0] is ')') symbols.Add(new(TokenType.Rp));
                else if (token[0] is '(') symbols.Add(new(TokenType.Lp));
            }

            return symbols.ToArray();
        }

        private record MathExpression(TokenType Type, ulong? Value = null)
        {
            public override string ToString()
            {
                return Type switch
                {
                    TokenType.Lp => "(",
                    TokenType.Rp => ")",
                    TokenType.Plus => " + ",
                    TokenType.Star => " * ",
                    TokenType.Numner => $"{Value!.Value}",
                    _ => throw new NotImplementedException(),
                };
            }
        }

        private enum TokenType
        {
            Lp,
            Rp,
            Numner,
            Star,
            Plus,
        }
    }
}
