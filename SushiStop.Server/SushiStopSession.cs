using System.Net.Sockets;
using System.Text;
using NetCoreServer;
using Newtonsoft.Json;
using SushiStop.Game;
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
            server.Players.Add(player = new Player());
            // Assign player a Number when PlayerNumberRequest
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"TCP session with Id {Id} disconnected!");
            server.Players.Remove(player);
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string messageString = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);

            TcpMessage? message = JsonConvert.DeserializeObject<TcpMessage>(messageString,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
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
                        Disconnect();
                        break;
                    }

                    player.Number = server.Players.Count;

                    Console.WriteLine("Sending player number: " + player.Number);
                    Send(JsonConvert.SerializeObject(new TcpMessage
                    {
                        Type = TcpMessageType.PlayerNumber,
                        PlayerNumber = player.Number
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
                    // We only want to reset for every [number of players] StartRoundRequests
                    // (because every player will send a StartRoundRequest)
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

                    for (int i = 0; i < numberOfStartingCards; i++)
                        player.Hand.Add(server.Deck.DrawRandomCard());

                    Console.WriteLine($"Sending starting hand of {player.Hand.Count} cards");
                    Send(JsonConvert.SerializeObject(new TcpMessage
                    {
                        Type = TcpMessageType.StartRound,
                        Players = server.Players
                    }, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));
                    break;

                case TcpMessageType.EndTurn:
                    server.EndTurnCount++;

                    var index = server.Players.FindIndex(p => p.Number == player.Number);
                    player = message.Player; // There's actually no point in tracking player anymore...
                    server.Players[index] = message.Player;

                    if (server.EndTurnCount % server.Players.Count == 0)
                    {
                        Console.WriteLine("Next turn!");
                        server.Multicast(JsonConvert.SerializeObject(new TcpMessage
                        {
                            Type = TcpMessageType.NextTurn,
                            Players = server.Players
                        }, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));
                    }

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
