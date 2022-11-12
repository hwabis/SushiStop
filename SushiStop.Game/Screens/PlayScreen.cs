using System.Collections.Generic;
using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osuTK;
using osuTK.Graphics;
using SushiStop.Game.Cards;
using SushiStop.Game.Cards.Drawables;
using SushiStop.Game.Networking;

namespace SushiStop.Game.Screens
{
    // TODO: client should call ResetForNewTurn() when NextTurn is received
    public class PlayScreen : Screen
    {
        public Player Player { get; set; } = new Player();

        private FillFlowContainer<DrawableCard> drawableHand;
        private List<DrawableCard> selectedCards = new List<DrawableCard>();
        // Limit can be immediately raised to 2 (client-side) by using Chopsticks
        private int selectedCardsLimit = 1;

        // Replace with a Button that can be Enabled or Disabled in the class itself?
        private BasicButton chopsticksButton;
        private BasicButton sendButton;

        // Can only use one chopsticks per round
        private bool canUseChopsticks = true;
        // Is disabled after sending cards to the server to prevent spam clicking
        private bool canSendCards = true;

        private SushiStopClient client;

        public PlayScreen(SushiStopClient client, int playerNumber)
        {
            this.client = client;
            Player.Number = playerNumber;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                new Container
                {
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Child = drawableHand = new FillFlowContainer<DrawableCard>
                    {
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Horizontal
                    }
                },
                new FillFlowContainer
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Spacing = new Vector2(0, 10),
                    Y = 70,
                    Children = new Drawable[]
                    {
                        chopsticksButton = new BasicButton
                        {
                            Text = "Use chopsticks!",
                            Width = 160,
                            Height = 40,
                            BackgroundColour = Color4.MediumBlue,
                            Action = useChopsticks
                        },
                        sendButton = new BasicButton
                        {
                            Text = "Send!",
                            Width = 80,
                            Height = 40,
                            BackgroundColour = Color4.MediumBlue,
                            Action = confirmCardSelection
                        }
                    }
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            // TODO: Highlight our player's row

            // Client may be null when we passed it as null in the constructor (FOR TEST SCENE PURPOSES ONLY)
            client?.SendAsync(JsonConvert.SerializeObject(new TcpMessage
            {
                Type = TcpMessageType.StartRoundRequest
            }));
        }

        public void CreateDrawableHand()
        {
            foreach (Card card in Player.Hand)
            {
                DrawableCard drawableCard = card.CreateDrawableCard();
                drawableCard.OnClickForHand = () =>
                {
                    if (selectedCards.Contains(drawableCard))
                    {
                        selectedCards.Remove(drawableCard);
                        drawableCard.Highlight(false);
                    }
                    else if (selectedCards.Count < selectedCardsLimit)
                    {
                        selectedCards.Add(drawableCard);
                        drawableCard.Highlight(true);
                    }
                };
                Schedule(() => drawableHand.Add(drawableCard));
            }
        }

        public void ResetForNewTurn()
        {
            selectedCards.Clear();
            selectedCardsLimit = 1;
            enableChopsticksButton(true);
            enableSendButton(true);
        }

        private void useChopsticks()
        {
            if (canUseChopsticks) // TODO: also check that the player also has chopsticks,
                                  // then remove it from Player.PlayedCards
            {
                enableChopsticksButton(false);
                selectedCardsLimit = 2;
            }
        }

        private void confirmCardSelection()
        {
            if (!canSendCards)
                return;

            // This enforces that you send at one card, or if you used chopsticks, you must send two
            if (selectedCards.Count < 1 || (selectedCards.Count < 2 && !canUseChopsticks))
                return;

            enableChopsticksButton(false);
            enableSendButton(false);

            foreach (DrawableCard selectedCard in selectedCards)
            {
                Player.Hand.Remove(selectedCard.Card);
                Player.PlayedCards.Add(selectedCard.Card);
            }

            // null check again so the test scene works
            client?.SendAsync(JsonConvert.SerializeObject(new TcpMessage
            {
                Type = TcpMessageType.PlayedCard,
                Player = Player
            }, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));

            // TODO: "waiting for other players" or something
        }

        private void enableChopsticksButton(bool enable)
        {
            if (enable)
            {
                canUseChopsticks = true;
                Schedule(() => chopsticksButton.BackgroundColour = Color4.MediumBlue);
            }
            else
            {
                canUseChopsticks = false;
                Schedule(() => chopsticksButton.BackgroundColour = Color4.Red);
            }
        }

        private void enableSendButton(bool enable)
        {
            if (enable)
            {
                canSendCards = true;
                Schedule(() => sendButton.BackgroundColour = Color4.MediumBlue);
            }
            else
            {
                canSendCards = false;
                Schedule(() => sendButton.BackgroundColour = Color4.Red);
            }
        }
    }
}
