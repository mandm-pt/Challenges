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

        private MathExpression[][] Expressions = Array.Empty<MathExpression[]>();
        private readonly Regex CaptureMathTokens = new Regex(@"\d+|\+|\*|\(|\)", RegexOptions.Compiled);
        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            var expressions = new List<MathExpression[]>();
            foreach (var line in inputLines)
            {
                var tokensToProcess = CaptureMathTokens.Matches(line).Select(m => m.Value).ToArray();
                expressions.Add(ParseExpression(tokensToProcess));
            }

            Expressions = expressions.ToArray();
        }

        protected override Task<string> Part1Async()
        {
            long sum = 0;
            int count = 0;
            foreach (var exp in Expressions)
            {
                count++;
                var result = ProcessExpression(exp);
                sum += result.Item2;
            }

            return Task.FromResult(sum.ToString());
        }

        private (int, long) ProcessExpression(MathExpression[] expressions)
        {
            var queue = new Queue<long>();
            for (int i = 0; i < expressions.Length; i++)
            {
                var exp = expressions[i];

                switch (expressions[i].Type)
                {
                    case TokenType.Lp:
                        (int newIdx, long innerResult) = ProcessExpression(expressions[++i..]);
                        queue.Enqueue(innerResult);
                        i += newIdx;
                        continue;
                        break;
                    case TokenType.Rp:
                        queue.Enqueue(ProcessQueue(queue));
                        return (i, queue.Dequeue());
                        break;
                    case TokenType.Numner:
                        queue.Enqueue(long.Parse(exp.Value!));
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

        private long ProcessQueue(Queue<long> stack)
        {
            long a = stack.Dequeue();
            while (stack.Count >= 2)
            {
                char op = (char)stack.Dequeue();
                long b = stack.Dequeue();

                a = op switch
                {
                    '+' => a + b,
                    '*' => a * b,
                    _ => throw new Exception()
                };
            }

            return a;
        }

        private static MathExpression[] ParseExpression(string[] tokens)
        {
            var symbols = new List<MathExpression>();
            foreach (var token in tokens)
            {
                if (int.TryParse(token, out _))
                    symbols.Add(new(TokenType.Numner, token));
                else if (token[0] is '*')
                    symbols.Add(new(TokenType.Star));
                else if (token[0] is '+')
                    symbols.Add(new(TokenType.Plus));
                else if (token[0] is ')')
                    symbols.Add(new(TokenType.Rp));
                else if (token[0] is '(')
                    symbols.Add(new(TokenType.Lp));
            }

            return symbols.ToArray();
        }

        private record MathExpression(TokenType Type, string? Value = null)
        {
            public override string ToString()
            {
                return Type switch
                {
                    TokenType.Lp => "(",
                    TokenType.Rp => ")",
                    TokenType.Plus => "+",
                    TokenType.Star => "*",
                    _ => Value ?? "",
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
