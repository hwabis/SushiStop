using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK.Graphics;
using SushiStop.Game.Networking;

namespace SushiStop.Game.Screens
{
    public class LobbyScreen : Screen
    {
        public readonly Bindable<int> PlayerNumberBindable = new Bindable<int>();
        private SpriteText playerNumberText;

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
                new FillFlowContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    AutoSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Children = new Drawable[]
                    {
                        playerNumberText = new SpriteText
                        {
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Text = "You are player: ",
                            Font = FontUsage.Default.With(size: 40)
                        },
                        new BasicButton
                        {
                            Text = "Start Game!",
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Width = 100,
                            Height = 50,
                            BackgroundColour = Color4.DarkBlue,
                            Action = requestStartGame,
                            Margin = new MarginPadding(5)
                        }
                    }
                }
            };

            this.screenStack = screenStack;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            client.SendAsync(JsonConvert.SerializeObject(new TcpMessage
            {
                Type = TcpMessageType.PlayerNumberRequest
            }));

            PlayerNumberBindable.ValueChanged += val => playerNumberText.Text = $"You are player: {val.NewValue}";
        }

        public void GoToPlayScreen()
        {
            Schedule(() => screenStack.Push(new PlayScreen(client, PlayerNumberBindable.Value)));
        }

        private void requestStartGame()
        {
            client.SendAsync(JsonConvert.SerializeObject(new TcpMessage
            {
                Type = TcpMessageType.StartGameRequest
            }));
        }
    }
}
