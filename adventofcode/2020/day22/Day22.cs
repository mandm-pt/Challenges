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
            ulong score = PlayGame(Player1, Player2, GetWinner).GetScore();

            return Task.FromResult(score.ToString());
        }

        protected override async Task<string> Part2Async()
        {
            await LoadyAsync(); // reload

            ulong score = PlayGame(Player1, Player2, GetRecursiveWinner).GetScore();

            return score.ToString();
        }

        private static Deck PlayGame(Deck p1Deck, Deck p2Deck, Func<Deck, Deck, Deck> checkWinningDeck)
        {
            while (p1Deck.RemainingCards > 0 && p2Deck.RemainingCards > 0)
            {
                if (p1Deck.HasRepeatedHand || p2Deck.HasRepeatedHand)
                    return p1Deck;

                p1Deck.GetTopCard();
                p2Deck.GetTopCard();

                if (checkWinningDeck(p1Deck, p2Deck).PlayerId == p1Deck.PlayerId)
                    p1Deck.AddCard(p1Deck.LastCard).AddCard(p2Deck.LastCard);
                else
                    p2Deck.AddCard(p2Deck.LastCard).AddCard(p1Deck.LastCard);
            }

            var winningDeck = p1Deck.RemainingCards == 0 ? p2Deck : p1Deck;

            return winningDeck;
        }

        private static Deck GetWinner(Deck p1Deck, Deck p2Deck)
        {
            if (p1Deck.LastCard > p2Deck.LastCard)
                return p1Deck;
            else
                return p2Deck;
        }

        private static Deck GetRecursiveWinner(Deck p1Deck, Deck p2Deck)
        {
            if (p1Deck.LastCard <= p1Deck.RemainingCards && p2Deck.LastCard <= p2Deck.RemainingCards)
                return PlayGame(p1Deck.Clone(), p2Deck.Clone(), GetRecursiveWinner);
            else if (p1Deck.LastCard > p2Deck.LastCard)
                return p1Deck;
            else
                return p2Deck;
        }

        private class Deck
        {
            private readonly List<string> PreviousHands = new List<string>();
            private readonly Queue<int> cards = new Queue<int>();

            public Deck(int playerId)
            {
                PlayerId = playerId;
            }

            public int PlayerId { get; }
            public int RemainingCards => cards.Count;
            public bool HasRepeatedHand => PreviousHands.Contains(ToString());
            public int LastCard { get; private set; }

            public Deck AddCard(int card)
            {
                cards.Enqueue(card);
                return this;
            }

            public void GetTopCard()
            {
                PreviousHands.Add(ToString());
                LastCard = cards.Dequeue();
            }

            public ulong GetScore()
            {
                var stack = new Stack<int>(cards);

                ulong score = 0;
                int multiplier = 1;
                while (stack.Count > 0)
                {
                    score += (ulong)(stack.Pop() * multiplier++);
                }

                return score;
            }

            public Deck Clone()
            {
                var newDeck = new Deck(PlayerId);

                foreach (var card in cards)
                {
                    newDeck.AddCard(card);
                }

                return newDeck;
            }

            public override string ToString() => string.Join(null, cards.ToArray());
        }
    }
}