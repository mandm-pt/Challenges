using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
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
            int turn = 0;
            while (Player1.RemainingCards > 0 && Player2.RemainingCards > 0)
            {
                turn++;

                var c1 = Player1.GetTopCard();
                var c2 = Player2.GetTopCard();

                if (c1 > c2)
                    Player1.AddCard(c1).AddCard(c2);
                else
                    Player2.AddCard(c2).AddCard(c1);
            }

            var winningDeck = Player1.RemainingCards == 0 ? Player2 : Player1;

            ulong score = winningDeck.GetScore();

            return Task.FromResult(score.ToString());
        }

        private class Deck
        {
            private readonly Queue<int> cards = new Queue<int>();

            public Deck(int playerId)
            {
                PlayerId = playerId;
            }

            public int PlayerId { get; }
            public int RemainingCards => cards.Count;

            public Deck AddCard(int card)
            {
                cards.Enqueue(card);
                return this;
            }

            public int GetTopCard() => cards.Dequeue();

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
        }
    }
}