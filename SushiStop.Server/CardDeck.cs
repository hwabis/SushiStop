using SushiStop.Game.Cards;

namespace SushiStop.Server
{
    public class CardDeck
    {
        private List<Card> deck = new List<Card>();
        private Random random = new Random();

        public CardDeck()
        {
            ResetDeck();
        }

        public Card DrawRandomCard()
        {
            Card topCard = deck[random.Next(deck.Count)];
            deck.Remove(topCard);
            return topCard;
        }

        public void ResetDeck()
        {
            for (int i = 0; i < 14; i++)
                deck.Add(Card.Tempura);
            for (int i = 0; i < 14; i++)
                deck.Add(Card.Sashimi);
            for (int i = 0; i < 14; i++)
                deck.Add(Card.Dumpling);
            for (int i = 0; i < 12; i++)
                deck.Add(Card.MakiRoll2);
            for (int i = 0; i < 8; i++)
                deck.Add(Card.MakiRoll3);
            for (int i = 0; i < 6; i++)
                deck.Add(Card.MakiRoll1);
            for (int i = 0; i < 10; i++)
                deck.Add(Card.SalmonNigiri);
            for (int i = 0; i < 5; i++)
                deck.Add(Card.SquidNigiri);
            for (int i = 0; i < 5; i++)
                deck.Add(Card.EggNigiri);
            for (int i = 0; i < 10; i++)
                deck.Add(Card.Pudding);
            for (int i = 0; i < 6; i++)
                deck.Add(Card.Wasabi);
            for (int i = 0; i < 4; i++)
                deck.Add(Card.Chopsticks);
        }
    }
}
