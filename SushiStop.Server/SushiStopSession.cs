using System.Net.Sockets;
using System.Text;
using NetCoreServer;
using Newtonsoft.Json;
using SushiStop.Game.Cards;
using SushiStop.Game.Networking;

namespace SushiStop.Server
{
    public class SushiStopSession : TcpSession
    {
        private Player player = new Player();

        private SushiStopServer server;

        public SushiStopSession(SushiStopServer server) : base(server)
        {
            this.server = server;
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"TCP session with Id {Id} connected!");
            player = new Player(Id);
            server.Players.Add(player);
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"TCP session with Id {Id} disconnected!");
            server.Players.Remove(player);

            // TODO: broadcast disconnect?
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string messageString = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Console.WriteLine("Incoming: " + messageString);

            TcpMessage? message = JsonConvert.DeserializeObject<TcpMessage>(messageString);
            if (message == null)
            {
                Console.WriteLine($"Received a message that failed to be deserialized");
                return;
            }

            switch (message.Type)
            {
                case TcpMessageType.PlayerNumberRequest:
                    if (server.Players.Count > 5)
                    {
                        // We're already at the max player limit
                        // TODO: all this lobby stuff and player tracking stuff is messed up when a player disconnects.
                        // This only works assuming everyone joins and doesn't leave
                        Disconnect();
                        break;
                    }

                    Console.WriteLine("Sending player number: " + server.Players.Count);
                    Send(JsonConvert.SerializeObject(new TcpMessage
                    {
                        Type = TcpMessageType.PlayerNumber,
                        PlayerNumber = server.Players.Count
                    }));
                    break;

                case TcpMessageType.StartGameRequest:
                    if (server.Players.Count <= 1)
                        return;

                    Console.WriteLine("Server starting game!");
                    server.Multicast(JsonConvert.SerializeObject(new TcpMessage
                    {
                        Type = TcpMessageType.StartGame
                    }));
                    break;

                case TcpMessageType.StartRoundRequest:
                    Console.WriteLine($"Starting round");
                    // We only want to reset for every [number of players] StartRoundRequests.
                    if (server.StartRoundRequestCount % server.Players.Count == 0)
                    {
                        Console.WriteLine("Resetting deck!");
                        server.ResetForNewRound();
                    }
                    server.StartRoundRequestCount++;

                    int numberOfStartingCards;
                    switch (server.Players.Count)
                    {
                        case 2:
                            numberOfStartingCards = 10;
                            break;
                        case 3:
                            numberOfStartingCards = 9;
                            break;
                        case 4:
                            numberOfStartingCards = 8;
                            break;
                        case 5:
                            numberOfStartingCards = 7;
                            break;
                        default:
                            Console.WriteLine($"Started a round with invalid number of players");
                            return;
                    }

                    // TODO: check that this is synchronized..? Every client is gonna be doing this at the same time
                    List<Card> startingHand = new List<Card>();
                    for (int i = 0; i < numberOfStartingCards; i++)
                        startingHand.Add(server.Deck.DrawRandomCard());

                    player.Hand = startingHand;

                    Console.WriteLine($"Sending starting hand of {player.Hand.Count} cards");
                    Send(JsonConvert.SerializeObject(new TcpMessage
                    {
                        Type = TcpMessageType.StartRound,
                        Hand = startingHand
                    }, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));
                    break;

                default:
                    Console.WriteLine($"Received a message with an invalid type");
                    break;
            }
        }

        protected override void OnError(SocketError error)
        {
            Console.WriteLine($"TCP session caught an error with code {error}");
        }
    }
}
