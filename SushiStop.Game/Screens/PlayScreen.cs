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
    public class PlayScreen : Screen
    {
        // Player-related info
        public List<Card> Hand { get; set; }
        private int playerNumber;

        private FillFlowContainer<DrawableCard> drawableHand;
        private List<DrawableCard> selectedCards = new List<DrawableCard>();
        // Limit can be immediately raised to 2 (client-side) by using Chopsticks
        private int selectedCardsLimit = 1;

        // Is disabled after sending cards to the server to prevent spam clicking
        public bool CanSendCards = true;

        private SushiStopClient client;

        public PlayScreen(SushiStopClient client, int playerNumber)
        {
            this.client = client;
            this.playerNumber = playerNumber;
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            InternalChildren = new Drawable[]
            {
                drawableHand = new FillFlowContainer<DrawableCard>
                {
                    // I'm doing something wrong here... Y = -70 shouldn't be needed
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    AutoSizeAxes = Axes.Both,
                    Spacing = new Vector2(94, 0),
                    Y = -70
                },
                new BasicButton
                {
                    Text = "Send!",
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Width = 80,
                    Height = 80,
                    X = -10,
                    Y = 150,
                    BackgroundColour = Color4.DarkBlue,
                    Action = confirmCardSelection
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
            foreach (Card card in Hand)
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

        private void confirmCardSelection()
        {
            if (selectedCards.Count > 0)
            {
                CanSendCards = false;
                List<Card> cards = new List<Card>();
                foreach (DrawableCard drawableCard in selectedCards)
                    cards.Add(drawableCard.Card);

                // null check again so the test scene works
                client?.SendAsync(JsonConvert.SerializeObject(new TcpMessage
                {
                    Type = TcpMessageType.PlayedCard,
                    PlayedCards = cards
                }, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));
            }
        }
    }
}
