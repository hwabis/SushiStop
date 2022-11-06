using System;
using System.Net;
using System.Text;
using NetCoreServer;
using Newtonsoft.Json;
using osu.Framework.Screens;
using SushiStop.Game.Screens;
using Logger = osu.Framework.Logging.Logger;

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

            TcpMessage message = JsonConvert.DeserializeObject<TcpMessage>(messageString);
            if (message == null)
            {
                return;
            }

            switch (message.Type)
            {
                case TcpMessageType.PlayerNumber:
                    if (screenStack.CurrentScreen is LobbyScreen screen)
                    {
                        screen.PlayerNumberText.Text += message.PlayerNumber.ToString();
                    }
                    break;

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
    }
}
