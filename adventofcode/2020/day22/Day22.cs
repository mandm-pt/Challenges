using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AoC.Solutions._2020
{
    internal class Day22 : BaseDayChallenge
    {
        public override int Year => 2020;

        public override int Day => 22;

        private Deck Player1 = new Deck(0);
        private Deck Player2 = new Deck(0);

        protected override async Task LoadyAsync()
        {
            await base.LoadyAsync();

            var deck = (Player1 = new Deck(1));
            foreach (var line in inputLines[1..].Where(l => !string.IsNullOrWhiteSpace(l)))
            {
                if (line.StartsWith("Player"))
                {
                    deck = (Player2 = new Deck(2));
                    continue;
                }

                deck.AddCard(int.Parse(line));
            }
        }

        protected override Task<string> Part1Async()
        {
            ulong score = PlayGame(Player1, Player2, P1IsWinner).GetScore();

            return Task.FromResult(score.ToString());
        }

        protected override async Task<string> Part2Async()
        {
            await LoadyAsync(); // reload

            ulong score = PlayGame(Player1, Player2, P1IsRecursiveWinner).GetScore();

            return score.ToString();
        }

        private static Deck PlayGame(Deck p1Deck, Deck p2Deck, Func<Deck, Deck, bool> isPlaywe1Winner)
        {
            while (p1Deck.RemainingCards > 0 && p2Deck.RemainingCards > 0)
            {
                if (p1Deck.HasRepeatedHand || p2Deck.HasRepeatedHand)
                    return p1Deck;

                p1Deck.GetTopCard();
                p2Deck.GetTopCard();

                if (isPlaywe1Winner(p1Deck, p2Deck))
                    p1Deck.AddCard(p1Deck.LastCard).AddCard(p2Deck.LastCard);
                else
                    p2Deck.AddCard(p2Deck.LastCard).AddCard(p1Deck.LastCard);
            }

            var winningDeck = p1Deck.RemainingCards == 0 ? p2Deck : p1Deck;

            return winningDeck;
        }

        private static bool P1IsWinner(Deck p1Deck, Deck p2Deck) => p1Deck.LastCard > p2Deck.LastCard;

        private static bool P1IsRecursiveWinner(Deck p1Deck, Deck p2Deck)
        {
            if (p1Deck.LastCard <= p1Deck.RemainingCards && p2Deck.LastCard <= p2Deck.RemainingCards)
                return PlayGame(p1Deck.GetDeckForSubGame(), p2Deck.GetDeckForSubGame(), P1IsRecursiveWinner).PlayerId == p1Deck.PlayerId;
            else
                return p1Deck.LastCard > p2Deck.LastCard;
        }

        private class Deck
        {
            private readonly List<string> PreviousHands = new List<string>();
            private readonly Queue<int> Cards = new Queue<int>();

            public Deck(int playerId)
            {
                PlayerId = playerId;
            }

            public Deck(int playerId, IEnumerable<int> cards)
                : this(playerId)
            {
                Cards = new Queue<int>(cards);
            }

            public int PlayerId { get; }
            public int RemainingCards => Cards.Count;
            public bool HasRepeatedHand => PreviousHands.Contains(ToString());
            public int LastCard { get; private set; }

            public Deck AddCard(int card)
            {
                Cards.Enqueue(card);
                return this;
            }

            public void GetTopCard()
            {
                PreviousHands.Add(ToString());
                LastCard = Cards.Dequeue();
            }

            public ulong GetScore()
            {
                var stack = new Stack<int>(Cards);

                ulong score = 0;
                int multiplier = 1;
                while (stack.Count > 0)
                {
                    score += (ulong)(stack.Pop() * multiplier++);
                }

                return score;
            }

            public Deck GetDeckForSubGame() => new Deck(PlayerId, Cards.Take(LastCard));

            public override string ToString() => string.Join(',', Cards);
        }
    }
}