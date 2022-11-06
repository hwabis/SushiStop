using System.Net;
using NetCoreServer;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK.Graphics;

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
            TcpClient client = new TcpClient(address, port);

            // Using Connect() freezes the whole game up, but I can't figure out
            // how to work with ConnectAsync()... :(
            bool connectionSucceeded = client.Connect();
            if (connectionSucceeded)
                screenStack.Push(new LobbyScreen());
            else
                Logger.Log("Failed to connect!");
        }
    }
}
