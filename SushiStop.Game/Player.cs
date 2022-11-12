using System.Collections.Generic;
using SushiStop.Game.Cards;

namespace SushiStop.Game
{
    public class Player
    {
        // 1, 2, 3, 4, or 5
        public int Number { get; set; }

        public List<Card> Hand = new List<Card>();
        public List<Card> PlayedCards = new List<Card>();
    }
}
