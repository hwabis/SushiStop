using System.Net;
using System.Net.Sockets;
using System.Text;
using Newtonsoft.Json;
using osu.Framework.Screens;
using SushiStop.Game.Screens;
using Logger = osu.Framework.Logging.Logger;
using TcpClient = NetCoreServer.TcpClient;

namespace SushiStop.Game.Networking
{
    public class SushiStopClient : TcpClient
    {
        private ScreenStack screenStack;

        public SushiStopClient(IPAddress address, int port, ScreenStack screenStack)
            : base(address, port)
        {
            this.screenStack = screenStack;
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string messageString = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Logger.Log("Incoming: " + messageString);

            TcpMessage message = JsonConvert.DeserializeObject<TcpMessage>(messageString,
                new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            if (message == null)
                return;

            switch (message.Type)
            {
                case TcpMessageType.PlayerNumber:
                {
                    if (screenStack.CurrentScreen is LobbyScreen lobbyScreen)
                        lobbyScreen.PlayerNumberBindable.Value = message.PlayerNumber;
                    break;
                }

                case TcpMessageType.StartGame:
                {
                    if (screenStack.CurrentScreen is LobbyScreen lobbyScreen)
                        lobbyScreen.GoToPlayScreen();
                    break;
                }

                case TcpMessageType.StartRound:
                {
                    if (screenStack.CurrentScreen is PlayScreen playScreen)
                    {
                        Logger.Log($"Received hand with {message.Hand.Count} cards");
                        playScreen.Hand = message.Hand;
                        playScreen.CreateDrawableHand();
                    }
                    break;
                }

                default:
                    Logger.Log($"Received a message with an invalid type");
                    break;
            }
        }

        protected override void OnDisconnected()
        {
            while (screenStack.CurrentScreen is not HomeScreen)
                screenStack.Exit();
        }

        protected override void OnError(SocketError error)
        {
            Logger.Log($"TCP client caught an error with code {error}");
        }
    }
}
