using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day23 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 23;

        private LinkedList StartingCups = new LinkedList();

        protected override async Task LoadyAsync()
        {
            string contents = await File.ReadAllTextAsync(InputFilePath);

            //contents = "389125467";
            StartingCups = new LinkedList();
            foreach (var value in contents.ToCharArray().Select(c => int.Parse(c.ToString())))
            {
                StartingCups.Add(value);
            }
        }

        protected override Task<string> Part1Async()
        {
            var list = Run(100, StartingCups);
            string result = list.ToString(1);

            return Task.FromResult(result);
        }

        protected override Task<string> Part2Async()
        {
            // not sure if this is correct
            string seed = "389125467";

            int max = seed.ToCharArray().Select(c => int.Parse(c.ToString())).Max();
            StartingCups = new LinkedList();

            for (int i = 1; i < max; i++)
            {
                StartingCups.Add(i);
            }

            foreach (var value in Enumerable.Range(max + 1, 1000000 - max))
            {
                StartingCups.Add(value);
            }

            var list = Run(10000000, StartingCups);

            var n1 = (ulong)list.Head.Next.Value;
            var n2 = (ulong)list.Head.Next.Next.Value;

            System.Console.WriteLine(n1);
            System.Console.WriteLine(n2);

            ulong result = n1 * n2;

            return Task.FromResult(result.ToString());
        }

        private LinkedList Run(int rounds, LinkedList startingCups)
        {
            while (rounds-- > 0)
            {
                var cup = startingCups.Head!;
                var cupToMove1 = cup.Next;
                var cupToMove3 = cupToMove1!.Next.Next;

                int currentCupValue = cup.Value;
                LinkedNumber? destination = null;

                while (destination == null)
                {
                    if (--currentCupValue < 0)
                        currentCupValue = startingCups.Count;

                    if (currentCupValue == cupToMove1.Value || currentCupValue == cupToMove1.Next.Value || currentCupValue == cupToMove3.Value)
                        continue;

                    destination = startingCups.GetLinkedNumberByValue(currentCupValue);
                }

                var bck = destination.Next;

                cup.Next = cupToMove3.Next;
                destination.Next = cupToMove1;
                destination.Previous = cup;

                cupToMove1.Previous = destination;
                cupToMove3.Next = bck;
                bck.Previous = cupToMove3;

                startingCups.Head = cup.Next;
            }

            return startingCups;
        }

        private class LinkedList
        {
            private readonly Dictionary<int, LinkedNumber> InverseMembersLookup = new Dictionary<int, LinkedNumber>();

            public LinkedList()
            {
            }
            public LinkedList(int value) : this()
            {
                Add(value);
            }

            public LinkedNumber? Head { get; set; }
            public int Count { get; private set; }

            public void Add(int value)
            {
                Count++;

                if (Head is null)
                {
                    Head = new LinkedNumber(value);
                    Head.Next = Head.Previous = Head;
                    InverseMembersLookup.Add(Head.Value, Head);
                    return;
                }

                var oldPrevious = Head.Previous;

                var newElement = new LinkedNumber(value);
                Head.Previous = newElement;
                newElement.Next = Head;
                newElement.Previous = oldPrevious;

                oldPrevious.Next = newElement;

                InverseMembersLookup.Add(newElement.Value, newElement);
            }

            public LinkedNumber? GetLinkedNumberByValue(int valueToGet)
                => InverseMembersLookup.ContainsKey(valueToGet) ? InverseMembersLookup[valueToGet] : null;

            public override string ToString()
            {
                if (Head is null) return string.Empty;

                var sb = new StringBuilder(Head.Value.ToString());

                var next = Head;
                while (next!.Next != Head)
                {
                    next = next.Next;
                    sb.Append(next.Value);
                }

                return sb.ToString();
            }

            public string ToString(int startValue)
            {
                // Find starting member
                var startMember = GetLinkedNumberByValue(startValue);

                if (startMember is null)
                    return string.Empty;

                // skip start member
                var next = startMember.Next;
                var sb = new StringBuilder(next.Value.ToString());
                while (next!.Next != startMember)
                {
                    next = next.Next;
                    sb.Append(next.Value);
                }

                return sb.ToString();
            }
        }

        private class LinkedNumber
        {
            public LinkedNumber(int value, LinkedNumber? previous = null, LinkedNumber? next = null)
            {
                Value = value;
                Previous = previous ?? this;
                Next = next ?? this;
            }

            public int Value { get; }
            public LinkedNumber Next { get; set; }
            public LinkedNumber Previous { get; set; }
            public override string ToString() => Value.ToString();
        }
    }
}