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
                deck.Add(new TempuraCard());
            for (int i = 0; i < 14; i++)
                deck.Add(new SashimiCard());
            for (int i = 0; i < 14; i++)
                deck.Add(new DumplingCard());
            for (int i = 0; i < 12; i++)
                deck.Add(new MakiRollCard(2));
            for (int i = 0; i < 8; i++)
                deck.Add(new MakiRollCard(3));
            for (int i = 0; i < 6; i++)
                deck.Add(new MakiRollCard(1));
            for (int i = 0; i < 10; i++)
                deck.Add(new NigiriCard(2));
            for (int i = 0; i < 5; i++)
                deck.Add(new NigiriCard(3));
            for (int i = 0; i < 5; i++)
                deck.Add(new NigiriCard(1));
            for (int i = 0; i < 10; i++)
                deck.Add(new PuddingCard());
            for (int i = 0; i < 6; i++)
                deck.Add(new WasabiCard());
            for (int i = 0; i < 4; i++)
                deck.Add(new ChopsticksCard());
        }
    }
}
