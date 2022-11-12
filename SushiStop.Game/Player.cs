using System;
using System.Collections.Generic;
using SushiStop.Game.Cards;

namespace SushiStop.Game
{
    public class Player
    {
        // 1, 2, 3, 4, or 5
        public int Number;/*
        {
            get => Number;
            set
            {
                if (value < 1 || value > 5)
                    throw new ArgumentOutOfRangeException($"Player cannot be Number {value}");

                Number = value;
            }
        }*/

        public List<Card> Hand = new List<Card>();
        public List<Card> PlayedCards = new List<Card>();
    }
}
