using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Screens;
using SushiStop.Game.Networking;

namespace SushiStop.Game.Screens
{
    public class LobbyScreen : Screen
    {
        public SpriteText PlayerNumberText;

        private SushiStopClient client;
        private ScreenStack screenStack;

        public LobbyScreen(SushiStopClient client)
        {
            this.client = client;
        }

        [BackgroundDependencyLoader]
        private void load(ScreenStack screenStack)
        {
            InternalChildren = new Drawable[]
            {
                PlayerNumberText = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = "You are player: ",
                    Font = FontUsage.Default.With(size: 40)
                }
            };

            this.screenStack = screenStack;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            // Now that we're in, we can request our player number.
            // Our SushiStopClient will handle the response
            client.SendAsync(JsonConvert.SerializeObject(new TcpMessage
            {
                Type = TcpMessageType.PlayerNumberRequest
            }));
        }
    }
}
