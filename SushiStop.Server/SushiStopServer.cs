using System.Net;
using System.Net.Sockets;
using NetCoreServer;

namespace SushiStop.Server
{
    public class SushiStopServer : TcpServer
    {
        // Player 1 is the Players[0], ...
        public List<Player> Players = new List<Player>();
        public CardDeck Deck = new CardDeck();

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
                player.Hand.Clear(); // Hand should already be clear but let's make sure
                player.PlayedCards.Clear();
            }
            Deck.Reset();
        }
    }
}
