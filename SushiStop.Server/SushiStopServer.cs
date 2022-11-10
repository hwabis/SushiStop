using System.Net.Sockets;
using System.Net;
using NetCoreServer;
using SushiStop.Game.Cards;

namespace SushiStop.Server
{
    public class SushiStopServer : TcpServer
    {
        public int PlayerCount = 0;
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
    }
}
