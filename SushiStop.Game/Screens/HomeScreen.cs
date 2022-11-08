using System.Net;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK.Graphics;
using SushiStop.Game.Networking;

namespace SushiStop.Game.Screens
{
    public class HomeScreen : Screen
    {
        private ScreenStack screenStack;

        private TextBox ipTextBox;
        private TextBox portTextBox;

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
                        new SpriteText
                        {
                            Text = "Sushi Stop :)",
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Font = FontUsage.Default.With(size: 40)
                        },
                        ipTextBox = new BasicTextBox
                        {
                            PlaceholderText = "Server IP",
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Width = 300,
                            Height = 50,
                            Margin = new MarginPadding(5)
                        },
                        portTextBox = new BasicTextBox
                        {
                            PlaceholderText = "Port",
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Width = 100,
                            Height = 50,
                            Margin = new MarginPadding(5)
                        },
                        new BasicButton
                        {
                            Text = "Connect!",
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Width = 100,
                            Height = 50,
                            BackgroundColour = Color4.DarkBlue,
                            Action = connectToServer,
                            Margin = new MarginPadding(5)
                        }
                    }
                }
            };

            this.screenStack = screenStack;
        }

        private void connectToServer()
        {
            // Use 127.0.0.1 for testing
            // TODO: check if you're connecting to an address/port that isn't being listened...?
            IPAddress address;
            int port;
            try
            {
                address = IPAddress.Parse(ipTextBox.Text);
                port = int.Parse(portTextBox.Text);
            }
            catch
            {
                Logger.Log("Invalid IP and/or port!");
                return;
            }
            SushiStopClient client = new SushiStopClient(address, port, screenStack);

            // No idea if this bool is ever going to be false
            bool connectionSucceeded = client.ConnectAsync();
            if (connectionSucceeded)
                screenStack.Push(new LobbyScreen(client));
            else
                Logger.Log("Failed to connect!");
        }
    }
}
