using System.Collections.Generic;
using System.Linq;
using System.Runtime.ExceptionServices;
using Newtonsoft.Json;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
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
        public List<Player> Players { get; set; }
        private int playerNumber;

        public Player Player => Players.Find(player => player.Number == playerNumber);

        // Each member at index i is a column representing the PlayedCards of Player with Number i + 1
        private FillFlowContainer<FillFlowContainer> playedCards;

        private FillFlowContainer<DrawableCard> drawableHand;
        private List<DrawableCard> selectedCards = new List<DrawableCard>();
        // Limit can be immediately raised to 2 (client-side) by using Chopsticks
        private int selectedCardsLimit = 1;

        // Replace with a Button that can be Enabled or Disabled in the class itself?
        private BasicButton chopsticksButton;
        private BasicButton endTurnButton;

        // Can only use one chopsticks per round
        private bool canUseChopsticks = true;
        // Is disabled after sending cards to the server to prevent spam clicking
        private bool canEndTurn = true;

        private SpriteText scoresText;
        private BasicButton startNewRoundButton;

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
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Beige
                },
                playedCards = new FillFlowContainer<FillFlowContainer>
                {
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Direction = FillDirection.Horizontal,
                    // Why do I need these...
                    Spacing = new Vector2(54, 0),
                    Y = 24
                },
                drawableHand = new FillFlowContainer<DrawableCard>
                {
                    AutoSizeAxes = Axes.Both,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Direction = FillDirection.Horizontal
                },
                new FillFlowContainer
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Spacing = new Vector2(0, 5),
                    Y = 120,
                    Children = new Drawable[]
                    {
                        chopsticksButton = new BasicButton
                        {
                            Text = "Use gem!",
                            Width = 120,
                            Height = 40,
                            BackgroundColour = Color4.MediumBlue,
                            Action = useChopsticks
                        },
                        endTurnButton = new BasicButton
                        {
                            Text = "End turn!",
                            Width = 120,
                            Height = 40,
                            BackgroundColour = Color4.MediumBlue,
                            Action = endTurn
                        }
                    }
                },
                scoresText = new SpriteText
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Font = FontUsage.Default.With(size: 40),
                    Colour = Color4.Brown
                },
                startNewRoundButton = new BasicButton
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Text = "Ready for next round!",
                    Width = 180,
                    Height = 40,
                    Y = 160,
                    BackgroundColour = Color4.MediumBlue,
                    Action = (() =>
                    {
                        client.SendAsync(JsonConvert.SerializeObject(new TcpMessage
                        {
                            Type = TcpMessageType.StartRoundRequest
                        }));
                    })
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            scoresText.Hide();
            startNewRoundButton.Hide();

            // Client may be null when we passed it as null in the constructor (FOR TEST SCENE PURPOSES ONLY)
            // Only have player 1 send the StartRoundRequest
            if (playerNumber == 1)
            {
                client?.SendAsync(JsonConvert.SerializeObject(new TcpMessage
                {
                    Type = TcpMessageType.StartRoundRequest
                }));
            }
        }

        public void CreateDrawablePlayedCards()
        {
            Schedule(() =>
            {
                // Null check in case this isn't loaded yet? Which means it was empty anyway?
                playedCards?.Clear();
                foreach (Player player in Players)
                {
                    FillFlowContainer playedCardsRow;
                    playedCards.Add(playedCardsRow = new FillFlowContainer
                    {
                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,
                        AutoSizeAxes = Axes.Both,
                        Direction = FillDirection.Vertical,
                        Spacing = new Vector2(0, -104)
                    });

                    SpriteText pText;
                    playedCardsRow.Add(pText = new SpriteText
                    {
                        Text = $"P{player.Number}: ",
                        Origin = Anchor.BottomCentre,
                        Font = FontUsage.Default.With(size: 20),
                        Colour = Color4.Brown
                    });
                    if (player.Number == playerNumber)
                        pText.Text = $"YOU ARE P{player.Number}: ";

                    foreach (Card card in player.PlayedCards)
                    {
                        DrawableCard drawableCard = card.CreateDrawableCard();
                        drawableCard.Origin = Anchor.TopCentre;
                        playedCardsRow.Add(drawableCard);
                    }
                }
            });
        }

        public void CreateDrawableHand()
        {
            Schedule(() =>
            {
                drawableHand?.Clear();
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
                    drawableHand.Add(drawableCard);
                }
            });
        }

        public void ResetForNewTurn()
        {
            selectedCards.Clear();
            selectedCardsLimit = 1;
            enableChopsticksButton(true);
            enableEndTurnButton(true);
        }

        public void ShowScoresAndNewRoundButton(bool show)
        {
            Schedule(() =>
            {
                if (!show)
                {
                    scoresText.Hide();
                    startNewRoundButton.Hide();
                    return;
                }

                scoresText.Show();
                // TODO: don't show new round button if 3 rounds have passed
                startNewRoundButton.Show();

                // TODO: set final round if it's final round
                int[] scores = calculateScores(Players, false);
                scoresText.Text = $"Scores: ";
                for (int i = 0; i < scores.Length; i++)
                    scoresText.Text += $"P{i + 1} - {scores[i]}, ";
            });
        }

        private void useChopsticks()
        {
            Card chopsticksCard = Player.PlayedCards.FirstOrDefault(card => card is ChopsticksCard);
            if (canUseChopsticks && chopsticksCard != null && Player.Hand.Count > 1)
            {
                Player.PlayedCards.Remove(chopsticksCard);
                Player.Hand.Add(chopsticksCard);

                CreateDrawablePlayedCards();
                // Don't do CreateDrawableHand(), because we don't want the player to re-play it
                enableChopsticksButton(false);
                selectedCardsLimit = 2;
            }
        }

        private void endTurn()
        {
            if (!canEndTurn)
                return;

            // This enforces that you send at one card, or if you used chopsticks, you must send two
            if (selectedCards.Count < 1 || (selectedCards.Count < 2 && !canUseChopsticks))
                return;

            enableChopsticksButton(false);
            enableEndTurnButton(false);

            foreach (DrawableCard selectedCard in selectedCards)
            {
                Player.Hand.Remove(selectedCard.Card);
                Player.PlayedCards.Add(selectedCard.Card);
            }

            // null check again so the test scene works
            client?.SendAsync(JsonConvert.SerializeObject(new TcpMessage
            {
                Type = TcpMessageType.EndTurn,
                Player = Player
            }, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All }));
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

        private void enableEndTurnButton(bool enable)
        {
            if (enable)
            {
                canEndTurn = true;
                Schedule(() => endTurnButton.BackgroundColour = Color4.MediumBlue);
            }
            else
            {
                canEndTurn = false;
                Schedule(() => endTurnButton.BackgroundColour = Color4.Red);
            }
        }

        // index 0 of the return is score for player 1, ...
        private int[] calculateScores(List<Player> players, bool finalRound)
        {
            int[] scores = new int[players.Count];
            // First, let's calculate the independent scores, and get the count of Maki and Pudding along the way
            int[] makiRollCount = new int[players.Count];
            int[] puddingCount = new int[players.Count];
            for (int i = 0; i < players.Count; i++)
            {
                int tempuraCount = 0;
                int sashimiCount = 0;
                int dumplingCount = 0;
                bool wasabiActivated = false;
                int nigiriScore = 0;
                foreach (Card card in players[i].PlayedCards)
                {
                    switch (card)
                    {
                        case TempuraCard:
                            tempuraCount++;
                            break;
                        case SashimiCard:
                            sashimiCount++;
                            break;
                        case DumplingCard:
                            dumplingCount++;
                            break;
                        case MakiRollCard:
                            makiRollCount[i]++;
                            break;
                        case WasabiCard:
                            wasabiActivated = true;
                            break;
                        case NigiriCard nigiriCard:
                            nigiriScore += wasabiActivated ? nigiriCard.Value * 3 : nigiriCard.Value;
                            wasabiActivated = false;
                            break;
                        case PuddingCard:
                            puddingCount[i]++;
                            break;
                        default:
                            break;
                    }
                }
                scores[i] += (tempuraCount / 2) * 5 + (sashimiCount / 3) * 10 + nigiriScore;
                switch (dumplingCount)
                {
                    case int count when count >= 5:
                        scores[i] += 15;
                        break;
                    case 4:
                        scores[i] += 10;
                        break;
                    case 3:
                        scores[i] += 6;
                        break;
                    case 2:
                        scores[i] += 3;
                        break;
                    case 1:
                        scores[i] += 1;
                        break;
                    default:
                        break;
                }
            }
            // TODO: then we calculate the maki scores

            // TODO: then the pudding scores, if finalRound

            return scores;
        }
    }
}
