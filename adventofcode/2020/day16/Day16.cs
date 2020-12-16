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

            Fields = CaptureValidNumbersRegex.Matches(contents)
                                .Select(m =>
                                {
                                    var from = int.Parse(m.Groups[2].Value);
                                    var to = int.Parse(m.Groups[3].Value) - from + 1;

                                    var orFrom = int.Parse(m.Groups[4].Value);
                                    var orTo = int.Parse(m.Groups[5].Value) - orFrom + 1;

                                    var validNumbers = Enumerable.Range(from, to).Select(n => (long)n).ToList()
                                                        .Concat(Enumerable.Range(orFrom, orTo).Select(n => (long)n));

                                    return new Field(m.Groups[1].Value, validNumbers.Cast<long>());
                                })
                                .ToArray();

            Tickets = CaptureTicketsRegex.Matches(contents)
                                .Select(m => m.Value.Split(',').Select(long.Parse).ToArray())
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

            for (int i = 0; i < allValidTickets.Length; i++)
            {
                var ticket = allValidTickets[i];

                for (int j = 0; j < ticket.Length; j++)
                {
                    var number = ticket[j];

                    foreach (var field in Fields)
                    {
                        if (field.ValidNumbers.Contains(number))
                        {
                            if (field.InvalidPositions.Contains(j))
                                _ = field.ValidPositions.Remove(j);
                            else if (!field.ValidPositions.Contains(j))
                                field.ValidPositions.Add(j);
                        }
                        else if (!field.InvalidPositions.Contains(j))
                        {
                            field.InvalidPositions.Add(j);

                            if (field.ValidPositions.Contains(j))
                                _ = field.ValidPositions.Remove(j);
                        }
                    }
                }
            }

            do
            {
                foreach (var field in Fields.Where(f => f.ValidPositions.Count == 1))
                {
                    var number = field.ValidPositions[0];

                    foreach (var filedToRemove in Fields
                                                .Where(f => f != field)
                                                .Where(f => f.ValidPositions.Contains(number)))
                    {
                        filedToRemove.ValidPositions.Remove(number);
                    }
                }
            } while (!Fields.All(f => f.ValidPositions.Count == 1));

            var departureFields = Fields.Where(f => f.Name.Contains("departure")).Select(f => f.ValidPositions[0]);

            long[] myTicket = allValidTickets[0];
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

        private record Field(string Name, IEnumerable<long> ValidNumbers)
        {
            private readonly IList<int> validPositions = new List<int>();
            private readonly IList<int> invalidPositions = new List<int>();

            public IList<int> ValidPositions => validPositions;
            public IList<int> InvalidPositions => invalidPositions;
        }
    }
}
