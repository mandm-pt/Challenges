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

        private MathExpression[] Expressions = Array.Empty<MathExpression>();
        private readonly Regex CaptureMathTokens = new Regex(@"\d+|[+|*]|(\(.*\))", RegexOptions.Compiled);
        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            List<MathExpression> expressions = new List<MathExpression>();
            foreach (var line in inputLines)
            {
                var tokensToProcess = CaptureMathTokens.Matches(line).Select(m => m.Value).ToArray();
                expressions.Add(ParseExpression(tokensToProcess));
            }

            Expressions = expressions.ToArray();
        }

        protected override Task<string> Part1Async()
        {
            int sum = 0;
            foreach (var exp in Expressions)
            {
                sum += ProcessExpression(exp);
            }

            return Task.FromResult(sum.ToString());
        }

        private int ProcessExpression(IMathOperation operation)
        {
            var memory = new Queue<int>();

            if (operation is NumberSymbol numberExp)
            {
                memory.Enqueue(numberExp.Number);
            }
            else if (operation is OperationSymbol opExp)
            {
                memory.Enqueue(opExp.Symbol);
            }
            else if (operation is MathExpression complexExp)
            {
                foreach (var innerOperation in complexExp.Operations)
                {
                    var result = ProcessExpression(innerOperation);

                    memory.Enqueue(result);
                }
            }

            if (memory.Count >= 3)
            {
                do
                {
                    int a = memory.Dequeue();
                    char op = (char)memory.Dequeue();
                    int b = memory.Dequeue();

                    var aux = new Stack<int>();
                    aux.Push(Calculate(a, op, b));

                    while (memory.Count % 3 != 0)
                    {
                        aux.Push(memory.Dequeue());
                    }

                    while (aux.Count > 0)
                    {
                        memory.Enqueue(aux.Pop());
                    }
                } while (memory.Count >= 3);
            }

            return memory.Dequeue();
        }

        int Calculate(int a, char op, int b)
        {
            return op switch
            {
                '+' => a + b,
                '*' => a * b,
                _ => throw new Exception()
            };
        }

        private MathExpression ParseExpression(string[] tokens)
        {
            var symbols = new Queue<IMathOperation>();
            foreach (var token in tokens)
            {
                if (int.TryParse(token, out int number))
                {
                    symbols.Enqueue(new NumberSymbol(number));
                }
                else if (token[0] is not '(' and not ')')
                {
                    symbols.Enqueue(new OperationSymbol(token[0]));
                }
                else
                {
                    var innerTokens = CaptureMathTokens.Matches(token[1..^1]).Select(m => m.Value).ToArray();
                    symbols.Enqueue(ParseExpression(innerTokens));
                }
            }

            return new MathExpression(symbols);
        }

        private interface IMathOperation { }

        private struct MathExpression : IMathOperation
        {
            public MathExpression(Queue<IMathOperation> operations)
            {
                Operations = operations;
            }

            public Queue<IMathOperation> Operations { get; }
        }

        private struct NumberSymbol : IMathOperation
        {
            public NumberSymbol(int number)
            {
                Number = number;
            }

            public int Number { get; }

            public override string ToString() => $"{Number}";
        }

        private struct OperationSymbol : IMathOperation
        {
            public OperationSymbol(char symbol)
            {
                Symbol = symbol;
            }

            public char Symbol { get; }

            public override string ToString() => $"{Symbol}";
        }
    }
}
