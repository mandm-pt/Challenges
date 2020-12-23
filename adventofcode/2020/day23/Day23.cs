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

            StartingCups = Load(contents);
        }

        protected override Task<string> Part1Async()
        {
            var list = Run(100, StartingCups);
            string result = list.ToString(1);

            return Task.FromResult(result);
        }

        protected override async Task<string> Part2Async()
        {
            string seed = "389125467";
            int max = seed.ToCharArray().Select(c => int.Parse(c.ToString())).Max();

            var nums = Enumerable.Range(1, max).ToList();
            nums.AddRange(Enumerable.Range(max + 1, 1000000 - max));
            string initial = string.Join(null, nums);

            StartingCups = Load(initial);

            var list = Run(10000000, StartingCups);
            string result = list.ToString(1);

            return result;
        }

        private LinkedList Load(string initial)
        {
            StartingCups = new LinkedList();
            foreach (var value in initial.ToCharArray().Select(c => int.Parse(c.ToString())))
            {
                StartingCups.Add(value);
            }

            return StartingCups;
        }

        private LinkedList Run(int rounds, LinkedList startingCups)
        {
            for (int i = 0; rounds-- > 0;)
            {
                var cup = startingCups.Head!;
                var cupToMove1 = cup.Next;
                var cupToMove2 = cupToMove1!.Next;
                var cupToMove3 = cupToMove2!.Next;

                int currentCupValue = cup.Value;
                LinkedNumber? destination = null;

                while (destination == null && destination != cupToMove1
                    && destination != cupToMove2 && destination != cupToMove3)
                {
                    if (--currentCupValue < 0)
                        currentCupValue = startingCups.Count;

                    destination = startingCups.GetClockwiseLinkedNumber(currentCupValue, cupToMove3, cup);
                }

                var bck = destination.Next;

                cup.Next = cupToMove3.Next;
                destination.Next = cupToMove1;
                destination.Previous = cup;

                cupToMove1.Previous = destination;
                cupToMove3.Next = bck;
                bck.Previous = cupToMove3;

                startingCups.Head = cup.Next;
                if (i++ >= startingCups.Count)
                    i = 0;
            }

            return startingCups;
        }

        private class LinkedList
        {
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
                    return;
                }

                var oldPrevious = Head.Previous;

                var newElement = new LinkedNumber(value);
                Head.Previous = newElement;
                newElement.Next = Head;
                newElement.Previous = oldPrevious;

                oldPrevious.Next = newElement;
            }

            public LinkedNumber? GetClockwiseLinkedNumber(int valueToCompare, LinkedNumber startingMember, LinkedNumber stopMember)
            {
                var next = startingMember;
                while (next.Next != stopMember)
                {
                    next = next!.Next;
                    if (next.Value == valueToCompare)
                        return next;
                }

                return null;
            }

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
                if (Head is null) return string.Empty;

                // Find starting member
                var startMember = Head;
                while (startMember!.Value != startValue)
                {
                    startMember = startMember.Next;
                }

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