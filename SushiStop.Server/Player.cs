using SushiStop.Game.Cards;

namespace SushiStop.Server
{
    public class Player
    {
        public Guid Id { get; set; }

        public List<Card> Hand = new List<Card>();
        public List<Card> PlayedCards = new List<Card>();

        public Player()
        {
        }

        public Player(Guid id)
        {
            Id = id;
        }
    }
}
