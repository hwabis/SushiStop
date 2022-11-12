using System.Net;
using System.Net.Sockets;
using NetCoreServer;
using SushiStop.Game;
using SushiStop.Game.Cards;

namespace SushiStop.Server
{
    public class SushiStopServer : TcpServer
    {
        public List<Player> Players = new List<Player>();
        public CardDeck Deck = new CardDeck();

        // Used for filtering out the duplicate RoundStartRequests that call ResetForNewRound() too many times 
        public int StartRoundRequestCount = 0;

        public SushiStopServer(IPAddress address, int port)
            : base(address, port)
        {
        }

        protected override TcpSession CreateSession()
        {
            return new SushiStopSession(this);
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"Server caught an error with code {error}");
        }

        public void ResetForNewRound()
        {
            foreach (Player player in Players)
            {
                // We clear out all PlayedCards and put them into server.Deck, EXCEPT we leave the puddings
                foreach (Card card in player.PlayedCards)
                {
                    if (card is not PuddingCard)
                        Deck.AddCard(card);
                }
            }
        }
    }
}
