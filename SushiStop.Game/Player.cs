using System;
using System.Collections.Generic;
using SushiStop.Game.Cards;

namespace SushiStop.Game
{
    public class Player
    {
        // 1 to 5
        private int number;
        public int Number
        {
            get => number;
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentOutOfRangeException($"Player cannot be Number {value}");

                number = value;
            }
        }

        public List<Card> Hand = new List<Card>();
        public List<Card> PlayedCards = new List<Card>();
    }
}
