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
        private int[][] Tickets = Array.Empty<int[]>();

        protected override async Task LoadyAsync()
        {
            string contents = await File.ReadAllTextAsync(InputFilePath);

            Fields = CaptureValidNumbersRegex.Matches(contents)
                                .Select(m =>
                                {
                                    int from = int.Parse(m.Groups[2].Value);
                                    int to = int.Parse(m.Groups[3].Value) - from + 1;

                                    int orFrom = int.Parse(m.Groups[4].Value);
                                    int orTo = int.Parse(m.Groups[5].Value) - orFrom + 1;

                                    var validNumbers = Enumerable.Range(from, to)
                                                        .Concat(Enumerable.Range(orFrom, orTo));

                                    return new Field(m.Groups[1].Value, validNumbers);
                                })
                                .ToArray();

            Tickets = CaptureTicketsRegex.Matches(contents)
                                .Select(m => m.Value.Split(',').Select(int.Parse).ToArray())
                                .ToArray();
        }

        protected override Task<string> Part1Async()
        {
            int[] allValidNumbers = Fields.SelectMany(f => f.ValidNumbers).ToArray();
            var allInvalidTickets = FilterTickets(Tickets.Skip(1), t => t.Any(n => !allValidNumbers.Contains(n)));

            int sumInvalid = 0;
            foreach (int[] ticket in allInvalidTickets)
            {
                sumInvalid += ticket.Where(n => !allValidNumbers.Contains(n)).Aggregate(0, (a, b) => a + b);
            }

            return Task.FromResult(sumInvalid.ToString());
        }

        protected IEnumerable<int[]> FilterTickets(IEnumerable<int[]> tickets, Func<int[], bool> ticketsFilter)
        {
            var filteredTickets = new List<int[]>();

            foreach (int[] ticket in tickets)
            {
                if (ticketsFilter(ticket))
                    filteredTickets.Add(ticket);
            }

            return filteredTickets.ToArray();
        }

        private record Field(string Name, IEnumerable<int> ValidNumbers);
    }
}
