using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day16 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 16;

        private readonly Regex CaptureValidNumbersRegex = new Regex(@"([ a-z]+): (\d+)-(\d+) or (\d+)-(\d+)", RegexOptions.Compiled);
        private readonly Regex CaptureTicketsRegex = new Regex(@"^[(\d+),]+", RegexOptions.Compiled | RegexOptions.Multiline);

        private Field[] Fields = Array.Empty<Field>();
        private long[][] Tickets = Array.Empty<long[]>();

        protected override async Task LoadyAsync()
        {
            string contents = await File.ReadAllTextAsync(InputFilePath);

            Tickets = CaptureTicketsRegex.Matches(contents)
                                .Select(m => m.Value.Split(',').Select(long.Parse).ToArray())
                                .ToArray();

            var allValidPositions = Enumerable.Range(0, Tickets[0].Length);

            Fields = CaptureValidNumbersRegex.Matches(contents)
                                .Select(m =>
                                {
                                    var from = int.Parse(m.Groups[2].Value);
                                    var to = int.Parse(m.Groups[3].Value) - from + 1;

                                    var orFrom = int.Parse(m.Groups[4].Value);
                                    var orTo = int.Parse(m.Groups[5].Value) - orFrom + 1;

                                    var validNumbers = Enumerable.Range(from, to).Select(n => (long)n).ToList()
                                                        .Concat(Enumerable.Range(orFrom, orTo)
                                                        .Select(n => (long)n))
                                                        .ToArray();

                                    var validPositions = new List<int>(allValidPositions);

                                    return new Field(m.Groups[1].Value, validNumbers, validPositions);
                                })
                                .ToArray();


        }

        protected override Task<string> Part1Async()
        {
            var allValidNumbers = Fields.SelectMany(f => f.ValidNumbers).ToArray();
            var allInvalidTickets = FilterTickets(Tickets.Skip(1), t => t.Any(n => !allValidNumbers.Contains(n)));

            long sumInvalid = 0;
            foreach (var ticket in allInvalidTickets)
            {
                sumInvalid += ticket.Where(n => !allValidNumbers.Contains(n)).Aggregate((long)0, (a, b) => a + b);
            }

            return Task.FromResult(sumInvalid.ToString());
        }

        protected override Task<string> Part2Async()
        {
            var allValidNumbers = Fields.SelectMany(f => f.ValidNumbers).ToArray();
            var allValidTickets = FilterTickets(Tickets, t => t.All(n => allValidNumbers.Contains(n))).ToArray();

            CheckPossiblePositions(allValidTickets, Fields);

            var departureFields = Fields
                                    .Where(f => f.Name.Contains("departure"))
                                    .Select(f => f.ValidPositions[0]);

            var myTicket = allValidTickets.First();
            long result = 1;
            foreach (int idx in departureFields)
            {
                result *= myTicket[idx];
            }

            return Task.FromResult(result.ToString());
        }

        protected IEnumerable<long[]> FilterTickets(IEnumerable<long[]> tickets, Func<long[], bool> ticketsFilter)
        {
            var filteredTickets = new List<long[]>();

            foreach (var ticket in tickets)
            {
                if (ticketsFilter(ticket))
                    filteredTickets.Add(ticket);
            }

            return filteredTickets.ToArray();
        }

        private void CheckPossiblePositions(long[][] tickets, Field[] fields)
        {
            var toRemove = new Stack<Tuple<Field, int>>();

            foreach (var ticket in tickets)
            {
                for (int position = 0; position < ticket.Length; position++)
                {
                    var number = ticket[position];

                    foreach (var field in fields)
                    {
                        if (field.ValidNumbers.Contains(number))
                            continue;

                        if (field.ValidPositions.Remove(position) &&
                            field.ValidPositions.Count == 1)
                        {
                            toRemove.Push(new(field, field.ValidPositions[0]));
                        }

                        while (toRemove.Count != 0)
                        {
                            (Field ownerField, int positionToRemove) = toRemove.Pop();
                            foreach (var otherField in fields.Where(f => f != ownerField))
                            {
                                if (otherField.ValidPositions.Remove(ownerField.ValidPositions[0]) &&
                                    otherField.ValidPositions.Count == 1)
                                {
                                    toRemove.Push(new(otherField, otherField.ValidPositions[0]));
                                }
                            }
                        }
                    }
                }
            }
        }

        private record Field(string Name, long[] ValidNumbers, IList<int> ValidPositions);
    }
}
