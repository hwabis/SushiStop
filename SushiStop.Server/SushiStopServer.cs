using System.Net;
using System.Net.Sockets;
using System.Numerics;
using NetCoreServer;
using NuGet.Protocol.Plugins;
using SixLabors.ImageSharp;
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
        // Used for knowing when all players are done (EndTurnCount % [number of players] == 0)
        public int EndTurnCount = 0;

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

        /// <summary>
        /// Gives the hand of the player in Players with Number 1 to the player with Number 2,
        /// gives 2's hand to 3, ... gives the last player's hand to 1.
        /// .. leetcode
        /// </summary>
        public void RotateHands()
        {
            List<Card> prevHand = Players[Players.FindIndex(p => p.Number == 1)].Hand;
            for (int number = 1; number <= Players.Count; number++)
            {
                int receivingPlayerNumber;
                if (number == Players.Count)
                    receivingPlayerNumber = 1;
                else
                    receivingPlayerNumber = number + 1;

                List<Card> tempHand = Players[Players.FindIndex(p => p.Number == receivingPlayerNumber)].Hand;
                Players[Players.FindIndex(p => p.Number == receivingPlayerNumber)].Hand = prevHand;
                prevHand = tempHand;
            }
        }
    }
}
