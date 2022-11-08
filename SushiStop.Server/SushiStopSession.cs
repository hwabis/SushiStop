using System.Net.Sockets;
using System.Text;
using NetCoreServer;
using Newtonsoft.Json;
using SushiStop.Game.Networking;

namespace SushiStop.Server
{
    public class SushiStopSession : TcpSession
    {
        private SushiStopServer server;

        public SushiStopSession(SushiStopServer server) : base(server)
        {
            this.server = server;
        }

        protected override void OnConnected()
        {
            Console.WriteLine($"TCP session with Id {Id} connected!");
            server.PlayerCount++;
        }

        protected override void OnDisconnected()
        {
            Console.WriteLine($"TCP session with Id {Id} disconnected!");
            server.PlayerCount--;
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
                    if (server.PlayerCount == 5)
                    {
                        // We're already at the max player limit
                        Disconnect();
                        break;
                    }

                    Console.WriteLine("Sending player number: " + server.PlayerCount);
                    Send(JsonConvert.SerializeObject(new TcpMessage
                    {
                        Type = TcpMessageType.PlayerNumber,
                        PlayerNumber = server.PlayerCount
                    }));
                    break;

                case TcpMessageType.StartGameRequest:
                    if (server.PlayerCount <= 1)
                        return;

                    Console.WriteLine("Server starting game!");
                    server.Multicast(JsonConvert.SerializeObject(new TcpMessage
                    {
                        Type = TcpMessageType.StartGame
                    }));
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
