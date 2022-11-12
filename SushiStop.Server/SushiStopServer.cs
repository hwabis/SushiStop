﻿using System.Net;
using System.Net.Sockets;
using NetCoreServer;
using SushiStop.Game;

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
                player.Hand.Clear(); // Hand should already be clear but let's make sure
                player.PlayedCards.Clear();
            }
            Deck.Reset();
        }
    }
}
