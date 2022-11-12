using System.Net;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using SushiStop.Game.Networking;

namespace SushiStop.Game.Screens
{
    public class HomeScreen : Screen
    {
        private ScreenStack screenStack;

        private TextBox ipTextBox;
        private TextBox portTextBox;
        private BasicButton connectButton;

        private const int connectButtonWidth = 100;
        private const int connectButtonHeight = 50;
        private const string connectButtonText = "Connect!";

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
                        connectButton = new BasicButton
                        {
                            Text = connectButtonText,
                            Anchor = Anchor.TopCentre,
                            Origin = Anchor.TopCentre,
                            Width = connectButtonWidth,
                            Height = connectButtonHeight,
                            BackgroundColour = Color4.DarkBlue,
                            Action = connectToServer,
                            Margin = new MarginPadding(5)
                        }
                    }
                }
            };

            this.screenStack = screenStack;
        }

        public override void OnResuming(ScreenTransitionEvent e)
        {
            base.OnResuming(e);

            enableConnectButton(true);
        }

        private void connectToServer()
        {
            // Use 127.0.0.1 for testing
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

            // Not sure why this isn't awaitable.
            // I need to await this so that we don't accidentally connect after we've already
            // send a PlayerNumberRequest, but I can't. So, this is a (terrible) solution for now.

            enableConnectButton(false);
            client.ConnectAsync();
            Scheduler.AddDelayed(() =>
            {
                screenStack.Push(new LobbyScreen(client));
            }, 1000);
        }

        private void enableConnectButton(bool enable)
        {
            if (enable)
            {
                connectButton.Width = connectButtonWidth;
                connectButton.Height = connectButtonHeight;
                connectButton.Text = connectButtonText;
            }
            else
            {
                connectButton.Width = 0;
                connectButton.Height = 0;
                connectButton.Text = "";
            }
        }
    }
}
