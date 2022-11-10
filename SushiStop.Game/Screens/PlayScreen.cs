using System.Collections.Generic;
using NetCoreServer;
using Newtonsoft.Json;
using osu.Framework.Screens;
using SushiStop.Game.Cards;
using SushiStop.Game.Networking;

namespace SushiStop.Game.Screens
{
    public class PlayScreen : Screen
    {
        public List<Card> Hand { get; set; }

        private SushiStopClient client;

        public PlayScreen(SushiStopClient client)
        {
            this.client = client;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            client.SendAsync(JsonConvert.SerializeObject(new TcpMessage
            {
                Type = TcpMessageType.StartRoundRequest
            }));
        }
    }
}
