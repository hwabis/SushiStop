using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
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

        private string totalString = "";

        public SushiStopClient(IPAddress address, int port, ScreenStack screenStack)
            : base(address, port)
        {
            this.screenStack = screenStack;
        }

        protected override void OnReceived(byte[] buffer, long offset, long size)
        {
            string messageString = Encoding.UTF8.GetString(buffer, (int)offset, (int)size);
            Logger.Log("Received fragment: " + messageString);
            totalString += messageString;

            TcpMessage message;
            try
            {
                message = JsonConvert.DeserializeObject<TcpMessage>(totalString,
                    new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            }
            catch (Exception)
            {
                return; // Stop and allow totalstring to keep building until it's a valid message
            }

            totalString = ""; // We created a valid message so we can reset this
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

                case TcpMessageType.NextTurn:
                {
                    if (screenStack.CurrentScreen is PlayScreen playScreen)
                    {
                        if (message.Players[0].Hand.Count == 0)
                            playScreen.ShowScoresAndNewRoundButton(true);
                        else
                            playScreen.ShowScoresAndNewRoundButton(false);

                        playScreen.Players = message.Players;
                        // Only initialize TotalScores at the first time this is received
                        playScreen.TotalScores ??= new int[message.Players.Count];

                        playScreen.ResetForNewTurn();
                        playScreen.CreateDrawablePlayedCards();
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
